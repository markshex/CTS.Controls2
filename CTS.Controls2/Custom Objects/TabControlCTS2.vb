Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.ComponentModel
Imports System.Runtime.InteropServices

<ComVisible(False)>
Public Class TabControlCT2
    Inherits TabControl

    Private SlantUnits As Integer = 5

    '  Public Overloads Property TabPages As CTSTabPageCollection




    Sub New()
        'SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or ControlStyles.DoubleBuffer, True)
        'DoubleBuffered = True
    End Sub

    Protected Overrides Sub CreateHandle()
        MyBase.CreateHandle()
        Alignment = TabAlignment.Top
    End Sub

    Private CS As CustomStyles
    <System.ComponentModel.Category("Appearance")>
    <System.ComponentModel.Description("Custom Style")>
    <System.ComponentModel.Browsable(True)>
    <System.ComponentModel.EditorBrowsable(EditorBrowsableState.Always)>
    <System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)>
    Public Property CustomStyle As CustomStyles
        Get
            Return CS
        End Get
        Set(value As CustomStyles)
            CS = value

            'Set default for unselected tabs
            If CS = CustomStyles.None Then

                SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or ControlStyles.DoubleBuffer, False)
                DrawMode = TabDrawMode.Normal

            ElseIf CS = CustomStyles.Chrome Then

                SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or ControlStyles.DoubleBuffer, True)
                DrawMode = TabDrawMode.OwnerDrawFixed

                If USBC = Color.Empty Then
                    ActualUSBC = Color.LightGray
                Else
                    ActualUSBC = USBC
                End If
            ElseIf CS = CustomStyles.VisualStudio Then

                SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or ControlStyles.DoubleBuffer, True)
                DrawMode = TabDrawMode.OwnerDrawFixed

                If USBC = Color.Empty Then
                    ActualUSBC = Color.Transparent
                Else
                    ActualUSBC = USBC
                End If
            End If

        End Set
    End Property


    'Unselected
    Private USFC As Color = Color.Black
    <System.ComponentModel.Category("Appearance")>
    <System.ComponentModel.Description("Selected Tab Text Color")>
    <System.ComponentModel.Browsable(True)>
    <System.ComponentModel.EditorBrowsable(EditorBrowsableState.Always)>
    <System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)>
    Public Property UnselectedTabForecolor As Color
        Get
            Return USFC
        End Get
        Set(value As Color)
            If value = Color.Empty Then
                USFC = Color.Black
            Else
                USFC = value
            End If
        End Set
    End Property

    Private ActualUSBC As Color = Color.Empty
    Private USBC As Color = Color.Empty
    <System.ComponentModel.Category("Appearance")>
    <System.ComponentModel.Description("Selected Tab Background Color")>
    <System.ComponentModel.Browsable(True)>
    <System.ComponentModel.EditorBrowsable(EditorBrowsableState.Always)>
    <System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)>
    Public Property UnselectedTabBackcolor As Color
        Get
            Return USBC
        End Get
        Set(value As Color)
            USBC = value

            'Set default for unselected tabs
            If CS = CustomStyles.Chrome Then
                If USBC = Color.Empty Then
                    ActualUSBC = Color.LightGray
                Else
                    ActualUSBC = USBC
                End If
            ElseIf CS = CustomStyles.VisualStudio Then
                If USBC = Color.Empty Then
                    ActualUSBC = Color.Transparent
                Else
                    ActualUSBC = USBC
                End If
            End If
        End Set
    End Property



    'Selected
    Private SFC As Color = Color.Black
    <System.ComponentModel.Category("Appearance")>
    <System.ComponentModel.Description("Unselected Tab Text Color")>
    <System.ComponentModel.Browsable(True)>
    <System.ComponentModel.EditorBrowsable(EditorBrowsableState.Always)>
    <System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)>
    Public Property SelectedTabForecolor As Color
        Get
            Return SFC
        End Get
        Set(value As Color)
            If value = Color.Empty Then
                SFC = Color.Black
            Else
                SFC = value
            End If
        End Set
    End Property

    Private ActualSBC As Color = Color.Empty
    Private SBC As Color = Color.Empty
    <System.ComponentModel.Category("Appearance")>
    <System.ComponentModel.Description("Unselected Tab Background Color")>
    <System.ComponentModel.Browsable(True)>
    <System.ComponentModel.EditorBrowsable(EditorBrowsableState.Always)>
    <System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)>
    Public Property SelectedTabBackcolor As Color
        Get
            Return SBC
        End Get
        Set(value As Color)
            SBC = value
        End Set
    End Property



    'Hover 
    Private HFC As Color = Color.Black
    <System.ComponentModel.Category("Appearance")>
    <System.ComponentModel.Description("Hover Color of Tab Header Text")>
    <System.ComponentModel.Browsable(True)>
    <System.ComponentModel.EditorBrowsable(EditorBrowsableState.Always)>
    <System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)>
    Public Property HoverForecolor As Color
        Get
            Return HFC
        End Get
        Set(value As Color)
            If value = Color.Empty Then
                HFC = Color.Black
            Else
                HFC = value
            End If
        End Set
    End Property

    Private HBC As Color = Color.Empty
    <System.ComponentModel.Category("Appearance")>
    <System.ComponentModel.Description("Hover BackColor of Tab")>
    <System.ComponentModel.Browsable(True)>
    <System.ComponentModel.EditorBrowsable(EditorBrowsableState.Always)>
    <System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)>
    Public Property HoverBackcolor As Color
        Get
            Return HBC
        End Get
        Set(value As Color)
            HBC = value
        End Set
    End Property



    Private TabBorderColor As Color = Color.DimGray
    Private BorderPen As Pen = New Pen(TabBorderColor, 1)

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)

        If CS = CustomStyles.None Then
            MyBase.OnPaint(e)
        Else
            Debug.Print("Paint " & e.ClipRectangle.X & " " & e.ClipRectangle.Y & " " & e.ClipRectangle.Width & " " & e.ClipRectangle.Height)

            Dim B As New Bitmap(Width, Height)
            Dim G As Graphics = Graphics.FromImage(B)

            'Try : SelectedTab.BackColor = PC : Catch : End Try
            If SBC = Color.Empty Then
                Try : ActualSBC = SelectedTab.BackColor : Catch : End Try
            Else
                ActualSBC = SBC
            End If
            If USBC = Color.Empty And CustomStyle = CustomStyles.VisualStudio Then
                ActualUSBC = Parent.BackColor
            End If

            'Clear and set base color
            G.Clear(Parent.BackColor)
            G.SmoothingMode = SmoothingMode.AntiAlias

            'Draw line around the visible tabcontrol border
            'G.DrawRectangle(New Pen(Brushes.Red), New Rectangle(e.ClipRectangle.Location, New Size(e.ClipRectangle.Width - 1, e.ClipRectangle.Height - 1)))


            DrawBorder(G)

            'redraw each tab  
            For i = 0 To TabCount - 1
                If i = SelectedIndex Then
                    DrawSelectedTab(G, i)
                Else
                    DrawUnselectedTab(G, i)
                End If
            Next

            'apply drawing
            e.Graphics.DrawImage(B.Clone, 0, 0)
            G.Dispose() : B.Dispose()

        End If
    End Sub


    Dim HoverIndex As Integer = -1
    Protected Overrides Sub OnMouseMove(e As MouseEventArgs)

        MyBase.OnMouseMove(e)

        If CS = CustomStyles.None Then
        Else
            For idx As Integer = 0 To TabCount - 1
                If idx <> SelectedIndex Then

                    Dim tabrect As Rectangle = MyBase.GetTabRect(idx)
                    If tabrect.Contains(MyBase.PointToClient(Cursor.Position)) AndAlso idx <> HoverIndex Then

                        If HoverIndex > -1 And HoverIndex <> idx And HoverIndex <> SelectedIndex Then
                            DrawUnhoveredTab(HoverIndex)
                        End If

                        HoverIndex = idx
                        Debug.Print(idx.ToString)
                        DrawHoveredTab(idx)
                    End If
                End If
            Next
        End If
    End Sub

    Protected Overrides Sub OnMouseLeave(e As EventArgs)

        If CS = CustomStyles.None Then
        Else
            HoverIndex = -1
            Me.Refresh()
            Debug.Print(HoverIndex)
        End If

        MyBase.OnMouseLeave(e)
    End Sub

    Private Sub DrawBorder(ByRef g As Graphics)
        Select Case CS
            Case CustomStyles.VisualStudio
            Case Else
                'Outline the selected panel - push out to farthest boundaries
                If SelectedIndex > -1 Then
                    Dim r2 As Rectangle = Me.TabPages(SelectedIndex).Bounds
                    r2.Offset(-4, -1)
                    r2.Width = r2.Width + 7
                    r2.Height = r2.Height + 4

                    g.FillRectangle(New SolidBrush(SelectedTab.BackColor), r2)
                    g.DrawLine(BorderPen, r2.X, r2.Y, r2.X, r2.Y + r2.Height)
                    g.DrawLine(BorderPen, r2.X, r2.Y + r2.Height, r2.Width, r2.Y + r2.Height)
                    g.DrawLine(BorderPen, r2.X + r2.Width, r2.Y, r2.X + r2.Width, r2.Y + r2.Height)

                End If
        End Select
    End Sub

    Private Sub DrawSelectedTab(ByRef g As Graphics, ByVal Idx As Integer)
        Select Case CS
            Case CustomStyles.VisualStudio
                DrawSelectedVS(g, Idx)
            Case Else
                DrawSelectedChrome(g, Idx)
        End Select
    End Sub

    Private Sub DrawUnselectedTab(ByRef g As Graphics, ByVal Idx As Integer)
        Select Case CS
            Case CustomStyles.VisualStudio
                DrawUnselectedVS(g, Idx)
            Case Else
                DrawUnselectedChrome(g, Idx)
        End Select
    End Sub

    Private Sub DrawUnhoveredTab(ByVal Idx As Integer)
        Select Case CS
            Case CustomStyles.VisualStudio
                DrawUnhoveredVS(HoverIndex)
            Case Else
                DrawUnhoveredChrome(HoverIndex)
        End Select
    End Sub

    Private Sub DrawHoveredTab(ByVal Idx As Integer)
        Select Case CS
            Case CustomStyles.VisualStudio
                DrawHoveredVS(HoverIndex)
            Case Else
                DrawHoveredChrome(HoverIndex)
        End Select
    End Sub




#Region "Style: Chrome"

    Private Sub DrawSelectedChrome(ByRef g As Graphics, ByVal Idx As Integer)
        Dim rect As Rectangle = GetTabRect(Idx)
        rect.Height = rect.Height + 2

        'Draw outline around this tab index
        Dim path As New GraphicsPath
        path.AddLine(New Point(rect.X, rect.Y + rect.Height), New Point(rect.X + SlantUnits, rect.Y))
        path.AddLine(New Point(rect.X + SlantUnits, rect.Y), New Point(rect.X + rect.Width - SlantUnits, rect.Y))
        path.AddLine(New Point(rect.X + rect.Width - SlantUnits, rect.Y), New Point(rect.X + rect.Width, rect.Y + rect.Height))

        g.DrawPath(BorderPen, path)
        g.FillPath(New SolidBrush(ActualSBC), path)
        g.DrawLine(New Pen(ActualSBC, 2), rect.X, ItemSize.Height + 2, rect.X + rect.Width, ItemSize.Height + 2)


        'Draw line under tabs (excluding span under selected tab)
        g.DrawLine(BorderPen, 0, ItemSize.Height + 3, rect.X, ItemSize.Height + 3)
        g.DrawLine(BorderPen, rect.X + rect.Width, ItemSize.Height + 3, Me.Width, ItemSize.Height + 3)

        'Draw text in the tab
        g.DrawString(TabPages(Idx).Text, Font, ToBrush(SFC), rect,
                     New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})

    End Sub

    Private Sub DrawUnselectedChrome(ByRef g As Graphics, ByVal Idx As Integer)
        Dim rect As Rectangle = GetTabRect(Idx)

        'Draw line around this tab index
        Dim path As New GraphicsPath
        path.AddLine(New Point(rect.X, rect.Y + rect.Height), New Point(rect.X + SlantUnits, rect.Y))
        path.AddLine(New Point(rect.X + SlantUnits, rect.Y), New Point(rect.X + rect.Width - SlantUnits, rect.Y))
        path.AddLine(New Point(rect.X + rect.Width - SlantUnits, rect.Y), New Point(rect.X + rect.Width, rect.Y + rect.Height))

        g.DrawPath(BorderPen, path)
        g.FillPath(New SolidBrush(ActualUSBC), path)

        'Draw text in the tab
        g.DrawString(TabPages(Idx).Text, Font, ToBrush(USFC), rect,
                     New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})

    End Sub

    Private Sub DrawUnhoveredChrome(ByVal Idx As Integer)
        Dim rect As Rectangle = GetTabRect(Idx)

        Dim B As New Bitmap(rect.Width, rect.Height)
        Dim G As Graphics = Graphics.FromImage(B)
        G.SmoothingMode = SmoothingMode.AntiAlias

        'Draw outline around this tab index
        Dim path As New GraphicsPath
        path.AddLine(New Point(0, 0 + rect.Height), New Point(0 + SlantUnits, 0))
        path.AddLine(New Point(0 + SlantUnits, 0), New Point(0 + rect.Width - SlantUnits, 0))
        path.AddLine(New Point(0 + rect.Width - SlantUnits, 0), New Point(0 + rect.Width, 0 + rect.Height))

        G.DrawPath(BorderPen, path)
        G.FillPath(New SolidBrush(ActualUSBC), path)

        'Draw text in the tab
        G.DrawString(TabPages(Idx).Text, Font, ToBrush(USFC), New Rectangle(0, 0, rect.Width, rect.Height),
                     New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})

        Dim g2 As Graphics = MyBase.CreateGraphics()
        g2.DrawImage(B.Clone, rect)
        g2.Dispose()

        G.Dispose() : B.Dispose()
    End Sub

    Private Sub DrawHoveredChrome(ByVal Idx As Integer)
        Dim rect As Rectangle = GetTabRect(Idx)

        Dim B As New Bitmap(rect.Width, rect.Height)
        Dim G As Graphics = Graphics.FromImage(B)
        G.SmoothingMode = SmoothingMode.AntiAlias

        'Draw outline around this tab index
        Dim path As New GraphicsPath
        path.AddLine(New Point(0, 0 + rect.Height), New Point(0 + SlantUnits, 0))
        path.AddLine(New Point(0 + SlantUnits, 0), New Point(0 + rect.Width - SlantUnits, 0))
        path.AddLine(New Point(0 + rect.Width - SlantUnits, 0), New Point(0 + rect.Width, 0 + rect.Height))
        G.DrawPath(BorderPen, path)
        G.FillPath(New SolidBrush(HBC), path)

        'Draw text in the tab
        G.DrawString(TabPages(Idx).Text, Font, ToBrush(HFC), New Rectangle(0, 0, rect.Width, rect.Height),
                     New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})

        Dim g2 As Graphics = MyBase.CreateGraphics()
        g2.DrawImage(B.Clone, rect)
        g2.Dispose()

        G.Dispose() : B.Dispose()
    End Sub

#End Region

#Region "Style: Visual Studio"

    Private Sub DrawSelectedVS(ByRef g As Graphics, ByVal Idx As Integer)
        Dim rect As Rectangle = GetTabRect(Idx)

        g.FillRectangle(New SolidBrush(ActualSBC), rect)

        'Draw line under all tabs  
        g.DrawLine(New Pen(ActualSBC, 3), 2, ItemSize.Height + 2, Me.Width - 4, ItemSize.Height + 2)

        'Draw text in the tab
        g.DrawString(TabPages(Idx).Text, Font, ToBrush(SFC), rect,
                     New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})

    End Sub

    Private Sub DrawUnselectedVS(ByRef g As Graphics, ByVal Idx As Integer)
        Dim rect As Rectangle = GetTabRect(Idx)
        rect.Height = rect.Height - 2

        g.FillRectangle(New SolidBrush(ActualUSBC), rect)

        'Draw text in the tab
        g.DrawString(TabPages(Idx).Text, Font, ToBrush(USFC), rect,
                     New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})

    End Sub

    Private Sub DrawUnhoveredVS(ByVal Idx As Integer)
        Dim rect As Rectangle = GetTabRect(Idx)
        rect.Height = rect.Height - 1

        Dim B As New Bitmap(rect.Width, rect.Height)
        Dim G As Graphics = Graphics.FromImage(B)
        G.SmoothingMode = SmoothingMode.AntiAlias

        G.FillRectangle(New SolidBrush(ActualUSBC), New Rectangle(0, 0, rect.Width, rect.Height))

        'Draw text in the tab
        G.DrawString(TabPages(Idx).Text, Font, ToBrush(USFC), New Rectangle(0, 0, rect.Width, rect.Height),
                     New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})

        Dim g2 As Graphics = MyBase.CreateGraphics()
        g2.DrawImage(B.Clone, rect)
        g2.Dispose()

        G.Dispose() : B.Dispose()
    End Sub

    Private Sub DrawHoveredVS(ByVal Idx As Integer)
        Dim rect As Rectangle = GetTabRect(Idx)
        rect.Height = rect.Height - 1

        Dim B As New Bitmap(rect.Width, rect.Height)
        Dim G As Graphics = Graphics.FromImage(B)
        G.SmoothingMode = SmoothingMode.AntiAlias

        G.FillRectangle(New SolidBrush(HBC), New Rectangle(0, 0, rect.Width, rect.Height))

        'Draw text in the tab
        G.DrawString(TabPages(Idx).Text, Font, ToBrush(HFC), New Rectangle(0, 0, rect.Width, rect.Height),
                     New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})

        Dim g2 As Graphics = MyBase.CreateGraphics()
        g2.DrawImage(B.Clone, rect)
        g2.Dispose()

        G.Dispose() : B.Dispose()
    End Sub

#End Region

    Public Enum CustomStyles
        None
        Chrome
        VisualStudio
    End Enum

    Function ToBrush(ByVal color As Color) As Brush
        Return New SolidBrush(color)
    End Function


End Class


Class CTSTabPageCollection

End Class

Class CTSTabPage
    Inherits TabPage


    Property Loaded As Boolean
    Overloads Property [Enabled] As Boolean
    Overloads Property [Visible] As Boolean



End Class