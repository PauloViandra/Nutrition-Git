<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmLogin
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.txtSenha = New System.Windows.Forms.TextBox()
        Me.txtUsuario = New System.Windows.Forms.TextBox()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.btnLogin = New System.Windows.Forms.Button()
        Me.btnSair = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'txtSenha
        '
        Me.txtSenha.BackColor = System.Drawing.Color.White
        Me.txtSenha.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtSenha.Font = New System.Drawing.Font("Inter SemiBold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSenha.ForeColor = System.Drawing.Color.FromArgb(CType(CType(179, Byte), Integer), CType(CType(179, Byte), Integer), CType(CType(179, Byte), Integer))
        Me.txtSenha.Location = New System.Drawing.Point(942, 520)
        Me.txtSenha.Name = "txtSenha"
        Me.txtSenha.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtSenha.Size = New System.Drawing.Size(247, 16)
        Me.txtSenha.TabIndex = 1
        '
        'txtUsuario
        '
        Me.txtUsuario.BackColor = System.Drawing.Color.White
        Me.txtUsuario.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtUsuario.Font = New System.Drawing.Font("Inter SemiBold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtUsuario.ForeColor = System.Drawing.Color.FromArgb(CType(CType(179, Byte), Integer), CType(CType(179, Byte), Integer), CType(CType(179, Byte), Integer))
        Me.txtUsuario.Location = New System.Drawing.Point(942, 421)
        Me.txtUsuario.Name = "txtUsuario"
        Me.txtUsuario.Size = New System.Drawing.Size(282, 16)
        Me.txtUsuario.TabIndex = 0
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.BackColor = System.Drawing.Color.White
        Me.CheckBox1.Font = New System.Drawing.Font("Inter SemiBold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox1.ForeColor = System.Drawing.Color.Black
        Me.CheckBox1.Location = New System.Drawing.Point(1025, 699)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(128, 20)
        Me.CheckBox1.TabIndex = 3
        Me.CheckBox1.Text = "Lembrar de mim"
        Me.CheckBox1.UseVisualStyleBackColor = False
        Me.CheckBox1.Visible = False
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.White
        Me.Panel1.BackgroundImage = Global.WindowsApplication1.My.Resources.Resources.Login___Copia
        Me.Panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.Panel1.Location = New System.Drawing.Point(0, 1)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(362, 748)
        Me.Panel1.TabIndex = 21
        '
        'Button1
        '
        Me.Button1.AutoSize = True
        Me.Button1.BackColor = System.Drawing.Color.White
        Me.Button1.BackgroundImage = Global.WindowsApplication1.My.Resources.Resources.visivel_off
        Me.Button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.Button1.FlatAppearance.BorderSize = 0
        Me.Button1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.Button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White
        Me.Button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button1.Font = New System.Drawing.Font("Inter SemiBold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.ForeColor = System.Drawing.Color.Black
        Me.Button1.Location = New System.Drawing.Point(1198, 515)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(28, 23)
        Me.Button1.TabIndex = 5
        Me.Button1.UseVisualStyleBackColor = False
        '
        'btnLogin
        '
        Me.btnLogin.AutoEllipsis = True
        Me.btnLogin.BackColor = System.Drawing.Color.Transparent
        Me.btnLogin.BackgroundImage = Global.WindowsApplication1.My.Resources.Resources.botao_cta_verde
        Me.btnLogin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnLogin.FlatAppearance.BorderColor = System.Drawing.Color.White
        Me.btnLogin.FlatAppearance.BorderSize = 0
        Me.btnLogin.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnLogin.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White
        Me.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnLogin.Font = New System.Drawing.Font("Inter SemiBold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLogin.ForeColor = System.Drawing.Color.Black
        Me.btnLogin.Location = New System.Drawing.Point(951, 637)
        Me.btnLogin.Name = "btnLogin"
        Me.btnLogin.Size = New System.Drawing.Size(273, 56)
        Me.btnLogin.TabIndex = 2
        Me.btnLogin.UseVisualStyleBackColor = False
        '
        'btnSair
        '
        Me.btnSair.AutoEllipsis = True
        Me.btnSair.BackColor = System.Drawing.Color.Transparent
        Me.btnSair.BackgroundImage = Global.WindowsApplication1.My.Resources.Resources.botoes_circulos_fechar
        Me.btnSair.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnSair.FlatAppearance.BorderColor = System.Drawing.Color.White
        Me.btnSair.FlatAppearance.BorderSize = 0
        Me.btnSair.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnSair.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White
        Me.btnSair.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSair.Font = New System.Drawing.Font("Inter SemiBold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSair.ForeColor = System.Drawing.Color.Black
        Me.btnSair.Location = New System.Drawing.Point(1272, 206)
        Me.btnSair.Name = "btnSair"
        Me.btnSair.Size = New System.Drawing.Size(28, 30)
        Me.btnSair.TabIndex = 11
        Me.btnSair.UseVisualStyleBackColor = False
        '
        'frmLogin
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImage = Global.WindowsApplication1.My.Resources.Resources.Login1
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ClientSize = New System.Drawing.Size(1370, 749)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.btnLogin)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.CheckBox1)
        Me.Controls.Add(Me.txtUsuario)
        Me.Controls.Add(Me.txtSenha)
        Me.Controls.Add(Me.btnSair)
        Me.DoubleBuffered = True
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmLogin"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Login"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtSenha As System.Windows.Forms.TextBox
    Friend WithEvents txtUsuario As System.Windows.Forms.TextBox
    Friend WithEvents btnLogin As System.Windows.Forms.Button
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    Friend WithEvents Panel1 As Panel
    Friend WithEvents btnSair As Button
    Friend WithEvents Button1 As Button
End Class
