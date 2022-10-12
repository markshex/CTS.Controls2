<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ucGridHeader
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
        Me.inpFrom = New CT2Input()
        Me.inpDistinct = New CT2Input()
        Me.inpTitle = New CT2Input()
        Me.inpID = New CT2Input()
        Me.inpName = New CT2Input()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.btnRefresh = New System.Windows.Forms.ToolStripButton()
        Me.btnUpdate = New System.Windows.Forms.ToolStripButton()
        Me.inpWhere = New CT2Input()
        Me.inpGroup = New CT2Input()
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'inpFrom
        '
        Me.inpFrom.Dock = System.Windows.Forms.DockStyle.Top
        Me.inpFrom.EditType = CT2Input.EditTypes.Text
        Me.inpFrom.LabelText = "From Clause:"
        Me.inpFrom.LabelWidth = 80
        Me.inpFrom.Location = New System.Drawing.Point(5, 118)
        Me.inpFrom.Name = "inpFrom"
        Me.inpFrom.Padding = New System.Windows.Forms.Padding(0, 3, 0, 3)
        Me.inpFrom.ReadOnly = False
        Me.inpFrom.Size = New System.Drawing.Size(508, 110)
        Me.inpFrom.TabIndex = 12
        Me.inpFrom.TextLines = 7
        Me.inpFrom.TextWidth = 400
        Me.inpFrom.Value = Nothing
        '
        'inpDistinct
        '
        Me.inpDistinct.Dock = System.Windows.Forms.DockStyle.Top
        Me.inpDistinct.EditType = CT2Input.EditTypes.Check
        Me.inpDistinct.LabelText = "Distinct"
        Me.inpDistinct.LabelWidth = 80
        Me.inpDistinct.Location = New System.Drawing.Point(5, 96)
        Me.inpDistinct.Name = "inpDistinct"
        Me.inpDistinct.Padding = New System.Windows.Forms.Padding(0, 3, 0, 3)
        Me.inpDistinct.ReadOnly = False
        Me.inpDistinct.Size = New System.Drawing.Size(508, 22)
        Me.inpDistinct.TabIndex = 10
        Me.inpDistinct.TextLines = 1
        Me.inpDistinct.TextWidth = 100
        Me.inpDistinct.Value = Nothing
        '
        'inpTitle
        '
        Me.inpTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.inpTitle.EditType = CT2Input.EditTypes.Text
        Me.inpTitle.LabelText = "Title:"
        Me.inpTitle.LabelWidth = 80
        Me.inpTitle.Location = New System.Drawing.Point(5, 74)
        Me.inpTitle.Name = "inpTitle"
        Me.inpTitle.Padding = New System.Windows.Forms.Padding(0, 3, 0, 3)
        Me.inpTitle.ReadOnly = False
        Me.inpTitle.Size = New System.Drawing.Size(508, 22)
        Me.inpTitle.TabIndex = 4
        Me.inpTitle.TextLines = 1
        Me.inpTitle.TextWidth = 200
        Me.inpTitle.Value = Nothing
        '
        'inpID
        '
        Me.inpID.Dock = System.Windows.Forms.DockStyle.Top
        Me.inpID.EditType = CT2Input.EditTypes.Text
        Me.inpID.LabelText = "ID:"
        Me.inpID.LabelWidth = 80
        Me.inpID.Location = New System.Drawing.Point(5, 30)
        Me.inpID.Name = "inpID"
        Me.inpID.Padding = New System.Windows.Forms.Padding(0, 3, 0, 3)
        Me.inpID.ReadOnly = True
        Me.inpID.Size = New System.Drawing.Size(508, 22)
        Me.inpID.TabIndex = 0
        Me.inpID.TextLines = 1
        Me.inpID.TextWidth = 50
        Me.inpID.Value = Nothing
        '
        'inpName
        '
        Me.inpName.Dock = System.Windows.Forms.DockStyle.Top
        Me.inpName.EditType = CT2Input.EditTypes.Text
        Me.inpName.LabelText = "Name:"
        Me.inpName.LabelWidth = 80
        Me.inpName.Location = New System.Drawing.Point(5, 52)
        Me.inpName.Name = "inpName"
        Me.inpName.Padding = New System.Windows.Forms.Padding(0, 3, 0, 3)
        Me.inpName.ReadOnly = False
        Me.inpName.Size = New System.Drawing.Size(508, 22)
        Me.inpName.TabIndex = 3
        Me.inpName.TextLines = 1
        Me.inpName.TextWidth = 100
        Me.inpName.Value = Nothing
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnRefresh, Me.btnUpdate})
        Me.ToolStrip1.Location = New System.Drawing.Point(5, 5)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(508, 25)
        Me.ToolStrip1.TabIndex = 14
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'btnRefresh
        '
        Me.btnRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnRefresh.Image = Global.My.Resources.Resources.Refresh
        Me.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(23, 22)
        Me.btnRefresh.Text = "Refresh"
        '
        'btnUpdate
        '
        Me.btnUpdate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnUpdate.Image = Global.My.Resources.Resources.Accept
        Me.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(23, 22)
        Me.btnUpdate.Text = "Update"
        '
        'inpWhere
        '
        Me.inpWhere.Dock = System.Windows.Forms.DockStyle.Top
        Me.inpWhere.EditType = CT2Input.EditTypes.Text
        Me.inpWhere.LabelText = "Default Where Clause:"
        Me.inpWhere.LabelWidth = 80
        Me.inpWhere.Location = New System.Drawing.Point(5, 228)
        Me.inpWhere.Name = "inpWhere"
        Me.inpWhere.Padding = New System.Windows.Forms.Padding(0, 3, 0, 3)
        Me.inpWhere.ReadOnly = False
        Me.inpWhere.Size = New System.Drawing.Size(508, 110)
        Me.inpWhere.TabIndex = 15
        Me.inpWhere.TextLines = 7
        Me.inpWhere.TextWidth = 400
        Me.inpWhere.Value = Nothing
        '
        'inpGroup
        '
        Me.inpGroup.Dock = System.Windows.Forms.DockStyle.Top
        Me.inpGroup.EditType = CT2Input.EditTypes.Text
        Me.inpGroup.LabelText = "Group by Clause:"
        Me.inpGroup.LabelWidth = 80
        Me.inpGroup.Location = New System.Drawing.Point(5, 338)
        Me.inpGroup.Name = "inpGroup"
        Me.inpGroup.Padding = New System.Windows.Forms.Padding(0, 3, 0, 3)
        Me.inpGroup.ReadOnly = False
        Me.inpGroup.Size = New System.Drawing.Size(508, 110)
        Me.inpGroup.TabIndex = 16
        Me.inpGroup.TextLines = 4
        Me.inpGroup.TextWidth = 400
        Me.inpGroup.Value = Nothing
        '
        'ucGridHeader
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.Controls.Add(Me.inpGroup)
        Me.Controls.Add(Me.inpWhere)
        Me.Controls.Add(Me.inpFrom)
        Me.Controls.Add(Me.inpDistinct)
        Me.Controls.Add(Me.inpTitle)
        Me.Controls.Add(Me.inpName)
        Me.Controls.Add(Me.inpID)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Name = "ucGridHeader"
        Me.Padding = New System.Windows.Forms.Padding(5)
        Me.Size = New System.Drawing.Size(518, 474)
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents inpID As CT2Input
    Friend WithEvents inpTitle As CT2Input
    Friend WithEvents inpName As CT2Input
    Friend WithEvents inpDistinct As CT2Input
    Friend WithEvents inpFrom As CT2Input
    Friend WithEvents ToolStrip1 As ToolStrip
    Friend WithEvents btnRefresh As ToolStripButton
    Friend WithEvents inpWhere As CT2Input
    Friend WithEvents inpGroup As CT2Input
    Friend WithEvents btnUpdate As ToolStripButton
End Class
