Imports System.Windows.Forms.DataVisualization.Charting
Imports System.Data.SQLite
Imports System.Drawing.Drawing2D
Imports System.Runtime.InteropServices


Public Class frmDieta
    Protected Overrides Sub WndProc(ByRef m As Message)
        Const WM_SYSCOMMAND As Integer = &H112
        Const SC_MOVE As Integer = &HF010

        ' Impede movimentação pela barra de título
        If m.Msg = WM_SYSCOMMAND AndAlso (m.WParam.ToInt32() And &HFFF0) = SC_MOVE Then
            Return ' Ignora a tentativa de mover o form
        End If

        MyBase.WndProc(m)
    End Sub
    Dim dtTemp As New DataTable
    Dim dt1 As New DataTable
    Dim dt2 As New DataTable
    Dim dt3 As New DataTable
    Dim dt4 As New DataTable

    Dim nomePremix As String
    Dim desmPrmx As Boolean

    Private borderRadius As Integer = 6
    Private borderSize As Integer = 3
    Private borderColor As Color = Color.FromArgb(237, 242, 207)

    Private Sub frmDieta_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        '' Tamanho e posição da tela
        Me.Width = 1440
        Me.Height = 900
        Me.Location = New Point(0, 78)

        'Tamanho e posição do pnlFaz
        pnlBarraFaz.Size = New Size(1438, 62)
        tcDieta.Location = New Point(2, 0)

        'Aba a ser aberta
        Me.tabDietas.Parent = Me.tcDieta
        Me.tcDieta.SelectedTab = tabDietas

        ''Preencher lista de dietas
        BuscarDietas()
        CarregarListaDieta()
        ''lblNFazDt.Text = lblNFaz.Text & "           "
        ''idDieta = ""

        ''Nome da faz. na barra
        lblIdCliente.Text = idFaz
        lblNFaz.Text = nomeFaz

        'LocLotes()
        ' Cursor.Current = Cursors.Default
        'variaveis para a redefinir o tamanho da Grid AlimentosDieta
        varAlim = 202 '303
        varD1 = True
        varD2 = False
        varAval = 225 '266
        varFinan = 336

    End Sub

    'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX    CARREGAR, ABRIR, EXCLUIR DIETA ETC      XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

    Function EImpar(ByVal iNum As Long) As Boolean
        'Verifica se o número é impar
        'Se for impar a função retorna True.
        'Se for par a função retorna False.
        EImpar = (iNum Mod 2)

    End Function

    Private Sub NovaDieta()
        'Lotes da propriedade ligada a dieta
        LocLotes()

        cbxAvaliador.Text = "Todos"
        ConfigGridAvaliadores()

        '======== esse não fica aqui ================
        'dt1 = TabelaRelatFinanceiro()
        TabelaRelatFinanceiro()
        'varFinan = 336
        ConfigGridFinanceiro()
        'dtgRelatFinan.DataSource = dt1

        RelatNutri()
        TabelaVolumoso()
        TabelaVol()
        '============================================

        If idDieta = "" Then 'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx         Para criar uma nova dieta          xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
            cbxLote.Text = "Selecione o lote"
            varms = False
            btnMN.BackgroundImage = My.Resources.mn_on
            btnMS.BackgroundImage = My.Resources.ms_of

        End If

        'ConfigGridAlimentosDieta() '======== esse não fica aqui ================

        'iniciar sem a dieta 2
        EsconderD2()
        'Paramentro da dos avaliadores visiveis ou não ===== opção selecionada na tela de configurações de avaliadores
        If My.Settings.corAvalOnOf = True Then
            Label77.Visible = True
            Panel26.Visible = True
            Label78.Visible = True
            Panel25.Visible = True
            Label79.Visible = True
            lblIdeal.Visible = True
        Else
            Label77.Visible = False
            Panel26.Visible = False
            Label78.Visible = False
            Panel25.Visible = False
            Label79.Visible = False
            lblIdeal.Visible = False
        End If

        'btnAbrirD2.BackgroundImage = My.Resources.dieta2_off '======== esse não fica aqui ================

        PreencherCbxItens()
        'Cursor.Current = Cursors.Default
        TabelaAlimentos()
        BuscarAlimentosMSMO() '======== esse não fica aqui ================

        'btnPreMix.Enabled = False '======== esse não fica aqui ================
        'btnPreMix.BackgroundImage = My.Resources.premistura_of
    End Sub

    Private Sub AbrirDieta()

        LocLotes()


        TabelaAlimentos()
        BuscarAlimentosMSMO()
     
        cbxAvaliador.Text = "Todos"
        ConfigGridAvaliadores()


        TabelaRelatFinanceiro()
        ConfigGridFinanceiro()
        RelatNutri()

        TabelaVolumoso()
        TabelaVol()
        
        If idDieta <> "" Then
            If varms = False Then
                btnMN.BackgroundImage = My.Resources.mn_on
                btnMS.BackgroundImage = My.Resources.ms_of
            Else
                btnMN.BackgroundImage = My.Resources.mn_of
                btnMS.BackgroundImage = My.Resources.ms_on
            End If

            CarregarDieta()
            PreencherGridTemp() ' junto com grid vol
            dtgAlimentosDieta.DataSource = dtgTemp.DataSource
            dtgAlimentosPremix.DataSource = dtgTemp.DataSource
            txtNomeDieta.Text = dtgAlimentoNome.Rows(1).Cells(0).Value.ToString
            PainelDieta()

            cbxLote.Text = ""
            Dim lote() As String = dtgAlimentoNome.Rows(1).Cells(74).Value.Split("|") ' Separar para evitar bug ao editar dieta
            cbxLote.SelectedText = lote(0)

            'nomeDieta = dtgAlimentoNome.Rows(0).Cells(0).Value.Split("|")
            CalcularValorDieta01()
            CalcularDieta01()
            CalcularFinan01()

            'Variavel para controle dos botões MS e MN
            Dim dte As String
            Dim tpDieta() As String
            dte = dtgAlimentoNome.Rows(0).Cells(76).Value
            tpDieta = dte.Split("|")
            If tpDieta(1) = "True" Then
                MSOn()
            End If

            For i As Integer = 0 To dtgAlimentosDieta.RowCount() - 1
                'Verificar a existencia de pré-mistura na dieta ao abrir
                If dtgAlimentosDieta.Rows(i).Cells(2).Value = "Pré-Mistura" Then
                    'dtgAlimentosDieta.Rows(i).Cells(1).Value = My.Resources.edit5
                    nomePremix = dtgAlimentosDieta.Rows(i).Cells(3).Value
                    Dim npre() As String
                    npre = nomePremix.Split("|")
                    dtgAlimentosDieta.Rows(i).Cells(3).Value = npre(0)
                    'alimeentos existente na pré istura ficam com a cor ver em qtdade
                    If dtgAlimentosPremix.Rows(i).Cells(69).Value > 0 Then
                        dtgAlimentosDieta.Rows(i).Cells(67).Style.ForeColor = Color.Green
                    Else
                        dtgAlimentosDieta.Rows(i).Cells(67).Style.ForeColor = Color.FromArgb(90, 90, 90)
                    End If
                    '    btnPreMix.Enabled = False
                    '    btnPreMix.BackgroundImage = My.Resources.premistura_of
                    'Else
                    '    btnPreMix.Enabled = True
                    '    btnPreMix.BackgroundImage = My.Resources.pre_mistura_on
                End If

            Next
        End If
        'configurar o datagrid dtgalimentosdieta
        ConfigGridAlimentosDieta()
        'iniciar sem a dieta 2
        EsconderD2()
        'Paramentro da dos avaliadores visiveis ou não ===== opção selecionada na tela de configurações de avaliadores
        If My.Settings.corAvalOnOf = True Then
            Label77.Visible = True
            Panel26.Visible = True
            Label78.Visible = True
            Panel25.Visible = True
            Label79.Visible = True
            lblIdeal.Visible = True
        Else
            Label77.Visible = False
            Panel26.Visible = False
            Label78.Visible = False
            Panel25.Visible = False
            Label79.Visible = False
            lblIdeal.Visible = False
        End If

        'btnAbrirD2.BackgroundImage = My.Resources.dieta2_off

        PreencherCbxItens()
        'Cursor.Current = Cursors.Default
        'desmPrmx = varms

    End Sub

    Private Sub btnDietaSair_Click(sender As Object, e As EventArgs) Handles btnDietaSair.Click
        'ZERAR TABE ALIMENTOS 66

        'Se qualquer painel estiver visível, desativa a função
        If pnlAlimDieta.Visible OrElse pnlAlimentos.Visible OrElse pnlAnaliseVolum.Visible OrElse
           pnlConfExcluirDieta.Visible OrElse pnlConfSalvarDt.Visible OrElse pnlGraficos.Visible OrElse
           pnlPreMix.Visible OrElse pnlVisLote.Visible Then
            Exit Sub

        Else
            If tcDieta.SelectedTab Is tabNvDieta Then
                ' Limpa os DataGridViews
                Dim gridsToClear As DataGridView() = {dtgAlimDt, dtgAlimentosDt, dtgAlimentoNome, dtgAlimentosDieta, dtgTemp, dtgAlimentosPremix} 'dtgVolumoso, dtgVol, 

                For Each dgv As DataGridView In gridsToClear
                    dgv.DataSource = Nothing
                    dgv.Rows.Clear()
                Next

                dtTemp.Clear()
                dt1.Clear()
                'dt2.Clear()
                dt3.Clear()
                dt4.Clear()
                cbxItens.Items.Clear()
                BuscarDietas()
                CarregarListaDieta()

                'Reexibe a aba e seleciona
                Me.tabDietas.Parent = Me.tcDieta
                Me.tcDieta.SelectedTab = tabDietas

                varms = False
            ElseIf tcDieta.SelectedTab Is tabDietas Then

                Me.Close()
            End If
            idDieta = ""
        End If

    End Sub

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

    End Sub
    Private Sub ExcluirDieta()

        Dim cmd As SQLiteCommand
        Dim sqlDelete As String = "Delete from Dieta where Data=@Data"
        Try
            abrir()
            cmd = New SQLiteCommand(sqlDelete, con)
            cmd.Parameters.AddWithValue("@Data", idDieta)
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox("Erro ao exluir Dieta!" + ex.Message)
            fechar()
        End Try
        CarregarListaDieta()
        idDieta = ""
    End Sub
    'Exibe os cards tipo lista das dietas atualizadas
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

    'Abrir dieta selecionanda
    Private Sub btnEdtDieta_Click(sender As Object, e As EventArgs) _
    Handles btnEdtDieta01.Click, btnEdtDieta02.Click, btnEdtDieta03.Click, btnEdtDieta04.Click, btnEdtDieta05.Click
        Me.tabNvDieta.Parent = Me.tcDieta
        Me.tcDieta.SelectedTab = tabNvDieta

        ' Identifica qual botão foi clicado
        Dim btn As Button = CType(sender, Button)
        Dim numero As String = btn.Name.Substring(btn.Name.Length - 2) ' Pega "01", "02", etc.

        ' Localiza o controle lblIdLDietaXX
        Dim lblId As Label = CType(Me.Controls.Find("lblIdLDieta" & numero, True).FirstOrDefault(), Label)

        If lblId IsNot Nothing Then
            idDieta = lblId.Text
        End If

        AbrirDieta()

    End Sub

    Private Sub btnExcDieta_Click(sender As Object, e As EventArgs) _
    Handles btnExcDieta01.Click, btnExcDieta02.Click, btnExcDieta03.Click, btnExcDieta04.Click, btnExcDieta05.Click

        ' Identifica qual botão foi clicado
        Dim btn As Button = CType(sender, Button)
        Dim numero As String = btn.Name.Substring(btn.Name.Length - 2) ' Pega "01", "02", etc.

        ' Localiza o Label correspondente (lblIdLDietaXX)
        Dim lblId As Label = CType(Me.Controls.Find("lblIdLDieta" & numero, True).FirstOrDefault(), Label)

        If lblId IsNot Nothing Then
            idDieta = lblId.Text
        End If

        ' Posiciona e exibe o painel de confirmação
        pnlConfExcluirDieta.Location = New Point(535, 105)
        pnlConfExcluirDieta.Visible = True
        pnlConfExcluirDieta.BringToFront()

    End Sub
    'Para criar nova dieta
    Private Sub btnNovaDieta_Click(sender As Object, e As EventArgs) Handles btnNovaDieta.Click
        Me.tabNvDieta.Parent = Me.tcDieta
        Me.tcDieta.SelectedTab = tabNvDieta
        NovaDieta()
    End Sub

    Private Sub btnConfExcluirDieta_Click(sender As Object, e As EventArgs) Handles btnConfExcluirDieta.Click
        ExcluirDieta()
        pnlConfExcluirDieta.Visible = False
        CarregarListaDieta()

        'btnStatus.Text = "Excluindo..."
        'btnStatus.Refresh()
        ' Threading.Thread.Sleep(1000)
        'btnStatus.Text = ""

    End Sub

    Private Sub btnCancExcluirDieta_Click(sender As Object, e As EventArgs) Handles btnCancExcluirDieta.Click
        pnlConfExcluirDieta.Visible = False
    End Sub

    Private Sub btnFecharExcluirDieta_Click_1(sender As Object, e As EventArgs) Handles btnFecharExcluirDieta.Click
        pnlConfExcluirDieta.Visible = False
    End Sub


    'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX             SALVAR DIETA         XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

    Private Sub SalvarDieta1()

        Dim sql As String
        Dim cmd As SQLiteCommand
        'sql = "Insert into AlimentosMO (AlimentoFamilia,Alimento,MS,PB,PDR,PND,FDN,eFDN,eFDN2,MNmaior8,MNmaior19,FDNF,FDA,Nel,NDT,EE,EE_Insat,Cinzas,CNF,Amido,kd_Amid,Ca,P,Mg,K,S,Na,Cl,Co,Cu,Mn,Zn,Se,I,A,D,E,Cromo,Biotina,Virginiamicina,Monensina,Levedura,Lisina,Metionina,dFDNp_48h,dAmido_7h,Pers1,Pers2,Pers3,Pers4,Pers5,Pers6,Pers7,Pers8,Pers9,Pers10,Pers11,Pers12,Pers13) values (@AlimentoFamilia,@Alimento,@MS,@PB,@PDR,@PND,@FDN,@eFDN,@eFDN2,@MNmaior8,@MNmaior19,@FDNF,@FDA,@Nel,@NDT,@EE,@EE_Insat,@Cinzas,@CNF,@Amido,@kd_Amid,@Ca,@P,@Mg,@K,@S,@Na,@Cl,@Co,@Cu,@Mn,@Zn,@Se,@I,@A,@D,@E,@Cromo,@Biotina,@Virginiamicina,@Monensina,@Levedura,@Lisina,@Metionina,@dFDNp_48h,@dAmido_7h,@Pers1,@Pers2,@Pers3,@Pers4,@Pers5,@Pers6,@Pers7,@Pers8,@Pers9,@Pers10,@Pers11,@Pers12,@Pers13)"
        Dim data As String
        data = Now.ToString("dd/MM/yyyy HH:mm:ss")

        sql = "Insert into Dieta (Nome,AlimentoFamilia,Alimento,MS,PB,PDR,PNDR,FDN,eFDN,MNmaior8,MNmaior19,FDNF,FDA,Nel,NDT,EE,EE_Insat,Cinzas,CNF,Amido,kd_Amido,Mor,Ca,P,Mg,K,S,Na,Cl,Co,Cu,Mn,Zn,Se,I,A,D,E,Cromo,Biotina,Virginiamicina,Monensina,Levedura,Arginina,Histidina,Isoleucina,Leucina,Lisina,Metionina,Fenilalanina,Treonina,Triptofano,Valina,dFDNp_48h,dAmido_7h,TTNDFD,Pers1,Pers2,Custo,QtdD1,QtdD2,Premix,PctPremix,QtdVagao,QtdPremix,Propriedade,IdPropriedade,Lote,QtdAnimais,Data) values (@Nome,@AlimentoFamilia,@Alimento,@MS,@PB,@PDR,@PNDR,@FDN,@eFDN,@MNmaior8,@MNmaior19,@FDNF,@FDA,@Nel,@NDT,@EE,@EE_Insat,@Cinzas,@CNF,@Amido,@kd_Amido,@Mor,@Ca,@P,@Mg,@K,@S,@Na,@Cl,@Co,@Cu,@Mn,@Zn,@Se,@I,@A,@D,@E,@Cromo,@Biotina,@Virginiamicina,@Monensina,@Levedura,@Arginina,@Histidina,@Isoleucina,@Leucina,@Lisina,@Metionina,@Fenilalanina,@Treonina,@Triptofano,@Valina,@dFDNp_48h,@dAmido_7h,@TTNDFD,@Pers1,@Pers2,@Custo,@QtdD1,@QtdD2,@Premix,@PctPremix,@QtdVagao,@QtdPremix,@Propriedade,@IdPropriedade,@Lote,@QtdAnimais,@Data)"

        For Each row As DataGridViewRow In dtgAlimentosDieta.Rows
            If cbxLote.Text <> "Escolha" Then

                Try

                    abrir()

                    cmd = New SQLiteCommand(sql, con)
                    'cmd.Parameters.AddWithValue("@Nome", nomeDieta)
                    cmd.Parameters.AddWithValue("@Nome", "Dieta") ' & " - " & data & "|" & varms)
                    cmd.Parameters.AddWithValue("@AlimentoFamilia", row.Cells(2).Value.ToString)
                    If row.Cells(2).Value = "Pré-Mistura" Then
                        cmd.Parameters.AddWithValue("@Alimento", nomePremix)
                    Else
                        cmd.Parameters.AddWithValue("@Alimento", row.Cells(3).Value.ToString)
                    End If
                    cmd.Parameters.AddWithValue("@MS", row.Cells(4).Value.ToString)
                    cmd.Parameters.AddWithValue("@PB", row.Cells(5).Value.ToString)
                    cmd.Parameters.AddWithValue("@PDR", row.Cells(6).Value.ToString)
                    cmd.Parameters.AddWithValue("@PNDR", row.Cells(7).Value.ToString)
                    cmd.Parameters.AddWithValue("@FDN", row.Cells(8).Value.ToString)
                    cmd.Parameters.AddWithValue("@eFDN", row.Cells(9).Value.ToString)

                    cmd.Parameters.AddWithValue("@MNmaior8", row.Cells(10).Value.ToString)
                    cmd.Parameters.AddWithValue("@MNmaior19", row.Cells(11).Value.ToString)
                    cmd.Parameters.AddWithValue("@FDNF", row.Cells(12).Value.ToString)
                    cmd.Parameters.AddWithValue("@FDA", row.Cells(13).Value.ToString)
                    cmd.Parameters.AddWithValue("@Nel", row.Cells(14).Value.ToString)
                    cmd.Parameters.AddWithValue("@NDT", row.Cells(15).Value.ToString)
                    cmd.Parameters.AddWithValue("@EE", row.Cells(16).Value.ToString)
                    cmd.Parameters.AddWithValue("@EE_Insat", row.Cells(17).Value.ToString)
                    cmd.Parameters.AddWithValue("@Cinzas", row.Cells(18).Value.ToString)
                    cmd.Parameters.AddWithValue("@CNF", row.Cells(19).Value.ToString)
                    cmd.Parameters.AddWithValue("@Amido", row.Cells(20).Value.ToString)
                    cmd.Parameters.AddWithValue("@kd_Amido", row.Cells(21).Value.ToString)
                    cmd.Parameters.AddWithValue("@Mor", row.Cells(22).Value.ToString)

                    cmd.Parameters.AddWithValue("@Ca", row.Cells(23).Value.ToString)
                    cmd.Parameters.AddWithValue("@P", row.Cells(24).Value.ToString)
                    cmd.Parameters.AddWithValue("@Mg", row.Cells(25).Value.ToString)
                    cmd.Parameters.AddWithValue("@K", row.Cells(26).Value.ToString)
                    cmd.Parameters.AddWithValue("@S", row.Cells(27).Value.ToString)
                    cmd.Parameters.AddWithValue("@Na", row.Cells(28).Value.ToString)
                    cmd.Parameters.AddWithValue("@Cl", row.Cells(29).Value.ToString)
                    cmd.Parameters.AddWithValue("@Co", row.Cells(30).Value.ToString)
                    cmd.Parameters.AddWithValue("@Cu", row.Cells(31).Value.ToString)
                    cmd.Parameters.AddWithValue("@Mn", row.Cells(32).Value.ToString)
                    cmd.Parameters.AddWithValue("@Zn", row.Cells(33).Value.ToString)
                    cmd.Parameters.AddWithValue("@Se", row.Cells(34).Value.ToString)
                    cmd.Parameters.AddWithValue("@I", row.Cells(35).Value.ToString)
                    cmd.Parameters.AddWithValue("@A", row.Cells(36).Value.ToString)
                    cmd.Parameters.AddWithValue("@D", row.Cells(37).Value.ToString)
                    cmd.Parameters.AddWithValue("@E", row.Cells(38).Value.ToString)
                    cmd.Parameters.AddWithValue("@Cromo", row.Cells(39).Value.ToString)

                    cmd.Parameters.AddWithValue("@Biotina", row.Cells(40).Value.ToString)
                    cmd.Parameters.AddWithValue("@Virginiamicina", row.Cells(41).Value.ToString)
                    cmd.Parameters.AddWithValue("@Monensina", row.Cells(42).Value.ToString)
                    cmd.Parameters.AddWithValue("@Levedura", row.Cells(43).Value.ToString)

                    cmd.Parameters.AddWithValue("@Arginina", row.Cells(44).Value.ToString)
                    cmd.Parameters.AddWithValue("@Histidina", row.Cells(45).Value.ToString)
                    cmd.Parameters.AddWithValue("@Isoleucina", row.Cells(46).Value.ToString)
                    cmd.Parameters.AddWithValue("@Leucina", row.Cells(47).Value.ToString)

                    cmd.Parameters.AddWithValue("@Lisina", row.Cells(48).Value.ToString)
                    cmd.Parameters.AddWithValue("@Metionina", row.Cells(49).Value.ToString)

                    cmd.Parameters.AddWithValue("@Fenilalanina", row.Cells(50).Value.ToString)
                    cmd.Parameters.AddWithValue("@Treonina", row.Cells(51).Value.ToString)
                    cmd.Parameters.AddWithValue("@Triptofano", row.Cells(52).Value.ToString)
                    cmd.Parameters.AddWithValue("@Valina", row.Cells(53).Value.ToString)

                    cmd.Parameters.AddWithValue("@dFDNp_48h", row.Cells(54).Value.ToString)
                    cmd.Parameters.AddWithValue("@dAmido_7h", row.Cells(55).Value.ToString)

                    cmd.Parameters.AddWithValue("@TTNDFD", row.Cells(56).Value.ToString)
                    cmd.Parameters.AddWithValue("@Pers1", row.Cells(57).Value.ToString)
                    cmd.Parameters.AddWithValue("@Pers2", row.Cells(58).Value.ToString)

                    cmd.Parameters.AddWithValue("@Pers3", row.Cells(59).Value.ToString)
                    cmd.Parameters.AddWithValue("@Pers4", row.Cells(60).Value.ToString)
                    cmd.Parameters.AddWithValue("@Pers5", row.Cells(61).Value.ToString)
                    cmd.Parameters.AddWithValue("@Pers6", row.Cells(62).Value.ToString)
                    cmd.Parameters.AddWithValue("@Pers7", row.Cells(63).Value.ToString)
                    cmd.Parameters.AddWithValue("@Pers8", row.Cells(64).Value.ToString)
                    cmd.Parameters.AddWithValue("@Pers9", row.Cells(65).Value.ToString)

                    cmd.Parameters.AddWithValue("@Custo", row.Cells(66).Value.ToString) '59
                    cmd.Parameters.AddWithValue("@QtdD1", row.Cells(67).Value.ToString) '60
                    cmd.Parameters.AddWithValue("@QtdD2", "0") '61
                    cmd.Parameters.AddWithValue("@Premix", row.Cells(69).Value.ToString) '62
                    cmd.Parameters.AddWithValue("@PctPremix", row.Cells(70).Value.ToString) '63
                    cmd.Parameters.AddWithValue("@QtdVagao", row.Cells(71).Value.ToString) '64
                    cmd.Parameters.AddWithValue("@QtdPremix", row.Cells(72).Value.ToString) '65
                    cmd.Parameters.AddWithValue("@Propriedade", nomeFaz)
                    cmd.Parameters.AddWithValue("@IdPropriedade", idFaz)
                    cmd.Parameters.AddWithValue("@Lote", cbxLote.Text & " | " & lblCat.Text)
                    cmd.Parameters.AddWithValue("@QtdAnimais", lblQtA.Text)
                    cmd.Parameters.AddWithValue("@Data", data & "|" & varms)

                    cmd.ExecuteNonQuery()

                Catch ex As Exception
                    MsgBox("Erro ao salvar!" + ex.Message)
                    fechar()
                End Try
            Else
                MsgBox("Você precisa escolher um lote para a nova Dieta!")
            End If

        Next
        'MsgBox("Alimento cadastrado com sucesso!")

    End Sub

    Private Sub DeleteDieta()

        Dim cmd As SQLiteCommand
        Dim sqlDelete As String = "Delete from Dieta where Data=@Data"
        'Mensagem se realmente quer excluir

        Try
            abrir()
            cmd = New SQLiteCommand(sqlDelete, con)
            'cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@Data", idDieta)
            cmd.ExecuteNonQuery()
            ' MsgBox("As alterações foram bem sucedidas!")
        Catch ex As Exception
            ' MsgBox("Erro ao editar!" + ex.Message)
            fechar()
        End Try

    End Sub

    Private Sub CarregarDieta()

        Dim da As New SQLiteDataAdapter
        Dim dt As New DataTable
        Dim sql As String

        sql = "Select * from Dieta where Data = " & "'" & idDieta & "'"

        Try
            abrir()

            da = New SQLiteDataAdapter(sql, con)
            dt = New DataTable
            da.Fill(dt)
            dtgAlimentoNome.DataSource = dt
            fechar()
        Catch ex As Exception

        End Try
        'dtgAlimentosDieta.DataSource = dtgTemp.DataSource
        'Label10.Text = Readquery2
    End Sub


    Private Sub btnSalvarDt1_Click(sender As Object, e As EventArgs) Handles btnSalvarDt1.Click
        'se não estiver com alguma dieta aberta então salvar dieta do zero
        If idDieta = "" Then
            SalvarNVDieta()
        Else ' caso uma dieta existente esteja sendo editada... o painel se abre perguntando se substitui a dieta existente ou salva como nova 
            pnlConfSalvarDt.Visible = True
            pnlConfSalvarDt.BringToFront()
            pnlConfSalvarDt.Location = New Point(539, 206)
        End If

    End Sub

    Private Sub SalvarNVDieta()
        'Variável para verificar se a dieta será salva em MS ou MN
        If varms = True Then
            For Each row As DataGridViewRow In dtgAlimentosDieta.Rows
                ' Verifica se as células necessárias não são nulas e evita divisão por zero
                If Not IsDBNull(row.Cells(67).Value) AndAlso Not IsDBNull(row.Cells(4).Value) Then
                    Dim valor67 As Double ' qtd de alimento
                    Dim valor4 As Double ' % de MS

                    Double.TryParse(row.Cells(67).Value.ToString(), valor67)
                    Double.TryParse(row.Cells(4).Value.ToString(), valor4)
                    'confirmado MS então qtd/% de MS
                    If valor4 <> 0 Then
                        row.Cells(67).Value = (valor67 / valor4) * 100
                    Else
                        ' salva como está
                        row.Cells(67).Value = row.Cells(67).Value
                    End If
                End If
            Next
        End If

        SalvarDieta1()

        ' Limpa os DataGridViews
        Dim gridsToClear As DataGridView() = {dtgAlimDt, dtgAlimentosDt, dtgAlimentoNome, dtgAlimentosDieta, dtgTemp, dtgAlimentosPremix} 'dtgVolumoso, dtgVol, 

        For Each dgv As DataGridView In gridsToClear
            dgv.DataSource = Nothing
            dgv.Rows.Clear()
        Next
        'limpar datatable e combobox para caso precise acessar novamente antes de sair de frmDieta
        dtTemp.Clear()
        dt1.Clear()
        'dt2.Clear()
        dt3.Clear()
        dt4.Clear()
        cbxItens.Items.Clear()
        'Carrega as dietassalvas ou modificadas
        BuscarDietas()
        CarregarListaDieta()

        'Reexibe a aba e seleciona
        Me.tabDietas.Parent = Me.tcDieta
        Me.tcDieta.SelectedTab = tabDietas
        'zerar o id da dieta
        idDieta = ""
        'sempre se encerra em false para inicia em false
        varms = False
        pnlConfSalvarDt.Visible = False
    End Sub
    'botão do painel pnlconfSalvarNvDieta
    Private Sub btnSalvarNvDt_Click(sender As Object, e As EventArgs) Handles btnSalvarNvDt.Click
        SalvarNVDieta()
    End Sub
    'botão do painel pnlconfSalvarNvDieta
    Private Sub btnSubstDt_Click(sender As Object, e As EventArgs) Handles btnSubstDt.Click
        ExcluirDieta() 'excluir dieta atual pois é mais prático do que atualizar 
        SalvarNVDieta() 'cria nova dieta
    End Sub

    Private Sub Button5_Click_1(sender As Object, e As EventArgs) Handles Button5.Click
        pnlConfSalvarDt.Visible = False
    End Sub
    'Salvar dieta2
    Private Sub btnSalvarDt2_Click(sender As Object, e As EventArgs) Handles btnSalvarDt2.Click

        'as qtds passam para a coluna 67 e salva a dieta normalmente
        For Each row As DataGridViewRow In dtgAlimentosDieta.Rows
            row.Cells(67).Value = row.Cells(68).Value
            If varms = True Then
                row.Cells(67).Value = row.Cells(68).Value / row.Cells(4).Value * 100 'Format(, "0.00")
            End If
        Next
        SalvarNVDieta()

    End Sub


    'Configurar dtgAlimentosDieta
    Dim varD1 As Boolean
    Dim varD2 As Boolean
    Dim varAlim As Integer
    Dim var60 As Boolean
    Dim var64 As Boolean

    Private Sub ConfigGridAlimentosDieta()
        'gpt
        'Try
        On Error Resume Next
        ' Centralizar todas as células
        For Each col As DataGridViewColumn In dtgAlimentosDieta.Columns
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        Next

        ' Numerar e formatar cabeçalhos das linhas
        For i = 0 To dtgAlimentosDieta.RowCount - 1
            With dtgAlimentosDieta.Rows(i).HeaderCell
                .Value = (i + 1).ToString("D2") ' PadLeft com 2 dígitos
                .Style.BackColor = Color.White
                .Style.Font = New Font("Inter", 8.5, FontStyle.Bold)
                .Style.ForeColor = Color.FromArgb(90, 90, 90)
            End With
        Next

        ' Estilo geral do DataGridView
        With dtgAlimentosDieta
            .RowHeadersWidth = 52
            .DefaultCellStyle.ForeColor = Color.FromArgb(90, 90, 90)
            .DefaultCellStyle.BackColor = Color.White
            .DefaultCellStyle.Font = New Font("Inter", 8.5, FontStyle.Bold)

            ' Configurar colunas específicas
            If .Columns.Count > 67 Then
                .Columns(0).DisplayIndex = 3
                .Columns(1).DisplayIndex = 4
                .Columns(2).Visible = False
                .Columns(3).Width = varAlim
                .Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .Columns(4).Visible = False
                .Columns(5).Visible = False
                ' Oculta colunas de 6 até 85 exceto as necessárias
                For i = 6 To 85
                    If i = 66 Or i = 67 Or i = 68 Or i = 72 Then Continue For
                    .Columns(i).Visible = False
                Next

                .Columns(66).Width = 101
                .Columns(66).DefaultCellStyle.BackColor = Color.WhiteSmoke

                .Columns(67).Visible = varD1
                .Columns(67).Width = 101

                .Columns(68).Visible = varD2
                .Columns(68).Width = 101
                .Columns(68).DefaultCellStyle.BackColor = Color.WhiteSmoke

                .Columns(72).Visible = var64
            End If

            'Se exixtir pré mistura na dieta aparece a imagem do lapis para edição
            For i As Integer = 0 To dtgAlimentosDieta.RowCount() - 1
                If dtgAlimentosDieta.Rows(i).Cells(2).Value = "Pré-Mistura" Then
                    dtgAlimentosDieta.Rows(i).Cells(1).Value = My.Resources.edit5
                    btnPreMix.Enabled = False
                    btnPreMix.BackgroundImage = My.Resources.premistura_of
                Else
                    If i > 0 And varms = False Then
                        btnPreMix.Enabled = True
                        btnPreMix.BackgroundImage = My.Resources.pre_mistura_on
                    End If

                End If
            Next

        End With

    End Sub

    'Configurar dtgAlimentosDt
    Private Sub ConfigGridAlimDt()
        'Se a coluna é impar muda a cor do backcolor e centralizar dados
        For Each columns As DataGridViewColumn In Me.dtgAlimDt.Columns
            dtgAlimDt.Columns(columns.Index).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter ' centralizar as celulas
            If EImpar(columns.Index) = False Then
                dtgAlimDt.Columns(columns.Index).DefaultCellStyle.BackColor = Color.WhiteSmoke ' se o index da coluna for impar então muda a cor

            End If
        Next
        'For Each columns As DataGridViewColumn In Me.dtgAlimDt.Columns
        '    dtgAlimDt.Columns(columns.Index).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter ' centralizar as celulas
        'Next

        On Error Resume Next
        With Me.dtgAlimDt

            .DefaultCellStyle.ForeColor = Color.FromArgb(90, 90, 90)
            .DefaultCellStyle.BackColor = Color.White
            .ColumnHeadersDefaultCellStyle.Font = New Font("Inter", 8, FontStyle.Bold)
            .DefaultCellStyle.Font = New Font("Inter", 8.5, FontStyle.Bold)

            '.Columns(0).Visible = False
            .Columns(1).Visible = False
            '.Columns(2).Visible = False
            '.Columns(3).Visible = False
            .Columns(0).DisplayIndex = 2
            .Columns(0).Frozen = True
            '.Columns(2).Visible = False
            '.Columns(3).Width = varAlim
            .Columns(2).HeaderText = "Selecione o alimento"
            .Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft ' voltar alimentos para esquerda
            .Columns(2).Width = 280
            .Columns(2).Frozen = True

            For i = 3 To 54
                .Columns(i).Width = 70
            Next

            For i = 55 To 85
                .Columns(i).Visible = False
            Next

        End With
    End Sub

    Dim varms As Boolean
    Private Sub btnMS_Click(sender As Object, e As EventArgs) Handles btnMS.Click
        MSOn()
    End Sub
    'Passar a dieta para a MN
    Private Sub btnMN_Click(sender As Object, e As EventArgs) Handles btnMN.Click

        If varms = True Then
            varms = False
            'If varms = False Then
            btnMN.BackgroundImage = My.Resources.mn_on
            btnMS.BackgroundImage = My.Resources.ms_of
            lblmtria.Text = "Base Matéria Natural"

            For i As Integer = 0 To dtgAlimentosDieta.Rows.Count - 1
                If dtgAlimentosDieta.Rows(i).Cells(67).Value > 0 Then
                    dtgAlimentosDieta.Rows(i).Cells(67).Value = dtgAlimentosDieta.Rows(i).Cells(67).Value / dtgAlimentosDieta.Rows(i).Cells(4).Value * 100

                End If
                If dtgAlimentosDieta.Rows(i).Cells(68).Value > 0 Then
                    dtgAlimentosDieta.Rows(i).Cells(68).Value = dtgAlimentosDieta.Rows(i).Cells(68).Value / dtgAlimentosDieta.Rows(i).Cells(4).Value * 100

                End If

            Next
            CalcularValorDieta01()
            CalcularValorDieta02()
            CalcularDieta01()
            CalcularDieta02()

            CalcularFinan01()
            CalcularFinan02()
            'ArredondarNmrosDtgDieta()
            'End If
        End If
    End Sub
    ' Passar a dieta para a MS
    Private Sub MSOn()
        ' Verifica se já está na base de matéria seca
        If varms = False Then
            varms = True

            ' Atualiza as imagens dos botões
            btnMN.BackgroundImage = My.Resources.mn_of
            btnMS.BackgroundImage = My.Resources.ms_on

            ' Atualiza o rótulo
            lblmtria.Text = "Base Matéria Seca"

            ' Percorre as linhas do DataGridView
            For i As Integer = 0 To dtgAlimentosDieta.Rows.Count - 1
                Dim row As DataGridViewRow = dtgAlimentosDieta.Rows(i)

                ' Valida os valores antes de fazer os cálculos
                Dim qtd1 = Convert.ToDouble(If(IsNumeric(row.Cells(67).Value), row.Cells(67).Value, 0))
                Dim qtd2 = Convert.ToDouble(If(IsNumeric(row.Cells(68).Value), row.Cells(68).Value, 0))
                Dim ms = Convert.ToDouble(If(IsNumeric(row.Cells(4).Value), row.Cells(4).Value, 0))

                ' Converte os valores para base MS
                If qtd1 > 0 Then row.Cells(67).Value = Math.Round(qtd1 * ms / 100, 4)
                If qtd2 > 0 Then row.Cells(68).Value = Math.Round(qtd2 * ms / 100, 4)
            Next

            ' Recalcula os valores da dieta
            CalcularValorDieta01()
            CalcularValorDieta02()
            CalcularDieta01MS()
            CalcularDieta02MS()
            CalcularFinan01()
            CalcularFinan02()
        End If

    End Sub
    'Configurar o painel inferior da dieta de acordo com a quantidade de linhas no datagrid 
    Private Sub PainelDieta()
        Dim x As Integer = dtgAlimentosDieta.Rows.Count

        ' Tamanho base do painel
        Dim baseAltura As Integer = 153
        Dim incremento As Integer = 22
        Dim maxLinhas As Integer = 10

        ' Calcula a altura do painel proporcional ao número de linhas (máximo de 10 linhas)
        Dim alturaPainel As Integer = If(x <= 3, baseAltura, Math.Min(baseAltura + ((x - 3) * incremento), baseAltura + ((maxLinhas - 3) * incremento)))
        pnlDieta.Size = New Size(608, alturaPainel)

        ' Calcula a posição do painel inferior com base na altura do painel
        Dim baseY As Integer = 400
        Dim posY As Integer = baseY + (alturaPainel - baseAltura)
        pnlDieta2.Location = New Point(118, posY)

    End Sub
    'Calculos ao editar as colunas 66, 67 e 68 no dtgAlimentosDieta
    Private Sub dtgAlimentosDieta_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dtgAlimentosDieta.CellEndEdit
        On Error Resume Next
        With dtgAlimentosDieta

            If .CurrentCell.ColumnIndex = 66 Or .CurrentCell.ColumnIndex = 67 Or .CurrentCell.ColumnIndex = 68 Then
                ' ArredondarNmrosDtgDieta()
                If varms = True Then
                    CalcularValorDieta01()
                    CalcularValorDieta02()

                    CalcularDieta01MS()
                    CalcularDieta02MS()

                    CalcularFinan01()
                    CalcularFinan02()
                Else
                    CalcularValorDieta01()
                    CalcularValorDieta02()

                    CalcularDieta01()
                    CalcularDieta02()

                    CalcularFinan01()
                    CalcularFinan02()
                End If


            End If
        End With
        If My.Settings.corAvalOnOf = True Then
            CorAval()
        End If
    End Sub

    'guardar o index da row pré-mistura para ser excluida ao desmontar
    Dim indxPremistura As Integer
    'Ao clicar no dtgAlimentosDieta
    Private Sub dtgAlimentosDieta_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dtgAlimentosDieta.CellContentClick
        'ao zerar a liha 0 não calcula
        'ao selecionar o lote entra apenas o avaliador deste e o todos
        '
        'For i As Integer = 0 To dtgAvaliadores.Rows.Count - 1
        '    dtgAvaliadores.Rows(i).Cells(11).Value = 0
        'Next
        For i As Integer = 0 To dtgAlimentosDieta.Rows.Count - 1
            Dim vlr As Double
            vlr += dtgAlimentosDieta.Rows(i).Cells(67).Value
            'desabilitei 14/05
            'If dtgAlimentosDieta.Rows(i).Cells(2).Value = "Pré-Mistura" Or vlr = 0 Or varD2 = True Then
            '    btnPreMix.Enabled = False
            '    btnPreMix.BackgroundImage = My.Resources.premistura_of
            'Else
            '    btnPreMix.Enabled = True
            '    btnPreMix.BackgroundImage = My.Resources.pre_mistura_on

            'End If

        Next


        On Error Resume Next
        With dtgAlimentosDieta

            If .CurrentCell.ColumnIndex = 66 Or .CurrentCell.ColumnIndex = 67 Or .CurrentCell.ColumnIndex = 68 Then
                ' ArredondarNmrosDtgDieta()
                If varms = True Then
                    CalcularValorDieta01()
                    CalcularValorDieta02()
                    CalcularDieta01MS()
                    CalcularDieta02MS()

                    CalcularFinan01()
                    CalcularFinan02()
                Else
                    CalcularValorDieta01()
                    CalcularValorDieta02()
                    CalcularDieta01()
                    CalcularDieta02()

                    CalcularFinan01()
                    CalcularFinan02()
                End If


            End If

            If .CurrentCell.ColumnIndex = 0 Then
                Dim iAlimento As String = dtgAlimentosDieta.CurrentRow.Cells(3).Value.ToString

                dtgAlimentosDieta.Rows.Remove(dtgAlimentosDieta.Rows.Item(dtgAlimentosDieta.CurrentCell.RowIndex))

                dtgAlimentos.Rows(iAlimento).Cells(66).Value = 0
                CorTabAlim()
            End If

            If .CurrentCell.ColumnIndex = 1 And .CurrentRow.Cells(2).Value = "Pré-Mistura" Then
                If desmPrmx = varms Then
                    indxPremistura = .CurrentRow.Index
                    'If .CurrentRow.Cells(3).Value.ToString = txtNomePremix.Text Then
                    '    pnlPreMix.Visible = True
                    '    pnlPreMix.Location = New Point(274, 41)
                    '    pnlPreMix.BringToFront()
                    '    dtgEdtPremix.Visible = False
                    '    dtgAlimentosPremix.Visible = True

                    '    dtgAlimentosPremix.BringToFront()

                    '    ConfigGridAlimentosPremix()
                    '    btnDesmontePremix.Visible = True
                    '    btnExcluirPremix.Enabled = True
                    'Else
                    '    'dtgAlimentosPremix.Rows.Clear()
                    '    'dtgAlimentosPremix.Columns.Clear()

                    'nomePremix = .CurrentRow.Cells(3).Value.ToString

                    pnlPreMix.Visible = True
                    pnlPreMix.Location = New Point(274, 41)
                    pnlPreMix.BringToFront()

                    dtgAlimentosPremix.Visible = False
                    dtgAlimentosPremix.SendToBack()
                    dtgEdtPremix.Visible = True
                    dtgEdtPremix.BringToFront()
                    btnSalvarPremix.Visible = False

                    BuscarPremistura()
                    ConfigGridEdtPremix()
                    btnDesmontePremix.Visible = True
                    'deixar o nome da preistura diferente para separar qual dtg será exibida
                    Dim nmePre() As String = nomePremix.Split("|")
                    txtNomePremix.Text = nmePre(0)

                    'tratamento dtgEdtPremix
                    Dim qtdPremix As Double
                    Dim pctPremix As Double
                    Dim pctTotalPremix As Double

                    For Each row As DataGridViewRow In dtgEdtPremix.Rows
                        qtdPremix += row.Cells(66).Value
                    Next
                    lblQtdTotalPremix.Text = qtdPremix
                    For i As Integer = 0 To dtgEdtPremix.RowCount - 1
                        dtgEdtPremix.Rows(i).Cells(68).Value = lblQtdTotalPremix.Text
                        pctPremix = dtgEdtPremix.Rows(i).Cells(66).Value / dtgEdtPremix.Rows(i).Cells(68).Value * 100
                        dtgEdtPremix.Rows(i).Cells(67).Value = Format(pctPremix, "0.00")
                        pctTotalPremix += dtgEdtPremix.Rows(i).Cells(67).Value
                        dtgEdtPremix.Rows(i).Cells(65).Value = Format(dtgEdtPremix.Rows(i).Cells(66).Value * 1, "0.00")
                        'dtgEdtPremix.Rows(i).Cells(71).Value = dtgEdtPremix.Rows(i).Cells(65).Value - dtgEdtPremix.Rows(i).Cells(69).Value
                    Next
                    'Colorir numero caso esteja no premix
                    lblQtdTotalPremix.Text = Format(qtdPremix, "0.00") & " Kg"
                    'If pctTotalPremix = 99.99 Or pctTotalPremix = 99.98 Or pctTotalPremix = 100.01 Or pctTotalPremix = 100.02 Then
                    Dim x As Integer = dtgEdtPremix.Rows.Count
                    If x > 0 Then
                        pctTotalPremix = 100
                    End If

                    lblPctTotalPremix.Text = pctTotalPremix.ToString("F2") & " %"
                    lblQtdTotalDisponivel.Text = qtdPremix.ToString("F2") & " Kg"

                Else
                    MsgBox("Verifique se a Pré-Misstura foi montada em MS ou MN.")
                End If
            End If

        End With

        PainelDieta()

        If My.Settings.corAvalOnOf = True Then
            CorAval()
        End If

        For i As Integer = 0 To dtgEdtPremix.Rows.Count - 1
            'Dim vlr As Double
            'vlr = 1
            dtgEdtPremix.Rows(i).Cells(66).Value = dtgEdtPremix.Rows(i).Cells(66).Value.ToString("F2") ' * vlr.ToString, "F")

        Next

    End Sub

    Private Sub dtgAlimentosPremix_CellLeave(sender As Object, e As DataGridViewCellEventArgs) Handles dtgAlimentosPremix.CellLeave
        CalcularPremix()
    End Sub

    Private col = ""
    Private colfinan = ""
    'Calculos no enter no dtgAlimentosDieta
    Private Sub dtgAlimentosDieta_CellEnter(sender As Object, e As DataGridViewCellEventArgs) Handles dtgAlimentosDieta.CellEnter
        'For i As Integer = 0 To dtgAvaliadores.Rows.Count - 1
        '    dtgAvaliadores.Rows(i).Cells(11).Value = 0
        'Next
        ' ArredondarNmrosDtgDieta()
        On Error Resume Next
        With dtgAlimentosDieta

            If .CurrentCell.ColumnIndex = 66 Or .CurrentCell.ColumnIndex = 67 Or .CurrentCell.ColumnIndex = 68 Then
                ' ArredondarNmrosDtgDieta()
                If varms = True Then
                    CalcularValorDieta01()
                    CalcularValorDieta02()

                    CalcularDieta01MS()
                    CalcularDieta02MS()

                    CalcularFinan01()
                    CalcularFinan02()
                Else
                    CalcularValorDieta01()
                    CalcularValorDieta02()

                    CalcularDieta01()
                    CalcularDieta02()

                    CalcularFinan01()
                    CalcularFinan02()
                End If

            End If
        End With
        If My.Settings.corAvalOnOf = True Then
            CorAval()
        End If

        col = dtgAlimentosDieta.Columns(e.ColumnIndex).Index

    End Sub
    ''VERIFICAR ACHO Q NAO SE USA MAISS
    'Private Sub dtgAlimentosDieta_EditingControlShowing(sender As Object, e As DataGridViewEditingControlShowingEventArgs) Handles dtgAlimentosDieta.EditingControlShowing
    '    If col = (66) Then
    '        AddHandler e.Control.KeyPress, AddressOf NDecimal
    '    ElseIf col = (67) Then
    '        AddHandler e.Control.KeyPress, AddressOf NDecimal
    '    ElseIf col = (68) Then
    '        AddHandler e.Control.KeyPress, AddressOf NDecimal
    '    End If

    'End Sub
    ''OBTER O INDICE DA COLUNA DO dtgRelatFinan
    'Private Sub dtgRelatFinan_CellEnter(sender As Object, e As DataGridViewCellEventArgs) Handles dtgRelatFinan.CellEnter

    '    colfinan = dtgRelatFinan.Columns(e.ColumnIndex).Index

    'End Sub
    ''VERIFICAR ACHO Q NAO SE USA MAISS
    'Private Sub dtgRelatFinan_EditingControlShowing(sender As Object, e As DataGridViewEditingControlShowingEventArgs) Handles dtgRelatFinan.EditingControlShowing
    '    If colfinan = (1) Then
    '        AddHandler e.Control.KeyPress, AddressOf NDecimal
    '    ElseIf colfinan = (2) Then
    '        AddHandler e.Control.KeyPress, AddressOf NDecimal

    '    End If

    'End Sub
    'Calculadora
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Process.Start("C:\Windows\system32\calc.exe")

    End Sub
    'Formatação da dtgAlimDt
    Private Sub dtgAlimDt_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dtgAlimDt.CellFormatting
        If e.ColumnIndex > 2 Then 'AndAlso IsNumeric(e.Value) 
            If IsNumeric(e.Value) Then
                e.Value = Format(CDbl(e.Value), "0.00")
                e.FormattingApplied = True
            End If
        End If
    End Sub
    'Abrir painel de alimentos da dieta com suas propriedades
    Private Sub btnAliDiet_Click(sender As Object, e As EventArgs) Handles btnAliDiet.Click
        pnlAlimDieta.Location = New Point(150, 2)
        pnlAlimDieta.BringToFront()
        dtgAlimDt.DataSource = dtgAlimentosDieta.DataSource

        pnlAlimDieta.Visible = True
        ConfigGridAlimDt()
    End Sub
    'Fechar painel com alimentos da dieta
    Private Sub btnFecharAliDiet_Click(sender As Object, e As EventArgs) Handles btnFecharAliDiet.Click
        pnlAlimDieta.Visible = False

    End Sub
    'Abrir dieta 2
    Private Sub btnAbrirD2_Click(sender As Object, e As EventArgs) Handles btnAbrirD2.Click
        'If varD2 = False Then
        varD2 = True
        varD1 = True
        'varAlim = 202 '301 + 90

        Dim x As Integer = dtgAlimentosDieta.Rows.Count
        If x < 10 Then
            varAlim = 202 '303
        Else
            varAlim = 188 '303
        End If

        btnFecharD2.Visible = True
        btnFecharD1.Visible = True
        btnFecharD1.Location = New Point(470, 15)
        lblD1.Visible = True
        lblKD1.Visible = True
        lblD2.Visible = True
        lblKD2.Visible = True

        lblD1.Location = New Point(417, 2)
        lblKD1.Location = New Point(417, 18)
        lblPreco.Location = New Point(336, 2)
        lblPrecokD.Location = New Point(336, 18)
        btnMS.Location = New Point(231, 6)
        btnMN.Location = New Point(269, 6)
        btnAbrirD2.BackgroundImage = My.Resources.dieta2_on
        'Base pnl Dieta
        lblTotalKgD2.Visible = True
        lblTotalVrD2.Visible = True
        lblTotalKgD1.Visible = True
        lblTotalVrD1.Visible = True

        lblTotalKgD1.Location = New Point(423, 3)
        lblTotalVrD1.Location = New Point(423, 28)

        btnSalvarDt2.Visible = True
        btnSalvarDt1.Location = New Point(415, 41)

        ' ajustar dtgAvaliadores
        'varAval = 225 '266    CONFIGURAR COM A BARRA E SEM A BARRA DE ROLAGEM

        If dtgRelatFinan.Visible = False Then
            'pnlAvalD1.Visible = False
            'pnlAvalD1.SendToBack()
            'pnlCustoD1.Visible = False
            'pnlCustoD1.SendToBack()
            'pnlCustoD2.Visible = False
            'pnlCustoD2.SendToBack()

            pnlAvalD1.Visible = False
            pnlAvalD2.Visible = False
            pnlCustoD1.Visible = False
            pnlCustoD2.Visible = False
            pnlCustoD1eD2.Visible = False

            'pnlCustoD1eD2.Visible = True
            'pnlCustoD1eD2.BringToFront()

        Else
            pnlAvalD1.Visible = False
            pnlAvalD2.Visible = False
            pnlCustoD1.Visible = False
            pnlCustoD2.Visible = False
            'pnlCustoD1eD2.Visible = False

            pnlCustoD1eD2.Location = New Point(270, 1)
            pnlCustoD1eD2.Visible = True
            pnlCustoD1eD2.BringToFront()
        End If
        varFinan = 334 ' ajustar dtgFinanceiro

        'ElseIf varD1 = False Then

        '    varD1 = True

        '    varAlim = 254 '301 + 90
        '    btnFecharD1.Visible = True
        '    'btnFecharD1.Location = New Point(470, 15)

        '    lblD1.Visible = True
        '    lblKD1.Visible = True
        '    lblPreco.Visible = True
        '    lblPrecokD.Visible = True
        '    btnMS.Location = New Point(231, 6)
        '    btnMN.Location = New Point(269, 6)
        '    btnAbrirD2.BackgroundImage = My.Resources.dieta2_on
        '    'Base pnl Dieta
        '    'lblTotalKgD2.Visible = True
        '    'lblTotalVrD2.Visible = True

        '    lblTotalKgD1.Visible = True
        '    lblTotalVrD1.Visible = True

        '    btnSalvarDt1.Visible = True
        '    'btnSalvarDt1.Location = New Point(415, 41)

        ' ajustar dtgAvaliadores
        'varAval = 225 '266    CONFIGURAR COM A BARRA E SEM A BARRA DE ROLAGEM

        If dtgRelatFinan.Visible = False Then
            'pnlAvalD1.Visible = False
            'pnlAvalD1.SendToBack()
            'pnlCustoD1.Visible = False
            'pnlCustoD1.SendToBack()
            'pnlCustoD2.Visible = False
            'pnlCustoD2.SendToBack()

            pnlAvalD1.Visible = False
            pnlAvalD2.Visible = False
            pnlCustoD1.Visible = False
            pnlCustoD2.Visible = False
            pnlCustoD1eD2.Visible = False

            'pnlCustoD1eD2.Visible = True
            'pnlCustoD1eD2.BringToFront()

        Else
            pnlAvalD1.Visible = False
            pnlAvalD2.Visible = False
            pnlCustoD1.Visible = False
            pnlCustoD2.Visible = False
            'pnlCustoD1eD2.Visible = False

            pnlCustoD1eD2.Location = New Point(270, 1)
            pnlCustoD1eD2.Visible = True
            pnlCustoD1eD2.BringToFront()
        End If
        varFinan = 334 ' ajustar dtgFinanceiro

        'End If
        If My.Settings.corAvalOnOf = False Then
            varAval7 = False
            varAval = 352
            Label21.Visible = False
            lblIdeal.Visible = False
            Label11.Visible = False
        Else
            varAval7 = True
            varAval = 225
            Label21.Visible = True
            lblIdeal.Visible = True
            Label11.Visible = True
        End If
        btnFecharD1.Enabled = True
        btnFecharD2.Enabled = True
        ConfigGridAlimentosDieta()
        ConfigGridAvaliadores()
        ConfigGridFinanceiro()
        'btnPreMix.Enabled = False
        'btnPreMix.BackgroundImage = My.Resources.premistura_of
    End Sub
    'Fechar dieta 2
    Private Sub btFecharD2_Click(sender As Object, e As EventArgs) Handles btnFecharD2.Click
        EsconderD2()

    End Sub
    'Sub para esconder a dieta 2
    Private Sub EsconderD2()
        ' ajustar dtgAlimentosDieta
        varD2 = False
        'varAlim = 302 '301 + 90

        Dim x As Integer = dtgAlimentosDieta.Rows.Count
        If x < 10 Then
            varAlim = 302 '303
        Else
            varAlim = 288 '303
        End If

        btnFecharD2.Visible = False
        btnFecharD1.Enabled = False

        btnFecharD1.Location = New Point(571, 15)
        lblD2.Visible = False
        lblKD2.Visible = False
        lblD1.Location = New Point(517, 2)
        lblKD1.Location = New Point(517, 18)
        lblPreco.Location = New Point(417, 2)
        lblPrecokD.Location = New Point(417, 18)
        btnMS.Location = New Point(300, 6)
        btnMN.Location = New Point(338, 6)
        btnAbrirD2.BackgroundImage = My.Resources.dieta2_off

        'Base pnl Dieta
        lblTotalKgD2.Visible = False
        lblTotalVrD2.Visible = False

        lblTotalKgD1.Location = New Point(523, 3)
        lblTotalVrD1.Location = New Point(523, 28)
        lblTotalKgD1.BringToFront()
        lblTotalVrD1.BringToFront()
        btnSalvarDt2.Visible = False
        btnSalvarDt1.Location = New Point(515, 41)

        ' ajustar dtgAvaliadores
        'varAval = 342 '402

        If dtgRelatFinan.Visible = False Then
            'pnlAvalD1.Visible = True
            'pnlAvalD1.BringToFront()
            'pnlCustoD1.Visible = False
            'pnlCustoD1.SendToBack()
            'pnlCustoD2.Visible = False
            'pnlCustoD2.SendToBack()

            'pnlAvalD1.Visible = False
            pnlAvalD2.Visible = False
            pnlCustoD1.Visible = False
            pnlCustoD2.Visible = False
            pnlCustoD1eD2.Visible = False

            pnlAvalD1.Location = New Point(270, 1)
            pnlAvalD1.Visible = True
            pnlAvalD1.BringToFront()


        Else
            pnlAvalD1.Visible = False
            pnlAvalD2.Visible = False
            'pnlCustoD1.Visible = False
            pnlCustoD2.Visible = False
            pnlCustoD1eD2.Visible = False

            pnlCustoD1.Location = New Point(270, 1)
            pnlCustoD1.Visible = True
            pnlCustoD1.BringToFront()
        End If
        varFinan = 468 ' ajustar dtgFinanceiro

        If My.Settings.corAvalOnOf = False Then
            varAval7 = False
            varAval = 469
            Label21.Visible = False
            lblIdeal.Visible = False
            Label11.Visible = False
        Else
            varAval7 = True
            varAval = 342
            Label21.Visible = True
            lblIdeal.Visible = True
            Label11.Visible = True
        End If

        ConfigGridAlimentosDieta()
        ConfigGridAvaliadores()
        ConfigGridFinanceiro()

    End Sub
    'Controles dos paineis e largura das colunas do dtgAvaliadores ao abrir ou fechar a dieta2
    Private Sub RelatNutri()
        btnRelatNutri.BackgroundImage = My.Resources.bt_Rel_nutri_on
        btnRelatFinan.BackgroundImage = My.Resources.bt_Rel_fin_off

        dtgRelatFinan.Visible = False
        dtgAvaliadores.Visible = True
        btnSelecAval.Visible = True

        lblResult.Text = "Resultado Nutricional"
        lblIndicadores.Text = "Avaliadores:"

        If varD2 = False Then
            varAval = 342

            'pnlAvalD1.Visible = False
            pnlAvalD2.Visible = False
            pnlCustoD1.Visible = False
            pnlCustoD2.Visible = False
            pnlCustoD1eD2.Visible = False

            pnlAvalD1.Location = New Point(270, 1)
            pnlAvalD1.Visible = True
            pnlAvalD1.BringToFront()
        ElseIf varD1 = False Then

            varAval = 342

            pnlAvalD1.Visible = False
            'pnlAvalD2.Visible = False
            pnlCustoD1.Visible = False
            pnlCustoD2.Visible = False
            pnlCustoD1eD2.Visible = False

            pnlAvalD2.Location = New Point(270, 1)
            pnlAvalD2.Visible = True
            pnlAvalD2.BringToFront()

        ElseIf varD1 And varD2 = True Then
            varAval = 225

            pnlAvalD1.Visible = False
            pnlAvalD2.Visible = False
            pnlCustoD1.Visible = False
            pnlCustoD2.Visible = False
            pnlCustoD1eD2.Visible = False

            'pnlAvalD1.Visible = True
            'pnlAvalD1.BringToFront()

        End If

    End Sub
    'Esconder dieta 1
    Private Sub btnFecharD1_Click(sender As Object, e As EventArgs) Handles btnFecharD1.Click
        EsconderD1()
    End Sub
    'Sub Esconder dieta 1
    Private Sub EsconderD1()
        ' ajustar dtgAlimentosDieta

        varD1 = False
        'varAlim = 302 '301 + 90

        Dim x As Integer = dtgAlimentosDieta.Rows.Count
        If x < 10 Then
            varAlim = 302 '303
        Else
            varAlim = 288 '303
        End If

        btnFecharD1.Visible = False
        btnFecharD2.Enabled = False
        'btnFecharD1.Location = New Point(571, 15)
        lblD1.Visible = False
        lblKD1.Visible = False
        lblPreco.Location = New Point(417, 2)
        lblPrecokD.Location = New Point(417, 18)
        btnMS.Location = New Point(300, 6)
        btnMN.Location = New Point(338, 6)
        btnAbrirD2.BackgroundImage = My.Resources.Dieta01_of

        lblTotalKgD1.Visible = False
        lblTotalVrD1.Visible = False
        btnSalvarDt1.Visible = False

        ' ajustar dtgAvaliadores
        ' varAval = 342 '402

        If dtgRelatFinan.Visible = False Then

            pnlAvalD1.Visible = False
            'pnlAvalD2.Visible = False
            pnlCustoD1.Visible = False
            pnlCustoD2.Visible = False
            pnlCustoD1eD2.Visible = False

            pnlAvalD2.Location = New Point(270, 1)
            pnlAvalD2.Visible = True
            pnlAvalD2.BringToFront()


        Else
            pnlAvalD1.Visible = False
            pnlAvalD2.Visible = False
            pnlCustoD1.Visible = False
            pnlCustoD2.Visible = False
            pnlCustoD1eD2.Visible = False

            pnlCustoD2.Location = New Point(270, 1)
            pnlCustoD2.Visible = True
            pnlCustoD2.BringToFront()
        End If
        varFinan = 468 ' ajustar dtgFinanceiro

        If My.Settings.corAvalOnOf = False Then
            varAval7 = False
            varAval = 469
            Label21.Visible = False
            lblIdeal.Visible = False
            Label11.Visible = False
        Else
            varAval7 = True
            varAval = 342
            Label21.Visible = True
            lblIdeal.Visible = True
            Label11.Visible = True
        End If

        ConfigGridAlimentosDieta()
        ConfigGridAvaliadores()
        ConfigGridFinanceiro()
        'btnPreMix.Enabled = True
        'btnPreMix.BackgroundImage = My.Resources.pre_mistura_on
    End Sub
    'btn dos avaliadores
    Private Sub btnRelatNutri_Click(sender As Object, e As EventArgs) Handles btnRelatNutri.Click
        RelatNutri()
    End Sub
    'btn financeiro
    Private Sub btnRelatFinan_Click(sender As Object, e As EventArgs) Handles btnRelatFinan.Click
        btnCalcularDieta.PerformClick()

        btnRelatNutri.BackgroundImage = My.Resources.bt_Rel_nutri_off
        btnRelatFinan.BackgroundImage = My.Resources.bt_Rel_fin_on
        lblResult.Text = "Resultado Financeiro"
        lblIndicadores.Text = "Indicadores:"

        btnSelecAval.Visible = False
        dtgRelatFinan.Visible = True
        dtgAvaliadores.Visible = False

        'pnlResultNutri.Size = New Size(608, 363)

        If varD2 = False Then
            varFinan = 468 ' ajustar dtgFinanceiro

            pnlAvalD1.Visible = False
            pnlAvalD2.Visible = False
            ' pnlCustoD1.Visible = False
            pnlCustoD2.Visible = False
            pnlCustoD1eD2.Visible = False

            pnlCustoD1.Location = New Point(270, 1)
            pnlCustoD1.Visible = True
            pnlCustoD1.BringToFront()
        ElseIf varD1 = False Then

            varFinan = 468 ' ajustar dtgFinanceiro
            pnlAvalD1.Visible = False
            pnlAvalD2.Visible = False
            pnlCustoD1.Visible = False
            'pnlCustoD2.Visible = True
            pnlCustoD1eD2.Visible = True

            pnlCustoD2.Location = New Point(270, 1)
            pnlCustoD2.Visible = True
            pnlCustoD2.BringToFront()

        ElseIf varD1 And varD2 = True Then
            varFinan = 334 ' ajustar dtgFinanceiro
            pnlAvalD1.Visible = False
            pnlAvalD2.Visible = False
            pnlCustoD1.Visible = False
            pnlCustoD2.Visible = False
            'pnlCustoD1eD2.Visible = False

            pnlCustoD1eD2.Location = New Point(270, 1)
            pnlCustoD1eD2.Visible = True
            pnlCustoD1eD2.BringToFront()
        End If

        ConfigGridFinanceiro()

        For Each row As DataGridViewRow In dtgRelatFinan.Rows
            Dim valor2 As Double = 0
            Dim valor3 As Double = 0

            ' Verifica e converte a célula 2
            If Not IsDBNull(row.Cells(2).Value) AndAlso IsNumeric(row.Cells(2).Value) Then
                valor2 = Convert.ToDouble(row.Cells(2).Value)
            End If

            ' Verifica e converte a célula 3
            If Not IsDBNull(row.Cells(3).Value) AndAlso IsNumeric(row.Cells(3).Value) Then
                valor3 = Convert.ToDouble(row.Cells(3).Value)
            End If

            ' Corrigir valores inválidos
            If Double.IsNaN(valor2) OrElse Double.IsInfinity(valor2) Then valor2 = 0
            If Double.IsNaN(valor3) OrElse Double.IsInfinity(valor3) Then valor3 = 0

            row.Cells(2).Value = valor2
            row.Cells(3).Value = valor3
        Next
    End Sub
    'Padronizar dados do dtgAlimentosDieta
    Private Sub dtgalimentosdieta_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dtgAlimentosDieta.CellFormatting
        If e.ColumnIndex = 66 Or 67 Or 68 Then 'AndAlso IsNumeric(e.Value)
            If IsNumeric(e.Value) Then
                e.Value = Format(CDbl(e.Value), "0.00")
                'e.Value = e.Value.ToString("F2")
                e.FormattingApplied = True
            End If
        End If
    End Sub
    'Padronizar dados do dtgRelatFinan
    Private Sub dtgRelatFinan_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dtgRelatFinan.CellFormatting
        If dtgRelatFinan.Rows(1).Cells(2).Value > 0 Then

            If e.ColumnIndex = 2 Or 3 Then 'AndAlso IsNumeric(e.Value)
                If IsNumeric(e.Value) Then
                    e.Value = Format(CDbl(e.Value), "0.00")
                    'e.Value = e.Value.ToString("C2")
                    e.FormattingApplied = True
                End If
            Else
                'On Error Resume Next
            End If
        End If
    End Sub
    'Configurar o dtgRelatFinan
    Dim varFinan As Integer
    Private Sub ConfigGridFinanceiro()

        For Each columns As DataGridViewColumn In Me.dtgRelatFinan.Columns
            dtgRelatFinan.Columns(columns.Index).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter ' centralizar as celulas

        Next

        On Error Resume Next
        With Me.dtgRelatFinan

            .DefaultCellStyle.ForeColor = Color.FromArgb(90, 90, 90)
            .DefaultCellStyle.BackColor = Color.White
            ' .ColumnHeadersDefaultCellStyle.Font = New Font("Inter", 8, FontStyle.Bold)
            .DefaultCellStyle.Font = New Font("Inter", 8.5, FontStyle.Bold)

            .Columns(0).Visible = False
            .Columns(1).Width = varFinan
            .Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft ' voltar alimentos para esquerda
            .Columns(2).Visible = varD1
            .Columns(2).Width = 133
            .Columns(2).DefaultCellStyle.BackColor = Color.WhiteSmoke
            .Columns(3).Width = 133
            .Columns(3).Visible = varD2

            .Rows(2).ReadOnly = True
            .Rows(3).ReadOnly = True
            .Rows(4).ReadOnly = True
            .Rows(5).ReadOnly = True
            .Rows(6).ReadOnly = True
            .Rows(7).ReadOnly = True
            .Rows(8).ReadOnly = True
            .Rows(9).ReadOnly = True
            .Rows(10).ReadOnly = True
            .Rows(11).ReadOnly = True
            .Rows(12).ReadOnly = True

        End With
        dtgRelatFinan.Rows(0).Cells(2).Style.BackColor = Color.FromArgb(207, 247, 211)
        dtgRelatFinan.Rows(1).Cells(2).Style.BackColor = Color.FromArgb(207, 247, 211)
        dtgRelatFinan.Rows(0).Cells(3).Style.BackColor = Color.FromArgb(207, 247, 211)
        dtgRelatFinan.Rows(1).Cells(3).Style.BackColor = Color.FromArgb(207, 247, 211)

        btnCalcularDieta.PerformClick()
    End Sub

    'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX    CALCULOS DO DTGALIMENTOSDIETA     XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
    'Calculos na Grid Alimentos Dieta01
    Dim qtdProduto As Double = 0
    Dim totalValor As Double = 0

    Private Sub CalcularValorDieta01()

        qtdProduto = 0
        totalValor = 0
        Try

            For Each row As DataGridViewRow In dtgAlimentosDieta.Rows
                If varms = False Then
                    qtdProduto += row.Cells(67).Value
                    totalValor += row.Cells(66).Value * row.Cells(67).Value

                    row.Cells(76).Value = row.Cells(66).Value * row.Cells(67).Value ' cell qtd animais usado p subtotal devido ao grafico custo

                ElseIf varms = True Then
                    qtdProduto += row.Cells(67).Value / row.Cells(4).Value * 100
                    totalValor += row.Cells(66).Value * (row.Cells(67).Value / row.Cells(4).Value) * 100
                End If

            Next

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try

        lblTotalVrD1.Text = "R$ " & totalValor.ToString("F2")
        lblTotalKgD1.Text = "Kg " & qtdProduto.ToString("F2")

        'totalMn += col.Cells(5).Value ' soma de MN
    End Sub
    'Calculos na Grid Alimentos Dieta02
    Dim qtdProduto2 As Double = 0
    Dim totalValor2 As Double = 0
    Private Sub CalcularValorDieta02()

        Dim vrProduto As Double = 0
        qtdProduto2 = 0
        totalValor2 = 0

        Try

            For Each row As DataGridViewRow In dtgAlimentosDieta.Rows
                If varms = False Then
                    qtdProduto2 += row.Cells(68).Value
                    totalValor2 += row.Cells(66).Value * row.Cells(68).Value

                    'row.Cells(76).Value = row.Cells(66).Value * row.Cells(67).Value
                ElseIf varms = True Then
                    qtdProduto2 += row.Cells(68).Value / row.Cells(4).Value * 100
                    totalValor2 += row.Cells(66).Value * (row.Cells(68).Value / row.Cells(4).Value) * 100

                    'row.Cells(76).Value = (row.Cells(66).Value / row.Cells(67).Value)
                End If
            Next
            lblTotalVrD2.Text = "R$ " & totalValor2.ToString("F2")
            lblTotalKgD2.Text = "Kg " & qtdProduto2.ToString("F2")

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try


        'totalMn += col.Cells(5).Value ' soma de MN
    End Sub

    'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX       CALCULOS FINANCEIROS       XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
    Dim leitePrev As Double = 0
    Dim leitePrec As Double = 0

    'Montar a tabela de RELATÓRIO FINANCEIRO
    'Private Function TabelaRelatFinanceiro() As DataTable
    Private Sub TabelaRelatFinanceiro()

        Try

            Dim dt1 As New DataTable()

            dt1.Columns.Add("Marcar", GetType(System.Boolean))
            dt1.Columns.Add("Indicador")
            dt1.Columns.Add("ValorD1")
            dt1.Columns.Add("ValorD2")

            dt1.Rows.Add(False, "Leite Previstro (Litros/dia)", leitePrev, leitePrev)   'lblleite.text
            dt1.Rows.Add(False, "Preço do Leite (R$)", leitePrec, leitePrec)
            dt1.Rows.Add(False, "LSCA Vaca/dia", "0", "0")                  '(producao/vaca*preco do leite)-custo por animal    - lblleite.text*lblPrecLeit.Text / custo anim/dia
            dt1.Rows.Add(False, "LSCA Lote/dia", "0", "0")                  '(producao/lote*preco do leite)-custo por lote      - lblleite.text*lblPrecLeit.Text*lblqta.text / custo anim/dia*lblqta.text
            dt1.Rows.Add(False, "LSCA Litro/dia", "0", "0")                 '(litro*preco do leite)-custo por / litro           -lblPrecLeit.Text- (custo anim/dia/lblleite.text)
            dt1.Rows.Add(False, "Custo Total Vaca/ dia (R$)", "0", "0")               'custo do lote                                      -custo anim/dia*lblqta.text
            dt1.Rows.Add(False, "Custo Total Lote/ dia (R$)", "0", "0")
            dt1.Rows.Add(False, "Custo Kg MN", "0", "0")                    'lblvalor.text/lblqtd.text / .88
            dt1.Rows.Add(False, "Custo Kg MS", "0", "0")                    'lblvalor.text/lblqtd.text
            dt1.Rows.Add(False, "R$/Litro de leite", "0", "0")              'custo animdia/lblleite.text
            dt1.Rows.Add(False, "R$/ Litro de leite corrigido 4%", "0", "0") '0.4*lblleite.text+(0.15*lblgord.text*lblleite.text)
            dt1.Rows.Add(False, "% do preço do leite", "0", "0")            'quanto % do leite se gasta com a dieta
            dt1.Rows.Add(False, "Relação Leite / Concentrado", "0")         '=leite.text/(qtd $ gasto em concentrado)
            dt1.Rows.Add(False, "Relação Leite / Consumo MS", "0", "0")     ' leite.text / qtd em kg/ms

            dtgRelatFinan.DataSource = dt1
            'Return dt1
        Catch ex As Exception
            Throw ex
        End Try

    End Sub


    'Calculos na Grid Financeiro Dieta01
    Private Sub CalcularFinan01()
        'Preencher campo preço e produção automaticamente
        If dtgRelatFinan.Rows(0).Cells(2).Value = 0 Then
            dtgRelatFinan.Rows(0).Cells(2).Value = leitePrev.ToString("F2")
        ElseIf dtgRelatFinan.Rows(1).Cells(2).Value = 0 Then
            dtgRelatFinan.Rows(1).Cells(2).Value = leitePrec.ToString("F2")
        End If

        Dim qtdConcent As Double
        Dim qtdMS As Double

        Dim lscaVacaDiaD1 As Double
        Dim lscaLoteDia1 As Double
        Dim lscaLitroDia1 As Double
        Dim leite4pct As Double
        Dim percleite As Double

        If varms = False Then

            For i As Integer = 0 To dtgAlimentosDieta.Rows.Count - 1
                qtdMS += dtgAlimentosDieta.Rows(i).Cells(67).Value * dtgAlimentosDieta.Rows(i).Cells(4).Value
                If dtgAlimentosDieta.Rows(i).Cells(2).Value.ToString = "Concentrados Energéticos" Or dtgAlimentosDieta.Rows(i).Cells(2).Value.ToString = "Pré-Mistura" Or dtgAlimentosDieta.Rows(i).Cells(2).Value.ToString = "Concentrados Proteicos" Or dtgAlimentosDieta.Rows(i).Cells(2).Value.ToString = "Minerais" Or dtgAlimentosDieta.Rows(i).Cells(2).Value.ToString = "Outros" Then 'Or dtgAlimentosDieta.Rows(i).Cells(1).Value = "Concentrados Proteicos" Then

                    qtdConcent += dtgAlimentosDieta.Rows(i).Cells(67).Value

                End If

            Next
            If qtdProduto > 0.001 Then

                On Error Resume Next
                'Calculos Financeiro LSCA /Animal
                lscaVacaDiaD1 = dtgRelatFinan.Rows(0).Cells(2).Value * dtgRelatFinan.Rows(1).Cells(2).Value - totalValor
                dtgRelatFinan.Rows(2).Cells(2).Value = lscaVacaDiaD1
                'Calculos Financeiro LSCA / Lote
                lscaLoteDia1 = lscaVacaDiaD1 * lblQtA.Text
                dtgRelatFinan.Rows(3).Cells(2).Value = lscaLoteDia1
                'Calculos Financeiro LSCA / Litro
                lscaLitroDia1 = lscaVacaDiaD1 / dtgRelatFinan.Rows(0).Cells(2).Value
                dtgRelatFinan.Rows(4).Cells(2).Value = lscaLitroDia1
                'Calculos Financeiro Custo total / animal dia
                dtgRelatFinan.Rows(5).Cells(2).Value = totalValor

                'Calculos Financeiro Custo total / lote dia
                dtgRelatFinan.Rows(6).Cells(2).Value = totalValor * lblQtA.Text

                'Custo kg na MN
                dtgRelatFinan.Rows(7).Cells(2).Value = totalValor / qtdProduto
                'Custo kg na MS
                dtgRelatFinan.Rows(8).Cells(2).Value = totalValor / qtdMS * 100
                'Custo por litro de leite
                dtgRelatFinan.Rows(9).Cells(2).Value = totalValor / dtgRelatFinan.Rows(0).Cells(2).Value
                'Custo por litro de leite 4%
                leite4pct = 0.4 * dtgRelatFinan.Rows(0).Cells(2).Value + (0.15 * lblGord.Text * dtgRelatFinan.Rows(0).Cells(2).Value)
                dtgRelatFinan.Rows(10).Cells(2).Value = totalValor / leite4pct
                '% do preço do leite
                percleite = (totalValor / dtgRelatFinan.Rows(0).Cells(2).Value / dtgRelatFinan.Rows(1).Cells(2).Value) * 100
                dtgRelatFinan.Rows(11).Cells(2).Value = percleite
                'Relação Leite / Concentrado
                If qtdConcent > 0 Then
                    dtgRelatFinan.Rows(12).Cells(2).Value = dtgRelatFinan.Rows(0).Cells(2).Value / qtdConcent
                Else
                    dtgRelatFinan.Rows(12).Cells(2).Value = 0
                End If
                'Relação Leite / Consumo MS
                dtgRelatFinan.Rows(13).Cells(2).Value = dtgRelatFinan.Rows(0).Cells(2).Value / qtdMS * 100


            ElseIf varms = True Then

                For i As Integer = 0 To dtgAlimentosDieta.Rows.Count - 1
                    qtdMS += dtgAlimentosDieta.Rows(i).Cells(67).Value ' * dtgAlimentosDieta.Rows(i).Cells(3).Value
                    If dtgAlimentosDieta.Rows(i).Cells(2).Value.ToString = "Concentrados Energéticos" Or dtgAlimentosDieta.Rows(i).Cells(2).Value.ToString = "Pré-Mistura" Or dtgAlimentosDieta.Rows(i).Cells(2).Value.ToString = "Concentrados Proteicos" Or dtgAlimentosDieta.Rows(i).Cells(2).Value.ToString = "Minerais" Or dtgAlimentosDieta.Rows(i).Cells(2).Value.ToString = "Outros" Then 'Or dtgAlimentosDieta.Rows(i).Cells(1).Value = "Concentrados Proteicos" Then

                        qtdConcent += dtgAlimentosDieta.Rows(i).Cells(67).Value / dtgAlimentosDieta.Rows(i).Cells(4).Value * 100

                    End If
                Next

                On Error Resume Next
                'Calculos Financeiro LSCA /Animal
                lscaVacaDiaD1 = dtgRelatFinan.Rows(0).Cells(2).Value * dtgRelatFinan.Rows(1).Cells(2).Value - totalValor
                dtgRelatFinan.Rows(2).Cells(2).Value = lscaVacaDiaD1
                'Calculos Financeiro LSCA / Lote
                lscaLoteDia1 = lscaVacaDiaD1 * lblQtA.Text
                dtgRelatFinan.Rows(3).Cells(2).Value = lscaLoteDia1
                'Calculos Financeiro LSCA / Litro
                lscaLitroDia1 = lscaVacaDiaD1 / dtgRelatFinan.Rows(0).Cells(2).Value
                dtgRelatFinan.Rows(4).Cells(2).Value = lscaLitroDia1
                'Calculos Financeiro Custo total / animal dia
                dtgRelatFinan.Rows(5).Cells(2).Value = totalValor

                'Calculos Financeiro Custo total / lote dia
                dtgRelatFinan.Rows(6).Cells(2).Value = totalValor * lblQtA.Text

                'Custo kg na MN
                dtgRelatFinan.Rows(7).Cells(2).Value = totalValor / qtdProduto
                'Custo kg na MS
                dtgRelatFinan.Rows(8).Cells(2).Value = totalValor / qtdMS
                'Custo por litro de leite
                dtgRelatFinan.Rows(9).Cells(2).Value = totalValor / dtgRelatFinan.Rows(0).Cells(2).Value
                'Custo por litro de leite 4%
                leite4pct = 0.4 * dtgRelatFinan.Rows(0).Cells(2).Value + (0.15 * lblGord.Text * dtgRelatFinan.Rows(0).Cells(2).Value)
                dtgRelatFinan.Rows(10).Cells(2).Value = totalValor / leite4pct
                '% do preço do leite
                percleite = (totalValor / dtgRelatFinan.Rows(0).Cells(2).Value / dtgRelatFinan.Rows(1).Cells(2).Value) * 100
                dtgRelatFinan.Rows(11).Cells(2).Value = percleite
                'Relação Leite / Concentrado
                If qtdConcent > 0 Then
                    dtgRelatFinan.Rows(12).Cells(2).Value = dtgRelatFinan.Rows(0).Cells(2).Value / qtdConcent
                    'dtgRelatFinan.Rows(12).Cells(2).Value = dtgRelatFinan.Rows(0).Cells(2).Value / qtdConcent
                Else
                    dtgRelatFinan.Rows(12).Cells(2).Value = 0
                End If

                'Relação Leite / Consumo MS
                dtgRelatFinan.Rows(13).Cells(2).Value = dtgRelatFinan.Rows(0).Cells(2).Value / qtdMS
            End If
            'Else

        End If
    End Sub

    'Calculos na Grid Financeiro Dieta02
    Private Sub CalcularFinan02()
        If dtgRelatFinan.Rows(0).Cells(3).Value = 0 Then
            dtgRelatFinan.Rows(0).Cells(3).Value = leitePrev
        ElseIf dtgRelatFinan.Rows(1).Cells(3).Value = 0 Then
            dtgRelatFinan.Rows(1).Cells(3).Value = leitePrec
        End If

        Dim qtdConcent As Double
        Dim qtdMS As Double

        Dim lscaVacaDiaD2 As Double
        Dim lscaLoteDia2 As Double
        Dim lscaLitroDia2 As Double
        Dim leite4pct As Double
        Dim percleite As Double

        If varms = False Then
            For i As Integer = 0 To dtgAlimentosDieta.Rows.Count - 1
                qtdMS += dtgAlimentosDieta.Rows(i).Cells(68).Value * dtgAlimentosDieta.Rows(i).Cells(4).Value
                If dtgAlimentosDieta.Rows(i).Cells(2).Value.ToString = "Concentrados Energéticos" Or dtgAlimentosDieta.Rows(i).Cells(2).Value.ToString = "Pré-Mistura" Or dtgAlimentosDieta.Rows(i).Cells(2).Value.ToString = "Concentrados Proteicos" Or dtgAlimentosDieta.Rows(i).Cells(2).Value.ToString = "Minerais" Or dtgAlimentosDieta.Rows(i).Cells(2).Value.ToString = "Outros" Then 'Or dtgAlimentosDieta.Rows(i).Cells(1).Value = "Concentrados Proteicos" Then

                    qtdConcent += dtgAlimentosDieta.Rows(i).Cells(68).Value

                End If
            Next
            If qtdProduto2 > 0.001 Then
                On Error Resume Next
                'Calculos Financeiro LSCA /Animal
                lscaVacaDiaD2 = dtgRelatFinan.Rows(0).Cells(3).Value * dtgRelatFinan.Rows(1).Cells(3).Value - totalValor2
                dtgRelatFinan.Rows(2).Cells(3).Value = lscaVacaDiaD2
                'Calculos Financeiro LSCA / Lote
                lscaLoteDia2 = lscaVacaDiaD2 * lblQtA.Text
                dtgRelatFinan.Rows(3).Cells(3).Value = lscaLoteDia2
                'Calculos Financeiro LSCA / Litro
                lscaLitroDia2 = lscaVacaDiaD2 / dtgRelatFinan.Rows(0).Cells(3).Value
                dtgRelatFinan.Rows(4).Cells(3).Value = lscaLitroDia2
                'Calculos Financeiro Custo total
                dtgRelatFinan.Rows(5).Cells(3).Value = totalValor2
                'Calculos Financeiro Custo total / lote dia
                dtgRelatFinan.Rows(6).Cells(3).Value = totalValor2 * lblQtA.Text
                'Custo kg na MN
                dtgRelatFinan.Rows(7).Cells(3).Value = totalValor2 / qtdProduto2
                'Custo kg na MS
                dtgRelatFinan.Rows(8).Cells(3).Value = totalValor2 / qtdMS * 100
                'Custo por litro de leite
                dtgRelatFinan.Rows(9).Cells(3).Value = totalValor2 / dtgRelatFinan.Rows(0).Cells(3).Value
                'Custo por litro de leite 4%
                leite4pct = 0.4 * dtgRelatFinan.Rows(0).Cells(3).Value + (0.15 * lblGord.Text * dtgRelatFinan.Rows(0).Cells(3).Value)
                dtgRelatFinan.Rows(10).Cells(3).Value = totalValor2 / leite4pct
                '% do preço do leite
                percleite = (totalValor2 / dtgRelatFinan.Rows(0).Cells(3).Value / dtgRelatFinan.Rows(1).Cells(3).Value) * 100
                dtgRelatFinan.Rows(11).Cells(3).Value = percleite
                'Relação Leite / Concentrado
                If qtdConcent > 0 Then
                    dtgRelatFinan.Rows(12).Cells(3).Value = dtgRelatFinan.Rows(0).Cells(3).Value / qtdConcent
                    'dtgRelatFinan.Rows(12).Cells(3).Value = dtgRelatFinan.Rows(0).Cells(3).Value / qtdConcent
                Else
                    dtgRelatFinan.Rows(12).Cells(3).Value = 0
                End If
                'Relação Leite / Consumo MS
                dtgRelatFinan.Rows(13).Cells(3).Value = dtgRelatFinan.Rows(0).Cells(3).Value / qtdMS * 100

            ElseIf varms = True Then
                For i As Integer = 0 To dtgAlimentosDieta.Rows.Count - 1
                    qtdMS += dtgAlimentosDieta.Rows(i).Cells(68).Value ' * dtgAlimentosDieta.Rows(i).Cells(3).Value
                    If dtgAlimentosDieta.Rows(i).Cells(2).Value.ToString = "Concentrados Energéticos" Or dtgAlimentosDieta.Rows(i).Cells(2).Value.ToString = "Pré-Mistura" Or dtgAlimentosDieta.Rows(i).Cells(2).Value.ToString = "Concentrados Proteicos" Or dtgAlimentosDieta.Rows(i).Cells(2).Value.ToString = "Minerais" Or dtgAlimentosDieta.Rows(i).Cells(2).Value.ToString = "Outros" Then 'Or dtgAlimentosDieta.Rows(i).Cells(1).Value = "Concentrados Proteicos" Then

                        qtdConcent += dtgAlimentosDieta.Rows(i).Cells(68).Value / dtgAlimentosDieta.Rows(i).Cells(4).Value * 100

                    End If
                Next
                On Error Resume Next
                'Calculos Financeiro LSCA /Animal
                lscaVacaDiaD2 = dtgRelatFinan.Rows(0).Cells(3).Value * dtgRelatFinan.Rows(1).Cells(3).Value - totalValor2
                dtgRelatFinan.Rows(2).Cells(3).Value = lscaVacaDiaD2
                'Calculos Financeiro LSCA / Lote
                lscaLoteDia2 = lscaVacaDiaD2 * lblQtA.Text
                dtgRelatFinan.Rows(3).Cells(3).Value = lscaLoteDia2
                'Calculos Financeiro LSCA / Litro
                lscaLitroDia2 = lscaVacaDiaD2 / dtgRelatFinan.Rows(0).Cells(3).Value
                dtgRelatFinan.Rows(4).Cells(3).Value = lscaLitroDia2
                'Calculos Financeiro Custo total
                dtgRelatFinan.Rows(5).Cells(3).Value = totalValor2
                'Calculos Financeiro Custo total / lote dia
                dtgRelatFinan.Rows(6).Cells(3).Value = totalValor2 * lblQtA.Text
                'Custo kg na MN
                dtgRelatFinan.Rows(7).Cells(3).Value = totalValor2 / qtdProduto2
                'Custo kg na MS
                dtgRelatFinan.Rows(8).Cells(3).Value = totalValor2 / qtdMS
                'Custo por litro de leite
                dtgRelatFinan.Rows(9).Cells(3).Value = totalValor2 / dtgRelatFinan.Rows(0).Cells(3).Value
                'Custo por litro de leite 4%
                leite4pct = 0.4 * dtgRelatFinan.Rows(0).Cells(3).Value + (0.15 * lblGord.Text * dtgRelatFinan.Rows(0).Cells(3).Value)
                dtgRelatFinan.Rows(10).Cells(3).Value = totalValor2 / leite4pct
                '% do preço do leite
                percleite = (totalValor2 / dtgRelatFinan.Rows(0).Cells(3).Value / dtgRelatFinan.Rows(1).Cells(3).Value) * 100
                dtgRelatFinan.Rows(11).Cells(3).Value = percleite
                'Relação Leite / Concentrado
                If qtdConcent > 0 Then
                    dtgRelatFinan.Rows(12).Cells(3).Value = dtgRelatFinan.Rows(0).Cells(3).Value / qtdConcent
                    'dtgRelatFinan.Rows(12).Cells(3).Value = dtgRelatFinan.Rows(0).Cells(3).Value / qtdConcent
                Else
                    dtgRelatFinan.Rows(12).Cells(3).Value = 0
                End If
                'Relação Leite / Consumo MS
                dtgRelatFinan.Rows(13).Cells(3).Value = dtgRelatFinan.Rows(0).Cells(3).Value / qtdMS

            End If
        End If
    End Sub
    'VER A NESCESSIDADE DESTE
    Private Sub dtgRelatFinan_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dtgRelatFinan.CellEndEdit
        CalcularFinan01()
        CalcularFinan02()
        Dim leite1 As Double
        Dim leite2 As Double
        Dim prod1 As Double
        Dim prod2 As Double

        prod1 = dtgRelatFinan.Rows(0).Cells(2).Value
        leite1 = dtgRelatFinan.Rows(1).Cells(2).Value
        prod2 = dtgRelatFinan.Rows(0).Cells(3).Value
        leite2 = dtgRelatFinan.Rows(1).Cells(3).Value

        dtgRelatFinan.Rows(0).Cells(2).Value = Format(Math.Round(prod1, 2), "0.00")
        dtgRelatFinan.Rows(1).Cells(2).Value = Format(Math.Round(leite1, 2), "0.00")
        dtgRelatFinan.Rows(0).Cells(3).Value = Format(Math.Round(prod2, 2), "0.00")
        dtgRelatFinan.Rows(1).Cells(3).Value = Format(Math.Round(leite2, 2), "0.00")

    End Sub

    'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX      CALCULOS DA DIETA NA MS     XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
    Dim vr67(15) As Double
    Dim vr68(15) As Double
    'Sub para zerar as vars para calculos das dieta1 e dieta2
    Private Sub ZerarTotais()
        totalMS = 0
        totalPB = 0
        totalPDR = 0
        totalPND = 0
        totalFDN = 0
        totaleFDN = 0
        'totaleFDN2 = 0
        totalMNmaior8 = 0
        totalMNmaior19 = 0
        totalFDNF = 0
        totalFDA = 0
        totalNEl = 0
        totalNDT = 0
        totalEE = 0
        totalEE_Insat = 0
        totalCinzas = 0
        totalCNF = 0
        totalAmido = 0
        totalkd_Amid = 0
        totalMor = 0
        totalCa = 0
        totalP = 0
        totalMg = 0
        totalK = 0
        totalS = 0
        totalNa = 0
        totalCl = 0
        totalCo = 0
        totalCu = 0
        totalMn = 0
        totalZn = 0
        totalSe = 0
        totalI = 0
        totalA = 0
        totalD = 0
        totalE = 0
        totalCromo = 0
        'totalDCAD = 0
        totalBiotina = 0
        totalVirginiamicina = 0
        totalMonensina = 0
        totalLevedura = 0
        totalArginina = 0
        totalHistidina = 0
        totalIsoleucina = 0
        totalLeucina = 0
        totalLisina = 0
        totalMetionina = 0
        totalFenilalanina = 0
        totalTreonina = 0
        totalTriptofano = 0
        totalValina = 0
        totaldFDNp48h = 0
        totaldAmido7h = 0
        totalTTNDFD = 0

        somaProduto = 0

        mn8AmiDR = 0
        mn8PV = 0
        dfnfPV = 0
        forragem = 0
        concentrado = 0
        dcad = 0
        consumo = 0
        caP = 0
        lysMet = 0
        enerProdLeite = 0
        ProtPrudLeite = 0

        amiDR = 0
        kc = 0
        msConc = 0
        msVol = 0
        kcVol = 0
        kcConc = 0
        qtdAmid = 0
        mnMaiorq8Dieta = 0
        msConc1 = 0
        qamidr = 0

        qtdNel = 0
        kgTotalMS = 0

    End Sub
    'dieta1 na MS
    Private Sub CalcularDieta01MS()

        ZerarTotais()
        Dim estimatLeiteLact As Double
        Dim estimatLeite As Double

        Try

            '=(1,169+(1,375*11,35)/650*100)+(1,721*(5,46)/650*100)
            For Each row As DataGridViewRow In dtgAlimentosDieta.Rows

                qtdKgMs = row.Cells(67).Value ' / row.Cells(3).Value ' * 100 ' ok
                kgTotalMS += qtdKgMs
                totalMS += row.Cells(67).Value / row.Cells(4).Value
                totalPB += row.Cells(5).Value * qtdKgMs
                totalPDR += row.Cells(6).Value * qtdKgMs
                totalPND += row.Cells(7).Value * qtdKgMs
                totalFDN += row.Cells(8).Value * qtdKgMs
                totaleFDN += row.Cells(9).Value * qtdKgMs

                totalMNmaior8 += row.Cells(10).Value * qtdKgMs '11
                totalMNmaior19 += row.Cells(11).Value * qtdKgMs '12
                totalFDNF += row.Cells(12).Value * qtdKgMs
                totalFDA += row.Cells(13).Value * qtdKgMs
                totalNEl += row.Cells(14).Value * qtdKgMs

                qtdNel += row.Cells(14).Value * row.Cells(67).Value ' * row.Cells(4).Value

                totalNDT += row.Cells(15).Value * qtdKgMs
                totalEE += row.Cells(16).Value * qtdKgMs
                totalEE_Insat += row.Cells(17).Value * qtdKgMs
                totalCinzas += row.Cells(18).Value * qtdKgMs
                totalCNF += row.Cells(19).Value * qtdKgMs
                totalAmido += row.Cells(20).Value * qtdKgMs
                totalkd_Amid += row.Cells(21).Value * qtdKgMs
                totalMor += row.Cells(22).Value * qtdKgMs

                totalCa += row.Cells(23).Value * qtdKgMs
                totalP += row.Cells(24).Value * qtdKgMs
                totalMg += row.Cells(25).Value * qtdKgMs
                totalK += row.Cells(26).Value * qtdKgMs
                totalS += row.Cells(27).Value * qtdKgMs
                totalNa += row.Cells(28).Value * qtdKgMs
                totalCl += row.Cells(29).Value * qtdKgMs
                totalCo += row.Cells(30).Value * qtdKgMs
                totalCu += row.Cells(31).Value * qtdKgMs
                totalMn += row.Cells(32).Value * qtdKgMs
                totalZn += row.Cells(33).Value * qtdKgMs
                totalSe += row.Cells(34).Value * qtdKgMs
                totalI += row.Cells(35).Value * qtdKgMs
                totalA += row.Cells(36).Value * qtdKgMs
                totalD += row.Cells(37).Value * qtdKgMs
                totalE += row.Cells(38).Value * qtdKgMs
                totalCromo += row.Cells(39).Value * qtdKgMs

                totalBiotina += row.Cells(40).Value * qtdKgMs
                totalVirginiamicina += row.Cells(41).Value * qtdKgMs
                totalMonensina += row.Cells(42).Value * qtdKgMs
                totalLevedura += row.Cells(43).Value * qtdKgMs
                totalArginina += row.Cells(44).Value * qtdKgMs
                totalHistidina += row.Cells(45).Value * qtdKgMs
                totalIsoleucina += row.Cells(46).Value * qtdKgMs
                totalLeucina += row.Cells(47).Value * qtdKgMs
                totalLisina += row.Cells(48).Value * qtdKgMs
                totalMetionina += row.Cells(49).Value * qtdKgMs
                totalFenilalanina += row.Cells(50).Value * qtdKgMs
                totalTreonina += row.Cells(51).Value * qtdKgMs
                totalTriptofano += row.Cells(52).Value * qtdKgMs
                totalValina += row.Cells(53).Value * qtdKgMs
                totaldFDNp48h += row.Cells(54).Value * qtdKgMs
                totaldAmido7h += row.Cells(55).Value * qtdKgMs
                totalTTNDFD += row.Cells(56).Value * qtdKgMs
                'End If
                If dtgAlimentosDieta.Rows(0).Cells(67).Value = 0 Then
                    dtgAlimentosDieta.Rows(0).Cells(67).Value = 0.00001
                End If
                Dim msVol1 As Double = 0

                ' XXXXXXXXXXXXXXXXXXXXXXXXXXXXX        Inicio de calculos para obter o valor do avaliador MN>8/AmiDR   XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                If lblPV.Text = "" Or lblPV.Text = 0 Then
                    lblPV.Text = 1
                End If
                'Separar MS de Volumoso e Concentrado necessário para consguir o valor de KC
                If row.Cells(2).Value.ToString = "Gramíneas e Leguminosas" Or row.Cells(2).Value.ToString = "Silagens" Then
                    msVol += row.Cells(67).Value * 100
                    kcVol = ((1.375 * msVol) / lblPV.Text)
                    'Para calculo do amido
                    msVol1 = row.Cells(67).Value

                ElseIf row.Cells(2).Value.ToString = "Concentrados Energéticos" Or row.Cells(2).Value.ToString = "Pré-Mistura" Or row.Cells(2).Value.ToString = "Concentrados Proteicos" Or row.Cells(2).Value.ToString = "Minerais" Or row.Cells(2).Value.ToString = "Outros" Then
                    msConc += row.Cells(67).Value * 100
                    kcConc = ((1.721 * msConc) / lblPV.Text)
                    'Para calculo do amido
                    msConc1 = row.Cells(67).Value

                End If

                kc = 1.169 + (kcVol + kcConc)

                '%amido * kg ms
                qtdAmid = row.Cells(20).Value * qtdKgMs ' ok

                'AmiDR = kd amid/(kd amid + kc)*qtd de amido
                qamidr += row.Cells(21).Value / (row.Cells(21).Value + kc) * qtdAmid 'ok
                amiDR = qamidr / totalMS 'ok

                'qamiddr = qamidr * 100

                'MN>8/AmiDR = MN >8 da dieta/ AmiDR da dieta
                'para obter o MN>8 da dieta

                'Se for volumoso
                '((((0,478*0,771*100)*0,9465)+4,5798)/100)*qtd de ms de volumoso
                Dim mn8Vol As Double
                mn8Vol += ((((0.478 * 0.771 * 100) * 0.9465) + 4.5798) / 100) * msVol1  ' ok

                'Se for concentrado
                'QTD CONCENTRADO * % DE MN>8
                Dim mn8Conc As Double
                mn8Conc += msConc1 * row.Cells(10).Value  ' ok
                'MN>8 = % MN>8
                Dim mnM8 As Double

                mnM8 = totalMNmaior8

                'mnM8 = (mn8Vol + mn8Conc) ' ok
                'mnMaiorq8Dieta = mnM8 / totalMS
                mn8AmiDR = mnM8 / qamidr

                'Label5.Text = qamidr
                'Label4.Text = kc
                'Label6.Text = mn8AmiDR
                ' XXXXXXXXXXXXXXXXXXXXXXXXXXXXX        Inicio de calculos para obter o valor do avaliador MN>8 % do PV   XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                'mn8PV = mnM8 / lblPV.Text
                mn8PV = totalMNmaior8 / lblPV.Text

                ' XXXXXXXXXXXXXXXXXXXXXXXXXXXXX        Inicio de calculos para obter o valor do avaliador FDNF % do PV   XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                dfnfPV = totalFDNF / lblPV.Text

                ' XXXXXXXXXXXXXXXXXXXXXXXXXXXXX        Inicio de calculos para obter o valor do avaliador Forragem       XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                forragem = msVol / kgTotalMS / 100
                ' XXXXXXXXXXXXXXXXXXXXXXXXXXXXX        Inicio de calculos para obter o valor do avaliador Concentrado       XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

                concentrado = msConc / kgTotalMS / 100

                ' XXXXXXXXXXXXXXXXXXXXXXXXXXXXX        Inicio de calculos para obter o valor do avaliador    DCAD       XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                'Sódio*100/0.023+Potássio*100/0.039)-(Cloro*100/0.0355+Enxofre*100/0.016
                '((((Na*100/0,023+k*100/0,039)-(Ci*100/0,0355+S*100/0,016))))
                Dim pctNA As Double
                Dim pctK As Double
                Dim pctCi As Double
                Dim pctS As Double

                pctNA = totalNa / kgTotalMS
                pctK = totalK / kgTotalMS
                pctCi = totalCl / kgTotalMS
                pctS = totalS / kgTotalMS

                dcad = ((((pctNA * 100 / 0.023 + pctK * 100 / 0.039) - (pctCi * 100 / 0.0355 + pctS * 100 / 0.016)))) / 100

                ' XXXXXXXXXXXXXXXXXXXXXXXXXXXXX        Inicio de calculos para obter o valor do avaliador    Consumo Total       XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                somaProduto += row.Cells(67).Value / row.Cells(4).Value * 100
                consumo = somaProduto

                '' XXXXXXXXXXXXXXXXXXXXXXXXXXXXX        Inicio de calculos para obter o valor do avaliador    Ca/P         XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

                'Dim pctCa As Double
                'Dim pctP As Double

                'pctCa = totalCa / totalMS
                'pctP = totalP / totalMS
                'caP = pctCa / pctP

                '' XXXXXXXXXXXXXXXXXXXXXXXXXXXXX        Inicio de calculos para obter o valor do avaliador    Lys / Met        XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                'Dim pctLys As Double
                'Dim pctMet As Double

                'pctLys = totalLisina / totalMS
                'pctMet = totalMetionina / totalMS
                'lysMet = pctLys / pctMet
                ' XXXXXXXXXXXXXXXXXXXXXXXXXXXXX        Inicio de calculos para obter o valor do avaliador    Ca/P         XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                Dim pctCa As Double
                Dim pctP As Double

                pctCa = totalCa / totalMS
                pctP = totalP / totalMS

                Dim vcap As Double
                vcap = pctCa / pctP
                If vcap > 0 Then
                    caP = vcap
                Else
                    caP = 0
                End If

                ' XXXXXXXXXXXXXXXXXXXXXXXXXXXXX        Inicio de calculos para obter o valor do avaliador    Lys / Met        XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                Dim pctLys As Double
                Dim pctMet As Double

                pctLys = totalLisina / totalMS
                pctMet = totalMetionina / totalMS
                Dim vlysmet As Double
                vlysmet = pctLys / pctMet
                If vlysmet > 0 Then
                    lysMet = vlysmet
                Else
                    lysMet = 0
                End If
                ' XXXXXXXXXXXXXXXXXXXXXXXXXXXXX        Inicio de calculos para obter o valor do avaliador    Energia produção de leite      XXXXXXXXXXXXXXXXXXXXXXX
                Dim energMtc As Double
                Dim energLac As Double
                Dim energLacComLact As Double
                Dim fatorCr As Double

                If lblGord.Text = "" Then
                    lblGord.Text = 0
                ElseIf lblProt.Text = "" Then
                    lblProt.Text = 0
                ElseIf lblLact.Text = "" Then
                    lblLact.Text = 0
                End If
                energMtc = (lblPV.Text ^ 0.75) * 0.08 'Energia de mantença	mcal/ dia	(PV elevado a 0.75) * 0.08
                energLac = (0.0929 * lblGord.Text) + ((0.0547 * lblProt.Text) + 0.192) 'Energia de lactação	mcal/ dia 0.0929*% de gordura +0.0547*% de proteína +0.192)
                energLacComLact = (0.0929 * lblGord.Text) + (0.0547 * lblProt.Text) + (0.0395 * lblLact.Text) 'Energia de lactação com lactose	mcal/ dia	0.0929*% de gordura +0.0547*% de proteína +0.0395* % de lactose

                Dim pctNel As Double
                pctNel = qtdNel ' / totalMS * 100
                'Fator de Correção FL		
                'Se o NEL da dieta for		
                '15 - 20	    10	
                '20.01 - 25	    7	
                '25.01 - 30	    5	
                '30.01 - 35	    -2	
                '35.01 - 40	    -7	
                '40.01 - 45	    -10	
                '>45	-12	
                If pctNel > 15 And pctNel <= 20 Then
                    fatorCr = 10
                ElseIf pctNel > 20 And pctNel <= 25 Then
                    fatorCr = 7
                ElseIf pctNel > 25 And pctNel <= 30 Then
                    fatorCr = 5
                ElseIf pctNel > 30 And pctNel <= 35 Then
                    fatorCr = -2
                ElseIf pctNel > 35 And pctNel <= 40 Then
                    fatorCr = -7
                ElseIf pctNel > 40 And pctNel <= 45 Then
                    fatorCr = -10
                ElseIf pctNel > 45 Then
                    fatorCr = -12
                ElseIf pctNel <= 15 Then
                    fatorCr = 15
                End If

                Dim el As Double
                Dim telpc As Double
                el = (pctNel - energMtc) / (energLac) ' + fatorCr) 'Estimatina prd leite EL	Kg/ dia	(Nel da dieta - Energia de mantença) / energia lactação  + fator de coreção FL

                'sem lactose
                telpc = el / 100 * fatorCr
                estimatLeite = el + telpc

                'com lactose
                Dim elcLact As Double
                Dim elc As Double
                elcLact = (pctNel - energMtc) / (energLacComLact) ' + fatorCr) 'Estimatina prd leite EL Lactose	Kg/ dia	(Nel da dieta - Energia de mantença com lactose) / energia lactação  + fator de coreção FL
                elc = elcLact / 100 * fatorCr


                estimatLeiteLact = elcLact + elc

                'xxxxxxxxxxxxxxxxxxxxxxx

                'xxxxxxxxxxxxxxxxxxxxxx

            Next
            'If dtgAlimentosDieta.Rows(0).Cells(67).Value = 0 Then
            '    dtgAlimentosDieta.Rows(0).Cells(67).Value = 0.00001
            'End If
            Dim qamiddr As Double
            qamiddr = qamidr


            ' XXXXXXXXXXXXXXXXXXXXXXXXXXXXX        Inicio de calculos para obter o valor do avaliador    Proteina produção de leite      XXXXXXXXXXXXXXXXXXXXXXX
            Dim ptnMet As Double

            'PTN Met =PNDR + (PDR*0.4772)

            'Proteína disponível para leite: PTN Metabolizável * Fator 

            'Fator: Até 26 kg = 76.46409    26 a 40= 69.1698     Acima de 40= 66.65636
            Dim fator As Double

            If lblLeite.Text < 26 Then
                fator = 76.46409
            ElseIf lblLeite.Text >= 26 And lblLeite.Text <= 40 Then
                fator = 69.1698
            ElseIf lblLeite.Text > 40 Then
                fator = 66.65636
            End If
            ptnMet = ((totalPND + (totalPDR * 0.4772)) / 100) * 1000
            ProtPrudLeite = ptnMet / fator

            'ptnMet = (totalPND + (totalPDR * 0.4772))
            'ProtPrudLeite = ptnMet / fator
            'calcular quando a primeira linha do dtgalimentosdieta estiver 0
            If qtdProduto > 0.0001 Then

                dtgAvaliadores.Rows(0).Cells(11).Value = kgTotalMS  ' antes de % ms
                dtgAvaliadores.Rows(1).Cells(11).Value = kgTotalMS / totalMS  ' / somaProduto 
                dtgAvaliadores.Rows(2).Cells(11).Value = totalPB / kgTotalMS
                dtgAvaliadores.Rows(3).Cells(11).Value = totalPDR / kgTotalMS
                dtgAvaliadores.Rows(4).Cells(11).Value = totalPND / kgTotalMS
                dtgAvaliadores.Rows(5).Cells(11).Value = totalFDN / kgTotalMS
                dtgAvaliadores.Rows(6).Cells(11).Value = totaleFDN / kgTotalMS

                dtgAvaliadores.Rows(7).Cells(11).Value = totalMNmaior8 / kgTotalMS
                dtgAvaliadores.Rows(8).Cells(11).Value = totalMNmaior19 / kgTotalMS
                dtgAvaliadores.Rows(9).Cells(11).Value = totalFDNF / kgTotalMS
                dtgAvaliadores.Rows(10).Cells(11).Value = totalFDA / kgTotalMS
                dtgAvaliadores.Rows(11).Cells(11).Value = totalNEl / kgTotalMS
                dtgAvaliadores.Rows(12).Cells(11).Value = totalNDT / kgTotalMS
                dtgAvaliadores.Rows(13).Cells(11).Value = totalEE / kgTotalMS
                dtgAvaliadores.Rows(14).Cells(11).Value = totalEE_Insat / kgTotalMS
                dtgAvaliadores.Rows(15).Cells(11).Value = totalCinzas / kgTotalMS
                dtgAvaliadores.Rows(16).Cells(11).Value = totalCNF / kgTotalMS
                dtgAvaliadores.Rows(17).Cells(11).Value = totalAmido / kgTotalMS
                dtgAvaliadores.Rows(18).Cells(11).Value = qamiddr / kgTotalMS  'xxxxxxxxxxxxxxxx

                dtgAvaliadores.Rows(19).Cells(11).Value = totalMor / kgTotalMS
                dtgAvaliadores.Rows(20).Cells(11).Value = totalCa / kgTotalMS
                dtgAvaliadores.Rows(21).Cells(11).Value = totalP / kgTotalMS
                dtgAvaliadores.Rows(22).Cells(11).Value = totalMg / kgTotalMS
                dtgAvaliadores.Rows(23).Cells(11).Value = totalK / kgTotalMS
                dtgAvaliadores.Rows(24).Cells(11).Value = totalS / kgTotalMS
                dtgAvaliadores.Rows(25).Cells(11).Value = totalNa / kgTotalMS
                dtgAvaliadores.Rows(26).Cells(11).Value = totalCl / kgTotalMS
                dtgAvaliadores.Rows(27).Cells(11).Value = totalCo / kgTotalMS
                dtgAvaliadores.Rows(28).Cells(11).Value = totalCu / kgTotalMS
                dtgAvaliadores.Rows(29).Cells(11).Value = totalMn / kgTotalMS
                dtgAvaliadores.Rows(30).Cells(11).Value = totalZn / kgTotalMS
                dtgAvaliadores.Rows(31).Cells(11).Value = totalSe / kgTotalMS
                dtgAvaliadores.Rows(32).Cells(11).Value = totalI / kgTotalMS
                dtgAvaliadores.Rows(33).Cells(11).Value = totalA / kgTotalMS
                dtgAvaliadores.Rows(34).Cells(11).Value = totalD / kgTotalMS
                dtgAvaliadores.Rows(35).Cells(11).Value = totalE / kgTotalMS
                dtgAvaliadores.Rows(36).Cells(11).Value = totalCromo / kgTotalMS

                dtgAvaliadores.Rows(37).Cells(11).Value = totalBiotina / kgTotalMS
                dtgAvaliadores.Rows(38).Cells(11).Value = totalVirginiamicina / kgTotalMS
                dtgAvaliadores.Rows(39).Cells(11).Value = totalMonensina / kgTotalMS
                dtgAvaliadores.Rows(40).Cells(11).Value = totalLevedura / kgTotalMS
                dtgAvaliadores.Rows(41).Cells(11).Value = totalArginina / kgTotalMS
                dtgAvaliadores.Rows(42).Cells(11).Value = totalHistidina / kgTotalMS
                dtgAvaliadores.Rows(43).Cells(11).Value = totalIsoleucina / kgTotalMS
                dtgAvaliadores.Rows(44).Cells(11).Value = totalLeucina / kgTotalMS
                dtgAvaliadores.Rows(45).Cells(11).Value = totalLisina / kgTotalMS
                dtgAvaliadores.Rows(46).Cells(11).Value = totalMetionina / kgTotalMS
                dtgAvaliadores.Rows(47).Cells(11).Value = totalFenilalanina / kgTotalMS
                dtgAvaliadores.Rows(48).Cells(11).Value = totalTreonina / kgTotalMS
                dtgAvaliadores.Rows(49).Cells(11).Value = totalTriptofano / kgTotalMS
                dtgAvaliadores.Rows(50).Cells(11).Value = totalValina / kgTotalMS
                dtgAvaliadores.Rows(51).Cells(11).Value = totaldFDNp48h / totalMS
                dtgAvaliadores.Rows(52).Cells(11).Value = totaldAmido7h / totalMS
                dtgAvaliadores.Rows(53).Cells(11).Value = totalTTNDFD / totalMS

                dtgAvaliadores.Rows(54).Cells(11).Value = mn8AmiDR
                dtgAvaliadores.Rows(55).Cells(11).Value = mn8PV
                dtgAvaliadores.Rows(56).Cells(11).Value = dfnfPV
                dtgAvaliadores.Rows(57).Cells(11).Value = forragem * 100
                dtgAvaliadores.Rows(58).Cells(11).Value = concentrado * 100
                dtgAvaliadores.Rows(59).Cells(11).Value = dcad
                dtgAvaliadores.Rows(60).Cells(11).Value = consumo
                dtgAvaliadores.Rows(61).Cells(11).Value = caP
                dtgAvaliadores.Rows(62).Cells(11).Value = lysMet

                dtgAvaliadores.Rows(63).Cells(11).Value = estimatLeite
                dtgAvaliadores.Rows(64).Cells(11).Value = estimatLeiteLact
                dtgAvaliadores.Rows(65).Cells(11).Value = ProtPrudLeite
                'calcular quando a primeira linha do dtgalimentosdieta estiver 0

            Else
                For Each row As DataGridViewRow In dtgAvaliadores.Rows
                    row.Cells(11).Value = 0
                Next

            End If
            'mn8AmiDR ='MN >8 da dieta/ AmiDR da dieta


        Catch exc As DivideByZeroException
            Console.WriteLine("Erro: Divisão por zero")
        Catch exc As OverflowException
            Console.WriteLine("Erro: Overflow")
        Finally
            Console.ReadLine()
        End Try

        If My.Settings.corAvalOnOf = True Then
            CorAval()
        End If

    End Sub
    'dieta2 na MS
    Private Sub CalcularDieta02MS()

        ZerarTotais()
        Dim estimatLeiteLact As Double
        Dim estimatLeite As Double

        Try

            '=(1,169+(1,375*11,35)/650*100)+(1,721*(5,46)/650*100)
            For Each row As DataGridViewRow In dtgAlimentosDieta.Rows

                qtdKgMs = row.Cells(68).Value ' / row.Cells(3).Value ' * 100 ' ok
                kgTotalMS += qtdKgMs
                totalMS += row.Cells(68).Value / row.Cells(4).Value
                totalPB += row.Cells(5).Value * qtdKgMs
                totalPDR += row.Cells(6).Value * qtdKgMs
                totalPND += row.Cells(7).Value * qtdKgMs
                totalFDN += row.Cells(8).Value * qtdKgMs
                totaleFDN += row.Cells(9).Value * qtdKgMs

                totalMNmaior8 += row.Cells(10).Value * qtdKgMs '11
                totalMNmaior19 += row.Cells(11).Value * qtdKgMs '12
                totalFDNF += row.Cells(12).Value * qtdKgMs
                totalFDA += row.Cells(13).Value * qtdKgMs
                totalNEl += row.Cells(14).Value * qtdKgMs

                qtdNel += row.Cells(14).Value * row.Cells(68).Value ' * row.Cells(4).Value

                totalNDT += row.Cells(15).Value * qtdKgMs
                totalEE += row.Cells(16).Value * qtdKgMs
                totalEE_Insat += row.Cells(17).Value * qtdKgMs
                totalCinzas += row.Cells(18).Value * qtdKgMs
                totalCNF += row.Cells(19).Value * qtdKgMs
                totalAmido += row.Cells(20).Value * qtdKgMs
                totalkd_Amid += row.Cells(21).Value * qtdKgMs
                totalMor += row.Cells(22).Value * qtdKgMs

                totalCa += row.Cells(23).Value * qtdKgMs
                totalP += row.Cells(24).Value * qtdKgMs
                totalMg += row.Cells(25).Value * qtdKgMs
                totalK += row.Cells(26).Value * qtdKgMs
                totalS += row.Cells(27).Value * qtdKgMs
                totalNa += row.Cells(28).Value * qtdKgMs
                totalCl += row.Cells(29).Value * qtdKgMs
                totalCo += row.Cells(30).Value * qtdKgMs
                totalCu += row.Cells(31).Value * qtdKgMs
                totalMn += row.Cells(32).Value * qtdKgMs
                totalZn += row.Cells(33).Value * qtdKgMs
                totalSe += row.Cells(34).Value * qtdKgMs
                totalI += row.Cells(35).Value * qtdKgMs
                totalA += row.Cells(36).Value * qtdKgMs
                totalD += row.Cells(37).Value * qtdKgMs
                totalE += row.Cells(38).Value * qtdKgMs
                totalCromo += row.Cells(39).Value * qtdKgMs

                totalBiotina += row.Cells(40).Value * qtdKgMs
                totalVirginiamicina += row.Cells(41).Value * qtdKgMs
                totalMonensina += row.Cells(42).Value * qtdKgMs
                totalLevedura += row.Cells(43).Value * qtdKgMs
                totalArginina += row.Cells(44).Value * qtdKgMs
                totalHistidina += row.Cells(45).Value * qtdKgMs
                totalIsoleucina += row.Cells(46).Value * qtdKgMs
                totalLeucina += row.Cells(47).Value * qtdKgMs
                totalLisina += row.Cells(48).Value * qtdKgMs
                totalMetionina += row.Cells(49).Value * qtdKgMs
                totalFenilalanina += row.Cells(50).Value * qtdKgMs
                totalTreonina += row.Cells(51).Value * qtdKgMs
                totalTriptofano += row.Cells(52).Value * qtdKgMs
                totalValina += row.Cells(53).Value * qtdKgMs
                totaldFDNp48h += row.Cells(54).Value * qtdKgMs
                totaldAmido7h += row.Cells(55).Value * qtdKgMs
                totalTTNDFD += row.Cells(56).Value * qtdKgMs
                'End If
                If dtgAlimentosDieta.Rows(0).Cells(68).Value = 0 Then
                    dtgAlimentosDieta.Rows(0).Cells(68).Value = 0.00001
                End If
                Dim msVol1 As Double = 0

                ' XXXXXXXXXXXXXXXXXXXXXXXXXXXXX        Inicio de calculos para obter o valor do avaliador MN>8/AmiDR   XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                If lblPV.Text = "" Or lblPV.Text = 0 Then
                    lblPV.Text = 1
                End If
                'Separar MS de Volumoso e Concentrado necessário para consguir o valor de KC
                If row.Cells(2).Value.ToString = "Gramíneas e Leguminosas" Or row.Cells(2).Value.ToString = "Silagens" Then
                    msVol += row.Cells(68).Value * 100
                    kcVol = ((1.375 * msVol) / lblPV.Text)
                    'Para calculo do amido
                    msVol1 = row.Cells(68).Value

                ElseIf row.Cells(2).Value.ToString = "Concentrados Energéticos" Or row.Cells(2).Value.ToString = "Pré-Mistura" Or row.Cells(2).Value.ToString = "Concentrados Proteicos" Or row.Cells(2).Value.ToString = "Minerais" Or row.Cells(2).Value.ToString = "Outros" Then
                    msConc += row.Cells(68).Value * 100
                    kcConc = ((1.721 * msConc) / lblPV.Text)
                    'Para calculo do amido
                    msConc1 = row.Cells(68).Value

                End If

                kc = 1.169 + (kcVol + kcConc)

                '%amido * kg ms
                qtdAmid = row.Cells(20).Value * qtdKgMs ' ok

                'AmiDR = kd amid/(kd amid + kc)*qtd de amido
                qamidr += row.Cells(21).Value / (row.Cells(21).Value + kc) * qtdAmid 'ok
                amiDR = qamidr / totalMS 'ok

                'qamiddr = qamidr * 100

                'MN>8/AmiDR = MN >8 da dieta/ AmiDR da dieta
                'para obter o MN>8 da dieta

                'Se for volumoso
                '((((0,478*0,771*100)*0,9465)+4,5798)/100)*qtd de ms de volumoso
                Dim mn8Vol As Double
                mn8Vol += ((((0.478 * 0.771 * 100) * 0.9465) + 4.5798) / 100) * msVol1  ' ok

                'Se for concentrado
                'QTD CONCENTRADO * % DE MN>8
                Dim mn8Conc As Double
                mn8Conc += msConc1 * row.Cells(10).Value  ' ok
                'MN>8 = % MN>8
                Dim mnM8 As Double

                mnM8 = totalMNmaior8

                'mnM8 = (mn8Vol + mn8Conc) ' ok
                'mnMaiorq8Dieta = mnM8 / totalMS
                mn8AmiDR = mnM8 / qamidr

                'Label5.Text = qamidr
                'Label4.Text = kc
                'Label6.Text = mn8AmiDR
                ' XXXXXXXXXXXXXXXXXXXXXXXXXXXXX        Inicio de calculos para obter o valor do avaliador MN>8 % do PV   XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                'mn8PV = mnM8 / lblPV.Text
                mn8PV = totalMNmaior8 / lblPV.Text

                ' XXXXXXXXXXXXXXXXXXXXXXXXXXXXX        Inicio de calculos para obter o valor do avaliador FDNF % do PV   XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                dfnfPV = totalFDNF / lblPV.Text

                ' XXXXXXXXXXXXXXXXXXXXXXXXXXXXX        Inicio de calculos para obter o valor do avaliador Forragem       XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                forragem = msVol / kgTotalMS / 100
                ' XXXXXXXXXXXXXXXXXXXXXXXXXXXXX        Inicio de calculos para obter o valor do avaliador Concentrado       XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

                concentrado = msConc / kgTotalMS / 100

                ' XXXXXXXXXXXXXXXXXXXXXXXXXXXXX        Inicio de calculos para obter o valor do avaliador    DCAD       XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                'Sódio*100/0.023+Potássio*100/0.039)-(Cloro*100/0.0355+Enxofre*100/0.016
                '((((Na*100/0,023+k*100/0,039)-(Ci*100/0,0355+S*100/0,016))))
                Dim pctNA As Double
                Dim pctK As Double
                Dim pctCi As Double
                Dim pctS As Double

                pctNA = totalNa / kgTotalMS
                pctK = totalK / kgTotalMS
                pctCi = totalCl / kgTotalMS
                pctS = totalS / kgTotalMS

                dcad = ((((pctNA * 100 / 0.023 + pctK * 100 / 0.039) - (pctCi * 100 / 0.0355 + pctS * 100 / 0.016)))) / 100

                ' XXXXXXXXXXXXXXXXXXXXXXXXXXXXX        Inicio de calculos para obter o valor do avaliador    Consumo Total       XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                somaProduto += row.Cells(68).Value / row.Cells(4).Value * 100
                consumo = somaProduto
                ' XXXXXXXXXXXXXXXXXXXXXXXXXXXXX        Inicio de calculos para obter o valor do avaliador    Ca/P         XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                Dim pctCa As Double
                Dim pctP As Double

                pctCa = totalCa / totalMS
                pctP = totalP / totalMS

                Dim vcap As Double
                vcap = pctCa / pctP
                If vcap > 0 Then
                    caP = vcap
                Else
                    caP = 0
                End If

                ' XXXXXXXXXXXXXXXXXXXXXXXXXXXXX        Inicio de calculos para obter o valor do avaliador    Lys / Met        XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                Dim pctLys As Double
                Dim pctMet As Double

                pctLys = totalLisina / totalMS
                pctMet = totalMetionina / totalMS
                Dim vlysmet As Double
                vlysmet = pctLys / pctMet
                If vlysmet > 0 Then
                    lysMet = vlysmet
                Else
                    lysMet = 0
                End If
                '' XXXXXXXXXXXXXXXXXXXXXXXXXXXXX        Inicio de calculos para obter o valor do avaliador    Ca/P         XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                'Dim pctCa As Double
                'Dim pctP As Double

                'pctCa = totalCa / totalMS
                'pctP = totalP / totalMS
                'caP = pctCa / pctP

                '' XXXXXXXXXXXXXXXXXXXXXXXXXXXXX        Inicio de calculos para obter o valor do avaliador    Lys / Met        XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                'Dim pctLys As Double
                'Dim pctMet As Double

                'pctLys = totalLisina / totalMS
                'pctMet = totalMetionina / totalMS
                'lysMet = pctLys / pctMet

                ' XXXXXXXXXXXXXXXXXXXXXXXXXXXXX        Inicio de calculos para obter o valor do avaliador    Energia produção de leite      XXXXXXXXXXXXXXXXXXXXXXX
                Dim energMtc As Double
                Dim energLac As Double
                Dim energLacComLact As Double
                Dim fatorCr As Double

                If lblGord.Text = "" Then
                    lblGord.Text = 0
                ElseIf lblProt.Text = "" Then
                    lblProt.Text = 0
                ElseIf lblLact.Text = "" Then
                    lblLact.Text = 0
                End If
                energMtc = (lblPV.Text ^ 0.75) * 0.08 'Energia de mantença	mcal/ dia	(PV elevado a 0.75) * 0.08
                energLac = (0.0929 * lblGord.Text) + ((0.0547 * lblProt.Text) + 0.192) 'Energia de lactação	mcal/ dia 0.0929*% de gordura +0.0547*% de proteína +0.192)
                energLacComLact = (0.0929 * lblGord.Text) + (0.0547 * lblProt.Text) + (0.0395 * lblLact.Text) 'Energia de lactação com lactose	mcal/ dia	0.0929*% de gordura +0.0547*% de proteína +0.0395* % de lactose

                Dim pctNel As Double
                pctNel = qtdNel ' / totalMS * 100
                'Fator de Correção FL		
                'Se o NEL da dieta for		
                '15 - 20	    10	
                '20.01 - 25	    7	
                '25.01 - 30	    5	
                '30.01 - 35	    -2	
                '35.01 - 40	    -7	
                '40.01 - 45	    -10	
                '>45	-12	
                If pctNel > 15 And pctNel <= 20 Then
                    fatorCr = 10
                ElseIf pctNel > 20 And pctNel <= 25 Then
                    fatorCr = 7
                ElseIf pctNel > 25 And pctNel <= 30 Then
                    fatorCr = 5
                ElseIf pctNel > 30 And pctNel <= 35 Then
                    fatorCr = -2
                ElseIf pctNel > 35 And pctNel <= 40 Then
                    fatorCr = -7
                ElseIf pctNel > 40 And pctNel <= 45 Then
                    fatorCr = -10
                ElseIf pctNel > 45 Then
                    fatorCr = -12
                ElseIf pctNel <= 15 Then
                    fatorCr = 15
                End If

                Dim el As Double
                Dim telpc As Double
                el = (pctNel - energMtc) / (energLac) ' + fatorCr) 'Estimatina prd leite EL	Kg/ dia	(Nel da dieta - Energia de mantença) / energia lactação  + fator de coreção FL

                'sem lactose
                telpc = el / 100 * fatorCr
                estimatLeite = el + telpc

                'com lactose
                Dim elcLact As Double
                Dim elc As Double
                elcLact = (pctNel - energMtc) / (energLacComLact) ' + fatorCr) 'Estimatina prd leite EL Lactose	Kg/ dia	(Nel da dieta - Energia de mantença com lactose) / energia lactação  + fator de coreção FL
                elc = elcLact / 100 * fatorCr


                estimatLeiteLact = elcLact + elc

                'xxxxxxxxxxxxxxxxxxxxxxx

                'xxxxxxxxxxxxxxxxxxxxxx

            Next
            'If dtgAlimentosDieta.Rows(0).Cells(68).Value = 0 Then
            '    dtgAlimentosDieta.Rows(0).Cells(68).Value = 0.00001
            'End If
            Dim qamiddr As Double
            qamiddr = qamidr

            ' XXXXXXXXXXXXXXXXXXXXXXXXXXXXX        Inicio de calculos para obter o valor do avaliador    Proteina produção de leite      XXXXXXXXXXXXXXXXXXXXXXX
            Dim ptnMet As Double

            'PTN Met =PNDR + (PDR*0.4772)

            'Proteína disponível para leite: PTN Metabolizável * Fator 

            'Fator: Até 26 kg = 76.46409    26 a 40= 69.1698     Acima de 40= 66.65636
            Dim fator As Double

            If lblLeite.Text < 26 Then
                fator = 76.46409
            ElseIf lblLeite.Text >= 26 And lblLeite.Text <= 40 Then
                fator = 69.1698
            ElseIf lblLeite.Text > 40 Then
                fator = 66.65636
            End If

            ptnMet = ((totalPND + (totalPDR * 0.4772)) / 100) * 1000
            ProtPrudLeite = ptnMet / fator

            'ptnMet = (totalPND + (totalPDR * 0.4772))
            'ProtPrudLeite = ptnMet / fator
            'calcular quando a primeira linha do dtgalimentosdieta estiver 0
            If qtdProduto2 > 0.0001 Then

                dtgAvaliadores.Rows(0).Cells(12).Value = kgTotalMS  ' antes de % ms
                dtgAvaliadores.Rows(1).Cells(12).Value = kgTotalMS / totalMS  ' / somaProduto 
                dtgAvaliadores.Rows(2).Cells(12).Value = totalPB / kgTotalMS
                dtgAvaliadores.Rows(3).Cells(12).Value = totalPDR / kgTotalMS
                dtgAvaliadores.Rows(4).Cells(12).Value = totalPND / kgTotalMS
                dtgAvaliadores.Rows(5).Cells(12).Value = totalFDN / kgTotalMS
                dtgAvaliadores.Rows(6).Cells(12).Value = totaleFDN / kgTotalMS

                dtgAvaliadores.Rows(7).Cells(12).Value = totalMNmaior8 / kgTotalMS
                dtgAvaliadores.Rows(8).Cells(12).Value = totalMNmaior19 / kgTotalMS
                dtgAvaliadores.Rows(9).Cells(12).Value = totalFDNF / kgTotalMS
                dtgAvaliadores.Rows(10).Cells(12).Value = totalFDA / kgTotalMS
                dtgAvaliadores.Rows(11).Cells(12).Value = totalNEl / kgTotalMS
                dtgAvaliadores.Rows(12).Cells(12).Value = totalNDT / kgTotalMS
                dtgAvaliadores.Rows(13).Cells(12).Value = totalEE / kgTotalMS
                dtgAvaliadores.Rows(14).Cells(12).Value = totalEE_Insat / kgTotalMS
                dtgAvaliadores.Rows(15).Cells(12).Value = totalCinzas / kgTotalMS
                dtgAvaliadores.Rows(16).Cells(12).Value = totalCNF / kgTotalMS
                dtgAvaliadores.Rows(17).Cells(12).Value = totalAmido / kgTotalMS
                dtgAvaliadores.Rows(18).Cells(12).Value = qamiddr / kgTotalMS  'xxxxxxxxxxxxxxxx

                dtgAvaliadores.Rows(19).Cells(12).Value = totalMor / kgTotalMS
                dtgAvaliadores.Rows(20).Cells(12).Value = totalCa / kgTotalMS
                dtgAvaliadores.Rows(21).Cells(12).Value = totalP / kgTotalMS
                dtgAvaliadores.Rows(22).Cells(12).Value = totalMg / kgTotalMS
                dtgAvaliadores.Rows(23).Cells(12).Value = totalK / kgTotalMS
                dtgAvaliadores.Rows(24).Cells(12).Value = totalS / kgTotalMS
                dtgAvaliadores.Rows(25).Cells(12).Value = totalNa / kgTotalMS
                dtgAvaliadores.Rows(26).Cells(12).Value = totalCl / kgTotalMS
                dtgAvaliadores.Rows(27).Cells(12).Value = totalCo / kgTotalMS
                dtgAvaliadores.Rows(28).Cells(12).Value = totalCu / kgTotalMS
                dtgAvaliadores.Rows(29).Cells(12).Value = totalMn / kgTotalMS
                dtgAvaliadores.Rows(30).Cells(12).Value = totalZn / kgTotalMS
                dtgAvaliadores.Rows(31).Cells(12).Value = totalSe / kgTotalMS
                dtgAvaliadores.Rows(32).Cells(12).Value = totalI / kgTotalMS
                dtgAvaliadores.Rows(33).Cells(12).Value = totalA / kgTotalMS
                dtgAvaliadores.Rows(34).Cells(12).Value = totalD / kgTotalMS
                dtgAvaliadores.Rows(35).Cells(12).Value = totalE / kgTotalMS
                dtgAvaliadores.Rows(36).Cells(12).Value = totalCromo / kgTotalMS

                dtgAvaliadores.Rows(37).Cells(12).Value = totalBiotina / kgTotalMS
                dtgAvaliadores.Rows(38).Cells(12).Value = totalVirginiamicina / kgTotalMS
                dtgAvaliadores.Rows(39).Cells(12).Value = totalMonensina / kgTotalMS
                dtgAvaliadores.Rows(40).Cells(12).Value = totalLevedura / kgTotalMS
                dtgAvaliadores.Rows(41).Cells(12).Value = totalArginina / kgTotalMS
                dtgAvaliadores.Rows(42).Cells(12).Value = totalHistidina / kgTotalMS
                dtgAvaliadores.Rows(43).Cells(12).Value = totalIsoleucina / kgTotalMS
                dtgAvaliadores.Rows(44).Cells(12).Value = totalLeucina / kgTotalMS
                dtgAvaliadores.Rows(45).Cells(12).Value = totalLisina / kgTotalMS
                dtgAvaliadores.Rows(46).Cells(12).Value = totalMetionina / kgTotalMS
                dtgAvaliadores.Rows(47).Cells(12).Value = totalFenilalanina / kgTotalMS
                dtgAvaliadores.Rows(48).Cells(12).Value = totalTreonina / kgTotalMS
                dtgAvaliadores.Rows(49).Cells(12).Value = totalTriptofano / kgTotalMS
                dtgAvaliadores.Rows(50).Cells(12).Value = totalValina / kgTotalMS
                dtgAvaliadores.Rows(51).Cells(12).Value = totaldFDNp48h / totalMS
                dtgAvaliadores.Rows(52).Cells(12).Value = totaldAmido7h / totalMS
                dtgAvaliadores.Rows(53).Cells(12).Value = totalTTNDFD / totalMS
                dtgAvaliadores.Rows(54).Cells(12).Value = mn8AmiDR
                dtgAvaliadores.Rows(55).Cells(12).Value = mn8PV
                dtgAvaliadores.Rows(56).Cells(12).Value = dfnfPV
                dtgAvaliadores.Rows(57).Cells(12).Value = forragem * 100
                dtgAvaliadores.Rows(58).Cells(12).Value = concentrado * 100
                dtgAvaliadores.Rows(59).Cells(12).Value = dcad
                dtgAvaliadores.Rows(60).Cells(12).Value = consumo
                dtgAvaliadores.Rows(61).Cells(12).Value = caP
                dtgAvaliadores.Rows(62).Cells(12).Value = lysMet

                dtgAvaliadores.Rows(63).Cells(12).Value = estimatLeite
                dtgAvaliadores.Rows(64).Cells(12).Value = estimatLeiteLact
                dtgAvaliadores.Rows(65).Cells(12).Value = ProtPrudLeite
                'calcular quando a primeira linha do dtgalimentosdieta estiver 0

            Else
                For Each row As DataGridViewRow In dtgAvaliadores.Rows
                    row.Cells(12).Value = 0
                Next

            End If

            'mn8AmiDR ='MN >8 da dieta/ AmiDR da dieta


        Catch exc As DivideByZeroException
            Console.WriteLine("Erro: Divisão por zero")
        Catch exc As OverflowException
            Console.WriteLine("Erro: Overflow")
        Finally
            Console.ReadLine()
        End Try

        If My.Settings.corAvalOnOf = True Then
            CorAval()
        End If

    End Sub

    'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX      CALCULOS DA DIETA NA MN     XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
    'dieta1 na MN
    Private Sub CalcularDieta01()

        ZerarTotais()
        Dim estimatLeiteLact As Double
        Dim estimatLeite As Double

        Try

            '=(1,169+(1,375*11,35)/650*100)+(1,721*(5,46)/650*100)
            For Each row As DataGridViewRow In dtgAlimentosDieta.Rows

                qtdKgMs = row.Cells(4).Value * row.Cells(67).Value / 100 ' ok

                totalMS += row.Cells(4).Value * row.Cells(67).Value
                totalPB += row.Cells(5).Value * qtdKgMs
                totalPDR += row.Cells(6).Value * qtdKgMs
                totalPND += row.Cells(7).Value * qtdKgMs
                totalFDN += row.Cells(8).Value * qtdKgMs
                totaleFDN += row.Cells(9).Value * qtdKgMs

                totalMNmaior8 += row.Cells(10).Value * qtdKgMs '11
                totalMNmaior19 += row.Cells(11).Value * qtdKgMs '12
                totalFDNF += row.Cells(12).Value * qtdKgMs
                totalFDA += row.Cells(13).Value * qtdKgMs
                totalNEl += row.Cells(14).Value * qtdKgMs

                qtdNel += row.Cells(14).Value * row.Cells(67).Value * row.Cells(4).Value

                totalNDT += row.Cells(15).Value * qtdKgMs
                totalEE += row.Cells(16).Value * qtdKgMs
                totalEE_Insat += row.Cells(17).Value * qtdKgMs
                totalCinzas += row.Cells(18).Value * qtdKgMs
                totalCNF += row.Cells(19).Value * qtdKgMs
                totalAmido += row.Cells(20).Value * qtdKgMs
                totalkd_Amid += row.Cells(21).Value * qtdKgMs
                totalMor += row.Cells(22).Value * qtdKgMs

                totalCa += row.Cells(23).Value * qtdKgMs
                totalP += row.Cells(24).Value * qtdKgMs
                totalMg += row.Cells(25).Value * qtdKgMs
                totalK += row.Cells(26).Value * qtdKgMs
                totalS += row.Cells(27).Value * qtdKgMs
                totalNa += row.Cells(28).Value * qtdKgMs
                totalCl += row.Cells(29).Value * qtdKgMs
                totalCo += row.Cells(30).Value * qtdKgMs
                totalCu += row.Cells(31).Value * qtdKgMs
                totalMn += row.Cells(32).Value * qtdKgMs
                totalZn += row.Cells(33).Value * qtdKgMs
                totalSe += row.Cells(34).Value * qtdKgMs
                totalI += row.Cells(35).Value * qtdKgMs
                totalA += row.Cells(36).Value * qtdKgMs
                totalD += row.Cells(37).Value * qtdKgMs
                totalE += row.Cells(38).Value * qtdKgMs
                totalCromo += row.Cells(39).Value * qtdKgMs

                totalBiotina += row.Cells(40).Value * qtdKgMs
                totalVirginiamicina += row.Cells(41).Value * qtdKgMs
                totalMonensina += row.Cells(42).Value * qtdKgMs
                totalLevedura += row.Cells(43).Value * qtdKgMs
                totalArginina += row.Cells(44).Value * qtdKgMs
                totalHistidina += row.Cells(45).Value * qtdKgMs
                totalIsoleucina += row.Cells(46).Value * qtdKgMs
                totalLeucina += row.Cells(47).Value * qtdKgMs
                totalLisina += row.Cells(48).Value * qtdKgMs
                totalMetionina += row.Cells(49).Value * qtdKgMs
                totalFenilalanina += row.Cells(50).Value * qtdKgMs
                totalTreonina += row.Cells(51).Value * qtdKgMs
                totalTriptofano += row.Cells(52).Value * qtdKgMs
                totalValina += row.Cells(53).Value * qtdKgMs
                totaldFDNp48h += row.Cells(54).Value * qtdKgMs
                totaldAmido7h += row.Cells(55).Value * qtdKgMs
                totalTTNDFD += row.Cells(56).Value * qtdKgMs
                'End If
                If dtgAlimentosDieta.Rows(0).Cells(67).Value = 0 Then
                    dtgAlimentosDieta.Rows(0).Cells(67).Value = 0.00001
                End If
                Dim msVol1 As Double = 0

                ' XXXXXXXXXXXXXXXXXXXXXXXXXXXXX        Inicio de calculos para obter o valor do avaliador MN>8/AmiDR   XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                If lblPV.Text = "" Or lblPV.Text = 0 Then
                    lblPV.Text = 1
                End If

                'Separar MS de Volumoso e Concentrado necessário para consguir o valor de KC
                If row.Cells(2).Value.ToString = "Gramíneas e Leguminosas" Or row.Cells(2).Value.ToString = "Silagens" Then
                    msVol += row.Cells(4).Value * row.Cells(67).Value

                    kcVol = ((1.375 * msVol) / lblPV.Text)
                    'Para calculo do amido
                    msVol1 = row.Cells(4).Value * row.Cells(67).Value

                ElseIf row.Cells(2).Value.ToString = "Concentrados Energéticos" Or row.Cells(2).Value.ToString = "Pré-Mistura" Or row.Cells(2).Value.ToString = "Concentrados Proteicos" Or row.Cells(2).Value.ToString = "Minerais" Or row.Cells(2).Value.ToString = "Outros" Then
                    msConc += row.Cells(4).Value * row.Cells(67).Value
                    kcConc = ((1.721 * msConc) / lblPV.Text)
                    'Para calculo do amido
                    msConc1 = row.Cells(4).Value * row.Cells(67).Value

                End If

                kc = 1.169 + (kcVol + kcConc)

                '%amido * kg ms
                qtdAmid = row.Cells(20).Value * qtdKgMs / 100 ' ok

                'AmiDR = kd amid/(kd amid + kc)*qtd de amido
                qamidr += row.Cells(21).Value / (row.Cells(21).Value + kc) * qtdAmid 'ok
                amiDR = qamidr / totalMS 'ok

                'qamiddr = qamidr * 100

                'MN>8/AmiDR = MN >8 da dieta/ AmiDR da dieta
                'para obter o MN>8 da dieta

                'Se for volumoso
                '((((0,478*0,771*100)*0,9465)+4,5798)/100)*qtd de ms de volumoso
                Dim mn8Vol As Double
                mn8Vol += ((((0.478 * 0.771 * 100) * 0.9465) + 4.5798) / 100) * msVol1  ' ok

                'Se for concentrado
                'QTD CONCENTRADO * % DE MN>8
                Dim mn8Conc As Double
                mn8Conc += msConc1 * row.Cells(10).Value  ' ok
                'MN>8 = % MN>8
                Dim mnM8 As Double

                mnM8 = totalMNmaior8 / 100

                'mnM8 = (mn8Vol + mn8Conc) ' ok
                'mnMaiorq8Dieta = mnM8 / totalMS
                mn8AmiDR = mnM8 / qamidr

                'Label5.Text = qamidr
                'Label4.Text = kc
                'Label6.Text = mn8AmiDR
                ' XXXXXXXXXXXXXXXXXXXXXXXXXXXXX        Inicio de calculos para obter o valor do avaliador MN>8 % do PV   XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                'mn8PV = mnM8 / lblPV.Text
                mn8PV = totalMNmaior8 / lblPV.Text

                ' XXXXXXXXXXXXXXXXXXXXXXXXXXXXX        Inicio de calculos para obter o valor do avaliador FDNF % do PV   XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                dfnfPV = totalFDNF / lblPV.Text

                ' XXXXXXXXXXXXXXXXXXXXXXXXXXXXX        Inicio de calculos para obter o valor do avaliador Forragem       XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                forragem = msVol / totalMS
                ' XXXXXXXXXXXXXXXXXXXXXXXXXXXXX        Inicio de calculos para obter o valor do avaliador Concentrado       XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

                concentrado = msConc / totalMS

                ' XXXXXXXXXXXXXXXXXXXXXXXXXXXXX        Inicio de calculos para obter o valor do avaliador    DCAD       XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                'Sódio*100/0.023+Potássio*100/0.039)-(Cloro*100/0.0355+Enxofre*100/0.016
                '((((Na*100/0,023+k*100/0,039)-(Ci*100/0,0355+S*100/0,016))))
                Dim pctNA As Double
                Dim pctK As Double
                Dim pctCi As Double
                Dim pctS As Double

                pctNA = totalNa / totalMS
                pctK = totalK / totalMS
                pctCi = totalCl / totalMS
                pctS = totalS / totalMS

                dcad = ((((pctNA * 100 / 0.023 + pctK * 100 / 0.039) - (pctCi * 100 / 0.0355 + pctS * 100 / 0.016))))

                ' XXXXXXXXXXXXXXXXXXXXXXXXXXXXX        Inicio de calculos para obter o valor do avaliador    Consumo Total       XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                somaProduto += row.Cells(67).Value
                consumo = somaProduto

                ' XXXXXXXXXXXXXXXXXXXXXXXXXXXXX        Inicio de calculos para obter o valor do avaliador    Ca/P         XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                Dim pctCa As Double
                Dim pctP As Double

                pctCa = totalCa / totalMS
                pctP = totalP / totalMS

                Dim vcap As Double
                vcap = pctCa / pctP
                If vcap > 0 Then
                    caP = vcap
                Else
                    caP = 0
                End If

                ' XXXXXXXXXXXXXXXXXXXXXXXXXXXXX        Inicio de calculos para obter o valor do avaliador    Lys / Met        XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                Dim pctLys As Double
                Dim pctMet As Double

                pctLys = totalLisina / totalMS
                pctMet = totalMetionina / totalMS
                Dim vlysmet As Double
                vlysmet = pctLys / pctMet
                If vlysmet > 0 Then
                    lysMet = vlysmet
                Else
                    lysMet = 0
                End If

                ' XXXXXXXXXXXXXXXXXXXXXXXXXXXXX        Inicio de calculos para obter o valor do avaliador    Energia produção de leite      XXXXXXXXXXXXXXXXXXXXXXX
                Dim energMtc As Double
                Dim energLac As Double
                Dim energLacComLact As Double
                Dim fatorCr As Double

                If lblGord.Text = "" Then
                    lblGord.Text = 0
                ElseIf lblProt.Text = "" Then
                    lblProt.Text = 0
                ElseIf lblLact.Text = "" Then
                    lblLact.Text = 0
                End If
                energMtc = (lblPV.Text ^ 0.75) * 0.08 'Energia de mantença	mcal/ dia	(PV elevado a 0.75) * 0.08
                energLac = (0.0929 * lblGord.Text) + ((0.0547 * lblProt.Text) + 0.192) 'Energia de lactação	mcal/ dia 0.0929*% de gordura +0.0547*% de proteína +0.192)
                energLacComLact = (0.0929 * lblGord.Text) + (0.0547 * lblProt.Text) + (0.0395 * lblLact.Text) 'Energia de lactação com lactose	mcal/ dia	0.0929*% de gordura +0.0547*% de proteína +0.0395* % de lactose

                Dim pctNel As Double
                pctNel = qtdNel / 100 ' / totalMS * 100
                'Fator de Correção FL		
                'Se o NEL da dieta for		
                '15 - 20	    10	
                '20.01 - 25	    7	
                '25.01 - 30	    5	
                '30.01 - 35	    -2	
                '35.01 - 40	    -7	
                '40.01 - 45	    -10	
                '>45	-12	
                If pctNel > 15 And pctNel <= 20 Then
                    fatorCr = 10
                ElseIf pctNel > 20 And pctNel <= 25 Then
                    fatorCr = 7
                ElseIf pctNel > 25 And pctNel <= 30 Then
                    fatorCr = 5
                ElseIf pctNel > 30 And pctNel <= 35 Then
                    fatorCr = -2
                ElseIf pctNel > 35 And pctNel <= 40 Then
                    fatorCr = -7
                ElseIf pctNel > 40 And pctNel <= 45 Then
                    fatorCr = -10
                ElseIf pctNel > 45 Then
                    fatorCr = -12
                ElseIf pctNel <= 15 Then
                    fatorCr = 15
                End If


                Dim el As Double
                Dim telpc As Double
                el = (pctNel - energMtc) / (energLac) ' + fatorCr) 'Estimatina prd leite EL	Kg/ dia	(Nel da dieta - Energia de mantença) / energia lactação  + fator de coreção FL



                'sem lactose
                telpc = el / 100 * fatorCr
                estimatLeite = el + telpc

                'com lactose
                Dim elcLact As Double
                Dim elc As Double
                elcLact = (pctNel - energMtc) / (energLacComLact) ' + fatorCr) 'Estimatina prd leite EL Lactose	Kg/ dia	(Nel da dieta - Energia de mantença com lactose) / energia lactação  + fator de coreção FL
                elc = elcLact / 100 * fatorCr


                estimatLeiteLact = elcLact + elc




            Next
            'If dtgAlimentosDieta.Rows(0).Cells(67).Value = 0 Then
            '    dtgAlimentosDieta.Rows(0).Cells(67).Value = 0.00001
            'End If
            Dim qamiddr As Double
            qamiddr = qamidr * 100

            ' XXXXXXXXXXXXXXXXXXXXXXXXXXXXX        Inicio de calculos para obter o valor do avaliador    Proteina produção de leite      XXXXXXXXXXXXXXXXXXXXXXX
            Dim ptnMet As Double

            'PTN Met =PNDR + (PDR*0.4772)

            'Proteína disponível para leite: PTN Metabolizável * Fator 

            'Fator: Até 26 kg = 76.46409    26 a 40= 69.1698     Acima de 40= 66.65636
            Dim fator As Double

            If lblLeite.Text < 26 Then
                fator = 76.46409
            ElseIf lblLeite.Text >= 26 And lblLeite.Text <= 40 Then
                fator = 69.1698
            ElseIf lblLeite.Text > 40 Then
                fator = 66.65636
            End If

            ptnMet = ((totalPND + (totalPDR * 0.4772)) / 100) * 1000
            ProtPrudLeite = ptnMet / fator

            'calcular quando a primeira linha do dtgalimentosdieta estiver 0
            If qtdProduto > 0.0001 Then

                dtgAvaliadores.Rows(0).Cells(11).Value = totalMS / 100  ' antes de % ms
                dtgAvaliadores.Rows(1).Cells(11).Value = totalMS / somaProduto
                dtgAvaliadores.Rows(2).Cells(11).Value = totalPB / totalMS * 100
                dtgAvaliadores.Rows(3).Cells(11).Value = totalPDR / totalMS * 100
                dtgAvaliadores.Rows(4).Cells(11).Value = totalPND / totalMS * 100
                dtgAvaliadores.Rows(5).Cells(11).Value = totalFDN / totalMS * 100
                dtgAvaliadores.Rows(6).Cells(11).Value = totaleFDN / totalMS * 100
                dtgAvaliadores.Rows(7).Cells(11).Value = totalMNmaior8 / totalMS * 100
                dtgAvaliadores.Rows(8).Cells(11).Value = totalMNmaior19 / totalMS * 100
                dtgAvaliadores.Rows(9).Cells(11).Value = totalFDNF / totalMS * 100
                dtgAvaliadores.Rows(10).Cells(11).Value = totalFDA / totalMS * 100
                dtgAvaliadores.Rows(11).Cells(11).Value = totalNEl / totalMS * 100
                dtgAvaliadores.Rows(12).Cells(11).Value = totalNDT / totalMS * 100
                dtgAvaliadores.Rows(13).Cells(11).Value = totalEE / totalMS * 100
                dtgAvaliadores.Rows(14).Cells(11).Value = totalEE_Insat / totalMS * 100
                dtgAvaliadores.Rows(15).Cells(11).Value = totalCinzas / totalMS * 100
                dtgAvaliadores.Rows(16).Cells(11).Value = totalCNF / totalMS * 100
                dtgAvaliadores.Rows(17).Cells(11).Value = totalAmido / totalMS * 100
                dtgAvaliadores.Rows(18).Cells(11).Value = qamiddr / totalMS * 100  'xxxxxxxxxxxxxxxx
                dtgAvaliadores.Rows(19).Cells(11).Value = totalMor / totalMS * 100
                dtgAvaliadores.Rows(20).Cells(11).Value = totalCa / totalMS * 100
                dtgAvaliadores.Rows(21).Cells(11).Value = totalP / totalMS * 100
                dtgAvaliadores.Rows(22).Cells(11).Value = totalMg / totalMS * 100
                dtgAvaliadores.Rows(23).Cells(11).Value = totalK / totalMS * 100
                dtgAvaliadores.Rows(24).Cells(11).Value = totalS / totalMS * 100
                dtgAvaliadores.Rows(25).Cells(11).Value = totalNa / totalMS * 100
                dtgAvaliadores.Rows(26).Cells(11).Value = totalCl / totalMS * 100
                dtgAvaliadores.Rows(27).Cells(11).Value = totalCo / totalMS * 100
                dtgAvaliadores.Rows(28).Cells(11).Value = totalCu / totalMS * 100
                dtgAvaliadores.Rows(29).Cells(11).Value = totalMn / totalMS * 100
                dtgAvaliadores.Rows(30).Cells(11).Value = totalZn / totalMS * 100
                dtgAvaliadores.Rows(31).Cells(11).Value = totalSe / totalMS * 100
                dtgAvaliadores.Rows(32).Cells(11).Value = totalI / totalMS * 100
                dtgAvaliadores.Rows(33).Cells(11).Value = totalA / totalMS * 100
                dtgAvaliadores.Rows(34).Cells(11).Value = totalD / totalMS * 100
                dtgAvaliadores.Rows(35).Cells(11).Value = totalE / totalMS * 100
                dtgAvaliadores.Rows(36).Cells(11).Value = totalCromo / totalMS * 100
                dtgAvaliadores.Rows(37).Cells(11).Value = totalBiotina / totalMS * 100
                dtgAvaliadores.Rows(38).Cells(11).Value = totalVirginiamicina / totalMS * 100
                dtgAvaliadores.Rows(39).Cells(11).Value = totalMonensina / totalMS * 100
                dtgAvaliadores.Rows(40).Cells(11).Value = totalLevedura / totalMS * 100
                dtgAvaliadores.Rows(41).Cells(11).Value = totalArginina / totalMS * 100
                dtgAvaliadores.Rows(42).Cells(11).Value = totalHistidina / totalMS * 100
                dtgAvaliadores.Rows(43).Cells(11).Value = totalIsoleucina / totalMS * 100
                dtgAvaliadores.Rows(44).Cells(11).Value = totalLeucina / totalMS * 100
                dtgAvaliadores.Rows(45).Cells(11).Value = totalLisina / totalMS * 100
                dtgAvaliadores.Rows(46).Cells(11).Value = totalMetionina / totalMS * 100
                dtgAvaliadores.Rows(47).Cells(11).Value = totalFenilalanina / totalMS * 100
                dtgAvaliadores.Rows(48).Cells(11).Value = totalTreonina / totalMS * 100
                dtgAvaliadores.Rows(49).Cells(11).Value = totalTriptofano / totalMS * 100
                dtgAvaliadores.Rows(50).Cells(11).Value = totalValina / totalMS * 100
                dtgAvaliadores.Rows(51).Cells(11).Value = totaldFDNp48h / totalMS
                dtgAvaliadores.Rows(52).Cells(11).Value = totaldAmido7h / totalMS
                dtgAvaliadores.Rows(53).Cells(11).Value = totalTTNDFD / totalMS

                dtgAvaliadores.Rows(54).Cells(11).Value = mn8AmiDR
                dtgAvaliadores.Rows(55).Cells(11).Value = mn8PV
                dtgAvaliadores.Rows(56).Cells(11).Value = dfnfPV
                dtgAvaliadores.Rows(57).Cells(11).Value = forragem * 100
                dtgAvaliadores.Rows(58).Cells(11).Value = concentrado * 100
                dtgAvaliadores.Rows(59).Cells(11).Value = dcad
                dtgAvaliadores.Rows(60).Cells(11).Value = consumo
                dtgAvaliadores.Rows(61).Cells(11).Value = caP
                dtgAvaliadores.Rows(62).Cells(11).Value = lysMet

                dtgAvaliadores.Rows(63).Cells(11).Value = estimatLeite
                dtgAvaliadores.Rows(64).Cells(11).Value = estimatLeiteLact
                dtgAvaliadores.Rows(65).Cells(11).Value = ProtPrudLeite

            Else
                For Each row As DataGridViewRow In dtgAvaliadores.Rows
                    row.Cells(11).Value = 0
                Next

            End If
            'xxxxxxxxxxxxxxxxxxxxxxxxxxxxx

            'mn8AmiDR ='MN >8 da dieta/ AmiDR da dieta


        Catch exc As DivideByZeroException
            Console.WriteLine("Erro: Divisão por zero")
        Catch exc As OverflowException
            Console.WriteLine("Erro: Overflow")
        Finally
            Console.ReadLine()
        End Try

        If My.Settings.corAvalOnOf = True Then
            CorAval()
        End If

    End Sub
    'dieta2 na MN
    Private Sub CalcularDieta02()

        ZerarTotais()
        Dim estimatLeiteLact As Double
        Dim estimatLeite As Double
        Try
            '=(1,169+(1,375*11,35)/650*100)+(1,721*(5,46)/650*100)
            For Each row As DataGridViewRow In dtgAlimentosDieta.Rows

                qtdKgMs = row.Cells(4).Value * row.Cells(68).Value / 100 ' ok

                totalMS += row.Cells(4).Value * row.Cells(68).Value
                totalPB += row.Cells(5).Value * qtdKgMs
                totalPDR += row.Cells(6).Value * qtdKgMs
                totalPND += row.Cells(7).Value * qtdKgMs
                totalFDN += row.Cells(8).Value * qtdKgMs
                totaleFDN += row.Cells(9).Value * qtdKgMs
                totalMNmaior8 += row.Cells(10).Value * qtdKgMs
                totalMNmaior19 += row.Cells(11).Value * qtdKgMs
                totalFDNF += row.Cells(12).Value * qtdKgMs
                totalFDA += row.Cells(13).Value * qtdKgMs
                totalNEl += row.Cells(14).Value * qtdKgMs

                qtdNel += row.Cells(14).Value * row.Cells(68).Value * row.Cells(4).Value

                totalNDT += row.Cells(15).Value * qtdKgMs
                totalEE += row.Cells(16).Value * qtdKgMs
                totalEE_Insat += row.Cells(17).Value * qtdKgMs
                totalCinzas += row.Cells(18).Value * qtdKgMs
                totalCNF += row.Cells(19).Value * qtdKgMs
                totalAmido += row.Cells(20).Value * qtdKgMs
                totalkd_Amid += row.Cells(21).Value * qtdKgMs
                totalMor += row.Cells(22).Value * qtdKgMs

                totalCa += row.Cells(23).Value * qtdKgMs
                totalP += row.Cells(24).Value * qtdKgMs
                totalMg += row.Cells(25).Value * qtdKgMs
                totalK += row.Cells(26).Value * qtdKgMs
                totalS += row.Cells(27).Value * qtdKgMs
                totalNa += row.Cells(28).Value * qtdKgMs
                totalCl += row.Cells(29).Value * qtdKgMs
                totalCo += row.Cells(30).Value * qtdKgMs
                totalCu += row.Cells(31).Value * qtdKgMs
                totalMn += row.Cells(32).Value * qtdKgMs
                totalZn += row.Cells(33).Value * qtdKgMs
                totalSe += row.Cells(34).Value * qtdKgMs
                totalI += row.Cells(35).Value * qtdKgMs
                totalA += row.Cells(36).Value * qtdKgMs
                totalD += row.Cells(37).Value * qtdKgMs
                totalE += row.Cells(38).Value * qtdKgMs
                totalCromo += row.Cells(39).Value * qtdKgMs

                totalBiotina += row.Cells(40).Value * qtdKgMs
                totalVirginiamicina += row.Cells(41).Value * qtdKgMs
                totalMonensina += row.Cells(42).Value * qtdKgMs
                totalLevedura += row.Cells(43).Value * qtdKgMs
                totalArginina += row.Cells(44).Value * qtdKgMs
                totalHistidina += row.Cells(45).Value * qtdKgMs
                totalIsoleucina += row.Cells(46).Value * qtdKgMs
                totalLeucina += row.Cells(47).Value * qtdKgMs
                totalLisina += row.Cells(48).Value * qtdKgMs
                totalMetionina += row.Cells(49).Value * qtdKgMs
                totalFenilalanina += row.Cells(50).Value * qtdKgMs
                totalTreonina += row.Cells(51).Value * qtdKgMs
                totalTriptofano += row.Cells(52).Value * qtdKgMs
                totalValina += row.Cells(53).Value * qtdKgMs
                totaldFDNp48h += row.Cells(54).Value * qtdKgMs
                totaldAmido7h += row.Cells(55).Value * qtdKgMs
                totalTTNDFD += row.Cells(56).Value * qtdKgMs
                'End If
                If dtgAlimentosDieta.Rows(0).Cells(68).Value = 0 Then
                    dtgAlimentosDieta.Rows(0).Cells(68).Value = 0.00001
                End If
                Dim msVol1 As Double = 0

                ' XXXXXXXXXXXXXXXXXXXXXXXXXXXXX        Inicio de calculos para obter o valor do avaliador MN>8/AmiDR   XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                If lblPV.Text = "" Or lblPV.Text = 0 Then
                    lblPV.Text = 1
                End If
                'Separar MS de Volumoso e Concentrado necessário para consguir o valor de KC
                If row.Cells(2).Value.ToString = "Gramíneas e Leguminosas" Or row.Cells(2).Value.ToString = "Silagens" Then
                    msVol += row.Cells(4).Value * row.Cells(68).Value
                    kcVol = ((1.375 * msVol) / lblPV.Text)
                    'Para calculo do amido
                    msVol1 = row.Cells(4).Value * row.Cells(68).Value

                ElseIf row.Cells(2).Value.ToString = "Concentrados Energéticos" Or row.Cells(2).Value.ToString = "Pré-Mistura" Or row.Cells(2).Value.ToString = "Concentrados Proteicos" Or row.Cells(2).Value.ToString = "Minerais" Or row.Cells(2).Value.ToString = "Outros" Then
                    msConc += row.Cells(4).Value * row.Cells(68).Value
                    kcConc = ((1.721 * msConc) / lblPV.Text)
                    'Para calculo do amido
                    msConc1 = row.Cells(4).Value * row.Cells(68).Value

                End If

                kc = 1.169 + (kcVol + kcConc)



                '%amido * kg ms
                qtdAmid = row.Cells(20).Value * qtdKgMs / 100 ' ok


                'AmiDR = kd amid/(kd amid + kc)*qtd de amido
                qamidr += row.Cells(21).Value / (row.Cells(21).Value + kc) * qtdAmid 'ok
                amiDR = qamidr / totalMS 'ok

                'MN>8/AmiDR = MN >8 da dieta/ AmiDR da dieta
                'para obter o MN>8 da dieta

                'Se for volumoso
                '((((0,478*0,771*100)*0,9465)+4,5798)/100)*qtd de ms de volumoso
                Dim mn8Vol As Double
                mn8Vol += ((((0.478 * 0.771 * 100) * 0.9465) + 4.5798) / 100) * msVol1  ' ok

                'Se for concentrado
                'QTD CONCENTRADO * % DE MN>8
                Dim mn8Conc As Double
                mn8Conc += msConc1 * row.Cells(10).Value  ' ok
                'MN>8 = % MN>8
                Dim mnM8 As Double

                mnM8 = totalMNmaior8 / 100

                'mnM8 = (mn8Vol + mn8Conc) ' ok
                'mnMaiorq8Dieta = mnM8 / totalMS
                mn8AmiDR = mnM8 / qamidr

                'Label5.Text = qamidr
                'Label4.Text = kc
                'Label6.Text = mn8AmiDR
                ' XXXXXXXXXXXXXXXXXXXXXXXXXXXXX        Inicio de calculos para obter o valor do avaliador MN>8 % do PV   XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                'mn8PV = mnM8 / lblPV.Text
                mn8PV = totalMNmaior8 / lblPV.Text

                ' XXXXXXXXXXXXXXXXXXXXXXXXXXXXX        Inicio de calculos para obter o valor do avaliador FDNF % do PV   XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                dfnfPV = totalFDNF / lblPV.Text

                ' XXXXXXXXXXXXXXXXXXXXXXXXXXXXX        Inicio de calculos para obter o valor do avaliador Forragem       XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                forragem = msVol / totalMS
                ' XXXXXXXXXXXXXXXXXXXXXXXXXXXXX        Inicio de calculos para obter o valor do avaliador Concentrado       XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

                concentrado = msConc / totalMS

                ' XXXXXXXXXXXXXXXXXXXXXXXXXXXXX        Inicio de calculos para obter o valor do avaliador    DCAD       XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                'Sódio*100/0.023+Potássio*100/0.039)-(Cloro*100/0.0355+Enxofre*100/0.016
                '((((Na*100/0,023+k*100/0,039)-(Ci*100/0,0355+S*100/0,016))))
                Dim pctNA As Double
                Dim pctK As Double
                Dim pctCi As Double
                Dim pctS As Double

                pctNA = totalNa / totalMS
                pctK = totalK / totalMS
                pctCi = totalCl / totalMS
                pctS = totalS / totalMS

                dcad = ((((pctNA * 100 / 0.023 + pctK * 100 / 0.039) - (pctCi * 100 / 0.0355 + pctS * 100 / 0.016))))

                ' XXXXXXXXXXXXXXXXXXXXXXXXXXXXX        Inicio de calculos para obter o valor do avaliador    Consumo Total       XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                somaProduto += row.Cells(68).Value
                consumo = somaProduto

                ' XXXXXXXXXXXXXXXXXXXXXXXXXXXXX        Inicio de calculos para obter o valor do avaliador    Ca/P         XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                Dim pctCa As Double
                Dim pctP As Double

                pctCa = totalCa / totalMS
                pctP = totalP / totalMS

                Dim vcap As Double
                vcap = pctCa / pctP
                If vcap > 0 Then
                    caP = vcap
                Else
                    caP = 0
                End If

                ' XXXXXXXXXXXXXXXXXXXXXXXXXXXXX        Inicio de calculos para obter o valor do avaliador    Lys / Met        XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                Dim pctLys As Double
                Dim pctMet As Double

                pctLys = totalLisina / totalMS
                pctMet = totalMetionina / totalMS
                Dim vlysmet As Double
                vlysmet = pctLys / pctMet
                If vlysmet > 0 Then
                    lysMet = vlysmet
                Else
                    lysMet = 0
                End If
                '' XXXXXXXXXXXXXXXXXXXXXXXXXXXXX        Inicio de calculos para obter o valor do avaliador    Ca/P         XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                'Dim pctCa As Double
                'Dim pctP As Double

                'pctCa = totalCa / totalMS
                'pctP = totalP / totalMS
                'caP = pctCa / pctP

                '' XXXXXXXXXXXXXXXXXXXXXXXXXXXXX        Inicio de calculos para obter o valor do avaliador    Lys / Met        XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                'Dim pctLys As Double
                'Dim pctMet As Double

                'pctLys = totalLisina / totalMS
                'pctMet = totalMetionina / totalMS
                'lysMet = pctLys / pctMet

                ' XXXXXXXXXXXXXXXXXXXXXXXXXXXXX        Inicio de calculos para obter o valor do avaliador    Energia produção de leite      XXXXXXXXXXXXXXXXXXXXXXX
                Dim energMtc As Double
                Dim energLac As Double
                Dim energLacComLact As Double
                Dim fatorCr As Double

                If lblGord.Text = "" Then
                    lblGord.Text = 0
                ElseIf lblProt.Text = "" Then
                    lblProt.Text = 0
                ElseIf lblLact.Text = "" Then
                    lblLact.Text = 0
                End If
                energMtc = (lblPV.Text ^ 0.75) * 0.08 'Energia de mantença	mcal/ dia	(PV elevado a 0.75) * 0.08
                energLac = (0.0929 * lblGord.Text) + ((0.0547 * lblProt.Text) + 0.192) 'Energia de lactação	mcal/ dia 0.0929*% de gordura +0.0547*% de proteína +0.192)
                energLacComLact = (0.0929 * lblGord.Text) + (0.0547 * lblProt.Text) + (0.0395 * lblLact.Text) 'Energia de lactação com lactose	mcal/ dia	0.0929*% de gordura +0.0547*% de proteína +0.0395* % de lactose

                Dim pctNel As Double
                pctNel = qtdNel / 100 ' / totalMS * 100
                'Fator de Correção FL		
                'Se o NEL da dieta for		
                '15 - 20	    10	
                '20.01 - 25	    7	
                '25.01 - 30	    5	
                '30.01 - 35	    -2	
                '35.01 - 40	    -7	
                '40.01 - 45	    -10	
                '>45	-12	
                If pctNel > 15 And pctNel <= 20 Then
                    fatorCr = 10
                ElseIf pctNel > 20 And pctNel <= 25 Then
                    fatorCr = 7
                ElseIf pctNel > 25 And pctNel <= 30 Then
                    fatorCr = 5
                ElseIf pctNel > 30 And pctNel <= 35 Then
                    fatorCr = -2
                ElseIf pctNel > 35 And pctNel <= 40 Then
                    fatorCr = -7
                ElseIf pctNel > 40 And pctNel <= 45 Then
                    fatorCr = -10
                ElseIf pctNel > 45 Then
                    fatorCr = -12
                ElseIf pctNel <= 15 Then
                    fatorCr = 15
                End If


                Dim el As Double
                Dim telpc As Double
                el = (pctNel - energMtc) / (energLac) ' + fatorCr) 'Estimatina prd leite EL	Kg/ dia	(Nel da dieta - Energia de mantença) / energia lactação  + fator de coreção FL



                'sem lactose
                telpc = el / 100 * fatorCr
                estimatLeite = el + telpc

                'com lactose
                Dim elcLact As Double
                Dim elc As Double
                elcLact = (pctNel - energMtc) / (energLacComLact) ' + fatorCr) 'Estimatina prd leite EL Lactose	Kg/ dia	(Nel da dieta - Energia de mantença com lactose) / energia lactação  + fator de coreção FL
                elc = elcLact / 100 * fatorCr


                estimatLeiteLact = elcLact + elc

            Next
            'If dtgAlimentosDieta.Rows(0).Cells(68).Value = 0 Then
            '    dtgAlimentosDieta.Rows(0).Cells(68).Value = 0.00001
            'End If
            Dim qamiddr As Double
            qamiddr = qamidr * 100

            ' XXXXXXXXXXXXXXXXXXXXXXXXXXXXX        Inicio de calculos para obter o valor do avaliador    Proteina produção de leite      XXXXXXXXXXXXXXXXXXXXXXX
            Dim ptnMet As Double

            'PTN Met =PNDR + (PDR*0.4772)

            'Proteína disponível para leite: PTN Metabolizável * Fator 

            'Fator: Até 26 kg = 76.46409    26 a 40= 69.1698     Acima de 40= 66.65636
            Dim fator As Double

            If lblLeite.Text < 26 Then
                fator = 76.46409
            ElseIf lblLeite.Text >= 26 And lblLeite.Text <= 40 Then
                fator = 69.1698
            ElseIf lblLeite.Text > 40 Then
                fator = 66.65636
            End If
            Dim tpnd As Double = totalPND * totalMS / 100
            Dim tpdr As Double = totalPDR * totalMS / 100

            ptnMet = ((totalPND + (totalPDR * 0.4772)) / 100) * 1000
            ProtPrudLeite = ptnMet / fator

            'calcular quando a primeira linha do dtgalimentosdieta estiver 0
            If qtdProduto2 > 0.0001 Then

                dtgAvaliadores.Rows(0).Cells(12).Value = totalMS / 100  ' antes de % ms
                dtgAvaliadores.Rows(1).Cells(12).Value = totalMS / somaProduto
                dtgAvaliadores.Rows(2).Cells(12).Value = totalPB / totalMS * 100
                dtgAvaliadores.Rows(3).Cells(12).Value = totalPDR / totalMS * 100
                dtgAvaliadores.Rows(4).Cells(12).Value = totalPND / totalMS * 100
                dtgAvaliadores.Rows(5).Cells(12).Value = totalFDN / totalMS * 100
                dtgAvaliadores.Rows(6).Cells(12).Value = totaleFDN / totalMS * 100
                dtgAvaliadores.Rows(7).Cells(12).Value = totalMNmaior8 / totalMS * 100
                dtgAvaliadores.Rows(8).Cells(12).Value = totalMNmaior19 / totalMS * 100
                dtgAvaliadores.Rows(9).Cells(12).Value = totalFDNF / totalMS * 100
                dtgAvaliadores.Rows(10).Cells(12).Value = totalFDA / totalMS * 100
                dtgAvaliadores.Rows(11).Cells(12).Value = totalNEl / totalMS * 100
                dtgAvaliadores.Rows(12).Cells(12).Value = totalNDT / totalMS * 100
                dtgAvaliadores.Rows(13).Cells(12).Value = totalEE / totalMS * 100
                dtgAvaliadores.Rows(14).Cells(12).Value = totalEE_Insat / totalMS * 100
                dtgAvaliadores.Rows(15).Cells(12).Value = totalCinzas / totalMS * 100
                dtgAvaliadores.Rows(16).Cells(12).Value = totalCNF / totalMS * 100
                dtgAvaliadores.Rows(17).Cells(12).Value = totalAmido / totalMS * 100
                dtgAvaliadores.Rows(18).Cells(12).Value = qamiddr / totalMS * 100  'xxxxxxxxxxxxxxxx
                dtgAvaliadores.Rows(19).Cells(12).Value = totalMor / totalMS * 100
                dtgAvaliadores.Rows(20).Cells(12).Value = totalCa / totalMS * 100
                dtgAvaliadores.Rows(21).Cells(12).Value = totalP / totalMS * 100
                dtgAvaliadores.Rows(22).Cells(12).Value = totalMg / totalMS * 100
                dtgAvaliadores.Rows(23).Cells(12).Value = totalK / totalMS * 100
                dtgAvaliadores.Rows(24).Cells(12).Value = totalS / totalMS * 100
                dtgAvaliadores.Rows(25).Cells(12).Value = totalNa / totalMS * 100
                dtgAvaliadores.Rows(26).Cells(12).Value = totalCl / totalMS * 100
                dtgAvaliadores.Rows(27).Cells(12).Value = totalCo / totalMS * 100
                dtgAvaliadores.Rows(28).Cells(12).Value = totalCu / totalMS * 100
                dtgAvaliadores.Rows(29).Cells(12).Value = totalMn / totalMS * 100
                dtgAvaliadores.Rows(30).Cells(12).Value = totalZn / totalMS * 100
                dtgAvaliadores.Rows(31).Cells(12).Value = totalSe / totalMS * 100
                dtgAvaliadores.Rows(32).Cells(12).Value = totalI / totalMS * 100
                dtgAvaliadores.Rows(33).Cells(12).Value = totalA / totalMS * 100
                dtgAvaliadores.Rows(34).Cells(12).Value = totalD / totalMS * 100
                dtgAvaliadores.Rows(35).Cells(12).Value = totalE / totalMS * 100
                dtgAvaliadores.Rows(36).Cells(12).Value = totalCromo / totalMS * 100
                dtgAvaliadores.Rows(37).Cells(12).Value = totalBiotina / totalMS * 100
                dtgAvaliadores.Rows(38).Cells(12).Value = totalVirginiamicina / totalMS
                dtgAvaliadores.Rows(39).Cells(12).Value = totalMonensina / totalMS * 100
                dtgAvaliadores.Rows(40).Cells(12).Value = totalLevedura / totalMS * 100
                dtgAvaliadores.Rows(41).Cells(12).Value = totalArginina / totalMS * 100
                dtgAvaliadores.Rows(42).Cells(12).Value = totalHistidina / totalMS * 100
                dtgAvaliadores.Rows(43).Cells(12).Value = totalIsoleucina / totalMS * 100
                dtgAvaliadores.Rows(44).Cells(12).Value = totalLeucina / totalMS * 100
                dtgAvaliadores.Rows(45).Cells(12).Value = totalLisina / totalMS * 100
                dtgAvaliadores.Rows(46).Cells(12).Value = totalMetionina / totalMS * 100
                dtgAvaliadores.Rows(47).Cells(12).Value = totalFenilalanina / totalMS * 100
                dtgAvaliadores.Rows(48).Cells(12).Value = totalTreonina / totalMS * 100
                dtgAvaliadores.Rows(49).Cells(12).Value = totalTriptofano / totalMS * 100
                dtgAvaliadores.Rows(50).Cells(12).Value = totalValina / totalMS * 100
                dtgAvaliadores.Rows(51).Cells(12).Value = totaldFDNp48h / totalMS
                dtgAvaliadores.Rows(52).Cells(12).Value = totaldAmido7h / totalMS
                dtgAvaliadores.Rows(53).Cells(12).Value = totalTTNDFD / totalMS

                dtgAvaliadores.Rows(54).Cells(12).Value = mn8AmiDR
                dtgAvaliadores.Rows(55).Cells(12).Value = mn8PV
                dtgAvaliadores.Rows(56).Cells(12).Value = dfnfPV
                dtgAvaliadores.Rows(57).Cells(12).Value = forragem * 100
                dtgAvaliadores.Rows(58).Cells(12).Value = concentrado * 100
                dtgAvaliadores.Rows(59).Cells(12).Value = dcad
                dtgAvaliadores.Rows(60).Cells(12).Value = consumo
                dtgAvaliadores.Rows(61).Cells(12).Value = caP
                dtgAvaliadores.Rows(62).Cells(12).Value = lysMet

                dtgAvaliadores.Rows(63).Cells(12).Value = estimatLeite
                dtgAvaliadores.Rows(64).Cells(12).Value = estimatLeiteLact
                dtgAvaliadores.Rows(65).Cells(12).Value = ProtPrudLeite

            Else
                For Each row As DataGridViewRow In dtgAvaliadores.Rows
                    row.Cells(12).Value = 0
                Next

            End If

        Catch exc As DivideByZeroException
            Console.WriteLine("Erro: Divisão por zero")
        Catch exc As OverflowException
            Console.WriteLine("Erro: Overflow")
        Finally
            Console.ReadLine()
        End Try


        If My.Settings.corAvalOnOf = True Then
            CorAval()
        End If

    End Sub

    'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX      CALCULOS POR CATEGRIA       XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
    'Calcular consumo por categoria de animais
    Private Sub CalculoConsumoMS()
        '        xxxxxxxxxxxxxxxxxxxxxxxxxxxxxx          Categoria	Consumo de Matária Seca   xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

        'Lactação	1.3131+(0.87*((0.372*(Produção de leite*(0.4+0.15*% de gordura do leite))+0.0968*(Peso do animal^0.75))*(1-EXP(-0.192*(semana da lactação+3.67)))))
        Try

            Dim exp1 As Double
            exp1 = (-0.192 * (lblDel.Text / 7 + 3.67))

            Dim a As Double
            a = System.Math.Exp(exp1)
            ' msLactacao = 1.3131 + (0.87 * 3o((0.372 * 2o(lblLeite.Text * 1o(0.4 + 0.15 * lblGord.Text)) + 0.0968 * (lblPV.Text ^ 0.75)) * (1 - a)))
            msLactacao = 1.3131 + (0.87 * ((0.372 * (lblLeite.Text * (0.4 + 0.15 * lblGord.Text)) + 0.0968 * (lblPV.Text ^ 0.75)) * (1 - a)))
            'Bezerra	3.3048+(Peso vivo*0.0168)
            msBezerra = 3.3048 + (lblPV.Text * 0.0168)

            'Novilha	3.3048+(Peso vivo*0.0168)
            msNovilha = 3.3048 + (lblPV.Text * 0.0168)

            'Vaca Seca	((1.97-(0.75*EXP(0.16*(Dias de gestação-280))))/100)*Peso vivo
            Dim exp2 As Double
            exp2 = (0.16 * (lblDiasGest.Text - 280))
            Dim b As Double
            b = System.Math.Exp(exp2)

            msVacaSeca = ((1.97 - (0.75 * b)) / 100) * lblPV.Text

            'Pré-parto	(1.47-((0.0365-0.0028*FDN da dieta)*(Dias que faltam para parir/7)-0.035*(Dias que faltam para parir/7)^2))*Peso vivo/100)
            msPreParto = (1.47 - ((0.0365 - 0.0028 * totalFDN) * (280 - lblDiasGest.Text / 7) - 0.035 * (280 - lblDiasGest.Text / 7) ^ 2)) * lblPV.Text / 100


            If lblCat.Text = "Lactação" Then
                dtgAvaliadores.Rows(0).Cells(7).Value = Format(msLactacao, "0.00")
                dtgAvaliadores.Rows(0).Cells(3).Value = msLactacao / 100 * 92
                dtgAvaliadores.Rows(0).Cells(9).Value = msLactacao / 100 * 108
                '3 9
            ElseIf lblCat.Text = "Bezerra" Then
                dtgAvaliadores.Rows(0).Cells(7).Value = Format(msBezerra, "0.00")
                dtgAvaliadores.Rows(0).Cells(3).Value = msBezerra / 100 * 92
                dtgAvaliadores.Rows(0).Cells(9).Value = msBezerra / 100 * 108

            ElseIf lblCat.Text = "Novilha" Then
                dtgAvaliadores.Rows(0).Cells(7).Value = Format(msNovilha, "0.00")
                dtgAvaliadores.Rows(0).Cells(3).Value = msNovilha / 100 * 92
                dtgAvaliadores.Rows(0).Cells(9).Value = msNovilha / 100 * 108

            ElseIf lblCat.Text = "Pré-Parto" Then
                dtgAvaliadores.Rows(0).Cells(7).Value = Format(msPreParto, "0.00")
                dtgAvaliadores.Rows(0).Cells(3).Value = msPreParto / 100 * 92
                dtgAvaliadores.Rows(0).Cells(9).Value = msPreParto / 100 * 108

            ElseIf lblCat.Text = "Vaca Seca" Then
                dtgAvaliadores.Rows(0).Cells(7).Value = Format(msVacaSeca, "0.00")
                dtgAvaliadores.Rows(0).Cells(3).Value = msVacaSeca / 100 * 92
                dtgAvaliadores.Rows(0).Cells(9).Value = msVacaSeca / 100 * 108


            End If

        Catch exc As DivideByZeroException
            Console.WriteLine("Erro: Divisão por zero")
        Catch exc As OverflowException
            Console.WriteLine("Erro: Overflow")
        Finally
            Console.ReadLine()
        End Try

    End Sub
    'btn seta verde
    Private Sub btnCalcularDieta_Click(sender As Object, e As EventArgs) Handles btnCalcularDieta.Click

        On Error Resume Next
        ' With dtgAlimentosDieta

        ' If .CurrentCell.ColumnIndex = 66 Or .CurrentCell.ColumnIndex = 67 Or .CurrentCell.ColumnIndex = 68 Then
        If varms = True Then
            CalcularValorDieta01()
            CalcularValorDieta02()

            CalcularDieta01MS()
            CalcularDieta02MS()

            CalcularFinan01()
            CalcularFinan02()
        Else
            CalcularValorDieta01()
            CalcularValorDieta02()

            CalcularDieta01()
            CalcularDieta02()

            CalcularFinan01()
            CalcularFinan02()
        End If


        PainelDieta()

        If My.Settings.corAvalOnOf = True Then
            CorAval()
        End If

    End Sub

    'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX                 LOTES            XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

    Dim idLote As String
    'Localizar lotes da propriedade
    Private Sub LocLotes()
        Dim da As SQLiteDataAdapter
        Dim dtLotes As New DataTable

        Try
            abrir()

            Dim sql As String = "SELECT * FROM DadosAnimais WHERE Cliente = @Cliente"
            Using cmd As New SQLiteCommand(sql, con)
                cmd.Parameters.AddWithValue("@Cliente", lblIdCliente.Text)

                da = New SQLiteDataAdapter(cmd)
                da.Fill(dtLotes)

                ' Atualiza DataGridView
                dtgltes.DataSource = dtLotes

                ' Atualiza ComboBox
                cbxLote.DisplayMember = "Lote"
                cbxLote.ValueMember = "Lote"
                cbxLote.DataSource = dtLotes
            End Using

        Catch ex As Exception
            MessageBox.Show("Erro ao localizar lotes: " & ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            fechar()
        End Try
    End Sub
    'Exibir o painel lotes
    Private Sub btnVisLote_Click(sender As Object, e As EventArgs) Handles btnVisLote.Click
        'If cbxLote.Text = "" Then
        '    MsgBox("Lote não encontrado.")
        'Else
        '    idLote = cbxLote.Text
        pnlVisLote.Visible = True
        pnlVisLote.BringToFront()
        pnlVisLote.Location = New Point(420, 70)
        '    VisDadosLote()
        'End If
        'Threading.Thread.Sleep(1000)
        'dtgAvaliadores.Rows(0).Cells(11).Selected = True
    End Sub
    'Fechar o painel lotes
    Private Sub btnFecharVisLotes_Click(sender As Object, e As EventArgs) Handles btnFecharVisLotes.Click
        pnlVisLote.Visible = False
    End Sub
    'Preencher o cbx avaliadores de acordo com o tipo de lote
    Private Sub dtgltes_Paint(sender As Object, e As PaintEventArgs) Handles dtgltes.Paint
        ' Evitar múltiplas execuções desnecessárias.
        If cbxAvaliador.Items.Count = 0 Then
            cbxAvaliador.Items.Clear()
            PreencherLotes()

            Select Case lblCat.Text
                Case "Lactação"
                    cbxAvaliador.Items.Add("Vacas em Lactação")
                    cbxAvaliador.Text = "Vacas em Lactação"
                Case "Vaca Seca"
                    cbxAvaliador.Items.Add("Vacas Secas")
                    cbxAvaliador.Text = "Vacas Secas"
                Case "Pré-parto"
                    cbxAvaliador.Items.Add("Pré-Parto")
                    cbxAvaliador.Text = "Pré-Parto"
                Case "Bezerra"
                    cbxAvaliador.Items.Add("Bezerras")
                    cbxAvaliador.Text = "Bezerras"
                Case "Novilha"
                    cbxAvaliador.Items.Add("Novilhas")
                    cbxAvaliador.Text = "Novilhas"
            End Select

            cbxAvaliador.Items.Add("Todos")
        End If

        leitePrev = lblLeite.Text
        leitePrec = lblPreLeite.Text
    End Sub
    'preencher o painel de informações do lote
    Private Sub PreencherLotes()
        Dim vlr As Double
        ' Label5.Text = dtgltes.CurrentRow.Cells(16).Value
        lblNomeLote.Text = dtgltes.CurrentRow.Cells(1).Value
        lblCat.Text = dtgltes.CurrentRow.Cells(2).Value
        lblQtA.Text = dtgltes.CurrentRow.Cells(3).Value
        lblPV.Text = dtgltes.CurrentRow.Cells(4).Value
        lblLeite.Text = dtgltes.CurrentRow.Cells(5).Value
        lblDel.Text = dtgltes.CurrentRow.Cells(6).Value
        lblNOrd.Text = dtgltes.CurrentRow.Cells(7).Value
        'lblPasto.Text = dtgltes.CurrentRow.Cells(8).Value
        lblDist.Text = dtgltes.CurrentRow.Cells(9).Value
        lblDiasGest.Text = dtgltes.CurrentRow.Cells(10).Value
        vlr = dtgltes.CurrentRow.Cells(11).Value / 100
        lblGord.Text = vlr.ToString("F2")
        vlr = dtgltes.CurrentRow.Cells(12).Value / 100
        lblProt.Text = vlr.ToString("F2")
        vlr = dtgltes.CurrentRow.Cells(13).Value / 100
        lblLact.Text = vlr.ToString("F2")
        vlr = dtgltes.CurrentRow.Cells(14).Value
        lblSobra.Text = vlr.ToString("F2")
        Dim lt As Double = dtgltes.CurrentRow.Cells(15).Value / 100
        lblPreLeite.Text = lt.ToString("F2")
    End Sub

    'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX              AVALIADORES         XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
    'Sub buscar os avaliadores
    Private Sub BuscarAvaliadores()
        Dim da As New SQLiteDataAdapter
        Dim dt As New DataTable
        Dim sql As String

        sql = "Select * from Avaliadores where NomeAvaliador = " & "'" & cbxAvaliador.Text & "'" '& '"' group by NomeAvaliador"

        Try

            abrir()

            da = New SQLiteDataAdapter(sql, con)
            dt = New DataTable
            da.Fill(dt)
            dtgAvaliadores.DataSource = dt

        Catch ex As Exception

            MsgBox(ex.Message)
            fechar()

        End Try

    End Sub
    'Buscar os avaliadores escolhidos no cbxavaliadores
    Private Sub cbxAvaliador_TextChanged(sender As Object, e As EventArgs) Handles cbxAvaliador.TextChanged
        BuscarAvaliadores()

        CalculoConsumoMS()
        ConfigGridAvaliadores()
        pnlSelectAval.Visible = False
        Dim x As Integer = dtgAlimentosDieta.Rows.Count
        If x > 0 Then
            If varms = True Then
                CalcularValorDieta01()
                CalcularDieta01MS()
                CalcularDieta02MS()
            Else
                CalcularValorDieta02()
                CalcularDieta01()
                CalcularDieta02()
            End If
        End If

    End Sub
    'Cor avaliadores caso seja escolhido esa função nas configurações dos avaliadores
    Private Sub CorAval()
        For i As Integer = 0 To dtgAvaliadores.RowCount - 1
            If dtgAvaliadores.Rows(i).Cells(11).Value > dtgAvaliadores.Rows(i).Cells(9).Value Or dtgAvaliadores.Rows(i).Cells(11).Value < dtgAvaliadores.Rows(i).Cells(3).Value Then
                dtgAvaliadores.Rows(i).Cells(11).Style.BackColor = Color.FromArgb(255, 241, 194)
            ElseIf dtgAvaliadores.Rows(i).Cells(11).Value < dtgAvaliadores.Rows(i).Cells(9).Value And dtgAvaliadores.Rows(i).Cells(11).Value > dtgAvaliadores.Rows(i).Cells(3).Value Then
                dtgAvaliadores.Rows(i).Cells(11).Style.BackColor = Color.FromArgb(207, 247, 211)
            End If

            If dtgAvaliadores.Rows(i).Cells(12).Value > dtgAvaliadores.Rows(i).Cells(9).Value Or dtgAvaliadores.Rows(i).Cells(12).Value < dtgAvaliadores.Rows(i).Cells(3).Value Then
                dtgAvaliadores.Rows(i).Cells(12).Style.BackColor = Color.FromArgb(255, 241, 194)
            ElseIf dtgAvaliadores.Rows(i).Cells(12).Value < dtgAvaliadores.Rows(i).Cells(9).Value And dtgAvaliadores.Rows(i).Cells(12).Value > dtgAvaliadores.Rows(i).Cells(3).Value Then
                dtgAvaliadores.Rows(i).Cells(12).Style.BackColor = Color.FromArgb(207, 247, 211)
            End If
            If dtgAvaliadores.Rows(i).Cells(3).Value = 0 And dtgAvaliadores.Rows(i).Cells(9).Value = 0 Then
                dtgAvaliadores.Rows(i).Cells(11).Style.BackColor = Color.White
                dtgAvaliadores.Rows(i).Cells(12).Style.BackColor = Color.White
            End If

        Next

    End Sub
    'brir painel para Selecionar o avaliador
    Private Sub btnSelecAval_Click(sender As Object, e As EventArgs) Handles btnSelecAval.Click
        pnlSelectAval.Visible = True
        pnlSelectAval.Location = New Point(400, 20)
        pnlSelectAval.BringToFront()
        cbxAvaliador.DropDownStyle = ComboBoxStyle.DropDownList
    End Sub
    'Fechar painel de Selecionar o avaliador
    Private Sub btnXEscAval_Click(sender As Object, e As EventArgs) Handles btnXEscAval.Click
        pnlSelectAval.Visible = False

    End Sub
    'Configurar dtgAvaliadores
    Dim varAval7 As Boolean
    Dim varAval As Integer
    Private Sub ConfigGridAvaliadores()

        'Dim x As Integer
        'x = dtgAvaliadores.Rows.Count
        'If x < 14 Then
        '    varAval = 245
        'Else
        '    varAval = 225
        'End If

        For Each columns As DataGridViewColumn In Me.dtgAvaliadores.Columns
            dtgAvaliadores.Columns(columns.Index).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter ' centralizar as celulas

        Next

        For i As Integer = 0 To dtgAvaliadores.RowCount() - 1

            If dtgAvaliadores.Rows(i).Cells(0).Value = 0 Then
                dtgAvaliadores.Rows(i).Visible = False
            ElseIf dtgAvaliadores.Rows(i).Cells(0).Value = 1 Then
                dtgAvaliadores.Rows(i).Visible = True
            End If
        Next

        On Error Resume Next
        With Me.dtgAvaliadores

            .DefaultCellStyle.ForeColor = Color.FromArgb(90, 90, 90)
            .DefaultCellStyle.BackColor = Color.White
            ' .ColumnHeadersDefaultCellStyle.Font = New Font("Inter", 8, FontStyle.Bold)
            .DefaultCellStyle.Font = New Font("Inter", 8.5, FontStyle.Bold)

            .Columns(0).Visible = False
            .Columns(1).Visible = False
            .Columns(2).Width = varAval
            .Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft ' voltar alimentos para esquerda
            .Columns(3).Visible = False
            .Columns(4).Visible = False
            .Columns(5).Visible = False
            .Columns(6).Visible = False
            .Columns(7).Visible = varAval7
            .Columns(7).Width = 127
            .Columns(7).DefaultCellStyle.BackColor = Color.WhiteSmoke
            .Columns(8).Visible = False
            .Columns(9).Visible = False
            .Columns(10).Visible = False
            .Columns(11).Visible = varD1
            .Columns(11).Width = 115
            .Columns(12).DefaultCellStyle.BackColor = Color.WhiteSmoke
            .Columns(12).Visible = varD2
            .Columns(12).Width = 115
            .Columns(13).Visible = False
        End With
    End Sub
    'Padronizar dados avaliadosres
    Private Sub dtgavaliadores_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dtgAvaliadores.CellFormatting
        If e.ColumnIndex = 11 Or 12 Then ' AndAlso IsNumeric(e.Value) 
            If IsNumeric(e.Value) Then
                e.Value = Format(CDbl(e.Value), "0.00")
                e.FormattingApplied = True
            End If
        End If
    End Sub

    'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX        ALIMENTOS TEMPORARIOS       XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
    ''TABELA Temp
    Private Sub TabelaAlimentos()

        ' Verifica se as colunas já foram adicionadas para evitar duplicidade
        If dtTemp.Columns.Count > 0 Then Exit Sub

        Dim colunas As String() = {
            "AlimentoFamilia", "Alimento", "MS", "PB", "PDR", "PNDR", "FDN", "eFDN", "MN>8", "MN>19", "FDNF", "FDA", "Nel", "NDT",
            "EE", "EE Insat", "Cinzas", "CNF", "Amido", "kd Amid", "Mor", "Ca", "P", "Mg", "K", "S", "Na", "Cl", "Co", "Cu", "Mn",
            "Zn", "Se", "I", "A", "D", "E", "Cromo", "Biotina", "Virginiamicina", "Monensina", "Levedura",
            "Arginina", "Histidina", "Isoleucina", "Leucina", "Lisina", "Metionina", "Fenilalanina", "Treonina",
            "Triptofano", "Valina", "dFDNp48h", "dAmido7h", "TTNDFD",
            "Pars1", "Pars2", "Pars3", "Pars4", "Pars5", "Pars6", "Pars7", "Pars8", "Pars9",
            "$ Prod", "QtdD1", "QtdD2", "Pré-mix", "PctPremix", "Qtd Vagão", "Qtd Premix",
            "Propriedade", "Lote", "IdPropriedade", "QtdAnimais", "Data", "IdAlim"
        }

        ' Adiciona todas as colunas como tipo String
        For Each nomeColuna As String In colunas
            If Not dtTemp.Columns.Contains(nomeColuna) Then
                dtTemp.Columns.Add(nomeColuna, GetType(String))
            End If
        Next

    End Sub
    'Configurar dtgTemp
    Private Sub ConfigGridTemp()
        For Each columns As DataGridViewColumn In Me.dtgTemp.Columns
            dtgTemp.Columns(columns.Index).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter ' centralizar as celulas
        Next

        On Error Resume Next
        With Me.dtgTemp

            .DefaultCellStyle.ForeColor = Color.FromArgb(90, 90, 90)
            .DefaultCellStyle.BackColor = Color.White
            '.ColumnHeadersDefaultCellStyle.Font = New Font("Inter", 8, FontStyle.Bold)
            .DefaultCellStyle.Font = New Font("Inter", 8.5, FontStyle.Bold)
            '.Columns(0).HeaderText = ""
            .Columns(1).Visible = False
            '.Columns(2).HeaderText = "Alimento"
            .Columns(2).Width = 220
            .Columns(2).Frozen = True
            .Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft ' voltar alimentos para esquerda

            For i = 3 To 79
                .Columns(i).Visible = False
            Next


        End With
    End Sub
    'Ao entrar em umma dieta o dtTemp será populado de acordo com o id da dieta selecionada
    Private Sub PreencherGridTemp()
        Dim ds As New DataSet
        'Dim dt As New DataTable
        For Each row As DataGridViewRow In dtgAlimentoNome.Rows
            dtTemp.Rows.Add(row.Cells(1).Value, row.Cells(2).Value, row.Cells(3).Value, row.Cells(4).Value, row.Cells(5).Value, row.Cells(6).Value, row.Cells(7).Value, row.Cells(8).Value, row.Cells(9).Value,
                        row.Cells(10).Value, row.Cells(11).Value, row.Cells(12).Value, row.Cells(13).Value, row.Cells(14).Value, row.Cells(15).Value, row.Cells(16).Value, row.Cells(17).Value, row.Cells(18).Value,
                        row.Cells(19).Value, row.Cells(20).Value, row.Cells(21).Value, row.Cells(22).Value, row.Cells(23).Value, row.Cells(24).Value, row.Cells(25).Value, row.Cells(26).Value, row.Cells(27).Value, row.Cells(28).Value,
                        row.Cells(29).Value, row.Cells(30).Value, row.Cells(31).Value, row.Cells(32).Value, row.Cells(33).Value, row.Cells(34).Value, row.Cells(35).Value, row.Cells(36).Value, row.Cells(37).Value,
row.Cells(38).Value, row.Cells(39).Value, row.Cells(40).Value, row.Cells(41).Value, row.Cells(42).Value, row.Cells(43).Value, row.Cells(44).Value, row.Cells(45).Value, row.Cells(46).Value, row.Cells(47).Value,
row.Cells(48).Value, row.Cells(49).Value, row.Cells(50).Value, row.Cells(51).Value, row.Cells(52).Value, row.Cells(53).Value, row.Cells(54).Value, row.Cells(55).Value, row.Cells(56).Value, row.Cells(57).Value,
row.Cells(58).Value, row.Cells(59).Value, row.Cells(60).Value, row.Cells(61).Value, row.Cells(62).Value, row.Cells(63).Value, row.Cells(64).Value, row.Cells(65).Value, row.Cells(66).Value, row.Cells(67).Value,
row.Cells(68).Value, row.Cells(69).Value, row.Cells(70).Value, row.Cells(71).Value, row.Cells(72).Value, row.Cells(73).Value, row.Cells(74).Value, row.Cells(75).Value, row.Cells(76).Value, row.Cells(77).Value)

        Next
        dtgTemp.DataSource = dtTemp
        PreencherGridVol()
    End Sub

    'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX          BIBLIOTECA ALIMENTOS       XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
    'Exibir o painel da biblioteca de alimentos
    Private Sub btnAlimentos_Click(sender As Object, e As EventArgs) Handles btnAlimentos.Click
        pnlAlimentos.Location = New Point(50, 2)
        pnlAlimentos.BringToFront()
        GridAlimentos()
        ConfigGridTemp()
        pnlAlimentos.Visible = True

    End Sub
    'Exibir alimentos na MS
    Private Sub rdbBuscarMS_CheckedChanged(sender As Object, e As EventArgs) Handles rdbBuscarMS.CheckedChanged
        queryAlimentos = "Select *  from AlimentosMS"
        BuscarAlimentosMSMO()

        txtBuscarAlimentoNome.Text = ""
        rdbVEnerg.Checked = False
        rdbVProt.Checked = False
        rdbCEnerg.Checked = False
        rdbCProt.Checked = False
        rdbMinerais.Checked = False
        'rdbAdt.Checked = False
        rdbOutros.Checked = False

        lblmtria.Text = "Base Matéria Seca"
        lblbasemsmo.Text = "Base Matéria Seca"
        'pnlDieta.BackColor = Color.FromArgb(237, 242, 207)
    End Sub
    'Exibir alimentos na MN
    Private Sub rdbBuscarMO_Click(sender As Object, e As EventArgs) Handles rdbBuscarMO.Click
        On Error Resume Next
        For Each row As DataGridViewRow In dtgAlimentos.Rows
            'row.Cells(4).Value = row.Cells(4).Value * row.Cells(4).Value / 100
            row.Cells(5).Value = row.Cells(5).Value * row.Cells(4).Value / 100
            row.Cells(6).Value = row.Cells(6).Value * row.Cells(4).Value / 100
            row.Cells(7).Value = row.Cells(7).Value * row.Cells(4).Value / 100
            row.Cells(8).Value = row.Cells(8).Value * row.Cells(4).Value / 100

            row.Cells(11).Value = row.Cells(11).Value * row.Cells(4).Value / 100
            row.Cells(12).Value = row.Cells(12).Value * row.Cells(4).Value / 100
            row.Cells(13).Value = row.Cells(13).Value * row.Cells(4).Value / 100
            row.Cells(14).Value = row.Cells(14).Value * row.Cells(4).Value / 100
            row.Cells(15).Value = row.Cells(15).Value * row.Cells(4).Value / 100
            row.Cells(16).Value = row.Cells(16).Value * row.Cells(4).Value / 100
            row.Cells(17).Value = row.Cells(17).Value * row.Cells(4).Value / 100
            row.Cells(18).Value = row.Cells(18).Value * row.Cells(4).Value / 100
            row.Cells(19).Value = row.Cells(19).Value * row.Cells(4).Value / 100
            row.Cells(20).Value = row.Cells(20).Value * row.Cells(4).Value / 100
            row.Cells(21).Value = row.Cells(21).Value * row.Cells(4).Value / 100
            row.Cells(22).Value = row.Cells(22).Value * row.Cells(4).Value / 100
            row.Cells(23).Value = row.Cells(23).Value * row.Cells(4).Value / 100
            row.Cells(24).Value = row.Cells(24).Value * row.Cells(4).Value / 100
            row.Cells(25).Value = row.Cells(25).Value * row.Cells(4).Value / 100
            row.Cells(26).Value = row.Cells(26).Value * row.Cells(4).Value / 100
            row.Cells(27).Value = row.Cells(27).Value * row.Cells(4).Value / 100
            row.Cells(28).Value = row.Cells(28).Value * row.Cells(4).Value / 100
            row.Cells(29).Value = row.Cells(29).Value * row.Cells(4).Value / 100
            row.Cells(30).Value = row.Cells(30).Value * row.Cells(4).Value / 100
            row.Cells(31).Value = row.Cells(31).Value * row.Cells(4).Value / 100
            row.Cells(32).Value = row.Cells(32).Value * row.Cells(4).Value / 100
            row.Cells(33).Value = row.Cells(33).Value * row.Cells(4).Value / 100
            row.Cells(34).Value = row.Cells(34).Value * row.Cells(4).Value / 100
            row.Cells(35).Value = row.Cells(35).Value * row.Cells(4).Value / 100
            row.Cells(36).Value = row.Cells(36).Value * row.Cells(4).Value / 100
            row.Cells(37).Value = row.Cells(37).Value * row.Cells(4).Value / 100
            row.Cells(38).Value = row.Cells(38).Value * row.Cells(4).Value / 100
            row.Cells(39).Value = row.Cells(39).Value * row.Cells(4).Value / 100
            row.Cells(40).Value = row.Cells(40).Value * row.Cells(4).Value / 100
            row.Cells(41).Value = row.Cells(41).Value * row.Cells(4).Value / 100
            row.Cells(42).Value = row.Cells(42).Value * row.Cells(4).Value / 100
            row.Cells(43).Value = row.Cells(43).Value * row.Cells(4).Value / 100
            row.Cells(44).Value = row.Cells(44).Value * row.Cells(4).Value / 100
            row.Cells(45).Value = row.Cells(45).Value * row.Cells(4).Value / 100
            row.Cells(46).Value = row.Cells(46).Value * row.Cells(4).Value / 100
            row.Cells(47).Value = row.Cells(47).Value * row.Cells(4).Value / 100
            row.Cells(48).Value = row.Cells(48).Value * row.Cells(4).Value / 100
            row.Cells(49).Value = row.Cells(49).Value * row.Cells(4).Value / 100
            row.Cells(50).Value = row.Cells(50).Value * row.Cells(4).Value / 100
            row.Cells(51).Value = row.Cells(51).Value * row.Cells(4).Value / 100
            row.Cells(52).Value = row.Cells(52).Value * row.Cells(4).Value / 100
            row.Cells(53).Value = row.Cells(53).Value * row.Cells(4).Value / 100
            row.Cells(54).Value = row.Cells(54).Value * row.Cells(4).Value / 100
            row.Cells(55).Value = row.Cells(55).Value * row.Cells(4).Value / 100
            row.Cells(56).Value = row.Cells(56).Value * row.Cells(4).Value / 100
            row.Cells(57).Value = row.Cells(57).Value * row.Cells(4).Value / 100
            row.Cells(58).Value = row.Cells(58).Value * row.Cells(4).Value / 100
            row.Cells(59).Value = row.Cells(59).Value * row.Cells(4).Value / 100
            row.Cells(60).Value = row.Cells(60).Value * row.Cells(4).Value / 100
            row.Cells(61).Value = row.Cells(61).Value * row.Cells(4).Value / 100
            row.Cells(62).Value = row.Cells(62).Value * row.Cells(4).Value / 100
            row.Cells(63).Value = row.Cells(63).Value * row.Cells(4).Value / 100
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
        Next
    End Sub
    'Sub Buscar alimentos no DB
    Dim queryAlimentos As String
    Private Sub BuscarAlimentosMSMO()
        Dim da As New SQLiteDataAdapter
        Dim dt As New DataTable

        Try
            abrir()

            da = New SQLiteDataAdapter(queryAlimentos, con)
            dt = New DataTable
            da.Fill(dt)
            dtgAlimentos.DataSource = dt

            fechar()

        Catch ex As Exception

        End Try
        'lblAlimentoSelect.Text = queryAlimentos
    End Sub

    'Método para desmarcar todos os RadioButtons
    Private Sub LimparFiltros()
        rdbVEnerg.Checked = False
        rdbVProt.Checked = False
        rdbCEnerg.Checked = False
        rdbCProt.Checked = False
        rdbMinerais.Checked = False
        'rdbAdt.Checked = False
        rdbOutros.Checked = False
        rdbPreMistura.Checked = False
    End Sub

    'Método para aplicar o filtro
    Private Sub AplicarFiltro(filtro As String)
        If rdbBuscarMS.Checked Then
            Try
                TryCast(dtgAlimentos.DataSource, DataTable).DefaultView.RowFilter = "AlimentoFamilia LIKE '%" & filtro & "%'"
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End Try
        End If
        CorTabAlim()
    End Sub

    'Evento do clique na caixa de busca
    Private Sub txtBuscarAlimentoNome_MouseClick(sender As Object, e As MouseEventArgs) Handles txtBuscarAlimentoNome.MouseClick
        LimparFiltros()
    End Sub

    'Eventos de clique nos RadioButtons
    'Volumosos proteicos
    Private Sub rdbVProt_Click(sender As Object, e As EventArgs) Handles rdbVProt.Click
        LimparFiltros()
        rdbVProt.Checked = True
        AplicarFiltro(rdbVProt.Text)
    End Sub
    'Volumosos energeticos
    Private Sub rdbVEnerg_Click(sender As Object, e As EventArgs) Handles rdbVEnerg.Click
        LimparFiltros()
        rdbVEnerg.Checked = True
        AplicarFiltro(rdbVEnerg.Text)
    End Sub
    'Concentrados energétidos
    Private Sub rdbCEnerg_Click(sender As Object, e As EventArgs) Handles rdbCEnerg.Click
        LimparFiltros()
        rdbCEnerg.Checked = True
        AplicarFiltro(rdbCEnerg.Text)
    End Sub
    'Concentrados proteicos
    Private Sub rdbCProt_Click(sender As Object, e As EventArgs) Handles rdbCProt.Click
        LimparFiltros()
        rdbCProt.Checked = True
        AplicarFiltro(rdbCProt.Text)
    End Sub
    'Minerais
    Private Sub rdbMinerais_Click(sender As Object, e As EventArgs) Handles rdbMinerais.Click
        LimparFiltros()
        rdbMinerais.Checked = True
        AplicarFiltro(rdbMinerais.Text)
    End Sub
    'Pré-misturas
    Private Sub rdbPreMistura_Click(sender As Object, e As EventArgs) Handles rdbPreMistura.Click
        LimparFiltros()
        rdbPreMistura.Checked = True
        AplicarFiltro(rdbPreMistura.Text)
    End Sub
    'Outros
    Private Sub rdbOutros_Click(sender As Object, e As EventArgs) Handles rdbOutros.Click
        LimparFiltros()
        rdbOutros.Checked = True
        AplicarFiltro(rdbOutros.Text)
    End Sub
    'Método para limpar todos os filtros (usado para "mostrar todos")
    Private Sub FiltroTodos()
        LimparFiltros()
        txtBuscarAlimentoNome.Text = ""
        AplicarFiltro("")
    End Sub
    'btnTodos
    Private Sub btnFiltroTodos_Click(sender As Object, e As EventArgs) Handles btnFiltroTodos.Click
        FiltroTodos()
    End Sub
    'Configurar dtgAlimentos
    Private Sub GridAlimentos()
        'Se a coluna é impar muda a cor do backcolor
        For Each columns As DataGridViewColumn In Me.dtgAlimentos.Columns
            dtgAlimentos.Columns(columns.Index).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter ' centralizar as celulas
            If EImpar(columns.Index) = False Then
                dtgAlimentos.Columns(columns.Index).DefaultCellStyle.BackColor = Color.WhiteSmoke ' se o index da coluna for impar então muda a cor

            End If
        Next
        dtgAlimentos.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter ' centralizar o cabeçalho

        With dtgAlimentos
            .DefaultCellStyle.ForeColor = Color.FromArgb(90, 90, 90)
            .DefaultCellStyle.BackColor = Color.White
            .ColumnHeadersDefaultCellStyle.Font = New Font("Inter", 9, FontStyle.Bold)
            .DefaultCellStyle.Font = New Font("Inter", 8.5, FontStyle.Bold)

            '.RowsDefaultCellStyle.BackColor = 
            '.Columns(1).HeaderText = "Selecione o alimento"
            '
            .Columns("Chk").Frozen = True
            .Columns("Chk").Width = 25
            .Columns("Chk").HeaderText = ""
            .Columns("lps").Width = 25
            .Columns("lps").HeaderText = ""
            .Columns("lps").DisplayIndex = 3
            .Columns("lps").Frozen = True
            .Columns(2).Frozen = True
            .Columns(2).Visible = False
            .Columns(3).Width = 220
            .Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft ' voltar alimentos para esquerda
            .Columns(3).Frozen = True

            For i = 4 To 46
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

            .Columns(47).HeaderText = My.Settings.lab1
            .Columns(48).HeaderText = My.Settings.lab2
            .Columns(49).HeaderText = My.Settings.lab3
            .Columns(50).HeaderText = My.Settings.lab4
            .Columns(51).HeaderText = My.Settings.lab5
            .Columns(52).HeaderText = My.Settings.lab6
            .Columns(53).HeaderText = My.Settings.lab7
            .Columns(54).HeaderText = My.Settings.lab8
            .Columns(55).HeaderText = My.Settings.lab9
            .Columns(56).HeaderText = My.Settings.lab10

        End With

    End Sub
    'Fechar painel alimentos
    Private Sub btnFecharAlimentos_Click_1(sender As Object, e As EventArgs) Handles btnFecharAlimentos.Click

        pnlAlimentos.Visible = False
        dtgAlimentosDieta.DataSource = dtgTemp.DataSource
        dtgAlimentosPremix.DataSource = dtgTemp.DataSource
        If dtgTemp.Rows.Count > 0 Then

            ConfigGridAlimentosDieta()
            ConfigGridAlimentosPremix()
            PainelDieta()
        End If

        ' IndiceGridDieta()
    End Sub
    'Pesquisar alimentos
    Private Sub txtBuscarAlimentoNome_TextChanged(sender As Object, e As EventArgs) Handles txtBuscarAlimentoNome.TextChanged
        ' EditarAlimentosTrueFalse()
        Try
            TryCast(dtgAlimentos.DataSource, DataTable).DefaultView.RowFilter = "Alimento LIKE '%" & txtBuscarAlimentoNome.Text & "%'"
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try

    End Sub
    'Pintar a linha selecionada
    Private Sub dtgAlimentos_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dtgAlimentos.CellEndEdit
        CorTabAlim()
    End Sub
    'ao clicar na linha dos alimentos para adiciona los a dieta
    Private Sub dtgAlimentos_Click(sender As Object, e As EventArgs) Handles dtgAlimentos.Click
        Try
            CapturarValoresDaLinha()
            ProcessarCliqueNaColunaCheckbox()
            AtualizarInterface()

        Catch ex As Exception
            'MsgBox("Erro ao processar os dados do alimento: " & ex.Message)
        End Try
    End Sub
    'Passar valores da linha que recebe o click para as variáveis
    Private Sub CapturarValoresDaLinha()
        With dtgAlimentos.CurrentRow
            almntoFamilia = .Cells(2).Value
            almnto = .Cells(3).Value
            v_MS = .Cells(4).Value
            v_PB = .Cells(5).Value
            v_PDR = .Cells(6).Value
            v_PND = .Cells(7).Value
            v_FDN = .Cells(8).Value
            v_eFDN = .Cells(9).Value
            v_MNmaior8 = .Cells(10).Value
            v_MNmaior19 = .Cells(11).Value
            v_FDNF = .Cells(12).Value
            v_FDA = .Cells(13).Value
            v_Nel = .Cells(14).Value
            v_NDT = .Cells(15).Value
            v_EE = .Cells(16).Value
            v_EE_Insat = .Cells(17).Value
            v_Cinzas = .Cells(18).Value
            v_CNF = .Cells(19).Value
            v_Amido = .Cells(20).Value
            v_kd_Amid = .Cells(21).Value
            v_MOR = .Cells(22).Value
            v_Ca = .Cells(23).Value
            v_P = .Cells(24).Value
            v_Mg = .Cells(25).Value
            v_K = .Cells(26).Value
            v_S = .Cells(27).Value
            v_Na = .Cells(28).Value
            v_Cl = .Cells(29).Value
            v_Co = .Cells(30).Value
            v_Cu = .Cells(31).Value
            v_Mn = .Cells(32).Value
            v_Zn = .Cells(33).Value
            v_Se = .Cells(34).Value
            v_I = .Cells(35).Value
            v_A = .Cells(36).Value
            v_D = .Cells(37).Value
            v_E = .Cells(38).Value
            v_Cromo = .Cells(39).Value
            v_Biotina = .Cells(40).Value
            v_Virginiamicina = .Cells(41).Value
            v_Monensina = .Cells(42).Value
            v_Levedura = .Cells(43).Value
            v_Arginina = .Cells(44).Value
            v_Histidina = .Cells(45).Value
            v_Isoleucina = .Cells(46).Value
            v_Leucina = .Cells(47).Value
            v_Lisina = .Cells(48).Value
            v_Metionina = .Cells(49).Value
            v_Fenilalanina = .Cells(50).Value
            v_Treonina = .Cells(51).Value
            v_Triptofano = .Cells(52).Value
            v_Valina = .Cells(53).Value
            v_dFDNp48h = .Cells(54).Value
            v_dAmido_7h = .Cells(55).Value
            v_TTNDFD = .Cells(56).Value
            v_id = .Cells(67).Value
        End With
    End Sub
    'Ao clicar na coluna 0 do dtgAlimentos ... 
    Private Sub ProcessarCliqueNaColunaCheckbox()
        With dtgAlimentos
            If .CurrentCell.ColumnIndex <> 0 Then Exit Sub

            If .CurrentRow.Cells(66).Value = 0 Then
                AdicionarLinhaAoDataTable()
                .CurrentCell.Value = True 'chbx da linha em checado
                .CurrentRow.DefaultCellStyle.BackColor = Color.FromArgb(207, 247, 211) ' linha fica verde
                PreencherTabVol() 'adicionar ao dtgvolumoso
                .CurrentRow.Cells(66).Value = 1 'mudar valor da row.cell(66) de 0 p 1
            Else
                RemoverLinhaDoDataTablePorId(v_id)
                .CurrentRow.Cells(66).Value = 0 ' se desmarcar volta pra 0 e sai dadtgtemp e volumoso
            End If
        End With
    End Sub
    'Adicionar a dtgTemp
    Private Sub AdicionarLinhaAoDataTable()
        dtTemp.Rows.Add(
            almntoFamilia, almnto, v_MS, v_PB, v_PDR, v_PND, v_FDN, v_eFDN, v_MNmaior8, v_MNmaior19,
            v_FDNF, v_FDA, v_Nel, v_NDT, v_EE, v_EE_Insat, v_Cinzas, v_CNF, v_Amido, v_kd_Amid, v_MOR,
            v_Ca, v_P, v_Mg, v_K, v_S, v_Na, v_Cl, v_Co, v_Cu, v_Mn, v_Zn, v_Se, v_I, v_A, v_D, v_E,
            v_Cromo, v_Biotina, v_Virginiamicina, v_Monensina, v_Levedura, v_Arginina, v_Histidina,
            v_Isoleucina, v_Leucina, v_Lisina, v_Metionina, v_Fenilalanina, v_Treonina, v_Triptofano,
            v_Valina, v_dFDNp48h, v_dAmido_7h, v_TTNDFD,
            "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", v_id
        )
        dtgTemp.DataSource = dtTemp
    End Sub
    'Remove a linha ao desmarcar um alimento no dtgAlimentos
    Private Sub RemoverLinhaDoDataTablePorId(id As String)
        For Each row As DataGridViewRow In dtgTemp.Rows
            If row.Cells(77).Value = id Then
                dtgTemp.Rows.Remove(row)
                Exit For
            End If
        Next
    End Sub
    'Atualiza a tabela de alimentos
    Private Sub AtualizarInterface()
        dtgAlimentos.Refresh()
        dtgTemp.Refresh()
        CorTabAlim()

        For Each row As DataGridViewRow In dtgTemp.Rows
            row.Cells(0).Value = True
        Next

        AnaliseVol()
        CorTabAlim()
        ConfigGridTemp()
    End Sub
    'Sub Pintar a linha selecionada
    Private Sub CorTabAlim()
        Try


            For Each row As DataGridViewRow In dtgAlimentos.Rows

                If row.Cells(66).Value = 1 Then
                    row.DefaultCellStyle.BackColor = Color.FromArgb(207, 247, 211)
                    row.Cells(0).Value = True
                ElseIf row.Cells(66).Value = 0 Then
                    row.DefaultCellStyle.BackColor = Color.White
                    row.Cells(0).Value = False
                End If
            Next
        Catch exc As DivideByZeroException
            Console.WriteLine("Erro: Divisão por zero")
        Catch exc As OverflowException
            Console.WriteLine("Erro: Overflow")
        Finally
            Console.ReadLine()
        End Try
    End Sub
    'Remover alimentos do dtgTemp, volumoso etc
    Private Sub dtgTemp_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dtgTemp.CellContentClick

        On Error Resume Next
        With dtgTemp
            If .CurrentCell.ColumnIndex = 0 Then
                v_id = .CurrentRow.Cells(77).Value
                For Each row As DataGridViewRow In dtgAlimentos.Rows
                    If row.Cells(67).Value = v_id Then
                        row.Cells(66).Value = 0
                        row.Cells(0).Value = False
                        Label16.Text = row.Cells(77).Value
                    End If
                Next

                dtgTemp.Rows.Remove(dtgTemp.Rows.Item(dtgTemp.CurrentCell.RowIndex))
                dtgAlimentos.Refresh()
                dtgTemp.Refresh()
            End If
        End With
        CorTabAlim()
    End Sub
    'Formato dos dados do dtgAlientos
    Private Sub dtgalimentos_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dtgAlimentos.CellFormatting
        If e.ColumnIndex > 3 Then 'AndAlso IsNumeric(e.Value) 
            If IsNumeric(e.Value) Then
                e.Value = Format(CDbl(e.Value), "0.00")
                e.FormattingApplied = True
            End If
        End If
    End Sub
    'Botão adicionar do painel alimentos
    Private Sub btnAdAlimentos_Click(sender As Object, e As EventArgs) Handles btnAdAlimentos.Click
        pnlAlimentos.Visible = False
        dtgAlimentosDieta.DataSource = dtgTemp.DataSource
        dtgAlimentosPremix.DataSource = dtgTemp.DataSource
        If dtgTemp.Rows.Count > 0 Then

            ConfigGridAlimentosDieta()
            ConfigGridAlimentosPremix()
            PainelDieta()

            'btnPreMix.Enabled = True
            ' btnPreMix.BackgroundImage = My.Resources.p
        End If

        ''Se exixtir pré mistura na dieta aparece a imagem do lapis para edição
        'For i As Integer = 0 To dtgAlimentosDieta.RowCount() - 1
        '    If dtgAlimentosDieta.Rows(i).Cells(2).Value = "Pré-Mistura" Then
        '        dtgAlimentosDieta.Rows(i).Cells(1).Value = My.Resources.edit5
        '        'btnPreMix.Enabled = False
        '        'btnPreMix.BackgroundImage = My.Resources.premistura_of

        '    End If
        'Next

    End Sub

    'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX     VOLUMOSO      XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

    ''TABELA Volumoso dtgVol
    Private Sub TabelaVol()

        ' Verifica se as colunas já foram adicionadas para evitar duplicidade
        If dt4.Columns.Count > 0 Then Exit Sub

        Dim colunas As String() = {
            "AlimentoFamilia", "Alimento", "MS", "PB", "PDR", "PNDR", "FDN", "eFDN", "MN>8", "MN>19", "FDNF", "FDA", "Nel", "NDT",
            "EE", "EE Insat", "Cinzas", "CNF", "Amido", "kd Amid", "Mor", "Ca", "P", "Mg", "K", "S", "Na", "Cl", "Co", "Cu", "Mn",
            "Zn", "Se", "I", "A", "D", "E", "Cromo", "Biotina", "Virginiamicina", "Monensina", "Levedura",
            "Arginina", "Histidina", "Isoleucina", "Leucina", "Lisina", "Metionina", "Fenilalanina", "Treonina",
            "Triptofano", "Valina", "dFDNp48h", "dAmido7h", "TTNDFD",
            "Pars1", "Pars2", "Pars3", "Pars4", "Pars5", "Pars6", "Pars7", "Pars8", "Pars9",
            "$ Prod", "QtdD1", "QtdD2", "Pré-mix", "PctPremix", "Qtd Vagão", "Qtd Premix",
            "Propriedade", "Lote", "IdPropriedade", "QtdAnimais", "Data"
        }

        ' Adiciona todas as colunas como tipo String
        For Each nomeColuna As String In colunas
            If Not dt4.Columns.Contains(nomeColuna) Then
                dt4.Columns.Add(nomeColuna, GetType(String))
            End If
        Next

    End Sub
    'Popular a tabela volumoso
    Private Sub PreencherTabVol()

        Try
            'Preenceer a grid vol
            '51
            dt4.Rows.Add(almntoFamilia, almnto, v_MS, v_PB, v_PDR, v_PND, v_FDN, v_eFDN, v_MNmaior8, v_MNmaior19, v_FDNF,
            v_FDA, v_Nel, v_NDT, v_EE, v_EE_Insat, v_Cinzas, v_CNF, v_Amido, v_kd_Amid, v_MOR, v_Ca, v_P, v_Mg, v_K, v_S, v_Na, v_Cl, v_Co, v_Cu, v_Mn, v_Zn, v_Se, v_I, v_A, v_D, v_E,
            v_Cromo, v_Biotina, v_Virginiamicina, v_Monensina, v_Levedura, v_Arginina, v_Histidina, v_Isoleucina, v_Leucina, v_Lisina, v_Metionina, v_Fenilalanina, v_Treonina, v_Triptofano, v_Valina,
            v_dFDNp48h, v_dAmido_7h, v_TTNDFD, "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0") ', "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")

            dtgVol.DataSource = dt4

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try

    End Sub
    'Montar o datatable p o dtg
    Private Sub TabelaVolumoso()
        If dt2.Columns.Count = 0 Then
            dt2.Columns.Add("Alimento")
            dt2.Columns.Add("Análises")

            Dim analises As String() = {
                "MS", "PB", "PDR", "PNDR", "FDN", "eFDN", "MN>8", "MN>19", "FDNF", "FDA", "Nel", "NDT", "EE", "EE Insat",
                "Cinzas", "CNF", "Amido", "kd Amid", "Mor", "Ca", "P", "Mg",
                "K", "S", "Na", "Cl", "Co", "Cu", "Mn", "Zn", "Se", "I",
                "A", "D", "E", "Cromo", "Biotina", "Virginiamicina", "Monensina",
                "Levedura", "Lisina", "Metionina", "dFDNp48h", "dAmido_7h"
            }

            For Each analise As String In analises
                dt2.Rows.Add(analise, "0")
            Next
        End If
    End Sub
    'Popular os combobox's do painel de analise de volumosos
    Private Sub TabCbx()
        On Error Resume Next

        Dim analises As String() = {
            "MS", "PB", "PDR", "PNDR", "FDN", "eFDN", "MN>8", "MN>19", "FDNF", "FDA", "Nel", "NDT", "EE", "EE Insat",
            "Cinzas", "CNF", "Amido", "kd Amid", "Mor", "Ca", "P", "Mg",
            "K", "S", "Na", "Cl", "Co", "Cu", "Mn", "Zn", "Se", "I",
            "A", "D", "E", "Cromo", "Biotina", "Virginiamicina", "Monensina",
            "Levedura", "Lisina", "Metionina", "dFDNp48h", "dAmido_7h"
        }

        Dim comboBoxes As ComboBox() = {cbxAval01, cbxAval02, cbxAval03, cbxAval04}

        For Each cb As ComboBox In comboBoxes
            cb.Items.Clear()
            cb.Items.AddRange(analises)
        Next
    End Sub
    'cbx1
    Private Sub cbxAval01_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxAval01.SelectedIndexChanged
        AtualizaGrafico(cbxAval01, lblResult01, pnlSet01)
    End Sub
    'cbx2
    Private Sub cbxAval02_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxAval02.SelectedIndexChanged
        AtualizaGrafico(cbxAval02, lblResult02, pnlSet02)
    End Sub
    'cbx3
    Private Sub cbxAval03_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxAval03.SelectedIndexChanged
        AtualizaGrafico(cbxAval03, lblResult03, pnlSet03)
    End Sub
    'cbx4
    Private Sub cbxAval04_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxAval04.SelectedIndexChanged
        AtualizaGrafico(cbxAval04, lblResult04, pnlSet04)
    End Sub
    'Atualizar os pontos no grafico de volumosos 
    Private Sub AtualizaGrafico(cbx As ComboBox, lbl As Label, pnl As Panel)
        Dim ponto As Double = 0

        For i As Integer = 0 To dtgVolumoso.RowCount - 1
            If dtgVolumoso.Rows(i).Cells(0).Value.ToString = cbx.Text Then
                Dim valor As Double = Convert.ToDouble(dtgVolumoso.Rows(i).Cells(1).Value)
                ponto = valor * 3.7
                lbl.Text = valor & " %"
                pnl.Location = New Point(CInt(ponto), 5)
                Exit For ' achou, pode sair do loop
            End If
        Next
    End Sub
    'Configurar a tabela de volumosos
    Private Sub ConfGridVolumoso()

        On Error Resume Next
        With Me.dtgVolumoso

            .DefaultCellStyle.ForeColor = Color.FromArgb(90, 90, 90)
            .DefaultCellStyle.BackColor = Color.White
            '.ColumnHeadersDefaultCellStyle.Font = New Font("Inter", 9, FontStyle.Bold)
            .DefaultCellStyle.Font = New Font("Inter", 8.5, FontStyle.Bold)
            '.ColumnHeadersVisible = True
            '.Columns(0).Visible = False
            .Columns(0).Width = 240
            .Columns(0).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns(1).Width = 102
            .Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter ' voltar alimentos para esquerda

        End With

    End Sub
    'Ao se selecionados atravez do cbx atualiza os valores
    Private Sub dtgVol_Paint(sender As Object, e As PaintEventArgs) Handles dtgVol.Paint

        DadosTabVolumoso()
        AtualizaGrafico(cbxAval01, lblResult01, pnlSet01)
        AtualizaGrafico(cbxAval02, lblResult02, pnlSet02)
        AtualizaGrafico(cbxAval03, lblResult03, pnlSet03)
        AtualizaGrafico(cbxAval04, lblResult04, pnlSet04)
    End Sub
    'Ao selecionaro volumoso a ser analizado a tabela de volumoso será populada
    Private Sub DadosTabVolumoso()
        'Dim item As String
        'Dim ponto As Double

        For i As Integer = 0 To dtgVol.RowCount() - 1
            If dtgVol.Rows(i).Cells(1).Value.ToString = cbxVolumoso.Text Then
                lblAlimento.Text = dtgVol.Rows(i).Cells(1).Value
                dtgVolumoso.Rows(0).Cells(1).Value = dtgVol.Rows(i).Cells(2).Value
                dtgVolumoso.Rows(1).Cells(1).Value = dtgVol.Rows(i).Cells(3).Value
                dtgVolumoso.Rows(2).Cells(1).Value = dtgVol.Rows(i).Cells(4).Value
                dtgVolumoso.Rows(3).Cells(1).Value = dtgVol.Rows(i).Cells(5).Value
                dtgVolumoso.Rows(4).Cells(1).Value = dtgVol.Rows(i).Cells(6).Value
                dtgVolumoso.Rows(5).Cells(1).Value = dtgVol.Rows(i).Cells(7).Value
                dtgVolumoso.Rows(6).Cells(1).Value = dtgVol.Rows(i).Cells(8).Value
                dtgVolumoso.Rows(7).Cells(1).Value = dtgVol.Rows(i).Cells(9).Value
                dtgVolumoso.Rows(8).Cells(1).Value = dtgVol.Rows(i).Cells(10).Value
                dtgVolumoso.Rows(9).Cells(1).Value = dtgVol.Rows(i).Cells(11).Value
                dtgVolumoso.Rows(10).Cells(1).Value = dtgVol.Rows(i).Cells(12).Value
                dtgVolumoso.Rows(11).Cells(1).Value = dtgVol.Rows(i).Cells(13).Value
                dtgVolumoso.Rows(12).Cells(1).Value = dtgVol.Rows(i).Cells(14).Value
                dtgVolumoso.Rows(13).Cells(1).Value = dtgVol.Rows(i).Cells(15).Value
                dtgVolumoso.Rows(14).Cells(1).Value = dtgVol.Rows(i).Cells(16).Value
                dtgVolumoso.Rows(15).Cells(1).Value = dtgVol.Rows(i).Cells(17).Value
                dtgVolumoso.Rows(16).Cells(1).Value = dtgVol.Rows(i).Cells(18).Value
                dtgVolumoso.Rows(17).Cells(1).Value = dtgVol.Rows(i).Cells(19).Value
                dtgVolumoso.Rows(18).Cells(1).Value = dtgVol.Rows(i).Cells(20).Value
                dtgVolumoso.Rows(19).Cells(1).Value = dtgVol.Rows(i).Cells(21).Value
                dtgVolumoso.Rows(20).Cells(1).Value = dtgVol.Rows(i).Cells(22).Value
                dtgVolumoso.Rows(21).Cells(1).Value = dtgVol.Rows(i).Cells(23).Value
                dtgVolumoso.Rows(22).Cells(1).Value = dtgVol.Rows(i).Cells(24).Value
                dtgVolumoso.Rows(23).Cells(1).Value = dtgVol.Rows(i).Cells(25).Value
                dtgVolumoso.Rows(24).Cells(1).Value = dtgVol.Rows(i).Cells(26).Value
                dtgVolumoso.Rows(25).Cells(1).Value = dtgVol.Rows(i).Cells(27).Value
                dtgVolumoso.Rows(26).Cells(1).Value = dtgVol.Rows(i).Cells(28).Value
                dtgVolumoso.Rows(27).Cells(1).Value = dtgVol.Rows(i).Cells(29).Value
                dtgVolumoso.Rows(28).Cells(1).Value = dtgVol.Rows(i).Cells(30).Value
                dtgVolumoso.Rows(29).Cells(1).Value = dtgVol.Rows(i).Cells(31).Value
                dtgVolumoso.Rows(30).Cells(1).Value = dtgVol.Rows(i).Cells(32).Value
                dtgVolumoso.Rows(31).Cells(1).Value = dtgVol.Rows(i).Cells(33).Value
                dtgVolumoso.Rows(32).Cells(1).Value = dtgVol.Rows(i).Cells(34).Value
                dtgVolumoso.Rows(33).Cells(1).Value = dtgVol.Rows(i).Cells(35).Value
                dtgVolumoso.Rows(34).Cells(1).Value = dtgVol.Rows(i).Cells(36).Value
                dtgVolumoso.Rows(35).Cells(1).Value = dtgVol.Rows(i).Cells(37).Value
                dtgVolumoso.Rows(36).Cells(1).Value = dtgVol.Rows(i).Cells(38).Value
                dtgVolumoso.Rows(37).Cells(1).Value = dtgVol.Rows(i).Cells(39).Value
                dtgVolumoso.Rows(38).Cells(1).Value = dtgVol.Rows(i).Cells(40).Value
                dtgVolumoso.Rows(39).Cells(1).Value = dtgVol.Rows(i).Cells(41).Value
                dtgVolumoso.Rows(40).Cells(1).Value = dtgVol.Rows(i).Cells(42).Value
                dtgVolumoso.Rows(41).Cells(1).Value = dtgVol.Rows(i).Cells(43).Value
                dtgVolumoso.Rows(42).Cells(1).Value = dtgVol.Rows(i).Cells(44).Value
                dtgVolumoso.Rows(43).Cells(1).Value = dtgVol.Rows(i).Cells(45).Value

            End If
        Next

    End Sub
    'Ao abrir uma dieta a tabela de volumosos será populada de acordo com o id da dieta escolhida
    Private Sub PreencherGridVol()
        Dim ds As New DataSet
        'Dim dt As New DataTable
        For Each row As DataGridViewRow In dtgAlimentoNome.Rows
            dt4.Rows.Add(row.Cells(1).Value, row.Cells(2).Value, row.Cells(3).Value, row.Cells(4).Value, row.Cells(5).Value, row.Cells(6).Value, row.Cells(7).Value, row.Cells(8).Value, row.Cells(9).Value,
                        row.Cells(10).Value, row.Cells(11).Value, row.Cells(12).Value, row.Cells(13).Value, row.Cells(14).Value, row.Cells(15).Value, row.Cells(16).Value, row.Cells(17).Value, row.Cells(18).Value,
                        row.Cells(19).Value, row.Cells(20).Value, row.Cells(21).Value, row.Cells(22).Value, row.Cells(23).Value, row.Cells(24).Value, row.Cells(25).Value, row.Cells(26).Value, row.Cells(27).Value, row.Cells(28).Value,
                        row.Cells(29).Value, row.Cells(30).Value, row.Cells(31).Value, row.Cells(32).Value, row.Cells(33).Value, row.Cells(34).Value, row.Cells(35).Value, row.Cells(36).Value, row.Cells(37).Value,
row.Cells(38).Value, row.Cells(39).Value, row.Cells(40).Value, row.Cells(41).Value, row.Cells(42).Value, row.Cells(43).Value, row.Cells(44).Value, row.Cells(45).Value, row.Cells(46).Value, row.Cells(47).Value,
row.Cells(48).Value, row.Cells(49).Value, row.Cells(50).Value, row.Cells(51).Value, row.Cells(52).Value, row.Cells(53).Value, row.Cells(54).Value, row.Cells(55).Value, row.Cells(56).Value, row.Cells(57).Value,
row.Cells(58).Value, row.Cells(59).Value, row.Cells(60).Value, row.Cells(61).Value, row.Cells(62).Value, row.Cells(63).Value, row.Cells(64).Value, row.Cells(65).Value, row.Cells(66).Value, row.Cells(67).Value,
row.Cells(68).Value, row.Cells(69).Value, row.Cells(70).Value, row.Cells(71).Value, row.Cells(72).Value, row.Cells(73).Value, row.Cells(74).Value, row.Cells(75).Value, row.Cells(76).Value) ', row.Cells(77).Value)

        Next
        dtgVol.DataSource = dt4

    End Sub
    'Formato dos dados do dtgvolumoso
    Private Sub dtgvolumoso_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dtgVolumoso.CellFormatting
        If e.ColumnIndex = 1 Then 'AndAlso IsNumeric(e.Value) 
            If IsNumeric(e.Value) Then
                e.Value = Format(CDbl(e.Value), "0.00")
                e.FormattingApplied = True
            End If
        End If
    End Sub
    'Abrir o painel de analise de volumoso
    Private Sub btnAnaliseVolum_Click(sender As Object, e As EventArgs) Handles btnAnaliseVolum.Click
        AnaliseVol()
        pnlAnaliseVolum.BringToFront()
        pnlAnaliseVolum.Location = New Point(274, 41)
        pnlAnaliseVolum.Visible = True
    End Sub
    'Filtrar somente volumosos
    Private Sub AnaliseVol()
        Dim totalLinhas As Integer = dtgVol.Rows.Count

        If totalLinhas > 0 Then
            Try
                ' Aplica filtro ao DataTable que está vinculado ao DataGridView
                Dim dt As DataTable = TryCast(dtgVol.DataSource, DataTable)
                If dt IsNot Nothing Then
                    dt.DefaultView.RowFilter = "AlimentoFamilia LIKE '%Gramíneas e Leguminosas%' OR AlimentoFamilia LIKE '%Silagens%'"
                End If
            Catch ex As Exception
                MessageBox.Show("Erro ao filtrar volumosos: " & ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If

        ' Recarrega dados relacionados
        dtgVolumoso.DataSource = dt2
        DadosTabVolumoso()

        ' Configura combobox dos avaliadores com índices padrão
        TabCbx()
        If cbxAval01.Items.Count > 0 Then cbxAval01.SelectedIndex = 0
        If cbxAval02.Items.Count > 1 Then cbxAval02.SelectedIndex = 1
        If cbxAval03.Items.Count > 2 Then cbxAval03.SelectedIndex = 2
        If cbxAval04.Items.Count > 3 Then cbxAval04.SelectedIndex = 3

        ' Atualiza o ComboBox de volumosos com os dados filtrados
        cbxVolumoso.DataSource = dtgVol.DataSource
        cbxVolumoso.ValueMember = "Alimento"
        'cbxVolumoso.DisplayMember = "Alimento" ' Descomente se quiser exibir o nome
        'cbxVolumoso.SelectedIndex = 0
        ' Atualiza o DataGridView que mostra dados dos volumosos
        'dtgVolumoso.DataSource = dt2
        ConfGridVolumoso()
    End Sub
    'Fechar o painel de analise de volumoso
    Private Sub btnXAnaliseVolum_Click(sender As Object, e As EventArgs) Handles btnXAnaliseVolum.Click
        txtBuscaAlim.Text = ""
        'analise = ""
        pnlAnaliseVolum.Visible = False
        'dtgVol.DataSource = Nothing

    End Sub

    'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX               GRAFICOS            XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
    'Grafico de PARTICIPAÇÃO NUTRICIONAL
    Dim item As String
    Private Sub GraficoFormula()
        Dim dt As New DataTable
        dt.Columns.Add("Cor")
        dt.Columns.Add("Alimento")
        dt.Columns.Add("item")

        chtFormula.Series.Clear()
        chtFormula.Titles.Clear()
        'forragem1 = ""
        Dim qtd(19) As Double
        Dim nomeAli(19) As String
        Dim cor() As String = {
     "CornflowerBlue", "Orange", "Crimson", "DarkBlue", "DarkGray",
     "MidnightBlue", "Moccasin", "DodgerBlue", "IndianRed", "RoyalBlue",
     "NavajoWhite", "SlateGray", "DarkSalmon", "Peru", "LightSlateGray",
     "CornflowerBlue", "SandyBrown", "FireBrick", "DarkCyan", "Gray"
 }

        ' Calcular qtd total de cada avaliador
        Dim qtdTotal As Double
        For Each row1 As DataGridViewRow In dtgAlimentosDieta.Rows

            qtdTotal += row1.Cells(item).Value

        Next
        Dim x As Integer = dtgAlimentosDieta.Rows.Count
        Dim n As Integer
        For i As Integer = 0 To dtgAlimentosDieta.Columns.Count - 1
            For Each row As DataGridViewRow In dtgAlimentosDieta.Rows
                'If dtgAlimentosDieta.Columns(i).ColumnName = item Then
                Dim nameCol As String
                nameCol = dtgAlimentosDieta.Columns(i).Name
                If nameCol = item Then
                    n = 0
                    While x > 0
                        qtd(n) = (dtgAlimentosDieta.Rows(n).Cells(i).Value / qtdTotal) * 100
                        'qtd(n) = dtgAlimentosDieta.Rows(n).Cells(i).Value.ToString
                        nomeAli(n) = dtgAlimentosDieta.Rows(n).Cells(3).Value.ToString
                        dt.Rows.Add(" ", (nomeAli(n)), Format(qtd(n), "0.00") & "%").ToString()

                        dtgLegGraf.DataSource = dt

                        'dtgLegGraf.Rows(n).Cells(i).Style.ForeColor = Color.FromArgb(90, 90, 90)
                        'dtgLegGraf.Rows(n).Cells(3).Style.ForeColor = Color.DarkCyan
                        dtgLegGraf.Rows(n).Cells(0).Style.BackColor = Color.RoyalBlue
                        dtgLegGraf.Rows(n).Cells(0).Style.BackColor = Color.FromName(cor(n))

                        n = n + 1
                        x = x - 1

                    End While

                End If
            Next
        Next

        If qtdTotal = 0 Then
            dtgLegGraf.Visible = False
            lblItemGrafico.Text = item
            lblQtdItemGraf.Text = "0,00"
            Exit Sub
        Else
            dtgLegGraf.Visible = True
        End If

        On Error Resume Next

        Dim title = New Title()
        title.Font = (New Font("Arial", 8, FontStyle.Bold))
        title.ForeColor = Color.Black
        'title.Text = forragem1
        chtFormula.Titles.Add(title)
        chtFormula.Series.Add(item)

        On Error Resume Next
        With chtFormula.Series(item)
            .ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie
            .BorderWidth = 2
            .Palette = ChartColorPalette.BrightPastel

            x = dtgAlimentosDieta.Rows.Count
            n = 0
            While x > 0
                .Points.AddY(qtd(n))
                n = n + 1
                x = x - 1
            End While

            'Tamanho
            '.Size = New Size(Size.Width, 250)
            ' .Size = New Size(Size.Height, 165)



        End With

        With chtFormula.ChartAreas("ChartArea1")
            'Eixo X

            chtFormula.ChartAreas(0).AxisX.LabelStyle.Angle = 1
            '.AxisX.Title = qtd(0)
            .AxisX.TitleFont = (New Font("Arial", 7, FontStyle.Bold))
            .AxisX.TitleForeColor = Color.Black
            'Eixo Y
            '.AxisY.Title = "Particulas em (%)"
            .AxisY.TitleFont = (New Font("Arial", 7, FontStyle.Bold))
            .AxisY.TitleForeColor = Color.Black


        End With

        ' chtFormula.Series(0).IsValueShownAsLabel = False
        chtFormula.Series(0).IsVisibleInLegend = False
        'Preencher cabeçalho
        lblItemGrafico.Text = item
        For i As Integer = 0 To dtgAvaliadores.Columns.Count - 1
            For Each row As DataGridViewRow In dtgAvaliadores.Rows
                Dim ava() As String = row.Cells(2).Value.Split(" ")

                If ava(0) = item Then
                    'lblQtdItemGraf.Text = row.Cells(11).Value.ToString("F2") & "% (Total da dieta)"
                    Dim qtItGraf As Double
                    qtItGraf = row.Cells(11).Value
                    lblQtdItemGraf.Text = qtItGraf.ToString("F2") & "% (Total da dieta)"
                End If
            Next
        Next

        With dtgLegGraf

            .DefaultCellStyle.BackColor = Color.White
            .DefaultCellStyle.Font = New Font("Inter", 8.5, FontStyle.Bold)
            .Columns(0).Width = 20
            .Columns(1).Width = 265
            .Columns(2).Width = 60
        End With
        For i As Integer = 0 To dtgLegGraf.Rows.Count - 1
            If qtd(i) = 0 Then
                dtgLegGraf.Rows(i).Visible = False
            End If
        Next

        'For Each row As DataGridViewRow In dtgLegGraf.Rows
        '    Dim vlr As String = row.Cells(2).Value
        '    Dim vlr1() As String = vlr.Split("")
        '    If row.Cells(2).Value = 0 Then
        '        row.Visible = False
        '    End If

        'Next
        ' chtFormula.Update()
        chtFormula.DataBind()
        'chtFormula.Visible = True
    End Sub
    'Grafico de CUSTO
    Private Sub GraficoCusto()
        Dim dt As New DataTable
        dt.Columns.Add("Cor")
        dt.Columns.Add("Alimento")
        dt.Columns.Add("item")

        chtCusto.Series.Clear()
        chtCusto.Titles.Clear()
        'forragem1 = ""
        Dim qtd(19) As Double
        Dim nomeAli(19) As String
        Dim cor() As String = {
   "CornflowerBlue", "Orange", "Crimson", "DarkBlue", "DarkGray",
   "MidnightBlue", "Moccasin", "DodgerBlue", "IndianRed", "RoyalBlue",
   "NavajoWhite", "SlateGray", "DarkSalmon", "Peru", "LightSlateGray",
   "CornflowerBlue", "SandyBrown", "FireBrick", "DarkCyan", "Gray"
}

        'Dim qtd1 As Double
        'Dim qtd2 As Double
        Dim x As Integer = dtgAlimentosDieta.Rows.Count
        Dim n As Integer
        For i As Integer = 0 To dtgAlimentosDieta.Columns.Count - 1
            For Each row As DataGridViewRow In dtgAlimentosDieta.Rows
                'If dtgAlimentosDieta.Columns(i).ColumnName = item Then
                Dim nameCol As String
                nameCol = dtgAlimentosDieta.Columns(i).Name
                If nameCol = item Then
                    n = 0
                    While x > 0
                        If varms = False Then
                            If varD1 = True Then
                                qtd(n) = ((dtgAlimentosDieta.Rows(n).Cells(66).Value * dtgAlimentosDieta.Rows(n).Cells(67).Value) / totalValor) * 100
                            ElseIf varD2 = True Then
                                qtd(n) = ((dtgAlimentosDieta.Rows(n).Cells(66).Value * dtgAlimentosDieta.Rows(n).Cells(68).Value) / totalValor2) * 100
                            End If
                        ElseIf varms = True Then
                            If varD1 = True Then
                                qtd(n) = (((dtgAlimentosDieta.Rows(n).Cells(66).Value * dtgAlimentosDieta.Rows(n).Cells(67).Value) / dtgAlimentosDieta.Rows(n).Cells(4).Value) * 100) / totalValor * 100
                            ElseIf varD2 = True Then
                                qtd(n) = (((dtgAlimentosDieta.Rows(n).Cells(66).Value * dtgAlimentosDieta.Rows(n).Cells(68).Value) / dtgAlimentosDieta.Rows(n).Cells(4).Value) * 100) / totalValor * 100
                            End If

                        End If


                        nomeAli(n) = dtgAlimentosDieta.Rows(n).Cells(3).Value.ToString
                        dt.Rows.Add(" ", (nomeAli(n)), Format(qtd(n), "0.00") & "%").ToString()

                        dtgCusto.DataSource = dt

                        'dtgLegGraf.Rows(n).Cells(i).Style.ForeColor = Color.FromArgb(90, 90, 90)
                        'dtgLegGraf.Rows(n).Cells(3).Style.ForeColor = Color.DarkCyan
                        'dtgLegGraf.Rows(n).Cells(0).Style.BackColor = Color.moc
                        dtgCusto.Rows(n).Cells(0).Style.BackColor = Color.FromName(cor(n))

                        n = n + 1
                        x = x - 1

                    End While

                End If
            Next
        Next
        If lblTotalVrD1.Text = 0 Then
            dtgCusto.Visible = False
            'lblItemGrafico.Text = item
            lblCustoGrafico.Text = lblTotalVrD1.Text & " (Vaca dia)"
            Exit Sub
        Else
            dtgCusto.Visible = True
        End If
        On Error Resume Next

        Dim title = New Title()
        title.Font = (New Font("Arial", 8, FontStyle.Bold))
        title.ForeColor = Color.Black
        'title.Text = forragem1
        chtCusto.Titles.Add(title)
        chtCusto.Series.Add(item)

        On Error Resume Next
        With chtCusto.Series(item)
            .ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie
            .BorderWidth = 2
            .Palette = ChartColorPalette.BrightPastel

            x = dtgAlimentosDieta.Rows.Count
            n = 0
            While x > 0
                .Points.AddY(qtd(n))
                n = n + 1
                x = x - 1
            End While

            'Tamanho
            '.Size = New Size(Size.Width, 250)
            ' .Size = New Size(Size.Height, 165)



        End With

        With chtCusto.ChartAreas("ChartArea1")
            'Eixo X

            chtCusto.ChartAreas(0).AxisX.LabelStyle.Angle = 1
            '.AxisX.Title = qtd(0)
            .AxisX.TitleFont = (New Font("Arial", 7, FontStyle.Bold))
            .AxisX.TitleForeColor = Color.Black
            'Eixo Y
            '.AxisY.Title = "Particulas em (%)"
            .AxisY.TitleFont = (New Font("Arial", 7, FontStyle.Bold))
            .AxisY.TitleForeColor = Color.Black


        End With

        'chtCusto.Series(0).IsValueShownAsLabel = False
        chtCusto.Series(0).IsVisibleInLegend = False
        With dtgCusto

            .DefaultCellStyle.BackColor = Color.White
            .DefaultCellStyle.Font = New Font("Inter", 8.5, FontStyle.Bold)
            .Columns(0).Width = 20
            .Columns(1).Width = 265
            .Columns(2).Width = 60
        End With
        'For i As Integer = 0 To dtgCusto.Rows.Count - 1
        '    If qtd(i) = 0 Then
        '        dtgLegGraf.Rows(i).Visible = False
        '    End If
        'Next

        lblCustoGrafico.Text = lblTotalVrD1.Text & " (Vaca dia)"
        ' chtCusto.Update()
        chtCusto.DataBind()
        'chtCusto.Visible = True
    End Sub
    'Popular CbxItens
    Private Sub PreencherCbxItens()
        Dim itens As String() = {
            "MS", "PB", "PDR", "PNDR", "FDN", "eFDN", "MN>8", "MN>19", "FDNF", "FDA",
            "Nel", "NDT", "EE", "EE Insat", "Cinzas", "CNF", "Amido", "kd Amid", "MOR",
            "Ca", "P", "Mg", "K", "S", "Na", "Cl", "Co", "Cu", "Mn", "Zn", "Se", "I",
            "A", "D", "E", "Cromo", "Biotina", "Virginiamicina", "Monensina", "Levedura",
            "Arginina", "Histidina", "Isoleucina", "Leucina", "Lisina", "Metionina",
            "Fenilalanina", "Treonina", "Triptofano", "Valina", "dFDNp48h", "dAmido7h", "TTNDFD"
        }

        cbxItens.Items.Clear() ' Importante limpar antes de adicionar
        cbxItens.Items.AddRange(itens)
    End Sub
    'Ao selecionar o item
    Private Sub cbxItens_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxItens.SelectedIndexChanged

        item = Me.cbxItens.Text
        GraficoFormula()
        GraficoCusto()
    End Sub
    'Abrir painel graficos
    Private Sub btnPartNutr_Click_1(sender As Object, e As EventArgs) Handles btnPartNutr.Click


        Dim vlr As Double
        For Each row As DataGridViewRow In dtgAlimentosDieta.Rows
            vlr += row.Cells(67).Value ' + row.Cells(68).Value
        Next
        If vlr > 0 Then
            pnlGraficos.Visible = True
            pnlGraficos.Location = New Point(300, 2)
            Me.cbxItens.Text = "MS"
            item = Me.cbxItens.Text

            GraficoFormula()
            GraficoCusto()
        End If

    End Sub
    'Fechar painel graficos
    Private Sub btnSair_Click(sender As Object, e As EventArgs) Handles btnSair.Click
        pnlGraficos.Visible = False

    End Sub
    'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX             PRÉ-MISTURA          XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
    'Configurar dtgEdtPremix
    Private Sub ConfigGridAlimentosPremix()
        For Each columns As DataGridViewColumn In Me.dtgAlimentosPremix.Columns
            dtgAlimentosPremix.Columns(columns.Index).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter ' centralizar as celulas

        Next

        On Error Resume Next
        With Me.dtgAlimentosPremix
            Dim x As Integer = .Rows.Count


            .DefaultCellStyle.ForeColor = Color.FromArgb(90, 90, 90)
            .DefaultCellStyle.BackColor = Color.White
            '.ColumnHeadersDefaultCellStyle.Font = New Font("Inter", 8, FontStyle.Bold)
            .DefaultCellStyle.Font = New Font("Inter", 8.5, FontStyle.Bold)
            ' if > 7 
            .Columns(0).Visible = False
            .Columns(1).Width = 399
            .Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft ' voltar alimentos para esquerda
            For i = 2 To 68
                .Columns(i).Visible = False
            Next
            .Columns(69).Width = 134  ' premix
            '.Columns(70).Width = 132   'pct premix

            If x > 7 Then
                .Columns(70).Width = 113
            Else
                .Columns(70).Width = 131
            End If

            .Columns(70).DefaultCellStyle.BackColor = Color.WhiteSmoke
            .Columns(71).DisplayIndex = 60
            .Columns(71).Width = 133
            .Columns(72).Visible = False
            .Columns(73).Visible = False
            .Columns(74).Visible = False
            .Columns(75).Visible = False
            .Columns(76).Visible = False

        End With
    End Sub

    Private Sub ConfigGridEdtPremix()
        For Each columns As DataGridViewColumn In Me.dtgAlimentosPremix.Columns
            dtgAlimentosPremix.Columns(columns.Index).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter ' centralizar as celulas

        Next

        On Error Resume Next
        With Me.dtgEdtPremix

            .DefaultCellStyle.ForeColor = Color.FromArgb(90, 90, 90)
            .DefaultCellStyle.BackColor = Color.White
            '.ColumnHeadersDefaultCellStyle.Font = New Font("Inter", 8, FontStyle.Bold)
            .DefaultCellStyle.Font = New Font("Inter", 8.5, FontStyle.Bold)

            .Columns(0).Visible = False
            .Columns(1).Visible = False
            .Columns(2).Width = 399
            .Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft ' voltar alimentos para esquerda

            For i = 3 To 64
                .Columns(i).Visible = False
            Next

            '.Columns(3).Visible = False
            '.Columns(4).Visible = False
            '.Columns(5).Visible = False
            '.Columns(6).Visible = False
            '.Columns(7).Visible = False
            '.Columns(8).Visible = False
            '.Columns(9).Visible = False
            '.Columns(10).Visible = False
            '.Columns(11).Visible = False
            '.Columns(12).Visible = False
            '.Columns(13).Visible = False
            '.Columns(14).Visible = False
            '.Columns(15).Visible = False
            '.Columns(16).Visible = False
            '.Columns(17).Visible = False
            '.Columns(18).Visible = False
            '.Columns(19).Visible = False
            '.Columns(20).Visible = False
            '.Columns(21).Visible = False
            '.Columns(22).Visible = False
            '.Columns(23).Visible = False
            '.Columns(24).Visible = False
            '.Columns(25).Visible = False
            '.Columns(26).Visible = False
            '.Columns(27).Visible = False
            '.Columns(28).Visible = False
            '.Columns(29).Visible = False
            '.Columns(30).Visible = False
            '.Columns(31).Visible = False
            '.Columns(32).Visible = False
            '.Columns(33).Visible = False
            '.Columns(34).Visible = False
            '.Columns(35).Visible = False
            '.Columns(36).Visible = False
            '.Columns(37).Visible = False
            '.Columns(38).Visible = False
            '.Columns(39).Visible = False
            '.Columns(40).Visible = False
            '.Columns(41).Visible = False
            '.Columns(42).Visible = False
            '.Columns(43).Visible = False
            '.Columns(44).Visible = False
            '.Columns(45).Visible = False
            '.Columns(46).Visible = False
            '.Columns(47).Visible = False
            '.Columns(48).Visible = False
            '.Columns(49).Visible = False
            '.Columns(50).Visible = False
            '.Columns(51).Visible = False
            '.Columns(52).Visible = False
            '.Columns(53).Visible = False
            '.Columns(54).Visible = False
            '.Columns(55).Visible = False
            '.Columns(56).Visible = False
            '.Columns(57).Visible = False
            '.Columns(58).Visible = False
            '.Columns(59).Visible = False        'custo
            ''.Columns(59).DefaultCellStyle.BackColor = Color.WhiteSmoke
            '.Columns(60).Visible = False    ' qtd d1
            '.Columns(61).Visible = False    ' qtd d2
            ''.Columns(61).Visible = False
            '.Columns(62).Visible = False
            '.Columns(63).Visible = False
            '.Columns(64).Visible = False
            '.Columns(65).Visible = False
            '.Columns(66).Visible = False
            '.Columns(67).Visible = False
            .Columns(65).Width = 134

            .Columns(66).Width = 132   ' premix
            .Columns(67).Width = 132   'pct premix
            '.Columns(70).DefaultCellStyle.BackColor = Color.WhiteSmoke
            '.Columns(71).DisplayIndex = 60
            '.Columns(71).Width = 132
            .Columns(68).Visible = False
            .Columns(69).Visible = False
            '.Columns(74).Visible = False
            '.Columns(75).Visible = False
            '.Columns(76).Visible = False

        End With
    End Sub
    'Calculos da tela de PRÉ-MISTURA
    Private Sub CalcularPremix()
        dtgAlimentosPremix.Rows(1).Cells(70).Value = "0,00"
        Dim qtdPremix As Double = 0
        Dim pctPremix As Double = 0
        Dim pctTotalPremix As Double = 0
        Dim qtdTotalDisponivel As Double = 0
        '63 = 60*62/100
        Try
            If pnlPreMix.Visible = True Then
                For Each row As DataGridViewRow In dtgAlimentosPremix.Rows
                    ' row.Cells(70).Value = "0,00"
                    qtdPremix += row.Cells(69).Value

                Next
                lblQtdTotalPremix.Text = qtdPremix
                For i As Integer = 0 To dtgAlimentosPremix.RowCount - 1
                    dtgAlimentosPremix.Rows(i).Cells(72).Value = lblQtdTotalPremix.Text
                    pctPremix = dtgAlimentosPremix.Rows(i).Cells(69).Value / dtgAlimentosPremix.Rows(i).Cells(72).Value * 100
                    dtgAlimentosPremix.Rows(i).Cells(70).Value = pctPremix 'Format(pctPremix, "0.00")
                    pctTotalPremix += dtgAlimentosPremix.Rows(i).Cells(70).Value
                    dtgAlimentosPremix.Rows(i).Cells(71).Value = dtgAlimentosPremix.Rows(i).Cells(65).Value - dtgAlimentosPremix.Rows(i).Cells(69).Value
                    qtdTotalDisponivel += dtgAlimentosPremix.Rows(i).Cells(71).Value
                    If dtgAlimentosPremix.Rows(i).Cells(69).Value > 0 Then
                        dtgAlimentosPremix.Rows(i).Cells(71).Style.ForeColor = Color.Green
                    End If
                    'dtgAlimentosPremix.Rows(i).Cells(69).Value = dtgAlimentosPremix.Rows(i).Cells(69).Value * Format(Math.Round(1, 2), "0.00")
                    'dtgAlimentosPremix.Rows(i).Cells(69).Value = Format(dtgAlimentosPremix.Rows(i).Cells(69).Value * Math.Round(1, 2), "0.00")
                    'dtgAlimentosPremix.Rows(i).Cells(71).Value = Format(dtgAlimentosPremix.Rows(i).Cells(71).Value * Math.Round(1, 2), "0.00")

                    'dtgAlimentosPremix.Rows(i).Cells(69).Value = dtgAlimentosPremix.Rows(i).Cells(69).Value.ToString("0.00")
                    'dtgAlimentosPremix.Rows(i).Cells(71).Value = dtgAlimentosPremix.Rows(i).Cells(71).Value.ToString("0.00")

                    Dim valor2 As Double = 0

                    ' Verifica e converte a célula 2
                    If Not IsDBNull(dtgAlimentosPremix.Rows(i).Cells(70).Value) AndAlso IsNumeric(dtgAlimentosPremix.Rows(i).Cells(70).Value) Then
                        valor2 = Convert.ToDouble(dtgAlimentosPremix.Rows(i).Cells(70).Value)
                    End If
                    ' Corrigir valores inválidos
                    If Double.IsNaN(valor2) OrElse Double.IsInfinity(valor2) Then valor2 = 0
                    If Double.IsNaN(pctTotalPremix) OrElse Double.IsInfinity(pctTotalPremix) Then pctTotalPremix = 0

                    dtgAlimentosPremix.Rows(i).Cells(70).Value = valor2

                Next
                'Colorir numero caso esteja no premix
                For i As Integer = 0 To dtgAlimentosDieta.RowCount - 1
                    If dtgAlimentosPremix.Rows(i).Cells(69).Value > 0 Then
                        dtgAlimentosDieta.Rows(i).Cells(67).Style.ForeColor = Color.Green
                        'pctTotalPremix = 100
                    Else
                        dtgAlimentosDieta.Rows(i).Cells(67).Style.ForeColor = Color.FromArgb(90, 90, 90)
                        'pctTotalPremix = 0
                    End If
                Next
                lblQtdTotalPremix.Text = Format(qtdPremix, "0.00") & " Kg"
                'If pctTotalPremix = 99.99 Or pctTotalPremix = 99.98 Or pctTotalPremix = 100.01 Or pctTotalPremix = 100.02 Then
                'Dim x As Integer = dtgAlimentosPremix.Rows.Count
                'If x > 0 Then
                '    pctTotalPremix = 100
                'End If
                lblPctTotalPremix.Text = pctTotalPremix.ToString("F2") & " %"
                lblQtdTotalDisponivel.Text = qtdTotalDisponivel.ToString("F2") & " Kg"

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try

    End Sub

    ' Transformar a PRÉ-MISTURA em um unico ingrediente
    Private Sub SomarIngPreMix()
        totalMS = 0
        totalPB = 0
        totalPDR = 0
        totalPND = 0
        totalFDN = 0
        totaleFDN = 0
        'totaleFDN2 = 0
        totalMNmaior8 = 0
        totalMNmaior19 = 0
        totalFDNF = 0
        totalFDA = 0
        totalNEl = 0
        totalNDT = 0
        totalEE = 0
        totalEE_Insat = 0
        totalCinzas = 0
        totalCNF = 0
        totalAmido = 0
        totalkd_Amid = 0
        totalMor = 0
        totalCa = 0
        totalP = 0
        totalMg = 0
        totalK = 0
        totalS = 0
        totalNa = 0
        totalCl = 0
        totalCo = 0
        totalCu = 0
        totalMn = 0
        totalZn = 0
        totalSe = 0
        totalI = 0
        totalA = 0
        totalD = 0
        totalE = 0
        totalCromo = 0

        totalBiotina = 0
        totalVirginiamicina = 0
        totalMonensina = 0
        totalLevedura = 0

        totalArginina = 0
        totalHistidina = 0
        totalIsoleucina = 0
        totalLeucina = 0

        totalLisina = 0
        totalMetionina = 0

        pctFenilalanina = 0
        pctTreonina = 0
        pctTriptofano = 0
        pctValina = 0

        pctdFDNp48h = 0
        pctdAmido7h = 0

        totaldFDNp48h = 0
        totaldAmido7h = 0

        pctTTNDFD = 0

        Pers1 = 0
        Pers2 = 0
        Pers3 = 0
        Pers4 = 0
        Pers5 = 0
        Pers6 = 0
        Pers7 = 0
        Pers8 = 0
        Pers9 = 0

        Dim qtdProduto As Double
        Dim vproduto As Double = 0
        Try

            For Each row As DataGridViewRow In dtgAlimentosPremix.Rows
                If row.Cells(69).Value > 0 Then

                    totalMS += row.Cells(2).Value * row.Cells(69).Value   'quantidade de MS
                    totalPB += row.Cells(3).Value * row.Cells(69).Value   'quantidade de PB
                    totalPDR += row.Cells(4).Value * row.Cells(69).Value
                    totalPND += row.Cells(5).Value * row.Cells(69).Value
                    totalFDN += row.Cells(6).Value * row.Cells(69).Value
                    totaleFDN += row.Cells(7).Value * row.Cells(69).Value

                    totalMNmaior8 += row.Cells(8).Value * row.Cells(69).Value
                    totalMNmaior19 += row.Cells(9).Value * row.Cells(69).Value
                    totalFDNF += row.Cells(10).Value * row.Cells(69).Value
                    totalFDA += row.Cells(11).Value * row.Cells(69).Value
                    totalNEl += row.Cells(12).Value * row.Cells(69).Value
                    totalNDT += row.Cells(13).Value * row.Cells(69).Value
                    totalEE += row.Cells(14).Value * row.Cells(69).Value
                    totalEE_Insat += row.Cells(15).Value * row.Cells(69).Value
                    totalCinzas += row.Cells(16).Value * row.Cells(69).Value
                    totalCNF += row.Cells(17).Value * row.Cells(69).Value
                    totalAmido += row.Cells(18).Value * row.Cells(69).Value
                    totalkd_Amid += row.Cells(19).Value * row.Cells(69).Value

                    totalMor += row.Cells(20).Value * row.Cells(69).Value

                    totalCa += row.Cells(21).Value * row.Cells(69).Value
                    totalP += row.Cells(22).Value * row.Cells(69).Value
                    totalMg += row.Cells(23).Value * row.Cells(69).Value
                    totalK += row.Cells(24).Value * row.Cells(69).Value
                    totalS += row.Cells(25).Value * row.Cells(69).Value
                    totalNa += row.Cells(26).Value * row.Cells(69).Value
                    totalCl += row.Cells(27).Value * row.Cells(69).Value
                    totalCo += row.Cells(28).Value * row.Cells(69).Value
                    totalCu += row.Cells(29).Value * row.Cells(69).Value
                    totalMn += row.Cells(30).Value * row.Cells(69).Value
                    totalZn += row.Cells(31).Value * row.Cells(69).Value
                    totalSe += row.Cells(32).Value * row.Cells(69).Value
                    totalI += row.Cells(33).Value * row.Cells(69).Value
                    totalA += row.Cells(34).Value * row.Cells(69).Value
                    totalD += row.Cells(35).Value * row.Cells(69).Value
                    totalE += row.Cells(36).Value * row.Cells(69).Value
                    totalCromo += row.Cells(37).Value * row.Cells(69).Value

                    totalBiotina += row.Cells(38).Value * row.Cells(69).Value
                    totalVirginiamicina += row.Cells(39).Value * row.Cells(69).Value
                    totalMonensina += row.Cells(40).Value * row.Cells(69).Value
                    totalLevedura += row.Cells(41).Value * row.Cells(69).Value

                    totalArginina += row.Cells(42).Value * row.Cells(69).Value
                    totalHistidina += row.Cells(43).Value * row.Cells(69).Value
                    totalIsoleucina += row.Cells(44).Value * row.Cells(69).Value
                    totalLeucina += row.Cells(45).Value * row.Cells(69).Value

                    totalLisina += row.Cells(46).Value * row.Cells(69).Value
                    totalMetionina += row.Cells(47).Value * row.Cells(69).Value

                    pctFenilalanina += row.Cells(48).Value * row.Cells(69).Value
                    pctTreonina += row.Cells(49).Value * row.Cells(69).Value
                    pctTriptofano += row.Cells(50).Value * row.Cells(69).Value
                    pctValina += row.Cells(51).Value * row.Cells(69).Value
                    pctdFDNp48h += row.Cells(52).Value * row.Cells(69).Value
                    pctdAmido7h += row.Cells(53).Value * row.Cells(69).Value

                    pctTTNDFD += row.Cells(54).Value * row.Cells(69).Value

                    qtdProduto += row.Cells(69).Value
                    vproduto += row.Cells(69).Value * row.Cells(64).Value

                End If
            Next
            pctMS = totalMS / qtdProduto
            pctPB = totalPB / qtdProduto
            pctPDR = totalPDR / qtdProduto
            pctPND = totalPND / qtdProduto
            pctFDN = totalFDN / qtdProduto
            pcteFDN = totaleFDN / qtdProduto

            pctMNmaior8 = totalMNmaior8 / qtdProduto
            pctMNmaior19 = totalMNmaior19 / qtdProduto
            pctFDNF = totalFDNF / qtdProduto
            pctFDA = totalFDA / qtdProduto
            pctNel = totalNEl / qtdProduto
            pctNDT = totalNDT / qtdProduto
            pctEE = totalEE / qtdProduto
            pctEE_Insat = totalEE_Insat / qtdProduto
            pctCinzas = totalCinzas / qtdProduto
            pctCNF = totalCNF / qtdProduto
            pctAmido = totalAmido / qtdProduto
            pctkd_Amid = totalkd_Amid / qtdProduto

            pctMor = totalMor / qtdProduto

            pctCa = totalCa / qtdProduto
            pctP = totalP / qtdProduto
            pctMg = totalMg / qtdProduto
            pctK = totalK / qtdProduto
            pctS = totalS / qtdProduto
            pctNa = totalNa / qtdProduto
            pctCl = totalCl / qtdProduto
            pctCo = totalCo / qtdProduto
            pctCu = totalCu / qtdProduto
            pctMn = totalMn / qtdProduto
            pctZn = totalZn / qtdProduto
            pctSe = totalSe / qtdProduto
            pctI = totalI / qtdProduto
            pctA = totalA / qtdProduto
            pctD = totalD / qtdProduto
            pctE = totalE / qtdProduto
            pctCromo = totalCromo / qtdProduto

            pctBiotina = totalBiotina / qtdProduto
            pctVirginiamicina = totalVirginiamicina / qtdProduto
            pctMonensina = totalMonensina / qtdProduto
            pctLevedura = totalLevedura / qtdProduto

            pctArginina = totalArginina
            pctHistidina = totalHistidina
            pctIsoleucina = totalIsoleucina
            pctLeucina = totalLeucina

            pctLisina = totalLisina / qtdProduto
            pctMetionina = totalMetionina / qtdProduto

            pctFenilalanina = totalFenilalanina
            pctTreonina = totalTreonina
            pctTriptofano = totalTriptofano
            pctValina = totalValina
            pctTTNDFD = totalTTNDFD
            pctdFDNp48h = totaldFDNp48h / qtdProduto
            pctdAmido7h = totaldAmido7h / qtdProduto

            qtdProdutoPremix = Format(qtdProduto, "0.00")
            valorPremix = Format(vproduto / qtdProdutoPremix, "0.00")
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try

    End Sub

    Private Sub btnSalvarPremix_Click(sender As Object, e As EventArgs) Handles btnSalvarPremix.Click
        If txtNomePremix.Text = "" Then
            MsgBox("Você precisa dar um nome para a pré-mistura")
        Else
            Dim data As String
            data = Now.ToString("dd/MM/yyyy| HH:mm:ss")
            nomePremix = txtNomePremix.Text & " " & data
            For i As Integer = 0 To dtgAlimentosDieta.RowCount() - 1
                'If dtgAlimentosDieta.Rows(i).Cells(2).Value = "Pré-Mistura" Then
                '    dtgAlimentosDieta.Rows(i).Cells(1).Value.BackgroundImage(My.Resources.edit5)
                '    'dtgAlimentosDieta.Rows(i).Cells(1).Value.
                'End If
                dtgAlimentosDieta.Rows(i).Cells(67).Value = dtgAlimentosDieta.Rows(i).Cells(67).Value - dtgAlimentosPremix.Rows(i).Cells(69).Value
                'btnPreMix.Enabled = False
                'btnPreMix.BackgroundImage = My.Resources.premistura_of
            Next

            'For i0 As Integer = 0 To dtgAlimentosDieta.RowCount() - 1

            '    If dtgAlimentosDieta.Rows(i0).Cells(67).Value = 0 Then
            '        'dtgAlimentosDieta.Rows(i0).remove()

            '        dtgAlimentosDieta.Rows.Remove(dtgAlimentosDieta.Rows.Item(i0))
            '    End If

            'Next

            CadastrarPremistura()

            'ConfigGridAlimentosDieta()
            AdicPreemixDieta()
            PainelDieta()
            ConfigGridAlimentosDieta()
            'dtgAlimentosPremix.Rows.Clear()
            pnlPreMix.Visible = False
            'For Each row As DataGridViewRow In dtgAlimentosPremix.Rows
            '    row.Cells(69).Value = 0
            'Next
        End If
        For i As Integer = 0 To dtgAlimentosDieta.RowCount() - 1
            If dtgAlimentosDieta.Rows(i).Cells(2).Value = "Pré-Mistura" Then
                dtgAlimentosDieta.Rows(i).Cells(1).Value = My.Resources.edit5
            End If
        Next
        BuscarAlimentosMSMO()

        'Label23.Text = nomePremix
    End Sub

    Private Sub btnFecharAlimPremix_Click(sender As Object, e As EventArgs) Handles btnFecharAlimPremix.Click
        pnlPreMix.Visible = False
    End Sub

    Private Sub dtgAlimentosPremix_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dtgAlimentosPremix.CellClick
        For Each row As DataGridViewRow In dtgAlimentosPremix.Rows
            Dim vlr As Double
            vlr += row.Cells(69).Value
            If vlr = 0 Then
                row.Cells(70).Value = 0
            End If
        Next

        CalcularPremix()
        SomarIngPreMix()
        Label22.Text = dtgAlimentosPremix.CurrentCell.ColumnIndex
    End Sub
    ' preencher a pré-mistura na tab alimentosDieta
    Private Sub AdicPreemixDieta()

        Try

            dtTemp.Rows.Add("Pré-Mistura", txtNomePremix.Text, pctMS, pctPB, pctPDR, pctPND, pctFDN, pcteFDN, pctMNmaior8, pctMNmaior19, pctFDNF, pctFDA, pctNel, pctNDT, pctEE, pctEE_Insat, pctCinzas,
            pctCNF, pctAmido, pctkd_Amid, pctMor, pctCa, pctP, pctMg, pctK, pctS, pctNa, pctCl, pctCo, pctCu, pctMn, pctZn, pctSe, pctI, pctA, pctD, pctE, pctCromo, pctBiotina, pctVirginiamicina, pctMonensina,
            pctLevedura, pctArginina, pctHistidina, pctIsoleucina, pctLeucina, pctLisina, pctMetionina, pctFenilalanina, pctTreonina, pctTriptofano, pctValina, pctdFDNp48h, pctdAmido7h, pctTTNDFD, Pers1, Pers2,
            Pers3, Pers4, Pers5, Pers6, Pers7, Pers8, Pers9, valorPremix, qtdProdutoPremix, "0", "0", "0", "0", "0", "0") ', "3", "4", "5", "6", "7")

            dtgTemp.DataSource = dtTemp

            'Dim x As Integer
            'x = dtgTemp.Rows.Count
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
        ' somar 62 e inserir no 64

    End Sub

    'Salvar PRÉ-MISTURA na DB AlimentosMS
    Private Sub SalvarPremisturaMS()
        Dim sql As String
        Dim cmd As SQLiteCommand

        sql = "Insert into AlimentosMS (AlimentoFamilia,Alimento,MS,PB,PDR,PNDR,FDN,eFDN,MNmaior8,MNmaior19,FDNF,FDA,Nel,NDT,EE,EE_Insat,Cinzas,CNF,Amido,kd_Amido,MOR,Ca,P,Mg,K,S,Na,Cl,Co,Cu,Mn,Zn,Se,I,A,D,E,Cromo,Biotina,Virginiamicina,Monensina,Levedura,Arginina,Histidina,Isoleucina,Leucina,Lisina,Metionina,Fenilalanina,Treonina,Triptofano,Valina,dFDNp_48h,dAmido_7h,TTNDFD,Pers1,Pers2,Pers3,Pers4,Pers5,Pers6,Pers7,Pers8,Pers9,Chk) values (@AlimentoFamilia,@Alimento,@MS,@PB,@PDR,@PNDR,@FDN,@eFDN,@MNmaior8,@MNmaior19,@FDNF,@FDA,@Nel,@NDT,@EE,@EE_Insat,@Cinzas,@CNF,@Amido,@kd_Amido,@MOR,@Ca,@P,@Mg,@K,@S,@Na,@Cl,@Co,@Cu,@Mn,@Zn,@Se,@I,@A,@D,@E,@Cromo,@Biotina,@Virginiamicina,@Monensina,@Levedura,@Arginina,@Histidina,@Isoleucina,@Leucina,@Lisina,@Metionina,@Fenilalanina,@Treonina,@Triptofano,@Valina,@dFDNp_48h,@dAmido_7h,@TTNDFD,@Pers1,@Pers2,@Pers3,@Pers4,@Pers5,@Pers6,@Pers7,@Pers8,@Pers9,@Chk)"
        If txtNomePremix.Text <> "" Then

            Try

                abrir()

                cmd = New SQLiteCommand(sql, con)
                cmd.Parameters.AddWithValue("@AlimentoFamilia", "Pré-Mistura")
                cmd.Parameters.AddWithValue("@Alimento", nomePremix)
                cmd.Parameters.AddWithValue("@MS", Format(pctMS, "#,###.00")) ' se ponto então substtuir p virgula
                cmd.Parameters.AddWithValue("@PB", Format(pctPB, "#,###.00"))
                cmd.Parameters.AddWithValue("@PDR", Format(pctPDR, "#,###.00"))
                cmd.Parameters.AddWithValue("@PNDR", Format(pctPND, "#,###.00"))
                cmd.Parameters.AddWithValue("@FDN", Format(pctFDN, "#,###.00"))
                cmd.Parameters.AddWithValue("@eFDN", Format(pcteFDN, "#,###.00"))

                cmd.Parameters.AddWithValue("@MNmaior8", Format(pctMNmaior8, "#,###.00"))
                cmd.Parameters.AddWithValue("@MNmaior19", Format(pctMNmaior19, "#,###.00"))
                cmd.Parameters.AddWithValue("@FDNF", Format(pctFDNF, "#,###.00"))
                cmd.Parameters.AddWithValue("@FDA", Format(pctFDA, "#,###.00"))
                cmd.Parameters.AddWithValue("@Nel", Format(pctNel, "#,###.00"))
                cmd.Parameters.AddWithValue("@NDT", Format(pctNDT, "#,###.00"))
                cmd.Parameters.AddWithValue("@EE", Format(pctEE, "#,###.00"))
                cmd.Parameters.AddWithValue("@EE_Insat", Format(pctEE_Insat, "#,###.00"))
                cmd.Parameters.AddWithValue("@Cinzas", Format(pctCinzas, "#,###.00"))
                cmd.Parameters.AddWithValue("@CNF", Format(pctCNF, "#,###.00"))
                cmd.Parameters.AddWithValue("@Amido", Format(pctAmido, "#,###.00"))
                cmd.Parameters.AddWithValue("@kd_Amido", Format(pctkd_Amid, "#,###.00"))
                cmd.Parameters.AddWithValue("@MOR", Format(pctMor, "#,###.00"))
                cmd.Parameters.AddWithValue("@Ca", Format(pctCa, "#,###.00"))
                cmd.Parameters.AddWithValue("@P", Format(pctP, "#,###.00"))
                cmd.Parameters.AddWithValue("@Mg", Format(pctMg, "#,###.00"))
                cmd.Parameters.AddWithValue("@K", Format(pctK, "#,###.00"))
                cmd.Parameters.AddWithValue("@S", Format(pctS, "#,###.00"))
                cmd.Parameters.AddWithValue("@Na", Format(pctNa, "#,###.00"))
                cmd.Parameters.AddWithValue("@Cl", Format(pctCl, "#,###.00"))
                cmd.Parameters.AddWithValue("@Co", Format(pctCo, "#,###.00"))
                cmd.Parameters.AddWithValue("@Cu", Format(pctCu, "#,###.00"))
                cmd.Parameters.AddWithValue("@Mn", Format(pctMn, "#,###.00"))
                cmd.Parameters.AddWithValue("@Zn", Format(pctZn, "#,###.00"))
                cmd.Parameters.AddWithValue("@Se", Format(pctSe, "#,###.00"))
                cmd.Parameters.AddWithValue("@I", Format(pctI, "#,###.00"))
                cmd.Parameters.AddWithValue("@A", Format(pctA, "#,###.00"))
                cmd.Parameters.AddWithValue("@D", Format(pctD, "#,###.00"))
                cmd.Parameters.AddWithValue("@E", Format(pctE, "#,###.00"))
                cmd.Parameters.AddWithValue("@Cromo", Format(pctCromo, "#,###.00"))

                cmd.Parameters.AddWithValue("@Biotina", Format(pctBiotina, "#,###.00"))
                cmd.Parameters.AddWithValue("@Virginiamicina", Format(pctVirginiamicina, "#,###.00"))
                cmd.Parameters.AddWithValue("@Monensina", Format(pctMonensina, "#,###.00"))
                cmd.Parameters.AddWithValue("@Levedura", Format(pctLevedura, "#,###.00"))

                cmd.Parameters.AddWithValue("@Arginina", Format(pctArginina, "#,###.00"))
                cmd.Parameters.AddWithValue("@Histidina", Format(pctHistidina, "#,###.00"))
                cmd.Parameters.AddWithValue("@Isoleucina", Format(pctIsoleucina, "#,###.00"))
                cmd.Parameters.AddWithValue("@Leucina", Format(pctLeucina, "#,###.00"))

                cmd.Parameters.AddWithValue("@Lisina", Format(pctLisina, "#,###.00"))
                cmd.Parameters.AddWithValue("@Metionina", Format(pctMetionina, "#,###.00"))

                cmd.Parameters.AddWithValue("@Fenilalanina", Format(pctFenilalanina, "#,###.00"))
                cmd.Parameters.AddWithValue("@Treonina", Format(pctTreonina, "#,###.00"))
                cmd.Parameters.AddWithValue("@Triptofano", Format(pctTriptofano, "#,###.00"))
                cmd.Parameters.AddWithValue("@Valina", Format(pctValina, "#,###.00"))

                cmd.Parameters.AddWithValue("@dFDNp_48h", Format(pctdFDNp48h, "#,###.00"))
                cmd.Parameters.AddWithValue("@dAmido_7h", Format(pctdAmido7h, "#,###.00"))

                cmd.Parameters.AddWithValue("@TTNDFD", Format(pctTTNDFD, "#,###.00"))

                cmd.Parameters.AddWithValue("@Pers1", "0") 'Format(Pers1, "#,###.00"))
                cmd.Parameters.AddWithValue("@Pers2", "0") 'Format(Pers2, "#,###.00"))
                cmd.Parameters.AddWithValue("@Pers3", "0") 'Format(Pers3, "#,###.00"))
                cmd.Parameters.AddWithValue("@Pers4", "0") 'Format(Pers4, "#,###.00"))
                cmd.Parameters.AddWithValue("@Pers5", "0") 'Format(Pers5, "#,###.00"))
                cmd.Parameters.AddWithValue("@Pers6", "0") 'Format(Pers6, "#,###.00"))
                cmd.Parameters.AddWithValue("@Pers7", "0") 'Format(Pers7, "#,###.00"))
                cmd.Parameters.AddWithValue("@Pers8", "0") 'Format(Pers8, "#,###.00"))
                cmd.Parameters.AddWithValue("@Pers9", "0") 'Format(Pers9, "#,###.00"))
                cmd.Parameters.AddWithValue("@Chk", "0")
                cmd.ExecuteNonQuery()

            Catch ex As Exception
                MsgBox("Erro ao salvar!" + ex.Message)
                fechar()
            End Try
        End If
    End Sub


    'Cadastrar a PRÉ-MISTURA DETALHADA NA TABELA Premisturas
    Private Sub CadastrarPremistura()
        Dim sql As String
        Dim cmd As SQLiteCommand
        Dim data As Date
        'data = Now.ToString("dd-MM-yyyy HH:mm:ss")

        sql = "Insert into Premisturas (AlimentoFamilia,Alimento,Ingrediente,MS,PB,PDR,PNDR,FDN,eFDN,MNmaior8,MNmaior19,FDNF,FDA,Nel,NDT,EE,EE_Insat,Cinzas,CNF,Amido,kd_Amido,MOR,Ca,P,Mg,K,S,Na,Cl,Co,Cu,Mn,Zn,Se,I,A,D,E,Cromo,Biotina,Virginiamicina,Monensina,Levedura,Arginina,Histidina,Isoleucina,Leucina,Lisina,Metionina,Fenilalanina,Treonina,Triptofano,Valina,dFDNp_48h,dAmido_7h,TTNDFD,Pers1,Pers2,Pers3,Pers4,Pers5,Pers6,Pers7,Pers8,Pers9,calc1,Qtd,calc2,calc3) values (@AlimentoFamilia,@Alimento,@Ingrediente,@MS,@PB,@PDR,@PNDR,@FDN,@eFDN,@MNmaior8,@MNmaior19,@FDNF,@FDA,@Nel,@NDT,@EE,@EE_Insat,@Cinzas,@CNF,@Amido,@kd_Amido,@MOR,@Ca,@P,@Mg,@K,@S,@Na,@Cl,@Co,@Cu,@Mn,@Zn,@Se,@I,@A,@D,@E,@Cromo,@Biotina,@Virginiamicina,@Monensina,@Levedura,@Arginina,@Histidina,@Isoleucina,@Leucina,@Lisina,@Metionina,@Fenilalanina,@Treonina,@Triptofano,@Valina,@dFDNp_48h,@dAmido_7h,@TTNDFD,@Pers1,@Pers2,@Pers3,@Pers4,@Pers5,@Pers6,@Pers7,@Pers8,@Pers9,@calc1,@Qtd,@calc2,@calc3)"

        If txtNomePremix.Text <> "" Then

            For Each row As DataGridViewRow In dtgAlimentosPremix.Rows

                If row.Cells(69).Value > 0 Then

                    Try

                        abrir()

                        cmd = New SQLiteCommand(sql, con)
                        cmd.Parameters.AddWithValue("@AlimentoFamilia", "Pré-Mistura")
                        cmd.Parameters.AddWithValue("@Alimento", nomePremix)
                        cmd.Parameters.AddWithValue("@Ingrediente", row.Cells(1).Value.ToString)
                        cmd.Parameters.AddWithValue("@MS", row.Cells(2).Value.ToString)
                        cmd.Parameters.AddWithValue("@PB", row.Cells(3).Value.ToString)
                        cmd.Parameters.AddWithValue("@PDR", row.Cells(4).Value.ToString)
                        cmd.Parameters.AddWithValue("@PNDR", row.Cells(5).Value.ToString)
                        cmd.Parameters.AddWithValue("@FDN", row.Cells(6).Value.ToString)
                        cmd.Parameters.AddWithValue("@eFDN", row.Cells(7).Value.ToString)

                        cmd.Parameters.AddWithValue("@MNmaior8", row.Cells(8).Value.ToString)
                        cmd.Parameters.AddWithValue("@MNmaior19", row.Cells(9).Value.ToString)
                        cmd.Parameters.AddWithValue("@FDNF", row.Cells(10).Value.ToString)
                        cmd.Parameters.AddWithValue("@FDA", row.Cells(11).Value.ToString)
                        cmd.Parameters.AddWithValue("@Nel", row.Cells(12).Value.ToString)
                        cmd.Parameters.AddWithValue("@NDT", row.Cells(13).Value.ToString)
                        cmd.Parameters.AddWithValue("@EE", row.Cells(14).Value.ToString)
                        cmd.Parameters.AddWithValue("@EE_Insat", row.Cells(15).Value.ToString)
                        cmd.Parameters.AddWithValue("@Cinzas", row.Cells(16).Value.ToString)
                        cmd.Parameters.AddWithValue("@CNF", row.Cells(17).Value.ToString)
                        cmd.Parameters.AddWithValue("@Amido", row.Cells(18).Value.ToString)
                        cmd.Parameters.AddWithValue("@kd_Amido", row.Cells(19).Value.ToString)

                        cmd.Parameters.AddWithValue("@MOR", row.Cells(20).Value.ToString)

                        cmd.Parameters.AddWithValue("@Ca", row.Cells(21).Value.ToString)
                        cmd.Parameters.AddWithValue("@P", row.Cells(22).Value.ToString)
                        cmd.Parameters.AddWithValue("@Mg", row.Cells(23).Value.ToString)
                        cmd.Parameters.AddWithValue("@K", row.Cells(24).Value.ToString)
                        cmd.Parameters.AddWithValue("@S", row.Cells(25).Value.ToString)
                        cmd.Parameters.AddWithValue("@Na", row.Cells(26).Value.ToString)
                        cmd.Parameters.AddWithValue("@Cl", row.Cells(27).Value.ToString)
                        cmd.Parameters.AddWithValue("@Co", row.Cells(28).Value.ToString)
                        cmd.Parameters.AddWithValue("@Cu", row.Cells(29).Value.ToString)
                        cmd.Parameters.AddWithValue("@Mn", row.Cells(30).Value.ToString)
                        cmd.Parameters.AddWithValue("@Zn", row.Cells(31).Value.ToString)
                        cmd.Parameters.AddWithValue("@Se", row.Cells(32).Value.ToString)
                        cmd.Parameters.AddWithValue("@I", row.Cells(33).Value.ToString)
                        cmd.Parameters.AddWithValue("@A", row.Cells(34).Value.ToString)
                        cmd.Parameters.AddWithValue("@D", row.Cells(35).Value.ToString)
                        cmd.Parameters.AddWithValue("@E", row.Cells(36).Value.ToString)
                        cmd.Parameters.AddWithValue("@Cromo", row.Cells(37).Value.ToString)

                        cmd.Parameters.AddWithValue("@Biotina", row.Cells(38).Value.ToString)
                        cmd.Parameters.AddWithValue("@Virginiamicina", row.Cells(39).Value.ToString)
                        cmd.Parameters.AddWithValue("@Monensina", row.Cells(40).Value.ToString)
                        cmd.Parameters.AddWithValue("@Levedura", row.Cells(41).Value.ToString)

                        cmd.Parameters.AddWithValue("@Arginina", row.Cells(42).Value.ToString)
                        cmd.Parameters.AddWithValue("@Histidina", row.Cells(43).Value.ToString)
                        cmd.Parameters.AddWithValue("@Isoleucina", row.Cells(44).Value.ToString)
                        cmd.Parameters.AddWithValue("@Leucina", row.Cells(45).Value.ToString)

                        cmd.Parameters.AddWithValue("@Lisina", row.Cells(46).Value.ToString)
                        cmd.Parameters.AddWithValue("@Metionina", row.Cells(47).Value.ToString)

                        cmd.Parameters.AddWithValue("@Fenilalanina", row.Cells(48).Value.ToString)
                        cmd.Parameters.AddWithValue("@Treonina", row.Cells(49).Value.ToString)
                        cmd.Parameters.AddWithValue("@Triptofano", row.Cells(50).Value.ToString)
                        cmd.Parameters.AddWithValue("@Valina", row.Cells(51).Value.ToString)

                        cmd.Parameters.AddWithValue("@dFDNp_48h", row.Cells(52).Value.ToString)
                        cmd.Parameters.AddWithValue("@dAmido_7h", row.Cells(53).Value.ToString)

                        cmd.Parameters.AddWithValue("@TTNDFD", row.Cells(54).Value.ToString)

                        cmd.Parameters.AddWithValue("@Pers1", row.Cells(55).Value.ToString)
                        cmd.Parameters.AddWithValue("@Pers2", row.Cells(56).Value.ToString)
                        cmd.Parameters.AddWithValue("@Pers3", row.Cells(57).Value.ToString)
                        cmd.Parameters.AddWithValue("@Pers4", row.Cells(58).Value.ToString)
                        cmd.Parameters.AddWithValue("@Pers5", row.Cells(59).Value.ToString)
                        cmd.Parameters.AddWithValue("@Pers6", row.Cells(60).Value.ToString)
                        cmd.Parameters.AddWithValue("@Pers7", row.Cells(61).Value.ToString)
                        cmd.Parameters.AddWithValue("@Pers8", row.Cells(62).Value.ToString)
                        cmd.Parameters.AddWithValue("@Pers9", row.Cells(63).Value.ToString)
                        cmd.Parameters.AddWithValue("@Qtd", row.Cells(69).Value.ToString)

                        cmd.Parameters.AddWithValue("@calc1", "0")
                        cmd.Parameters.AddWithValue("@calc2", "0")
                        cmd.Parameters.AddWithValue("@calc3", "0")

                        'cmd.Parameters.AddWithValue("@Custo", row.Cells(64).Value.ToString) '59
                        'cmd.Parameters.AddWithValue("@QtdD1", row.Cells(65).Value.ToString) '60
                        'cmd.Parameters.AddWithValue("@QtdD2", "0") '61
                        'cmd.Parameters.AddWithValue("@Premix", row.Cells(67).Value.ToString) '62
                        'cmd.Parameters.AddWithValue("@PctPremix", row.Cells(70).Value.ToString) '63
                        'cmd.Parameters.AddWithValue("@QtdVagao", row.Cells(71).Value.ToString) '64
                        'cmd.Parameters.AddWithValue("@QtdPremix", row.Cells(72).Value.ToString) '65
                        'cmd.Parameters.AddWithValue("@Propriedade", nomeFaz)
                        'cmd.Parameters.AddWithValue("@IdPropriedade", idFaz)
                        'cmd.Parameters.AddWithValue("@Lote", cbxLote.Text & " | " & lblCat.Text)
                        'cmd.Parameters.AddWithValue("@QtdAnimais", lblQtA.Text)
                        'cmd.Parameters.AddWithValue("@Data", data)



                        cmd.ExecuteNonQuery()

                    Catch ex As Exception
                        MsgBox("Erro ao salvar!" + ex.Message)
                        fechar()
                    End Try

                End If
            Next
            MsgBox("Alimento cadastrado com sucesso!")
        Else
            MsgBox("Você precisa dar um nome para a nova Pré-Mistura!")
        End If
        SalvarPremisturaMS()
        'SalvarPremisturaMO()
    End Sub

    Private Sub btnPreMix_Click(sender As Object, e As EventArgs) Handles btnPreMix.Click
        pnlPreMix.Visible = True
        pnlPreMix.Location = New Point(274, 41)
        pnlPreMix.BringToFront()
        btnDesmontePremix.Visible = False
        dtgEdtPremix.Visible = False
        dtgEdtPremix.SendToBack()
        dtgAlimentosPremix.Visible = True
        dtgAlimentosPremix.BringToFront()
        ConfigGridAlimentosPremix()
        btnSalvarPremix.Visible = True
        ' For i As Integer = 0 To dtgAlimentosPremix.Rows.Count - 1
        For Each row As DataGridViewRow In dtgAlimentosPremix.Rows
            Dim vlr As Double
            vlr += row.Cells(69).Value
            If vlr = 0 Then
                row.Cells(70).Value = 0
            End If
        Next


        'Next
        'CalcularPremix()
        'SomarIngPreMix()

        'dtgAlimentosPremix.Rows(1).Cells(70).Value = "0,00"
    End Sub

    Private Sub BuscarPremistura()
        Dim da As New SQLiteDataAdapter
        Dim dt8 As New DataTable
        Dim sql As String

        sql = "Select * from premisturas where Alimento = " & "'" & nomePremix & "'" '& '"' group by NomeAvaliador"
        'sql = "Select * from premisturas"
        Try

            abrir()

            da = New SQLiteDataAdapter(sql, con)
            dt8 = New DataTable
            da.Fill(dt8)
            'dtgAlimentosPremix.DataSource = dt8
            dtgEdtPremix.DataSource = dt8

        Catch ex As Exception

            MsgBox(ex.Message)
            fechar()

        End Try
    End Sub

    Private Sub DeletePremix()

        Dim cmd As SQLiteCommand
        Dim sqlDelete As String = "Delete from premisturas where Alimento=@Alimento"
        'Mensagem se realmente quer excluir

        Try
            abrir()
            cmd = New SQLiteCommand(sqlDelete, con)
            'cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@Alimento", nomePremix)
            cmd.ExecuteNonQuery()
            ' MsgBox("As alterações foram bem sucedidas!")
        Catch ex As Exception
            ' MsgBox("Erro ao editar!" + ex.Message)
            fechar()
        End Try
        DeletePremixMS()
        DeletePremixDieta()

    End Sub

    Private Sub DeletePremixMS()

        Dim cmd As SQLiteCommand
        Dim sqlDelete As String = "Delete from AlimentosMS where Alimento=@Alimento"
        'Mensagem se realmente quer excluir

        Try
            abrir()
            cmd = New SQLiteCommand(sqlDelete, con)
            'cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@Alimento", nomePremix)
            cmd.ExecuteNonQuery()
            ' MsgBox("As alterações foram bem sucedidas!")
        Catch ex As Exception
            ' MsgBox("Erro ao editar!" + ex.Message)
            fechar()
        End Try

    End Sub
    Private Sub DeletePremixDieta()

        Dim cmd As SQLiteCommand
        Dim sqlDelete As String = "Delete from Dieta where Alimento=@Alimento"
        'Mensagem se realmente quer excluir

        Try
            abrir()
            cmd = New SQLiteCommand(sqlDelete, con)
            'cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@Alimento", nomePremix)
            cmd.ExecuteNonQuery()
            ' MsgBox("As alterações foram bem sucedidas!")
        Catch ex As Exception
            ' MsgBox("Erro ao editar!" + ex.Message)
            fechar()
        End Try

    End Sub

    Private Sub btnExcluirPremix_Click(sender As Object, e As EventArgs) Handles btnExcluirPremix.Click

        DeletePremix()

        btnExcluirPremix.Enabled = False
    End Sub

    Private Sub dtgalimentospremix_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dtgAlimentosPremix.CellFormatting
        If e.ColumnIndex = 70 Or 69 Or 71 Then 'AndAlso IsNumeric(e.Value)
            If IsNumeric(e.Value) Then
                e.Value = Format(CDbl(e.Value), "0.00")
                'e.Value = e.Value.ToString("F2")
                e.FormattingApplied = True
            End If
        End If
    End Sub

    Dim pmxPronto As Boolean
    Private Sub btnEdtPremix_Click(sender As Object, e As EventArgs) Handles btnEdtPremix.Click
        Try
            TryCast(dtgEdtPremix.DataSource, DataTable).DefaultView.RowFilter = "Alimento LIKE '%" & txtNomePremix.Text & "%'"
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX    DESMONTAR PRÉ-MISTURA  XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

    Private Sub btnDesmontePremix_Click(sender As Object, e As EventArgs) Handles btnDesmontePremix.Click

        For Each row As DataGridViewRow In dtgAlimentosDieta.Rows
            For Each row1 As DataGridViewRow In dtgEdtPremix.Rows
                If row.Cells(3).Value.ToString = row1.Cells(2).Value.ToString Then
                    row.Cells(67).Value = ((row.Cells(67).Value / 100) + (row1.Cells(66).Value / 100)) * 100
                    row1.Cells(66).Value = 0
                End If
            Next

        Next
        For Each row As DataGridViewRow In dtgAlimentosDieta.Rows
            For Each row1 As DataGridViewRow In dtgEdtPremix.Rows

                If row1.Cells(66).Value > 0 Then
                    Dim x As Integer
                    x += row1.Cells(66).Value
                    While x > 0

                        If row1.Cells(66).Value > 0 Then

                            'almntoFamilia = row1.Cells(2).Value
                            almnto = row1.Cells(2).Value
                            v_MS = row1.Cells(3).Value
                            v_PB = row1.Cells(4).Value
                            v_PDR = row1.Cells(5).Value
                            v_PND = row1.Cells(6).Value
                            v_FDN = row1.Cells(7).Value
                            v_eFDN = row1.Cells(8).Value
                            v_MNmaior8 = row1.Cells(10).Value
                            v_MNmaior19 = row1.Cells(11).Value
                            v_FDNF = row1.Cells(12).Value
                            v_FDA = row1.Cells(13).Value
                            v_Nel = row1.Cells(14).Value
                            v_NDT = row1.Cells(15).Value
                            v_EE = row1.Cells(16).Value
                            v_EE_Insat = row1.Cells(17).Value
                            v_Cinzas = row1.Cells(18).Value
                            v_CNF = row1.Cells(19).Value
                            v_Amido = row1.Cells(20).Value
                            v_kd_Amid = row1.Cells(21).Value
                            v_MOR = row1.Cells(22).Value

                            v_Ca = row1.Cells(23).Value
                            v_P = row1.Cells(24).Value
                            v_Mg = row1.Cells(25).Value
                            v_K = row1.Cells(26).Value
                            v_S = row1.Cells(27).Value
                            v_Na = row1.Cells(28).Value
                            v_Cl = row1.Cells(29).Value
                            v_Co = row1.Cells(30).Value
                            v_Cu = row1.Cells(31).Value
                            v_Mn = row1.Cells(32).Value
                            v_Zn = row1.Cells(33).Value
                            v_Se = row1.Cells(34).Value
                            v_I = row1.Cells(35).Value
                            v_A = row1.Cells(36).Value
                            v_D = row1.Cells(37).Value
                            v_E = row1.Cells(38).Value
                            v_Cromo = row1.Cells(39).Value
                            v_Biotina = row1.Cells(40).Value
                            v_Virginiamicina = row1.Cells(41).Value
                            v_Monensina = row1.Cells(42).Value
                            v_Levedura = row1.Cells(43).Value
                            v_Arginina = row1.Cells(44).Value
                            v_Histidina = row1.Cells(45).Value
                            v_Isoleucina = row1.Cells(46).Value
                            v_Leucina = row1.Cells(47).Value
                            v_Lisina = row1.Cells(48).Value
                            v_Metionina = row1.Cells(49).Value
                            v_Fenilalanina = row1.Cells(50).Value
                            v_Treonina = row1.Cells(51).Value
                            v_Triptofano = row1.Cells(52).Value
                            v_Valina = row1.Cells(53).Value
                            v_dFDNp48h = row1.Cells(54).Value
                            v_dAmido_7h = row1.Cells(55).Value
                            v_TTNDFD = row1.Cells(56).Value
                            Dim v_qtd As Double = row1.Cells(66).Value
                            v_id = row1.Cells(67).Value

                            'Dim varTF As Boolean = True
                            ' On Error Resume Next
                            ' With dtgAlimentos
                            'If .CurrentCell.ColumnIndex = 0 Then

                            'If .CurrentRow.Cells(66).Value = 0 Then
                            dtTemp.Rows.Add("Tipo", almnto, v_MS, v_PB, v_PDR, v_PND, v_FDN, v_eFDN, v_MNmaior8, v_MNmaior19, v_FDNF, v_FDA, v_Nel, v_NDT, v_EE, v_EE_Insat, v_Cinzas, v_CNF, v_Amido, v_kd_Amid, v_MOR, v_Ca, v_P, v_Mg, v_K, v_S, v_Na, v_Cl, v_Co, v_Cu, v_Mn, v_Zn, v_Se, v_I, v_A, v_D, v_E,
                      v_Cromo, v_Biotina, v_Virginiamicina, v_Monensina, v_Levedura, v_Arginina, v_Histidina, v_Isoleucina, v_Leucina, v_Lisina, v_Metionina, v_Fenilalanina, v_Treonina, v_Triptofano, v_Valina, v_dFDNp48h, v_dAmido_7h, v_TTNDFD, "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", v_qtd, "0", "0", "0", "0", "0", "0", "0", "0", "0", "0") ', "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")

                            dtgTemp.DataSource = dtTemp
                            '            .CurrentCell.Value = True
                            '            .CurrentRow.DefaultCellStyle.BackColor = Color.FromArgb(207, 247, 211)

                            '            PreencherTabVol()

                            '            .CurrentRow.Cells(66).Value = 1
                            '        ElseIf .CurrentRow.Cells(66).Value = 1 Then
                            '            For Each row As DataGridViewRow In dtgTemp.Rows
                            '                If row.Cells(77).Value = v_id Then
                            '                    dtgTemp.Rows.Remove(row)
                            '                End If
                            '                row.Cells(0).Value = True

                            '            Next
                            '            .CurrentRow.Cells(66).Value = 0

                            '        End If

                            '    End If

                            '    dtgAlimentos.Refresh()
                            '    dtgTemp.Refresh()


                            '    CorTabAlim()

                            'End With

                            'For Each row As DataGridViewRow In dtgTemp.Rows
                            '    row.Cells(0).Value = True
                            'Next

                            'AnaliseVol()


                            ' CorTabAlim()
                            'ConfigGridTemp()
                            x -= row1.Cells(66).Value

                            row1.Cells(66).Value = 0


                        End If

                    End While
                End If
            Next
        Next
        'excluir premistura na gridalimentosdieta sai
        dtgAlimentosDieta.Rows.Remove(dtgAlimentosDieta.Rows.Item(dtgAlimentosDieta.Rows(indxPremistura).Index))

        PainelDieta()
        'desabilitei 14/05
        'btnPreMix.Enabled = True
        'btnPreMix.BackgroundImage = My.Resources.pre_mistura_on

        'pnlPreMix.Visible = False
        DeletePremix()
        dtgEdtPremix.Visible = False
        dtgAlimentosPremix.Visible = True
        dtgAlimentosPremix.BringToFront()
        dtgAlimentosPremix.DataSource = dtgTemp.DataSource
        For Each row As DataGridViewRow In dtgAlimentosPremix.Rows
            row.Cells(69).Value = 0
            CalcularPremix()
        Next
        btnDesmontePremix.Visible = False
        btnExcluirPremix.Enabled = False
        ConfigGridAlimentosPremix()
        BuscarAlimentosMSMO()

        DeleteDieta()
        SalvarDieta1()

        txtNomePremix.Text = ""
        pnlPreMix.Visible = False

        'AtualizarDieta1()
    End Sub

    'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX    Arredondar paineis   XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

    Public Sub New()

        ' Esta chamada é requerida pelo designer.
        InitializeComponent()

        ' Adicione qualquer inicialização após a chamada InitializeComponent().
        pnlDieta.BorderStyle = BorderStyle.None
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
    Private Sub pnlResultNutri_Paint(sender As Object, e As PaintEventArgs) Handles pnlResultNutri.Paint
        borderColor = Color.FromArgb(207, 247, 211)
        PnlRegionAndBorder(pnlResultNutri, borderRadius, e.Graphics, borderColor, borderSize)
    End Sub
    Private Sub pnlDieta_Paint(sender As Object, e As PaintEventArgs) Handles pnlDieta.Paint
        borderColor = Color.FromArgb(237, 242, 207)
        PnlRegionAndBorder(pnlDieta, borderRadius, e.Graphics, borderColor, borderSize)
    End Sub

    'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX    Fim Arredondar paineis   XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX



    '???????????????????????
    Private Sub Button2_Click(sender As Object, e As EventArgs)
        pnlAvalD1.Visible = False
        'pnlAvalD2.Visible = False
        pnlCustoD1.Visible = False
        pnlCustoD2.Visible = False
        pnlCustoD1eD2.Visible = False

        pnlAvalD2.Location = New Point(270, 1)
        pnlAvalD2.Visible = True
        pnlAvalD2.BringToFront()
    End Sub


    Private Sub txtNomeDieta_Enter(sender As Object, e As EventArgs)
        MascaraEnter(Me.ActiveControl, "Nome da dieta")
    End Sub

    Private Sub txtNomeDieta_Leave(sender As Object, e As EventArgs)
        MascaraLeave(txtNomeDieta, "Nome da dieta")
    End Sub

End Class

