Namespace CTS.Controls2

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class CT2DateTime
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
            Me.components = New System.ComponentModel.Container()
            Me.cmsRightClick = New System.Windows.Forms.ContextMenuStrip(Me.components)
            Me.miClear = New System.Windows.Forms.ToolStripMenuItem()
            Me.miSetCurrent = New System.Windows.Forms.ToolStripMenuItem()
            Me.miViewCalendar = New System.Windows.Forms.ToolStripMenuItem()
            Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
            Me.miResetCurrent = New System.Windows.Forms.ToolStripMenuItem()
            Me.miResetPrevious = New System.Windows.Forms.ToolStripMenuItem()
            Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
            Me.miPaste = New System.Windows.Forms.ToolStripMenuItem()
            Me.miCopy = New System.Windows.Forms.ToolStripMenuItem()
            Me.pnlDate = New System.Windows.Forms.Panel()
            Me.mcDate = New System.Windows.Forms.MonthCalendar()
            Me.cbEnableDate = New System.Windows.Forms.CheckBox()
            Me.year4 = New CTS.Controls2.CT2DateTime.DatePartsTB()
            Me.day = New CTS.Controls2.CT2DateTime.DatePartsTB()
            Me.month = New CTS.Controls2.CT2DateTime.DatePartsTB()
            Me.datesep2 = New System.Windows.Forms.Label()
            Me.datesep1 = New System.Windows.Forms.Label()
            Me.btnCalendar = New System.Windows.Forms.Button()
            Me.pnlTime = New System.Windows.Forms.Panel()
            Me.minute = New CTS.Controls2.CT2DateTime.DatePartsTB()
            Me.hour = New CTS.Controls2.CT2DateTime.DatePartsTB()
            Me.timesep = New System.Windows.Forms.Label()
            Me.pnlFiller = New System.Windows.Forms.Panel()
            Me.ttDate = New System.Windows.Forms.ToolTip(Me.components)
            Me.ttTime = New System.Windows.Forms.ToolTip(Me.components)
            Me.cmsRightClick.SuspendLayout()
            Me.pnlDate.SuspendLayout()
            Me.pnlTime.SuspendLayout()
            Me.SuspendLayout()
            '
            'cmsRightClick
            '
            Me.cmsRightClick.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.miClear, Me.miSetCurrent, Me.miViewCalendar, Me.ToolStripSeparator2, Me.miResetCurrent, Me.miResetPrevious, Me.ToolStripSeparator1, Me.miPaste, Me.miCopy})
            Me.cmsRightClick.Name = "cmsRightClick"
            Me.cmsRightClick.ShowImageMargin = False
            Me.cmsRightClick.Size = New System.Drawing.Size(185, 170)
            '
            'miClear
            '
            Me.miClear.Name = "miClear"
            Me.miClear.ShortcutKeys = System.Windows.Forms.Keys.Delete
            Me.miClear.Size = New System.Drawing.Size(184, 22)
            Me.miClear.Text = "Clear Date/Time"
            '
            'miSetCurrent
            '
            Me.miSetCurrent.Name = "miSetCurrent"
            Me.miSetCurrent.ShortcutKeys = System.Windows.Forms.Keys.Insert
            Me.miSetCurrent.Size = New System.Drawing.Size(184, 22)
            Me.miSetCurrent.Text = "Set to Current"
            '
            'miViewCalendar
            '
            Me.miViewCalendar.Name = "miViewCalendar"
            Me.miViewCalendar.ShortcutKeys = System.Windows.Forms.Keys.F4
            Me.miViewCalendar.Size = New System.Drawing.Size(184, 22)
            Me.miViewCalendar.Text = "View Calendar"
            '
            'ToolStripSeparator2
            '
            Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
            Me.ToolStripSeparator2.Size = New System.Drawing.Size(181, 6)
            '
            'miResetCurrent
            '
            Me.miResetCurrent.Name = "miResetCurrent"
            Me.miResetCurrent.Size = New System.Drawing.Size(184, 22)
            Me.miResetCurrent.Text = "Reset Current ()"
            '
            'miResetPrevious
            '
            Me.miResetPrevious.Name = "miResetPrevious"
            Me.miResetPrevious.ShortcutKeys = CType((System.Windows.Forms.Keys.Shift Or System.Windows.Forms.Keys.Delete), System.Windows.Forms.Keys)
            Me.miResetPrevious.Size = New System.Drawing.Size(184, 22)
            Me.miResetPrevious.Text = "Reset Previous ()"
            '
            'ToolStripSeparator1
            '
            Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
            Me.ToolStripSeparator1.Size = New System.Drawing.Size(181, 6)
            '
            'miPaste
            '
            Me.miPaste.Name = "miPaste"
            Me.miPaste.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.V), System.Windows.Forms.Keys)
            Me.miPaste.Size = New System.Drawing.Size(184, 22)
            Me.miPaste.Text = "Paste"
            '
            'miCopy
            '
            Me.miCopy.Name = "miCopy"
            Me.miCopy.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.C), System.Windows.Forms.Keys)
            Me.miCopy.Size = New System.Drawing.Size(184, 22)
            Me.miCopy.Text = "Copy"
            '
            'pnlDate
            '
            Me.pnlDate.BackColor = System.Drawing.Color.White
            Me.pnlDate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
            Me.pnlDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.pnlDate.Controls.Add(Me.mcDate)
            Me.pnlDate.Controls.Add(Me.cbEnableDate)
            Me.pnlDate.Controls.Add(Me.year4)
            Me.pnlDate.Controls.Add(Me.day)
            Me.pnlDate.Controls.Add(Me.month)
            Me.pnlDate.Controls.Add(Me.datesep2)
            Me.pnlDate.Controls.Add(Me.datesep1)
            Me.pnlDate.Controls.Add(Me.btnCalendar)
            Me.pnlDate.Dock = System.Windows.Forms.DockStyle.Left
            Me.pnlDate.Location = New System.Drawing.Point(0, 0)
            Me.pnlDate.Margin = New System.Windows.Forms.Padding(0)
            Me.pnlDate.Name = "pnlDate"
            Me.pnlDate.Size = New System.Drawing.Size(127, 22)
            Me.pnlDate.TabIndex = 38
            Me.pnlDate.TabStop = True
            '
            'mcDate
            '
            Me.mcDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.mcDate.Location = New System.Drawing.Point(14, 29)
            Me.mcDate.Name = "mcDate"
            Me.mcDate.TabIndex = 47
            Me.mcDate.TabStop = False
            Me.mcDate.Visible = False
            '
            'cbEnableDate
            '
            Me.cbEnableDate.Dock = System.Windows.Forms.DockStyle.Left
            Me.cbEnableDate.Location = New System.Drawing.Point(0, 0)
            Me.cbEnableDate.Name = "cbEnableDate"
            Me.cbEnableDate.Size = New System.Drawing.Size(12, 20)
            Me.cbEnableDate.TabIndex = 46
            Me.cbEnableDate.TabStop = False
            Me.cbEnableDate.UseVisualStyleBackColor = True
            '
            'year4
            '
            Me.year4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.year4.ContextMenuStrip = Me.cmsRightClick
            Me.year4.Location = New System.Drawing.Point(75, 0)
            Me.year4.MaxLength = 4
            Me.year4.Name = "year4"
            Me.year4.Size = New System.Drawing.Size(24, 20)
            Me.year4.TabIndex = 43
            Me.year4.TabStop = False
            '
            'day
            '
            Me.day.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.day.ContextMenuStrip = Me.cmsRightClick
            Me.day.Location = New System.Drawing.Point(43, 0)
            Me.day.Margin = New System.Windows.Forms.Padding(0)
            Me.day.MaxLength = 2
            Me.day.Name = "day"
            Me.day.Size = New System.Drawing.Size(18, 20)
            Me.day.TabIndex = 42
            Me.day.TabStop = False
            '
            'month
            '
            Me.month.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.month.ContextMenuStrip = Me.cmsRightClick
            Me.month.Location = New System.Drawing.Point(14, 0)
            Me.month.Margin = New System.Windows.Forms.Padding(0)
            Me.month.MaxLength = 2
            Me.month.Name = "month"
            Me.month.Size = New System.Drawing.Size(18, 20)
            Me.month.TabIndex = 41
            '
            'datesep2
            '
            Me.datesep2.AutoSize = True
            Me.datesep2.Location = New System.Drawing.Point(62, 0)
            Me.datesep2.Name = "datesep2"
            Me.datesep2.Size = New System.Drawing.Size(12, 13)
            Me.datesep2.TabIndex = 45
            Me.datesep2.Text = "/"
            Me.datesep2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'datesep1
            '
            Me.datesep1.AutoEllipsis = True
            Me.datesep1.AutoSize = True
            Me.datesep1.Location = New System.Drawing.Point(31, 0)
            Me.datesep1.Margin = New System.Windows.Forms.Padding(0)
            Me.datesep1.Name = "datesep1"
            Me.datesep1.Size = New System.Drawing.Size(12, 13)
            Me.datesep1.TabIndex = 44
            Me.datesep1.Text = "/"
            Me.datesep1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'btnCalendar
            '
            Me.btnCalendar.BackColor = System.Drawing.Color.Transparent
            Me.btnCalendar.BackgroundImage = Global.My.Resources.Calendar_small
            Me.btnCalendar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
            Me.btnCalendar.CausesValidation = False
            Me.btnCalendar.Dock = System.Windows.Forms.DockStyle.Right
            Me.btnCalendar.FlatAppearance.BorderSize = 0
            Me.btnCalendar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnCalendar.Location = New System.Drawing.Point(105, 0)
            Me.btnCalendar.Margin = New System.Windows.Forms.Padding(0)
            Me.btnCalendar.Name = "btnCalendar"
            Me.btnCalendar.Size = New System.Drawing.Size(20, 20)
            Me.btnCalendar.TabIndex = 40
            Me.btnCalendar.TabStop = False
            Me.btnCalendar.UseMnemonic = False
            Me.btnCalendar.UseVisualStyleBackColor = False
            '
            'pnlTime
            '
            Me.pnlTime.BackColor = System.Drawing.Color.White
            Me.pnlTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.pnlTime.Controls.Add(Me.minute)
            Me.pnlTime.Controls.Add(Me.hour)
            Me.pnlTime.Controls.Add(Me.timesep)
            Me.pnlTime.Dock = System.Windows.Forms.DockStyle.Left
            Me.pnlTime.Location = New System.Drawing.Point(130, 0)
            Me.pnlTime.Name = "pnlTime"
            Me.pnlTime.Size = New System.Drawing.Size(53, 22)
            Me.pnlTime.TabIndex = 39
            Me.pnlTime.TabStop = True
            '
            'minute
            '
            Me.minute.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.minute.ContextMenuStrip = Me.cmsRightClick
            Me.minute.Location = New System.Drawing.Point(31, 0)
            Me.minute.Name = "minute"
            Me.minute.Size = New System.Drawing.Size(18, 20)
            Me.minute.TabIndex = 37
            Me.minute.TabStop = False
            '
            'hour
            '
            Me.hour.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.hour.ContextMenuStrip = Me.cmsRightClick
            Me.hour.Location = New System.Drawing.Point(2, 0)
            Me.hour.Name = "hour"
            Me.hour.Size = New System.Drawing.Size(18, 20)
            Me.hour.TabIndex = 36
            '
            'timesep
            '
            Me.timesep.AutoSize = True
            Me.timesep.Location = New System.Drawing.Point(20, 0)
            Me.timesep.Name = "timesep"
            Me.timesep.Size = New System.Drawing.Size(10, 13)
            Me.timesep.TabIndex = 35
            Me.timesep.Text = ":"
            '
            'pnlFiller
            '
            Me.pnlFiller.Dock = System.Windows.Forms.DockStyle.Left
            Me.pnlFiller.Location = New System.Drawing.Point(127, 0)
            Me.pnlFiller.Name = "pnlFiller"
            Me.pnlFiller.Size = New System.Drawing.Size(3, 22)
            Me.pnlFiller.TabIndex = 40
            '
            'ttDate
            '
            Me.ttDate.ForeColor = System.Drawing.Color.Black
            Me.ttDate.ToolTipIcon = System.Windows.Forms.ToolTipIcon.[Error]
            Me.ttDate.ToolTipTitle = "Date Error"
            '
            'ttTime
            '
            Me.ttTime.ForeColor = System.Drawing.Color.Black
            Me.ttTime.ToolTipIcon = System.Windows.Forms.ToolTipIcon.[Error]
            Me.ttTime.ToolTipTitle = "Time Error"
            '
            'CTSDateTime
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.Color.Transparent
            Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
            Me.ContextMenuStrip = Me.cmsRightClick
            Me.Controls.Add(Me.pnlTime)
            Me.Controls.Add(Me.pnlFiller)
            Me.Controls.Add(Me.pnlDate)
            Me.Margin = New System.Windows.Forms.Padding(0)
            Me.Name = "CTSDateTime"
            Me.Size = New System.Drawing.Size(184, 22)
            Me.cmsRightClick.ResumeLayout(False)
            Me.pnlDate.ResumeLayout(False)
            Me.pnlDate.PerformLayout()
            Me.pnlTime.ResumeLayout(False)
            Me.pnlTime.PerformLayout()
            Me.ResumeLayout(False)

        End Sub

        Protected Overrides Function ProcessCmdKey(ByRef msg As Message, ByVal keyData As Keys) As Boolean
            Const WM_KEYDOWN As Integer = &H100
            Const WM_SYSKEYDOWN As Integer = &H104

            If ((msg.Msg = WM_KEYDOWN) Or (msg.Msg = WM_SYSKEYDOWN)) Then
                Select Case (keyData)
                    Case Keys.Down
                    Case Keys.Up
                    Case Keys.Tab
                    Case (Keys.Control Or Keys.M)
                    Case (Keys.Alt Or Keys.Z)
                    Case Keys.Escape
                End Select
            End If

            Return MyBase.ProcessCmdKey(msg, keyData)
        End Function

        Friend WithEvents cmsRightClick As System.Windows.Forms.ContextMenuStrip
        Friend WithEvents miResetPrevious As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents miCopy As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents miPaste As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents miSetCurrent As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents pnlDate As System.Windows.Forms.Panel
        Friend WithEvents btnCalendar As System.Windows.Forms.Button
        Friend WithEvents cbEnableDate As System.Windows.Forms.CheckBox
        Friend WithEvents year4 As CTS.Controls2.CT2DateTime.DatePartsTB
        Friend WithEvents day As CTS.Controls2.CT2DateTime.DatePartsTB
        Friend WithEvents month As CTS.Controls2.CT2DateTime.DatePartsTB
        Friend WithEvents datesep2 As System.Windows.Forms.Label
        Friend WithEvents datesep1 As System.Windows.Forms.Label
        Friend WithEvents pnlTime As System.Windows.Forms.Panel
        Friend WithEvents minute As CTS.Controls2.CT2DateTime.DatePartsTB
        Friend WithEvents hour As CTS.Controls2.CT2DateTime.DatePartsTB
        Friend WithEvents timesep As System.Windows.Forms.Label
        Friend WithEvents pnlFiller As System.Windows.Forms.Panel
        Friend WithEvents mcDate As System.Windows.Forms.MonthCalendar
        Friend WithEvents ttDate As System.Windows.Forms.ToolTip
        Friend WithEvents miResetCurrent As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents miClear As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents miViewCalendar As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ttTime As System.Windows.Forms.ToolTip

    End Class

End Namespace