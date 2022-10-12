<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ucStyleCopy
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ucStyle))
        Me.txtName = New System.Windows.Forms.TextBox()
        Me.txtTestValue = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lblApply = New System.Windows.Forms.Label()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.btnApply = New System.Windows.Forms.ToolStripButton()
        Me.rbRow = New System.Windows.Forms.RadioButton()
        Me.rbCell = New System.Windows.Forms.RadioButton()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ColorDialog1 = New System.Windows.Forms.ColorDialog()
        Me.lblBackColor = New System.Windows.Forms.Label()
        Me.lblForeColor = New System.Windows.Forms.Label()
        Me.cbOperator = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.cbTestColumn = New System.Windows.Forms.ComboBox()
        Me.cbApplyColumn = New System.Windows.Forms.ComboBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.ceBackground = New ColorEntry()
        Me.ceForeground = New ColorEntry()
        Me.ToolStrip1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtName
        '
        Me.txtName.Location = New System.Drawing.Point(92, 50)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(183, 20)
        Me.txtName.TabIndex = 1
        '
        'txtTestValue
        '
        Me.txtTestValue.Location = New System.Drawing.Point(85, 81)
        Me.txtTestValue.Name = "txtTestValue"
        Me.txtTestValue.Size = New System.Drawing.Size(141, 20)
        Me.txtTestValue.TabIndex = 2
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(18, 53)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(35, 13)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Name"
        '
        'lblApply
        '
        Me.lblApply.AutoSize = True
        Me.lblApply.Location = New System.Drawing.Point(17, 58)
        Me.lblApply.Name = "lblApply"
        Me.lblApply.Size = New System.Drawing.Size(45, 13)
        Me.lblApply.TabIndex = 6
        Me.lblApply.Text = "Column:"
        '
        'ToolStrip1
        '
        Me.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnApply})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(339, 25)
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
        'rbRow
        '
        Me.rbRow.AutoSize = True
        Me.rbRow.Location = New System.Drawing.Point(85, 27)
        Me.rbRow.Name = "rbRow"
        Me.rbRow.Size = New System.Drawing.Size(47, 17)
        Me.rbRow.TabIndex = 22
        Me.rbRow.TabStop = True
        Me.rbRow.Text = "Row"
        Me.rbRow.UseVisualStyleBackColor = True
        '
        'rbCell
        '
        Me.rbCell.AutoSize = True
        Me.rbCell.Location = New System.Drawing.Point(138, 27)
        Me.rbCell.Name = "rbCell"
        Me.rbCell.Size = New System.Drawing.Size(42, 17)
        Me.rbCell.TabIndex = 23
        Me.rbCell.TabStop = True
        Me.rbCell.Text = "Cell"
        Me.rbCell.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(17, 29)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(34, 13)
        Me.Label1.TabIndex = 24
        Me.Label1.Text = "Type:"
        '
        'lblBackColor
        '
        Me.lblBackColor.AutoSize = True
        Me.lblBackColor.Location = New System.Drawing.Point(19, 38)
        Me.lblBackColor.Name = "lblBackColor"
        Me.lblBackColor.Size = New System.Drawing.Size(95, 13)
        Me.lblBackColor.TabIndex = 26
        Me.lblBackColor.Text = "Background Color:"
        '
        'lblForeColor
        '
        Me.lblForeColor.AutoSize = True
        Me.lblForeColor.Location = New System.Drawing.Point(19, 68)
        Me.lblForeColor.Name = "lblForeColor"
        Me.lblForeColor.Size = New System.Drawing.Size(91, 13)
        Me.lblForeColor.TabIndex = 29
        Me.lblForeColor.Text = "Foreground Color:"
        '
        'cbOperator
        '
        Me.cbOperator.FormattingEnabled = True
        Me.cbOperator.Location = New System.Drawing.Point(85, 48)
        Me.cbOperator.Name = "cbOperator"
        Me.cbOperator.Size = New System.Drawing.Size(139, 21)
        Me.cbOperator.TabIndex = 40
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(13, 23)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(45, 13)
        Me.Label3.TabIndex = 41
        Me.Label3.Text = "Column:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(13, 56)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(51, 13)
        Me.Label4.TabIndex = 42
        Me.Label4.Text = "Operator:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(13, 84)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(34, 13)
        Me.Label5.TabIndex = 43
        Me.Label5.Text = "Value"
        '
        'cbTestColumn
        '
        Me.cbTestColumn.FormattingEnabled = True
        Me.cbTestColumn.Location = New System.Drawing.Point(85, 19)
        Me.cbTestColumn.Name = "cbTestColumn"
        Me.cbTestColumn.Size = New System.Drawing.Size(139, 21)
        Me.cbTestColumn.TabIndex = 44
        '
        'cbApplyColumn
        '
        Me.cbApplyColumn.FormattingEnabled = True
        Me.cbApplyColumn.Location = New System.Drawing.Point(85, 55)
        Me.cbApplyColumn.Name = "cbApplyColumn"
        Me.cbApplyColumn.Size = New System.Drawing.Size(139, 21)
        Me.cbApplyColumn.TabIndex = 45
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.txtTestValue)
        Me.GroupBox1.Controls.Add(Me.cbOperator)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.cbTestColumn)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Location = New System.Drawing.Point(21, 90)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(254, 123)
        Me.GroupBox1.TabIndex = 46
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Test"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.lblApply)
        Me.GroupBox2.Controls.Add(Me.cbApplyColumn)
        Me.GroupBox2.Controls.Add(Me.rbRow)
        Me.GroupBox2.Controls.Add(Me.rbCell)
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Location = New System.Drawing.Point(21, 233)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(254, 85)
        Me.GroupBox2.TabIndex = 47
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Apply To"
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.ceBackground)
        Me.GroupBox3.Controls.Add(Me.lblBackColor)
        Me.GroupBox3.Controls.Add(Me.lblForeColor)
        Me.GroupBox3.Controls.Add(Me.ceForeground)
        Me.GroupBox3.Location = New System.Drawing.Point(21, 342)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(254, 100)
        Me.GroupBox3.TabIndex = 48
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Treatment"
        '
        'ceBackground
        '
        Me.ceBackground.BackColor = System.Drawing.Color.White
        Me.ceBackground.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ceBackground.Location = New System.Drawing.Point(118, 31)
        Me.ceBackground.Name = "ceBackground"
        Me.ceBackground.Padding = New System.Windows.Forms.Padding(2, 2, 0, 2)
        Me.ceBackground.Size = New System.Drawing.Size(130, 21)
        Me.ceBackground.TabIndex = 37
        Me.ceBackground.value = System.Drawing.Color.Empty
        '
        'ceForeground
        '
        Me.ceForeground.BackColor = System.Drawing.Color.White
        Me.ceForeground.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ceForeground.Location = New System.Drawing.Point(118, 58)
        Me.ceForeground.Name = "ceForeground"
        Me.ceForeground.Padding = New System.Windows.Forms.Padding(2, 2, 0, 2)
        Me.ceForeground.Size = New System.Drawing.Size(130, 21)
        Me.ceForeground.TabIndex = 38
        Me.ceForeground.value = System.Drawing.Color.Empty
        '
        'ucStyle
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtName)
        Me.Name = "ucStyle"
        Me.Size = New System.Drawing.Size(339, 469)
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtName As TextBox
    Friend WithEvents txtTestValue As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents lblApply As Label
    Friend WithEvents ToolStrip1 As ToolStrip
    Friend WithEvents btnApply As ToolStripButton
    Friend WithEvents rbRow As RadioButton
    Friend WithEvents rbCell As RadioButton
    Friend WithEvents Label1 As Label
    Friend WithEvents ColorDialog1 As ColorDialog
    Friend WithEvents lblBackColor As Label
    Friend WithEvents lblForeColor As Label
    Friend WithEvents ceBackground As ColorEntry
    Friend WithEvents ceForeground As ColorEntry
    Friend WithEvents cbOperator As ComboBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents cbTestColumn As ComboBox
    Friend WithEvents cbApplyColumn As ComboBox
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents GroupBox3 As GroupBox
End Class
