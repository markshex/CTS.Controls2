Public Class ucGridFunction

    Public GF As GridFunction

    Public Sub LoadModel(GridFunction As GridFunction)

        GF = GridFunction

        nudSeq.Value = GF.Sequence
        txtName.Text = GF.Name
        txtText.Text = GF.Text

        If GF.Align = ToolStripItemAlignment.Right Then
            rbRight.Checked = True
        ElseIf GF.Align = ToolStripItemAlignment.left Then
            rbLeft.Checked = True
        End If

        cbDefault.Checked = GF.Default
        cbEnabled.Checked = GF.Enable
        cbVisible.Checked = GF.Visible

        Select Case GF.Display
            Case DisplayMode.Any
                rbAny.Checked = True
            Case DisplayMode.Display
                rbDisplay.Checked = True
            Case DisplayMode.Update
                rbUpdate.Checked = True
        End Select

        For Each Str As FunctionImage In System.Enum.GetValues(GetType(FunctionImage))
            cbImage.Items.Add(Str.ToString)
        Next

    End Sub


    Public Sub SaveModel()

        GF.Sequence = nudSeq.Value
        GF.Name = txtName.Text
        GF.Text = txtText.Text

        If rbRight.Checked Then
            GF.Align = ToolStripItemAlignment.Right
        ElseIf rbLeft.Checked Then
            GF.Align = ToolStripItemAlignment.Left
        End If

        GF.Default = cbDefault.Checked
        GF.Enable = cbEnabled.Checked
        GF.Visible = cbVisible.Checked

        GF.ImageName = [Enum].Parse(GetType(FunctionImage), cbImage.SelectedItem, True)

    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnApply.Click
        SaveModel()
        Me.FindForm.Close()
    End Sub

    Private Sub cbImage_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbImage.SelectedIndexChanged
        If cbImage.SelectedIndex >= 0 Then
            pbImage.Image = My.Resources.ResourceManager.GetObject(cbImage.SelectedItem)
            pbImage.Refresh()
        End If
    End Sub
End Class
