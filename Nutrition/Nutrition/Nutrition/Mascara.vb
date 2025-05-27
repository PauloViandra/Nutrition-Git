
Module Mascara
    Private textBoxForecolor = Color.FromArgb(90, 90, 90)

    Public Sub MascaraEnter(textBox As TextBox, msg As String)
        If textBox.Text = msg Then
            textBox.Text = ""
            textBox.ForeColor = textBoxForecolor
        End If
    End Sub
    Public Sub MascaraLeave(textBox As TextBox, msg As String)
        If textBox.Text = "" Then
            textBox.Text = msg
            textBox.ForeColor = Color.FromArgb(179, 179, 179)
        End If
    End Sub

    Public Sub MascaraEnterCbx(textBox As ComboBox, msg As String)
        If textBox.Text = msg Then
            textBox.Text = ""
            textBox.ForeColor = textBoxForecolor
        End If
    End Sub
    Public Sub MascaraLeaveCbx(textBox As ComboBox, msg As String)
        If textBox.Text = "" Then
            textBox.Text = msg
            textBox.ForeColor = Color.FromArgb(179, 179, 179)

        End If
    End Sub

    Public Sub NDecimal(sender As Object, e As KeyPressEventArgs)
        If Not Char.IsDigit(e.KeyChar) And Asc(e.KeyChar) <> 8 And e.KeyChar <> "," Then
            e.Handled = True
        ElseIf (e.KeyChar = ",") Then
            Dim txt = DirectCast(sender, TextBox)
            If txt.Text.Contains(",") Then
                e.Handled = True
            End If

        End If
    End Sub

End Module
