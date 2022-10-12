Imports System.ComponentModel

Public Class ColorEntry

    Private _Value As Color = Color.Empty

    <Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)>
    <Category("Appearance")>
    <Description("Selected Color.")>
    Public Shadows Property value As Color
        Set(value As Color)
            _Value = value
            ColorBox.BackColor = value
            TextBox1.Text = value.Name
        End Set
        Get
            Return _Value
        End Get
    End Property

    <Browsable(True)>
    <Category("Appearance")>
    <Description("The text associated with the control.")>
    Public Shadows Property Text As String
        Set(value As String)
            TextBox1.Text = value
        End Set
        Get
            Return TextBox1.Text
        End Get
    End Property

    Private WithEvents ctsp As PopupCT2
    Private WithEvents cs As ColorSelector


    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Text = String.Empty
        TextBox1.Text = String.Empty
    End Sub

    Private Sub setcontrolsize()
        'Me.MinimumSize = New Size(PictureBox1.Width + 10, TextBox1.Height + Me.Padding.Top + Me.Padding.Bottom)
        'Me.MaximumSize = New Size(0, TextBox1.Height + Me.Padding.Top + Me.Padding.Bottom)

        Me.Refresh()
    End Sub

    Private Sub TextBox1_SizeChanged(sender As Object, e As EventArgs) Handles TextBox1.SizeChanged
        ' setcontrolsize()
    End Sub

    Private Sub PictureBox1_MouseEnter(sender As Object, e As EventArgs) Handles pbPulldown.MouseEnter
        pbPulldown.BackColor = SystemColors.GradientActiveCaption
    End Sub

    Private Sub PictureBox1_MouseLeave(sender As Object, e As EventArgs) Handles pbPulldown.MouseLeave
        pbPulldown.BackColor = TextBox1.BackColor
    End Sub

    Private Sub ColorEntry_FontChanged(sender As Object, e As EventArgs) Handles MyBase.FontChanged
        setcontrolsize()
    End Sub

    Private Sub TextBox1_Validating(sender As Object, e As CancelEventArgs) Handles TextBox1.Validating

        Dim c = Color.FromName(TextBox1.Text)
        If c <> Color.Empty Then
            value = c
        End If

    End Sub

    Private Sub pbPulldown_Click(sender As Object, e As EventArgs) Handles pbPulldown.Click

        cs = New ColorSelector
        ctsp = New PopupCT2(cs)
        'ctsp.AllowDragging = True
        'ctsp.ShowCloseButton = True
        ctsp.Show(Me)

    End Sub

    Sub ColorSelector_Done() Handles cs.Done
        value = cs.value
        ctsp.Close()
    End Sub



End Class
