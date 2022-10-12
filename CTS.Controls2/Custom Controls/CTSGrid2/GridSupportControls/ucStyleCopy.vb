Public Class ucStyleCopy

    Private G As Grid
    Public DS As DynamicStyle

    Sub New(ByVal Grid As Grid, DynamicStyle As DynamicStyle)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        G = Grid
        DS = DynamicStyle
    End Sub

    Sub New(ByVal Grid As Grid)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        G = Grid
    End Sub

    Private Sub ucStyles_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadModel()
    End Sub


    Public Sub LoadModel()

        txtName.Text = DS.Name
        txtTestValue.Text = DS.FieldName
        ceBackground.value = DS.Style.BackColor
        ceForeground.value = DS.Style.ForeColor

        For Each gc In G.Columns
            cbTestColumn.Items.Add(gc.Field)
        Next

        For Each op In [Enum].GetValues(GetType(Filtering.OperatorName))
            cbOperator.Items.Add(op)
        Next

        For Each gc In G.Columns
            cbApplyColumn.Items.Add(gc.Field)
        Next


        Select Case DS.ApplyType
            Case StyleApplyType.Cell
                rbCell.Checked = True
            Case StyleApplyType.Row
                rbRow.Checked = True
        End Select

    End Sub


    Public Sub Mapping()

        Try
            DS.Name = txtName.Text
            DS.FieldName = cbTestColumn.SelectedItem.ToString
            DS.Op = cbOperator.SelectedItem
            DS.Value = txtTestValue.Text

            If rbCell.Checked Then
                DS.ApplyType = StyleApplyType.Cell
                DS.ApplyCellName = cbApplyColumn.SelectedItem.ToString
            Else
                DS.ApplyType = StyleApplyType.Row
                DS.ApplyCellName = String.Empty
            End If

            DS.Style.BackColor = ceBackground.value
            DS.Style.ForeColor = ceForeground.value

            If Not G.DynamicStyles.Contains(DS) Then
                G.DynamicStyles.Add(DS)
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnApply.Click
        Mapping()

        Me.FindForm.Close()
    End Sub

    Private Sub SaveModel()

        ' DS.ApplyCellName
        ' DS.ApplyCellName
        ' DS.ApplyCellName

    End Sub

End Class
