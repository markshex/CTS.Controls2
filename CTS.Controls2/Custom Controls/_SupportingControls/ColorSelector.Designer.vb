<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ColorSelector
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
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.tpSystemColors = New System.Windows.Forms.TabPage()
        Me.tpWebColors = New System.Windows.Forms.TabPage()
        Me.TabControl1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.tpWebColors)
        Me.TabControl1.Controls.Add(Me.tpSystemColors)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(180, 282)
        Me.TabControl1.TabIndex = 0
        '
        'tpSystemColors
        '
        Me.tpSystemColors.Location = New System.Drawing.Point(4, 22)
        Me.tpSystemColors.Name = "tpSystemColors"
        Me.tpSystemColors.Padding = New System.Windows.Forms.Padding(3)
        Me.tpSystemColors.Size = New System.Drawing.Size(172, 256)
        Me.tpSystemColors.TabIndex = 0
        Me.tpSystemColors.Text = "System"
        Me.tpSystemColors.UseVisualStyleBackColor = True
        '
        'tpWebColors
        '
        Me.tpWebColors.Location = New System.Drawing.Point(4, 22)
        Me.tpWebColors.Name = "tpWebColors"
        Me.tpWebColors.Padding = New System.Windows.Forms.Padding(3)
        Me.tpWebColors.Size = New System.Drawing.Size(172, 256)
        Me.tpWebColors.TabIndex = 1
        Me.tpWebColors.Text = "Web"
        Me.tpWebColors.UseVisualStyleBackColor = True
        '
        'ColorSelector
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.TabControl1)
        Me.Name = "ColorSelector"
        Me.Size = New System.Drawing.Size(180, 282)
        Me.TabControl1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents tpWebColors As TabPage
    Friend WithEvents tpSystemColors As TabPage
End Class
