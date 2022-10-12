<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ucRowOption
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ucRowOption))
        Me.txtName = New System.Windows.Forms.TextBox()
        Me.txtText = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.cbDefault = New System.Windows.Forms.CheckBox()
        Me.cbEnabled = New System.Windows.Forms.CheckBox()
        Me.cbVisible = New System.Windows.Forms.CheckBox()
        Me.rbAny = New System.Windows.Forms.RadioButton()
        Me.rbDisplay = New System.Windows.Forms.RadioButton()
        Me.rbUpdate = New System.Windows.Forms.RadioButton()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.btnApply = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButton1 = New System.Windows.Forms.ToolStripButton()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.nudSeq = New System.Windows.Forms.NumericUpDown()
        Me.ToolStrip1.SuspendLayout()
        CType(Me.nudSeq, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtName
        '
        Me.txtName.Location = New System.Drawing.Point(113, 58)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(141, 20)
        Me.txtName.TabIndex = 1
        '
        'txtText
        '
        Me.txtText.Location = New System.Drawing.Point(113, 84)
        Me.txtText.Name = "txtText"
        Me.txtText.Size = New System.Drawing.Size(174, 20)
        Me.txtText.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(14, 39)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(26, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Seq"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(14, 65)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(35, 13)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Name"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(14, 91)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(28, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Text"
        '
        'cbDefault
        '
        Me.cbDefault.AutoSize = True
        Me.cbDefault.Location = New System.Drawing.Point(113, 125)
        Me.cbDefault.Name = "cbDefault"
        Me.cbDefault.Size = New System.Drawing.Size(60, 17)
        Me.cbDefault.TabIndex = 8
        Me.cbDefault.Text = "Default"
        Me.cbDefault.UseVisualStyleBackColor = True
        '
        'cbEnabled
        '
        Me.cbEnabled.AutoSize = True
        Me.cbEnabled.Checked = True
        Me.cbEnabled.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbEnabled.Location = New System.Drawing.Point(113, 148)
        Me.cbEnabled.Name = "cbEnabled"
        Me.cbEnabled.Size = New System.Drawing.Size(65, 17)
        Me.cbEnabled.TabIndex = 10
        Me.cbEnabled.Text = "Enabled"
        Me.cbEnabled.UseVisualStyleBackColor = True
        '
        'cbVisible
        '
        Me.cbVisible.AutoSize = True
        Me.cbVisible.Checked = True
        Me.cbVisible.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbVisible.Location = New System.Drawing.Point(113, 171)
        Me.cbVisible.Name = "cbVisible"
        Me.cbVisible.Size = New System.Drawing.Size(56, 17)
        Me.cbVisible.TabIndex = 12
        Me.cbVisible.Text = "Visible"
        Me.cbVisible.UseVisualStyleBackColor = True
        '
        'rbAny
        '
        Me.rbAny.AutoSize = True
        Me.rbAny.Location = New System.Drawing.Point(113, 212)
        Me.rbAny.Name = "rbAny"
        Me.rbAny.Size = New System.Drawing.Size(43, 17)
        Me.rbAny.TabIndex = 13
        Me.rbAny.TabStop = True
        Me.rbAny.Text = "Any"
        Me.rbAny.UseVisualStyleBackColor = True
        '
        'rbDisplay
        '
        Me.rbDisplay.AutoSize = True
        Me.rbDisplay.Location = New System.Drawing.Point(162, 212)
        Me.rbDisplay.Name = "rbDisplay"
        Me.rbDisplay.Size = New System.Drawing.Size(59, 17)
        Me.rbDisplay.TabIndex = 14
        Me.rbDisplay.TabStop = True
        Me.rbDisplay.Text = "Display"
        Me.rbDisplay.UseVisualStyleBackColor = True
        '
        'rbUpdate
        '
        Me.rbUpdate.AutoSize = True
        Me.rbUpdate.Location = New System.Drawing.Point(227, 212)
        Me.rbUpdate.Name = "rbUpdate"
        Me.rbUpdate.Size = New System.Drawing.Size(60, 17)
        Me.rbUpdate.TabIndex = 15
        Me.rbUpdate.TabStop = True
        Me.rbUpdate.Text = "Update"
        Me.rbUpdate.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(14, 214)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(71, 13)
        Me.Label4.TabIndex = 16
        Me.Label4.Text = "Display Mode"
        '
        'ToolStrip1
        '
        Me.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnApply, Me.ToolStripButton1})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(440, 25)
        Me.ToolStrip1.TabIndex = 17
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'btnApply
        '
        Me.btnApply.Image = CType(resources.GetObject("btnApply.Image"), System.Drawing.Image)
        Me.btnApply.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnApply.Name = "btnApply"
        Me.btnApply.Size = New System.Drawing.Size(58, 22)
        Me.btnApply.Text = "Apply"
        '
        'ToolStripButton1
        '
        Me.ToolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton1.Image = CType(resources.GetObject("ToolStripButton1.Image"), System.Drawing.Image)
        Me.ToolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton1.Name = "ToolStripButton1"
        Me.ToolStripButton1.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButton1.Text = "ToolStripButton1"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(14, 246)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(60, 13)
        Me.Label5.TabIndex = 18
        Me.Label5.Text = "Permission:"
        '
        'nudSeq
        '
        Me.nudSeq.Location = New System.Drawing.Point(113, 32)
        Me.nudSeq.Name = "nudSeq"
        Me.nudSeq.Size = New System.Drawing.Size(66, 20)
        Me.nudSeq.TabIndex = 23
        '
        'ucRowOption
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.nudSeq)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.rbUpdate)
        Me.Controls.Add(Me.rbDisplay)
        Me.Controls.Add(Me.rbAny)
        Me.Controls.Add(Me.cbVisible)
        Me.Controls.Add(Me.cbEnabled)
        Me.Controls.Add(Me.cbDefault)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtText)
        Me.Controls.Add(Me.txtName)
        Me.Name = "ucRowOption"
        Me.Size = New System.Drawing.Size(440, 325)
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        CType(Me.nudSeq, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtName As TextBox
    Friend WithEvents txtText As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents cbDefault As CheckBox
    Friend WithEvents cbEnabled As CheckBox
    Friend WithEvents cbVisible As CheckBox
    Friend WithEvents rbAny As RadioButton
    Friend WithEvents rbDisplay As RadioButton
    Friend WithEvents rbUpdate As RadioButton
    Friend WithEvents Label4 As Label
    Friend WithEvents ToolStrip1 As ToolStrip
    Friend WithEvents btnApply As ToolStripButton
    Friend WithEvents Label5 As Label
    Friend WithEvents nudSeq As NumericUpDown
    Friend WithEvents ToolStripButton1 As ToolStripButton
End Class
