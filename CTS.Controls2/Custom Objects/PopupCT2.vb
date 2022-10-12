Imports System.ComponentModel

Public Class PopupCT2
    Inherits ToolStripDropDown

    <System.Diagnostics.DebuggerNonUserCode()>
    Public Sub New(ByVal container As System.ComponentModel.IContainer)
        MyClass.New()

        'Required for Windows.Forms Class Composition Designer support
        If (container IsNot Nothing) Then
            container.Add(Me)
        End If

    End Sub

    <System.Diagnostics.DebuggerNonUserCode()>
    Public Sub New()
        MyBase.New()

        'This call is required by the Component Designer.
        InitializeComponent()

    End Sub

    'Component overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If

            If _child IsNot Nothing Then
                Dim _content As System.Windows.Forms.Control = _child
                _child = Nothing
                _content.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Component Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Component Designer
    'It can be modified using the Component Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        components = New System.ComponentModel.Container()

    End Sub




    Private _host As ToolStripControlHost
    Private _wrapper As New PupChildWrapper
    Private _child As Control

    Private _AllowDragging As Boolean = False
    Property AllowDragging() As Boolean
        Get
            Return _AllowDragging
        End Get
        Set(ByVal value As Boolean)
            _AllowDragging = value
            If value = True Then
                _wrapper.Banner.Visible = True
                _wrapper.Banner._AllowDragging = True
            Else
                _wrapper.Banner._AllowDragging = False
                If Not _ShowCloseButton Then
                    _wrapper.Banner.Visible = False
                End If
            End If
        End Set
    End Property

    Private _ShowCloseButton As Boolean = False
    Property ShowCloseButton() As Boolean
        Get
            Return _ShowCloseButton
        End Get
        Set(ByVal value As Boolean)
            _ShowCloseButton = value
            If value = True Then
                _wrapper.Banner.Visible = True
            Else
                If Not _AllowDragging Then
                    _wrapper.Banner.Visible = False
                End If
            End If
        End Set
    End Property

    Public Sub New(ChildControl As Control)
        InitializeComponent()

        InitializeChildPCW(ChildControl)

    End Sub

    Sub InitializeChildPCW(Child As Control)

        If Child Is Nothing Then
            Throw New ArgumentNullException("no content")
        End If

        _child = Child

        _wrapper.ChildPanel.Controls.Add(Child)
        'Child.Dock = DockStyle.Fill


        _host = New ToolStripControlHost(_wrapper)

        'make it take the same size as the popup
        _host.AutoSize = False

        Me.Padding = Padding.Empty
        Me.Margin = Padding.Empty

        _host.Padding = Padding.Empty
        _host.Margin = Padding.Empty

        _child.Location = Point.Empty
        _wrapper.Location = Point.Empty

        Items.Add(_host)

        _wrapper.Banner._HostHandle = Handle
        _wrapper.Banner.Visible = False
    End Sub

    Protected Overrides Function ProcessDialogKey(keyData As Keys) As Boolean
        'prevent alt from closing it and allow alt+menumonic to work
        If (keyData And Keys.Alt) = Keys.Alt Then
            Return False
        End If

        Return MyBase.ProcessDialogKey(keyData)
    End Function

    Public Overloads Sub Show(Owner As Control)
        If Owner Is Nothing Then
            'Throw New ArgumentNullException("control")
            Show()
        Else
            Show(Owner, Owner.ClientRectangle)
        End If

    End Sub

    Private Overloads Sub Show(control As Control, area As Rectangle)
        If control Is Nothing Then
            Throw New ArgumentNullException("control")
        End If

        Dim location As Point = control.PointToScreen(New Point(area.Left, area.Top + area.Height))

        Dim screen__1 As Rectangle = Screen.FromControl(control).WorkingArea

        If location.X + Size.Width > (screen__1.Left + screen__1.Width) Then
            location.X = (screen__1.Left + screen__1.Width) - Size.Width
        End If

        If location.Y + Size.Height > (screen__1.Top + screen__1.Height) Then
            location.Y -= Size.Height + area.Height
        End If

        location = control.PointToClient(location)

        Show(control, location, ToolStripDropDownDirection.BelowRight)
    End Sub

    Protected Overrides Sub OnOpening(e As CancelEventArgs)
        If _child.IsDisposed OrElse _child.Disposing Then
            e.Cancel = True
            Return
        End If
        MyBase.OnOpening(e)
    End Sub

    Protected Overrides Sub OnOpened(e As EventArgs)
        _child.Focus()

        MyBase.OnOpened(e)
    End Sub

End Class


<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Public Class PupChildWrapper
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Friend WithEvents ChildPanel As Panel

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.ChildPanel = New System.Windows.Forms.Panel()
        Me.Banner = New PupBanner()
        Me.SuspendLayout()
        '
        'ChildPanel
        '
        Me.ChildPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ChildPanel.Location = New System.Drawing.Point(0, 16)
        Me.ChildPanel.Name = "ChildPanel"
        Me.ChildPanel.Size = New System.Drawing.Size(300, 200)
        Me.ChildPanel.TabIndex = 1
        '
        'Banner
        '
        Me.Banner.Dock = System.Windows.Forms.DockStyle.Top
        Me.Banner.Location = New System.Drawing.Point(0, 0)
        Me.Banner.Name = "Banner"
        Me.Banner.Size = New System.Drawing.Size(469, 16)
        Me.Banner.TabIndex = 0
        '
        'PupChildWrapper
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit
        Me.BackColor = System.Drawing.SystemColors.ControlLightLight
        Me.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Controls.Add(Me.ChildPanel)
        Me.Controls.Add(Me.Banner)
        Me.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Margin = New System.Windows.Forms.Padding(0)
        Me.Name = "PupChildWrapper"
        Me.Size = New System.Drawing.Size(469, 130)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Banner As PupBanner

    Protected Overrides Function ProcessDialogKey(keyData As Keys) As Boolean
        ' Alt+F4 is to closing
        If (keyData And Keys.Alt) = Keys.Alt Then
            If (keyData And Keys.F4) = Keys.F4 Then
                Me.Parent.Hide()
                Return True
            End If
        End If

        If (keyData And Keys.Enter) = Keys.Enter Then
            If TypeOf Me.ActiveControl Is Button Then
                TryCast(Me.ActiveControl, Button).PerformClick()
                Return True
            End If
        End If

        Return MyBase.ProcessDialogKey(keyData)
    End Function

    Private Sub Banner_CloseClicked(sender As Object, e As EventArgs) Handles Banner.CloseClicked
        Dim obj As Object = DirectCast(Me.Parent, PopupCT2)
        obj.close()
    End Sub

    Private Sub ChildPanel_ControlAdded(sender As Object, e As ControlEventArgs) Handles ChildPanel.ControlAdded

        Dim WrapperHeight = e.Control.Height
        Dim WrapperWidth = e.Control.Width

        If Banner.Visible Then
            WrapperHeight += Banner.Height
        End If

        Size = New Size(WrapperWidth, WrapperHeight)
        e.Control.Dock = DockStyle.Fill
    End Sub

End Class





<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Class PupBanner
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
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
        Me.pbClose = New System.Windows.Forms.PictureBox()
        CType(Me.pbClose, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pbClose
        '
        Me.pbClose.Dock = System.Windows.Forms.DockStyle.Right
        'Me.pbClose.Image = Global.SuperContextMenuVB.My.Resources.Resources.close161
        Me.pbClose.Location = New System.Drawing.Point(293, 0)
        Me.pbClose.Name = "pbClose"
        Me.pbClose.Size = New System.Drawing.Size(16, 16)
        Me.pbClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.pbClose.TabIndex = 3
        Me.pbClose.TabStop = False
        '
        'PupBanner
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.pbClose)
        Me.Name = "PupBanner"
        Me.BackColor = Color.LightGray
        Me.Size = New System.Drawing.Size(309, 16)
        CType(Me.pbClose, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents pbClose As System.Windows.Forms.PictureBox






#Region "Mousedown: Move & Resize Contants"
    Friend Declare Sub ReleaseCapture Lib "user32" ()
    Friend Const WM_NCLBUTTONDOWN As Integer = &HA1
    Friend Const HTBORDER As Integer = 18
    Friend Const HTBOTTOM As Integer = 15
    Friend Const HTBOTTOMLEFT As Integer = 16
    Friend Const HTBOTTOMRIGHT As Integer = 17
    Friend Const HTCAPTION As Integer = 2
    Friend Const HTLEFT As Integer = 10
    Friend Const HTRIGHT As Integer = 11
    Friend Const HTTOP As Integer = 12
    Friend Const HTTOPLEFT As Integer = 13
    Friend Const HTTOPRIGHT As Integer = 14
#End Region


    Friend _AllowDragging As Boolean = False
    Friend _ShowCloseButton As Boolean = False
    Friend _HostHandle As IntPtr = Nothing

    Public Event CloseClicked(ByVal sender As System.Object, ByVal e As System.EventArgs)
    Private Sub pbClose_Click(sender As Object, e As EventArgs) Handles pbClose.Click
        RaiseEvent CloseClicked(Me, New EventArgs)
    End Sub


    'Allow Dragging by Banner Object
    Private Sub BannerMouseMove(sender As Object, e As MouseEventArgs) Handles Me.MouseMove
        If e.Button = System.Windows.Forms.MouseButtons.Left Then
            If _AllowDragging Then
                Me.Capture = False
                Dim msg As Message

                If _HostHandle = Nothing Then
                    msg = Message.Create(Me.ParentForm.Handle, WM_NCLBUTTONDOWN, New IntPtr(HTCAPTION), IntPtr.Zero)
                Else
                    msg = Message.Create(_HostHandle, WM_NCLBUTTONDOWN, New IntPtr(HTCAPTION), IntPtr.Zero)
                End If

                Me.DefWndProc(msg)

                ReleaseCapture()
            End If
        End If
    End Sub

End Class
