<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class CT2Dialog
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(CT2Dialog))
        Me.pnlBanner = New System.Windows.Forms.Panel()
        Me.pnlTitle = New System.Windows.Forms.Panel()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.pnlContent = New System.Windows.Forms.Panel()
        Me.DialogStatus = New System.Windows.Forms.StatusStrip()
        Me.StatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.pnlBanner.SuspendLayout()
        Me.pnlTitle.SuspendLayout()
        Me.DialogStatus.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlBanner
        '
        Me.pnlBanner.Controls.Add(Me.pnlTitle)
        Me.pnlBanner.Controls.Add(Me.btnClose)
        Me.pnlBanner.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlBanner.Location = New System.Drawing.Point(3, 3)
        Me.pnlBanner.Name = "pnlBanner"
        Me.pnlBanner.Padding = New System.Windows.Forms.Padding(0, 0, 0, 2)
        Me.pnlBanner.Size = New System.Drawing.Size(488, 23)
        Me.pnlBanner.TabIndex = 98
        '
        'pnlTitle
        '
        Me.pnlTitle.Controls.Add(Me.lblTitle)
        Me.pnlTitle.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlTitle.Location = New System.Drawing.Point(0, 0)
        Me.pnlTitle.Name = "pnlTitle"
        Me.pnlTitle.Size = New System.Drawing.Size(463, 21)
        Me.pnlTitle.TabIndex = 2
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(3, 4)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(32, 13)
        Me.lblTitle.TabIndex = 1
        Me.lblTitle.Text = "Title"
        '
        'btnClose
        '
        Me.btnClose.Dock = System.Windows.Forms.DockStyle.Right
        Me.btnClose.FlatAppearance.BorderSize = 0
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Image = CType(resources.GetObject("btnClose.Image"), System.Drawing.Image)
        Me.btnClose.Location = New System.Drawing.Point(463, 0)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(25, 21)
        Me.btnClose.TabIndex = 0
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'pnlContent
        '
        Me.pnlContent.BackColor = System.Drawing.Color.WhiteSmoke
        Me.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlContent.Location = New System.Drawing.Point(3, 26)
        Me.pnlContent.Name = "pnlContent"
        Me.pnlContent.Size = New System.Drawing.Size(488, 232)
        Me.pnlContent.TabIndex = 99
        '
        'DialogStatus
        '
        Me.DialogStatus.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DialogStatus.GripMargin = New System.Windows.Forms.Padding(1)
        Me.DialogStatus.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible
        Me.DialogStatus.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StatusLabel})
        Me.DialogStatus.Location = New System.Drawing.Point(3, 258)
        Me.DialogStatus.Name = "DialogStatus"
        Me.DialogStatus.Size = New System.Drawing.Size(488, 22)
        Me.DialogStatus.TabIndex = 100
        Me.DialogStatus.Text = "StatusStrip1"
        '
        'StatusLabel
        '
        Me.StatusLabel.Name = "StatusLabel"
        Me.StatusLabel.Size = New System.Drawing.Size(63, 17)
        Me.StatusLabel.Text = "StatusLabel"
        '
        'CTSDialog2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.pnlContent)
        Me.Controls.Add(Me.DialogStatus)
        Me.Controls.Add(Me.pnlBanner)
        Me.Name = "CTSDialog2"
        Me.Padding = New System.Windows.Forms.Padding(3)
        Me.Size = New System.Drawing.Size(494, 283)
        Me.pnlBanner.ResumeLayout(False)
        Me.pnlTitle.ResumeLayout(False)
        Me.pnlTitle.PerformLayout()
        Me.DialogStatus.ResumeLayout(False)
        Me.DialogStatus.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents pnlBanner As Panel
    Friend WithEvents pnlTitle As Panel
    Friend WithEvents lblTitle As Label
    Friend WithEvents btnClose As Button
    Friend WithEvents pnlContent As Panel
    Public WithEvents DialogStatus As StatusStrip
    Public WithEvents StatusLabel As ToolStripStatusLabel
End Class
