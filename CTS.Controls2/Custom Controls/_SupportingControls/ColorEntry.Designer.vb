<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ColorEntry
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ColorEntry))
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.pbPulldown = New System.Windows.Forms.PictureBox()
        Me.ColorBox = New System.Windows.Forms.PictureBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        CType(Me.pbPulldown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ColorBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TextBox1
        '
        Me.TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextBox1.Location = New System.Drawing.Point(36, 2)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(85, 13)
        Me.TextBox1.TabIndex = 0
        '
        'pbPulldown
        '
        Me.pbPulldown.BackgroundImage = CType(resources.GetObject("pbPulldown.BackgroundImage"), System.Drawing.Image)
        Me.pbPulldown.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.pbPulldown.Dock = System.Windows.Forms.DockStyle.Right
        Me.pbPulldown.Location = New System.Drawing.Point(121, 2)
        Me.pbPulldown.Name = "pbPulldown"
        Me.pbPulldown.Size = New System.Drawing.Size(16, 13)
        Me.pbPulldown.TabIndex = 1
        Me.pbPulldown.TabStop = False
        '
        'ColorBox
        '
        Me.ColorBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ColorBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ColorBox.Location = New System.Drawing.Point(1, 1)
        Me.ColorBox.Name = "ColorBox"
        Me.ColorBox.Size = New System.Drawing.Size(25, 11)
        Me.ColorBox.TabIndex = 2
        Me.ColorBox.TabStop = False
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.ColorBox)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Left
        Me.Panel1.Location = New System.Drawing.Point(2, 2)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Padding = New System.Windows.Forms.Padding(1, 1, 8, 1)
        Me.Panel1.Size = New System.Drawing.Size(34, 13)
        Me.Panel1.TabIndex = 3
        '
        'ColorEntry
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.pbPulldown)
        Me.Name = "ColorEntry"
        Me.Padding = New System.Windows.Forms.Padding(2, 2, 0, 2)
        Me.Size = New System.Drawing.Size(137, 17)
        CType(Me.pbPulldown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ColorBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents pbPulldown As PictureBox

    Protected Overrides Sub SetBoundsCore(ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal specified As BoundsSpecified)
        Dim DEFAULT_CONTROL_HEIGHT As Integer = TextBox1.Height + 8

        If (specified And BoundsSpecified.Height) = 0 OrElse height = DEFAULT_CONTROL_HEIGHT Then
            MyBase.SetBoundsCore(x, y, width, DEFAULT_CONTROL_HEIGHT, specified)
        Else
            Return
        End If

    End Sub

    Friend WithEvents ColorBox As PictureBox
    Friend WithEvents Panel1 As Panel
End Class
