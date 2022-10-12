Imports System.Runtime.InteropServices
Imports CTS.Extensions
Imports Filtering

Public Module modControls

    Public C2App As New CTS.AppInstance
    Public C2Data As New CTS.AppInstance

    'colors 
    Public CooperGreen As Color = Color.FromArgb(0, 64, 0)
    Public LighterBlue As Color = Color.FromArgb(93, 128, 174)
    Public DarkerBlue As Color = Color.FromArgb(15, 60, 120)

    'color functions
    Public GridPrimary As Color = DarkerBlue
    Public GridSecondary As Color = Color.WhiteSmoke

    Public SelectedRowBorder As Color = DarkerBlue
    Public SelectedBackColor As Color = Color.LightBlue
    Public HoverBorder As Color = DarkerBlue

    Public EditErrorBack As Color = Color.Red
    Public EditErrorFore As Color = Color.White
    Public EditBack As Color = Color.LemonChiffon
    Public EditFocusBack As Color = Color.PaleTurquoise
    Public EditProtect As Color = Color.White
    Public EditPending As Color = Color.DarkCyan

    Public CooperPrompt As Color = Color.FromArgb(50, 100, 200)
    Public CooperPrompt2 As Color = Color.CornflowerBlue
    Public GridBackgroundImage As Bitmap = My.Resources.gradientblue

    Public IsDeveloper As Boolean = False

#Region "Mousedown: Move & Resize Contants"
    Friend Declare Sub ReleaseCapture Lib "user32" ()
    Friend Const WM_NCLBUTTONDOWN As Integer = &HA1
    Friend Const HTBORDER As Integer = 18
    Friend Const HTBOTTOM As Integer = 15
    Friend Const HTBOTTOMLEFT As Integer = 16
    Friend Const HTBOTTOMRIGHT As Integer = 17
    Friend Const HTCAPTION As Integer = 2
    Friend Const HTLEFT As Integer = 10
    Friend Const HTRIGHT As Integer = 11
    Friend Const HTTOP As Integer = 12
    Friend Const HTTOPLEFT As Integer = 13
    Friend Const HTTOPRIGHT As Integer = 14
#End Region







    Public Function SaveAs(G As Grid, NewName As String) As Grid
        Dim NewG As New Grid

        If GridExists(NewName) Then
            'Throwexception("already exists")
        Else
            Dim dt2500 As DataTable = C2App.Data.DB2GetEmptyTable("dnpf2500")
            Dim dr2500 As DataRow = dt2500.NewRow
            dr2500("ghname") = NewName
            dr2500("ghtitle") = G.Title
            dr2500("ghllist") = G.LibraryList
            dr2500("ghdistinct") = G.Distinct.ToStringDB2
            dr2500("ghfco") = G.FromClause
            dr2500("ghwhere") = G.WhereClause
            dr2500("ghorder") = G.OrderByClause
            dr2500("ghgroup") = G.GroupByClause
            dr2500("ghrtot") = G.TotalsRow.ToStringDB2
            C2App.Data.DB2RowInsert(dr2500)


            For Each gridcol In G.Columns
                Dim dt2510 As DataTable = C2App.Data.DB2GetEmptyTable("dnpf2510")
                Dim dr2510 As DataRow = dt2510.NewRow
                dr2510("gcname") = NewName
                dr2510("gcfield") = gridcol.Field
                dr2510("gcfile") = gridcol.File
                dr2510("gctext") = gridcol.Text
                dr2510("gcheader") = gridcol.HeaderText
                dr2510("gcseq") = gridcol.Seq
                dr2510("gcwidth") = gridcol.Width
                dr2510("gcsort") = gridcol.SortSeq
                dr2510("gcdir") = IIf(gridcol.SortOrder = SortOrder.Ascending, "ASC", "DESC")
                dr2510("gcalign") = gridcol.Alignment
                dr2510("gcrestrict") = gridcol.Restrict.ToStringDB2
                dr2510("gchidden") = gridcol.Hidden.ToStringDB2
                dr2510("gcsumcol") = gridcol.SumColumn.ToStringDB2
                dr2510("gcformat") = gridcol.Format
                dr2510("gcsumfmt") = gridcol.SumFormat
                dr2510("gccoltyp") = gridcol.ColumnType.ToString
                dr2510("gcptype") = gridcol.ProviderTypeCode
                dr2510("gcsize") = gridcol.Size
                dr2510("gcscale") = gridcol.Scale
                dr2510("gcJDECVT") = gridcol.CastCCSID37.ToStringDB2
                dr2510("gcfunction") = gridcol.SQLFunction
                dr2510("gcscale") = gridcol.SecurityType
                dr2510("gcpresel") = gridcol.Preselect.ToStringDB2
                dr2510("gcalwupd") = gridcol.Updatable.ToStringDB2
                C2App.Data.DB2RowInsert(dr2510)
            Next

        End If


        Return NewG
    End Function



    Public Function GridExists(gridname As String) As Boolean
        Dim result As Boolean
        If C2App.Data.GetSQLInteger($"Select count(*) from dnpf2500 where ghname = '{gridname.Trim}'") > 0 Then
            result = True
        Else
            result = False
        End If
        Return result
    End Function

End Module

Public Module modGrid

    Public CurGrid As String
    Public CurColumn As GridColumn
    Public CurValue As String
    Public dtGridData As DataTable

#Region "API: GetScrollBarInfo"
    Friend Const CONST_OBJID_VSCROLL As UInteger = &HFFFFFFFBUI
    Friend Const CONST_OBJID_HSCROLL As UInteger = &HFFFFFFFAUI

    Friend Const STATE_SYSTEM_UNAVAILABLE As Integer = &H1
    Friend Const STATE_SYSTEM_PRESSED As Integer = &H8
    Friend Const STATE_SYSTEM_INVISIBLE As Integer = &H8000
    Friend Const STATE_SYSTEM_OFFSCREEN As Integer = &H10000

    'API Structires.
    <StructLayout(LayoutKind.Sequential)>
    Public Structure SCROLLBARINFO
        Public cbSize As Integer
        Public rcScrollBar As Rectangle
        Public dxyLineButton As Integer
        Public xyThumbTop As Integer
        Public xyThumbBottom As Integer
        Public reserved As Integer
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=6)>
        Public rgstate As Integer()
    End Structure

    'API Calls.
    <DllImport("user32.dll", EntryPoint:="GetScrollBarInfo")>
    Public Function GetScrollBarInfo(hWnd As IntPtr, idObject As UInteger, ByRef psbi As SCROLLBARINFO) As Integer
    End Function

    Public Function IsVScrollBarVisibleXX(ByVal c As Control) As Boolean
        Dim holdScroll As New SCROLLBARINFO()
        holdScroll.cbSize = Marshal.SizeOf(holdScroll)
        Dim returnValue As Integer = GetScrollBarInfo(c.Handle, CONST_OBJID_VSCROLL, holdScroll)
        Dim result As Integer = holdScroll.rgstate(0)

        If result = STATE_SYSTEM_INVISIBLE Then
            Return False
        Else
            Return True
        End If

    End Function

    Public Function IsVScrollBarVisible(ByVal ctrl As Control) As Boolean
        For Each c In ctrl.Controls
            If c.GetType() Is GetType(VScrollBar) Then
                If c.Visible = True Then
                    Return True
                Else
                    Return False
                End If
            End If
        Next
        Return Nothing
    End Function

#End Region


    Friend Function GetDataTypeFromProvider(ByVal ProviderType As Integer) As String
        '---------------------------------------------------------------
        ' Function to determine the column type to be assigned in the grid
        ' 2 = Small int
        ' 4 = Numeric Pack
        ' 5 = Numberic Zone
        ' 6 = Sting
        ' 12 = Date
        ' 13 = Time
        ' 14 = Timestamp
        '------------------------------------------------------------------      
        Dim vbtype As String = "System.String"
        Select Case ProviderType
            Case 1
                vbtype = "System.Int64"
            Case 2
                vbtype = "System.Int32"
            Case 3
                vbtype = "System.Int16"
            Case 4
                vbtype = "System.Decimal"
            Case 5
                vbtype = "System.Decimal"
            Case 6
                vbtype = "System.String"
            Case 12
                'Date only
                vbtype = "System.DateTime"
            Case 13
                'Time only
                vbtype = "System.DateTime"
            Case 14
                'timestamp
                vbtype = "System.DateTime"
        End Select

        Return vbtype
    End Function

    Friend Sub BuildSQLFields(ByVal GCFIELD As String, ByVal GCFUNCTION As String, ByVal GCPTYPE As String,
                          ByVal GCJDECVT As Boolean, ByVal GCSIZE As Integer, ByVal SuppressTrim As Boolean,
                          ByRef myFields As String)

        Dim NextField As String

        If GCPTYPE = "6" And SuppressTrim Then
            'Figure out the next SQL column/field text 
            If GCFUNCTION = String.Empty Then
                NextField = GCFIELD
            Else
                If GCFIELD = String.Empty Then
                    NextField = GCFUNCTION
                Else
                    NextField = GCFUNCTION & " as " & GCFIELD
                End If
            End If

        ElseIf GCPTYPE = "6" And Not SuppressTrim Then
            'Figure out the next SQL column/field text 
            If GCFUNCTION = String.Empty Then
                NextField = "Trim(" & GCFIELD & ") as " & GCFIELD
            Else
                If GCFIELD = String.Empty Then
                    NextField = "Trim(" & GCFUNCTION & ") as " & GCFUNCTION
                Else
                    NextField = "Trim(" & GCFUNCTION & ") as " & GCFIELD
                End If
            End If

        ElseIf GCPTYPE = "8" And SuppressTrim Then

            'Figure out the next SQL column/field text 
            If GCFUNCTION = String.Empty Then
                If GCJDECVT Then
                    NextField = "Cast(" & GCFIELD & " as CHAR(" & GCSIZE & ") CCSID 37) as " & GCFIELD
                Else
                    NextField = GCFIELD
                End If

            Else
                If GCFIELD = String.Empty Then
                    NextField = GCFUNCTION
                Else
                    NextField = GCFUNCTION & " as " & GCFIELD
                End If
            End If

        ElseIf GCPTYPE = "8" And Not SuppressTrim Then

            'Figure out the next SQL column/field text 
            If GCFUNCTION = String.Empty Then
                If GCJDECVT Then
                    NextField = "Trim(Cast(" & GCFIELD & " as CHAR(" & GCSIZE & ") CCSID 37)) as " & GCFIELD
                Else
                    NextField = "Trim(" & GCFIELD & ") as " & GCFIELD
                End If

            Else
                If GCFIELD = String.Empty Then
                    NextField = "Trim(" & GCFUNCTION & ") as " & GCFUNCTION
                Else
                    NextField = "Trim(" & GCFUNCTION & ") as " & GCFIELD
                End If
            End If

        Else
            'Figure out the next SQL column/field text 
            If GCFUNCTION = String.Empty Then
                NextField = Trim(GCFIELD)
            Else
                If GCFIELD = String.Empty Then
                    NextField = GCFUNCTION
                Else
                    NextField = GCFUNCTION & " as " & GCFIELD
                End If
            End If
        End If


        'Put the string together 
        If myFields = "" Then
            myFields = NextField
        Else
            myFields = myFields & "," & NextField
        End If

    End Sub

    'Friend Function BuildSQLWhereX(ByVal lFilter As List(Of GridFilter)) As String

    '    Dim strWhere As String = String.Empty
    '    Dim Value1 As String = String.Empty
    '    Dim Value2 As String = String.Empty

    '    If lFilter IsNot Nothing Then
    '        For Each e As Filtering.GridFilter In lFilter
    '            Dim strValue As String = String.Empty

    '            If e.Value = "*BLANKS" Then
    '                Value1 = String.Empty
    '            Else
    '                Value1 = e.Value
    '            End If

    '            If e.Value2 = "*BLANKS" Then
    '                Value2 = String.Empty
    '            Else
    '                Value2 = e.Value2
    '            End If

    '            If e.Value = "*ISNULL" Then
    '                e.OpCode = "IS"
    '                strValue = "NULL"

    '            ElseIf e.Value = "*NOTNULL" Then
    '                e.OpCode = "IS NOT"
    '                strValue = "NULL"

    '            Else
    '                Select Case e.OpCode
    '                    Case "BETWEEN"
    '                        Select Case e.Fldt
    '                            Case "System.String"
    '                                strValue = String.Format("'{0}' AND '{1}'", Value1, Value2)
    '                            Case "System.DateTime"
    '                                strValue = String.Format("'{0}' AND '{1}'", Value1, Value2)
    '                            Case Else
    '                                strValue = String.Format("{0} AND {1}", Value1, Value2)
    '                        End Select

    '                    Case "IN"
    '                        'Uses the valuelist array
    '                        Dim sep As String = String.Empty
    '                        For Each s In e.ValueList
    '                            If s = "*BLANKS" Then
    '                                s = String.Empty
    '                            End If

    '                            Select Case e.Fldt
    '                                Case "System.String"
    '                                    strValue = String.Format("{0}{1} '{2}'", strValue, sep, s)
    '                                Case "System.DateTime"
    '                                    strValue = String.Format("{0}{1} '{2}'", strValue, sep, s)
    '                                Case Else
    '                                    strValue = String.Format("{0}{1} {2}", strValue, sep, s)
    '                            End Select
    '                            sep = ","
    '                        Next
    '                        strValue = String.Format("({0})", strValue)

    '                    Case Else
    '                        Select Case e.Fldt
    '                            Case "System.String"
    '                                strValue = String.Format("'{0}'", Value1)

    '                            Case "System.DateTime"
    '                                strValue = String.Format("'{0}'", Value1)
    '                            Case Else
    '                                strValue = String.Format("{0}", Value1)
    '                        End Select
    '                End Select
    '            End If

    '            If strWhere = String.Empty Then

    '                If e.Fldt = "System.String" And e.TrimField Then
    '                    strWhere = String.Format("Trim({0}) {1} {2}", e.FieldName, e.OpCode, strValue)
    '                Else
    '                    strWhere = String.Format("{0} {1} {2}", e.FieldName, e.OpCode, strValue)
    '                End If

    '            Else
    '                If e.Fldt = "System.String" And e.TrimField Then
    '                    strWhere = String.Format("{0} {1} Trim({2}) {3} {4}", strWhere, e.AO, e.FieldName, e.OpCode, strValue)
    '                Else
    '                    strWhere = String.Format("{0} {1} {2} {3} {4}", strWhere, e.AO, e.FieldName, e.OpCode, strValue)
    '                End If
    '            End If
    '        Next

    '    End If
    '    Return strWhere

    'End Function

    Friend Function BuildSQLWhere(ByVal lfilter As Filter) As String
        Dim strWhere As String = String.Empty
        Dim Value1 As String = String.Empty
        Dim TrimValue As Boolean = True

        If lfilter IsNot Nothing Then
            For Each e As Filtering.Condition In lfilter.Conditions
                Dim strValue As String = String.Empty

                TrimValue = e.Trim

                If e.Values.Count > 0 Then
                    Value1 = e.Values(0)
                Else
                    Value1 = e.Value
                End If

                If Value1 = "*BLANKS" Then Value1 = String.Empty

                If Value1 = "*ISNULL" Then
                    e.Op = OperatorName.IsNull
                End If


                Select Case e.Op
                    Case OperatorName.IsNull, OperatorName.IsNotNull
                        strValue = "NULL"

                    Case OperatorName.Between, OperatorName.NotBetween

                        Dim Value2 As String = String.Empty
                        If e.Values.Count > 1 Then Value2 = e.Values(1)
                        If Value2 = "*BLANKS" Then Value2 = String.Empty


                        If e.InsertQuotes Then
                            strValue = String.Format("'{0}' AND '{1}'", Value1, Value2)
                        Else
                            strValue = String.Format("{0} AND {1}", Value1, Value2)
                        End If

                    Case OperatorName.In, OperatorName.NotIn
                        'Uses the valuelist array
                        Dim sep As String = String.Empty
                        For Each s In e.Values
                            If s = "*BLANKS" Then
                                s = String.Empty
                            End If

                            If e.InsertQuotes Then
                                strValue = String.Format("{0}{1} '{2}'", strValue, sep, s)
                            Else
                                strValue = String.Format("{0}{1} {2}", strValue, sep, s)
                            End If

                            sep = ","
                        Next
                        strValue = String.Format("({0})", strValue)

                    Case OperatorName.None


                    Case Else
                        If e.InsertQuotes Then
                            strValue = String.Format("'{0}'", Value1)
                        Else
                            strValue = String.Format("{0}", Value1)
                        End If
                End Select


                'todo handle NOT property



                Dim NewConditionText As String
                If String.IsNullOrWhiteSpace(e.Text) Then
                    If TrimValue Then
                        NewConditionText = $"Trim({e.ColumnName}) {GetOperator(e.Op)} {strValue}"
                    Else
                        NewConditionText = $"{e.ColumnName} {GetOperator(e.Op)} {strValue}"
                    End If
                Else
                    NewConditionText = e.Text
                End If

                If strWhere = String.Empty Then
                    strWhere = NewConditionText
                Else
                    strWhere = $"{strWhere} {e.LOP} {NewConditionText}"
                End If



                If strWhere = String.Empty Then
                    'If TrimValue Then
                    '    strWhere = String.Format("Trim({0}) {1} {2}", e.ColumnName, GetOperator(e.Op), strValue)
                    'Else
                    '    strWhere = String.Format("{0} {1} {2}", e.ColumnName, GetOperator(e.Op), strValue)
                    'End If

                    'If e.Fldt = "System.String" And e.Trim Then
                    '    strWhere = String.Format("Trim({0}) {1} {2}", e.ColumnName, GetOperator(e.Op), strValue)
                    'Else
                    '    strWhere = String.Format("{0} {1} {2}", e.ColumnName, GetOperator(e.Op), strValue)
                    'End If

                Else
                    'If TrimValue Then
                    '    strWhere = String.Format("{0} {1} Trim({2}) {3} {4}", strWhere, e.LOP, e.ColumnName, GetOperator(e.Op), strValue)
                    'Else
                    '    strWhere = String.Format("{0} {1} {2} {3} {4}", strWhere, e.LOP, e.ColumnName, GetOperator(e.Op), strValue)
                    'End If

                    'If e.Fldt = "System.String" And e.Trim Then
                    '    strWhere = String.Format("{0} {1} Trim({2}) {3} {4}", strWhere, e.LOP, e.ColumnName, GetOperator(e.Op), strValue)
                    'Else
                    '    strWhere = String.Format("{0} {1} {2} {3} {4}", strWhere, e.LOP, e.ColumnName, GetOperator(e.Op), strValue)
                    'End If
                End If

            Next

        End If
        Return strWhere

    End Function



    Friend Function TestEdittedValue(ByVal val As Object, ByVal typ As String) As Boolean

        Dim x As Object

        If Trim(val) = String.Empty Then
            Return True
        End If

        Try
            Select Case typ
                Case "System.String"
                    x = CType(val, String)
                Case "System.Decimal"
                    x = CType(val, Decimal)
                Case "System.Integer"
                    x = CType(val, Integer)
                Case "System.DateTime"
                    x = CType(val, DateTime)
                Case "System.Date"
                    x = CType(val, Date)
            End Select
            Return True
        Catch
            Return False
        End Try

    End Function

    Friend Function HasCellErrors(ByVal dgvr As DataGridViewRow) As Boolean
        For Each c As DataGridViewCell In dgvr.Cells
            If c.ErrorText <> String.Empty Then
                Return True
            End If
        Next
        Return False
    End Function

    Public Sub ClearCellErrorsByRow(ByVal dgvr As DataGridViewRow)
        For Each c As DataGridViewCell In dgvr.Cells
            If c.ErrorText <> String.Empty Then
                c.ErrorText = String.Empty
            End If
        Next
    End Sub

    Public Function HasRowErrors(ByVal dgv As DataGridView) As Boolean
        For Each r As DataGridViewRow In dgv.Rows
            If r.ErrorText <> String.Empty Then
                Return True
            End If
        Next
        Return False

    End Function

    Public Sub ClearRowErrors(ByVal dgv As DataGridView)

        For Each r As DataGridViewRow In dgv.Rows
            If r.ErrorText <> String.Empty Then
                r.ErrorText = String.Empty
            End If
        Next
    End Sub

    Public Function GetDataRowState(ByVal thisrow As DataGridViewRow) As DataRowState
        Try
            Dim drv As DataRowView = thisrow.DataBoundItem

            If drv.Row.RowState = DataRowState.Detached Then
                Return DataRowState.Added
            End If

            Return drv.Row.RowState
        Catch
            Return Nothing
        End Try

    End Function



    Public Sub GetDateRange(ByVal strType As String, ByVal CurDate As Date, ByRef BeginDate As Date, ByRef EndDate As Date)

        Select Case strType
            Case "Current Day"
                BeginDate = DateSerial(CurDate.Year, CurDate.Month, CurDate.Day)
                EndDate = DateAdd(DateInterval.Day, 1, BeginDate)
                EndDate = DateAdd(DateInterval.Second, -1, EndDate)

            Case "Current Month"
                BeginDate = DateSerial(CurDate.Year, CurDate.Month, 1)
                EndDate = DateAdd(DateInterval.Month, 1, BeginDate)
                EndDate = DateAdd(DateInterval.Second, -1, EndDate)

            Case "Current Year"
                BeginDate = DateSerial(CurDate.Year, 1, 1)
                EndDate = DateAdd(DateInterval.Year, 1, BeginDate)
                EndDate = DateAdd(DateInterval.Second, -1, EndDate)

            Case "Prior Month"
                BeginDate = DateSerial(CurDate.Year, CurDate.Month, 1)
                BeginDate = DateAdd(DateInterval.Month, -1, BeginDate)
                EndDate = DateAdd(DateInterval.Month, 1, BeginDate)
                EndDate = DateAdd(DateInterval.Second, -1, EndDate)

            Case "Prior Year"
                BeginDate = DateSerial(CurDate.Year, 1, 1)
                BeginDate = DateAdd(DateInterval.Year, -1, BeginDate)
                EndDate = DateAdd(DateInterval.Year, 1, BeginDate)
                EndDate = DateAdd(DateInterval.Second, -1, EndDate)

        End Select

    End Sub








    'filter find tools;  depricated
    'Dim TempField As String = String.Empty
    'Dim TempOpCode As String = String.Empty
    'Public Function GetFilter(ByVal UF As List(Of Filtering.GridFilter), ByVal strFieldName As String, ByVal strOpCode As String) As Filtering.GridFilter
    '    TempField = strFieldName
    '    TempOpCode = strOpCode
    '    Dim result As Filtering.GridFilter
    '    result = UF.Find(AddressOf FindFilter)
    '    Return result
    'End Function
    'Private Function FindFilter(f As Filtering.GridFilter) As Boolean
    '    If f.FieldName = TempField And f.OpCode = TempOpCode Then
    '        Return True
    '    Else
    '        Return False
    '    End If
    'End Function
    'Public Function GetColumnFilters(ByVal UF As List(Of Filtering.GridFilter), ByVal strFieldName As String) As List(Of Filtering.GridFilter)
    '    TempField = strFieldName
    '    Return UF.FindAll(AddressOf FindColumnFilters)
    'End Function
    'Private Function FindColumnFilters(f As Filtering.GridFilter) As Boolean
    '    If f.FieldName = TempField Then
    '        Return True
    '    Else
    '        Return False
    '    End If
    'End Function
    'Public Function FindFilterIdx(ByVal fl As List(Of Filtering.GridFilter), ColName As String, OpCode As String) As Integer
    '    Dim i As Integer
    '    For i = 0 To fl.Count - 1
    '        If fl(i).FieldName = ColName And fl(i).OpCode = OpCode Then
    '            Exit For
    '        End If
    '    Next
    '    Return i
    'End Function

    'Public Function GetFilterDescriptionX(ByRef _UF As Filtering.GridFilter) As String
    '    Dim strReturn As String

    '    If String.IsNullOrEmpty(_UF.Desc) Then

    '        Dim Val As String

    '        Select Case _UF.OpCode
    '            Case "BETWEEN"
    '                Val = _UF.Value & " and " & _UF.Value2
    '            Case "IN"
    '                Dim ara As String()
    '                ReDim ara(_UF.ValueList.Count - 1)
    '                _UF.ValueList.CopyTo(ara)
    '                Val = Join(ara, ",")
    '            Case Else
    '                Val = _UF.Value
    '        End Select

    '        Dim OpText As String
    '        Select Case _UF.OpCode
    '            Case "="
    '                OpText = "Equals"
    '            Case ">"
    '                OpText = "Greater Than"
    '            Case "<"
    '                OpText = "Less Than"
    '            Case "<="
    '                OpText = "Less Than/Equal"
    '            Case ">="
    '                OpText = "Greater Than/Equal"
    '            Case "BETWEEN"
    '                OpText = "Between"
    '            Case Else
    '                OpText = _UF.OpCode
    '        End Select

    '        strReturn = String.Format("{0} {1}", OpText, Val)
    '    Else
    '        strReturn = _UF.Desc
    '    End If

    '    Return strReturn
    'End Function




#Region "Export/Excel"

    Public Sub ExportXLS(dgv As DataGridView, SheetName As String)

        'Dim xlapp As Object = New System.Dynamic.ExpandoObject()
        Dim xlapp As Object
        Dim xlworkbook As Object
        Dim xlworksheet As Object
        Dim xlStyle As Object

        xlapp = CreateObject("Excel.Application")

        'Export DataGridView to Excel
        'Dim xlapp As Excel.Application
        'Dim xlworkbook As Excel.Workbook
        'Dim xlworksheet As Excel.Worksheet
        'Dim xlstyle As Excel.Style
        'Dim xlRange As Excel.Range

        Dim misValue As Object = System.Reflection.Missing.Value
        Dim strSheet As String

        If SheetName = String.Empty Then
            strSheet = "Sheet1"
        Else
            SheetName = SheetName.Replace(":", "")
            SheetName = SheetName.Replace("*", "")
            SheetName = SheetName.Replace("/", "")
            SheetName = SheetName.Replace("\", "")
            SheetName = SheetName.Replace("[", "")
            SheetName = SheetName.Replace("]", "")
            SheetName = SheetName.Trim
            strSheet = Mid(SheetName, 1, 31)
        End If

        'xlapp = New Excel.Application
        xlworkbook = xlapp.Workbooks.Add(misValue)
        xlworksheet = xlworkbook.Sheets("Sheet1")

        Try
            xlworksheet.Name = strSheet
        Catch
        End Try

        xlStyle = xlworksheet.Application.ActiveWorkbook.Styles.Add("HeaderStyle")
        xlStyle.Interior.Color = System.Drawing.ColorTranslator.ToOle(GridPrimary)
        xlStyle.Font.Color = System.Drawing.ColorTranslator.ToOle(Color.White)
        xlStyle.Font.Bold = True

        Dim ColIdx As Integer = 0
        For Each dgvc As DataGridViewColumn In dgv.Columns
            If dgvc.Visible Then
                ColIdx += 1
                xlworksheet.Cells(1, ColIdx) = dgvc.HeaderText
                xlworksheet.Columns(ColIdx).AutoFit()
                xlworksheet.Cells(1, ColIdx).Style = "HeaderStyle"
            End If
        Next

        For Each dgvr As DataGridViewRow In dgv.Rows
            ColIdx = 0
            For Each dgvcell As DataGridViewCell In dgvr.Cells
                If dgvcell.Visible Then
                    ColIdx += 1
                    xlworksheet.Cells(dgvcell.RowIndex + 2, ColIdx) = dgvcell.Value.ToString
                End If
            Next
        Next

        xlapp.Visible = True

        'Automatically save?  not yet...
        If 1 = 2 Then
            Dim dialog As New SaveFileDialog
            Dim result As DialogResult = dialog.ShowDialog

            Try
                xlworksheet.SaveAs(dialog.FileName)
            Catch ex As Exception
            End Try

            xlworkbook.Close()
            xlapp.Quit()
        End If

        releaseObject(xlapp)
        releaseObject(xlworkbook)
        releaseObject(xlworksheet)
    End Sub

    Private Sub releaseObject(ByVal obj As Object)
        Try
            System.Runtime.InteropServices.Marshal.ReleaseComObject(obj)
            obj = Nothing
        Catch ex As Exception
            obj = Nothing
        Finally
            GC.Collect()
        End Try
    End Sub

#End Region



End Module

Class ListViewItemComparer
    Implements IComparer
    Private col As Integer

    Public Sub New()
        col = 0
    End Sub

    Public Sub New(column As Integer)
        col = column
    End Sub

    Public Function Compare(x As Object, y As Object) As Integer _
                        Implements System.Collections.IComparer.Compare
        Dim returnVal As Integer = -1
        returnVal = [String].Compare(CType(x,
                    ListViewItem).SubItems(col).Text,
                    CType(y, ListViewItem).SubItems(col).Text)
        Return returnVal
    End Function
End Class

Class RowEventArgs
    Property sender As Object
    Property CurrentRow As DataGridViewRow

End Class
