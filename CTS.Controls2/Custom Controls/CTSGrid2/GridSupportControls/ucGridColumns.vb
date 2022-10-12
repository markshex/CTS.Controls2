Imports System.Text
Imports System.Collections.Generic
Imports System.ComponentModel

Public Class ucGridColumns
    Private dtColumns As New DataTable
    Private IsDirty As Boolean = False

    Private gUserGrid As CT2Grid
    Property UserGrid() As CT2Grid
        Get
            Return gUserGrid
        End Get
        Set(value As CT2Grid)
            gUserGrid = value
        End Set
    End Property

    Private Sub GridColumn_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    End Sub


    Public Sub LoadData(grid As CT2Grid)
        Dim N As String
        Dim F As String
        Dim Seq As Integer
        Dim v As Boolean
        Dim Size As String = String.Empty

        If grid IsNot Nothing Then
            gUserGrid = grid

            dtColumns.Columns.Add("CVisible", GetType(System.Boolean))
            dtColumns.Columns.Add("CName", GetType(System.String))
            dtColumns.Columns.Add("CField", GetType(System.String))
            dtColumns.Columns.Add("CSeq", GetType(System.Int32))
            dtColumns.Columns.Add("CPType", GetType(System.String))
            dtColumns.Columns.Add("CSize", GetType(System.String))
            dtColumns.Columns.Add("CRestrict", GetType(System.String))
            dtColumns.PrimaryKey = {dtColumns.Columns("CField")}

            If UserGrid.G IsNot Nothing Then
                For Each col In UserGrid.G.Columns

                    Dim dgvc As DataGridViewColumn
                    If IsDeveloper OrElse Not col.Restrict Then
                        dgvc = UserGrid.dgvCTS.Columns(col.Field)
                        If dgvc IsNot Nothing Then
                            N = Trim(dgvc.HeaderText)
                            F = Trim(dgvc.DataPropertyName)
                            Seq = dgvc.DisplayIndex
                            v = dgvc.Visible

                            Dim strType As String = String.Empty
                            Select Case col.ProviderTypeCode
                                Case "4", "5"
                                    strType = "Numeric"
                                    If col.Scale > 0 Then
                                        Size = col.Size & "." & col.Scale
                                    Else
                                        Size = col.Size
                                    End If
                                Case "6"
                                    strType = "Char"
                                    Size = col.Size
                                Case "12"
                                    strType = "Date"
                                    Size = col.Size
                                Case "13"
                                    strType = "Time"
                                    Size = col.Size
                                Case "14"
                                    strType = "Date/Time"
                                    Size = col.Size
                            End Select

                            dtColumns.Rows.Add({v, N, F, Seq, strType, Size, col.Restrict})
                        End If
                    End If
                Next

            End If

            dgvColumns.AutoGenerateColumns = False
            dgvColumns.DataSource = dtColumns
            dgvColumns.Sort(dgvColumns.Columns("CSEQ"), ListSortDirection.Ascending)
            dgvColumns.Columns("CVISIBLE").HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgvColumns.Columns("CSIZE").HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
            For Each c As DataGridViewColumn In dgvColumns.Columns
                c.SortMode = DataGridViewColumnSortMode.NotSortable
            Next

            IsDirty = False
            FormStateChange()

        End If

    End Sub

    Private Sub btnUp_Click(sender As Object, e As EventArgs) Handles btnUp.Click

        Dim SaveValue As Integer
        Dim rIdx As Integer

        If dgvColumns.CurrentRow.Index > 0 Then
            rIdx = dgvColumns.CurrentRow.Index
            Dim drvFrom As DataRowView = dgvColumns.Rows(rIdx).DataBoundItem
            Dim drvTo As DataRowView = dgvColumns.Rows(rIdx - 1).DataBoundItem

            SaveValue = drvTo("CSEQ")
            drvTo("CSEQ") = drvFrom("CSEQ")
            drvFrom("CSEQ") = SaveValue

            dgvColumns.CurrentCell = dgvColumns.Rows(rIdx - 1).Cells(0)
            dgvColumns.Rows(rIdx).DefaultCellStyle.ForeColor = IIf(dgvColumns.Rows(rIdx).Cells("CRestrict").Value = "Y", Color.Blue, Color.Black)
            dgvColumns.Rows(rIdx - 1).DefaultCellStyle.ForeColor = IIf(dgvColumns.Rows(rIdx - 1).Cells("CRestrict").Value = "Y", Color.Blue, Color.Black)
        End If
        IsDirty = True
        FormStateChange()
    End Sub

    Private Sub btnDown_Click(sender As Object, e As EventArgs) Handles btnDown.Click
        Dim SaveValue As Integer
        Dim rIdx As Integer

        If dgvColumns.CurrentRow.Index < dgvColumns.Rows.Count - 1 Then
            rIdx = dgvColumns.CurrentRow.Index

            Dim drvFrom As DataRowView = dgvColumns.Rows(rIdx).DataBoundItem
            Dim drvTo As DataRowView = dgvColumns.Rows(rIdx + 1).DataBoundItem

            SaveValue = drvTo("CSEQ")
            drvTo("CSEQ") = drvFrom("CSEQ")
            drvFrom("CSEQ") = SaveValue

            dgvColumns.CurrentCell = dgvColumns.Rows(rIdx + 1).Cells(0)
            dgvColumns.Rows(rIdx).DefaultCellStyle.ForeColor = IIf(dgvColumns.Rows(rIdx).Cells("CRestrict").Value = "Y", Color.Blue, Color.Black)
            dgvColumns.Rows(rIdx + 1).DefaultCellStyle.ForeColor = IIf(dgvColumns.Rows(rIdx + 1).Cells("CRestrict").Value = "Y", Color.Blue, Color.Black)
        End If
        IsDirty = True
        FormStateChange()

    End Sub

    Private Sub btnSelectAll_Click(sender As Object, e As EventArgs) Handles btnSelectAll.Click
        For Each dgvr As DataGridViewRow In dgvColumns.Rows
            dgvr.Cells("CVisible").Value = True
        Next
        dgvColumns.EndEdit()
        IsDirty = True
        FormStateChange()

    End Sub

    Private Sub tsbApply_Click_1(sender As Object, e As EventArgs) Handles tsbApply.Click
        Dim NewSeq As Integer
        Dim NewHidden As String
        Dim CurField As String
        Dim strSQL As String

        dgvColumns.EndEdit()

        For Each dr As DataRow In dtColumns.Rows
            NewSeq = dr("CSEQ")
            CurField = dr("CField")
            If dr("CVisible") = True Then
                NewHidden = "N"
            Else
                NewHidden = "Y"
            End If
            strSQL = String.Format("Update DNPF2511 set GPSEQ = {0}, GPHIDDEN = '{1}' Where GPUSER = '{2}' and GPNAME = '{3}' and GPFIELD = '{4}'",
                                   NewSeq, NewHidden, Environment.UserName.ToUpper, Trim(UserGrid.GridName), CurField)
            C2App.Data.ExecuteNonQuery(strSQL)

            UserGrid.dgvCTS.Columns(CurField).DisplayIndex = 0
        Next


        For Each x As DataGridViewRow In dgvColumns.Rows
            CurField = x.Cells("CField").Value
            UserGrid.dgvCTS.Columns(CurField).DisplayIndex = x.Index
            If x.Cells("CVisible").Value = True Then
                UserGrid.dgvCTS.Columns(CurField).Visible = True
            Else
                UserGrid.dgvCTS.Columns(CurField).Visible = False
            End If
            If UserGrid.ShowTotals Then
                If x.Cells("CVisible").Value = True Then
                    UserGrid.dgvTotals.Columns(CurField).Visible = True
                Else
                    UserGrid.dgvTotals.Columns(CurField).Visible = False
                End If
            End If
        Next

        IsDirty = False
        FormStateChange()

    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvColumns.CellContentClick

        If dgvColumns.Columns(e.ColumnIndex).DataPropertyName = "CVisible" Then
            'dgvColumns.Sort(dgvColumns.Columns("CSEQ"), ListSortDirection.Ascending)
            'dgvColumns.Refresh()
            'SetFocusTo(drv("CName"))
            IsDirty = True
            FormStateChange()

        End If

    End Sub

    Private Sub DataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvColumns.CellDoubleClick
        If e.RowIndex >= 0 AndAlso e.ColumnIndex >= 0 Then
            If dgvColumns.Rows(e.RowIndex).Cells("CVisible").Value = True Then
                dgvColumns.Rows(e.RowIndex).Cells("CVisible").Value = False
            Else
                dgvColumns.Rows(e.RowIndex).Cells("CVisible").Value = True
            End If
            IsDirty = True
            FormStateChange()
        End If
    End Sub

    Private Sub SetFocusTo(ByVal ColName As String)
        For Each dgvr As DataGridViewRow In dgvColumns.Rows
            If dgvr.Cells("CName").Value = ColName Then
                dgvColumns.CurrentCell = dgvr.Cells("CName")
                Exit For
            End If
        Next
    End Sub

    Private Sub SetRetrictedColor()
        For Each r As DataGridViewRow In dgvColumns.Rows
            If r.Cells("CRestrict").Value = "Y" Then
                r.DefaultCellStyle.ForeColor = Color.Blue
            Else
                r.DefaultCellStyle.ForeColor = Color.Black
            End If
        Next
        dgvColumns.Refresh()
    End Sub

    Private Sub FormStateChange()
        If IsDirty Then
            tsbApply.Visible = True
        Else
            tsbApply.Visible = False
        End If
    End Sub

    Private Sub btnDefaults_Click(sender As Object, e As EventArgs) Handles btnDefaults.Click
        Dim strSQL As String = String.Format("Select * From DNPF2510 where gcname = '{0}'", Trim(UserGrid.GridName))
        Dim dt As DataTable = C2App.Data.GetTable(strSQL)

        dgvColumns.EndEdit()

        For Each dr2510 As DataRow In dt.Rows

            Dim drColumns As DataRow = dtColumns.Rows.Find(dr2510("GCFIELD"))
            If drColumns IsNot Nothing Then
                drColumns("CSEQ") = dr2510("GCSEQ")
                If dr2510("GCHIDDEN") = "Y" Then
                    drColumns("CVisible") = False
                Else
                    drColumns("CVisible") = True
                End If
            End If
        Next

        SetRetrictedColor()
        dgvColumns.EndEdit()

        IsDirty = True
        FormStateChange()

    End Sub

End Class