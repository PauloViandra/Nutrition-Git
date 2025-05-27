<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmContrato
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.RichTextBox1 = New System.Windows.Forms.RichTextBox()
        Me.btnAceito = New System.Windows.Forms.Button()
        Me.btnFecharTermos = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.BackColor = System.Drawing.Color.Transparent
        Me.CheckBox1.Location = New System.Drawing.Point(619, 606)
        Me.CheckBox1.Margin = New System.Windows.Forms.Padding(2)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(15, 14)
        Me.CheckBox1.TabIndex = 0
        Me.CheckBox1.UseVisualStyleBackColor = False
        '
        'RichTextBox1
        '
        Me.RichTextBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 72.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RichTextBox1.ForeColor = System.Drawing.Color.Black
        Me.RichTextBox1.Location = New System.Drawing.Point(320, 11)
        Me.RichTextBox1.Margin = New System.Windows.Forms.Padding(2)
        Me.RichTextBox1.Name = "RichTextBox1"
        Me.RichTextBox1.Size = New System.Drawing.Size(909, 26)
        Me.RichTextBox1.TabIndex = 2
        Me.RichTextBox1.Text = "           O" & Global.Microsoft.VisualBasic.ChrW(10) & "       Contrato" & Global.Microsoft.VisualBasic.ChrW(10) & "         ficará" & Global.Microsoft.VisualBasic.ChrW(10) & "           aqui"
        Me.RichTextBox1.Visible = False
        '
        'btnAceito
        '
        Me.btnAceito.BackColor = System.Drawing.Color.Transparent
        Me.btnAceito.BackgroundImage = Global.WindowsApplication1.My.Resources.Resources.botao_cta_aceito_desativo
        Me.btnAceito.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnAceito.Enabled = False
        Me.btnAceito.FlatAppearance.BorderSize = 0
        Me.btnAceito.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.btnAceito.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent
        Me.btnAceito.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAceito.Location = New System.Drawing.Point(693, 649)
        Me.btnAceito.Margin = New System.Windows.Forms.Padding(2)
        Me.btnAceito.Name = "btnAceito"
        Me.btnAceito.Size = New System.Drawing.Size(283, 62)
        Me.btnAceito.TabIndex = 3
        Me.btnAceito.UseVisualStyleBackColor = False
        '
        'btnFecharTermos
        '
        Me.btnFecharTermos.AutoEllipsis = True
        Me.btnFecharTermos.BackColor = System.Drawing.Color.Transparent
        Me.btnFecharTermos.BackgroundImage = Global.WindowsApplication1.My.Resources.Resources.botoes_circulos_fechar
        Me.btnFecharTermos.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnFecharTermos.FlatAppearance.BorderColor = System.Drawing.Color.White
        Me.btnFecharTermos.FlatAppearance.BorderSize = 0
        Me.btnFecharTermos.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnFecharTermos.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White
        Me.btnFecharTermos.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnFecharTermos.Font = New System.Drawing.Font("Inter SemiBold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnFecharTermos.ForeColor = System.Drawing.Color.Black
        Me.btnFecharTermos.Location = New System.Drawing.Point(1312, 14)
        Me.btnFecharTermos.Name = "btnFecharTermos"
        Me.btnFecharTermos.Size = New System.Drawing.Size(41, 44)
        Me.btnFecharTermos.TabIndex = 508
        Me.btnFecharTermos.UseVisualStyleBackColor = False
        '
        'frmContrato
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.BackgroundImage = Global.WindowsApplication1.My.Resources.Resources.Login_termos_aceite2
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ClientSize = New System.Drawing.Size(1384, 786)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnFecharTermos)
        Me.Controls.Add(Me.RichTextBox1)
        Me.Controls.Add(Me.btnAceito)
        Me.Controls.Add(Me.CheckBox1)
        Me.DoubleBuffered = True
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmContrato"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    Friend WithEvents RichTextBox1 As System.Windows.Forms.RichTextBox
    Friend WithEvents btnAceito As System.Windows.Forms.Button
    Friend WithEvents btnFecharTermos As System.Windows.Forms.Button
End Class
