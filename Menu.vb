Imports System.ComponentModel
Imports System.Data.SQLite
Imports System.Drawing.Drawing2D
Imports System.Runtime.ConstrainedExecution
Imports System.Runtime.InteropServices


Public Class frmMenu
    'Private _form1 As frmDieta

    Private borderSizeFiltro As Integer = 2
    Private pnlFiltroborderRadius As Integer = 20

    Private pnlDietborderRadius As Integer = 4
    Private borderRadius As Integer = 6
    Private borderSize As Integer = 1
    Private borderColor As Color = Color.White

    Private pnlMSborderRadius As Integer = 12
    Private pnlMSNborderRadius As Integer = 8
    Private pnDieListborderColor As Color = Color.WhiteSmoke
    Private pnlMSborderColor As Color = Color.FromArgb(237, 242, 207)
    Private pnlMNborderColor As Color = Color.FromArgb(233, 245, 227)

    'Dim localLogoFazda As String = Application.StartupPath & "\" & "logo-farm.png"
    Private Sub frmMenu_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.Width = 1440
        Me.Height = 930
        Panel7.Size = New Size(1442, 80)
        'EsconderAbas()
        Me.tbHome.Parent = Me.tcMenu
        Me.tcMenu.SelectedTab = tbHome
        pnlHome.Visible = True
        btnHome.ForeColor = Color.FromArgb(104, 127, 44)
        BuscarFazenda()
        CarregarCards()
        txtBuscaCliente.Text = "Buscar fazenda"
        BuscarDietasTodas()
        CarregarCardsDieta()
        CarregarAvaliadores()

        If My.Settings.corAvalOnOf = True Then
            rdbCorAvalOn.Checked = True
        Else
            rdbCorAvalOn.Checked = False
        End If

    End Sub

    Public Sub New(ByVal valorform1 As String)
        InitializeComponent()
       
    End Sub

    'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX       TELA HOME    XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
    'Cards da tela Home
    Private Sub CarregarCardsDieta()
        BuscarDietasTodas()

        ' Arrays com painéis e labels
        Dim panels() As Panel = {pnlDieta01, pnlDieta02, pnlDieta03, pnlDieta04}
        Dim lblDieta() As Label = {lblDieta01, lblDieta02, lblDieta03, lblDieta04}
        Dim lblFazenda() As Label = {lblFazDieta01, lblFazDieta02, lblFazDieta03, lblFazDieta04}
        Dim lblDataCriacao() As Label = {lblDtCriacaoDieta01, lblDtCriacaoDieta02, lblDtCriacaoDieta03, lblDtCriacaoDieta04}

        ' Oculta todos os painéis inicialmente
        For Each pnl As Panel In panels
            pnl.Visible = False
        Next

        ' Carrega os dados (máximo 4)
        Dim total As Integer = Math.Min(dtgDietasTodas.Rows.Count, 4)

        For i As Integer = 0 To total - 1
            Dim row As DataGridViewRow = dtgDietasTodas.Rows(i)
            panels(i).Visible = True

            ' Nome da dieta (antes do "-")
            Dim nomeParts() As String = row.Cells(0).Value.ToString().Split("-"c)
            lblDieta(i).Text = nomeParts(0).Trim()

            ' Nome da fazenda
            lblFazenda(i).Text = row.Cells(4).Value.ToString()

            ' Data da criação (parte antes do espaço)
            Dim dataParts() As String = row.Cells(3).Value.ToString().Split(" "c)
            lblDataCriacao(i).Text = "Criada em: " & dataParts(0)
        Next
    End Sub
    'Botão tela Home
    Private Sub btnHome_Click(sender As Object, e As EventArgs) Handles btnHome.Click
        pnlHome.Visible = True
        pnlBbAlimentos.Visible = False
        pnlFazendas.Visible = False
        btnHome.ForeColor = Color.FromArgb(76, 132, 53)

        btnFazendas.ForeColor = Color.FromArgb(30, 30, 30)
        btnBbAlimentos.ForeColor = Color.FromArgb(30, 30, 30)

        'EsconderAbas()
        Me.tbHome.Parent = Me.tcMenu
        Me.tcMenu.SelectedTab = tbHome
    End Sub
    ''XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX  Arredondar bordas XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
    Public Sub New()
        InitializeComponent()
        Me.Padding = New Padding(borderSize)
    End Sub
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
    Private Sub pnlMSN_Paint(sender As Object, e As PaintEventArgs) Handles pnlMSN.Paint
        PnlRegionAndBorder(pnlMSN, borderRadius, e.Graphics, pnlMSborderColor, borderSize)
    End Sub
    Private Sub FormRegionAndBorder(panel As PictureBox, radius As Single, graph As Graphics, borderColor As Color, borderSize As Single)
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
    Private Sub picLogoFaz01_Paint(sender As Object, e As PaintEventArgs) Handles picLogoFaz01.Paint
        borderSize = 0
        borderColor = Color.FromArgb(246, 250, 243)
        FormRegionAndBorder(picLogoFaz01, borderRadius, e.Graphics, borderColor, borderSize)


    End Sub
    Private Sub picLogoFz_Paint(sender As Object, e As PaintEventArgs) Handles picLogoFz.Paint
        borderSize = 0
        borderColor = Color.FromArgb(246, 250, 243)
        FormRegionAndBorder(picLogoFz, borderRadius, e.Graphics, borderColor, borderSize)


    End Sub
    Private Sub picLogoFaz02_Paint(sender As Object, e As PaintEventArgs) Handles picLogoFaz02.Paint
        borderColor = Color.FromArgb(226, 232, 240)
        FormRegionAndBorder(picLogoFaz02, borderRadius, e.Graphics, borderColor, borderSize)
        Me.BackColor = Color.FromArgb(254, 253, 249)

    End Sub
    Private Sub picLogoFaz03_Paint(sender As Object, e As PaintEventArgs) Handles picLogoFaz03.Paint
        borderColor = Color.FromArgb(226, 232, 240)
        FormRegionAndBorder(picLogoFaz03, borderRadius, e.Graphics, borderColor, borderSize)
        Me.BackColor = Color.FromArgb(254, 253, 249)

    End Sub
    Private Sub picLogoFaz04_Paint(sender As Object, e As PaintEventArgs) Handles picLogoFaz04.Paint
        borderColor = Color.FromArgb(226, 232, 240)
        FormRegionAndBorder(picLogoFaz04, borderRadius, e.Graphics, borderColor, borderSize)
        Me.BackColor = Color.FromArgb(254, 253, 249)

    End Sub
    Private Sub picLogoFaz05_Paint(sender As Object, e As PaintEventArgs) Handles picLogoFaz05.Paint
        borderColor = Color.FromArgb(226, 232, 240)
        FormRegionAndBorder(picLogoFaz05, borderRadius, e.Graphics, borderColor, borderSize)
        Me.BackColor = Color.FromArgb(254, 253, 249)

    End Sub
    Private Sub picLogoFaz06_Paint(sender As Object, e As PaintEventArgs) Handles picLogoFaz06.Paint
        borderColor = Color.FromArgb(226, 232, 240)
        FormRegionAndBorder(picLogoFaz06, borderRadius, e.Graphics, borderColor, borderSize)
        Me.BackColor = Color.FromArgb(254, 253, 249)

    End Sub
    Private Sub picLogoFaz07_Paint(sender As Object, e As PaintEventArgs) Handles picLogoFaz07.Paint
        borderColor = Color.FromArgb(226, 232, 240)
        FormRegionAndBorder(picLogoFaz07, borderRadius, e.Graphics, borderColor, borderSize)
        Me.BackColor = Color.FromArgb(254, 253, 249)

    End Sub
    Private Sub picLogoFaz08_Paint(sender As Object, e As PaintEventArgs) Handles picLogoFaz08.Paint
        borderColor = Color.FromArgb(226, 232, 240)
        FormRegionAndBorder(picLogoFaz08, borderRadius, e.Graphics, borderColor, borderSize)
        Me.BackColor = Color.FromArgb(254, 253, 249)

    End Sub
    Private Sub pnlDietas_Paint(sender As Object, e As PaintEventArgs) Handles pnlDietas.Paint
        PnlRegionAndBorder(pnlDietas, pnlMSNborderRadius, e.Graphics, pnlMNborderColor, borderSize)
    End Sub
    Private nutriBorderColor As Color = Color.FromArgb(241, 235, 206)
    Private Sub pnlRelatNutri_Paint(sender As Object, e As PaintEventArgs) Handles pnlRelatNutri.Paint
        PnlRegionAndBorder(pnlRelatNutri, pnlMSNborderRadius, e.Graphics, nutriBorderColor, borderSize)
    End Sub
    Private manejBorderColor As Color = Color.FromArgb(237, 242, 207)

    Private Sub pnlRelatManej_Paint(sender As Object, e As PaintEventArgs) Handles pnlRelatManej.Paint
        PnlRegionAndBorder(pnlRelatManej, pnlMSNborderRadius, e.Graphics, manejBorderColor, borderSize)
    End Sub
    'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX      TELA FAZENDA     XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
    Private Sub CadastrarFazenda()
        Dim sql As String
        Dim cmd As SQLiteCommand
        Dim data As String

        data = Now.ToString("dd-MM-yyyy HH:mm:ss")
       
        If txtNomeFazenda.Text <> "Nome" And txtNomeFazenda.Text <> "" Then

            Try

                abrir()

                sql = "Insert into Cliente (Fazenda,Produtor,Municipio,Estado,Localizacao,Tecnico,Fone,DataNascimento,Foto,Data) values (@Fazenda,@Produtor,@Municipio,@Estado,@Localizacao,@Tecnico,@Fone,@DataNascimento,@Foto,@Data)"
                cmd = New SQLiteCommand(sql, con)
                cmd.Parameters.AddWithValue("@Fazenda", txtNomeFazenda.Text)
                cmd.Parameters.AddWithValue("@Produtor", txtProdutor.Text)
                cmd.Parameters.AddWithValue("@Municipio", txtMunicipioFazenda.Text)
                cmd.Parameters.AddWithValue("@Estado", CbxEstadoFazenda.Text)
                cmd.Parameters.AddWithValue("@Localizacao", txtLocalizacaoFazenda.Text)
                cmd.Parameters.AddWithValue("@Tecnico", txtTecRespFazenda.Text)
                cmd.Parameters.AddWithValue("@Fone", txtFone.Text)
                cmd.Parameters.AddWithValue("@DataNascimento", txtNascimento.Text)
                cmd.Parameters.AddWithValue("@Foto", txtFoto.Text)
                cmd.Parameters.AddWithValue("@Data", data)

                cmd.ExecuteNonQuery()

            Catch ex As Exception
                MsgBox("Erro ao salvar!" + ex.Message)
                fechar()
            End Try

            LimparCamposFazenda()

        Else
            MsgBox("Preencha os campos!")

        End If

    End Sub
    'Botão cadastrar
    Private Sub btnCadastrarFazenda_Click(sender As Object, e As EventArgs) Handles btnCadastrarFazenda.Click
        lblCampoObrigatorio.Visible = False
        lblCampoObrigatorio1.Visible = False

        If txtNomeFazenda.Text = "Nome" Then
            lblCampoObrigatorio.Visible = True
        ElseIf txtNomeFazenda.Text = "" Then
            lblCampoObrigatorio.Visible = True
        ElseIf txtProdutor.Text = "Nome" Then
            lblCampoObrigatorio1.Visible = True
        ElseIf txtProdutor.Text = "" Then
            lblCampoObrigatorio1.Visible = True

        Else

            btnStatus.Text = "Salvando..."
            btnStatus.Refresh()
            CadastrarFazenda()
            Threading.Thread.Sleep(1000)
            btnStatus.Text = ""
            mascara()
            OcultarPNLClientes()
            CarregarCards()
            pnlCadCliente.Visible = False

        End If

    End Sub
    'Carregar logo da propriedade
    Private Sub CarregarFoto()

        Dim openfile = New OpenFileDialog()
        openfile.Filter = "Arquivos de imagem jpg e png|*.jpg; *.png"
        openfile.Multiselect = False
        If openfile.ShowDialog() = Windows.Forms.DialogResult.OK Then
            txtFoto.Text = openfile.FileName
            picLogoFaz.Image = New System.Drawing.Bitmap(txtFoto.Text)

        End If

    End Sub
    'Sub editar dados da fazenda
    Private Sub EditarCliente() 'id As Int32)

        Dim cmd As New SQLiteCommand
        Dim sql As String = "Update Cliente set Fazenda=@Fazenda,Produtor=@Produtor,Municipio=@Municipio,Estado=@Estado,Localizacao=@Localizacao,Tecnico=@Tecnico,Fone=@Fone,DataNascimento=@DataNascimento,Foto=@Foto,Data=@Data where id=@ID"
        Dim Data As String = Now.ToString("dd-MM-yyyy HH:mm:ss")

        If txtNomeFazenda.Text <> "" Then 'And MsgBox("Editar dados da propriedade na base de dados?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then

            Try
                abrir()
                cmd = New SQLiteCommand(sql, con)
                'cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@Fazenda", txtNomeFazenda.Text)
                cmd.Parameters.AddWithValue("@Produtor", txtProdutor.Text)
                cmd.Parameters.AddWithValue("@Municipio", txtMunicipioFazenda.Text)
                cmd.Parameters.AddWithValue("@Estado", CbxEstadoFazenda.Text)
                cmd.Parameters.AddWithValue("@Localizacao", txtLocalizacaoFazenda.Text)
                cmd.Parameters.AddWithValue("@Tecnico", txtTecRespFazenda.Text)
                cmd.Parameters.AddWithValue("@Fone", txtFone.Text)
                cmd.Parameters.AddWithValue("@DataNascimento", txtNascimento.Text)
                cmd.Parameters.AddWithValue("@Foto", txtFoto.Text)
                cmd.Parameters.AddWithValue("@Data", Data)
                cmd.Parameters.AddWithValue("@ID", idFaz)

                cmd.ExecuteNonQuery()
                ' MsgBox("Cliente atualizado com sucesso!")
            Catch ex As Exception
                MsgBox("Erro ao atualizar!" + ex.Message)
                fechar()
            End Try
            LimparCamposFazenda()

        Else
            MsgBox("Preencha os campos nescessários!")

        End If

    End Sub
    'VER SE USA ISSO
    Private Sub OcultarPNLClientes()
        pnlCard01.Visible = False
        pnlCard02.Visible = False
        pnlCard03.Visible = False
        pnlCard04.Visible = False
        pnlCard05.Visible = False
        pnlCard06.Visible = False
        pnlCard07.Visible = False
        pnlCard08.Visible = False
    End Sub
    'Delete 
    Private Sub DeleteFazenda()

        Dim cmd As SQLiteCommand
        Dim sqlDelete As String = "Delete from Cliente where id=@ID"
        'Mensagem se realmente quer excluir
        'If MsgBox("Excluir cliente e propriedade(s) cadastrada(s) na base de dados?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
        Try
            abrir()
            cmd = New SQLiteCommand(sqlDelete, con)
            'cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@ID", idFaz)
            cmd.ExecuteNonQuery()
            'MsgBox("Cliente excluido com sucesso!")
        Catch ex As Exception
            'MsgBox("Erro ao exluir cliente!" + ex.Message)
            fechar()
        End Try
        'txtBuscarCliente.Text = ""
        BuscarFazenda()

        ' Else
        ' MsgBox("Você precisa escolher um cliente na tabela!")
        ' End If

    End Sub
    'Limpar campos do cadastro e edição
    Private Sub LimparCamposFazenda()

        txtProdutor.Text = ""
        txtFone.Text = ""
        txtNomeFazenda.Text = ""
        txtMunicipioFazenda.Text = ""
        CbxEstadoFazenda.Text = ""
        txtLocalizacaoFazenda.Text = ""
        txtTecRespFazenda.Text = ""
        txtNascimento.Text = ""
        txtFoto.Text = ""
        picLogoFaz.Image = Nothing
    End Sub
    'Botão p buscar a logo
    Private Sub btnBuscarFoto_Click(sender As Object, e As EventArgs) Handles btnBuscarFoto.Click
        CarregarFoto()
    End Sub
    'Buscar localização do clinte
    Private Sub btnLocalMaps_Click(sender As Object, e As EventArgs) Handles btnLocalMaps.Click
        Try
            Dim chromePath As String = "C:\Program Files\Google\Chrome\Application\chrome.exe"
            Dim url As String = "https://www.google.com/maps/@-10.9539873,-60.6634587,4.92z?entry=ttu"

            If IO.File.Exists(chromePath) Then
                Process.Start(chromePath, url)
            Else
                MessageBox.Show("O Google Chrome não foi encontrado no caminho padrão.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

            Threading.Thread.Sleep(1000)
            txtLocalizacaoFazenda.Enabled = True

        Catch ex As Exception
            MessageBox.Show("Erro ao abrir o Google Maps: " & ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub
    'carregar dados do cliente
    Private Sub BuscarFazenda()

        Dim da As New SQLiteDataAdapter
        Dim dt As New DataTable

        Try

            abrir()

            Dim sql As String = "Select *  from Cliente"
            da = New SQLiteDataAdapter(sql, con)
            dt = New DataTable
            da.Fill(dt)
            dtgClientes.DataSource = dt

            fechar()

        Catch ex As Exception

        End Try

    End Sub
    'FAZER UM SÓ
    Private Sub LocFazenda()

        Dim da As New SQLiteDataAdapter
        Dim dt As New DataTable

        Try

            abrir()

            Dim sql As String = "Select *  from Cliente"
            da = New SQLiteDataAdapter(sql, con)
            dt = New DataTable
            da.Fill(dt)
            dtgLocCliente.DataSource = dt
            dtgEscolheerCliente.DataSource = dt

            fechar()

        Catch ex As Exception

        End Try
        dtgLocCliente.DataSource = dt.DefaultView
    End Sub
    'Botão para abrir o painel para escolher qual cliente editar
    Private Sub btnEdtFazenda01_Click(sender As Object, e As EventArgs) Handles btnEdtFazenda01.Click
        LocFazenda()
        pnlEscolherFaz.Location = New Point(400, 30)
        pnlEscolherFaz.BringToFront()
        pnlEscolherFaz.Visible = True
        On Error Resume Next
        With dtgEscolheerCliente
            .Columns(0).Width = 150
            .Columns(1).Width = 150
            .Columns(2).Visible = False
            .Columns(3).Visible = False
            .Columns(4).Visible = False
            .Columns(5).Visible = False
            .Columns(6).Visible = False
            .Columns(7).Visible = False
            .Columns(8).Visible = False
            .Columns(9).Visible = False
            .Columns(10).Visible = False
        End With
    End Sub
    'Abrir painel pnlDadosFazenda ' ****************em analise
    Private Sub btnVisFazenda01_Click(sender As Object, e As EventArgs) Handles btnVisFazenda01.Click
        LocFazenda()
        idFaz = lblidFaz01.Text

        Dim cm = CType(Me.BindingContext(dtgLocCliente.DataSource), CurrencyManager)
        Dim dv = CType(dtgLocCliente.DataSource, DataView)
        dv.Sort = "ID"
        Dim x As Integer = dv.Find(lblidFaz01.Text)
        dtgLocCliente.Rows(x).Selected = True

        lblFazenda.Text = dtgLocCliente.Rows(x).Cells(0).Value
        lblProprietario.Text = dtgLocCliente.Rows(x).Cells(1).Value
        lblCidade.Text = dtgLocCliente.Rows(x).Cells(2).Value
        lblUF.Text = dtgLocCliente.Rows(x).Cells(3).Value
        'txtLocalizacaoFazenda.Text = dtgLocCliente.Rows(x).Cells(4).Value              
        lblTecnico.Text = dtgLocCliente.Rows(x).Cells(5).Value
        lblTelefone.Text = dtgLocCliente.Rows(x).Cells(6).Value
        lblDtNasc.Text = dtgLocCliente.Rows(x).Cells(7).Value
        picLogoFz.Image = picLogoFaz01.Image
        pnlDadosFazenda.Visible = True
        pnlDadosFazenda.Location = New Point(414, 1)
        pnlDadosFazenda.BringToFront()

    End Sub
    'Fechar painel pnlDadosFazenda
    Private Sub btnFecharVisFz_Click(sender As Object, e As EventArgs) Handles btnFecharVisFz.Click
        pnlDadosFazenda.Visible = False
    End Sub
    'Botão para atualizar os dados do cliente
    Private Sub btnSalvarFazenda_Click(sender As Object, e As EventArgs) Handles btnSalvarFazenda.Click
        lblCampoObrigatorio.Visible = False
        lblCampoObrigatorio1.Visible = False

        If txtNomeFazenda.Text = "Nome" Then
            lblCampoObrigatorio.Visible = True
        ElseIf txtNomeFazenda.Text = "" Then
            lblCampoObrigatorio.Visible = True
        ElseIf txtProdutor.Text = "Nome" Then
            lblCampoObrigatorio1.Visible = True
        ElseIf txtProdutor.Text = "" Then
            lblCampoObrigatorio1.Visible = True
        Else
            btnStatus.Text = "Salvando..."
            btnStatus.Refresh()
            EditarCliente()
            Threading.Thread.Sleep(500)
            btnStatus.Text = ""

            LimparCamposFazenda()
            btnCadastrarFazenda.BringToFront()
            OcultarPNLClientes()
            CarregarCards()
            pnlCadCliente.Visible = False
        End If

    End Sub
    'Botão excluir fazenda
    Private Sub btnExcluirFazenda_Click(sender As Object, e As EventArgs) Handles btnExcluirFazenda.Click
        btnStatus.Text = "Excluindo..."
        btnStatus.Refresh()
        DeleteFazenda()
        Threading.Thread.Sleep(500)
        btnStatus.Text = ""

        OcultarPNLClientes()
        CarregarCards()
        pnlCadCliente.Visible = False
    End Sub
   
    ' Mascaras
    Private Sub txtNomeFazenda_Enter(sender As Object, e As EventArgs) Handles txtNomeFazenda.Enter
        MascaraEnter(Me.ActiveControl, "Nome")
    End Sub
    Private Sub txtNomeFazenda_Leave(sender As Object, e As EventArgs) Handles txtNomeFazenda.Leave
        MascaraLeave(txtNomeFazenda, "Nome")

    End Sub
    Private Sub txtProdutor_Enter(sender As Object, e As EventArgs) Handles txtProdutor.Enter
        MascaraEnter(Me.ActiveControl, "Nome")
    End Sub
    Private Sub txtProdutor_Leave(sender As Object, e As EventArgs) Handles txtProdutor.Leave
        MascaraLeave(txtProdutor, "Nome")
    End Sub
    Private Sub txtMunicipioFazenda_Enter(sender As Object, e As EventArgs) Handles txtMunicipioFazenda.Enter
        MascaraEnter(Me.ActiveControl, "Cidade")
    End Sub
    Private Sub txtMunicipioFazenda_Leave(sender As Object, e As EventArgs) Handles txtMunicipioFazenda.Leave
        MascaraLeave(txtMunicipioFazenda, "Cidade")
    End Sub
    Private Sub CbxEstadoFazenda_Enter(sender As Object, e As EventArgs) Handles CbxEstadoFazenda.Enter
        MascaraEnterCbx(Me.ActiveControl, "Selecione")
    End Sub
    Private Sub CbxEstadoFazenda_Leave(sender As Object, e As EventArgs) Handles CbxEstadoFazenda.Leave
        MascaraLeaveCbx(CbxEstadoFazenda, "Selecione")
    End Sub
    Private Sub txtTecRespFazenda_Enter(sender As Object, e As EventArgs) Handles txtTecRespFazenda.Enter
        MascaraEnter(Me.ActiveControl, "Nome")
    End Sub
    Private Sub txtTecRespFazenda_Leave(sender As Object, e As EventArgs) Handles txtTecRespFazenda.Leave
        MascaraLeave(txtTecRespFazenda, "Nome")
    End Sub
    Private Sub txtLocalizacaoFazenda_Enter(sender As Object, e As EventArgs) Handles txtLocalizacaoFazenda.Enter
        MascaraEnter(Me.ActiveControl, "Localize")
    End Sub
    Private Sub txtLocalizacaoFazenda_Leave(sender As Object, e As EventArgs) Handles txtLocalizacaoFazenda.Leave
        MascaraLeave(txtLocalizacaoFazenda, "Localize")
    End Sub
    'Ao clicar no txt ir p a 1a posição
    Private Sub txtNascimento_Click(sender As Object, e As EventArgs) Handles txtNascimento.Click
        txtNascimento.SelectionStart = 0
    End Sub
    Private Sub txtNascimento_Enter(sender As Object, e As EventArgs) Handles txtNascimento.Enter
        txtNascimento.SelectionStart = 0
    End Sub
    Private Sub txtFone_Click(sender As Object, e As EventArgs) Handles txtFone.Click
        txtFone.SelectionStart = 0
    End Sub
    Private Sub txtFone_Enter(sender As Object, e As EventArgs) Handles txtFone.Enter
        txtFone.SelectionStart = 0
    End Sub
    '==================================
    'Painel + Cadastrar cliente
    Private Sub pnlCadFazenda_Click(sender As Object, e As EventArgs) Handles pnlCadFazenda.Click
        pnlCadCliente.Location = New Point(400, 2)
        Label3.Text = "Nova Fazenda"
        pnlCadCliente.Visible = True
        pnlCadCliente.BringToFront()
        mascara()
    End Sub
    'Sub mascara
    Private Sub mascara()
        txtTecRespFazenda.Text = "Nome"
        txtProdutor.Text = "Nome"
        txtLocalizacaoFazenda.Text = "Localize"
        CbxEstadoFazenda.Text = "Selecione"
        txtMunicipioFazenda.Text = "Cidade"
        txtNomeFazenda.Text = "Nome"

    End Sub
    'Abrir o painel cadastro de cliente
    Private Sub btnCadFazenda_Click(sender As Object, e As EventArgs) Handles btnCadFazenda.Click
        Label3.Text = "Nova Fazenda"
        pnlCadCliente.Location = New Point(400, 2)
        pnlCadCliente.Visible = True
        pnlCadCliente.BringToFront()
        mascara()
        'btnSalvarFazenda.
        btnCadastrarFazenda.BringToFront()
        btnCadastrarFazenda.Visible = True
        btnSalvarFazenda.Visible = False

    End Sub
    'Fechar o painel cadastro de cliente
    Private Sub btnFecharCadCliente_Click(sender As Object, e As EventArgs) Handles btnFecharCadCliente.Click
        pnlCadCliente.Visible = False
        LimparCamposFazenda()
    End Sub

    Private Sub dtgClientes_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dtgClientes.CellClick
        lblIDFazenda.Text = dtgClientes.CurrentRow.Cells(10).Value.ToString
        EditarDtCliente()
        dtgClientes.Sort(dtgClientes.Columns(9), ListSortDirection.Descending)
        CarregarCards()
        txtBuscaCliente.Text = "Buscar fazenda"
        txtBuscaCliente.ForeColor = SystemColors.ActiveCaption
        'BuscarDietas()
    End Sub
    'Cards da tela fazendas
    Private Sub CarregarCards()
        BuscarFazenda()

        Dim total As Integer = dtgClientes.Rows.Count

        Dim cards() As Panel = {pnlCard01, pnlCard02, pnlCard03, pnlCard04, pnlCard05, pnlCard06}
        Dim pics() As PictureBox = {picLogoFaz01, picLogoFaz02, picLogoFaz03, picLogoFaz04, picLogoFaz05, picLogoFaz06}
        Dim lblFazs() As Label = {lblFaz01, lblFaz02, lblFaz03, lblFaz04, lblFaz05, lblFaz06}
        Dim lblProds() As Label = {lblProdutor01, lblProdutor02, lblProdutor03, lblProdutor04, lblProdutor05, lblProdutor06}
        Dim lblCidades() As Label = {lblCidade01, lblCidade02, lblCidade03, lblCidade04, lblCidade05, lblCidade06}
        Dim lblIDs() As Label = {lblidFaz01, lblidFaz02, lblidFaz03, lblidFaz04, lblidFaz05, lblidFaz06}

        ' Oculta todos os cards inicialmente
        For i As Integer = 0 To cards.Length - 1
            cards(i).Visible = False
        Next

        ' Exibe e preenche os cards de acordo com a quantidade de fazendas
        For i As Integer = 0 To Math.Min(total - 1, cards.Length - 1)
            cards(i).Visible = True

            Dim logo As String = dtgClientes.Rows(i).Cells(8).Value.ToString()
            If String.IsNullOrWhiteSpace(logo) Then
                pics(i).Image = My.Resources.logo_farm
            Else
                Try
                    pics(i).Image = New Bitmap(logo)
                Catch ex As Exception
                    pics(i).Image = My.Resources.logo_farm
                End Try
            End If

            lblFazs(i).Text = dtgClientes.Rows(i).Cells(0).Value.ToString()
            lblProds(i).Text = dtgClientes.Rows(i).Cells(1).Value.ToString()
            lblCidades(i).Text = dtgClientes.Rows(i).Cells(2).Value.ToString() & " / " & dtgClientes.Rows(i).Cells(3).Value.ToString()
            lblIDs(i).Text = dtgClientes.Rows(i).Cells(10).Value.ToString()
        Next

        ' Define posição do painel de cadastro conforme a quantidade
        Select Case total
            Case 0
                pnlCadFazenda.Location = New Point(10, 7)
            Case 1
                pnlCadFazenda.Location = New Point(411, 7)
            Case 2
                pnlCadFazenda.Location = New Point(820, 7)
            Case 3
                pnlCadFazenda.Location = New Point(10, 160)
            Case 4
                pnlCadFazenda.Location = New Point(411, 160)
            Case 5
                pnlCadFazenda.Location = New Point(820, 160)
            Case Else
                pnlCadFazenda.Location = New Point(10, 316)
        End Select
    End Sub
  

    Private Sub EditarDtCliente() 'id As Int32

        Dim cmd As New SQLiteCommand
        Dim sql As String = "Update Cliente set Data=@Data where id=@ID"
        Dim data = Now.ToString("dd-MM-yyyy HH:mm:ss")

        Try
            abrir()
            cmd = New SQLiteCommand(sql, con)
            'cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@Data", data)
            cmd.Parameters.AddWithValue("@ID", lblIDFazenda.Text)

            cmd.ExecuteNonQuery()
            'MsgBox("Cliente atualizado com sucesso!")
        Catch ex As Exception
            'MsgBox("Erro ao atualizar!" + ex.Message)
            fechar()
        End Try
        BuscarFazenda()
    End Sub
    'Configurar o dtgClientes
    Private Sub ConfigGrid()

        On Error Resume Next
        With Me.dtgClientes

            .DefaultCellStyle.Font = New Font("Inter", 9, FontStyle.Bold)

            .Columns(0).HeaderText = "Fazenda"

            .Columns(0).Width = 190

            For i = 1 To 10
                .Columns(i).Visible = False
            Next
            '.Columns(1).Visible = False
            '.Columns(2).Visible = False
            '.Columns(3).Visible = False
            '.Columns(4).Visible = False
            '.Columns(5).Visible = False
            '.Columns(6).Visible = False
            '.Columns(7).Visible = False
            '.Columns(8).Visible = False
            '.Columns(9).Visible = False
            '.Columns(10).Visible = False
        End With
    End Sub
    'Filtrar por cliente
    Private Sub txtBuscaCliente_TextChanged(sender As Object, e As EventArgs) Handles txtBuscaCliente.TextChanged

        If txtBuscaCliente.Text = "Buscar fazenda" Or txtBuscaCliente.Text = "" Then
            dtgClientes.Visible = False

        Else
            dtgClientes.Visible = True
            ConfigGrid()
            dtgClientes.BringToFront()
        End If
        TryCast(dtgClientes.DataSource, DataTable).DefaultView.RowFilter = "Fazenda LIKE '%" & txtBuscaCliente.Text & "%'"
        ConfigGrid()

    End Sub

    Private Sub txtBuscaCliente_Enter(sender As Object, e As EventArgs) Handles txtBuscaCliente.Enter
        MascaraEnter(Me.ActiveControl, "Buscar fazenda")

    End Sub

    Private Sub txtBuscaCliente_Leave(sender As Object, e As EventArgs) Handles txtBuscaCliente.Leave
        MascaraLeave(txtBuscaCliente, "Buscar fazenda")
    End Sub

    Private Sub btnFazendas_Click(sender As Object, e As EventArgs) Handles btnFazendas.Click
        pnlHome.Visible = False
        pnlBbAlimentos.Visible = False
        pnlFazendas.Visible = True
        btnFazendas.ForeColor = Color.FromArgb(76, 132, 53)

        btnBbAlimentos.ForeColor = Color.FromArgb(30, 30, 30)
        btnHome.ForeColor = Color.FromArgb(30, 30, 30)

        Me.tbFazendas.Parent = Me.tcMenu
        Me.tcMenu.SelectedTab = tbFazendas
    End Sub

    Private Sub btnBbAlimentos_Click(sender As Object, e As EventArgs) Handles btnBbAlimentos.Click
        pnlHome.Visible = False
        pnlBbAlimentos.Visible = True
        pnlFazendas.Visible = False
        btnBbAlimentos.ForeColor = Color.FromArgb(76, 132, 53)

        btnHome.ForeColor = Color.FromArgb(30, 30, 30)
        btnFazendas.ForeColor = Color.FromArgb(30, 30, 30)

        Me.tbAlimentos.Parent = Me.tcMenu
        Me.tcMenu.SelectedTab = tbAlimentos
        pnlMSN.BackColor = Color.FromArgb(237, 242, 207)
        lblMatPainel.Text = "Base Matéria Seca"
        rdbBuscarMS.Checked = True
        GridAlimentos()
    End Sub

    Private Sub pnlCard_Click(sender As Object, e As EventArgs) _
    Handles pnlCard01.Click, pnlCard02.Click, pnlCard03.Click, pnlCard04.Click, pnlCard05.Click, pnlCard06.Click, pnlCard07.Click

        Me.tbLotes.Parent = Me.tcMenu
        Me.tcMenu.SelectedTab = tbLotes

        'Identifica qual card foi clicado
        Dim card As Panel = CType(sender, Panel)
        Dim numeroCard As String = card.Name.Substring(card.Name.Length - 2) 'Pega o número do card: "01", "02", etc.

        'Localiza o controle lblFaz e lblIdFaz
        Dim lblFaz As Label = CType(Me.Controls.Find("lblFaz" & numeroCard, True).FirstOrDefault(), Label)
        Dim lblIdFaz As Label = CType(Me.Controls.Find("lblidFaz" & numeroCard, True).FirstOrDefault(), Label)

        'Grava o nome e o  id da fazenda na var idFaz nomeFaz
        nomeFaz = lblFaz.Text
        idFaz = lblIdFaz.Text

        BuscarAnimais()
        CarregarListaLotes()

        lblNFaz.Text = lblFaz.Text
    End Sub

    Private Sub btnNovoLote_Click(sender As Object, e As EventArgs) Handles btnNovoLote.Click
        pnlCadLotes.Visible = True
        pnlCadLotes.BringToFront()
        lblLotes.Text = "Novo Lote"
        MascaraLote()
        pnlCadLotes.Location = New Point(293, 72)
        btnSalvarLotes.Visible = True
        btnAtualizarLotes.Visible = False
    End Sub

    Private Sub btnFecharCadLotes_Click(sender As Object, e As EventArgs)
        pnlCadLotes.Visible = False
    End Sub

    'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXxxXXX        DIETA     XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

    Private Sub BuscarDietas()

        Dim da As New SQLiteDataAdapter
        Dim dt As New DataTable
        Dim sql As String
        Try

            abrir()
            sql = "Select Nome, Lote, QtdAnimais, Data from Dieta where IdPropriedade = " & "'" & idFaz & "'" & " group by Nome, Lote, QtdAnimais, Data, IdPropriedade"

            da = New SQLiteDataAdapter(sql, con)
            dt = New DataTable
            da.Fill(dt)
            dtgBuscarDieta.DataSource = dt

            fechar()

        Catch ex As Exception

        End Try

        'dtgClientes.DataSource = dtgFazendas.DataSource
    End Sub

    Private Sub CarregarListaDieta()
        BuscarDietas()

        ' Oculta todos os painéis de dietas
        Dim painels() As Panel = {pnlLDieta01, pnlLDieta02, pnlLDieta03, pnlLDieta04, pnlLDieta05}
        Dim lblsNome() As Label = {lblLDieta01, lblLDieta02, lblLDieta03, lblLDieta04, lblLDieta05}
        Dim lblsDados() As Label = {lblDadosLDieta01, lblDadosLDieta02, lblDadosLDieta03, lblDadosLDieta04, lblDadosLDieta05}
        Dim lblsId() As Label = {lblIdLDieta01, lblIdLDieta02, lblIdLDieta03, lblIdLDieta04, lblIdLDieta05}

        For i As Integer = 0 To 4
            painels(i).Visible = False
        Next

        Dim x As Integer = dtgBuscarDieta.Rows.Count
        lblQtdDietas.Text = "A propriedade possui " & x & " dietas ativas:"

        If x = 0 Then Exit Sub

        Dim limite As Integer = Math.Min(x, 5)

        For i As Integer = 0 To limite - 1
            Dim nomeDieta() As String = dtgBuscarDieta.Rows(i).Cells(0).Value.ToString().Split("-"c)
            Dim dataCriacao() As String = dtgBuscarDieta.Rows(i).Cells(3).Value.ToString().Split(" "c)

            painels(i).Visible = True
            lblsNome(i).Text = nomeDieta(0)
            lblsDados(i).Text = dtgBuscarDieta.Rows(i).Cells(1).Value & " | " &
                                dtgBuscarDieta.Rows(i).Cells(2).Value & " animais" & " | " &
                                "Criada em: " & dataCriacao(0)
            lblsId(i).Text = dtgBuscarDieta.Rows(i).Cells(3).Value.ToString()
        Next
    End Sub

    Public Sub AtualizarDietas()
        BuscarDietasTodas()
        BuscarDietas()
        CarregarListaDieta()
    End Sub
    Private Sub btnDietaMenuLat_Click(sender As Object, e As EventArgs) Handles btnDietaMenuLat.Click
        BuscarDietasTodas()
        BuscarDietas()
        CarregarListaDieta()
    End Sub
    Private Sub btnDietaMenuLat2_Click(sender As Object, e As EventArgs) Handles btnDietaMenuLat2.Click
        Dim x As Integer = dtgDadAnimais.Rows.Count
        If x > 0 Then
            'Dim frm As New frmDieta
            frmDieta.ShowDialog()
        Else
            MsgBox("Para criar uma dieta, você precisa ter 01 ou mais lotes de animais cadastrados.")
        End If



        'Me.tbDietas.Parent = Me.tcMenu
        'Me.tcMenu.SelectedTab = tbDietas
        'BuscarDietas()
        'CarregarListaDieta()
        'lblNFazDt.Text = lblNFaz.Text & "           "
        'idDieta = ""
    End Sub

    Private Sub Panel41_Paint(sender As Object, e As PaintEventArgs) Handles Panel41.Paint
        PnlRegionAndBorder(Panel41, pnlDietborderRadius, e.Graphics, pnDieListborderColor, borderSize)
    End Sub

    Private Sub Panel39_Paint(sender As Object, e As PaintEventArgs) Handles Panel39.Paint
        PnlRegionAndBorder(Panel39, pnlDietborderRadius, e.Graphics, pnDieListborderColor, borderSize)
    End Sub

    Private Sub Panel21_Paint(sender As Object, e As PaintEventArgs) Handles Panel21.Paint
        PnlRegionAndBorder(Panel21, pnlDietborderRadius, e.Graphics, pnDieListborderColor, borderSize)
    End Sub

    Private Sub Panel31_Paint(sender As Object, e As PaintEventArgs) Handles Panel31.Paint
        PnlRegionAndBorder(Panel31, pnlDietborderRadius, e.Graphics, pnDieListborderColor, borderSize)
    End Sub

    Private Sub Panel35_Paint(sender As Object, e As PaintEventArgs) Handles Panel35.Paint
        PnlRegionAndBorder(Panel35, pnlDietborderRadius, e.Graphics, pnDieListborderColor, borderSize)
    End Sub
    'Private Sub btnExcDieta01_Click(sender As Object, e As EventArgs) Handles btnExcDieta01.Click
    '    idDieta = lblIdLDieta01.Text
    '    pnlConfExcluirDieta.Location = New Point(535, 105)
    '    pnlConfExcluirDieta.Visible = True
    '    pnlConfExcluirDieta.BringToFront()
    '    BuscarDietasTodas()
    '    CarregarListaDieta()
    'End Sub

    'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXxxXXX        LOTES      XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
    Private Sub CadastrarAnimais()
        Dim sql As String
        Dim cmd As SQLiteCommand
        Dim data As String

        'Formatando a data para o padrão aa/mm/dd
        data = Now.ToString("dd-MM-yyyy")

        Try

            abrir()

            sql = "Insert into DadosAnimais (Cliente,Lote,Categoria,QtdAnimais,Peso,Leite,DEL,NOrdenhaDia,Pasto_S_N,PastoDist,DiasGest,Gordura,Proteina,Lactose,Sobra,PcoLeite) values (@Cliente,@Lote,@Categoria,@QtdAnimais,@Peso,@Leite,@DEL,@NOrdenhaDia,@Pasto_S_N,@PastoDist,@DiasGest,@Gordura,@Proteina,@Lactose,@Sobra,@PcoLeite)"
            cmd = New SQLiteCommand(sql, con)
            cmd.Parameters.AddWithValue("@Cliente", idFaz)
            cmd.Parameters.AddWithValue("@Lote", txtNomeLote.Text)
            cmd.Parameters.AddWithValue("@Categoria", cbxCat.Text)
            cmd.Parameters.AddWithValue("@QtdAnimais", txtQtdAnimais.Text)
            cmd.Parameters.AddWithValue("@Peso", txtPVivo.Text)
            cmd.Parameters.AddWithValue("@Leite", txtLt.Text)
            cmd.Parameters.AddWithValue("@DEL", txtDel.Text)
            cmd.Parameters.AddWithValue("@NOrdenhaDia", txtQtdOrdDia.Text)
            cmd.Parameters.AddWithValue("@Pasto_S_N", cbxPastoSN.Text)
            cmd.Parameters.AddWithValue("@PastoDist", txtDist.Text)
            cmd.Parameters.AddWithValue("@DiasGest", txtdiasgest.Text)
            cmd.Parameters.AddWithValue("@Gordura", txtgordura.Text * 100)
            cmd.Parameters.AddWithValue("@Proteina", txtprotn.Text * 100)
            cmd.Parameters.AddWithValue("@Lactose", txtlctse.Text * 100)
            cmd.Parameters.AddWithValue("@Sobra", txtsobra.Text)
            cmd.Parameters.AddWithValue("@PcoLeite", txtPrecoLeite2.Text * 100)
            cmd.ExecuteNonQuery()
            ' MsgBox("Animais cadastrado com sucesso!")
        Catch ex As Exception
            MsgBox("Erro ao salvar!" + ex.Message)
            fechar()
        End Try

        LimparCamposAnim()
    End Sub

    Private Sub EditarAnimais() 'id As Int32)

        Dim cmd As New SQLiteCommand
        Dim sql As String = "Update DadosAnimais Set Cliente=@Cliente,Lote=@Lote,Categoria=@Categoria,QtdAnimais=@QtdAnimais,Peso=@Peso,Leite=@Leite,DEL=@DEL,NOrdenhaDia=@NOrdenhaDia,Pasto_S_N=@Pasto_S_N,PastoDist=@PastoDist,DiasGest=@DiasGest,Gordura=@Gordura,Proteina=@Proteina,Lactose=@Lactose,Sobra=@Sobra,PcoLeite=@PcoLeite where ID=@ID"

        Try
            abrir()
            cmd = New SQLiteCommand(sql, con)
            cmd.Parameters.AddWithValue("@Cliente", idFaz)
            cmd.Parameters.AddWithValue("@Lote", txtNomeLote.Text)
            cmd.Parameters.AddWithValue("@Categoria", cbxCat.Text)
            cmd.Parameters.AddWithValue("@QtdAnimais", txtQtdAnimais.Text)
            cmd.Parameters.AddWithValue("@Peso", txtPVivo.Text)
            cmd.Parameters.AddWithValue("@Leite", txtLt.Text)
            cmd.Parameters.AddWithValue("@DEL", txtDel.Text)
            cmd.Parameters.AddWithValue("@NOrdenhaDia", txtQtdOrdDia.Text)
            cmd.Parameters.AddWithValue("@Pasto_S_N", cbxPastoSN.Text)
            cmd.Parameters.AddWithValue("@PastoDist", txtDist.Text)
            cmd.Parameters.AddWithValue("@DiasGest", txtdiasgest.Text)
            cmd.Parameters.AddWithValue("@Gordura", txtgordura.Text * 100)
            cmd.Parameters.AddWithValue("@Proteina", txtprotn.Text * 100)
            cmd.Parameters.AddWithValue("@Lactose", txtlctse.Text * 100)
            cmd.Parameters.AddWithValue("@Sobra", txtsobra.Text)
            cmd.Parameters.AddWithValue("@PcoLeite", txtPrecoLeite2.Text * 100)
            cmd.Parameters.AddWithValue("@ID", idLote)
            cmd.ExecuteNonQuery()
            ' MsgBox("Lote atualizado com sucesso!")
        Catch ex As Exception
            MsgBox("Erro ao atualizar!" + ex.Message)
            fechar()
        End Try
        LimparCamposAnim()
        BuscarAnimais()
    End Sub

    Private Sub btnSalvarLotes_Click_1(sender As Object, e As EventArgs) Handles btnSalvarLotes.Click
        lblCampoObrigatorio2.Visible = False
        lblCampoObrigatorio3.Visible = False
        lblCampoObrigatorio4.Visible = False
        If txtNomeLote.Text = "Nome" Then
            lblCampoObrigatorio2.Visible = True
        ElseIf txtNomeLote.Text = "" Then
            lblCampoObrigatorio2.Visible = True
        ElseIf cbxCat.Text = "Selecione" Then
            lblCampoObrigatorio3.Visible = True
        ElseIf cbxCat.Text = "Nome" Then
            lblCampoObrigatorio3.Visible = True
        ElseIf txtQtdAnimais.Text = "0" Then
            lblCampoObrigatorio4.Visible = True
        ElseIf txtQtdAnimais.Text = "" Then
            lblCampoObrigatorio4.Visible = True
        Else
            btnStatus.Text = "Salvando..."
            btnStatus.Refresh()
            CadastrarAnimais()
            Threading.Thread.Sleep(500)
            btnStatus.Text = ""
            mascara()

            CarregarListaLotes()
            pnlCadLotes.Visible = False
            'barra de rolagem
        End If
    End Sub
    Private Sub btnatualizarLotes_Click_1(sender As Object, e As EventArgs) Handles btnAtualizarLotes.Click
        lblCampoObrigatorio2.Visible = False
        lblCampoObrigatorio3.Visible = False
        lblCampoObrigatorio4.Visible = False
        If txtNomeLote.Text = "Nome" Then
            lblCampoObrigatorio2.Visible = True
        ElseIf txtNomeLote.Text = "" Then
            lblCampoObrigatorio2.Visible = True
        ElseIf cbxCat.Text = "Selecione" Then
            lblCampoObrigatorio3.Visible = True
        ElseIf cbxCat.Text = "Nome" Then
            lblCampoObrigatorio3.Visible = True
        ElseIf txtQtdAnimais.Text = "0" Then
            lblCampoObrigatorio4.Visible = True
        ElseIf txtQtdAnimais.Text = "" Then
            lblCampoObrigatorio4.Visible = True
        Else
            btnStatus.Text = "Salvando..."
            btnStatus.Refresh()
            EditarAnimais()
            Threading.Thread.Sleep(500)
            btnStatus.Text = ""
            mascara()

            CarregarListaLotes()
            pnlCadLotes.Visible = False
            'barra de rolagem
        End If
    End Sub

    Private Sub BuscarAnimais()
        dtgDadAnimais.Refresh()
        Dim da As New SQLiteDataAdapter
        Dim dt As New DataTable

        Try

            abrir()

            Dim sql As String = "Select * from DadosAnimais where Cliente = " & "'" & idFaz & "'"
            da = New SQLiteDataAdapter(sql, con)
            dt = New DataTable
            da.Fill(dt)
            dtgDadAnimais.DataSource = dt
            'dtgDadAnimais.DataBindings.Clear()
            fechar()

        Catch ex As Exception

        End Try

    End Sub

    Private Sub LocLotes()

        Dim da As New SQLiteDataAdapter
        Dim dt As New DataTable

        Try

            abrir()

            Dim sql As String = "Select * from DadosAnimais"
            da = New SQLiteDataAdapter(sql, con)
            dt = New DataTable
            da.Fill(dt)
            dtgDadosAnimais.DataSource = dt

            fechar()

        Catch ex As Exception

        End Try
        dtgDadosAnimais.DataSource = dt.DefaultView

    End Sub


    Private Sub VisDadosLote()
        LocLotes()
        Dim cm = CType(Me.BindingContext(dtgDadosAnimais.DataSource), CurrencyManager)
        Dim dv = CType(dtgDadosAnimais.DataSource, DataView)
        dv.Sort = "ID"
        Dim x As Integer = dv.Find(idLote)
        dtgDadosAnimais.Rows(x).Selected = True
        Dim vlr As Double
        lblNomeLote.Text = dtgDadosAnimais.Rows(x).Cells(1).Value
        lblCat.Text = dtgDadosAnimais.Rows(x).Cells(2).Value
        lblQtA.Text = dtgDadosAnimais.Rows(x).Cells(3).Value
        lblPV.Text = dtgDadosAnimais.Rows(x).Cells(4).Value
        lblLeite.Text = dtgDadosAnimais.Rows(x).Cells(5).Value
        lblDel.Text = dtgDadosAnimais.Rows(x).Cells(6).Value
        lblNOrd.Text = dtgDadosAnimais.Rows(x).Cells(7).Value
        lblPasto.Text = dtgDadosAnimais.Rows(x).Cells(8).Value
        lblDist.Text = dtgDadosAnimais.Rows(x).Cells(9).Value
        lblDiasGest.Text = dtgDadosAnimais.Rows(x).Cells(10).Value

        vlr = dtgDadosAnimais.Rows(x).Cells(11).Value / 100
        lblGord.Text = vlr.ToString("F2")
        vlr = dtgDadosAnimais.Rows(x).Cells(12).Value / 100
        lblProt.Text = vlr.ToString("F2")
        vlr = dtgDadosAnimais.Rows(x).Cells(13).Value / 100
        lblLact.Text = vlr.ToString("F2")
        vlr = dtgDadosAnimais.Rows(x).Cells(14).Value
        lblSobra.Text = vlr.ToString("F2")
        vlr = dtgDadosAnimais.Rows(x).Cells(15).Value / 100
        lblPrecLeit.Text = vlr.ToString("F2")


        'lblGord.Text = dtgDadosAnimais.Rows(x).Cells(11).Value / 100
        'lblProt.Text = dtgDadosAnimais.Rows(x).Cells(12).Value / 100
        'lblLact.Text = dtgDadosAnimais.Rows(x).Cells(13).Value / 100
        'lblSobra.Text = dtgDadosAnimais.Rows(x).Cells(14).Value
        'lblPrecLeit.Text = dtgDadosAnimais.Rows(x).Cells(15).Value / 100

    End Sub

    Private Sub btnEdtLote_Click(sender As Object, e As EventArgs) _
    Handles btnEdtLote01.Click, btnEdtLote02.Click, btnEdtLote03.Click, btnEdtLote04.Click, btnEdtLote05.Click

        Dim btn As Button = CType(sender, Button)
        Dim numeroBtn As String = btn.Name.Substring(btn.Name.Length - 2) 'Pega o número: "01", "02"...

        Dim lblIdLote As Label = CType(Me.Controls.Find("lblIdLote" & numeroBtn, True).FirstOrDefault(), Label)
        idLote = lblIdLote.Text

        lblLotes.Text = "Editar Lote"
        pnlCadLotes.Visible = True
        pnlCadLotes.BringToFront()
        pnlCadLotes.Location = New Point(293, 72)

        EdtDadosLote()

        btnSalvarLotes.Visible = False
        btnAtualizarLotes.Visible = True
    End Sub

    Private Sub EdtDadosLote()
        LocLotes()
        Dim cm = CType(Me.BindingContext(dtgDadosAnimais.DataSource), CurrencyManager)
        Dim dv = CType(dtgDadosAnimais.DataSource, DataView)
        dv.Sort = "ID"
        Dim x As Integer = dv.Find(idLote)
        dtgDadosAnimais.Rows(x).Selected = True

        txtNomeLote.Text = dtgDadosAnimais.Rows(x).Cells(1).Value
        cbxCat.Text = dtgDadosAnimais.Rows(x).Cells(2).Value
        txtQtdAnimais.Text = dtgDadosAnimais.Rows(x).Cells(3).Value
        txtPVivo.Text = dtgDadosAnimais.Rows(x).Cells(4).Value
        txtLt.Text = dtgDadosAnimais.Rows(x).Cells(5).Value ', "0.00")
        txtDel.Text = dtgDadosAnimais.Rows(x).Cells(6).Value
        txtQtdOrdDia.Text = dtgDadosAnimais.Rows(x).Cells(7).Value
        cbxPastoSN.Text = dtgDadosAnimais.Rows(x).Cells(8).Value
        txtDist.Text = dtgDadosAnimais.Rows(x).Cells(9).Value
        txtdiasgest.Text = dtgDadosAnimais.Rows(x).Cells(10).Value
        txtgordura.Text = Format(dtgDadosAnimais.Rows(x).Cells(11).Value / 100, "0.00")
        txtprotn.Text = Format(dtgDadosAnimais.Rows(x).Cells(12).Value / 100, "0.00")
        txtlctse.Text = Format(dtgDadosAnimais.Rows(x).Cells(13).Value / 100, "0.00")
        txtsobra.Text = Format(dtgDadosAnimais.Rows(x).Cells(14).Value, "0.00")
        txtPrecoLeite2.Text = Format(dtgDadosAnimais.Rows(x).Cells(15).Value / 100, "0.00")

    End Sub

    Private Sub pnlLote01_MouseClick(sender As Object, e As MouseEventArgs) Handles pnlLote01.MouseClick
        idLote = lblIdLote01.Text
        pnlVisLote.Visible = True
        pnlVisLote.BringToFront()
        pnlVisLote.Location = New Point(351, 84)

        VisDadosLote()
    End Sub

    Private Sub pnlLote02_MouseClick(sender As Object, e As MouseEventArgs) Handles pnlLote02.MouseClick
        idLote = lblIdLote02.Text
        pnlVisLote.Visible = True
        pnlVisLote.BringToFront()
        pnlVisLote.Location = New Point(351, 84)
        VisDadosLote()
    End Sub

    Private Sub pnlLote03_MouseClick(sender As Object, e As MouseEventArgs) Handles pnlLote03.MouseClick
        idLote = lblIdLote03.Text
        pnlVisLote.Visible = True
        pnlVisLote.BringToFront()
        pnlVisLote.Location = New Point(351, 84)
        VisDadosLote()
    End Sub

    Private Sub pnlLote04_MouseClick(sender As Object, e As MouseEventArgs) Handles pnlLote04.MouseClick
        idLote = lblIdLote04.Text
        pnlVisLote.Visible = True
        pnlVisLote.BringToFront()
        pnlVisLote.Location = New Point(351, 84)
        VisDadosLote()
    End Sub

    Private Sub pnlLote05_MouseClick(sender As Object, e As MouseEventArgs) Handles pnlLote05.MouseClick
        idLote = lblIdLote05.Text
        pnlVisLote.Visible = True
        pnlVisLote.BringToFront()
        pnlVisLote.Location = New Point(351, 84)
        VisDadosLote()
    End Sub

    Dim idLote As Integer
    Private Sub btnExcluirLote1_Click(sender As Object, e As EventArgs) Handles btnExcluirLote01.Click
        pnlExcluirLote.Location = New Point(500, 2)
        pnlExcluirLote.BringToFront()
        pnlExcluirLote.Visible = True

        idLote = lblIdLote01.Text

    End Sub
    Private Sub btnExcluirLote02_Click(sender As Object, e As EventArgs) Handles btnExcluirLote02.Click
        pnlExcluirLote.Location = New Point(500, 2)
        pnlExcluirLote.BringToFront()
        pnlExcluirLote.Visible = True

        idLote = lblIdLote02.Text

    End Sub
    Private Sub btnExcluirLote03_Click(sender As Object, e As EventArgs) Handles btnExcluirLote03.Click
        pnlExcluirLote.Location = New Point(500, 2)
        pnlExcluirLote.BringToFront()
        pnlExcluirLote.Visible = True

        idLote = lblIdLote03.Text

    End Sub
    Private Sub btnExcluirLote04_Click(sender As Object, e As EventArgs) Handles btnExcluirLote04.Click
        pnlExcluirLote.Location = New Point(500, 2)
        pnlExcluirLote.BringToFront()
        pnlExcluirLote.Visible = True

        idLote = lblIdLote04.Text

    End Sub
    Private Sub btnExcluirLote05_Click(sender As Object, e As EventArgs) Handles btnExcluirLote05.Click
        pnlExcluirLote.Location = New Point(500, 2)
        pnlExcluirLote.BringToFront()
        pnlExcluirLote.Visible = True

        idLote = lblIdLote05.Text
    End Sub


    Private Sub DeleteLote()

        Dim cmd As SQLiteCommand
        Dim sqlDelete As String = "Delete from DadosAnimais where id=@ID"
        'Mensagem se realmente quer excluir
        Try
            abrir()
            cmd = New SQLiteCommand(sqlDelete, con)
            cmd.Parameters.AddWithValue("@ID", idLote)
            cmd.ExecuteNonQuery()
            ' MsgBox("Lote excluido com sucesso!")
        Catch ex As Exception
            MsgBox("Erro ao exluir Lote!" + ex.Message)
            fechar()
        End Try

    End Sub
    Private Sub btnConfExcluirLote_Click(sender As Object, e As EventArgs) Handles btnConfExcluirLote.Click
        btnStatus.Text = "Excluindo..."
        btnStatus.Refresh()
        DeleteLote()
        Threading.Thread.Sleep(1000)
        btnStatus.Text = ""
        mascara()

        CarregarListaLotes()
        pnlExcluirLote.Visible = False

    End Sub
    Private Sub btnCancExcluirLote_Click(sender As Object, e As EventArgs) Handles btnCancExcluirLote.Click
        pnlExcluirLote.Visible = False
    End Sub

    Private Sub btnFecharExcluirLote_Click(sender As Object, e As EventArgs) Handles btnFecharExcluirLote.Click
        pnlExcluirLote.Visible = False
    End Sub
    Private Sub LimparCamposAnim()
        'Limpar os campos
        txtQtdOrdDia.Text = ""
        txtNomeLote.Text = ""
        cbxCat.Text = ""
        cbxPastoSN.Text = ""
        txtQtdAnimais.Text = ""
        txtPVivo.Text = ""
        txtLt.Text = ""
        txtDel.Text = ""
        txtDist.Text = ""
        txtdiasgest.Text = ""
        txtgordura.Text = ""
        txtprotn.Text = ""
        txtlctse.Text = ""
        txtsobra.Text = ""
        txtPrecoLeite2.Text = ""
    End Sub
    Private Sub MascaraLote()
        'Limpar os campos
        txtQtdOrdDia.Text = "00"
        txtNomeLote.Text = "Nome"
        cbxCat.Text = "Selecione"
        cbxPastoSN.Text = "Selecione"
        txtQtdAnimais.Text = "000"
        txtPVivo.Text = "000"
        txtLt.Text = "00"
        txtDel.Text = "000"
        txtDist.Text = "000"
        txtdiasgest.Text = "000"
        txtgordura.Text = "0,00"
        txtprotn.Text = "0,00"
        txtlctse.Text = "0,00"
        txtsobra.Text = "0,00"
        txtPrecoLeite2.Text = "0,00"
    End Sub


    Private Sub txtNomeLote_Enter(sender As Object, e As EventArgs) Handles txtNomeLote.Enter
        MascaraEnter(Me.ActiveControl, "Nome")
    End Sub

    Private Sub txtNomeLote_Leave(sender As Object, e As EventArgs) Handles txtNomeLote.Leave
        MascaraLeave(txtNomeLote, "Nome")
    End Sub

    Private Sub cbxCat_Enter(sender As Object, e As EventArgs) Handles cbxCat.Enter
        MascaraEnterCbx(Me.ActiveControl, "Selecione")
    End Sub

    Private Sub cbxCat_Leave(sender As Object, e As EventArgs) Handles cbxCat.Leave
        MascaraLeaveCbx(cbxCat, "Selecione")
    End Sub
    Private Sub cbxPastoSN_Enter(sender As Object, e As EventArgs) Handles cbxPastoSN.Enter
        MascaraEnterCbx(Me.ActiveControl, "Selecione")
    End Sub
    Private Sub cbxpastosn_Leave(sender As Object, e As EventArgs) Handles cbxPastoSN.Leave
        MascaraLeaveCbx(cbxPastoSN, "Selecione")
    End Sub
    Private Sub txtQtdAnimais_Enter(sender As Object, e As EventArgs) Handles txtQtdAnimais.Enter
        MascaraEnter(Me.ActiveControl, "000")
    End Sub
    Private Sub txtQtdAnimais_Leave(sender As Object, e As EventArgs) Handles txtQtdAnimais.Leave
        MascaraLeave(txtQtdAnimais, "000")
    End Sub
    Private Sub txtPVivo_Enter(sender As Object, e As EventArgs) Handles txtPVivo.Enter
        MascaraEnter(Me.ActiveControl, "000")
    End Sub
    Private Sub txtPVivo_Leave(sender As Object, e As EventArgs) Handles txtPVivo.Leave
        MascaraLeave(txtPVivo, "000")
    End Sub

    Private Sub txtDist_Enter(sender As Object, e As EventArgs) Handles txtDist.Enter
        MascaraEnter(Me.ActiveControl, "000")
    End Sub
    Private Sub txtDist_Leave(sender As Object, e As EventArgs) Handles txtDist.Leave
        MascaraLeave(txtDist, "000")
    End Sub

    Private Sub txtsobra_Enter(sender As Object, e As EventArgs) Handles txtsobra.Enter
        MascaraEnter(Me.ActiveControl, "00")
    End Sub
    Private Sub txtsobra_Leave(sender As Object, e As EventArgs) Handles txtsobra.Leave
        MascaraLeave(txtsobra, "0,00")
        Dim vlr As Double
        vlr = txtsobra.Text
        txtsobra.Text = vlr.ToString("F2")
    End Sub
    Private Sub txtdiasgest_Enter(sender As Object, e As EventArgs) Handles txtdiasgest.Enter
        MascaraEnter(Me.ActiveControl, "000")
    End Sub
    Private Sub txtdiasgest_Leave(sender As Object, e As EventArgs) Handles txtdiasgest.Leave
        MascaraLeave(txtdiasgest, "000")
    End Sub
    Private Sub txtQtdOrdDia_Enter(sender As Object, e As EventArgs) Handles txtQtdOrdDia.Enter
        MascaraEnter(Me.ActiveControl, "00")
    End Sub
    Private Sub txtQtdOrdDia_Leave(sender As Object, e As EventArgs) Handles txtQtdOrdDia.Leave
        MascaraLeave(txtQtdOrdDia, "00")
    End Sub
    Private Sub txtLt_Enter(sender As Object, e As EventArgs) Handles txtLt.Enter
        MascaraEnter(Me.ActiveControl, "00")
    End Sub
    Private Sub txtLt_Leave(sender As Object, e As EventArgs) Handles txtLt.Leave
        MascaraLeave(txtLt, "Nome")
    End Sub
    Private Sub txtDel_Enter(sender As Object, e As EventArgs) Handles txtDel.Enter
        MascaraEnter(Me.ActiveControl, "000")
    End Sub
    Private Sub txtDel_Leave(sender As Object, e As EventArgs) Handles txtDel.Leave
        MascaraLeave(txtDel, "000")
    End Sub
    Private Sub txtgordura_Enter(sender As Object, e As EventArgs) Handles txtgordura.Enter
        MascaraEnter(Me.ActiveControl, "0,00")
    End Sub
    Private Sub txtgordura_Leave(sender As Object, e As EventArgs) Handles txtgordura.Leave
        MascaraLeave(txtgordura, "0,00")
        Dim vlr As Double
        vlr = txtgordura.Text
        txtgordura.Text = vlr.ToString("F2")
    End Sub
    Private Sub txtprotn_Enter(sender As Object, e As EventArgs) Handles txtprotn.Enter
        MascaraEnter(Me.ActiveControl, "0,00")
    End Sub
    Private Sub txtprotn_Leave(sender As Object, e As EventArgs) Handles txtprotn.Leave
        MascaraLeave(txtprotn, "0,00")
        Dim vlr As Double
        vlr = txtprotn.Text
        txtprotn.Text = vlr.ToString("F2")
    End Sub
    Private Sub txtlctse_Enter(sender As Object, e As EventArgs) Handles txtlctse.Enter
        MascaraEnter(Me.ActiveControl, "0,00")
    End Sub
    Private Sub txtlctse_Leave(sender As Object, e As EventArgs) Handles txtlctse.Leave
        MascaraLeave(txtlctse, "0,00")
        Dim vlr As Double
        vlr = txtlctse.Text
        txtlctse.Text = vlr.ToString("F2")
    End Sub
    Private Sub txtprecoleite2_Enter(sender As Object, e As EventArgs) Handles txtPrecoLeite2.Enter
        MascaraEnter(Me.ActiveControl, "0,00")
    End Sub
    Private Sub txtprecoleite2_Leave(sender As Object, e As EventArgs) Handles txtPrecoLeite2.Leave
        MascaraLeave(txtPrecoLeite2, "0,00")
        Dim vlr As Double
        vlr = txtPrecoLeite2.Text
        txtPrecoLeite2.Text = vlr.ToString("F2")
    End Sub

    Private Sub CarregarListaLotes()
        BuscarAnimais()

        ' Arrays de painéis e labels
        Dim panels() As Panel = {pnlLote01, pnlLote02, pnlLote03, pnlLote04, pnlLote05}
        Dim lblLote() As Label = {lblLote01, lblLote02, lblLote03, lblLote04, lblLote05}
        Dim lblDadosLote() As Label = {lblDadosLote01, lblDadosLote02, lblDadosLote03, lblDadosLote04, lblDadosLote05}
        Dim lblIdLote() As Label = {lblIdLote01, lblIdLote02, lblIdLote03, lblIdLote04, lblIdLote05}

        ' Oculta todos os painéis inicialmente
        For Each pnl As Panel In panels
            pnl.Visible = False
        Next

        ' Quantidade de lotes a carregar (máximo 5)
        Dim total As Integer = Math.Min(dtgDadAnimais.Rows.Count, 5)

        For i As Integer = 0 To total - 1
            Dim row As DataGridViewRow = dtgDadAnimais.Rows(i)
            panels(i).Visible = True
            lblLote(i).Text = row.Cells(1).Value.ToString()
            lblDadosLote(i).Text = row.Cells(2).Value.ToString() & " | " & row.Cells(3).Value.ToString()
            lblIdLote(i).Text = row.Cells(16).Value.ToString()
        Next
    End Sub

    Private Sub btnFecharVisLotes_Click(sender As Object, e As EventArgs) Handles btnFecharVisLotes.Click
        pnlVisLote.Visible = False
        pnlCadLotes.Visible = False
        'pnlVisLote.SendToBack()
    End Sub
    Private Sub cbxPastoSN_SelectedValueChanged(sender As Object, e As EventArgs) Handles cbxPastoSN.SelectedValueChanged
        If Me.cbxPastoSN.Text = "Sim" Then
            txtDist.Enabled = True
        Else
            txtDist.Enabled = False
        End If
    End Sub

    Private Sub cbxCat_SelectedValueChanged(sender As Object, e As EventArgs) Handles cbxCat.SelectedValueChanged

        If Me.cbxCat.Text = "Lactação" Then
            txtLt.Enabled = True
            txtgordura.Enabled = True
            txtprotn.Enabled = True
            txtDel.Enabled = True
            txtlctse.Enabled = True
            txtPrecoLeite2.Enabled = True
            txtdiasgest.Enabled = True
            txtQtdOrdDia.Enabled = True
        ElseIf Me.cbxCat.Text = "Novilha" Or Me.cbxCat.Text = "Bezerra" Or Me.cbxCat.Text = "Seca" Then
            txtLt.Enabled = False
            txtgordura.Enabled = False
            txtprotn.Enabled = False
            txtDel.Enabled = False
            txtlctse.Enabled = False
            txtPrecoLeite2.Enabled = False
            txtdiasgest.Enabled = True
            txtQtdOrdDia.Enabled = False
        End If
    End Sub
    'Private pnlBackColor As Color

    'Cada categoria de animais tera uma cor diferente no card
    Private Sub CorLote(index As Integer, pnlSubLote As Panel, pnlLote As Panel)
        If dtgDadAnimais.Rows.Count <= index Then Exit Sub ' Evita erro se não tiver a linha

        Dim status As String = dtgDadAnimais.Rows(index).Cells(2).Value.ToString()

        Select Case status
            Case "Lactação"
                pnlSubLote.BackColor = Color.FromArgb(245, 245, 245)
            Case "Vaca seca"
                pnlSubLote.BackColor = Color.FromArgb(250, 248, 237)
            Case "Bezerra"
                pnlSubLote.BackColor = Color.FromArgb(247, 249, 233)
            Case "Novilha"
                pnlSubLote.BackColor = Color.FromArgb(246, 250, 243)
            Case "Pré-parto"
                pnlSubLote.BackColor = Color.FromArgb(250, 248, 244)
            Case Else
                pnlLote.BackColor = Color.White
        End Select
    End Sub
    Private Sub pnlSubLote1_Paint(sender As Object, e As PaintEventArgs) Handles pnlSubLote1.Paint
        PnlRegionAndBorder(pnlSubLote1, pnlDietborderRadius, e.Graphics, pnDieListborderColor, borderSize)
        CorLote(0, pnlSubLote1, pnlLote01)
    End Sub

    Private Sub pnlSubLote2_Paint(sender As Object, e As PaintEventArgs) Handles pnlSubLote2.Paint
        PnlRegionAndBorder(pnlSubLote2, pnlDietborderRadius, e.Graphics, pnDieListborderColor, borderSize)
        CorLote(1, pnlSubLote2, pnlLote02)
    End Sub

    Private Sub pnlSubLote3_Paint(sender As Object, e As PaintEventArgs) Handles pnlSubLote3.Paint
        PnlRegionAndBorder(pnlSubLote3, pnlDietborderRadius, e.Graphics, pnDieListborderColor, borderSize)
        CorLote(2, pnlSubLote3, pnlLote03)
    End Sub

    Private Sub pnlSubLote4_Paint(sender As Object, e As PaintEventArgs) Handles pnlSubLote4.Paint
        PnlRegionAndBorder(pnlSubLote4, pnlDietborderRadius, e.Graphics, pnDieListborderColor, borderSize)
        CorLote(3, pnlSubLote4, pnlLote04)
    End Sub

    Private Sub pnlSubLote5_Paint(sender As Object, e As PaintEventArgs) Handles pnlSubLote5.Paint
        PnlRegionAndBorder(pnlSubLote5, pnlDietborderRadius, e.Graphics, pnDieListborderColor, borderSize)
        CorLote(4, pnlSubLote5, pnlLote05)
    End Sub

    'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXxxXXX        ALIMENTOS       XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

    Dim Readquery2 As String
    Private Sub BuscarAlimentosMSMO()
        Dim da As New SQLiteDataAdapter
        Dim dt As New DataTable

        Try
            abrir()

            da = New SQLiteDataAdapter(Readquery2, con)
            dt = New DataTable
            da.Fill(dt)
            dtgAlimentoNome.DataSource = dt

            fechar()

        Catch ex As Exception

        End Try

    End Sub

    Private Sub btnFecharFiltro_Click(sender As Object, e As EventArgs) Handles btnFecharFiltro.Click
        pnlFiltro.Visible = False
        pnlFiltro.SendToBack()
        ' TransicaoPnFiltro()
    End Sub

    Private Sub btnAbrirFiltro_Click(sender As Object, e As EventArgs) Handles btnAbrirFiltro.Click
        pnlFiltro.Visible = True
        pnlFiltro.BringToFront()
        ' TransicaoPnFiltro()
    End Sub

    Private Sub rdbBuscarMS_CheckedChanged(sender As Object, e As EventArgs) Handles rdbBuscarMS.CheckedChanged
        Readquery2 = "Select *  from alimentosMS"
        BuscarAlimentosMSMO()

        pnlMSN.BackColor = Color.FromArgb(237, 242, 207)

        lblMatPainel.Text = "Base Matéria Seca"
        txtBuscarAlimentoNome.Text = ""
        cbxVEnerg.Checked = False
        cbxVProt.Checked = False
        cbxCEner.Checked = False
        cbxCProt.Checked = False
        cbxMiner.Checked = False
        cbxAditv.Checked = False
        'rdbOutros.Checked = False
        GridAlimentos()

    End Sub

    Private Sub rdbBuscarMO_CheckedChanged(sender As Object, e As EventArgs) Handles rdbBuscarMO.CheckedChanged
        '    Readquery2 = "Select *  from alimentosMO"
        '    BuscarAlimentosMSMO()

        pnlMSN.BackColor = Color.FromArgb(233, 245, 227)
        '    lblMatPainel.Text = "Base Matéria Natural"

        '    txtBuscarAlimentoNome.Text = ""
        '    cbxVEnerg.Checked = False
        '    cbxVProt.Checked = False
        '    cbxCEner.Checked = False
        '    cbxCProt.Checked = False
        '    cbxMiner.Checked = False
        '    cbxAditv.Checked = False
        '    'cbxOutros.Checked = False
        '    GridAlimentos()

    End Sub

    Private Sub btnFiltroLimpar_Click(sender As Object, e As EventArgs) Handles btnFiltroLimpar.Click
        cbxAditv.Checked = False
        cbxCEner.Checked = False
        cbxCProt.Checked = False
        cbxMiner.Checked = False
        cbxVEnerg.Checked = False
        cbxVProt.Checked = False
        cbxOutros.Checked = False
    End Sub

    Private Sub btnFiltroTodos_Click(sender As Object, e As EventArgs) Handles btnFiltroTodos.Click
        cbxAditv.Checked = True
        cbxCEner.Checked = True
        cbxCProt.Checked = True
        cbxMiner.Checked = True
        cbxVEnerg.Checked = True
        cbxVProt.Checked = True
        cbxOutros.Checked = True
    End Sub
    Private Sub cbxVEnerg_CheckedChanged(sender As Object, e As EventArgs) Handles cbxVEnerg.CheckedChanged
        cbxAditv.Checked = False
        cbxCEner.Checked = False
        cbxCProt.Checked = False
        cbxMiner.Checked = False
        'cbxVEnerg.Checked = true
        cbxVProt.Checked = False
        cbxOutros.Checked = False
        If rdbBuscarMS.Checked Then
            Readquery2 = "Select *  from alimentosMS where AlimentoFamilia = " & "'" & Me.cbxVEnerg.Text & "'"
        ElseIf rdbBuscarMO.Checked Then
            Readquery2 = "Select *  from alimentosMO where AlimentoFamilia = " & "'" & Me.cbxVEnerg.Text & "'"
        End If
        BuscarAlimentosMSMO()

    End Sub

    Private Sub cbxVProt_CheckedChanged(sender As Object, e As EventArgs) Handles cbxVProt.CheckedChanged
        cbxAditv.Checked = False
        cbxCEner.Checked = False
        cbxCProt.Checked = False
        cbxMiner.Checked = False
        cbxVEnerg.Checked = False
        'cbxVProt.Checked = False
        cbxOutros.Checked = False
        If rdbBuscarMS.Checked Then
            Readquery2 = "Select *  from alimentosMS where AlimentoFamilia = " & "'" & Me.cbxVProt.Text & "'"
        ElseIf rdbBuscarMO.Checked Then
            Readquery2 = "Select *  from alimentosMO where AlimentoFamilia = " & "'" & Me.cbxVProt.Text & "'"
        End If
        BuscarAlimentosMSMO()
    End Sub

    Private Sub cbxCEner_CheckedChanged(sender As Object, e As EventArgs) Handles cbxCEner.CheckedChanged
        cbxAditv.Checked = False
        'cbxCEner.Checked = False
        cbxCProt.Checked = False
        cbxMiner.Checked = False
        cbxVEnerg.Checked = False
        cbxVProt.Checked = False
        cbxOutros.Checked = False
        If rdbBuscarMS.Checked Then
            Readquery2 = "Select *  from alimentosMS where AlimentoFamilia = " & "'" & Me.cbxCEner.Text & "'"
        ElseIf rdbBuscarMO.Checked Then
            Readquery2 = "Select *  from alimentosMO where AlimentoFamilia = " & "'" & Me.cbxCEner.Text & "'"
        End If
        BuscarAlimentosMSMO()
    End Sub

    Private Sub cbxMiner_CheckedChanged(sender As Object, e As EventArgs) Handles cbxMiner.CheckedChanged
        cbxAditv.Checked = False
        cbxCEner.Checked = False
        cbxCProt.Checked = False
        'cbxMiner.Checked = False
        cbxVEnerg.Checked = False
        cbxVProt.Checked = False
        cbxOutros.Checked = False
        If rdbBuscarMS.Checked Then
            Readquery2 = "Select *  from alimentosMS where AlimentoFamilia = " & "'" & Me.cbxMiner.Text & "'"
        ElseIf rdbBuscarMO.Checked Then
            Readquery2 = "Select *  from alimentosMO where AlimentoFamilia = " & "'" & Me.cbxMiner.Text & "'"
        End If
        BuscarAlimentosMSMO()
    End Sub

    Private Sub cbxCProt_CheckedChanged(sender As Object, e As EventArgs) Handles cbxCProt.CheckedChanged

        cbxAditv.Checked = False
        cbxCEner.Checked = False
        'cbxCProt.Checked = False
        cbxMiner.Checked = False
        cbxVEnerg.Checked = False
        cbxVProt.Checked = False
        cbxOutros.Checked = False
        If rdbBuscarMS.Checked Then
            Readquery2 = "Select *  from alimentosMS where AlimentoFamilia = " & "'" & Me.cbxCProt.Text & "'"
        ElseIf rdbBuscarMO.Checked Then
            Readquery2 = "Select *  from alimentosMO where AlimentoFamilia = " & "'" & Me.cbxCProt.Text & "'"
        End If
        BuscarAlimentosMSMO()
    End Sub

    Private Sub cbxAditv_CheckedChanged(sender As Object, e As EventArgs) Handles cbxAditv.CheckedChanged

        'cbxAditv.Checked = False
        cbxCEner.Checked = False
        cbxCProt.Checked = False
        cbxMiner.Checked = False
        cbxVEnerg.Checked = False
        cbxVProt.Checked = False
        cbxOutros.Checked = False
        If rdbBuscarMS.Checked Then
            Readquery2 = "Select *  from alimentosMS where AlimentoFamilia = " & "'" & Me.cbxAditv.Text & "'"
        ElseIf rdbBuscarMO.Checked Then
            Readquery2 = "Select *  from alimentosMO where AlimentoFamilia = " & "'" & Me.cbxAditv.Text & "'"
        End If
        BuscarAlimentosMSMO()
    End Sub

    Private Sub txtBuscarAlimentoNome_MouseClick(sender As Object, e As MouseEventArgs) Handles txtBuscarAlimentoNome.MouseClick
        cbxVEnerg.Checked = False
        cbxVProt.Checked = False
        cbxCEner.Checked = False
        cbxCProt.Checked = False
        cbxMiner.Checked = False
        cbxAditv.Checked = False
        'cbxOutros.Checked = False
    End Sub

    'Função para separar colunas entre par e impar
    Function EImpar(ByVal iNum As Long) As Boolean
        'Verifica se o número é impar
        'Se for impar a função retorna True.
        'Se for par a função retorna False.
        EImpar = (iNum Mod 2)

    End Function

    Private Sub GridAlimentos()

        For Each columns As DataGridViewColumn In Me.dtgAlimentoNome.Columns
            dtgAlimentoNome.Columns(columns.Index).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter ' centralizar as celulas
            If EImpar(columns.Index) = True Then
                dtgAlimentoNome.Columns(columns.Index).DefaultCellStyle.BackColor = Color.WhiteSmoke ' se o index da coluna for impar então muda a cor

            End If
        Next

        dtgAlimentoNome.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter ' centralizar o cabeçalho

        On Error Resume Next
        With dtgAlimentoNome

            .DefaultCellStyle.BackColor = Color.White
            .ColumnHeadersDefaultCellStyle.Font = New Font("Inter", 8, FontStyle.Bold)
            .DefaultCellStyle.Font = New Font("Inter", 8, FontStyle.Regular)

            '.RowsDefaultCellStyle.BackColor = 
            .Columns(0).DisplayIndex = 2
            .Columns(0).Frozen = True
            .Columns(1).Visible = False
            '.Columns(2).Visible = False
            .Columns(2).Frozen = True
            .Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns(2).HeaderText = "Selecione o alimento"
            .Columns(2).Width = 260
            '.Columns(2).Width = 70
            For i = 3 To 46
                .Columns(i).Width = 70
            Next
            
            .Columns(47).Visible = My.Settings.text1
            .Columns(48).Visible = My.Settings.text2
            .Columns(49).Visible = My.Settings.text3
            .Columns(50).Visible = My.Settings.text4
            .Columns(51).Visible = My.Settings.text5
            .Columns(52).Visible = My.Settings.text6
            .Columns(53).Visible = My.Settings.text7
            .Columns(54).Visible = My.Settings.text8
            .Columns(55).Visible = My.Settings.text9
            .Columns(56).Visible = My.Settings.text10
            .Columns(57).Visible = False
            .Columns(58).Visible = False
            .Columns(59).Visible = False
            .Columns(60).Visible = False

            .Columns(47).HeaderText = My.Settings.lab2
            .Columns(48).HeaderText = My.Settings.lab3
            .Columns(49).HeaderText = My.Settings.lab4
            .Columns(50).HeaderText = My.Settings.lab5
            .Columns(51).HeaderText = My.Settings.lab6
            .Columns(52).HeaderText = My.Settings.lab7
            .Columns(53).HeaderText = My.Settings.lab8
            .Columns(54).HeaderText = My.Settings.lab9
            .Columns(55).HeaderText = My.Settings.lab10
            .Columns(56).HeaderText = My.Settings.lab1

        End With

        'For i As Integer = 0 To dtgAlimentoNome.Columns.Count - 1
        '    For Each row As DataGridViewRow In dtgAlimentoNome.Rows
        '        If row.Cells(i).ColumnIndex > 3 Then
        '            Dim vr As Double = row.Cells(i).Value
        '            row.Cells(i).Value = Format(Math.Round(vr, 2))
        '        End If


        '    Next
        'Next

    End Sub

    Private Sub dtgAlimentoNome_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dtgAlimentoNome.CellFormatting
        If e.ColumnIndex > 2 Then 'AndAlso IsNumeric(e.Value) 
            If IsNumeric(e.Value) Then
                e.Value = Format(CDbl(e.Value), "0.00")
                e.FormattingApplied = True
            End If
        End If
    End Sub

    Private Sub txtBuscarAlimentoNome_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBuscarAlimentoNome.TextChanged

        TryCast(dtgAlimentoNome.DataSource, DataTable).DefaultView.RowFilter = "Alimento LIKE '%" & txtBuscarAlimentoNome.Text & "%'"

    End Sub
    Private Sub PictureBox10_Click(sender As Object, e As EventArgs) Handles PictureBox10.Click
        txtBuscarAlimentoNome.BringToFront()
        txtBuscarAlimentoNome.Focus()
    End Sub

    Private Sub txtBuscarAlimentoNome_Leave(sender As Object, e As EventArgs) Handles txtBuscarAlimentoNome.Leave
        txtBuscarAlimentoNome.SendToBack()
    End Sub


    'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX        DIETA       XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
    Private Sub BuscarDietasTodas()

        Dim da As New SQLiteDataAdapter
        Dim dt As New DataTable
        Dim sql As String
        Try
            sql = "Select Nome, Lote, QtdAnimais, Data, Propriedade from Dieta group by Nome, Lote, QtdAnimais, Data, Propriedade"
            'sql = "Select * from Dieta"

            abrir()

            da = New SQLiteDataAdapter(sql, con)
            dt = New DataTable

            da.Fill(dt)
            dtgDietasTodas.DataSource = dt

            fechar()

        Catch ex As Exception

        End Try

    End Sub


    Private Sub btnDietas_Click(sender As Object, e As EventArgs)

        Me.tbDietas.Parent = Me.tcMenu
        Me.tcMenu.SelectedTab = tbDietas
        ConfigGrid()
        On Error Resume Next
        With Me.dtgBuscarDieta

            .ColumnHeadersDefaultCellStyle.BackColor = Color.Gainsboro
            .ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Regular)
            .DefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Regular)
            '.Columns(0).HeaderText = "Fazenda"
            .Columns(0).HeaderText = "Dieta"
            .Columns(2).HeaderText = "Nº animais"
            .Columns(3).HeaderText = "Data da criação"
            '.Columns(4).HeaderText = "$Leite"
            '.Columns(5).HeaderText = 

            .Columns(0).Width = 140
            .Columns(1).Visible = False
            .Columns(2).Width = 110
            .Columns(3).Width = 200
            '.Columns(4).Visible = False
            '.Columns(5).Visible = False
        End With

    End Sub

    Private Sub txtBuscarDieta_TextChanged(sender As Object, e As EventArgs)
        TryCast(dtgBuscarDieta.DataSource, DataTable).DefaultView.RowFilter = "Lote LIKE '%" & txtBuscarDieta.Text & "%'"
    End Sub


    Private Sub Button1_Click(sender As Object, e As EventArgs)
        Me.tbFazendas.Parent = Me.tcMenu
        Me.tcMenu.SelectedTab = tbFazendas
    End Sub

    'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX       PROPRIEDADES     XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX


    'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX       AVALIADORES      XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
    Private Sub btnConfiguracao_Click(sender As Object, e As EventArgs) Handles btnConfiguracao.Click
        cbxAvaliadores.Items.Clear()
        Me.tbConfig.Parent = Me.tcMenu
        Me.tcMenu.SelectedTab = tbConfig
        'ConfGridAvaliadores()
        pnlMenuUser.Visible = False
        With Me.cbxAvaliadores
            .Items.Add("Todos")
            .Items.Add("Bezerras")
            .Items.Add("Novilhas")
            .Items.Add("Vacas Secas")
            .Items.Add("Vacas em Lactação")
            .Items.Add("Vacas Pré-parto")

        End With

        dtgTemp.Visible = True
        dtgTemp.BringToFront()

        cbxAvaliadores.Text = "Todos"
        dtgAvaliadores.Visible = False
        BuscarAvaliadores()
        'dtgAvaliadores.BringToFront()
        'txtNomeAval.Text = "Nome do Avaliador"

        If My.Settings.corAvalOnOf = True Then
            rdbCorAvalOn.Checked = True
        Else
            rdbCorAvalOn.Checked = False
        End If

    End Sub

    Private Function GeraTabela() As DataTable

        Try

            Dim dt As New DataTable()

            dt.Columns.Add("Escolher", GetType(System.Boolean))
            dt.Columns.Add("Avaliador")
            dt.Columns.Add("Abx")
            dt.Columns.Add("Abaixo da Meta")
            dt.Columns.Add("")
            dt.Columns.Add("Ideal")
            dt.Columns.Add("Meta Ideal")
            dt.Columns.Add("")
            dt.Columns.Add("Acm")
            dt.Columns.Add("Acima da Meta")
            dt.Columns.Add("Dt1")
            dt.Columns.Add("Dt2")

            'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
            'CRIAR VARIÁVEIS ESSES NÃO APARECEM
            'Energia de mantença	mcal/ dia
            'Energia de lactação	mcal/ dia
            'Energia de lactação com lactose	mcal/ dia
            'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
            dt.Rows.Add(False, "Consumo de Matéria Seca (Kg)", "0", "0", "<", "0", "0", "<", "0", "0", "0", "0") '
            dt.Rows.Add(False, "MS (% MN)", "0", "30 - 20% do consumo", "<", "0", "0", "<", "0", "30 - 20% do consumo", "0", "0")
            dt.Rows.Add(False, "PB (% MS)", "15", "<15%", "<", "0", "15 - 17%", "<", "17", ">17%", "0", "0")
            dt.Rows.Add(False, "PDR (% MS)", "10,2", "<10.2%", "<", "0", "10.2  - 10.9%", "<", "11", ">11%", "0", "0")
            dt.Rows.Add(False, "PND (% MS)", "0", "0", "<", "0", "0", "<", "0", "0", "0", "0")
            dt.Rows.Add(False, "FDN (% MS)", "25", "<25%", "<", "0", "25 - 35%", "<", "35", ">35%", "0", "0")
            dt.Rows.Add(False, "eFDN (% MS)", "21", "<21%", "<", "0", "21 - 28%", "<", "28", ">28%", "0", "0")
            dt.Rows.Add(False, "eFDN2 (% MS)", "0", "0", "<", "0", "0", "<", "0", "0", "0", "0")
            dt.Rows.Add(False, "MN>8 (% MS)", "15", "<15%", "<", "0", "15 - 21%", "<", "22", ">22%", "0", "0")
            dt.Rows.Add(False, "MN>19 (% MS)", "0", "0", "<", "0", "0", "<", "0", "0", "0", "0")
            dt.Rows.Add(False, "FDNF (% MS)", "22", "<22%", "<", "0", "22 - 24%", "<", "24", ">24%", "0", "0")
            dt.Rows.Add(False, "FDA (% MS)", "0", "0", "<", "0", "0", "<", "0", "0", "0", "0")
            dt.Rows.Add(False, "NEl (% MS)", "0", "0", "<", "0", "0", "<", "0", "0", "0", "0")
            dt.Rows.Add(False, "NDT (% MS)", "0", "0", "<", "0", "0", "<", "0", "0", "0", "0")
            dt.Rows.Add(False, "EE (% MS)", "2,5", "<2.5%", "<", "0", "2.5 - 5%", "<", "5", ">5%", "0", "0")
            dt.Rows.Add(False, "EE Insat (% MS)", "1,5", "<1.5%", "<", "0", "1.6 - 3%", "<", "3", ">3%", "0", "0")
            dt.Rows.Add(False, "Cinzas (% MS)", "0", "0", "<", "0", "0", "<", "0", "0", "0", "0")
            dt.Rows.Add(False, "CNF (% MS)", "0", "0", "<", "0", "0", "<", "0", "0", "0", "0")
            dt.Rows.Add(False, "Amido (% MS)", "23", "<23", "<", "0", "23 - 28%", "<", "28", ">28%", "0", "0")
            dt.Rows.Add(False, "kd Amid (% h)", "15", "<15%", "<", "0", "15 - 20%", "<", "20", ">20%", "0", "0")
            dt.Rows.Add(False, "Ca (% MS)", "5,7", "<5.7%", "<", "0", "5.7 - 7.5%", "<", "7,50", ">7.5%", "0", "0")
            dt.Rows.Add(False, "P (% MS)", "3,2", "<3.2%", "<", "0", "3.2 - 3.5%", "<", "3,5", ">3.5%", "0", "0")
            dt.Rows.Add(False, "Mg (% MS)", "1,6", "<1.6%", "<", "0", "1.6 - 3.0%", "<", "3,0", ">3.0%", "0", "0")
            dt.Rows.Add(False, "K (% MS)", "10,2", "<10.2%", "<", "0", "10.2 - 12%", "<", "12", ">12%", "0", "0")
            dt.Rows.Add(False, "S (% MS)", "1,8", "<1.8%", "<", "0", "1.8 - 2.2%", "<", "2,2", ">2.2%", "0", "0")
            dt.Rows.Add(False, "Na (% MS)", "2,0", "<2.0%", "<", "0", "2.0 - 24%", "<", "2", ">2.0%", "0", "0")
            dt.Rows.Add(False, "Cl (% MS)", "2,8", "<2.8%", "<", "0", "2.8 - 3.4%", "<", "3,4", ">3.4%", "0", "0")
            dt.Rows.Add(False, "Co (Mg/ Kg)", "0,20", "<0.20Mg/ Kg", "<", "0", "0.20 - 24Mg/ Kg", "<", "24", ">24Mg/ Kg", "0", "0")
            dt.Rows.Add(False, "Cu (Mg/ Kg)", "10", "<10Mg/ Kg", "<", "0", "10 - 18Mg/ Kg", "<", "18", ">18Mg/ Kg", "0", "0")
            dt.Rows.Add(False, "Mn (Mg/ Kg)", "27", "<27Mg/ Kg", "<", "0", "27 - 40Mg/ Kg", "<", "40", ">40Mg/ Kg", "0", "0")
            dt.Rows.Add(False, "Zn (Mg/ Kg)", "55", "<55Mg/ Kg", "<", "0", "55 - 70Mg/ Kg", "<", "70", ">70Mg/ Kg", "0", "0")
            dt.Rows.Add(False, "Se (Mg/ Kg)", "0,28", "<0.28Mg/ Kg", "<", "0", "0.28 - 0.33Mg/ Kg", "<", "0,33", ">0.33Mg/ Kg", "0", "0")
            dt.Rows.Add(False, "I (Mg/ Kg)", "0,4", "<0.4Mg/ Kg", "<", "0", "0.4 - 0.5Mg/ Kg", "<", "0,5", ">0.5Mg/ Kg", "0", "0")
            dt.Rows.Add(False, "A (Mg/ Kg)", "0", "0", "<", "0", "0", "<", "0", "0", "0", "0")
            dt.Rows.Add(False, "D (Mg/ Kg)", "0", "0", "<", "0", "0", "<", "0", "0", "0", "0")
            dt.Rows.Add(False, "E (Mg/ Kg)", "0", "0", "<", "0", "0", "<", "0", "0", "0", "0") '
            dt.Rows.Add(False, "Cromo (Mg/ Kg)", "0", "0", "<", "0", "0", "<", "0", "0", "0", "0") '
            dt.Rows.Add(False, "DCAD (meq/kg / MS)", "0", "0", "<", "0", "0", "<", "0", "0", "0", "0") '
            dt.Rows.Add(False, "Biotina (Mg/ Kg)", "0", "0", "<", "0", "0", "<", "0", "0", "0", "0") '
            dt.Rows.Add(False, "Virginiamicina (Mg/ Kg)", "0", "0", "<", "0", "0", "<", "0", "0", "0", "0") '
            dt.Rows.Add(False, "Monensina (Mg/ Kg)", "0", "0", "<", "0", "0", "<", "0", "0", "0", "0") '
            dt.Rows.Add(False, "Levedura (UFC)", "0", "0", "<", "0", "0", "<", "0", "0", "0", "0") '
            dt.Rows.Add(False, "Arginina (Mg/ Kg)", "0", "0", "<", "0", "0", "<", "0", "0", "0", "0") '
            dt.Rows.Add(False, "Histidina (Mg/ Kg)", "0", "0", "<", "0", "0", "<", "0", "0", "0", "0") '
            dt.Rows.Add(False, "Isoleucina (Mg/ Kg)", "0", "0", "<", "0", "0", "<", "0", "0", "0", "0") '
            dt.Rows.Add(False, "Leucina (Mg/ Kg)", "0", "0", "<", "0", "0", "<", "0", "0", "0", "0") '
            dt.Rows.Add(False, "Lisina (Mg/ Kg)", "0", "0", "<", "0", "0", "<", "0", "0", "0", "0") '
            dt.Rows.Add(False, "Metionina (Mg/ Kg)", "0", "0", "<", "0", "0", "<", "0", "0", "0", "0") '
            dt.Rows.Add(False, "Fenilalanina (Mg/ Kg)", "0", "0", "<", "0", "0", "<", "0", "0", "0", "0") '
            dt.Rows.Add(False, "Treonina (Mg/ Kg)", "0", "0", "<", "0", "0", "<", "0", "0", "0", "0") '
            dt.Rows.Add(False, "Triptofano (Mg/ Kg)", "0", "0", "<", "0", "0", "<", "0", "0", "0", "0") '
            dt.Rows.Add(False, "Valina (Mg/ Kg)", "0", "0", "<", "0", "0", "<", "0", "0", "0", "0") '
            dt.Rows.Add(False, "dFDNp 48h (% FDN)", "0", "0", "<", "0", "0", "<", "0", "0", "0", "0") '
            dt.Rows.Add(False, "dAmido 7h (% Amido)", "0", "0", "<", "0", "0", "<", "0", "0", "0", "0") '
            dt.Rows.Add(False, "TTNDFD (% FDN)", "0", "0", "<", "0", "0", "<", "0", "0", "0", "0") '

            'dt.Rows.Add(False, "Estimatina prd leite EL(Kg/ dia)", "0", "0", "<", "0", "0", "<", "0", "0", "0", "0") '
            dt.Rows.Add(False, "Estimatina prd leite EL Lactose (Kg/ dia)", "0", "0", "<", "0", "0", "<", "0", "0", "0", "0") '
            'dt.Rows.Add(False, "Fator de Correção FL", "0", "0", "<", "0", "0", "<", "0", "0", "0", "0") '

            'Adicionar os avaliadores personalizáveis na lista de avaliadores
            If My.Settings.text1 = True Then
                dt.Rows.Add(False, My.Settings.lab1, "0", "0", "<", "0", "0", "<", "0", "0", "0", "0") '
                If My.Settings.text2 = True Then
                    dt.Rows.Add(False, My.Settings.lab2, "0", "0", "<", "0", "0", "<", "0", "0", "0", "0") '
                    If My.Settings.text3 = True Then
                        dt.Rows.Add(False, My.Settings.lab3, "0", "0", "<", "0", "0", "<", "0", "0", "0", "0") '
                        If My.Settings.text4 = True Then
                            dt.Rows.Add(False, My.Settings.lab4, "0", "0", "<", "0", "0", "<", "0", "0", "0", "0") '
                            If My.Settings.text5 = True Then
                                dt.Rows.Add(False, My.Settings.lab5, "0", "0", "<", "0", "0", "<", "0", "0", "0", "0") '
                                If My.Settings.text6 = True Then
                                    dt.Rows.Add(False, My.Settings.lab6, "0", "0", "<", "0", "0", "<", "0", "0", "0", "0") '
                                    If My.Settings.text7 = True Then
                                        dt.Rows.Add(False, My.Settings.lab7, "0", "0", "<", "0", "0", "<", "0", "0", "0", "0") '
                                        If My.Settings.text8 = True Then
                                            dt.Rows.Add(False, My.Settings.lab8, "0", "0", "<", "0", "0", "<", "0", "0", "0", "0") '
                                            If My.Settings.text9 = True Then
                                                dt.Rows.Add(False, My.Settings.lab9, "0", "0", "<", "0", "0", "<", "0", "0", "0", "0") '
                                                If My.Settings.text10 = True Then
                                                    dt.Rows.Add(False, My.Settings.lab10, "0", "0", "<", "0", "0", "<", "0", "0", "0", "0") '
                                                End If
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            End If

            dt.Rows.Add(False, "FDN>8/AmiDR (% MS)", "0", "0", "<", "0", "0", "<", "0", "0", "0", "0") '          MN >8 da dieta/ AmiDR da dieta
            dt.Rows.Add(False, "FDN>8 % do PV (% MS)", "0", "0", "<", "0", "0", "<", "0", "0", "0", "0") '        MN >8 da dieta/ Peso do animal x 100
            dt.Rows.Add(False, "FDNF % do PV (% MS)", "0", "0", "<", "0", "0", "<", "0", "0", "0", "0") '         FDNF da dieta/ Peso do animal x 100
            dt.Rows.Add(False, "Forragem (% MS)", "0", "0", "<", "0", "0", "<", "0", "0", "0", "0") '             Do total da dieta quantos % é concentrado
            dt.Rows.Add(False, "Concentrado (% MS)", "0", "0", "<", "0", "0", "<", "0", "0", "0", "0") '          Do total da dieta quantos % é volumoso
            dt.Rows.Add(False, "Dcad (meq/100g / MS)", "0", "0", "<", "0", "0", "<", "0", "0", "0", "0") '        Sódio*100/0.023+Potássio*100/0.039)-(Cloro*100/0.0355+Enxofre*100/0.016
            dt.Rows.Add(False, "Consumo Total (Kg)", "0", "0", "<", "0", "0", "<", "0", "0", "0", "0") '          Somatória de todos os ingredientes na MN
            dt.Rows.Add(False, "Ca/P", "0", "0", "<", "0", "0", "<", "0", "0", "0", "0") '                        Dividir Cálcio por Fósforo
            'dt.Rows.Add(False, "Relação Leite/ Concentrado",           "0","0", "<", "0", "0", "<", "0", "0", "0", "0")
            'dt.Rows.Add(False, "Relação Leite/ Consumo",               "0","0", "<", "0", "0", "<", "0", "0", "0", "0") '
            dt.Rows.Add(False, "Lys / Met", "0", "0", "<", "0", "0", "<", "0", "0", "0", "0") 'Dividir Lisina por Metionina
            dt.Rows.Add(False, "Energia produção de leite (Litros)", "0", "0", "<", "0", "0", "<", "0", "0", "0", "0") '
            dt.Rows.Add(False, "Proteína produção de leite (Litros)", "0", "0", "<", "0", "0", "<", "0", "0", "0", "0") '

            Return dt
        Catch ex As Exception
            Throw ex
        End Try

    End Function
    Private Sub txtNomeAval_Leave(sender As Object, e As EventArgs) Handles txtNomeAval.Leave
        MascaraLeave(txtNomeAval, "Nome do Avaliador")
    End Sub
    Private Sub txtNomeAval_Enter(sender As Object, e As EventArgs) Handles txtNomeAval.Enter
        MascaraEnter(Me.ActiveControl, "Nome do Avaliador")
    End Sub
    Private Sub SalvarAvaliadores()

        Dim sql As String
        Dim cmd As SQLiteCommand
        Dim data As String

        'Formatando a data para o padrão aa/mm/dd
        data = Now.ToString("dd-MM-yyyy hh:mm")
        If dtgTemp.RowCount > 1 Then
            'If txtNomeAval.Text <> "" Then
            For Each row As DataGridViewRow In dtgAvaliadores.Rows
                Try
                    sql = "Insert into Avaliadores (Escolher,NomeAvaliador,ListaAvaliadores,Abx,Abaixo,Valor1,VIdeal,Ideal,Valor2,Acm,Acima,Dt1,Dt2) values (@Escolher,@NomeAvaliador,@ListaAvaliadores,@Abx,@Abaixo,@Valor1,@VIdeal,@Ideal,@Valor2,@Acm,@Acima,@Dt1,@Dt2)"

                    abrir()

                    cmd = New SQLiteCommand(sql, con)
                    cmd.Parameters.AddWithValue("@Escolher", row.Cells(1).Value)
                    cmd.Parameters.AddWithValue("@NomeAvaliador", txtNomeAval.Text)
                    cmd.Parameters.AddWithValue("@ListaAvaliadores", row.Cells(2).Value)
                    cmd.Parameters.AddWithValue("@Abx", row.Cells(3).Value)
                    cmd.Parameters.AddWithValue("@Abaixo", row.Cells(4).Value)
                    cmd.Parameters.AddWithValue("@Valor1", row.Cells(5).Value)
                    cmd.Parameters.AddWithValue("@VIdeal", row.Cells(6).Value)
                    cmd.Parameters.AddWithValue("@Ideal", row.Cells(7).Value)
                    cmd.Parameters.AddWithValue("@Valor2", row.Cells(8).Value.ToString)
                    cmd.Parameters.AddWithValue("@Acm", row.Cells(9).Value)
                    cmd.Parameters.AddWithValue("@Acima", row.Cells(10).Value)
                    cmd.Parameters.AddWithValue("@Dt1", row.Cells(11).Value)
                    cmd.Parameters.AddWithValue("@Dt2", row.Cells(12).Value)
                    cmd.ExecuteNonQuery()

                Catch ex As Exception
                    'MsgBox("Erro ao salvar!" + ex.Message)

                    fechar()
                End Try
            Next
            '    MsgBox("Avaliador cadastrado com sucesso!")
            'Else
            '    MsgBox("Você precisa dar um nome para o conjunto de avaliadores.")
            '    txtNomeAval.Focus()
            'End If
        Else
            MsgBox("Nenhum avaliador selecionado!")

        End If
        My.Settings.NovoAvaliador = txtNomeAval.Text
        'btnNovoAva.Text = My.Settings.NovoAvaliador
        My.Settings.Save()

    End Sub

    Private Sub BuscarAvaliadores()

        Dim da As New SQLiteDataAdapter
        Dim dt As New DataTable

        Try

            abrir()
            'Dim sql As String = "Select * from Avaliadores where Escolher = " & "'" & nomelote & "'"
            Dim sql As String = "Select * from Avaliadores where NomeAvaliador = " & "'" & cbxAvaliadores.Text & "'"
            da = New SQLiteDataAdapter(sql, con)
            dt = New DataTable
            da.Fill(dt)
            dtgTemp.DataSource = dt

            fechar()

        Catch ex As Exception

        End Try
        dtgAvaliadores.Visible = False
        dtgAvaliadores.SendToBack()
        dtgTemp.Visible = True
        dtgTemp.BringToFront()
        ConfGridTemp()


    End Sub

    Private Sub ConfGridAvaliadores()

        For Each columns As DataGridViewColumn In Me.dtgAvaliadores.Columns
            dtgAvaliadores.Columns(columns.Index).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter ' centralizar as celulas
            'If EImpar(columns.Index) = False Then
            '    dtgAlimentos.Columns(columns.Index).DefaultCellStyle.BackColor = Color.WhiteSmoke ' se o index da coluna for impar então muda a cor

            'End If
        Next

        'dtgAlimentos.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter ' centralizar o cabeçalho
        'dtgAvaliadores.Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft ' voltar alimentos para esquerda

        On Error Resume Next
        With Me.dtgAvaliadores
            .DefaultCellStyle.ForeColor = Color.FromArgb(90, 90, 90)
            .DefaultCellStyle.BackColor = Color.White
            .ColumnHeadersDefaultCellStyle.Font = New Font("Inter", 9, FontStyle.Bold)
            .DefaultCellStyle.Font = New Font("Inter", 8.5, FontStyle.Bold)
            '.Columns(3).HeaderText = "Meta Ideal"

            .Columns("Editar").Width = 70
            .Columns("Editar").DisplayIndex = 11
            .Columns(1).Width = 22

            .Columns(2).Width = 405
            .Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns(3).Visible = False
            .Columns(4).Width = 160
            .Columns(4).DefaultCellStyle.BackColor = Color.FromArgb(255, 241, 194)
            .Columns(5).Width = 62
            .Columns(6).Visible = False
            .Columns(7).Width = 160
            .Columns(7).DefaultCellStyle.BackColor = Color.FromArgb(207, 247, 211)
            .Columns(8).Width = 62
            .Columns(9).Visible = False
            .Columns(10).Width = 160
            .Columns(10).DefaultCellStyle.BackColor = Color.FromArgb(255, 241, 194)
            .Columns(11).Visible = False
            .Columns(12).Visible = False

        End With
    End Sub

    Private Sub ConfGridTemp()
        For i As Integer = 0 To dtgTemp.RowCount - 1
            If dtgTemp.Rows(i).Cells(1).Value = 1 Then
                dtgTemp.Rows(i).Cells("Abaixo").Style.BackColor = Color.FromArgb(255, 241, 194)
                dtgTemp.Rows(i).Cells("Ideal").Style.BackColor = Color.FromArgb(207, 247, 211)
                dtgTemp.Rows(i).Cells("Acima").Style.BackColor = Color.FromArgb(255, 241, 194)
            Else
                dtgTemp.Rows(i).Cells("Abaixo").Style.BackColor = Color.White
                dtgTemp.Rows(i).Cells("Ideal").Style.BackColor = Color.White
                dtgTemp.Rows(i).Cells("Acima").Style.BackColor = Color.White

            End If
        Next

        For Each columns As DataGridViewColumn In Me.dtgTemp.Columns
            dtgTemp.Columns(columns.Index).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter ' centralizar as celulas
        Next

        On Error Resume Next
        With Me.dtgTemp
            .DefaultCellStyle.ForeColor = Color.FromArgb(90, 90, 90)
            .DefaultCellStyle.BackColor = Color.White
            .ColumnHeadersDefaultCellStyle.Font = New Font("Inter", 9, FontStyle.Bold)
            .DefaultCellStyle.Font = New Font("Inter", 8.5, FontStyle.Bold)

            .Columns("EditarT").Width = 70
            .Columns("EditarT").DisplayIndex = 9
            .Columns("Escolher").Width = 22
            .Columns("NomeAvaliador").Visible = False
            .Columns("ListaAvaliadores").Width = 405
            .Columns("ListaAvaliadores").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns("ListaAvaliadores").DisplayIndex = 1
            .Columns("Abx").Visible = False
            .Columns("Abaixo").Width = 160
            .Columns("Abaixo").DisplayIndex = 3
            .Columns("Valor1").Width = 62
            .Columns("Valor1").DisplayIndex = 4
            .Columns("Ideal").Width = 160
            .Columns("Ideal").DisplayIndex = 5
            .Columns("VIdeal").Visible = False
            .Columns("Acima").Width = 160
            .Columns("Acima").DisplayIndex = 7
            .Columns("Valor2").Width = 62
            .Columns("Valor2").DisplayIndex = 6
            '.Columns("Acima").DisplayIndex = 8
            .Columns("Acm").Visible = False
            .Columns("Dt1").Visible = False
            .Columns("Dt2").Visible = False
            .Columns("ID").Visible = False
            '.Columns(5).DefaultCellStyle.BackColor = Color.FromArgb(255, 241, 194)
            '.Columns(6).DisplayIndex = 5

            ''.Columns(6).DefaultCellStyle.BackColor = Color.FromArgb(207, 247, 211)

        End With
    End Sub

    Private Sub cbxAvaliadores_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxAvaliadores.SelectedIndexChanged
        If cbxAvaliadores.Text = "Todos" Then

            BuscarAvaliadores()

            lblAvaNome.Text = "Todos os Avaliadores"
        End If
        If cbxAvaliadores.Text = "Vacas Pré-parto" Or cbxAvaliadores.Text = "Bezerras" Or cbxAvaliadores.Text = "Novilhas" Or cbxAvaliadores.Text = "Vacas Secas" Or cbxAvaliadores.Text = "Vacas em Lactação" Then
            BuscarAvaliadores()

            lblAvaNome.Text = cbxAvaliadores.Text

        End If

    End Sub

    Private Sub txtBuscarAvalTodos_TextChanged(sender As Object, e As EventArgs) Handles txtBuscarAvalTodos.TextChanged
        Try
            TryCast(dtgTemp.DataSource, DataTable).DefaultView.RowFilter = "ListaAvaliadores LIKE '%" & txtBuscarAvalTodos.Text & "%'"
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    Private Sub dtgTemp_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dtgTemp.CellContentClick
        On Error Resume Next
        With dtgTemp
            If .CurrentCell.ColumnIndex = 0 Then
                pnlEdtAval.Location = New Point(260, 313)
                pnlEdtAval.Visible = True
                pnlEdtAval.BringToFront()

                txtNomeAvaliador.Text = .CurrentRow.Cells(3).Value.ToString
                txtAbxMt.Text = .CurrentRow.Cells(4).Value.ToString
                txtIdealMt.Text = .CurrentRow.Cells(8).Value.ToString
                txtAcmMt.Text = .CurrentRow.Cells(10).Value.ToString
                lblIdAval.Text = .CurrentRow.Cells(14).Value.ToString

            End If
            If .CurrentCell.ColumnIndex = 1 Then
                lblIdAval.Text = .CurrentRow.Cells(14).Value.ToString
                If .CurrentRow.Cells(1).Value = 1 Then
                    varTF = 0
                ElseIf .CurrentRow.Cells(1).Value = 0 Then
                    varTF = 1
                End If
                EditarAvalTrueFalse()
            End If
        End With
    End Sub
    Dim varTF As Integer
    Private Sub EditarAvalTrueFalse()
        Dim cmd As New SQLiteCommand
        Dim sql As String = "Update Avaliadores Set Escolher=@Escolher where ID=@ID"

        Try
            abrir()
            cmd = New SQLiteCommand(sql, con)
            cmd.Parameters.AddWithValue("@Escolher", varTF)
            cmd.Parameters.AddWithValue("@ID", lblIdAval.Text)
            cmd.ExecuteNonQuery()
            ' MsgBox("Lote atualizado com sucesso!")
        Catch ex As Exception
            MsgBox("Erro ao atualizar!" + ex.Message)
            fechar()
        End Try
        BuscarAvaliadores()
        ConfGridTemp()
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        pnlEdtAval.Visible = False
    End Sub

    Private Sub CarregarAvaliadores()
        Dim dt As New DataTable
        Try
            dt = GeraTabela()
            dtgAvaliadores.DataSource = dt.DefaultView

            For Each dr As DataRow In dt.Rows
                dr("Escolher") = False
            Next
            'faz a seleção da coluna 
            Dim dv As New DataView(dt)
            'define o filtro
            dv.RowFilter = "Escolher = true"
            dtgTemp.DataSource = dv
            'esconde a coluna 'Marcar'
            'dtgTemp.Columns("Escolher").Visible = False
            'impede o usuário de incluir linhas
            dtgTemp.AllowUserToAddRows = False
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub dtgAvaliadores_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles dtgAvaliadores.Paint
        Try
            'Comita as alterações
            dtgAvaliadores.CommitEdit(DataGridViewDataErrorContexts.Commit) 'Not for DataGrid
            'Envia as mudanças para o datasource 
            BindingContext(DirectCast(dtgAvaliadores.DataSource, DataView)).EndCurrentEdit()
        Catch ex As Exception
        End Try
    End Sub
    'Tabela Avaliadores


    Private Sub DeleteAvaliadores()

        Dim cmd As SQLiteCommand
        Dim sqlDelete As String = "Delete from Avaliadores where NomeAvaliador=@NomeAvaliador"
        'Mensagem se realmente quer excluir

        Try
            abrir()
            cmd = New SQLiteCommand(sqlDelete, con)
            'cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@NomeAvaliador", lblCodAva.Text)
            cmd.ExecuteNonQuery()
            ' MsgBox("As alterações foram bem sucedidas!")
        Catch ex As Exception
            ' MsgBox("Erro ao editar!" + ex.Message)
            fechar()
        End Try

    End Sub

    Private Sub btnSalvarAvaliador_Click(sender As Object, e As EventArgs) Handles btnSalvarAvaliador.Click
        'DeleteAvaliadores()
        'SalvarAvaliadores()

        'txtNomeAval.Text = ""
        'dtgAvaliadores.Visible = False
        'dtgTemp.Visible = False
        'txtNomeAval.Visible = False
        'lblNomeAva.Visible = False
        'btnSalvarAvaliador.Visible = False
        'EscondeBotoes()
    End Sub
    Private Sub txtAbxMt_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtAbxMt.KeyPress
        NDecimal(sender, e)
    End Sub

    Private Sub txtAcmMt_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtAcmMt.KeyPress
        NDecimal(sender, e)
    End Sub

    Private Sub btnSalvarEdtAval_Click(sender As Object, e As EventArgs) Handles btnSalvarEdtAval.Click
        EditarAvaliadores()
        btnStatus.Text = "Salvando..."
        btnStatus.Refresh()
        Threading.Thread.Sleep(500)
        btnStatus.Text = ""
        BuscarAvaliadores()
        ConfGridTemp()
        pnlEdtAval.Visible = False
    End Sub
    Private Sub EditarAvaliadores()
        Dim cmd As New SQLiteCommand
        Dim sql As String = "Update Avaliadores Set ListaAvaliadores=@ListaAvaliadores,Abx=@Abx,Abaixo=@Abaixo,Ideal=@Ideal,Acm=@Acm,Acima=@Acima where ID=@ID"

        Try
            abrir()
            cmd = New SQLiteCommand(sql, con)
            cmd.Parameters.AddWithValue("@ListaAvaliadores", txtNomeAvaliador.Text)
            cmd.Parameters.AddWithValue("@Abx", txtAbxMt.Text)
            cmd.Parameters.AddWithValue("@Abaixo", "<" & txtAbxMt.Text)
            cmd.Parameters.AddWithValue("@Ideal", txtIdealMt.Text)
            cmd.Parameters.AddWithValue("@Acm", txtAcmMt.Text)
            cmd.Parameters.AddWithValue("@Acima", ">" & txtAcmMt.Text)
            cmd.Parameters.AddWithValue("@ID", lblIdAval.Text)
            cmd.ExecuteNonQuery()
            ' MsgBox("Lote atualizado com sucesso!")
        Catch ex As Exception
            MsgBox("Erro ao atualizar!" + ex.Message)
            fechar()
        End Try
        txtNomeAvaliador.Text = ""
        txtAbxMt.Text = ""
        txtIdealMt.Text = ""
        txtAcmMt.Text = ""

    End Sub

    'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX       LOTES DE ANIMAIS     XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX


    Private Sub btnSalvarLotes_Click(sender As Object, e As EventArgs)
        CadastrarAnimais()
        BuscarAnimais()
        'DesabilitarCamposDA()
    End Sub

    Private Sub txtprecoleite2_MouseClick(sender As Object, e As MouseEventArgs)
        Me.txtPrecoLeite2.SelectAll()
    End Sub

    'Private Sub dtgDadAnimais_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs)

    '    'Carrregar dados
    '    txtNomeLote.Text = dtgDadAnimais.CurrentRow.Cells(1).Value
    '    cbxCat.Text = dtgDadAnimais.CurrentRow.Cells(2).Value
    '    cbxPasto.Text = dtgDadAnimais.CurrentRow.Cells(8).Value
    '    txtQtdAnimais.Text = dtgDadAnimais.CurrentRow.Cells(3).Value
    '    txtPVivo.Text = dtgDadAnimais.CurrentRow.Cells(4).Value
    '    txtLt.Text = dtgDadAnimais.CurrentRow.Cells(5).Value
    '    txtDel.Text = dtgDadAnimais.CurrentRow.Cells(6).Value
    '    txtQtdOrdDia.Text = dtgDadAnimais.CurrentRow.Cells(7).Value
    '    txtDist.Text = dtgDadAnimais.CurrentRow.Cells(9).Value
    '    txtdiasgest.Text = dtgDadAnimais.CurrentRow.Cells(10).Value
    '    txtgordura.Text = dtgDadAnimais.CurrentRow.Cells(11).Value
    '    txtprotn.Text = dtgDadAnimais.CurrentRow.Cells(12).Value
    '    txtlctse.Text = dtgDadAnimais.CurrentRow.Cells(13).Value
    '    txtsobra.Text = dtgDadAnimais.CurrentRow.Cells(14).Value
    '    txtPrecoLeite2.Text = dtgDadAnimais.CurrentRow.Cells(15).Value

    '    DesabilitarCamposDA()
    'End Sub

    Private Sub btnExcluirDAnim_Click(sender As Object, e As EventArgs)
        DeleteLote()
        BuscarAnimais()

    End Sub

    'Private Sub DesabilitarCamposDA()

    '    'Desabilitar campos
    '    txtQtdAnimais.Enabled = False
    '    txtNomeLote.Enabled = False
    '    txtPVivo.Enabled = False
    '    cbxPasto.Enabled = False
    '    txtsobra.Enabled = False

    '    cbxCat.Enabled = False
    '    txtLt.Enabled = False
    '    txtgordura.Enabled = False
    '    txtprotn.Enabled = False
    '    txtDel.Enabled = False
    '    txtlctse.Enabled = False
    '    txtPrecoLeite2.Enabled = False
    '    txtdiasgest.Enabled = False
    '    txtQtdOrdDia.Enabled = False
    '    txtDist.Enabled = False

    'End Sub

    'Private Sub btnDadAnimais_Click(sender As Object, e As EventArgs)

    '    BuscarAnimais()
    '    DesabilitarCamposDA()
    '    On Error Resume Next
    '    With Me.dtgDadAnimais

    '        .ColumnHeadersDefaultCellStyle.BackColor = Color.Gainsboro
    '        .ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Regular)
    '        .DefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Regular)

    '        .Columns(1).HeaderText = "Lotes"

    '        .Columns(0).Visible = False
    '        .Columns(1).Width = 100
    '        .Columns(2).Visible = False
    '        .Columns(3).Visible = False
    '        .Columns(4).Visible = False
    '        .Columns(5).Visible = False
    '        .Columns(6).Visible = False
    '        .Columns(7).Visible = False
    '        .Columns(8).Visible = False
    '        .Columns(9).Visible = False
    '        .Columns(10).Visible = False
    '        .Columns(11).Visible = False
    '        .Columns(12).Visible = False
    '        .Columns(13).Visible = False
    '        .Columns(14).Visible = False
    '        .Columns(15).Visible = False
    '        .Columns(16).Visible = False
    '    End With
    'End Sub



    'Private Sub Button7_Click(sender As Object, e As EventArgs)
    '    Dim frm As New frmAlimento
    '    frm.ShowDialog()
    'End Sub

    'Private Sub Button4_Click(sender As Object, e As EventArgs)
    '    Dim frm As New frmDieta
    '    'Me.Hide()
    '    frm.ShowDialog()
    'End Sub

    'Private Sub Button4_Click_1(sender As Object, e As EventArgs)
    '    Me.TransparencyKey = ForeColor
    'End Sub

    Private Sub btnLotesMenuLatDt_Click(sender As Object, e As EventArgs) Handles btnLotesMenuLatDt.Click
        Me.tbLotes.Parent = Me.tcMenu
        Me.tcMenu.SelectedTab = tbLotes
    End Sub

    Private Sub btnFecharCadLotes_Click_1(sender As Object, e As EventArgs) Handles btnFecharCadLotes.Click
        pnlCadLotes.Visible = False
    End Sub
    Private Sub btnNovaDieta_Click(sender As Object, e As EventArgs) Handles btnNovaDieta.Click
        BuscarAnimais()
        ' CarregarListaLotes()

        Dim x As Integer = dtgDadAnimais.Rows.Count
        If x > 0 Then
            Dim frm As New frmDieta
            'Me.Hide()
            frm.ShowDialog()
            'Label5.Visible = True
            'Threading.Thread.Sleep(10000)
            'Label5.Visible = False
        Else
            MsgBox("Para criar uma dieta, você precisa ter 01 ou mais lotes de animais cadastrados.")
        End If

    End Sub

    Private Sub btnCadastrarAlimento_Click(sender As Object, e As EventArgs) Handles btnCadastrarAlimento.Click
        edtAlim = False

        pnlCadAlimentos.Visible = True
        pnlCadAlimentos.Location = New Point(34, 11)
        pnlCadAlimentos.BringToFront()

        'load
        BuscarAlimentos()
        rdbMS.Checked = True
        ConfigGridAlimentos()

        'btnSalvarAlimEdt.Visible = False
        lblID.Text = v_id
        If edtAlim = True Then
            PreencherEdtAlimentos()
        Else
            btnCadastrarMSMO.Visible = True
            btnSalvarAlimEdt.Visible = False
        End If
        'pnlTxtAli.BackColor = Color.FromArgb(50, 0, 0, 0)



        ' Ligando os avaliadores personalizáveis as variáveis não votáteis
        lbl1.Visible = My.Settings.text1
        lbl2.Visible = My.Settings.text2
        lbl3.Visible = My.Settings.text3
        lbl4.Visible = My.Settings.text4
        lbl5.Visible = My.Settings.text5
        lbl6.Visible = My.Settings.text6
        lbl7.Visible = My.Settings.text7
        lbl8.Visible = My.Settings.text8
        lbl9.Visible = My.Settings.text9
        'lbl10.Visible = My.Settings.text10

        txt1.Visible = My.Settings.text1
        txt2.Visible = My.Settings.text2
        txt3.Visible = My.Settings.text3
        txt4.Visible = My.Settings.text4
        txt5.Visible = My.Settings.text5
        txt6.Visible = My.Settings.text6
        txt7.Visible = My.Settings.text7
        txt8.Visible = My.Settings.text8
        txt9.Visible = My.Settings.text9
        'txt10.Visible = My.Settings.text10

        lbl1.Text = My.Settings.lab1
        lbl2.Text = My.Settings.lab2
        lbl3.Text = My.Settings.lab3
        lbl4.Text = My.Settings.lab4
        lbl5.Text = My.Settings.lab5
        lbl6.Text = My.Settings.lab6
        lbl7.Text = My.Settings.lab7
        lbl8.Text = My.Settings.lab8
        lbl9.Text = My.Settings.lab9
        'lbl10.Text = My.Settings.lab10

        My.Settings.lab1 = lbl1.Text
        My.Settings.lab2 = lbl2.Text
        My.Settings.lab3 = lbl3.Text
        My.Settings.lab4 = lbl4.Text
        My.Settings.lab5 = lbl5.Text
        My.Settings.lab6 = lbl6.Text
        My.Settings.lab7 = lbl7.Text
        My.Settings.lab8 = lbl8.Text
        My.Settings.lab9 = lbl9.Text
        'My.Settings.lab10 = lbl10.Text

        'Dim frm As New frmAlimento
        ' ''Me.Hide()
        'frm.ShowDialog()
    End Sub



    Private Sub picUsuario_Click(sender As Object, e As EventArgs) Handles picUsuario.Click
        pnlMenuUser.Visible = True
        pnlMenuUser.BringToFront()
        pnlMenuUser.Location = New Point(1166, 61)

        'tcMenu.SendToBack()
    End Sub

    Private Sub dtgEscolheerCliente_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dtgEscolheerCliente.CellClick
        Label3.Text = "Editar Fazenda"
        pnlEscolherFaz.Visible = False
        pnlCadCliente.Location = New Point(400, 2)
        pnlCadCliente.BringToFront()
        pnlCadCliente.Visible = True
        pnlEdtFaz.Visible = True

        btnSalvarFazenda.Visible = True
        btnCadastrarFazenda.Visible = False

        txtNomeFazenda.Text = dtgEscolheerCliente.CurrentRow.Cells(0).Value
        txtProdutor.Text = dtgEscolheerCliente.CurrentRow.Cells(1).Value
        txtMunicipioFazenda.Text = dtgEscolheerCliente.CurrentRow.Cells(2).Value
        CbxEstadoFazenda.Text = dtgEscolheerCliente.CurrentRow.Cells(3).Value
        txtLocalizacaoFazenda.Text = dtgEscolheerCliente.CurrentRow.Cells(4).Value
        txtTecRespFazenda.Text = dtgEscolheerCliente.CurrentRow.Cells(5).Value
        txtFone.Text = dtgEscolheerCliente.CurrentRow.Cells(6).Value
        txtNascimento.Text = dtgEscolheerCliente.CurrentRow.Cells(7).Value
        txtFoto.Text = dtgEscolheerCliente.CurrentRow.Cells(8).Value
        idFaz = dtgEscolheerCliente.CurrentRow.Cells(10).Value
        Dim logofaz As String = txtFoto.Text
        If logofaz = "" Then
            picLogoFaz.Image = My.Resources.fzda
        Else
            picLogoFaz.Image = New System.Drawing.Bitmap(logofaz)
        End If
        Label4.Text = idFaz
    End Sub

    Private Sub Button16_Click(sender As Object, e As EventArgs) Handles Button16.Click
        pnlEscolherFaz.Visible = False
    End Sub

    Private Sub btnXAplicativo_Click(sender As Object, e As EventArgs) Handles btnXAplicativo.Click
        'Me.Hide()
        Me.Dispose()
        Application.Exit()

    End Sub

    Private Sub rdbCorAvalOn_CheckedChanged(sender As Object, e As EventArgs) Handles rdbCorAvalOn.CheckedChanged
        My.Settings.corAvalOnOf = True
    End Sub

    Private Sub rdbCorAvalOf_CheckedChanged(sender As Object, e As EventArgs) Handles rdbCorAvalOf.CheckedChanged
        My.Settings.corAvalOnOf = False
    End Sub

    Private Sub btnConfExcluirDieta_Click(sender As Object, e As EventArgs)
      
        btnStatus.Text = "Excluindo..."
        btnStatus.Refresh()
        Threading.Thread.Sleep(1000)
        btnStatus.Text = ""

        CarregarCardsDieta()
    End Sub

    Dim enderecolocal As String
    Private Sub txtLocalizacaoFazenda_Click(sender As Object, e As EventArgs) Handles txtLocalizacaoFazenda.Click

        enderecolocal = Clipboard.GetText()

        If enderecolocal.Contains("maps.app.goo.gl") Then
            Me.txtLocalizacaoFazenda.Text = enderecolocal
            'lblMsgm.Visible = False
        Else
            MsgBox("O texto não parece ser um link válido")
        End If
    End Sub

    Private Sub pnlCard03_Paint(sender As Object, e As PaintEventArgs) Handles pnlCard03.Paint

    End Sub

    'Private Sub btnAvaNutri_Click(sender As Object, e As EventArgs)
    '    lblAvaTipo.Text = "Avaliadores Nutricionais:"
    '    cbxAvaliadores.Text = "Nutricionais"
    '    BuscarAvaliadores()
    '    ' btnAvaNutri.BackgroundImage = My.Resources.mn_on
    'End Sub

    'Private Sub btnAvaRelac_Click(sender As Object, e As EventArgs)
    '    lblAvaTipo.Text = "Avaliadores Relacionais:"
    '    cbxAvaliadores.Text = "Relacionais"
    '    BuscarAvaliadores()
    'End Sub


    Private Sub pnlCard02_Paint(sender As Object, e As PaintEventArgs) Handles pnlCard02.Paint

    End Sub


    Private Sub btnVoltarFazenda_Click(sender As Object, e As EventArgs) Handles btnVoltarFazenda.Click
        Me.tbFazendas.Parent = Me.tcMenu
        Me.tcMenu.SelectedTab = tbFazendas
    End Sub

    Private Sub btnVoltarFaz_Click(sender As Object, e As EventArgs) Handles btnVoltarFaz.Click
        Me.tbFazendas.Parent = Me.tcMenu
        Me.tcMenu.SelectedTab = tbFazendas
    End Sub

    Private Sub tbConfig_Click(sender As Object, e As EventArgs) Handles tbConfig.Click
        pnlMenuUser.Visible = False
    End Sub

    Private Sub tbHome_Click(sender As Object, e As EventArgs) Handles tbHome.Click
        pnlMenuUser.Visible = False
    End Sub

    Private Sub tbAlimentos_Click(sender As Object, e As EventArgs) Handles tbAlimentos.Click
        pnlMenuUser.Visible = False
    End Sub

    Private Sub tbFazendas_Click(sender As Object, e As EventArgs) Handles tbFazendas.Click
        pnlMenuUser.Visible = False
    End Sub

    Private Sub tbLotes_Click(sender As Object, e As EventArgs) Handles tbLotes.Click
        pnlMenuUser.Visible = False
    End Sub

    'Campo aceita apenas decimais
    Private Sub txtPrecoLeite2_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtPrecoLeite2.KeyPress
        NDecimal(sender, e)
    End Sub

    Private Sub txtQtdAnimais_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtQtdAnimais.KeyPress
        NDecimal(sender, e)
    End Sub

    Private Sub txtPVivo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtPVivo.KeyPress
        NDecimal(sender, e)
    End Sub

    Private Sub txtQtdOrdDia_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtQtdOrdDia.KeyPress
        NDecimal(sender, e)
    End Sub

    Private Sub txtLt_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtLt.KeyPress
        NDecimal(sender, e)
    End Sub

    Private Sub txtDel_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtDel.KeyPress
        NDecimal(sender, e)
    End Sub

    Private Sub txtDist_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtDist.KeyPress
        NDecimal(sender, e)
    End Sub

    Private Sub txtgordura_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtgordura.KeyPress
        NDecimal(sender, e)
    End Sub

    Private Sub txtsobra_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtsobra.KeyPress
        NDecimal(sender, e)
    End Sub

    Private Sub txtprotn_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtprotn.KeyPress
        NDecimal(sender, e)
    End Sub

    Private Sub txtdiasgest_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtdiasgest.KeyPress
        NDecimal(sender, e)
    End Sub

    Private Sub txtlctse_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtlctse.KeyPress
        NDecimal(sender, e)
    End Sub

    Private Sub dtgAlimentoNome_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dtgAlimentoNome.CellContentClick
        On Error Resume Next
        With dtgAlimentoNome
            If .CurrentCell.ColumnIndex = 0 Then
                edtAlim = True

                almntoFamilia = .CurrentRow.Cells(1).Value
                almnto = .CurrentRow.Cells(2).Value
                v_MS = .CurrentRow.Cells(3).Value
                v_PB = .CurrentRow.Cells(4).Value
                v_PDR = .CurrentRow.Cells(5).Value
                v_PND = .CurrentRow.Cells(6).Value
                v_FDN = .CurrentRow.Cells(7).Value
                v_eFDN = .CurrentRow.Cells(8).Value
                v_MNmaior8 = .CurrentRow.Cells(9).Value
                v_MNmaior19 = .CurrentRow.Cells(10).Value
                v_FDNF = .CurrentRow.Cells(11).Value
                v_FDA = .CurrentRow.Cells(12).Value
                v_Nel = .CurrentRow.Cells(13).Value
                v_NDT = .CurrentRow.Cells(14).Value
                v_EE = .CurrentRow.Cells(15).Value
                v_EE_Insat = .CurrentRow.Cells(16).Value
                v_Cinzas = .CurrentRow.Cells(17).Value
                v_CNF = .CurrentRow.Cells(18).Value
                v_Amido = .CurrentRow.Cells(19).Value
                v_kd_Amid = .CurrentRow.Cells(20).Value
                v_MOR = .CurrentRow.Cells(21).Value
                v_Ca = .CurrentRow.Cells(22).Value
                v_P = .CurrentRow.Cells(23).Value
                v_Mg = .CurrentRow.Cells(24).Value
                v_K = .CurrentRow.Cells(25).Value
                v_S = .CurrentRow.Cells(26).Value
                v_Na = .CurrentRow.Cells(27).Value
                v_Cl = .CurrentRow.Cells(28).Value
                v_Co = .CurrentRow.Cells(29).Value
                v_Cu = .CurrentRow.Cells(30).Value
                v_Mn = .CurrentRow.Cells(31).Value
                v_Zn = .CurrentRow.Cells(32).Value
                v_Se = .CurrentRow.Cells(33).Value
                v_I = .CurrentRow.Cells(34).Value
                v_A = .CurrentRow.Cells(35).Value
                v_D = .CurrentRow.Cells(36).Value
                v_E = .CurrentRow.Cells(37).Value
                v_Cromo = .CurrentRow.Cells(38).Value
                v_Biotina = .CurrentRow.Cells(39).Value
                v_Virginiamicina = .CurrentRow.Cells(40).Value
                v_Monensina = .CurrentRow.Cells(41).Value
                v_Levedura = .CurrentRow.Cells(42).Value
                v_Arginina = .CurrentRow.Cells(43).Value
                v_Histidina = .CurrentRow.Cells(44).Value
                v_Isoleucina = .CurrentRow.Cells(45).Value
                v_Leucina = .CurrentRow.Cells(46).Value
                v_Lisina = .CurrentRow.Cells(47).Value
                v_Metionina = .CurrentRow.Cells(48).Value
                v_Fenilalanina = .CurrentRow.Cells(49).Value
                v_Treonina = .CurrentRow.Cells(50).Value
                v_Triptofano = .CurrentRow.Cells(51).Value
                v_Valina = .CurrentRow.Cells(52).Value
                v_dFDNp48h = .CurrentRow.Cells(53).Value
                v_dAmido_7h = .CurrentRow.Cells(54).Value
                v_TTNDFD = .CurrentRow.Cells(55).Value
                v_id = .CurrentRow.Cells(66).Value

                'abrir p edição xxxxxxxxxxxxxxxxxxxxxxx
               
                pnlCadAlimentos.Visible = True
                pnlCadAlimentos.Location = New Point(34, 11)
                pnlCadAlimentos.BringToFront()

                'load
                BuscarAlimentos()
                rdbMS.Checked = True
                ConfigGridAlimentos()

                'btnSalvarAlimEdt.Visible = False
                lblID.Text = v_id
                If edtAlim = True Then
                    PreencherEdtAlimentos()
                Else
                    btnCadastrarMSMO.Visible = True
                    btnSalvarAlimEdt.Visible = False
                End If
                'pnlTxtAli.BackColor = Color.FromArgb(50, 0, 0, 0)



                ' Ligando os avaliadores personalizáveis as variáveis não votáteis
                lbl1.Visible = My.Settings.text1
                lbl2.Visible = My.Settings.text2
                lbl3.Visible = My.Settings.text3
                lbl4.Visible = My.Settings.text4
                lbl5.Visible = My.Settings.text5
                lbl6.Visible = My.Settings.text6
                lbl7.Visible = My.Settings.text7
                lbl8.Visible = My.Settings.text8
                lbl9.Visible = My.Settings.text9
                'lbl10.Visible = My.Settings.text10

                txt1.Visible = My.Settings.text1
                txt2.Visible = My.Settings.text2
                txt3.Visible = My.Settings.text3
                txt4.Visible = My.Settings.text4
                txt5.Visible = My.Settings.text5
                txt6.Visible = My.Settings.text6
                txt7.Visible = My.Settings.text7
                txt8.Visible = My.Settings.text8
                txt9.Visible = My.Settings.text9
                'txt10.Visible = My.Settings.text10

                lbl1.Text = My.Settings.lab1
                lbl2.Text = My.Settings.lab2
                lbl3.Text = My.Settings.lab3
                lbl4.Text = My.Settings.lab4
                lbl5.Text = My.Settings.lab5
                lbl6.Text = My.Settings.lab6
                lbl7.Text = My.Settings.lab7
                lbl8.Text = My.Settings.lab8
                lbl9.Text = My.Settings.lab9
                'lbl10.Text = My.Settings.lab10

                My.Settings.lab1 = lbl1.Text
                My.Settings.lab2 = lbl2.Text
                My.Settings.lab3 = lbl3.Text
                My.Settings.lab4 = lbl4.Text
                My.Settings.lab5 = lbl5.Text
                My.Settings.lab6 = lbl6.Text
                My.Settings.lab7 = lbl7.Text
                My.Settings.lab8 = lbl8.Text
                My.Settings.lab9 = lbl9.Text
                'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
              
            End If
        End With

    End Sub

    Private Sub rdbBuscarMO_Click(sender As Object, e As EventArgs) Handles rdbBuscarMO.Click
        On Error Resume Next
        For Each row As DataGridViewRow In dtgAlimentoNome.Rows
            row.Cells(4).Value = row.Cells(4).Value * row.Cells(3).Value / 100
            row.Cells(5).Value = row.Cells(5).Value * row.Cells(3).Value / 100
            row.Cells(6).Value = row.Cells(6).Value * row.Cells(3).Value / 100
            row.Cells(7).Value = row.Cells(7).Value * row.Cells(3).Value / 100
            row.Cells(8).Value = row.Cells(8).Value * row.Cells(3).Value / 100

            row.Cells(11).Value = row.Cells(11).Value * row.Cells(3).Value / 100
            row.Cells(12).Value = row.Cells(12).Value * row.Cells(3).Value / 100
            row.Cells(13).Value = row.Cells(13).Value * row.Cells(3).Value / 100
            row.Cells(14).Value = row.Cells(14).Value * row.Cells(3).Value / 100
            row.Cells(15).Value = row.Cells(15).Value * row.Cells(3).Value / 100
            row.Cells(16).Value = row.Cells(16).Value * row.Cells(3).Value / 100
            row.Cells(17).Value = row.Cells(17).Value * row.Cells(3).Value / 100
            row.Cells(18).Value = row.Cells(18).Value * row.Cells(3).Value / 100
            row.Cells(19).Value = row.Cells(19).Value * row.Cells(3).Value / 100
            row.Cells(20).Value = row.Cells(20).Value * row.Cells(3).Value / 100
            row.Cells(21).Value = row.Cells(21).Value * row.Cells(3).Value / 100
            row.Cells(22).Value = row.Cells(22).Value * row.Cells(3).Value / 100
            row.Cells(23).Value = row.Cells(23).Value * row.Cells(3).Value / 100
            row.Cells(24).Value = row.Cells(24).Value * row.Cells(3).Value / 100
            row.Cells(25).Value = row.Cells(25).Value * row.Cells(3).Value / 100
            row.Cells(26).Value = row.Cells(26).Value * row.Cells(3).Value / 100
            row.Cells(27).Value = row.Cells(27).Value * row.Cells(3).Value / 100
            row.Cells(28).Value = row.Cells(28).Value * row.Cells(3).Value / 100
            row.Cells(29).Value = row.Cells(29).Value * row.Cells(3).Value / 100
            row.Cells(30).Value = row.Cells(30).Value * row.Cells(3).Value / 100
            row.Cells(31).Value = row.Cells(31).Value * row.Cells(3).Value / 100
            row.Cells(32).Value = row.Cells(32).Value * row.Cells(3).Value / 100
            row.Cells(33).Value = row.Cells(33).Value * row.Cells(3).Value / 100
            row.Cells(34).Value = row.Cells(34).Value * row.Cells(3).Value / 100
            row.Cells(35).Value = row.Cells(35).Value * row.Cells(3).Value / 100
            row.Cells(36).Value = row.Cells(36).Value * row.Cells(3).Value / 100
            row.Cells(37).Value = row.Cells(37).Value * row.Cells(3).Value / 100
            row.Cells(38).Value = row.Cells(38).Value * row.Cells(3).Value / 100
            row.Cells(39).Value = row.Cells(39).Value * row.Cells(3).Value / 100
            row.Cells(40).Value = row.Cells(40).Value * row.Cells(3).Value / 100
            row.Cells(41).Value = row.Cells(41).Value * row.Cells(3).Value / 100
            row.Cells(42).Value = row.Cells(42).Value * row.Cells(3).Value / 100
            row.Cells(43).Value = row.Cells(43).Value * row.Cells(3).Value / 100
            row.Cells(44).Value = row.Cells(44).Value * row.Cells(3).Value / 100
            row.Cells(45).Value = row.Cells(45).Value * row.Cells(3).Value / 100
            row.Cells(46).Value = row.Cells(46).Value * row.Cells(3).Value / 100
            row.Cells(47).Value = row.Cells(47).Value * row.Cells(3).Value / 100
            row.Cells(48).Value = row.Cells(48).Value * row.Cells(3).Value / 100
            row.Cells(49).Value = row.Cells(49).Value * row.Cells(3).Value / 100
            row.Cells(50).Value = row.Cells(50).Value * row.Cells(3).Value / 100
            row.Cells(51).Value = row.Cells(51).Value * row.Cells(3).Value / 100
            row.Cells(52).Value = row.Cells(52).Value * row.Cells(3).Value / 100
            row.Cells(53).Value = row.Cells(53).Value * row.Cells(3).Value / 100
            row.Cells(54).Value = row.Cells(54).Value * row.Cells(3).Value / 100
            row.Cells(55).Value = row.Cells(55).Value * row.Cells(3).Value / 100
            row.Cells(56).Value = row.Cells(56).Value * row.Cells(3).Value / 100
            row.Cells(57).Value = row.Cells(57).Value * row.Cells(3).Value / 100
            row.Cells(58).Value = row.Cells(58).Value * row.Cells(3).Value / 100
            row.Cells(59).Value = row.Cells(59).Value * row.Cells(3).Value / 100
            row.Cells(60).Value = row.Cells(60).Value * row.Cells(3).Value / 100
            row.Cells(61).Value = row.Cells(61).Value * row.Cells(3).Value / 100
            row.Cells(62).Value = row.Cells(62).Value * row.Cells(3).Value / 100
            row.Cells(63).Value = row.Cells(63).Value * row.Cells(3).Value / 100
            'row.Cells(64).Value = row.Cells(64).Value * row.Cells(3).Value / 100
            'row.Cells(65).Value = row.Cells(65).Value * row.Cells(3).Value / 100
            'row.Cells(66).Value = row.Cells(66).Value * row.Cells(3).Value / 100
            'row.Cells(67).Value = row.Cells(67).Value * row.Cells(3).Value / 100
            'row.Cells(68).Value = row.Cells(68).Value * row.Cells(3).Value / 100
            'row.Cells(69).Value = row.Cells(69).Value * row.Cells(3).Value / 100
            'row.Cells(70).Value = row.Cells(70).Value * row.Cells(3).Value / 100
            'row.Cells(71).Value = row.Cells(71).Value * row.Cells(3).Value / 100
            'row.Cells(72).Value = row.Cells(72).Value * row.Cells(3).Value / 100
            'row.Cells(73).Value = row.Cells(73).Value * row.Cells(3).Value / 100
            GridAlimentos()
            pnlMSN.BackColor = Color.FromArgb(233, 245, 227)
        Next
    End Sub

    'Private Sub frmDieta_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
    '    'For Each frm As Form In frmMenu.MdiChildren
    '    'If frm.GetType.Name = "frmDieta" Then
    '    'Dim f As frmDieta = frm
    '    AtualizarDietas()
    '    ' End If
    '    'Next
    'End Sub

    Private Sub txtaabbcc_TextChanged(sender As Object, e As EventArgs) Handles txtaabbcc.TextChanged
        'If txtaabbcc.Text <> "" Then
        '    Me.tbDietas.Parent = Me.tcMenu
        '    Me.tcMenu.SelectedTab = tbDietas
        '    BuscarDietas()
        '    CarregarListaDieta()
        '    lblNFazDt.Text = lblNFaz.Text & "           "
        '    idDieta = ""
        'End If
    End Sub

    Private Sub Button19_Click(sender As Object, e As EventArgs) Handles Button19.Click
        Dim x As Integer = dtgDadAnimais.Rows.Count
        If x > 0 Then
            Dim frm As New frmManejo
            'Me.Hide()
            frm.ShowDialog()
            'Label5.Visible = True
            'Threading.Thread.Sleep(10000)
            'Label5.Visible = False
        Else
            MsgBox("Para criar uma relatório de manejo, você precisa ter 01 ou mais lotes de animais cadastrados.")
        End If
    End Sub

    Private Sub pnlCard01_Paint(sender As Object, e As PaintEventArgs) Handles pnlCard01.Paint

    End Sub

    Private Sub Button59_Click(sender As Object, e As EventArgs) Handles Button59.Click
        Dim x As Integer = dtgDadAnimais.Rows.Count
        If x > 0 Then
            Dim frm As New frmManejo
            'Me.Hide()
            frm.ShowDialog()
            'Label5.Visible = True
            'Threading.Thread.Sleep(10000)
            'Label5.Visible = False
        Else
            MsgBox("Para criar uma relatório de manejo, você precisa ter 01 ou mais lotes de animais cadastrados.")
        End If
    End Sub

    Public Sub AtualizarDados()
        'Threading.Thread.Sleep(10000)
        frmDieta.Show()
        ' MessageBox.Show("Dados atualizados!")
    End Sub


    'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX         ALIMENTOS        XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX


    Private Sub CadastrarAlimentosMS()
        Dim sql As String
        Dim cmd As SQLiteCommand

        'sql = "Insert into AlimentosMO (Chk,AlimentoFamilia,Alimento,MS,PB,PDR,PND,FDN,eFDN,eFDN2,MNmaior8,MNmaior19,FDNF,FDA,Nel,NDT,EE,EE_Insat,Cinzas,CNF,Amido,kd_Amido,Ca,P,Mg,K,S,Na,Cl,Co,Cu,Mn,Zn,Se,I,A,D,E,Cromo,DCAD,Biotina,Virginiamicina,Monensina,Levedura,Arginina,Histidina,Isoleucina,Leucina,Lisina,Metionina,Fenilalanina,Treonina,Triptofano,Valina,dFDNp_48h,dAmido_7h,TTNDFD,Pers1,Pers2,Pers3,Pers4,Pers5,Pers6,Pers7,Pers8,Pers9) values (@Chk,@AlimentoFamilia,@Alimento,@MS,@PB,@PDR,@PND,@FDN,@eFDN,@eFDN2,@MNmaior8,@MNmaior19,@FDNF,@FDA,@Nel,@NDT,@EE,@EE_Insat,@Cinzas,@CNF,@Amido,@kd_Amido,@Ca,@P,@Mg,@K,@S,@Na,@Cl,@Co,@Cu,@Mn,@Zn,@Se,@I,@A,@D,@E,@Cromo,@DCAD,@Biotina,@Virginiamicina,@Monensina,@Levedura,@Arginina,@Histidina,@Isoleucina,@Leucina,@Lisina,@Metionina,@Fenilalanina,@Treonina,@Triptofano,@Valina,@dFDNp_48h,@dAmido_7h,@TTNDF,@Pers1,@Pers2,@Pers3,@Pers4,@Pers5,@Pers6,@Pers7,@Pers8,@Pers9)"

        sql = "Insert into AlimentosMS (AlimentoFamilia,Alimento,MS,PB,PDR,PNDR,FDN,eFDN,MNmaior8,MNmaior19,FDNF,FDA,Nel,NDT,EE,EE_Insat,Cinzas,CNF,Amido,kd_Amido,MOR,Ca,P,Mg,K,S,Na,Cl,Co,Cu,Mn,Zn,Se,I,A,D,E,Cromo,Biotina,Virginiamicina,Monensina,Levedura,Arginina,Histidina,Isoleucina,Leucina,Lisina,Metionina,Fenilalanina,Treonina,Triptofano,Valina,dFDNp_48h,dAmido_7h,TTNDFD,Pers1,Pers2,Pers3,Pers4,Pers5,Pers6,Pers7,Pers8,Pers9,Chk) values (@AlimentoFamilia,@Alimento,@MS,@PB,@PDR,@PNDR,@FDN,@eFDN,@MNmaior8,@MNmaior19,@FDNF,@FDA,@Nel,@NDT,@EE,@EE_Insat,@Cinzas,@CNF,@Amido,@kd_Amido,@MOR,@Ca,@P,@Mg,@K,@S,@Na,@Cl,@Co,@Cu,@Mn,@Zn,@Se,@I,@A,@D,@E,@Cromo,@Biotina,@Virginiamicina,@Monensina,@Levedura,@Arginina,@Histidina,@Isoleucina,@Leucina,@Lisina,@Metionina,@Fenilalanina,@Treonina,@Triptofano,@Valina,@dFDNp_48h,@dAmido_7h,@TTNDFD,@Pers1,@Pers2,@Pers3,@Pers4,@Pers5,@Pers6,@Pers7,@Pers8,@Pers9,@Chk)"

        If txtAlimento.Text <> "" And cbxFuncao.Text <> "" Then

            Try

                abrir()

                cmd = New SQLiteCommand(sql, con)
                cmd.Parameters.AddWithValue("@AlimentoFamilia", cbxFuncao.Text)
                cmd.Parameters.AddWithValue("@Alimento", nomeAlimento)
                cmd.Parameters.AddWithValue("@MS", txtMS.Text)
                cmd.Parameters.AddWithValue("@PB", txtPB.Text)
                cmd.Parameters.AddWithValue("@PDR", txtPDR.Text)
                cmd.Parameters.AddWithValue("@PNDR", txtPND.Text)
                cmd.Parameters.AddWithValue("@FDN", txtFDN.Text)
                cmd.Parameters.AddWithValue("@eFDN", txtEFDN.Text)
                cmd.Parameters.AddWithValue("@MNmaior8", txtMNmaior8.Text)
                cmd.Parameters.AddWithValue("@MNmaior19", txtMNmaior19.Text)
                cmd.Parameters.AddWithValue("@FDNF", txtFDNF.Text)
                cmd.Parameters.AddWithValue("@FDA", txtFDA.Text)
                cmd.Parameters.AddWithValue("@Nel", txtNel.Text)
                cmd.Parameters.AddWithValue("@NDT", txtNDT.Text)
                cmd.Parameters.AddWithValue("@EE", txtEE.Text)
                cmd.Parameters.AddWithValue("@EE_Insat", txtEE_Insat.Text)
                cmd.Parameters.AddWithValue("@Cinzas", txtCinzas.Text)
                cmd.Parameters.AddWithValue("@CNF", txtCNF.Text)
                cmd.Parameters.AddWithValue("@Amido", txtAmido.Text)
                cmd.Parameters.AddWithValue("@kd_Amido", txtkd_Amid.Text)
                cmd.Parameters.AddWithValue("@MOR", txtMOR.Text)
                cmd.Parameters.AddWithValue("@Ca", txtCa.Text)
                cmd.Parameters.AddWithValue("@P", txtP.Text)
                cmd.Parameters.AddWithValue("@Mg", txtMg.Text)
                cmd.Parameters.AddWithValue("@K", txtK.Text)
                cmd.Parameters.AddWithValue("@S", txtS.Text)
                cmd.Parameters.AddWithValue("@Na", txtNa.Text)
                cmd.Parameters.AddWithValue("@Cl", txtCl.Text)
                cmd.Parameters.AddWithValue("@Co", txtCo.Text)
                cmd.Parameters.AddWithValue("@Cu", txtCu.Text)
                cmd.Parameters.AddWithValue("@Mn", txtMn.Text)
                cmd.Parameters.AddWithValue("@Zn", txtZn.Text)
                cmd.Parameters.AddWithValue("@Se", txtSe.Text)
                cmd.Parameters.AddWithValue("@I", txtI.Text)
                cmd.Parameters.AddWithValue("@A", txtA.Text)
                cmd.Parameters.AddWithValue("@D", txtD.Text)
                cmd.Parameters.AddWithValue("@E", txtE.Text)
                cmd.Parameters.AddWithValue("@Cromo", txtCromo.Text)

                cmd.Parameters.AddWithValue("@Biotina", txtBiotina.Text)
                cmd.Parameters.AddWithValue("@Virginiamicina", txtVirginamicina.Text)
                cmd.Parameters.AddWithValue("@Monensina", txtMonensina.Text)
                cmd.Parameters.AddWithValue("@Levedura", txtLevedura.Text)

                cmd.Parameters.AddWithValue("@Arginina", txtArginina.Text)
                cmd.Parameters.AddWithValue("@Histidina", txtHistid.Text)
                cmd.Parameters.AddWithValue("@Isoleucina", txtIsoleu.Text)
                cmd.Parameters.AddWithValue("@Leucina", txtLeuc.Text)

                cmd.Parameters.AddWithValue("@Lisina", txtLisina.Text)
                cmd.Parameters.AddWithValue("@Metionina", txtMetionina.Text)

                cmd.Parameters.AddWithValue("@Fenilalanina", txtFelinal.Text)
                cmd.Parameters.AddWithValue("@Treonina", txtTreon.Text)
                cmd.Parameters.AddWithValue("@Triptofano", txtTripto.Text)
                cmd.Parameters.AddWithValue("@Valina", txtValina.Text)

                cmd.Parameters.AddWithValue("@dFDNp_48h", txtdFDNP48h.Text)
                cmd.Parameters.AddWithValue("@dAmido_7h", txtdAmido7h.Text)

                cmd.Parameters.AddWithValue("@TTNDFD", txtTTNDFD.Text)

                cmd.Parameters.AddWithValue("@Pers1", txt1.Text)
                cmd.Parameters.AddWithValue("@Pers2", txt2.Text)
                cmd.Parameters.AddWithValue("@Pers3", txt3.Text)
                cmd.Parameters.AddWithValue("@Pers4", txt4.Text)
                cmd.Parameters.AddWithValue("@Pers5", txt5.Text)
                cmd.Parameters.AddWithValue("@Pers6", txt6.Text)
                cmd.Parameters.AddWithValue("@Pers7", txt7.Text)
                cmd.Parameters.AddWithValue("@Pers8", txt8.Text)
                cmd.Parameters.AddWithValue("@Pers9", txt9.Text)
                cmd.Parameters.AddWithValue("@Chk", "0")

                'aqui adicionar os campos adicionais

                cmd.ExecuteNonQuery()
                MsgBox("Alimento cadastrado com sucesso!")

            Catch ex As Exception
                MsgBox("Erro ao salvar!" + ex.Message)
                fechar()
            End Try

            LimparCampos()
            'btnCadastrarMSMO.Enabled = False

        Else
            MsgBox("Preencha os campos!")

        End If
        pnlNvAlimento.Visible = False
        'CadastrarAlimentosMO()
    End Sub

    Private Sub EditarAlimentosMS()
        Dim sql As String
        Dim cmd As New SQLiteCommand

        sql = "Update alimentosMS set AlimentoFamilia=@AlimentoFamilia,Alimento=@Alimento,MS=@MS,PB=@PB,PDR=@PDR,PNDR=@PNDR,FDN=@FDN,eFDN=@eFDN,MNmaior8=@MNmaior8,MNmaior19=@MNmaior19,FDNF=@FDNF,FDA=@FDA,Nel=@Nel,NDT=@NDT,EE=@EE,EE_Insat=@EE_Insat,Cinzas=@Cinzas,CNF=@CNF,Amido=@Amido,kd_Amido=@kd_Amido,MOR=@MOR,Ca=@Ca,P=@P,Mg=@Mg,K=@K,S=@S,Na=@Na,Cl=@Cl,Co=@Co,Cu=@Cu,Mn=@Mn,Zn=@Zn,Se=@Se,I=@I,A=@A,D=@D,E=@E,Cromo=@Cromo,Biotina=@Biotina,Virginiamicina=@Virginiamicina,Monensina=@Monensina,Levedura=@Levedura,Arginina=@Arginina,Histidina=@Histidina,Isoleucina=@Isoleucina,Leucina=@Leucina,Lisina=@Lisina,Metionina=@Metionina,Fenilalanina=@Fenilalanina,Treonina=@Treonina,Triptofano=@Triptofano,Valina=@Valina,dFDNp_48h=@dFDNp_48h,dAmido_7h=@dAmido_7h,TTNDFD=@TTNDFD,Pers1=@Pers1,Pers2=@Pers2,Pers3=@Pers3,Pers4=@Pers4,Pers5=@Pers5,Pers6=@Pers6,Pers7=@Pers7,Pers8=@Pers8,Pers9=@Pers9,Chk=@Chk where ID=@ID"

        If txtAlimento.Text <> "" And cbxFuncao.Text <> "" And MsgBox("Editar dados da propriedade na base de dados?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            Try
                abrir()
                cmd = New SQLiteCommand(sql, con)
                cmd.Parameters.AddWithValue("@AlimentoFamilia", txtAlimento.Text)
                cmd.Parameters.AddWithValue("@Alimento", nomeAlimento)
                cmd.Parameters.AddWithValue("@MS", txtMS.Text)
                cmd.Parameters.AddWithValue("@PB", txtPB.Text)
                cmd.Parameters.AddWithValue("@PDR", txtPDR.Text)
                cmd.Parameters.AddWithValue("@PNDR", txtPND.Text)
                cmd.Parameters.AddWithValue("@FDN", txtFDN.Text)
                cmd.Parameters.AddWithValue("@eFDN", txtEFDN.Text)
                cmd.Parameters.AddWithValue("@MNmaior8", txtMNmaior8.Text)
                cmd.Parameters.AddWithValue("@MNmaior19", txtMNmaior19.Text)
                cmd.Parameters.AddWithValue("@FDNF", txtFDNF.Text)
                cmd.Parameters.AddWithValue("@FDA", txtFDA.Text)
                cmd.Parameters.AddWithValue("@Nel", txtNel.Text)
                cmd.Parameters.AddWithValue("@NDT", txtNDT.Text)
                cmd.Parameters.AddWithValue("@EE", txtEE.Text)
                cmd.Parameters.AddWithValue("@EE_Insat", txtEE_Insat.Text)
                cmd.Parameters.AddWithValue("@Cinzas", txtCinzas.Text)
                cmd.Parameters.AddWithValue("@CNF", txtCNF.Text)
                cmd.Parameters.AddWithValue("@Amido", txtAmido.Text)
                cmd.Parameters.AddWithValue("@kd_Amido", txtkd_Amid.Text)
                cmd.Parameters.AddWithValue("@MOR", txtMOR.Text)
                cmd.Parameters.AddWithValue("@Ca", txtCa.Text)
                cmd.Parameters.AddWithValue("@P", txtP.Text)
                cmd.Parameters.AddWithValue("@Mg", txtMg.Text)
                cmd.Parameters.AddWithValue("@K", txtK.Text)
                cmd.Parameters.AddWithValue("@S", txtS.Text)
                cmd.Parameters.AddWithValue("@Na", txtNa.Text)
                cmd.Parameters.AddWithValue("@Cl", txtCl.Text)
                cmd.Parameters.AddWithValue("@Co", txtCo.Text)
                cmd.Parameters.AddWithValue("@Cu", txtCu.Text)
                cmd.Parameters.AddWithValue("@Mn", txtMn.Text)
                cmd.Parameters.AddWithValue("@Zn", txtZn.Text)
                cmd.Parameters.AddWithValue("@Se", txtSe.Text)
                cmd.Parameters.AddWithValue("@I", txtI.Text)
                cmd.Parameters.AddWithValue("@A", txtA.Text)
                cmd.Parameters.AddWithValue("@D", txtD.Text)
                cmd.Parameters.AddWithValue("@E", txtE.Text)
                cmd.Parameters.AddWithValue("@Cromo", txtCromo.Text)

                cmd.Parameters.AddWithValue("@Biotina", txtBiotina.Text)
                cmd.Parameters.AddWithValue("@Virginiamicina", txtVirginamicina.Text)
                cmd.Parameters.AddWithValue("@Monensina", txtMonensina.Text)
                cmd.Parameters.AddWithValue("@Levedura", txtLevedura.Text)

                cmd.Parameters.AddWithValue("@Arginina", txtArginina.Text)
                cmd.Parameters.AddWithValue("@Histidina", txtHistid.Text)
                cmd.Parameters.AddWithValue("@Isoleucina", txtIsoleu.Text)
                cmd.Parameters.AddWithValue("@Leucina", txtLeuc.Text)

                cmd.Parameters.AddWithValue("@Lisina", txtLisina.Text)
                cmd.Parameters.AddWithValue("@Metionina", txtMetionina.Text)

                cmd.Parameters.AddWithValue("@Fenilalanina", txtFelinal.Text)
                cmd.Parameters.AddWithValue("@Treonina", txtTreon.Text)
                cmd.Parameters.AddWithValue("@Triptofano", txtTripto.Text)
                cmd.Parameters.AddWithValue("@Valina", txtValina.Text)

                cmd.Parameters.AddWithValue("@dFDNp_48h", txtdFDNP48h.Text)
                cmd.Parameters.AddWithValue("@dAmido_7h", txtdAmido7h.Text)

                cmd.Parameters.AddWithValue("@TTNDFD", txtTTNDFD.Text)

                cmd.Parameters.AddWithValue("@Pers1", txt1.Text)
                cmd.Parameters.AddWithValue("@Pers2", txt2.Text)
                cmd.Parameters.AddWithValue("@Pers3", txt3.Text)
                cmd.Parameters.AddWithValue("@Pers4", txt4.Text)
                cmd.Parameters.AddWithValue("@Pers5", txt5.Text)
                cmd.Parameters.AddWithValue("@Pers6", txt6.Text)
                cmd.Parameters.AddWithValue("@Pers7", txt7.Text)
                cmd.Parameters.AddWithValue("@Pers8", txt8.Text)
                cmd.Parameters.AddWithValue("@Pers9", txt9.Text)
                cmd.Parameters.AddWithValue("@Chk", "0")
                cmd.Parameters.AddWithValue("@ID", lblID.Text)
                cmd.ExecuteNonQuery()
                MsgBox("Alimento atualizado com sucesso!")
                LimparCampos()
            Catch ex As Exception
                MsgBox("Erro ao atualizar!" + ex.Message)
                fechar()
            End Try
            'LimparCampos()

        Else
            MsgBox("Preencha os campos nescessários!")

        End If

    End Sub

    Private Sub BuscarAlimentos()

        Dim da As New SQLiteDataAdapter
        Dim dt As New DataTable

        Try

            abrir()

            Dim sql As String = "Select * from AlimentosMS"
            da = New SQLiteDataAdapter(sql, con)
            dt = New DataTable
            da.Fill(dt)
            dtgAlimentos.DataSource = dt

            'cbxLote.ValueMember = "Lote"
            ''cbxAvaliadores.DisplayMember = "NomeAvaliador"
            'cbxLote.DataSource = (dt)

            fechar()

        Catch ex As Exception

        End Try
        ' dtgltes.DataSource = dt.DefaultView

    End Sub
    Private Sub btnSalvarAlimEdt_Click(sender As Object, e As EventArgs) Handles btnSalvarAlimEdt.Click
        EditarAlimentosMS()
        BuscarAlimentos()
    End Sub

    Private Sub btnCadastrarMSMO_Click(sender As Object, e As EventArgs) Handles btnCadastrarMSMO.Click
        If txtAlimento.Text = nomeAlimento Then
            pnlNvAlimento.Location = New Point(450, 230)
            pnlNvAlimento.Visible = True
            txtNovoAlimento.Text = txtAlimento.Text
        Else
            nomeAlimento = txtAlimento.Text
            CadastrarAlimentosMS()
        End If

        BuscarAlimentos()
    End Sub

    Private Sub btnSalvarNvAlimento_Click(sender As Object, e As EventArgs) Handles btnSalvarNvAlimento.Click
        Dim data As String
        data = Now.ToString("dd-MM-yyyy hh:mm")
        If txtAlimento.Text = txtNovoAlimento.Text Then
            nomeAlimento = txtAlimento.Text & " " & data
        Else
            nomeAlimento = txtNovoAlimento.Text
        End If
        CadastrarAlimentosMS()
    End Sub

    Private Sub btnSair_Click_1(sender As Object, e As EventArgs) Handles btnSair.Click
        LimparCampos()
        edtAlim = False
        pnlCadAlimentos.Visible = False

    End Sub

    Private Sub txtBuscaAlimentos_TextChanged(sender As Object, e As EventArgs) Handles txtBuscaAlimentos.TextChanged
        If Me.txtBuscaAlimentos.Text <> "" Then
            dtgAlimentos.Visible = True
        Else
            dtgAlimentos.Visible = False
        End If

        Try
            TryCast(dtgAlimentos.DataSource, DataTable).DefaultView.RowFilter = "Alimento LIKE '%" & txtBuscaAlimentos.Text & "%'"
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try

    End Sub

    Private Sub dtgAlimentos_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dtgAlimentos.CellClick
        With dtgAlimentos
            'cbxFuncao.Text = .CurrentRow.Cells(0).Value
            'txtAlimento.Text = .CurrentRow.Cells(1).Value
            'txtMS.Text = .CurrentRow.Cells(2).Value

            almntoFamilia = .CurrentRow.Cells(0).Value
            almnto = .CurrentRow.Cells(1).Value
            v_MS = .CurrentRow.Cells(2).Value
            v_PB = .CurrentRow.Cells(3).Value
            v_PDR = .CurrentRow.Cells(4).Value
            v_PND = .CurrentRow.Cells(5).Value
            v_FDN = .CurrentRow.Cells(6).Value
            v_eFDN = .CurrentRow.Cells(7).Value
            v_MNmaior8 = .CurrentRow.Cells(8).Value
            v_MNmaior19 = .CurrentRow.Cells(9).Value
            v_FDNF = .CurrentRow.Cells(10).Value
            v_FDA = .CurrentRow.Cells(11).Value
            v_Nel = .CurrentRow.Cells(12).Value
            v_NDT = .CurrentRow.Cells(13).Value
            v_EE = .CurrentRow.Cells(14).Value
            v_EE_Insat = .CurrentRow.Cells(15).Value
            v_Cinzas = .CurrentRow.Cells(16).Value
            v_CNF = .CurrentRow.Cells(17).Value
            v_Amido = .CurrentRow.Cells(18).Value
            v_kd_Amid = .CurrentRow.Cells(19).Value
            v_MOR = .CurrentRow.Cells(20).Value
            v_Ca = .CurrentRow.Cells(21).Value
            v_P = .CurrentRow.Cells(22).Value
            v_Mg = .CurrentRow.Cells(23).Value
            v_K = .CurrentRow.Cells(24).Value
            v_S = .CurrentRow.Cells(25).Value
            v_Na = .CurrentRow.Cells(26).Value
            v_Cl = .CurrentRow.Cells(27).Value
            v_Co = .CurrentRow.Cells(28).Value
            v_Cu = .CurrentRow.Cells(29).Value
            v_Mn = .CurrentRow.Cells(30).Value
            v_Zn = .CurrentRow.Cells(31).Value
            v_Se = .CurrentRow.Cells(32).Value
            v_I = .CurrentRow.Cells(33).Value
            v_A = .CurrentRow.Cells(34).Value
            v_D = .CurrentRow.Cells(35).Value
            v_E = .CurrentRow.Cells(36).Value
            v_Cromo = .CurrentRow.Cells(37).Value
            v_Biotina = .CurrentRow.Cells(38).Value
            v_Virginiamicina = .CurrentRow.Cells(39).Value
            v_Monensina = .CurrentRow.Cells(40).Value
            v_Levedura = .CurrentRow.Cells(41).Value
            v_Arginina = .CurrentRow.Cells(42).Value
            v_Histidina = .CurrentRow.Cells(43).Value
            v_Isoleucina = .CurrentRow.Cells(44).Value
            v_Leucina = .CurrentRow.Cells(45).Value
            v_Lisina = .CurrentRow.Cells(46).Value
            v_Metionina = .CurrentRow.Cells(47).Value
            v_Fenilalanina = .CurrentRow.Cells(48).Value
            v_Treonina = .CurrentRow.Cells(49).Value
            v_Triptofano = .CurrentRow.Cells(50).Value
            v_Valina = .CurrentRow.Cells(51).Value
            v_dFDNp48h = .CurrentRow.Cells(52).Value
            v_dAmido_7h = .CurrentRow.Cells(53).Value
            v_TTNDFD = .CurrentRow.Cells(54).Value
            v_id = .CurrentRow.Cells(65).Value

        End With
        PreencherAlimentos()
        txtBuscaAlimentos.Text = ""
    End Sub

    Private Sub PreencherEdtAlimentos()
        'passar valores de avaliadores as variáveis p transição entre frmAlimentos e frmDietas
        Me.cbxFuncao.Text = almntoFamilia
        Me.txtAlimento.Text = almnto
        Me.txtMS.Text = v_MS
        Me.txtPB.Text = v_PB
        Me.txtPDR.Text = v_PDR
        Me.txtPND.Text = v_PND
        Me.txtFDN.Text = v_FDN
        Me.txtEFDN.Text = v_eFDN
        Me.txtMNmaior8.Text = v_MNmaior8
        Me.txtMNmaior19.Text = v_MNmaior19
        Me.txtFDNF.Text = v_FDNF
        Me.txtFDA.Text = v_FDA
        Me.txtNel.Text = v_Nel
        Me.txtNDT.Text = v_NDT
        Me.txtEE.Text = v_EE
        Me.txtEE_Insat.Text = v_EE_Insat
        Me.txtCinzas.Text = v_Cinzas
        Me.txtCNF.Text = v_CNF
        Me.txtAmido.Text = v_Amido
        Me.txtkd_Amid.Text = v_kd_Amid
        Me.txtMOR.Text = v_MOR
        Me.txtCa.Text = v_Ca
        Me.txtP.Text = v_P
        Me.txtMg.Text = v_Mg
        Me.txtK.Text = v_K
        Me.txtS.Text = v_S
        Me.txtNa.Text = v_Na
        Me.txtCl.Text = v_Cl
        Me.txtCo.Text = v_Co
        Me.txtCu.Text = v_Cu
        Me.txtMn.Text = v_Mn
        Me.txtZn.Text = v_Zn
        Me.txtSe.Text = v_Se
        Me.txtI.Text = v_I
        Me.txtA.Text = v_A
        Me.txtD.Text = v_D
        Me.txtE.Text = v_E
        Me.txtCromo.Text = v_Cromo
        Me.txtBiotina.Text = v_Biotina
        Me.txtVirginamicina.Text = v_Virginiamicina
        Me.txtMonensina.Text = v_Monensina
        Me.txtLevedura.Text = v_Levedura

        Me.txtArginina.Text = v_Arginina
        Me.txtHistid.Text = v_Histidina
        Me.txtIsoleu.Text = v_Isoleucina
        Me.txtLeuc.Text = v_Leucina

        Me.txtLisina.Text = v_Lisina
        Me.txtMetionina.Text = v_Metionina

        Me.txtFelinal.Text = v_Fenilalanina
        Me.txtTreon.Text = v_Treonina
        Me.txtTripto.Text = v_Triptofano
        Me.txtValina.Text = v_Valina

        Me.txtdFDNP48h.Text = v_dFDNp48h
        Me.txtdAmido7h.Text = v_dAmido_7h


        Me.txtTTNDFD.Text = v_TTNDFD
        Me.txt1.Text = v_Pers1
        Me.txt2.Text = v_Pers2
        Me.txt3.Text = v_Pers3
        Me.txt4.Text = v_Pers4
        Me.txt5.Text = v_Pers5
        Me.txt6.Text = v_Pers6
        Me.txt7.Text = v_Pers7
        Me.txt8.Text = v_Pers8
        Me.txt9.Text = v_Pers9
        'Me.txt10.Text = v_Pers10
        'Me.txt11.Text = v_Pers11
        'Me.txt12.Text = v_Pers12
        'Me.txt13.Text = v_Pers13

        Me.lblID.Text = v_id

        txtPDR.Enabled = False
        txtEFDN.Enabled = False
        txtFDNF.Enabled = False
        txtNel.Enabled = False
        txtNDT.Enabled = False
        txtCNF.Enabled = False
        txtAlimento.Enabled = False
        nomeAlimento = txtAlimento.Text
        btnCadastrarMSMO.Visible = False
        btnSalvarAlimEdt.Visible = True

        txtPDR.BackColor = Color.WhiteSmoke
        txtEFDN.BackColor = Color.WhiteSmoke
        txtFDNF.BackColor = Color.WhiteSmoke
        txtNel.BackColor = Color.WhiteSmoke
        txtNDT.BackColor = Color.WhiteSmoke
        txtCNF.BackColor = Color.WhiteSmoke
        txtAlimento.BackColor = Color.WhiteSmoke

    End Sub

    Private Sub PreencherAlimentos()
        'passar valores de avaliadores as variáveis p transição entre frmAlimentos e frmDietas
        Me.cbxFuncao.Text = almntoFamilia
        Me.txtAlimento.Text = almnto
        Me.txtMS.Text = v_MS
        Me.txtPB.Text = v_PB
        Me.txtPDR.Text = v_PDR
        Me.txtPND.Text = v_PND
        Me.txtFDN.Text = v_FDN
        Me.txtEFDN.Text = v_eFDN
        Me.txtMNmaior8.Text = v_MNmaior8
        Me.txtMNmaior19.Text = v_MNmaior19
        Me.txtFDNF.Text = v_FDNF
        Me.txtFDA.Text = v_FDA
        Me.txtNel.Text = v_Nel
        Me.txtNDT.Text = v_NDT
        Me.txtEE.Text = v_EE
        Me.txtEE_Insat.Text = v_EE_Insat
        Me.txtCinzas.Text = v_Cinzas
        Me.txtCNF.Text = v_CNF
        Me.txtAmido.Text = v_Amido
        Me.txtkd_Amid.Text = v_kd_Amid
        Me.txtMOR.Text = v_MOR
        Me.txtCa.Text = v_Ca
        Me.txtP.Text = v_P
        Me.txtMg.Text = v_Mg
        Me.txtK.Text = v_K
        Me.txtS.Text = v_S
        Me.txtNa.Text = v_Na
        Me.txtCl.Text = v_Cl
        Me.txtCo.Text = v_Co
        Me.txtCu.Text = v_Cu
        Me.txtMn.Text = v_Mn
        Me.txtZn.Text = v_Zn
        Me.txtSe.Text = v_Se
        Me.txtI.Text = v_I
        Me.txtA.Text = v_A
        Me.txtD.Text = v_D
        Me.txtE.Text = v_E
        Me.txtCromo.Text = v_Cromo
        Me.txtBiotina.Text = v_Biotina
        Me.txtVirginamicina.Text = v_Virginiamicina
        Me.txtMonensina.Text = v_Monensina
        Me.txtLevedura.Text = v_Levedura

        Me.txtArginina.Text = v_Arginina
        Me.txtHistid.Text = v_Histidina
        Me.txtIsoleu.Text = v_Isoleucina
        Me.txtLeuc.Text = v_Leucina

        Me.txtLisina.Text = v_Lisina
        Me.txtMetionina.Text = v_Metionina

        Me.txtFelinal.Text = v_Fenilalanina
        Me.txtTreon.Text = v_Treonina
        Me.txtTripto.Text = v_Triptofano
        Me.txtValina.Text = v_Valina

        Me.txtdFDNP48h.Text = v_dFDNp48h
        Me.txtdAmido7h.Text = v_dAmido_7h


        Me.txtTTNDFD.Text = v_TTNDFD
        Me.txt1.Text = v_Pers1
        Me.txt2.Text = v_Pers2
        Me.txt3.Text = v_Pers3
        Me.txt4.Text = v_Pers4
        Me.txt5.Text = v_Pers5
        Me.txt6.Text = v_Pers6
        Me.txt7.Text = v_Pers7
        Me.txt8.Text = v_Pers8
        Me.txt9.Text = v_Pers9
        'Me.txt10.Text = v_Pers10
        'Me.txt11.Text = v_Pers11
        'Me.txt12.Text = v_Pers12
        'Me.txt13.Text = v_Pers13

        Me.lblID.Text = v_id

        txtPDR.Enabled = False
        txtEFDN.Enabled = False
        txtFDNF.Enabled = False
        txtNel.Enabled = False
        txtNDT.Enabled = False
        txtCNF.Enabled = False
        txtAlimento.Enabled = False
        nomeAlimento = txtAlimento.Text
        btnCadastrarMSMO.Visible = True
        btnSalvarAlimEdt.Visible = False

        txtPDR.BackColor = Color.WhiteSmoke
        txtEFDN.BackColor = Color.WhiteSmoke
        txtFDNF.BackColor = Color.WhiteSmoke
        txtNel.BackColor = Color.WhiteSmoke
        txtNDT.BackColor = Color.WhiteSmoke
        txtCNF.BackColor = Color.WhiteSmoke
        txtAlimento.BackColor = Color.WhiteSmoke
    End Sub
    Dim nomeAlimento As String

    Private Sub LimparCampos()
        Me.cbxFuncao.Text = ""
        Me.txtAlimento.Text = ""
        Me.txtMS.Text = ""
        Me.txtPB.Text = ""
        Me.txtPDR.Text = ""
        Me.txtPND.Text = ""
        Me.txtFDN.Text = ""
        Me.txtEFDN.Text = ""
        Me.txtMNmaior8.Text = ""
        Me.txtMNmaior19.Text = ""
        Me.txtFDNF.Text = ""
        Me.txtFDA.Text = ""
        Me.txtNel.Text = ""
        Me.txtNDT.Text = ""
        Me.txtEE.Text = ""
        Me.txtEE_Insat.Text = ""
        Me.txtCinzas.Text = ""
        Me.txtCNF.Text = ""
        Me.txtAmido.Text = ""
        Me.txtkd_Amid.Text = ""
        Me.txtMOR.Text = ""
        Me.txtCa.Text = ""
        Me.txtP.Text = ""
        Me.txtMg.Text = ""
        Me.txtK.Text = ""
        Me.txtS.Text = ""
        Me.txtNa.Text = ""
        Me.txtCl.Text = ""
        Me.txtCo.Text = ""
        Me.txtCu.Text = ""
        Me.txtMn.Text = ""
        Me.txtZn.Text = ""
        Me.txtSe.Text = ""
        Me.txtI.Text = ""
        Me.txtA.Text = ""
        Me.txtD.Text = ""
        Me.txtE.Text = ""
        Me.txtCromo.Text = ""
        Me.txtBiotina.Text = ""
        Me.txtVirginamicina.Text = ""
        Me.txtMonensina.Text = ""
        Me.txtLevedura.Text = ""

        Me.txtArginina.Text = ""
        Me.txtHistid.Text = ""
        Me.txtIsoleu.Text = ""
        Me.txtLeuc.Text = ""

        Me.txtLisina.Text = ""
        Me.txtMetionina.Text = ""

        Me.txtFelinal.Text = ""
        Me.txtTreon.Text = ""
        Me.txtTripto.Text = ""
        Me.txtValina.Text = ""

        Me.txtdFDNP48h.Text = ""
        Me.txtdAmido7h.Text = ""


        Me.txtTTNDFD.Text = ""
        Me.txt1.Text = ""
        Me.txt2.Text = ""
        Me.txt3.Text = ""
        Me.txt4.Text = ""
        Me.txt5.Text = ""
        Me.txt6.Text = ""
        Me.txt7.Text = ""
        Me.txt8.Text = ""
        Me.txt9.Text = ""
        'Me.txt10.Text = ""
        'Me.txt11.Text = ""
        'Me.txt12.Text = ""
        'Me.txt13.Text = ""

        Me.lblID.Text = ""

        txtPDR.Enabled = True
        txtEFDN.Enabled = True
        txtFDNF.Enabled = True
        txtNel.Enabled = True
        txtNDT.Enabled = True
        txtCNF.Enabled = True
        txtAlimento.Enabled = True
        nomeAlimento = ""
    End Sub
    Private Sub ConfigGridAlimentos()
        On Error Resume Next
        With Me.dtgAlimentos

            '.DefaultCellStyle.ForeColor = Color.FromArgb(90, 90, 90)
            '.DefaultCellStyle.BackColor = Color.White
            ''.ColumnHeadersDefaultCellStyle.Font = New Font("Inter", 8, FontStyle.Bold)
            '.DefaultCellStyle.Font = New Font("Inter", 8.5, FontStyle.Bold)
            .Columns(0).Visible = False
            .Columns(1).Width = 250
            '.Columns(2).Frozen = True
            '.Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft ' voltar alimentos para esquerda

            For i = 2 To 79
                .Columns(i).Visible = False
            Next

        End With
    End Sub

    'Taxa do alimento para cada tipo de alimento FALTA TESTAR
    Private Sub CalculoCamposAlimentos()

        'gpt
        Dim varPND As Double = 0
        Dim alimento = txtAlimento.Text.Trim()

        Select Case True

            Case {"Farinha de peixe", "Farinha de penas"}.Contains(alimento)
                varPND = 70

            Case {"Milho moido grosso", "Milho floculado"}.Contains(alimento)
                varPND = 60

            Case {"Milho moido fino", "Milheto", "Resíduo de cervejaria desidratado", "Resíduo de destilaria (DDG) úmido",
                  "Farinha de carne e ossos", "Sal Branco"}.Contains(alimento)
                varPND = 50

            Case {"Soja integral extrusada", "Soja integral inativada"}.Contains(alimento)
                varPND = 40

            Case {"Tifton verde colhido", "Pastagem tropical bem manejada", "Pastagem tropical mal manejada",
                  "Pastejo Azevém, bem manejado", "Pastejo Azevém, mal manejado", "Capim tifton bem manejado",
                  "Capim tifton mal manejado", "Cana-de-açúcar ensilada", "Cana-de-açúcar fresca", "Cana-de-açúcar bagaço",
                  "Feno de Centeio anual", "Feno de milheto (painço)", "Feno de soja ", "Feno de Sorgo",
                  "Feno de Sorgo Sudão", "Napier ensilado", "Napier verde bem manejado", "Napier verde mal manejado",
                  "Polpa de beterraba", "Torta de Algodão", "Silagem de milho, 32 MS", "Silagem de milho, 34 MS",
                  "Silagem de Sorgo", "Silagem de Aveia", "Silagem de Capim tropical, intermediário",
                  "Silagem de Capim tropical, madura", "Silagem de Cevada com Grãos", "Silagem de Cevada Estágio Vegetativo",
                  "Silagem de cevada média", "Silagem de milheto (painço)", "Silagem de Soja", "Snaplage", "Milho Gérmen",
                  "Casca de soja", "Sorgo grão moído", "Sorgo grão reidratado", "Farinha de mandioca",
                  "Polpa cítrica seca", "Polpa cítrica, úmida", "Subprodutos da Batata", "Polpa de beterraba seca",
                  "Polpa de maçã úmido", "Pomace de Tomate", "Grãos de cervejaria, seco", "Trigo grão moído fino",
                  "Triticale grão", "Sêbo bovino"}.Contains(alimento)
                varPND = 30

            Case {"Alfafa silagem pré-secada", "Gramínea temperada silagem pré-secada", "Aveia grão moído fino"}.Contains(alimento)
                varPND = 20

            Case alimento = "Farinha de carne  "
                varPND = 75

            Case alimento = "Farinha de sangue"
                varPND = 65

            Case {"Milho moido médio", "Farelo de soja Soy Pass", "Resíduo de destilaria (DDG) seco",
                  "Glúten de milho, 60% PB", "Farelo Bypass"}.Contains(alimento)
                varPND = 55

            Case alimento = "Sorgo floculado"
                varPND = 47

            Case {"Milho úmido ensilado, 68 MS fino", "Milho úmido ensilado, 68 MS grosso",
                  "Milho úmido ensilado, 78 MS fino", "Milho úmido ensilado, 78 MS grosso",
                  "Milho com palha e sabugo", "Soja integral tostada", "Resíduo de cervejaria úmido"}.Contains(alimento)
                varPND = 45

            Case alimento = "Farelo de algodão 38"
                varPND = 43

            Case {"Alfafa verde colhida", "Gramínea temperada verde colhido", "Pastagem temperada bem manejada",
                  "Pastagem temperada mal manejada", "Caroço de algodão", "Concentral Farelado",
                  "Concentral Peletizado", "Farelo de linhaça", "Farelo de soja  "}.Contains(alimento)
                varPND = 35

            Case alimento = "Farelo de canola"
                varPND = 28

            Case {"Cevada grão moído fino", "Cevada Úmida"}.Contains(alimento)
                varPND = 27

            Case {"Farelo de glúten de milho úmido", "Soja integral crua"}.Contains(alimento)
                varPND = 26

            Case {"Gramínea temperada feno", "Tifton feno inteiro", "Tifton feno picado", "Farelo de trigo",
                  "Farelo de arroz", "Farelo de amendoim", "Farelo de girassol com casca", "Farelo de girassol sem casca",
                  "Alfafa feno"}.Contains(alimento)
                varPND = 25

            Case alimento = "Farelo de glúten de milho seco"
                varPND = 22

            Case alimento = "Ração comercial 22%"
                varPND = 9

        End Select

        'xxxxxxxxxxxxxxxxxxx

        Dim varEFDN As Double
        Dim varalimento As String = txtAlimento.Text.Trim()
        Dim alimentosEFDN100 As New HashSet(Of String) From {
            "Alfafa feno", "Alfafa silagem pré-secada", "Alfafa verde colhida", "Gramínea temperada feno", "Gramínea temperada silagem pré-secada",
            "Gramínea temperada verde colhido", "Tifton feno inteiro", "Tifton feno picado", "Tifton verde colhido", "Pastagem temperada bem manejada",
            "Pastagem temperada mal manejada", "Pastagem tropical bem manejada", "Pastagem tropical mal manejada", "Pastejo Azevém, bem manejado",
            "Pastejo Azevém, mal manejado", "Capim tifton bem manejado", "Capim tifton mal manejado", "Cana-de-açúcar ensilada", "Cana-de-açúcar fresca",
            "Cana-de-açúcar bagaço", "Feno de Centeio anual", "Feno de milheto (painço)", "Feno de soja ", "Feno de Sorgo", "Feno de Sorgo Sudão",
            "Napier ensilado", "Napier verde bem manejado", "Napier verde mal manejado", "Polpa de beterraba", "Torta de Algodão", "SILAGENS",
            "Silagem de milho, 32 MS", "Silagem de milho, 34 MS", "Silagem de Sorgo", "Silagem de Aveia", "Silagem de Capim tropical, intermediário",
            "Silagem de Capim tropical, madura", "Silagem de Cevada com Grãos", "Silagem de Cevada Estágio Vegetativo", "Silagem de cevada média",
            "Silagem de milheto (painço)", "Silagem de Soja", "Snaplage", "Caroço de algodão", "Concentral Farelado", "Concentral Peletizado"
        }

        Dim alimentosEFDN33 As New HashSet(Of String) From {
            "Milho moido fino", "Milho moido médio", "Milho moido grosso", "Milho úmido ensilado, 68 MS fino", "Milho úmido ensilado, 68 MS grosso",
            "Milho úmido ensilado, 78 MS fino", "Milho úmido ensilado, 78 MS grosso", "Milheto", "Milho com palha e sabugo", "Milho floculado",
            "Milho Gérmen", "Casca de soja", "Sorgo floculado", "Sorgo grão moído", "Sorgo grão reidratado", "Casca de algodão", "Farelo de trigo",
            "Farelo de arroz", "Farinha de mandioca", "Polpa cítrica seca", "Polpa cítrica, úmida", "Subprodutos da Batata", "Polpa de beterraba seca",
            "Polpa de maçã úmido", "Pomace de Tomate", "Cevada grão moído fino", "Grãos de cervejaria, seco", "Cevada Úmida", "Trigo grão moído fino",
            "Triticale grão", "Glúten de milho, 60% PB", "Resíduo de cervejaria desidratado", "Resíduo de cervejaria úmido", "Resíduo de destilaria (DDG) seco",
            "Resíduo de destilaria (DDG) úmido", "Farelo Bypass", "Sal Branco", "Farelo de amendoim", "Farelo de canola", "Farelo de girassol com casca",
            "Farelo de girassol sem casca", "Farelo de glúten de milho seco", "Farelo de glúten de milho úmido", "Farelo de linhaça", "Aveia grão moído fino",
            "Sêbo bovino"
        }

        Dim alimentosEFDN50 As New HashSet(Of String) From {
            "Farelo de algodão 38"
        }

        Dim alimentosEFDN25 As New HashSet(Of String) From {
            "Farelo de soja  ", "Farelo de soja Soy Pass", "Soja integral crua", "Soja integral extrusada", "Soja integral inativada",
            "Soja integral tostada", "Água"
        }

        If alimentosEFDN100.Contains(alimento) Then
            varEFDN = 100
        ElseIf alimentosEFDN33.Contains(alimento) Then
            varEFDN = 33
        ElseIf alimentosEFDN50.Contains(alimento) Then
            varEFDN = 50
        ElseIf alimentosEFDN25.Contains(alimento) Then
            varEFDN = 25
        Else
            varEFDN = 0 ' Valor padrão caso não encontrado
        End If

        Try

            ' Conversões seguras
            Dim pb As Double = CDbl(txtPB.Text)
            Dim fdn As Double = CDbl(txtFDN.Text)
            Dim fda As Double = CDbl(txtFDA.Text)
            Dim ee As Double = CDbl(txtEE.Text)
            Dim cinzas As Double = CDbl(txtCinzas.Text)

            ' PND = PB * varPND / 100
            txtPND.Text = (pb * varPND / 100).ToString("F2")

            ' PDR = PB - PND
            txtPDR.Text = (pb - CDbl(txtPND.Text)).ToString("F2")

            ' EFDN = FDN * varEFDN / 100
            txtEFDN.Text = (fdn * varEFDN / 100).ToString("F2")

            ' FDNF = FDN se for forragem ou silagem
            If cbxFuncao.Text = "Gramíneas e Leguminosas" Or cbxFuncao.Text = "Silagens" Then
                txtFDNF.Text = fdn.ToString("F2")
            Else
                txtFDNF.Text = "0"
            End If

            ' NDT = (87.84 - (0.7 x FDA x 100)) / 100
            txtNDT.Text = ((87.84 - (0.7 * fda * 100)) / 100).ToString("F2")

            ' NEL = (0.0245 x NDT x 100) - 0.12
            Dim ndt As Double = CDbl(txtNDT.Text)
            txtNel.Text = ((0.0245 * ndt * 100) - 0.12).ToString("F2")

            ' CNF = 1 - (FDN + EE + Cinzas + PB)
            txtCNF.Text = (1 - (fdn + ee + cinzas + pb)).ToString("F2")

        Catch ex As Exception
            MessageBox.Show("Erro nos cálculos: " & ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub txtPB_Leave(sender As Object, e As EventArgs) Handles txtPB.Leave
        CalculoCamposAlimentos()
    End Sub

    Private Sub txtFDN_Leave(sender As Object, e As EventArgs) Handles txtFDN.Leave
        CalculoCamposAlimentos()
    End Sub

    Private Sub txtFDA_Leave(sender As Object, e As EventArgs) Handles txtFDA.Leave
        CalculoCamposAlimentos()
    End Sub

    Private Sub txtEE_Leave(sender As Object, e As EventArgs) Handles txtEE.Leave
        CalculoCamposAlimentos()
    End Sub

    Private Sub txtCinzas_Leave(sender As Object, e As EventArgs) Handles txtCinzas.Leave
        CalculoCamposAlimentos()
    End Sub

    Private Sub txtAmido_Leave(sender As Object, e As EventArgs) Handles txtAmido.Leave
        CalculoCamposAlimentos()
    End Sub

    'Transformar valores de MS em MN
    Private Sub rdbMO_Click(sender As Object, e As EventArgs) Handles rdbMO.Click

        v_PB = Me.txtPB.Text
        v_PDR = Me.txtPDR.Text
        v_PND = Me.txtPND.Text
        v_FDN = Me.txtFDN.Text
        v_eFDN = Me.txtEFDN.Text
        v_MNmaior8 = Me.txtMNmaior8.Text
        v_MNmaior19 = Me.txtMNmaior19.Text
        v_FDNF = Me.txtFDNF.Text
        v_FDA = Me.txtFDA.Text
        v_Nel = Me.txtNel.Text
        v_NDT = Me.txtNDT.Text
        v_EE = Me.txtEE.Text
        v_EE_Insat = Me.txtEE_Insat.Text
        v_Cinzas = Me.txtCinzas.Text
        v_CNF = Me.txtCNF.Text
        v_Amido = Me.txtAmido.Text
        v_kd_Amid = Me.txtkd_Amid.Text
        v_MOR = Me.txtMOR.Text
        v_Ca = Me.txtCa.Text
        v_P = Me.txtP.Text
        v_Mg = Me.txtMg.Text
        v_K = Me.txtK.Text
        v_S = Me.txtS.Text
        v_Na = Me.txtNa.Text
        v_Cl = Me.txtCl.Text
        v_Co = Me.txtCo.Text
        v_Cu = Me.txtCu.Text
        v_Mn = Me.txtMn.Text
        v_Zn = Me.txtZn.Text
        v_Se = Me.txtSe.Text
        v_I = Me.txtI.Text
        v_A = Me.txtA.Text
        v_D = Me.txtD.Text
        v_E = Me.txtE.Text
        v_Cromo = Me.txtCromo.Text
        v_Biotina = Me.txtBiotina.Text
        v_Virginiamicina = Me.txtVirginamicina.Text
        v_Monensina = Me.txtMonensina.Text
        v_Levedura = Me.txtLevedura.Text

        v_Arginina = Me.txtArginina.Text
        v_Histidina = Me.txtHistid.Text
        v_Isoleucina = Me.txtIsoleu.Text
        v_Leucina = Me.txtLeuc.Text

        v_Lisina = Me.txtLisina.Text
        v_Metionina = Me.txtMetionina.Text

        v_Fenilalanina = Me.txtFelinal.Text
        v_Treonina = Me.txtTreon.Text
        v_Triptofano = Me.txtTripto.Text
        v_Valina = Me.txtValina.Text

        v_dFDNp48h = Me.txtdFDNP48h.Text
        v_dAmido_7h = Me.txtdAmido7h.Text


        v_TTNDFD = Me.txtTTNDFD.Text
        v_Pers1 = Me.txt1.Text
        v_Pers2 = Me.txt2.Text
        v_Pers3 = Me.txt3.Text
        v_Pers4 = Me.txt4.Text
        v_Pers5 = Me.txt5.Text
        v_Pers6 = Me.txt6.Text
        v_Pers7 = Me.txt7.Text
        v_Pers8 = Me.txt8.Text
        v_Pers9 = Me.txt9.Text
        'v_Pers10 = Me.txt10.Text
        'v_Pers11 = Me.txt11.Text
        'v_Pers12 = Me.txt12.Text
        'v_Pers13 = Me.txt13.Text

        Me.txtPB.Text = v_PB * txtMS.Text / 100
        Me.txtPDR.Text = v_PDR * txtMS.Text / 100
        Me.txtPND.Text = v_PND * txtMS.Text / 100
        Me.txtFDN.Text = v_FDN * txtMS.Text / 100
        Me.txtEFDN.Text = v_eFDN * txtMS.Text / 100
        'Me.txtMNmaior8.Text = v_MNmaior8 * txtMS.Text                                 /100
        'Me.txtMNmaior19.Text = v_MNmaior19 * txtMS.Text                               /100
        Me.txtFDNF.Text = v_FDNF * txtMS.Text / 100
        Me.txtFDA.Text = v_FDA * txtMS.Text / 100
        Me.txtNel.Text = v_Nel * txtMS.Text / 100
        Me.txtNDT.Text = v_NDT * txtMS.Text / 100
        Me.txtEE.Text = v_EE * txtMS.Text / 100
        Me.txtEE_Insat.Text = v_EE_Insat * txtMS.Text / 100
        Me.txtCinzas.Text = v_Cinzas * txtMS.Text / 100
        Me.txtCNF.Text = v_CNF * txtMS.Text / 100
        Me.txtAmido.Text = v_Amido * txtMS.Text / 100
        Me.txtkd_Amid.Text = v_kd_Amid * txtMS.Text / 100
        Me.txtMOR.Text = v_MOR * txtMS.Text / 100
        Me.txtCa.Text = v_Ca * txtMS.Text / 100
        Me.txtP.Text = v_P * txtMS.Text / 100
        Me.txtMg.Text = v_Mg * txtMS.Text / 100
        Me.txtK.Text = v_K * txtMS.Text / 100
        Me.txtS.Text = v_S * txtMS.Text / 100
        Me.txtNa.Text = v_Na * txtMS.Text / 100
        Me.txtCl.Text = v_Cl * txtMS.Text / 100
        Me.txtCo.Text = v_Co * txtMS.Text / 100
        Me.txtCu.Text = v_Cu * txtMS.Text / 100
        Me.txtMn.Text = v_Mn * txtMS.Text / 100
        Me.txtZn.Text = v_Zn * txtMS.Text / 100
        Me.txtSe.Text = v_Se * txtMS.Text / 100
        Me.txtI.Text = v_I * txtMS.Text / 100
        Me.txtA.Text = v_A * txtMS.Text / 100
        Me.txtD.Text = v_D * txtMS.Text / 100
        Me.txtE.Text = v_E * txtMS.Text / 100
        Me.txtCromo.Text = v_Cromo * txtMS.Text / 100
        Me.txtBiotina.Text = v_Biotina * txtMS.Text / 100
        Me.txtVirginamicina.Text = v_Virginiamicina * txtMS.Text / 100
        Me.txtMonensina.Text = v_Monensina * txtMS.Text / 100
        Me.txtLevedura.Text = v_Levedura * txtMS.Text / 100
        Me.txtArginina.Text = v_Arginina * txtMS.Text / 100
        Me.txtHistid.Text = v_Histidina * txtMS.Text / 100
        Me.txtIsoleu.Text = v_Isoleucina * txtMS.Text / 100
        Me.txtLeuc.Text = v_Leucina * txtMS.Text / 100
        Me.txtLisina.Text = v_Lisina * txtMS.Text / 100
        Me.txtMetionina.Text = v_Metionina * txtMS.Text / 100
        Me.txtFelinal.Text = v_Fenilalanina * txtMS.Text / 100
        Me.txtTreon.Text = v_Treonina * txtMS.Text / 100
        Me.txtTripto.Text = v_Triptofano * txtMS.Text / 100
        Me.txtValina.Text = v_Valina * txtMS.Text / 100
        Me.txtdFDNP48h.Text = v_dFDNp48h * txtMS.Text / 100
        Me.txtdAmido7h.Text = v_dAmido_7h * txtMS.Text / 100
        Me.txtTTNDFD.Text = v_TTNDFD * txtMS.Text / 100
        Me.txt1.Text = v_Pers1 * txtMS.Text / 100
        Me.txt2.Text = v_Pers2 * txtMS.Text / 100
        Me.txt3.Text = v_Pers3 * txtMS.Text / 100
        Me.txt4.Text = v_Pers4 * txtMS.Text / 100
        Me.txt5.Text = v_Pers5 * txtMS.Text / 100
        Me.txt6.Text = v_Pers6 * txtMS.Text / 100
        Me.txt7.Text = v_Pers7 * txtMS.Text / 100
        Me.txt8.Text = v_Pers8 * txtMS.Text / 100
        Me.txt9.Text = v_Pers9 * txtMS.Text / 100
        'Me.txt10.Text = v_Pers10 * txtMS.Text / 100
        'Me.txt11.Text = v_Pers11 * txtMS.Text / 100
        'Me.txt12.Text = v_Pers12 * txtMS.Text / 100
        'Me.txt13.Text = v_Pers13 * txtMS.Text / 100

    End Sub
    'Valores de MN em MS
    Private Sub rdbMS_Click(sender As Object, e As EventArgs) Handles rdbMS.Click

        Me.txtPB.Text = v_PB
        Me.txtPDR.Text = v_PDR
        Me.txtPND.Text = v_PND
        Me.txtFDN.Text = v_FDN
        Me.txtEFDN.Text = v_eFDN
        'Me.txtMNmaior8.Text = v_MNmaior8
        'Me.txtMNmaior19.Text = v_MNmaior19
        Me.txtFDNF.Text = v_FDNF
        Me.txtFDA.Text = v_FDA
        Me.txtNel.Text = v_Nel
        Me.txtNDT.Text = v_NDT
        Me.txtEE.Text = v_EE
        Me.txtEE_Insat.Text = v_EE_Insat
        Me.txtCinzas.Text = v_Cinzas
        Me.txtCNF.Text = v_CNF
        Me.txtAmido.Text = v_Amido
        Me.txtkd_Amid.Text = v_kd_Amid
        Me.txtMOR.Text = v_MOR
        Me.txtCa.Text = v_Ca
        Me.txtP.Text = v_P
        Me.txtMg.Text = v_Mg
        Me.txtK.Text = v_K
        Me.txtS.Text = v_S
        Me.txtNa.Text = v_Na
        Me.txtCl.Text = v_Cl
        Me.txtCo.Text = v_Co
        Me.txtCu.Text = v_Cu
        Me.txtMn.Text = v_Mn
        Me.txtZn.Text = v_Zn
        Me.txtSe.Text = v_Se
        Me.txtI.Text = v_I
        Me.txtA.Text = v_A
        Me.txtD.Text = v_D
        Me.txtE.Text = v_E
        Me.txtCromo.Text = v_Cromo
        Me.txtBiotina.Text = v_Biotina
        Me.txtVirginamicina.Text = v_Virginiamicina
        Me.txtMonensina.Text = v_Monensina
        Me.txtLevedura.Text = v_Levedura

        Me.txtArginina.Text = v_Arginina
        Me.txtHistid.Text = v_Histidina
        Me.txtIsoleu.Text = v_Isoleucina
        Me.txtLeuc.Text = v_Leucina

        Me.txtLisina.Text = v_Lisina
        Me.txtMetionina.Text = v_Metionina

        Me.txtFelinal.Text = v_Fenilalanina
        Me.txtTreon.Text = v_Treonina
        Me.txtTripto.Text = v_Triptofano
        Me.txtValina.Text = v_Valina

        Me.txtdFDNP48h.Text = v_dFDNp48h
        Me.txtdAmido7h.Text = v_dAmido_7h


        Me.txtTTNDFD.Text = v_TTNDFD
        Me.txt1.Text = v_Pers1
        Me.txt2.Text = v_Pers2
        Me.txt3.Text = v_Pers3
        Me.txt4.Text = v_Pers4
        Me.txt5.Text = v_Pers5
        Me.txt6.Text = v_Pers6
        Me.txt7.Text = v_Pers7
        Me.txt8.Text = v_Pers8
        Me.txt9.Text = v_Pers9
        'Me.txt10.Text = v_Pers10
        'Me.txt11.Text = v_Pers11
        'Me.txt12.Text = v_Pers12
        'Me.txt13.Text = v_Pers13
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        pnlNvAlimento.Visible = False

    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        pnlNvAlimento.Visible = False
    End Sub

    Private Sub pnlCard05_Paint(sender As Object, e As PaintEventArgs) Handles pnlCard05.Paint

    End Sub
End Class

