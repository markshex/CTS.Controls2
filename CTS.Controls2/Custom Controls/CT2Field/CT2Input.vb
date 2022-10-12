Imports CTS.Controls2

Public Class CT2Input

#Region "Declarations/Properties"

    Public WithEvents FieldText As TextBoxCT2
    Public WithEvents FieldCombo As ComboBoxCTS
    Public WithEvents FieldCheck As CheckBoxCT2
    Public WithEvents FieldDateTime As CT2DateTime

    Private TextDrawSize As Size
    Private TextBack As Color


    Private _ValueText As String = String.Empty
    Private _Value As Object
    Public Property [Value] As Object
        Set(value As Object)
            _Value = value
            If value IsNot Nothing Then
                _ValueText = value.ToString

                If TypeOf value Is [Enum] Then
                    If EditType <> EditTypes.RadioButtons And EditType <> EditTypes.Combo Then
                        EditType = EditTypes.Combo
                    End If
                End If

                If TypeOf value Is Date Then
                    If EditType <> EditTypes.DateTime And EditType <> EditTypes.Date And EditType <> EditTypes.Time Then
                        EditType = EditTypes.DateTime
                    End If
                End If

            End If
        End Set
        Get
            Return _Value
        End Get
    End Property

    Private gReadOnly As Boolean = False
    <System.ComponentModel.Description("Display the control as 'ReadOnly'")>
    Public Property [ReadOnly]() As Boolean
        Get
            Return gReadOnly
        End Get
        Set(ByVal value As Boolean)

            If gReadOnly <> value Then

                gReadOnly = value

                If DesignMode Then
                    'RedrawControl()
                Else
                    'InitializeControlColor()
                    Me.Refresh()
                End If

            End If


            If gReadOnly Then TextBack = Color.White Else TextBack = EditBack

        End Set
    End Property


    Public Property LabelText As String = "Label"
    Public Property LabelWidth As Integer = 50
    Public Property TextWidth As Integer = 100
    Public Property TextLines As Integer = 1



    Private _EditType As EditTypes = EditTypes.Text
    Public Property EditType As EditTypes
        Set(value As EditTypes)
            _EditType = value
        End Set
        Get
            Return _EditType
        End Get
    End Property



    Public Enum EditTypes
        UseDataType = 0
        Text = 1
        [DateTime] = 2
        [Date] = 3
        [Time] = 4
        Combo = 5
        Check = 6
        RadioButtons = 7
    End Enum






    Private ControlFocus As Boolean = False
    Private DefaultHeight As Integer
#End Region


    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        DefaultHeight = getdefaultHeight()
    End Sub

    Private Sub CT2Input_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Controls.Remove(Label1)
        Me.Controls.Remove(Label2)
    End Sub

    Private Sub MePaint(sender As Object, e As PaintEventArgs) Handles Me.Paint
        WriteLabel()
        WriteValue(e.Graphics)
        Debug.Print($"{Name} MePaint")
    End Sub


    Private Sub WriteLabel()
        Dim flags As TextFormatFlags = TextFormatFlags.EndEllipsis + TextFormatFlags.WordBreak

        Dim labelheight = Height
        If Height < (DefaultHeight * TextLines) + 6 Then
            labelheight = (DefaultHeight * TextLines) + 6
        End If

        Dim r As Rectangle = New Rectangle(0, 3, LabelWidth, labelheight)


        Dim g As Graphics = Me.CreateGraphics
        g.FillRectangle(Brushes.Transparent, r)

        TextRenderer.DrawText(g, LabelText, Me.Font, r, ForeColor, flags)
    End Sub

    Private Sub WriteValue(g As Graphics)
        Select Case _EditType
            Case EditTypes.Text
                WriteText(g)
            Case EditTypes.Combo
                WriteCombo(g)
            Case EditTypes.Check
                WriteCheck()
            Case EditTypes.DateTime
                WriteDateTime(g)
            Case EditTypes.RadioButtons
                WriteRadio(g)
        End Select
    End Sub

    Private Sub WriteText(g As Graphics)

        If Not ControlFocus Then
            TextDrawSize = New Size(TextWidth, (DefaultHeight * TextLines) + 6)
            Dim r As Rectangle = New Rectangle(New Point(LabelWidth, 0), TextDrawSize)
            Dim flags As TextFormatFlags = TextFormatFlags.EndEllipsis
            If TextLines > 1 Then flags += TextFormatFlags.WordBreak

            Dim b As New SolidBrush(TextBack)
            g.FillRectangle(b, r)
            TextRenderer.DrawText(g, _ValueText, Me.Font, New Rectangle(r.Left - 1, r.Top + 2, r.Width, r.Height), Me.ForeColor, TextBack, flags)
            g.DrawRectangle(Pens.Black, Rectangle.Round(r))
        End If

    End Sub

    Private Sub WriteRadio(g As Graphics)

        If Not ControlFocus Then
            TextDrawSize = New Size(TextWidth, (DefaultHeight * TextLines) + 6)
            Dim r As Rectangle = New Rectangle(New Point(LabelWidth, 0), TextDrawSize)
            Dim flags As TextFormatFlags = TextFormatFlags.EndEllipsis
            If TextLines > 1 Then flags += TextFormatFlags.WordBreak
            g.FillRectangle(Brushes.LemonChiffon, r)
            TextRenderer.DrawText(g, _ValueText, Me.Font, New Rectangle(r.Left - 1, r.Top + 2, r.Width, r.Height), Me.ForeColor, Color.LemonChiffon, flags)
            g.DrawRectangle(Pens.Black, Rectangle.Round(r))
        End If

    End Sub

    Private Sub WriteCombo(g As Graphics)

        If Not ControlFocus Then
            TextDrawSize = New Size(TextWidth, (DefaultHeight * TextLines) + 6)
            Dim r As Rectangle = New Rectangle(LabelWidth, 0, TextWidth, (DefaultHeight * TextLines) + 6)
            Dim flags As TextFormatFlags = TextFormatFlags.EndEllipsis
            If TextLines > 1 Then flags += TextFormatFlags.WordBreak
            Dim b As New SolidBrush(TextBack)
            g.FillRectangle(b, r)
            TextRenderer.DrawText(g, _ValueText, Me.Font, New Rectangle(r.Left + 1, r.Top + 4, r.Width, r.Height), Me.ForeColor, TextBack, flags)
            g.DrawRectangle(Pens.Black, Rectangle.Round(r))
        End If

    End Sub

    Private Sub WriteDateTime(g As Graphics)

        If Not ControlFocus Then
            Dim width1 As Integer = 72
            Dim spacer As Integer = 4
            Dim width2 As Integer = 37

            Dim DateValue As Date = Date.MinValue
            If IsDate(_Value) Then
                DateValue = _Value
            End If

            Dim b As New SolidBrush(TextBack)
            Dim flags As TextFormatFlags = TextFormatFlags.Default

            Dim r1 As Rectangle = New Rectangle(LabelWidth, 0, width1, DefaultHeight + 8)
            g.FillRectangle(b, r1)
            TextRenderer.DrawText(g, DateValue.ToString("MM/dd/yyyy"), Me.Font, New Rectangle(r1.Left + 1, r1.Top + 3, r1.Width, r1.Height), Me.ForeColor, TextBack, flags)
            g.DrawRectangle(Pens.Black, Rectangle.Round(r1))

            Dim r2 As Rectangle = New Rectangle(LabelWidth + width1 + spacer, 0, width2, DefaultHeight + 8)
            g.FillRectangle(b, r2)
            TextRenderer.DrawText(g, DateValue.ToString("HH:mm"), Me.Font, New Rectangle(r2.Left + 1, r2.Top + 3, r2.Width, r2.Height), Me.ForeColor, TextBack, flags)
            g.DrawRectangle(Pens.Black, Rectangle.Round(r2))
        End If

    End Sub

    Private Sub WriteCheck()

        If Not ControlFocus Then
            Dim b As New SolidBrush(TextBack)
            Dim r As Rectangle = New Rectangle(LabelWidth, 0, TextWidth, DefaultHeight + 6)
            Dim g As Graphics = Me.CreateGraphics
            'Dim wdfont As New Font("Wingdings 2", Me.Font.Size)
            g.FillRectangle(b, LabelWidth, 0, 12, 12)
            If _Value Then
                g.DrawLine(New Pen(Color.Black, 1.5), New Point(LabelWidth + 1.5, 5.5), New Point(LabelWidth + 4, 9))
                g.DrawLine(New Pen(Color.Black, 1.5), New Point(LabelWidth + 4, 9), New Point(LabelWidth + 10, 3))
            End If
            g.DrawRectangle(Pens.Black, LabelWidth, 0, 12, 12)

        End If

    End Sub


    Private Function getdefaultHeight() As Integer
        Dim textsize As Size = TextRenderer.MeasureText(Me.CreateGraphics(), "X", Me.Font, New Size(0, 0), TextFormatFlags.Default)
        Return textsize.Height
    End Function

    Private Sub Me_FontChanged(sender As Object, e As EventArgs) Handles Me.FontChanged
        DefaultHeight = getdefaultHeight()
    End Sub

    Private Sub Me_Enter(sender As Object, e As EventArgs) Handles Me.Enter

        Select Case _EditType
            Case EditTypes.Text
                If FieldText Is Nothing Then
                    FieldText = New TextBoxCT2
                    FieldText.Name = $"{Name}_FieldText"
                    FieldText.Location = New Point(LabelWidth, 0)
                    If TextLines > 1 Then FieldText.Multiline = True
                    FieldText.Size = New Size(TextDrawSize.Width + 1, TextDrawSize.Height + 1)
                    FieldText.ReadOnly = [ReadOnly]
                    Me.Controls.Add(FieldText)
                    AddHandler FieldText.TabPress, AddressOf TabPress
                Else
                    FieldText.Visible = True
                End If

                If _Value IsNot Nothing Then
                    FieldText.Text = _Value.ToString
                End If

            Case EditTypes.Check
                If FieldCheck Is Nothing Then
                    FieldCheck = New CheckBoxCT2
                    FieldCheck.Name = $"{Name}_FieldCheck"
                    FieldCheck.Location = New Point(LabelWidth, 0)
                    FieldCheck.Size = New Size(TextWidth + 1, DefaultHeight)
                    FieldCheck.ReadOnly = [ReadOnly]
                    Me.Controls.Add(FieldCheck)
                Else
                    FieldCheck.Visible = True
                End If

                If _Value IsNot Nothing Then
                    FieldCheck.Checked = _Value
                End If


            Case EditTypes.Combo
                If FieldCombo Is Nothing Then
                    FieldCombo = New ComboBoxCTS
                    FieldCombo.Name = $"{Name}_FieldCombo"
                    FieldCombo.Location = New Point(LabelWidth, 0)
                    FieldCombo.Size = New Size(TextDrawSize.Width + 1, TextDrawSize.Height + 1)
                    FieldCombo.ReadOnly = [ReadOnly]

                    If _Value.GetType IsNot Nothing Then
                        If TypeOf Value Is [Enum] Then
                            For Each x In [Enum].GetValues(Value.GetType())
                                FieldCombo.Items.Add(x)
                            Next
                            FieldCombo.DropDownStyle = ComboBoxStyle.DropDownList
                        End If
                    End If

                    Me.Controls.Add(FieldCombo)
                Else
                    FieldCombo.Visible = True
                End If

                FieldCombo.SelectedItem = _Value


            Case EditTypes.RadioButtons
                FieldDateTime = New CT2DateTime

                Me.Controls.Add(FieldDateTime)

            Case EditTypes.DateTime
                If FieldDateTime Is Nothing Then
                    FieldDateTime = New CT2DateTime
                    FieldDateTime.Name = $"{Name}_FieldDateTime"
                    FieldDateTime.Location = New Point(LabelWidth, 0)
                    Me.Controls.Add(FieldDateTime)
                Else
                    FieldDateTime.Visible = True
                End If

                If _Value.GetType IsNot Nothing Then

                End If

                FieldDateTime.Value = _Value

        End Select

        Debug.Print($"{Name} enter")
        ControlFocus = True
    End Sub

    Private Sub TabPress(sender As Object)

        'Me.ProcessTabKey(True)

        'SelectNextControl(Me, True, True, False, True)


        Dim i = Me.TabIndex




    End Sub


    Private Sub Me_Leave(sender As Object, e As EventArgs) Handles Me.Leave

        Select Case _EditType
            Case EditTypes.Text
                Value = FieldText.Text
                FieldText.Visible = False
                'Me.Controls.Remove(FieldText)
            Case EditTypes.Check
                Value = FieldCheck.Checked
                FieldCheck.Visible = False
                'Me.Controls.Remove(FieldCheck)
            Case EditTypes.Combo
                Value = FieldCombo.SelectedItem
                FieldCombo.Visible = False
                'Me.Controls.Remove(FieldCombo)
            Case EditTypes.DateTime
                Value = FieldDateTime.Value
                FieldDateTime.Visible = False
                'Me.Controls.Remove(FieldDateTime)
        End Select

        ControlFocus = False

        Debug.Print($"{Name} left ")
    End Sub


    Private Sub CT2Input_PreviewKeyDown(sender As Object, e As PreviewKeyDownEventArgs) Handles MyBase.PreviewKeyDown
        If e.KeyCode = Keys.Tab Then
            Dim s = Keys.Tab
        End If
    End Sub

    Private Sub CT2Input_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Tab Then
            Dim s = Keys.Tab
        End If
    End Sub
End Class
