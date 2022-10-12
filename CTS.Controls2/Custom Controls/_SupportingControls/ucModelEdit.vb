Imports System.ComponentModel
Imports System.Reflection


Public Class ucModelEdit

    Private gCurrentDataItem As Object = Nothing
    Private gSourceData As Object = Nothing
    <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Property SourceData() As Object
        Get
            Return gSourceData
        End Get
        Set(value As Object)
            gSourceData = value
        End Set
    End Property


    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub


    Sub New(model As Object)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        gSourceData = model
    End Sub



    Sub init()

        Me.Controls.Clear()

        Dim flds = GetInput(gSourceData)

        Me.SuspendLayout()

        For Each fld In flds
            Me.Controls.Add(fld)
            fld.Dock = DockStyle.Top
        Next

        Me.ResumeLayout()

    End Sub

#Region "Model Translate"

    Private Function TranslateDataObject(ByVal DataObject As Object) As DataTable
        Dim result As DataTable = Nothing

        If DataObject.GetType.IsGenericType Then
            For Each itm In DataObject
                TranslateDataObjectItem(itm, result)
            Next
        End If

        Return result
    End Function

    Friend Function GetInput(ByVal DataItem As Object) As List(Of CT2Input)

        Dim result As New List(Of CT2Input)

        Dim t As Type = DataItem.GetType()

        If gCurrentDataItem Is Nothing Then
            gCurrentDataItem = CloneObject(DataItem)
        End If

        'Get the properties of our type
        Dim props As PropertyInfo() = t.GetProperties()
        For Each xtemp2 As PropertyInfo In props
            Dim ct2i As New CT2Input()
            ct2i.Name = $"fld{xtemp2.Name}"
            ct2i.LabelText = xtemp2.Name
            result.Add(ct2i)
        Next


        'Now the table should exist so add records to it.
        Dim tmpObj As Object = New Object(props.Length - 1) {}
        For i As Int32 = 0 To tmpObj.Length - 1
            tmpObj(i) = t.InvokeMember(props(i).Name, BindingFlags.GetProperty, Nothing, DataItem, New Object(-1) {})
        Next


        Return result
    End Function




    Private Sub TranslateDataObject(ByVal DataTable As DataTable, ByRef DataObject As Object)

        DataObject.Clear
        For Each dr In DataTable.Rows
            Dim DataItem = CloneObject(gCurrentDataItem)
            TranslateDataObjectItem(dr, DataItem)
            DataObject.add(DataItem)
        Next

    End Sub

    Public Shared Function CloneObject(Of T As Class)(obj As T) As T
        If obj Is Nothing Then
            Return Nothing
        End If
        Dim inst As System.Reflection.MethodInfo = obj.[GetType]().GetMethod("MemberwiseClone", System.Reflection.BindingFlags.Instance Or System.Reflection.BindingFlags.NonPublic)
        If inst IsNot Nothing Then
            Return DirectCast(inst.Invoke(obj, Nothing), T)
        Else
            Return Nothing
        End If
    End Function

    Friend Sub TranslateDataObjectItem(ByVal DataItem As Object, ByRef ToTable As DataTable)
        Dim t As Type = DataItem.GetType()

        If gCurrentDataItem Is Nothing Then
            gCurrentDataItem = CloneObject(DataItem)
        End If

        'Get the properties of our type
        Dim props As PropertyInfo() = t.GetProperties()

        'We need to create the table if it doesn't already exist
        If ToTable Is Nothing Then
            ToTable = New DataTable

            'Create the columns of the table based off the properties we reflected from the type
            For Each xtemp2 As PropertyInfo In props
                ToTable.Columns.Add(xtemp2.Name, xtemp2.PropertyType)
            Next
        End If

        'Now the table should exist so add records to it.
        Dim tmpObj As Object = New Object(props.Length - 1) {}
        For i As Int32 = 0 To tmpObj.Length - 1
            tmpObj(i) = t.InvokeMember(props(i).Name, BindingFlags.GetProperty, Nothing, DataItem, New Object(-1) {})
        Next

        'Add the row to the table in the dataset
        ToTable.LoadDataRow(tmpObj, True)
    End Sub

    Public Sub TranslateDataObjectItem(ByVal dr As DataRow, ByRef itm As Object)

        'This is used to do the reflection
        Dim t As Type = itm.GetType()

        Dim dcc As DataColumnCollection = dr.Table.Columns
        For i As Int32 = 0 To dcc.Count - 1
            Try
                'NOTE the datarow column names must match exactly (including case) to the object property names
                t.InvokeMember(dcc(i).ColumnName, BindingFlags.SetProperty, Nothing, itm, New Object() {dr(dcc(i).ColumnName)})
            Catch ex As Exception
                'Usually you are getting here because a column doesn't exist or it is null
                If ex.ToString() IsNot Nothing Then
                End If
            End Try
        Next

    End Sub


#End Region



End Class
