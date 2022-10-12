Friend Class CT2Dialog
    Private ResizeDrag As Integer
    Private Resizing As Boolean
    Private Loaded As Boolean
    Private PrimaryColor As Color = Color.FromArgb(2, 62, 125)

    Private gTitle As String = "CTS Dialog"
    <System.ComponentModel.Category("_Custom Properties")>
    <System.ComponentModel.Description("Dialog Title")>
    Public Property Title() As String
        Get
            Return gTitle
        End Get
        Set(ByVal value As String)
            gTitle = value.Trim
            If frm IsNot Nothing Then
                frm.Text = value.Trim
            End If
        End Set
    End Property

    Private gShowClose As Boolean = True
    <System.ComponentModel.Category("_Custom Properties")>
    <System.ComponentModel.Description("Show the Close Button")>
    Public Property ShowClose() As Boolean
        Get
            Return gShowClose
        End Get
        Set(ByVal value As Boolean)
            gShowClose = value
            btnClose.Visible = value
        End Set
    End Property

    Private gShowStatus As Boolean = True
    <System.ComponentModel.Category("_Custom Properties")>
    <System.ComponentModel.Description("Show the Status Strip")>
    Public Property ShowStatus() As Boolean
        Get
            Return gShowStatus
        End Get
        Set(ByVal value As Boolean)
            gShowStatus = value
            DialogStatus.Visible = value
        End Set
    End Property

    Private gAllowResize As Boolean = False
    <System.ComponentModel.Category("_Custom Properties")>
    <System.ComponentModel.Description("Allow the dialog to be resized")>
    Public Property AllowResize() As Boolean
        Get
            Return gAllowResize
        End Get
        Set(ByVal value As Boolean)
            gAllowResize = value
            DialogStatus.SizingGrip = value
        End Set
    End Property

    Private gControl As UserControl
    <System.ComponentModel.Category("_Custom Properties")>
    <System.ComponentModel.Description("Dialog Controls")>
    Public Property Control() As UserControl
        Get
            Return gControl
        End Get
        Set(ByVal value As UserControl)
            gControl = value
            If gShowStatus Then
                Me.Size = New Size(value.Width + Me.Padding.Left + Me.Padding.Right,
                                   value.Height + pnlBanner.Height + DialogStatus.Height + Me.Padding.Top + Me.Padding.Bottom)
            Else
                Me.Size = New Size(value.Width + Me.Padding.Left + Me.Padding.Right,
                                   value.Height + pnlBanner.Height + Me.Padding.Top + Me.Padding.Bottom)
            End If

        End Set
    End Property

    Private gSize As Size
    <System.ComponentModel.Category("_Custom Properties")>
    <System.ComponentModel.Description("Dialog Size")>
    Public Overloads Property [Size]() As Size
        Get
            Return gSize
        End Get
        Set(ByVal value As Size)
            gSize = value
        End Set
    End Property

    Private gLocation As Point = Nothing
    <System.ComponentModel.Category("_Custom Properties")>
    <System.ComponentModel.Description("Dialog location")>
    Public Overloads Property [Location]() As Point
        Get
            Return gLocation
        End Get
        Set(ByVal value As Point)
            gLocation = value
        End Set
    End Property

    Private gFocusColor As Color = PrimaryColor
    <System.ComponentModel.Category("Appearance")>
    <System.ComponentModel.Description("Form Color when focused")>
    <System.ComponentModel.Browsable(True)>
    Public Property FocusColor As Color
        Get
            Return gFocusColor
        End Get
        Set(value As Color)
            If value = Color.Empty Then
                gFocusColor = PrimaryColor
            Else
                gFocusColor = value
                frm.BackColor = value
                DialogStatus.BackColor = value
            End If
        End Set
    End Property

    Private gFocusForeColor As Color = Color.White
    <System.ComponentModel.Category("Appearance")>
    <System.ComponentModel.Description("ForeColor when focused")>
    <System.ComponentModel.Browsable(True)>
    Public Property FocusForeColor As Color
        Get
            Return gFocusForeColor
        End Get
        Set(value As Color)
            If value = Color.Empty Then
                gFocusForeColor = Me.ForeColor
            Else
                gFocusForeColor = value
                pnlTitle.ForeColor = value
                DialogStatus.ForeColor = value
            End If
        End Set
    End Property

    Private gWindowPosition As CTS.UI.WindowPosition
    <System.ComponentModel.Category("_Custom Properties")>
    <System.ComponentModel.Description("Startup Position")>
    Public Overloads Property WindowPosition() As CTS.UI.WindowPosition
        Get
            Return gWindowPosition
        End Get
        Set(ByVal value As CTS.UI.WindowPosition)
            gWindowPosition = value
        End Set
    End Property

    'Private gDialogResult As DialogResult
    <System.ComponentModel.Category("_Custom Properties")>
    <System.ComponentModel.Description("DialogResult")>
    Public ReadOnly Property DialogResult() As DialogResult
        Get
            If frm IsNot Nothing Then
                Return frm.DialogResult
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private frm As Form
    <System.ComponentModel.Category("_Custom Properties")>
    <System.ComponentModel.Description("Dialog Window/Form")>
    Public ReadOnly Property Form() As Form
        Get
            Return frm
        End Get
    End Property

    <System.ComponentModel.Category("_Custom Properties")>
    <System.ComponentModel.Description("Dialog Window/Form")>
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)>
    Public Property Owner As IWin32Window
        Get
            Return frm.Owner
        End Get
        Set(value As IWin32Window)
            frm.Owner = value
        End Set
    End Property

    Public Sub New()
        InitializeComponent()

        frm = New Form
        frm.FormBorderStyle = FormBorderStyle.None
        frm.Text = gTitle
        frm.KeyPreview = True

        AddHandler frm.Activated, AddressOf FormActivated
        AddHandler frm.Deactivate, AddressOf FormDeactivated
        AddHandler frm.FormClosing, AddressOf FormClosing
        AddHandler frm.Shown, AddressOf Shown
        AddHandler frm.KeyUp, AddressOf FormKeyup
        'Initializing = False
    End Sub

    Private Sub Dialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        StatusLabel.Text = String.Empty
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        frm.Close()
    End Sub

    Public Overloads Sub Show()
        Try
            If Not Loaded Then
                Loaded = True

                lblTitle.Text = Me.Title
                frm.Size = Me.Size
                frm.Visible = False

                If frm.Owner Is Nothing Then
                    If Me.FindForm IsNot Nothing Then
                        frm.Owner = Me.FindForm
                    End If
                End If

                If Me.Location = Nothing Then
                    CTS.UI.SetFormLocation(frm, Me.WindowPosition)
                    frm.StartPosition = FormStartPosition.Manual
                    'frm.Location = New Point(0, 0)
                    'frm.StartPosition = FormStartPosition.CenterParent
                Else
                    frm.Location = Me.Location
                    frm.StartPosition = FormStartPosition.Manual
                End If

                frm.KeyPreview = True

                pnlContent.Controls.Add(Me.Control)
                Me.Dock = DockStyle.Fill

                frm.Controls.Add(Me)
                Me.Control.Dock = DockStyle.Fill

                pnlBanner.SendToBack()
                Me.Control.BringToFront()
            End If

            frm.Show()
            frm.Activate()

        Catch ex As Exception
            Debug.Print(ex.Message)
            Try
                frm.DialogResult = DialogResult.Abort
            Catch
            End Try
        End Try

    End Sub
    Public Function ShowDialog() As DialogResult

        Try
            lblTitle.Text = Me.Title
            frm.Size = Me.Size
            frm.Visible = False

            If frm.Owner Is Nothing Then
                If Me.FindForm IsNot Nothing Then
                    frm.Owner = Me.FindForm
                End If
            End If

            If Me.Location = Nothing Then
                CTS.UI.SetFormLocation(frm, Me.WindowPosition)
                frm.StartPosition = FormStartPosition.Manual
                'frm.Location = New Point(0, 0)
                'frm.StartPosition = FormStartPosition.CenterParent
            Else
                frm.Location = Me.Location
                frm.StartPosition = FormStartPosition.Manual
            End If

            frm.KeyPreview = True
            'frm.Padding = New Padding(0, 0, 0, 10)
            'frm.SizeGripStyle = SizeGripStyle.Show

            pnlContent.Controls.Add(Me.Control)
            Me.Dock = DockStyle.Fill

            frm.Controls.Add(Me)
            Me.Control.Dock = DockStyle.Fill

            pnlBanner.SendToBack()
            Me.Control.BringToFront()

            frm.ShowDialog()

            Return frm.DialogResult

        Catch ex As Exception
            Debug.Print(ex.Message)
            Return DialogResult.Abort
        End Try

    End Function

    Private Sub FormActivated(sender As Object, e As EventArgs)
        FocusTasks()
    End Sub

    Private Sub FocusTasks()
        frm.BackColor = gFocusColor
        DialogStatus.BackColor = gFocusColor
        pnlTitle.ForeColor = gFocusForeColor
        DialogStatus.ForeColor = gFocusForeColor
    End Sub

    Private Sub FormDeactivated(sender As Object, e As EventArgs)
        LostFocusTasks()
    End Sub

    Private Sub LostFocusTasks()
        frm.BackColor = SystemColors.InactiveCaption
        DialogStatus.BackColor = SystemColors.InactiveCaption
        pnlTitle.ForeColor = SystemColors.InactiveCaptionText
        DialogStatus.ForeColor = SystemColors.InactiveCaptionText
    End Sub

    Public Event DialogClosing(sender As Object, e As FormClosingEventArgs)
    Private Sub FormClosing(sender As Object, e As FormClosingEventArgs)
        RaiseEvent DialogClosing(sender, e)
    End Sub

    Public Event DialogShown(sender As Object, e As EventArgs)
    Private Sub Shown(sender As Object, e As EventArgs)
        RaiseEvent DialogShown(sender, e)
    End Sub

    Public Event EnterPressed(sender As Object, e As EventArgs)
    Private Sub FormKeyup(sender As Object, e As KeyEventArgs)
        If e.KeyCode = Keys.Enter Then
            RaiseEvent EnterPressed(sender, e)
        End If
    End Sub



    'Allow Dragging by Banner (panels) 
    Private Sub BannerMouseMove(sender As Object, e As MouseEventArgs) Handles lblTitle.MouseMove, pnlTitle.MouseMove, pnlBanner.MouseMove
        If e.Button = System.Windows.Forms.MouseButtons.Left Then
            sender.Capture = False
            Dim msg As System.Windows.Forms.Message = System.Windows.Forms.Message.Create(Me.ParentForm.Handle, WM_NCLBUTTONDOWN, New IntPtr(HTCAPTION), IntPtr.Zero)
            Me.DefWndProc(msg)

            ReleaseCapture()
        End If
    End Sub



#Region "Resizing/Maximize/Minimize"
    Private Sub Maximize_DoubleClick(sender As Object, e As MouseEventArgs) Handles lblTitle.DoubleClick, pnlTitle.DoubleClick
        If gAllowResize Then
            Me.ParentForm.WindowState = IIf(Me.ParentForm.WindowState = FormWindowState.Maximized,
                                        FormWindowState.Normal, FormWindowState.Maximized)
        End If
    End Sub

    Private Sub ResizeSelectorForm()
        If gAllowResize Then
            Resizing = True
            ReleaseCapture()
            Dim msg As Message = Message.Create(Me.ParentForm.Handle, WM_NCLBUTTONDOWN, New IntPtr(ResizeDrag), IntPtr.Zero)
            Me.DefWndProc(msg)

            Resizing = False
            ResizeDrag = 0
            Me.ParentForm.Cursor = Cursors.Default
        End If
    End Sub

    Private Sub Dialog_MouseDown(sender As Object, e As MouseEventArgs) Handles MyBase.MouseDown, DialogStatus.MouseDown
        If gAllowResize Then
            ResizeSelectorForm()
        End If
    End Sub

    Private Sub Dialog_MouseHover(sender As Object, e As EventArgs) Handles MyBase.MouseHover, DialogStatus.MouseHover

        If gAllowResize Then
            Dim p As Point = MyBase.PointToClient(Cursor.Position)

            'top left 
            If p.X <= 20 And p.Y < 20 Then
                Me.ParentForm.Cursor = Cursors.SizeNWSE
                ResizeDrag = HTTOPLEFT

                'bottom right
            ElseIf p.X >= MyBase.Width - 20 And p.Y >= MyBase.Height - 20 Then
                Me.ParentForm.Cursor = Cursors.SizeNWSE
                ResizeDrag = HTBOTTOMRIGHT

                'top right
            ElseIf p.X >= MyBase.Width - 20 And p.Y < 20 Then
                Me.ParentForm.Cursor = Cursors.SizeNESW
                ResizeDrag = HTTOPRIGHT

                'bottom left
            ElseIf p.X <= 20 And p.Y >= MyBase.Height - 20 Then
                Me.ParentForm.Cursor = Cursors.SizeNESW
                ResizeDrag = HTBOTTOMLEFT

                'left 
            ElseIf p.X <= 5 Then
                Me.ParentForm.Cursor = Cursors.SizeWE
                ResizeDrag = HTLEFT

                'right 
            ElseIf p.X >= MyBase.Width - 5 Then
                Me.ParentForm.Cursor = Cursors.SizeWE
                ResizeDrag = HTRIGHT

                'top
            ElseIf p.Y < 5 Then
                Me.ParentForm.Cursor = Cursors.SizeNS
                ResizeDrag = HTTOP

                'bottom
            ElseIf p.Y >= MyBase.Height - 5 Then
                Me.ParentForm.Cursor = Cursors.SizeNS
                ResizeDrag = HTBOTTOM

            Else
                ResizeDrag = 0
            End If
        End If

    End Sub

    Private Sub Dialog_MouseLeave(sender As Object, e As EventArgs) Handles MyBase.MouseLeave, DialogStatus.MouseLeave
        If gAllowResize Then
            Me.ParentForm.Cursor = Cursors.Default
            ResizeDrag = 0
        End If
    End Sub

#End Region

End Class
