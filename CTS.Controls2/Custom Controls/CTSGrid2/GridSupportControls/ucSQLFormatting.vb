Public Class ucSQLFormatting
    Public SelectClause As String
    Public FromClause As String
    Public InternalWhere As String
    Public WhereClause As String
    Public GroupByClause As String

    Public Sub FormatSQLData()

        Dim sb = New System.Text.StringBuilder()
        sb.Append("{\rtf1\ansi")

        If SelectClause <> String.Empty Then
            sb.Append("\b SELECT \b0")
            sb.AppendLine("\line")

            Dim f() As String = SelectClause.Split(",")
            For i As Integer = 0 To f.GetUpperBound(0) - 1
                f(i) = f(i) & ","
            Next
            For Each s As String In f
                sb.Append("\tab " & s)
                sb.AppendLine("\line")
            Next
        End If

        If FromClause <> String.Empty Then
            sb.Append("\b FROM \b0")
            sb.AppendLine("\line")

            FromClause = FromClause.Replace("LEFT JOIN", ",LJ")
            FromClause = FromClause.Replace("INNER JOIN", ",IJ")
            FromClause = FromClause.Replace("EXCEPTION JOIN", ",EJ")
            FromClause = FromClause.Replace("JOIN", ",J")

            Dim f() As String = FromClause.Split(",")
            For Each s As String In f
                If Mid(UCase(s), 1, 5) = "FROM " Then s = Mid(s, 5)
                If Mid(s, 1, 3) = "LJ " Then s = s.Replace("LJ ", "Left Join ")
                If Mid(s, 1, 3) = "IJ " Then s = s.Replace("IJ ", "Inner Join ")
                If Mid(s, 1, 3) = "EJ " Then s = s.Replace("EJ ", "Exception Join ")
                If Mid(s, 1, 2) = "J " Then s = s.Replace("J ", "Join ")
                sb.Append("\tab " & s)
                sb.AppendLine("\line")
            Next

        End If


        If InternalWhere <> String.Empty Or WhereClause <> String.Empty Then
            sb.Append("\b WHERE \b0")
            sb.AppendLine("\line")

            If Mid(UCase(InternalWhere), 1, 6) = "WHERE " Then InternalWhere = Mid(InternalWhere, 6)
            sb.Append("\tab " & InternalWhere)
            sb.AppendLine("\line")

            If Mid(UCase(WhereClause), 1, 6) = "WHERE " Then WhereClause = Mid(WhereClause, 6)
            sb.Append("\tab " & WhereClause)
            sb.AppendLine("\line")
        End If

        If GroupByClause <> String.Empty Then
            sb.Append("\b GROUP BY \b0")
            sb.AppendLine("\line")

            If GroupByClause.ToUpper.StartsWith("GROUP BY ") Then GroupByClause = Mid(GroupByClause, 9)
            sb.Append("\tab " & GroupByClause)
            sb.AppendLine("\line")
        End If

        sb.Append("}")
        RichTextBox1.Rtf = sb.ToString()


    End Sub



End Class
