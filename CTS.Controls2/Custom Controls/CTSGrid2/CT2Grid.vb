Imports SQL
Imports CTS.Extensions
Imports System.ComponentModel
Imports Filtering


Public Class CT2Grid

#Region "CTSGrid Declarations"
    Public GH As New Grid2Handler

    Private Initializing As Boolean = True
    Private PositionString As String
    Private GridSettingIsDirty As Boolean

    Private MyEC As Control

    Public G As Grid
    Public sql As SQL

    Public UFilters As New Filter
    Public PFilters As New Filter
    Public SFilters As New Filter

    Private FilterList As New List(Of ColumnFilterValues)
    Private CurFL As ColumnFilterValues

    'todo new filter object
    'Public UserFiltersX As New List(Of Filtering.GridFilter)
    'Private ProgramFiltersX As New List(Of Filtering.GridFilter)
    'Private SecurityFiltersX As New List(Of Filtering.GridFilter)

    Public IsDirty As Boolean
    Private SaveGridID As Integer
    Private SaveSQL As String

    Private SuppressPositionTextChange As Boolean
    Private LastRefresh As DateTime
    Private SelectionDelayTimer As System.Windows.Forms.Timer

    Private ts1 As New Stopwatch
    Private ts2 As New Stopwatch
    Private ts3 As New Stopwatch
    Private ts4 As Stopwatch
#End Region


#Region "CTSGrid Properties"
    Private gApp As String = String.Empty
    <Category("_Custom Properties")>
    <Description("Application Code from DNPF2500")>
    Property GridApp() As String
        Get
            Return gApp
        End Get
        Set(ByVal value As String)
            If value IsNot Nothing Then
                gApp = value
            End If
        End Set
    End Property

    Private gDataEnvironmentCTS As CTS.DataEnvironments
    Private gDataEnvironment As DataEnvironments
    <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    <Category("_Custom Properties")>
    <Description("Data Environment provided by Handler/Serial")>
    Property DataEnvironment() As DataEnvironments
        Get
            Return gDataEnvironment
        End Get
        Set(ByVal value As DataEnvironments)
            gDataEnvironment = value
            Select Case value
                Case DataEnvironments.Development
                    gDataEnvironmentCTS = CTS.DataEnvironments.Development
                Case DataEnvironments.Production
                    gDataEnvironmentCTS = CTS.DataEnvironments.Production
                Case DataEnvironments.Test
                    gDataEnvironmentCTS = CTS.DataEnvironments.Test
            End Select
        End Set
    End Property

    Private gProgramLevel As ProgramLevels = ProgramLevels.Production
    <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    <Category("_Custom Properties")>
    <Description("Program Level")>
    Property ProgramLevel() As ProgramLevels
        Get
            Return gProgramLevel
        End Get
        Set(ByVal value As ProgramLevels)
            gProgramLevel = value
        End Set
    End Property


    Private gConnString As String
    <Category("_Custom Properties")>
    <Description("Connection String")>
    Property ConnectionString() As String
        Get
            Return gConnString
        End Get
        Set(ByVal value As String)
            If value IsNot Nothing Then
                gConnString = value
            End If
        End Set
    End Property

    Private gLoadMethod As GridLoadMethods = GridLoadMethods.DataTable
    <Category("_Custom Properties")>
    <Description("Load Method (DataReader or DataTable)")>
    Property LoadMethod() As GridLoadMethods
        Get
            Return gLoadMethod
        End Get
        Set(ByVal value As GridLoadMethods)
            gLoadMethod = value
        End Set
    End Property

    Private gVersion As Integer
    Private gDefaultVersion As Integer
    Private gOverrideVersion As Integer
    <Category("_Custom Properties")>
    <Description("Override the default ReleaseID of the Grid")>
    Property OverrideVersion() As Integer
        Get
            Return gOverrideVersion
        End Get
        Set(ByVal value As Integer)
            gOverrideVersion = value
        End Set
    End Property

    Private gID As Integer
    <Category("_Custom Properties")>
    <Description("ID of the Grid from DNPF2500 setup")>
    Property GridID() As Integer
        Get
            Return gID
        End Get
        Set(ByVal value As Integer)
            gID = value
        End Set
    End Property

    Private gName As String
    <Category("_Custom Properties")>
    <Description("Name of the Grid from DNPF2500 setup")>
    Property GridName() As String
        Get
            Return gName
        End Get
        Set(ByVal value As String)
            gName = value
        End Set
    End Property

    Private gTitle As String
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Property GridTitle() As String
        Get
            Return gTitle
        End Get
        Set(ByVal value As String)
            gTitle = value
        End Set
    End Property

    Private gBannerText As String
    <Category("_Custom Properties")>
    <Description("Title to display in the Grid Banner")>
    Property BannerText() As String
        Get
            Return gBannerText
        End Get
        Set(ByVal value As String)
            gBannerText = value
            If value = String.Empty Then
                lblGridTitle.Text = gTitle
            Else
                lblGridTitle.Text = gBannerText
            End If
        End Set
    End Property

    Private gIsLoading As Boolean = True
    <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    ReadOnly Property IsLoading() As Boolean
        Get
            Return gIsLoading
        End Get
    End Property

    Private gDataLoaded As Boolean = False
    <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    ReadOnly Property DataLoaded() As Boolean
        Get
            Return gDataLoaded
        End Get
    End Property

    Private gMode As GridModes = GridModes.Display
    <Category("_Custom Properties")>
    Property GridMode() As GridModes
        Get
            Return gMode
        End Get
        Set(ByVal value As GridModes)
            gMode = value
        End Set
    End Property

    Private gAutoRefresh As Integer = 0
    <Category("_Custom Properties")>
    <Description("Number of idle minutes to wait before a refresh is executed.")>
    Property AutoRefresh() As Integer
        Get
            Return gAutoRefresh
        End Get
        Set(ByVal value As Integer)
            gAutoRefresh = value
            If value = 0 Then
                tmrIdle.Enabled = False
            Else
                tmrIdle.Enabled = True
            End If
        End Set
    End Property

    Private gSelectionDelay As Integer = 0
    <Category("_Custom Properties")>
    <Description("Number of milliseconds to delay before sending selction change event.")>
    Property SelectionDelay() As Integer
        Get
            Return gSelectionDelay
        End Get
        Set(ByVal value As Integer)
            gSelectionDelay = value
        End Set
    End Property

    Private gAllowFilter As Boolean = True
    <Category("_Custom Properties")>
    <Description("Allow the user to access Filter Form")>
    Property AllowFilter() As Boolean
        Get
            Return gAllowFilter
        End Get
        Set(ByVal value As Boolean)
            gAllowFilter = value
        End Set
    End Property

    Private gPrimaryKeys As String()
    <System.ComponentModel.Category("_Custom Properties")>
    <System.ComponentModel.Description("Column Names (array) that make up the primary key.")>
    Property PrimaryKeys() As String()
        Get
            Return gPrimaryKeys
        End Get
        Set(value As String())
            gPrimaryKeys = value
        End Set
    End Property

    Private gAllowSort As Boolean = True
    <Category("_Custom Properties")>
    <Description("Allow the user to sort by clicking column headers")>
    Property AllowSort() As Boolean
        Get
            Return gAllowSort
        End Get
        Set(ByVal value As Boolean)
            If value Then
                dgvCTS.CustomSort = "CUSTOM"
            Else
                dgvCTS.CustomSort = "NOSORT"
            End If
            gAllowSort = value
            miSorting.Visible = value
            tsmiSortAscending.Visible = value
            tsmiSortDescending.Visible = value
        End Set
    End Property

    Private gAllowWrapping As Boolean = False
    <Category("_Custom Properties")>
    <Description("Allow row heights to be autosized for columns added to the wrappingcolumn collection")>
    Property AllowWrapping() As Boolean
        Get
            Return gAllowWrapping
        End Get
        Set(ByVal value As Boolean)
            gAllowWrapping = value
            miAllowWrapping.Checked = gAllowWrapping
            ApplyWrapping()
        End Set
    End Property


    Private gUpdateMethod As GridUpdateMethods
    <Category("_Custom Properties")>
    <Description("How to handle Grid Updates")>
    Property UpdateMethod() As GridUpdateMethods
        Get
            Return gUpdateMethod
        End Get
        Set(ByVal value As GridUpdateMethods)
            gUpdateMethod = value
        End Set
    End Property

    Private gShowBanner As Boolean = True
    <Category("_Custom Properties")>
    <Description("Show the title bar for the Data Grid")>
    Property ShowBanner() As Boolean
        Get
            Return gShowBanner
        End Get
        Set(ByVal value As Boolean)
            gShowBanner = value
        End Set
    End Property

    Private gShowStatusStrip As Boolean = True
    <Category("_Custom Properties")>
    <Description("Show the status strip at bottom of the control.")>
    Property ShowStatusStrip() As Boolean
        Get
            Return gShowStatusStrip
        End Get
        Set(ByVal value As Boolean)
            gShowStatusStrip = value
        End Set
    End Property

    Private gShowTotals As Boolean = False
    <Category("_Custom Properties")>
    <Description("Show the totals Grid.")>
    Property ShowTotals() As Boolean
        Get
            Return gShowTotals
        End Get
        Set(ByVal value As Boolean)
            gShowTotals = value

            'Set Totals checkbox in menu
            If gShowTotals = True Then
                miShowTotals.Checked = True
                miShowTotals.Text = "Hide Totals"
            Else
                miShowTotals.Checked = False
                miShowTotals.Text = "Show Totals"
            End If

        End Set
    End Property


    Private gMaxRows As Integer = 0
    <Category("_Custom Properties")>
    <Description("Maximum number of rows to retreive (0 means no max);")>
    Property MaxRows() As Integer
        Get
            Return gMaxRows
        End Get
        Set(ByVal value As Integer)
            gMaxRows = value
        End Set
    End Property

    Private gInternalWhere As String = String.Empty
    <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Property InternalWhere() As String
        Get
            Return gInternalWhere
        End Get
        Set(ByVal value As String)
            gInternalWhere = value
        End Set
    End Property


    Property Statement As String

#End Region

#Region "Loading and Refreshing"
    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Initialize()
    End Sub


    Private Sub Initialize()

        SuspendOff()
        Initializing = True

        GridSettingIsDirty = False
        lblGridStatus.Text = String.Empty

        If gAllowSort Then
            dgvCTS.CustomSort = "CUSTOM"
        Else
            dgvCTS.CustomSort = "NOSORT"
        End If

        lvwErrors.Visible = False

        pnlLoading.Visible = False
        pnlLoading.BackColor = dgvCTS.BackgroundColor

        tsBanner.Visible = ShowBanner
        ssBottom.Visible = ShowStatusStrip

        miFilters.Visible = AllowFilter
        tsmiColumnFilter.Visible = AllowFilter
        lblFiltered.Visible = False

        dgvCTS.AutoGenerateColumns = False

        dgvCTS.ColumnHeadersDefaultCellStyle.BackColor = GridPrimary
        dgvCTS.RowHeadersDefaultCellStyle.BackColor = GridPrimary
        dgvCTS.RowHeadersDefaultCellStyle.SelectionBackColor = GridPrimary
        dgvCTS.DefaultCellStyle.SelectionBackColor = SelectedBackColor
        dgvCTS.DefaultCellStyle.SelectionForeColor = Color.Black

        dgvTotals.ColumnHeadersDefaultCellStyle.BackColor = GridPrimary
        dgvTotals.RowHeadersDefaultCellStyle.BackColor = GridPrimary
        dgvTotals.RowHeadersDefaultCellStyle.SelectionBackColor = GridPrimary
        dgvTotals.DefaultCellStyle.BackColor = GridPrimary
        dgvTotals.DefaultCellStyle.SelectionBackColor = GridPrimary

        BackColor = GridSecondary
        'dgvTotals.BackgroundColor = GridSecondary
        dgvCTS.BackgroundColor = GridSecondary

        Initializing = False

        'Set Developer to provide for Developer-level options
        GH.SetDeveloperFlag()

    End Sub


    'Grid Loading
    Private Sub CTSGrid_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        gIsLoading = True

        If Me.Focused Then
            tsBanner.BackgroundImage = GridBackgroundImage
            ssBottom.BackgroundImage = GridBackgroundImage
        Else
            tsBanner.BackgroundImage = Nothing
            ssBottom.BackgroundImage = Nothing
        End If

        'Check permission for XLS export of grid
        ''Dim dr As DataRow = App.Data.DB2Chain("DNPF2530", String.Format("OFNAME = '{0}' and OFTYPE = '*EXPORTXLS'", Me.GridName))
        ''If dr IsNot Nothing Then
        ''    Dim Allowed As Boolean = App.HasPermission(dr("OFFGROUP"), dr("OFFUNCTION"))
        ''    If Allowed Then
        ''        tsmiExportToExcel.Visible = True
        ''    Else
        ''        tsmiExportToExcel.Visible = False
        ''    End If
        ''Else
        ''    tsmiExportToExcel.Visible = True
        ''End If

        gIsLoading = False
    End Sub

    ''' <summary>
    ''' Rebuild entire grid control from scratch. To just refresh data use the RefreshData method. 
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Reload()
        Initialize()
    End Sub

    ''' <summary>
    ''' Refresh data for this grid.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub RefreshData()
        RefreshData(False)
    End Sub


    ''' <summary>
    ''' Refresh ALL the configuration and data for this grid.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub RefreshData(updatesettings As Boolean)
        ts1.Reset()
        ts1.Start()

        'todo this may be a good thing... confirm 
        dgvCTS.CurrentCell = Nothing

        SuspendOn()
        'gIsLoading = True
        RaiseEvent dgv_GridLoadingStart(Me)

        'Hide Grid during refresh
        pnlLoading.Dock = DockStyle.Fill
        pnlLoading.BringToFront()
        pnlLoading.Visible = True
        dgvCTS.Visible = False
        pnlTotals.Visible = False

        'Clear Dirty/Change indicators 
        dgvCTS.SetState(False)

        'Set the filter icon to indicate filters exist
        If UFilters IsNot Nothing AndAlso UFilters.Conditions IsNot Nothing AndAlso UFilters.Conditions.Count > 0 Then
            lblFiltered.Visible = True
        Else
            lblFiltered.Visible = False
        End If

        'Clear error listview 
        lvwErrors.Items.Clear()
        lvwErrors.Visible = False

        If GridMode = GridModes.Update Then
            LoadMethod = GridLoadMethods.DataTable
            dgvCTS.TabToEditCell = True
        Else
            dgvCTS.TabToEditCell = False
        End If


        If ConnectionString = String.Empty Then
            If G IsNot Nothing AndAlso G.AltDataSource IsNot Nothing Then
                C2Data.Data = New CTS.Data(G.AltDataSource.SourceType, G.AltDataSource.Server, G.AltDataSource.Database, "", "")
            Else
                C2Data.Data.SetConnectionString(GridApp, gDataEnvironmentCTS)
            End If
        Else
            C2Data.Data.ConnectionString = ConnectionString
        End If


        lblGridStatus.Visible = False
        dgvCTS.Columns.Clear()
        Me.Refresh()

        'test for saved/db definition
        LoadDefinition()


        'If sql.Loaded Then
        LoadGridFromModel()
        'End If


        RefreshFinisher()
    End Sub

    Private Sub RefreshFinisher()

        dgvCTS.Visible = True

        pnlLoading.Visible = False
        pnlLoading.Dock = DockStyle.None

        LastRefresh = Now
        gDataLoaded = True

        SuspendOff()
        RaiseEvent dgv_GridLoadingComplete(Me)

        ts1.Stop()
    End Sub


    'flagged for removal
    'Private Function LoadGridFromModelx() As String
    '    Dim mySQL As String = String.Empty

    '    'removed from public
    '    Dim myFlds As String
    '    Dim myFrom As String
    '    Dim myWhere As String
    '    Dim myGroup As String

    '    'Set Grid Title 
    '    gTitle = G.Title
    '    If BannerText = String.Empty Then
    '        lblGridTitle.Text = gTitle
    '    End If

    '    '************************************************************************
    '    'Build select field string from the 2510 record adding columns to the data grid
    '    myFlds = ProcessColumns()

    '    '*************************************************************************
    '    'Build the From Clause
    '    myFrom = Replace(G.FromClause, "/", ".")

    '    '*************************************************************************
    '    'Build a where clause from the items stored in the filter array
    '    myWhere = ProcessWhereFilters()

    '    'gInternalWhere = G.WhereClause.Trim
    '    If Not String.IsNullOrWhiteSpace(gInternalWhere) AndAlso Not gInternalWhere.ToUpper.StartsWith("WHERE ") Then
    '        gInternalWhere = $"Where {gInternalWhere}"
    '    End If

    '    '*************************************************************************
    '    'Build the Group By Clause
    '    If G.GroupByClause <> String.Empty Then
    '        myGroup = $"Group By {G.GroupByClause}"
    '    Else
    '        myGroup = String.Empty
    '    End If

    '    'Allow possible changes before data request
    '    RaiseEvent dgv_GridPreDataRequest(Me)

    '    '*************************************************************************
    '    'Fill the datatable/grid using the resulting SQL
    '    If G.Distinct Then
    '        mySQL = $"Select * From (Select Distinct {myFlds} {myFrom} {InternalWhere} {myGroup}) myfile {myWhere}"
    '    Else
    '        mySQL = $"Select * From (Select {myFlds} {myFrom} {InternalWhere} {myGroup}) myfile {myWhere}"
    '    End If

    '    If MaxRows > 0 Then
    '        mySQL = $"{mySQL} fetch first {MaxRows} rows only"
    '    End If

    '    '*******************************************
    '    'Load the Grid based on LoadMethod property  
    '    '*******************************************
    '    If LoadMethod = GridLoadMethods.DataTable Then
    '        GetDataTable(mySQL)
    '        LoadDataTable(dtGridData)
    '        If ShowTotals Then SyncTotalsGrid()
    '    End If

    '    If LoadMethod = GridLoadMethods.DataReader Then
    '        LoadDataReader(mySQL)
    '    End If

    '    GH.SetDefaultSort(G, dgvCTS)

    '    HideColumns()

    '    SetGridMessage($"{dgvCTS.RowCount} records loaded")
    '    lblGridStatus.Visible = True

    '    Return mySQL
    'End Function

    Private Sub LoadGridFromModel()

        If G IsNot Nothing Then

            'Set Grid Title 
            If Not String.IsNullOrEmpty(G.Title) Then
                GridTitle = G.Title
            End If
            If BannerText = String.Empty Then
                lblGridTitle.Text = GridTitle
            End If

            'Grid Functions
            If G.GridFunctions.Count = 0 Then
                tsFunctions.Visible = False
            Else
                tsFunctions.Items.Clear()
                For Each GF As GridFunction In G.GridFunctions
                    Dim tsi = tsFunctions.Items.Add(GF.Text)
                    tsi.Name = GF.Name
                    tsi.Alignment = GF.Align
                    tsi.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText
                    tsi.Image = My.Resources.ResourceManager.GetObject(GF.ImageName.ToString)
                    tsi.ImageScaling = ToolStripItemImageScaling.None
                    tsi.Tag = GF
                Next
                tsFunctions.Visible = True
            End If

            'Grid Styles
            SetCellStyle(dgvCTS.DefaultCellStyle)

            BuildSelectStatment()

            'Allow possible changes before data request
            RaiseEvent dgv_GridPreDataRequest(Me)

            BuildSelectStatment()


            '*******************************************
            'Load the Grid based on LoadMethod property  
            '*******************************************
            Select Case LoadMethod
                Case GridLoadMethods.DataTable
                    GetDataTable(G.Statement)
                    LoadDataTable(dtGridData)

                Case GridLoadMethods.DataReader
                    LoadDataReader(G.Statement)
                    GH.SetDefaultSort(G, dgvCTS)
                    HideColumns()
                    SetGridMessage($"{dgvCTS.RowCount} records loaded")
                    lblGridStatus.Visible = True
            End Select

        End If

    End Sub

    Private Sub BuildSelectStatment()

        G.Columns.Clause = ProcessColumns()

        G.FromClause = Replace(G.FromClause, "/", ".")

        G.FilterClause = ProcessWhereFilters()

        gInternalWhere = G.WhereClause.Trim
        If Not String.IsNullOrWhiteSpace(gInternalWhere) AndAlso Not gInternalWhere.ToUpper.StartsWith("WHERE ") Then
            gInternalWhere = $"Where {gInternalWhere}"
        End If

        If Not String.IsNullOrWhiteSpace(G.GroupByClause) AndAlso Not G.GroupByClause.ToUpper.StartsWith("GROUP BY ") Then
            G.GroupByClause = $"Group By {G.GroupByClause}"
        End If

        If G.Distinct Then
            G._Statement = $"Select * From (Select Distinct {G.Columns.Clause} {G.FromClause} {InternalWhere} {G.GroupByClause}) myfile {G.FilterClause}"
        Else
            G._Statement = $"Select * From (Select {G.Columns.Clause} {G.FromClause} {InternalWhere} {G.GroupByClause}) myfile {G.FilterClause}"
        End If

        If MaxRows > 0 Then
            G._Statement = $"{G._Statement} fetch first {MaxRows} rows only"
        End If

    End Sub




    Private Function ProcessColumns() As String
        Dim myFlds As String = String.Empty
        FilterList.Clear()

        'Clear all security contraints, they will be rebuilt.
        ClearFilters(FilterType.Security)

        dgvCTS.Columns.Clear()

        For Each gc As GridColumn In G.Columns

            'Add new column to the "field" clause
            BuildSQLFields(gc.Field, gc.SQLFunction, gc.ProviderTypeCode, gc.CastCCSID37, gc.Size, gc.SuppressTrim, myFlds)

            GH.SetRowSecurityConstraint(gc.Field, gc.SecurityType)

            'When using the datareader you need to setup the columns manually
            If LoadMethod = GridLoadMethods.DataReader Or dgvCTS.AutoGenerateColumns = False Then
                SetupGridColumn(gc)
            End If

        Next

        Debug.Print("CTSGrid: " & myFlds)
        Return myFlds
    End Function

    'Put together the Where clause from each filter lists
    Private Function ProcessWhereFilters() As String
        Dim strWhere As String = String.Empty
        Dim UserWhere As String = BuildSQLWhere(UFilters)
        Dim ProgramWhere As String = BuildSQLWhere(PFilters)
        Dim SecurityWhere As String = BuildSQLWhere(SFilters)

        If SecurityWhere <> String.Empty Then
            If strWhere <> String.Empty Then
                strWhere = "(" & strWhere.Trim & ") AND (" & SecurityWhere.Trim & ")"
            Else
                strWhere = SecurityWhere
            End If
        End If

        If ProgramWhere <> String.Empty Then
            If strWhere <> String.Empty Then
                strWhere = "(" & strWhere.Trim & ") AND (" & ProgramWhere.Trim & ")"
            Else
                strWhere = ProgramWhere
            End If
        End If

        If UserWhere <> String.Empty Then
            If strWhere <> String.Empty Then
                strWhere = "(" & strWhere.Trim & ") AND (" & UserWhere.Trim & ")"
            Else
                strWhere = UserWhere
            End If
        End If

        If strWhere <> String.Empty Then
            strWhere = "Where " & strWhere
        End If

        Debug.Print("CTSGrid: " & strWhere)
        Return strWhere
    End Function

    'This routine MUST be after data binding...
    Private Sub HideColumns()
        Dim Visible As Boolean

        For Each gc In G.Columns
            Visible = True

            If gc.Restrict Then
                Visible = False
            End If

            If Visible = True Then
                Visible = Not gc.Hidden
            End If

            dgvCTS.Columns(gc.Field).Visible = Visible
            If miShowTotals.Checked Then
                dgvTotals.Columns(gc.Field).Visible = Visible
            End If
        Next

    End Sub






    'Load the resulting SQL Data Table into the grid
    Private Sub GetDataTable(ByVal strSQL As String)

        ts2.Reset()
        ts2.Start()
        Try
            dtGridData = C2Data.Data.GetTable(strSQL)
        Catch ex As Exception
            MsgBox(ex.Message & vbNewLine & vbNewLine & ex.StackTrace & vbNewLine & vbNewLine & strSQL, , "GetDataTable(" & GridName.Trim & ")")
        End Try


        'Dim Thread = New System.Threading.Thread(Sub() dtGridData = App.Data.GetTable(strSQL))
        'Thread.Start()

        ts2.Stop()

    End Sub

    Private Sub LoadDataTable(ByVal dtGridData As DataTable)

        ts3.Reset()
        ts3.Start()

        dgvCTS.DataSource = dtGridData
        Me.Refresh()

        ts3.Stop()

        'Apply the ValueType
        For Each col In dgvCTS.Columns
            Dim gc As GridColumn = G.Columns.Item(col.name)
            col.ValueType = System.Type.GetType(GetDataTypeFromProvider(gc.ProviderTypeCode))
        Next

        'If columns were auto-created then configure based on Grid Model
        If dgvCTS.AutoGenerateColumns = True Then
            For Each col In dgvCTS.Columns
                Dim gc As GridColumn = G.Columns(col.name)
                If gc IsNot Nothing Then
                    ConfigureGridColumn(col, gc)
                End If
            Next
        End If

        If ShowTotals Then SyncTotalsGrid()
        GH.SetDefaultSort(G, dgvCTS)
        HideColumns()

        SetGridMessage($"{dgvCTS.RowCount} records loaded")
        lblGridStatus.Visible = True

    End Sub




    '*****************************commented out b/c of GRID 2 issues ***********************

    'Load the resulting SQL Data Reader into the grid
    Private Sub LoadDataReader(ByVal strSQL As String)
        'dgvCTS.DataSource = Nothing
        'GridConnString = GH.ConnectionString
        'Using conn As New iDB2Connection(GridConnString)
        '    conn.Open()

        '    Dim cmd As New iDB2Command(strSQL, CommandType.Text, conn)
        '    Dim dr As iDB2DataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)

        '    ' load all records from the data reader
        '    If dr.HasRows Then
        '        Do While dr.Read()

        '            dgvCTS.Rows.Add()
        '            For Each col In dgvCTS.Columns

        '                Dim LastIdx As Integer = dgvCTS.Rows.Count - 1

        '                If IsDBNull(dr.Item(col.Name)) Then
        '                    dgvCTS.Rows(LastIdx).Cells(col.Index).Value = String.Empty
        '                Else
        '                    dgvCTS.Rows(LastIdx).Cells(col.Index).Value = dr.Item(col.Name).trim)
        '                End If

        '                Select Case col.CellType.Name
        '                    Case "DataGridViewTextBoxCell"
        '                    Case "DataGridViewComboBoxCell"
        '                End Select

        '            Next
        '            lblGridStatus.Text = dgvCTS.RowCount & " records loaded"
        '            ssBottom.Invalidate()
        '        Loop
        '    End If

        'End Using
    End Sub






#Region "DataGridView Column Setup"

    'Setup Grid Columns (when data is not bound)
    Private Sub ConfigureGridColumn(ByRef col As DataGridViewColumn, gc As GridColumn)

        col.HeaderText = gc.HeaderText
        col.ReadOnly = If(gc.Updatable And gMode = GridModes.Update, False, True)
        If gc.Width > 0 Then col.Width = gc.Width

        Select Case gc.Alignment
            Case GeneralAlignment.Center
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.BottomCenter
                If gc.ProviderTypeCode = "13" Then
                    col.DefaultCellStyle.Format = "hh:mm:ss tt"
                End If
            Case GeneralAlignment.Right
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.BottomRight
            Case GeneralAlignment.Left
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.BottomLeft
        End Select


    End Sub

    'Setup Grid Columns (when data is not bound)
    Private Sub SetupGridColumn(gc As GridColumn)

        ' add column to the grid based on column type
        Select Case gc.ColumnType
            Case GridColumnTypes.TEXT
                SetupTextColumn(gc)

            Case GridColumnTypes.MASKED
                SetupMaskedColumn(gc)

            Case GridColumnTypes.CUSTOMPROMPT
                SetupPromptColumn(gc)

            Case GridColumnTypes.CALENDAR
                SetupCalendarColumn(gc)

            Case GridColumnTypes.COMBOBOX_SQL, GridColumnTypes.COMBOBOX_VAL
                SetupComboColumn(gc)
        End Select

        'Fill the filter values structure
        If gc.ColumnType = GridColumnTypes.COMBOBOX_VAL And gc.ValueList IsNot Nothing Then
            AddColumnVAL(gc)
        End If

        'Fill the filter values structure
        If gc.ColumnType = GridColumnTypes.COMBOBOX_SQL And gc.ValueTable IsNot Nothing Then
            AddColumnSQL(gc)
        End If

    End Sub

    Private Sub SetupTextColumn(ByVal gc As GridColumn)
        Dim Col As New DataGridViewColumnCTS With {
            .Name = gc.Field,
            .DataPropertyName = gc.Field,
            .HeaderText = IIf(gc.HeaderText = String.Empty, gc.Field, gc.HeaderText),
            .ValueType = System.Type.GetType(GetDataTypeFromProvider(gc.ProviderTypeCode)),
            .DB2ProviderType = gc.ProviderTypeCode,
            .DB2Size = gc.Size,
            .DB2Scale = gc.Scale,
            .CTSPreSelect = gc.Preselect,
            .CTSSumCol = gc.SumColumn
        }

        If gc.Width > 0 Then Col.Width = gc.Width

        ' Determine if column is editable
        If gc.Updatable And gMode = GridModes.Update Then
            Col.ReadOnly = False
        Else
            Col.ReadOnly = True
        End If

        If gc.ProviderTypeCode = "13" Then
            Col.DefaultCellStyle.Format = "hh:mm:ss tt"
        End If

        If gc.Format <> String.Empty Then
            Col.DefaultCellStyle.Format = gc.Format
            Col.DefaultCellStyle.NullValue = ""
        End If

        If gc.SumFormat <> String.Empty Then
            Col.CTSSumFormat = gc.SumFormat
        Else
            Col.CTSSumFormat = gc.Format
        End If

        'Allow wrapping on added columns 
        If AllowWrapping Then
            Col.DefaultCellStyle.WrapMode = DataGridViewTriState.True
        End If

        ' column alignment
        Select Case gc.Alignment
            Case GeneralAlignment.Center
                Col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                Col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.BottomCenter
            Case GeneralAlignment.Right
                Col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                Col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.BottomRight
            Case GeneralAlignment.Left
                Col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                Col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.BottomLeft
        End Select

        dgvCTS.Columns.Add(Col)
    End Sub

    Private Sub SetupMaskedColumn(ByVal gc As GridColumn)
        Dim col As New DataGridViewMaskedEditColumn With {
            .Name = gc.Field,
            .DataPropertyName = gc.Field,
            .HeaderText = IIf(gc.HeaderText = String.Empty, gc.Field, gc.HeaderText),
            .ValueType = System.Type.GetType(GetDataTypeFromProvider(gc.ProviderTypeCode)),
            .DB2ProviderType = gc.ProviderTypeCode,
            .DB2Size = gc.Size,
            .DB2Scale = gc.Scale,
            .CTSPreSelect = gc.Preselect
        }
        If gc.Width > 0 Then col.Width = gc.Width

        ' Determine if column is editable
        If gc.Updatable And gMode = GridModes.Update Then
            col.ReadOnly = False
        Else
            col.ReadOnly = True
        End If

        If gc.ProviderTypeCode = "13" Then
            col.DefaultCellStyle.Format = "hh:mm:ss tt"
        End If

        ' column alignment
        Select Case gc.Alignment
            Case GeneralAlignment.Center
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.BottomCenter
            Case GeneralAlignment.Right
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.BottomRight
            Case GeneralAlignment.Left
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.BottomLeft
        End Select
        dgvCTS.Columns.Add(col)

    End Sub

    Private Sub SetupCalendarColumn(ByVal gc As GridColumn)
        Dim calCol As New CalendarColumn With {
            .Name = gc.Field,
            .DataPropertyName = gc.Field,
            .HeaderText = IIf(gc.HeaderText = String.Empty, gc.Field, gc.HeaderText),
            .DB2ProviderType = gc.ProviderTypeCode,
            .DB2Size = gc.Size,
            .DB2Scale = gc.Scale
        }

        ' Determine if column is editable
        If gc.Updatable And gMode = GridModes.Update Then
            calCol.ReadOnly = False
        Else
            calCol.ReadOnly = True
        End If

        dgvCTS.Columns.Add(calCol)
    End Sub

    Private Sub SetupPromptColumn(gc As GridColumn)
        Dim col As New PromptColumn With {
            .PromptType = UCase(gc.ColumnType.ToString),
            .Name = gc.Field,
            .DataPropertyName = gc.Field,
            .HeaderText = IIf(gc.HeaderText = String.Empty, gc.Field, gc.HeaderText),
            .ValueType = System.Type.GetType(GetDataTypeFromProvider(gc.ProviderTypeCode)),
            .DB2ProviderType = gc.ProviderTypeCode,
            .DB2Size = gc.Size,
            .DB2Scale = gc.Scale,
            .CTSPreSelect = gc.Preselect
        }
        If gc.Width > 0 Then col.Width = gc.Width

        ' Determine if column is editable
        If gc.Updatable And gMode = GridModes.Update Then
            col.ReadOnly = False
        Else
            col.ReadOnly = True
        End If

        If gc.ProviderTypeCode = "13" Then
            col.DefaultCellStyle.Format = "hh:mm:ss tt"
        End If

        ' column alignment
        Select Case gc.Alignment
            Case GeneralAlignment.Center
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.BottomCenter
            Case GeneralAlignment.Right
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.BottomRight
            Case GeneralAlignment.Left
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.BottomLeft
        End Select

        dgvCTS.Columns.Add(col)
    End Sub

    Private Sub SetupComboColumn(gc As GridColumn)
        Dim col As New DataGridViewComboBoxColumnCTS With {
            .Name = gc.Field,
            .DataPropertyName = gc.Field,
            .HeaderText = IIf(gc.HeaderText = String.Empty, gc.Field, gc.HeaderText),
            .DB2ProviderType = gc.ProviderTypeCode,
            .DB2Size = gc.Size,
            .DB2Scale = gc.Scale
        }

        ' Determine if column is editable
        If gc.Updatable And gMode = GridModes.Update Then
            col.ReadOnly = False
        Else
            col.ReadOnly = True
        End If

        col.DisplayStyleForCurrentCellOnly = True

        If gc.ColumnType = GridColumnTypes.COMBOBOX_SQL And gc.ValueTable IsNot Nothing Then
            Dim PullDownDR As DataRow = gc.ValueTable.NewRow
            gc.ValueTable.Rows.InsertAt(PullDownDR, 0)
            col.DataSource = gc.ValueTable
            col.DisplayMember = "DisplayValue"
        End If

        If gc.ColumnType = GridColumnTypes.COMBOBOX_VAL And gc.ValueList IsNot Nothing Then
            col.Items.AddRange(gc.ValueList)
        End If


        ' column alignment
        Select Case gc.Alignment
            Case GeneralAlignment.Center
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.BottomCenter
            Case GeneralAlignment.Right
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.BottomRight
            Case GeneralAlignment.Left
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.BottomLeft
        End Select

        dgvCTS.Columns.Add(col)
    End Sub

#End Region

#Region "Column Values (filtering)"

    Private Sub AddColumnVAL(gc As GridColumn)

        If gc.ValueList.Count > 0 Then

            Dim cfv As New ColumnFilterValues
            cfv.FieldName = gc.Field
            For Each Value In gc.ValueList
                Dim fv As New FilterValue
                fv.Value = Value
                Dim cb As New CheckBox
                cb.BackColor = Color.Transparent
                cb.Text = Value
                fv.CB = cb
                cfv.Values.Add(fv)
            Next

            FilterList.Add(cfv)
            SyncColumnFilters(cfv)

        End If
    End Sub

    Private Sub AddColumnSQL(gc As GridColumn)
        If gc.ValueTable.Rows.Count > 0 Then

            Dim cfv As New ColumnFilterValues
            cfv.FieldName = gc.Field
            For Each vRow In gc.ValueTable.Rows
                If Not IsDBNull(vRow.Item("DisplayValue")) Then
                    Dim fv As New FilterValue
                    fv.Value = vRow.Item("DisplayValue").ToString
                    Dim cb As New CheckBox
                    cb.BackColor = Color.Transparent
                    cb.Text = vRow.Item("DisplayValue").ToString
                    fv.CB = cb
                    cfv.Values.Add(fv)
                End If
            Next

            FilterList.Add(cfv)
            SyncColumnFilters(cfv)

        End If
    End Sub

    Private Sub SyncColumnFilters(cfv As ColumnFilterValues)

        'Check for existing "IN" filters 
        'Dim result As Filtering.GridFilter = GetFilter(UserFilters, cfv.FieldName, "IN")

        Dim result = UFilters.Conditions.Subset(cfv.FieldName, OperatorName.In)
        If result IsNot Nothing AndAlso result.Count > 0 Then

            For Each Value In result(0).Values
                Dim Found As Boolean = False

                For Each fv As FilterValue In cfv.Values
                    If fv.CB.Text = Value Then
                        fv.CB.Checked = True
                        fv.Selected = True
                        Found = True
                        Exit For
                    End If
                Next

                If Not Found Then
                    Dim fv As New FilterValue
                    fv.Value = Value.Trim
                    fv.Selected = True

                    Dim cb As New CheckBox
                    cb.BackColor = Color.Transparent
                    cb.Text = Value.Trim
                    cb.Checked = True
                    fv.CB = cb
                    cfv.Values.Add(fv)
                End If

            Next
        End If

    End Sub

    'todo great idea... finish it
    Public Sub RefreshColumnFilterValues(ByVal ColumnName As String, ValueList As List(Of String))
        Dim flist = FilterList.Find(Function(x) x.FieldName = ColumnName)
        If flist IsNot Nothing Then
            FilterList.Remove(flist)
        End If

        Dim gc = G.Columns(ColumnName)
        If gc IsNot Nothing Then
            gc.ValueList = ValueList
            AddColumnVAL(gc)
        End If
    End Sub

#End Region


#End Region



#Region "Load SQL"
    Public Sub LoadStatement(ByVal Statement As String)

        gID = 0
        gOverrideVersion = 0 = 0
        gName = String.Empty
        sql = New SQL(gApp, gDataEnvironment, Statement)
    End Sub

    Public Function ImportSQL(ByVal Statement As String) As Grid
        Statement = Statement.Replace(vbCrLf, "")
        Statement = Statement.Replace(vbLf, "")

        gID = 0
        gOverrideVersion = 0
        gName = String.Empty

        sql = Nothing
        sql = New SQL(gApp, gDataEnvironment, Statement)

        G = New Grid
        G.Distinct = sql.Distinct
        G.FromClause = sql.FromString
        G.Columns = ImportSQLColumns(sql.ColumnList)
        G.GroupByClause = sql.GetClause("group by")
        G.WhereClause = sql.GetClause("where")
        Dim OrderByString = sql.GetClause("order by")
        ApplySort(OrderByString, G.Columns)

        If Not Statement.Equals(SaveSQL) Then
            'check for saved column overrides
            If True Then
                Dim s = System.AppDomain.CurrentDomain.FriendlyName
                Dim c = Me.Name

            End If

            'check for saved filter overrides
            SaveSQL = Statement
        End If

        Return G
    End Function

    Private Sub ApplySort(ByVal OrderByString As String, ByRef GridColumns As GridColumnCollection)
        Dim cols As String() = OrderByString.Trim.Split(",")
        Dim Idx As Short = 0

        For Each col In cols
            Dim ColName As String = String.Empty
            Dim SortOrder As SortOrder = SortOrder.Ascending

            Dim advsort As String() = col.Trim.Split(" ")
            If advsort.Length = 1 Then
                ColName = advsort(0)

            ElseIf advsort.Length >= 2 Then
                ColName = advsort(0)
                If advsort(1) = "DESC" Then
                    SortOrder = SortOrder.Descending
                End If
            End If

            'todo this bombs if column not found
            Try
                If Not String.IsNullOrEmpty(ColName) Then
                    Dim gc As GridColumn = GridColumns(ColName.Trim.ToUpper)
                    If gc IsNot Nothing Then
                        Idx += 1
                        gc.SortSeq = Idx
                        gc.SortOrder = SortOrder.Ascending
                    End If
                End If

            Catch ex As Exception

            End Try
        Next

    End Sub

    Public Function ImportSQLColumns(SqlColumnList As List(Of SqlColumn)) As GridColumnCollection
        Dim GColumns As New GridColumnCollection

        Dim idx As Short = 0
        For Each sqlc In SqlColumnList
            Dim gcol As New GridColumn
            If String.IsNullOrEmpty(sqlc.ColumnAlias) Then
                gcol.Name = sqlc.Name
                gcol.Field = sqlc.Name
            Else
                gcol.Name = sqlc.ColumnAlias
                gcol.Field = sqlc.ColumnAlias
            End If
            gcol.SQLFunction = sqlc.ColFunction
            gcol.Text = sqlc.Text
            gcol.HeaderText = sqlc.HeaderText
            gcol.ProviderTypeCode = sqlc.ProviderType
            Select Case sqlc.ProviderType
                Case "6", "12", "13", "14"
                    gcol.Alignment = GeneralAlignment.Left
                Case Else
                    gcol.Alignment = GeneralAlignment.Right
            End Select
            gcol.Size = sqlc.Size
            gcol.Scale = sqlc.Scale
            gcol.Seq = idx
            GColumns.Add(gcol)
            idx += 1
        Next

        Return GColumns
    End Function
#End Region



#Region "Load Definition From DB"

    Private Sub LoadDefinition()

        If gID = 0 And gName <> String.Empty Then
            'Load by name
            Dim row = C2App.Data.GetRow($"Select GRIDID, VersionID From DNGRIDS where Gridname = '{gName}'")
            If row IsNot Nothing Then
                gID = row("GridID")
                If Not IsDBNull(row("VersionID")) Then
                    gVersion = row("VersionID")
                End If
            End If

        ElseIf gID > 0 Then
            'load by ID
            If gOverrideVersion = 0 Then
                gVersion = GH.GetDefaultVersion(gID)
            Else
                gVersion = gOverrideVersion
            End If
            Dim row = C2App.Data.GetRow($"select GRIDNAME From DNGRIDS where GridID = {gID} and VersionID = {gVersion} ")
            If row IsNot Nothing Then
                gName = row("GRIDNAME")
            End If
        End If

        'first time processing for gID
        If gID > 0 And gID <> SaveGridID Then
            LoadDefinitionfromDB(gID, gVersion, gName)

            'TODO !!! GRIDID/RELEASE
            GH.SaveUserFilters(UFilters, "*CURRENT", gName, "*LAST")
            SaveGridID = gID

            If GridMode = GridModes.Update Then
                'dgvCTS.PositionToFirstEditableCell()
            End If
        End If

    End Sub

    Private Sub LoadDefinitionfromDB(GridID As Integer, ReleaseID As Integer, GridName As String)

        lblGridTitle.ToolTipText = GridName

        If GridSettingIsDirty Then
            GridSettingIsDirty = False
            GH.SaveGridSettings(dgvCTS, GridID, ReleaseID, GridName)
        End If

        Try
            'Fill Grid Model with Grid specifications
            LoadModelFromDB(GridID, ReleaseID, GridName)

            'If an app is not specified then use the value from the grid table then reset conn string
            If GridApp = String.Empty Then
                Dim strDftApp As String = G.LibraryList
                If strDftApp <> String.Empty Then
                    GridApp = strDftApp
                    C2App.Data.SetConnectionString(GridApp, gDataEnvironmentCTS)
                End If
            End If

        Catch exc As Exception
            MsgBox(exc.Message & vbNewLine & vbNewLine & exc.StackTrace, , "LoadDefinitionfromDB(" & GridName.Trim & ")")
        End Try

    End Sub

    Private Sub LoadModelFromDB(GridID As Integer, ReleaseID As Integer, GridName As String)

        If GridID > 0 Then

            'Get Grid Definition  
            G = GH.LoadGridHeader(GridID, ReleaseID)
            G.Columns = GH.LoadGridColumns(GridID, ReleaseID)

            'Create RRN column(s)
            GH.LoadRRNColumns(G)

            'Load the right-click options for the row header menu
            G.RowOptions = GH.LoadCustomOptions(GridID, ReleaseID)
            SetRowOptionVisibility()

            'Load the right-click options for the row header menu
            G.GridFunctions = GH.LoadCustomFunctions(GridID, ReleaseID)
            SetGridFunctionVisibility()

            'Get Custom Styles 
            G.DynamicStyles = GH.LoadDynamicStyles(GridID, ReleaseID)
            SetCellStyle(dgvCTS.DefaultCellStyle)


            'Save the internally set where in a property (that can be overridden)
            gInternalWhere = G.WhereClause

            'Load Saved Filters on first load
            If SaveGridID <> GridID Then
                GH.LoadUserFilters(UFilters, "*CURRENT", GridName, "*LAST")
            End If

        End If

    End Sub

#End Region


#Region "Refresh/Add Loaded Datatable"

    ''' <summary>
    ''' Refresh the data for the row given.  PrimaryKeys property must be set for the update to occur.
    ''' </summary>
    ''' <param name="RowIndex">The index of the row to be updated.</param>
    ''' <remarks></remarks>
    Public Sub RefreshRow(ByVal RowIndex As Integer)

        If RowIndex >= 0 And RowIndex < Me.dgvCTS.Rows.Count Then

            If gPrimaryKeys IsNot Nothing AndAlso gPrimaryKeys.GetLength(0) > 0 Then

                Using dgvr As DataGridViewRow = Me.dgvCTS.Rows(RowIndex)

                    Dim TempFilter As New Filtering.Filter
                    For Each s As String In gPrimaryKeys
                        If Me.dgvCTS.Columns.Contains(s) Then
                            Dim c As Filtering.Condition = TempFilter.AddFilter(s, OperatorName.Equals, dgvr.Cells(s).Value.trim)
                            'c.valuelist = New List(Of String)
                        End If
                    Next

                    Dim strWhere = BuildSQLWhere(TempFilter)
                    Dim drNew As DataRow = C2App.Data.GetRow(String.Format("select {0} {1} Where {2} {3}", G.Columns.Clause, G.FromClause, strWhere, G.GroupByClause))
                    RefreshRow(RowIndex, drNew)
                End Using
            End If
        End If
    End Sub

    'Public Sub RefreshRowX(ByVal RowIndex As Integer)

    '    If RowIndex >= 0 And RowIndex < Me.dgvCTS.Rows.Count Then

    '        If gPrimaryKeys IsNot Nothing AndAlso gPrimaryKeys.GetLength(0) > 0 Then

    '            Using dgvr As DataGridViewRow = Me.dgvCTS.Rows(RowIndex)
    '                Dim TempFilters As New List(Of Filtering.GridFilter)

    '                For Each s As String In gPrimaryKeys
    '                    If Me.dgvCTS.Columns.Contains(s) Then
    '                        Dim NewFilter As New Filtering.GridFilter
    '                        NewFilter.ValueList = New List(Of String)
    '                        NewFilter.AO = "AND"
    '                        NewFilter.FieldName = s.Trim
    '                        NewFilter.OpCode = "="
    '                        NewFilter.Value = dgvr.Cells(s).Value.trim
    '                        NewFilter.Value2 = String.Empty
    '                        NewFilter.Fldt = Me.dgvCTS.Columns(s).ValueType.FullName
    '                        NewFilter.Hidden = False
    '                        NewFilter.Protect = False
    '                        TempFilters.Add(NewFilter)
    '                    End If
    '                Next

    '                Dim strWhere = BuildSQLWhere(TempFilters)
    '                Dim drNew As DataRow = App.Data.GetRow(String.Format("select {0} {1} Where {2} {3}", G.Columns.Clause, G.FromClause, strWhere, G.GroupByClause))
    '                RefreshRow(RowIndex, drNew)
    '            End Using
    '        End If
    '    End If
    'End Sub

    ''' <summary>
    ''' Refresh the data for row at given index with values from given row.  PrimaryKeys property not required.
    ''' </summary>
    ''' <param name="RowIndex">The index of the row to be updated.</param>
    ''' <param name="NewRow">The datarow object containing new values.  Columns with same Name will be updated.</param>
    ''' <remarks></remarks>
    Public Sub RefreshRow(ByVal RowIndex As Integer, ByVal NewRow As DataRow)

        If RowIndex >= 0 And RowIndex < Me.dgvCTS.Rows.Count Then

            Dim dgvr As DataGridViewRow = Me.dgvCTS.Rows(RowIndex)

            If NewRow IsNot Nothing Then

                'Trim columns 
                NewRow.TrimColumns

                'For each DataGridViewCell apply datarow value
                Dim cname As String
                For Each c As DataGridViewCell In dgvr.Cells
                    cname = c.OwningColumn.DataPropertyName
                    If NewRow.Table.Columns.Contains(cname) Then
                        If IsDBNull(NewRow(cname)) Then
                            c.Value = DBNull.Value
                        Else
                            c.Value = NewRow(cname)
                        End If
                    End If
                Next

                Dim drv As DataRowView = dgvr.DataBoundItem
                drv.Row.AcceptChanges()

                dgvCTS.InvalidateRow(RowIndex)
            End If
        End If

    End Sub

    ''' <summary>
    ''' Add new row to the grid.  PrimaryKeys property must be set.
    ''' </summary>
    ''' <param name="KeyValues">Array with key values for the new row.  Key Values should be in sequence with PrimaryKeys property.</param>
    ''' <remarks></remarks>
    Public Sub AddRow(ByVal KeyValues As String())

        If gPrimaryKeys IsNot Nothing AndAlso gPrimaryKeys.Length > 0 Then
            If KeyValues IsNot Nothing AndAlso KeyValues.Length > 0 Then
                If KeyValues.Length = gPrimaryKeys.Length Then

                    Dim TempFilter As New Filtering.Filter

                    For i = 0 To gPrimaryKeys.GetUpperBound(0)
                        If Me.dgvCTS.Columns.Contains(gPrimaryKeys(i)) Then
                            Dim c As Filtering.Condition = TempFilter.AddFilter(gPrimaryKeys(i).Trim, OperatorName.Equals, KeyValues(i).Trim)
                            'NewFilter.ValueList = New List(Of String)
                        End If
                    Next

                    Dim strWhere = BuildSQLWhere(TempFilter)
                    Dim drNew As DataRow = C2App.Data.GetRow(String.Format("select {0} {1} Where {2} {3}", G.Columns.Clause, G.FromClause, strWhere, G.GroupByClause))
                    AddRow(drNew)

                End If
            End If
        End If

    End Sub

    'Public Sub AddRow(ByVal KeyValues As String())

    '    If gPrimaryKeys IsNot Nothing AndAlso gPrimaryKeys.Length > 0 Then
    '        If KeyValues IsNot Nothing AndAlso KeyValues.Length > 0 Then
    '            If KeyValues.Length = gPrimaryKeys.Length Then

    '                Dim TempFilters As New List(Of Filtering.GridFilter)

    '                For i = 0 To gPrimaryKeys.GetUpperBound(0)
    '                    If Me.dgvCTS.Columns.Contains(gPrimaryKeys(i)) Then
    '                        Dim NewFilter As New Filtering.GridFilter
    '                        NewFilter.ValueList = New List(Of String)
    '                        NewFilter.AO = "AND"
    '                        NewFilter.FieldName = gPrimaryKeys(i).Trim
    '                        NewFilter.OpCode = "="
    '                        NewFilter.Value = KeyValues(i).Trim
    '                        NewFilter.Value2 = String.Empty
    '                        NewFilter.Fldt = Me.dgvCTS.Columns(gPrimaryKeys(i)).ValueType.FullName
    '                        TempFilters.Add(NewFilter)
    '                    End If
    '                Next

    '                Dim strWhere = BuildSQLWhere(TempFilters)
    '                Dim drNew As DataRow = App.Data.GetRow(String.Format("select {0} {1} Where {2} {3}", G.Columns.Clause, G.FromClause, strWhere, G.GroupByClause))
    '                AddRow(drNew)

    '            End If
    '        End If
    '    End If

    'End Sub

    ''' <summary>
    ''' Add new row to the grid.  PrimaryKeys property not required.
    ''' </summary>
    ''' <param name="NewRow">The datarow object containing the new row values.</param>
    ''' <remarks></remarks>
    Public Sub AddRow(ByVal NewRow As DataRow)

        If NewRow IsNot Nothing Then
            NewRow.TrimColumns

            Dim dtSource As DataTable = Me.dgvCTS.DataSource
            Dim drNewSourceRow As DataRow = dtSource.NewRow

            'For each column in datarow set DataGridViewCell
            For Each c As DataColumn In drNewSourceRow.Table.Columns
                If NewRow.Table.Columns.Contains(c.ColumnName) Then
                    drNewSourceRow(c.ColumnName) = NewRow(c.ColumnName)
                End If
            Next

            dtSource.Rows.Add(drNewSourceRow)
            dtSource.AcceptChanges()

        End If

    End Sub

#End Region


#Region "Runtime painting"

    Private Sub dgvCTS_RowPrePaint(sender As Object, e As DataGridViewRowPrePaintEventArgs) Handles dgvCTS.RowPrePaint

        ApplyDynamicStyle(e.RowIndex)

    End Sub

    Private Sub ApplyDynamicStyle(ByVal RowIndex As Integer)

        If G.DynamicStyles IsNot Nothing Then
            For Each item As DynamicStyle In G.DynamicStyles
                If item.Style IsNot Nothing Then
                    If Not IsDBNull(dgvCTS.Rows(RowIndex).Cells(item.FieldName).Value) Then
                        If item.Op = OperatorName.Equals AndAlso dgvCTS.Rows(RowIndex).Cells(item.FieldName).Value = item.Value _
                        OrElse item.Op = OperatorName.NotEquals AndAlso dgvCTS.Rows(RowIndex).Cells(item.FieldName).Value <> item.Value _
                        OrElse item.Op = OperatorName.GreaterThan AndAlso dgvCTS.Rows(RowIndex).Cells(item.FieldName).Value > item.Value _
                        OrElse item.Op = OperatorName.GreaterThanOrEqual AndAlso dgvCTS.Rows(RowIndex).Cells(item.FieldName).Value >= item.Value _
                        OrElse item.Op = OperatorName.LessThan AndAlso dgvCTS.Rows(RowIndex).Cells(item.FieldName).Value < item.Value _
                        OrElse item.Op = OperatorName.LessThanOrEqual AndAlso dgvCTS.Rows(RowIndex).Cells(item.FieldName).Value <= item.Value Then
                            Select Case item.ApplyType
                                Case StyleApplyType.Row
                                    dgvCTS.Rows(RowIndex).DefaultCellStyle = item.Style
                                Case StyleApplyType.Cell
                                    dgvCTS.Rows(RowIndex).Cells(item.ApplyCellName).Style = item.Style
                            End Select
                        End If
                    End If
                End If
            Next
        End If

    End Sub

    Private Sub dgvCTS_CellPainting(sender As Object, e As DataGridViewCellPaintingEventArgs) Handles dgvCTS.CellPainting


        'Paint the Column Headers
        If e.RowIndex = -1 And e.ColumnIndex > -1 Then
            Dim Sorted As Boolean = False
            Dim Filtered As Boolean = False

            Dim SortImage As Image = Nothing
            Dim FilterImage As Image = Nothing
            Dim HOffset As Integer = 0
            Dim VOffset As Integer = 0

            Dim col As DataGridViewColumn = dgvCTS.Columns(e.ColumnIndex)
            Dim txt As String = col.HeaderText


            If UFilters.Conditions.ContainsColumnName(col.Name) Then
                FilterImage = My.Resources.LimeDogear
                Filtered = True
            End If

            'todo filter updates.... flagged for removal
            'If GetColFilterCount(col.Name) > 0 Then
            '    FilterImage = My.Resources.LimeDogear
            '    Filtered = True
            'End If

            If dgvCTS.CustomSort = "DATAGRIDVIEW" Then
                If dgvCTS.SortedColumn IsNot Nothing AndAlso dgvCTS.SortedColumn.Index = e.ColumnIndex Then
                    Sorted = True
                    Select Case dgvCTS.SortOrder
                        Case SortOrder.Ascending
                            SortImage = My.Resources.uparrow
                        Case SortOrder.Descending
                            SortImage = My.Resources.downarrow
                    End Select
                End If
            End If

            If dgvCTS.CustomSort = "CUSTOM" Then

                Select Case dgvCTS.CustomSorter(e.ColumnIndex).Order
                    Case "ASC"
                        Sorted = True
                        SortImage = My.Resources.uparrow

                    Case "DESC"
                        Sorted = True
                        SortImage = My.Resources.downarrow
                End Select
            End If

            If Not Sorted And Not Filtered Then
                e.Handled = False
            Else
                'Debug.Print("CTSGrid: cellpainting r" & e.RowIndex & " c" & e.ColumnIndex)
                'Allow room for sort image 
                If Sorted Then
                    HOffset = SortImage.Width
                End If

                'Allow room for filtered image
                If Filtered Then
                    HOffset = HOffset + FilterImage.Width
                End If

                Dim TextRect As Rectangle = New Rectangle(e.CellBounds.X + 3, e.CellBounds.Y + 1, e.CellBounds.Width - HOffset - 3, e.CellBounds.Height - 3)

                ' Create solid brush and fill rectangle
                Dim CooperBrush As New SolidBrush(GridPrimary)
                e.Graphics.FillRectangle(CooperBrush, e.CellBounds.X, e.CellBounds.Y, e.CellBounds.Width - 1, e.CellBounds.Height - 1)


                Try
                    ' Specify the text is wrapped. 
                    Dim flags As TextFormatFlags = TextFormatFlags.WordBreak

                    Select Case col.HeaderCell.Style.Alignment
                        Case DataGridViewContentAlignment.BottomRight
                            flags = flags + TextFormatFlags.Bottom
                            flags = flags + TextFormatFlags.Right

                        Case DataGridViewContentAlignment.BottomLeft
                            flags = flags + TextFormatFlags.Bottom
                            flags = flags + TextFormatFlags.Left

                        Case DataGridViewContentAlignment.BottomCenter
                            flags = flags + TextFormatFlags.Bottom
                            flags = flags + TextFormatFlags.HorizontalCenter

                        Case Else
                            flags = flags + TextFormatFlags.Bottom
                            flags = flags + TextFormatFlags.Left
                    End Select

                    TextRenderer.DrawText(e.Graphics, txt, dgvCTS.ColumnHeadersDefaultCellStyle.Font, TextRect, Color.White, flags)
                    e.Graphics.DrawRectangle(Pens.White, e.CellBounds.X - 1, e.CellBounds.Y, e.CellBounds.Width, e.CellBounds.Height - 1)
                Catch
                End Try

                If Filtered = True Then
                    e.Graphics.DrawImage(FilterImage, e.CellBounds.Left + e.CellBounds.Width - FilterImage.Width, e.CellBounds.Top + 1)
                End If

                If Sorted = True Then

                    'show sort seq
                    If dgvCTS.SorterCount > 1 Then
                        VOffset = (e.CellBounds.Height - SortImage.Height - 10) / 2
                        e.Graphics.DrawImage(SortImage, e.CellBounds.Left + e.CellBounds.Width - HOffset - 3, e.CellBounds.Top + VOffset)

                        Dim pt As Point = New Point(e.CellBounds.Left + e.CellBounds.Width - HOffset - 2, e.CellBounds.Top + VOffset + SortImage.Height)
                        Dim f As Font = New Font("Tahoma", 7)
                        TextRenderer.DrawText(e.Graphics, dgvCTS.CustomSorter(e.ColumnIndex).Seq, f, pt, Color.White)
                    Else
                        VOffset = (e.CellBounds.Height - SortImage.Height) / 2
                        e.Graphics.DrawImage(SortImage, e.CellBounds.Left + e.CellBounds.Width - HOffset - 3, e.CellBounds.Top + VOffset)
                    End If
                End If

                e.Handled = True
            End If

        End If

    End Sub

    Private Sub SetCellStyle(ByVal DefaultStyle As DataGridViewCellStyle)

        For Each DynamicStyle In G.DynamicStyles
            Try

                Dim ds As DataGridViewCellStyle = DefaultStyle.Clone
                Dim NewFontName As FontFamily
                Dim NewFontStyle As FontStyle = FontStyle.Regular
                Dim NewFontSize As Single

                If String.IsNullOrEmpty(DynamicStyle.FontName) Then
                    NewFontName = ds.Font.FontFamily
                Else
                    NewFontName = New FontFamily(DynamicStyle.FontName.Trim)
                End If

                If DynamicStyle.FontBold Then
                    NewFontStyle = FontStyle.Bold
                End If

                If DynamicStyle.FontSize > 0 Then
                    NewFontSize = DynamicStyle.FontSize
                Else
                    NewFontSize = ds.Font.Size
                End If
                ds.Font = New Font(NewFontName, NewFontSize, NewFontStyle)

                If Not String.IsNullOrEmpty(DynamicStyle.BackColorName) Then
                    ds.BackColor = Color.FromName(DynamicStyle.BackColorName)
                End If

                If Not String.IsNullOrEmpty(DynamicStyle.ForeColorName) Then
                    ds.ForeColor = Color.FromName(DynamicStyle.ForeColorName)
                End If

                DynamicStyle.Style = ds

            Catch ex As Exception
            End Try
        Next

    End Sub
#End Region


#Region "Filtering Functions"

    ''' <summary>
    ''' Add a new filter to the grid.  This is a legacy call.  Use new the new methods.
    ''' </summary>
    ''' <param name="FieldName">Column name.</param>
    ''' <param name="OpCode">Operation code.</param>
    ''' <param name="Value">Value to test.</param>
    ''' <param name="Value2">Second value to test.  Only used with between opcode.</param>
    ''' <param name="Hide">Keep this filter hidden from view.</param>
    ''' <param name="AsType">Data type.</param>
    ''' <param name="FilterType">Filter Type (*USER or *PROGRAM)</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddFilter(ByVal FieldName As String,
                          ByVal OpCode As String,
                          ByVal Value As String,
                          Optional ByVal Value2 As String = "",
                          Optional ByVal Hide As Boolean = False,
                          Optional ByVal AsType As String = "System.String",
                          Optional ByVal FilterType As String = "*USER",
                          Optional ByVal TrimField As Boolean = False) As Boolean
        Try

            Dim NewCondition As New Condition With {
            .LOP = LogicalOperator.And,
            .ColumnName = FieldName.Trim,
            .Op = GetOperatorName(OpCode),
            .Value = Value.Trim
        }

            If NewCondition.Op = OperatorName.In Then
                Dim ara As Array = Split(Value, ",")
                NewCondition.Values.AddRange(ara)
            End If

            If NewCondition.Op = OperatorName.Between Then
                NewCondition.Values.Add(Value)
                NewCondition.Values.Add(Value2)
            End If

            NewCondition.Hidden = Hide
            NewCondition.Protect = False
            NewCondition.Trim = TrimField

            If AsType.ToLower.Contains("string") Then
                NewCondition.UseDataType(SQLTypes.String)
            ElseIf AsType.ToLower.Contains("datetime") Then
                NewCondition.UseDataType(SQLTypes.DateTime)
            Else
                NewCondition.UseDataType(SQLTypes.Numeric)
            End If

            Select Case UCase(FilterType)
                Case "*USER"
                    UFilters.Conditions.Add(NewCondition)
                    ReSeqFilter()
                Case "*PROGRAM"
                    PFilters.Conditions.Add(NewCondition)
                Case "*SECURITY"
                    SFilters.Conditions.Add(NewCondition)
            End Select

        Catch ex As Exception
            Return False
        Finally

        End Try

        Return True
    End Function

    ''' <summary>
    ''' Add a new filter to the grid using a valuelist.  This is a legacy call.  Use new the new methods.
    ''' </summary>
    ''' <param name="FieldName">Column name.</param>
    ''' <param name="OpCode">Operation code.</param>
    ''' <param name="ValueList">Array containing test values.</param>
    ''' <param name="Hide">Keep this filter hidden from view.</param>
    ''' <param name="AsType">Data type.</param>
    ''' <param name="FilterType">Filter Type (*USER or *PROGRAM)</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddFilter(ByVal FieldName As String,
                      ByVal OpCode As String,
                      ByVal ValueList() As String,
                      Optional ByVal Hide As Boolean = False,
                      Optional ByVal AsType As String = "System.String",
                      Optional ByVal FilterType As String = "*USER",
                      Optional ByVal TrimField As Boolean = False) As Boolean

        Dim NewCondition As New Condition With {
            .LOP = LogicalOperator.And,
            .ColumnName = FieldName.Trim,
            .Op = GetOperatorName(OpCode),
            .Value = String.Empty,
            .Protect = False,
            .Hidden = Hide,
            .Trim = TrimField
        }

        If AsType.ToLower.Contains("string") Then
            NewCondition.UseDataType(SQLTypes.String)
        ElseIf AsType.ToLower.Contains("datetime") Then
            NewCondition.UseDataType(SQLTypes.DateTime)
        Else
            NewCondition.UseDataType(SQLTypes.Numeric)
        End If

        For Each e In ValueList
            NewCondition.Values.Add(e)
        Next

        If NewCondition.Op = OperatorName.Between Then
            If NewCondition.Values.Count < 1 Then
                NewCondition.Values.Add(String.Empty)
            End If
            If NewCondition.Values.Count < 2 Then
                NewCondition.Values.Add(String.Empty)
            End If
        End If

        Select Case UCase(FilterType)
            Case "*USER"
                UFilters.Conditions.Add(NewCondition)
                ReSeqFilter()
            Case "*PROGRAM"
                PFilters.Conditions.Add(NewCondition)
            Case "*SECURITY"
                SFilters.Conditions.Add(NewCondition)
        End Select

        Return True
    End Function









    'todo saved routine; flagged for removal
    'Public Function AddFilterX(ByVal FieldName As String,
    '                  ByVal OpCode As String,
    '                  ByVal ValueList() As String,
    '                  Optional ByVal Hide As Boolean = False,
    '                  Optional ByVal AsType As String = "System.String",
    '                  Optional ByVal FilterType As String = "*USER",
    '                  Optional ByVal TrimField As Boolean = False) As Boolean

    '    Dim NewFilter As New Filtering.GridFilter With {
    '        .ValueList = New List(Of String),
    '        .AO = "AND",
    '        .FieldName = FieldName.Trim,
    '        .OpCode = OpCode.Trim,
    '        .Value = String.Empty,
    '        .Value2 = String.Empty,
    '        .Fldt = AsType.Trim,
    '        .Hidden = Hide,
    '        .Protect = False,
    '        .TrimField = TrimField
    '    }

    '    For Each e In ValueList
    '        NewFilter.ValueList.Add(e)
    '    Next

    '    Select Case UCase(FilterType)
    '        Case "*USER"
    '            UserFiltersX.Add(NewFilter)
    '            ReSeqFilter()
    '        Case "*PROGRAM"
    '            ProgramFiltersX.Add(NewFilter)
    '        Case "*SECURITY"
    '            SecurityFiltersX.Add(NewFilter)
    '    End Select

    '    Return True
    'End Function


    'todo saved routine; flagged for removal
    'Public Function AddFilterX(ByVal FieldName As String,
    '                      ByVal OpCode As String,
    '                      ByVal Value As String,
    '                      Optional ByVal Value2 As String = "",
    '                      Optional ByVal Hide As Boolean = False,
    '                      Optional ByVal AsType As String = "System.String",
    '                      Optional ByVal FilterType As String = "*USER",
    '                      Optional ByVal TrimField As Boolean = False) As Boolean

    '    Dim NewFilter As New Filtering.GridFilter
    '    NewFilter.ValueList = New List(Of String)

    '    If OpCode = "IN" Then
    '        Dim ara As Array = Split(Value, ",")
    '        NewFilter.ValueList.AddRange(ara)
    '    End If

    '    NewFilter.AO = "AND"
    '    NewFilter.FieldName = FieldName.Trim
    '    NewFilter.OpCode = OpCode.Trim
    '    NewFilter.Value = Value.Trim
    '    NewFilter.Value2 = Value2.Trim
    '    NewFilter.Fldt = AsType.Trim
    '    NewFilter.Hidden = Hide
    '    NewFilter.Protect = False
    '    NewFilter.TrimField = TrimField

    '    Select Case UCase(FilterType)
    '        Case "*USER"
    '            UserFiltersX.Add(NewFilter)
    '            ReSeqFilter()
    '        Case "*PROGRAM"
    '            ProgramFiltersX.Add(NewFilter)
    '        Case "*SECURITY"
    '            SecurityFiltersX.Add(NewFilter)
    '    End Select

    '    Return True
    'End Function






    ''' <summary>
    ''' Remove filter by fieldname for one or all opcodes.  
    ''' </summary>
    ''' <param name="FieldName">Fieldname to target.</param>
    ''' <param name="OpCode">Operation code to remove.  Leave blank or specify *AllOpCodes to remove all.</param>
    ''' <remarks></remarks>
    Public Sub RemoveFilter(ByVal FieldName As String, ByVal OpCode As Filtering.OperatorName)
        'Dim _Conditions As List(Of Condition) = UFilters.Conditions.GetRange(0, UFilters.Conditions.Count)
        'For Each c In _Conditions
        '    If c.ColumnName = FieldName And c.Op = OpCode Then
        '        UFilters.Conditions.Remove(c)
        '    End If
        'Next
        For i = UFilters.Conditions.Count - 1 To 0 Step -1
            Dim c = UFilters.Conditions(i)
            If c.ColumnName = FieldName And c.Op = OpCode Then
                UFilters.Conditions.Remove(c)
            End If
        Next

    End Sub

    'legacy filter support...
    Public Sub RemoveFilter(ByVal FieldName As String, ByVal OpCodeString As String)
        'Dim _Conditions As List(Of Condition) = UFilters.Conditions.GetRange(0, UFilters.Conditions.Count)
        'For Each c In _Conditions
        '    If c.ColumnName = FieldName And GetOperator(c.Op).ToString = OpCodeString Then
        '        UFilters.Conditions.Remove(c)
        '    End If
        'Next
        For i = UFilters.Conditions.Count - 1 To 0 Step -1
            Dim c = UFilters.Conditions(i)
            If c.ColumnName = FieldName And GetOperator(c.Op).ToString = OpCodeString Then
                UFilters.Conditions.Remove(c)
            End If
        Next
    End Sub

    Public Sub RemoveFilter(ByVal FieldName As String)
        'Dim _Conditions As List(Of Condition) = UFilters.Conditions.GetRange(0, UFilters.Conditions.Count)
        'For Each c In _Conditions
        '    If c.ColumnName = FieldName Then
        '        UFilters.Conditions.Remove(c)
        '    End If
        'Next
        For i = UFilters.Conditions.Count - 1 To 0 Step -1
            Dim c = UFilters.Conditions(i)
            If c.ColumnName = FieldName Then
                UFilters.Conditions.Remove(c)
            End If
        Next
    End Sub


    ''' <summary>
    ''' Remove filter by number
    ''' </summary>
    ''' <param name="Seq">filter Sequence to remove</param>
    ''' <remarks></remarks>
    Public Sub RemoveFilter(ByVal Seq As Integer)
        'Dim _Conditions As List(Of Condition) = UFilters.Conditions.GetRange(0, UFilters.Conditions.Count)
        'For Each c In _Conditions
        '    If c.Seq = Seq Then
        '        UFilters.Conditions.Remove(c)
        '        Exit For
        '    End If
        'Next
        For i = UFilters.Conditions.Count - 1 To 0 Step -1
            Dim c = UFilters.Conditions(i)
            If c.Seq = Seq Then
                UFilters.Conditions.Remove(c)
                Exit For
            End If
        Next
    End Sub




    'todo filtering updates; flagged for removal
    'Public Sub RemoveFilterX(ByVal FieldName As String, Optional ByVal OpCode As String = "*AllOpCodes")
    '    Dim _UF As List(Of Filtering.GridFilter) = UserFiltersX.GetRange(0, UserFiltersX.Count)

    '    For Each f In _UF
    '        If UCase(OpCode) = "*ALLOPCODES" Then
    '            If f.FieldName = FieldName Then
    '                UserFiltersX.Remove(f)
    '            End If
    '        Else
    '            If f.FieldName = FieldName And f.OpCode = OpCode Then
    '                UserFiltersX.Remove(f)
    '            End If
    '        End If

    '    Next
    'End Sub

    'todo filtering updates; flagged for removal
    'Public Sub RemoveFilterX(ByVal Seq As Integer)
    '    Dim _UF As List(Of Filtering.GridFilter) = UserFiltersX.GetRange(0, UserFiltersX.Count)
    '    For Each f In _UF
    '        If f.Seq = Seq Then
    '            UserFiltersX.Remove(f)
    '            Exit For
    '        End If
    '    Next
    'End Sub



    'legacy support
    Public Sub ClearFilters(Optional ByVal FilterType As String = "*USER")
        Select Case UCase(FilterType)
            Case "*USER"
                UFilters.Conditions.Clear()
                'TODO !!! GRIDID/RELEASE
                GH.DeleteSavedUserFilters("*CURRENT", gName, "*LAST")
            Case "*PROGRAM"
                PFilters.Conditions.Clear()
            Case "*SECURITY"
                SFilters.Conditions.Clear()
        End Select
    End Sub


    ''' <summary>
    ''' Clear All Filters by type.
    ''' </summary>
    ''' <param name="FilterType">Filter Type to remove. Values are *USER or *PROGRAM</param>
    ''' <remarks></remarks>
    Public Sub ClearFilters(ByVal FilterType As FilterType)
        Select Case FilterType
            Case FilterType.User
                UFilters.Conditions.Clear()
                GH.DeleteSavedUserFilters("*CURRENT", gName, "*LAST")
            Case FilterType.Program
                PFilters.Conditions.Clear()
            Case FilterType.Security
                SFilters.Conditions.Clear()
        End Select
    End Sub

    Public Sub ClearFilters()
        UFilters.Conditions.Clear()

        'TODO !!! GRIDID/RELEASE
        GH.DeleteSavedUserFilters("*CURRENT", gName, "*LAST")
    End Sub


    ''' <summary>
    ''' Clear all Program Filters
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ClearProgramFilters()
        PFilters.Conditions.Clear()
    End Sub







    'todo new filter object
    Private Sub ReSeqFilter()
        For i As Integer = 0 To UFilters.Conditions.Count - 1
            Dim TempFilter = UFilters.Conditions(i)
            TempFilter.Seq = i
            UFilters.Conditions(i) = TempFilter
        Next
    End Sub

    'todo new filter object; moved to subset feature; flagged for removal 
    'Private Function GetColFilterCountX(ByVal ColName As String) As Integer
    '    Dim c As Integer = 0
    '    'Add menu item for each existing column filter
    '    For Each f In UserFiltersX
    '        If f.FieldName = ColName Then
    '            c += 1
    '        End If
    '    Next
    '    Return c
    'End Function

    Private Sub btnFilter_MouseUp(sender As Object, e As MouseEventArgs) Handles btnFilter.MouseUp
        cmsSettings.Show(dgvCTS, dgvCTS.Width - cmsGrid.Width, 0)
    End Sub

    Private Sub btnSettings_MouseUp(sender As Object, e As MouseEventArgs) Handles btnSettings.MouseUp
        cmsSettings.Show(tsBanner, tsBanner.Width - cmsSettings.Width, tsBanner.Height)
    End Sub

    Private Sub tsmiRemoveColFilters_Click(sender As Object, e As EventArgs)
        RemoveFilter(CurColumn.Name)
        RefreshData()
    End Sub

#End Region



#Region "Context Menu Display and Click Events"

    'Show cms menus
    Private Sub dgvCTS_CellMouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgvCTS.CellMouseUp
        Debug.Print("CTSGrid: cellmouseup (" & gName.Trim & " CurHeaderIndex:" & dgvCTS.CurHeaderIndex & ") " & e.RowIndex & "/" & e.ColumnIndex)

        Dim rect As Rectangle = dgvCTS.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, True)

        'Cell clicked
        If e.RowIndex >= 0 And e.ColumnIndex >= 0 Then
            If e.Button = Windows.Forms.MouseButtons.Right Then
                dgvCTS.CurrentCell = dgvCTS.Item(e.ColumnIndex, e.RowIndex)
            End If
        End If

        'Handle Left-Click  
        If e.Button = Windows.Forms.MouseButtons.Left Then

            'Grid Header Clicked
            If e.RowIndex = -1 And e.ColumnIndex = -1 Then
                'GH.Call_MNSP025C("", "")
            End If

        End If

        'Handle Right-Click  
        If e.Button = Windows.Forms.MouseButtons.Right Then

            'Grid Header Clicked
            If e.RowIndex = -1 And e.ColumnIndex = -1 Then
                'tsmiExportToExcel.Visible = True
                'ResetToolStripMenuItem.Visible = True
                'miShowTotals.Visible = True
                cmsGrid.Show(CType(sender, Control), e.Location.X + rect.X, e.Location.Y + rect.Y)
            End If

            'Row Header Clicked
            If e.RowIndex > -1 And e.ColumnIndex = -1 Then

                SetRowOptionVisibility()

                Dim Cancel As Boolean = False
                RaiseEvent CustomRowOptionBefore(e.RowIndex, Cancel)
                If Not Cancel Then

                    If G.RowOptions.Count > 0 Then

                        cmsRowOptions.Items.Clear()
                        For Each RO As RowOption In G.RowOptions
                            Dim tsi = cmsRowOptions.Items.Add(RO.Text)
                            tsi.Visible = RO.Visible
                            tsi.Enabled = RO.Enable
                            tsi.Tag = RO
                        Next

                        dgvCTS.CurRowHeaderIndex = e.RowIndex
                        If Not dgvCTS.Rows(e.RowIndex).Selected Then
                            dgvCTS.CurrentCell = dgvCTS.Item(dgvCTS.Columns.GetFirstColumn(DataGridViewElementStates.Visible).Index, e.RowIndex)
                        End If


                        cmsRowOptions.Show(CType(sender, Control), e.Location.X + rect.X, e.Location.Y + rect.Y)
                    End If
                End If
            End If

            'Column Header Clicked
            If e.RowIndex = -1 And e.ColumnIndex > -1 Then
                dgvCTS.CurHeaderIndex = e.ColumnIndex

                CurColumn = G.Columns(dgvCTS.Columns(e.ColumnIndex).Name)

                'CurField = dgvCTS.Columns(e.ColumnIndex).Name
                'CurType = dgvCTS.Columns(e.ColumnIndex).ValueType.ToString
                FormatColumnHeadContext(e.ColumnIndex)
                cmsColumn.Show(CType(sender, Control), e.Location.X + rect.X, e.Location.Y + rect.Y)
            End If

            'Cell Clicked
            If e.RowIndex >= 0 And e.ColumnIndex >= 0 Then
                CurColumn = G.Columns(dgvCTS.Columns(e.ColumnIndex).Name)

                'CurField = dgvCTS.Columns(e.ColumnIndex).Name
                'CurType = dgvCTS.Columns(e.ColumnIndex).ValueType.ToString

                'Try
                ' Dim ctrl As Object = dgvCTS.Columns(e.ColumnIndex)
                ' CurPType = ctrl.DB2ProviderType
                'Catch ex As Exception
                ' CurPType = ""
                'End Try

                If IsDBNull(dgvCTS.Item(e.ColumnIndex, e.RowIndex).Value) Or dgvCTS.Item(e.ColumnIndex, e.RowIndex).Value Is Nothing Then
                    CurValue = "*NULL"
                    miFilterValue.Text = "Filter on " & CurValue.Trim
                Else
                    'Select Case CurPType
                    Select Case CurColumn.ProviderTypeCode
                        Case "12"
                            Dim d As Date = dgvCTS.Item(e.ColumnIndex, e.RowIndex).Value
                            CurValue = d.ToString("yyyy-MM-dd")
                            miFilterOnly.Text = "Only Show " & CurValue.Trim
                            miFilterExclude.Text = "Exclude " & CurValue.Trim
                            miFilterValue.Text = "Filter on " & CurValue.Trim
                        Case "13"
                            Dim d As Date = dgvCTS.Item(e.ColumnIndex, e.RowIndex).Value
                            CurValue = d.ToString("HH.mm.ss")
                            miFilterOnly.Text = "Only Show " & d.ToString("hh:mm:ss tt")
                            miFilterExclude.Text = "Exclude " & d.ToString("hh:mm:ss tt")
                            miFilterValue.Text = "Filter on " & d.ToString("hh:mm:ss tt")
                        Case "14"
                            Dim d As Date = dgvCTS.Item(e.ColumnIndex, e.RowIndex).Value
                            CurValue = d.ToString("yyyy-MM-dd-HH.mm.ss.000000")
                            miFilterOnly.Text = "Only Show " & d.ToString("yyyy-MM-dd hh:mm:ss tt")
                            miFilterExclude.Text = "Exclude " & d.ToString("yyyy-MM-dd hh:mm:ss tt")
                            miFilterValue.Text = "Filter on " & d.ToString("yyyy-MM-dd hh:mm:ss tt")
                        Case Else
                            CurValue = dgvCTS.Item(e.ColumnIndex, e.RowIndex).Value.ToString
                            miFilterOnly.Text = "Only Show '" & CurValue.Trim & "'"
                            miFilterExclude.Text = "Exclude '" & CurValue.Trim & "'"
                            miFilterValue.Text = "Filter on " & CurValue.Trim
                    End Select
                End If

                If CurValue.Trim = "" Then
                    CurValue = "*BLANKS"
                    miFilterOnly.Text = "Only Show " & CurValue.Trim
                    miFilterExclude.Text = "Exclude " & CurValue.Trim
                    miFilterValue.Text = "Filter on " & CurValue.Trim
                End If

                cmsCellMenu.Show(CType(sender, Control), e.Location.X + rect.X, e.Location.Y + rect.Y)

            End If

        End If

    End Sub

    Private Sub SetRowOptionVisibility()
        For Each ro In G.RowOptions
            If ro.Display = DisplayMode.Any Then
                ro.Enable = True
                ro.Visible = True
            ElseIf GridMode = GridModes.Display And ro.Display = DisplayMode.Display Then
                If ro.Display = DisplayMode.Display Then
                    ro.Enable = True
                    ro.Visible = True
                Else
                    ro.Enable = False
                    ro.Visible = False
                End If
            ElseIf GridMode = GridModes.Update And ro.Display = DisplayMode.Update Then
                ro.Enable = True
                ro.Visible = True
            Else
                ro.Enable = False
                ro.Visible = False
            End If
        Next
    End Sub

    Private Sub SetGridFunctionVisibility()
        For Each gf In G.GridFunctions
            If gf.Display = DisplayMode.Any Then
                gf.Enable = True
                gf.Visible = True
            ElseIf GridMode = GridModes.Display And gf.Display = DisplayMode.Display Then
                If gf.Display = DisplayMode.Display Then
                    gf.Enable = True
                    gf.Visible = True
                Else
                    gf.Enable = False
                    gf.Visible = False
                End If
            ElseIf GridMode = GridModes.Update And gf.Display = DisplayMode.Update Then
                gf.Enable = True
                gf.Visible = True
            Else
                gf.Enable = False
                gf.Visible = False
            End If
        Next
    End Sub

    Private Sub FormatColumnHeadContext(ByVal ColumnIndex As Integer)
        Debug.Print("CTSGrid: Format Col menu " & ColumnIndex & " Field=" & CurColumn.Name)
        Dim idx As Short


        Try
            'Dim prop = dgvCTS.Columns(ColumnIndex).GetType().GetProperty("DB2ProviderType")
            'If prop IsNot Nothing Then
            ' CurPType = prop.GetValue(dgvCTS.Columns(ColumnIndex))
            'Else
            ' CurPType = String.Empty
            'End If

            'Menu Header
            Dim lbl As New Label
            lbl.MinimumSize = New Size(215, 20)
            lbl.Text = dgvCTS.Columns(ColumnIndex).HeaderText
            lbl.Font = New Font("Tahoma", 9)
            lbl.TextAlign = ContentAlignment.MiddleCenter
            lbl.Font = New System.Drawing.Font(lbl.Font, FontStyle.Bold)
            lbl.ForeColor = Color.White
            lbl.BackColor = GridPrimary
            lbl.BackgroundImage = GridBackgroundImage
            lbl.BackgroundImageLayout = ImageLayout.Stretch
            Dim ch2 As New ToolStripControlHost(lbl)
            cmsColumn.Items.Insert(0, ch2)

        Catch ex As Exception
            'CurPType = String.Empty
        End Try


        'Allow date filtering options...
        If AllowFilter AndAlso (CurColumn.ProviderTypeCode = "12" Or CurColumn.ProviderTypeCode = "14") Then
            miDateFilters.Visible = True
        Else
            miDateFilters.Visible = False
        End If



        'Handle options dependant on column being sorted...
        If dgvCTS.SorterCount > 0 AndAlso dgvCTS.SorterIndex = ColumnIndex Then
            PositionSeparator.Visible = True

            'This was being envoked when the old "position to" textbox was entered
            'Is it needed?
            If dgvCTS.IsCurrentCellInEditMode Then
                Dim bCancel As Boolean = dgvCTS.CancelEdit()
                Dim bEnd As Boolean = dgvCTS.EndEdit()
            End If


            idx = cmsColumn.Items.IndexOf(PlaceHolderPosition)
            Dim ch1 As New ToolStripEntryHost("Position To:", PositionString, "PositionHost")
            AddHandler ch1.Done, AddressOf CloseCMS
            AddHandler ch1.TextChanged, AddressOf PositionString_TextChanged
            cmsColumn.Items.Insert(idx, ch1)

            If dgvCTS.CustomSorter(dgvCTS.SorterIndex).Order = "ASC" Then
                tsmiSortAscending.Enabled = False
                tsmiSortDescending.Enabled = True
            Else
                tsmiSortAscending.Enabled = True
                tsmiSortDescending.Enabled = False
            End If
        Else
            PositionSeparator.Visible = False
            tsmiSortAscending.Enabled = True
            tsmiSortDescending.Enabled = True
        End If


        'Scanning filters...
        If CurColumn.ProviderTypeCode <> "12" And CurColumn.ProviderTypeCode <> "13" And CurColumn.ProviderTypeCode <> "14" Then
            idx = cmsColumn.Items.IndexOf(PlaceHolderScan)
            Dim ch5 As New ToolStripEntryHost("Ends with:", "", "EndsHost")
            AddHandler ch5.Done, AddressOf CloseCMS
            cmsColumn.Items.Insert(idx, ch5)

            Dim ch4 As New ToolStripEntryHost("Starts with:", "", "StartsHost")
            AddHandler ch4.Done, AddressOf CloseCMS
            cmsColumn.Items.Insert(idx, ch4)

            Dim ch3 As New ToolStripEntryHost("Contains:", "", "ContainsHost")
            AddHandler ch3.Done, AddressOf CloseCMS
            cmsColumn.Items.Insert(idx, ch3)
            ScanSeparator.Visible = True
        Else
            ScanSeparator.Visible = False
        End If



        'Load distinct values
        If CurColumn.DistinctLookup Then
            ' distinct values should be within Program filter conditions
            Dim ProgramFilters = BuildSQLWhere(PFilters)
            If Not String.IsNullOrEmpty(ProgramFilters) Then
                ProgramFilters = $"Where {ProgramFilters}"
            End If

            Dim sql As String = $"Select Distinct {CurColumn.Name} as DV {G.FromClause} {ProgramFilters} Group By {CurColumn.Name}"
            Dim distincttable As DataTable = C2App.Data.GetTable(sql)
            miDistinctValues.Text = $"Distinct ({distincttable.Rows.Count})"
            miDistinctValues.Visible = True
        Else
            miDistinctValues.Visible = False
        End If


        'Handle Filter "IN" values
        CurFL = Nothing
        cmsColumn.Tag = Nothing
        If FilterList IsNot Nothing Then
            'Dim result As GridFilter = GetFilter(dgvCTS.Columns(ColumnIndex).Name, "IN")

            For Each cfv As ColumnFilterValues In FilterList
                If cfv.FieldName = dgvCTS.Columns(ColumnIndex).Name Then

                    Dim tssCurrentFilter As New ToolStripSeparator
                    cmsColumn.Items.Add(tssCurrentFilter)

                    Dim lbl As New Button
                    lbl.Text = "Include Values"
                    lbl.Font = New Font("Tahoma", 9)
                    lbl.Font = New System.Drawing.Font(lbl.Font, FontStyle.Bold)
                    lbl.ForeColor = Color.White
                    lbl.BackColor = GridPrimary
                    lbl.BackgroundImage = GridBackgroundImage
                    lbl.BackgroundImageLayout = ImageLayout.Stretch
                    lbl.FlatStyle = FlatStyle.Flat

                    Dim ch2 As New ToolStripControlHost(lbl)
                    cmsColumn.Items.Add(ch2)

                    For Each fv As FilterValue In cfv.Values
                        Dim ch As New ToolStripControlHost(fv.CB)
                        cmsColumn.Items.Add(ch)
                    Next
                    CurFL = cfv
                    cmsColumn.Tag = cfv
                    Exit For
                End If
            Next

        End If

        Dim Flist = UFilters.Conditions.Subset(CurColumn.Name)
        If Flist.Count > 0 Then
            Dim tssCurrentFilter As New ToolStripSeparator
            cmsColumn.Items.Add(tssCurrentFilter)

            Dim lbl As New Button
            lbl.Text = "Column Filters"
            lbl.Font = New Font("Tahoma", 9)
            lbl.Font = New System.Drawing.Font(lbl.Font, FontStyle.Bold)
            lbl.ForeColor = Color.White
            lbl.BackColor = GridPrimary
            lbl.BackgroundImage = GridBackgroundImage
            lbl.BackgroundImageLayout = ImageLayout.Stretch
            lbl.FlatStyle = FlatStyle.Flat

            Dim ch2 As New ToolStripControlHost(lbl)
            cmsColumn.Items.Add(ch2)

            If Flist.Count > 1 Then
                Dim itm As New ToolStripMenuItem()
                itm.Text = "Remove All Column Filters"
                itm.Tag = "RemoveFilter,-1"
                itm.Image = My.Resources.RedDelete
                cmsColumn.Items.Add(itm)
            End If

            'Add menu item for each existing column filter
            For Each f In UFilters.Conditions
                If f.ColumnName = dgvCTS.Columns(ColumnIndex).Name Then
                    Dim itm As New ToolStripMenuItem()
                    itm.Text = f.Describe
                    itm.Tag = "RemoveFilter," & f.Seq
                    itm.Image = My.Resources.RedDelete
                    cmsColumn.Items.Add(itm)
                End If
            Next
        End If

    End Sub

    Private Sub CloseCMS()
        cmsColumn.Close()
    End Sub


    Private Sub cmsColumn_Closing(sender As Object, e As System.EventArgs) Handles cmsColumn.Closing
        Dim RefreshNeeded As Boolean = False

        If cmsColumn.Items.Contains(cmsColumn.Items("PositionHost")) Then
            cmsColumn.Items.RemoveByKey("PositionHost")
        End If

        If cmsColumn.Items.Contains(cmsColumn.Items("ContainsHost")) Then
            Dim Itm = cmsColumn.Items("ContainsHost")
            If Not String.IsNullOrWhiteSpace(Itm.Text) Then
                RemoveFilter(CurColumn.Name, OperatorName.Like)
                Dim cond = UFilters.AddFilter(CurColumn.Name, OperatorName.Like, $"%{Itm.Text}%")
                cond.Description = $"Contains {Itm.Text}"
                RefreshNeeded = True
            End If
            cmsColumn.Items.Remove(Itm)
        End If

        If cmsColumn.Items.Contains(cmsColumn.Items("StartsHost")) Then
            Dim Itm = cmsColumn.Items("StartsHost")
            If Not String.IsNullOrWhiteSpace(Itm.Text) Then
                RemoveFilter(CurColumn.Name, OperatorName.Like)
                Dim cond = UFilters.AddFilter(CurColumn.Name, OperatorName.Like, $"{Itm.Text}%")
                cond.Description = $"Starts with {Itm.Text}"
                RefreshNeeded = True
            End If
            cmsColumn.Items.Remove(Itm)
        End If

        If cmsColumn.Items.Contains(cmsColumn.Items("EndsHost")) Then
            Dim Itm = cmsColumn.Items("EndsHost")
            If Not String.IsNullOrWhiteSpace(Itm.Text) Then
                RemoveFilter(CurColumn.Name, OperatorName.Like)
                Dim cond = UFilters.AddFilter(CurColumn.Name, OperatorName.Like, $"%{Itm.Text}")
                cond.Description = $"Ends with {Itm.Text}"
                RefreshNeeded = True
            End If
            cmsColumn.Items.Remove(Itm)
        End If



        If CurFL IsNot Nothing Then
            Dim ChangesMade As Boolean = False
            Dim strValue As String = String.Empty

            For Each li As FilterValue In CurFL.Values

                If li.Selected <> li.CB.Checked Then
                    ChangesMade = True
                End If

                If li.CB.Checked Then
                    If strValue = String.Empty Then
                        strValue = String.Format("{0}", li.CB.Text)
                    Else
                        strValue = String.Format("{0},{1}", strValue.Trim, li.CB.Text)
                    End If

                    li.Selected = True
                Else
                    li.Selected = False
                End If

            Next

            If ChangesMade Then
                RemoveFilter(CurFL.FieldName, OperatorName.In)
                If strValue <> String.Empty Then
                    'AddFilter(CurFL.FieldName, "IN", strValue)
                    Dim cond = UFilters.AddFilter(CurFL.FieldName, OperatorName.In, strValue)
                    cond.Description = $"In List: strvalue"
                End If
                RefreshNeeded = True
            End If
        End If

        'Cleanup
        For i As Integer = cmsColumn.Items.Count - 1 To 0 Step -1
            If cmsColumn.Items(i).Name = String.Empty Then
                cmsColumn.Items.RemoveAt(i)
            End If
        Next


        If RefreshNeeded Then
            RefreshData()
        End If

    End Sub

    Private Sub cmsColumn_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles cmsColumn.ItemClicked
        If Mid(e.ClickedItem.Tag, 1, 12) = "RemoveFilter" Then
            Dim ara As String() = Split(e.ClickedItem.Tag, ",")
            Dim intSeq As Integer = ara(1)
            If intSeq = -1 Then
                RemoveFilter(CurColumn.Name)
            Else
                RemoveFilter(intSeq)
            End If
            RefreshData()
        End If
    End Sub

    'Context menu events
    Private Sub miClearAllFilters_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles miClearAllFilters.Click
        ClearFilters()
        RefreshData()
    End Sub

    Private Sub miHideColumn_Click(sender As Object, e As EventArgs) Handles miHideColumn.Click
        GridSettingIsDirty = True
        dgvCTS.Columns(CurColumn.Name).Visible = False
        If miShowTotals.Checked Then
            dgvTotals.Columns(CurColumn.Name).Visible = False
        End If
    End Sub



    Private Sub miFilterEqual_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles miFilterOnly.Click, miFilterEqual.Click

        Dim nf = G.Columns(CurColumn.Name).NewFilter(OperatorName.Equals, CurValue)
        UFilters.Conditions.Add(nf)

        'AddFilter(CurField, "=", CurValue, , , CurType)
        RefreshData()
    End Sub

    Private Sub miFilterNotEqual_Click(sender As Object, e As EventArgs) Handles miFilterExclude.Click, miFilterNotEqual.Click
        Dim nf = G.Columns(CurColumn.Name).NewFilter(OperatorName.NotEquals, CurValue)
        UFilters.Conditions.Add(nf)

        'AddFilter(CurField, "<>", CurValue, , , CurType)
        RefreshData()
    End Sub

    Private Sub miFilterGreater_Click(sender As Object, e As EventArgs) Handles miFilterGreater.Click
        Dim nf = G.Columns(CurColumn.Name).NewFilter(OperatorName.GreaterThan, CurValue)
        UFilters.Conditions.Add(nf)

        'AddFilter(CurField, ">", CurValue, , , CurType)
        RefreshData()
    End Sub

    Private Sub miFilterGreaterEq_Click(sender As Object, e As EventArgs) Handles miFilterGreaterEq.Click
        Dim nf = G.Columns(CurColumn.Name).NewFilter(OperatorName.GreaterThanOrEqual, CurValue)
        UFilters.Conditions.Add(nf)

        'AddFilter(CurField, ">=", CurValue, , , CurType)
        RefreshData()
    End Sub

    Private Sub miFilterLess_Click(sender As Object, e As EventArgs) Handles miFilterLess.Click
        Dim nf = G.Columns(CurColumn.Name).NewFilter(OperatorName.LessThan, CurValue)
        UFilters.Conditions.Add(nf)

        'AddFilter(CurField, "<", CurValue, , , CurType)
        RefreshData()
    End Sub

    Private Sub miFilterLessEq_Click(sender As Object, e As EventArgs) Handles miFilterLessEq.Click
        Dim nf = G.Columns(CurColumn.Name).NewFilter(OperatorName.LessThanOrEqual, CurValue)
        UFilters.Conditions.Add(nf)

        'AddFilter(CurField, "<=", CurValue, , , CurType)
        RefreshData()
    End Sub

    Private Sub miAdvancedFilterCol_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles miAdvancedFilterCol.Click, tsmiColumnFilter.Click
        CurGrid = Me.Name
    End Sub

    Private Sub tsmiSortAscending_Click(sender As Object, e As EventArgs) Handles tsmiSortAscending.Click
        dgvCTS.ClearSorter()
        dgvCTS.SorterCount = 1
        dgvCTS.CustomSorter(dgvCTS.CurHeaderIndex).Seq = 1
        dgvCTS.CustomSorter(dgvCTS.CurHeaderIndex).Order = "ASC"
        dgvCTS.ApplySorter()
    End Sub

    Private Sub tsmiSortDescending_Click(sender As Object, e As EventArgs) Handles tsmiSortDescending.Click
        dgvCTS.ClearSorter()
        dgvCTS.SorterCount = 1
        dgvCTS.CustomSorter(dgvCTS.CurHeaderIndex).Seq = 1
        dgvCTS.CustomSorter(dgvCTS.CurHeaderIndex).Order = "DESC"
        dgvCTS.ApplySorter()
    End Sub

    Private Sub tsmiExportToExcel_Click(sender As Object, e As EventArgs) Handles tsmiExportToExcel.Click
        ExportXLS(dgvCTS, Me.GridTitle)
    End Sub

    Private Sub ResetToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ResetToolStripMenuItem.Click
        GH.DeleteGridSettings(gID, gVersion)
        GridSettingIsDirty = False
        Me.RefreshData()
    End Sub

    'Mousedown events for setting UserControl Focus
    Private Sub MouseDownOnBanner(sender As Object, e As MouseEventArgs) Handles tsBanner.MouseDown, btnGridRefresh.MouseDown, lblGridTitle.MouseDown, btnFilter.MouseDown
        If Me.Enabled AndAlso Not Me.Focused Then
            Me.Focus()
        End If
    End Sub

    Private Sub MouseDownOnBottom(sender As Object, e As MouseEventArgs) Handles ssBottom.MouseDown, lblGridStatus.MouseDown, lblSuspend.MouseDown
        If Me.Enabled AndAlso Not Me.Focused Then
            Me.Focus()
        End If
    End Sub

    Private Sub miFilters_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles miFilters.Click

        GH.SaveGridSettings(dgvCTS, gID, gVersion, gName)
        LoadModelFromDB(gID, gVersion, gName)

        'call the settings form...

    End Sub

    Private Sub ColumnSettingsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles miColumns.Click

        'GH.SaveGridSettings(Me.dgvCTS, Me.GridName)
        'SetupDS(Me.GridName)

        ShowSettings()

    End Sub

    Private Sub miSorting_Click(sender As Object, e As EventArgs) Handles miSorting.Click

        GH.SaveGridSettings(dgvCTS, gID, gVersion, gName)
        LoadModelFromDB(gID, gVersion, gName)

        'Dim frmSettings As New SettingsForm(Me, "tpSorting")
        'frmSettings.StartPosition = FormStartPosition.CenterParent
        'frmSettings.ShowDialog(Me)

    End Sub

    Private Sub miRefresh_Click(sender As Object, e As EventArgs) Handles miRefresh.Click
        Me.RefreshData()
    End Sub

    Private Sub miProperties_Click(sender As Object, e As EventArgs) Handles miProperties.Click
        'miColumns.PerformClick()
        ShowSettings()
    End Sub

#End Region

#Region "Custom Sorting & Positioning"

    'Positioning in a sorted column
    Private Sub dgvCTS_Sorted(sender As Object, e As EventArgs) Handles dgvCTS.Sorted

        'Clear the "position to" strings 
        PositionString = String.Empty
        dgvCTS.CurrentCell = Nothing

    End Sub

    Public Event CustomSorted(ByVal Sender As Object, ByVal ColumnIndex As Integer, ByVal SortSequence As Integer, ByVal SortOrder As String)
    Private Sub dgvCTS_CustomSorted(sender As Object, Idx As Integer, Seq As Integer, Order As String) Handles dgvCTS.CustomSorted

        'Clear the "position to" strings 
        PositionString = String.Empty
        dgvCTS.CurrentCell = Nothing

        'Cause settings rewrite for new sorting
        GridSettingIsDirty = True

        RaiseEvent CustomSorted(Me, Idx, Seq, Order)
    End Sub

    Private Sub dgvCTS_SorterPositionCleared(sender As Object) Handles dgvCTS.SorterPositionCleared
        Try
            SetGridMessage(dgvCTS.RowCount & " records loaded")
        Catch
        End Try
    End Sub

    Public Event CustomPosition(ByVal Sender As Object, ByVal PositionString As String, ByVal RowIndex As Integer)
    Private Sub dgvCTS_CustomPosition(Sender As Object, RepositionString As String, RowIndex As Integer) Handles dgvCTS.CustomPosition
        SuppressPositionTextChange = True
        PositionString = RepositionString
        SuppressPositionTextChange = False

        If RepositionString = String.Empty Then
            SetGridMessage(dgvCTS.RowCount & " records loaded")
        Else
            SetGridMessage("Repositioned '" & dgvCTS.Columns(dgvCTS.SorterIndex).HeaderText & "' to '" & PositionString & "'.")
        End If

        RaiseEvent CustomPosition(Me, PositionString, RowIndex)
    End Sub

    Private Sub PositionString_TextChanged(sender As Object, e As EventArgs)
        If SuppressPositionTextChange Then Exit Sub

        Dim Reposition As String = sender.text

        dgvCTS.EditMode = DataGridViewEditMode.EditOnF2
        dgvCTS.SortedColumnPositioning(Reposition)
        dgvCTS.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2
    End Sub


    Private Sub tmrPositionReset_Tick(sender As Object, e As EventArgs) Handles tmrPositionReset.Tick
        PositionString = String.Empty
        SetGridMessage($"{dgvCTS.RowCount} records loaded")
        tmrPositionReset.Enabled = False
    End Sub


    Private Sub dgvCTS_SortCompare(ByVal sender As Object, ByVal e As DataGridViewSortCompareEventArgs) Handles dgvCTS.SortCompare

        ' Try to sort based on the contents of the cell in the current column.
        e.SortResult = System.String.Compare(UCase(e.CellValue1.ToString()), UCase(e.CellValue2.ToString()))

        'If the cells are equal, sort based on a secondary column. 
        'If (e.SortResult = 0) AndAlso Not (e.Column.Name = "AnotherRCol") Then
        'e.SortResult = System.String.Compare(dgvCTS.Rows(e.RowIndex1).Cells("AnotherRCol").Value.ToString(), _ 
        '               dgvCTS.Rows(e.RowIndex2).Cells("AnotherRCol").Value.ToString())
        'End If

        e.Handled = True

    End Sub

#End Region

#Region "Scanning"
    Private Sub ColumnScan(ByVal ColumnName As String, ByVal ScanFor As String)
        Dim expression As String = String.Empty

        If ScanFor = String.Empty Then
            dgvCTS.DataSource = dtGridData
        Else
            'If expression = String.Empty Then
            '    expression = String.Format("{0} Like '*{1}*'", Name, ScanFor)
            'Else
            '    expression = String.Format("{0} or {1} Like '*{2}*'", expression, Name, ScanFor)
            'End If

            'Dim SourceView As New DataView(dtGridData)
            'SourceView.RowFilter = expression
            'dgvCTS.DataSource = SourceView

            AddFilter(ColumnName, "Like", $"%{ScanFor}%", " ",  , , ,  )
            UFilters.AddFilter(ColumnName, OperatorName.Like, $"%{ScanFor}%")
        End If

    End Sub

#End Region


#Region "Editing Controls and Prompting"

    'Editing control / Prompting 
    Public Event HandleCustomPrompt(ByVal colname As String, ByVal r As DataGridViewRow)

    Public Event dgv_HandlePromptSelection(ByVal dt As DataTable, ByVal colname As String, ByVal c As DataGridViewCell)

    Private Sub dgvCTS_EditingControlShowing(sender As Object, e As DataGridViewEditingControlShowingEventArgs) Handles dgvCTS.EditingControlShowing

        MyEC = e.Control
        If MyEC.PreferredSize.Height + 4 > dgvCTS.CurrentRow.Height Then
            dgvCTS.CurrentRow.Height = MyEC.PreferredSize.Height + 4
            Debug.Print("CTSGrid: Row increased for edit control: " & MyEC.PreferredSize.Height + 4)
        End If
        MyEC.Width = dgvCTS.CurrentCell.Size.Width

        Debug.Print("CTSGrid: " & e.Control.ToString)

        If e.Control.GetType Is GetType(PromptEditingControl) Then
            Dim ctrl As PromptEditingControl = e.Control

            RemoveHandler ctrl.CellPrompted, AddressOf Me.HandlePromptClick
            AddHandler ctrl.CellPrompted, AddressOf Me.HandlePromptClick

            RemoveHandler ctrl.KeyUp, AddressOf Me.UpdatePromptCellText
            AddHandler ctrl.KeyUp, AddressOf Me.UpdatePromptCellText
        End If

        If e.Control.GetType Is GetType(DataGridViewTextBoxEditingControl) Then
            Dim ctrl As DataGridViewTextBoxEditingControl = e.Control
            Dim col As DataGridViewColumnCTS = dgvCTS.Columns(sender.currentcelladdress.x)
            If col.DB2ProviderType = "6" AndAlso col.DB2Size > 0 Then
                ctrl.MaxLength = col.DB2Size
            End If
        End If
    End Sub

    Private Sub HandlePromptClick(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim PEC As PromptEditingControl = sender

        Select Case PEC.PromptType
            Case "PMT", "CTSPROMPT"

                'Dim frm As New Form
                'frm.FormBorderStyle = Windows.Forms.FormBorderStyle.None
                'frm.Location = New Point(MousePosition.X, MousePosition.Y)
                'frm.Size = New System.Drawing.Size(550, 700)
                'frm.Show()

                'Dim CtsPrompt1 As New CTS.Controls.CTSPrompt
                'CtsPrompt1.Dock = DockStyle.Fill
                'frm.Controls.Add(CtsPrompt1)
                'frm.Refresh()

                'Dim dr As DataRow = GH.SelectGridCol(GridName, dgvCTS.CurrentCell.OwningColumn.Name)
                'CtsPrompt1.GridName = dr("GCPRMTGRID")
                'CtsPrompt1.GridApp = "ER"
                'CtsPrompt1.DataEnvironment = DataEnvironments.Production
                'CtsPrompt1.RefreshData()

                'RemoveHandler CtsPrompt1.dgv_PromptResults, AddressOf Me.HandlePromptSelection
                'AddHandler CtsPrompt1.dgv_PromptResults, AddressOf Me.HandlePromptSelection

                'frm.Hide()
                'frm.ShowDialog()


            Case "CUSTOMPROMPT"
                RaiseEvent HandleCustomPrompt(dgvCTS.CurrentCell.OwningColumn.Name, dgvCTS.CurrentCell.OwningRow)

        End Select

    End Sub

    Private Sub HandlePromptSelection(ByVal dt As DataTable)
        RaiseEvent dgv_HandlePromptSelection(dt, dgvCTS.CurrentCell.OwningColumn.Name, dgvCTS.CurrentCell)
    End Sub

    Private Sub UpdatePromptCellText(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim cb As ComboBox = sender
        If IsDBNull(dgvCTS.CurrentCell.Value) And cb.Text = String.Empty Then
        Else
            dgvCTS.CurrentCell.Value = cb.Text
        End If
        Debug.Print("CTSGrid: updatepromptcelltext=" & cb.Text)
    End Sub

    Public Sub UpdateCurrentPromptCellText(ByVal txt As String)
        MyEC.Text = txt
    End Sub

    ''' <summary>
    ''' Dynamically sets the combobox data 
    ''' </summary>
    ''' <param name="ColumnName"></param>
    ''' <param name="strSQL"></param>
    ''' <param name="strDisplayMember"></param>
    ''' <param name="strValueMember"></param>
    ''' <remarks></remarks>
    Public Sub SetComboBoxData(ByVal ColumnName As String, ByVal strSQL As String,
                           Optional ByVal strDisplayMember As String = "DisplayValue",
                           Optional ByVal strValueMember As String = "")
        Dim cmbCol As DataGridViewComboBoxColumnCTS
        Try
            cmbCol = Me.dgvCTS.Columns(ColumnName)
            Dim PulldownDT As DataTable = C2App.Data.GetTable(strSQL)
            Dim PullDownDR As DataRow = PulldownDT.NewRow
            PulldownDT.Rows.InsertAt(PullDownDR, 0)
            cmbCol.DataSource = PulldownDT
            cmbCol.DisplayMember = strDisplayMember
            If strValueMember <> String.Empty Then
                cmbCol.ValueMember = strValueMember
            End If
        Catch ex As Exception
            Debug.Print("CTSGrid: " & ex.Message)
        End Try
    End Sub

#End Region

#Region "Validating and Updating"

    Private Sub CTSGrid_Validating(sender As Object, e As CancelEventArgs) Handles MyBase.Validating
        Me.ValidateChildren()
    End Sub

    'Cell-Level
    Public Event dgv_CellNeedsValidating(ByVal CurGridRow As DataGridViewRow, ByVal ColName As String, ByRef ErrorText As String, ByRef AllowExit As Boolean)

    Private Sub PerformCellValidation(CurGridRow As DataGridViewRow, ColName As String, ByRef myErrorText As String, ByRef allowExit As Boolean)
        'If Validation routine setup then 
        '  Call CTS Validation routine Handler
        'Else
        '  pass control to parent to do the validation
        RaiseEvent dgv_CellNeedsValidating(CurGridRow, ColName, myErrorText, allowExit)
        'Endif
    End Sub

    Private Sub dgvCTS_CellValidating(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellValidatingEventArgs) Handles dgvCTS.CellValidating
        If Not Initializing And Not IsLoading And Not dgvCTS.IsSuspended Then

            Debug.Print("CTSGrid: cellvalidating row " & e.RowIndex & " col " & e.ColumnIndex)
            Dim thisGrid As DataGridView = sender
            Dim thiscell As DataGridViewCell = sender.item(e.ColumnIndex, e.RowIndex)

            If thiscell.IsInEditMode Then
                Dim myErrorText As String = String.Empty
                Dim allowExit As Boolean

                If TestEdittedValue(thiscell.EditedFormattedValue, thiscell.ValueType.ToString) Then

                    thiscell.ErrorText = String.Empty
                    thiscell.ToolTipText = String.Empty

                    PerformCellValidation(thisGrid.Rows(e.RowIndex), thisGrid.Columns(e.ColumnIndex).Name, myErrorText, allowExit)


                    thiscell.ErrorText = myErrorText
                    thiscell.ToolTipText = thiscell.ErrorText

                    If myErrorText <> String.Empty Then
                        MyEC.BackColor = EditErrorBack
                        If allowExit Then
                            e.Cancel = False
                        Else
                            e.Cancel = True
                        End If

                    End If
                Else
                    e.Cancel = False
                    MyEC.BackColor = EditErrorBack
                    thiscell.ErrorText = "Invalid Data Entered in Cell. Press <Esc> to reset."
                    thiscell.ToolTipText = thiscell.ErrorText
                End If
            End If
        End If

    End Sub

    'Row-Level
    Public Event dgv_RowNeedsValidating(ByVal CurGridRow As DataGridViewRow, ByRef ErrorText As String, ByRef AllowExit As Boolean)
    Public Event dgv_RowNeedsUpdating(ByVal CurGridRow As DataGridViewRow, ByRef AllowExit As Boolean)
    Public Event dgv_RowUpdateCompleted(ByVal CurGridRow As DataGridViewRow)

    Private Sub PerformRowValidation(CurGridRow As DataGridViewRow, ByRef myErrorText As String, ByRef allowExit As Boolean)
        'If Validation routine setup then 
        'Call CTS Validation routine Handler
        'Else
        'pass control to parent to do the validation
        RaiseEvent dgv_RowNeedsValidating(CurGridRow, myErrorText, allowExit)
        'Endif
    End Sub

    Private Sub dgvCTS_RowValidating(sender As Object, e As DataGridViewCellCancelEventArgs) Handles dgvCTS.RowValidating
        If Not Initializing And Not IsLoading And Not dgvCTS.IsSuspended Then
            Debug.Print("CTSGrid: rowvaliding row " & e.RowIndex)

            Dim thisGrid As DataGridView = sender
            Dim thisRow As DataGridViewRow = thisGrid.Rows(e.RowIndex)

            Dim drv As DataRowView
            Try
                drv = thisRow.DataBoundItem
            Catch ex As Exception
                drv = Nothing
            End Try

            If dgvCTS.IsCurrentRowDirty Then

                Dim myErrorText As String = String.Empty
                Dim allowExit As Boolean

                PerformRowValidation(thisRow, myErrorText, allowExit)
                If myErrorText = String.Empty Then

                    'Fix new row
                    If thisRow.IsNewRow Then

                    End If

                    'Fix detached row (add to datatable) 
                    If drv IsNot Nothing AndAlso drv.Row.RowState = DataRowState.Detached Then
                        dtGridData.ImportRow(drv.Row)
                    End If



                    If UpdateMethod = GridUpdateMethods.RowAutomatic Then
                        If HasCellErrors(thisRow) Then
                            thisRow.ErrorText = "Could not update row - Cell errors exist"
                            e.Cancel = True
                        Else

                            If GH.UpdateRow2(G, thisRow) > 0 Then
                                thisRow.ErrorText = String.Empty

                                'Clear dirty state
                                drv = thisRow.DataBoundItem
                                If drv.Row.RowState = DataRowState.Detached Then
                                    'drv.Row.SetAdded()
                                    'drv.Row.AcceptChanges()
                                Else
                                    drv.Row.AcceptChanges()
                                End If

                                RaiseEvent dgv_RowUpdateCompleted(thisRow)
                                e.Cancel = False
                            Else
                                If thisRow.ErrorText = String.Empty Then
                                    thisRow.ErrorText = "Unable to update row"
                                End If
                                e.Cancel = True
                            End If

                        End If
                    End If

                    If UpdateMethod = GridUpdateMethods.RowParent Then
                        'Raise Update Event for parent
                        thisRow.ErrorText = "Update failed or not handled"
                        RaiseEvent dgv_RowNeedsUpdating(thisRow, allowExit)
                        If thisRow.ErrorText = String.Empty Then

                            'Clear dirty state
                            drv = thisRow.DataBoundItem
                            If drv.Row.RowState = DataRowState.Detached Then
                                'drv.Row.SetAdded()
                                'drv.Row.AcceptChanges()
                            Else
                                drv.Row.AcceptChanges()
                            End If

                            RaiseEvent dgv_RowUpdateCompleted(thisRow)
                            e.Cancel = False
                        Else
                            e.Cancel = True
                        End If
                    End If

                    If UpdateMethod = GridUpdateMethods.GridAutomatic _
                    Or UpdateMethod = GridUpdateMethods.GridParent Then
                        dgvCTS.SetState(True)
                    End If

                End If

            Else
                dgvCTS.Rows(e.RowIndex).ErrorText = String.Empty
                If drv IsNot Nothing AndAlso drv.Row.RowState = DataRowState.Unchanged Then
                    dgvCTS.CheckState()
                End If
            End If
        Else


        End If

        dgvCTS.Invalidate()
    End Sub

    'Grid-Level
    'Pass on the state change to user control 
    Public Event dgv_GridStateChanged(ByVal sender As Object, Dirty As Boolean)
    Private Sub dgvCTS_StateChanged(ByVal sender As Object, Dirty As Boolean) Handles dgvCTS.StateChanged
        IsDirty = Dirty
        RaiseEvent dgv_GridStateChanged(sender, Dirty)
    End Sub

    Public Event dgv_GridNeedsValidating(ByVal sender As Object, ByRef gridErrorText As String, ByRef AllowExit As Boolean)
    Private Sub PerformGridValidation(ByVal sender As Object, ByRef gridErrorText As String, ByRef allowExit As Boolean)

        'If Validation routine setup then 
        'Call CTS Validation routine Handler HERE.....
        'Else
        'pass control to parent to do the validation
        RaiseEvent dgv_GridNeedsValidating(sender, gridErrorText, allowExit)
        'Endif

    End Sub


    '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    'just a test...  put this save version back! 
    Private Sub dgvCTS_GridValidatingSave(ByVal ThisGrid As DataGridView)

        Dim GridErrorText As String = String.Empty
        Dim allowExit As Boolean
        PerformGridValidation(ThisGrid, GridErrorText, allowExit)
        If GridErrorText = String.Empty Then

            If UpdateMethod = GridUpdateMethods.GridAutomatic Then

                For Each thisrow As DataGridViewRow In dgvCTS.Rows

                    Select Case GetDataRowState(thisrow)
                        Case DataRowState.Modified

                            If GH.UpdateRow2(G, thisrow) > 0 Then
                                GridErrorText = String.Empty

                                'Clear dirty state
                                Dim drv As DataRowView = thisrow.DataBoundItem
                                drv.Row.AcceptChanges()

                                RaiseEvent dgv_RowUpdateCompleted(thisrow)
                                'e.Cancel = False
                            Else
                                If thisrow.ErrorText = String.Empty Then
                                    thisrow.ErrorText = "Unable to update row"
                                End If
                                'e.Cancel = True
                            End If

                        Case DataRowState.Added, 1
                            MsgBox("The Grid cannot add this row")
                    End Select

                Next

                'Handle Deleted rows 
                Dim dt As DataTable = dtGridData.Copy
                Dim dv As DataView = New DataView(dt)
                dv.RowStateFilter = DataViewRowState.Deleted

                If dv.Count > 0 Then
                    MsgBox("Cannot perform delete operation.")
                    'allow this routine to work on datarow not datagridviewrow
                    'rc = GH.DeleteRow(GridName, DataRow, dtUpdateFiles)
                End If
            End If


            If UpdateMethod = GridUpdateMethods.GridParent Then

                For Each thisrow As DataGridViewRow In dgvCTS.Rows
                    Dim dvrState As DataViewRowState = GetDataRowState(thisrow)

                    If dvrState = DataRowState.Modified _
                    Or dvrState = DataViewRowState.Added Then

                        'Raise Update Event for parent
                        thisrow.ErrorText = "Update failed or not handled"
                        RaiseEvent dgv_RowNeedsUpdating(thisrow, allowExit)
                        If thisrow.ErrorText = String.Empty Then

                            'Clear dirty state
                            Dim drv As DataRowView = thisrow.DataBoundItem
                            drv.Row.AcceptChanges()
                            ThisGrid.Invalidate()
                            RaiseEvent dgv_RowUpdateCompleted(thisrow)
                            'e.Cancel = False
                        Else
                            'e.Cancel = True
                        End If

                    End If

                Next

                'Handle Deleted rows
                Dim dt As DataTable = dtGridData.Copy
                Dim dv As DataView = New DataView(dt)
                dv.RowStateFilter = DataViewRowState.Deleted

                If dv.Count > 0 Then
                    Dim FailCount = 0
                    RaiseEvent dgv_RowsNeedDeleting(dv, FailCount)
                    If FailCount > 0 Then
                        MsgBox(FailCount & " rows could not be deleted.")
                    End If
                End If
            End If

        Else
            MsgBox(GridErrorText, MsgBoxStyle.Critical, "Grid Update Error")
        End If

        dgvCTS.Invalidate()
    End Sub

    Private Sub dgvCTS_GridValidating(ByVal ThisGrid As DataGridView)

        Dim GridErrorText As String = String.Empty
        Dim allowExit As Boolean
        PerformGridValidation(ThisGrid, GridErrorText, allowExit)
        If GridErrorText = String.Empty Then

            If UpdateMethod = GridUpdateMethods.GridAutomatic Then

                For Each thisrow As DataGridViewRow In dgvCTS.Rows

                    Select Case GetDataRowState(thisrow)
                        Case DataRowState.Modified

                            If GH.UpdateRow2(G, thisrow) > 0 Then
                                GridErrorText = String.Empty

                                'Clear dirty state
                                Dim drv As DataRowView = thisrow.DataBoundItem
                                drv.Row.AcceptChanges()

                                RaiseEvent dgv_RowUpdateCompleted(thisrow)
                                'e.Cancel = False
                            Else
                                If thisrow.ErrorText = String.Empty Then
                                    thisrow.ErrorText = "Unable to update row"
                                End If
                                'e.Cancel = True
                            End If

                        Case DataRowState.Added, 1
                            MsgBox("The Grid cannot add this row")
                    End Select

                Next

                'Handle Deleted rows 
                Dim dt As DataTable = dtGridData.Copy
                Dim dv As DataView = New DataView(dt)
                dv.RowStateFilter = DataViewRowState.Deleted

                If dv.Count > 0 Then
                    MsgBox("Cannot perform delete operation.")
                    'allow this routine to work on datarow not datagridviewrow
                    'rc = GH.DeleteRow(GridName, DataRow, dtUpdateFiles)
                End If
            End If


            If UpdateMethod = GridUpdateMethods.GridParent Then

                For Each thisrow As DataGridViewRow In dgvCTS.Rows
                    Dim dvrState As DataViewRowState = GetDataRowState(thisrow)

                    If dvrState = DataRowState.Modified _
                    Or dvrState = DataViewRowState.Added Then

                        'Raise Update Event for parent
                        thisrow.ErrorText = "Update failed or not handled"
                        RaiseEvent dgv_RowNeedsUpdating(thisrow, allowExit)
                        If thisrow.ErrorText = String.Empty Then

                            'Clear dirty state
                            Dim drv As DataRowView = thisrow.DataBoundItem
                            drv.Row.AcceptChanges()
                            ThisGrid.Invalidate()
                            RaiseEvent dgv_RowUpdateCompleted(thisrow)
                            'e.Cancel = False
                        Else
                            'e.Cancel = True
                        End If

                    End If

                Next

                'Handle Deleted rows
                Dim dt As DataTable = dtGridData.Copy
                Dim dv As DataView = New DataView(dt)
                dv.RowStateFilter = DataViewRowState.Deleted

                If dv.Count > 0 Then
                    Dim FailCount = 0
                    RaiseEvent dgv_RowsNeedDeleting(dv, FailCount)
                    If FailCount > 0 Then
                        MsgBox(FailCount & " rows could not be deleted.")
                    End If
                End If
            End If

        Else
            MsgBox(GridErrorText, MsgBoxStyle.Critical, "Grid Update Error")
        End If

        dgvCTS.Invalidate()
    End Sub

    Public Function UpdateGrid(Optional ByVal AllowPartial As Boolean = False) As Integer
        Try

            If Not AllowPartial And HasErrors() Then
                MsgBox("Update(s) cannot be performed.  Errors must be corrected.")
                Return -1
            End If

            If UpdateMethod = GridUpdateMethods.GridParent _
            Or UpdateMethod = GridUpdateMethods.GridAutomatic Then
                dgvCTS_GridValidating(dgvCTS)
                dgvCTS.CheckState()
            End If
            Return 0

        Catch ex As Exception
            Return -1
        End Try
    End Function

    'Deleting 
    Public Event dgv_RowsNeedDeleting(ByVal DeletedRows As DataView, ByRef FailCount As Integer)
    Public Event dgv_RowNeedsDeleting(ByVal CurGridRow As DataGridViewRow, ByRef AllowDelete As Boolean)

    Private Sub dgvCTS_UserDeletingRow(sender As Object, e As DataGridViewRowCancelEventArgs) Handles dgvCTS.UserDeletingRow
        Debug.Print("CTSGrid: before delete...")

        Dim rc As Integer = 0
        Dim AllowDelete As Boolean = True

        Select Case UpdateMethod
            Case GridUpdateMethods.RowParent
                RaiseEvent dgv_RowNeedsDeleting(e.Row, AllowDelete)
                If AllowDelete Then
                    e.Cancel = False
                Else
                    e.Cancel = True
                End If

            Case GridUpdateMethods.RowAutomatic
                rc = GH.DeleteRow2(G, e.Row)
                If rc >= 1 Then
                    e.Cancel = False
                Else
                    e.Cancel = True
                End If

            Case GridUpdateMethods.GridParent
                'do nothing

            Case GridUpdateMethods.GridAutomatic
                'do nothing 

        End Select

    End Sub

    Private Sub dgvCTS_UserDeletedRow(sender As Object, e As DataGridViewRowEventArgs) Handles dgvCTS.UserDeletedRow
        If UpdateMethod = GridUpdateMethods.GridParent _
        Or UpdateMethod = GridUpdateMethods.GridAutomatic Then
            dgvCTS.SetState(True)
        End If
    End Sub

    'Adding
    Public Event dgv_RowAddStarted(ByRef CurGridRow As DataGridViewRow)

    Private Sub dgvCTS_UserAddedRow(sender As Object, e As DataGridViewRowEventArgs) Handles dgvCTS.UserAddedRow

        RaiseEvent dgv_RowAddStarted(e.Row)

        If UpdateMethod = GridUpdateMethods.GridParent Or UpdateMethod = GridUpdateMethods.GridAutomatic Then
            dgvCTS.SetState(True)
        End If

    End Sub

    Private Sub dgvCTS_RowsAdded(sender As Object, e As DataGridViewRowsAddedEventArgs) Handles dgvCTS.RowsAdded
        Debug.Print("CTSGrid: ROW IS NOW ADDED..is it detached?" & e.RowIndex)
    End Sub

#End Region

#Region "Error Handling"

    'Error Handling 
    Private Sub dgvCTS_CellErrorTextChanged(sender As Object, e As DataGridViewCellEventArgs) Handles dgvCTS.CellErrorTextChanged
        Dim ThisCell As DataGridViewCell = dgvCTS.Item(e.ColumnIndex, e.RowIndex)
        ChangeErrorList(e.RowIndex, e.ColumnIndex, ThisCell.ErrorText)

        Dim thisrow As DataGridViewRow = dgvCTS.Rows(e.RowIndex)
        If Not HasCellErrors(thisrow) Then
            ChangeErrorList(e.RowIndex, -1, String.Empty)
        End If

    End Sub

    Private Sub dgvCTS_RowErrorTextChanged(sender As Object, e As DataGridViewRowEventArgs) Handles dgvCTS.RowErrorTextChanged
        ChangeErrorList(e.Row.Index, -1, e.Row.ErrorText)
    End Sub

    Private Sub ChangeErrorList(ByVal r As Integer, ByVal c As Integer, text As String)

        'Remove any preexisting messages for this row
        Dim lvicollection As ListViewItem() = lvwErrors.Items.Find("R" & r & "C" & c, False)
        For Each lvi As ListViewItem In lvicollection
            lvi.Remove()
        Next

        'Add a message if Errortext exists 
        If text <> String.Empty Then
            Dim lvi As New ListViewItem(lvwErrors.Items.Count + 1)
            lvi.Name = "R" & r & "C" & c
            lvi.SubItems.Add(text)
            lvi.SubItems.Add(r)
            lvi.SubItems.Add(c)
            lvi.SubItems.Add("R" & r & "C" & c)
            lvwErrors.Items.Add(lvi)
        End If

        Select Case lvwErrors.Items.Count
            Case 0
                lvwErrors.Visible = False
            Case 1
                lvwErrors.Height = 20
                lvwErrors.Visible = True
            Case 2
                lvwErrors.Height = 40
                lvwErrors.Visible = True
            Case Is >= 3
                lvwErrors.Height = 60
                lvwErrors.Visible = True
        End Select

        lvwErrors.ListViewItemSorter = New ListViewItemComparer(4)
        lvwErrors.Sort()

        Dim i As Integer = 0
        For Each lvi As ListViewItem In lvwErrors.Items
            i += 1
            lvi.Text = i
        Next
    End Sub

    Private Sub dgvCTS_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles dgvCTS.DataError

        If e.Exception IsNot Nothing Then
            'Debug.Print("DATAERROR-->" & dgvCTS.Rows(e.RowIndex).Cells(e.ColumnIndex).EditedFormattedValue)
            'MyEC.BackColor = Color.Pink
            'dgvCTS.Rows(e.RowIndex).Cells(e.ColumnIndex).Style.BackColor = Color.Pink
            'dgvCTS.Rows(e.RowIndex).Cells(e.ColumnIndex).ErrorText = "Invalid Data Entered In Cell"
            'e.ThrowException = False
            e.Cancel = False
        End If

    End Sub

    Private Sub lvwErrors_DoubleClick(sender As Object, e As EventArgs) Handles lvwErrors.DoubleClick

        If lvwErrors.SelectedItems.Count > 0 Then
            Dim gotoRow As Integer = lvwErrors.SelectedItems.Item(0).SubItems(2).Text
            Dim gotoCol As Integer = lvwErrors.SelectedItems.Item(0).SubItems(3).Text
            If dgvCTS.CurrentCell.RowIndex = gotoRow And dgvCTS.CurrentCell.ColumnIndex = gotoCol Then
                If Not dgvCTS.Rows(gotoRow).Displayed Then
                    dgvCTS.FirstDisplayedScrollingRowIndex = gotoRow
                End If
            Else
                dgvCTS.CurrentCell = dgvCTS.Rows(gotoRow).Cells(gotoCol)
            End If
            dgvCTS.Focus()
            dgvCTS.BeginEdit(True)
            Debug.Print("CTSGrid: performed DC error focus")
        End If
    End Sub

    Public Function HasErrors() As Boolean

        For Each r As DataGridViewRow In Me.dgvCTS.Rows

            If r.ErrorText <> String.Empty Then
                Return True
            End If

            If HasCellErrors(r) Then
                Return True
            End If
        Next
        Return False

    End Function

#End Region

#Region "General Functions and Events"

    Private Sub btnGridRefresh_Click(sender As Object, e As EventArgs) Handles btnGridRefresh.Click
        RefreshData()
    End Sub

    Private Sub dgvCTS_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles dgvCTS.ColumnWidthChanged
        If Not Initializing And Not IsLoading And Not dgvCTS.IsSuspended Then
            GridSettingIsDirty = True
        End If
        If (dgvTotals.Columns.Count - 1) >= e.Column.Index Then
            dgvTotals.Columns(e.Column.Index).Width = dgvCTS.Columns(e.Column.Index).Width
            dgvTotals.HorizontalScrollingOffset = dgvCTS.HorizontalScrollingOffset
        End If
    End Sub

    Private Sub dgvTotals_ColumnWidthChanged(sender As Object, e As DataGridViewColumnEventArgs) Handles dgvTotals.ColumnWidthChanged
        If (dgvCTS.Columns.Count - 1) >= e.Column.Index Then
            dgvCTS.Columns(e.Column.Index).Width = dgvTotals.Columns(e.Column.Index).Width
            dgvTotals.HorizontalScrollingOffset = dgvCTS.HorizontalScrollingOffset
        End If
    End Sub

    Private Sub dgvCTS_ColumnDisplayIndexChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles dgvCTS.ColumnDisplayIndexChanged
        If Not Initializing And Not IsLoading And Not dgvCTS.IsSuspended Then
            GridSettingIsDirty = True
            If miShowTotals.Checked Then
                dgvTotals.Columns(e.Column.Name).DisplayIndex = e.Column.DisplayIndex
            End If
        End If
    End Sub

    Public Sub dgvCTS_AddColumn(ByVal HeaderText As String,
                        ByVal Name As String,
                        ByVal ReadO As Boolean,
                        ByVal HeaderStyle As DataGridViewContentAlignment,
                        ByVal CellStyle As DataGridViewContentAlignment,
                        ByVal Width As Integer)
        Dim col As New DataGridViewTextBoxColumn
        col.HeaderText = HeaderText
        col.Name = Name
        col.ReadOnly = ReadO
        col.HeaderCell.Style.Alignment = HeaderStyle
        col.DefaultCellStyle.Alignment = CellStyle
        col.Width = Width
        dgvCTS.Columns.Add(col)

    End Sub

    Public Sub SetGridMessage(ByVal strMessage As String)
        'lblGridStatus.Text = $"{strMessage} ts1:{ts1.ElapsedMilliseconds} ts2:{ts2.ElapsedMilliseconds} ts3:{ts3.ElapsedMilliseconds}"
        lblGridStatus.Text = $"{strMessage} in {ts1.ElapsedMilliseconds / 1000} seconds"
    End Sub

    Public Sub SuspendOn()
        dgvCTS.IsSuspended = True
        lblSuspend.Visible = True
    End Sub
    Public Sub SuspendOff()
        dgvCTS.IsSuspended = False
        lblSuspend.Visible = False
    End Sub

    Private Sub dgvCTS_Resize(sender As Object, e As EventArgs) Handles dgvCTS.Resize

        If IsVScrollBarVisible(dgvCTS) Then
            pnlTotFiller.Visible = True
        Else
            pnlTotFiller.Visible = False
        End If

        dgvTotals.HorizontalScrollingOffset = dgvCTS.HorizontalScrollingOffset
    End Sub

    Public Function GetSelectedRows() As DataTable

        Dim dt As New DataTable
        For Each dgvc As DataGridViewColumn In dgvCTS.Columns
            If dgvc.ValueType IsNot Nothing Then
                dt.Columns.Add(dgvc.Name, dgvc.ValueType)
            Else
                dt.Columns.Add(dgvc.Name)
            End If
        Next

        If dgvCTS.SelectedRows.Count > 0 Then
            For Each dgvr As DataGridViewRow In dgvCTS.SelectedRows
                Dim dr As DataRow = dt.NewRow()
                For Each col As DataColumn In dt.Columns
                    Dim idx = dgvCTS.Columns(col.ColumnName).Index
                    dr(col.ColumnName) = dgvr.Cells(idx).Value
                Next

                dt.Rows.Add(dr)
            Next
        End If

        Return dt

    End Function

    Public Function GetCurrentRow() As DataTable

        If dgvCTS.CurrentRow Is Nothing Then
            Return Nothing
        Else
            Dim dt As New DataTable
            For Each dgvc As DataGridViewColumn In dgvCTS.Columns
                If dgvc.ValueType IsNot Nothing Then
                    dt.Columns.Add(dgvc.Name, dgvc.ValueType)
                Else
                    dt.Columns.Add(dgvc.Name)
                End If
            Next

            Dim dgvr As DataGridViewRow
            dgvr = dgvCTS.Rows(dgvCTS.CurrentRow.Index)

            Dim dr As DataRow = dt.NewRow()
            For Each col As DataColumn In dt.Columns
                Dim idx = dgvCTS.Columns(col.ColumnName).Index
                dr(col.ColumnName) = dgvr.Cells(idx).Value
            Next

            dt.Rows.Add(dr)
            Return dt
        End If

    End Function

    Private Sub tmrIdle_Tick(sender As Object, e As EventArgs) Handles tmrIdle.Tick

        If gAutoRefresh <> 0 AndAlso LastRefresh <> Date.MinValue Then
            If DateDiff(DateInterval.Second, LastRefresh, Now) > gAutoRefresh Then
                LastRefresh = Date.MinValue
                Me.RefreshData()
            End If
        End If

    End Sub

#End Region

#Region "Exposed Events"

    'Exposed Events
    Public Event dgv_GridLoadingComplete(sender As Object)
    Public Event dgv_GridLoadingStart(sender As Object)
    Public Event dgv_GridPreDataRequest(sender As Object)

    Public Event dgv_RowsRemoved(ByVal sender As System.Object, ByVal e As DataGridViewRowsRemovedEventArgs)
    Private Sub dgvCTS_RowsRemoved(sender As Object, e As DataGridViewRowsRemovedEventArgs) Handles dgvCTS.RowsRemoved
        If Not Initializing And Not IsLoading And Not dgvCTS.IsSuspended Then
            If UpdateMethod = GridUpdateMethods.GridParent _
            Or UpdateMethod = GridUpdateMethods.GridAutomatic Then
                dgvCTS.SetState(True)
            End If
            RaiseEvent dgv_RowsRemoved(sender, e)
        End If
    End Sub

    Public Event CurrentRowChanged(ByVal sender As System.Object, ByVal CurrentRow As DataGridViewRow)
    Public Event dgv_CurrentCellChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    Private Sub dgvCTS_CurrentCellChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvCTS.CurrentCellChanged

        If Not Initializing And Not IsLoading And Not dgvCTS.IsSuspended Then
            RaiseEvent dgv_CurrentCellChanged(sender, e)

            If dgvCTS.CurrentRow Is Nothing Then
                If dgvCTS.CurRowIndex <> -1 Then
                    dgvCTS.CurRowIndex = -1
                    If SelectionDelayTimer IsNot Nothing Then SelectionDelayTimer.Stop()
                    RaiseEvent CurrentRowChanged(sender, dgvCTS.CurrentRow)
                End If
            Else
                If dgvCTS.CurRowIndex <> dgvCTS.CurrentRow.Index Then

                    If SelectionDelay > 0 Then
                        If SelectionDelayTimer Is Nothing Then
                            SelectionDelayTimer = New System.Windows.Forms.Timer
                            SelectionDelayTimer.Interval = SelectionDelay
                            AddHandler SelectionDelayTimer.Tick, New EventHandler(AddressOf SelectionDelayHandler)
                        End If
                        SelectionDelayTimer.Stop()
                        SelectionDelayTimer.Start()
                    Else
                        If SelectionDelayTimer IsNot Nothing Then SelectionDelayTimer.Stop()
                        dgvCTS.CurRowIndex = dgvCTS.CurrentRow.Index
                        RaiseEvent CurrentRowChanged(sender, dgvCTS.CurrentRow)
                    End If

                End If
            End If
        End If
    End Sub

    Private Sub SelectionDelayHandler(ByVal sender As Object, ByVal e As EventArgs)
        SelectionDelayTimer.Stop()
        If dgvCTS.CurrentRow Is Nothing Then
            dgvCTS.CurRowIndex = -1
        Else
            dgvCTS.CurRowIndex = dgvCTS.CurrentRow.Index
        End If

        RaiseEvent CurrentRowChanged(sender, dgvCTS.CurrentRow)
        Debug.Print("Selection Delay: " & dgvCTS.CurRowIndex)
    End Sub

    Public Event dgv_CellLeave(sender As Object, e As DataGridViewCellEventArgs)
    Private Sub dgvCTS_CellLeave(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs) Handles dgvCTS.CellLeave
        If Not Initializing And Not IsLoading And Not dgvCTS.IsSuspended Then
            Debug.Print("CTSGrid: CellLeave row " & e.RowIndex & " col " & e.ColumnIndex)

            If UpdateMethod = GridUpdateMethods.GridParent Or UpdateMethod = GridUpdateMethods.GridAutomatic Then
                If dgvCTS.Rows(e.RowIndex).Cells(e.ColumnIndex).ReadOnly = False Then
                    dgvCTS.CheckState()
                End If
            End If

            RaiseEvent dgv_CellLeave(sender, e)
        End If
    End Sub

    Public Event CustomRowOptionBefore(RowIndex As Integer, ByRef Cancel As Boolean)
    Public Event CustomRowOption(sender As Object, RowOption As RowOption, RowIndex As Integer)
    Private Sub cmsRowOptions_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles cmsRowOptions.ItemClicked
        'todo send back the rowoption object
        RaiseEvent CustomRowOption(sender, e.ClickedItem.Tag, dgvCTS.CurRowHeaderIndex)
    End Sub


    Public Event GridFunctionSelected(sender As Object, GridFunction As GridFunction)
    Private Sub tsFunctions_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles tsFunctions.ItemClicked
        RaiseEvent GridFunctionSelected(sender, e.ClickedItem.Tag)
    End Sub


    Public Event dgv_MouseDoubleClick(sender As Object, e As MouseEventArgs)
    Private Sub dgvCTS_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles dgvCTS.MouseDoubleClick
        RaiseEvent dgv_MouseDoubleClick(sender, e)
    End Sub


#End Region





#Region "Date Filter Menu Items"

    Private Sub miCurrentDayFilter_Click(sender As Object, e As EventArgs) Handles miCurrentDayFilter.Click
        Dim d1, d2 As Date
        GetDateRange("Current Day", Now, d1, d2)
        If CurColumn.ProviderTypeCode = "12" Then
            Dim nf = G.Columns(CurColumn.Name).NewFilter(OperatorName.Equals, d1.ToString("yyyy-MM-dd"))
            nf.Description = "Equals Today"
            UFilters.Conditions.Add(nf)
        End If
        If CurColumn.ProviderTypeCode = "14" Then
            Dim nf = G.Columns(CurColumn.Name).NewFilter($"Date({CurColumn.Name}) = '{d1.ToString("yyyy-MM-dd")}'")
            nf.Description = "Equals Today"
            UFilters.Conditions.Add(nf)
        End If
        RefreshData()
    End Sub

    Private Sub miCurrentMonthFilter_Click(sender As Object, e As EventArgs) Handles miCurrentMonthFilter.Click
        Dim d1, d2 As Date
        GetDateRange("Current Month", Now, d1, d2)
        If CurColumn.ProviderTypeCode = "12" Then
            Dim cond = UFilters.AddFilterRange(CurColumn.Name, d1.ToString("yyyy-MM-dd"), d2.ToString("yyyy-MM-dd"))
            cond.Description = "Current Month"
        End If
        If CurColumn.ProviderTypeCode = "14" Then
            Dim cond = UFilters.AddFilter(CurColumn.Name, $"Date({CurColumn.Name}) Between '{d1.ToString("yyyy-MM-dd")}' and '{d2.ToString("yyyy-MM-dd")}'")
            cond.Description = "Current Month"
        End If
        RefreshData()
    End Sub

    Private Sub miCurrentYearFilter_Click(sender As Object, e As EventArgs) Handles miCurrentYearFilter.Click
        Dim d1, d2 As Date
        GetDateRange("Current Year", Now, d1, d2)
        If CurColumn.ProviderTypeCode = "12" Then
            Dim cond = UFilters.AddFilterRange(CurColumn.Name, d1.ToString("yyyy-MM-dd"), d2.ToString("yyyy-MM-dd"))
            cond.Description = "Current Year"
        End If
        If CurColumn.ProviderTypeCode = "14" Then
            Dim cond = UFilters.AddFilter(CurColumn.Name, $"Date({CurColumn.Name}) Between '{d1.ToString("yyyy-MM-dd")}' and '{d2.ToString("yyyy-MM-dd")}'")
            cond.Description = "Current Year"
        End If
        RefreshData()
    End Sub

    Private Sub miPriorMonthFilter_Click(sender As Object, e As EventArgs) Handles miPriorMonthFilter.Click
        Dim d1, d2 As Date
        GetDateRange("Prior Month", Now, d1, d2)
        If CurColumn.ProviderTypeCode = "12" Then
            AddFilter(CurColumn.Name, "BETWEEN", d1.ToString("yyyy-MM-dd"), d2.ToString("yyyy-MM-dd"), , "System.DateTime")
        End If
        If CurColumn.ProviderTypeCode = "14" Then
            AddFilter(CurColumn.Name, "BETWEEN", d1.ToString("yyyy-MM-dd-HH.mm.ss"), d2.ToString("yyyy-MM-dd-HH.mm.ss"), , "System.DateTime")
        End If
        RefreshData()
    End Sub

    Private Sub miPriorYearFilter_Click(sender As Object, e As EventArgs) Handles miPriorYearFilter.Click
        Dim d1, d2 As Date
        GetDateRange("Prior Year", Now, d1, d2)
        If CurColumn.ProviderTypeCode = "12" Then
            AddFilter(CurColumn.Name, "BETWEEN", d1.ToString("yyyy-MM-dd"), d2.ToString("yyyy-MM-dd"), , "System.DateTime")
        End If
        If CurColumn.ProviderTypeCode = "14" Then
            AddFilter(CurColumn.Name, "BETWEEN", d1.ToString("yyyy-MM-dd-HH.mm.ss"), d2.ToString("yyyy-MM-dd-HH.mm.ss"), , "System.DateTime")
        End If
        RefreshData()
    End Sub

    Private Sub Last30DaysToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles Last30DaysToolStripMenuItem.Click
        Dim d1 As Date
        d1 = DateAdd(DateInterval.Day, -30, Today)
        If CurColumn.ProviderTypeCode = "12" Then
            AddFilter(CurColumn.Name, ">=", d1.ToString("yyyy-MM-dd"), , , "System.DateTime")
        End If
        If CurColumn.ProviderTypeCode = "14" Then
            AddFilter(CurColumn.Name, ">=", d1.ToString("yyyy-MM-dd-HH.mm.ss"), , , "System.DateTime")
        End If
        RefreshData()
    End Sub

    Private Sub Last60DaysToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles Last60DaysToolStripMenuItem.Click
        Dim d1 As Date
        d1 = DateAdd(DateInterval.Day, -60, Today)
        If CurColumn.ProviderTypeCode = "12" Then
            AddFilter(CurColumn.Name, ">=", d1.ToString("yyyy-MM-dd"), , , "System.DateTime")
        End If
        If CurColumn.ProviderTypeCode = "14" Then
            AddFilter(CurColumn.Name, ">=", d1.ToString("yyyy-MM-dd-HH.mm.ss"), , , "System.DateTime")
        End If
        RefreshData()
    End Sub

    Private Sub Last90DaysToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles Last90DaysToolStripMenuItem.Click
        Dim d1 As Date
        d1 = DateAdd(DateInterval.Day, -90, Today)
        If CurColumn.ProviderTypeCode = "12" Then
            AddFilter(CurColumn.Name, ">=", d1.ToString("yyyy-MM-dd"), , , "System.DateTime")
        End If
        If CurColumn.ProviderTypeCode = "14" Then
            AddFilter(CurColumn.Name, ">=", d1.ToString("yyyy-MM-dd-HH.mm.ss"), , , "System.DateTime")
        End If
        RefreshData()
    End Sub
#End Region

#Region "TotalsView"
    Private Sub dgvCTS_Scroll(sender As Object, e As ScrollEventArgs) Handles dgvCTS.Scroll
        If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
            dgvTotals.HorizontalScrollingOffset = e.NewValue
        End If
    End Sub

    Private Sub miShowTotals_Click(sender As Object, e As EventArgs) Handles miShowTotals.Click
        SyncTotalsGrid()
    End Sub

    Private Sub SyncTotalsGrid()

        If miShowTotals.Checked Then

            dgvTotals.Columns.Clear()
            For Each dgvc As DataGridViewColumn In dgvCTS.Columns

                If dgvc.GetType.Name = "DataGridViewColumnCTS" Then
                    Dim col As DataGridViewColumnCTS = dgvc.Clone
                    dgvTotals.Columns.Add(col)
                Else
                    Dim col As New DataGridViewTextBoxColumn
                    col.Name = dgvc.Name
                    col.Width = dgvc.Width
                    col.Visible = dgvc.Visible
                    dgvTotals.Columns.Add(col)
                End If
            Next

            dgvTotals.Rows.Add()
            dgvTotals.RowHeadersWidth = dgvCTS.RowHeadersWidth
            dgvTotals.Rows(0).HeaderCell.Style.Padding = New Padding(2, 3, 0, 3)
            dgvTotals.Rows(0).HeaderCell.Style.Font = New Font("Lucida Bright", 8)
            dgvTotals.Rows(0).HeaderCell.Value = "∑"
            dgvTotals.Rows(0).HeaderCell.ToolTipText = "Totals"

            dgvTotals.Width = dgvCTS.Width
            dgvTotals.Height = dgvTotals.Rows(0).Height + 0
            pnlTotals.Height = dgvTotals.Rows(0).Height + 0

            ConfigTotalsRow()
            pnlTotals.Visible = True
            Me.ShowTotals = True
        Else
            pnlTotals.Visible = False
            Me.ShowTotals = False
        End If

    End Sub

    Private Sub ConfigTotalsRow()
        Try

            For Each GridColumn As DataGridViewColumn In dgvCTS.Columns

                If GridColumn.GetType.Name = "DataGridViewColumnCTS" Then
                    Dim dgvc_CTS As DataGridViewColumnCTS = GridColumn
                    If dgvc_CTS.CTSSumCol Then
                        Dim Idx As Integer = dgvc_CTS.Index
                        Dim strColName As String = dtGridData.Columns(Idx).ColumnName
                        dgvTotals.Rows(0).Cells(Idx).Value = dtGridData.Compute("Sum(" & strColName & ")", "TRUE")
                        dgvTotals.Rows(0).Cells(Idx).Style.BackColor = Color.White
                        dgvTotals.Rows(0).Cells(Idx).Style.ForeColor = Color.Black
                        dgvTotals.Rows(0).Cells(Idx).Style.SelectionBackColor = Color.White
                        dgvTotals.Rows(0).Cells(Idx).Style.SelectionForeColor = Color.Black
                    End If
                End If

            Next

        Catch ex As Exception
            'MsgBox("Could not Compute Sum on column " & dgvc.DataPropertyName)
        End Try

    End Sub


    Public Sub RecalculateTotals(ByVal ColumnName As String)

        If ShowTotals Then
            If dgvCTS.Columns.Contains(ColumnName) Then
                If dgvCTS.Columns(ColumnName).GetType.Name = "DataGridViewColumnCTS" Then
                    Dim dgvc_CTS As DataGridViewColumnCTS = dgvCTS.Columns(ColumnName)
                    If dgvc_CTS.CTSSumCol Then
                        dgvTotals.Rows(0).Cells(dgvc_CTS.Index).Value = dtGridData.Compute("Sum(" & ColumnName & ")", "TRUE")
                    End If
                End If
            End If
        End If
    End Sub

    Public Sub RecalculateTotals()

        Try
            If ShowTotals Then

                For Each GridColumn As DataGridViewColumn In dgvCTS.Columns

                    If GridColumn.GetType.Name = "DataGridViewColumnCTS" Then
                        Dim dgvc_CTS As DataGridViewColumnCTS = GridColumn
                        If dgvc_CTS.CTSSumCol Then
                            Dim Idx As Integer = dgvc_CTS.Index
                            Dim strColName As String = dtGridData.Columns(Idx).ColumnName
                            dgvTotals.Rows(0).Cells(Idx).Value = dtGridData.Compute("Sum(" & strColName & ")", "TRUE")

                        End If
                    End If
                Next
            End If

        Catch ex As Exception
            'MsgBox("Could not Compute Sum on column " & dgvc.DataPropertyName)
        End Try
    End Sub

    Private Sub miTotalsTop_Click(sender As Object, e As EventArgs) Handles miTotalsTop.Click
        pnlTotals.Dock = DockStyle.Top
    End Sub

    Private Sub miTotalsBottom_Click(sender As Object, e As EventArgs) Handles miTotalsBottom.Click
        pnlTotals.Dock = DockStyle.Bottom
    End Sub

    Private Sub miHideTotals_Click(sender As Object, e As EventArgs) Handles miHideTotals.Click
        pnlTotals.Visible = False
        Me.ShowTotals = False
    End Sub
#End Region

#Region "Wrapping Features"

    Private Sub ApplyWrapping()
        Try

            If AllowWrapping Then
                For Each dgvc In Me.dgvCTS.Columns
                    dgvc.DefaultCellStyle.WrapMode = DataGridViewTriState.True
                Next
                Me.dgvCTS.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders
            Else
                For Each dgvc In Me.dgvCTS.Columns
                    dgvc.DefaultCellStyle.WrapMode = DataGridViewTriState.False
                Next
                Me.dgvCTS.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
            End If

        Catch ex As Exception

        End Try

    End Sub

    Private Sub miAllowWrapping_Click(sender As Object, e As EventArgs) Handles miAllowWrapping.Click
        If AllowWrapping Then
            AllowWrapping = False
        Else
            AllowWrapping = True
        End If
    End Sub

#End Region






    'Distinct Value filtering
    Private Sub miDistinctValues_DropDownItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles miDistinctValues.DropDownItemClicked
        Dim nf = G.Columns(CurColumn.Name).NewFilter(OperatorName.Equals, e.ClickedItem.Text)
        UFilters.Conditions.Add(nf)
        RefreshData()
    End Sub

    Private Sub miDistinctValues_DropDownOpening(sender As Object, e As EventArgs) Handles miDistinctValues.DropDownOpening

        If miDistinctValues.Tag Is Nothing OrElse miDistinctValues.Tag <> CurColumn.Name Then
            miDistinctValues.DropDownItems.Clear()
            Dim sql As String = $"select distinct {CurColumn.Name} as DV {G.FromClause} fetch first 30 rows only"
            Dim dt As DataTable = C2App.Data.GetTable(sql)
            If dt IsNot Nothing Then
                For Each Row As DataRow In dt.Rows
                    miDistinctValues.DropDownItems.Add(Row("DV"))
                Next
            End If
        End If

        miDistinctValues.Tag = CurColumn.Name
    End Sub




    Public Sub ShowSettings()
        Using ucSet As New ucSettingTab(Me)
            ucSet.ShowDialog()
        End Using
    End Sub

    Private Sub dgvCTS_SelectionChanged(sender As Object, e As EventArgs) Handles dgvCTS.SelectionChanged
        'MsgBox($"SelectionChanged CRI={dgvCTS.CurrentRow.Index} SRC={dgvCTS.SelectedRows.Count} SCC={dgvCTS.SelectedCells.Count}")
    End Sub

    Private Sub dgvCTS_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles dgvCTS.DataBindingComplete
        Debug.Print(e.ListChangedType.ToString)
    End Sub

    Private Sub dgvCTS_RowHeaderMouseDoubleClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles dgvCTS.RowHeaderMouseDoubleClick
        dgvCTS.Rows(e.RowIndex).Height = dgvCTS.Rows(e.RowIndex).Height + 20
        dgvCTS.Rows(e.RowIndex).DefaultCellStyle.Padding = New Padding(
                dgvCTS.Rows(e.RowIndex).DefaultCellStyle.Padding.Left,
                        dgvCTS.Rows(e.RowIndex).DefaultCellStyle.Padding.Right,
                                dgvCTS.Rows(e.RowIndex).DefaultCellStyle.Padding.Top,
        dgvCTS.Rows(e.RowIndex).DefaultCellStyle.Padding.Bottom + 20)
    End Sub
End Class

