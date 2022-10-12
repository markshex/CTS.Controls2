Imports System.ComponentModel
Imports System.Runtime.InteropServices

Class ToolStripEntryHost
    Inherits ToolStripControlHost

    'Sub New()
    '    MyBase.New(New Control)
    'End Sub

    Sub New(ByVal LabelText As String, ByVal EntryText As String, Name As String)
        MyBase.New(New tsTextEntry(LabelText, EntryText), Name)

        Dim tse As tsTextEntry = Me.Control
        AddHandler tse.Done, AddressOf Me.DoneHandled
        AddHandler tse.TextChanged, AddressOf Me.TextChangedHandled
        tse.myHost = Me

    End Sub

    Public Event Done(sender As Object, e As EventArgs)
    Public Shadows Event TextChanged(sender As Object, e As EventArgs)

    Sub DoneHandled(ByVal sender As Object, e As EventArgs)
        RaiseEvent Done(sender, e)
    End Sub

    Sub TextChangedHandled(ByVal sender As Object, e As EventArgs)
        RaiseEvent TextChanged(sender, e)
    End Sub

End Class


Public Class tsTextEntry
    Inherits UserControl

#Region "Properties/Declarations"

    Property LabelText() As String
    Property EntryText() As String

    Public Label As New Label
    Public TextBox As New TextBox
    Public Button As New Button
    Public myHost As Object

    Public Event Done(sender As Object, e As EventArgs)
    Public Shadows Event TextChanged(sender As Object, e As EventArgs)

#End Region

    Sub New(ByVal LabelText As String, ByVal EntryText As String)
        MyBase.New

        Me.LabelText = LabelText
        Me.EntryText = EntryText
        Initialize()
    End Sub

    Private Sub Initialize()
        SetStyle(ControlStyles.Selectable, True)
        Me.TabStop = True

        Me.BorderStyle = BorderStyle.None
        Me.BackColor = Color.Transparent
        Me.MinimumSize = New Size(200, 24)
        Me.AutoSizeMode = False

        Label.Text = LabelText
        Label.TextAlign = ContentAlignment.MiddleLeft
        Label.Dock = DockStyle.Left
        Label.AutoSize = True
        Label.MinimumSize = New Size(0, 22)

        Dim fillerLeft As New Button
        fillerLeft.MaximumSize = New Size(1, 1)
        fillerLeft.MinimumSize = New Size(1, 1)
        fillerLeft.Dock = DockStyle.Left
        TextBox.TabIndex = 0

        TextBox.Text = EntryText
        TextBox.Dock = DockStyle.Fill
        TextBox.BorderStyle = BorderStyle.FixedSingle
        TextBox.BackColor = EditBack
        TextBox.Font = New Font(TextBox.Font.FontFamily, 9)
        TextBox.TabStop = True
        TextBox.TabIndex = 1

        Button.Text = "OK"
        Button.FlatStyle = FlatStyle.Flat
        Button.Font = New Font(TextBox.Font.FontFamily, 8)
        Button.MaximumSize = New Size(36, 22)
        Button.MinimumSize = New Size(36, 22)
        Button.BackColor = GridPrimary
        Button.ForeColor = Color.White
        Button.Dock = DockStyle.Right
        TextBox.TabIndex = 2

        Dim fillerright As New Button
        fillerright.MaximumSize = New Size(1, 1)
        fillerright.MinimumSize = New Size(1, 1)
        fillerright.Dock = DockStyle.Right
        TextBox.TabIndex = 3

        Controls.Add(fillerLeft)
        Controls.Add(Label)
        Controls.Add(TextBox)
        Controls.Add(Button)
        Controls.Add(fillerright)

        fillerLeft.BringToFront()
        Label.BringToFront()
        Button.BringToFront()
        TextBox.BringToFront()
        fillerright.SendToBack()

        AddHandler TextBox.TextChanged, AddressOf EntryTextChanged
        AddHandler TextBox.PreviewKeyDown, AddressOf TextBoxPreviewKeyDown
        AddHandler Button.PreviewKeyDown, AddressOf ButtonPreviewKeyDown
        AddHandler Button.Click, AddressOf ButtonClicked
    End Sub

    Public Sub ButtonLeave(sender As Object, e As EventArgs)
        Me.Text = TextBox.Text
    End Sub
    Public Sub EntryTextChanged(sender As Object, e As EventArgs)
        Me.Text = TextBox.Text
        RaiseEvent TextChanged(sender, e)
    End Sub

    Public Sub TextBoxPreviewKeyDown(sender As Object, e As PreviewKeyDownEventArgs)

        If e.KeyCode = Keys.Enter Then
            Me.Text = TextBox.Text
            RaiseEvent Done(sender, New EventArgs)
        End If

        If e.KeyCode = Keys.Up Then
            Dim cms As ContextMenuStrip = TryCast(Me.Parent, ContextMenuStrip)
            If cms IsNot Nothing Then
                SelectPreviousItem(Me, cms.Items)
            End If
        End If

        If e.KeyCode = Keys.Down Then
            Dim cms As ContextMenuStrip = TryCast(Me.Parent, ContextMenuStrip)
            If cms IsNot Nothing Then
                SelectNextItem(Me, cms.Items)
            End If
        End If

        If (e.Shift And e.KeyCode = Keys.Tab) Then
            Dim cms As ContextMenuStrip = TryCast(Me.Parent, ContextMenuStrip)
            If cms IsNot Nothing Then
                SelectPreviousItem(Me, cms.Items)
            End If
        End If

    End Sub
    Public Sub ButtonPreviewKeyDown(sender As Object, e As PreviewKeyDownEventArgs)

        If e.KeyCode = Keys.Enter Then
            Me.Text = TextBox.Text
            RaiseEvent Done(sender, New EventArgs)
        End If

        If e.KeyCode = Keys.Up Then
            Dim cms As ContextMenuStrip = TryCast(Me.Parent, ContextMenuStrip)
            If cms IsNot Nothing Then
                SelectPreviousItem(Me, cms.Items)
            End If
        End If

        If e.KeyCode = Keys.Down Then
            Dim cms As ContextMenuStrip = TryCast(Me.Parent, ContextMenuStrip)
            If cms IsNot Nothing Then
                SelectNextItem(Me, cms.Items)
            End If
        End If


        If (Not e.Shift And e.KeyCode = Keys.Tab) Then
            Dim cms As ContextMenuStrip = TryCast(Me.Parent, ContextMenuStrip)
            If cms IsNot Nothing Then
                SelectNextItem(Me, cms.Items)
            End If

        End If

    End Sub

    Public Sub ButtonClicked(sender As Object, e As EventArgs)
        Me.Text = TextBox.Text
        RaiseEvent Done(sender, e)
    End Sub

    Public Sub MyBase_LostFocus(sender As Object, e As EventArgs) Handles MyBase.LostFocus
        TextBox.Focus()
    End Sub
    Public Sub MyBase_Click(sender As Object, e As EventArgs) Handles MyBase.Click
        TextBox.Focus()
    End Sub

    Private Sub SelectNextItem(c As Control, tsic As ToolStripItemCollection)
        Dim found As Boolean
        Dim i As Short = 0

        For i = 0 To tsic.Count - 1
            Dim itm As Object = tsic.Item(i)
            If found And itm.CanSelect And itm.visible Then
                itm.Select()
                If itm.GetType = GetType(ToolStripEntryHost) Or
                    itm.GetType = GetType(ToolStripControlHost) Then
                    itm.focus
                End If
                Debug.Print("next " & itm.Name)
                Exit For
            End If
            If itm.GetType = GetType(ToolStripEntryHost) Then
                Dim hostitem As ToolStripEntryHost = itm
                If hostitem.Control IsNot Nothing AndAlso hostitem.Control Is c Then
                    found = True
                End If
            End If
        Next

    End Sub

    Private Sub SelectPreviousItem(c As Control, tsic As ToolStripItemCollection)
        Dim found As Boolean
        Dim i As Short = 0

        For i = tsic.Count - 1 To 0 Step -1
            Dim itm As Object = tsic.Item(i)
            If found And itm.CanSelect And itm.visible Then
                itm.Select()
                If itm.GetType = GetType(ToolStripEntryHost) Or
                    itm.GetType = GetType(ToolStripControlHost) Then
                    itm.focus
                End If
                Debug.Print("prev " & itm.Name)
                Exit For
            End If
            If itm.GetType = GetType(ToolStripEntryHost) Then
                Dim hostitem As ToolStripEntryHost = itm
                If hostitem.Control IsNot Nothing AndAlso hostitem.Control Is c Then
                    found = True
                End If
            End If
        Next

    End Sub


End Class

Public Class tsStringFilter
    Inherits UserControl

#Region "Properties/Declarations"

    Property LabelText() As String

    Public Value As New Label
    Public btnEq As New Button
    Public btnNotEq As New Button
    Public myHost As Object

    Public Event Done(sender As Object, e As EventArgs)
    Public Shadows Event TextChanged(sender As Object, e As EventArgs)

#End Region

    Sub New(ByVal Value As String)
        MyBase.New
        Me.LabelText = Value
        Initialize()
    End Sub

    Private Sub Initialize()
        SetStyle(ControlStyles.Selectable, True)
        Me.TabStop = True

        Me.BorderStyle = BorderStyle.None
        Me.BackColor = Color.Transparent
        Me.MinimumSize = New Size(200, 24)
        Me.AutoSizeMode = False

        Value.Text = LabelText
        Value.TextAlign = ContentAlignment.MiddleLeft
        Value.Dock = DockStyle.Left
        Value.AutoSize = True
        Value.MinimumSize = New Size(0, 22)

        btnEq.Text = "="
        btnEq.FlatStyle = FlatStyle.Flat
        btnEq.Font = New Font(btnEq.Font.FontFamily, 8)
        btnEq.MaximumSize = New Size(36, 22)
        btnEq.MinimumSize = New Size(36, 22)
        btnEq.BackColor = GridPrimary
        btnEq.ForeColor = Color.White
        btnEq.Dock = DockStyle.Right

        btnNotEq.Text = "="
        btnNotEq.FlatStyle = FlatStyle.Flat
        btnNotEq.Font = New Font(btnEq.Font.FontFamily, 8)
        btnNotEq.MaximumSize = New Size(36, 22)
        btnNotEq.MinimumSize = New Size(36, 22)
        btnNotEq.BackColor = GridPrimary
        btnNotEq.ForeColor = Color.White
        btnNotEq.Dock = DockStyle.Right

        Controls.Add(Value)
        Controls.Add(btnEq)
        Controls.Add(btnNotEq)

        Value.BringToFront()
        btnEq.BringToFront()
        btnNotEq.BringToFront()

        AddHandler btnEq.PreviewKeyDown, AddressOf ButtonPreviewKeyDown
        AddHandler btnEq.Click, AddressOf ButtonClicked

        AddHandler btnNotEq.PreviewKeyDown, AddressOf ButtonPreviewKeyDown
        AddHandler btnNotEq.Click, AddressOf ButtonClicked
    End Sub


    Public Sub ButtonPreviewKeyDown(sender As Object, e As PreviewKeyDownEventArgs)

        If e.KeyCode = Keys.Enter Then
            'Me.Text = TextBox.Text
            RaiseEvent Done(sender, New EventArgs)
        End If

        If e.KeyCode = Keys.Up Then
            Dim cms As ContextMenuStrip = TryCast(Me.Parent, ContextMenuStrip)
            If cms IsNot Nothing Then
                SelectPreviousItem(Me, cms.Items)
            End If
        End If

        If e.KeyCode = Keys.Down Then
            Dim cms As ContextMenuStrip = TryCast(Me.Parent, ContextMenuStrip)
            If cms IsNot Nothing Then
                SelectNextItem(Me, cms.Items)
            End If
        End If


        If (Not e.Shift And e.KeyCode = Keys.Tab) Then
            Dim cms As ContextMenuStrip = TryCast(Me.Parent, ContextMenuStrip)
            If cms IsNot Nothing Then
                SelectNextItem(Me, cms.Items)
            End If

        End If

    End Sub

    Public Sub ButtonClicked(sender As Object, e As EventArgs)
        'Me.Text = TextBox.Text
        RaiseEvent Done(sender, e)
    End Sub

    Public Sub MyBase_LostFocus(sender As Object, e As EventArgs) Handles MyBase.LostFocus
    End Sub
    Public Sub MyBase_Click(sender As Object, e As EventArgs) Handles MyBase.Click
    End Sub

    Private Sub SelectNextItem(c As Control, tsic As ToolStripItemCollection)
        Dim found As Boolean
        Dim i As Short = 0

        For i = 0 To tsic.Count - 1
            Dim itm As Object = tsic.Item(i)
            If found And itm.CanSelect And itm.visible Then
                itm.Select()
                If itm.GetType = GetType(ToolStripEntryHost) Or
                    itm.GetType = GetType(ToolStripControlHost) Then
                    itm.focus
                End If
                Debug.Print("next " & itm.Name)
                Exit For
            End If
            If itm.GetType = GetType(ToolStripEntryHost) Then
                Dim hostitem As ToolStripEntryHost = itm
                If hostitem.Control IsNot Nothing AndAlso hostitem.Control Is c Then
                    found = True
                End If
            End If
        Next

    End Sub

    Private Sub SelectPreviousItem(c As Control, tsic As ToolStripItemCollection)
        Dim found As Boolean
        Dim i As Short = 0

        For i = tsic.Count - 1 To 0 Step -1
            Dim itm As Object = tsic.Item(i)
            If found And itm.CanSelect And itm.visible Then
                itm.Select()
                If itm.GetType = GetType(ToolStripEntryHost) Or
                    itm.GetType = GetType(ToolStripControlHost) Then
                    itm.focus
                End If
                Debug.Print("prev " & itm.Name)
                Exit For
            End If
            If itm.GetType = GetType(ToolStripEntryHost) Then
                Dim hostitem As ToolStripEntryHost = itm
                If hostitem.Control IsNot Nothing AndAlso hostitem.Control Is c Then
                    found = True
                End If
            End If
        Next

    End Sub


End Class
