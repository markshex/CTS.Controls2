Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports System.ComponentModel
Imports System.Runtime.InteropServices

<ComVisible(False)> _
Public Class TextBoxCT2
    Inherits TextBox

#Region "Textbox Properties"
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


    <Browsable(True), EditorBrowsable(EditorBrowsableState.Always), _
     Category("_Custom Properties"), Description("Background color when textbox is readonly")> _
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
    <Browsable(False), EditorBrowsable(EditorBrowsableState.Always), _
    DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public ReadOnly Property OriginalText() As String
        Get
            Return gOriginalText
        End Get
    End Property

    Private gFormat As String
    <Browsable(True), EditorBrowsable(EditorBrowsableState.Always), _
     Category("_Custom Properties"), Description("Formatting string used when text data is numeric")> _
    Public Property Format() As String
        Get
            Return gFormat
        End Get
        Set(ByVal value As String)
            gFormat = value
        End Set
    End Property

    Private gFormattedText As String = Nothing
    <Browsable(False), EditorBrowsable(EditorBrowsableState.Always), _
    DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public ReadOnly Property FormattedText() As String
        Get
            Return gFormattedText
        End Get
    End Property

    Private gValue As Object = Nothing
    <Browsable(False), EditorBrowsable(EditorBrowsableState.Always), _
    DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public ReadOnly Property Value() As Object
        Get
            Return gValue
        End Get
    End Property

    Private gDataType As DataTypes
    <Browsable(True), EditorBrowsable(EditorBrowsableState.Always), _
     Category("_Custom Properties"), Description("Resulting data type for this textbox")> _
    Public Property DataType() As DataTypes
        Get
            Return gDataType
        End Get
        Set(ByVal value As DataTypes)
            gDataType = value
        End Set
    End Property

    Private gPromptButton As String = String.Empty
    ''' <summary>
    ''' Name of the button the click when F4 is pressed while control has focus.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Browsable(True), EditorBrowsable(EditorBrowsableState.Always), _
     DefaultValue(""), _
     Category("_Custom Properties"), Description("Button linked to F4 Prompt")>
    Public Property PromptButton() As String
        Get
            Return gPromptButton
        End Get
        Set(ByVal value As String)
            gPromptButton = value
        End Set
    End Property

    Private gErrorText As String
    <Browsable(False), EditorBrowsable(EditorBrowsableState.Always), _
     DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
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
    <Browsable(True), EditorBrowsable(EditorBrowsableState.Always), _
    DefaultValue(ErrorStyles.ChangeBackcolor), _
    Category("_Custom Properties"), Description("How to handle the appearance when ErrorText exists.")> _
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

    Private gInput As InputTypes
    <Browsable(True), EditorBrowsable(EditorBrowsableState.Always), _
    Category("_Custom Properties"), Description("Keyboard Character Restrictions"), _
    DesignerSerializationVisibility(DesignerSerializationVisibility.Content)> _
    Public Property Input() As InputTypes
        Get
            Return Me.gInput
        End Get
        Set(ByVal value As InputTypes)
            Me.gInput = value
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

    Public Enum InputTypes
        AllCharacters
        NumericOnly
        CharactersOnly
    End Enum

    Public Enum DataTypes
        StringType
        IntegerType
        ShortType
        LongType
        DecimalType
        DoubleType
        DB2Numeric
        DB2Character
    End Enum

    Public Suppress As Boolean

    Public Event ErrorTextChanged(ByVal sender As Object)
    Public Event StateChanged(ByVal sender As Object)
    Public Event TabPress(ByVal sender As Object)
#End Region

    ''' <summary>
    ''' Default Constructor
    ''' </summary>
    Public Sub New()
        ttError = New ToolTip()
        ttError.ToolTipTitle = "ERROR:"
        ttError.ToolTipIcon = ToolTipIcon.Error

        Me.BorderStyle = Windows.Forms.BorderStyle.FixedSingle
    End Sub

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing Then
                ttError.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub


    Protected Overrides Sub OnCreateControl()
        gDefaultForeColor = Me.ForeColor
        gDefaultBackColor = Me.BackColor

        InitializeControlColor()
        MyBase.OnCreateControl()
    End Sub

    Protected Overrides Sub OnKeyDown(ByVal e As System.Windows.Forms.KeyEventArgs)

        'Prompt
        If e.KeyData = Keys.F4 AndAlso gPromptButton <> String.Empty Then
            Dim btn As Button = DirectCast(FindControl(gPromptButton, Me.Parent), Button)
            btn.PerformClick()
        End If

    End Sub

    Protected Overrides Sub OnKeyPress(ByVal e As System.Windows.Forms.KeyPressEventArgs)
        MyBase.OnKeyPress(e)

        Select Case Me.gInput
            Case InputTypes.CharactersOnly
                If IsNumeric(e.KeyChar) Then
                    e.Handled = True
                End If
            Case InputTypes.NumericOnly
                If Not IsNumeric(e.KeyChar) And Asc(e.KeyChar) <> 8 Then
                    e.Handled = True
                End If
        End Select

    End Sub

    Protected Overrides Sub OnKeyUp(ByVal e As System.Windows.Forms.KeyEventArgs)

        'tab
        If e.KeyData = Keys.Tab Then
            RaiseEvent TabPress(Me)
        End If

    End Sub

    Protected Overrides Sub OnEnter(e As EventArgs)
        If Not Suppress Then
            Select Case True
                'Test for error back conditions 
                Case gErrorText <> String.Empty And gErrorStyle = ErrorStyles.ChangeBackcolor
                    BackColor = gErrorBackColor
                    ForeColor = gErrorForeColor

                    'Test for readonly conditions 
                Case [ReadOnly] And gReadOnlyBackColor = Color.Transparent
                    BackColor = gTransparentBack
                    ForeColor = gDefaultForeColor

                    'Test for readonly conditions 
                Case [ReadOnly]
                    BackColor = gReadOnlyBackColor
                    ForeColor = gDefaultForeColor

                    'otherwise
                Case Else
                    BackColor = gFocusBackColor
                    ForeColor = gDefaultForeColor
            End Select
        End If
        Debug.Print($"textboxcts {Name} OnEnter")
        MyBase.OnEnter(e)
    End Sub

    Protected Overrides Sub OnParentChanged(e As EventArgs)
        If Not Suppress Then
            'If gReadOnlyBackColor = Color.Transparent Then
            SetTransparentBackColor()
            'End If
        End If
        MyBase.OnParentChanged(e)
    End Sub

    Protected Overrides Sub OnLeave(e As EventArgs)
        If Not Suppress Then
            If ErrorText = String.Empty Then
                Me.BackColor = BackSave
            End If
        End If
        Debug.Print($"textboxcts {Name} OnLeave")
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
        End If
        Debug.Print($"textboxcts {Name} OnTextChanged")
        MyBase.OnTextChanged(e)
    End Sub


    Protected Overrides Sub OnReadOnlyChanged(e As EventArgs)
        If Not Suppress Then
            InitializeControlColor()
            Me.TabStop = Not Me.ReadOnly
        End If
        MyBase.OnReadOnlyChanged(e)
    End Sub

    Public Sub AcceptChanges()
        Try
            gOriginalText = MyBase.Text
            IsDirty = False
            ErrorText = String.Empty
            InitializeControlColor()
        Catch ex As Exception

        End Try
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

        Debug.Print($"textboxcts {Name} InitializeControlColor")
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

    Friend Function FindControl(ByVal ControlName As String, ByVal ParentControl As Control) As Control
        Dim ctr As Control
        For Each ctr In ParentControl.Controls
            If ctr.Name = ControlName Then
                Return ctr
            Else
                ctr = FindControl(ControlName, ctr)
                If Not ctr Is Nothing Then
                    Return ctr
                End If
            End If
        Next ctr
        Return Nothing
    End Function

    Public Sub SetFocus()
        'Me.Select()
        OnEnter(New EventArgs)
        Debug.Print($"textboxcts {Name} SetFocus")
    End Sub

End Class




