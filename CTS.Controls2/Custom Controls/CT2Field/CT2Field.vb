Imports System
Imports System.Globalization
Imports System.ComponentModel
Imports CTS.Controls2
Imports CTS.Controls

Public Class CT2Field

#Region "Properties"
    Private Initializing As Boolean = True
    Private Loading As Boolean = False
    Private Loaded As Boolean = False
    Private Formatting As Boolean = False


    Public FieldDateTime As CT2DateTime
    Public FieldCombo As CTS.Controls.ComboBoxCTS
    Public FieldCheck As CheckBoxCTS


    Friend Collection As CT2FieldCollection
    Friend CurrentEdit As CurrentEditTypes


    Private Const MinTextWidth = 20
    Private gDataSourceKey As String
    Private gProviderInfo As New ProviderInfo
    Private gEditType As EditTypes = 0
    Private gControlLayout As ControlLayouts = 0

    Private gLabelWidth As Integer = 50

    Private gDescriptionWidth As Integer = 50
    Private gTextWidth As Integer = 100
    Private gTextLines As Integer = 1
    Private gTextFormat As EntryFormat = 0
    Private gShowPrompt As Boolean = False
    Private gReadOnly As Boolean = False
    Private gUserProtect As Boolean = False


    Private gLabelFormat As SectionFormat = 0
    Private gDescriptionFormat As SectionFormat = 3

    Private gItems As List(Of GenericItem)






    <Browsable(True), EditorBrowsable(EditorBrowsableState.Always), DefaultValue(""),
    Category("_Custom Properties"), Description("DataSource Key/Fieldname for the control")>
    Public Property DataSourceKey() As String
        Get
            Return gDataSourceKey
        End Get
        Set(value As String)
            If Collection IsNot Nothing Then
                Collection.ChangeKey(Me, value)
            End If
            gDataSourceKey = value
        End Set
    End Property

    <System.ComponentModel.Category("_Custom Properties")>
    <System.ComponentModel.DefaultValue("False")>
    <System.ComponentModel.Description("Display the control as 'ReadOnly'")>
    Public Property [ReadOnly]() As Boolean
        Get
            Return gReadOnly
        End Get
        Set(ByVal value As Boolean)

            If gReadOnly <> value Then

                gReadOnly = value
                miProtect.Visible = Not gReadOnly

                Select Case CurrentEdit
                    Case CurrentEditTypes.FieldText
                        FieldText.ReadOnly = gReadOnly Or gUserProtect
                        FieldText.TabStop = Not gReadOnly And Not gUserProtect

                    Case CurrentEditTypes.FieldDateTime
                        FieldDateTime.ReadOnly = gReadOnly Or gUserProtect
                        FieldDateTime.TabStop = Not gReadOnly And Not gUserProtect

                    Case CurrentEditTypes.FieldCombo
                        FieldCombo.ReadOnly = gReadOnly Or gUserProtect
                        FieldCombo.TabStop = Not gReadOnly And Not gUserProtect

                    Case CurrentEditTypes.FieldCheck
                        FieldCheck.ReadOnly = gReadOnly Or gUserProtect
                        FieldCheck.TabStop = Not gReadOnly And Not gUserProtect
                End Select


                If DesignMode Then
                    'RedrawControl()
                Else
                    'InitializeControlColor()
                    Me.Refresh()
                End If

            End If


        End Set
    End Property

    <System.ComponentModel.Category("_Custom Properties")>
    <System.ComponentModel.DefaultValue("False")>
    <System.ComponentModel.Description("Protect the data entry (User Controlled)")>
    Public Property UserProtect() As Boolean
        Get
            Return gUserProtect
        End Get
        Set(ByVal value As Boolean)

            If gUserProtect <> value Then
                gUserProtect = value
                miProtect.Checked = value

                Select Case CurrentEdit
                    Case CurrentEditTypes.FieldText
                        FieldText.ReadOnly = gReadOnly Or gUserProtect
                        FieldText.TabStop = Not gReadOnly And Not gUserProtect

                    Case CurrentEditTypes.FieldDateTime
                        FieldDateTime.ReadOnly = gReadOnly Or gUserProtect
                        FieldDateTime.TabStop = Not gReadOnly And Not gUserProtect

                    Case CurrentEditTypes.FieldCombo
                        FieldCombo.ReadOnly = gReadOnly Or gUserProtect
                        FieldCombo.TabStop = Not gReadOnly And Not gUserProtect

                    Case CurrentEditTypes.FieldCheck
                        FieldCheck.ReadOnly = gReadOnly Or gUserProtect
                        FieldCheck.TabStop = Not gReadOnly And Not gUserProtect
                End Select


                If DesignMode Then
                    'RedrawControl()
                Else
                    'InitializeControlColor()
                    Me.Refresh()
                End If

            End If


        End Set
    End Property


    <Browsable(True), EditorBrowsable(EditorBrowsableState.Always),
    Category("_Custom Properties"), Description("DB2 Provider Information")>
    Public Property ProviderInfo() As ProviderInfo
        Get
            Return Me.gProviderInfo
        End Get
        Set(ByVal value As ProviderInfo)
            gProviderInfo = value
        End Set

    End Property

    <Browsable(True), EditorBrowsable(EditorBrowsableState.Always), DefaultValue(0),
    Category("_Custom Properties"), Description("Edit control type.")>
    Public Property EditType() As EditTypes
        Get
            Return gEditType
        End Get
        Set(value As EditTypes)
            If gEditType <> value Then
                gEditType = value
            End If

            If DesignMode And Not Initializing Then
                HideAllEditTypes()
                EditTypeSetup()
                FormatData()

                If gControlLayout = ControlLayouts.AutoSize Then FormatControlSize()
            End If
        End Set
    End Property

    <Browsable(True), EditorBrowsable(EditorBrowsableState.Always), DefaultValue(0),
    Category("_Custom Properties"), Description("Layout of the control.")>
    Public Property ControlLayout() As ControlLayouts
        Get
            Return gControlLayout
        End Get
        Set(value As ControlLayouts)
            If gControlLayout <> value Then
                gControlLayout = value
            End If

            If DesignMode And Not Initializing Then
                If gControlLayout = ControlLayouts.AutoSize Then FormatControlSize()
            End If

        End Set
    End Property

    'Label
    <Browsable(True), EditorBrowsable(EditorBrowsableState.Always), DefaultValue(0),
    Category("_Custom Properties"), Description("Format of the label portion of the control.")>
    Public Property LabelFormat() As SectionFormat
        Get
            Return gLabelFormat
        End Get
        Set(value As SectionFormat)
            If gLabelFormat <> value Then
                gLabelFormat = value
            End If

            If DesignMode And Not Initializing Then
                FormatLabel()
                If gControlLayout = ControlLayouts.AutoSize Then FormatControlSize()
            End If

        End Set
    End Property

    <Browsable(True), EditorBrowsable(EditorBrowsableState.Always), DefaultValue(50),
    Category("_Custom Properties"), Description("Width for the label when fixed; Max width when auto.")>
    Public Property LabelWidth() As Integer
        Get
            Return gLabelWidth
        End Get
        Set(ByVal value As Integer)
            If value >= 0 Then
                gLabelWidth = value

                If DesignMode And Not Initializing Then
                    FormatLabel()
                    If gControlLayout = ControlLayouts.AutoSize Then FormatControlSize()
                End If

            End If
        End Set
    End Property

    <Browsable(True), EditorBrowsable(EditorBrowsableState.Always), DefaultValue("Label:"),
    Category("_Custom Properties"), Description("Display text for the field label")>
    Public Property LabelText() As String
        Get
            Return FieldLabel.Text
        End Get
        Set(value As String)
            If FieldLabel.Text <> value Then
                FieldLabel.Text = value
            End If
        End Set
    End Property

    'Description
    <Browsable(True), EditorBrowsable(EditorBrowsableState.Always), DefaultValue(3),
    Category("_Custom Properties"), Description("Format of the description portion of the control.")>
    Public Property DescriptionFormat() As SectionFormat
        Get
            Return gDescriptionFormat
        End Get
        Set(value As SectionFormat)
            If gDescriptionFormat <> value Then
                gDescriptionFormat = value
            End If

            If DesignMode And Not Initializing Then
                FormatDescription()
                If gControlLayout = ControlLayouts.AutoSize Then FormatControlSize()
            End If

        End Set
    End Property

    <Browsable(True), EditorBrowsable(EditorBrowsableState.Always), DefaultValue(50),
    Category("_Custom Properties"), Description("Width allocated for the description")>
    Public Property DescriptionWidth() As Integer
        Get
            Return gDescriptionWidth
        End Get
        Set(ByVal value As Integer)
            If value >= 0 Then
                gDescriptionWidth = value
                'Description label sits in a Dock/Fill panel
                'Do not try to adjust its width

                If DesignMode And Not Initializing Then
                    If gControlLayout = ControlLayouts.AutoSize Then FormatControlSize()
                End If
            End If
        End Set
    End Property

    <Browsable(True), EditorBrowsable(EditorBrowsableState.Always), DefaultValue("Description"),
    Category("_Custom Properties"), Description("Display text for the description label")>
    Public Property DescriptionText() As String
        Get
            Return FieldDescription.Text
        End Get
        Set(value As String)
            If FieldDescription.Text <> value Then
                FieldDescription.Text = value
            End If
        End Set
    End Property

    'Data/Text
    <Browsable(True), EditorBrowsable(EditorBrowsableState.Always), DefaultValue(0),
    Category("_Custom Properties"), Description("Format of the entry portion of the control.")>
    Public Property TextFormat() As EntryFormat
        Get
            Return gTextFormat
        End Get
        Set(value As EntryFormat)
            If gTextFormat <> value Then
                gTextFormat = value
            End If

            If DesignMode And Not Initializing Then
                FormatData()
                If gControlLayout = ControlLayouts.AutoSize Then FormatControlSize()
            End If

        End Set
    End Property

    <Browsable(True), EditorBrowsable(EditorBrowsableState.Always), DefaultValue(100),
    Category("_Custom Properties"), Description("Width allocated for the text/entry field")>
    Public Property TextWidth() As Integer
        Get
            Return gTextWidth
        End Get
        Set(ByVal value As Integer)
            If value >= 0 Then
                gTextWidth = value
            End If
            Debug.Print("passing through...")
            If DesignMode And Not Initializing Then
                FormatData()
                If gControlLayout = ControlLayouts.AutoSize Then FormatControlSize()
            End If
        End Set
    End Property

    <Browsable(True), EditorBrowsable(EditorBrowsableState.Always), DefaultValue(1),
    Category("_Custom Properties"), Description("Number of text lines to display")>
    Public Property TextLines() As Integer
        Get
            Return gTextLines
        End Get
        Set(ByVal value As Integer)
            If value < 1 Then
                value = 1
            End If

            If value > 1 Then
                FieldText.Multiline = True
                gTextLines = value
            Else
                FieldText.Multiline = False
                gTextLines = value
            End If

            If DesignMode And Not Initializing Then
                FormatData()
                If gControlLayout = ControlLayouts.AutoSize Then FormatControlSize()
            End If

        End Set
    End Property

    <Browsable(True), EditorBrowsable(EditorBrowsableState.Always), DefaultValue(False),
    Category("_Custom Properties"), Description("Show the prompt button for this textbox")>
    Public Property ShowPrompt() As Boolean
        Get
            Return Me.gShowPrompt
        End Get
        Set(ByVal value As Boolean)
            gShowPrompt = value

            If DesignMode And Not Initializing Then
                'If Not Initializing Then
                FormatPrompt()
                FormatData()
                If gControlLayout = ControlLayouts.AutoSize Then FormatControlSize()
            End If

        End Set
    End Property

    Private gErrorText As String
    <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Property ErrorText() As String
        Get
            Return gErrorText
        End Get
        Set(value As String)
            If value <> gErrorText Then
                gErrorText = value
                SetColor()

                SetErrorToolTip(DataToolTip, value)

                RaiseEvent ErrorTextChanged(Me)
            End If
        End Set

    End Property

    Private gIsDirty As Boolean
    <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Property IsDirty() As Boolean
        Get
            Return gIsDirty
        End Get
        Set(value As Boolean)
            If value <> gIsDirty Then
                gIsDirty = value
                If Collection IsNot Nothing Then
                    Collection.CheckState()
                End If

                RaiseEvent StateChanged(Me)
            End If
        End Set

    End Property

    Private gDataValidate As Boolean = True
    <Browsable(True), EditorBrowsable(EditorBrowsableState.Always), DefaultValue(True),
    Category("_Custom Properties"), Description("Validate the data based on provider type.")>
    Public Property DataValidate() As Boolean
        Get
            Return Me.gDataValidate
        End Get
        Set(ByVal value As Boolean)
            gDataValidate = value
        End Set
    End Property

    Friend Enum CurrentEditTypes
        FieldText
        FieldDateTime
        FieldCombo
        FieldCheck
        FieldRadio
    End Enum




    Public Enum EditTypes
        UseDataType = 0
        Text = 1
        [DateTime] = 2
        [Date] = 3
        [Time] = 4
        Combo = 5
        Check = 6
        RadioButtons = 7
    End Enum

    Public Enum ControlLayouts
        AutoSize = 0
        FixedSize = 1
    End Enum

    Public Enum SectionFormat
        AutoWidth = 0
        FixedWidthNoWrap = 1
        FixedWidthWrapped = 2
        Hidden = 3
    End Enum

    Public Enum EntryFormat
        AutoWidth = 0
        FixedWidth = 1
    End Enum


    Public Event CheckChanged(ByVal sender As Object, ByVal e As EventArgs)
    Public Event StateChanged(ByVal sender As Object)
    Public Event ErrorTextChanged(ByVal sender As Object)
#End Region

#Region "Loading & Initialization"
    Public Sub New(data As Object)
        InitializeComponent()

        Initializing = False
    End Sub


    Public Sub New()
        InitializeComponent()

        Initializing = False
    End Sub



    Private Sub CT2Field_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Loading = True
        SuspendLayout()



        'FormatLabel()
        'EditTypeSetup()
        'FormatPrompt()
        'FormatData()
        'FormatDescription()



        If gControlLayout = ControlLayouts.AutoSize Then FormatControlSize()




        SetColor()

        ResumeLayout()
        Loading = False
        Loaded = True
    End Sub

    Public Sub Reload()
        CT2Field_Load(Me, New EventArgs)
    End Sub



    Private Sub EditTypeSetup()

        If EditType = EditTypes.UseDataType Then
            Select Case gProviderInfo.Type

                Case ProviderInfo.DB2ProviderDataTypes.Character
                    CurrentEdit = CurrentEditTypes.FieldText
                    FieldText.Visible = True
                    FieldText.Location = New Point(2, 2)
                    FieldText.MaxLength = gProviderInfo.Size
                    FieldText.ReadOnly = gReadOnly Or gUserProtect
                    FieldText.TabStop = Not gReadOnly And Not gUserProtect

                Case ProviderInfo.DB2ProviderDataTypes.Numeric
                    CurrentEdit = CurrentEditTypes.FieldText
                    FieldText.Visible = True
                    FieldText.Location = New Point(2, 2)
                    FieldText.ReadOnly = gReadOnly Or gUserProtect
                    FieldText.TabStop = Not gReadOnly And Not gUserProtect

                Case ProviderInfo.DB2ProviderDataTypes.DB2Date
                    CurrentEdit = CurrentEditTypes.FieldDateTime

                    SetDateTime()
                    FieldText.Visible = False
                    pnlPrompt.Visible = False
                    FieldDateTime.Visible = True

                    gTextFormat = EntryFormat.AutoWidth

                    FieldDateTime.Location = New Point(2, 2)
                    FieldDateTime.ReadOnly = gReadOnly
                    FieldDateTime.TabStop = Not gReadOnly
                    FieldDateTime.EditFormat = CTSDateTime.DisplayType.DateOnly
                    FieldDateTime.ProviderFormat = CTSDateTime.DisplayType.DateOnly
                    FieldDateTime.Reload()

                    RemoveHandler FieldDateTime.StateChanged, AddressOf Me.Field_StateChanged
                    RemoveHandler FieldDateTime.ErrorTextChanged, AddressOf Me.Field_ErrorTextChanged
                    AddHandler FieldDateTime.StateChanged, AddressOf Me.Field_StateChanged
                    AddHandler FieldDateTime.ErrorTextChanged, AddressOf Me.Field_ErrorTextChanged


                    'FROM THE DESIGNER

                    '
                    'FieldDateTime
                    '
                    'Me.FieldDateTime.AllowIncrement = False
                    'Me.FieldDateTime.BackColor = System.Drawing.Color.Transparent
                    'Me.FieldDateTime.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
                    'Me.FieldDateTime.ErrorStyle = CTS.Controls.ErrorStyles.None
                    'Me.FieldDateTime.Location = New System.Drawing.Point(3, 25)
                    'Me.FieldDateTime.Margin = New System.Windows.Forms.Padding(0)
                    'Me.FieldDateTime.MaxValue = New Date(CType(0, Long))
                    'Me.FieldDateTime.MinValue = New Date(CType(0, Long))
                    'Me.FieldDateTime.Name = "FieldDateTime"
                    'Me.FieldDateTime.ReadOnly = False
                    'Me.FieldDateTime.ReadOnlyBackColor = System.Drawing.Color.Transparent
                    'Me.FieldDateTime.ShowCalendarPrompt = False
                    'Me.FieldDateTime.ShowDateCheckBox = False
                    'Me.FieldDateTime.Size = New System.Drawing.Size(114, 22)
                    'Me.FieldDateTime.TabIndex = 1
                    'Me.FieldDateTime.Value = New Date(CType(0, Long))



                Case ProviderInfo.DB2ProviderDataTypes.DB2Time
                    CurrentEdit = CurrentEditTypes.FieldDateTime
                    SetDateTime()
                    FieldText.Visible = False
                    pnlPrompt.Visible = False
                    FieldDateTime.Visible = True

                    gTextFormat = EntryFormat.AutoWidth

                    FieldDateTime.Location = New Point(2, 2)
                    FieldDateTime.ReadOnly = gReadOnly
                    FieldDateTime.TabStop = Not gReadOnly
                    FieldDateTime.EditFormat = CTSDateTime.DisplayType.TimeOnly
                    FieldDateTime.ProviderFormat = CTSDateTime.DisplayType.TimeOnly

                    RemoveHandler FieldDateTime.StateChanged, AddressOf Me.Field_StateChanged
                    RemoveHandler FieldDateTime.ErrorTextChanged, AddressOf Me.Field_ErrorTextChanged
                    AddHandler FieldDateTime.StateChanged, AddressOf Me.Field_StateChanged
                    AddHandler FieldDateTime.ErrorTextChanged, AddressOf Me.Field_ErrorTextChanged


                Case ProviderInfo.DB2ProviderDataTypes.DB2Timestamp
                    CurrentEdit = CurrentEditTypes.FieldDateTime
                    SetDateTime()
                    FieldText.Visible = False
                    pnlPrompt.Visible = False
                    FieldDateTime.Visible = True

                    gTextFormat = EntryFormat.AutoWidth

                    FieldDateTime.Location = New Point(2, 2)
                    FieldDateTime.ReadOnly = gReadOnly
                    FieldDateTime.TabStop = Not gReadOnly
                    FieldDateTime.EditFormat = CTSDateTime.DisplayType.Timestamp
                    FieldDateTime.ProviderFormat = CTSDateTime.DisplayType.Timestamp

                    RemoveHandler FieldDateTime.StateChanged, AddressOf Me.Field_StateChanged
                    RemoveHandler FieldDateTime.ErrorTextChanged, AddressOf Me.Field_ErrorTextChanged
                    AddHandler FieldDateTime.StateChanged, AddressOf Me.Field_StateChanged
                    AddHandler FieldDateTime.ErrorTextChanged, AddressOf Me.Field_ErrorTextChanged

            End Select
        Else
            Select Case EditType
                Case EditTypes.Text
                    CurrentEdit = CurrentEditTypes.FieldText

                    FieldText.Visible = True
                    FieldText.Location = New Point(2, 2)
                    FieldText.ReadOnly = gReadOnly Or gUserProtect
                    FieldText.TabStop = Not gReadOnly And Not gUserProtect
                    FieldText.ContextMenuStrip = Me.ContextMenuStrip

                Case EditTypes.DateTime, EditTypes.Date, EditTypes.Time

                    CurrentEdit = CurrentEditTypes.FieldDateTime
                    SetDateTime()
                    FieldText.Visible = False

                    pnlPrompt.Visible = False
                    FieldDateTime.Visible = True

                    gTextFormat = EntryFormat.AutoWidth

                    FieldDateTime.Location = New Point(2, 2)
                    FieldDateTime.ReadOnly = gReadOnly
                    FieldDateTime.TabStop = Not gReadOnly

                    Select Case EditType
                        Case EditTypes.Date
                            FieldDateTime.ProviderFormat = CTSDateTime.DisplayType.DateOnly
                            FieldDateTime.EditFormat = CTSDateTime.DisplayType.DateOnly
                        Case EditTypes.Time
                            FieldDateTime.ProviderFormat = CTSDateTime.DisplayType.TimeOnly
                            FieldDateTime.EditFormat = CTSDateTime.DisplayType.TimeOnly
                        Case EditTypes.DateTime
                            FieldDateTime.ProviderFormat = CTSDateTime.DisplayType.Timestamp
                            FieldDateTime.EditFormat = CTSDateTime.DisplayType.Timestamp
                    End Select
                    FieldDateTime.Reload()

                    RemoveHandler FieldDateTime.StateChanged, AddressOf Me.Field_StateChanged
                    RemoveHandler FieldDateTime.ErrorTextChanged, AddressOf Me.Field_ErrorTextChanged
                    AddHandler FieldDateTime.StateChanged, AddressOf Me.Field_StateChanged
                    AddHandler FieldDateTime.ErrorTextChanged, AddressOf Me.Field_ErrorTextChanged

                Case EditTypes.Combo
                    CurrentEdit = CurrentEditTypes.FieldCombo
                    If pnlDataInner.Controls.Item("FieldCombo") Is Nothing Then
                        FieldCombo = New CTS.Controls.ComboBoxCTS
                        FieldCombo.Name = "FieldCombo"
                        FieldCombo.FlatStyle = FlatStyle.Flat
                        FieldCombo.ContextMenuStrip = Me.ContextMenuStrip
                        FieldCombo.DropDownStyle = ComboBoxStyle.DropDownList
                        pnlDataInner.Controls.Add(FieldCombo)
                    End If

                    FieldText.Visible = False

                    FieldCombo.Visible = True
                    FieldCombo.Location = New Point(2, 2)
                    FieldCombo.ReadOnly = gReadOnly
                    FieldCombo.TabStop = Not gReadOnly

                    RemoveHandler FieldCombo.StateChanged, AddressOf Me.Field_StateChanged
                    RemoveHandler FieldCombo.ErrorTextChanged, AddressOf Me.Field_ErrorTextChanged
                    AddHandler FieldCombo.StateChanged, AddressOf Me.Field_StateChanged
                    AddHandler FieldCombo.ErrorTextChanged, AddressOf Me.Field_ErrorTextChanged

                Case EditTypes.Check
                    CurrentEdit = CurrentEditTypes.FieldCheck
                    If pnlDataInner.Controls.Item("FieldCheck") Is Nothing Then
                        FieldCheck = New CheckBoxCTS
                        FieldCheck.Name = "FieldCheck"
                        FieldCheck.FlatStyle = FlatStyle.Flat
                        FieldCheck.ContextMenuStrip = Me.ContextMenuStrip
                        pnlDataInner.Controls.Add(FieldCheck)
                    End If

                    FieldText.Visible = False

                    FieldCheck.Visible = True
                    FieldCheck.Location = New Point(2, 2)
                    FieldCheck.Enabled = Not gReadOnly
                    FieldCheck.TabStop = Not gReadOnly

                    RemoveHandler FieldCheck.StateChanged, AddressOf Me.Field_StateChanged
                    AddHandler FieldCheck.StateChanged, AddressOf Me.Field_StateChanged
                    RemoveHandler FieldCheck.CheckedChanged, AddressOf Me.FieldCheck_CheckedChanged
                    AddHandler FieldCheck.CheckedChanged, AddressOf Me.FieldCheck_CheckedChanged

                Case EditTypes.RadioButtons

            End Select
        End If
    End Sub

    Private Sub HideAllEditTypes()
        FieldText.Visible = False
        If FieldDateTime IsNot Nothing Then FieldDateTime.Visible = False
        If FieldCombo IsNot Nothing Then FieldCombo.Visible = False
        If FieldCheck IsNot Nothing Then FieldCheck.Visible = False
    End Sub

    'LABEL: Sets the width;  Sets the visibility; Allows for wrapping in the Label segment
    Private Sub FormatLabel()
        Debug.Print(Me.Name & ": FormatLabel-" & Initializing & Loading & Formatting)
        Formatting = True

        Select Case gLabelFormat
            Case SectionFormat.Hidden
                pnlFieldLabel.Visible = False

            Case SectionFormat.AutoWidth
                FieldLabel.AutoSize = True
                FieldLabel.Height = FieldLabel.Font.Height

                'Use gLabelWidth as max value since we are in auto 
                If FieldLabel.Width < gLabelWidth Then
                    pnlFieldLabel.Width = FieldLabel.Width
                Else
                    pnlFieldLabel.Width = gLabelWidth
                    FieldLabel.AutoSize = False
                    FieldLabel.Width = gLabelWidth
                End If

                FieldLabel.MaximumSize = New Size(0, 0)
                pnlFieldLabel.Visible = True

            Case SectionFormat.FixedWidthNoWrap
                FieldLabel.AutoSize = False
                FieldLabel.Width = gLabelWidth
                FieldLabel.Height = FieldLabel.Font.Height
                FieldLabel.MaximumSize = New Size(0, 0)
                pnlFieldLabel.Width = gLabelWidth
                pnlFieldLabel.Visible = True

            Case SectionFormat.FixedWidthWrapped
                FieldLabel.AutoSize = True
                FieldLabel.MaximumSize = New Size(gLabelWidth, 0)
                pnlFieldLabel.Width = gLabelWidth
                pnlFieldLabel.Visible = True
        End Select

        Formatting = False

    End Sub

    'DATA: Set the width based on format and/or width property 
    Private Sub FormatData()
        Debug.Print(Me.Name & ": FormatData-" & Initializing & Loading & Formatting)
        Formatting = True

        Select Case EditType
            Case EditTypes.UseDataType

                Select Case gProviderInfo.Type
                    Case ProviderInfo.DB2ProviderDataTypes.Character,
                         ProviderInfo.DB2ProviderDataTypes.Numeric

                        FormatTextBoxSize()

                    Case ProviderInfo.DB2ProviderDataTypes.DB2Time,
                           ProviderInfo.DB2ProviderDataTypes.DB2Date,
                           ProviderInfo.DB2ProviderDataTypes.DB2Timestamp

                        SetDateTime()
                        Dim FieldDateTime As CTSDateTime = pnlDataInner.Controls.Item("FieldDateTime")
                        pnlDataOuter.Width = FieldDateTime.Width + pnlDataInner.Padding.Left + pnlDataInner.Padding.Right

                End Select

            Case EditTypes.Text
                FormatTextBoxSize()

            Case EditTypes.Date, EditTypes.Time, EditTypes.DateTime
                SetDateTime()
                Dim FieldDateTime As CTSDateTime = pnlDataInner.Controls.Item("FieldDateTime")
                pnlDataOuter.Width = FieldDateTime.Width + pnlDataInner.Padding.Left + pnlDataInner.Padding.Right

            Case EditTypes.Check
                Dim checkb As CheckBoxCT2 = pnlDataInner.Controls.Item("FieldCheck")
                If gTextFormat = EntryFormat.FixedWidth Then checkb.Width = gTextWidth
                pnlDataOuter.Width = checkb.Width + pnlDataInner.Padding.Left + pnlDataInner.Padding.Right

            Case EditTypes.Combo
                Dim combo As ComboBoxCTS = pnlDataInner.Controls.Item("FieldCombo")
                If gTextFormat = EntryFormat.FixedWidth Then combo.Width = gTextWidth
                pnlDataOuter.Width = combo.Width + pnlDataInner.Padding.Left + pnlDataInner.Padding.Right

            Case EditTypes.RadioButtons

        End Select

        'Calculate the Height
        Dim DataH As Integer = 0
        Select Case CurrentEdit
            Case CurrentEditTypes.FieldText
                DataH = FieldText.Height
            Case CurrentEditTypes.FieldDateTime
                DataH = FieldDateTime.Height
            Case CurrentEditTypes.FieldCombo
                DataH = FieldCombo.Height
            Case CurrentEditTypes.FieldCheck
                DataH = FieldCheck.Height

                'this needs attention! 
            Case CurrentEditTypes.FieldRadio
                DataH = FieldText.Height
        End Select

        'set height of the inner container (creates outline box)
        pnlDataInner.Height = pnlDataInner.Padding.Top + DataH + pnlDataInner.Padding.Bottom


        Formatting = False

    End Sub

    Private Sub FormatTextBoxSize()
        Select Case gTextFormat
            Case EntryFormat.AutoWidth

                'Calculate Width based on Max size
                Using g As Graphics = Me.CreateGraphics
                    Dim strSample As String = StrDup(FieldText.MaxLength, "W")
                    Dim EstimatedWidth As Integer = g.MeasureString(strSample, FieldText.Font).Width
                    If EstimatedWidth < MinTextWidth Then
                        EstimatedWidth = MinTextWidth
                    End If

                    If gTextLines > 1 Then
                        FieldText.Height = (Convert.ToInt32(g.MeasureString("A", FieldText.Font).Height) * gTextLines) + 6
                        pnlDataOuter.Height = FieldText.Height + pnlDataInner.Padding.Top + pnlDataInner.Padding.Bottom
                    End If

                    'Use gTextWidth as max value since we are in auto 
                    If EstimatedWidth > gTextWidth Then
                        EstimatedWidth = gTextWidth
                    End If

                    If (EstimatedWidth / gTextLines) < MinTextWidth Then
                        FieldText.Width = MinTextWidth
                    Else
                        FieldText.Width = EstimatedWidth / gTextLines
                    End If

                    pnlDataOuter.Width = FieldText.Width + pnlDataInner.Padding.Left + pnlDataInner.Padding.Right

                    '!!! debugging enable just for testing
                    'gTextWidth = FieldText.Width + pnlText.Padding.Left + pnlText.Padding.Right
                End Using

            Case EntryFormat.FixedWidth
                If gTextLines > 1 Then
                    Using g As Graphics = Me.CreateGraphics
                        FieldText.Height = (Convert.ToInt32(g.MeasureString("A", FieldText.Font).Height) * gTextLines) + 8
                        pnlDataOuter.Height = FieldText.Height + pnlDataInner.Padding.Top + pnlDataInner.Padding.Bottom
                    End Using
                End If
                FieldText.Width = gTextWidth
                pnlDataOuter.Width = FieldText.Width + pnlDataInner.Padding.Left + pnlDataInner.Padding.Right
        End Select
    End Sub

    Private Sub FormatPrompt()
        Select Case CurrentEdit
            Case CurrentEditTypes.FieldText
                If gShowPrompt Then
                    pnlPrompt.Visible = True
                    btnPrompt.Visible = True
                Else
                    pnlPrompt.Visible = False
                End If

            Case CurrentEditTypes.FieldDateTime
                pnlPrompt.Visible = False
                If gShowPrompt Then
                    FieldDateTime.ShowCalendarPrompt = True
                Else
                    FieldDateTime.ShowCalendarPrompt = False
                End If
                FieldDateTime.Reload()

            Case Else
                pnlPrompt.Visible = False
        End Select
    End Sub

    ''' <summary>
    '''Sets the visibility and allow for wrapping in the Description segment; Does NOT set the height/width
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FormatDescription()
        Debug.Print(Me.Name & ": FormatDescription-" & Initializing & Loading & Formatting)
        Formatting = True

        Select Case gDescriptionFormat
            Case SectionFormat.Hidden
                pnlFieldDescription.Visible = False

            Case SectionFormat.AutoWidth
                FieldDescription.AutoSize = True
                FieldDescription.Height = FieldDescription.Font.Height
                FieldDescription.MaximumSize = New Size(0, 0)

                Select Case gControlLayout
                    Case ControlLayouts.AutoSize
                        pnlFieldDescription.Width = FieldDescription.Width
                    Case ControlLayouts.FixedSize
                End Select

                pnlFieldDescription.Visible = True

            Case SectionFormat.FixedWidthNoWrap
                FieldDescription.AutoSize = False
                FieldDescription.Width = gDescriptionWidth
                FieldDescription.Height = FieldDescription.Font.Height
                FieldDescription.MaximumSize = New Size(0, 0)

                Select Case gControlLayout
                    Case ControlLayouts.AutoSize
                        pnlFieldDescription.Width = FieldDescription.Width
                    Case ControlLayouts.FixedSize
                End Select

                pnlFieldDescription.Visible = True

            Case SectionFormat.FixedWidthWrapped
                FieldDescription.AutoSize = True

                If pnlFieldDescription.Width < gDescriptionWidth Then
                    FieldDescription.MaximumSize = New Size(pnlFieldDescription.Width, 0)
                Else
                    FieldDescription.MaximumSize = New Size(gDescriptionWidth, 0)
                End If

                Select Case gControlLayout
                    Case ControlLayouts.AutoSize
                        pnlFieldDescription.Width = FieldDescription.Width
                    Case ControlLayouts.FixedSize
                End Select

                pnlFieldDescription.Visible = True
        End Select

        Formatting = False

    End Sub

    ''' <summary>
    ''' Adds the height/width of each visible segment to determine overall control size;  Does NOT set the height/width
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FormatControlSize()
        Formatting = True

        Dim NewHeight As Integer = Me.Height
        Dim NewWidth As Integer = Me.Width

        If Me.Dock <> DockStyle.Fill And Me.Dock <> DockStyle.Top And Me.Dock <> DockStyle.Bottom Then

            'Determine Control Width
            Dim w As Integer = Me.Padding.Left + Me.Padding.Right

            w += pnlDataOuter.Width
            If pnlFieldLabel.Visible Then w += pnlFieldLabel.Width
            If pnlFieldDescription.Visible Then w += FieldDescription.Width
            If pnlPrompt.Visible Then w += pnlPrompt.Width

            'Honor the Maxsize property on the usercontrol
            If Me.MaximumSize.Width > 0 AndAlso Me.MaximumSize.Width <= w Then
                w = Me.MaximumSize.Width
            End If

            NewWidth = w
        End If

        If Me.Dock <> DockStyle.Fill Then

            'Determine Control Height
            Dim DataH As Integer = 0
            Select Case CurrentEdit
                Case CurrentEditTypes.FieldText
                    DataH = FieldText.Height
                Case CurrentEditTypes.FieldDateTime
                    DataH = FieldDateTime.Height
                Case CurrentEditTypes.FieldCombo
                    DataH = FieldCombo.Height
                Case CurrentEditTypes.FieldCheck
                    DataH = FieldCheck.Height
            End Select

            'Start with the text/data height 
            Dim MaxH As Integer = Me.Padding.Top + pnlDataOuter.Padding.Top + pnlDataInner.Padding.Top + DataH _
                               + pnlDataInner.Padding.Bottom + pnlDataOuter.Padding.Bottom + Me.Padding.Bottom

            'Increase to label height, if needed
            If MaxH < Me.Padding.Top + pnlFieldLabel.Padding.Top + FieldLabel.Height + pnlFieldLabel.Padding.Bottom + Me.Padding.Bottom Then
                MaxH = Me.Padding.Top + pnlFieldLabel.Padding.Top + FieldLabel.Height + pnlFieldLabel.Padding.Bottom + Me.Padding.Bottom
            End If

            'Increase to description height, if needed 
            If MaxH < Me.Padding.Top + pnlFieldDescription.Padding.Top + FieldDescription.Height + pnlFieldDescription.Padding.Bottom + Me.Padding.Bottom Then
                MaxH = Me.Padding.Top + pnlFieldDescription.Padding.Top + FieldDescription.Height + pnlFieldDescription.Padding.Bottom + Me.Padding.Bottom
            End If

            NewHeight = MaxH
        End If

        Me.Size = New Size(NewWidth, NewHeight)
        Me.Refresh()

        Debug.Print(Me.Name & ": FormatControlSize w=" & Me.Width & " h=" & Me.Height)
        Formatting = False
    End Sub

    Private Sub SetDateTime()
        If pnlDataInner.Controls.Item("FieldDateTime") Is Nothing Then
            FieldDateTime = New CT2DateTime
            FieldDateTime.Name = "FieldDateTime"
            pnlDataInner.Controls.Add(FieldDateTime)
        End If
    End Sub

    Private Sub SetColor()

        Select Case CurrentEdit
            Case CurrentEditTypes.FieldText
                Select Case True
                    Case FieldText.IsDirty And gErrorText = String.Empty
                        pnlDataInner.BackColor = EditPending

                    Case gErrorText <> String.Empty
                        pnlDataInner.BackColor = EditErrorBack

                    Case Else
                        pnlDataInner.BackColor = Color.Transparent
                        pnlDataInner.ForeColor = Color.Black
                End Select

            Case CurrentEditTypes.FieldDateTime
                Dim FieldDateTime As CTSDateTime = pnlDataInner.Controls.Item("FieldDateTime")
                Select Case True
                    Case FieldDateTime.IsDirty And gErrorText = String.Empty
                        pnlDataInner.BackColor = EditPending

                    Case gErrorText <> String.Empty
                        pnlDataInner.BackColor = EditErrorBack

                    Case Else
                        pnlDataInner.BackColor = Color.Transparent
                End Select

            Case CurrentEditTypes.FieldCombo
                If FieldCombo.IsDirty And gErrorText = String.Empty Then
                    pnlDataInner.BackColor = EditPending

                ElseIf gErrorText <> String.Empty Then
                    pnlDataInner.BackColor = EditErrorBack

                Else
                    pnlDataInner.BackColor = Color.Transparent
                End If

            Case CurrentEditTypes.FieldCheck
                If FieldCheck.IsDirty Then
                    pnlDataInner.BackColor = EditPending

                ElseIf gErrorText <> String.Empty Then
                    pnlDataInner.BackColor = EditErrorBack

                Else
                    pnlDataInner.BackColor = Color.Transparent
                End If

            Case CurrentEditTypes.FieldRadio

        End Select

    End Sub

    Public Sub SetErrorToolTip(ByVal tt As ToolTip, ByVal message As String)
        tt.ToolTipIcon = ToolTipIcon.Error
        tt.ToolTipTitle = "Error"
        ApplyToolTipToChildControls(pnlDataOuter, tt, message)
    End Sub
    Private Sub ApplyToolTipToChildControls(ByVal TopControl As Control, ByVal tt As ToolTip, ByVal message As String)
        For Each Control In TopControl.Controls
            tt.SetToolTip(Control, message)
            If TopControl.HasChildren Then
                ApplyToolTipToChildControls(Control, tt, message)
            End If
        Next
    End Sub
#End Region

#Region "Formating & Sizing"

    Private Sub FieldLabel_Resize(sender As Object, e As EventArgs) Handles FieldLabel.Resize
        If Loaded Then
            If Not Initializing And Not Loading And Not Formatting Then
                FormatLabel()
                If gControlLayout = ControlLayouts.AutoSize Then FormatControlSize()
            End If
        End If
    End Sub

    Private Sub FieldDescription_Resize(sender As Object, e As EventArgs) Handles FieldDescription.Resize
        If Loaded Then
            If Not Initializing And Not Loading And Not Formatting Then
                FormatDescription()
                If gControlLayout = ControlLayouts.AutoSize Then FormatControlSize()
            End If
        End If
    End Sub

    Private Sub FieldText_Resize(sender As Object, e As EventArgs) Handles FieldText.Resize
        If Loaded Then
            If Not Initializing And Not Loading And Not Formatting Then
                If gControlLayout = ControlLayouts.AutoSize Then FormatControlSize()
            End If
        End If
    End Sub

    Private Sub CT2Field_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        If Loaded Then
            If Not Initializing And Not Loading And Not Formatting Then
                Debug.Print(Me.Name & ": Resize-" & Initializing & Loading & Formatting)
                If gControlLayout = ControlLayouts.AutoSize Then FormatControlSize()
            End If
        End If
    End Sub

#End Region

    Private Sub Field_StateChanged(sender As Object) Handles FieldText.StateChanged
        Me.IsDirty = sender.isdirty
        SetColor()
    End Sub

    Private Sub Field_ErrorTextChanged(sender As Object) Handles FieldText.ErrorTextChanged
        Me.ErrorText = sender.ErrorText
        'SetColor()
    End Sub

    Private Sub FieldCheck_CheckedChanged(sender As Object, e As EventArgs)
        RaiseEvent CheckChanged(Me, e)
    End Sub

    Public Event PromptClick(sender As Object, e As EventArgs)
    Private Sub btnPrompt_Click(sender As Object, e As EventArgs) Handles btnPrompt.Click
        RaiseEvent PromptClick(Me, e)
    End Sub

    ''' <summary>
    ''' Sets the value of the data portion of CT2Field. 
    ''' </summary>
    ''' <param name="NewValue">The value to be set.  This should match the data type of the provider/edit type</param>
    ''' <param name="CausesEvents">True will let the control react as it would when a user enters a value. Default is False.</param>
    ''' <remarks></remarks>
    Public Sub SetValue(ByVal NewValue As Object, Optional CausesEvents As Boolean = False)

        If Not CausesEvents Then
            Me.SuppressOn()
        End If

        If gEditType = CT2Field.EditTypes.UseDataType Then
            Select Case gProviderInfo.Type
                Case ProviderInfo.DB2ProviderDataTypes.Character,
                     ProviderInfo.DB2ProviderDataTypes.Numeric

                    Try
                        FieldText.Text = NewValue
                        'Only accept changes when NOT trying to let events 
                        If Not CausesEvents Then
                            FieldText.AcceptChanges()
                        End If

                    Catch ex As Exception
                        Debug.Print(ex.Message)
                    End Try

                Case ProviderInfo.DB2ProviderDataTypes.DB2Date,
                     ProviderInfo.DB2ProviderDataTypes.DB2Time,
                     ProviderInfo.DB2ProviderDataTypes.DB2Timestamp

                    Try
                        SetDateTime()
                        FieldDateTime.Value = NewValue

                        'Only accept changes when NOT trying to let events 
                        If Not CausesEvents Then
                            FieldDateTime.AcceptChanges()
                        End If
                    Catch ex As Exception
                        Debug.Print(ex.Message)
                    End Try

            End Select

        Else
            Select Case gEditType
                Case EditTypes.Text
                    Try : FieldText.Text = NewValue : Catch : End Try

                    'Only accept changes when NOT trying to let events 
                    If Not CausesEvents Then
                        FieldText.AcceptChanges()
                    End If

                Case EditTypes.Combo

                    If TypeOf NewValue Is [Enum] Then
                        NewValue = CInt(NewValue)
                    End If

                    Try : FieldCombo.SelectedValue = NewValue : Catch : End Try

                    'Only accept changes when NOT trying to let events 
                    If Not CausesEvents Then
                        FieldCombo.AcceptChanges()
                    End If


                Case CT2Field.EditTypes.DateTime, EditTypes.Date, EditTypes.Time

                    SetDateTime()
                    Try : FieldDateTime.Value = NewValue : Catch : End Try

                    'Only accept changes when NOT trying to let events 
                    If Not CausesEvents Then
                        FieldDateTime.AcceptChanges()
                    End If

                Case CT2Field.EditTypes.Check

                    If TypeOf NewValue Is Integer Then
                        Dim int As Integer = DirectCast(NewValue, Integer)
                        Select Case int
                            Case System.Windows.Forms.CheckState.Checked
                                FieldCheck.CheckState = CheckState.Checked

                            Case System.Windows.Forms.CheckState.Unchecked
                                FieldCheck.CheckState = CheckState.Unchecked

                            Case Else
                                FieldCheck.CheckState = CheckState.Indeterminate
                        End Select

                        'Only accept changes when NOT trying to let events 
                        If Not CausesEvents Then
                            FieldCheck.AcceptChanges()
                        End If

                    ElseIf TypeOf NewValue Is String Then
                        Dim str As String = DirectCast(NewValue, String)
                        Select Case str
                            Case FieldCheck.CheckedValue
                                FieldCheck.CheckState = CheckState.Checked

                            Case FieldCheck.UncheckedValue
                                FieldCheck.CheckState = CheckState.Unchecked

                            Case Else
                                FieldCheck.CheckState = CheckState.Indeterminate
                        End Select

                        'Only accept changes when NOT trying to let events 
                        If Not CausesEvents Then
                            FieldCheck.AcceptChanges()
                        End If
                    End If

                Case CT2Field.EditTypes.RadioButtons
            End Select
        End If

        If Not CausesEvents Then
            Me.SuppressOff()

            'turn dirty flag off for this control; NOT to property.. that would cause an event to fire
            gIsDirty = False
            SetColor()
        End If

    End Sub

    ''' <summary>
    ''' Gets the value of the data portion of CT2Field.
    ''' </summary>
    ''' <returns>The object returned may be string or datetime depending on the edittype.</returns>
    ''' <remarks></remarks>
    Public Function GetValue() As Object
        Dim CurValue As Object = Nothing

        If gEditType = CT2Field.EditTypes.UseDataType Then
            Select Case gProviderInfo.Type
                Case ProviderInfo.DB2ProviderDataTypes.Character,
                     ProviderInfo.DB2ProviderDataTypes.Numeric

                    CurValue = FieldText.Text

                Case ProviderInfo.DB2ProviderDataTypes.DB2Date,
                     ProviderInfo.DB2ProviderDataTypes.DB2Time,
                     ProviderInfo.DB2ProviderDataTypes.DB2Timestamp

                    CurValue = FieldDateTime.Value

            End Select

        Else
            Select Case gEditType
                Case CT2Field.EditTypes.Text
                    CurValue = FieldText.Text

                Case CT2Field.EditTypes.Combo
                    CurValue = FieldCombo.SelectedValue

                Case CT2Field.EditTypes.RadioButtons

                Case CT2Field.EditTypes.DateTime, EditTypes.Date, EditTypes.Time
                    CurValue = FieldDateTime.Value

                Case CT2Field.EditTypes.Check
                    If FieldCheck.Value = String.Empty Then
                        CurValue = FieldCheck.Checked
                    Else
                        CurValue = FieldCheck.Value
                    End If

            End Select
        End If

        Return CurValue
    End Function

    Public Sub AcceptChanges()

        Try
            Select Case CurrentEdit
                Case CT2Field.CurrentEditTypes.FieldText
                    FieldText.AcceptChanges()
                Case CT2Field.CurrentEditTypes.FieldDateTime
                    FieldDateTime.AcceptChanges()
                Case CT2Field.CurrentEditTypes.FieldCombo
                    FieldCombo.AcceptChanges()
                Case CT2Field.CurrentEditTypes.FieldCheck
                    FieldCheck.AcceptChanges()
            End Select

            Me.IsDirty = False

        Catch ex As Exception
        End Try

    End Sub

    ''' <summary>
    ''' Puts the edit portion of CT2Field in suppress mode.  Suppress mode suppresses some of the events that are fired while changing properties.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SuppressOn()
        Select Case CurrentEdit
            Case CurrentEditTypes.FieldText
                FieldText.Suppress = True

            Case CurrentEditTypes.FieldDateTime
                'FieldDateTime.???

            Case CurrentEditTypes.FieldCombo
                FieldCombo.Suppress = True

            Case CurrentEditTypes.FieldCheck
                FieldCheck.Suppress = True
        End Select
    End Sub

    ''' <summary>
    ''' Removes the edit portion of CT2Field from suppress mode. 
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SuppressOff()
        Select Case CurrentEdit
            Case CurrentEditTypes.FieldText
                FieldText.Suppress = False
            Case CurrentEditTypes.FieldDateTime
                'FieldDateTime.???
            Case CurrentEditTypes.FieldCombo
                FieldCombo.Suppress = False
            Case CurrentEditTypes.FieldCheck
                FieldCheck.Suppress = False
        End Select
    End Sub


#Region "DataSource (Combo/Check/Radio)"
    Public Sub ClearDataSource()
        gItems = Nothing

        Select Case CurrentEdit
            Case CurrentEditTypes.FieldCombo
                FieldCombo.DataSource = Nothing

            Case CurrentEditTypes.FieldRadio

        End Select
    End Sub

    Public Sub SetDataSource(ByVal DataSource As Object, ByVal DisplayMember As String, ByVal ValueMember As String)
        Select Case CurrentEdit
            Case CurrentEditTypes.FieldCombo
                FieldCombo.DataSource = Nothing
                FieldCombo.DataSource = DataSource
                FieldCombo.DisplayMember = DisplayMember
                FieldCombo.ValueMember = ValueMember
                FieldCombo.SelectedIndex = -1

            Case CurrentEditTypes.FieldRadio



        End Select
    End Sub

    Public Sub SetDataSource(ByVal DataSource As Object, ByVal DisplayMember As String)
        SetDataSource(DataSource, DisplayMember, DisplayMember)
    End Sub

    Public Sub SetDataValues(ByVal ActualValue As Object, ByVal DisplayValue As String)

        Try
            Select Case CurrentEdit
                Case CurrentEditTypes.FieldCombo

                    FieldCombo.DataSource = Nothing
                    If IsNothing(gItems) Then
                        gItems = New List(Of GenericItem)
                    End If
                    gItems.Add(New GenericItem(ActualValue, DisplayValue))

                    FieldCombo.DisplayMember = "DisplayValue"
                    FieldCombo.ValueMember = "ActualValue"
                    FieldCombo.DataSource = gItems
                    FieldCombo.SelectedIndex = -1

                Case CurrentEditTypes.FieldRadio


            End Select

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Sub SetCheckData(ByVal CheckValue As String, ByVal UncheckValue As String, ByVal CheckDisplay As String, ByVal UncheckDisplay As String)
        Try
            If CurrentEdit = CurrentEditTypes.FieldCheck Then
                Dim cb As CheckBoxCTS = FieldCheck
                cb.CheckedValue = CheckValue
                cb.UncheckedValue = UncheckValue
                cb.CheckedDisplay = CheckDisplay
                cb.UncheckedDisplay = UncheckDisplay
                If cb.Checked Then

                Else

                End If
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Sub SetCheckData(ByVal CheckValue As String, ByVal UncheckValue As String)
        SetCheckData(CheckValue, UncheckValue, String.Empty, String.Empty)
    End Sub

#End Region


#Region "ContextMenu"

    Private Sub cmsField_Opening(sender As Object, e As CancelEventArgs) Handles cmsField.Opening


        If Clipboard.ContainsText Then
            miPaste.Enabled = True
        Else
            miPaste.Enabled = False
        End If

        Select Case Me.EditType
            Case EditTypes.DateTime, EditTypes.Date, EditTypes.Time
                miUndo.Enabled = False
                miCut.Enabled = False
                miCopy.Enabled = False
                miPaste.Enabled = False
                miClear.Enabled = False

            Case EditTypes.Text
                If FieldText.SelectedText <> String.Empty Then
                    miCut.Enabled = True
                    miCopy.Enabled = True
                Else
                    miCut.Enabled = False
                    miCopy.Enabled = False
                End If
                miUndo.Enabled = True
                If FieldText.Text <> String.Empty Then
                    miClear.Enabled = True
                Else
                    miClear.Enabled = False
                End If

            Case EditTypes.Combo
                If FieldCombo.SelectedText <> String.Empty Then
                    miCut.Enabled = True
                    miCopy.Enabled = True
                Else
                    miCut.Enabled = False
                    miCopy.Enabled = False
                End If
                miUndo.Enabled = False
                If FieldCombo.Text <> String.Empty Then
                    miClear.Enabled = True
                Else
                    miClear.Enabled = False
                End If

            Case EditTypes.Check
                miUndo.Enabled = False
                miCut.Enabled = False
                miCopy.Enabled = False
                miPaste.Enabled = False
                miClear.Enabled = False
        End Select

    End Sub

    Private Sub miPaste_Click(sender As Object, e As EventArgs) Handles miPaste.Click
        Select Case Me.EditType
            Case EditTypes.DateTime, EditTypes.Date, EditTypes.Time
                FieldDateTime.miPaste.PerformClick()
            Case EditTypes.Text
                FieldText.Text = Clipboard.GetText
            Case EditTypes.Combo
                FieldCombo.Text = Clipboard.GetText

        End Select
    End Sub

#End Region

    Private Sub miUndo_Click(sender As Object, e As EventArgs) Handles miUndo.Click
        Select Case Me.EditType
            Case EditTypes.DateTime, EditTypes.Date, EditTypes.Time
                FieldDateTime.miResetPrevious.PerformClick()
            Case EditTypes.Text
                FieldText.Text = FieldText.OriginalText
            Case EditTypes.Combo
                FieldCombo.Text = FieldCombo.OriginalText

        End Select
    End Sub

    Private Sub miClear_Click(sender As Object, e As EventArgs) Handles miClear.Click
        Select Case Me.EditType
            Case EditTypes.DateTime, EditTypes.Date, EditTypes.Time
                FieldDateTime.Value = Date.MinValue
            Case EditTypes.Text
                FieldText.Clear()
            Case EditTypes.Combo
                FieldCombo.Text = String.Empty
        End Select
    End Sub

    Private Sub CT2Field_Validating(sender As Object, e As CancelEventArgs) Handles MyBase.Validating


        If gDataValidate Then

            Debug.Print("CT2Field_Validating: " & Me.Name & Me.CurrentEdit.ToString & " ts:" & Me.FieldText.TabStop)
            Dim NewErrorText As String = String.Empty
            Dim str As String

            Try
                Select Case CurrentEdit
                    Case CT2Field.CurrentEditTypes.FieldText,
                        CurrentEditTypes.FieldCheck,
                        CurrentEditTypes.FieldCombo,
                        CurrentEditTypes.FieldRadio

                        str = Me.GetValue()

                        Select Case Me.ProviderInfo.Type
                            Case CTS.Controls.ProviderInfo.DB2ProviderDataTypes.Character
                                If Trim(str).Length > Me.ProviderInfo.Size AndAlso Me.ProviderInfo.Size > 0 Then
                                    NewErrorText = String.Format("Value entered is too long.  Must be less than or equal to {0} characters",
                                                                 Me.ProviderInfo.Size)
                                End If

                            Case CTS.Controls.ProviderInfo.DB2ProviderDataTypes.Numeric
                                If IsNumeric(str) Then

                                    Try
                                        If Me.ProviderInfo.Scale > 0 Then
                                            Dim Max As Decimal = CLng(Replace(Space(Me.ProviderInfo.Size), " ", "9"))
                                            'Max = Max / 10 * Scale()
                                            Dim dec As Decimal = CDec(str)


                                        Else
                                            Dim Max As Long = CLng(Replace(Space(Me.ProviderInfo.Size), " ", "9"))
                                            Dim Lng As Long = CLng(str)

                                            If Lng > Max Or Lng < (Max * -1) Then
                                                NewErrorText = String.Format("Numeric Data Error. Value must be between -{0} and {0}.", Max)
                                            End If
                                        End If
                                    Catch ex As Exception
                                        NewErrorText = String.Format("Numeric Data Error ({0}/{1}).",
                                                        Me.ProviderInfo.Size, Me.ProviderInfo.Scale)
                                    End Try
                                Else
                                    NewErrorText = String.Format("Numeric Data Error ({0}/{1}).",
                                                   Me.ProviderInfo.Size, Me.ProviderInfo.Scale)
                                End If
                        End Select


                    Case CurrentEditTypes.FieldDateTime
                        'handled by the control
                End Select

                Me.ErrorText = NewErrorText

            Catch ex As Exception
            End Try

        End If

    End Sub

    Private Sub miProtect_Click(sender As Object, e As EventArgs) Handles miProtect.Click
        UserProtect = miProtect.Checked
    End Sub

    Private Sub FieldText_DoubleClick(sender As Object, e As EventArgs) Handles FieldText.DoubleClick

        'Dim ctsf As New CT2Field
        'ctsf.ProviderInfo = Me.ProviderInfo
        'ctsf.Dock = DockStyle.Fill
        'ctsf.SetValue(Me.GetValue())
        'ctsf.TextLines = 100
        'ctsf.FieldText.ScrollBars = ScrollBars.Vertical
        'ctsf.TextWidth = 300
        'ctsf.TextFormat = EntryFormat.FixedWidth
        'ctsf.LabelFormat = SectionFormat.Hidden

        'Dim ctsd As New CTSDialog
        'ctsd.Control = ctsf
        'ctsd.Size = New Size(400, 200)
        'ctsd.Title = ctsf.LabelText
        'ctsd.ShowDialog()

    End Sub

    Public Shadows Event KeyPress(sender As Object, e As KeyPressEventArgs)
    Private Sub FieldText_KeyPress(sender As Object, e As KeyPressEventArgs) Handles FieldText.KeyPress
        RaiseEvent KeyPress(Me, e)
    End Sub

    Public Shadows Event KeyDown(sender As Object, e As KeyEventArgs)
    Private Sub FieldText_KeyDown(sender As Object, e As KeyEventArgs) Handles FieldText.KeyDown
        RaiseEvent KeyDown(Me, e)
    End Sub

    Public Shadows Event KeyUp(sender As Object, e As KeyEventArgs)
    Private Sub FieldText_KeyUp(sender As Object, e As KeyEventArgs) Handles FieldText.KeyUp
        RaiseEvent KeyUp(Me, e)
    End Sub

    Public Shadows Event PreviewKeyDown(sender As Object, e As PreviewKeyDownEventArgs)
    Private Sub FieldText_PreviewKeyDown(sender As Object, e As PreviewKeyDownEventArgs) Handles FieldText.PreviewKeyDown
        RaiseEvent PreviewKeyDown(Me, e)
    End Sub

    Protected Friend Class SubWindow
        Inherits System.Windows.Forms.ToolStripDropDown
        Private _content As System.Windows.Forms.Control
        Private _host As System.Windows.Forms.ToolStripControlHost

        Public Sub New(content As System.Windows.Forms.Control)
            'Basic setup...
            Me.AutoSize = False
            Me.DoubleBuffered = True
            Me.ResizeRedraw = True

            Me._content = content
            Me._host = New System.Windows.Forms.ToolStripControlHost(content)

            'Positioning and Sizing
            Me.MinimumSize = content.MinimumSize
            Me.MaximumSize = content.Size
            Me.Size = content.Size
            content.Location = Point.Empty

            'Add the host to the list
            Me.Items.Add(Me._host)
        End Sub

    End Class

    Private Sub pnlFieldLabel_Resize(sender As Object, e As EventArgs) Handles pnlFieldLabel.Resize
        FieldLabel.Width = pnlFieldLabel.Width - pnlFieldLabel.padding.left - pnlFieldLabel.padding.right
    End Sub
End Class


<TypeConverter(GetType(ProviderInfoConverter))>
Public Class ProviderInfo
    Private gProvider As Providers = Providers.DB2
    Private gSchema As String
    Private gFile As String
    Private gField As String
    Private gType As DB2ProviderDataTypes = 6
    Private gSize As Integer = 0
    Private gScale As Integer = 0

    Public Enum Providers
        DB2
        Other
    End Enum

    Public Enum DB2ProviderDataTypes
        Numeric = 19
        Character = 6
        DB2Date = 12
        DB2Time = 13
        DB2Timestamp = 14
    End Enum

    <Browsable(True), NotifyParentProperty(True), EditorBrowsable(EditorBrowsableState.Always), DisplayName("Field/Property"), DefaultValue("")>
    Public Property Field() As String
        Get
            Return gField
        End Get
        Set(value As String)
            If gField <> value Then
                gField = value
            End If
        End Set
    End Property

    <Browsable(True), NotifyParentProperty(True), EditorBrowsable(EditorBrowsableState.Always), DefaultValue("")>
    Public Property File() As String
        Get
            Return gFile
        End Get
        Set(value As String)
            If gFile <> value Then
                gFile = value
            End If
        End Set
    End Property

    <Browsable(True), NotifyParentProperty(True), EditorBrowsable(EditorBrowsableState.Always), DefaultValue(6)>
    Public Property Type() As DB2ProviderDataTypes
        Get
            Return gType
        End Get
        Set(value As DB2ProviderDataTypes)

            If gType = DB2ProviderDataTypes.DB2Time Then
                gSize = "10"
            End If
            If gType = DB2ProviderDataTypes.DB2Date Then
                gSize = "10"
            End If
            If gType = DB2ProviderDataTypes.DB2Timestamp Then
                gSize = "26"
            End If

            If gType <> value Then
                gType = value
            End If

        End Set
    End Property

    <Browsable(True), NotifyParentProperty(True), EditorBrowsable(EditorBrowsableState.Always), DefaultValue(0)>
    Public Property Size() As Integer
        Get
            Return gSize
        End Get
        Set(value As Integer)
            If value < 0 Then
                Throw New ArgumentOutOfRangeException("Size", value, "must be >= 0")
            End If

            If gSize <> value Then
                gSize = value
            End If
        End Set
    End Property

    <Browsable(True), NotifyParentProperty(True), EditorBrowsable(EditorBrowsableState.Always), DefaultValue(0)>
    Public Property Scale() As Integer
        Get
            Return gScale
        End Get
        Set(value As Integer)
            If value < 0 Then
                Throw New ArgumentOutOfRangeException("Scale", value, "must be >= 0")
            End If

            If gScale <> value Then
                gScale = value
            End If
        End Set
    End Property

End Class

Public Class ProviderInfoConverter
    Inherits ExpandableObjectConverter

    ' This override prevents the PropertyGrid from displaying the full type name in the value cell. 
    Public Overrides Function ConvertTo(ByVal context As ITypeDescriptorContext,
                                        ByVal culture As CultureInfo,
                                        ByVal value As Object,
                                        ByVal destinationType As Type) As Object
        Dim StrDisplay As String = String.Empty

        If destinationType Is GetType(String) Then
            Dim v As ProviderInfo = value


            If v.Field = String.Empty Then
                StrDisplay = "(None)"
            Else
                If v.File = String.Empty Then
                    StrDisplay = v.Field
                Else
                    StrDisplay = v.File & "." & v.Field
                End If
            End If

            Select Case v.Type
                Case ProviderInfo.DB2ProviderDataTypes.Character
                    StrDisplay = Trim(StrDisplay) & " (" & CStr(v.Size) & ")"
                Case ProviderInfo.DB2ProviderDataTypes.Numeric
                    StrDisplay = Trim(StrDisplay) & " (" & CStr(v.Size) & "," & CStr(v.Scale) & ")"
                Case Else
                    StrDisplay = Trim(StrDisplay) & "," & v.Type.ToString
            End Select

            Return StrDisplay
        End If

        Return MyBase.ConvertTo(context, culture, value, destinationType)

    End Function

End Class


Public Class GenericItem

    Sub New(ByVal ActualValue As Object, ByVal DisplayValue As String)
        mValue = ActualValue
        mDisplay = DisplayValue
    End Sub
    Sub New()
    End Sub

    Public mValue As Object
    Public mDisplay As String
    Public Property ActualValue() As Object
        Get
            Return mValue
        End Get
        Set(ByVal Value As Object)
            mValue = Value
        End Set
    End Property

    Public Property DisplayValue() As String
        Get
            Return mDisplay
        End Get
        Set(ByVal Value As String)
            mDisplay = Value
        End Set
    End Property
End Class


Public Class CT2FieldCollection
    Inherits Collections.ObjectModel.KeyedCollection(Of String, CT2Field)

    Public Event StateChanged(ByVal sender As Object)
    Private BS As BindingSource

#Region "Properties"
    Private gIsDirty As Boolean
    <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Property IsDirty() As Boolean
        Get
            Return gIsDirty
        End Get
        Set(value As Boolean)
            If value <> gIsDirty Then
                gIsDirty = value
                RaiseEvent StateChanged(Me)
            End If
        End Set

    End Property

    Private gName As String = "DEFAULT"
    Property Name() As String
        Get
            Return gName
        End Get
        Set(value As String)
            gName = value
        End Set
    End Property
#End Region

    Public Sub New()
        MyBase.New()
        Me.Name = "Default"
    End Sub

    Public Sub New(Name As String)
        Me.Name = Name
    End Sub


    Public Sub dispose()

    End Sub


    Protected Overrides Function GetKeyForItem(ByVal item As CT2Field) As String
        Return item.DataSourceKey
    End Function

    'Overrides are mainly to help maintain the collection property on the CT2Field object
    Protected Overrides Sub InsertItem(ByVal index As Integer, ByVal newItem As CT2Field)

        If newItem.Collection IsNot Nothing Then
            Throw New ArgumentException("The item already belongs to a field collection.")
        End If

        MyBase.InsertItem(index, newItem)
        newItem.Collection = Me
    End Sub

    Protected Overrides Sub SetItem(ByVal index As Integer, ByVal newItem As CT2Field)

        Dim replaced As CT2Field = Items(index)

        If newItem.Collection IsNot Nothing Then
            Throw New ArgumentException("The item already belongs to a field collection.")
        End If

        MyBase.SetItem(index, newItem)
        newItem.Collection = Me
        replaced.Collection = Nothing
    End Sub

    Protected Overrides Sub RemoveItem(ByVal index As Integer)
        Dim removedItem As CT2Field = Items(index)
        removedItem.Collection = Nothing
        MyBase.RemoveItem(index)
    End Sub

    Protected Overrides Sub ClearItems()
        ''For Each fld As CT2Field In Items
        ''    fld.Collection = Nothing
        ''Next

        Do While Items.Count > 0
            Items.RemoveAt(0)
        Loop
        Me.IsDirty = False
        MyBase.ClearItems()
    End Sub



    Friend Sub CheckState()
        Dim NewDirt As Boolean = False
        Try
            For Each fld As CT2Field In Items
                If fld.IsDirty Then
                    NewDirt = True
                    Exit For
                End If
            Next

            Me.IsDirty = NewDirt

        Catch ex As Exception
        End Try

    End Sub

    Friend Sub ChangeKey(ByVal item As CT2Field, ByVal newKey As String)
        MyBase.ChangeItemKey(item, newKey)
    End Sub


    Public Sub AcceptAll()
        Try
            For Each fld As CT2Field In Items
                fld.AcceptChanges()
            Next
            Me.IsDirty = False
        Catch ex As Exception
        End Try

    End Sub


    Public Function HasErrors() As Boolean
        Dim ErrorFound As Boolean = False

        Try
            For Each fld As CT2Field In Items
                If fld.ErrorText <> String.Empty Then
                    ErrorFound = True
                    Exit For
                Else
                    Select Case fld.CurrentEdit
                        Case CT2Field.CurrentEditTypes.FieldText
                            If fld.FieldText.ErrorText <> String.Empty Then
                                ErrorFound = True
                                Exit For
                            End If
                        Case CT2Field.CurrentEditTypes.FieldDateTime
                            If fld.FieldDateTime.ErrorText <> String.Empty Then
                                ErrorFound = True
                                Exit For
                            End If
                        Case CT2Field.CurrentEditTypes.FieldCombo
                            If fld.FieldCombo.ErrorText <> String.Empty Then
                                ErrorFound = True
                                Exit For
                            End If
                        Case CT2Field.CurrentEditTypes.FieldCheck
                            If fld.FieldCheck.ErrorText <> String.Empty Then
                                ErrorFound = True
                                Exit For
                            End If
                    End Select
                End If
            Next

            Return ErrorFound

        Catch ex As Exception
            'there were problems so lets just turn this on....
            Return True
        End Try

    End Function

    Public Sub SetReadOnlyBackColor(BackColor As Drawing.Color)
        Try
            For Each fld As CT2Field In Items
                Select Case fld.CurrentEdit
                    Case CT2Field.CurrentEditTypes.FieldText
                        fld.FieldText.ReadOnlyBackColor = BackColor
                    Case CT2Field.CurrentEditTypes.FieldDateTime
                        fld.FieldDateTime.ReadOnlyBackColor = BackColor
                    Case CT2Field.CurrentEditTypes.FieldCombo
                        fld.FieldCombo.ReadOnlyBackColor = BackColor
                    Case CT2Field.CurrentEditTypes.FieldCheck
                        fld.FieldCheck.ReadOnlyBackColor = BackColor
                End Select
            Next
            Me.IsDirty = False
        Catch ex As Exception
        End Try

    End Sub

    Public Sub SetFieldError(ByVal Key As String, ByVal ErrorText As String)
        Try
            Item(Key).FieldText.ErrorText = ErrorText
        Catch ex As Exception
        End Try
    End Sub

    ''' <summary>
    ''' Set the label width for all items in the collection 
    ''' </summary>
    ''' <param name="Width">The width for the labels. The value of -1 will set all labels to the largest width found in the collection. The default is -1. </param>
    ''' <remarks></remarks>
    Public Sub SetLabelWidths(Optional ByVal Width As Integer = -1)
        Dim NewWidth As Integer = 0

        Try
            If Width = -1 Then
                Dim Max As Integer
                For Each fld In Items
                    If fld.pnlFieldLabel.Width > Max Then
                        Max = fld.pnlFieldLabel.Width
                    End If
                Next
                NewWidth = Max
            End If

            For Each fld In Items
                fld.LabelWidth = NewWidth + 5
                fld.LabelFormat = CT2Field.SectionFormat.FixedWidthNoWrap
            Next

        Catch ex As Exception
        End Try
    End Sub




    ''' <summary>
    ''' SetValues will update all the Fields in the collection and accept the changes without firing events
    ''' </summary>
    ''' <param name="CurRow">DataRow to use as a source for the fields in the collection.</param>
    ''' <remarks>It is assumed that the collection is no longer dirty after this method is executed.</remarks>
    Public Sub SetValues(ByVal CurRow As DataRow)

        For Each fld In Items
            Dim FieldName As String = fld.ProviderInfo.Field
            fld.SetValue(CurRow(FieldName))
        Next
        Me.IsDirty = False

    End Sub

    Public Sub SetValues(Of t)(ByVal obj As t)

        For Each fld In Items
            Try
                Dim FieldName As String = fld.ProviderInfo.Field
                Dim piInstance As Reflection.PropertyInfo = GetType(t).GetProperty(FieldName)
                fld.SetValue(piInstance.GetValue(obj))
            Catch ex As Exception

            End Try
        Next
        Me.IsDirty = False

    End Sub

    Public Sub GetValues(Of t)(ByVal obj As t)
        For Each fld In Items
            Try
                If fld.ProviderInfo.Field IsNot Nothing Then
                    Dim FieldName As String = fld.ProviderInfo.Field
                    Dim piInstance As Reflection.PropertyInfo = GetType(t).GetProperty(FieldName)
                    Dim NewValue = fld.GetValue()

                    If piInstance.PropertyType = GetType(System.Decimal) Or
                         piInstance.PropertyType = GetType(System.Int16) Or
                         piInstance.PropertyType = GetType(System.Int32) Or
                         piInstance.PropertyType = GetType(System.Int64) Then
                        If String.IsNullOrEmpty(NewValue) Then
                            NewValue = 0
                        End If
                    End If

                    piInstance.SetValue(obj, NewValue)
                End If

            Catch ex As Exception

            End Try
        Next
    End Sub

    Public Sub GetValues(ByRef CurRow As DataRow)

        For Each fld In Items
            Dim FieldName As String = fld.ProviderInfo.Field
            CurRow(FieldName) = fld.GetValue()
        Next

    End Sub

    Public Sub SuppressOn()
        For Each fld As CT2Field In Items
            Select Case fld.CurrentEdit
                Case CT2Field.CurrentEditTypes.FieldText
                    fld.FieldText.Suppress = True
                Case CT2Field.CurrentEditTypes.FieldDateTime
                    'fld.FieldDateTime.???
                Case CT2Field.CurrentEditTypes.FieldCombo
                    fld.FieldCombo.Suppress = True
                Case CT2Field.CurrentEditTypes.FieldCheck
                    fld.FieldCheck.Suppress = True
            End Select
        Next
    End Sub

    Public Sub SuppressOff()
        For Each fld As CT2Field In Items
            Select Case fld.CurrentEdit
                Case CT2Field.CurrentEditTypes.FieldText
                    fld.FieldText.Suppress = False
                Case CT2Field.CurrentEditTypes.FieldDateTime
                    'fld.FieldDateTime.???
                Case CT2Field.CurrentEditTypes.FieldCombo
                    fld.FieldCombo.Suppress = False
                Case CT2Field.CurrentEditTypes.FieldCheck
                    fld.FieldCheck.Suppress = False
            End Select
        Next
    End Sub


    Public Sub Initialize(Of t)(ByVal obj As t)

        For Each fld In Items
            Try
                If fld.ProviderInfo.Field IsNot Nothing Then

                    Dim FieldName As String = fld.ProviderInfo.Field
                    Dim piInstance As Reflection.PropertyInfo = GetType(t).GetProperty(FieldName)


                    Select Case piInstance.PropertyType
                        Case GetType(System.String)

                            Dim CA = piInstance.GetCustomAttributes(False)
                            If CA IsNot Nothing Then
                                Debug.Print(piInstance.Name.ToString & " CA ")
                            End If

                        Case GetType(System.DateTime)
                            If fld.EditType = CT2Field.EditTypes.UseDataType Then
                                fld.EditType = CT2Field.EditTypes.Date
                                fld.ShowPrompt = True
                                fld.Reload()
                            End If

                        Case GetType(System.Decimal), GetType(System.Int16), GetType(System.Int32), GetType(System.Int64)

                        Case GetType(System.Boolean)

                            If fld.EditType = CT2Field.EditTypes.UseDataType Then
                                fld.EditType = CT2Field.EditTypes.Check
                                fld.Reload()
                            End If

                            If fld.EditType = CT2Field.EditTypes.Check Then
                                fld.SetCheckData("", "", "", "")
                            End If


                        Case Else

                            If piInstance.PropertyType.BaseType = GetType(System.Enum) Then

                                If fld.EditType = CT2Field.EditTypes.UseDataType Then
                                    fld.EditType = CT2Field.EditTypes.Combo
                                    fld.Reload()
                                End If

                                If fld.EditType = CT2Field.EditTypes.Combo Then
                                    fld.ClearDataSource()
                                    Dim values As Integer() = piInstance.PropertyType.GetEnumValues
                                    For Each v In values
                                        Dim Name = piInstance.PropertyType.GetEnumName(v)
                                        fld.SetDataValues(v, Name)
                                    Next
                                End If

                            End If
                    End Select
                End If

            Catch ex As Exception
            End Try
        Next

    End Sub

#Region "Binding"

    Public Sub ClearBindings()
        For Each fld As CT2Field In Items
            Select Case fld.CurrentEdit
                Case CT2Field.CurrentEditTypes.FieldText
                    fld.FieldText.DataBindings.Clear()
                Case CT2Field.CurrentEditTypes.FieldDateTime
                    fld.FieldDateTime.DataBindings.Clear()
                Case CT2Field.CurrentEditTypes.FieldCombo
                    fld.FieldCombo.DataBindings.Clear()
                Case CT2Field.CurrentEditTypes.FieldCheck
                    fld.FieldCheck.DataBindings.Clear()
            End Select
        Next
    End Sub

    Public Sub BindDataRow(ByVal BindingRow As DataRow)

        BS = New BindingSource
        BS.DataSource = BindingRow.Table

        For Each fld In Items
            fld.SuppressOn()

            Dim FieldName As String = fld.ProviderInfo.Field

            If fld.EditType = CT2Field.EditTypes.UseDataType Then
                Select Case fld.ProviderInfo.Type
                    Case ProviderInfo.DB2ProviderDataTypes.Character,
                         ProviderInfo.DB2ProviderDataTypes.Numeric

                        Try
                            fld.FieldText.DataBindings.Clear()
                            fld.FieldText.DataBindings.Add(New Binding("Text", BS, FieldName))
                        Catch ex As Exception
                            Debug.Print(ex.Message)
                        End Try

                    Case ProviderInfo.DB2ProviderDataTypes.DB2Date,
                         ProviderInfo.DB2ProviderDataTypes.DB2Time,
                         ProviderInfo.DB2ProviderDataTypes.DB2Timestamp

                        Dim FieldDateTime As CT2DateTime = fld.FieldDateTime
                        FieldDateTime.DataBindings.Clear()
                        FieldDateTime.DataBindings.Add(New Binding("Value", BS, FieldName))
                End Select
            Else
                Select Case fld.EditType
                    Case CT2Field.EditTypes.Combo
                        Try
                            fld.FieldCombo.DataBindings.Clear()
                            fld.FieldCombo.DataBindings.Add(New Binding("Text", BS, FieldName))
                        Catch ex As Exception
                            Debug.Print(ex.Message)
                        End Try
                    Case CT2Field.EditTypes.Check
                        Try
                            fld.FieldCheck.DataBindings.Clear()
                            fld.FieldCheck.DataBindings.Add(New Binding("Value", BS, FieldName))
                        Catch ex As Exception
                            Debug.Print(ex.Message)
                        End Try


                End Select
            End If

            fld.SuppressOff()
        Next

        AcceptAll()

    End Sub


#End Region

End Class