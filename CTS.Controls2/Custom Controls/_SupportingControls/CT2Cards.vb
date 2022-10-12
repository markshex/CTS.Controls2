Public Class CT2Cards

#Region "Declarations/Properties"
    Public Title As String

    Public DataSource As Object


    Public card As CT2Card




    Public FieldText As TextBoxCT2
    Public FieldCombo As ComboBoxCTS
    Public FieldCheck As CheckBoxCT2
    Public FieldDateTime As CTS.Controls2.CT2DateTime


    Public Property LabelText As String = "Label"


    Private _ValueText As String = String.Empty
    Private _Value As Object
    Public Property [Value] As Object
        Set(value As Object)
            _Value = value
            If value IsNot Nothing Then
                _ValueText = value.ToString
            End If
        End Set
        Get
            Return _Value
        End Get
    End Property

    Public Property DescriptionText As String = "Description"
    Public Property LabelWidth As Integer = 50
    Public Property TextWidth As Integer = 100



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









    Public DescriptionWidth As Integer = 100

    Private ControlFocus As Boolean = False
    Private DefaultHeight As Integer
#End Region

    Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        DefaultHeight = getdefaultHeight()
    End Sub

    Private Sub MePaint(sender As Object, e As PaintEventArgs) Handles Me.Paint
        WriteLabel()
        WriteValue()
        WriteDescription()
        Debug.Print($"{Name} MePaint")
    End Sub


    Private Sub WriteLabel()

        Dim r As Rectangle = New Rectangle(0, 0, LabelWidth, DefaultHeight + 4)
        Dim g As Graphics = Me.CreateGraphics
        g.FillRectangle(Brushes.Transparent, r)

        'Dim flags As TextFormatFlags = TextFormatFlags.WordBreak + TextFormatFlags.EndEllipsis + TextFormatFlags.TextBoxControl
        Dim flags As TextFormatFlags = TextFormatFlags.EndEllipsis
        TextRenderer.DrawText(g, LabelText, Me.Font, r, ForeColor, flags)


    End Sub

    Private Sub WriteValue()
        Select Case _EditType
            Case EditTypes.Text
                WriteText()
            Case EditTypes.Combo
                WriteText()
            Case EditTypes.Check
                WriteCheck()
        End Select
    End Sub

    Private Sub WriteText()

        If Not ControlFocus Then

            Dim r As Rectangle = New Rectangle(LabelWidth, 0, TextWidth, DefaultHeight + 4)
            Dim flags As TextFormatFlags = TextFormatFlags.EndEllipsis
            Dim g As Graphics = Me.CreateGraphics

            g.FillRectangle(Brushes.LemonChiffon, r)

            TextRenderer.DrawText(g, _ValueText, Me.Font, New Point(r.Left - 1, r.Top + 2), Me.ForeColor, Color.LemonChiffon, flags)

            g.DrawRectangle(Pens.Black, Rectangle.Round(r))

        End If

    End Sub

    Private Sub WriteCheck()

        If Not ControlFocus Then

            Dim r As Rectangle = New Rectangle(LabelWidth, 0, TextWidth, DefaultHeight + 4)
            Dim g As Graphics = Me.CreateGraphics
            'Dim wdfont As New Font("Wingdings 2", Me.Font.Size)
            g.FillRectangle(Brushes.LemonChiffon, LabelWidth, 0, 12, 12)
            If _Value Then
                g.DrawLine(New Pen(Color.Black, 1.5), New Point(LabelWidth + 1.5, 5.5), New Point(LabelWidth + 4, 9))
                g.DrawLine(New Pen(Color.Black, 1.5), New Point(LabelWidth + 4, 9), New Point(LabelWidth + 10, 3))
            End If
            g.DrawRectangle(Pens.Black, LabelWidth, 0, 12, 12)

        End If

    End Sub

    Private Sub WriteDescription()
        Dim r As Rectangle = New Rectangle(LabelWidth + TextWidth, 0, DescriptionWidth, DefaultHeight + 4)

        Dim g As Graphics = Me.CreateGraphics
        g.FillRectangle(Brushes.Transparent, r)

        Dim flags As TextFormatFlags = TextFormatFlags.EndEllipsis
        TextRenderer.DrawText(g, DescriptionText, Me.Font, r, Me.ForeColor, flags)

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
                FieldText = New TextBoxCT2
                FieldText.Name = $"{Name}_FieldText"
                FieldText.Location = New Point(LabelWidth, 0)
                FieldText.Size = New Size(TextWidth + 1, DefaultHeight)
                FieldText.Text = _Value
                Me.Controls.Add(FieldText)
                FieldText.SetFocus()

            Case EditTypes.Check
                FieldCheck = New CheckBoxCT2
                FieldCheck.Name = $"{Name}_FieldText"
                FieldCheck.Location = New Point(LabelWidth, 0)
                FieldCheck.Size = New Size(TextWidth + 1, DefaultHeight)
                FieldCheck.Checked = _Value
                Me.Controls.Add(FieldCheck)
                'FieldCheck.SetFocus()

        End Select

        Debug.Print($"{Name} enter")
        ControlFocus = True
    End Sub


    Private Sub Me_Leave(sender As Object, e As EventArgs) Handles Me.Leave

        Select Case _EditType
            Case EditTypes.Text
                Me.Controls.Remove(FieldText)
            Case EditTypes.Check
                Me.Controls.Remove(FieldCheck)
        End Select


        ControlFocus = False

        Debug.Print($"{Name} left")
    End Sub

    Private Sub CT2Input_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Controls.Remove(Label1)
        Me.Controls.Remove(Label2)
        Me.Controls.Remove(Label3)
    End Sub




End Class

Public Class CT2Card

    Public DataElement As Object




End Class
