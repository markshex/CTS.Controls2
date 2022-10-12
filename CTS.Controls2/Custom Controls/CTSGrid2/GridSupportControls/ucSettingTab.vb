Public Class ucSettingTab

    Private gUserGrid As CT2Grid
    Property UserGrid() As CT2Grid
        Get
            Return gUserGrid
        End Get
        Set(value As CT2Grid)
            gUserGrid = value
        End Set
    End Property

    Public Sub New(UG As CT2Grid)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        gUserGrid = UG
    End Sub

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        gUserGrid = Nothing
    End Sub


    Public Sub ShowDialog()
        Dim ctsd As New CT2Dialog
        ctsd.Title = "Grid Settings"
        ctsd.Control = Me
        ctsd.AllowResize = True
        ctsd.ShowDialog()
    End Sub

    Private Sub ucSettingTab_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If UserGrid IsNot Nothing Then

            UcGridColumns1.LoadData(UserGrid)

            If UserGrid.G IsNot Nothing Then
                ucSQLFormatting1.SelectClause = UserGrid.G.Columns.Clause
                ucSQLFormatting1.FromClause = UserGrid.G.FromClause
                ucSQLFormatting1.InternalWhere = UserGrid.InternalWhere
                ucSQLFormatting1.WhereClause = UserGrid.G.FilterClause
                ucSQLFormatting1.GroupByClause = UserGrid.G.GroupByClause
                ucSQLFormatting1.FormatSQLData()
            End If

        End If
    End Sub

End Class
