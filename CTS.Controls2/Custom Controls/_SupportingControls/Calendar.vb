Imports System.Globalization

Public Class Calendar

    Dim w As Short = 40

    Dim DisplayDate As DateTime = Today


    Private Sub Calendar_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Reload(DisplayDate.Year, DisplayDate.Month)

    End Sub

    Private Sub Calendar_Paint(sender As Object, e As PaintEventArgs) Handles MyBase.Paint


    End Sub


    Public Sub Reload(_Year As Short, _month As Short)
        pnlDays.Controls.Clear()

        Dim workdate As New Date(_Year, _month, 1)
        Dim dow = workdate.DayOfWeek
        Dim first As Boolean = False

        lblCalendarLabel.Text = $"{CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(workdate.Month)} {workdate.Year}"

        Dim t = 5
        Dim l = 5

        For r = 0 To 5
            For c = 0 To 6

                If _month <> workdate.Month Then
                    Continue For
                End If

                If Not first Then
                    If c < dow Then
                        Continue For
                    Else
                        first = True
                    End If
                End If

                Dim d As New CalDay(workdate)
                d.Top = t + (r * w)
                d.Left = l + (c * w)
                pnlDays.Controls.Add(d)

                workdate = workdate.AddDays(1)
            Next
        Next

        Refresh()

    End Sub

    Private Sub Calendar_DoubleClick(sender As Object, e As EventArgs) Handles MyBase.DoubleClick
        Reload(DisplayDate.Year, DisplayDate.Month)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        DisplayDate = DisplayDate.AddMonths(-1)
        Reload(DisplayDate.Year, DisplayDate.Month)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        DisplayDate = DisplayDate.AddMonths(1)
        Reload(DisplayDate.Year, DisplayDate.Month)
    End Sub
End Class
