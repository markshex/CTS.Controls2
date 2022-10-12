Public Class ImageSelector

    Private WithEvents lbImages As ListBox = New ListBox()
    Private SelectedIdx As Integer = -1
    Public Event Done()

    Private _Value As Image = Nothing
    Public Shadows Property value As Image
        Set(value As Image)
            _Value = value
        End Set
        Get
            Return _Value
        End Get
    End Property

    Class ImageItem
        Property Name As String
        Property image As Image
        Overrides Function ToString() As String
            Return Name
        End Function
    End Class

    Private Sub Selector_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InitializeImageList(lbImages)
        Controls.Add(lbImages)
        lbImages.Dock = DockStyle.Fill
        lbImages.Name = "lbImages"
    End Sub


    Private Sub InitializeImageList(ListBox As ListBox)

        Dim ii1 As New ImageItem
        ii1.Name = "Add"
        ii1.image = My.Resources.Add
        ListBox.Items.Add(ii1)

        Dim ii2 As New ImageItem
        ii2.Name = "Accept"
        ii2.image = My.Resources.Accept
        ListBox.Items.Add(ii2)

        Dim ii3 As New ImageItem
        ii3.Name = "Delete"
        ii3.image = My.Resources.Delete
        ListBox.Items.Add(ii3)


        ListBox.Location = New System.Drawing.Point(81, 69)
        ListBox.Size = New System.Drawing.Size(120, 95)
        ListBox.DrawMode = DrawMode.OwnerDrawVariable
        AddHandler ListBox.DrawItem, (AddressOf ListBox_DrawItem)
        AddHandler ListBox.MeasureItem, (AddressOf ListBox_MeasureItem)

    End Sub


    Private Sub ListBox_DrawItem(ByVal sender As Object, ByVal e As System.Windows.Forms.DrawItemEventArgs)

        Dim listbox As ListBox = sender
        Dim ii As ImageItem = listbox.Items(e.Index)


        e.DrawBackground()

        Dim forecolor As Color
        If listbox.SelectedIndex = e.Index Then
            forecolor = Color.White
        Else
            forecolor = Color.Black
        End If

        e.Graphics.DrawImage(ii.image, New Point(1, e.Bounds.Y + 2))
        TextRenderer.DrawText(e.Graphics, ii.Name, e.Font, New Rectangle(28, e.Bounds.Y, e.Bounds.Width - 28, e.Bounds.Height), forecolor, TextFormatFlags.Left)
        e.DrawFocusRectangle()

    End Sub

    Private Sub ListBox_MeasureItem(sender As Object, e As MeasureItemEventArgs)
        Dim listbox As ListBox = sender
        Dim ii As ImageItem = listbox.Items(e.Index)
        e.ItemHeight = ii.image.Height
    End Sub


    Private Sub lbImages_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lbImages.SelectedIndexChanged
        If lbImages.SelectedIndex >= 0 Then
            ' _Value = Color.FromName(lbImages.Items(lbImages.SelectedIndex).ToString)
        End If
    End Sub

    Private Sub DoubleClickWebItem(sender As Object, e As EventArgs) Handles lbImages.DoubleClick
        RaiseEvent Done()
    End Sub


End Class

