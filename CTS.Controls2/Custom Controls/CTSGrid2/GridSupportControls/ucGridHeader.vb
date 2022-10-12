Public Class ucGridHeader

    Public g As Grid

    Sub LoadData(GridHeader As Grid)
        g = GridHeader

        inpID.Value = g.ID
        inpName.Value = g.Name
        inpTitle.Value = g.Title
        inpFrom.Value = g.FromClause
        inpWhere.Value = g.WhereClause
        inpGroup.Value = g.GroupByClause
        inpDistinct.Value = g.Distinct
        inpDistinct.EditType = CT2Input.EditTypes.Check
        Me.Refresh()

    End Sub

    Sub GetValues()
        g.Name = inpName.Value
        g.ID = inpID.Value
        g.Title = inpTitle.Value
        g.WhereClause = inpWhere.Value
        g.FromClause = inpFrom.Value
        g.GroupByClause = inpGroup.Value
        g.Distinct = inpDistinct.Value
    End Sub

    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        GetValues()
    End Sub

    Private Sub ToolStripButton2_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        Dim gh As New Grid2Handler
        gh.UpdateGridHeader(g)
    End Sub
End Class
