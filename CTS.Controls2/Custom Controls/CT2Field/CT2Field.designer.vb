<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CT2Field
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try

            'Is this needed?  I was getting errors on CTSRecord when closing 
            'the form if bindings still existed.
            Me.DataBindings.Clear()

            If disposing AndAlso FieldDateTime IsNot Nothing Then
                RemoveHandler FieldDateTime.StateChanged, AddressOf Me.Field_StateChanged
                RemoveHandler FieldDateTime.ErrorTextChanged, AddressOf Me.Field_ErrorTextChanged
            End If

            If disposing AndAlso FieldCombo IsNot Nothing Then
                RemoveHandler FieldCombo.StateChanged, AddressOf Me.Field_StateChanged
                RemoveHandler FieldCombo.ErrorTextChanged, AddressOf Me.Field_ErrorTextChanged
            End If

            If disposing AndAlso FieldCheck IsNot Nothing Then
                RemoveHandler FieldCheck.StateChanged, AddressOf Me.Field_StateChanged
                RemoveHandler FieldCheck.CheckedChanged, AddressOf Me.FieldCheck_CheckedChanged
            End If

            If disposing AndAlso Collection IsNot Nothing Then
                Collection.Remove(Me)
            End If

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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(CT2Field))
        Me.FieldLabel = New System.Windows.Forms.Label()
        Me.FieldDescription = New System.Windows.Forms.Label()
        Me.pnlFieldLabel = New System.Windows.Forms.Panel()
        Me.pnlFieldDescription = New System.Windows.Forms.Panel()
        Me.pnlPrompt = New System.Windows.Forms.Panel()
        Me.btnPrompt = New System.Windows.Forms.Button()
        Me.pnlDataOuter = New System.Windows.Forms.Panel()
        Me.pnlDataInner = New System.Windows.Forms.Panel()
        Me.FieldText = New CTS.Controls.TextBoxCTS()
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.cmsField = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.miProtect = New System.Windows.Forms.ToolStripMenuItem()
        Me.miUndo = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.miCut = New System.Windows.Forms.ToolStripMenuItem()
        Me.miCopy = New System.Windows.Forms.ToolStripMenuItem()
        Me.miPaste = New System.Windows.Forms.ToolStripMenuItem()
        Me.miClear = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.miProperties = New System.Windows.Forms.ToolStripMenuItem()
        Me.DataToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.pnlFieldLabel.SuspendLayout()
        Me.pnlFieldDescription.SuspendLayout()
        Me.pnlPrompt.SuspendLayout()
        Me.pnlDataOuter.SuspendLayout()
        Me.pnlDataInner.SuspendLayout()
        Me.cmsField.SuspendLayout()
        Me.SuspendLayout()
        '
        'FieldLabel
        '
        Me.FieldLabel.AutoEllipsis = True
        Me.FieldLabel.AutoSize = True
        Me.FieldLabel.Location = New System.Drawing.Point(1, 7)
        Me.FieldLabel.MaximumSize = New System.Drawing.Size(140, 0)
        Me.FieldLabel.Name = "FieldLabel"
        Me.FieldLabel.Size = New System.Drawing.Size(135, 26)
        Me.FieldLabel.TabIndex = 1
        Me.FieldLabel.Text = "Label: Label: Label: Label: Label:"
        '
        'FieldDescription
        '
        Me.FieldDescription.AutoEllipsis = True
        Me.FieldDescription.Location = New System.Drawing.Point(-3, 6)
        Me.FieldDescription.Name = "FieldDescription"
        Me.FieldDescription.Size = New System.Drawing.Size(132, 44)
        Me.FieldDescription.TabIndex = 9
        Me.FieldDescription.Text = "Description Description Description Description Description Description"
        '
        'pnlFieldLabel
        '
        Me.pnlFieldLabel.BackColor = System.Drawing.Color.Transparent
        Me.pnlFieldLabel.Controls.Add(Me.FieldLabel)
        Me.pnlFieldLabel.Dock = System.Windows.Forms.DockStyle.Left
        Me.pnlFieldLabel.Location = New System.Drawing.Point(0, 0)
        Me.pnlFieldLabel.Name = "pnlFieldLabel"
        Me.pnlFieldLabel.Padding = New System.Windows.Forms.Padding(1, 7, 1, 0)
        Me.pnlFieldLabel.Size = New System.Drawing.Size(141, 70)
        Me.pnlFieldLabel.TabIndex = 10
        '
        'pnlFieldDescription
        '
        Me.pnlFieldDescription.BackColor = System.Drawing.Color.Transparent
        Me.pnlFieldDescription.Controls.Add(Me.FieldDescription)
        Me.pnlFieldDescription.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlFieldDescription.Location = New System.Drawing.Point(267, 0)
        Me.pnlFieldDescription.Name = "pnlFieldDescription"
        Me.pnlFieldDescription.Padding = New System.Windows.Forms.Padding(1, 7, 1, 0)
        Me.pnlFieldDescription.Size = New System.Drawing.Size(134, 70)
        Me.pnlFieldDescription.TabIndex = 11
        '
        'pnlPrompt
        '
        Me.pnlPrompt.BackColor = System.Drawing.Color.Transparent
        Me.pnlPrompt.Controls.Add(Me.btnPrompt)
        Me.pnlPrompt.Dock = System.Windows.Forms.DockStyle.Left
        Me.pnlPrompt.Location = New System.Drawing.Point(245, 0)
        Me.pnlPrompt.Name = "pnlPrompt"
        Me.pnlPrompt.Size = New System.Drawing.Size(22, 70)
        Me.pnlPrompt.TabIndex = 12
        Me.pnlPrompt.Visible = False
        '
        'btnPrompt
        '
        Me.btnPrompt.BackColor = System.Drawing.Color.SteelBlue
        Me.btnPrompt.BackgroundImage = CType(resources.GetObject("btnPrompt.BackgroundImage"), System.Drawing.Image)
        Me.btnPrompt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnPrompt.FlatAppearance.BorderSize = 0
        Me.btnPrompt.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnPrompt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPrompt.ForeColor = System.Drawing.Color.White
        Me.btnPrompt.Location = New System.Drawing.Point(0, 2)
        Me.btnPrompt.Margin = New System.Windows.Forms.Padding(0)
        Me.btnPrompt.Name = "btnPrompt"
        Me.btnPrompt.Size = New System.Drawing.Size(20, 20)
        Me.btnPrompt.TabIndex = 8
        Me.btnPrompt.TabStop = False
        Me.btnPrompt.UseVisualStyleBackColor = False
        '
        'pnlDataOuter
        '
        Me.pnlDataOuter.BackColor = System.Drawing.Color.Transparent
        Me.pnlDataOuter.Controls.Add(Me.pnlDataInner)
        Me.pnlDataOuter.Dock = System.Windows.Forms.DockStyle.Left
        Me.pnlDataOuter.Location = New System.Drawing.Point(141, 0)
        Me.pnlDataOuter.Name = "pnlDataOuter"
        Me.pnlDataOuter.Size = New System.Drawing.Size(104, 70)
        Me.pnlDataOuter.TabIndex = 14
        '
        'pnlDataInner
        '
        Me.pnlDataInner.BackColor = System.Drawing.Color.Transparent
        Me.pnlDataInner.Controls.Add(Me.FieldText)
        Me.pnlDataInner.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlDataInner.Location = New System.Drawing.Point(0, 0)
        Me.pnlDataInner.Name = "pnlDataInner"
        Me.pnlDataInner.Padding = New System.Windows.Forms.Padding(2)
        Me.pnlDataInner.Size = New System.Drawing.Size(104, 24)
        Me.pnlDataInner.TabIndex = 4
        '
        'FieldText
        '
        Me.FieldText.BackColor = System.Drawing.Color.LemonChiffon
        Me.FieldText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.FieldText.DataType = CTS.Controls.TextBoxCTS.DataTypes.StringType
        Me.FieldText.ErrorStyle = CTS.Controls.ErrorStyles.None
        Me.FieldText.ForeColor = System.Drawing.Color.Black
        Me.FieldText.Format = Nothing
        Me.FieldText.Location = New System.Drawing.Point(2, 2)
        Me.FieldText.Name = "FieldText"
        Me.FieldText.ReadOnlyBackColor = System.Drawing.Color.Transparent
        Me.FieldText.Size = New System.Drawing.Size(100, 20)
        Me.FieldText.TabIndex = 0
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "DefaultPrompt.png")
        '
        'cmsField
        '
        Me.cmsField.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.miProtect, Me.miUndo, Me.ToolStripSeparator1, Me.miCut, Me.miCopy, Me.miPaste, Me.miClear, Me.ToolStripSeparator2, Me.miProperties})
        Me.cmsField.Name = "cmsField"
        Me.cmsField.ShowCheckMargin = True
        Me.cmsField.ShowImageMargin = False
        Me.cmsField.Size = New System.Drawing.Size(128, 170)
        '
        'miProtect
        '
        Me.miProtect.CheckOnClick = True
        Me.miProtect.Name = "miProtect"
        Me.miProtect.Size = New System.Drawing.Size(127, 22)
        Me.miProtect.Text = "Protect"
        '
        'miUndo
        '
        Me.miUndo.Name = "miUndo"
        Me.miUndo.Size = New System.Drawing.Size(127, 22)
        Me.miUndo.Text = "Undo"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(124, 6)
        '
        'miCut
        '
        Me.miCut.Name = "miCut"
        Me.miCut.Size = New System.Drawing.Size(127, 22)
        Me.miCut.Text = "Cut"
        '
        'miCopy
        '
        Me.miCopy.Name = "miCopy"
        Me.miCopy.Size = New System.Drawing.Size(127, 22)
        Me.miCopy.Text = "Copy"
        '
        'miPaste
        '
        Me.miPaste.Name = "miPaste"
        Me.miPaste.Size = New System.Drawing.Size(127, 22)
        Me.miPaste.Text = "Paste"
        '
        'miClear
        '
        Me.miClear.Name = "miClear"
        Me.miClear.Size = New System.Drawing.Size(127, 22)
        Me.miClear.Text = "Clear"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(124, 6)
        '
        'miProperties
        '
        Me.miProperties.Name = "miProperties"
        Me.miProperties.Size = New System.Drawing.Size(127, 22)
        Me.miProperties.Text = "Properties"
        '
        'DataToolTip
        '
        Me.DataToolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.[Error]
        Me.DataToolTip.ToolTipTitle = "Error"
        '
        'CT2Field
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Transparent
        Me.ContextMenuStrip = Me.cmsField
        Me.Controls.Add(Me.pnlFieldDescription)
        Me.Controls.Add(Me.pnlPrompt)
        Me.Controls.Add(Me.pnlDataOuter)
        Me.Controls.Add(Me.pnlFieldLabel)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Name = "CT2Field"
        Me.Size = New System.Drawing.Size(401, 70)
        Me.pnlFieldLabel.ResumeLayout(False)
        Me.pnlFieldLabel.PerformLayout()
        Me.pnlFieldDescription.ResumeLayout(False)
        Me.pnlPrompt.ResumeLayout(False)
        Me.pnlDataOuter.ResumeLayout(False)
        Me.pnlDataInner.ResumeLayout(False)
        Me.pnlDataInner.PerformLayout()
        Me.cmsField.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnPrompt As System.Windows.Forms.Button
    Friend WithEvents pnlFieldLabel As System.Windows.Forms.Panel
    Friend WithEvents pnlFieldDescription As System.Windows.Forms.Panel
    Friend WithEvents pnlPrompt As System.Windows.Forms.Panel
    Friend WithEvents pnlDataOuter As System.Windows.Forms.Panel
    Public WithEvents FieldDescription As System.Windows.Forms.Label
    Public WithEvents FieldText As CTS.Controls.TextBoxCTS
    Public WithEvents FieldLabel As System.Windows.Forms.Label
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents pnlDataInner As System.Windows.Forms.Panel
    Friend WithEvents cmsField As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents miUndo As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents miCut As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents miCopy As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents miPaste As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents miClear As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents miProperties As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DataToolTip As ToolTip
    Friend WithEvents miProtect As ToolStripMenuItem
End Class
