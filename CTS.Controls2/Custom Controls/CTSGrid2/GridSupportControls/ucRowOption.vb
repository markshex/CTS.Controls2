Public Class ucRowOption

    Public RO As RowOption

    Public Sub LoadModel(RowOption As RowOption)

        RO = RowOption

        nudSeq.Value = RO.Sequence
        txtName.Text = RO.Name
        txtText.Text = RO.Text

        cbDefault.Checked = RO.Default
        cbEnabled.Checked = RO.Enable
        cbVisible.Checked = RO.Visible

        Select Case RO.Display
            Case DisplayMode.Any
                rbAny.Checked = True
            Case DisplayMode.Display
                rbDisplay.Checked = True
            Case DisplayMode.Update
                rbUpdate.Checked = True
        End Select

    End Sub


    Public Sub SaveModel()

        RO.Sequence = nudSeq.Value
        RO.Name = txtName.Text
        RO.Text = txtText.Text

        RO.Default = cbDefault.Checked
        RO.Enable = cbEnabled.Checked
        RO.Visible = cbVisible.Checked

    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnApply.Click
        SaveModel()
        Me.FindForm.Close()
    End Sub

    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click

    End Sub
End Class
