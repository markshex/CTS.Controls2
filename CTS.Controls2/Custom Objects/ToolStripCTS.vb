Imports System.Globalization
Imports System.ComponentModel
Imports System.Runtime.InteropServices

<ToolboxItem(True), ComVisible(False), DesignTimeVisible(False)>
Public Class ToolStripCTS
    Inherits ToolStrip

#Region "Properties/Declarations"

    Enum ToolStripCTSRenderModes
        CTS
        System
        ManagerRenderMode
        Professional
    End Enum

    Public Enum ProcessModes
        Display
        Update
        Add
        Copy
        Delete
    End Enum

    <ComponentModel.Browsable(False), ComponentModel.DesignerSerializationVisibility(ComponentModel.DesignerSerializationVisibility.Hidden)>
    Overloads Property RenderMode As ToolStripRenderMode

    Private _NewRenderMode As ToolStripCTSRenderModes
    <ComponentModel.Browsable(True), ComponentModel.DesignerSerializationVisibility(ComponentModel.DesignerSerializationVisibility.Hidden)>
    <Category("Appearance"), DisplayName("RenderMode2"), Description("How to draw this control.")>
    Property RenderMode2 As ToolStripCTSRenderModes
        Get
            Return _NewRenderMode
        End Get
        Set(value As ToolStripCTSRenderModes)
            _NewRenderMode = value
            If value = ToolStripCTSRenderModes.CTS Then
                Me.RenderMode = ToolStripRenderMode.ManagerRenderMode
                Me.Renderer = New MyRenderer
            Else
                Me.RenderMode = value
            End If
        End Set
    End Property


    Private _IsDirty As Boolean = False
    <ComponentModel.Browsable(False), ComponentModel.DesignerSerializationVisibility(ComponentModel.DesignerSerializationVisibility.Hidden)>
    Property IsDirty As Boolean
        Get
            Return _IsDirty
        End Get
        Set(value As Boolean)
            If value <> _IsDirty Then
                _IsDirty = value
                Invalidate()
            End If
        End Set
    End Property

    Private _IsLoading As Boolean = False
    <ComponentModel.Browsable(False), ComponentModel.DesignerSerializationVisibility(ComponentModel.DesignerSerializationVisibility.Hidden)>
    Property IsLoading As Boolean
        Get
            Return _IsLoading
        End Get
        Set(value As Boolean)
            If value <> _IsLoading Then
                _IsLoading = value
            End If
        End Set
    End Property

    Private _ProcessMode As ProcessModes = ProcessModes.Display
    <ComponentModel.Browsable(False), ComponentModel.DesignerSerializationVisibility(ComponentModel.DesignerSerializationVisibility.Hidden)>
    Property ProcessMode As ProcessModes
        Get
            Return _ProcessMode
        End Get
        Set(value As ProcessModes)
            If value <> _ProcessMode Then
                _ProcessMode = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' Control the toolbar buttons based on process mode / dirty / error flags
    ''' </summary>
    Private _ModeManagement As Boolean = True
    Property ModeManagement As Boolean
        Get
            Return _ModeManagement
        End Get
        Set(value As Boolean)
            If value <> _ModeManagement Then
                _ModeManagement = value
            End If
        End Set
    End Property


#Region "Label Management"
    Private EnvironmentLabel As New ToolStripLabel
    Private ErrorLabel As New ToolStripLabel

    Private _Environment As CTS.DataEnvironments = CTS.DataEnvironments.Production
    <ComponentModel.Browsable(False), ComponentModel.DesignerSerializationVisibility(ComponentModel.DesignerSerializationVisibility.Hidden)>
    Property Environment As CTS.DataEnvironments
        Get
            Return _Environment
        End Get
        Set(value As CTS.DataEnvironments)
            If value <> _Environment Then
                _Environment = value
                Select Case value
                    Case CTS.DataEnvironments.Production
                        If Items.Contains(EnvironmentLabel) Then
                            Items.Remove(EnvironmentLabel)
                        End If

                    Case CTS.DataEnvironments.Development
                        EnvironmentLabel.Text = "DEV"
                        EnvironmentLabel.ForeColor = DevelopmentLabelFore
                        Me.Items.Insert(0, EnvironmentLabel)

                    Case CTS.DataEnvironments.Test
                        EnvironmentLabel.Text = "TEST"
                        EnvironmentLabel.ForeColor = TestLabelFore
                        Me.Items.Insert(0, EnvironmentLabel)
                End Select

            End If
        End Set
    End Property

    Private _HasErrors As Boolean = False
    <ComponentModel.Browsable(False), ComponentModel.DesignerSerializationVisibility(ComponentModel.DesignerSerializationVisibility.Hidden)>
    Property HasErrors As Boolean
        Get
            Return _HasErrors
        End Get
        Set(value As Boolean)
            If value <> _HasErrors Then
                _HasErrors = value

                If value = True Then
                    ErrorLabel.DisplayStyle = ToolStripItemDisplayStyle.Image
                    ErrorLabel.Image = My.Resources.ErrorSheild
                    Me.Items.Insert(0, ErrorLabel)
                Else
                    If Items.Contains(ErrorLabel) Then
                        Items.Remove(ErrorLabel)
                    End If
                End If

                Invalidate()
            End If
        End Set
    End Property

#End Region

#Region "Button Management"
    Private WithEvents btnCancel As New ToolStripButton
    Private WithEvents btnApply As New ToolStripButton
    Private WithEvents btnAdd As New ToolStripButton
    Private WithEvents btnDelete As New ToolStripButton

    Private WithEvents _Cancel As New BtnDisplay(BtnDisplay.btnTypes.Cancel)
    Private WithEvents _Apply As New BtnDisplay(BtnDisplay.btnTypes.Apply)
    Private WithEvents _Add As New BtnDisplay(BtnDisplay.btnTypes.Add)
    Private WithEvents _Delete As New BtnDisplay(BtnDisplay.btnTypes.Delete)

    Public Event CancelClicked(sender As Object, e As EventArgs)
    Public Event ApplyClicked(sender As Object, e As EventArgs)
    Public Event AddClicked(sender As Object, e As EventArgs)
    Public Event DeleteClicked(sender As Object, e As EventArgs)

    <Category("Toolstrip Buttons")> <Description("Built-in button management")>
    <ComponentModel.DesignerSerializationVisibility(ComponentModel.DesignerSerializationVisibility.Content)>
    Property CancelButton As BtnDisplay
        Get
            Return _Cancel
        End Get
        Set(value As BtnDisplay)
            If value.Visible <> _Cancel.Visible Then
                _Cancel = value
            End If
        End Set
    End Property

    <Category("Toolstrip Buttons")> <Description("Built-in button management")>
    <ComponentModel.DesignerSerializationVisibility(ComponentModel.DesignerSerializationVisibility.Content)>
    Property ApplyButton As BtnDisplay
        Get
            Return _Apply
        End Get
        Set(value As BtnDisplay)
            If value.Visible <> _Apply.Visible Then
                _Apply = value
            End If
        End Set
    End Property

    <Category("Toolstrip Buttons")> <Description("Built-in button management")>
    <ComponentModel.DesignerSerializationVisibility(ComponentModel.DesignerSerializationVisibility.Content)>
    Property AddButton As BtnDisplay
        Get
            Return _Add
        End Get
        Set(value As BtnDisplay)
            If value.Visible <> _Add.Visible Then
                _Add = value
            End If
        End Set
    End Property

    <Category("Toolstrip Buttons")> <Description("Built-in button management")>
    <ComponentModel.DesignerSerializationVisibility(ComponentModel.DesignerSerializationVisibility.Content)>
    Property DeleteButton As BtnDisplay
        Get
            Return _Delete
        End Get
        Set(value As BtnDisplay)
            If value.Visible <> _Delete.Visible Then
                _Delete = value
            End If
        End Set
    End Property

    Private Sub UpdateCancelButtonVisibility(sender As Object) Handles _Cancel.AppearanceChanged
        Dim btn As BtnDisplay = sender
        If btn.Visible Then
            btnCancel.Name = "btnCancel"
            btnCancel.Text = btn.Text
            btnCancel.Image = btn.Image
            Me.Items.Insert(0, btnCancel)
        Else
            Items.Remove(btnCancel)
        End If
    End Sub

    Private Sub UpdateApplyButtonVisibility(sender As Object) Handles _Apply.AppearanceChanged
        Dim btn As BtnDisplay = sender
        If btn.Visible Then
            btnApply.Name = "btnApply"
            btnApply.Text = btn.Text
            btnApply.Image = btn.Image
            Me.Items.Insert(0, btnApply)
        Else
            Items.Remove(btnApply)
        End If
    End Sub

    Private Sub UpdateAddButtonVisibility(sender As Object) Handles _Add.AppearanceChanged
        Dim btn As BtnDisplay = sender
        If btn.Visible Then
            btnAdd.Name = "btnAdd"
            btnAdd.Text = btn.Text
            btnAdd.Image = btn.Image
            Me.Items.Insert(0, btnAdd)
        Else
            Items.Remove(btnAdd)
        End If
    End Sub

    Private Sub UpdateDeleteButtonVisibility(sender As Object) Handles _Delete.AppearanceChanged
        Dim btn As BtnDisplay = sender
        If btn.Visible Then
            btnDelete.Name = "btnDelete"
            btnDelete.Text = btn.Text
            btnDelete.Image = btn.Image
            btnDelete.Alignment = HorizontalAlignment.Right
            Me.Items.Insert(0, btnDelete)
        Else
            Items.Remove(btnDelete)
        End If
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        RaiseEvent CancelClicked(btnCancel, New EventArgs)
    End Sub
    Private Sub btnApply_Click(sender As Object, e As EventArgs) Handles btnApply.Click
        RaiseEvent ApplyClicked(btnApply, New EventArgs)
    End Sub
    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        RaiseEvent CancelClicked(btnAdd, New EventArgs)
    End Sub
    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        RaiseEvent DeleteClicked(btnDelete, New EventArgs)
    End Sub

#End Region

#Region "Color Properties"
    Private _ShadowColor As Color = Color.Transparent
    Private _ActiveBorder As Color = Color.Gold
    Private _ActiveBackground As Color = Color.PaleGoldenrod
    Private _ActivePressed As Color = Color.Goldenrod
    Private _BackGroundColor1 As Color = Color.White
    Private _BackGroundColor2 As Color = Color.LightSteelBlue
    Private _DirtyColor1 As Color = Color.White
    Private _DirtyColor2 As Color = Color.DarkCyan

    Friend TestLabelBack As Color = Color.Yellow
    Friend TestLabelFore As Color = Color.Black
    Friend DevelopmentLabelBack As Color = Color.DarkRed
    Friend DevelopmentLabelFore As Color = Color.White

    <Category("CTS Renderer Colors")> <Description("Shadow Color for the tool strip background.")>
    Property ShadowColor As Color
        Get
            Return _ShadowColor
        End Get
        Set(value As Color)
            _ShadowColor = value
        End Set
    End Property

    <Category("CTS Renderer Colors")> <Description("Border color for any active button (hovered or pressed).")>
    Property ActiveBorder As Color
        Get
            Return _ActiveBorder
        End Get
        Set(value As Color)
            _ActiveBorder = value
        End Set
    End Property

    <Category("CTS Renderer Colors")> <Description("Background color for any active/hovered button.")>
    Property ActiveBackground As Color
        Get
            Return _ActiveBackground
        End Get
        Set(value As Color)
            _ActiveBackground = value
        End Set
    End Property

    <Category("CTS Renderer Colors")> <Description("Background color for any active/pressed button.")>
    Property ActivePressed As Color
        Get
            Return _ActivePressed
        End Get
        Set(value As Color)
            _ActivePressed = value
        End Set
    End Property

    <Category("CTS Renderer Colors")> <Description("Main Background Color")>
    Property BackgroundColor1 As Color
        Get
            Return _BackGroundColor1
        End Get
        Set(value As Color)
            _BackGroundColor1 = value
        End Set
    End Property

    <Category("CTS Renderer Colors")> <Description("Secondary Background Color (for linear gradients) ")>
    Property BackgroundColor2 As Color
        Get
            Return _BackGroundColor2
        End Get
        Set(value As Color)
            _BackGroundColor2 = value
        End Set
    End Property

    <Category("CTS Renderer Colors")> <Description("Main Dirty Background Color")>
    Property DirtyColor1 As Color
        Get
            Return _DirtyColor1
        End Get
        Set(value As Color)
            _DirtyColor1 = value
        End Set
    End Property

    <Category("CTS Renderer Colors")> <Description("Secondary Dirty Background Color (for linear gradients) ")>
    Property DirtyColor2 As Color
        Get
            Return _DirtyColor2
        End Get
        Set(value As Color)
            _DirtyColor2 = value
        End Set
    End Property

#End Region

#Region "Field Collections"

    'Public Event FieldStateChanged(sender As Object)
    'Private Sub Fields_StateChanged(sender As Object) Handles _Fields.FieldStateChanged

    '    RaiseEvent FieldStateChanged(sender)

    '    If ModeManagement Then
    '        Dim Fields As CTSFieldCollection = sender

    '        If Fields.IsDirty Then
    '            IsDirty = True
    '        End If

    '        If Fields.HasErrors Then
    '            HasErrors = True
    '        End If

    '        SetMode()
    '    End If

    'End Sub

    'Private WithEvents _Fields As New FieldCollections()

    '<ComponentModel.Browsable(False), ComponentModel.DesignerSerializationVisibility(ComponentModel.DesignerSerializationVisibility.Hidden)>
    'Public Property Fields As FieldCollections
    '    Get
    '        Return _Fields
    '    End Get
    '    Set(value As FieldCollections)
    '        _Fields = value
    '    End Set
    'End Property


#End Region

#End Region


    Sub New()
        MyBase.New

        Me.GripStyle = ToolStripGripStyle.Hidden
        Me.RenderMode2 = ToolStripCTSRenderModes.CTS

        Dim p As Padding = Me.Padding
        p.Left = 2
        Me.Padding = p

        _Cancel.Visible = CancelButton.Visible
        _Add.Visible = AddButton.Visible
        _Apply.Visible = ApplyButton.Visible
        _Delete.Visible = DeleteButton.Visible

    End Sub

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing Then
                EnvironmentLabel = Nothing
                ErrorLabel = Nothing
                If Renderer IsNot Nothing Then
                    Renderer = Nothing
                End If
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Private Sub InitializeComponent()
        Me.SuspendLayout()
        '
        'ToolStripCTS
        '
        Me.ResumeLayout(False)

    End Sub

    Public Sub SetMode()
        Select Case ProcessMode
            Case ProcessModes.Display
                btnAdd.Visible = False
                btnApply.Visible = False
                btnDelete.Visible = False
                btnCancel.Text = "Return"
                btnCancel.Image = My.Resources.Back

            Case ProcessModes.Update
                btnAdd.Visible = False
                If IsDirty Then
                    btnCancel.Text = "Cancel"
                    btnCancel.Image = My.Resources.RedDelete
                    btnDelete.Visible = False
                    If HasErrors Then
                        btnApply.Visible = False
                    Else
                        btnApply.Visible = True
                    End If
                Else
                    btnCancel.Text = "Return"
                    btnCancel.Image = My.Resources.Back
                    btnDelete.Visible = True
                End If

            Case ProcessModes.Add
                btnAdd.Visible = False
                btnDelete.Visible = False
                If IsDirty Then
                    btnCancel.Text = "Cancel"
                    btnCancel.Image = My.Resources.RedDelete
                    If HasErrors Then
                        btnApply.Visible = False
                    Else
                        btnApply.Visible = True
                    End If
                Else
                    btnCancel.Text = "Return"
                    btnCancel.Image = My.Resources.Back
                End If

            Case ProcessModes.Delete
                btnAdd.Visible = False
                btnDelete.Visible = False
                btnCancel.Text = "Cancel"
                btnCancel.Image = My.Resources.RedDelete
                btnApply.Visible = True

            Case ProcessModes.Copy
        End Select

    End Sub


#Region "helper Classes"

    Public Class MyRenderer
        ' Class to handle drawing routines for toolstrips 
        Inherits System.Windows.Forms.ToolStripProfessionalRenderer

        ''' <summary>
        ''' Render container background gradient
        ''' </summary>
        ''' <param name="e"></param>
        Protected Overrides Sub OnRenderToolStripBackground(ByVal e As ToolStripRenderEventArgs)
            MyBase.OnRenderToolStripBackground(e)

            Dim ts As ToolStripCTS = e.ToolStrip

            Dim c1 As Color = Color.Empty
            Dim c2 As Color = Color.Empty
            If ts.IsDirty Then
                c1 = ts.DirtyColor1
                c2 = ts.DirtyColor2
            Else
                c1 = ts.BackgroundColor1
                c2 = ts.BackgroundColor2
            End If

            If c1 = c2 Or c2 = Color.Empty Or c2 = Color.Transparent Then
                Dim b As New Drawing.SolidBrush(c1)
                e.Graphics.FillRectangle(b, e.ToolStrip.ClientRectangle)
            Else
                Dim b As New Drawing2D.LinearGradientBrush(e.AffectedBounds, c1, c2, Drawing2D.LinearGradientMode.Vertical)
                e.Graphics.FillRectangle(b, e.ToolStrip.ClientRectangle)
            End If

        End Sub


        Protected Overrides Sub OnRenderToolStripBorder(e As ToolStripRenderEventArgs)

            Dim ts As ToolStripCTS = e.ToolStrip

            Select Case ts.ShadowColor
                Case Color.Empty
                    MyBase.OnRenderToolStripBorder(e)

                Case Color.Transparent

                Case Else
                    Dim shadow As New Drawing.SolidBrush(ts.ShadowColor)
                    e.Graphics.FillRectangle(shadow, New Rectangle(1, ts.Height - 1, ts.Width - 2, 1))
                    e.Graphics.FillRectangle(shadow, New Rectangle(ts.Width - 1, 0, 1, ts.Height - 1))
            End Select

        End Sub

        Protected Overrides Sub OnRenderLabelBackground(e As ToolStripItemRenderEventArgs)
            MyBase.OnRenderLabelBackground(e)

            Dim ts As ToolStripCTS = e.ToolStrip
            If e.Item Is ts.EnvironmentLabel Then
                Select Case ts.Environment
                    Case CTS.DataEnvironments.Development
                        Dim b As New Drawing.SolidBrush(ts.DevelopmentLabelBack)
                        e.Graphics.FillRectangle(b, e.ToolStrip.ClientRectangle)
                    Case CTS.DataEnvironments.Production
                    Case CTS.DataEnvironments.Test
                        Dim b As New Drawing.SolidBrush(ts.TestLabelBack)
                        e.Graphics.FillRectangle(b, e.ToolStrip.ClientRectangle)
                End Select
            End If

        End Sub



        ' Render button selected and pressed state
        Protected Overrides Sub OnRenderButtonBackground(ByVal e As System.Windows.Forms.ToolStripItemRenderEventArgs)
            MyBase.OnRenderButtonBackground(e)

            Dim ts As ToolStripCTS = e.ToolStrip

            If e.Item.Selected Or CType(e.Item, ToolStripButton).Checked Then
                Dim rectBorder As New Rectangle(0, 0, e.Item.Width - 1, e.Item.Height - 1)
                Dim BorderBrush As New Drawing.SolidBrush(ts.ActiveBorder)
                e.Graphics.DrawRectangle(New Pen(BorderBrush, 1), rectBorder)

                Dim rect As New Rectangle(1, 1, e.Item.Width - 2, e.Item.Height - 2)
                Dim b As New Drawing.SolidBrush(ts.ActiveBackground)
                e.Graphics.FillRectangle(b, rect)
            End If

            If e.Item.Pressed Then
                Dim rect As New Rectangle(1, 1, e.Item.Width - 2, e.Item.Height - 2)
                Dim b As New Drawing.SolidBrush(ts.ActivePressed)
                e.Graphics.FillRectangle(b, rect)
            End If

        End Sub


        Private Sub drawGradient(ByRef G As Graphics, w As Integer, h As Integer, c1 As Color, c2 As Color, BorderColor As Color)
            Dim rectBorder As New Rectangle(0, 0, w - 1, h - 1)
            Dim BorderBrush As New Drawing.SolidBrush(BorderColor)
            G.DrawRectangle(New Pen(BorderBrush, 1), rectBorder)

            Dim rect As New Rectangle(1, 1, w - 2, h - 2)
            Dim b As New Drawing2D.LinearGradientBrush(rect, c1, c2, Drawing2D.LinearGradientMode.Vertical)
            G.FillRectangle(b, rect)
        End Sub


        Private Sub drawGradient(ByRef G As Graphics, w As Integer, h As Integer, c1 As Color, c2 As Color)
            Dim rect As New Rectangle(0, 0, w, h)
            Dim b As New Drawing2D.LinearGradientBrush(rect, c1, c2, Drawing2D.LinearGradientMode.Vertical)
            G.FillRectangle(b, rect)
        End Sub

    End Class

    <TypeConverter(GetType(BtnConverter))>
    Public Class BtnDisplay
        Private _Visible As Boolean = False
        Private _Name As String = "btn"
        Private _Text As String = "btn"
        Private _Image As Image = Nothing
        Public Event AppearanceChanged(sender As Object)

        Public Enum btnTypes
            Cancel
            Apply
            Add
            Delete
        End Enum

        Sub New(Type As btnTypes)
            If Not Debugger.IsAttached Then
                Select Case Type
                    Case btnTypes.Cancel
                        Name = "_btnCancel"
                        Text = "Return"
                        Image = My.Resources.Back

                    Case btnTypes.Apply
                        Name = "_btnApply"
                        Text = "Apply"
                        Image = My.Resources.Accept

                    Case btnTypes.Add
                        Name = "_btnAdd"
                        Text = "Add"
                        Image = My.Resources.Add

                    Case btnTypes.Delete
                        Name = "_btnDelete"
                        Text = "Delete"
                        Image = My.Resources.Delete
                End Select
            End If
        End Sub

        Overloads Function Tostring() As String
            Return _Visible.ToString
        End Function

        <Browsable(True), NotifyParentProperty(True), EditorBrowsable(EditorBrowsableState.Always)>
        Public Property Visible() As Boolean
            Get
                Return _Visible
            End Get
            Set(value As Boolean)
                If _Visible <> value Then
                    _Visible = value
                    RaiseEvent AppearanceChanged(Me)
                End If
            End Set
        End Property

        <Browsable(True), NotifyParentProperty(True), EditorBrowsable(EditorBrowsableState.Always)>
        Public Property Text() As String
            Get
                Return _Text
            End Get
            Set(value As String)
                If _Text <> value Then
                    _Text = value
                    RaiseEvent AppearanceChanged(Me)
                End If
            End Set
        End Property

        <Browsable(True), NotifyParentProperty(True), EditorBrowsable(EditorBrowsableState.Always)>
        Public Property Name() As String
            Get
                Return _Name
            End Get
            Set(value As String)
                If _Name <> value Then
                    _Name = value
                End If
            End Set
        End Property

        <Browsable(True), NotifyParentProperty(True), EditorBrowsable(EditorBrowsableState.Always)>
        Public Property Image() As Image
            Get
                Return _Image
            End Get
            Set(value As Image)
                _Image = value
                RaiseEvent AppearanceChanged(Me)
            End Set
        End Property

    End Class

    Public Class BtnConverter
        Inherits ExpandableObjectConverter

        ' This override prevents the PropertyGrid from displaying the full type name in the value cell. 
        Public Overrides Function ConvertTo(ByVal context As ITypeDescriptorContext,
                                            ByVal culture As CultureInfo,
                                            ByVal value As Object,
                                            ByVal destinationType As Type) As Object

            If destinationType Is GetType(String) Then
                Dim btn As BtnDisplay = value
                Return btn.Visible.ToString
            End If

            Return MyBase.ConvertTo(context, culture, value, destinationType)

        End Function

    End Class

#End Region

End Class


