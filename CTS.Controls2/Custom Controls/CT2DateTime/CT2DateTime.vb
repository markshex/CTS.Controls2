Imports System.Drawing
Imports System.Windows.Forms
Imports System.ComponentModel

Namespace CTS.Controls2

    Public Class CT2DateTime
        Implements INotifyPropertyChanged

#Region "CTSDateTime Control Properties"
        Private Initializing As Boolean = True
        Private Loading As Boolean = True
        Private RedrawingControl As Boolean

        Private DateBackSave As Color
        Private TimeBackSave As Color

        Private CustomDateErrorText As String = String.Empty
        Private CustomTimeErrorText As String = String.Empty

        Private FormatString As String
        Private d As Date
        Private t As Date
        Private secs As Short
        Private millisecs As Integer
        Private dwPopup As DateWindow

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
        Private Sub NotifyPropertyChanged(ByVal info As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(info))
        End Sub

        Private gValue As Date
        <System.ComponentModel.Category("_Custom Properties")>
        <System.ComponentModel.Description("Value")>
        <System.ComponentModel.Bindable(True)>
        Property Value() As Date
            Get
                Return gValue
            End Get
            Set(ByVal value As Date)

                'rebuild this value without the ticks (too precise) 
                value = New DateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second, value.Millisecond)

                If IsDate(value) Then

                    If value <> gValue Then
                        NotifyPropertyChanged("Value")
                    End If

                    gValue = value

                    If gValue = gOriginalValue Then
                        IsDirty = False
                    Else
                        IsDirty = True
                    End If

                    'Suppress individuals fields (dont fire unneeded events) 
                    month.Suppressed = True
                    day.Suppressed = True
                    year4.Suppressed = True
                    hour.Suppressed = True
                    minute.Suppressed = True

                    If gValue > Date.MinValue Then

                        If gValue.Date > Date.MinValue Then
                            month.Text = String.Format("{0:00}", gValue.Month)
                            day.Text = String.Format("{0:00}", gValue.Day)
                            year4.Text = String.Format("{0:0000}", gValue.Year)
                        End If

                        'If gValue > Date.MinValue Then
                        hour.Text = String.Format("{0:00}", gValue.Hour)
                        minute.Text = String.Format("{0:00}", gValue.Minute)
                        secs = gValue.Second
                        millisecs = gValue.Millisecond
                        'End If

                        d = value
                        t = CDate(hour.Text & ":" & minute.Text & ":" & String.Format("{0:00}", secs) & "." & millisecs)
                    Else
                        'uncommented the next 5 lines b/c setting value property to minvalue didnt do anything
                        'not sure why it was commented out 
                        month.Text = String.Empty
                        day.Text = String.Empty
                        year4.Text = String.Empty
                        hour.Text = String.Empty
                        minute.Text = String.Empty
                        secs = 0
                        millisecs = 0
                    End If

                    month.Suppressed = False
                    day.Suppressed = False
                    year4.Suppressed = False
                    day.Suppressed = False
                    year4.Suppressed = False
                End If

            End Set
        End Property

        Private gOriginalValue As Date
        <Browsable(False), EditorBrowsable(EditorBrowsableState.Always)>
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Public ReadOnly Property OriginalValue() As Date
            Get
                Return gOriginalValue
            End Get
        End Property

        Private gMaxValue As Date
        <System.ComponentModel.Category("_Custom Properties")>
        <System.ComponentModel.Description("The maximum date/time allowed")>
        Property MaxValue() As Date
            Get
                Return gMaxValue
            End Get
            Set(ByVal value As Date)
                gMaxValue = value
            End Set
        End Property

        Private gMinValue As Date
        <System.ComponentModel.Category("_Custom Properties")>
        <System.ComponentModel.Description("The minimum date/time allowed")>
        Property MinValue() As Date
            Get
                Return gMinValue
            End Get
            Set(ByVal value As Date)
                gMinValue = value
            End Set
        End Property

        Private gProviderFormat As ProviderType
        <System.ComponentModel.Category("_Custom Properties")>
        <System.ComponentModel.Description("DB2 Data Type")>
        <System.ComponentModel.DefaultValue(0)>
        Property ProviderFormat() As ProviderType
            Get
                Return gProviderFormat
            End Get
            Set(ByVal value As ProviderType)
                gProviderFormat = value
            End Set
        End Property

        Private gEditFormat As DisplayType
        <System.ComponentModel.Category("_Custom Properties")>
        <System.ComponentModel.Description("Format for Date, Time or Timestamp")>
        <System.ComponentModel.DefaultValue(0)>
        Property EditFormat() As DisplayType
            Get
                Return gEditFormat
            End Get
            Set(ByVal value As DisplayType)
                gEditFormat = value

                If ProviderFormat = ProviderType.DateOnly Then
                    gEditFormat = DisplayType.DateOnly
                End If
                If ProviderFormat = ProviderType.TimeOnly Then
                    gEditFormat = DisplayType.TimeOnly
                End If

                Select Case gEditFormat
                    Case DisplayType.DateOnly
                        FormatString = "MM/dd/yyyy"
                    Case DisplayType.TimeOnly
                        FormatString = "HH:mm"
                    Case DisplayType.Timestamp
                        FormatString = "MM/dd/yyyy HH:mm"
                    Case Else
                        FormatString = String.Empty
                End Select

                If DesignMode Then
                    RedrawControl()
                End If
            End Set
        End Property

        Private gAllowIncrement As Boolean = False
        <System.ComponentModel.Category("_Custom Properties")>
        <System.ComponentModel.Description("Allow individual date parts to be incremented with up/down arrow")>
        Property AllowIncrement() As Boolean
            Get
                Return gAllowIncrement
            End Get
            Set(ByVal value As Boolean)
                gAllowIncrement = value
            End Set
        End Property

        Private gErrorText As String = String.Empty
        <System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)>
        ReadOnly Property ErrorText() As String
            Get
                Return gErrorText
            End Get
        End Property

        Private gDateErrorText As String = String.Empty
        <System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)>
        ReadOnly Property DateErrorText() As String
            Get
                Return gDateErrorText
            End Get
        End Property

        Private gTimeErrorText As String = String.Empty
        <System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)>
        ReadOnly Property TimeErrorText() As String
            Get
                Return gTimeErrorText
            End Get
        End Property

        Private gErrorStyle As ErrorStyles = ErrorStyles.ChangeBackcolor
        <Browsable(True), EditorBrowsable(EditorBrowsableState.Always),
    DefaultValue(ErrorStyles.ChangeBackcolor),
    Category("_Custom Properties"), Description("How to handle the appearance when ErrorText exists.")>
        Public Property ErrorStyle() As ErrorStyles
            Get
                Return gErrorStyle
            End Get
            Set(ByVal value As ErrorStyles)
                If gErrorStyle <> value Then
                    gErrorStyle = value
                End If
            End Set
        End Property



        Private gShowDateCheckBox As Boolean
        <System.ComponentModel.Category("_Custom Properties")>
        <System.ComponentModel.Description("Show the checkbox to manually enable/disable date portion")>
        <System.ComponentModel.DefaultValue("False")>
        Property ShowDateCheckBox() As Boolean
            Get
                Return gShowDateCheckBox
            End Get
            Set(ByVal value As Boolean)
                gShowDateCheckBox = value
                If gShowDateCheckBox = False Then cbEnableDate.Checked = True
                If DesignMode Then
                    RedrawControl()
                End If
            End Set
        End Property

        Private gShowCalendarPrompt As Boolean
        <System.ComponentModel.Category("_Custom Properties")>
        <System.ComponentModel.Description("Show the calendar prompt button")>
        <System.ComponentModel.DefaultValue("True")>
        Property ShowCalendarPrompt() As Boolean
            Get
                Return gShowCalendarPrompt
            End Get
            Set(ByVal value As Boolean)
                gShowCalendarPrompt = value
                If DesignMode Then
                    RedrawControl()
                End If
            End Set
        End Property

        Private gReadOnly As Boolean
        <System.ComponentModel.Category("_Custom Properties")>
        <System.ComponentModel.DefaultValue("False")>
        <System.ComponentModel.Description("Display the control as 'ReadOnly'")>
        Public Property [ReadOnly]() As Boolean
            Get
                Return gReadOnly
            End Get
            Set(ByVal value As Boolean)

                'If gReadOnly <> value Then
                gReadOnly = value
                '' ''Try
                '' ''    If Me.BackColor <> Color.Transparent Then
                '' ''        gReadOnlyBackColor = Me.BackColor
                '' ''    Else
                '' ''        If Me.Parent.BackColor = Color.Transparent Then
                '' ''            gReadOnlyBackColor = Me.ParentForm.BackColor
                '' ''        Else
                '' ''            gReadOnlyBackColor = Me.Parent.BackColor
                '' ''        End If
                '' ''    End If
                '' ''Catch
                '' ''    gReadOnlyBackColor = EditProtect
                '' ''End Try
                'End If

                If DesignMode Then
                    RedrawControl()
                Else
                    InitializeControlColor()
                    Me.Refresh()
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
                    RaiseEvent StateChanged(Me)
                End If
            End Set

        End Property

        Private gTransparentBack As Color
        Private gReadOnlyBackColor As Color = EditProtect
        <Browsable(True), EditorBrowsable(EditorBrowsableState.Always),
     Category("_Custom Properties"), Description("Background color when textbox is readonly")>
        Public Property ReadOnlyBackColor() As Color
            Get
                Return gReadOnlyBackColor
            End Get
            Set(ByVal value As Color)
                If value <> gReadOnlyBackColor Then
                    Select Case value
                        Case Color.Empty
                            gReadOnlyBackColor = EditProtect
                        Case Else
                            gReadOnlyBackColor = value
                    End Select
                    InitializeControlColor()
                End If
            End Set
        End Property

        Private gInputBackColor As Color = EditBack
        Private gErrorBackColor As Color = EditErrorBack
        Private gFocusBackColor As Color = EditFocusBack


        Public Enum ProviderType
            Timestamp = 0
            DateOnly = 1
            TimeOnly = 2
        End Enum

        Public Enum DisplayType
            Timestamp = 0
            DateOnly = 1
            TimeOnly = 2
        End Enum

        Enum DateTimePart
            DatePart
            TimePart
        End Enum

        Public Sub New()
            InitializeComponent()

            Initializing = False
        End Sub

        Protected Friend Class DatePartsTB
            Inherits TextBox

            Friend Suppressed As Boolean
            Public gInt As Integer
            Public gMin As Integer = 1
            Public gMax As Integer = 30
            Public gDefault As Integer = 1
            Public gCurBack As Color
            Public gErrorText As String
            Public AllowIncrement As Boolean
            Private Incremented As Boolean

            Protected Overrides Sub OnKeyDown(ByVal e As System.Windows.Forms.KeyEventArgs)
                Dim val As Integer
                Dim strValue As String
                Suppressed = False

                'Increment on up arrow key
                If e.KeyData = Keys.Up Then
                    If AllowIncrement Then
                        If IsNumeric(Me.Text) Then
                            val = Me.Text

                            If val < gMax And val >= gMin Then
                                val += 1
                            Else
                                val = gMin
                            End If
                        Else
                            val = gDefault
                        End If

                        strValue = val.ToString("00")

                        Incremented = True
                        Me.Text = strValue
                        Me.SelectAll()
                    Else
                        SendKeys.Send("+({TAB})")
                    End If
                    e.Handled = True
                End If

                'Decrement on down rrow key
                If e.KeyData = Keys.Down Then
                    If AllowIncrement Then
                        If IsNumeric(Me.Text) Then
                            val = Me.Text

                            If val > gMin And val <= gMax Then
                                val -= 1
                            Else
                                val = gMax
                            End If
                        Else
                            val = gDefault
                        End If

                        strValue = val.ToString("00")

                        Incremented = True
                        Me.Text = strValue
                        Me.SelectAll()
                    Else
                        SendKeys.Send("({TAB})")
                    End If
                    e.Handled = True
                End If

                'Move right, then send tab if all the way...
                If e.KeyData = Keys.Right Then
                    'If Me.SelectionStart >= Me.TextLength Then
                    'Debug.Print("DATEPARTS-RIGHT" & Me.Name)
                    'SendKeys.Send("{TAB}")
                    Suppressed = True


                    If Me.Name = "year4" Or Me.Name = "minute" Then
                        SendKeys.Send("{TAB}")
                        Suppressed = True
                    Else
                        Dim p As Control = Me.Parent
                        Dim b As Boolean = p.SelectNextControl(Me, True, False, True, False)
                    End If


                    e.Handled = True
                    'End If
                End If

                'Move left, then send tab if all the way...
                If e.KeyData = Keys.Left Then
                    'If Me.SelectionStart = 0 And Me.SelectionLength = 0 Then
                    'SendKeys.Send("+({TAB})")
                    Suppressed = True
                    Select Case Me.Name
                        Case "month"
                            SendKeys.Send("+({TAB})")
                            Suppressed = True
                        Case "hour"
                            'SendKeys.Send("+({TAB}){RIGHT}{RIGHT}")
                            'Suppressed = True
                            Me.Parent.Parent.SelectNextControl(Me, False, False, True, False)
                        Case Else

                            Me.Parent.SelectNextControl(Me, False, False, True, False)
                    End Select

                    e.Handled = True
                    'End If
                End If
                MyBase.OnKeyDown(e)
            End Sub

            Protected Overrides Sub OnEnter(e As EventArgs)

                If Not Me.ReadOnly Then
                    Me.gCurBack = Me.BackColor

                    If Me.Text = String.Empty Then
                        'Me.BackColor = gInputBackColor
                    Else
                        Me.SelectAll()
                    End If
                End If

                MyBase.OnEnter(e)
            End Sub

            Protected Overrides Sub OnLeave(e As EventArgs)
                If Not Me.ReadOnly Then

                    Me.Text = Trim(Me.Text)

                    If IsNumeric(Me.Text) Then
                        gInt = CInt(Me.Text)
                    Else
                        gInt = 0
                    End If

                End If

                MyBase.OnLeave(e)
            End Sub

            Protected Overrides Sub OnTextChanged(e As EventArgs)
                If Not Suppressed Then
                    If Me.TextLength = Me.MaxLength And Not Incremented Then
                        Debug.Print("CTSDateTime TEXTCHANGED " & Me.Name)
                        Select Case Me.Name
                            Case "year4", "minute"
                                SendKeys.Send("{TAB}")
                            Case Else
                                Me.Parent.SelectNextControl(Me, True, False, True, False)
                        End Select
                    End If
                    MyBase.OnTextChanged(e)
                End If
                Incremented = False
            End Sub

            Protected Overrides Sub OnClick(e As EventArgs)
                Me.SelectAll()
                MyBase.OnClick(e)
            End Sub
        End Class

        Protected Friend Class DateWindow
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

#End Region

#Region "Loading and Redrawing"

        Private Sub CTSDateTime_Load(sender As Object, e As EventArgs) Handles Me.Load
            Loading = True

            month.BorderStyle = Windows.Forms.BorderStyle.None
            day.BorderStyle = Windows.Forms.BorderStyle.None
            year4.BorderStyle = Windows.Forms.BorderStyle.None
            hour.BorderStyle = Windows.Forms.BorderStyle.None
            minute.BorderStyle = Windows.Forms.BorderStyle.None

            month.MaxLength = 2
            month.gMin = 1
            month.gMax = 12
            month.gDefault = Now.Month

            day.MaxLength = 2
            day.gMin = 1
            day.gMax = 31
            day.gDefault = Now.Day

            year4.MaxLength = 4
            year4.gMin = 1900
            year4.gMax = 2099
            year4.gDefault = Now.Year

            hour.MaxLength = 2
            hour.gMin = 0
            hour.gMax = 23
            hour.gDefault = Now.Hour

            minute.MaxLength = 2
            minute.gMin = 0
            minute.gMax = 59
            minute.gDefault = Now.Minute

            month.AllowIncrement = AllowIncrement
            day.AllowIncrement = AllowIncrement
            year4.AllowIncrement = AllowIncrement
            hour.AllowIncrement = AllowIncrement
            minute.AllowIncrement = AllowIncrement

            Select Case gEditFormat
                Case DisplayType.DateOnly
                    FormatString = "MM/dd/yyyy"
                Case DisplayType.TimeOnly
                    FormatString = "HH:mm"
                Case DisplayType.Timestamp
                    FormatString = "MM/dd/yyyy HH:mm"
                Case Else
                    FormatString = String.Empty
            End Select

            RedrawControl()
            Loading = False
        End Sub

        Private Sub RedrawControl()
            RedrawingControl = True

            Dim DatePanelWidth As Integer = 3
            Dim TimePanelWidth As Integer = 3
            Dim TabIdx As Integer = 0
            Dim t As Integer = 2

            pnlDate.Visible = False
            pnlFiller.Visible = False
            pnlTime.Visible = False

            month.Top = t
            day.Top = t
            datesep1.Top = t
            year4.Top = t
            datesep2.Top = t
            minute.Top = t
            timesep.Top = t
            hour.Top = t

            Dim g As Graphics = Me.CreateGraphics
            Dim w2 As Integer = g.MeasureString("55", Me.Font).Width - 3
            Dim w4 As Integer = g.MeasureString("5555", Me.Font).Width - 3
            g.Dispose()


            month.Width = w2
            day.Width = w2
            year4.Width = w4
            hour.Width = w2
            minute.Width = w2

            month.ReadOnly = gReadOnly
            day.ReadOnly = gReadOnly
            year4.ReadOnly = gReadOnly
            hour.ReadOnly = gReadOnly
            minute.ReadOnly = gReadOnly

            If gReadOnly Then
                month.TabStop = False
                hour.TabStop = False
                btnCalendar.Enabled = False
                cbEnableDate.Enabled = False
            Else
                If gShowDateCheckBox Then
                    If cbEnableDate.Checked Then
                        month.TabStop = True
                    Else
                        month.TabStop = False
                    End If
                Else
                    month.TabStop = True
                End If

                hour.TabStop = True
                btnCalendar.Enabled = True
                cbEnableDate.Enabled = True
            End If

            InitializeControlColor()


            If gEditFormat = DisplayType.Timestamp Or gEditFormat = DisplayType.DateOnly Then

                If gShowDateCheckBox Then
                    cbEnableDate.TabIndex = TabIdx
                    TabIdx += 1
                    cbEnableDate.Top = t
                    cbEnableDate.Left = DatePanelWidth
                    DatePanelWidth += cbEnableDate.Width
                    cbEnableDate.CheckState = CheckState.Checked
                    cbEnableDate.Visible = True
                Else
                    cbEnableDate.Visible = False
                End If

                month.TabIndex = TabIdx
                TabIdx += 1
                month.Left = DatePanelWidth
                DatePanelWidth += month.Width

                datesep1.Left = DatePanelWidth - 3
                DatePanelWidth += datesep1.Width - 3

                day.TabIndex = TabIdx
                TabIdx += 1
                day.Left = DatePanelWidth - 3
                DatePanelWidth += day.Width - 3

                datesep2.Left = DatePanelWidth - 3
                DatePanelWidth += datesep2.Width - 3

                year4.TabIndex = TabIdx
                TabIdx += 1
                year4.Left = DatePanelWidth - 3
                DatePanelWidth += year4.Width - 3

                If gShowCalendarPrompt Then
                    btnCalendar.Visible = True
                    DatePanelWidth += btnCalendar.Width + 8
                Else
                    btnCalendar.Visible = False
                    DatePanelWidth += 6
                End If

                pnlDate.Width = DatePanelWidth
                pnlDate.Visible = True
            End If

            If gEditFormat = DisplayType.Timestamp Then
                pnlFiller.Visible = True
            End If

            If gEditFormat = DisplayType.Timestamp Or gEditFormat = DisplayType.TimeOnly Then
                hour.TabIndex = TabIdx
                TabIdx += 1
                hour.Left = TimePanelWidth
                TimePanelWidth += hour.Width

                timesep.Left = TimePanelWidth - 3
                TimePanelWidth += timesep.Width - 3

                minute.TabIndex = TabIdx
                TabIdx += 1
                minute.Left = TimePanelWidth - 4
                TimePanelWidth += minute.Width - 4

                TimePanelWidth += 6
                pnlTime.Width = TimePanelWidth
                pnlTime.Visible = True
            End If

            pnlTime.BringToFront()
            RedrawingControl = False
            UpdateControlSize()
        End Sub

        Public Sub Reload()
            CTSDateTime_Load(Me, New EventArgs)
        End Sub

        Private Sub PanelsResized(sender As Object, e As EventArgs) Handles pnlTime.Resize, pnlDate.Resize, pnlFiller.Resize
            If Not Initializing And Not Loading And Not RedrawingControl Then
                UpdateControlSize()
                Debug.Print("CTSDateTime PanelsResize-" & Me.Name & " " & sender.name)
            End If
        End Sub


        'Private Sub PanelsVisibleChanged(sender As Object, e As EventArgs) Handles pnlDate.VisibleChanged, pnlTime.VisibleChanged, pnlFiller.VisibleChanged
        '    If Not Initializing And Not Loading And Not RedrawingControl Then
        '        UpdateControlSize()
        '        Debug.Print("panel visible change- " & Me.Name & " " & sender.name)
        '    End If
        'End Sub

        Private Sub CTSDateTime_FontChanged(sender As Object, e As EventArgs) Handles MyBase.FontChanged
            If DesignMode Then
                RedrawControl()
            End If
        End Sub

        Private Sub UpdateControlSize()
            Me.Height = month.Top + month.Height + 7
            Dim w As Integer = 0
            If pnlDate.Visible Then w += pnlDate.Width
            If pnlTime.Visible Then w += pnlTime.Width
            If pnlDate.Visible And pnlTime.Visible Then w += pnlFiller.Width
            Me.Width = w
        End Sub

        'Sets the control back/fore color based on readonly & error text/style 
        'uses readonlybackcolor property, inputbackcolor property or errorbackcolor property 
        'OR the derived transparent color
        Private Sub InitializeControlColor()

            If gReadOnly And gReadOnlyBackColor = Color.Transparent Then
                If gTransparentBack = Color.Empty Then SetTransparentBackColor()
                DateBackSave = gTransparentBack
                TimeBackSave = gTransparentBack

            ElseIf gReadOnly And gReadOnlyBackColor <> Color.Transparent Then
                DateBackSave = gReadOnlyBackColor
                TimeBackSave = gReadOnlyBackColor

            Else

                If ShowDateCheckBox AndAlso Not cbEnableDate.Checked Then
                    DateBackSave = gReadOnlyBackColor
                Else
                    DateBackSave = gInputBackColor
                End If

                TimeBackSave = gInputBackColor
            End If

            If ErrorStyle = ErrorStyles.ChangeBackcolor _
            AndAlso (DateErrorText <> String.Empty Or CustomDateErrorText <> String.Empty) Then
                DateBackSave = gErrorBackColor
            End If

            If ErrorStyle = ErrorStyles.ChangeBackcolor _
            AndAlso (TimeErrorText <> String.Empty Or CustomTimeErrorText <> String.Empty) Then
                TimeBackSave = gErrorBackColor
            End If

            pnlDate.BackColor = DateBackSave
            month.BackColor = DateBackSave
            day.BackColor = DateBackSave
            year4.BackColor = DateBackSave

            pnlTime.BackColor = TimeBackSave
            hour.BackColor = TimeBackSave
            minute.BackColor = TimeBackSave

        End Sub

        Private Sub SetTransparentBackColor()
            Dim NewBackColor As Color = EditProtect
            GetTransparentBack(Me.Parent, NewBackColor)
            gTransparentBack = NewBackColor
        End Sub

        Private Sub GetTransparentBack(ByVal ctrl As Control, ByRef clr As Color)
            Try
                If clr = Color.Empty Then
                    If ctrl.BackColor <> Color.Transparent Then
                        clr = ctrl.BackColor
                    Else
                        If ctrl.Parent Is Nothing Then
                            clr = EditProtect
                        Else
                            GetTransparentBack(ctrl.Parent, clr)
                        End If

                    End If
                End If
            Catch
                clr = EditProtect
            End Try
        End Sub

        Protected Overrides Sub OnParentChanged(e As EventArgs)
            'If Not Suppress Then
            ''If gReadOnlyBackColor = Color.Transparent Then
            SetTransparentBackColor()
            ''End If
            'End If
            MyBase.OnParentChanged(e)
        End Sub

#End Region

#Region "Validating & Error Handling"

        Public Event DateTimeValidatingEnd(ByVal DatePart As DateTimePart, ByVal ErrorText As String, ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs)

        Private Sub CTSDateTime_Validating(sender As Object, e As CancelEventArgs) Handles MyBase.Validating
            Me.ValidateChildren()
        End Sub

        Private Sub pnlDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles pnlDate.Validating
            Dim s As String = String.Empty
            Dim EmptyDate As Boolean = False
            Dim ValidLevel1 As Boolean = False
            Dim ValidLevel2 As Boolean = False
            gDateErrorText = String.Empty

            CustomDateErrorText = String.Empty

            If year4.Text = String.Empty Then
                s = String.Format("{0}/{1}", Trim(month.Text), Trim(day.Text))
            Else
                s = String.Format("{0}/{1}/{2}", Trim(month.Text), Trim(day.Text), Trim(year4.Text))
            End If

            'Date portion has been cleared
            If s = "/" OrElse s = "//" Or s = "01/01/0001" Then
                EmptyDate = True
            End If

            'If Date or timestamp format validate the date 
            If Me.gEditFormat = DisplayType.DateOnly Or Me.gEditFormat = DisplayType.Timestamp Then

                'Level 1: Is it a valid date?
                If IsDate(s) Or EmptyDate Then
                    ValidLevel1 = True
                Else
                    ValidLevel1 = False
                End If

                'Level 2: Does the date meet the min & max contraints?
                ValidLevel2 = True
                'If ValidLevel1 And Not EmptyDate Then
                'Dim temp As Date = CDate(s)
                'If gMaxValue = Date.MinValue Then
                ' gMaxValue = Date.MaxValue
                'End If
                'If temp >= gMinValue.Date And temp <= gMaxValue.Date Then
                ' ValidLevel2 = True
                'Else
                ' ValidLevel2 = False
                'End If
                'End If


                'Construct error message for Level 1 error
                If Not ValidLevel1 Then
                    ttDate.ToolTipTitle = "Date Error"
                    If month.gInt < month.gMin Or month.gInt > month.gMax Then
                        gDateErrorText = "Invalid Month"
                    End If
                    If day.gInt < day.gMin Or day.gInt > day.gMax Then
                        gDateErrorText = gDateErrorText & vbNewLine & "Invalid Day"
                    End If
                    If year4.gInt < year4.gMin Or year4.gInt > year4.gMax Then
                        gDateErrorText = gDateErrorText & vbNewLine & "Invalid Year"
                    End If

                Else
                    'Construct error message for Level 1 error
                    If Not ValidLevel2 Then
                        Dim temp As Date = CDate(s)
                        If temp < gMinValue.Date Then
                            gDateErrorText = "Date can not be less than " & Format(gMinValue, "MM/dd/yyyy")
                        End If
                        If temp > gMaxValue.Date Then
                            gDateErrorText = "Date can not be greater than " & Format(gMaxValue, "MM/dd/yyyy")
                        End If


                    End If
                End If


                'If no errors exist... 
                If gDateErrorText = String.Empty Then

                    If EmptyDate Then
                        d = Date.MinValue

                    Else
                        d = CDate(s)
                        hour.Suppressed = True
                        minute.Suppressed = True
                        Me.Value = d.Date + t.TimeOfDay
                        hour.Suppressed = False
                        minute.Suppressed = False
                    End If

                    pnlDate.BackColor = DateBackSave
                    ApplyToolTipToChildControls(Me.pnlDate, ttDate, String.Empty)
                    'e.Cancel = False
                Else
                    d = Date.MinValue
                    If ErrorStyle = ErrorStyles.ChangeBackcolor Then
                        pnlDate.BackColor = gErrorBackColor
                    Else
                        pnlDate.BackColor = DateBackSave
                    End If

                    ApplyToolTipToChildControls(Me.pnlDate, ttDate, gDateErrorText)
                    'e.Cancel = True
                End If

            End If

            month.BackColor = pnlDate.BackColor
            day.BackColor = pnlDate.BackColor
            year4.BackColor = pnlDate.BackColor

            Refresh()

            BuildErrorTextString()

            RaiseEvent DateTimeValidatingEnd(DateTimePart.DatePart, gDateErrorText, Me, e)
            Debug.Print("CTSDateTime-Date " & Me.Name & " Date=" & d.ToString & "  Time=" & t.TimeOfDay.ToString)
        End Sub

        Private Sub pnlTime_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles pnlTime.Validating
            Dim s As String = String.Empty
            Dim EmptyTime As Boolean = False
            Dim ValidLevel1 As Boolean = False
            Dim ValidLevel2 As Boolean = False
            gTimeErrorText = String.Empty

            CustomTimeErrorText = String.Empty

            If hour.Text = String.Empty AndAlso minute.Text <> String.Empty Then
                hour.Text = "00"
            End If

            If minute.Text = String.Empty AndAlso hour.Text <> String.Empty Then
                minute.Text = "00"
            End If

            '!s = String.Format("{0}:{1}", Trim(hour.Text), Trim(minute.Text))
            s = String.Format("{0}:{1}:{2}.{3}", Trim(hour.Text), Trim(minute.Text), String.Format("{0:00}", secs), millisecs)

            If Trim(hour.Text) = String.Empty And Trim(minute.Text) = String.Empty Then
                EmptyTime = True
                secs = 0
                millisecs = 0
            End If

            If Me.gEditFormat = DisplayType.TimeOnly Then
                d = Date.MinValue
            End If

            If Me.gEditFormat = DisplayType.Timestamp Or Me.gEditFormat = DisplayType.TimeOnly Then

                'Level 1: Is it a valid time?
                If IsDate(s) Or EmptyTime Then
                    ValidLevel1 = True
                Else
                    ValidLevel1 = False
                End If

                'Construct Level 1 error text 
                If Not ValidLevel1 Then
                    gTimeErrorText = "Time format is incorrect"

                    If hour.gInt < hour.gMin Or hour.gInt > hour.gMax Then
                        gTimeErrorText = "Invalid Hour"
                    End If
                    If minute.gInt < minute.gMin Or minute.gInt > minute.gMax Then
                        gTimeErrorText = gTimeErrorText & vbNewLine & "Invalid Minute"
                    End If
                End If

                'If no errors exist...
                If gTimeErrorText = String.Empty Then
                    If EmptyTime Then
                        t = Date.MinValue
                    Else
                        t = CDate(s)
                        Me.Value = d.Date + t.TimeOfDay
                    End If

                    gTimeErrorText = String.Empty
                    pnlTime.BackColor = TimeBackSave
                    ApplyToolTipToChildControls(Me.pnlTime, ttTime, String.Empty)
                    'e.Cancel = False
                Else
                    t = Date.MinValue
                    If ErrorStyle = ErrorStyles.ChangeBackcolor Then
                        pnlTime.BackColor = gErrorBackColor
                    Else
                        pnlTime.BackColor = TimeBackSave
                    End If

                    ApplyToolTipToChildControls(Me.pnlTime, ttTime, gTimeErrorText)
                    'e.Cancel = True
                End If
            End If

            hour.BackColor = pnlTime.BackColor
            minute.BackColor = pnlTime.BackColor

            Refresh()

            BuildErrorTextString()

            RaiseEvent DateTimeValidatingEnd(DateTimePart.TimePart, gTimeErrorText, Me, e)
            Debug.Print("CTSDateTime-Time " & Me.Name & " Date=" & d.ToString & "  Time=" & t.TimeOfDay.ToString)
        End Sub

        Private Sub BuildErrorTextString()
            Dim SaveErrorText As String = gErrorText

            gErrorText = String.Empty

            If gDateErrorText <> String.Empty Then
                gErrorText = IIf(gErrorText = String.Empty, gDateErrorText, gErrorText & "; " & gDateErrorText)
            End If

            If CustomDateErrorText <> String.Empty Then
                gErrorText = IIf(gErrorText = String.Empty, CustomDateErrorText, gErrorText & "; " & CustomDateErrorText)
            End If

            If gTimeErrorText <> String.Empty Then
                gErrorText = IIf(gErrorText = String.Empty, gTimeErrorText, gErrorText & "; " & gTimeErrorText)
            End If

            If CustomTimeErrorText <> String.Empty Then
                gErrorText = IIf(gErrorText = String.Empty, CustomTimeErrorText, gErrorText & "; " & CustomTimeErrorText)
            End If

            If gErrorText <> SaveErrorText Then
                RaiseEvent ErrorTextChanged(Me)
            End If

        End Sub

        Private Sub CTSDateTime_Validated(sender As Object, e As EventArgs) Handles Me.Validated
            Debug.Print("CTSDateTime " & Me.Name & ": VALIDATED")
        End Sub

        Public Sub SetCustomError(ByVal DatePart As DateTimePart, ByVal ErrorText As String)
            Dim BC As Color

            Select Case DatePart
                Case DateTimePart.DatePart
                    CustomDateErrorText = ErrorText
                    If ErrorText <> String.Empty And ErrorStyle = ErrorStyles.ChangeBackcolor Then
                        BC = gErrorBackColor
                    Else
                        BC = DateBackSave
                    End If
                    pnlDate.BackColor = BC
                    month.BackColor = BC
                    day.BackColor = BC
                    year4.BackColor = BC
                    ApplyToolTipToChildControls(Me.pnlDate, ttDate, ErrorText)

                Case DateTimePart.TimePart
                    CustomTimeErrorText = ErrorText
                    If ErrorText <> String.Empty And ErrorStyle = ErrorStyles.ChangeBackcolor Then
                        BC = gErrorBackColor
                    Else
                        BC = TimeBackSave
                    End If
                    pnlTime.BackColor = BC
                    hour.BackColor = BC
                    minute.BackColor = BC
                    ApplyToolTipToChildControls(Me.pnlTime, ttTime, ErrorText)
            End Select

            BuildErrorTextString()
        End Sub

        Shadows Event TextChanged(ByVal sender As System.Object, ByVal e As EventArgs)
        Private Sub CTSDateTime_TextChanged(sender As Object, e As EventArgs) Handles month.TextChanged, day.TextChanged, year4.TextChanged, hour.TextChanged, minute.TextChanged
            RaiseEvent TextChanged(sender, e)
        End Sub

        Public Event StateChanged(ByVal sender As Object)
        Public Event ErrorTextChanged(ByVal sender As Object)

        Public Sub AcceptChanges()
            gOriginalValue = Me.Value
            IsDirty = False
            ClearErrors()
            InitializeControlColor()
        End Sub

#End Region

#Region "Calendar Prompt & Checkbox Enabler"

        Private Sub btnCalendar_Click(sender As Object, e As EventArgs) Handles btnCalendar.Click
            pnlDate.Focus()
            dwPopup = New DateWindow(mcDate)
            AddHandler dwPopup.Closing, AddressOf CalendarClosed

            Dim p As Point = Me.PointToScreen(Point.Empty)
            p.Y += Me.Height
            dwPopup.Show(p)
            mcDate.Focus()
        End Sub

        Private Sub CalendarClosed(sender As Object, e As ToolStripDropDownClosingEventArgs)

            Debug.Print("closing")
            dwPopup.Close()
        End Sub

        Private Sub mcDate_DateSelected(sender As Object, e As DateRangeEventArgs) Handles mcDate.DateSelected
            Dim d As Date = e.Start
            month.Text = d.Month.ToString("00")
            day.Text = d.Day.ToString("00")
            year4.Text = d.Year.ToString
            dwPopup.Close()
        End Sub

        Private Sub cbEnableDate_CheckedChanged(sender As Object, e As EventArgs) Handles cbEnableDate.CheckedChanged

            month.ReadOnly = Not cbEnableDate.Checked
            day.ReadOnly = Not cbEnableDate.Checked
            year4.ReadOnly = Not cbEnableDate.Checked

            btnCalendar.Enabled = cbEnableDate.Checked

            If cbEnableDate.Checked Then
                month.TabStop = True
                month.Focus()
                datesep1.ForeColor = month.ForeColor
                datesep2.ForeColor = month.ForeColor
            Else
                month.TabStop = False
                datesep1.ForeColor = Color.Gray
                datesep2.ForeColor = Color.Gray
            End If

            InitializeControlColor()

            If Not DesignMode AndAlso Not Loading AndAlso cbEnableDate.Checked Then
                pnlDate_Enter(pnlDate, New EventArgs)
            End If

        End Sub

        Public Sub SetCheckBox(ByVal Checked As Boolean)
            If ShowDateCheckBox Then
                cbEnableDate.Checked = Checked
            End If
        End Sub

#End Region

#Region "Context Menu & Menu Items"

        Private Sub cmsRightClick_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles cmsRightClick.Opening

            If gReadOnly Then
                miClear.Enabled = False
                miSetCurrent.Enabled = False
                miResetCurrent.Enabled = False
                miResetPrevious.Enabled = False
                miViewCalendar.Enabled = False
            Else
                miClear.Enabled = True
                miSetCurrent.Enabled = True
                miResetCurrent.Enabled = True
                miResetPrevious.Enabled = True
                miViewCalendar.Enabled = True
            End If

            If gValue = Date.MinValue Then
                miResetCurrent.Text = "Reset Current (No Date)"
            Else
                miResetCurrent.Text = "Reset Current (" & gValue.ToString(FormatString) & ")"
            End If

            If Not IsDirty Then
                miResetPrevious.Visible = False
            Else
                If gOriginalValue = Date.MinValue Then
                    miResetPrevious.Text = "Reset Previous (No Date)"
                Else
                    miResetPrevious.Text = "Reset Previous (" & gOriginalValue.ToString(FormatString) & ")"
                End If
                miResetPrevious.Visible = True
            End If


            If gReadOnly Then
                miPaste.Enabled = False
            Else
                Try
                    If Clipboard.ContainsText AndAlso IsDate(Clipboard.GetText) Then
                        Dim ClipDate As Date = CDate(Clipboard.GetText)
                        miPaste.Enabled = True
                        miPaste.Text = "Paste " & ClipDate.ToString(FormatString)
                    Else
                        miPaste.Enabled = False
                    End If
                Catch
                End Try
            End If

            If Me.gEditFormat = DisplayType.TimeOnly Then
                miViewCalendar.Visible = False
            Else
                miViewCalendar.Visible = True
            End If

        End Sub

        Private Sub miResetPrevious_Click(sender As Object, e As EventArgs) Handles miResetPrevious.Click
            If gOriginalValue = Date.MinValue Then
                miClear_Click(Me, New EventArgs)
            Else
                Value = gOriginalValue
                ClearErrors()
            End If
        End Sub

        Private Sub miSetCurrent_Click(sender As Object, e As EventArgs) Handles miSetCurrent.Click
            'Value = Now
            Value = New DateTime(Now.Year, Now.Month, Now.Day, Now.Hour, Now.Minute, Now.Second, Now.Millisecond)
            ClearErrors()
        End Sub

        Private Sub miResetCurrent_Click(sender As Object, e As EventArgs) Handles miResetCurrent.Click
            If gValue = Date.MinValue Then
                miClear_Click(Me, New EventArgs)
            Else
                Value = Value
                ClearErrors()
            End If
        End Sub

        Private Sub miPaste_Click(sender As Object, e As EventArgs) Handles miPaste.Click
            If Clipboard.ContainsText AndAlso IsDate(Clipboard.GetText) Then
                Dim ClipDate As Date = CDate(Clipboard.GetText)
                Value = ClipDate
                ClearErrors()
            End If
        End Sub

        Private Sub miClear_Click(sender As Object, e As EventArgs) Handles miClear.Click
            Value = Date.MinValue
            ClearErrors()
            If month.Focused OrElse day.Focused OrElse year4.Focused Then
                pnlDate_Enter(pnlDate, New EventArgs)
            End If
            If hour.Focused OrElse minute.Focused Then
                pnlTime_Enter(pnlTime, New EventArgs)
            End If
        End Sub

        Private Sub pnlFiller_DoubleClick(sender As Object, e As EventArgs) Handles pnlFiller.DoubleClick
            Debug.Print(Value.ToString)
        End Sub

        Private Sub miCopy_Click(sender As Object, e As EventArgs) Handles miCopy.Click
            If gValue <> Date.MinValue Then
                Clipboard.SetText(gValue.ToString("MM/dd/yyyy HH:mm"))
            End If
        End Sub

        Private Sub miViewCalendar_Click(sender As Object, e As EventArgs) Handles miViewCalendar.Click
            btnCalendar_Click(Me, New EventArgs)
        End Sub

        Private Sub ClearErrors()
            CustomDateErrorText = String.Empty
            gDateErrorText = String.Empty

            CustomTimeErrorText = String.Empty
            gTimeErrorText = String.Empty

            InitializeControlColor()

            ApplyToolTipToChildControls(Me.pnlDate, ttDate, String.Empty)
            ApplyToolTipToChildControls(Me.pnlTime, ttTime, String.Empty)

            BuildErrorTextString()
        End Sub
#End Region

#Region "General Routines"

        Private Sub ApplyToolTipToChildControls(ByVal parent As Control, ByVal tt As ToolTip, ByVal message As String)
            For Each Control In parent.Controls
                tt.SetToolTip(Control, message)
                If parent.HasChildren Then
                    ApplyToolTipToChildControls(Control, tt, message)
                End If
            Next
        End Sub

        Public Sub FocusDate()
            If pnlDate.Enabled Then
                Me.Select()
                Me.month.Select()
            End If
        End Sub

        Public Sub FocusTime()
            If pnlTime.Enabled Then
                Me.Select()
                Me.hour.Select()
            End If
        End Sub


        Private Sub CTSDateTime_Enter(sender As Object, e As EventArgs) Handles Me.Enter
            Debug.Print("CTSDateTime " & Me.Name & ": ENTER")
            'Me.Refresh()
        End Sub

        Private Sub CTSDateTime_Leave(sender As Object, e As EventArgs) Handles Me.Leave
            Debug.Print("CTSDateTime " & Me.Name & ": LEAVE")
            'Me.Refresh()
        End Sub


        Private Sub month_Leave(sender As Object, e As EventArgs) Handles month.Leave
            month.Suppressed = True
            If IsNumeric(month.Text) Then
                Dim intMonth As Integer = CInt(month.Text)
                Select Case intMonth
                    Case 1, 3, 5, 7, 8, 10, 12
                        day.gMax = 31
                    Case 2
                        day.gMax = 28
                    Case 4, 6, 9, 11
                        day.gMax = 30
                End Select
                month.Text = String.Format("{0:00}", intMonth)
            End If
            month.Suppressed = False
        End Sub

        Private Sub day_Leave(sender As Object, e As EventArgs) Handles day.Leave
            day.Suppressed = True
            If IsNumeric(day.Text) And day.TextLength = 1 Then
                day.Text = String.Format("{0:00}", CInt(day.Text))
            End If
            day.Suppressed = False
        End Sub

        Private Sub year4_Leave(sender As Object, e As EventArgs) Handles year4.Leave
            year4.Suppressed = True
            If IsNumeric(year4.Text) And year4.TextLength = 1 Then
                year4.Text = String.Format("{0:2000}", CInt(year4.Text))
            End If
            If IsNumeric(year4.Text) And year4.TextLength = 2 Then
                year4.Text = String.Format("{0:2000}", CInt(year4.Text))
            End If
            year4.Suppressed = False
        End Sub

        Private Sub hour_Leave(sender As Object, e As EventArgs) Handles hour.Leave
            hour.Suppressed = True
            If IsNumeric(hour.Text) And hour.TextLength = 1 Then
                hour.Text = String.Format("{0:00}", CInt(hour.Text))
            End If
            hour.Suppressed = False
        End Sub

        Private Sub minute_Leave(sender As Object, e As EventArgs) Handles minute.Leave
            minute.Suppressed = True
            If IsNumeric(minute.Text) And minute.TextLength = 1 Then
                minute.Text = String.Format("{0:00}", CInt(minute.Text))
            End If
            minute.Suppressed = False
        End Sub


        Private Sub pnlDate_Leave(sender As Object, e As EventArgs) Handles pnlDate.Leave
            Debug.Print("CTSDateTime " & Me.Name & ": pnlDate LEAVE")
            'Me.Refresh()
        End Sub

        Private Sub pnlTime_Leave(sender As Object, e As EventArgs) Handles pnlTime.Leave
            Debug.Print("CTSDateTime " & Me.Name & ": pnlTime LEAVE")
            'Me.Refresh()
        End Sub


        Private Sub pnlDate_Enter(sender As Object, e As EventArgs) Handles pnlDate.Enter
            Debug.Print("CTSDateTime " & Me.Name & ": pnlDate ENTER")

            Select Case True
            'Test for error back conditions 
                Case ErrorStyle = ErrorStyles.ChangeBackcolor AndAlso (DateErrorText <> String.Empty Or CustomDateErrorText <> String.Empty)
                    pnlDate.BackColor = gErrorBackColor

                'Test for readonly conditions 
                Case gReadOnly
                    pnlDate.BackColor = gReadOnlyBackColor

                'Test for readonly conditions 
                Case gShowDateCheckBox And Not cbEnableDate.Checked
                    pnlDate.BackColor = gReadOnlyBackColor

                    'otherwise
                Case Else
                    pnlDate.BackColor = gFocusBackColor
            End Select

            If pnlDate.BackColor = Color.Transparent Then
                month.BackColor = gTransparentBack
                day.BackColor = gTransparentBack
                year4.BackColor = gTransparentBack
            Else
                month.BackColor = pnlDate.BackColor
                day.BackColor = pnlDate.BackColor
                year4.BackColor = pnlDate.BackColor
            End If

            Me.Refresh()
        End Sub

        Private Sub pnlTime_Enter(sender As Object, e As EventArgs) Handles pnlTime.Enter
            Debug.Print("CTSDateTime " & Me.Name & ": pnlTime ENTER")

            Select Case True
            'Test for error back conditions 
                Case ErrorStyle = ErrorStyles.ChangeBackcolor AndAlso (gTimeErrorText <> String.Empty Or CustomTimeErrorText <> String.Empty)
                    pnlTime.BackColor = gErrorBackColor

                'Test for readonly conditions 
                Case gReadOnly
                    pnlTime.BackColor = gReadOnlyBackColor

                    'otherwise
                Case Else
                    pnlTime.BackColor = gFocusBackColor
            End Select

            If pnlTime.BackColor = Color.Transparent Then
                hour.BackColor = gTransparentBack
                minute.BackColor = gTransparentBack
            Else
                hour.BackColor = pnlTime.BackColor
                minute.BackColor = pnlTime.BackColor
            End If

            Me.Refresh()
        End Sub

        'pass over calendar button
        Private Sub btnCalendar_Enter(sender As Object, e As EventArgs) Handles btnCalendar.Enter
            year4.Focus()
        End Sub

#End Region

    End Class

End Namespace