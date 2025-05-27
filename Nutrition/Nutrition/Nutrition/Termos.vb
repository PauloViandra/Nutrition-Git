Imports System.Data.SqlClient
Imports System.Data.SQLite

Public Class frmContrato
    Private Sub frmContrato_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Width = 1386
        Me.Height = 681

    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnAceito.Click
        'usuario aceitou os termos de uso
        If CheckBox1.Checked = True Then

            Dim frm As New frmMenu
            Me.Hide()
            frm.ShowDialog()
        Else
            MsgBox("Para acessar o sistema você precisa estar ciente dos termos de uso.")

        End If
        CheckBox1.BackColor = Color.OrangeRed
    End Sub



    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked Then
            btnAceito.Enabled = True
            btnAceito.BackgroundImage = My.Resources.botao_cta_aceito_ativo
        Else
            btnAceito.BackgroundImage = My.Resources.botao_cta_aceito_desativo
            btnAceito.Enabled = False
        End If
    End Sub

  
    Private Sub btnFecharTermos_Click(sender As Object, e As EventArgs) Handles btnFecharTermos.Click
        'Me.Hide()
        Me.Dispose()
        Application.Exit()
    End Sub
End Class