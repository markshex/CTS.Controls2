<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ucGridColumns
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ucGridColumns))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.btnDown = New System.Windows.Forms.Button()
        Me.btnUp = New System.Windows.Forms.Button()
        Me.btnSelectAll = New System.Windows.Forms.Button()
        Me.btnDefaults = New System.Windows.Forms.Button()
        Me.CVisible = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.dgvColumns = New System.Windows.Forms.DataGridView()
        Me.CName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.CField = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.CPType = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.CSize = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.CSeq = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.CRestrict = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.pnlColumns = New System.Windows.Forms.Panel()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.tsbApply = New System.Windows.Forms.ToolStripButton()
        CType(Me.dgvColumns, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlColumns.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnDown
        '
        Me.btnDown.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDown.BackgroundImage = CType(resources.GetObject("btnDown.BackgroundImage"), System.Drawing.Image)
        Me.btnDown.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnDown.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnDown.Location = New System.Drawing.Point(550, 80)
        Me.btnDown.Name = "btnDown"
        Me.btnDown.Size = New System.Drawing.Size(25, 25)
        Me.btnDown.TabIndex = 54
        Me.btnDown.UseVisualStyleBackColor = True
        '
        'btnUp
        '
        Me.btnUp.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnUp.BackgroundImage = CType(resources.GetObject("btnUp.BackgroundImage"), System.Drawing.Image)
        Me.btnUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnUp.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnUp.ImageAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnUp.Location = New System.Drawing.Point(550, 44)
        Me.btnUp.Name = "btnUp"
        Me.btnUp.Size = New System.Drawing.Size(25, 25)
        Me.btnUp.TabIndex = 53
        Me.btnUp.UseVisualStyleBackColor = True
        '
        'btnSelectAll
        '
        Me.btnSelectAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSelectAll.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.btnSelectAll.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnSelectAll.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnSelectAll.ForeColor = System.Drawing.Color.White
        Me.btnSelectAll.Location = New System.Drawing.Point(449, 16)
        Me.btnSelectAll.Name = "btnSelectAll"
        Me.btnSelectAll.Size = New System.Drawing.Size(95, 22)
        Me.btnSelectAll.TabIndex = 51
        Me.btnSelectAll.Text = "Make All Visible"
        Me.btnSelectAll.UseVisualStyleBackColor = False
        '
        'btnDefaults
        '
        Me.btnDefaults.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDefaults.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.btnDefaults.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnDefaults.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnDefaults.ForeColor = System.Drawing.Color.White
        Me.btnDefaults.Location = New System.Drawing.Point(348, 16)
        Me.btnDefaults.Name = "btnDefaults"
        Me.btnDefaults.Size = New System.Drawing.Size(95, 22)
        Me.btnDefaults.TabIndex = 52
        Me.btnDefaults.Text = "Set to Default"
        Me.btnDefaults.UseVisualStyleBackColor = False
        Me.btnDefaults.Visible = False
        '
        'CVisible
        '
        Me.CVisible.DataPropertyName = "CVisible"
        Me.CVisible.HeaderText = "Visible"
        Me.CVisible.Name = "CVisible"
        Me.CVisible.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.CVisible.Width = 60
        '
        'dgvColumns
        '
        Me.dgvColumns.AllowUserToAddRows = False
        Me.dgvColumns.AllowUserToDeleteRows = False
        Me.dgvColumns.AllowUserToResizeColumns = False
        Me.dgvColumns.AllowUserToResizeRows = False
        Me.dgvColumns.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvColumns.BackgroundColor = System.Drawing.Color.White
        Me.dgvColumns.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None
        Me.dgvColumns.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.Color.White
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvColumns.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvColumns.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvColumns.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.CVisible, Me.CName, Me.CField, Me.CPType, Me.CSize, Me.CSeq, Me.CRestrict})
        Me.dgvColumns.Location = New System.Drawing.Point(26, 44)
        Me.dgvColumns.MultiSelect = False
        Me.dgvColumns.Name = "dgvColumns"
        Me.dgvColumns.RowHeadersVisible = False
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.LightGray
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black
        Me.dgvColumns.RowsDefaultCellStyle = DataGridViewCellStyle3
        Me.dgvColumns.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvColumns.Size = New System.Drawing.Size(518, 391)
        Me.dgvColumns.TabIndex = 50
        '
        'CName
        '
        Me.CName.DataPropertyName = "CName"
        Me.CName.HeaderText = "Name"
        Me.CName.Name = "CName"
        Me.CName.ReadOnly = True
        Me.CName.Width = 200
        '
        'CField
        '
        Me.CField.DataPropertyName = "CField"
        Me.CField.HeaderText = "Field"
        Me.CField.Name = "CField"
        Me.CField.ReadOnly = True
        Me.CField.Width = 110
        '
        'CPType
        '
        Me.CPType.DataPropertyName = "CPType"
        Me.CPType.HeaderText = "Type"
        Me.CPType.Name = "CPType"
        Me.CPType.ReadOnly = True
        Me.CPType.Width = 80
        '
        'CSize
        '
        Me.CSize.DataPropertyName = "CSize"
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.CSize.DefaultCellStyle = DataGridViewCellStyle2
        Me.CSize.HeaderText = "Size"
        Me.CSize.Name = "CSize"
        Me.CSize.ReadOnly = True
        Me.CSize.Width = 50
        '
        'CSeq
        '
        Me.CSeq.DataPropertyName = "CSeq"
        Me.CSeq.HeaderText = "Display Sequence"
        Me.CSeq.Name = "CSeq"
        Me.CSeq.ReadOnly = True
        Me.CSeq.Visible = False
        Me.CSeq.Width = 80
        '
        'CRestrict
        '
        Me.CRestrict.DataPropertyName = "CRestrict"
        Me.CRestrict.HeaderText = "Restrict"
        Me.CRestrict.Name = "CRestrict"
        Me.CRestrict.ReadOnly = True
        Me.CRestrict.Visible = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(26, 28)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(64, 13)
        Me.Label1.TabIndex = 48
        Me.Label1.Text = "All Columns:"
        '
        'pnlColumns
        '
        Me.pnlColumns.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlColumns.Controls.Add(Me.btnDown)
        Me.pnlColumns.Controls.Add(Me.btnUp)
        Me.pnlColumns.Controls.Add(Me.btnDefaults)
        Me.pnlColumns.Controls.Add(Me.btnSelectAll)
        Me.pnlColumns.Controls.Add(Me.Label1)
        Me.pnlColumns.Controls.Add(Me.dgvColumns)
        Me.pnlColumns.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlColumns.Location = New System.Drawing.Point(0, 25)
        Me.pnlColumns.Name = "pnlColumns"
        Me.pnlColumns.Size = New System.Drawing.Size(588, 462)
        Me.pnlColumns.TabIndex = 55
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.DataPropertyName = "CName"
        Me.DataGridViewTextBoxColumn1.HeaderText = "Name"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.ReadOnly = True
        Me.DataGridViewTextBoxColumn1.Width = 200
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.DataPropertyName = "CField"
        Me.DataGridViewTextBoxColumn2.HeaderText = "Field"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.ReadOnly = True
        Me.DataGridViewTextBoxColumn2.Width = 110
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.DataPropertyName = "CPType"
        Me.DataGridViewTextBoxColumn3.HeaderText = "Type"
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.ReadOnly = True
        Me.DataGridViewTextBoxColumn3.Width = 80
        '
        'DataGridViewTextBoxColumn4
        '
        Me.DataGridViewTextBoxColumn4.DataPropertyName = "CSize"
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.DataGridViewTextBoxColumn4.DefaultCellStyle = DataGridViewCellStyle4
        Me.DataGridViewTextBoxColumn4.HeaderText = "Size"
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        Me.DataGridViewTextBoxColumn4.ReadOnly = True
        Me.DataGridViewTextBoxColumn4.Width = 50
        '
        'DataGridViewTextBoxColumn5
        '
        Me.DataGridViewTextBoxColumn5.DataPropertyName = "CSeq"
        Me.DataGridViewTextBoxColumn5.HeaderText = "Display Sequence"
        Me.DataGridViewTextBoxColumn5.Name = "DataGridViewTextBoxColumn5"
        Me.DataGridViewTextBoxColumn5.ReadOnly = True
        Me.DataGridViewTextBoxColumn5.Visible = False
        Me.DataGridViewTextBoxColumn5.Width = 80
        '
        'DataGridViewTextBoxColumn6
        '
        Me.DataGridViewTextBoxColumn6.DataPropertyName = "CRestrict"
        Me.DataGridViewTextBoxColumn6.HeaderText = "Restrict"
        Me.DataGridViewTextBoxColumn6.Name = "DataGridViewTextBoxColumn6"
        Me.DataGridViewTextBoxColumn6.ReadOnly = True
        Me.DataGridViewTextBoxColumn6.Visible = False
        '
        'ToolStrip1
        '
        Me.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbApply})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(588, 25)
        Me.ToolStrip1.TabIndex = 56
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'tsbApply
        '
        Me.tsbApply.Image = Global.My.Resources.Resources.Accept
        Me.tsbApply.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbApply.Name = "tsbApply"
        Me.tsbApply.Size = New System.Drawing.Size(58, 22)
        Me.tsbApply.Text = "Apply"
        '
        'ucGridColumns
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.pnlColumns)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Name = "ucGridColumns"
        Me.Size = New System.Drawing.Size(588, 487)
        CType(Me.dgvColumns, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlColumns.ResumeLayout(False)
        Me.pnlColumns.PerformLayout()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnDown As Button
    Friend WithEvents btnUp As Button
    Friend WithEvents btnSelectAll As Button
    Friend WithEvents CRestrict As DataGridViewTextBoxColumn
    Friend WithEvents CSeq As DataGridViewTextBoxColumn
    Friend WithEvents CSize As DataGridViewTextBoxColumn
    Friend WithEvents btnDefaults As Button
    Friend WithEvents CPType As DataGridViewTextBoxColumn
    Friend WithEvents CName As DataGridViewTextBoxColumn
    Friend WithEvents CVisible As DataGridViewCheckBoxColumn
    Friend WithEvents dgvColumns As DataGridView
    Friend WithEvents CField As DataGridViewTextBoxColumn
    Friend WithEvents Label1 As Label
    Friend WithEvents pnlColumns As Panel
    Friend WithEvents DataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn4 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn5 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn6 As DataGridViewTextBoxColumn
    Friend WithEvents ToolStrip1 As ToolStrip
    Friend WithEvents tsbApply As ToolStripButton
End Class
