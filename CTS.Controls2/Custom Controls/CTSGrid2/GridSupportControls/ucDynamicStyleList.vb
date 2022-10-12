Public Class ucDynamicStyleList
    Private Sub ucDynamicStyles_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim dh As New CTS.Data
        dh.SetConnectionString("GP")
        Dim dt = dh.GetTable("Select * from DNPF2535")
        DataGridView1.DataSource = dt
    End Sub
End Class
