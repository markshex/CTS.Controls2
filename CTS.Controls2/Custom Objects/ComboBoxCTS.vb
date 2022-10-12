Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Runtime.InteropServices
Imports System.ComponentModel


<ComVisible(False)>
Public Class ComboBoxCTS
    Inherits ComboBox

#Region "Declarations & Properties"
    Private txtReadOnly As TextBox
    Private ttError As ToolTip

#Region "Color Constants"
    Private gTransparentBack As Color
    Private gInputBackColor As Color = EditBack
    Private gErrorBackColor As Color = EditErrorBack
    Private gErrorForeColor As Color = EditErrorFore
    Private gFocusBackColor As Color = EditFocusBack
    Private gReadOnlyBackColor As Color = EditProtect
    Private gDefaultBackColor As Color
    Private gDefaultForeColor As Color
    Private BackSave As Color
    Private ForeSave As Color
#End Region

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

    Private gOriginalText As String = Nothing
    <Browsable(False), EditorBrowsable(EditorBrowsableState.Always),
    DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public ReadOnly Property OriginalText() As String
        Get
            Return gOriginalText
        End Get
    End Property

    Private gErrorText As String
    <Browsable(False), EditorBrowsable(EditorBrowsableState.Always),
     DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property ErrorText() As String
        Get
            Return gErrorText
        End Get
        Set(ByVal value As String)
            If gErrorText <> value Then
                gErrorText = value
                If Not Suppress Then
                    RaiseEvent ErrorTextChanged(Me)
                    ttError.SetToolTip(Me, value)
                    InitializeControlColor()
                End If
            End If
        End Set
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

    Private gIsDirty As Boolean
    <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Property IsDirty() As Boolean
        Get
            Return gIsDirty
        End Get
        Set(value As Boolean)
            If value <> gIsDirty Then
                gIsDirty = value
                If Not Suppress Then
                    RaiseEvent StateChanged(Me)
                End If
            End If
        End Set

    End Property

    Private gReadOnly As Boolean = False
    ''' <summary>
    ''' Gets or sets a value indicating whether the control is read-only.
    ''' </summary>
    ''' <value>
    ''' <b>true</b> if the combo box is read-only; otherwise, <b>false</b>. The default is <b>false</b>.
    ''' </value>
    ''' <remarks>
    ''' When this property is set to <b>true</b>, the contents of the control cannot be changed 
    ''' by the user at runtime. With this property set to <b>true</b>, you can still set the value
    ''' in code. You can use this feature instead of disabling the control with the Enabled
    ''' property to allow the contents to be copied.
    ''' </remarks>
    <Browsable(True)>
    <DefaultValue(False)>
    <Category("Behavior")>
    <Description("Controls whether the value in the combobox control can be changed or not")>
    Public Property [ReadOnly]() As Boolean
        Get
            Return gReadOnly
        End Get
        Set(value As Boolean)
            If value <> gReadOnly Then
                gReadOnly = value
                ShowControl()
                OnReadOnlyChanged()
            End If
        End Set
    End Property

    Private gVisible As Boolean = True
    ''' <summary>
    ''' Gets or sets a value indicating wether the control is displayed.
    ''' </summary>
    ''' <value><b>true</b> if the control is displayed; otherwise, <b>false</b>. 
    ''' The default is <b>true</b>.</value>
    Public Shadows Property Visible() As Boolean
        Get
            Return gVisible
        End Get
        Set(value As Boolean)
            gVisible = value
            ShowControl()
        End Set
    End Property


    Public Suppress As Boolean

    Public Event ErrorTextChanged(ByVal sender As Object)
    Public Event StateChanged(ByVal sender As Object)
#End Region


    ''' <summary>
    ''' Default Constructor
    ''' </summary>
    Public Sub New()
        txtReadOnly = New TextBox()
        txtReadOnly.BorderStyle = BorderStyle.FixedSingle
        txtReadOnly.Visible = False

        ttError = New ToolTip()
        ttError.ToolTipTitle = "ERROR:"
        ttError.ToolTipIcon = ToolTipIcon.Error
    End Sub


#Region "On Event Overrides"
    ''' <summary>
    ''' This member overrides <see cref="Control.OnParentChanged"/>
    ''' </summary>
    ''' <param name="e"></param>
    Protected Overrides Sub OnParentChanged(e As EventArgs)
        MyBase.OnParentChanged(e)

        If Parent IsNot Nothing Then
            AddTextbox()
        End If
        txtReadOnly.Parent = Me.Parent

        If Not Suppress Then
            'If gReadOnlyBackColor = Color.Transparent Then
            SetTransparentBackColor()
            'End If
        End If

    End Sub

    ''' <summary>
    ''' This member overrides <see cref="ComboBoxCTS.OnSelectedIndexChanged"/>.
    ''' </summary>
    Protected Overrides Sub OnSelectedIndexChanged(e As EventArgs)
        MyBase.OnSelectedIndexChanged(e)
        If Not Suppress Then
            Dim Current As Object = Nothing
            If SelectedItem IsNot Nothing Then Current = SelectedItem
            If SelectedValue IsNot Nothing Then Current = SelectedValue

            If Current Is Nothing Then
                txtReadOnly.Clear()
                If gOriginalText Is Nothing Then
                    IsDirty = False
                Else
                    IsDirty = True
                End If
            Else
                txtReadOnly.Text = Current.ToString

                If IsNothing(gOriginalText) AndAlso Current.ToString <> String.Empty Then
                    IsDirty = True
                Else
                    If Current.ToString <> gOriginalText Then
                        IsDirty = True
                    Else
                        IsDirty = False
                    End If
                End If
            End If
        End If

    End Sub

    ''' <summary>
    ''' This member overrides <see cref="ComboBoxCTS.OnDropDownStyleChanged"/>.
    ''' </summary>
    Protected Overrides Sub OnDropDownStyleChanged(e As EventArgs)
        MyBase.OnDropDownStyleChanged(e)
        txtReadOnly.Text = Me.Text
    End Sub

    ''' <summary>
    ''' This member overrides <see cref="ComboBoxCTS.OnFontChanged"/>.
    ''' </summary>
    ''' <param name="e"></param>
    Protected Overrides Sub OnFontChanged(e As EventArgs)
        MyBase.OnFontChanged(e)
        txtReadOnly.Font = Me.Font
    End Sub

    ''' <summary>
    ''' This member overrides <see cref="ComboBoxCTS.OnResize"/>.
    ''' </summary>
    ''' <param name="e"></param>
    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        txtReadOnly.Size = Me.Size
    End Sub

    ''' <summary>
    ''' This member overrides <see cref="Control.OnDockChanged"/>.
    ''' </summary>
    ''' <param name="e"></param>
    Protected Overrides Sub OnDockChanged(e As EventArgs)
        MyBase.OnDockChanged(e)
        txtReadOnly.Dock = Me.Dock
    End Sub

    ''' <summary>
    ''' This member overrides <see cref="Control.OnEnabledChanged"/>.
    ''' </summary>
    ''' <param name="e"></param>
    Protected Overrides Sub OnEnabledChanged(e As EventArgs)
        MyBase.OnEnabledChanged(e)
        ShowControl()
    End Sub

    ''' <summary>
    ''' This member overrides <see cref="Control.OnRightToLeftChanged"/>.
    ''' </summary>
    ''' <param name="e"></param>
    Protected Overrides Sub OnRightToLeftChanged(e As EventArgs)
        MyBase.OnRightToLeftChanged(e)
        txtReadOnly.RightToLeft = Me.RightToLeft
    End Sub

    ' <summary>
    ' This member overrides <see cref="Control.OnTextChanged"/>.
    ' </summary>
    ' <param name="e"></param>
    'Protected Overrides Sub OnTextChanged(e As EventArgs)
    '    MyBase.OnTextChanged(e)
    '    txtReadOnly.Text = Me.Text
    'End Sub

    ''' <summary>
    ''' This member overrides <see cref="Control.OnLocationChanged"/>.
    ''' </summary>
    ''' <param name="e"></param>
    Protected Overrides Sub OnLocationChanged(e As EventArgs)
        MyBase.OnLocationChanged(e)
        txtReadOnly.Location = Me.Location
    End Sub

    ''' <summary>
    ''' This member overrides <see cref="Control.OnTabIndexChanged"/>.
    ''' </summary>
    ''' <param name="e"></param>
    Protected Overrides Sub OnTabIndexChanged(e As EventArgs)
        MyBase.OnTabIndexChanged(e)
        txtReadOnly.TabIndex = Me.TabIndex
    End Sub

    Protected Overrides Sub OnCreateControl()
        gDefaultForeColor = Me.ForeColor
        gDefaultBackColor = Me.BackColor

        ' Debug.Print("ONCREATE ComboBoxCTS: " & Me.Name & "/" & Me.Parent.Name & "/" & Me.Parent.Parent.Name & "/")

        InitializeControlColor()
        MyBase.OnCreateControl()
    End Sub

    Protected Overrides Sub OnEnter(e As EventArgs)
        If Not Suppress Then
            Select Case True
                'Test for error back conditions 
                Case gErrorText <> String.Empty And gErrorStyle = ErrorStyles.ChangeBackcolor
                    Me.BackColor = gErrorBackColor
                    Me.txtReadOnly.BackColor = gErrorBackColor
                    Me.ForeColor = gErrorForeColor

                    'Test for readonly conditions 
                Case Me.ReadOnly And gReadOnlyBackColor = Color.Transparent
                    Me.BackColor = gTransparentBack
                    Me.txtReadOnly.BackColor = gTransparentBack
                    Me.ForeColor = gDefaultForeColor

                    'Test for readonly conditions 
                Case Me.ReadOnly
                    Me.BackColor = gReadOnlyBackColor
                    Me.txtReadOnly.BackColor = gReadOnlyBackColor
                    Me.ForeColor = gDefaultForeColor

                    'otherwise
                Case Else
                    Me.BackColor = gFocusBackColor
                    Me.ForeColor = gDefaultForeColor
            End Select
        End If
        MyBase.OnEnter(e)
    End Sub

    Protected Overrides Sub OnLeave(e As EventArgs)
        If ErrorText = String.Empty Then
            Debug.Print($"back is {BackSave.ToString}")
            Me.BackColor = BackSave
        End If
        MyBase.OnLeave(e)
    End Sub

    Protected Overrides Sub OnTextChanged(e As EventArgs)
        If Not Suppress Then
            If IsNothing(gOriginalText) AndAlso MyBase.Text <> String.Empty Then
                IsDirty = True
            Else
                If MyBase.Text <> gOriginalText Then
                    IsDirty = True
                Else
                    IsDirty = False
                End If
            End If
            txtReadOnly.Text = Me.Text
        End If

        MyBase.OnTextChanged(e)
    End Sub

    Protected Sub OnReadOnlyChanged()
        If Not Suppress Then
            'If gReadOnly = True Then
            '    Dim NewBackColor As Color = Color.Empty
            '    GetTransparentBack(Me.Parent, NewBackColor)
            '    gReadOnlyBackColor = NewBackColor
            'End If

            InitializeControlColor()
            Me.TabStop = Not Me.ReadOnly
        End If
    End Sub

#End Region

#Region "Public methods"
    ''' <summary>
    ''' Conceals the control from the user.
    ''' </summary>
    ''' <remarks>
    ''' Hiding the control is equvalent to setting the <see cref="Visible"/> property to <b>false</b>. 
    ''' After the <b>Hide</b> method is called, the <b>Visible</b> property returns a value of 
    ''' <b>false</b> until the <see cref="Show"/> method is called.
    ''' </remarks>
    Public Shadows Sub Hide()
        Visible = False
    End Sub

    ''' <summary>
    ''' Displays the control to the user.
    ''' </summary>
    ''' <remarks>
    ''' Showing the control is equivalent to setting the <see cref="Visible"/> property to <b>true</b>.
    ''' After the <b>Show</b> method is called, the <b>Visible</b> property returns a value of 
    ''' <b>true</b> until the <see cref="Hide"/> method is called.
    ''' </remarks>
    Public Shadows Sub Show()
        Visible = True
    End Sub

    Public Sub AcceptChanges()
        If Me.SelectedValue IsNot Nothing Then
            gOriginalText = Me.SelectedValue.ToString
        Else
            gOriginalText = Nothing
        End If
        IsDirty = False
        ErrorText = String.Empty
        InitializeControlColor()
    End Sub
#End Region

#Region "Private methods"
    ''' <summary>
    ''' Initializes the embedded TextBox with the default values from the ComboBox
    ''' </summary>
    Private Sub AddTextbox()
        txtReadOnly.[ReadOnly] = True
        txtReadOnly.Location = Me.Location
        txtReadOnly.Size = Me.Size
        txtReadOnly.Dock = Me.Dock
        txtReadOnly.Anchor = Me.Anchor
        txtReadOnly.Enabled = MyBase.Enabled
        txtReadOnly.Visible = Me.Visible
        txtReadOnly.RightToLeft = Me.RightToLeft
        txtReadOnly.Font = Me.Font
        txtReadOnly.Text = Me.Text
        txtReadOnly.TabStop = Me.TabStop
        txtReadOnly.TabIndex = Me.TabIndex
    End Sub

    ''' <summary>
    ''' Shows either the ComboBox or the TextBox or nothing, depending on the state
    ''' of the ReadOnly, Enable and Visible flags.
    ''' </summary>
    Private Sub ShowControl()
        If Not DesignMode Then
            If gReadOnly Then
                txtReadOnly.Visible = gVisible AndAlso Me.Enabled
                MyBase.Visible = gVisible AndAlso Not Me.Enabled
                txtReadOnly.Text = Me.Text
            Else
                txtReadOnly.Visible = False
                MyBase.Visible = gVisible
            End If
        End If
    End Sub

    'Sets the control back/fore color based on readonly & error text/style 
    'uses readonlybackcolor property, inputbackcolor property or errorbackcolor property 
    'OR the derived transparent color
    Private Sub InitializeControlColor()

        If Me.ReadOnly And gReadOnlyBackColor = Color.Transparent Then
            BackSave = gTransparentBack
        ElseIf Me.ReadOnly And gReadOnlyBackColor <> Color.Transparent Then
            BackSave = gReadOnlyBackColor
        Else
            BackSave = gInputBackColor
        End If

        If gErrorText <> String.Empty And gErrorStyle = ErrorStyles.ChangeBackcolor Then
            BackSave = gErrorBackColor
            ForeSave = gErrorForeColor
        Else
            ForeSave = Color.Black
        End If

        Debug.Print($"back is {BackSave.ToString}")
        Me.BackColor = BackSave
        Me.txtReadOnly.BackColor = BackSave
        Me.ForeColor = ForeSave
    End Sub

    Private Sub SetTransparentBackColor()
        Dim NewBackColor As Color = EditProtect
        'GetTransparentBack(Me.Parent, NewBackColor)
        gTransparentBack = NewBackColor
    End Sub

    Private Sub GetTransparentBack(ByVal ctrl As Control, ByRef clr As Color)
        Try
            If clr = Color.Empty Then
                If ctrl.BackColor <> Color.Transparent Then
                    clr = ctrl.BackColor
                Else
                    GetTransparentBack(ctrl.Parent, clr)
                End If
            End If
        Catch
            clr = EditProtect
        End Try
    End Sub
#End Region

End Class


