Imports System.Drawing.Drawing2D
Imports System.Runtime.InteropServices

Public Class frmLogin
    Private borderRadius As Integer = 20
    Private borderSize As Integer = 3
    Private borderColor As Color = Color.White
    Private Sub frmLogin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Width = 1440
        Me.Height = 930

        txtSenha.Text = "*****"
        'txtUsuario.ForeColor = Color.FromArgb(104, 127, 44)
        txtUsuario.Text = "Digite seu e-mail"
        'txtUsuario.ForeColor = Color.FromArgb(104, 127, 44)

    End Sub

    Public Sub New()

        ' Esta chamada é requerida pelo designer.
        InitializeComponent()

        ' Adicione qualquer inicialização após a chamada InitializeComponent().
        Panel1.BorderStyle = BorderStyle.None
        Me.Padding = New Padding(borderSize)

    End Sub
    'Drag Form
    <DllImport("user32.dll", EntryPoint:="ReleaseCapture")>
    Private Shared Sub ReleaseCapture()
    End Sub
    <DllImport("user32.dll", EntryPoint:="SendMessage")>
    Private Shared Sub SendMessage(ByVal hWnd As System.IntPtr, ByVal wMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer)

    End Sub
    Private Sub panelTitleBar_MouseDown(ByVal send As Object, ByVal e As MouseEventArgs) Handles MyBase.MouseDown
        ReleaseCapture()
        SendMessage(Me.Handle, &H112, &HF012, 0)

    End Sub

    Protected Overrides ReadOnly Property CreateParams As CreateParams
        Get
            Dim cp As CreateParams = MyBase.CreateParams
            cp.Style = cp.Style Or &H20000 ' Minimiza borderless form from taskbar
            Return cp

        End Get
    End Property
    Private Function GetRoundedPath(rect As Rectangle, radius As Single) As GraphicsPath
        Dim path As GraphicsPath = New GraphicsPath()
        Dim curveSize As Single = radius * 2.0F
        path.StartFigure()
        path.AddArc(rect.X, rect.Y, curveSize, curveSize, 180, 90)
        path.AddArc(rect.Right - curveSize, rect.Y, curveSize, curveSize, 270, 90)
        path.AddArc(rect.Right - curveSize, rect.Bottom - curveSize, curveSize, curveSize, 0, 90)
        path.AddArc(rect.X, rect.Bottom - curveSize, curveSize, curveSize, 90, 90)
        path.CloseFigure()
        Return path

    End Function
    Private Sub PnlRegionAndBorder(panel As Panel, radius As Single, graph As Graphics, borderColor As Color, borderSize As Single)
        If Me.WindowState <> FormWindowState.Minimized Then
            Using roundPath As GraphicsPath = GetRoundedPath(panel.ClientRectangle, radius)
                Using penBorder As Pen = New Pen(borderColor, borderSize)
                    Using transform As Matrix = New Matrix()

                        graph.SmoothingMode = SmoothingMode.AntiAlias
                        panel.Region = New Region(roundPath)
                        If borderSize >= 1 Then
                            Dim rect As Rectangle = panel.ClientRectangle
                            Dim scaleX As Single = 1.0F - ((borderSize + 1) / rect.Width)
                            Dim scaleY As Single = 1.0F - ((borderSize + 1) / rect.Height)
                            transform.Scale(scaleX, scaleY)
                            transform.Translate(borderSize / 1.6F, borderSize / 1.6F)
                            graph.Transform = transform
                            graph.DrawPath(penBorder, roundPath)
                        End If


                    End Using
                End Using
            End Using
        End If
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint
        'PnlRegionAndBorder(Panel1, borderRadius, e.Graphics, borderColor, borderSize)
    End Sub

    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click

        'If txtUsuario.Text = "Admin" And txtSenha.Text = "123" Then
        Dim frm As New frmContrato
        Me.Hide()
        frm.ShowDialog()
        ' Else
        'MsgBox("Usuário ou Senha incorretos.")
        'End If

    End Sub

    Private Sub btnSair_Click_1(sender As Object, e As EventArgs) Handles btnSair.Click
        If MsgBox("Deseja sair?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            Me.Close()
        Else

        End If
    End Sub

    Private Sub txtUsuario_Enter(sender As Object, e As EventArgs) Handles txtUsuario.Enter
        MascaraEnter(Me.ActiveControl, "Digite seu e-mail")
    End Sub

    Private Sub txtUsuario_Leave(sender As Object, e As EventArgs) Handles txtUsuario.Leave
        MascaraLeave(txtUsuario, "Digite seu e-mail")
    End Sub

    Private Sub txtSenha_Enter(sender As Object, e As EventArgs) Handles txtSenha.Enter
        MascaraEnter(Me.ActiveControl, "*****")
    End Sub

    Private Sub txtSenha_Leave(sender As Object, e As EventArgs) Handles txtSenha.Leave
        MascaraLeave(txtUsuario, "*****")
    End Sub

    'Visualizar a senha
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If txtSenha.PasswordChar = Nothing Then
            txtSenha.PasswordChar = "*"
            Button1.BackgroundImage = My.Resources.visivel_off
        ElseIf txtSenha.PasswordChar = "*" Then
            txtSenha.PasswordChar = Nothing
            Button1.BackgroundImage = My.Resources.visivel_on
        End If

    End Sub

    Private Sub Panel2_Paint(sender As Object, e As PaintEventArgs)

    End Sub
End Class

'Public Class frmLogin

'    Private Sub frmLogin_Load(sender As Object, e As EventArgs) Handles MyBase.Load

'    End Sub

'    Private Sub BtnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click

'        If txtUsuario.Text = "Admin" And txtSenha.Text = "***" Then
'            Dim frm As New frmMenu
'            ' Dim frm As New frmAlimentos
'            Me.Hide()
'            frm.ShowDialog()
'        Else
'            MsgBox("Usuário ou Senha incorretos.")
'        End If

'    End Sub

'    Private Sub btnSair_Click_1(sender As Object, e As EventArgs) Handles btnSair.Click
'        If MsgBox("Deseja sair?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
'            Me.Close()
'        Else

'        End If
'    End Sub
'End Class