Imports System.Collections.ObjectModel

Public Class Grid
    Property ID As Integer
    Property VersionID As Integer
    Property Name As String
    Property Title As String
    Property LibraryList As String
    Property FromClause As String
    Property WhereClause As String
    Property FilterClause As String
    Property OrderByClause As String
    Property GroupByClause As String
    Property Distinct As Boolean
    Property TotalsRow As Boolean

    Property Columns As New GridColumnCollection
    Property RowOptions As New RowOptionCollection
    Property GridFunctions As New GridFunctionCollection
    Property DynamicStyles As New DynamicStyleCollection
    Property UpdateTables As New List(Of String)

    Property AltDataSource As GridDataSource

    Friend _Statement As String
    ReadOnly Property Statement As String
        Get
            Return _Statement
        End Get
    End Property

End Class

Public Class GridDataSource
    Property SourceType As CTS.Data.DataSourceTypes
    Property Server As String
    Property Database As String
    Property ConnectionString As String
End Class

Public Class GridColumn

    Property Name As String
    Property Field As String
    Property File As String
    Property Text As String
    Property HeaderText As String
    Property Seq As Short
    Property SortSeq As Short
    Property SortOrder As SortOrder
    Property Width As Short
    Property Restrict As Boolean
    Property Hidden As Boolean
    Property ColumnType As GridColumnTypes


    Property ProviderTypeCode As String
    Property Size As Short
    Property Scale As Short

    Property Provider As ProviderType


    Property Format As String
    Property SQLFunction As String
    Property Alignment As GeneralAlignment
    Property Updatable As Boolean
    Property CastCCSID37 As Boolean
    Property Preselect As Boolean
    Property SuppressTrim As Boolean
    Property SumColumn As Boolean
    Property SumFormat As String
    Property SecurityType As SecurityColumnType
    Property [ReadOnly] As Boolean
    Property ScanFilter As String

    Property DistinctLookup As Boolean
    Property ValueList As New List(Of String)
    Property ValueTable As DataTable

    Public Overloads Function tostring() As String
        Return Name
    End Function

    Public Function NewFilter(ByVal [Operator] As Filtering.OperatorName, ByVal Value As String) As Filtering.Condition
        Dim Result As New Filtering.Condition
        Result.LOP = Filtering.LogicalOperator.And
        Result.ColumnName = Name
        Result.Op = [Operator]
        Result.Value = Value
        Result.UseDataType(GetSQLType(Me.ProviderTypeCode))
        Return Result
    End Function

    Public Function NewFilter(ByVal ConditionText As String) As Filtering.Condition
        Dim Result As New Filtering.Condition
        Result.LOP = Filtering.LogicalOperator.And
        Result.ColumnName = Name
        Result.Op = Filtering.OperatorName.None
        Result.Value = String.Empty
        Result.Text = ConditionText
        Result.Quotes = Filtering.QuoteFormatting.Manual
        Return Result
    End Function

    Private Function GetSQLType(ByVal ProviderTypeCode As String) As Filtering.SQLTypes
        Dim result As Filtering.SQLTypes

        Select Case ProviderTypeCode
            Case "1", "2", "3", "4", "5"
                Return Filtering.SQLTypes.Numeric
            Case "12", "13", "14"
                Return Filtering.SQLTypes.DateTime
            Case Else
                Return Filtering.SQLTypes.String
        End Select

        Return result
    End Function

End Class

Public Class GridColumnCollection
    Inherits KeyedCollection(Of String, GridColumn)

    Property Clause As String

    Protected Overrides Function GetKeyForItem(ByVal item As GridColumn) As String
        Return item.Name
    End Function

End Class

Public Class RowOption
    Property Sequence As Short
    Property Name As String
    Property Text As String
    Property [Enable] As Boolean = True
    Property [Visible] As Boolean = True
    Property [Default] As Boolean = False
    Property Display As DisplayMode

    Sub New(Name As String, Text As String)
        Me.Name = Name
        Me.Text = Text
        Display = DisplayMode.Any
    End Sub

    Sub New(Text As String)
        Me.Name = Text
        Me.Text = Text
        Display = DisplayMode.Any
    End Sub

    Sub New()
    End Sub

End Class

Public Class RowOptionCollection
    Inherits KeyedCollection(Of String, RowOption)

    Protected Overrides Function GetKeyForItem(ByVal item As RowOption) As String
        Return item.Name
    End Function
End Class

Public Class GridFunction
    Property Sequence As Short
    Property Name As String
    Property Text As String
    Property [Enable] As Boolean = True
    Property [Visible] As Boolean = True
    Property [Default] As Boolean
    Property Display As DisplayMode
    Property Align As ToolStripItemAlignment
    Property ImageName As FunctionImage

    Sub New(Name As String, Text As String)
        Me.Name = Name
        Me.Text = Text
        Display = DisplayMode.Any
    End Sub

    Sub New(Text As String)
        Me.Name = Text
        Me.Text = Text
        Display = DisplayMode.Any
    End Sub

    Sub New()
    End Sub
End Class

Public Class griduser

    Public Class columnuser
        Property Name As String
        Property Field As String
        Property File As String
        Property Text As String
        Property HeaderText As String
        Property Seq As Short
        Property SortSeq As Short
        Property SortOrder As SortOrder
        Property Width As Short
        Property Hidden As Boolean

    End Class

End Class

Public Class GridFunctionCollection
    Inherits KeyedCollection(Of String, GridFunction)

    Protected Overrides Function GetKeyForItem(ByVal item As GridFunction) As String
        Return item.Name
    End Function
End Class


Public Class DynamicStyle
    Property Name As String
    Property LegacyName As String

    Property FieldName As String
    Property Op As Filtering.OperatorName
    Property Value As String

    Property Condition As String

    Property ApplyType As StyleApplyType
    Property ApplyCellName As String
    Property Style As DataGridViewCellStyle

    Property FontName As String
    Property FontBold As Boolean
    Property FontSize As Integer
    Property ForeColorName As String
    Property BackColorName As String

    Friend Shared Function GetStyleApplyType(ApplyTypeText As String) As StyleApplyType
        Dim result As StyleApplyType

        If ApplyTypeText.ToUpper.Contains("ROW") Then
            result = StyleApplyType.Row
        ElseIf ApplyTypeText.ToUpper.Contains("CELL") Then
            result = StyleApplyType.Cell
        Else
            result = StyleApplyType.Cell
        End If

        Return result
    End Function


End Class

Public Class DynamicStyleCollection
    Inherits KeyedCollection(Of String, DynamicStyle)

    Protected Overrides Function GetKeyForItem(ByVal item As DynamicStyle) As String
        Return item.Name
    End Function
End Class


Public Class ProviderType
    Private _Provider As Providers = Providers.DB2
    Private _Code As DB2ProviderDataTypes = 6
    Private _Size As Integer = 0
    Private _Scale As Integer = 0
    Private _Format As String = String.Empty

    Public Enum Providers
        DB2
        Other
    End Enum

    Public Enum DB2ProviderDataTypes
        NotSupported = 0
        SQLLong = 1
        SQLInt = 2
        SQLShort = 3
        NumericPacked = 4
        NumericZoned = 5
        Character = 6
        DB2Date = 12
        DB2Time = 13
        DB2Timestamp = 14
    End Enum

    Public Function DataType() As System.Type

        Select Case _Code
            Case 1
                Return System.Type.GetType("Int64")
            Case 2
                Return System.Type.GetType("Int32")
            Case 3
                Return System.Type.GetType("Int16")
            Case 4
                Return System.Type.GetType("Decimal")
            Case 5
                Return System.Type.GetType("Decimal")
            Case 6
                Return System.Type.GetType("String")
            Case 12
                'Date only
                Return System.Type.GetType("DateTime")
            Case 13
                'Time only
                Return System.Type.GetType("DateTime")
            Case 14
                'timestamp
                Return System.Type.GetType("DateTime")
        End Select

        Return System.Type.GetType("String")
    End Function


    Public Property Code() As DB2ProviderDataTypes
        Get
            Return _Code
        End Get
        Set(value As DB2ProviderDataTypes)
            If _Code = DB2ProviderDataTypes.DB2Time Then
                _Size = 10
            End If
            If _Code = DB2ProviderDataTypes.DB2Date Then
                _Size = 10
            End If
            If _Code = DB2ProviderDataTypes.DB2Timestamp Then
                _Size = "26"
            End If

            If _Code <> value Then
                _Code = value
            End If
        End Set
    End Property


    Public Property Size() As Integer
        Get
            Return _Size
        End Get
        Set(value As Integer)
            If value < 0 Then
                Throw New ArgumentOutOfRangeException("Size", value, "must be >= 0")
            End If

            If _Size <> value Then
                _Size = value
            End If
        End Set
    End Property


    Public Property Scale() As Integer
        Get
            Return _Scale
        End Get
        Set(value As Integer)
            If value < 0 Then
                Throw New ArgumentOutOfRangeException("Scale", value, "must be >= 0")
            End If

            If _Scale <> value Then
                _Scale = value
            End If
        End Set
    End Property

    Friend Shared Function GetProvider(DataType As DataType) As ProviderType.DB2ProviderDataTypes
        Dim result As ProviderType.DB2ProviderDataTypes

        Select Case DataType
            Case DataType.CHAR
                result = ProviderType.DB2ProviderDataTypes.Character
            Case DataType.VARCHAR
                result = ProviderType.DB2ProviderDataTypes.Character
            Case DataType.BIGINT
                result = ProviderType.DB2ProviderDataTypes.SQLLong
            Case DataType.SMALLINT
                result = ProviderType.DB2ProviderDataTypes.SQLShort
            Case DataType.INTEGER
                result = ProviderType.DB2ProviderDataTypes.SQLInt
            Case DataType.TIME
                result = ProviderType.DB2ProviderDataTypes.DB2Time
            Case DataType.DATE
                result = ProviderType.DB2ProviderDataTypes.DB2Date
            Case DataType.TIMESTMP
                result = ProviderType.DB2ProviderDataTypes.DB2Timestamp
            Case DataType.NUMERIC
                result = ProviderType.DB2ProviderDataTypes.NumericZoned
            Case DataType.DECIMAL
                result = ProviderType.DB2ProviderDataTypes.NumericZoned
            Case DataType.FLOAT
                result = ProviderType.DB2ProviderDataTypes.NumericZoned
            Case Else
                result = DB2ProviderDataTypes.NotSupported
        End Select

        Return result
    End Function

End Class



Public Class Filtering

#Region "Filtering Enumerations"

    Public Enum FilterType
        User
        Program
        Security
    End Enum

    Public Enum LogicalOperator
        [And]
        [Or]
    End Enum

    Public Enum OperatorName
        None
        Equals
        NotEquals
        LessThan
        LessThanOrEqual
        GreaterThan
        GreaterThanOrEqual
        Between
        NotBetween
        [Like]
        NotLike
        [In]
        NotIn
        IsNull
        IsNotNull
    End Enum

    Public Enum QuoteFormatting
        UseColumnType
        Manual
    End Enum

    Public Enum SQLTypes
        [String]
        [Numeric]
        [DateTime]
    End Enum

    Public Enum DateRanges
        CurrentDay
        CurrentMonth
        CurrentYear
        PriorDay
        PriorMonth
        PriorYear
    End Enum

#End Region

    Public Class Condition

        Property Seq As Short
        Property Name As String
        Property Description As String


        Property Protect As Boolean = False
        Property Hidden As Boolean = False
        Property Trim As Boolean = False


        Property LOP As LogicalOperator
        Property [Not] As Boolean
        Property ColumnName As String
        Property Op As OperatorName

        Property Value As String
        Property Values As New List(Of String)
        Property Text As String

        Property SubGroup As List(Of Filter)


        Private _Quotes As QuoteFormatting = QuoteFormatting.UseColumnType
        Property Quotes As QuoteFormatting
            Get
                Return _Quotes
            End Get
            Set(value As QuoteFormatting)
                _Quotes = value
                If _Quotes = QuoteFormatting.Manual Then
                    _InsertQuotes = False
                Else
                    Select Case DataType
                        Case SQLTypes.DateTime
                            _InsertQuotes = True
                        Case SQLTypes.Numeric
                            _InsertQuotes = False
                        Case SQLTypes.String
                            _InsertQuotes = True
                    End Select
                End If
            End Set
        End Property

        Private _InsertQuotes As Boolean = True
        ReadOnly Property InsertQuotes As Boolean
            Get
                Return _InsertQuotes
            End Get
        End Property

        Private _DataType As SQLTypes = SQLTypes.String
        ReadOnly Property DataType As SQLTypes
            Get
                Return _DataType
            End Get
        End Property

        Public Function Describe() As String
            Dim result As String

            If String.IsNullOrEmpty(Description) Then

                Dim Val As String

                Select Case Op
                    Case OperatorName.Between, OperatorName.NotBetween
                        Val = Values(0) & " and " & Values(1)

                    Case OperatorName.In, OperatorName.NotIn
                        Dim ara As String()
                        ReDim ara(Values.Count - 1)
                        Values.CopyTo(ara)
                        Val = Join(ara, ",")
                    Case Else
                        Val = Value
                End Select

                Dim OpText As String
                Select Case Op
                    Case OperatorName.Equals
                        OpText = "Equals"
                    Case OperatorName.GreaterThan
                        OpText = "Greater Than"
                    Case OperatorName.LessThan
                        OpText = "Less Than"
                    Case OperatorName.LessThanOrEqual
                        OpText = "Less Than/Equal"
                    Case OperatorName.GreaterThanOrEqual
                        OpText = "Greater Than/Equal"
                    Case OperatorName.NotBetween
                        OpText = "Not Between"
                    Case OperatorName.NotLike
                        OpText = "Not Like"
                    Case OperatorName.NotIn
                        OpText = "Not In"
                    Case Else
                        OpText = Op.ToString
                End Select

                If [Not] Then
                    result = String.Format("Not {0} {1}", OpText, Val)
                Else
                    result = String.Format("{0} {1}", OpText, Val)
                End If

            Else
                    result = Description
            End If

            Return result
        End Function

        Public Sub UseDataType(ByVal DataType As SQLTypes)
            _DataType = DataType
            Quotes = QuoteFormatting.UseColumnType
        End Sub

    End Class

    Public Class ConditionCollection
        Inherits KeyedCollection(Of String, Condition)

        Overloads Sub Add(ByVal item As Condition)
            If Count > 0 Then
                item.Seq = Max(Function(x) x.Seq) + 1
            End If
            MyBase.Add(item)
        End Sub

        Protected Overrides Function GetKeyForItem(ByVal item As Condition) As String
            Return item.Name
        End Function

        Public Function ContainsColumnName(ByVal ColumnName As String) As Boolean
            Dim result As Boolean = False
            For Each obj In Me
                If obj.ColumnName = ColumnName Then
                    result = True
                    Exit For
                End If
            Next
            Return result
        End Function

        Public Function Subset(ByVal ColumnName As String) As IEnumerable(Of Condition)
            Dim result As New List(Of Condition)
            For Each obj In Me
                If obj.ColumnName = ColumnName Then
                    result.Add(obj)
                End If
            Next
            Return result
        End Function

        Public Function Subset(ByVal ColumnName As String, OperatorName As OperatorName) As IEnumerable(Of Condition)
            Dim result As New List(Of Condition)
            For Each obj In Me
                If obj.ColumnName = ColumnName And obj.Op = OperatorName Then
                    result.Add(obj)
                End If
            Next
            Return result
        End Function

    End Class

    Public Class Filter
        Property Seq As Short
        Property Name As String
        Property Description As String
        Property Conditions As New ConditionCollection
        Property Text As String

        Public Function AddFilterRange(ByVal ColumnName As String, ByVal FromValue As String, ToValue As String) As Condition
            Dim Result As New Condition
            Result.LOP = LogicalOperator.And
            Result.ColumnName = ColumnName
            Result.Op = OperatorName.Between
            Result.Values.Add(FromValue)
            Result.Values.Add(ToValue)
            Conditions.Add(Result)
            Return Result
        End Function

        Public Function AddFilterList(ByVal ColumnName As String, ByVal Values As List(Of String)) As Condition
            Dim Result As New Condition
            Result.LOP = LogicalOperator.And
            Result.ColumnName = ColumnName
            Result.Op = OperatorName.In
            Result.Values = Values
            Conditions.Add(Result)
            Return Result
        End Function

        Public Function AddFilterList(ByVal ColumnName As String, ByVal Values As String()) As Condition
            Dim Result As New Condition
            Result.LOP = LogicalOperator.And
            Result.ColumnName = ColumnName
            Result.Op = OperatorName.In
            Result.Values = Values.ToList
            Conditions.Add(Result)
            Return Result
        End Function

        Public Function AddFilter(ByVal ColumnName As String, ByVal ConditionText As String) As Condition
            Dim Result As New Condition
            Result.LOP = LogicalOperator.And
            Result.ColumnName = ColumnName
            Result.Op = OperatorName.None
            Result.Text = ConditionText
            Result.Quotes = QuoteFormatting.Manual
            Conditions.Add(Result)
            Return Result
        End Function

        Public Function AddFilter(ByVal ColumnName As String, ByVal [Operator] As OperatorName, ByVal Value As String) As Condition
            Dim Result As New Condition
            Result.LOP = LogicalOperator.And
            Result.ColumnName = ColumnName
            Result.Op = [Operator]
            Result.Value = Value
            Conditions.Add(Result)
            Return Result
        End Function

        Public Function AddFilter(ByVal ColumnName As String, ByVal [Operator] As OperatorName, ByVal Values As String()) As Condition
            Dim Result As New Condition
            Result.LOP = LogicalOperator.And
            Result.ColumnName = ColumnName
            Result.Op = [Operator]
            Result.Values = Values.ToList
            Conditions.Add(Result)
            Return Result
        End Function

        Public Function AddFilter(ByVal ColumnName As String, ByVal [Operator] As OperatorName, ByVal Values As List(Of String)) As Condition
            Dim Result As New Condition
            Result.LOP = LogicalOperator.And
            Result.ColumnName = ColumnName
            Result.Op = [Operator]
            Result.Values = Values
            Conditions.Add(Result)
            Return Result
        End Function

    End Class

    Friend Shared Function GetOperator(Op As OperatorName) As String
        Dim result As String = String.Empty

        Select Case Op
            Case OperatorName.Between
                result = "Between"
            Case OperatorName.Between
                result = "Not Between"

            Case OperatorName.Equals
                result = "="
            Case OperatorName.GreaterThan
                result = ">"
            Case OperatorName.GreaterThanOrEqual
                result = ">="
            Case OperatorName.In
                result = "In"
            Case OperatorName.NotIn
                result = "Not In"
            Case OperatorName.LessThan
                result = "<"
            Case OperatorName.LessThanOrEqual
                result = "<="
            Case OperatorName.Like
                result = "Like"
            Case OperatorName.None
                result = String.Empty
            Case OperatorName.NotEquals
                result = "<>"
            Case OperatorName.IsNull
                result = "Is"
        End Select

        Return result
    End Function

    Friend Shared Function GetOperatorName([Operator] As String) As OperatorName
        Dim result As OperatorName = OperatorName.None

        Select Case [Operator].ToLower.Trim
            Case "between"
                result = OperatorName.Between
            Case "="
                result = OperatorName.Equals
            Case ">"
                result = OperatorName.GreaterThan
            Case ">="
                result = OperatorName.GreaterThanOrEqual
            Case "in"
                result = OperatorName.In
            Case "<"
                result = OperatorName.LessThan
            Case "<="
                result = OperatorName.LessThanOrEqual
            Case "like"
                result = OperatorName.Like
            Case String.Empty
                result = OperatorName.None
            Case "<>"
                result = OperatorName.NotEquals
        End Select

        Return result
    End Function

    Friend Shared Function GetLogicalOperator([LogicalOperatorText] As String) As LogicalOperator
        Dim result As LogicalOperator = LogicalOperator.And
        If LogicalOperatorText.ToLower.Trim = "or" Then
            result = LogicalOperator.Or
        End If
        Return result
    End Function

    Friend Shared Function GetDataType(DataTypeString As String) As DataType
        Dim result As DataType = DataType.CHAR

        Select Case DataTypeString
            Case "CHAR"
            Case ""
        End Select

        Return result
    End Function




    'OLD OLD OLD; flagged for removal
    'Public Class GridFilterX
    '    Property FieldName As String
    '    Property Hidden As Boolean
    '    Property Protect As Boolean
    '    Property AO As String
    '    Property Seq As Integer
    '    Property OpCode As String
    '    Property Value As String
    '    Property TrimField As Boolean
    '    Property Value2 As String
    '    Property ValueList As List(Of String)
    '    Property Fldt As String
    '    Property Desc As String
    '    Property Source As String
    'End Class
End Class




'filter checkbox tracking 
Public Class ColumnFilterValues
    Property FieldName As String
    Property Values As New List(Of FilterValue)
End Class

'filter checkbox tracking 
Public Class FilterValue
    Property Value As String
    Property Selected As Boolean
    Property CB As CheckBox
End Class



#Region "Enumerations"

Public Enum GeneralAlignment
    Left
    Right
    Center
End Enum

Public Enum GridColumnTypes
    TEXT
    MASKED
    CUSTOMPROMPT
    CALENDAR
    COMBOBOX_SQL
    COMBOBOX_VAL
End Enum

Public Enum GridModes
    Display
    Update
End Enum

Public Enum GridLoadMethods
    DataTable
    DataReader
End Enum

Public Enum DataEnvironments
    Production
    Development
    Test
End Enum

Public Enum ProgramLevels
    Production
    Test
    Development
End Enum

Public Enum GridUpdateMethods
    None
    RowAutomatic
    RowParent
    GridAutomatic
    GridParent
End Enum

Public Enum SecurityColumnType
    None = 0
    BU = 1
    Company = 2
    CompanyArea = 3
End Enum

<FlagsAttribute>
Public Enum RecAllowModes As Short
    Display = 0
    Update = 1
    Add = 2
    Delete = 4
    Copy = 8
End Enum

Public Enum RecModes
    Update
    Add
    Delete
End Enum

Public Enum RecUpdateMethods
    None
    Automatic
    Parent
End Enum

Public Enum RecEditTypes
    Text
    DateTime
    ComboBox
    RadioButton
    CheckBox
End Enum

Public Enum ErrorStyles
    None = 0
    ChangeBackcolor = 1
End Enum

''' <summary>
''' Default editing mode. This property dictates if a note enters edit mode when clicked in CTSNotes control.  
''' This property can be overrided by handling the "NoteOpening" event and changing the CTSNote object directly.
''' </summary>
''' <remarks></remarks>
Public Enum DefaultEditTypes
    Editable
    NotEditable
    OwnerOnly
End Enum

''' <summary>
''' Default Note Sequence. This property dictates the sequence in which the notes appear in CTSNotes control.  
''' </summary>
''' <remarks></remarks>
Public Enum NoteSequence
    Importance
    EnteredDescending
    EnteredAscending
End Enum

Public Enum DisplayMode
    Any
    Display
    Update
End Enum

Public Enum FunctionImage
    None
    Accept
    Add
    Delete
    Back
    Forward
    Cancel
    Copy
    Refresh
    Upload
    Download
    ButtonBlue
    ButtonGreen
    ButtonRed
    ButtonYellow
End Enum

Public Enum StyleApplyType
    Row
    Cell
End Enum

Public Enum DataType
    [CHAR]
    [VARCHAR]
    [DECIMAL]
    [NUMERIC]
    [INTEGER]
    [SMALLINT]
    [BIGINT]
    [FLOAT]
    [DATE]
    [TIME]
    [TIMESTMP]
    [BINARY]
    [BLOB]
    [VARBIN]
    [GRAPHIC]
    [VARG]
    [CLOB]
    [DBCLOB]
End Enum

'SQLLong = 1
'SQLInt = 2
'SQLShort = 3
'NumericPacked = 4
'NumericZoned = 5
'Character = 6
'DB2Date = 12
'DB2Time = 13
'DB2Timestamp = 14

#End Region

