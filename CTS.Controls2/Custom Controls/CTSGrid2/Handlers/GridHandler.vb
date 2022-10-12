Imports CTS.Extensions
Imports System.Text.RegularExpressions

Public Class Grid2Handler

#Region "Loading/Saving Definition From DB2"

    'Get Grid Data from Database
    Public Function LoadGrid(ByVal GridID As Integer, ReleaseID As Integer) As Grid
        Dim grd As Grid = Nothing

        If GridID > 0 Then
            grd = LoadGridHeader(GridID, ReleaseID)
            grd.Columns = LoadGridColumns(GridID, ReleaseID)

            'Apply hidden RRN columns (for update rows)
            LoadRRNColumns(grd)

            grd.RowOptions = LoadCustomOptions(GridID, ReleaseID)
            grd.GridFunctions = LoadCustomFunctions(GridID, ReleaseID)
            grd.DynamicStyles = LoadDynamicStyles(GridID, ReleaseID)

            'Save the internally set where in a property (that can be overridden)
            'gInternalWhere = grd.WhereClause

            'Load Saved Filters on first load
            'If SaveGridName <> GridName Then
            'LoadUserFilters(UFilters, "*CURRENT", GridName, "*LAST")
            'End If

        End If

        Return grd
    End Function


    'Get Grid Data from DNPF2500
    Public Function LoadGridHeader(ByVal GridID As Integer, ReleaseID As Integer) As Grid
        Dim myGrid As Grid = Nothing

        If GridID > 0 Then
            Dim row = C2App.Data.GetRow($"Select * From DNPF2500 Where ghid = {GridID} and ghrid = {ReleaseID}")
            If row IsNot Nothing Then
                row.TrimColumns()
                myGrid = New Grid
                myGrid.ID = GridID
                myGrid.VersionID = ReleaseID
                myGrid.Name = row("GHName")
                myGrid.Title = row("GHTitle")
                myGrid.LibraryList = row("GHLList")
                myGrid.FromClause = row("GHFCO")
                myGrid.WhereClause = row("GHWhere")
                myGrid.OrderByClause = row("GHOrder")
                myGrid.GroupByClause = row("GHGroup")
                myGrid.Distinct = row("GHDistinct").ToString.ToBoolean
                myGrid.TotalsRow = row("GHRTot").ToString.ToBoolean

                Dim rowDS = C2App.Data.GetRow($"Select * From DNPF2505 Where gsid = {GridID} and gsrid = {ReleaseID}")
                If rowDS IsNot Nothing Then
                    myGrid.AltDataSource = New GridDataSource
                    myGrid.AltDataSource.Server = rowDS("GSSOURCE").ToString.Trim
                    myGrid.AltDataSource.Database = rowDS("GSDATABASE").ToString.Trim
                    myGrid.AltDataSource.SourceType = rowDS("GSTYPE").ToString.Trim
                    myGrid.AltDataSource.ConnectionString = rowDS("GSCONNSTR").ToString.Trim
                Else
                    myGrid.AltDataSource = Nothing
                End If

            End If
        End If

        Return myGrid
    End Function



    Public Function LoadGridColumns(ByVal GridID As Integer, ReleaseID As Integer) As GridColumnCollection
        Dim NewList As New GridColumnCollection

        If GridID > 0 Then
            Dim strSQL = String.Format(
                         "Select * From DNPF2510 " &
                         "Left Join " &
                         "(SELECT a.*, " &
                         " ROW_NUMBER() OVER (PARTITION BY gpfield ORDER BY gpfield) RowRank " &
                         " FROM DNPF2511 a " &
                         " WHERE gpid = {0} and gprid = {1} and gpuser = '{2}') f2511 " &
                         "On GCID = f2511.GPID and GCRID = GPRID and GCFIELD = f2511.GPFIELD and f2511.RowRank = 1 " &
                         "Where GCID = {0} and GCRID = {1} and GCSEQ > 0 " &
                         "Order by f2511.GPSEQ, GCSEQ",
                         GridID, ReleaseID, C2App.User)
            Dim dt As DataTable = C2App.Data.GetTable(strSQL)

            'Columns Check for Column level security 
            For Each row As DataRow In dt.Rows
                Try
                    row.TrimColumns
                    Dim gc As New GridColumn

                    'One of these fields is sometimes NULL/Nothing
                    If IsDBNull(row("GPWidth")) OrElse row("GPWidth") = 0 Then
                        gc.Width = row("GCWIDTH")
                    Else
                        gc.Width = row("GPWidth")
                    End If

                    'One of these fields is sometimes NULL/Nothing
                    If IsDBNull(row("GPSort")) OrElse row("GPSort") = 0 Then
                        gc.SortSeq = row("GCSort")
                    Else
                        gc.SortSeq = row("GPSort")
                    End If

                    If gc.SortSeq > 0 Then
                        If IsDBNull(row("GPDIR")) OrElse row("GPDIR") = String.Empty Then
                            gc.SortOrder = SortOrder.Ascending
                        Else
                            If row("GPDIR") = "ASC" Then
                                gc.SortOrder = SortOrder.Ascending
                            Else
                                gc.SortOrder = SortOrder.Descending
                            End If
                        End If
                    End If

                    gc.SuppressTrim = row("GCFLAG1").ToString.ToBoolean
                    gc.Field = row("GCField")
                    gc.Name = row("GCField")
                    gc.SQLFunction = row("GCFunction")
                    gc.Seq = row("GCSEQ")
                    gc.HeaderText = row("GCHeader")
                    gc.ProviderTypeCode = row("GCPTYPE")
                    gc.Size = row("GCSIZE")
                    gc.Scale = row("GCScale")
                    gc.CastCCSID37 = row("GCJDECVT").ToString.ToBoolean
                    gc.Format = row("GCFormat")
                    gc.SumColumn = row("GCSumCol").ToString.ToBoolean
                    gc.SumFormat = row("GCSumFmt")
                    gc.Preselect = row("GCPreSel").ToString.ToBoolean
                    gc.Updatable = row("GCAlwUpd").ToString.ToBoolean

                    If IsDBNull(row("GPHIDDEN")) Then
                        If row("GCHIDDEN") = "Y" Then gc.Hidden = True
                    Else
                        If row("GPHIDDEN") = "Y" Then gc.Hidden = True
                    End If

                    [Enum].TryParse(row("GCColTyp"), gc.ColumnType)

                    If Not String.IsNullOrWhiteSpace(row("GCPrmtSQL")) Then
                        If gc.ColumnType = GridColumnTypes.COMBOBOX_SQL Then
                            gc.ValueTable = C2App.Data.GetTable(row("GCPrmtSQL"))

                        ElseIf gc.ColumnType = GridColumnTypes.COMBOBOX_VAL Then
                            Try
                                Dim Values() As String = Split(row("GCPrmtSQL"), ",")
                                gc.ValueList = Values.ToList
                            Catch ex As Exception
                                Throw New Exception($"Errors occured translating {gc.Field} value list (GCPRMTSQL)")
                            End Try
                        End If
                    End If

                    'Check for any security contraints
                    [Enum].TryParse(row("GCSecurity"), gc.SecurityType)

                    [Enum].TryParse(row("GCAlign"), gc.Alignment)

                    Dim GCColSecFG As String = IIf(IsDBNull(row("GCColSecFG")), "", row("GCColSecFG"))
                    Dim GCColSecFN As String = IIf(IsDBNull(row("GCColSecFN")), "", row("GCColSecFN"))
                    If Not String.IsNullOrEmpty(Trim(GCColSecFG)) AndAlso Not String.IsNullOrEmpty(Trim(GCColSecFN)) Then
                        If Not C2App.HasPermission(GCColSecFG, GCColSecFN) Then
                            gc.Restrict = True
                        End If
                    End If

                    NewList.Add(gc)

                Catch ex As Exception
                    'possible duplicate?  
                End Try

            Next
        End If

        Return NewList
    End Function



    'Get Default Sort Columns 
    Public Sub SetDefaultSort_XXX(ByVal Gridname As String, ByRef dgvCTS As DataGridViewCT2)
        Dim dtSort As DataTable
        Dim strSQL As String
        Dim Idx As Integer

        dgvCTS.ClearSorter()

        strSQL = String.Format("Select GPFIELD, GPSORT, GPDIR From DNPF2511 " &
                     "Where GPNAME = '{0}' and GPUSER = '{1}' and GPSORT > 0 " &
                     "Order by GPSORT", Trim(Gridname), C2App.User)
        dtSort = C2App.Data.GetTable(strSQL)
        If dtSort.Rows.Count > 0 Then
            dgvCTS.SorterCount = dtSort.Rows.Count
            For Each r As DataRow In dtSort.Rows
                If Trim(r.Item("GPDIR")) = String.Empty Then r.Item("GPDIR") = "ASC"
                Try
                    Idx = dgvCTS.Columns(Trim(r.Item("GPFIELD"))).Index
                    dgvCTS.CustomSorter(Idx).Seq = r.Item("GPSORT")
                    dgvCTS.CustomSorter(Idx).Order = Trim(r.Item("GPDIR"))
                Catch ex As Exception
                    dgvCTS.ClearSorter()
                End Try
            Next
        Else
            strSQL = String.Format("Select GCFIELD, GCSORT, GCDIR From DNPF2510 " &
                         "Where GCNAME = '{0}' and GCSORT > 0 " &
                         "Order by GCSORT", Trim(Gridname))
            dtSort = C2App.Data.GetTable(strSQL)
            If dtSort.Rows.Count > 0 Then
                dgvCTS.SorterCount = dtSort.Rows.Count
                For Each r As DataRow In dtSort.Rows
                    If Trim(r.Item("GCDIR")) = String.Empty Then r.Item("GCDIR") = "ASC"
                    Try
                        Idx = dgvCTS.Columns(Trim(r.Item("GCFIELD"))).Index
                        dgvCTS.CustomSorter(Idx).Seq = r.Item("GCSORT")
                        dgvCTS.CustomSorter(Idx).Order = Trim(r.Item("GCDIR"))
                    Catch ex As Exception
                        dgvCTS.ClearSorter()
                    End Try
                Next
            End If
        End If

        dgvCTS.ApplySorter()

    End Sub

    'Get Default Sort Columns 
    Public Sub SetDefaultSort(ByVal G As Grid, ByRef dgvCTS As DataGridViewCT2)
        Dim Idx As Integer

        dgvCTS.ClearSorter()

        Try
            Dim SortCols = G.Columns.Where(Function(x) x.SortSeq > 0).OrderBy(Function(x) x.SortSeq)
            dgvCTS.SorterCount = 0

            For Each gc In SortCols
                dgvCTS.SorterCount += 1

                Dim sOrder As String
                If gc.SortOrder = SortOrder.Descending Then
                    sOrder = "DESC"
                Else
                    sOrder = "ASC"
                End If

                Idx = dgvCTS.Columns(Trim(gc.Field)).Index
                dgvCTS.CustomSorter(Idx).Seq = gc.SortSeq
                dgvCTS.CustomSorter(Idx).Order = sOrder
            Next

        Catch ex As Exception
            dgvCTS.ClearSorter()
        End Try

        dgvCTS.ApplySorter()

    End Sub


    Public Sub LoadRRNColumns(g As Grid)

        'get a distinct list of tables referenced in DNPF2510 
        Dim dtFiles = C2App.Data.GetTable($"Select Distinct GCFILE From DNPF2510 Where GCID = {g.ID} And GCRID = {g.VersionID} and GCFILE <> ''")
        g.UpdateTables = dtFiles.AsEnumerable.Select(Function(x) x("GCFILE").ToString.Trim).ToList

        For Each DB2File In g.UpdateTables
            Dim newcol As New GridColumn
            newcol.SQLFunction = $"RRN({DB2File})"
            newcol.Field = $"RRN_{DB2File}"
            newcol.Text = $"RRN_{DB2File}"
            newcol.HeaderText = $"RRN_{DB2File}"
            newcol.ProviderTypeCode = "4"
            newcol.Updatable = False
            newcol.Restrict = True
            newcol.Seq = 999
            newcol.ColumnType = GridColumnTypes.TEXT
            newcol.Width = 100
            newcol.Alignment = GeneralAlignment.Right
            newcol.Size = 6
            newcol.Scale = 0
            newcol.Name = $"RRN_{DB2File}"
            g.Columns.Add(newcol)
        Next

    End Sub


    'Get Options from DNPF2530
    Public Function LoadCustomOptionsXXX(ByVal GridName As String, ByVal mode As GridModes) As RowOptionCollection
        Dim NewList As New RowOptionCollection

        Dim dt As DataTable = C2App.Data.GetTable(
            "Select OFSeq, OFText, OFFGroup, OFFunction From DNPF2530 
            Where OFName = '{0}' and OFType = '{1}' and OFStatus = 'A' and (UCase(OFMode) = '{2}' or UCase(OFMode) = '*ANY')",
            GridName, "*OPTION", mode.ToString.ToUpper)
        If dt IsNot Nothing Then
            For Each Row As DataRow In dt.Rows
                Row.TrimColumns

                If C2App.HasPermission(Row("OFFGroup"), Row("OFFunction"), C2App.User) Or String.IsNullOrWhiteSpace(Row("OFFunction").ToString) Then
                    Dim NewRO As New RowOption
                    NewRO.Sequence = Row("OFSeq")
                    NewRO.Name = Row("OFText")
                    NewRO.Text = Row("OFText")
                    NewRO.Visible = True
                    NewRO.Enable = True
                    NewList.Add(NewRO)
                End If
            Next
        End If
        Return NewList

    End Function

    Public Function LoadCustomOptions(ByVal GridID As Integer, ReleaseID As Integer) As RowOptionCollection
        Dim NewList As New RowOptionCollection

        Dim dt As DataTable = C2App.Data.GetTable(
            "Select OFSeq, OFText, OFFGroup, OFFunction, OFMode From DNPF2530 
            Where OFGridID = {0} and OFRID = {1} and OFType = '{2}' and OFStatus = 'A'",
            GridID, ReleaseID, "*OPTION")
        If dt IsNot Nothing Then
            For Each Row As DataRow In dt.Rows
                Row.TrimColumns

                If C2App.HasPermission(Row("OFFGroup"), Row("OFFunction"), C2App.User) Or String.IsNullOrWhiteSpace(Row("OFFunction").ToString) Then
                    Dim NewRO As New RowOption
                    NewRO.Sequence = Row("OFSeq")
                    NewRO.Name = Row("OFText")
                    NewRO.Text = Row("OFText")
                    NewRO.Visible = True
                    NewRO.Enable = True
                    Select Case Row("OFMode").ToString.ToUpper
                        Case "DISPLAY"
                            NewRO.Display = DisplayMode.Display
                        Case "UPDATE"
                            NewRO.Display = DisplayMode.Update
                        Case Else
                            NewRO.Display = DisplayMode.Any
                    End Select
                    '[Enum].TryParse(Row("OFMode"), NewRO.Display)
                    NewList.Add(NewRO)
                End If
            Next
        End If
        Return NewList

    End Function



    'Get Functions from DNPF2530
    Public Function LoadCustomFunctions(ByVal GridID As Integer, ReleaseID As Integer) As GridFunctionCollection
        Dim NewList As New GridFunctionCollection

        Dim dt As DataTable = C2App.Data.GetTable(
            "Select OFSeq, OFText, OFFGroup, OFFunction From DNPF2530 
            Where OFGridId = {0} and OFRID = {1} and OFType = '*FUNCTION' and OFStatus = 'A'",
            GridID, ReleaseID)
        If dt IsNot Nothing Then
            For Each Row As DataRow In dt.Rows
                Row.TrimColumns

                If C2App.HasPermission(Row("OFFGroup"), Row("OFFunction"), C2App.User) Or String.IsNullOrWhiteSpace(Row("OFFunction").ToString) Then
                    Dim NewGF As New GridFunction
                    NewGF.Sequence = Row("OFSeq")
                    NewGF.Name = Row("OFText")
                    NewGF.Text = Row("OFText")
                    NewGF.Visible = True
                    NewGF.Enable = True
                    [Enum].TryParse(Row("OFMode"), NewGF.Display)
                    NewList.Add(NewGF)
                End If
            Next
        End If
        Return NewList

    End Function




    'Load Dynamic Styles (DNPF2535) 
    'Public Function LoadDynamicStyles(ByVal GridName As String, ByVal DefaultStyle As DataGridViewCellStyle) As List(Of DynamicStyle)
    '    Dim _DynamicStyles As New List(Of DynamicStyle)

    '    _DynamicStyles.Clear()
    '    Dim strSQL As String = "Select A.GCFIELD, A.GCDSFIELD, B.GSSTYLE, B.GSOP, B.GSVALUE, B.GSAPPLY," &
    '                           " B.GSFCOLOR, B.GSBCOLOR, B.GSFONTNAME, B.GSFONTSIZE, B.GSFONTBOLD" &
    '                           " From DNPF2510 A Join DNPF2535 B On GCDSNAME = GSSTYLE" &
    '                           " Where GCNAME = '" & Trim(GridName) & "'"
    '    Dim dt = App.Data.GetTable(strSQL)
    '    If dt IsNot Nothing Then
    '        For Each dr As DataRow In dt.Rows
    '            Dim item As New DynamicStyle
    '            item.FieldName = Trim(dr("GCFIELD"))
    '            If Trim(dr("GCDSFIELD")) = String.Empty Then
    '                item.ApplyField = Trim(dr("GCFIELD"))
    '            Else
    '                item.ApplyField = Trim(dr("GCDSFIELD"))
    '            End If
    '            item.Name = Trim(dr("GSSTYLE"))
    '            item.Op = Trim(dr("GSOP"))
    '            item.Value = Trim(dr("GSVALUE"))
    '            item.ApplyType = Trim(dr("GSAPPLY"))

    '            Dim ds As DataGridViewCellStyle = DefaultStyle.Clone
    '            Dim NewFontName As FontFamily
    '            Dim NewFontStyle As FontStyle = FontStyle.Regular
    '            Dim NewFontSize As Single

    '            If Trim(dr("GSFONTNAME")) <> "" Then
    '                NewFontName = New FontFamily(Trim(dr("GSFONTNAME")))
    '            Else
    '                NewFontName = ds.Font.FontFamily
    '            End If

    '            If Trim(dr("GSFONTBOLD")) = 1 Then
    '                NewFontStyle = FontStyle.Bold
    '            End If

    '            If Trim(dr("GSFONTSIZE")) <> 0 Then
    '                NewFontSize = Trim(dr("GSFONTSIZE"))
    '            Else
    '                NewFontSize = ds.Font.Size
    '            End If

    '            'Dim dsNewFont As New Font(NewFontName, NewFontSize, NewFontStyle)
    '            'ds.Font = dsNewFont
    '            ds.Font = New Font(NewFontName, NewFontSize, NewFontStyle)

    '            If Trim(dr("GSBCOLOR")) <> String.Empty Then
    '                ds.BackColor = Color.FromName(Trim(dr("GSBCOLOR")))
    '            End If

    '            If Trim(dr("GSFCOLOR")) <> String.Empty Then
    '                ds.ForeColor = Color.FromName(Trim(dr("GSFCOLOR")))
    '            End If

    '            item.Style = ds
    '            _DynamicStyles.Add(item)
    '        Next
    '    End If

    '    Return _DynamicStyles
    'End Function

    'Public Function LoadDynamicStylesXXX(ByVal GridName As String, ByVal DefaultStyle As DataGridViewCellStyle) As DynamicStyleCollection

    '    Dim NewCollection As DynamicStyleCollection = LoadDynamicStyles(GridName)

    '    For Each DynamicStyle In NewCollection
    '        Try

    '            Dim ds As DataGridViewCellStyle = DefaultStyle.Clone
    '            Dim NewFontName As FontFamily
    '            Dim NewFontStyle As FontStyle = FontStyle.Regular
    '            Dim NewFontSize As Single

    '            If String.IsNullOrEmpty(DynamicStyle.FontName) Then
    '                NewFontName = ds.Font.FontFamily
    '            Else
    '                NewFontName = New FontFamily(DynamicStyle.FontName.Trim)
    '            End If

    '            If DynamicStyle.FontBold Then
    '                NewFontStyle = FontStyle.Bold
    '            End If

    '            If DynamicStyle.FontSize > 0 Then
    '                NewFontSize = DynamicStyle.FontSize
    '            Else
    '                NewFontSize = ds.Font.Size
    '            End If
    '            ds.Font = New Font(NewFontName, NewFontSize, NewFontStyle)

    '            If Not String.IsNullOrEmpty(DynamicStyle.BackColorName) Then
    '                ds.BackColor = Color.FromName(DynamicStyle.BackColorName)
    '            End If

    '            If Not String.IsNullOrEmpty(DynamicStyle.ForeColorName) Then
    '                ds.ForeColor = Color.FromName(DynamicStyle.ForeColorName)
    '            End If

    '            DynamicStyle.Style = ds

    '        Catch ex As Exception
    '        End Try
    '    Next

    '    Return NewCollection
    'End Function


    Public Function LoadDynamicStyles(ByVal GridID As Integer, ReleaseID As Integer) As DynamicStyleCollection
        Dim NewCollection As New DynamicStyleCollection
        Dim sequence As Short

        Dim strSQL As String = "Select A.GCFIELD, A.GCDSFIELD, B.GSSTYLE, B.GSOP, B.GSVALUE, B.GSAPPLY," &
                               " B.GSFCOLOR, B.GSBCOLOR, B.GSFONTNAME, B.GSFONTSIZE, B.GSFONTBOLD" &
                               " From DNPF2510 A Join DNPF2535 B On GCDSNAME = GSSTYLE" &
                               $" Where GCID = {GridID} and GCRID = {ReleaseID}"
        Dim dt = C2App.Data.GetTable(strSQL)
        If dt IsNot Nothing Then
            For Each dr As DataRow In dt.Rows
                sequence += 1

                Dim NewDS As New DynamicStyle
                NewDS.Name = $"{dr("GCFIELD").trim}_{dr("GSSTYLE").trim}_{sequence}"
                NewDS.LegacyName = dr("GSSTYLE").trim

                NewDS.FieldName = dr("GCFIELD").trim
                If dr("GCDSFIELD").trim = String.Empty Then
                    NewDS.ApplyCellName = dr("GCFIELD").trim
                Else
                    NewDS.ApplyCellName = dr("GCDSFIELD").trim
                End If
                NewDS.Op = Filtering.GetOperatorName(dr("GSOP").trim)
                NewDS.Value = dr("GSVALUE").trim
                NewDS.ApplyType = DynamicStyle.GetStyleApplyType(("GSAPPLY").ToString.Trim)
                NewDS.FontName = dr("GSFONTNAME").ToString.Trim
                If dr("GSFONTSIZE") <> 0 Then
                    NewDS.FontBold = True
                Else
                    NewDS.FontBold = False
                End If
                NewDS.FontSize = dr("GSFONTSIZE")
                NewDS.BackColorName = dr("GSBCOLOR").ToString.Trim
                NewDS.ForeColorName = dr("GSFCOLOR").ToString.Trim
                NewDS.Style = Nothing
                NewCollection.Add(NewDS)
            Next
        End If

        Return NewCollection
    End Function


    Public Sub SaveGridSettings(ByVal dgvcts As DataGridViewCT2, ByVal GridID As Integer, ReleaseID As Integer, GridName As String)
        Dim dgvCol As DataGridViewColumn
        Dim Hidden As String
        Dim strSQL As String
        Dim RowsEffected As Integer

        If dgvcts.Columns.Count > 0 Then
            strSQL = $"Delete from DNPF2511 Where GPUSER = '{C2App.User}' and gpid = {GridID} and gprid = {ReleaseID}"
            RowsEffected = C2App.Data.ExecuteNonQuery(strSQL)

            For Each dgvCol In dgvcts.Columns
                Hidden = IIf(dgvCol.Visible, "N", "Y")

                strSQL = String.Format("Insert into DNPF2511" &
                            " (GPUSER,GPID,GPRID,GPNAME,GPFIELD,GPWIDTH,GPSEQ,GPSORT,GPDIR,GPHIDDEN)" &
                            " VALUES('{0}','{1}','{2}',{3},{4},{5},'{6}','{7}')",
                            C2App.User, GridID, ReleaseID, GridName, dgvCol.Name, dgvCol.Width, dgvCol.DisplayIndex,
                            dgvcts.CustomSorter(dgvCol.Index).Seq, dgvcts.CustomSorter(dgvCol.Index).Order, Hidden)
                RowsEffected = C2App.Data.ExecuteNonQuery(strSQL)

            Next
        End If

    End Sub

    Public Sub SaveGridSettings(ByVal grid As DataGridViewCT2, GridName As String)
        Dim dgvCol As DataGridViewColumn
        Dim Hidden As String
        Dim strSQL As String
        Dim RowsEffected As Integer

        If grid.Columns.Count > 0 Then
            strSQL = $"Delete from DNPF2511 Where GPUSER = '{C2App.User}' and ucase(GPNAME) = '{GridName.ToUpper.Trim}'"
            RowsEffected = C2App.Data.ExecuteNonQuery(strSQL)

            For Each dgvCol In grid.Columns
                Hidden = IIf(dgvCol.Visible, "N", "Y")

                strSQL = String.Format("Insert into DNPF2511" &
                            " (GPUSER,GPNAME,GPFIELD,GPWIDTH,GPSEQ,GPSORT,GPDIR,GPHIDDEN)" &
                            " VALUES('{0}','{1}','{2}',{3},{4},{5},'{6}','{7}')",
                            C2App.User, GridName, dgvCol.Name, dgvCol.Width, dgvCol.DisplayIndex,
                            grid.CustomSorter(dgvCol.Index).Seq, grid.CustomSorter(dgvCol.Index).Order, Hidden)
                RowsEffected = C2App.Data.ExecuteNonQuery(strSQL)

            Next
        End If

    End Sub

    Public Sub DeleteGridSettings(ByVal GridID As String, ReleaseID As Integer)
        Dim strSQL = $"Delete from DNPF2511 Where GPUSER = '{C2App.User}' and GPID = {GridID} and GPRID = {ReleaseID}"
        Dim RowsEffected = C2App.Data.ExecuteNonQuery(strSQL)
    End Sub


#End Region



#Region "Updating Grid Definition"

    Public Sub UpdateGridHeader(ByVal Grid As Grid)
        Dim row2501 = C2App.Data.DB2Chain("DNPF2501", $"gmid = {Grid.ID}")
        If row2501 IsNot Nothing Then
            row2501.TrimColumns()
            row2501("GMName") = Grid.Name
            row2501("GMTitle") = Grid.Title
            row2501("GMMenu") = Grid.LibraryList
            C2App.Data.DB2RowUpdate(row2501, "DNPF2501")
        End If

        Dim row2500 = C2App.Data.DB2Chain("DNPF2500", $"ghid = {Grid.ID} and ghrid = {Grid.VersionID}")
        If row2500 IsNot Nothing Then
            row2500.TrimColumns()
            row2500("GHName") = Grid.Name
            row2500("GHTitle") = Grid.Title
            row2500("GHLList") = Grid.LibraryList
            row2500("GHFCO") = Grid.FromClause
            row2500("GHWhere") = Grid.WhereClause
            row2500("GHOrder") = Grid.OrderByClause
            row2500("GHGroup") = Grid.GroupByClause
            row2500("GHDistinct") = Grid.Distinct.ToStringDB2
            row2500("GHRTot") = Grid.TotalsRow.ToStringDB2
            C2App.Data.DB2RowUpdate(row2500, "DNPF2500")
        End If
    End Sub

    Public Sub AddGridHeader(ByVal Grid As Grid)
        Dim row2501 = C2App.Data.DB2GetEmptyRow("DNPF2501")
        If row2501 IsNot Nothing Then
            row2501("GMName") = Grid.Name
            row2501("GMTitle") = Grid.Title
            row2501("GMMenu") = Grid.LibraryList
            C2App.Data.DB2RowInsert(row2501, "DNPF2501")
        End If

        Dim row2500 = C2App.Data.DB2GetEmptyRow("DNPF2500")
        If row2500 IsNot Nothing Then
            row2500("GHName") = Grid.Name
            row2500("GHTitle") = Grid.Title
            row2500("GHLList") = Grid.LibraryList
            row2500("GHFCO") = Grid.FromClause
            row2500("GHWhere") = Grid.WhereClause
            row2500("GHOrder") = Grid.OrderByClause
            row2500("GHGroup") = Grid.GroupByClause
            row2500("GHDistinct") = Grid.Distinct.ToStringDB2
            row2500("GHRTot") = Grid.TotalsRow.ToStringDB2
            C2App.Data.DB2RowInsert(row2500, "DNPF2500")
        End If
    End Sub




    Public Function SaveDefinition(Grid As Grid) As Boolean
        Dim sql As String

        For Each gc In Grid.Columns
            sql = $"select * from dnpf2510 where gcid = {Grid.ID} and gcrid = {Grid.VersionID} and gcfield = '{gc.Field.Trim}'"
            Dim row As DataRow = C2App.Data.GetRow(sql)
            If row IsNot Nothing Then
                UpdateGridColumn(Grid.ID, Grid.VersionID, gc)
            Else
                AddGridColumn(Grid.ID, Grid.VersionID, gc)
            End If
        Next
        DeleteGC(Grid)

        For Each RO In Grid.RowOptions
            sql = $"select * from dnpf2530 where ofgridid = {Grid.ID} and ofrid = {Grid.VersionID} and oftype = '*OPTION' and ofname = '{RO.Name.Trim}'"
            Dim row As DataRow = C2App.Data.GetRow(sql)
            If row IsNot Nothing Then
                UpdateRowOption(Grid.ID, Grid.VersionID, RO)
            Else
                AddRowOption(Grid.ID, Grid.VersionID, RO)
            End If
        Next
        DeleteRO(Grid)

        For Each gf In Grid.GridFunctions



        Next


        For Each ds In Grid.DynamicStyles



        Next





        Return True
    End Function



    Public Sub AddGridColumn(ByVal ID As Integer, ByVal VersionID As Integer, ByVal GC As GridColumn)

        Dim row2510 = C2App.Data.DB2GetEmptyRow("DNPF2510")
        If row2510 IsNot Nothing Then
            row2510.TrimColumns()

            row2510("GCID") = ID
            row2510("GCRID") = VersionID
            row2510("GCName") = GC.Name
            row2510("GCFLAG1") = GC.SuppressTrim.ToStringDB2
            row2510("GCField") = GC.Field
            row2510("GCFunction") = GC.SQLFunction
            row2510("GCSeq") = GC.Seq
            row2510("GCHeader") = GC.HeaderText
            row2510("GCSort") = GC.SortSeq
            Select Case GC.SortOrder
                Case SortOrder.None
                    row2510("GPDIR") = String.Empty
                Case SortOrder.Ascending
                    row2510("GPDIR") = "ASC"
                Case SortOrder.Descending
                    row2510("GPDIR") = "DESC"
            End Select
            row2510("GCWIDTH") = GC.Width
            row2510("GCPTYPE") = GC.ProviderTypeCode
            row2510("GCSize") = GC.Size
            row2510("GCScale") = GC.Scale
            row2510("GCJDECVT") = GC.CastCCSID37.ToStringDB2
            row2510("GCFormat") = GC.Format
            row2510("GCSumCol") = GC.SumColumn.ToStringDB2
            row2510("GCSumFmt") = GC.SumFormat
            row2510("GCPreSel") = GC.Preselect.ToStringDB2
            row2510("GCAlwUpd") = GC.Updatable.ToStringDB2
            row2510("GCHIDDEN") = GC.Hidden.ToStringDB2

            row2510("GCColTyp") = GC.ColumnType.ToString
            row2510("GCSecurity") = GC.SecurityType.ToString
            row2510("GCAlign") = GC.Alignment.ToString
            C2App.Data.DB2RowInsert(row2510, "DNPF2510")
        End If

    End Sub

    Public Sub UpdateGridColumn(ByVal ID As Integer, ByVal VersionID As Integer, ByVal GC As GridColumn)

        Dim row2510 = C2App.Data.DB2Chain("DNPF2510", $"gcid = {ID} and gcrid = {VersionID} and GCField = '{GC.Field}'")
        If row2510 IsNot Nothing Then
            row2510.TrimColumns()
            row2510("GCName") = GC.Name
            row2510("GCFLAG1") = GC.SuppressTrim.ToStringDB2
            row2510("GCField") = GC.Field
            row2510("GCFunction") = GC.SQLFunction
            row2510("GCSeq") = GC.Seq
            row2510("GCHeader") = GC.HeaderText
            row2510("GCSort") = GC.SortSeq
            Select Case GC.SortOrder
                Case SortOrder.None
                    row2510("GCDIR") = String.Empty
                Case SortOrder.Ascending
                    row2510("GCDIR") = "ASC"
                Case SortOrder.Descending
                    row2510("GCDIR") = "DESC"
            End Select
            row2510("GCWIDTH") = GC.Width
            row2510("GCPTYPE") = GC.ProviderTypeCode
            row2510("GCSize") = GC.Size
            row2510("GCScale") = GC.Scale
            row2510("GCJDECVT") = GC.CastCCSID37.ToStringDB2
            row2510("GCFormat") = GC.Format
            row2510("GCSumCol") = GC.SumColumn.ToStringDB2
            row2510("GCSumFmt") = GC.SumFormat
            row2510("GCPreSel") = GC.Preselect.ToStringDB2
            row2510("GCAlwUpd") = GC.Updatable.ToStringDB2
            row2510("GCHIDDEN") = GC.Hidden.ToStringDB2

            row2510("GCColTyp") = GC.ColumnType.ToString
            row2510("GCSecurity") = GC.SecurityType.ToString
            row2510("GCAlign") = GC.Alignment
            C2App.Data.DB2RowUpdate(row2510, "DNPF2510")
        End If

    End Sub

    Public Sub DeleteGC(Grid As Grid)

        Dim SQL = $"select * from dnpf2510 where gcid = {Grid.ID} and gcrid = {Grid.VersionID} "
        Dim dt2510 As DataTable = C2App.Data.GetTable(SQL)
        If dt2510 IsNot Nothing Then
            For Each dr2510 As DataRow In dt2510.Rows
                If Grid.Columns.Contains(dr2510("gcfield").ToString.Trim) Then
                    Dim y As String = "yes"
                Else
                    Dim n As String = "no"
                End If
            Next
        End If

    End Sub


    Public Sub AddRowOption(ByVal ID As Integer, ByVal VersionID As Integer, ByVal RO As RowOption)

        Dim row2530 = C2App.Data.DB2GetEmptyRow("DNPF2530")
        If row2530 IsNot Nothing Then
            row2530.TrimColumns()
            row2530("OFTYPE") = "*OPTION"
            row2530("OFNAME") = RO.Name
            'row2530("OFVERSION") = RO.
            row2530("OFSEQ") = RO.Sequence
            'row2530("OFKEY") = RO.
            row2530("OFTEXT") = RO.Text
            Select Case RO.Display
                Case DisplayMode.Any
                    row2530("OFMODE") = "*ANY"
                Case DisplayMode.Display
                    row2530("OFMODE") = "DISPLAY"
                Case DisplayMode.Update
                    row2530("OFMODE") = "UPDATE"
            End Select
            'row2530("OFSHORTCUT") = RO. 
            'row2530("OFFUNCTION") = RO. 
            'row2530("OFSECKEY") = RO. 
            row2530("OFSTATUS") = "A"
            'row2530("OFIMAGE") = RO. 
            'row2530("OFGRID") = ID
            row2530("OFGRIDID") = ID
            row2530("OFRID") = VersionID
            C2App.Data.DB2RowInsert(row2530, "DNPF2530")
        End If

    End Sub

    Public Sub UpdateRowOption(ByVal ID As Integer, ByVal VersionID As Integer, ByVal RO As RowOption)

        Dim row2530 = C2App.Data.DB2Chain("DNPF2530", $"ofgridid = {ID} and ofrid = {VersionID} and oftype = '*OPTION' and ofname = '{RO.Name}'")
        If row2530 IsNot Nothing Then
            row2530("OFNAME") = RO.Name
            'row2530("OFVERSION") = RO.
            row2530("OFSEQ") = RO.Sequence
            'row2530("OFKEY") = RO.
            row2530("OFTEXT") = RO.Text
            Select Case RO.Display
                Case DisplayMode.Any
                    row2530("OFMODE") = "*ANY"
                Case DisplayMode.Display
                    row2530("OFMODE") = "DISPLAY"
                Case DisplayMode.Update
                    row2530("OFMODE") = "UPDATE"
            End Select
            'row2530("OFSHORTCUT") = RO. 
            'row2530("OFFUNCTION") = RO. 
            'row2530("OFSECKEY") = RO. 
            row2530("OFSTATUS") = "A"
            'row2530("OFIMAGE") = RO. 
            'row2530("OFGRID") = ID
            C2App.Data.DB2RowUpdate(row2530, "DNPF2530")
        End If

    End Sub

    Public Sub DeleteRO(Grid As Grid)

        Dim SQL = $"select * from dnpf2530 where ofgridid = {Grid.ID} and ofrid = {Grid.VersionID} and oftype = '*OPTION'"
        Dim dt2530 As DataTable = C2App.Data.GetTable(SQL)
        If dt2530 IsNot Nothing Then
            For Each dr2530 As DataRow In dt2530.Rows
                If Grid.RowOptions.Contains(dr2530("ofname").ToString.Trim) Then
                    Dim y As String = "yes"
                Else
                    Dim n As String = "no"
                End If
            Next
        End If

    End Sub

#End Region



#Region "Updating/Deleting Rows"

    'Automatic Row Update  
    Public Function UpdateRow2(ByVal G As Grid, ByVal dgvr As DataGridViewRow) As Integer
        Dim rc As Integer = 0

        Try
            For Each DB2File In G.UpdateTables
                Dim RRN As Integer = dgvr.Cells($"RRN_{DB2File}").Value
                Dim strSet = BuildSet(G.Name, dgvr, DB2File)

                If Not String.IsNullOrWhiteSpace(strSet) And RRN <> 0 Then
                    rc = C2App.Data.ExecuteNonQuery($"Update {DB2File} Set {strSet.Trim} Where RRN({DB2File}) = {RRN.ToString}")
                End If
            Next

        Catch ex As Exception
            dgvr.ErrorText = $"Unable to update row. {ex.Message}"
            MessageBox.Show(ReadException(ex))
        End Try

        Return rc
    End Function

    'Automatic Row Delete 
    Public Function DeleteRow2(ByVal G As Grid, ByVal dgvr As DataGridViewRow) As Integer
        Dim rc As Integer = 0

        Try
            'for now.. just allow delete when one file is eligible per row
            If G.UpdateTables.Count = 1 Then
                Dim DB2File = G.UpdateTables(0)
                Dim RRN As Integer = dgvr.Cells($"RRN_{DB2File}").Value
                rc = C2App.Data.ExecuteNonQuery($"Delete From {DB2File} Where RRN({DB2File}) = {RRN.ToString}")
            End If

        Catch ex As Exception
            dgvr.ErrorText = $"Unable to delete row. {ex.Message}"
            MessageBox.Show(ReadException(ex))
        End Try

        Return rc
    End Function

    Public Function BuildSet(ByVal GridName As String, ByVal dgvr As DataGridViewRow, ByVal FileName As String) As String
        Dim strField As String = String.Empty
        Dim strSet As String = String.Empty
        Dim Sep As String = String.Empty

        Dim strSQL = "Select Distinct GCFILE, GCFIELD From DNPF2510 Where GCNAME='{0}' and GCFile = '{1}' and GCALWUPD = 'Y'"
        Dim dt As DataTable = C2App.Data.GetTable(strSQL, GridName.Trim, FileName.Trim)

        For Each r As DataRow In dt.Rows
            strField = r("GCFIELD").trim

            Select Case dgvr.Cells(strField).ValueType.ToString
                Case "System.String"
                    strSet = strSet.Trim & Sep & strField & "='" & dgvr.Cells(strField).Value & "'"
                Case "System.Decimal"
                    strSet = strSet.Trim & Sep & strField & "=" & dgvr.Cells(strField).Value
                Case "System.DateTime"
                    strSet = strSet.Trim & Sep & strField & "='" & dgvr.Cells(strField).Value & "'"
            End Select
            Sep = ","
        Next
        Return strSet

    End Function

    Public Function ReadException(ByVal ex As Exception) As String
        Dim msg As String = ex.Message
        If ex.InnerException IsNot Nothing Then
            msg = msg & vbCrLf & "---------" & vbCrLf & ReadException(ex.InnerException)
        End If
        Return msg
    End Function

#End Region

#Region "Filtering"

    Public Sub SetRowSecurityConstraint(ByVal ColName As String, SecurityType As SecurityColumnType)
        Try
            Select Case SecurityType
                Case SecurityColumnType.BU
                Case SecurityColumnType.Company
                Case SecurityColumnType.CompanyArea
            End Select
        Catch
        End Try
    End Sub

    Public Sub DeleteSavedUserFilters(ByVal UserName As String, ByVal GridName As String, ByVal FilterName As String)

        If UserName = "*CURRENT" Then
            UserName = C2App.User
        End If

        If Not String.IsNullOrEmpty(GridName) Then
            Dim strSQL As String = String.Format("Delete from DNPF2516 Where FDUSER = '{0}' and FDNAME = '{1}' and FDFNAME = '{2}'",
                               UserName.Trim, GridName.Trim, FilterName.Trim)
            C2App.Data.ExecuteNonQuery(strSQL)
        End If
    End Sub

    'flagged for removal
    'Public Sub SaveUserFiltersX(ByVal UF As List(Of Filtering.GridFilter), ByVal UserName As String, ByVal GridName As String, ByVal FilterName As String)
    '    If UserName = "*CURRENT" Then
    '        UserName = App.User
    '    End If

    '    DeleteSavedUserFilters(UserName, GridName, FilterName)

    '    Dim dt As DataTable = App.Data.DB2GetEmptyTable("DNPF2516")
    '    For i As Integer = 0 To UF.Count - 1
    '        Dim dr As DataRow = dt.NewRow
    '        dr("FDUSER") = UserName
    '        dr("FDNAME") = GridName
    '        dr("FDFNAME") = FilterName
    '        dr("FDSEQ") = UF(i).Seq
    '        dr("FDFIELD") = UF(i).FieldName
    '        If UF(i).Hidden Then
    '            dr("FDHIDDEN") = 1
    '        Else
    '            dr("FDHIDDEN") = 0
    '        End If
    '        If UF(i).Protect Then
    '            dr("FDPROTECT") = 1
    '        Else
    '            dr("FDPROTECT") = 0
    '        End If
    '        dr("FDAO") = UF(i).AO
    '        dr("FDOPCODE") = UF(i).OpCode
    '        dr("FDVALUE") = UF(i).Value
    '        dr("FDVALUE2") = UF(i).Value2

    '        If Not IsNothing(UF(i).ValueList) Then
    '            dr("FDVALLIST") = String.Join(",", UF(i).ValueList)
    '        Else
    '            dr("FDVALLIST") = String.Empty
    '        End If

    '        dr("FDFLDTYPE") = UF(i).Fldt
    '        dr("FDDESC") = UF(i).Desc
    '        dr("FDSOURCE") = UF(i).Source

    '        App.Data.DB2RowInsert(dr, "DNPF2516")

    '    Next

    'End Sub

    'Save User Filters 
    Public Sub SaveUserFilters(ByVal F As Filtering.Filter, ByVal UserName As String, ByVal GridName As String, ByVal FilterName As String)
        If UserName = "*CURRENT" Then
            UserName = C2App.User
        End If

        DeleteSavedUserFilters(UserName, GridName, FilterName)

        Dim dt As DataTable = C2App.Data.DB2GetEmptyTable("DNPF2516")

        For Each c As Filtering.Condition In F.Conditions
            Dim dr As DataRow = dt.NewRow
            dr("FDUSER") = UserName
            dr("FDNAME") = GridName
            dr("FDFNAME") = FilterName
            dr("FDSEQ") = c.Seq
            dr("FDFIELD") = c.ColumnName
            dr("FDHIDDEN") = IIf(c.Hidden, 1, 0)
            dr("FDPROTECT") = IIf(c.Protect, 1, 0)
            dr("FDAO") = c.LOP.ToString
            dr("FDOPCODE") = Filtering.GetOperator(c.Op)
            dr("FDDESC") = c.Description

            If Not IsNothing(c.Values) Then
                If c.Values.Count > 1 Then
                    dr("FDVALUE") = c.Values(0)
                End If
                dr("FDVALLIST") = String.Join(",", c.Values)
            Else
                dr("FDVALLIST") = String.Empty
            End If

            'dr("FDFLDTYPE") = UF(i).Fldt
            'dr("FDSOURCE") = UF(i).Source

            C2App.Data.DB2RowInsert(dr, "DNPF2516")
        Next



    End Sub

    'todo flagged for removal 
    'Public Sub LoadUserFiltersX(ByVal flist As List(Of Filtering.GridFilter), ByVal UserName As String, ByVal GridName As String, ByVal FilterName As String)
    '    If UserName = "*CURRENT" Then
    '        UserName = App.User
    '    End If

    '    flist.Clear()

    '    Dim strSQL As String = String.Format("Select * From DNPF2516 Where FDUSER = '{0}' and FDNAME = '{1}' and FDFNAME = '{2}'",
    '                           UserName.Trim, GridName.Trim, FilterName.Trim)
    '    Dim dt As DataTable = App.Data.GetTable(strSQL)

    '    For Each dr As DataRow In dt.Rows

    '        Dim f As New Filtering.GridFilter
    '        f.ValueList = New List(Of String)

    '        f.Seq = dr("FDSEQ")
    '        f.FieldName = dr("FDFIELD").trim
    '        If dr("FDHIDDEN") = 0 Then
    '            f.Hidden = False
    '        Else
    '            f.Hidden = True
    '        End If
    '        If dr("FDPROTECT") = 0 Then
    '            f.Protect = False
    '        Else
    '            f.Protect = True
    '        End If
    '        f.AO = dr("FDAO").trim
    '        f.OpCode = dr("FDOPCODE").trim
    '        f.Value = dr("FDVALUE").trim
    '        f.Value2 = dr("FDVALUE2").trim

    '        Dim str As String = dr("FDVALLIST").trim
    '        Dim ara As String() = str.Split(",")
    '        f.ValueList.AddRange(ara)

    '        f.Fldt = dr("FDFLDTYPE").trim
    '        f.Desc = dr("FDDESC").trim
    '        f.Source = dr("FDSOURCE").trim
    '        flist.Add(f)
    '    Next

    'End Sub

    'Load User Filters

    Public Sub LoadUserFilters(ByVal F As Filtering.Filter, ByVal UserName As String, ByVal GridName As String, ByVal FilterName As String)
        If UserName = "*CURRENT" Then
            UserName = C2App.User
        End If

        F.Conditions.Clear()

        Dim strSQL As String = String.Format("Select * From DNPF2516 Where FDUSER = '{0}' and FDNAME = '{1}' and FDFNAME = '{2}'",
                               UserName.Trim, GridName.Trim, FilterName.Trim)
        Dim dt As DataTable = C2App.Data.GetTable(strSQL)

        For Each dr As DataRow In dt.Rows

            Dim c As New Filtering.Condition
            c.Seq = dr("FDSEQ")
            c.ColumnName = Trim(dr("FDFIELD"))
            c.Hidden = IIf(dr("FDHIDDEN") = 0, False, True)
            c.Protect = IIf(dr("FDPROTECT") = 0, False, True)
            c.LOP = Filtering.GetLogicalOperator(dr("FDAO").ToString)
            c.Op = Filtering.GetOperatorName(dr("FDOPCODE").ToString)
            c.Description = dr("FDDESC").ToString.Trim

            c.Values = New List(Of String)
            Dim str As String = dr("FDVALLIST").ToString.Trim
            Dim ara As String() = str.Split(",")
            c.Values.AddRange(ara.ToList)
            If c.Values.Count = 0 Then
                c.Values.Add(dr("FDVALUE").ToString.Trim)
            End If

            'Dim AsType = dr("FDFLDTYPE").ToString.Trim
            'If AsType.ToLower.Contains("string") Or AsType.ToLower.Contains("datetime") Then
            '    c.InsertQuotes = True
            'Else
            '    c.InsertQuotes = False
            'End If

            F.Conditions.Add(c)
        Next

    End Sub

#End Region

#Region "General Functions"

    Public Sub SetDeveloperFlag()
        If C2App.IsMember("IT - Developer", C2App.User) Then
            IsDeveloper = True
        End If
    End Sub

#End Region


#Region "Grid Versioning"

    Public Function GetDefaultVersion(GridID As Integer) As Integer
        Dim result As Integer
        result = C2App.Data.GetSQLInteger($"select GVID from DNPF2550 where gvgid = {GridID} and gvstatus = 'Online' ")
        Return result
    End Function


    Public Sub ActivateGrid(GridID As Integer, VersionID As Integer)

        Dim dr2501 As DataRow = C2App.Data.GetRow($"Select * from dnpf2501 where gmid = {GridID}")
        If dr2501 IsNot Nothing Then

            'put all grids offline
            Dim OfflineName = $"_{dr2501("GMNAME")}"
            If OfflineName.Trim.Length > 20 Then
                OfflineName = OfflineName.Substring(0, 20)
            End If
            C2App.Data.ExecuteNonQuery($"Update dnpf2550 Set gvstatus = 'Offline' Where gvgid = {GridID}")
            C2App.Data.ExecuteNonQuery($"Update dnpf2500 Set ghname = '{OfflineName}' Where ghid = {GridID}")
            C2App.Data.ExecuteNonQuery($"Update dnpf2510 Set gcname = '{OfflineName}' Where gcid = {GridID}")
            C2App.Data.ExecuteNonQuery($"Update dnpf2530 Set ofname = '{OfflineName}', ofgrid = '{OfflineName}' Where ofgridid = {GridID}")
            C2App.Data.ExecuteNonQuery($"Update dnpf2535 Set gsgrid = '{OfflineName}' Where gsgridid = {GridID}")

            'put VersionID online
            Dim OnlineName = dr2501("GMNAME")
            C2App.Data.ExecuteNonQuery($"Update dnpf2550 Set gvstatus = 'Online', gvPgmLevel = 'Production' Where gvgid = {GridID} and gvid = {VersionID}")
            C2App.Data.ExecuteNonQuery($"Update dnpf2500 Set ghname = '{OnlineName}' Where ghid = {GridID} and ghrid = {VersionID}")
            C2App.Data.ExecuteNonQuery($"Update dnpf2510 Set gcname = '{OnlineName}' Where gcid = {GridID} and gcrid = {VersionID}")
            C2App.Data.ExecuteNonQuery($"Update dnpf2530 Set ofname = '{OnlineName}', ofgrid = '{OnlineName}' Where ofgridid = {GridID} and ofrid = {VersionID}")
            C2App.Data.ExecuteNonQuery($"Update dnpf2535 Set gsgrid = '{OnlineName}' Where gsgridid = {GridID} and gsrid = {VersionID}")
        End If

    End Sub

    Public Function AssignProgramLevel(GridID As Integer, VersionID As Integer, PgmLevel As ProgramLevels) As Boolean
        Dim result As Boolean

        Try
            'Check 2550 for this version
            Dim count = C2App.Data.GetSQLInteger($"Select count(*) from dnpf2550 where gvgid = {GridID} and gvname = '{PgmLevel.ToString}' and gvid <> {VersionID} ")
            If count > 0 Then
                Dim ctsm As New CTS.Controls.CTSMessage
                ctsm.Title = "Version Level Assignment"
                ctsm.MessageText = $"Another Version is assigned to {PgmLevel.ToString}.  Only one version can be at this Level.  Replace?"
                ctsm.ButtonPreset = CTS.Controls.CTSMessage.ButtonPresets.YesNo
                If ctsm.ShowDialog() = vbYes Then
                    'reset grid name
                    C2App.Data.ExecuteNonQuery($"update dnpf2550 set gvname = '' Where gvgid = {GridID} and gvname = '{PgmLevel.ToString}' and gvid <> {VersionID}")
                End If
            End If

            'assign level to gridname and pgmlevel
            C2App.Data.ExecuteNonQuery($"update dnpf2550 set gvname = '{PgmLevel.ToString}', gvPgmLevel = '{PgmLevel.ToString}' Where gvgid = {GridID} and gvid = {VersionID}")

            If PgmLevel = ProgramLevels.Production Then
                ActivateGrid(GridID, VersionID)
            End If


        Catch ex As Exception
            CTS.Controls.CTSException.ShowDialog(ex)
        End Try

        Return result
    End Function

    Public Function ClearProgramLevel(GridID As Integer, VersionID As Integer) As Boolean
        Dim result As Boolean

        Try
            'Check 2550 for this version
            Dim count = C2App.Data.GetSQLInteger($"Select count(*) from dnpf2550 where gvgid = {GridID} and gvname = 'Production' and gvid = {VersionID} ")
            If count > 0 Then
                Dim ctsm As New CTS.Controls.CTSMessage
                ctsm.Title = "Version Level Assignment"
                ctsm.MessageText = $"You can not unassign a Production version.  A Production version must exist."
                ctsm.ShowDialog()
            Else
                'assign level to gridname and pgmlevel
                C2App.Data.ExecuteNonQuery($"update dnpf2550 set gvname = '' Where gvgid = {GridID} and gvid = {VersionID}")
            End If

        Catch ex As Exception
            CTS.Controls.CTSException.ShowDialog(ex)
        End Try

        Return result
    End Function


    Public Function CheckoutGrid(GridID As Integer, VersionID As Integer) As Integer
        Dim NewVID As Integer = -1

        Try
            'Check 2550 for this Version ID
            Dim dr2550 As DataRow = C2App.Data.GetRow($"Select * from dnpf2550 where gvgid = {GridID} and gvid = {VersionID}")
            If dr2550 IsNot Nothing Then

                Dim dr2501 As DataRow = C2App.Data.GetRow($"Select * from dnpf2501 where gmid = {GridID}")
                Dim dr2500 As DataRow = C2App.Data.GetRow($"Select * from dnpf2500 where ghid = {GridID} and ghrid = {VersionID}")
                If dr2501 IsNot Nothing And dr2500 IsNot Nothing Then
                    NewVID = GetNextVersion(GridID)

                    Dim dt = C2App.Data.DB2GetEmptyTable("DNPF2550")
                    Dim dr2550New = dt.NewRow
                    dr2550New("GVID") = NewVID
                    dr2550New("GVGID") = GridID
                    dr2550New("GVRID") = NewVID
                    dr2550New("GVMENU") = dr2550("GVMENU")
                    dr2550New("GVNAME") = ""
                    dr2550New("GVPGMLEVEL") = dr2550("GVPGMLEVEL")
                    dr2550New("GVDATAENV") = dr2550("GVDATAENV")
                    dr2550New("GVStatus") = "Offline"
                    dr2550New("GVADDED") = Now
                    dr2550New("GVADDU") = Environment.UserName.ToUpper
                    dt.Rows.Add(dr2550New)
                    Dim rc = C2App.Data.DB2RowInsert(dr2550New, "DNPF2550")

                    Dim NewName As String = $"_{dr2501("GMNAME")}"
                    If NewName.Trim.Length > 20 Then
                        NewName = NewName.Substring(0, 20)
                    End If
                    Copy(GridID, VersionID, GridID, NewVID, NewName)
                End If
            End If

        Catch ex As Exception
            NewVID = -1
        End Try

        Return NewVID
    End Function


    Public Function ReplicateGrid(GridID As Integer, VersionID As Integer) As Integer
        Dim rc As Integer
        Dim NextGID As Integer = 0

        Try
            'Check 2550 for this Version ID
            Dim dr2550 As DataRow = C2App.Data.GetRow($"Select * from dnpf2550 where gvgid = {GridID} and gvid = {VersionID}")
            If dr2550 IsNot Nothing Then

                Dim dr2501 As DataRow = C2App.Data.GetRow($"Select * from dnpf2501 where gmid = {GridID}")
                Dim dr2500 As DataRow = C2App.Data.GetRow($"Select * from dnpf2500 where ghid = {GridID} and ghrid = {VersionID}")

                If dr2501 IsNot Nothing And dr2500 IsNot Nothing Then

                    'Get new ID and duplicate grid master
                    NextGID = GetNextGridID()
                    Do Until rc <> 0
                        dr2501("GMID") = NextGID
                        dr2501("GMNAME") = $"COPY_{NextGID}"
                        dr2501.AcceptChanges()
                        dr2501.SetAdded()
                        rc = C2App.Data.DB2RowInsert(dr2501, "DNPF2501")
                        If rc = 0 Then
                            NextGID += 1
                        End If
                    Loop

                    Dim dt = C2App.Data.DB2GetEmptyTable("DNPF2550")
                    Dim dr2550New = dt.NewRow
                    dr2550New("GVID") = 0
                    dr2550New("GVGID") = NextGID
                    dr2550New("GVRID") = 0
                    dr2550New("GVMENU") = dr2550("GVMENU")
                    dr2550New("GVNAME") = ""
                    dr2550New("GVPGMLEVEL") = dr2550("GVPGMLEVEL")
                    dr2550New("GVDATAENV") = dr2550("GVDATAENV")
                    dr2550New("GVStatus") = "Offline"
                    dr2550New("GVADDED") = Now
                    dr2550New("GVADDU") = Environment.UserName.ToUpper
                    dt.Rows.Add(dr2550New)
                    Dim rc2 = C2App.Data.DB2RowInsert(dr2550New)

                    If rc > 0 Then
                        Copy(GridID, VersionID, NextGID, 0, $"COPY_{NextGID}")
                    End If

                End If
            End If

        Catch ex As Exception
            NextGID = 0
        End Try

        Return NextGID
    End Function

    Public Function ReplicateGrid(GridID As Integer) As Integer
        Return ReplicateGrid(GridID, GetProductionVersion(GridID))
    End Function

    Public Function RenameGrid(GridID As Integer, NewName As String) As Boolean
        Dim rc As Integer
        Dim rcheader As Integer
        Dim RID As Integer

        Try
            NewName = NewName.Trim

            rcheader = C2App.Data.ExecuteNonQuery($"update dnpf2501 set gmname = '{NewName}' where gmid = {GridID}")

            RID = C2App.Data.GetSQLInteger($"select GVRID from dnpf2550 where gvgid = {GridID} and gvpgmlevel = 'Production' fetch first 1 rows only ")
            rc = C2App.Data.ExecuteNonQuery($"update dnpf2500 set ghname = '{NewName}' where ghid = {GridID} and GHRID = {RID}")
            rc = C2App.Data.ExecuteNonQuery($"update dnpf2510 set gcname = '{NewName}' where gcid = {GridID} and GCRID = {RID}")
            rc = C2App.Data.ExecuteNonQuery($"update dnpf2530 set ofname = '{NewName}' where ofgridid = {GridID} and ofRID = {RID}")
            'rc = App.Data.ExecuteNonQuery($"update dnpf2535 set gsgrid = '{NewName}' where gsgridid = {GridID} and gsRID = {RID}")

        Catch ex As Exception
        End Try

        If rcheader = 1 Then
            Return True
        Else
            Return False
        End If

    End Function

    Public Function RenameGrid(GridID As Integer) As Integer
        Return ReplicateGrid(GridID, GetProductionVersion(GridID))
    End Function


    Public Function GetProductionVersion(GridID As Integer) As Integer
        Dim VID = C2App.Data.GetSQLInteger($"select VersionID from DNGRIDS where GridID = {GridID} And PgmLevel = 'Production'")
        Return VID
    End Function


    Private Sub Copy(GridID As Integer, VersionID As Integer, NewGridID As Integer, NewVersion As Integer, NewName As String)
        Dim rc As Integer

        'duplicate grid header
        Dim dr2500 As DataRow = C2App.Data.GetRow($"Select * from dnpf2500 where ghid = {GridID} and ghrid = {VersionID}")
        If dr2500 IsNot Nothing Then
            dr2500("GHID") = NewGridID
            dr2500("GHRID") = NewVersion
            dr2500("GHNAME") = NewName
            dr2500.AcceptChanges()
            dr2500.SetAdded()
            rc = C2App.Data.DB2RowInsert(dr2500, "DNPF2500")
        End If

        'duplicate grid columns
        Dim dt2510 As DataTable = C2App.Data.GetTable($"Select * from dnpf2510 where gcid = {GridID} and gcrid = {VersionID}")
        If dt2510 IsNot Nothing Then
            For Each dr In dt2510.Rows
                dr("GCID") = NewGridID
                dr("GCRID") = NewVersion
                dr("GCNAME") = NewName
                dr.AcceptChanges()
                dr.SetAdded()
                rc = C2App.Data.DB2RowInsert(dr, "DNPF2510")
            Next
        End If

        'duplicate grid options
        Dim dt2530 As DataTable = C2App.Data.GetTable($"Select * from dnpf2530 where ofgridid = {GridID} and ofrid = {VersionID}")
        If dt2530 IsNot Nothing Then
            For Each dr In dt2530.Rows
                dr("OFGRIDID") = NewGridID
                dr("OFRID") = NewVersion
                dr("OFGRID") = NewName
                dr("OFNAME") = NewName
                dr.AcceptChanges()
                dr.SetAdded()
                rc = C2App.Data.DB2RowInsert(dr, "DNPF2530")
            Next
        End If

        'duplicate grid styles
        Dim dt2535 As DataTable = C2App.Data.GetTable($"Select * from dnpf2535 where gsgridid = {GridID} and gsrid = {VersionID}")
        If dt2535 IsNot Nothing Then
            For Each dr In dt2535.Rows
                dr("GSGRIDID") = NewGridID
                dr("GSRID") = NewVersion
                dr("GSGRID") = NewName
                dr.AcceptChanges()
                dr.SetAdded()
                rc = C2App.Data.DB2RowInsert(dr, "DNPF2535")
            Next
        End If


    End Sub

    Public Sub DeleteGrid(GridID As Integer)
        C2App.Data.ExecuteNonQuery($"delete dnpf2550 Where gvgid = {GridID} ")
        C2App.Data.ExecuteNonQuery($"delete dnpf2501 Where gmid = {GridID} ")
        C2App.Data.ExecuteNonQuery($"delete dnpf2500 Where ghid = {GridID} ")
        C2App.Data.ExecuteNonQuery($"delete dnpf2510 Where gcid = {GridID} ")
        C2App.Data.ExecuteNonQuery($"delete dnpf2530 Where ofgridid = {GridID} ")
        C2App.Data.ExecuteNonQuery($"delete dnpf2535 Where gsgridid = {GridID} ")
    End Sub

    Public Sub DeleteGridVersion(GridID As Integer, VersionID As Integer)
        C2App.Data.ExecuteNonQuery($"delete dnpf2550 Where gvgid = {GridID} and gvid = {VersionID}")
        C2App.Data.ExecuteNonQuery($"delete dnpf2500 Where ghid = {GridID} and ghrid = {VersionID}")
        C2App.Data.ExecuteNonQuery($"delete dnpf2510 Where gcid = {GridID} and gcrid = {VersionID}")
        C2App.Data.ExecuteNonQuery($"delete dnpf2530 Where ofgridid = {GridID} and ofrid = {VersionID}")
        C2App.Data.ExecuteNonQuery($"delete dnpf2535 Where gsgridid = {GridID} and gsrid = {VersionID}")
    End Sub


    Public Function GetNextGridID() As Integer
        Dim result = C2App.Data.GetSQLInteger($"select max(GMID) + 1 from dnpf2501")
        Return result
    End Function

    Public Function GetNextVersion(GridID As Integer) As Integer
        Dim result = C2App.Data.GetSQLInteger($"select max(GVID) + 1 from dnpf2550 where GVGID = {GridID}")
        Return result
    End Function

    'Public Function GetNextReleaseID(GridID As Integer) As Integer
    '    Dim result = App.Data.GetSQLInteger($"select max(GRRelease) + 1 from dnpf2555 where GRGID = {GridID}")
    '    Return result
    'End Function

#End Region

End Class



#Region "SQL Class - Statement Breakdown"

Public Class SQL

    Property Original As String
    Property Statement As String
    Property Distinct As Boolean

    Property TableList As New List(Of SqlTable)
    Property ColumnList As New List(Of SqlColumn)
    Property ClauseList As New List(Of Clause)

    Property ErrorLog As New List(Of String)
    Property Errors As Boolean = False
    Property Loaded As Boolean = False

    Private SchemasList As DataTable
    Private SchemaData As DataTable


#Region "Supporting Classes"
    Public Class Clause
        Property Position As Short
        Property DataPosition As Short
        Property Name As String
        Property Value As String

    End Class

    Public Class SqlTable
        Property Name As String
        Property AliasName As String
        Property SchemaName As String
        Property SchemaExplicit As String
        Property RawData As String
        Property JoinType As String
        Property Criteria As String
        Property TableText As String
    End Class

    Public Class SqlColumn
        Property Name As String
        Property TableName As String
        Property ColFunction As String
        Property DataType As DataType
        Property ProviderType As String
        Property Size As Short
        Property Scale As Short
        Property Text As String
        Property HeaderText As String
        Property TableAlias As String
        Property ColumnAlias As String
    End Class

    Private StringSub As New List(Of KeyValuePair(Of Short, String))

    Private ParenthesisSub As New List(Of KeyValuePair(Of Short, String))


#End Region


#Region "Constructors"

    Sub New(SQLStatement As String)
        Initiate("GP", DataEnvironments.Production, SQLStatement)
    End Sub

    Sub New(LibraryList As String, SQLStatement As String)
        If String.IsNullOrEmpty(LibraryList) Then
            LibraryList = "GP"
        End If
        Initiate(LibraryList, DataEnvironments.Production, SQLStatement)
    End Sub

    Sub New(LibraryList As String, DataEnvironment As DataEnvironments, SQLStatement As String)
        If String.IsNullOrEmpty(LibraryList) Then
            LibraryList = "GP"
        End If
        Initiate(LibraryList, DataEnvironment, SQLStatement)
    End Sub

    Sub Initiate(LibraryList As String, DataEnvironment As DataEnvironments, SQLStatement As String)
        SchemasList = DNRG0011_Table(LibraryList, DataEnvironment)
        LoadStatement(SQLStatement)
    End Sub

#End Region


    Public Sub LoadStatement(ByVal SQStatement As String)
        Original = SQStatement
        Statement = SQStatement

        ExtractStrings()
        ExtractAllParenthesis()

        'select distinct??

        FindClauses("Select")
        FindClauses("From")
        FindClauses("Left Join")
        FindClauses("Inner Join")
        FindClauses("Outer Join")
        FindClauses("Exception Join")
        FindClauses("Join")
        FindClauses("Where")
        FindClauses("Group By")
        FindClauses("Order By")
        ExtractClauses()

        Try
            LoadTables()
            SchemaData = C2App.Data.GetThisSchema($"select * {FromString()} ")
        Catch ex As Exception
            SchemaData = Nothing
            ErrorLog.Add($"Error in From Clause. {ex.Message}")
        End Try

        If SchemaData IsNot Nothing Then
            LoadColumns()
        End If

        If Errors Then
            Loaded = False
        Else
            Loaded = True
        End If

    End Sub

    Public Function FromString() As String
        Dim result As String = String.Empty

        Dim FromTable = TableList.Find(Function(x) x.JoinType.ToLower = "from")
        If FromTable IsNot Nothing Then

            If Not String.IsNullOrWhiteSpace(FromTable.SchemaExplicit) Then
                result = $"From {FromTable.SchemaExplicit}.{FromTable.Name}"

            ElseIf Not String.IsNullOrWhiteSpace(FromTable.SchemaName) Then
                result = $"From {FromTable.SchemaName}.{FromTable.Name}"
            Else
                result = $"From {FromTable.RawData}"
            End If


            For Each sqltable In TableList
                If sqltable.JoinType.ToLower.Contains("join") Then
                    result = $"{result.Trim} {sqltable.JoinType} {sqltable.RawData}"
                End If
            Next

            result = Embed(result)
        End If

        Return result
    End Function

    Public Function GroupByString() As String
        Dim result As String = String.Empty

        Dim GBClause = ClauseList.Find(Function(x) x.Name.ToLower = "group by")
        If GBClause IsNot Nothing Then
            result = GBClause.Value
        End If
        result = Embed(result)

        Return result
    End Function

    Public Function GetClause(ByVal Name As String) As String
        Dim result As String = String.Empty

        Dim Clause = ClauseList.Find(Function(x) x.Name.ToLower = Name)
        If Clause IsNot Nothing Then
            result = Clause.Value
        End If

        result = Embed(result)

        Return result
    End Function



#Region "SHARE TEST"


    Public Function GetAllColumns() As List(Of SqlColumn)

        Dim sqlcl As New List(Of SqlColumn)
        Try
            Dim sql As String
            sql = "Select Column_Name, Data_Type, Length, Numeric_Scale, Column_Text, Column_Heading " &
                          "From qsys2.syscolumns " &
                          "Where table_Schema = '{0}' and table_name = '{1}' "

            For Each sqlt In TableList
                Dim dtCols = C2App.Data.GetTable(sql, sqlt.SchemaName.ToUpper, sqlt.Name.ToUpper)
                If dtCols IsNot Nothing Then
                    For Each drCol In dtCols.Rows

                        Dim data_type As DataType = [Enum].Parse(GetType(DataType), drCol("Data_Type"))
                        Dim P As ProviderType.DB2ProviderDataTypes = ProviderType.GetProvider(data_type)
                        If P <> ProviderType.DB2ProviderDataTypes.NotSupported Then
                            Dim sqlc As New SqlColumn
                            sqlc.Name = drCol("Column_Name")
                            sqlc.TableName = sqlt.Name.ToUpper
                            sqlc.ColFunction = String.Empty
                            sqlc.DataType = data_type
                            sqlc.ProviderType = P
                            sqlc.Size = IIf(IsDBNull(drCol("Length")), 0, drCol("Length"))
                            sqlc.Scale = IIf(IsDBNull(drCol("Numeric_Scale")), 0, drCol("Numeric_Scale"))
                            sqlc.Text = IIf(IsDBNull(drCol("Column_Text")), String.Empty, drCol("Column_Text").ToString.Trim)
                            sqlc.HeaderText = IIf(IsDBNull(drCol("Column_Heading")), String.Empty, drCol("Column_Heading").ToString.Trim)
                            sqlcl.Add(sqlc)
                        End If
                    Next
                End If
            Next

        Catch ex As Exception
        End Try

        Return sqlcl
    End Function



#End Region





#Region "Table work"

    Private Sub LoadTables()

        'find main from clause...
        Dim FromClause = ClauseList.Find(Function(x) x.Name.ToLower = "from")
        If FromClause IsNot Nothing Then
            Dim sqlFrom As New SqlTable
            sqlFrom.JoinType = FromClause.Name
            sqlFrom.RawData = FromClause.Value
            sqlFrom = BreakdownTableReference(sqlFrom)
            TableList.Add(sqlFrom)
        End If

        'Find all join clauses in arrival sequence
        Dim SortedList = ClauseList.OrderBy(Function(x) x.Position)
        For Each clause In SortedList
            If clause.Name.ToLower.Contains("join") Then
                Dim sqlt As New SqlTable
                sqlt.JoinType = clause.Name
                sqlt.RawData = clause.Value
                sqlt = BreakdownTableReference(sqlt)
                TableList.Add(sqlt)
            End If
        Next

    End Sub

    Private Function BreakdownTableReference(sqlt As SqlTable) As SqlTable
        Dim name As String = String.Empty

        If sqlt.RawData IsNot Nothing Then
            sqlt.RawData = sqlt.RawData.Trim.Replace("/", ".")

            Dim wordbreak = sqlt.RawData.IndexOf(" ")
            If wordbreak = -1 Then
                name = sqlt.RawData
            Else
                name = sqlt.RawData.Substring(0, wordbreak)
            End If
        End If

        If name.Contains(".") Then
            sqlt.SchemaExplicit = name.Substring(0, name.IndexOf("."))
            sqlt.Name = name.Substring(name.IndexOf(".") + 1)
        Else
            sqlt.SchemaExplicit = String.Empty
            sqlt.Name = name
        End If

        If sqlt.RawData IsNot Nothing Then
            If sqlt.RawData.ToLower.Contains(" on ") Then
                Dim idx = sqlt.RawData.ToLower.IndexOf(" on ")
                sqlt.Criteria = sqlt.RawData.Substring(idx + 1)
            End If
        End If

        If String.IsNullOrWhiteSpace(sqlt.SchemaExplicit) Then
            FindTableSchema(sqlt, SchemasList)
        Else
            LookupTableSchema(sqlt)
        End If



        Return sqlt
    End Function

    Private Sub LookupTableSchema(sqlt As SqlTable)
        Dim savedrow As DataRow = Nothing
        Dim seq As Short = Short.MaxValue

        Dim row = C2App.Data.GetRow("Select Table_Text from qsys2.systables where Table_Schema = '{0}' and Table_Name = '{1}'",
                                   sqlt.SchemaExplicit.ToUpper, sqlt.Name.ToUpper)
        If row IsNot Nothing Then
            sqlt.SchemaName = sqlt.SchemaExplicit.ToUpper
            sqlt.TableText = row("Table_Text").ToString.Trim
        End If

    End Sub

    Private Sub FindTableSchema(sqlt As SqlTable, SchemaTable As DataTable)
        Dim savedrow As DataRow = Nothing
        Dim seq As Short = Short.MaxValue

        Dim dt = C2App.Data.GetTable("Select Table_Schema, Table_Text from qsys2.systables where table_name = '{0}'", sqlt.Name.ToUpper)
        If dt IsNot Nothing Then

            For Each row In dt.Rows
                Dim foundrow = SchemaTable.Select($"name='{row("Table_Schema").ToString}'")
                If foundrow.Length > 0 AndAlso foundrow(0)("seq") < seq Then
                    seq = foundrow(0)("seq")
                    savedrow = row
                End If
            Next

            If savedrow IsNot Nothing Then
                sqlt.SchemaName = savedrow("Table_Schema")
                sqlt.TableText = savedrow("Table_Text")




            End If

        End If





    End Sub

#End Region


#Region "Column work"
    Private Function LoadColumns() As String
        Dim result As String = String.Empty

        'todo breakdown select columns; need to separate table alias ??
        Dim SelectClause = ClauseList.Find(Function(x) x.Name.ToLower = "select")
        If SelectClause Is Nothing Then
            'error
        Else

            If SelectClause.Value.ToLower.StartsWith("distinct ") Then
                SelectClause.Value = SelectClause.Value.Remove(0, 9)
                Distinct = True
            End If

            If SelectClause.Value.Trim = "*" Then
                AddAllColumns()
            Else
                Dim cols As String() = SelectClause.Value.Split(",")
                For Each col In cols

                    Dim sqlc As SqlColumn = BreakSelectColumn(col.Trim)

                    Dim rows As DataRow() = SchemaData.Select($"ColumnName = '{sqlc.Name.Trim.ToUpper}'")
                    If rows IsNot Nothing AndAlso rows.Count > 0 Then
                        'sqlc.Name = rows(0)("ColumnName")
                        sqlc.ProviderType = rows(0)("ProviderType")
                        sqlc.Size = rows(0)("ColumnSize")
                        sqlc.ColFunction = Embed(sqlc.ColFunction)
                        AddToColumnList(sqlc)
                    Else
                        'will not find any field embedded in a function
                        'like trim(field) or concat(fielda,fieldb)
                        sqlc.ColFunction = Embed(sqlc.ColFunction)
                        AddToColumnList(sqlc)

                        'todo this is a warning NOT an error! 
                        'ErrorLog.Add($"'{col.Trim.ToUpper}' not found in current schema")
                    End If


                Next
            End If

            result = SelectClause.Value
        End If

        result = EmbedStrings(result)
        result = EmbedParenthesis(result)

        Return result
    End Function

    Private Sub AddAllColumns()

        Try
            If SchemaData IsNot Nothing Then
                For Each dr In SchemaData.Rows
                    Dim sqlc As New SqlColumn
                    sqlc.Name = dr("ColumnName")
                    sqlc.ColFunction = String.Empty
                    sqlc.ProviderType = dr("ProviderType")
                    sqlc.Size = dr("ColumnSize")
                    AddToColumnList(sqlc)
                Next
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub


    Public Sub AddToColumnList(ByVal sqlc As SqlColumn)

        Dim sql As String
        sql = "Select Data_Type, Length, Numeric_Scale, Column_Heading, Column_Text " &
                          "From qsys2.syscolumns " &
                          " where table_Schema = '{0}' " &
                          " And table_name = '{1}' " &
                          " And Column_Name = '{2}' "
        Dim Added As Boolean

        For Each sqlt In TableList
            If sqlt.SchemaName IsNot Nothing Then
                Dim drow = C2App.Data.GetRow(sql, sqlt.SchemaName.ToUpper, sqlt.Name.ToUpper, sqlc.Name.ToUpper)
                If drow IsNot Nothing Then
                    sqlc.TableName = sqlt.Name.ToUpper
                    sqlc.Text = IIf(IsDBNull(drow("Column_Text")), String.Empty, drow("Column_Text").ToString.Trim)
                    sqlc.HeaderText = IIf(IsDBNull(drow("Column_Heading")), String.Empty, drow("Column_Heading").ToString.Trim)
                    ColumnList.Add(sqlc)
                    Added = True
                    Exit For
                End If
            End If
        Next

        If Not Added Then
            'is valid function or result field?
            ColumnList.Add(sqlc)
        End If

    End Sub

    Private Function BreakSelectColumn(ByVal ColString As String) As SqlColumn
        Dim result As New SqlColumn

        Dim words = ColString.Split(" ").ToList

        If words.Count > 1 Then
            If words(1).Trim = "" Then
                words.RemoveAt(1)
            End If
        End If

        If words.Count = 1 Then
            result = PullColumnTable(words(0), result)

        ElseIf words.Count = 2 Then
            result.ColFunction = words(0)
            result = PullColumnTable(words(0), result)
            result.ColumnAlias = words(1)
            result.Name = words(1)

        ElseIf words.Count = 3 And words(1).ToLower.Trim = "as" Then
            result.ColFunction = words(0)
            result = PullColumnTable(words(0), result)
            result.ColumnAlias = words(2)
            result.Name = words(2)
        End If

        Return result
    End Function

    Private Function PullColumnTable(ByVal ColumnName As String, sqlc As SqlColumn) As SqlColumn
        Dim words As String() = ColumnName.Split(".")
        If words.Count = 1 Then
            sqlc.Name = words(0)
        ElseIf words.Count = 2 Then
            sqlc.TableAlias = words(0)
            sqlc.Name = words(1)
        End If

        Return sqlc
    End Function


#End Region


#Region "Helper functions"

    Private Sub ExtractStrings()
        Dim Done As Boolean = False
        Dim startindex As Short = 0
        Dim endindex As Short = 0
        Dim nextstartindex As Short = 0

        Do Until Done
            startindex = Statement.IndexOf("'", nextstartindex)
            endindex = Statement.IndexOf("'", startindex + 1)

            If startindex < 0 Then
                Done = True
            Else
                startindex += 1
                endindex -= 1

                If startindex < endindex Then
                    Dim extracted = Statement.Substring(startindex, (endindex - startindex) + 1)
                    StringSub.Add(New KeyValuePair(Of Short, String)(startindex, extracted))

                    Dim sb As New Text.StringBuilder(Statement)
                    sb.Remove(startindex, (endindex - startindex) + 1)
                    sb.Insert(startindex, $"{{{startindex}}}")
                    Statement = sb.ToString

                    nextstartindex = startindex + startindex.ToString.Length + 3
                Else
                    Done = True
                End If
            End If
        Loop

    End Sub

    Private Sub ExtractAllParenthesis()
        Dim si As Short = ExtractParenthesis()
        Do While si > 0
            si = ExtractParenthesis(si + 1)
        Loop
    End Sub

    'todo extract ALL paren not just the first 
    Private Function ExtractParenthesis() As Short
        Return ExtractParenthesis(Statement.IndexOf("("))
    End Function


    Private Function ExtractParenthesis(nextstartindex As Short) As Short
        Dim done As Boolean
        Dim depth As Short
        Dim nextindex As Short
        Dim endindex As Short
        '
        If nextstartindex > 0 And nextstartindex < Statement.Length Then
            Dim startindex = Statement.IndexOf("(", nextstartindex)
            Dim lastopenindex = startindex
            Dim lastcloseindex = startindex
            nextstartindex = startindex

            If startindex > -1 Then
                Do Until done
                    nextindex = Statement.IndexOf("(", lastopenindex + 1)
                    endindex = Statement.IndexOf(")", lastcloseindex + 1)

                    If nextindex < 0 Or (endindex < nextindex) Then
                        If depth = 0 Then
                            done = True
                        Else
                            depth -= 1
                            lastcloseindex = endindex
                        End If
                    Else
                        depth += 1
                        lastopenindex = nextindex
                    End If
                Loop

                startindex += 1
                endindex -= 1

                If startindex < endindex Then
                    Dim extracted = Statement.Substring(startindex, (endindex - startindex) + 1)
                    ParenthesisSub.Add(New KeyValuePair(Of Short, String)(startindex, extracted))

                    Dim sb As New Text.StringBuilder(Statement)
                    sb.Remove(startindex, (endindex - startindex) + 1)
                    sb.Insert(startindex, $"{{{startindex}}}")
                    Statement = sb.ToString

                    nextstartindex = startindex + startindex.ToString.Length + 3
                End If
            End If
        Else
            nextstartindex = 0
        End If

        Return nextstartindex
    End Function

    Private Sub FindClauses(ByVal Name As String)
        Dim Done As Boolean
        Dim startindex As Short
        Dim clausename As String = Name.Trim.ToLower & " "

        Do Until Done
            startindex = Statement.ToLower.IndexOf(clausename, startindex)
            If startindex = -1 Then
                Done = True
            Else
                Dim c As New Clause
                c.Name = Name.Trim
                c.Position = startindex
                c.DataPosition = startindex + Len(clausename)
                ClauseList.Add(c)
                startindex += 1
            End If
        Loop

    End Sub

    Private Sub ExtractClauses()
        Dim curposition As Short = 0
        Dim lastposition As Short = 0
        Dim lastdataposition As Short = 0

        'find duplicate entries and tag
        For Each obj In ClauseList.OrderBy(Function(x) x.DataPosition).ToList
            If lastdataposition = obj.DataPosition And lastdataposition > 0 Then
                obj.Position = -1
            End If
            lastdataposition = obj.DataPosition
        Next

        'remove tagged entries
        For i = ClauseList.Count - 1 To 0 Step -1
            If ClauseList(i).Position = -1 Then
                ClauseList.RemoveAt(i)
            End If
        Next

        Dim SortedList = ClauseList.OrderByDescending(Function(x) x.Position).ToList
        For Each obj In SortedList

            If lastposition = 0 Then
                obj.Value = Statement.Substring(obj.DataPosition)
            Else
                If lastposition - obj.DataPosition > 0 Then
                    obj.Value = Statement.Substring(obj.DataPosition, lastposition - obj.DataPosition)
                End If
            End If

            lastposition = obj.Position
        Next
    End Sub

    Private Function Embed(ByVal Text As String) As String
        Text = EmbedParenthesis(Text)
        Text = EmbedStrings(Text)
        Text = EmbedParenthesis(Text)
        Text = EmbedStrings(Text)
        Return Text
    End Function

    Private Function EmbedStrings(ByVal Text As String) As String

        If Not String.IsNullOrEmpty(Text) Then
            For Each match As Match In Regex.Matches(Text, "{\d+}", RegexOptions.IgnoreCase)
                Dim sshort = match.Value.Substring(1, match.Value.Length - 2)
                Dim embed As String = StringSub.Find(Function(x) x.Key = CShort(sshort)).Value
                If embed IsNot Nothing Then
                    Text = Text.Replace(match.Value, embed)
                End If
            Next
        End If

        Return Text
    End Function

    Private Function EmbedParenthesis(ByVal Text As String) As String

        If Not String.IsNullOrEmpty(Text) Then
            For Each match As Match In Regex.Matches(Text, "{\d+}", RegexOptions.IgnoreCase)
                Dim sshort = match.Value.Substring(1, match.Value.Length - 2)
                Dim embed As String = ParenthesisSub.Find(Function(x) x.Key = CShort(sshort)).Value
                If embed IsNot Nothing Then
                    Text = Text.Replace(match.Value, embed)
                End If
            Next
        End If

        Return Text
    End Function

    Friend Function DNRG0011_Table(AppCode As String, Env As DataEnvironments) As DataTable
        Dim result As New DataTable
        result.Columns.Add(New DataColumn("Seq", GetType(System.Int16)))
        result.Columns.Add(New DataColumn("Name", GetType(System.String)))
        result.PrimaryKey = {result.Columns("Name")}

        Dim LibraryList As New List(Of String)
        Dim LLList As String = C2App.Data.Call_DNSP0011(AppCode, Env)
        If Not String.IsNullOrWhiteSpace(LLList) Then
            LibraryList = LLList.Split(",").ToList
        End If

        Dim seq As Short = 0
        For Each library In LibraryList
            Dim row = result.NewRow
            row("seq") = seq
            row("name") = library
            result.Rows.Add(row)
            seq += 1
        Next

        Return result
    End Function

    Friend Function DNRG0011_List(AppCode As String, Env As DataEnvironments) As List(Of String)
        Dim result As New List(Of String)

        Dim LLList As String = C2App.Data.Call_DNSP0011(AppCode, Env)
        If Not String.IsNullOrWhiteSpace(LLList) Then
            result = LLList.Split(",").ToList
        End If

        Return result
    End Function

#End Region


End Class

#End Region
