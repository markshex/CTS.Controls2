Public Class ColorSelector

    Private WithEvents SysColors As ListBox = New ListBox()
    Private WithEvents WebColors As ListBox = New ListBox()
    Private SelectedIdx As Integer = -1

    Public Event Done()

    Private _Value As Color = Color.Empty
    Public Shadows Property value As Color
        Set(value As Color)
            _Value = value
        End Set
        Get
            Return _Value
        End Get
    End Property


    Class ColorItem
        Property Name As String
        Property Color As Color
        Overrides Function ToString() As String
            Return Name
        End Function
    End Class

    Private Sub NamedColorDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        InitializeSysColorList(SysColors)
        tpSystemColors.Controls.Add(SysColors)
        SysColors.Dock = DockStyle.Fill
        SysColors.Name = "SysColors"

        InitializeWebColorList(WebColors)
        tpWebColors.Controls.Add(WebColors)
        WebColors.Dock = DockStyle.Fill
        WebColors.Name = "WebColors"
    End Sub

    Private Sub InitializeSysColorList(ListBox As ListBox)

        For Each color_enum In [Enum].GetValues(GetType(KnownColor))
            Dim c = Color.FromName(color_enum.ToString)
            If c.IsSystemColor Then
                Dim ci As New ColorItem
                ci.name = c.Name
                ci.Color = c
                Dim i = ListBox.Items.Add(ci)
            End If
        Next

        ListBox.Location = New System.Drawing.Point(81, 69)
        ListBox.Size = New System.Drawing.Size(120, 95)
        ListBox.DrawMode = DrawMode.OwnerDrawVariable
        AddHandler ListBox.DrawItem, (AddressOf ListBox_DrawItem)
        AddHandler ListBox.MeasureItem, (AddressOf ListBox_MeasureItem)

    End Sub

    Private Sub InitializeWebColorList(ListBox As ListBox)

        For Each color_enum In [Enum].GetValues(GetType(KnownColor))
            Dim c = Color.FromName(color_enum.ToString)
            If Not c.IsSystemColor Then
                Dim i = ListBox.Items.Add(color_enum.ToString)
            End If
        Next

        ListBox.Location = New System.Drawing.Point(81, 69)
        ListBox.Size = New System.Drawing.Size(120, 95)
        ListBox.DrawMode = DrawMode.OwnerDrawVariable
        AddHandler ListBox.DrawItem, (AddressOf ListBox_DrawItem)
        AddHandler ListBox.MeasureItem, (AddressOf ListBox_MeasureItem)

    End Sub


    Private Sub ListBox_DrawItem(ByVal sender As Object, ByVal e As System.Windows.Forms.DrawItemEventArgs)

        Dim listbox As ListBox = sender

        e.DrawBackground()

        Dim forecolor As Color
        If listbox.SelectedIndex = e.Index Then
            forecolor = Color.White
        Else
            forecolor = Color.Black
        End If

        Dim c = Color.FromName(listbox.Items(e.Index).ToString)

        Dim Colorbrush As Brush = New Drawing.SolidBrush(c)

        e.Graphics.FillRectangle(Colorbrush, New Rectangle(1, e.Bounds.Y + 2, 25, e.Bounds.Height - 4))
        e.Graphics.DrawRectangle(New Pen(Brushes.Black), New Rectangle(1, e.Bounds.Y + 2, 25, e.Bounds.Height - 4))
        TextRenderer.DrawText(e.Graphics, listbox.Items(e.Index).ToString, e.Font, New Rectangle(28, e.Bounds.Y, e.Bounds.Width - 28, e.Bounds.Height), forecolor, TextFormatFlags.Left)
        e.DrawFocusRectangle()

    End Sub

    Private Sub ListBox_MeasureItem(sender As Object, e As MeasureItemEventArgs)
        e.ItemHeight = 16
    End Sub


    Private Sub SelectedWebItem(sender As Object, e As EventArgs) Handles WebColors.SelectedIndexChanged
        If WebColors.SelectedIndex >= 0 Then
            _Value = Color.FromName(WebColors.Items(WebColors.SelectedIndex).ToString)
        End If
    End Sub

    Private Sub SelectedSystemItem(sender As Object, e As EventArgs) Handles SysColors.SelectedIndexChanged
        If SysColors.SelectedIndex >= 0 Then

            If SelectedIdx <> SysColors.SelectedIndex Then
                If SelectedIdx >= 0 Then
                    SysColors.Items(SelectedIdx).name = SysColors.Items(SelectedIdx).name
                End If
                SelectedIdx = SysColors.SelectedIndex
            End If

            _Value = Color.FromName(SysColors.Items(SysColors.SelectedIndex).ToString)
        End If
    End Sub

    Private Sub DoubleClickWebItem(sender As Object, e As EventArgs) Handles WebColors.DoubleClick
        RaiseEvent Done()
    End Sub
    Private Sub DoubleClickSysItem(sender As Object, e As EventArgs) Handles SysColors.DoubleClick
        RaiseEvent Done()
    End Sub

End Class

