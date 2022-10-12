<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Calendar
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
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.lblCalendarLabel = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.lblSaturday = New System.Windows.Forms.Label()
        Me.lblFriday = New System.Windows.Forms.Label()
        Me.lblThursday = New System.Windows.Forms.Label()
        Me.lblWednesday = New System.Windows.Forms.Label()
        Me.lblSunday = New System.Windows.Forms.Label()
        Me.lblTuesday = New System.Windows.Forms.Label()
        Me.lblMonday = New System.Windows.Forms.Label()
        Me.pnlDays = New System.Windows.Forms.Panel()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Button2)
        Me.Panel1.Controls.Add(Me.Button1)
        Me.Panel1.Controls.Add(Me.lblCalendarLabel)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(606, 32)
        Me.Panel1.TabIndex = 0
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(262, 6)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(37, 23)
        Me.Button2.TabIndex = 2
        Me.Button2.Text = ">>"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(4, 6)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(37, 23)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "<<"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'lblCalendarLabel
        '
        Me.lblCalendarLabel.AutoSize = True
        Me.lblCalendarLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCalendarLabel.Location = New System.Drawing.Point(93, 9)
        Me.lblCalendarLabel.Name = "lblCalendarLabel"
        Me.lblCalendarLabel.Size = New System.Drawing.Size(112, 20)
        Me.lblCalendarLabel.TabIndex = 0
        Me.lblCalendarLabel.Text = "CalendarLabel"
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.lblSaturday)
        Me.Panel2.Controls.Add(Me.lblFriday)
        Me.Panel2.Controls.Add(Me.lblThursday)
        Me.Panel2.Controls.Add(Me.lblWednesday)
        Me.Panel2.Controls.Add(Me.lblSunday)
        Me.Panel2.Controls.Add(Me.lblTuesday)
        Me.Panel2.Controls.Add(Me.lblMonday)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel2.Location = New System.Drawing.Point(0, 32)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(606, 32)
        Me.Panel2.TabIndex = 1
        '
        'lblSaturday
        '
        Me.lblSaturday.AutoSize = True
        Me.lblSaturday.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSaturday.Location = New System.Drawing.Point(258, 7)
        Me.lblSaturday.Name = "lblSaturday"
        Me.lblSaturday.Size = New System.Drawing.Size(34, 20)
        Me.lblSaturday.TabIndex = 6
        Me.lblSaturday.Text = "Sat"
        '
        'lblFriday
        '
        Me.lblFriday.AutoSize = True
        Me.lblFriday.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFriday.Location = New System.Drawing.Point(225, 7)
        Me.lblFriday.Name = "lblFriday"
        Me.lblFriday.Size = New System.Drawing.Size(27, 20)
        Me.lblFriday.TabIndex = 5
        Me.lblFriday.Text = "Fri"
        '
        'lblThursday
        '
        Me.lblThursday.AutoSize = True
        Me.lblThursday.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblThursday.Location = New System.Drawing.Point(183, 7)
        Me.lblThursday.Name = "lblThursday"
        Me.lblThursday.Size = New System.Drawing.Size(36, 20)
        Me.lblThursday.TabIndex = 4
        Me.lblThursday.Text = "Thu"
        '
        'lblWednesday
        '
        Me.lblWednesday.AutoSize = True
        Me.lblWednesday.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWednesday.Location = New System.Drawing.Point(135, 7)
        Me.lblWednesday.Name = "lblWednesday"
        Me.lblWednesday.Size = New System.Drawing.Size(42, 20)
        Me.lblWednesday.TabIndex = 3
        Me.lblWednesday.Text = "Wed"
        '
        'lblSunday
        '
        Me.lblSunday.AutoSize = True
        Me.lblSunday.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSunday.Location = New System.Drawing.Point(3, 7)
        Me.lblSunday.Name = "lblSunday"
        Me.lblSunday.Size = New System.Drawing.Size(38, 20)
        Me.lblSunday.TabIndex = 2
        Me.lblSunday.Text = "Sun"
        '
        'lblTuesday
        '
        Me.lblTuesday.AutoSize = True
        Me.lblTuesday.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTuesday.Location = New System.Drawing.Point(93, 7)
        Me.lblTuesday.Name = "lblTuesday"
        Me.lblTuesday.Size = New System.Drawing.Size(36, 20)
        Me.lblTuesday.TabIndex = 1
        Me.lblTuesday.Text = "Tue"
        '
        'lblMonday
        '
        Me.lblMonday.AutoSize = True
        Me.lblMonday.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMonday.Location = New System.Drawing.Point(47, 7)
        Me.lblMonday.Name = "lblMonday"
        Me.lblMonday.Size = New System.Drawing.Size(40, 20)
        Me.lblMonday.TabIndex = 0
        Me.lblMonday.Text = "Mon"
        '
        'pnlDays
        '
        Me.pnlDays.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlDays.Location = New System.Drawing.Point(0, 64)
        Me.pnlDays.Name = "pnlDays"
        Me.pnlDays.Size = New System.Drawing.Size(606, 357)
        Me.pnlDays.TabIndex = 2
        '
        'Calendar
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.WhiteSmoke
        Me.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Controls.Add(Me.pnlDays)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "Calendar"
        Me.Size = New System.Drawing.Size(606, 421)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Panel1 As Panel
    Friend WithEvents lblCalendarLabel As Label
    Friend WithEvents Panel2 As Panel
    Friend WithEvents lblSaturday As Label
    Friend WithEvents lblFriday As Label
    Friend WithEvents lblThursday As Label
    Friend WithEvents lblWednesday As Label
    Friend WithEvents lblSunday As Label
    Friend WithEvents lblTuesday As Label
    Friend WithEvents lblMonday As Label
    Friend WithEvents Button2 As Button
    Friend WithEvents Button1 As Button
    Friend WithEvents pnlDays As Panel
End Class
