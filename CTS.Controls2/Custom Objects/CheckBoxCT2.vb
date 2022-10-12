Imports System.ComponentModel
Imports System.Runtime.InteropServices

<ComVisible(False)>
Public Class CheckBoxCT2
    Inherits CheckBox
    Private ttError As ToolTip

#Region "Color Constants"
    Private gTransparentBack As Color

    'Private gInputBackColor As Color = EditBack
    'Private gErrorBackColor As Color = EditErrorBack
    'Private gErrorForeColor As Color = EditErrorFore
    'Private gFocusBackColor As Color = EditFocusBack
    'Private gReadOnlyBackColor As Color = EditProtect

    Private gInputBackColor As Color = Color.Empty
    Private gErrorBackColor As Color = Color.Empty
    Private gErrorForeColor As Color = Color.Empty
    Private gFocusBackColor As Color = Color.Empty
    Private gReadOnlyBackColor As Color = Color.Empty

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

    Private gOriginalValue As Object = Nothing
    <Browsable(False), EditorBrowsable(EditorBrowsableState.Always),
    DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public ReadOnly Property OriginalValue() As String
        Get
            Return gOriginalValue
        End Get
    End Property

    Private gValue As Object = Nothing
    <Browsable(False), EditorBrowsable(EditorBrowsableState.Always),
    DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public ReadOnly Property Value() As Object
        Get
            Return gValue
        End Get
    End Property

    Private gCheckedValue As String
    Public Property CheckedValue As String
        Get
            Return gCheckedValue
        End Get
        Set(value As String)
            gCheckedValue = value
            If Me.Checked Then
                gValue = value
            End If
        End Set
    End Property

    Private gCheckedDisplay As String
    Public Property CheckedDisplay As String
        Get
            Return gCheckedDisplay
        End Get
        Set(value As String)
            gCheckedDisplay = value
            If Me.Checked Then
                MyBase.Text = value
            End If
        End Set
    End Property

    Private gUncheckedValue As String
    Public Property UncheckedValue As String
        Get
            Return gUncheckedValue
        End Get
        Set(value As String)
            gUncheckedValue = value
            If Not Me.Checked Then
                gValue = value
            End If
        End Set
    End Property

    Private gUncheckedDisplay As String
    Public Property UncheckedDisplay As String
        Get
            Return gUncheckedDisplay
        End Get
        Set(value As String)
            gUncheckedDisplay = value
            If Not Me.Checked Then
                MyBase.Text = value
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
                'ShowControl()
                OnReadOnlyChanged()
            End If
        End Set
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

    Public Suppress As Boolean

    Public Event ErrorTextChanged(ByVal sender As Object)
    Public Event StateChanged(ByVal sender As Object)


#Region "On Event Overrides"

    Protected Overrides Sub OnCreateControl()
        gDefaultForeColor = Me.ForeColor
        gDefaultBackColor = Me.BackColor

        Debug.Print("ONCREATE CheckBoxCTS: " & Me.Name & "/" & Me.Parent.Name & "/" & Me.Parent.Parent.Name & "/")

        InitializeControlColor()
        MyBase.OnCreateControl()
    End Sub

    Protected Overrides Sub OnEnter(e As EventArgs)
        If Not Suppress Then
            Select Case True
                'Test for error back conditions 
                Case gErrorText <> String.Empty And gErrorStyle = ErrorStyles.ChangeBackcolor
                    Me.BackColor = gErrorBackColor
                    Me.ForeColor = gErrorForeColor

                    'Test for readonly conditions 
                Case Me.ReadOnly And gReadOnlyBackColor = Color.Transparent
                    Me.BackColor = gTransparentBack
                    Me.ForeColor = gDefaultForeColor

                    'Test for readonly conditions 
                Case Me.ReadOnly
                    Me.BackColor = gReadOnlyBackColor
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
            Me.BackColor = BackSave
        End If
        MyBase.OnLeave(e)
    End Sub

    Protected Overrides Sub OnCheckedChanged(e As EventArgs)

        Select Case MyBase.CheckState
            Case Windows.Forms.CheckState.Checked
                If gCheckedDisplay <> String.Empty Then
                    MyBase.Text = gCheckedDisplay
                End If
                gValue = gCheckedValue

            Case Windows.Forms.CheckState.Unchecked
                If gUncheckedDisplay <> String.Empty Then
                    MyBase.Text = gUncheckedDisplay
                End If
                gValue = gUncheckedValue

            Case Windows.Forms.CheckState.Indeterminate
                gValue = Nothing
        End Select

        MyBase.OnCheckedChanged(e)
    End Sub

    Protected Overrides Sub OnCheckStateChanged(e As EventArgs)

        If Not Suppress Then
            If IsNothing(gOriginalValue) AndAlso IsNothing(gValue) Then
                IsDirty = False
            ElseIf IsNothing(gOriginalValue) AndAlso Not IsNothing(gValue) Then
                IsDirty = True
            ElseIf gValue <> gOriginalValue Then
                IsDirty = True
            Else
                IsDirty = False
            End If
        End If

        MyBase.OnCheckStateChanged(e)
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


    Public Sub AcceptChanges()
        gOriginalValue = gValue
        IsDirty = False
    End Sub

    'Sets the control back/fore color based on readonly & error text/style 
    'uses readonlybackcolor property, inputbackcolor property or errorbackcolor property 
    'OR the derived transparent color
    Private Sub InitializeControlColor()

        If Me.ReadOnly And gReadOnlyBackColor = Color.Transparent Then
            If gTransparentBack = Color.Empty Then SetTransparentBackColor()
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

        Me.BackColor = BackSave
        Me.ForeColor = ForeSave
    End Sub

    Private Sub SetTransparentBackColor()
        Dim NewBackColor As Color = EditProtect
        gTransparentBack = NewBackColor
    End Sub
End Class