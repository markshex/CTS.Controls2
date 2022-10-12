Imports System.ComponentModel

Public Class CalDay
    Inherits PictureBox

    Private gBackColor As Color
    Private gDayBack As Color = Color.SteelBlue
    Private gDayBackHover As Color = Color.LightSteelBlue
    Private gDayBackSelected As Color = Color.Red

    Private gForeColor As Color
    Private gDayFore As Color = Color.White
    Private gDayForeHover As Color = Color.Black
    Private gDayForeSelected As Color = Color.White


    <Category("Appearance")>
    <Description("Color for the Day")>
    Public Property DayColor As Color
        Get
            Return gDayBack
        End Get
        Set(value As Color)
            gDayBack = value
        End Set
    End Property


    <Category("Appearance")>
    <Description("Color when Hovering")>
    Public Property DayHoverColor As Color
        Get
            Return gDayBackHover
        End Get
        Set(value As Color)
            gDayBackHover = value
        End Set
    End Property


    <Category("Appearance")>
    <Description("Color when Selected")>
    Public Property DaySelectedColor As Color
        Get
            Return gDayBackSelected
        End Get
        Set(value As Color)
            gDayBackSelected = value
        End Set
    End Property


    Private _Day As DateTime
    Public Property Day As DateTime
        Get
            Return _Day
        End Get
        Set(value As DateTime)
            _Day = value
            If Not IsNothing(value) Then
                Me.Text = _Day.Day
            End If
        End Set
    End Property




    <EditorBrowsable(EditorBrowsableState.Always)>
    <Browsable(True)>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)>
    <Bindable(True)>
    Public Overrides Property Text() As String


    '<EditorBrowsable(EditorBrowsableState.Always)>
    '<Browsable(True)>
    '<DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)>
    '<Bindable(True)>
    'Public Overrides Property ForeColor() As Color = Color.White



    <Category("Appearance")>
    <Description("Font for the Day")>
    Public Property DayFont As Font = New Font(DefaultFont.FontFamily, 12, FontStyle.Bold)
    'Public Property DayFont As Font = New Font(DefaultFont.FontFamily, DefaultFont.Size, FontStyle.Bold)

    Sub New()

    End Sub

    Sub New(ByVal CalendarDay As DateTime)
        Day = CalendarDay
        BackColor = Color.Transparent

        gBackColor = gDayBack
        gForeColor = gDayFore
    End Sub

    Protected Overrides Sub OnPaint(pe As PaintEventArgs)

        Paint1(pe)

        MyBase.OnPaint(pe)
    End Sub

    Private Sub Paint1(pe As PaintEventArgs)
        Dim textsize As SizeF = pe.Graphics.MeasureString(Text, DayFont)

        Dim pad As New Padding(5, 4, 8, 4)

        Using b1 As New SolidBrush(gBackColor)
            pe.Graphics.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
            pe.Graphics.FillEllipse(b1, New Rectangle(0, 0,
                                                      textsize.Width + pad.Left + pad.Right,
                                                      textsize.Height + pad.Top + pad.Bottom))
        End Using

        Dim drawFormat As New StringFormat
        'drawFormat.Alignment = StringAlignment.Center


        Using b2 As New SolidBrush(gForeColor)
            pe.Graphics.DrawString(Me.Text,
                               DayFont, b2,
                               New Rectangle(pad.Left, pad.Top,
                                             textsize.Width + pad.Left + pad.Right,
                                             textsize.Height + pad.Top + pad.Bottom),
                               drawFormat)
        End Using

        Me.Size = New Size(textsize.Width + pad.Left + pad.Right, textsize.Height + pad.Top + pad.Bottom)
    End Sub

    Private Sub Paint2(pe As PaintEventArgs)
        ExtendedDraw(pe)
        DrawBorder(pe.Graphics)
    End Sub

    Private Sub ExtendedDraw(ByVal e As PaintEventArgs)
        Dim edge = 2

        e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
        Dim path As Drawing2D.GraphicsPath = New Drawing2D.GraphicsPath()
        path.StartFigure()
        path.StartFigure()
        path.AddArc(GetLeftUpper(Edge), 180, 90)
        path.AddLine(Edge, 0, Width - Edge, 0)
        path.AddArc(GetRightUpper(Edge), 270, 90)
        path.AddLine(Width, Edge, Width, Height - Edge)
        path.AddArc(GetRightLower(Edge), 0, 90)
        path.AddLine(Width - Edge, Height, Edge, Height)
        path.AddArc(GetLeftLower(Edge), 90, 90)
        path.AddLine(0, Height - Edge, 0, Edge)
        path.CloseFigure()
        Region = New Region(path)
    End Sub

    Private Sub DrawBorder(ByVal graphics As Graphics)
        Dim edge = 2
        Dim Pen As Pen = New Pen(Color.Black, 1.0F)
        graphics.DrawArc(Pen, New Rectangle(0, 0, edge, edge), 180, 90)
        graphics.DrawArc(Pen, New Rectangle(Width - edge - 1, -1, edge, edge), 270, 90)
        graphics.DrawArc(Pen, New Rectangle(Width - edge - 1, Height - edge - 1, edge, edge), 0, 90)
        graphics.DrawArc(Pen, New Rectangle(0, Height - edge - 1, edge, edge), 90, 90)
        graphics.DrawRectangle(Pen, 0.0F, 0.0F, CType((Width - 1), Single), CType((Height - 1), Single))
    End Sub

    Private Function GetLeftUpper(ByVal e As Integer) As Rectangle
        Return New Rectangle(0, 0, e, e)
    End Function

    Private Function GetRightUpper(ByVal e As Integer) As Rectangle
        Return New Rectangle(Width - e, 0, e, e)
    End Function

    Private Function GetRightLower(ByVal e As Integer) As Rectangle
        Return New Rectangle(Width - e, Height - e, e, e)
    End Function

    Private Function GetLeftLower(ByVal e As Integer) As Rectangle
        Return New Rectangle(0, Height - e, e, e)
    End Function

    Private Sub InitializeComponent()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'CalDay
        '
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Private Sub CalDay_MouseEnter(sender As Object, e As EventArgs) Handles MyBase.MouseEnter
        gBackColor = gDayBackHover
        gForeColor = gDayForeHover
        Refresh()
    End Sub

    Private Sub CalDay_MouseLeave(sender As Object, e As EventArgs) Handles MyBase.MouseLeave
        gBackColor = gDayBack
        gForeColor = gDayFore
        Refresh()
    End Sub
End Class
