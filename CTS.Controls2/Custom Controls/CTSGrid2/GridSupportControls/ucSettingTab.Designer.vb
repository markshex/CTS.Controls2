<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ucSettingTab
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
        Me.TabControl1 = New TabControl()
        Me.tpColumns = New System.Windows.Forms.TabPage()
        Me.UcGridColumns1 = New ucGridColumns()
        Me.tpSQL = New System.Windows.Forms.TabPage()
        Me.ucSQLFormatting1 = New ucSQLFormatting()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.TabControl1.SuspendLayout()
        Me.tpColumns.SuspendLayout()
        Me.tpSQL.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.tpColumns)
        Me.TabControl1.Controls.Add(Me.tpSQL)
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(787, 613)
        Me.TabControl1.TabIndex = 0
        '
        'tpColumns
        '
        Me.tpColumns.Controls.Add(Me.UcGridColumns1)
        Me.tpColumns.Location = New System.Drawing.Point(4, 25)
        Me.tpColumns.Name = "tpColumns"
        Me.tpColumns.Padding = New System.Windows.Forms.Padding(3)
        Me.tpColumns.Size = New System.Drawing.Size(779, 584)
        Me.tpColumns.TabIndex = 1
        Me.tpColumns.Text = "Columns"
        Me.tpColumns.UseVisualStyleBackColor = True
        '
        'UcGridColumns1
        '
        Me.UcGridColumns1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.UcGridColumns1.Location = New System.Drawing.Point(3, 3)
        Me.UcGridColumns1.Name = "UcGridColumns1"
        Me.UcGridColumns1.Size = New System.Drawing.Size(773, 578)
        Me.UcGridColumns1.TabIndex = 0
        Me.UcGridColumns1.UserGrid = Nothing
        '
        'tpSQL
        '
        Me.tpSQL.Controls.Add(Me.ucSQLFormatting1)
        Me.tpSQL.Location = New System.Drawing.Point(4, 25)
        Me.tpSQL.Name = "tpSQL"
        Me.tpSQL.Padding = New System.Windows.Forms.Padding(3)
        Me.tpSQL.Size = New System.Drawing.Size(712, 342)
        Me.tpSQL.TabIndex = 0
        Me.tpSQL.Text = "SQL Statement"
        Me.tpSQL.UseVisualStyleBackColor = True
        '
        'ucSQLFormatting1
        '
        Me.ucSQLFormatting1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ucSQLFormatting1.Location = New System.Drawing.Point(3, 3)
        Me.ucSQLFormatting1.Name = "ucSQLFormatting1"
        Me.ucSQLFormatting1.Size = New System.Drawing.Size(706, 336)
        Me.ucSQLFormatting1.TabIndex = 0
        '
        'TabPage1
        '
        Me.TabPage1.Location = New System.Drawing.Point(4, 25)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(712, 342)
        Me.TabPage1.TabIndex = 2
        Me.TabPage1.Text = "Properties"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'ucSettingTab
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.TabControl1)
        Me.Name = "ucSettingTab"
        Me.Size = New System.Drawing.Size(787, 613)
        Me.TabControl1.ResumeLayout(False)
        Me.tpColumns.ResumeLayout(False)
        Me.tpSQL.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents tpSQL As TabPage
    Friend WithEvents tpColumns As TabPage
    Friend WithEvents ucSQLFormatting1 As ucSQLFormatting
    Friend WithEvents UcGridColumns1 As ucGridColumns
    Friend WithEvents TabPage1 As TabPage
End Class
