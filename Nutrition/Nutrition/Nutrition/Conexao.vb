'Imports System.Data.OleDb
Imports System.Data.SQLite

Module Conexao
    'Public con = New SQLiteConnection("Data Source=C:\Nutrition\db\NutritionDados.db;Version=3")
    Public dbName As String = "NutritionDados.db"
    Public dbPath As String = Application.StartupPath & "\" & dbName
    Public connString As String = "Data Source=" & dbPath & ";Version=3"
    Public con As New SQLiteConnection(connString)
    Sub abrir()
        If con.State = 0 Then
            con.Open()

        End If
    End Sub

    Sub fechar()
        If con.State = 1 Then
            con.Close()

        End If
    End Sub

End Module
