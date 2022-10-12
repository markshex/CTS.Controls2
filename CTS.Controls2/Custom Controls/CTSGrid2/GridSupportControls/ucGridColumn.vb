Public Class ucGridColumn

    Public ID As Integer
    Public VersionID As Integer
    Public gc As GridColumn

    Sub Loaddata(id As Integer, versionid As Integer, GridColumn As GridColumn)
        Me.ID = id
        Me.VersionID = versionid
        gc = GridColumn

        inpName.Value = gc.Name
        inpField.Value = gc.Field
        inpTable.Value = gc.File
        inpText.Value = gc.Text
        inpHeaderText.Value = gc.HeaderText
        inpSeq.Value = gc.Seq
        inpSortSeq.Value = gc.SortSeq
        inpSortOrder.Value = gc.SortOrder
        inpWidth.Value = gc.Width
        inpFunction.Value = gc.SQLFunction
        CT2Input1.Value = Now

        inpRestrict.Value = gc.Restrict
        inpRestrict.EditType = CT2Input.EditTypes.Check

        inpHidden.Value = gc.Hidden
        inpHidden.EditType = CT2Input.EditTypes.Check

        inpColumnType.Value = gc.ColumnType

        Me.Refresh()

    End Sub

    Sub GetValues()
        gc.Name = inpName.Value
        gc.Field = inpField.Value
        gc.File = inpTable.Value
        gc.Text = inpText.Value
        gc.HeaderText = inpHeaderText.Value
        gc.SortSeq = inpSortOrder.Value
        gc.SortOrder = inpSortOrder.Value
        gc.Width = inpWidth.Value
        gc.SQLFunction = inpFunction.Value
        gc.Restrict = inpRestrict.Value
        gc.Hidden = inpHidden.Value
        gc.ColumnType = inpColumnType.Value
    End Sub

    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
        GetValues()
    End Sub

    Private Sub ToolStripButton2_Click(sender As Object, e As EventArgs) Handles ToolStripButton2.Click
        GetValues()

        Dim gh As New Grid2Handler
        gh.UpdateGridColumn(ID, VersionID, gc)
    End Sub
End Class
