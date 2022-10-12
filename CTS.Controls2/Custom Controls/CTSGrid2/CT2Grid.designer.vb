<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class CT2Grid
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
                If GridSettingIsDirty Then
                    GridSettingIsDirty = False
                    GH.SaveGridSettings(dgvCTS, gName)
                End If
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(CT2Grid))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.cmsCellMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.miFilterOnly = New System.Windows.Forms.ToolStripMenuItem()
        Me.miFilterExclude = New System.Windows.Forms.ToolStripMenuItem()
        Me.miFilterValue = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.miFilterEqual = New System.Windows.Forms.ToolStripMenuItem()
        Me.miFilterNotEqual = New System.Windows.Forms.ToolStripMenuItem()
        Me.miFilterGreater = New System.Windows.Forms.ToolStripMenuItem()
        Me.miFilterGreaterEq = New System.Windows.Forms.ToolStripMenuItem()
        Me.miFilterLess = New System.Windows.Forms.ToolStripMenuItem()
        Me.miFilterLessEq = New System.Windows.Forms.ToolStripMenuItem()
        Me.miAdvancedFilterCol = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmsGrid = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.tsmiExportToExcel = New System.Windows.Forms.ToolStripMenuItem()
        Me.ResetToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.miShowTotals = New System.Windows.Forms.ToolStripMenuItem()
        Me.miAllowWrapping = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
        Me.miRefresh = New System.Windows.Forms.ToolStripMenuItem()
        Me.miProperties = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmsRowOptions = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.lvwErrors = New System.Windows.Forms.ListView()
        Me.ErrorRow = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Description = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Row = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Column = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Key = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.tmrPositionReset = New System.Windows.Forms.Timer(Me.components)
        Me.cmsColumn = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.PlaceHolderPosition = New System.Windows.Forms.ToolStripMenuItem()
        Me.PositionSeparator = New System.Windows.Forms.ToolStripSeparator()
        Me.PlaceHolderScan = New System.Windows.Forms.ToolStripMenuItem()
        Me.ScanSeparator = New System.Windows.Forms.ToolStripSeparator()
        Me.miHideColumn = New System.Windows.Forms.ToolStripMenuItem()
        Me.HideColumnSeparator = New System.Windows.Forms.ToolStripSeparator()
        Me.miDistinctValues = New System.Windows.Forms.ToolStripMenuItem()
        Me.PlaceHolderDistinct = New System.Windows.Forms.ToolStripMenuItem()
        Me.miDateFilters = New System.Windows.Forms.ToolStripMenuItem()
        Me.miCurrentDayFilter = New System.Windows.Forms.ToolStripMenuItem()
        Me.miCurrentMonthFilter = New System.Windows.Forms.ToolStripMenuItem()
        Me.miCurrentYearFilter = New System.Windows.Forms.ToolStripMenuItem()
        Me.miPriorMonthFilter = New System.Windows.Forms.ToolStripMenuItem()
        Me.miPriorYearFilter = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.Last30DaysToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Last60DaysToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Last90DaysToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiColumnFilter = New System.Windows.Forms.ToolStripMenuItem()
        Me.FilteringSeparator = New System.Windows.Forms.ToolStripSeparator()
        Me.tsmiSortDescending = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiSortAscending = New System.Windows.Forms.ToolStripMenuItem()
        Me.pnlTotals = New System.Windows.Forms.Panel()
        Me.dgvTotals = New System.Windows.Forms.DataGridView()
        Me.cmsTotals = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.miHideTotals = New System.Windows.Forms.ToolStripMenuItem()
        Me.LocationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.miTotalsTop = New System.Windows.Forms.ToolStripMenuItem()
        Me.miTotalsBottom = New System.Windows.Forms.ToolStripMenuItem()
        Me.pnlTotFiller = New System.Windows.Forms.Panel()
        Me.cmsSettings = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.miColumns = New System.Windows.Forms.ToolStripMenuItem()
        Me.miSorting = New System.Windows.Forms.ToolStripMenuItem()
        Me.miFilters = New System.Windows.Forms.ToolStripMenuItem()
        Me.miClearAllFilters = New System.Windows.Forms.ToolStripMenuItem()
        Me.tmrIdle = New System.Windows.Forms.Timer(Me.components)
        Me.pnlLoading = New System.Windows.Forms.Panel()
        Me.lblLoading = New System.Windows.Forms.Label()
        Me.tsFunctions = New System.Windows.Forms.ToolStrip()
        Me.tsBanner = New System.Windows.Forms.ToolStrip()
        Me.btnGridRefresh = New System.Windows.Forms.ToolStripButton()
        Me.lblGridTitle = New System.Windows.Forms.ToolStripStatusLabel()
        Me.btnSettings = New System.Windows.Forms.ToolStripButton()
        Me.lblFiltered = New System.Windows.Forms.ToolStripLabel()
        Me.btnFilter = New System.Windows.Forms.ToolStripButton()
        Me.ssBottom = New System.Windows.Forms.StatusStrip()
        Me.lblGridStatus = New System.Windows.Forms.ToolStripStatusLabel()
        Me.lblSuspend = New System.Windows.Forms.ToolStripStatusLabel()
        Me.dgvCTS = New DataGridViewCT2()
        Me.cmsCellMenu.SuspendLayout()
        Me.cmsGrid.SuspendLayout()
        Me.cmsColumn.SuspendLayout()
        Me.pnlTotals.SuspendLayout()
        CType(Me.dgvTotals, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cmsTotals.SuspendLayout()
        Me.cmsSettings.SuspendLayout()
        Me.pnlLoading.SuspendLayout()
        Me.tsBanner.SuspendLayout()
        Me.ssBottom.SuspendLayout()
        CType(Me.dgvCTS, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmsCellMenu
        '
        Me.cmsCellMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.miFilterOnly, Me.miFilterExclude, Me.miFilterValue, Me.miAdvancedFilterCol})
        Me.cmsCellMenu.Name = "cmsCellMenu"
        Me.cmsCellMenu.Size = New System.Drawing.Size(209, 92)
        '
        'miFilterOnly
        '
        Me.miFilterOnly.Image = CType(resources.GetObject("miFilterOnly.Image"), System.Drawing.Image)
        Me.miFilterOnly.Name = "miFilterOnly"
        Me.miFilterOnly.Size = New System.Drawing.Size(208, 22)
        Me.miFilterOnly.Text = "Only show value"
        '
        'miFilterExclude
        '
        Me.miFilterExclude.Image = CType(resources.GetObject("miFilterExclude.Image"), System.Drawing.Image)
        Me.miFilterExclude.Name = "miFilterExclude"
        Me.miFilterExclude.Size = New System.Drawing.Size(208, 22)
        Me.miFilterExclude.Text = "Exclude value"
        '
        'miFilterValue
        '
        Me.miFilterValue.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripSeparator1, Me.miFilterEqual, Me.miFilterNotEqual, Me.miFilterGreater, Me.miFilterGreaterEq, Me.miFilterLess, Me.miFilterLessEq})
        Me.miFilterValue.Image = CType(resources.GetObject("miFilterValue.Image"), System.Drawing.Image)
        Me.miFilterValue.Name = "miFilterValue"
        Me.miFilterValue.Size = New System.Drawing.Size(208, 22)
        Me.miFilterValue.Text = "Filter on this value"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(147, 6)
        '
        'miFilterEqual
        '
        Me.miFilterEqual.Name = "miFilterEqual"
        Me.miFilterEqual.Size = New System.Drawing.Size(150, 22)
        Me.miFilterEqual.Text = "Equal"
        '
        'miFilterNotEqual
        '
        Me.miFilterNotEqual.Name = "miFilterNotEqual"
        Me.miFilterNotEqual.Size = New System.Drawing.Size(150, 22)
        Me.miFilterNotEqual.Text = "Not Equal"
        '
        'miFilterGreater
        '
        Me.miFilterGreater.Name = "miFilterGreater"
        Me.miFilterGreater.Size = New System.Drawing.Size(150, 22)
        Me.miFilterGreater.Text = "Greater than"
        '
        'miFilterGreaterEq
        '
        Me.miFilterGreaterEq.Name = "miFilterGreaterEq"
        Me.miFilterGreaterEq.Size = New System.Drawing.Size(150, 22)
        Me.miFilterGreaterEq.Text = "Greater than ="
        '
        'miFilterLess
        '
        Me.miFilterLess.Name = "miFilterLess"
        Me.miFilterLess.Size = New System.Drawing.Size(150, 22)
        Me.miFilterLess.Text = "Less than"
        '
        'miFilterLessEq
        '
        Me.miFilterLessEq.Name = "miFilterLessEq"
        Me.miFilterLessEq.Size = New System.Drawing.Size(150, 22)
        Me.miFilterLessEq.Text = "Less than ="
        '
        'miAdvancedFilterCol
        '
        Me.miAdvancedFilterCol.Image = CType(resources.GetObject("miAdvancedFilterCol.Image"), System.Drawing.Image)
        Me.miAdvancedFilterCol.Name = "miAdvancedFilterCol"
        Me.miAdvancedFilterCol.Size = New System.Drawing.Size(208, 22)
        Me.miAdvancedFilterCol.Text = "Custom Column Filtering"
        '
        'cmsGrid
        '
        Me.cmsGrid.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiExportToExcel, Me.ResetToolStripMenuItem, Me.miShowTotals, Me.miAllowWrapping, Me.ToolStripSeparator5, Me.miRefresh, Me.miProperties})
        Me.cmsGrid.Name = "cmsGrid"
        Me.cmsGrid.Size = New System.Drawing.Size(200, 142)
        '
        'tsmiExportToExcel
        '
        Me.tsmiExportToExcel.Name = "tsmiExportToExcel"
        Me.tsmiExportToExcel.Size = New System.Drawing.Size(199, 22)
        Me.tsmiExportToExcel.Text = "Export to Excel"
        '
        'ResetToolStripMenuItem
        '
        Me.ResetToolStripMenuItem.Name = "ResetToolStripMenuItem"
        Me.ResetToolStripMenuItem.Size = New System.Drawing.Size(199, 22)
        Me.ResetToolStripMenuItem.Text = "Restore Default Settings"
        '
        'miShowTotals
        '
        Me.miShowTotals.CheckOnClick = True
        Me.miShowTotals.Name = "miShowTotals"
        Me.miShowTotals.Size = New System.Drawing.Size(199, 22)
        Me.miShowTotals.Text = "Show Totals"
        '
        'miAllowWrapping
        '
        Me.miAllowWrapping.Name = "miAllowWrapping"
        Me.miAllowWrapping.Size = New System.Drawing.Size(199, 22)
        Me.miAllowWrapping.Text = "Allow Wrapping"
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(196, 6)
        '
        'miRefresh
        '
        Me.miRefresh.Name = "miRefresh"
        Me.miRefresh.ShortcutKeys = System.Windows.Forms.Keys.F5
        Me.miRefresh.Size = New System.Drawing.Size(199, 22)
        Me.miRefresh.Text = "Refresh"
        '
        'miProperties
        '
        Me.miProperties.Name = "miProperties"
        Me.miProperties.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.miProperties.Size = New System.Drawing.Size(199, 22)
        Me.miProperties.Text = "Properties"
        '
        'cmsRowOptions
        '
        Me.cmsRowOptions.Name = "cmsRowOptions"
        Me.cmsRowOptions.Size = New System.Drawing.Size(61, 4)
        '
        'lvwErrors
        '
        Me.lvwErrors.BackColor = System.Drawing.Color.Red
        Me.lvwErrors.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lvwErrors.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ErrorRow, Me.Description, Me.Row, Me.Column, Me.Key})
        Me.lvwErrors.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lvwErrors.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwErrors.ForeColor = System.Drawing.Color.White
        Me.lvwErrors.FullRowSelect = True
        Me.lvwErrors.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        Me.lvwErrors.Location = New System.Drawing.Point(0, 149)
        Me.lvwErrors.Name = "lvwErrors"
        Me.lvwErrors.Size = New System.Drawing.Size(484, 18)
        Me.lvwErrors.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lvwErrors.TabIndex = 24
        Me.lvwErrors.UseCompatibleStateImageBehavior = False
        Me.lvwErrors.View = System.Windows.Forms.View.Details
        '
        'ErrorRow
        '
        Me.ErrorRow.Text = ""
        Me.ErrorRow.Width = 20
        '
        'Description
        '
        Me.Description.Text = "Error Description "
        Me.Description.Width = 400
        '
        'Row
        '
        Me.Row.Text = "Row"
        Me.Row.Width = 0
        '
        'Column
        '
        Me.Column.Text = "Column"
        Me.Column.Width = 0
        '
        'Key
        '
        Me.Key.Text = "Key"
        Me.Key.Width = 0
        '
        'tmrPositionReset
        '
        Me.tmrPositionReset.Interval = 1500
        '
        'cmsColumn
        '
        Me.cmsColumn.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PlaceHolderPosition, Me.PositionSeparator, Me.PlaceHolderScan, Me.ScanSeparator, Me.miHideColumn, Me.HideColumnSeparator, Me.miDistinctValues, Me.miDateFilters, Me.tsmiColumnFilter, Me.FilteringSeparator, Me.tsmiSortDescending, Me.tsmiSortAscending})
        Me.cmsColumn.Name = "cmsGrid"
        Me.cmsColumn.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional
        Me.cmsColumn.Size = New System.Drawing.Size(209, 204)
        '
        'PlaceHolderPosition
        '
        Me.PlaceHolderPosition.Name = "PlaceHolderPosition"
        Me.PlaceHolderPosition.Size = New System.Drawing.Size(208, 22)
        Me.PlaceHolderPosition.Text = "PlaceHolderPosition"
        Me.PlaceHolderPosition.Visible = False
        '
        'PositionSeparator
        '
        Me.PositionSeparator.Name = "PositionSeparator"
        Me.PositionSeparator.Size = New System.Drawing.Size(205, 6)
        '
        'PlaceHolderScan
        '
        Me.PlaceHolderScan.Name = "PlaceHolderScan"
        Me.PlaceHolderScan.Size = New System.Drawing.Size(208, 22)
        Me.PlaceHolderScan.Text = "PlaceHolderScan"
        Me.PlaceHolderScan.Visible = False
        '
        'ScanSeparator
        '
        Me.ScanSeparator.Name = "ScanSeparator"
        Me.ScanSeparator.Size = New System.Drawing.Size(205, 6)
        '
        'miHideColumn
        '
        Me.miHideColumn.Name = "miHideColumn"
        Me.miHideColumn.Size = New System.Drawing.Size(208, 22)
        Me.miHideColumn.Text = "Hide this Column"
        '
        'HideColumnSeparator
        '
        Me.HideColumnSeparator.Name = "HideColumnSeparator"
        Me.HideColumnSeparator.Size = New System.Drawing.Size(205, 6)
        '
        'miDistinctValues
        '
        Me.miDistinctValues.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PlaceHolderDistinct})
        Me.miDistinctValues.Image = CType(resources.GetObject("miDistinctValues.Image"), System.Drawing.Image)
        Me.miDistinctValues.Name = "miDistinctValues"
        Me.miDistinctValues.Size = New System.Drawing.Size(208, 22)
        Me.miDistinctValues.Text = "Distinct Values"
        '
        'PlaceHolderDistinct
        '
        Me.PlaceHolderDistinct.Name = "PlaceHolderDistinct"
        Me.PlaceHolderDistinct.Size = New System.Drawing.Size(178, 22)
        Me.PlaceHolderDistinct.Text = "PlaceHolderDistinct"
        '
        'miDateFilters
        '
        Me.miDateFilters.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.miCurrentDayFilter, Me.miCurrentMonthFilter, Me.miCurrentYearFilter, Me.miPriorMonthFilter, Me.miPriorYearFilter, Me.ToolStripSeparator4, Me.Last30DaysToolStripMenuItem, Me.Last60DaysToolStripMenuItem, Me.Last90DaysToolStripMenuItem})
        Me.miDateFilters.Image = CType(resources.GetObject("miDateFilters.Image"), System.Drawing.Image)
        Me.miDateFilters.Name = "miDateFilters"
        Me.miDateFilters.Size = New System.Drawing.Size(208, 22)
        Me.miDateFilters.Text = "Date Ranges / Filters"
        '
        'miCurrentDayFilter
        '
        Me.miCurrentDayFilter.Name = "miCurrentDayFilter"
        Me.miCurrentDayFilter.Size = New System.Drawing.Size(153, 22)
        Me.miCurrentDayFilter.Text = "Current Day"
        '
        'miCurrentMonthFilter
        '
        Me.miCurrentMonthFilter.Name = "miCurrentMonthFilter"
        Me.miCurrentMonthFilter.Size = New System.Drawing.Size(153, 22)
        Me.miCurrentMonthFilter.Text = "Current Month"
        '
        'miCurrentYearFilter
        '
        Me.miCurrentYearFilter.Name = "miCurrentYearFilter"
        Me.miCurrentYearFilter.Size = New System.Drawing.Size(153, 22)
        Me.miCurrentYearFilter.Text = "Current Year"
        '
        'miPriorMonthFilter
        '
        Me.miPriorMonthFilter.Name = "miPriorMonthFilter"
        Me.miPriorMonthFilter.Size = New System.Drawing.Size(153, 22)
        Me.miPriorMonthFilter.Text = "Prior Month"
        '
        'miPriorYearFilter
        '
        Me.miPriorYearFilter.Name = "miPriorYearFilter"
        Me.miPriorYearFilter.Size = New System.Drawing.Size(153, 22)
        Me.miPriorYearFilter.Text = "Prior Year"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(150, 6)
        '
        'Last30DaysToolStripMenuItem
        '
        Me.Last30DaysToolStripMenuItem.Name = "Last30DaysToolStripMenuItem"
        Me.Last30DaysToolStripMenuItem.Size = New System.Drawing.Size(153, 22)
        Me.Last30DaysToolStripMenuItem.Text = "Last 30 Days"
        '
        'Last60DaysToolStripMenuItem
        '
        Me.Last60DaysToolStripMenuItem.Name = "Last60DaysToolStripMenuItem"
        Me.Last60DaysToolStripMenuItem.Size = New System.Drawing.Size(153, 22)
        Me.Last60DaysToolStripMenuItem.Text = "Last 60 Days"
        '
        'Last90DaysToolStripMenuItem
        '
        Me.Last90DaysToolStripMenuItem.Name = "Last90DaysToolStripMenuItem"
        Me.Last90DaysToolStripMenuItem.Size = New System.Drawing.Size(153, 22)
        Me.Last90DaysToolStripMenuItem.Text = "Last 90 Days"
        '
        'tsmiColumnFilter
        '
        Me.tsmiColumnFilter.Image = CType(resources.GetObject("tsmiColumnFilter.Image"), System.Drawing.Image)
        Me.tsmiColumnFilter.Name = "tsmiColumnFilter"
        Me.tsmiColumnFilter.Size = New System.Drawing.Size(208, 22)
        Me.tsmiColumnFilter.Text = "Custom Column Filtering"
        '
        'FilteringSeparator
        '
        Me.FilteringSeparator.Name = "FilteringSeparator"
        Me.FilteringSeparator.Size = New System.Drawing.Size(205, 6)
        '
        'tsmiSortDescending
        '
        Me.tsmiSortDescending.Image = CType(resources.GetObject("tsmiSortDescending.Image"), System.Drawing.Image)
        Me.tsmiSortDescending.Name = "tsmiSortDescending"
        Me.tsmiSortDescending.Size = New System.Drawing.Size(208, 22)
        Me.tsmiSortDescending.Text = "Sort Descending"
        '
        'tsmiSortAscending
        '
        Me.tsmiSortAscending.Image = CType(resources.GetObject("tsmiSortAscending.Image"), System.Drawing.Image)
        Me.tsmiSortAscending.Name = "tsmiSortAscending"
        Me.tsmiSortAscending.Size = New System.Drawing.Size(208, 22)
        Me.tsmiSortAscending.Text = "Sort Ascending"
        '
        'pnlTotals
        '
        Me.pnlTotals.BackColor = System.Drawing.Color.White
        Me.pnlTotals.Controls.Add(Me.dgvTotals)
        Me.pnlTotals.Controls.Add(Me.pnlTotFiller)
        Me.pnlTotals.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlTotals.Location = New System.Drawing.Point(0, 25)
        Me.pnlTotals.Name = "pnlTotals"
        Me.pnlTotals.Size = New System.Drawing.Size(484, 23)
        Me.pnlTotals.TabIndex = 27
        Me.pnlTotals.Visible = False
        '
        'dgvTotals
        '
        Me.dgvTotals.AllowUserToAddRows = False
        Me.dgvTotals.AllowUserToDeleteRows = False
        Me.dgvTotals.AllowUserToResizeRows = False
        Me.dgvTotals.BackgroundColor = System.Drawing.Color.White
        Me.dgvTotals.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvTotals.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvTotals.ColumnHeadersVisible = False
        Me.dgvTotals.ContextMenuStrip = Me.cmsTotals
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.Color.White
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvTotals.DefaultCellStyle = DataGridViewCellStyle1
        Me.dgvTotals.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvTotals.EnableHeadersVisualStyles = False
        Me.dgvTotals.Location = New System.Drawing.Point(0, 0)
        Me.dgvTotals.Name = "dgvTotals"
        Me.dgvTotals.ReadOnly = True
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.Color.White
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvTotals.RowHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.dgvTotals.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.dgvTotals.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvTotals.ShowCellErrors = False
        Me.dgvTotals.ShowCellToolTips = False
        Me.dgvTotals.ShowEditingIcon = False
        Me.dgvTotals.ShowRowErrors = False
        Me.dgvTotals.Size = New System.Drawing.Size(467, 23)
        Me.dgvTotals.TabIndex = 10
        '
        'cmsTotals
        '
        Me.cmsTotals.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.miHideTotals, Me.LocationToolStripMenuItem})
        Me.cmsTotals.Name = "cmsTotals"
        Me.cmsTotals.Size = New System.Drawing.Size(134, 48)
        '
        'miHideTotals
        '
        Me.miHideTotals.Name = "miHideTotals"
        Me.miHideTotals.Size = New System.Drawing.Size(133, 22)
        Me.miHideTotals.Text = "Hide Totals"
        '
        'LocationToolStripMenuItem
        '
        Me.LocationToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.miTotalsTop, Me.miTotalsBottom})
        Me.LocationToolStripMenuItem.Name = "LocationToolStripMenuItem"
        Me.LocationToolStripMenuItem.Size = New System.Drawing.Size(133, 22)
        Me.LocationToolStripMenuItem.Text = "Location"
        '
        'miTotalsTop
        '
        Me.miTotalsTop.Name = "miTotalsTop"
        Me.miTotalsTop.Size = New System.Drawing.Size(114, 22)
        Me.miTotalsTop.Text = "Top"
        '
        'miTotalsBottom
        '
        Me.miTotalsBottom.Name = "miTotalsBottom"
        Me.miTotalsBottom.Size = New System.Drawing.Size(114, 22)
        Me.miTotalsBottom.Text = "Bottom"
        '
        'pnlTotFiller
        '
        Me.pnlTotFiller.BackColor = System.Drawing.Color.Transparent
        Me.pnlTotFiller.Dock = System.Windows.Forms.DockStyle.Right
        Me.pnlTotFiller.Location = New System.Drawing.Point(467, 0)
        Me.pnlTotFiller.Name = "pnlTotFiller"
        Me.pnlTotFiller.Size = New System.Drawing.Size(17, 23)
        Me.pnlTotFiller.TabIndex = 11
        '
        'cmsSettings
        '
        Me.cmsSettings.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.miColumns, Me.miSorting, Me.miFilters, Me.miClearAllFilters})
        Me.cmsSettings.Name = "cmsFilter"
        Me.cmsSettings.Size = New System.Drawing.Size(174, 92)
        '
        'miColumns
        '
        Me.miColumns.Image = CType(resources.GetObject("miColumns.Image"), System.Drawing.Image)
        Me.miColumns.Name = "miColumns"
        Me.miColumns.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.miColumns.ShowShortcutKeys = False
        Me.miColumns.Size = New System.Drawing.Size(173, 22)
        Me.miColumns.Text = "Column Settings"
        '
        'miSorting
        '
        Me.miSorting.Image = CType(resources.GetObject("miSorting.Image"), System.Drawing.Image)
        Me.miSorting.Name = "miSorting"
        Me.miSorting.Size = New System.Drawing.Size(173, 22)
        Me.miSorting.Text = "Column Sorting"
        '
        'miFilters
        '
        Me.miFilters.Image = CType(resources.GetObject("miFilters.Image"), System.Drawing.Image)
        Me.miFilters.Name = "miFilters"
        Me.miFilters.Size = New System.Drawing.Size(173, 22)
        Me.miFilters.Text = "Advanced Filtering"
        '
        'miClearAllFilters
        '
        Me.miClearAllFilters.Name = "miClearAllFilters"
        Me.miClearAllFilters.Size = New System.Drawing.Size(173, 22)
        Me.miClearAllFilters.Text = "Clear All Filters"
        '
        'tmrIdle
        '
        Me.tmrIdle.Interval = 1000
        '
        'pnlLoading
        '
        Me.pnlLoading.BackColor = System.Drawing.Color.Transparent
        Me.pnlLoading.Controls.Add(Me.lblLoading)
        Me.pnlLoading.Location = New System.Drawing.Point(3, 65)
        Me.pnlLoading.Name = "pnlLoading"
        Me.pnlLoading.Padding = New System.Windows.Forms.Padding(10)
        Me.pnlLoading.Size = New System.Drawing.Size(345, 61)
        Me.pnlLoading.TabIndex = 28
        '
        'lblLoading
        '
        Me.lblLoading.BackColor = System.Drawing.Color.Transparent
        Me.lblLoading.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblLoading.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLoading.Location = New System.Drawing.Point(10, 10)
        Me.lblLoading.Name = "lblLoading"
        Me.lblLoading.Size = New System.Drawing.Size(325, 41)
        Me.lblLoading.TabIndex = 32
        Me.lblLoading.Text = "Loading..."
        '
        'tsFunctions
        '
        Me.tsFunctions.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.tsFunctions.Location = New System.Drawing.Point(0, 25)
        Me.tsFunctions.Name = "tsFunctions"
        Me.tsFunctions.Size = New System.Drawing.Size(484, 25)
        Me.tsFunctions.TabIndex = 29
        Me.tsFunctions.Text = "tsGridFunctions"
        Me.tsFunctions.Visible = False
        '
        'tsBanner
        '
        Me.tsBanner.BackColor = System.Drawing.Color.DarkGray
        Me.tsBanner.BackgroundImage = Global.My.Resources.Resources.GreenGradientDark
        Me.tsBanner.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.tsBanner.ContextMenuStrip = Me.cmsGrid
        Me.tsBanner.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.tsBanner.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnGridRefresh, Me.lblGridTitle, Me.btnSettings, Me.lblFiltered, Me.btnFilter})
        Me.tsBanner.Location = New System.Drawing.Point(0, 0)
        Me.tsBanner.Name = "tsBanner"
        Me.tsBanner.Size = New System.Drawing.Size(484, 25)
        Me.tsBanner.TabIndex = 22
        Me.tsBanner.TabStop = True
        Me.tsBanner.Text = "ToolStrip1"
        '
        'btnGridRefresh
        '
        Me.btnGridRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnGridRefresh.Image = CType(resources.GetObject("btnGridRefresh.Image"), System.Drawing.Image)
        Me.btnGridRefresh.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnGridRefresh.Name = "btnGridRefresh"
        Me.btnGridRefresh.Padding = New System.Windows.Forms.Padding(10, 0, 0, 0)
        Me.btnGridRefresh.Size = New System.Drawing.Size(30, 22)
        Me.btnGridRefresh.Text = "btnGridRefresh"
        Me.btnGridRefresh.ToolTipText = "Refresh"
        '
        'lblGridTitle
        '
        Me.lblGridTitle.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.lblGridTitle.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGridTitle.ForeColor = System.Drawing.Color.White
        Me.lblGridTitle.Name = "lblGridTitle"
        Me.lblGridTitle.Size = New System.Drawing.Size(80, 20)
        Me.lblGridTitle.Spring = True
        Me.lblGridTitle.Text = "lblGridTitle"
        Me.lblGridTitle.TextAlign = System.Drawing.ContentAlignment.TopLeft
        '
        'btnSettings
        '
        Me.btnSettings.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.btnSettings.AutoToolTip = False
        Me.btnSettings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnSettings.ForeColor = System.Drawing.Color.White
        Me.btnSettings.Image = CType(resources.GetObject("btnSettings.Image"), System.Drawing.Image)
        Me.btnSettings.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnSettings.Name = "btnSettings"
        Me.btnSettings.Padding = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.btnSettings.Size = New System.Drawing.Size(30, 22)
        Me.btnSettings.ToolTipText = "Grid Settings"
        '
        'lblFiltered
        '
        Me.lblFiltered.ForeColor = System.Drawing.Color.White
        Me.lblFiltered.Name = "lblFiltered"
        Me.lblFiltered.Size = New System.Drawing.Size(54, 22)
        Me.lblFiltered.Text = "(Filtered)"
        '
        'btnFilter
        '
        Me.btnFilter.ForeColor = System.Drawing.Color.White
        Me.btnFilter.Image = CType(resources.GetObject("btnFilter.Image"), System.Drawing.Image)
        Me.btnFilter.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnFilter.Name = "btnFilter"
        Me.btnFilter.Size = New System.Drawing.Size(66, 22)
        Me.btnFilter.Text = "Filtered"
        Me.btnFilter.ToolTipText = "Grid Settings"
        Me.btnFilter.Visible = False
        '
        'ssBottom
        '
        Me.ssBottom.BackColor = System.Drawing.Color.DarkGray
        Me.ssBottom.BackgroundImage = Global.My.Resources.Resources.GreenGradientDark
        Me.ssBottom.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ssBottom.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.lblGridStatus, Me.lblSuspend})
        Me.ssBottom.Location = New System.Drawing.Point(0, 167)
        Me.ssBottom.Name = "ssBottom"
        Me.ssBottom.Size = New System.Drawing.Size(484, 22)
        Me.ssBottom.SizingGrip = False
        Me.ssBottom.TabIndex = 17
        Me.ssBottom.Text = "ssBottom"
        '
        'lblGridStatus
        '
        Me.lblGridStatus.BackColor = System.Drawing.Color.Transparent
        Me.lblGridStatus.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGridStatus.ForeColor = System.Drawing.Color.White
        Me.lblGridStatus.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.lblGridStatus.Name = "lblGridStatus"
        Me.lblGridStatus.Size = New System.Drawing.Size(404, 17)
        Me.lblGridStatus.Spring = True
        Me.lblGridStatus.Text = "lblGridStatus"
        Me.lblGridStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblSuspend
        '
        Me.lblSuspend.BackColor = System.Drawing.Color.Transparent
        Me.lblSuspend.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSuspend.ForeColor = System.Drawing.Color.White
        Me.lblSuspend.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.lblSuspend.Name = "lblSuspend"
        Me.lblSuspend.Size = New System.Drawing.Size(65, 17)
        Me.lblSuspend.Tag = ""
        Me.lblSuspend.Text = "SUSPENDED"
        Me.lblSuspend.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'dgvCTS
        '
        Me.dgvCTS.AllowUserToAddRows = False
        Me.dgvCTS.AllowUserToDeleteRows = False
        Me.dgvCTS.AllowUserToOrderColumns = True
        Me.dgvCTS.BackgroundColor = System.Drawing.Color.White
        Me.dgvCTS.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvCTS.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.Color.White
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvCTS.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.dgvCTS.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvCTS.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvCTS.EnableHeadersVisualStyles = False
        Me.dgvCTS.GridColor = System.Drawing.SystemColors.Control
        Me.dgvCTS.IsSuspended = False
        Me.dgvCTS.Location = New System.Drawing.Point(0, 48)
        Me.dgvCTS.Name = "dgvCTS"
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.Color.White
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.White
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvCTS.RowHeadersDefaultCellStyle = DataGridViewCellStyle4
        Me.dgvCTS.RowHeadersWidth = 30
        Me.dgvCTS.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dgvCTS.ShowCellErrors = False
        Me.dgvCTS.ShowRowErrors = False
        Me.dgvCTS.Size = New System.Drawing.Size(484, 101)
        Me.dgvCTS.SuppressEnterKey = "False"
        Me.dgvCTS.TabIndex = 11
        Me.dgvCTS.TabToEditCell = True
        '
        'CT2Grid
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ContextMenuStrip = Me.cmsGrid
        Me.Controls.Add(Me.pnlLoading)
        Me.Controls.Add(Me.dgvCTS)
        Me.Controls.Add(Me.pnlTotals)
        Me.Controls.Add(Me.lvwErrors)
        Me.Controls.Add(Me.ssBottom)
        Me.Controls.Add(Me.tsFunctions)
        Me.Controls.Add(Me.tsBanner)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "CT2Grid"
        Me.Size = New System.Drawing.Size(484, 189)
        Me.cmsCellMenu.ResumeLayout(False)
        Me.cmsGrid.ResumeLayout(False)
        Me.cmsColumn.ResumeLayout(False)
        Me.pnlTotals.ResumeLayout(False)
        CType(Me.dgvTotals, System.ComponentModel.ISupportInitialize).EndInit()
        Me.cmsTotals.ResumeLayout(False)
        Me.cmsSettings.ResumeLayout(False)
        Me.pnlLoading.ResumeLayout(False)
        Me.tsBanner.ResumeLayout(False)
        Me.tsBanner.PerformLayout()
        Me.ssBottom.ResumeLayout(False)
        Me.ssBottom.PerformLayout()
        CType(Me.dgvCTS, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents cmsCellMenu As System.Windows.Forms.ContextMenuStrip
    Public WithEvents cmsGrid As System.Windows.Forms.ContextMenuStrip
    Public WithEvents miFilterValue As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents ssBottom As System.Windows.Forms.StatusStrip
    Friend WithEvents cmsRowOptions As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents btnGridRefresh As System.Windows.Forms.ToolStripButton
    Friend WithEvents lblGridTitle As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents lblSuspend As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents lvwErrors As System.Windows.Forms.ListView
    Friend WithEvents ErrorRow As System.Windows.Forms.ColumnHeader
    Friend WithEvents Description As System.Windows.Forms.ColumnHeader
    Friend WithEvents Row As System.Windows.Forms.ColumnHeader
    Friend WithEvents Column As System.Windows.Forms.ColumnHeader
    Friend WithEvents Key As System.Windows.Forms.ColumnHeader
    Public WithEvents dgvCTS As DataGridViewCT2
    Friend WithEvents tmrPositionReset As System.Windows.Forms.Timer
    Public WithEvents cmsColumn As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents FilteringSeparator As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents miAdvancedFilterCol As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiSortAscending As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiSortDescending As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents miHideColumn As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmiExportToExcel As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ScanSeparator As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsmiColumnFilter As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnFilter As System.Windows.Forms.ToolStripButton
    Friend WithEvents HideColumnSeparator As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ResetToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Protected WithEvents lblGridStatus As System.Windows.Forms.ToolStripStatusLabel
    Public WithEvents tsBanner As System.Windows.Forms.ToolStrip
    Friend WithEvents miDateFilters As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents miCurrentMonthFilter As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents miCurrentYearFilter As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents miPriorMonthFilter As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents miPriorYearFilter As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents Last30DaysToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Last60DaysToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Last90DaysToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents miFilterEqual As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents miFilterNotEqual As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents miFilterGreater As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents miFilterGreaterEq As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents miFilterLess As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents miFilterLessEq As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents pnlTotals As System.Windows.Forms.Panel
    Friend WithEvents dgvTotals As System.Windows.Forms.DataGridView
    Friend WithEvents miShowTotals As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmsTotals As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents miHideTotals As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LocationToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents miTotalsTop As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents miTotalsBottom As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents miFilterOnly As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmsSettings As System.Windows.Forms.ContextMenuStrip
    Public WithEvents miClearAllFilters As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents miFilters As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnSettings As System.Windows.Forms.ToolStripButton
    Friend WithEvents miColumns As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblFiltered As System.Windows.Forms.ToolStripLabel
    Friend WithEvents miSorting As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents miCurrentDayFilter As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tmrIdle As System.Windows.Forms.Timer
    Friend WithEvents miRefresh As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents miProperties As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents pnlLoading As System.Windows.Forms.Panel
    Friend WithEvents pnlTotFiller As System.Windows.Forms.Panel

    Protected Overrides Sub OnEnter(e As EventArgs)
        MyBase.OnEnter(e)
        'tsBanner.BackgroundImage = My.Resources.GreenGradientDark
        tsBanner.BackgroundImage = My.Resources.gradientblue
        ssBottom.BackgroundImage = My.Resources.gradientblue
    End Sub

    Protected Overrides Sub OnLeave(e As EventArgs)
        MyBase.OnLeave(e)
        tsBanner.BackgroundImage = Nothing
        ssBottom.BackgroundImage = Nothing
    End Sub

    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        If Me.Enabled AndAlso Not Me.Focused Then
            Me.Focus()
        End If

        MyBase.OnMouseDown(e)
    End Sub
    Friend WithEvents lblLoading As System.Windows.Forms.Label
    Friend WithEvents ToolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents miAllowWrapping As ToolStripMenuItem
    Friend WithEvents miFilterExclude As ToolStripMenuItem
    Friend WithEvents PlaceHolderPosition As ToolStripMenuItem
    Friend WithEvents PositionSeparator As ToolStripSeparator
    Friend WithEvents PlaceHolderScan As ToolStripMenuItem
    Friend WithEvents miDistinctValues As ToolStripMenuItem
    Friend WithEvents PlaceHolderDistinct As ToolStripMenuItem
    Friend WithEvents tsFunctions As ToolStrip
End Class
