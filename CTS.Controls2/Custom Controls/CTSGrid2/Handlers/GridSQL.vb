Imports System.Text.RegularExpressions

Public Class GridSQL

    Property Original As String
    Property Statement As String
    Property Distinct As Boolean

    Property TableList As New List(Of SqlTable)
    Property ColumnList As New List(Of SqlColumn)
    Property ClauseList As New List(Of Clause)

    Property ErrorLog As New List(Of String)
    Property Errors As Boolean = False
    Property Loaded As Boolean = False

    Private SchemasList As DataTable
    Private SchemaData As DataTable


#Region "Supporting Classes"
    Public Class Clause
        Property Position As Short
        Property DataPosition As Short
        Property Name As String
        Property Value As String

    End Class

    Public Class SqlTable
        Property Name As String
        Property AliasName As String
        Property SchemaName As String
        Property SchemaExplicit As String
        Property RawData As String
        Property JoinType As String
        Property Criteria As String
        Property TableText As String
    End Class

    Public Class SqlColumn
        Property Name As String
        Property TableName As String
        Property ColFunction As String
        Property DataType As DataType
        Property ProviderType As String
        Property Size As Short
        Property Scale As Short
        Property Text As String
        Property HeaderText As String
        Property TableAlias As String
        Property ColumnAlias As String
    End Class

    Private StringSub As New List(Of KeyValuePair(Of Short, String))

    Private ParenthesisSub As New List(Of KeyValuePair(Of Short, String))

#End Region


#Region "Constructors"

    Sub New(SQLStatement As String)
        Initiate("GP", SQLStatement)
    End Sub

    Sub New(LibraryList As String, SQLStatement As String)
        If String.IsNullOrEmpty(LibraryList) Then
            LibraryList = "GP"
        End If
        Initiate(LibraryList, SQLStatement)
    End Sub

    Sub Initiate(LibraryList As String, SQLStatement As String)
        SchemasList = DNRG0011_Table(LibraryList, DataEnvironments.Production)
        LoadStatement(SQLStatement)
    End Sub

#End Region


    Public Sub LoadStatement(ByVal SQStatement As String)
        Original = SQStatement
        Statement = SQStatement

        ExtractStrings()
        ExtractAllParenthesis()

        'select distinct??

        FindClauses("Select")
        FindClauses("From")
        FindClauses("Left Join")
        FindClauses("Inner Join")
        FindClauses("Outer Join")
        FindClauses("Exception Join")
        FindClauses("Join")
        FindClauses("Where")
        FindClauses("Group By")
        FindClauses("Order By")
        ExtractClauses()

        Try
            LoadTables()
            SchemaData = C2App.Data.GetThisSchema($"select * {FromString()} ")
        Catch ex As Exception
            SchemaData = Nothing
            ErrorLog.Add($"Error in From Clause. {ex.Message}")
        End Try

        If SchemaData IsNot Nothing Then
            LoadColumns()
        End If

        If Errors Then
            Loaded = False
        Else
            Loaded = True
        End If

    End Sub

    Public Function FromString() As String
        Dim result As String = String.Empty

        Dim FromTable = TableList.Find(Function(x) x.JoinType.ToLower = "from")
        If FromTable IsNot Nothing Then
            'If String.IsNullOrWhiteSpace(FromTable.SchemaExplicit) Then
            '    result = $"From {FromTable.SchemaName}.{FromTable.Name}"
            'Else
            '    result = $"From {FromTable.SchemaExplicit}.{FromTable.Name}"
            'End If
            result = $"From {FromTable.SchemaName}.{FromTable.Name}"
            result = $"From {FromTable.RawData}"

            For Each sqltable In TableList
                If sqltable.JoinType.ToLower.Contains("join") Then
                    result = $"{result.Trim} {sqltable.JoinType} {sqltable.RawData}"
                End If
            Next

            result = EmbedParenthesis(result)
            result = EmbedStrings(result)
        End If

        Return result
    End Function

    Public Function GroupByString() As String
        Dim result As String = String.Empty

        Dim GBClause = ClauseList.Find(Function(x) x.Name.ToLower = "group by")
        If GBClause IsNot Nothing Then
            result = GBClause.Value
        End If

        result = EmbedStrings(result)
        result = EmbedParenthesis(result)

        Return result
    End Function

    Public Function GetClause(ByVal Name As String) As String
        Dim result As String = String.Empty

        Dim Clause = ClauseList.Find(Function(x) x.Name.ToLower = Name)
        If Clause IsNot Nothing Then
            result = Clause.Value
        End If

        result = EmbedStrings(result)
        result = EmbedParenthesis(result)

        Return result
    End Function



#Region "SHARE TEST"

    Public Function GetAllColumns() As List(Of SqlColumn)
        Dim sqlcl As New List(Of SqlColumn)
        Return sqlcl
    End Function

    Public Function GetAllColumnsDB2() As List(Of SqlColumn)

        Dim sqlcl As New List(Of SqlColumn)
        Try
            Dim sql As String
            sql = "Select Column_Name, Data_Type, Length, Numeric_Scale, Column_Text, Column_Heading " &
                          "From qsys2.syscolumns " &
                          "Where table_Schema = '{0}' and table_name = '{1}' "

            For Each sqlt In TableList
                Dim dtCols = C2App.Data.GetTable(sql, sqlt.SchemaName.ToUpper, sqlt.Name.ToUpper)
                If dtCols IsNot Nothing Then
                    For Each drCol In dtCols.Rows

                        Dim data_type As DataType = [Enum].Parse(GetType(DataType), drCol("Data_Type"))
                        Dim P As ProviderType.DB2ProviderDataTypes = ProviderType.GetProvider(data_type)
                        If P <> ProviderType.DB2ProviderDataTypes.NotSupported Then
                            Dim sqlc As New SqlColumn
                            sqlc.Name = drCol("Column_Name")
                            sqlc.TableName = sqlt.Name.ToUpper
                            sqlc.ColFunction = String.Empty
                            sqlc.DataType = data_type
                            sqlc.ProviderType = P
                            sqlc.Size = IIf(IsDBNull(drCol("Length")), 0, drCol("Length"))
                            sqlc.Scale = IIf(IsDBNull(drCol("Numeric_Scale")), 0, drCol("Numeric_Scale"))
                            sqlc.Text = IIf(IsDBNull(drCol("Column_Text")), String.Empty, drCol("Column_Text").ToString.Trim)
                            sqlc.HeaderText = IIf(IsDBNull(drCol("Column_Heading")), String.Empty, drCol("Column_Heading").ToString.Trim)
                            sqlcl.Add(sqlc)
                        End If
                    Next
                End If
            Next

        Catch ex As Exception
        End Try

        Return sqlcl
    End Function



#End Region





#Region "Table work"

    Private Sub LoadTables()

        'find main from clause...
        Dim FromClause = ClauseList.Find(Function(x) x.Name.ToLower = "from")
        If FromClause IsNot Nothing Then
            Dim sqlFrom As New SqlTable
            sqlFrom.JoinType = FromClause.Name
            sqlFrom.RawData = FromClause.Value
            sqlFrom = BreakdownTableReference(sqlFrom)
            TableList.Add(sqlFrom)
        End If

        'Find all join clauses in arrival sequence
        Dim SortedList = ClauseList.OrderBy(Function(x) x.Position)
        For Each clause In SortedList
            If clause.Name.ToLower.Contains("join") Then
                Dim sqlt As New SqlTable
                sqlt.JoinType = clause.Name
                sqlt.RawData = clause.Value
                sqlt = BreakdownTableReference(sqlt)
                TableList.Add(sqlt)
            End If
        Next

    End Sub

    Private Function BreakdownTableReference(sqlt As SqlTable) As SqlTable
        Dim name As String = String.Empty

        If sqlt.RawData IsNot Nothing Then
            sqlt.RawData = sqlt.RawData.Trim.Replace("/", ".")

            Dim wordbreak = sqlt.RawData.IndexOf(" ")
            If wordbreak = -1 Then
                name = sqlt.RawData
            Else
                name = sqlt.RawData.Substring(0, wordbreak)
            End If
        End If

        If name.Contains(".") Then
            sqlt.SchemaExplicit = name.Substring(0, name.IndexOf("."))
            sqlt.Name = name.Substring(name.IndexOf(".") + 1)
        Else
            sqlt.SchemaExplicit = String.Empty
            sqlt.Name = name
        End If

        If sqlt.RawData IsNot Nothing Then
            If sqlt.RawData.ToLower.Contains(" on ") Then
                Dim idx = sqlt.RawData.ToLower.IndexOf(" on ")
                sqlt.Criteria = sqlt.RawData.Substring(idx + 1)
            End If
        End If

        If String.IsNullOrWhiteSpace(sqlt.SchemaExplicit) Then
            FindTableSchema(sqlt, SchemasList)
        Else
            LookupTableSchema(sqlt)
        End If



        Return sqlt
    End Function

    Private Sub LookupTableSchema(sqlt As SqlTable)
        Dim savedrow As DataRow = Nothing
        Dim seq As Short = Short.MaxValue

        Dim row = C2App.Data.GetRow("Select Table_Text from qsys2.systables where Table_Schema = '{0}' and Table_Name = '{1}'",
                                   sqlt.SchemaExplicit.ToUpper, sqlt.Name.ToUpper)
        If row IsNot Nothing Then
            sqlt.SchemaName = sqlt.SchemaExplicit.ToUpper
            sqlt.TableText = row("Table_Text").ToString.Trim
        End If

    End Sub

    Private Sub FindTableSchema(sqlt As SqlTable, SchemaTable As DataTable)
        Dim savedrow As DataRow = Nothing
        Dim seq As Short = Short.MaxValue

        Dim dt = C2App.Data.GetTable("Select Table_Schema, Table_Text from qsys2.systables where table_name = '{0}'", sqlt.Name.ToUpper)
        If dt IsNot Nothing Then

            For Each row In dt.Rows
                Dim foundrow = SchemaTable.Select($"name='{row("Table_Schema").ToString}'")
                If foundrow.Length > 0 AndAlso foundrow(0)("seq") < seq Then
                    seq = foundrow(0)("seq")
                    savedrow = row
                End If
            Next

            If savedrow IsNot Nothing Then
                sqlt.SchemaName = savedrow("Table_Schema")
                sqlt.TableText = savedrow("Table_Text")




            End If

        End If





    End Sub

#End Region


#Region "Column work"
    Private Function LoadColumns() As String
        Dim result As String = String.Empty

        'todo breakdown select columns; need to separate table alias ??
        Dim SelectClause = ClauseList.Find(Function(x) x.Name.ToLower = "select")
        If SelectClause Is Nothing Then
            'error
        Else

            If SelectClause.Value.ToLower.StartsWith("distinct ") Then
                SelectClause.Value = SelectClause.Value.Remove(0, 9)
                Distinct = True
            End If

            If SelectClause.Value.Trim = "*" Then
                AddAllColumns()
            Else
                Dim cols As String() = SelectClause.Value.Split(",")
                For Each col In cols

                    Dim sqlc As SqlColumn = BreakSelectColumn(col.Trim)

                    Dim rows As DataRow() = SchemaData.Select($"ColumnName = '{sqlc.Name.Trim.ToUpper}'")
                    If rows IsNot Nothing AndAlso rows.Count > 0 Then
                        'sqlc.Name = rows(0)("ColumnName")
                        sqlc.ProviderType = rows(0)("ProviderType")
                        sqlc.Size = rows(0)("ColumnSize")
                        sqlc.ColFunction = EmbedStrings(sqlc.ColFunction)
                        sqlc.ColFunction = EmbedParenthesis(sqlc.ColFunction)
                        AddToColumnList(sqlc)
                    Else
                        'will not find any field embedded in a function
                        'like trim(field) or concat(fielda,fieldb)

                        sqlc.ColFunction = EmbedStrings(sqlc.ColFunction)
                        sqlc.ColFunction = EmbedParenthesis(sqlc.ColFunction)
                        AddToColumnList(sqlc)

                        'todo this is a warning NOT an error! 
                        'ErrorLog.Add($"'{col.Trim.ToUpper}' not found in current schema")
                    End If


                Next
            End If

            result = SelectClause.Value
        End If

        result = EmbedStrings(result)
        result = EmbedParenthesis(result)

        Return result
    End Function

    Private Sub AddAllColumns()

        Try
            If SchemaData IsNot Nothing Then
                For Each dr In SchemaData.Rows
                    Dim sqlc As New SqlColumn
                    sqlc.Name = dr("ColumnName")
                    sqlc.ColFunction = String.Empty
                    sqlc.ProviderType = dr("ProviderType")
                    sqlc.Size = dr("ColumnSize")
                    AddToColumnList(sqlc)
                Next
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub


    Public Sub AddToColumnList(ByVal sqlc As SqlColumn)

        Dim sql As String
        sql = "Select Data_Type, Length, Numeric_Scale, Column_Heading, Column_Text " &
                          "From qsys2.syscolumns " &
                          " where table_Schema = '{0}' " &
                          " And table_name = '{1}' " &
                          " And Column_Name = '{2}' "
        Dim Added As Boolean

        For Each sqlt In TableList
            If sqlt.SchemaName IsNot Nothing Then
                Dim drow = C2App.Data.GetRow(sql, sqlt.SchemaName.ToUpper, sqlt.Name.ToUpper, sqlc.Name.ToUpper)
                If drow IsNot Nothing Then
                    sqlc.TableName = sqlt.Name.ToUpper
                    sqlc.Text = IIf(IsDBNull(drow("Column_Text")), String.Empty, drow("Column_Text").ToString.Trim)
                    sqlc.HeaderText = IIf(IsDBNull(drow("Column_Heading")), String.Empty, drow("Column_Heading").ToString.Trim)
                    ColumnList.Add(sqlc)
                    Added = True
                    Exit For
                End If
            End If
        Next

        If Not Added Then
            'is valid function or result field?
            ColumnList.Add(sqlc)
        End If

    End Sub

    Private Function BreakSelectColumn(ByVal ColString As String) As SqlColumn
        Dim result As New SqlColumn

        Dim words As String() = ColString.Split(" ")
        If words.Count = 1 Then
            result = PullColumnTable(words(0), result)

        ElseIf words.Count = 2 Then
            result.ColFunction = words(0)
            result = PullColumnTable(words(0), result)
            result.ColumnAlias = words(1)
            result.Name = words(1)

        ElseIf words.Count = 3 And words(1).ToLower.Trim = "as" Then
            result.ColFunction = words(0)
            result = PullColumnTable(words(0), result)
            result.ColumnAlias = words(2)
            result.Name = words(2)
        End If

        Return result
    End Function

    Private Function PullColumnTable(ByVal ColumnName As String, sqlc As SqlColumn) As SqlColumn
        Dim words As String() = ColumnName.Split(".")
        If words.Count = 1 Then
            sqlc.Name = words(0)
        ElseIf words.Count = 2 Then
            sqlc.TableAlias = words(0)
            sqlc.Name = words(1)
        End If

        Return sqlc
    End Function


#End Region


#Region "Helper functions"

    Private Sub ExtractStrings()
        Dim Done As Boolean = False
        Dim startindex As Short = 0
        Dim endindex As Short = 0
        Dim nextstartindex As Short = 0

        Do Until Done
            startindex = Statement.IndexOf("'", nextstartindex)
            endindex = Statement.IndexOf("'", startindex + 1)

            If startindex < 0 Then
                Done = True
            Else
                startindex += 1
                endindex -= 1

                If startindex < endindex Then
                    Dim extracted = Statement.Substring(startindex, (endindex - startindex) + 1)
                    StringSub.Add(New KeyValuePair(Of Short, String)(startindex, extracted))

                    Dim sb As New Text.StringBuilder(Statement)
                    sb.Remove(startindex, (endindex - startindex) + 1)
                    sb.Insert(startindex, $"{{{startindex}}}")
                    Statement = sb.ToString

                    nextstartindex = startindex + startindex.ToString.Length + 3
                Else
                    Done = True
                End If
            End If
        Loop

    End Sub

    Private Sub ExtractAllParenthesis()
        Dim si As Short = ExtractParenthesis()
        Do While si > 0
            si = ExtractParenthesis(si + 1)
        Loop
    End Sub

    'todo extract ALL paren not just the first 
    Private Function ExtractParenthesis() As Short
        Return ExtractParenthesis(Statement.IndexOf("("))
    End Function


    Private Function ExtractParenthesis(nextstartindex As Short) As Short
        Dim done As Boolean
        Dim depth As Short
        Dim nextindex As Short
        Dim endindex As Short

        If nextstartindex > 0 Then
            Dim startindex = Statement.IndexOf("(", nextstartindex)
            Dim lastopenindex = startindex
            Dim lastcloseindex = startindex
            nextstartindex = startindex

            If startindex > -1 Then
                Do Until done
                    nextindex = Statement.IndexOf("(", lastopenindex + 1)
                    endindex = Statement.IndexOf(")", lastcloseindex + 1)

                    If nextindex < 0 Or (endindex < nextindex) Then
                        If depth = 0 Then
                            done = True
                        Else
                            depth -= 1
                            lastcloseindex = endindex
                        End If
                    Else
                        depth += 1
                        lastopenindex = nextindex
                    End If
                Loop

                startindex += 1
                endindex -= 1

                If startindex < endindex Then
                    Dim extracted = Statement.Substring(startindex, (endindex - startindex) + 1)
                    ParenthesisSub.Add(New KeyValuePair(Of Short, String)(startindex, extracted))

                    Dim sb As New Text.StringBuilder(Statement)
                    sb.Remove(startindex, (endindex - startindex) + 1)
                    sb.Insert(startindex, $"{{{startindex}}}")
                    Statement = sb.ToString

                    nextstartindex = startindex + startindex.ToString.Length + 3
                End If
            End If
        End If

        Return nextstartindex
    End Function

    Private Sub FindClauses(ByVal Name As String)
        Dim Done As Boolean
        Dim startindex As Short
        Dim clausename As String = Name.Trim.ToLower & " "

        Do Until Done
            startindex = Statement.ToLower.IndexOf(clausename, startindex)
            If startindex = -1 Then
                Done = True
            Else
                Dim c As New Clause
                c.Name = Name.Trim
                c.Position = startindex
                c.DataPosition = startindex + Len(clausename)
                ClauseList.Add(c)
                startindex += 1
            End If
        Loop

    End Sub

    Private Sub ExtractClauses()
        Dim curposition As Short = 0
        Dim lastposition As Short = 0
        Dim lastdataposition As Short = 0

        'find duplicate entries and tag
        For Each obj In ClauseList.OrderBy(Function(x) x.DataPosition).ToList
            If lastdataposition = obj.DataPosition And lastdataposition > 0 Then
                obj.Position = -1
            End If
            lastdataposition = obj.DataPosition
        Next

        'remove tagged entries
        For i = ClauseList.Count - 1 To 0 Step -1
            If ClauseList(i).Position = -1 Then
                ClauseList.RemoveAt(i)
            End If
        Next

        Dim SortedList = ClauseList.OrderByDescending(Function(x) x.Position).ToList
        For Each obj In SortedList

            If lastposition = 0 Then
                obj.Value = Statement.Substring(obj.DataPosition)
            Else
                If lastposition - obj.DataPosition > 0 Then
                    obj.Value = Statement.Substring(obj.DataPosition, lastposition - obj.DataPosition)
                End If
            End If

            lastposition = obj.Position
        Next
    End Sub

    Private Function EmbedStrings(ByVal Text As String) As String

        If Not String.IsNullOrEmpty(Text) Then
            For Each match As Match In Regex.Matches(Text, "{\d+}", RegexOptions.IgnoreCase)
                Dim sshort = match.Value.Substring(1, match.Value.Length - 2)
                Dim embed As String = StringSub.Find(Function(x) x.Key = CShort(sshort)).Value
                If embed IsNot Nothing Then
                    Text = Text.Replace(match.Value, embed)
                End If
            Next
        End If

        Return Text
    End Function

    Private Function EmbedParenthesis(ByVal Text As String) As String

        If Not String.IsNullOrEmpty(Text) Then
            For Each match As Match In Regex.Matches(Text, "{\d+}", RegexOptions.IgnoreCase)
                Dim sshort = match.Value.Substring(1, match.Value.Length - 2)
                Dim embed As String = ParenthesisSub.Find(Function(x) x.Key = CShort(sshort)).Value
                If embed IsNot Nothing Then
                    Text = Text.Replace(match.Value, embed)
                End If
            Next
        End If

        Return Text
    End Function

    Friend Function DNRG0011_Table(AppCode As String, Env As DataEnvironments) As DataTable
        Dim result As New DataTable
        result.Columns.Add(New DataColumn("Seq", GetType(System.Int16)))
        result.Columns.Add(New DataColumn("Name", GetType(System.String)))
        result.PrimaryKey = {result.Columns("Name")}

        Dim LibraryList As New List(Of String)
        Dim LLList As String = C2App.Data.Call_DNSP0011(AppCode, Env)
        If Not String.IsNullOrWhiteSpace(LLList) Then
            LibraryList = LLList.Split(",").ToList
        End If

        Dim seq As Short = 0
        For Each library In LibraryList
            Dim row = result.NewRow
            row("seq") = seq
            row("name") = library
            result.Rows.Add(row)
            seq += 1
        Next

        Return result
    End Function

    Friend Function DNRG0011_List(AppCode As String, Env As DataEnvironments) As List(Of String)
        Dim result As New List(Of String)

        Dim LLList As String = C2App.Data.Call_DNSP0011(AppCode, Env)
        If Not String.IsNullOrWhiteSpace(LLList) Then
            result = LLList.Split(",").ToList
        End If

        Return result
    End Function

#End Region


End Class