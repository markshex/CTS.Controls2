Imports System
Imports System.Timers
Imports System.Windows.Forms
Imports System.ComponentModel
Imports System.Runtime.InteropServices


<ComVisible(False), DesignTimeVisible(False)>
Public Class DataGridViewCT2
    Inherits DataGridView

    Public IsGridDirty As Boolean

    Private gSuppressEnterKey As Boolean
    <System.ComponentModel.Category("_Custom Properties")>
    <System.ComponentModel.Description("Suppress default grid ENTER functionality.")>
    Public Property SuppressEnterKey() As String
        Get
            Return gSuppressEnterKey
        End Get
        Set(value As String)
            gSuppressEnterKey = value
        End Set
    End Property

    Private SorterPosition As String
    Public StaticPosition As String
    Public CustomSort As String
    Friend SorterCount As Integer = 0
    Friend SorterIndex As Integer = 0
    Friend CustomSorter As ListSorter()
    Friend Structure ListSorter
        Dim Seq As Integer
        Dim Order As String
    End Structure

    Friend CurHeaderIndex As Integer
    Friend CurRowHeaderIndex As Integer
    Friend CurRowIndex As Integer = -1

    Public Sub New()
        InitializeComponent()

        InitPositionTimer()
    End Sub

    Protected Overrides Sub OnColumnAdded(e As DataGridViewColumnEventArgs)
        CustomSorter = New ListSorter(Me.Columns.Count - 1) {}
        Select Case CustomSort
            Case "DATAGRIDVIEW"
                e.Column.SortMode = DataGridViewColumnSortMode.Automatic
            Case "NOSORT"
                e.Column.SortMode = DataGridViewColumnSortMode.NotSortable
            Case "CUSTOM"
                e.Column.SortMode = DataGridViewColumnSortMode.Programmatic
        End Select
        MyBase.OnColumnAdded(e)
    End Sub

    Protected Overrides Sub OnRowHeaderMouseClick(e As DataGridViewCellMouseEventArgs)
        CurRowHeaderIndex = e.RowIndex
        MyBase.OnRowHeaderMouseClick(e)
    End Sub

    Protected Overrides Sub OnColumnHeaderMouseClick(e As DataGridViewCellMouseEventArgs)
        CurHeaderIndex = e.ColumnIndex

        If CustomSort = "NOSORT" Then Exit Sub

        If CustomSort = "DATAGRIDVIEW" Then

            'Column Header Clicked 
            If e.Button = Windows.Forms.MouseButtons.Left Then

                If e.RowIndex = -1 And e.ColumnIndex > -1 Then
                    If Me.SortedColumn IsNot Nothing Then
                        If Me.SortedColumn.Index = e.ColumnIndex Then
                            If Me.SortOrder = SortOrder.Ascending Then
                                Me.Sort(Me.Columns(e.ColumnIndex), ListSortDirection.Descending)
                            Else
                                Me.Sort(Me.Columns(e.ColumnIndex), ListSortDirection.Ascending)
                            End If
                        Else
                            Me.Sort(Me.Columns(e.ColumnIndex), ListSortDirection.Ascending)
                        End If
                    Else
                        Me.Sort(Me.Columns(e.ColumnIndex), ListSortDirection.Ascending)
                    End If
                End If
            End If
        End If

        If CustomSort = "CUSTOM" Then
            'If CustomSort = "MULTI_DV" Then
            If e.Button = Windows.Forms.MouseButtons.Left Then
                CustomSortClick(e)
            End If
        End If

        MyBase.OnColumnHeaderMouseClick(e)
    End Sub

    Protected Overrides Sub OnCellBeginEdit(e As DataGridViewCellCancelEventArgs)
        If Not Suspended Then
            'Debug.Print("DataGridViewCTS: CellBeginEdit row " & e.RowIndex & " col " & e.ColumnIndex)

            Me.Item(e.ColumnIndex, e.RowIndex).Style.Padding = New Padding(0, 3, 0, 2)

            If Me.Item(e.ColumnIndex, e.RowIndex).ErrorText = String.Empty Then
                Me.Item(e.ColumnIndex, e.RowIndex).Style.BackColor = EditFocusBack
            Else
                Me.Item(e.ColumnIndex, e.RowIndex).Style.BackColor = EditErrorBack
            End If

            MyBase.OnCellBeginEdit(e)
        End If
    End Sub

    Protected Overrides Sub OnCellEndEdit(e As DataGridViewCellEventArgs)
        If Not Suspended Then
            'Debug.Print("DataGridViewCTS: CellEndEdit row " & e.RowIndex & " col " & e.ColumnIndex)

            Me.Item(e.ColumnIndex, e.RowIndex).Style.Padding = New Padding(0)

            MyBase.OnCellEndEdit(e)
        End If
    End Sub

    Protected Overrides Sub OnCellErrorTextChanged(e As DataGridViewCellEventArgs)
        If Not Suspended Then
            If Me.Item(e.ColumnIndex, e.RowIndex).ErrorText = String.Empty Then
                InvalidateCell(Me.Item(e.ColumnIndex, e.RowIndex))
            Else
                'Debug.Print("DataGridViewCTS: ERROR TEXT ADDED at " & e.ColumnIndex & "-" & e.RowIndex)
                InvalidateCell(Me.Item(e.ColumnIndex, e.RowIndex))
            End If
            MyBase.OnCellErrorTextChanged(e)
        End If
    End Sub

    Protected Overrides Sub OnRowPostPaint(e As DataGridViewRowPostPaintEventArgs)
        If Not Suspended Then
            Dim ChangesFound As Boolean
            Dim EditState As DataRowState = GetDataRowState(e.RowIndex)
            Dim TempColor As Drawing.Color = SelectedRowBorder

            'If EditState = DataRowState.Modified Then
            If EditState <> DataRowState.Unchanged Then
                TempColor = EditPending
                ChangesFound = True
            End If

            If e.ErrorText <> String.Empty Then
                TempColor = EditErrorBack
            End If

            'Draw row border if currently active row 
            If Me.CurrentRow IsNot Nothing Then
                If Me.CurrentRow.Index = e.RowIndex Then

                    'Override to edit color if row is dirty and no error exist
                    If Me.IsCurrentRowDirty And e.ErrorText = String.Empty Then
                        TempColor = EditPending
                        ChangesFound = True
                    End If

                    ' Calculate the bounds of the row 
                    Dim rowHeaderWidth As Integer = If(Me.RowHeadersVisible, Me.RowHeadersWidth, 0)

                    Dim rowBounds As New Rectangle(rowHeaderWidth, e.RowBounds.Top,
                    Me.Columns.GetColumnsWidth(DataGridViewElementStates.Visible) -
                    Me.HorizontalScrollingOffset + 1, e.RowBounds.Height)

                    ' Paint the border 
                    ControlPaint.DrawBorder(e.Graphics, rowBounds,
                                     TempColor, 0, ButtonBorderStyle.Solid,
                                     TempColor, 2, ButtonBorderStyle.Solid,
                                     TempColor, 2, ButtonBorderStyle.Solid,
                                     TempColor, 2, ButtonBorderStyle.Solid)
                End If
            End If

            'set row header color 
            Me.Rows(e.RowIndex).HeaderCell.Style.BackColor = TempColor

            'if changes found try to set the grid dirty state
            If ChangesFound Then
                SetState(True)
            End If

            MyBase.OnRowPostPaint(e)
        End If
    End Sub

    Protected Overrides Sub OnRowEnter(e As DataGridViewCellEventArgs)
        If Not Suspended Then
            'Debug.Print("RowEnter row " & e.RowIndex & " height=" & Me.Rows(e.RowIndex).Height)

            Me.InvalidateRow(e.RowIndex)

            MyBase.OnRowEnter(e)
        End If
    End Sub

    Protected Overrides Sub OnRowLeave(e As DataGridViewCellEventArgs)
        If Not Suspended Then
            'Debug.Print("RowLeave row " & e.RowIndex & " height=" & Me.Rows(e.RowIndex).Height & " rowtemplate.height=" & Me.RowTemplate.Height)

            If Me.Rows(e.RowIndex).Height <> Me.RowTemplate.Height Then
                Me.Rows(e.RowIndex).Height = Me.RowTemplate.Height
            Else
                Me.InvalidateRow(e.RowIndex)
            End If

            MyBase.OnRowLeave(e)
        End If
    End Sub

    Protected Overrides Sub OnKeyPress(e As KeyPressEventArgs)
        If Not Suspended Then

            If Me.RowCount > 0 Then
                If Char.IsLetter(e.KeyChar) OrElse Char.IsNumber(e.KeyChar) OrElse e.KeyChar = vbBack Then
                    'If Not String.IsNullOrEmpty(e.KeyChar) And e.KeyChar <> vbTab Then
                    'Debug.Print("datagridviewCTS: key=" & e.KeyChar)

                    If e.KeyChar = ControlChars.Back Then
                        If Len(SorterPosition) > 0 Then
                            SorterPosition = Mid(SorterPosition, 1, Len(SorterPosition) - 1)
                        End If
                    Else
                        SorterPosition = SorterPosition + e.KeyChar
                    End If

                    If (Me.CurrentCell IsNot Nothing AndAlso Not Me.CurrentCell.IsInEditMode) _
                    Or (Me.CurrentCell Is Nothing) Then
                        Me.EditMode = DataGridViewEditMode.EditOnF2

                        StaticPosition = SorterPosition
                        SorterPositionTextChanged(SorterPosition)

                        Me.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2
                    End If

                End If
            End If

            MyBase.OnKeyPress(e)
        End If
    End Sub

    Protected Overrides Sub OnCellMouseUp(e As DataGridViewCellMouseEventArgs)
        'CurHeaderIndex = e.ColumnIndex

        MyBase.OnCellMouseUp(e)
    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean
        If keyData = Keys.Enter AndAlso gSuppressEnterKey Then
            Return True
        End If

        Return MyBase.ProcessCmdKey(msg, keyData)
    End Function


#Region "State Changes"
    Public Event StateChanged(ByVal sender As Object, ByVal Dirty As Boolean)

    Friend Sub SetState(ByVal IsDirty As Boolean)

        If IsDirty <> IsGridDirty Then
            IsGridDirty = IsDirty
            RaiseEvent StateChanged(Me, IsGridDirty)
        End If

    End Sub

    Friend Function GetDataRowState(ByVal RowIndex As Integer) As DataRowState
        Try
            Dim thisrow As DataGridViewRow = Me.Rows(RowIndex)
            Dim drv As DataRowView
            If thisrow.IsNewRow Then
                Return DataRowState.Unchanged
            Else
                drv = thisrow.DataBoundItem
                Return drv.Row.RowState
            End If

        Catch
            Return Nothing
        End Try

    End Function

    Friend Sub CheckState()
        Dim Dirty As Boolean

        If Me.CurrentRow IsNot Nothing AndAlso Me.IsCurrentRowDirty Then
            Dirty = True
        Else
            If Me.DataSource IsNot Nothing Then
                Dim dv As DataView = New DataView(Me.DataSource)
                dv.RowStateFilter = DataViewRowState.ModifiedCurrent + DataViewRowState.Added + DataViewRowState.Deleted
                If dv.Count > 0 Then
                    Dirty = True
                Else
                    Dirty = False
                End If
            End If
        End If

        If Dirty <> IsGridDirty Then
            IsGridDirty = Dirty
            RaiseEvent StateChanged(Me, IsGridDirty)
        End If
    End Sub
#End Region

#Region "Multi-Column Sorter"

    'Column header click to sort
    Private Sub CustomSortClick(e As DataGridViewCellMouseEventArgs)
        Dim i As Integer

        Dim keyCtrlHold As Boolean = ((ModifierKeys And Keys.Control) = Keys.Control)
        If keyCtrlHold Then

            'Mouse click with Ctrl pressed
            For i = 0 To Me.Columns.Count - 1

                If e.ColumnIndex = i Then
                    If SorterCount = 0 Then
                        'SorterIndex = i
                    End If
                    If CustomSorter(i).Seq = 0 Then
                        SorterCount += 1
                        CustomSorter(i).Seq = SorterCount
                        CustomSorter(i).Order = "ASC"
                    Else
                        Select Case CustomSorter(i).Order
                            Case "ASC"
                                CustomSorter(i).Order = "DESC"
                            Case Else
                                CustomSorter(i).Order = "ASC"
                        End Select
                    End If

                End If
            Next
        Else
            'Mouse click without Ctrl press
            For i = 0 To Me.Columns.Count - 1
                If e.ColumnIndex = i Then

                    SorterCount = 1
                    'SorterIndex = i
                    CustomSorter(i).Seq = SorterCount

                    Select Case CustomSorter(i).Order
                        Case "ASC"
                            CustomSorter(i).Order = "DESC"
                        Case Else
                            CustomSorter(i).Order = "ASC"
                    End Select

                Else
                    CustomSorter(i).Seq = 0
                    CustomSorter(i).Order = String.Empty
                End If
            Next
        End If

        'Construct a sort string and apply to datasource 
        ApplySorter()

    End Sub

    'Clear the "CustomSorter" array 
    Public Sub ClearSorter()
        SorterCount = 0
        SorterIndex = 0
        If CustomSorter IsNot Nothing Then
            For i = 0 To CustomSorter.GetUpperBound(0) - 1
                CustomSorter(i).Seq = 0
                CustomSorter(i).Order = String.Empty
            Next
        End If
    End Sub

    'Resort the grid based on the "CustomSorter" array specification
    Public Sub ApplySorter()
        If Me.DataSource IsNot Nothing And Me.Rows.Count > 0 Then

            Dim strCustomSort As String = String.Empty
            Dim strFinalSort As String = String.Empty

            'Build Sorting String 
            strCustomSort = String.Empty

            'Put sorting indexes into Temp array
            Dim Temp(SorterCount) As Integer
            For i = 0 To Me.Columns.Count - 1
                If CustomSorter(i).Seq <> 0 Then
                    Temp(CustomSorter(i).Seq - 1) = i
                End If
                If CustomSorter(i).Seq = 1 Then
                    SorterIndex = i
                End If
            Next

            'Build Sort String from temp array
            For i = 0 To SorterCount - 1
                strCustomSort = strCustomSort & Me.Columns(Temp(i)).Name() & " " & CustomSorter(Temp(i)).Order & ","
            Next

            If strCustomSort <> String.Empty Then
                strFinalSort = strCustomSort.Remove(strCustomSort.Length - 1)
                Try
                    Suspended = True
                    Me.DataSource.defaultview.sort = strFinalSort
                    Me.FirstDisplayedScrollingRowIndex = 0
                    Suspended = False
                Catch
                End Try
            End If

            SorterPosition = String.Empty
            RaiseEvent CustomSorted(Me, SorterIndex, CustomSorter(SorterIndex).Seq, CustomSorter(SorterIndex).Order)
            'Debug.Print("DataGridViewCTS: AppySorter-" & strFinalSort)
        End If
    End Sub

    Public Event CustomSorted(ByVal Sender As Object, ByVal ColumnIndex As Integer, ByVal SortSequence As Integer, ByVal SortOrder As String)
#End Region

#Region "Positioning Routines"

    Private Shared tmrPositionReset As New System.Timers.Timer
    Public Event CustomPosition(ByVal Sender As Object, ByVal PositionString As String, ByVal RowIndex As Integer)
    Public Event SorterPositionCleared(ByVal Sender As Object)

    Private Sub SorterPositionTextChanged(position As String)
        If SorterCount > 0 And position <> String.Empty Then
            If Me.Columns(SorterIndex).ReadOnly = True Then

                'restart the reset timer
                tmrPositionReset.Enabled = False
                tmrPositionReset.Enabled = True

                SortedColumnPositioning(position)

            End If
        End If
    End Sub

    Private Sub InitPositionTimer()
        tmrPositionReset.Enabled = False

        ' Set the Interval to 1.5 seconds (1500 milliseconds).
        tmrPositionReset.Interval = 1500

        ' Hook up the Elapsed event for the timer. 
        AddHandler tmrPositionReset.Elapsed, AddressOf OnTimedEvent
    End Sub

    Private Sub OnTimedEvent()
        tmrPositionReset.Enabled = False
        SorterPosition = String.Empty
        'Debug.Print("datagridviewCTS: Dynamic Sort string cleared")
        RaiseEvent SorterPositionCleared(Me)
    End Sub

    Public Sub SortedColumnPositioning(ByVal position As String)
        Dim RowIndex As Integer = -1

        Select Case Me.Columns(SorterIndex).ValueType.Name
            Case "String"
                RowIndex = PositionString(position)

            Case "Decimal", "Integer", "Int16", "Int32", "Int64"
                If IsNumeric(position) Then
                    Dim dec As Decimal = CDec(position)
                    RowIndex = PositionNumeric(dec)
                End If
            Case "DateTime"
                'what to do?
        End Select

        RaiseEvent CustomPosition(Me, position, RowIndex)
    End Sub

    Private Function PositionString(ByVal position As String) As Integer
        position = UCase(position)

        Dim cIdx As Integer = SorterIndex
        Dim rIdx As Integer = 0

        'Use Binary Search Method to get correct position quickly
        'Binary search splits the list in half until desired row is found
        'instead of walking through every row.  Drastically faster.

        Dim CurValue, PrevValue, CurFullValue As String
        Dim Done As Boolean = False
        Dim bIdx As Integer = 0
        Dim eIdx As Integer = Me.RowCount - 1

        Do Until Done
            'Debug.Print($"PositionString: finding middle ->{rIdx}/{eIdx}/{bIdx}<-")

            'Find middle
            rIdx = ((eIdx - bIdx + 1) / 2) + bIdx
            If rIdx > eIdx Then
                rIdx = eIdx
            End If

            If Not IsDBNull(Me.Rows(rIdx).Cells(cIdx).Value) AndAlso Me.Rows(rIdx).Cells(cIdx).Value <> String.Empty Then
                CurValue = UCase(Mid(Me.Rows(rIdx).Cells(cIdx).Value, 1, Len(position)))
                CurFullValue = UCase(Me.Rows(rIdx).Cells(cIdx).Value) + ChrW(255)
            Else
                CurValue = Space(Len(position))
                CurFullValue = Space(Len(position)) + ChrW(255)
            End If

            If rIdx > 0 AndAlso Not IsDBNull(Me.Rows(rIdx - 1).Cells(cIdx).Value) Then
                PrevValue = UCase(Mid(Me.Rows(rIdx - 1).Cells(cIdx).Value, 1, Len(position)))
            Else
                PrevValue = String.Empty
            End If

            'Test middle
            If position = CurFullValue Or bIdx >= eIdx Then
                'Debug.Print("DataGridViewCTS: position to checkpoint 1; " & position & "-" & CurFullValue)
                Me.CurrentCell = Me.Rows(rIdx).Cells(cIdx)
                Me.FirstDisplayedScrollingRowIndex = rIdx
                Return rIdx
                Exit Function
            End If

            'Figure out which half to discard
            Select Case CustomSorter(SorterIndex).Order
                Case "ASC"

                    If position < CurFullValue Then
                        If position = CurValue Then
                            If position <> PrevValue Then
                                'Debug.Print("DataGridViewCTS: first value/partial match -asc; " & position & "-" & CurFullValue)
                                Me.CurrentCell = Me.Rows(rIdx).Cells(cIdx)
                                Me.FirstDisplayedScrollingRowIndex = rIdx
                                Return rIdx
                                Exit Function
                            End If
                        End If
                        eIdx = rIdx - 1
                    Else
                        bIdx = rIdx + 1
                    End If

                Case "DESC"
                    If position = CurValue Then

                        If position < CurFullValue Then
                            If position <> PrevValue Then
                                'Debug.Print("DataGridViewCTS: first value/partial match -dsc; " & position & "-" & CurFullValue)
                                Me.CurrentCell = Me.Rows(rIdx).Cells(cIdx)
                                Me.FirstDisplayedScrollingRowIndex = rIdx
                                Return rIdx
                                Exit Function
                            End If
                            eIdx = rIdx - 1
                        Else
                            bIdx = rIdx + 1
                        End If
                    Else

                        If position > CurFullValue Then
                            eIdx = rIdx - 1
                        Else
                            bIdx = rIdx + 1
                        End If
                    End If

                Case Else
                    Done = True
            End Select

        Loop

        Return -1
    End Function

    Private Function PositionNumeric(ByVal position As Decimal) As Integer

        Dim cIdx As Integer = SorterIndex
        Dim rIdx As Integer = 0

        'Use Binary Search Method to get correct position quickly
        'Binary search splits the list in half until desired row is found
        'instead of walking through every row.  Drastically faster.

        Dim CurValue As Decimal

        Dim Done As Boolean = False
        Dim bIdx As Integer = 0
        Dim eIdx As Integer = Me.RowCount - 1

        Do Until Done
            'Debug.Print($"PositionNumeric:{position} finding middle ->{rIdx}/{eIdx}/{bIdx}<-")

            'Find middle
            rIdx = ((eIdx - bIdx + 1) / 2) + bIdx
            If eIdx < 0 Then
                rIdx = 1
                Return rIdx
            ElseIf rIdx > eIdx Then
                rIdx = eIdx
            End If


            'jump out if null data found
            If rIdx = eIdx AndAlso (Me.Rows(rIdx).Cells(cIdx).Value Is Nothing OrElse IsDBNull(Me.Rows(rIdx).Cells(cIdx).Value)) Then
                Me.CurrentCell = Me.Rows(rIdx).Cells(cIdx)
                Me.FirstDisplayedScrollingRowIndex = rIdx
                Return rIdx
                Exit Function
            End If



            If Me.Rows(rIdx).Cells(cIdx).Value IsNot Nothing AndAlso
            Not IsDBNull(Me.Rows(rIdx).Cells(cIdx).Value) Then

                CurValue = Me.Rows(rIdx).Cells(cIdx).Value

                'Test middle
                If position = CurValue Or bIdx >= eIdx Then
                    'If bIdx >= eIdx Then
                    Me.CurrentCell = Me.Rows(rIdx).Cells(cIdx)
                    Me.FirstDisplayedScrollingRowIndex = rIdx
                    Return rIdx
                    Exit Function
                End If

                'Figure out which half to discard
                Select Case CustomSorter(SorterIndex).Order
                    Case "ASC"
                        If position < CurValue Then
                            eIdx = rIdx - 1
                        Else
                            bIdx = rIdx + 1
                        End If

                    Case "DESC"
                        If position > CurValue Then
                            eIdx = rIdx - 1
                        Else
                            bIdx = rIdx + 1
                        End If

                    Case Else
                        Done = True
                End Select

            Else

                'Figure out which half to discard
                Select Case CustomSorter(SorterIndex).Order
                    Case "ASC"
                        If position < CurValue Then
                            eIdx = rIdx - 1
                        Else
                            bIdx = rIdx + 1
                        End If

                    Case "DESC"
                        If position > CurValue Then
                            eIdx = rIdx - 1
                        Else
                            bIdx = rIdx + 1
                        End If

                    Case Else
                        Done = True
                End Select
            End If
        Loop

        Return -1
    End Function


    Public Sub PositionToFirstEditableCell()
        If MyBase.Rows.Count > 0 And MyBase.Columns.Count > 0 Then

            Dim ColIndex As Integer
            ColIndex = MyBase.Columns.GetNextColumn(MyBase.Columns(0), DataGridViewElementStates.Visible, DataGridViewElementStates.ReadOnly).Index
            MyBase.CurrentCell = MyBase.Rows(0).Cells(ColIndex)

        End If

    End Sub
#End Region

End Class


#Region "Custom Cells & Columns"

'CTS TextBox column
Public Class DataGridViewCellCTS
        Inherits DataGridViewTextBoxCell

        Protected Overrides Sub Paint(
            ByVal graphics As Graphics,
            ByVal clipBounds As Rectangle,
            ByVal cellBounds As Rectangle,
            ByVal rowIndex As Integer,
            ByVal elementState As DataGridViewElementStates,
            ByVal value As Object,
            ByVal formattedValue As Object,
            ByVal errorText As String,
            ByVal cellStyle As DataGridViewCellStyle,
            ByVal advancedBorderStyle As DataGridViewAdvancedBorderStyle,
            ByVal paintParts As DataGridViewPaintParts)

            'Change back color to ERROR
            If errorText <> String.Empty Then
                Me.DataGridView.Item(ColumnIndex, rowIndex).Style.BackColor = EditErrorBack
            End If

            'Change back color to EDITABLE
            If (elementState And DataGridViewElementStates.ReadOnly) Then
            Else
                If errorText = String.Empty Then
                    Me.DataGridView.Item(ColumnIndex, rowIndex).Style.BackColor = EditBack
                End If
            End If

            ' Call the base class method to paint the default cell appearance. 
            MyBase.Paint(graphics, clipBounds, cellBounds, rowIndex, elementState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts)


            ' HOVER - If the mouse pointer is over the current cell, draw a custom border.
            ' Retrieve the client location of the mouse pointer. 
            Dim cursorPosition As Point = Me.DataGridView.PointToClient(Cursor.Position)
            If cellBounds.Contains(cursorPosition) Then
                Dim newRect As New Rectangle(cellBounds.X + 1, cellBounds.Y + 1, cellBounds.Width - 4, cellBounds.Height - 4)
                graphics.DrawRectangle(Pens.DarkGreen, newRect)
            End If

        End Sub

        ' Force the cell to repaint itself when the mouse pointer enters it. 
        Protected Overrides Sub OnMouseEnter(ByVal rowIndex As Integer)
            Me.DataGridView.InvalidateCell(Me)
        End Sub

        ' Force the cell to repaint itself when the mouse pointer leaves it. 
        Protected Overrides Sub OnMouseLeave(ByVal rowIndex As Integer)
            Me.DataGridView.InvalidateCell(Me)
        End Sub

    End Class

    Public Class DataGridViewColumnCTS
        Inherits DataGridViewColumn

        Public Sub New()
            Me.CellTemplate = New DataGridViewCellCTS()
            'Me.SortMode = DataGridViewColumnSortMode.Programmatic
            Me.SortMode = DataGridViewColumnSortMode.NotSortable
        End Sub

        Private gDB2ProviderType As String
        Public Property DB2ProviderType() As String
            Get
                ' Return the value for the property 
                Return gDB2ProviderType
            End Get
            Set(value As String)
                gDB2ProviderType = value
            End Set
        End Property

        Private gDB2Size As Short
        Public Property DB2Size() As Short
            Get
                ' Return the value for the property 
                Return gDB2Size
            End Get
            Set(value As Short)
                gDB2Size = value
            End Set
        End Property

        Private gDB2Scale As Short
        Public Property DB2Scale() As Short
            Get
                ' Return the value for the property 
                Return gDB2Scale
            End Get
            Set(value As Short)
                gDB2Scale = value
            End Set
        End Property

        Private gCTSPreSelect As Boolean
        Public Property CTSPreSelect() As Boolean
            Get
                ' Return the value for the property 
                Return gCTSPreSelect
            End Get
            Set(value As Boolean)
                gCTSPreSelect = value
            End Set
        End Property

        Private gCTSSumCol As Boolean
        Public Property CTSSumCol() As Boolean
            Get
                ' Return the value for the property 
                Return gCTSSumCol
            End Get
            Set(value As Boolean)
                gCTSSumCol = value
            End Set
        End Property

        Private gCTSSumFormat As String
        Public Property CTSSumFormat() As String
            Get
                ' Return the value for the property 
                Return gCTSSumFormat
            End Get
            Set(value As String)
                gCTSSumFormat = value
            End Set
        End Property
    End Class


    'CTS ComboBox column
    Public Class DataGridViewComboBoxCellCTS
        Inherits DataGridViewComboBoxCell

        Protected Overrides Sub Paint(
            ByVal graphics As Graphics,
            ByVal clipBounds As Rectangle,
            ByVal cellBounds As Rectangle,
            ByVal rowIndex As Integer,
            ByVal elementState As DataGridViewElementStates,
            ByVal value As Object,
            ByVal formattedValue As Object,
            ByVal errorText As String,
            ByVal cellStyle As DataGridViewCellStyle,
            ByVal advancedBorderStyle As DataGridViewAdvancedBorderStyle,
            ByVal paintParts As DataGridViewPaintParts)

            'Change back color to ERROR
            If errorText <> String.Empty Then
                Me.DataGridView.Item(ColumnIndex, rowIndex).Style.BackColor = EditErrorBack
            End If

            'Change back color to EDITABLE
            If (elementState And DataGridViewElementStates.ReadOnly) Then
            Else
                If errorText = String.Empty Then
                    Me.DataGridView.Item(ColumnIndex, rowIndex).Style.BackColor = EditBack
                End If
            End If

            ' Call the base class method to paint the default cell appearance. 
            MyBase.Paint(graphics, clipBounds, cellBounds, rowIndex, elementState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts)


            ' HOVER - If the mouse pointer is over the current cell, draw a custom border.
            ' Retrieve the client location of the mouse pointer. 
            Dim cursorPosition As Point = Me.DataGridView.PointToClient(Cursor.Position)
            If cellBounds.Contains(cursorPosition) Then
                Dim newRect As New Rectangle(cellBounds.X + 1, cellBounds.Y + 1, cellBounds.Width - 4, cellBounds.Height - 4)
                graphics.DrawRectangle(Pens.DarkGreen, newRect)
            End If

        End Sub

        ' Force the cell to repaint itself when the mouse pointer enters it. 
        Protected Overrides Sub OnMouseEnter(ByVal rowIndex As Integer)
            Me.DataGridView.InvalidateCell(Me)
        End Sub

        ' Force the cell to repaint itself when the mouse pointer leaves it. 
        Protected Overrides Sub OnMouseLeave(ByVal rowIndex As Integer)
            Me.DataGridView.InvalidateCell(Me)
        End Sub

    End Class

    Public Class DataGridViewComboBoxColumnCTS
        Inherits DataGridViewComboBoxColumn

        Public Sub New()
            Me.CellTemplate = New DataGridViewComboBoxCellCTS()
            'Me.SortMode = DataGridViewColumnSortMode.Automatic
            Me.SortMode = DataGridViewColumnSortMode.NotSortable
        End Sub

        Private gDB2ProviderType As String
        Public Property DB2ProviderType() As String
            Get
                ' Return the value for the property 
                Return gDB2ProviderType
            End Get
            Set(value As String)
                gDB2ProviderType = value
            End Set
        End Property

        Private gDB2Size As Short
        Public Property DB2Size() As Short
            Get
                ' Return the value for the property 
                Return gDB2Size
            End Get
            Set(value As Short)
                gDB2Size = value
            End Set
        End Property

        Private gDB2Scale As Short
        Public Property DB2Scale() As Short
            Get
                ' Return the value for the property 
                Return gDB2Scale
            End Get
            Set(value As Short)
                gDB2Scale = value
            End Set
        End Property

        Private gCTSPreSelect As Boolean
        Public Property CTSPreSelect() As Boolean
            Get
                ' Return the value for the property 
                Return gCTSPreSelect
            End Get
            Set(value As Boolean)
                gCTSPreSelect = value
            End Set
        End Property

    End Class

    'CTS Calendar Editing Control/Column
    Public Class CalendarColumn
        Inherits DataGridViewColumnCTS

        Public Sub New()
            Me.CellTemplate = New CalendarCell()
        End Sub

        Public Overrides Property CellTemplate() As DataGridViewCell
            Get
                Return MyBase.CellTemplate
            End Get

            Set(ByVal value As DataGridViewCell)
                ' Ensure that the cell used for the template is a CalendarCell.
                If (value IsNot Nothing) AndAlso Not value.GetType().IsAssignableFrom(GetType(CalendarCell)) Then
                    Throw New InvalidCastException("Must be a CalendarCell")
                End If
                MyBase.CellTemplate = value

            End Set
        End Property

    End Class

    Public Class CalendarCell
        Inherits DataGridViewCellCTS

        Public Sub New()
            ' Use the short date format.
            Me.Style.Format = "d"
        End Sub

        Public Overrides Sub InitializeEditingControl(ByVal rowIndex As Integer, ByVal initialFormattedValue As Object, ByVal dataGridViewCellStyle As DataGridViewCellStyle)

            ' Set the value of the editing control to the current cell value.
            MyBase.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle)

            Dim ctl As CalendarEditingControl = CType(DataGridView.EditingControl, CalendarEditingControl)
            If Not IsDBNull(Me.Value) Then
                ctl.Value = CType(Me.Value, DateTime)
            End If

        End Sub

        Public Overrides ReadOnly Property EditType() As Type
            Get
                ' Return the type of the editing contol that CalendarCell uses.
                Return GetType(CalendarEditingControl)
            End Get
        End Property

        Public Overrides ReadOnly Property ValueType() As Type
            Get
                ' Return the type of the value that CalendarCell contains.
                Return GetType(DateTime)
            End Get
        End Property

        Public Overrides ReadOnly Property DefaultNewRowValue() As Object
            Get
                ' Use the current date and time as the default value.
                Return DateTime.Now
            End Get
        End Property

    End Class

    <ToolboxItem(False)>
    Class CalendarEditingControl
        Inherits DateTimePicker
        Implements IDataGridViewEditingControl

        Private dataGridViewControl As DataGridView
        Private valueIsChanged As Boolean = False
        Private rowIndexNum As Integer

        Public Sub New()
            Me.Format = DateTimePickerFormat.Short
        End Sub

        Public Property EditingControlFormattedValue() As Object _
        Implements IDataGridViewEditingControl.EditingControlFormattedValue

            Get
                Return Me.Value.ToShortDateString()
            End Get

            Set(ByVal value As Object)
                If TypeOf value Is String Then
                    Me.Value = DateTime.Parse(CStr(value))
                End If
            End Set

        End Property

        Public Function GetEditingControlFormattedValue(ByVal context As DataGridViewDataErrorContexts) As Object _
        Implements IDataGridViewEditingControl.GetEditingControlFormattedValue

            Return Me.Value.ToShortDateString()

        End Function

        Public Sub ApplyCellStyleToEditingControl(ByVal dataGridViewCellStyle As DataGridViewCellStyle) _
        Implements IDataGridViewEditingControl.ApplyCellStyleToEditingControl

            Me.Font = dataGridViewCellStyle.Font
            Me.CalendarForeColor = dataGridViewCellStyle.ForeColor
            Me.CalendarMonthBackground = dataGridViewCellStyle.BackColor

        End Sub

        Public Property EditingControlRowIndex() As Integer _
        Implements IDataGridViewEditingControl.EditingControlRowIndex

            Get
                Return rowIndexNum
            End Get
            Set(ByVal value As Integer)
                rowIndexNum = value
            End Set

        End Property

        Public Function EditingControlWantsInputKey(ByVal key As Keys, ByVal dataGridViewWantsInputKey As Boolean) As Boolean _
        Implements IDataGridViewEditingControl.EditingControlWantsInputKey

            ' Let the DateTimePicker handle the keys listed.
            Select Case key And Keys.KeyCode
                Case Keys.Left, Keys.Up, Keys.Down, Keys.Right,
                Keys.Home, Keys.End, Keys.PageDown, Keys.PageUp

                    Return True

                Case Else
                    Return Not dataGridViewWantsInputKey

            End Select

        End Function

        Public Sub PrepareEditingControlForEdit(ByVal selectAll As Boolean) _
        Implements IDataGridViewEditingControl.PrepareEditingControlForEdit

            ' No preparation needs to be done.

        End Sub

        Public ReadOnly Property RepositionEditingControlOnValueChange() _
        As Boolean Implements IDataGridViewEditingControl.RepositionEditingControlOnValueChange

            Get
                Return False
            End Get

        End Property

        Public Property EditingControlDataGridView() As DataGridView _
        Implements IDataGridViewEditingControl.EditingControlDataGridView

            Get
                Return dataGridViewControl
            End Get
            Set(ByVal value As DataGridView)
                dataGridViewControl = value
            End Set

        End Property

        Public Property EditingControlValueChanged() As Boolean _
        Implements IDataGridViewEditingControl.EditingControlValueChanged

            Get
                Return valueIsChanged
            End Get
            Set(ByVal value As Boolean)
                valueIsChanged = value
            End Set

        End Property

        Public ReadOnly Property EditingControlCursor() As Cursor _
        Implements IDataGridViewEditingControl.EditingPanelCursor

            Get
                Return MyBase.Cursor
            End Get

        End Property

        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            MyBase.OnPaint(e)
        End Sub

        Protected Overrides Sub OnValueChanged(ByVal eventargs As EventArgs)

            ' Notify the DataGridView that the contents of the cell have changed.
            valueIsChanged = True
            Me.EditingControlDataGridView.NotifyCurrentCellDirty(True)
            MyBase.OnValueChanged(eventargs)

        End Sub

    End Class


    Public Class PromptColumn
        Inherits DataGridViewColumnCTS
        Private gPromptType As String

        Public Sub New()
            Me.CellTemplate = New PromptCell()
        End Sub

        Public Overrides Property CellTemplate() As DataGridViewCell
            Get
                Return MyBase.CellTemplate
            End Get

            Set(ByVal value As DataGridViewCell)
                ' Ensure that the cell used for the template is a PromptCell.
                If (value IsNot Nothing) AndAlso Not value.GetType().IsAssignableFrom(GetType(PromptCell)) Then
                    Throw New InvalidCastException("Must be a PromptCell")
                End If
                MyBase.CellTemplate = value

            End Set
        End Property
        Public Property PromptType() As String
            Get
                Return gPromptType
            End Get
            Set(value As String)
                gPromptType = value
            End Set
        End Property
    End Class

    Public Class PromptCell
        Inherits DataGridViewCellCTS
        Private gPromptType As String

        Public Sub New()
            'no comment
        End Sub

        Public Overrides Sub InitializeEditingControl(ByVal rowIndex As Integer, ByVal initialFormattedValue As Object, ByVal dataGridViewCellStyle As DataGridViewCellStyle)

            If Me.OwningColumn IsNot Nothing Then
                Dim pc As PromptColumn = Me.OwningColumn
                gPromptType = pc.PromptType
            End If

            ' Set the value of the editing control to the current cell value.
            MyBase.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle)

            Dim ctl As PromptEditingControl = CType(DataGridView.EditingControl, PromptEditingControl)
            ctl.PromptType = gPromptType

            If Not IsDBNull(Me.Value) Then
                ctl.Text = Me.Value
            End If
        End Sub

        Public Overrides ReadOnly Property EditType() As Type
            Get
                ' Return the type of the editing contol that PromptCell uses.
                Return GetType(PromptEditingControl)
            End Get
        End Property

        Public Overrides ReadOnly Property ValueType() As Type
            Get
                ' Return the type of the value that PromptCell contains.
                Return GetType(String)
            End Get
        End Property

        Public Overrides ReadOnly Property DefaultNewRowValue() As Object
            Get
                ' Use the current date and time as the default value.
                Return String.Empty
            End Get
        End Property


    End Class

    <ToolboxItem(False)>
    Class PromptEditingControl
        Inherits ComboBox
        Implements IDataGridViewEditingControl

        Declare Function SendMessage Lib "User" (ByVal hWnd As Integer,
      ByVal wMsg As Integer, ByVal wParam As Integer, lParam As Integer) As Long

        Const CB_SHOWDROPDOWN = &H14F

        Private gPromptType As String
        Public Property PromptType() As String
            Get
                Return gPromptType
            End Get
            Set(value As String)
                gPromptType = value
            End Set
        End Property


        Private dataGridViewControl As DataGridView
        Private valueIsChanged As Boolean = False
        Private rowIndexNum As Integer

        Public Sub New()
            Me.DropDownStyle = ComboBoxStyle.DropDown
            Me.DropDownHeight = 1
        End Sub

        Public Property EditingControlFormattedValue() As Object _
        Implements IDataGridViewEditingControl.EditingControlFormattedValue

            Get
                Return Me.Text
            End Get

            Set(ByVal value As Object)
                If TypeOf value Is String Then
                    Me.Text = value
                End If
            End Set

        End Property

        Public Function GetEditingControlFormattedValue(ByVal context As DataGridViewDataErrorContexts) As Object _
        Implements IDataGridViewEditingControl.GetEditingControlFormattedValue

            Return Me.Text

        End Function

        Public Sub ApplyCellStyleToEditingControl(ByVal dataGridViewCellStyle As DataGridViewCellStyle) _
        Implements IDataGridViewEditingControl.ApplyCellStyleToEditingControl

            Me.Font = dataGridViewCellStyle.Font
            Me.ForeColor = dataGridViewCellStyle.ForeColor
            Me.BackColor = dataGridViewCellStyle.BackColor

        End Sub

        Public Property EditingControlRowIndex() As Integer _
        Implements IDataGridViewEditingControl.EditingControlRowIndex

            Get
                Return rowIndexNum
            End Get
            Set(ByVal value As Integer)
                rowIndexNum = value
            End Set

        End Property

        Public Function EditingControlWantsInputKey(ByVal key As Keys, ByVal dataGridViewWantsInputKey As Boolean) As Boolean _
        Implements IDataGridViewEditingControl.EditingControlWantsInputKey

            Return Not dataGridViewWantsInputKey

        End Function

        Public Sub PrepareEditingControlForEdit(ByVal selectAll As Boolean) _
        Implements IDataGridViewEditingControl.PrepareEditingControlForEdit

            ' No preparation needs to be done.

        End Sub

        Public ReadOnly Property RepositionEditingControlOnValueChange() _
        As Boolean Implements IDataGridViewEditingControl.RepositionEditingControlOnValueChange

            Get
                Return False
            End Get

        End Property

        Public Property EditingControlDataGridView() As DataGridView _
        Implements IDataGridViewEditingControl.EditingControlDataGridView

            Get
                Return dataGridViewControl
            End Get
            Set(ByVal value As DataGridView)
                dataGridViewControl = value
            End Set

        End Property

        Public Property EditingControlValueChanged() As Boolean _
        Implements IDataGridViewEditingControl.EditingControlValueChanged

            Get
                Return valueIsChanged
            End Get
            Set(ByVal value As Boolean)
                valueIsChanged = value
            End Set

        End Property

        Public ReadOnly Property EditingControlCursor() As Cursor _
        Implements IDataGridViewEditingControl.EditingPanelCursor

            Get
                Return MyBase.Cursor
            End Get

        End Property

        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            MyBase.OnPaint(e)
        End Sub

        Protected Overrides Sub OnSelectedValueChanged(ByVal eventargs As EventArgs)

            ' Notify the DataGridView that the contents of the cell have changed.
            valueIsChanged = True
            Me.EditingControlDataGridView.NotifyCurrentCellDirty(True)
            MyBase.OnSelectedValueChanged(eventargs)

        End Sub

        Protected Overrides Sub OnDropDown(e As EventArgs)

            'Retract the Dropdown
            Try
                MyBase.RecreateHandle()
                SendMessage(Me.Handle.ToInt32, CB_SHOWDROPDOWN, 0, 0&)
            Catch
            End Try
            RaiseEvent CellPrompted(Me, e)

            MyBase.OnDropDown(e)

        End Sub

        Public Event CellPrompted As EventHandler
        Protected Overridable Sub onCellPrompted(e As EventArgs)
            RaiseEvent CellPrompted(Me, e)
        End Sub

    End Class


    Public Class DataGridViewMaskedEditColumn
        Inherits DataGridViewColumnCTS

        Private pPromptChar As Char = "_"c
        Private pValidatingType As Type = GetType(String)
        Private pMask As String = ""

        Public Sub New()
            Me.CellTemplate = New DataGridViewMaskedEditCell()
        End Sub

        Public Overrides Property CellTemplate() As DataGridViewCell
            Get
                Return MyBase.CellTemplate
            End Get
            Set(ByVal value As DataGridViewCell)
                ' Ensure that the cell used for the template is a MaskedEditCell
                If Not (value Is Nothing) And Not value.GetType().IsAssignableFrom(GetType(DataGridViewMaskedEditCell)) Then
                    Throw New InvalidCastException("Must be a DataGridViewMaskedEditCell")
                End If
                MyBase.CellTemplate = value
            End Set
        End Property

        '
        ' New properties required by the MaskedEditTextBox control
        '
        Public Property Mask() As String
            Get
                Return pMask
            End Get
            Set(ByVal value As String)
                pMask = value
            End Set
        End Property

        Public Property PromptChar() As Char
            Get
                Return pPromptChar
            End Get
            Set(ByVal value As Char)
                pPromptChar = value
            End Set
        End Property

        Public Property ValidatingType() As Type
            Get
                Return pValidatingType
            End Get
            Set(ByVal value As Type)
                pValidatingType = value
            End Set
        End Property
    End Class

    Public Class DataGridViewMaskedEditCell
        Inherits DataGridViewCellCTS

        Dim pColumn As DataGridViewMaskedEditColumn

        Public Sub New()
            ' nothing to do
        End Sub

        Public Overrides Sub InitializeEditingControl(ByVal rowIndex As Integer, ByVal initialFormattedValue As Object, ByVal dataGridViewCellStyle As DataGridViewCellStyle)
            MyBase.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle)

            pColumn = CType(Me.OwningColumn, DataGridViewMaskedEditColumn)
            Dim ctl As MaskedEditEditingControl = CType(DataGridView.EditingControl, MaskedEditEditingControl)

            ' copy over the properties of the column
            If Not IsNothing(Me.Value) Then
                ctl.Text = Me.Value.ToString
            Else
                ctl.Text = ""
            End If
            ctl.ValidatingType = pColumn.ValidatingType
            ctl.Mask = pColumn.Mask
            ctl.PromptChar = pColumn.PromptChar
        End Sub

        Public Overrides ReadOnly Property EditType() As Type
            Get
                ' Return the type of the editing contol
                Return GetType(MaskedEditEditingControl)
            End Get
        End Property

        Public Overrides ReadOnly Property ValueType() As Type
            Get
                ' The validating type is set in the column class
                Return pColumn.ValidatingType
            End Get
        End Property

        Public Overrides ReadOnly Property DefaultNewRowValue() As Object
            Get
                Return ""
            End Get
        End Property
    End Class

    <ToolboxItem(False)>
    Class MaskedEditEditingControl
        Inherits MaskedTextBox
        Implements IDataGridViewEditingControl

        Private dataGridViewControl As DataGridView
        Private valueIsChanged As Boolean = False
        Private rowIndexNum As Integer

        Public Sub New()
            ' nothing to do
        End Sub

        Public Property EditingControlFormattedValue() As Object Implements IDataGridViewEditingControl.EditingControlFormattedValue
            Get
                Return Me.valueIsChanged.ToString
            End Get
            Set(ByVal value As Object)
                If TypeOf value Is [String] Then
                    Me.Text = value.ToString
                End If
            End Set
        End Property

        Public Function GetEditingControlFormattedValue(ByVal context As DataGridViewDataErrorContexts) As Object Implements IDataGridViewEditingControl.GetEditingControlFormattedValue
            Return Me.Text
        End Function

        Public Sub ApplyCellStyleToEditingControl(ByVal dataGridViewCellStyle As DataGridViewCellStyle) Implements IDataGridViewEditingControl.ApplyCellStyleToEditingControl
            Me.Font = dataGridViewCellStyle.Font
            Me.ForeColor = dataGridViewCellStyle.ForeColor
            Me.BackColor = dataGridViewCellStyle.BackColor
        End Sub

        Public Property EditingControlRowIndex() As Integer Implements IDataGridViewEditingControl.EditingControlRowIndex
            Get
                Return rowIndexNum
            End Get
            Set(ByVal value As Integer)
                rowIndexNum = value
            End Set
        End Property

        Public Function EditingControlWantsInputKey(ByVal key As Keys, ByVal dataGridViewWantsInputKey As Boolean) As Boolean Implements IDataGridViewEditingControl.EditingControlWantsInputKey
            Return True
        End Function

        Public Sub PrepareEditingControlForEdit(ByVal selectAll As Boolean) Implements IDataGridViewEditingControl.PrepareEditingControlForEdit
            ' No preparation needs to be done.
        End Sub

        Public ReadOnly Property RepositionEditingControlOnValueChange() As Boolean Implements IDataGridViewEditingControl.RepositionEditingControlOnValueChange
            Get
                Return False
            End Get
        End Property

        Public Property EditingControlDataGridView() As DataGridView Implements IDataGridViewEditingControl.EditingControlDataGridView
            Get
                Return dataGridViewControl
            End Get
            Set(ByVal value As DataGridView)
                dataGridViewControl = value
            End Set
        End Property

        Public Property EditingControlValueChanged() As Boolean Implements IDataGridViewEditingControl.EditingControlValueChanged
            Get
                Return valueIsChanged
            End Get
            Set(ByVal value As Boolean)
                valueIsChanged = value
            End Set
        End Property

        Public ReadOnly Property EditingPanelCursor() As Cursor Implements IDataGridViewEditingControl.EditingPanelCursor
            Get
                Return MyBase.Cursor
            End Get
        End Property

        Protected Overrides Sub OnTextChanged(ByVal eventargs As EventArgs)
            ' Notify the DataGridView that the contents of the cell have changed.
            valueIsChanged = True
            Me.EditingControlDataGridView.NotifyCurrentCellDirty(True)
            MyBase.OnTextChanged(eventargs)
        End Sub
    End Class

#End Region
