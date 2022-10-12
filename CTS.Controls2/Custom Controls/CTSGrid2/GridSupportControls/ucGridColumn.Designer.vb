<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ucGridColumn
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ucGridColumn))
        Me.CT2Input1 = New CT2Input()
        Me.inpFunction = New CT2Input()
        Me.inpColumnType = New CT2Input()
        Me.inpHidden = New CT2Input()
        Me.inpRestrict = New CT2Input()
        Me.inpWidth = New CT2Input()
        Me.inpSortOrder = New CT2Input()
        Me.inpSortSeq = New CT2Input()
        Me.inpSeq = New CT2Input()
        Me.inpHeaderText = New CT2Input()
        Me.inpText = New CT2Input()
        Me.inpTable = New CT2Input()
        Me.inpField = New CT2Input()
        Me.inpName = New CT2Input()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.ToolStripButton1 = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButton2 = New System.Windows.Forms.ToolStripButton()
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'CT2Input1
        '
        Me.CT2Input1.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange
        Me.CT2Input1.BackColor = System.Drawing.SystemColors.Control
        Me.CT2Input1.Dock = System.Windows.Forms.DockStyle.Top
        Me.CT2Input1.EditType = CT2Input.EditTypes.DateTime
        Me.CT2Input1.LabelText = "date test:"
        Me.CT2Input1.LabelWidth = 80
        Me.CT2Input1.Location = New System.Drawing.Point(5, 404)
        Me.CT2Input1.Name = "CT2Input1"
        Me.CT2Input1.Padding = New System.Windows.Forms.Padding(0, 3, 0, 3)
        Me.CT2Input1.ReadOnly = False
        Me.CT2Input1.Size = New System.Drawing.Size(508, 22)
        Me.CT2Input1.TabIndex = 14
        Me.CT2Input1.TextLines = 1
        Me.CT2Input1.TextWidth = 100
        Me.CT2Input1.Value = Nothing
        '
        'inpFunction
        '
        Me.inpFunction.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange
        Me.inpFunction.BackColor = System.Drawing.SystemColors.Control
        Me.inpFunction.Dock = System.Windows.Forms.DockStyle.Top
        Me.inpFunction.EditType = CT2Input.EditTypes.Text
        Me.inpFunction.LabelText = "Function"
        Me.inpFunction.LabelWidth = 80
        Me.inpFunction.Location = New System.Drawing.Point(5, 294)
        Me.inpFunction.Name = "inpFunction"
        Me.inpFunction.Padding = New System.Windows.Forms.Padding(0, 3, 0, 3)
        Me.inpFunction.ReadOnly = False
        Me.inpFunction.Size = New System.Drawing.Size(508, 110)
        Me.inpFunction.TabIndex = 13
        Me.inpFunction.TextLines = 7
        Me.inpFunction.TextWidth = 300
        Me.inpFunction.Value = Nothing
        '
        'inpColumnType
        '
        Me.inpColumnType.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange
        Me.inpColumnType.BackColor = System.Drawing.SystemColors.Control
        Me.inpColumnType.Dock = System.Windows.Forms.DockStyle.Top
        Me.inpColumnType.EditType = CT2Input.EditTypes.Text
        Me.inpColumnType.LabelText = "Column Type:"
        Me.inpColumnType.LabelWidth = 80
        Me.inpColumnType.Location = New System.Drawing.Point(5, 272)
        Me.inpColumnType.Name = "inpColumnType"
        Me.inpColumnType.Padding = New System.Windows.Forms.Padding(0, 3, 0, 3)
        Me.inpColumnType.ReadOnly = False
        Me.inpColumnType.Size = New System.Drawing.Size(508, 22)
        Me.inpColumnType.TabIndex = 12
        Me.inpColumnType.TextLines = 1
        Me.inpColumnType.TextWidth = 100
        Me.inpColumnType.Value = Nothing
        '
        'inpHidden
        '
        Me.inpHidden.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange
        Me.inpHidden.BackColor = System.Drawing.SystemColors.Control
        Me.inpHidden.Dock = System.Windows.Forms.DockStyle.Top
        Me.inpHidden.EditType = CT2Input.EditTypes.Check
        Me.inpHidden.LabelText = "Hidden"
        Me.inpHidden.LabelWidth = 80
        Me.inpHidden.Location = New System.Drawing.Point(5, 250)
        Me.inpHidden.Name = "inpHidden"
        Me.inpHidden.Padding = New System.Windows.Forms.Padding(0, 3, 0, 3)
        Me.inpHidden.ReadOnly = False
        Me.inpHidden.Size = New System.Drawing.Size(508, 22)
        Me.inpHidden.TabIndex = 11
        Me.inpHidden.TextLines = 1
        Me.inpHidden.TextWidth = 100
        Me.inpHidden.Value = Nothing
        '
        'inpRestrict
        '
        Me.inpRestrict.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange
        Me.inpRestrict.BackColor = System.Drawing.SystemColors.Control
        Me.inpRestrict.Dock = System.Windows.Forms.DockStyle.Top
        Me.inpRestrict.EditType = CT2Input.EditTypes.Check
        Me.inpRestrict.LabelText = "Restricted:"
        Me.inpRestrict.LabelWidth = 80
        Me.inpRestrict.Location = New System.Drawing.Point(5, 228)
        Me.inpRestrict.Name = "inpRestrict"
        Me.inpRestrict.Padding = New System.Windows.Forms.Padding(0, 3, 0, 3)
        Me.inpRestrict.ReadOnly = False
        Me.inpRestrict.Size = New System.Drawing.Size(508, 22)
        Me.inpRestrict.TabIndex = 10
        Me.inpRestrict.TextLines = 1
        Me.inpRestrict.TextWidth = 100
        Me.inpRestrict.Value = Nothing
        '
        'inpWidth
        '
        Me.inpWidth.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange
        Me.inpWidth.BackColor = System.Drawing.SystemColors.Control
        Me.inpWidth.Dock = System.Windows.Forms.DockStyle.Top
        Me.inpWidth.EditType = CT2Input.EditTypes.Text
        Me.inpWidth.LabelText = "Width"
        Me.inpWidth.LabelWidth = 80
        Me.inpWidth.Location = New System.Drawing.Point(5, 206)
        Me.inpWidth.Name = "inpWidth"
        Me.inpWidth.Padding = New System.Windows.Forms.Padding(0, 3, 0, 3)
        Me.inpWidth.ReadOnly = False
        Me.inpWidth.Size = New System.Drawing.Size(508, 22)
        Me.inpWidth.TabIndex = 9
        Me.inpWidth.TextLines = 1
        Me.inpWidth.TextWidth = 100
        Me.inpWidth.Value = Nothing
        '
        'inpSortOrder
        '
        Me.inpSortOrder.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange
        Me.inpSortOrder.BackColor = System.Drawing.SystemColors.Control
        Me.inpSortOrder.Dock = System.Windows.Forms.DockStyle.Top
        Me.inpSortOrder.EditType = CT2Input.EditTypes.Text
        Me.inpSortOrder.LabelText = "Sort Order"
        Me.inpSortOrder.LabelWidth = 80
        Me.inpSortOrder.Location = New System.Drawing.Point(5, 184)
        Me.inpSortOrder.Name = "inpSortOrder"
        Me.inpSortOrder.Padding = New System.Windows.Forms.Padding(0, 3, 0, 3)
        Me.inpSortOrder.ReadOnly = False
        Me.inpSortOrder.Size = New System.Drawing.Size(508, 22)
        Me.inpSortOrder.TabIndex = 8
        Me.inpSortOrder.TextLines = 1
        Me.inpSortOrder.TextWidth = 100
        Me.inpSortOrder.Value = Nothing
        '
        'inpSortSeq
        '
        Me.inpSortSeq.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange
        Me.inpSortSeq.BackColor = System.Drawing.SystemColors.Control
        Me.inpSortSeq.Dock = System.Windows.Forms.DockStyle.Top
        Me.inpSortSeq.EditType = CT2Input.EditTypes.Text
        Me.inpSortSeq.LabelText = "Sort Seq"
        Me.inpSortSeq.LabelWidth = 80
        Me.inpSortSeq.Location = New System.Drawing.Point(5, 162)
        Me.inpSortSeq.Name = "inpSortSeq"
        Me.inpSortSeq.Padding = New System.Windows.Forms.Padding(0, 3, 0, 3)
        Me.inpSortSeq.ReadOnly = False
        Me.inpSortSeq.Size = New System.Drawing.Size(508, 22)
        Me.inpSortSeq.TabIndex = 7
        Me.inpSortSeq.TextLines = 1
        Me.inpSortSeq.TextWidth = 100
        Me.inpSortSeq.Value = Nothing
        '
        'inpSeq
        '
        Me.inpSeq.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange
        Me.inpSeq.BackColor = System.Drawing.SystemColors.Control
        Me.inpSeq.Dock = System.Windows.Forms.DockStyle.Top
        Me.inpSeq.EditType = CT2Input.EditTypes.Text
        Me.inpSeq.LabelText = "Seq"
        Me.inpSeq.LabelWidth = 80
        Me.inpSeq.Location = New System.Drawing.Point(5, 140)
        Me.inpSeq.Name = "inpSeq"
        Me.inpSeq.Padding = New System.Windows.Forms.Padding(0, 3, 0, 3)
        Me.inpSeq.ReadOnly = False
        Me.inpSeq.Size = New System.Drawing.Size(508, 22)
        Me.inpSeq.TabIndex = 6
        Me.inpSeq.TextLines = 1
        Me.inpSeq.TextWidth = 100
        Me.inpSeq.Value = Nothing
        '
        'inpHeaderText
        '
        Me.inpHeaderText.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange
        Me.inpHeaderText.BackColor = System.Drawing.SystemColors.Control
        Me.inpHeaderText.Dock = System.Windows.Forms.DockStyle.Top
        Me.inpHeaderText.EditType = CT2Input.EditTypes.Text
        Me.inpHeaderText.LabelText = "Header Text:"
        Me.inpHeaderText.LabelWidth = 80
        Me.inpHeaderText.Location = New System.Drawing.Point(5, 118)
        Me.inpHeaderText.Name = "inpHeaderText"
        Me.inpHeaderText.Padding = New System.Windows.Forms.Padding(0, 3, 0, 3)
        Me.inpHeaderText.ReadOnly = False
        Me.inpHeaderText.Size = New System.Drawing.Size(508, 22)
        Me.inpHeaderText.TabIndex = 5
        Me.inpHeaderText.TextLines = 1
        Me.inpHeaderText.TextWidth = 100
        Me.inpHeaderText.Value = Nothing
        '
        'inpText
        '
        Me.inpText.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange
        Me.inpText.BackColor = System.Drawing.SystemColors.Control
        Me.inpText.Dock = System.Windows.Forms.DockStyle.Top
        Me.inpText.EditType = CT2Input.EditTypes.Text
        Me.inpText.LabelText = "Text:"
        Me.inpText.LabelWidth = 80
        Me.inpText.Location = New System.Drawing.Point(5, 96)
        Me.inpText.Name = "inpText"
        Me.inpText.Padding = New System.Windows.Forms.Padding(0, 3, 0, 3)
        Me.inpText.ReadOnly = False
        Me.inpText.Size = New System.Drawing.Size(508, 22)
        Me.inpText.TabIndex = 4
        Me.inpText.TextLines = 1
        Me.inpText.TextWidth = 100
        Me.inpText.Value = Nothing
        '
        'inpTable
        '
        Me.inpTable.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange
        Me.inpTable.BackColor = System.Drawing.SystemColors.Control
        Me.inpTable.Dock = System.Windows.Forms.DockStyle.Top
        Me.inpTable.EditType = CT2Input.EditTypes.Text
        Me.inpTable.LabelText = "Table:"
        Me.inpTable.LabelWidth = 80
        Me.inpTable.Location = New System.Drawing.Point(5, 74)
        Me.inpTable.Name = "inpTable"
        Me.inpTable.Padding = New System.Windows.Forms.Padding(0, 3, 0, 3)
        Me.inpTable.ReadOnly = False
        Me.inpTable.Size = New System.Drawing.Size(508, 22)
        Me.inpTable.TabIndex = 3
        Me.inpTable.TextLines = 1
        Me.inpTable.TextWidth = 100
        Me.inpTable.Value = Nothing
        '
        'inpField
        '
        Me.inpField.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange
        Me.inpField.BackColor = System.Drawing.SystemColors.Control
        Me.inpField.Dock = System.Windows.Forms.DockStyle.Top
        Me.inpField.EditType = CT2Input.EditTypes.Text
        Me.inpField.LabelText = "Field:"
        Me.inpField.LabelWidth = 80
        Me.inpField.Location = New System.Drawing.Point(5, 52)
        Me.inpField.Name = "inpField"
        Me.inpField.Padding = New System.Windows.Forms.Padding(0, 3, 0, 3)
        Me.inpField.ReadOnly = False
        Me.inpField.Size = New System.Drawing.Size(508, 22)
        Me.inpField.TabIndex = 2
        Me.inpField.TextLines = 1
        Me.inpField.TextWidth = 100
        Me.inpField.Value = Nothing
        '
        'inpName
        '
        Me.inpName.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange
        Me.inpName.BackColor = System.Drawing.SystemColors.Control
        Me.inpName.Dock = System.Windows.Forms.DockStyle.Top
        Me.inpName.EditType = CT2Input.EditTypes.Text
        Me.inpName.LabelText = "Name:"
        Me.inpName.LabelWidth = 80
        Me.inpName.Location = New System.Drawing.Point(5, 30)
        Me.inpName.Name = "inpName"
        Me.inpName.Padding = New System.Windows.Forms.Padding(0, 3, 0, 3)
        Me.inpName.ReadOnly = False
        Me.inpName.Size = New System.Drawing.Size(508, 22)
        Me.inpName.TabIndex = 1
        Me.inpName.TextLines = 1
        Me.inpName.TextWidth = 100
        Me.inpName.Value = Nothing
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripButton1, Me.ToolStripButton2})
        Me.ToolStrip1.Location = New System.Drawing.Point(5, 5)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(508, 25)
        Me.ToolStrip1.TabIndex = 0
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'ToolStripButton1
        '
        Me.ToolStripButton1.Image = CType(resources.GetObject("ToolStripButton1.Image"), System.Drawing.Image)
        Me.ToolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton1.Name = "ToolStripButton1"
        Me.ToolStripButton1.Size = New System.Drawing.Size(112, 22)
        Me.ToolStripButton1.Text = "screen to model"
        '
        'ToolStripButton2
        '
        Me.ToolStripButton2.Image = CType(resources.GetObject("ToolStripButton2.Image"), System.Drawing.Image)
        Me.ToolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton2.Name = "ToolStripButton2"
        Me.ToolStripButton2.Size = New System.Drawing.Size(197, 22)
        Me.ToolStripButton2.Text = "GridHandler.UpdateGridColumn"
        '
        'ucGridColumn
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.Controls.Add(Me.CT2Input1)
        Me.Controls.Add(Me.inpFunction)
        Me.Controls.Add(Me.inpColumnType)
        Me.Controls.Add(Me.inpHidden)
        Me.Controls.Add(Me.inpRestrict)
        Me.Controls.Add(Me.inpWidth)
        Me.Controls.Add(Me.inpSortOrder)
        Me.Controls.Add(Me.inpSortSeq)
        Me.Controls.Add(Me.inpSeq)
        Me.Controls.Add(Me.inpHeaderText)
        Me.Controls.Add(Me.inpText)
        Me.Controls.Add(Me.inpTable)
        Me.Controls.Add(Me.inpField)
        Me.Controls.Add(Me.inpName)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Name = "ucGridColumn"
        Me.Padding = New System.Windows.Forms.Padding(5)
        Me.Size = New System.Drawing.Size(518, 540)
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents inpField As CT2Input
    Friend WithEvents inpTable As CT2Input
    Friend WithEvents inpText As CT2Input
    Friend WithEvents inpSeq As CT2Input
    Friend WithEvents inpHeaderText As CT2Input
    Friend WithEvents inpName As CT2Input
    Friend WithEvents inpWidth As CT2Input
    Friend WithEvents inpSortOrder As CT2Input
    Friend WithEvents inpSortSeq As CT2Input
    Friend WithEvents inpColumnType As CT2Input
    Friend WithEvents inpHidden As CT2Input
    Friend WithEvents inpRestrict As CT2Input
    Friend WithEvents inpFunction As CT2Input
    Friend WithEvents CT2Input1 As CT2Input
    Friend WithEvents ToolStrip1 As ToolStrip
    Friend WithEvents ToolStripButton1 As ToolStripButton
    Friend WithEvents ToolStripButton2 As ToolStripButton
End Class
