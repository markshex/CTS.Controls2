Partial Class DataGridViewCT2
    Inherits System.Windows.Forms.DataGridView

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        components = New System.ComponentModel.Container()
    End Sub

    <System.ComponentModel.Category("_Custom Properties")>
    Private Suspended As Boolean
    Property IsSuspended() As Boolean
        Get
            Return Suspended
        End Get
        Set(ByVal value As Boolean)
            Suspended = value
        End Set
    End Property

    <System.ComponentModel.Category("_Custom Properties")>
    Private TabEdit As Boolean
    Property TabToEditCell() As Boolean
        Get
            Return TabEdit
        End Get
        Set(ByVal value As Boolean)
            TabEdit = value
        End Set
    End Property

    Protected Overrides Function ProcessDialogKey(keyData As System.Windows.Forms.Keys) As Boolean

        Try

            'If MyBase.RowCount > 0 Then
            'If MyBase.CurrentCell IsNot Nothing And Not MyBase.CurrentCell.IsInEditMode Then
            'Dim c As String = ConvertKeydatatoChar(keyData)
            'If Not String.IsNullOrEmpty(c) Then
            'Debug.Print("key=" & c)
            'strPosition = strPosition + c
            'PositionTextChanged(strPosition)
            'End If
            'End If
            'End If

            If keyData = Keys.Escape Then
                Debug.Print("ESCAPE")
                Dim EscapeReset As Boolean = False
                If Not IsNothing(MyBase.CurrentCell) Then
                    If MyBase.IsCurrentCellDirty Then
                        MyBase.CurrentCell.ErrorText = String.Empty
                        EscapeReset = True
                    End If
                End If

                If Not EscapeReset Then
                    If Not IsNothing(MyBase.CurrentRow) Then
                        If MyBase.IsCurrentRowDirty Then
                            MyBase.CurrentRow.ErrorText = String.Empty
                            ClearCellErrorsByRow(MyBase.CurrentRow)
                        End If
                    End If
                End If
            End If


            If keyData = Keys.Tab And TabEdit Then
                If Not IsNothing(MyBase.CurrentCell) Then
                    GotoNextEditableCell()
                End If
                Return True
            End If

            'Handle Shift - Tab
            If keyData = Keys.Tab + Keys.Shift And TabEdit Then
                If Not IsNothing(MyBase.CurrentCell) Then
                    GotoPreviousEditableCell()
                End If
                Return True
            End If

        Catch ex As Exception
            Return True
        End Try

        Return MyBase.ProcessDialogKey(keyData)
    End Function

    Protected Overrides Function ProcessDataGridViewKey(e As System.Windows.Forms.KeyEventArgs) As Boolean
        Try
            If e.KeyData = Keys.Tab And e.Shift = False And TabEdit Then

                If Not IsNothing(MyBase.CurrentCell) Then
                    GotoNextEditableCell()
                End If

                Return True
            End If

            If e.KeyData = 65545 And e.Shift = True And TabEdit Then
                If Not IsNothing(MyBase.CurrentCell) Then
                    GotoPreviousEditableCell()
                End If

                Return True
            End If

        Catch ex As Exception

        End Try

        Return MyBase.ProcessDataGridViewKey(e)
    End Function

    Private Sub GotoNextEditableCell()
        Try
            Dim nextColumn As DataGridViewColumn
            nextColumn = MyBase.Columns.GetNextColumn(MyBase.Columns(MyBase.CurrentCell.ColumnIndex), DataGridViewElementStates.Visible, DataGridViewElementStates.ReadOnly)

            If Not IsNothing(nextColumn) Then
                MyBase.CurrentCell = MyBase.Rows(MyBase.CurrentCell.RowIndex).Cells(nextColumn.Index)
            Else
                nextColumn = MyBase.Columns.GetFirstColumn(DataGridViewElementStates.Visible, DataGridViewElementStates.ReadOnly)
                If Not IsNothing(nextColumn) Then
                    'if we are here then we need to goto the next row (if bottom goto first row) 
                    If (MyBase.CurrentCell.RowIndex + 1) = MyBase.Rows.Count Then
                        MyBase.CurrentCell = MyBase.Rows(0).Cells(nextColumn.Index)
                    Else
                        MyBase.CurrentCell = MyBase.Rows(MyBase.CurrentCell.RowIndex + 1).Cells(nextColumn.Index)
                    End If
                End If
            End If

        Catch ex As Exception
            Debug.Print(ex.Message)
        End Try
    End Sub

    Private Sub GotoPreviousEditableCell()
        Try
            Dim PreviousColumn As DataGridViewColumn
            PreviousColumn = MyBase.Columns.GetPreviousColumn(MyBase.Columns(MyBase.CurrentCell.ColumnIndex), DataGridViewElementStates.Visible, DataGridViewElementStates.ReadOnly)

            If Not IsNothing(PreviousColumn) Then
                MyBase.CurrentCell = MyBase.Rows(MyBase.CurrentCell.RowIndex).Cells(PreviousColumn.Index)
            Else
                PreviousColumn = MyBase.Columns.GetLastColumn(DataGridViewElementStates.Visible, DataGridViewElementStates.ReadOnly)
                If Not IsNothing(PreviousColumn) Then
                    'if we are here then we need to goto the prior row (if top goto last row) 
                    If (MyBase.CurrentCell.RowIndex) = 0 Then
                        MyBase.CurrentCell = MyBase.Rows(MyBase.RowCount - 1).Cells(PreviousColumn.Index)
                    Else
                        MyBase.CurrentCell = MyBase.Rows(MyBase.CurrentCell.RowIndex - 1).Cells(PreviousColumn.Index)
                    End If
                End If
            End If

        Catch ex As Exception
            Debug.Print(ex.Message)
        End Try
    End Sub

End Class
