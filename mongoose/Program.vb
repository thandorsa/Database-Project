Imports System
Imports MongoDB.Bson
Imports MongoDB.Driver
Imports System.Console
Imports System.IO
Imports System.Math
Imports System.Random
Imports Newtonsoft.Json.Linq
Imports System.Threading

Module Program
    Dim connectionString, grade1, databaseName, name, surname, fullname As String
    Dim tmpgrade As Integer
    Dim resumer, pass As Boolean


    Sub Main()
        resumer = True
        LoadConfig()
        Dim client As New MongoClient(connectionString)
        Dim database As IMongoDatabase = client.GetDatabase(databaseName)
        Dim collectionName As String = "TestCollection"
        Dim collections = database.ListCollectionNames().ToList()
        Dim collection As IMongoCollection(Of BsonDocument) = database.GetCollection(Of BsonDocument)("Students")

        If Not collections.Contains(collectionName) Then
            database.CreateCollection(collectionName)
            Console.WriteLine("Database was Not Present making one: " & collectionName)
        Else
            Console.WriteLine("Loading.")
        End If
        Thread.Sleep(1000)
        Clear()

        While resumer = True
            promptuser()
            fullname = String.Format("{0} {1}", name, surname)
            Dim status1 As String
            If pass = True Then
                status1 = "Pass"
            Else
                status1 = "False"
            End If
            Dim doc As New BsonDocument From {
            {"studentFullName", fullname},
            {"grade", grade1},
            {"pass", status1}
        }
            collection.InsertOne(doc)
            WriteLine("Document inserted.")
            Thread.Sleep(1000)
            Clear()
            WriteLine("Double You Wish To Add More Names? Y/N")
            Dim input As String = ReadLine()
            If input = "n" Or input = "N" Then
                resumer = False
            End If
        End While

    End Sub
    Sub LoadConfig()
        Dim jsonText As String = File.ReadAllText("config.json")
        Dim jsonData As JObject = JObject.Parse(jsonText)

        connectionString = jsonData("MongoDB")("ConnectionString").ToString()
        databaseName = jsonData("MongoDB")("DatabaseName").ToString()
    End Sub
    Sub Promptuser()

        WriteLine("Please State Your Name")
        name = ReadLine()
        Clear()
        WriteLine("Please Enter Surname")
        surname = ReadLine()
        Clear()
        WriteLine("Please pick the number for the grade")
        WriteLine()
        WriteLine("1. A*")
        WriteLine("2. A")
        WriteLine("3. B")
        WriteLine("4. C")
        WriteLine("5. Other")
        tmpgrade = ReadLine()


        If tmpgrade = 1 Then
            grade1 = "A*"
        ElseIf tmpgrade = 2 Then
            grade1 = "A"
        ElseIf tmpgrade = 3 Then
            grade1 = "B"
        ElseIf tmpgrade = 4 Then
            grade1 = "C"
        ElseIf tmpgrade = 5 Then
            grade1 = "Fail"
        End If
        passes(grade1)
    End Sub

    Sub Passes(grade)
        If grade = "A*" Or grade = "A" Or grade = "B" Or grade = "C" Then
            pass = True
        End If
    End Sub
End Module
