
Imports System.Windows.Forms.DataVisualization.Charting
Imports System.Data.SQLite
Imports System.Drawing.Printing
Imports System.Data
Imports System.IO
Imports System.Windows.Forms
Imports System.Runtime.InteropServices
Public Class frmManejo

    Private Sub Manejo_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Width = 1440
        Me.Height = 900
        Me.Location = New Point(0, 78)
        Me.tabParticulas.Parent = Me.tcManejo
        Me.tcManejo.SelectedTab = tabParticulas

        TabelaForragem()
        ConfigGridForragem()

        BuscarForragemAgrup()
        cbxQtdPeneiras.Text = "04 Peneiras"

        lblFazenda.Text = nomeFaz
        lblIdProp.Text = idFaz

        btnSalvarForragem.Enabled = False
        btnSalvarSobras.Enabled = False

        BuscarLotes()
        Dim x As Integer = dtgHistForragem.Rows.Count
        If x > 0 Then
            CarregarForragem()
        End If

        'xxxxxxxxxxxxxxxxxx
        txtMisturador.Text = My.Settings.MistPremix
        txtPesoVagao.Text = My.Settings.Vagao

        'TabelaPH()
        ' ConfigGrid()
        'pnlBarraFaz.Size = New Size(1460, 67)

        'RichTextBox1.Size = New Point(richtextboxsize, RichTextBox1.Height)
        'InitializeContextMenu()
        cbxQtdPeneirasSobras.Text = "04 Peneiras"
        lblTAmostra.Text = 0
        lblTAmostraSobra.Text = 0
        lblNomeFezes.Text = data
        lblNomeUrina.Text = data

        'cbxQtdPeneirasTrato.Text = "04 Peneiras"
        'cbxQtdAmostras.Text = "10"
        'BuscarTratosAgrup()
        'TabelaAmostras()


        TabelaSobras()
        TabelaKPS()
        TabelaAmostrasUrina()

        BuscarAmostrasUrinaAgrup()
        BuscarFezesAgrup()
        BuscarDietaAgrup()
        BuscarKPSAgrup()
        BuscarSobrasAgrup()

        cbxQtdPeneirasSobras.Text = "04 Peneiras"
        'BuscarKPSAgrup()


    End Sub



    ' Public Class frmManejo
    'Impressora PDF em ImpressoraPadrão
    <DllImport("winspool.drv", CharSet:=CharSet.Auto, SetLastError:=True)>
    Public Shared Function SetDefaultPrinter(Nome As String) As Boolean

    End Function


    Dim data As String
    Private WithEvents m_PrintDocument As PrintDocument

    'Dim cmd As sqliteCommand

    Private paginaAtual As Integer = 1

    'Private MyConnection As sqliteConnection

    Private Leitor As SQLiteDataReader

    Private RelatorioTitulo As String

    'variavel para criação editor de texto
    Dim leitor1 As StringReader
    Friend WithEvents Form1 As System.Windows.Forms.Form
    'Dim richtextboxsize As Integer = 567
    'Private Sub frmManejo_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    'End Sub




    'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX       CONFIGURAR TABELAS      XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
  
    Private Sub ConfigGrid()

        On Error Resume Next

        With dtgHistTratos
            .ColumnHeadersDefaultCellStyle.BackColor = Color.Gainsboro
            .ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Regular)
            .DefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Regular)

            .Columns(0).HeaderText = "Tratos cadastrados:"

            .Columns(0).Width = 160
            .Columns(1).Visible = False
            .Columns(2).Visible = False
            .Columns(3).Visible = False
            .Columns(4).Visible = False
            .Columns(5).Visible = False


        End With

        'With dtgHistKPS

        '    .ColumnHeadersDefaultCellStyle.BackColor = Color.Gainsboro
        '    .ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Regular)
        '    .DefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Regular)

        '    '.Columns(0).HeaderText = "Peneira"

        '    .Columns(0).Width = 150
        '    .Columns(1).Visible = False

        'End With

       

        With Me.dtgKPS

            .ColumnHeadersDefaultCellStyle.BackColor = Color.Gainsboro
            .ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Regular)
            .DefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Regular)

            'Alihhar texto ao centro
            .Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter

            .Columns(0).Width = 100
            .Columns(1).Width = 100

        End With

        With Me.dtgSobras

            .ColumnHeadersDefaultCellStyle.BackColor = Color.Gainsboro
            .ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Regular)
            .DefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Regular)

            'Alihhar texto ao centro
            .Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter
            .Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter
            .Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter

            .Columns(0).HeaderText = "Peneiras"
            .Columns(1).HeaderText = "Tamanho (mm)"
            .Columns(2).HeaderText = "Qtd por peneira (g)"
            .Columns(3).HeaderText = "% Por peneira"
            .Columns(4).HeaderText = "Forragem acima de 8mm"

            .Columns(0).Width = 80
            .Columns(1).Width = 110
            .Columns(2).Width = 130
            .Columns(3).Width = 120
            .Columns(4).Width = 150
            '.Columns(4).Visible = False

        End With
        With Me.dtgHistSobras

            .ColumnHeadersDefaultCellStyle.BackColor = Color.Gainsboro
            .ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Regular)
            .DefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Regular)

            .Columns(0).HeaderText = "Sobras cadastradas:"

            .Columns(0).Width = 160
            .Columns(1).Visible = False
            .Columns(2).Visible = False
            .Columns(3).Visible = False
            .Columns(4).Visible = False
            .Columns(5).Visible = False
        End With
        With Me.dtgPH

            .ColumnHeadersDefaultCellStyle.BackColor = Color.Gainsboro
            .ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Regular)
            .DefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Regular)

            .Columns(0).Width = 160
            .Columns(1).Width = 60

        End With

    End Sub

    'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
    'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX            TRATOS                XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

  
    Dim gpos5 As Integer = 0
    Dim trato1 As String = ""
    Private Sub GraficoTrato1()

        Chart5.Series.Clear()
        Chart5.Titles.Clear()
        trato1 = ""

        Dim pm1 As Double = dtgAmostras.Rows(0).Cells(11).Value
        Dim pm2 As Double = dtgAmostras.Rows(1).Cells(11).Value
        Dim pm3 As Double = dtgAmostras.Rows(2).Cells(11).Value
        Dim pm4 As Double = 0
        If dtgAmostras.Rows(3).Cells(11).Value > 0 Then
            pm4 = dtgAmostras.Rows(3).Cells(11).Value
        Else
            pm4 = 0
        End If
        Dim pcv1 As Double = dtgAmostras.Rows(0).Cells(12).Value
        Dim pcv2 As Double = dtgAmostras.Rows(1).Cells(12).Value
        Dim pcv3 As Double = dtgAmostras.Rows(2).Cells(12).Value
        Dim pcv4 As Double = 0
        If dtgAmostras.Rows(3).Cells(12).Value > 0 Then
            pcv4 = dtgAmostras.Rows(3).Cells(12).Value
        Else
            pcv4 = 0
        End If

        Dim nome() As String
        nome = txtNomeTrato.Text.Split(" ")
        trato1 = nome(0) & nome(1)

        Dim title = New Title()
        title.Font = (New Font("Arial", 8, FontStyle.Bold))
        title.ForeColor = Color.Black
        title.Text = trato1
        Chart5.Titles.Add(title)

        Chart5.Series.Add("Tratos")

        On Error Resume Next
        With Chart5.Series("Tratos")

            'define o tipo de gráfico
            .ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column
            .BorderWidth = 2
            'define o titulo do gráfico
            ' .Titles.Add("Ali")
            .Palette = ChartColorPalette.BrightPastel
            .Points.AddXY("P1", pm1)
            .Points.AddXY("P2", pm2)
            If dtgAmostras.Rows(3).Cells(11).Value <> 0 Then
                .Points.AddXY("P3", pm3)
            End If
            .Points.AddXY("FD", pm4)

            .Points.AddXY("%CV", pcv1)
            .Points.AddXY("%CV", pcv2)
            If dtgAmostras.Rows(3).Cells(12).Value <> 0 Then
                .Points.AddXY("%CV", pcv3)
            End If
            .Points.AddXY("%CV", pcv4)
            'Tamanho
            '.Size = New Size(Size.Width, 250)
            ' .Size = New Size(Size.Height, 165)

        End With
        With Chart5.ChartAreas("ChartArea1")
            'Eixo X
            Chart5.ChartAreas(0).AxisX.LabelStyle.Angle = -90
            .AxisX.Title = "Peneiras em (mm)"
            .AxisX.TitleFont = (New Font("Arial", 7, FontStyle.Bold))
            .AxisX.TitleForeColor = Color.Black
            'Eixo Y
            .AxisY.Title = "Particulas em (%)"
            .AxisY.TitleFont = (New Font("Arial", 7, FontStyle.Bold))
            .AxisY.TitleForeColor = Color.Black

        End With

        Chart5.DataBind()
        Chart5.Visible = True
    End Sub

    Dim gpos6 As Integer = 0
    Dim trato2 As String = ""
    Private Sub GraficoTrato2()

        Chart6.Series.Clear()
        Chart6.Titles.Clear()
        trato2 = ""

        Dim pm1 As Double = dtgAmostras.Rows(0).Cells(11).Value
        Dim pm2 As Double = dtgAmostras.Rows(1).Cells(11).Value
        Dim pm3 As Double = dtgAmostras.Rows(2).Cells(11).Value
        Dim pm4 As Double = 0
        If dtgAmostras.Rows(3).Cells(11).Value > 0 Then
            pm4 = dtgAmostras.Rows(3).Cells(11).Value
        Else
            pm4 = 0
        End If
        Dim pcv1 As Double = dtgAmostras.Rows(0).Cells(12).Value
        Dim pcv2 As Double = dtgAmostras.Rows(1).Cells(12).Value
        Dim pcv3 As Double = dtgAmostras.Rows(2).Cells(12).Value
        Dim pcv4 As Double = 0
        If dtgAmostras.Rows(3).Cells(12).Value > 0 Then
            pcv4 = dtgAmostras.Rows(3).Cells(12).Value
        Else
            pcv4 = 0
        End If

        Dim nome() As String
        nome = txtNomeTrato.Text.Split(" ")
        trato2 = nome(0) & nome(1)

        Dim title = New Title()
        title.Font = (New Font("Arial", 8, FontStyle.Bold))
        title.ForeColor = Color.Black
        title.Text = trato2
        Chart6.Titles.Add(title)

        Chart6.Series.Add("Tratos")

        On Error Resume Next
        With Chart6.Series("Tratos")

            'define o tipo de gráfico
            .ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column
            .BorderWidth = 2
            'define o titulo do gráfico
            ' .Titles.Add("Ali")
            .Palette = ChartColorPalette.BrightPastel
            .Points.AddXY("P1", pm1)
            .Points.AddXY("P2", pm2)
            If dtgAmostras.Rows(3).Cells(11).Value <> 0 Then
                .Points.AddXY("P3", pm3)
            End If
            .Points.AddXY("FD", pm4)

            .Points.AddXY("%CV", pcv1)
            .Points.AddXY("%CV", pcv2)
            If dtgAmostras.Rows(3).Cells(12).Value <> 0 Then
                .Points.AddXY("%CV", pcv3)
            End If
            .Points.AddXY("%CV", pcv4)

            'Tamanho
            '.Size = New Size(Size.Width, 250)
            ' .Size = New Size(Size.Height, 165)

        End With
        With Chart6.ChartAreas("ChartArea1")
            'Eixo X
            Chart6.ChartAreas(0).AxisX.LabelStyle.Angle = -90
            .AxisX.Title = "Peneiras em (mm)"
            .AxisX.TitleFont = (New Font("Arial", 7, FontStyle.Bold))
            .AxisX.TitleForeColor = Color.Black
            'Eixo Y
            .AxisY.Title = "Particulas em (%)"
            .AxisY.TitleFont = (New Font("Arial", 7, FontStyle.Bold))
            .AxisY.TitleForeColor = Color.Black

        End With
        Chart6.DataBind()
        Chart6.Visible = True
    End Sub

    'Dim trato3 As String = ""
    'Private Sub GraficoTrato3()

    '    Chart12.Series.Clear()
    '    Chart12.Titles.Clear()
    '    trato3 = ""

    '    Dim var1 As Double = dtgAmostras.Rows(0).Cells(12).Value
    '    Dim var12 As Double = dtgAmostras.Rows(1).Cells(12).Value
    '    Dim var13 As Double = dtgAmostras.Rows(2).Cells(12).Value
    '    Dim var14 As Double = dtgAmostras.Rows(3).Cells(12).Value
    '    'Dim var15 As Double = dtgAmostras.Rows(4).Cells(12).Value
    '    trato2 = txtNomeTrato.Text

    '    Dim title = New Title()
    '    title.Font = (New Font("Arial", 10, FontStyle.Bold))
    '    title.ForeColor = Color.Black
    '    title.Text = trato3
    '    Chart12.Titles.Add(title)


    '    Chart12.Series.Add("Trato")

    '    On Error Resume Next
    '    With Chart12.Series("Trato")

    '        'define o tipo de gráfico
    '        .ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column
    '        .BorderWidth = 2
    '        'define o titulo do gráfico
    '        ' .Titles.Add("Ali")
    '        .Palette = ChartColorPalette.BrightPastel
    '        .Points.AddXY("P1", var1)
    '        .Points.AddXY("P2", var12)
    '        .Points.AddXY("P3", var13)
    '        '.Points.AddXY("P4", var14)
    '        .Points.AddXY("Fd", var14)

    '        'Tamanho
    '        '.Size = New Size(Size.Width, 250)
    '        ' .Size = New Size(Size.Height, 165)

    '    End With
    '    
    '    Chart12.DataBind()
    '    Chart12.Visible = True
    'End Sub

    Private Sub BuscarLotes() 'Trato

        Dim da As New SQLiteDataAdapter
        Dim dt As New DataTable

        Try

            abrir()

            Dim sql As String = "Select * from dadosAnimais where Cliente = " & "'" & idFaz & "'"
            da = New SQLiteDataAdapter(sql, con)
            dt = New DataTable
            da.Fill(dt)
            'dtgltes.DataSource = dt

            cbxNomeLote.ValueMember = "Lote"

            'cbxAvaliadores.DisplayMember = "NomeAvaliador"
            cbxNomeLote.DataSource = (dt)
            dtgLotesPremix.DataSource = dt
            dtgHistLotes.DataSource = dt

            fechar()


        Catch ex As Exception

        End Try
        ConfigLotePremix()
        ConfigLotesVagao()
    End Sub

    Private Sub btnExcluirTrato_Click(sender As Object, e As EventArgs) Handles btnExcluirTrato.Click
        DeletarTratos()
        BuscarTratosAgrup()
    End Sub

    Private Sub cbxNomeLote_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxNomeLote.SelectedIndexChanged
        BuscarLotes()
    End Sub


    Private Sub MediaTratos() ' Calculos de CV coeficiente de variação

        Dim dp As Double
        Dim md As Double
        Dim soma As Double = 0
        Dim nCelulas As Integer = 0
        Dim cv As Double
        'Calcular média
        For i = 0 To dtgAmostras.ColumnCount - 1
            If dtgAmostras.Rows(1).Cells(i).ColumnIndex >= 1 And dtgAmostras.Rows(1).Cells(i).ColumnIndex < 11 Then
                'For Each row As DataGridViewRow In dtgAmostras.Rows
                If dtgAmostras.CurrentRow.Cells(i).Value > 0 Then
                    nCelulas += 1
                End If
                'Next
                soma += dtgAmostras.CurrentRow.Cells(i).Value

            End If

        Next

        md = Format(soma / nCelulas, "#.#0")

        soma = 0
        nCelulas = 0

        ' Desvio padrão
        For i = 0 To dtgAmostras.ColumnCount - 1
            If dtgAmostras.Rows(1).Cells(i).ColumnIndex >= 1 And dtgAmostras.Rows(1).Cells(i).ColumnIndex < 11 Then
                'For Each row As DataGridViewRow In dtgAmostras.Rows
                If dtgAmostras.CurrentRow.Cells(i).Value > 0 Then
                    nCelulas += 1
                End If
                'Next
                soma += (dtgAmostras.CurrentRow.Cells(i).Value - md) ^ 2

            End If

        Next
        'CV
        dp = Math.Sqrt(soma / nCelulas)
        cv = dp / md * 100
        dtgAmostras.CurrentRow.Cells(11).Value = Format(md, "#.#0")
        dtgAmostras.CurrentRow.Cells(12).Value = Format(cv, "#.#0")

    End Sub


    'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
    'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX            FORRAGEM               XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

    Private Sub CadastrarForragem()

        Dim sql As String
        Dim cmd As SQLiteCommand
        Dim dataf As String
        dataf = Now.ToString("dd/MM/yyyy HH:mm:ss") '& " | " & dataf

        For Each row As DataGridViewRow In dtgForragem.Rows
            Try
                abrir()
                sql = "Insert into Forragem (Nome,QtdPeneiras,Peneira,Tamanho,Quantidade,PorcPorPeneira,Acima8mm,Cod,IdPropriedade) values (@Nome,@QtdPeneiras,@Peneira,@Tamanho,@Quantidade,@PorcPorPeneira,@Acima8mm,@Cod,@IdPropriedade)"
                cmd = New SQLiteCommand(sql, con)
                'cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@Nome", txtNomeForragem.Text & " | " & dataf)
                cmd.Parameters.AddWithValue("@QtdPeneiras", cbxQtdPeneiras.Text)
                cmd.Parameters.AddWithValue("@Peneira", row.Cells("Peneiras").Value.ToString)
                cmd.Parameters.AddWithValue("@Tamanho", row.Cells("Tamanho (mm)").Value.ToString)
                cmd.Parameters.AddWithValue("@Quantidade", row.Cells("Qtd por peneira (g)").Value.ToString)
                cmd.Parameters.AddWithValue("@PorcPorPeneira", row.Cells("% Por peneira").Value.ToString)
                cmd.Parameters.AddWithValue("@Acima8mm", row.Cells("Forragem acima de 8mm").Value.ToString)
                cmd.Parameters.AddWithValue("@Cod", dataf)
                cmd.Parameters.AddWithValue("@IdPropriedade", lblIdProp.Text)


                cmd.ExecuteNonQuery()
                'ListarComissoes()

            Catch ex As Exception
                'MsgBox("Erro ao salvar!" + ex.Message)
                fechar()
            End Try

        Next
        MsgBox("Silagem cadastrada com sucesso!")

        'btnEditarCliente.Enabled = False

        TabelaForragem()
        txtNomeForragem.Text = ""
        cbxQtdPeneiras.Text = "04 Peneiras"
        ConfigGridForragem()
        lblAcima8.Text = "0"
        'Else
        'MsgBox("Preencha os campos corretamente!")

        'End If
    End Sub

    Private Sub BuscarForragemAgrup()

        Dim da As New SQLiteDataAdapter
        Dim dt As New DataTable
        Dim sql As String
        Try

            abrir()
            sql = "Select Nome, QtdPeneiras, IdPropriedade from Forragem where IdPropriedade = " & "'" & idFaz & "'" & " group by Nome, QtdPeneiras, IdPropriedade "
            'sql = "Select Nome, QtdPeneiras from Forragem group by Nome"
            'SUM(Quantidade)
            da = New SQLiteDataAdapter(sql, con)
            dt = New DataTable
            da.Fill(dt)
            dtgHistForragem.DataSource = dt

            fechar()

        Catch ex As Exception

        End Try

        'dtgClientes.DataSource = dtgFazendas.DataSource
    End Sub

    Private Sub BuscarForragem()

        Dim da As New SQLiteDataAdapter
        Dim dt As New DataTable
        Dim sql As String
        Try

            abrir()
            sql = "Select Peneira,Tamanho,Quantidade,PorcPorPeneira,Acima8mm,Cod from Forragem where Nome = " & "'" & Me.txtNomeForragem.Text & "'"
            'sql = "Select Nome, QtdPeneiras from Forragem group by Nome"
            'SUM(Quantidade)
            da = New SQLiteDataAdapter(sql, con)
            dt = New DataTable
            da.Fill(dt)
            dtgForragem.DataSource = dt

            fechar()

        Catch ex As Exception

        End Try

        'SomarAmostras()

        'dtgClientes.DataSource = dtgFazendas.DataSource
    End Sub

    Private Sub DeletarForragem()

        Dim cmd As SQLiteCommand
        Dim sqlDelete As String = "Delete from Forragem where Nome = " & "'" & Me.txtNomeForragem.Text & "'"
        'Mensagem se realmente quer excluir
        If MsgBox("Excluir forragem?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            Try
                abrir()
                cmd = New SQLiteCommand(sqlDelete, con)
                'cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@Nome", txtNomeForragem.Text)
                cmd.ExecuteNonQuery()
                MsgBox("Forragem excluida com sucesso!")

                DeletarKPS() ' Isso precisa ser aqui.
                TabelaForragem()
                txtNomeForragem.Text = ""
                cbxQtdPeneiras.Text = "04 Peneiras"
                ConfigGrid()
            Catch ex As Exception
                MsgBox("Erro ao exluir forragem!" + ex.Message)
                fechar()
            End Try

        Else
            MsgBox("Você precisa escolher uma forragem na tabela!")
        End If

    End Sub

    Private Sub btnHistForragem_Click_1(sender As Object, e As EventArgs) Handles btnHistForragem.Click
        BuscarForragemAgrup1()
        BuscarForragemAgrup2()

        Dim x As Integer = dtgHistForrag1.Rows.Count
        If x > 0 Then
            HistForragem1()
            pnlHistForragem.Visible = True
            pnlForragemLabel.Visible = True
            pnlHistForragem.Location = New Point(1, 136)
            pnlHistForragem.BringToFront()
        End If

        Dim x2 As Integer = dtgHistForrag2.Rows.Count
        If x2 > 1 Then
            HistForragem2()
            pnlFor2.Visible = False
            pnlFor2.SendToBack()
        Else
            pnlFor2.Visible = True
            pnlFor2.BringToFront()
        End If

    End Sub

    Private Sub ConfigGridForragem()

        For Each columns As DataGridViewColumn In dtgForragem.Columns
            dtgForragem.Columns(columns.Index).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter ' centralizar as celulas
        Next

        With Me.dtgForragem

            .DefaultCellStyle.ForeColor = Color.FromArgb(90, 90, 90)
            .DefaultCellStyle.BackColor = Color.White
            '.ColumnHeadersDefaultCellStyle.Font = New Font("Inter", 8, FontStyle.Bold)
            .DefaultCellStyle.Font = New Font("Inter", 8.5, FontStyle.Bold)

            .Columns(0).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns(0).Width = 300
            .Columns(1).Width = 201
            .Columns(1).DefaultCellStyle.BackColor = Color.WhiteSmoke
            .Columns(2).Width = 271
            .Columns(3).Width = 272
            .Columns(3).DefaultCellStyle.BackColor = Color.WhiteSmoke
            .Columns(4).Visible = False
            .Columns(5).Visible = False
        End With

    End Sub

    Private Sub ConfigGridHistForragem1()

        For Each columns As DataGridViewColumn In dtgHistForragem1.Columns
            dtgHistForragem1.Columns(columns.Index).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter ' centralizar as celulas
        Next

        With Me.dtgHistForragem1

            .DefaultCellStyle.ForeColor = Color.FromArgb(90, 90, 90)
            .DefaultCellStyle.BackColor = Color.White
            '.ColumnHeadersDefaultCellStyle.Font = New Font("Inter", 8, FontStyle.Bold)
            .DefaultCellStyle.Font = New Font("Inter", 8.5, FontStyle.Bold)

            .Columns(0).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns(0).Width = 300
            .Columns(1).Width = 201
            .Columns(1).DefaultCellStyle.BackColor = Color.WhiteSmoke
            .Columns(2).Width = 271
            .Columns(3).Width = 272
            .Columns(3).DefaultCellStyle.BackColor = Color.WhiteSmoke
            .Columns(4).Visible = False
            .Columns(5).Visible = False
        End With


    End Sub
    Private Sub ConfigGridHistForragem2()

        For Each columns As DataGridViewColumn In dtgHistForragem2.Columns
            dtgHistForragem2.Columns(columns.Index).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter ' centralizar as celulas
        Next

        With Me.dtgHistForragem2

            .DefaultCellStyle.ForeColor = Color.FromArgb(90, 90, 90)
            .DefaultCellStyle.BackColor = Color.White
            '.ColumnHeadersDefaultCellStyle.Font = New Font("Inter", 8, FontStyle.Bold)
            .DefaultCellStyle.Font = New Font("Inter", 8.5, FontStyle.Bold)

            .Columns(0).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns(0).Width = 300
            .Columns(1).Width = 201
            .Columns(1).DefaultCellStyle.BackColor = Color.WhiteSmoke
            .Columns(2).Width = 271
            .Columns(3).Width = 272
            .Columns(3).DefaultCellStyle.BackColor = Color.WhiteSmoke
            .Columns(4).Visible = False
            .Columns(5).Visible = False
        End With

    End Sub

    Private Sub BuscarForragemAgrup1()

        Dim da As New SQLiteDataAdapter
        Dim dt As New DataTable
        Dim sql As String
        Try

            abrir()
            sql = "Select Nome, QtdPeneiras, IdPropriedade from Forragem where IdPropriedade = " & "'" & idFaz & "'" & " group by Nome, QtdPeneiras, IdPropriedade "
            'sql = "Select Nome, QtdPeneiras from Forragem group by Nome"
            'SUM(Quantidade)
            da = New SQLiteDataAdapter(sql, con)
            dt = New DataTable
            da.Fill(dt)
            dtgHistForrag1.DataSource = dt

            fechar()

        Catch ex As Exception

        End Try

        'dtgClientes.DataSource = dtgFazendas.DataSource
    End Sub

    Private Sub BuscarForragem1()

        Dim da As New SQLiteDataAdapter
        Dim dt As New DataTable
        Dim sql As String
        Try

            abrir()
            sql = "Select Peneira,Tamanho,Quantidade,PorcPorPeneira,Acima8mm,Cod from Forragem where Nome = " & "'" & Me.lblNomeForragem1.Text & "'"
            'sql = "Select Nome, QtdPeneiras from Forragem group by Nome"
            'SUM(Quantidade)
            da = New SQLiteDataAdapter(sql, con)
            dt = New DataTable
            da.Fill(dt)
            dtgHistForragem1.DataSource = dt

            fechar()

        Catch ex As Exception

        End Try

        'SomarAmostras()

        'dtgClientes.DataSource = dtgFazendas.DataSource
    End Sub

    Private Sub BuscarForragemAgrup2()

        Dim da As New SQLiteDataAdapter
        Dim dt As New DataTable
        Dim sql As String
        Try

            abrir()
            sql = "Select Nome, QtdPeneiras, IdPropriedade from Forragem where IdPropriedade = " & "'" & idFaz & "'" & " group by Nome, QtdPeneiras, IdPropriedade "
            'sql = "Select Nome, QtdPeneiras from Forragem group by Nome"
            'SUM(Quantidade)
            da = New SQLiteDataAdapter(sql, con)
            dt = New DataTable
            da.Fill(dt)
            dtgHistForrag2.DataSource = dt

            fechar()

        Catch ex As Exception

        End Try

        'dtgClientes.DataSource = dtgFazendas.DataSource
    End Sub

    Private Sub BuscarForragem2()

        Dim da As New SQLiteDataAdapter
        Dim dt As New DataTable
        Dim sql As String
        Try

            abrir()
            sql = "Select Peneira,Tamanho,Quantidade,PorcPorPeneira,Acima8mm,Cod from Forragem where Nome = " & "'" & Me.lblNomeForragem2.Text & "'"
            'sql = "Select Nome, QtdPeneiras from Forragem group by Nome"
            'SUM(Quantidade)
            da = New SQLiteDataAdapter(sql, con)
            dt = New DataTable
            da.Fill(dt)
            dtgHistForragem2.DataSource = dt

            fechar()

        Catch ex As Exception

        End Try

        'SomarAmostras()

        'dtgClientes.DataSource = dtgFazendas.DataSource
    End Sub

    Dim ForragemOld1 As String
    Dim row1 As Integer = 0
    Dim row2 As Integer = 1
    Private Sub HistForragem1()
        'lblNomeForragem1.Text = dtgHistForrag1.CurrentRow.Cells(0).Value
        'ForragemOld1 = dtgHistForrag1.CurrentRow.Cells(1).Value
        'lblPeneiraForragem1.Text = dtgHistForrag1.CurrentRow.Cells(1).Value

        lblNomeForragem1.Text = dtgHistForrag1.Rows(row1).Cells(0).Value
        ForragemOld1 = dtgHistForrag1.Rows(row1).Cells(1).Value
        lblPeneiraForragem1.Text = dtgHistForrag1.Rows(row1).Cells(1).Value

        BuscarForragem1()

        SomarAmostras()
        dtgForragem.Enabled = False
        ConfigGridHistForragem1()

        Dim vlr As Double = dtgHistForragem1.Rows(0).Cells(4).Value
        lblForAcima81.Text = vlr.ToString("F0") & "%"

        If lblPeneiraForragem1.Text = "03 Peneiras" Then
            pnlBase1.Location = New Point(42, 223)
        ElseIf lblPeneiraForragem1.Text = "04 Peneiras" Then
            pnlBase1.Location = New Point(42, 246)
        End If

        Dim dt() As String
        dt = dtgHistForragem1.Rows(0).Cells(5).Value.Split(" ")
        lblDtForragem1.Text = dt(0)
        lblDtForr1.Text = lblDtForragem1.Text

        If dtgHistForragem1.Rows.Count > 1 Then
            For Each row As DataGridViewRow In dtgHistForragem1.Rows
                Dim pctT As Double
                Dim qtd As Double
                qtd += row.Cells(2).Value.ToString
                Dim pct As Double = Double.Parse(row.Cells(3).Value.ToString().Replace("%", ""))
                pctT += pct

                lblQtdForragem1.Text = qtd.ToString("F0")
                lblPctForragem1.Text = pctT.ToString("F0") & "%"
            Next
        End If

    End Sub
    Dim ForragemOld2 As String

    Private Sub dtgHistForrag1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dtgHistForrag1.CellClick
        HistForragem1()
    End Sub
    Private Sub HistForragem2()
        'lblNomeForragem2.Text = dtgHistForrag2.CurrentRow.Cells(0).Value
        'ForragemOld2 = dtgHistForrag2.CurrentRow.Cells(1).Value
        'lblPeneiraForragem2.Text = dtgHistForrag2.CurrentRow.Cells(1).Value

        lblNomeForragem2.Text = dtgHistForrag2.Rows(row2).Cells(0).Value
        ForragemOld2 = dtgHistForrag2.Rows(row2).Cells(1).Value
        lblPeneiraForragem2.Text = dtgHistForrag2.Rows(row2).Cells(1).Value


        BuscarForragem2()

        SomarAmostras()
        dtgForragem.Enabled = False
        ConfigGridHistForragem2()
        Dim vlr As Double = dtgHistForragem2.Rows(0).Cells(4).Value
        lblForAcima82.Text = vlr.ToString("F0") & "%"

        If lblPeneiraForragem2.Text = "03 Peneiras" Then
            pnlBase2.Location = New Point(43, 538)
        ElseIf lblPeneiraForragem2.Text = "04 Peneiras" Then
            pnlBase2.Location = New Point(43, 561)
        End If

        Dim dt() As String
        dt = dtgHistForragem2.Rows(0).Cells(5).Value.Split(" ")
        lblDtForragem2.Text = dt(0)
        lbldtforr2.Text = lblDtForragem2.Text

        If dtgHistForragem2.Rows.Count > 1 Then
            For Each row As DataGridViewRow In dtgHistForragem2.Rows
                Dim pctT As Double
                Dim qtd As Double
                qtd += row.Cells(2).Value.ToString
                Dim pct As Double = Double.Parse(row.Cells(3).Value.ToString().Replace("%", ""))
                pctT += pct

                lblQtdForragem2.Text = qtd.ToString("F0")
                lblPctForragem2.Text = pctT.ToString("F0") & "%"
            Next
        End If

    End Sub
    Private Sub dtgHistForrag2_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dtgHistForrag2.CellClick
        HistForragem2()
    End Sub

    Private Sub SomarAmostras() 'Forragem
        Dim soma As Double = 0

        For Each row As DataGridViewRow In dtgForragem.Rows
            If Not IsDBNull(row.Cells(2).Value) AndAlso IsNumeric(row.Cells(2).Value) Then
                soma += Convert.ToDouble(row.Cells(2).Value)
            End If
        Next

        lblQtdForragem.Text = soma.ToString("F0")
    End Sub
    'Dim vlr As Double
    '    For i As Integer = 0 To dtgForragem.Rows.Count - 1
    '        If Double.IsNaN(vlr) Then
    '            dtgForragem.Rows(i).Cells(3).Value = "0"
    '            lblPctForragem.Text = "0"
    '        Else
    '            dtgForragem.Rows(i).Cells(3).Value = vlr.ToString("F2")
    '        End If
    '    Next
    Private Sub CalcularParticulas()

        Try
            Dim totalForragem As Double

            If Not Double.TryParse(lblQtdForragem.Text, totalForragem) OrElse totalForragem = 0 Then
                'MessageBox.Show("Quantidade total de forragem inválida ou zero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                'Exit Sub
            End If

            Dim somaPct As Double = 0

            For i As Integer = 0 To dtgForragem.Rows.Count - 1
                If Not IsDBNull(dtgForragem.Rows(i).Cells(2).Value) AndAlso IsNumeric(dtgForragem.Rows(i).Cells(2).Value) Then
                    Dim valor As Double = Convert.ToDouble(dtgForragem.Rows(i).Cells(2).Value)
                    Dim pct As Double = (valor / totalForragem) * 100

                    dtgForragem.Rows(i).Cells(3).Value = pct.ToString("F2") & "%"
                    If Double.IsNaN(pct) Then
                        dtgForragem.Rows(i).Cells(3).Value = "0"
                        lblPctForragem.Text = "0"
                    Else
                        somaPct += pct
                    End If
                End If
            Next

            lblPctForragem.Text = somaPct.ToString("F0") & "%"

            ' Soma dos dois primeiros percentuais (sem o "%")
            If dtgForragem.Rows.Count > 1 Then
                Dim ac8 As Double = Double.Parse(dtgForragem.Rows(0).Cells(3).Value.ToString().Replace("%", ""))
                Dim ac82 As Double = Double.Parse(dtgForragem.Rows(1).Cells(3).Value.ToString().Replace("%", ""))
                dtgForragem.Rows(0).Cells(4).Value = (ac8 + ac82).ToString("F0")
                lblAcima8.Text = (ac8 + ac82).ToString("F0") & "%"
            End If

        Catch ex As Exception
            MessageBox.Show("Erro ao calcular partículas: " & ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        'Dim pct As Double

        'On Error Resume Next
        'For i As Integer = 0 To dtgForragem.Rows.Count - 1

        '    pct = dtgForragem.Rows(i).Cells(2).Value / lblQtdForragem.Text * 100
        '    dtgForragem.Rows(i).Cells(3).Value = pct.ToString("F2") & "%"
        '    Dim somaPct As Double
        '    somaPct += dtgForragem.Rows(i).Cells(3).Value
        '    lblPctForragem.Text = somaPct.ToString("F0") & "%"
        'Next
        'Dim ac8 As Double
        'Dim ac82 As Double
        'ac8 = dtgForragem.Rows(0).Cells(3).Value
        'ac82 = dtgForragem.Rows(1).Cells(3).Value
        'dtgForragem.Rows(0).Cells(4).Value = ac8 + ac82

    End Sub

    Private Sub btnSalvarForragem_Click(sender As Object, e As EventArgs) Handles btnSalvarForragem.Click
        'EditarForragem()
        'EditarKPS()
        BuscarForragemAgrup()

    End Sub

    Private Sub btnNovaForragem_Click(sender As Object, e As EventArgs) Handles btnNovaForragem.Click
        txtNomeForragem.Text = ""
        cbxQtdPeneiras.Text = "04 Peneiras"
        txtNomeForragem.Enabled = True
        cbxQtdPeneiras.Enabled = True
        TabelaKPS()
        TabelaForragem()
        ConfigGridForragem()
        Label42.Visible = True
        'lblTAmostra.Visible = True
        lblQtdForragem.Text = 0
        lblPctForragem.Text = 0
        dtgForragem.Enabled = True
        'dtgKPS.Enabled = True

        btnExcluirForragem.Enabled = False
        btnEditarForragem.Enabled = False
        'NaN()

    End Sub

    Private Sub CarregarForragem() '_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dtgHistForragem.CellClick
        txtNomeForragem.Text = dtgHistForragem.Rows(0).Cells(0).Value
        ForragemOld = dtgHistForragem.Rows(0).Cells(1).Value
        cbxQtdPeneiras.Text = dtgHistForragem.Rows(0).Cells(1).Value

        txtNomeForragem.Enabled = False
        cbxQtdPeneiras.Enabled = False
        btnSalvarForragem.Enabled = False
        'btnExcluirForragem.Enabled = True
        'btnEditarForragem.Enabled = True
        BuscarForragem()

        'Label42.Visible = True
        'lblTAmostra.Visible = True
        AjustarLabelAmostras()

        dtgForragem.Enabled = False
        'dtgKPS.Enabled = False
        'BuscarKPS()
        'BuscarKPS()
        'CoresKPS()
        ConfigGridForragem()
        'If txtNomeKPS.Text <> "" Then
        '    btnSalvarKPS.Enabled = True
        'End If
        'Dim vlr As Double = dtgForragem.Rows(0).Cells(4).Value
        'lblAcima8.Text = vlr.ToString("F0") & "%"

        ''Calcular o rodapé da grid
        ''Dim qtd As Double
        ''Dim pct As Double
        'If dtgForragem.Rows.Count > 1 Then
        '    For Each row As DataGridViewRow In dtgForragem.Rows
        '        Dim pctT As Double
        '        Dim qtd As Double
        '        qtd += row.Cells(2).Value.ToString
        '        Dim pct As Double = Double.Parse(row.Cells(3).Value.ToString().Replace("%", ""))
        '        pctT += pct

        '        lblQtdForragem.Text = qtd.ToString("F0")
        '        lblPctForragem.Text = pctt.ToString("F0") & "%"
        '    Next
        'End If
        SomarAmostras()
        CalcularParticulas()

    End Sub

    'Private Sub dtgForragem_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles dtgForragem.CellBeginEdit
    '    If txtNomeForragem.Text <> "" And cbxQtdPeneiras.Text <> "" And lblTAmostra.Text <> "0" Then
    '        btnSalvarForragem.Enabled = True
    '    Else
    '        btnSalvarForragem.Enabled = False
    '    End If

    '    CalcularParticulas()
    'End Sub

    'Private Sub dtgForragem_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dtgForragem.CellEndEdit
    '    SomarAmostras()
    '    CalcularParticulas()
    'End Sub

    ' não aparecer NaN na celula
    'Private Sub NaN()
    '    Dim vlr As Double
    '    For i As Integer = 0 To dtgForragem.Rows.Count - 1
    '        If Double.IsNaN(vlr) Then
    '            dtgForragem.Rows(i).Cells(3).Value = "0"
    '            lblPctForragem.Text = "0"
    '        Else
    '            dtgForragem.Rows(i).Cells(3).Value = vlr.ToString("F2")
    '        End If
    '    Next

    'End Sub
    Private Sub dtgForragem_CellEnter(sender As Object, e As DataGridViewCellEventArgs) Handles dtgForragem.CellEnter
        SomarAmostras()
        CalcularParticulas()
    End Sub
    Private Sub dtgForragem_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dtgForragem.CellContentClick
        SomarAmostras()
        CalcularParticulas()

    End Sub
    'Private Sub btnAc_Click(sender As Object, e As EventArgs) Handles btnAc.Click
    '    row1 -= 2
    '    row2 -= 2
    '    ' row1 e 2 não podem ser menor q 0
    '    If row1 < 0 Then
    '        row1 = 0
    '    ElseIf row2 < 0 Then
    '        row2 = 0
    '    End If

    '    Dim x As Integer = dtgHistForrag1.Rows.Count
    '    If x < row1 Then
    '        HistForragem1()
    '    End If
    '    Dim x2 As Integer = dtgHistForrag2.Rows.Count
    '    If x2 < row2 Then
    '        HistForragem2()
    '    End If
    'End Sub

    'Private Sub btnAb_Click(sender As Object, e As EventArgs) Handles btnAb.Click
    '    row1 += 2
    '    row2 += 2
    '    Dim x As Integer = dtgHistForrag1.Rows.Count
    '    If x > row1 Then
    '        HistForragem1()
    '    End If
    '    Dim x2 As Integer = dtgHistForrag2.Rows.Count
    '    If x2 > row2 Then
    '        HistForragem2()
    '    End If
    'End Sub


    Private Sub btnGrafico_Click(sender As Object, e As EventArgs) Handles btnGrafico.Click
        pnlGrafico.Location = New Point(168, 68)
        tabParticulas.Controls.Add(pnlGrafico)
        pnlGrafico.BringToFront()
        pnlGrafico.Visible = True
    End Sub

    Private Sub btnFecharGrafico_Click(sender As Object, e As EventArgs) Handles btnFecharGrafico.Click
        pnlGrafico.Visible = False
    End Sub

    Private Sub btnGrafico1_Click(sender As Object, e As EventArgs) Handles btnGrafico1.Click
        pnlGrafico.Location = New Point(168, 68)
        pnlHistForragem.Controls.Add(pnlGrafico)
        pnlGrafico.BringToFront()
        pnlGrafico.Visible = True

    End Sub

    'Private Sub DeletarForragemPE()

    '    Dim cmd As SQLiteCommand
    '    Dim sqlDelete As String = "Delete from Forragem where Nome = " & "'" & txtNomeForragem.Text & "'"
    '    'Mensagem se realmente quer excluir


    '    abrir()
    '        cmd = New SQLiteCommand(sqlDelete, con)
    '    'cmd.CommandType = CommandType.StoredProcedure
    '    cmd.Parameters.AddWithValue("@Nome", ForragemOld)
    '    cmd.ExecuteNonQuery()

    '    'DeletarKPSPE() ' Isso precisa ser aqui.

    'End Sub


    'Private Sub btnExcluirForragem_Click(sender As Object, e As EventArgs) Handles btnExcluirForragem.Click
    '    'DeletarKPS()
    '    DeletarForragem()

    '    'BuscarForragemAgrup()
    '    btnCadastrarForragem.Visible = True
    '    btnCadastrarForragem.Enabled = True
    '    'TabelaKPS()
    '    'TabelaForragem()
    'End Sub

    'Private Sub EditarForragem()


    '    Dim sql As String
    '    Dim cmd As SQLiteCommand

    '    'sql = "Update Forragem set Nome=@Nome,QtdPeneiras=@QtdPeneira,Peneira=@Peneira,Tamanho=@Tamanho,Quantidade=@Quantidade,PorcPorPeneira=@PorcPorPeneira,Acima8mm=@Acima8mm,IdPropriedade=@IdPropriedade where Cod=@Cod"


    '    For Each row As DataGridViewRow In dtgForragem.Rows
    '        Try
    '            'sql = "UPDATE Forragem" ' set Nome=@Nome,QtdPeneiras=@QtdPeneira,Peneira=@Peneira,Tamanho=@Tamanho,Quantidade=@Quantidade,PorcPorPeneira=@PorcPorPeneira,Acima8mm=@Acima8mm,IdPropriedade=@IdPropriedade where Cod=@Cod"

    '            sql = "Update Forragem set Nome,QtdPeneiras,Peneira,Tamanho,Quantidade,PorcPorPeneira,Acima8mm,Cod,IdPropriedade WHERE Nome=@Nome,QtdPeneiras=@QtdPeneira,Peneira=@Peneira,Tamanho=@Tamanho,Quantidade=@Quantidade,PorcPorPeneira=@PorcPorPeneira,Acima8mm=@Acima8mm,Cod=@Cod,IdPropriedade=@IdPropriedade"

    '            cmd = New SQLiteCommand(sql, con)
    '            'cmd.CommandType = CommandType.StoredProcedure
    '            cmd.Parameters.AddWithValue("@Nome", txtNomeForragem.Text)
    '            cmd.Parameters.AddWithValue("@QtdPeneiras", cbxQtdPeneiras.Text)
    '            cmd.Parameters.AddWithValue("@Peneira", row.Cells("Peneiras").Value.ToString)
    '            cmd.Parameters.AddWithValue("@Tamanho", row.Cells("Tamanho (mm)").Value.ToString)
    '            cmd.Parameters.AddWithValue("@Quantidade", row.Cells("Qtd por peneira (g)").Value.ToString)
    '            cmd.Parameters.AddWithValue("@PorcPorPeneira", row.Cells("% Por peneira").Value.ToString)
    '            cmd.Parameters.AddWithValue("@Acima8mm", row.Cells("Forragem acima de 8mm").Value.ToString)
    '            cmd.Parameters.AddWithValue("@Cod", row.Cells("Cod").Value.ToString)
    '            cmd.Parameters.AddWithValue("@IdPropriedade", lblIdProp.Text)
    '            abrir()
    '            cmd.ExecuteNonQuery()
    '            'ListarComissoes()

    '        Catch ex As Exception
    '            'MsgBox("Erro ao salvar!" + ex.Message)
    '            fechar()
    '        End Try

    '    Next
    '    MsgBox("Atualizações realizadas com sucesso!")

    '    'btnEditarCliente.Enabled = False

    '    TabelaForragem()
    '    txtNomeForragem.Text = ""
    '    cbxQtdPeneiras.Text = "04 Peneiras"
    '    ConfigGrid()

    '    'Else
    '    'MsgBox("Preencha os campos corretamente!")

    '    'End If
    'End Sub

    Private Sub AjustarLabelAmostras() 'Forragem

        If Me.cbxQtdPeneiras.Text = "03 Peneiras" Then
            pnlBase.Location = New Point(38, 404)
        ElseIf Me.cbxQtdPeneiras.Text = "04 Peneiras" Then
            pnlBase.Location = New Point(38, 426)
        End If

        'If cbxQtdPeneiras.Text = "01 Peneira" Then
        '    Label42.Location = New Point(180, 257)
        '    lblTAmostra.Location = New Point(430, 257)
        'ElseIf cbxQtdPeneiras.Text = "02 Peneiras" Then
        '    Label42.Location = New Point(180, 280)
        '    lblTAmostra.Location = New Point(430, 280)
        'ElseIf cbxQtdPeneiras.Text = "03 Peneiras" Then
        '    Label42.Location = New Point(180, 303)
        '    lblTAmostra.Location = New Point(430, 303)
        '    'ElseIf cbxQtdPeneiras.Text = "04 Peneiras" Then
        '    '    Label42.Location = New Point(180, 326)
        '    '    lblTAmostra.Location = New Point(430, 326)
        'End If

        'If cbxQtdPeneiras.Text = "01 Peneira" Then
        '    Label42.Location = New Point(250, 257)
        '    lblTAmostra.Location = New Point(430, 257)
        'ElseIf cbxQtdPeneiras.Text = "02 Peneiras" Then
        '    Label42.Location = New Point(250, 257)
        '    lblTAmostra.Location = New Point(430, 257)
        'If cbxQtdPeneiras.Text = "03 Peneiras" Then
        '    Label42.Location = New Point(250, 280)
        '    lblTAmostra.Location = New Point(430, 280)
        'ElseIf cbxQtdPeneiras.Text = "04 Peneiras" Then
        '    Label42.Location = New Point(250, 303)
        '    lblTAmostra.Location = New Point(430, 303)
        'End If

    End Sub

    Private Sub TabelaForragem()

        Try

            Dim dt As New DataTable()

            dt.Columns.Add("Peneiras")
            dt.Columns.Add("Tamanho (mm)")
            dt.Columns.Add("Qtd por peneira (g)")
            dt.Columns.Add("% Por peneira")
            dt.Columns.Add("Forragem acima de 8mm")
            dt.Columns.Add("Cod")
            'If cbxQtdPeneiras.Text = "01 Peneira" Then
            '    dt.Rows.Add("Peneira 01", "19mm", "0", "0")
            '    'dt.Rows.Add("Fundo", "0", "0", "0")
            '    'Label42.Location = New Point(180, 143)
            '    'lblTAmostra.Location = New Point(430, 143)
            'ElseIf cbxQtdPeneiras.Text = "02 Peneiras" Then
            '    dt.Rows.Add("Peneira 01", "19mm", "0", "0")
            '    dt.Rows.Add("Peneira 02", "8mm", "0", "0")
            '    'dt.Rows.Add("Fundo", "0", "0", "0")
            '    'Label42.Location = New Point(180, 164)
            '    'lblTAmostra.Location = New Point(430, 164)
            If cbxQtdPeneiras.Text = "03 Peneiras" Then
                dt.Rows.Add("Peneira 01", "19mm", "0", "0", "0", "1")
                dt.Rows.Add("Peneira 02", "8mm", "0", "0", "0", "2")
                'dt.Rows.Add("Peneira 03", "1.8mm", "", "")
                dt.Rows.Add("Fundo", "0", "0", "0", "0", "4")
                ' Label42.Location = New Point(180, 187)
                ' lblTAmostra.Location = New Point(430, 187)
            ElseIf cbxQtdPeneiras.Text = "04 Peneiras" Then
                dt.Rows.Add("Peneira 01", "19mm", "0", "0", "0", "1")
                dt.Rows.Add("Peneira 02", "8mm", "0", "0", "0", "2")
                dt.Rows.Add("Peneira 03", "1.8mm", "0", "0", "0", "3")
                dt.Rows.Add("Fundo", "0", "0", "0", "0", "4")
                '    ' Label42.Location = New Point(180, 209)
                '    ' lblTAmostra.Location = New Point(430, 209)
            End If

            dtgForragem.DataSource = dt
        Catch ex As Exception
            Throw ex
        End Try
        'AjustarLabelAmostras()
    End Sub

    'Private Sub SomarAmostras() ' Forragem
    '    Dim soma As Double = 0

    '    For Each linha As DataGridViewRow In dtgForragem.Rows
    '        If Not IsDBNull(linha.Cells(2).Value) AndAlso IsNumeric(linha.Cells(2).Value) Then
    '            soma += Convert.ToDouble(linha.Cells(2).Value)
    '        End If
    '    Next

    '    lblQtdForragem.Text = soma.ToString("F0")
    'End Sub

    'Private Sub CalcularParticulas()
    '    Try
    '        Dim totalForragem As Double = 0
    '        If Double.TryParse(lblQtdForragem.Text, totalForragem) = False OrElse totalForragem = 0 Then Exit Sub

    '        Dim pct As Double = 0

    '        For i As Integer = 0 To dtgForragem.Rows.Count - 1
    '            If Not IsDBNull(dtgForragem.Rows(i).Cells(2).Value) AndAlso IsNumeric(dtgForragem.Rows(i).Cells(2).Value) Then
    '                Dim valor As Double = Convert.ToDouble(dtgForragem.Rows(i).Cells(2).Value)
    '                pct = (valor / totalForragem) * 100
    '                dtgForragem.Rows(i).Cells(3).Value = pct.ToString("F0") & "%"
    '            End If
    '        Next

    '        ' Soma dos dois primeiros percentuais (removendo "%")
    '        Dim ac8 As Double = 0
    '        Dim ac82 As Double = 0

    '        If dtgForragem.Rows.Count > 1 Then
    '            ac8 = Double.Parse(dtgForragem.Rows(0).Cells(3).Value.ToString().Replace("%", ""))
    '            ac82 = Double.Parse(dtgForragem.Rows(1).Cells(3).Value.ToString().Replace("%", ""))
    '            dtgForragem.Rows(0).Cells(4).Value = (ac8 + ac82).ToString("F0") & "%"
    '        End If

    '        lblPctForragem.Text = pct.ToString("F0") & "%"

    '    Catch ex As Exception
    '        MessageBox.Show("Erro ao calcular partículas: " & ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End Try
    'End Sub

   

    Private Sub cbxQtdPeneiras_MouseClick(sender As Object, e As MouseEventArgs) Handles cbxQtdPeneiras.MouseClick
        Threading.Thread.Sleep(100)
        TabelaForragem()
        AjustarLabelAmostras()
        'Label42.Visible = True
        'lblTAmostra.Visible = True
        'ConfigGrid()
        'Label42.Visible = True
        'lblTAmostra.Visible = True
    End Sub

    Private Sub cbxQtdPeneiras_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxQtdPeneiras.SelectedIndexChanged
        Threading.Thread.Sleep(100)
        TabelaForragem()
        
        'ConfigGrid()
        AjustarLabelAmostras()
    End Sub

    Dim gpos1 As Integer = 0
    Dim forragem1 As String
    Private Sub GraficoForragem1()

        Chart1.Series.Clear()
        Chart1.Titles.Clear()
        forragem1 = ""
        On Error Resume Next
        Dim p1 As Double = dtgForragem.Rows(0).Cells(3).Value ' = Format(p1.ToString("P"))
        Dim p2 As Double = dtgForragem.Rows(1).Cells(3).Value ' = Format(p2.ToString("P"))
        Dim p3 As Double = dtgForragem.Rows(2).Cells(3).Value ' = Format(p3.ToString("P"))
        Dim p4 As Double = 0
        If dtgForragem.Rows(3).Cells(3).Value > 0 Then
            p4 = dtgForragem.Rows(3).Cells(3).Value ' = Format(p4.ToString("P"))
        Else
            p4 = 0
        End If
        Dim ac8 As Double = dtgForragem.Rows(0).Cells(4).Value ' = Format(ac8.ToString("P"))

        Dim nome() As String
        nome = txtNomeForragem.Text.Split(" ")
        forragem1 = nome(0) & nome(1)

        Dim title = New Title()
        title.Font = (New Font("Arial", 8, FontStyle.Bold))
        title.ForeColor = Color.Black
        title.Text = forragem1
        Chart1.Titles.Add(title)
        Chart1.Series.Add("Forragem")

        On Error Resume Next
        With Chart1.Series("Forragem")

            'define o tipo de gráfico
            .ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column
            .BorderWidth = 2
            'define o titulo do gráfico
            ' .Titles.Add("Ali")
            .Palette = ChartColorPalette.BrightPastel
            .Points.AddXY("19", p1)
            .Points.AddXY("8", p2)
            If dtgForragem.Rows(2).Cells(3).Value <> 0 Then
                .Points.AddXY("1.8", p3)
            End If
            .Points.AddXY("Fd", p4)
            .Points.AddXY(">8", ac8)

            'Tamanho
            '.Size = New Size(Size.Width, 250)
            ' .Size = New Size(Size.Height, 165)

        End With

        With Chart1.ChartAreas("ChartArea1")
            'Eixo X
            Chart1.ChartAreas(0).AxisX.LabelStyle.Angle = -90
            .AxisX.Title = "Peneiras em (mm)"
            .AxisX.TitleFont = (New Font("Arial", 7, FontStyle.Bold))
            .AxisX.TitleForeColor = Color.Black
            'Eixo Y
            .AxisY.Title = "Particulas em (%)"
            .AxisY.TitleFont = (New Font("Arial", 7, FontStyle.Bold))
            .AxisY.TitleForeColor = Color.Black

        End With
        ' Chart1.Update()
        Chart1.DataBind()
        Chart1.Visible = True
    End Sub

    Dim gpos2 As Integer = 0
    Dim forragem2 As String = ""
    Private Sub GraficoForragem2()

        Chart2.Series.Clear()
        Chart2.Titles.Clear()
        forragem2 = ""
        On Error Resume Next
        Dim p1 As Double = dtgForragem.Rows(0).Cells(3).Value
        Dim p2 As Double = dtgForragem.Rows(1).Cells(3).Value
        Dim p3 As Double = dtgForragem.Rows(2).Cells(3).Value
        Dim p4 As Double = 0
        If dtgForragem.Rows(3).Cells(3).Value > 0 Then
            p4 = dtgForragem.Rows(3).Cells(3).Value
        Else
            p4 = 0
        End If
        Dim ac8 As Double = dtgForragem.Rows(0).Cells(4).Value

        Dim nome() As String
        nome = txtNomeForragem.Text.Split(" ")
        forragem2 = nome(0) & nome(1)

        Dim title = New Title()
        title.Font = (New Font("Arial", 8, FontStyle.Bold))
        title.ForeColor = Color.Black
        title.Text = forragem2
        Chart2.Titles.Add(title)


        Chart2.Series.Add("Forragem")

        On Error Resume Next
        With Chart2.Series("Forragem")

            'define o tipo de gráfico
            .ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column
            .BorderWidth = 2
            'define o titulo do gráfico
            ' .Titles.Add("Ali")
            .Palette = ChartColorPalette.BrightPastel
            .Points.AddXY("19", p1)
            .Points.AddXY("8", p2)
            If dtgForragem.Rows(2).Cells(3).Value <> 0 Then
                .Points.AddXY("1.8", p3)
            End If
            .Points.AddXY("Fd", p4)
            .Points.AddXY(">8", ac8)

            'Tamanho
            '.Size = New Size(Size.Width, 250)
            ' .Size = New Size(Size.Height, 165)

        End With

        With Chart2.ChartAreas("ChartArea1")
            'Eixo X
            Chart2.ChartAreas(0).AxisX.LabelStyle.Angle = -90
            .AxisX.Title = "Peneiras em (mm)"
            .AxisX.TitleFont = (New Font("Arial", 7, FontStyle.Bold))
            .AxisX.TitleForeColor = Color.Black
            'Eixo Y
            .AxisY.Title = "Particulas em (%)"
            .AxisY.TitleFont = (New Font("Arial", 7, FontStyle.Bold))
            .AxisY.TitleForeColor = Color.Black

        End With
        Chart2.DataBind()
        Chart2.Visible = True
    End Sub

    'Dim gpos3 As Integer = 0
    'Dim forragem3 As String = ""
    'Private Sub GraficoForragem3()

    '    Chart3.Series.Clear()
    '    Chart3.Titles.Clear()
    '    forragem3 = ""
    '    On Error Resume Next
    '    Dim p1 As Double = dtgForragem.Rows(0).Cells(3).Value
    '    Dim p2 As Double = dtgForragem.Rows(1).Cells(3).Value
    '    Dim p3 As Double = dtgForragem.Rows(2).Cells(3).Value
    '    Dim p4 As Double = 0
    '    If dtgForragem.Rows(3).Cells(3).Value > 0 Then
    '        p4 = dtgForragem.Rows(3).Cells(3).Value
    '    Else
    '        p4 = 0
    '    End If
    '    Dim ac8 As Double = dtgForragem.Rows(0).Cells(4).Value
    '    forragem3 = txtNomeForragem.Text

    '    Dim title = New Title()
    '    title.Font = (New Font("Arial", 10, FontStyle.Bold))
    '    title.ForeColor = Color.Black
    '    title.Text = forragem3
    '    Chart3.Titles.Add(title)

    '    Chart3.Series.Add("Forragem")

    '    On Error Resume Next
    '    With Chart3.Series("Forragem")

    '        'define o tipo de gráfico
    '        .ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column
    '        .BorderWidth = 2
    '        'define o titulo do gráfico
    '        ' .Titles.Add("Ali")
    '        .Palette = ChartColorPalette.BrightPastel
    '        .Points.AddXY("P1", p1)
    '        .Points.AddXY("P2", p2)
    '        If dtgForragem.Rows(2).Cells(3).Value <> 0 Then
    '            .Points.AddXY("P3", p3)
    '        End If
    '        .Points.AddXY("FD", p4)
    '        .Points.AddXY(">8mm", ac8)

    '        'Tamanho
    '        '.Size = New Size(Size.Width, 250)
    '        ' .Size = New Size(Size.Height, 165)

    '    End With
    '    Chart3.DataBind()
    '    Chart3.Visible = True
    'End Sub

    Dim ForragemOld As String
    Private Sub btnEditarForragem_Click(sender As Object, e As EventArgs) Handles btnEditarForragem.Click
        dtgForragem.Enabled = True
        dtgKPS.Enabled = True

        btnSalvarForragem.Enabled = True
        btnSalvarForragem.Visible = True
        'btnCadastrarForragem.visible = False

    End Sub
    'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
    'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX          SOBRAS          XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

    Private Sub CadastrarSobras()

        Dim sql As String
        Dim cmd As SQLiteCommand

        data = Now.ToString("dd-MM-yyyy")

        For Each row As DataGridViewRow In dtgSobras.Rows
            Try
                abrir()
                sql = "Insert into Sobras (Nome,QtdPeneiras,Peneira,Tamanho,Quantidade,PorcPorPeneira,Acima8mm,IdPropriedade) values (@Nome,@QtdPeneiras,@Peneira,@Tamanho,@Quantidade,@PorcPorPeneira,@Acima8mm,@IdPropriedade)"
                cmd = New SQLiteCommand(sql, con)
                'cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@Nome", txtNomeSobra.Text & " - " & data)
                cmd.Parameters.AddWithValue("@QtdPeneiras", cbxQtdPeneirasSobras.Text)
                cmd.Parameters.AddWithValue("@Peneira", row.Cells("Peneiras").Value.ToString)
                cmd.Parameters.AddWithValue("@Tamanho", row.Cells("Tamanho (mm)").Value.ToString)
                cmd.Parameters.AddWithValue("@Quantidade", row.Cells("Qtd por peneira (g)").Value.ToString)
                cmd.Parameters.AddWithValue("@PorcPorPeneira", row.Cells("% Por peneira").Value.ToString)
                cmd.Parameters.AddWithValue("@Acima8mm", row.Cells("Forragem acima de 8mm").Value.ToString)
                cmd.Parameters.AddWithValue("@IdPropriedade", lblIdProp.Text)

                cmd.ExecuteNonQuery()
                'ListarComissoes()

            Catch ex As Exception
                'MsgBox("Erro ao salvar!" + ex.Message)
                fechar()
            End Try

        Next
        MsgBox("Silagem cadastrada com sucesso!")

        'btnEditarCliente.Enabled = False

        ' dtgForragem.Rows.Clear()
        txtNomeSobra.Text = ""
        cbxQtdPeneirasSobras.Text = ""


        'Else
        'MsgBox("Preencha os campos corretamente!")

        'End If
    End Sub

    Private Sub BuscarSobrasAgrup()

        Dim da As New SQLiteDataAdapter
        Dim dt As New DataTable
        Dim sql As String
        Try

            abrir()
            sql = "Select Nome, QtdPeneiras from Sobras where IdPropriedade = " & " '" & idFaz & "'" & " group by Nome, QtdPeneiras"
            'sql = "Select Nome, QtdPeneiras from Forragem group by Nome"
            'SUM(Quantidade)
            da = New SQLiteDataAdapter(sql, con)
            dt = New DataTable
            da.Fill(dt)
            dtgHistSobras.DataSource = dt

            fechar()

        Catch ex As Exception

        End Try

        'dtgClientes.DataSource = dtgFazendas.DataSource
    End Sub

    Private Sub BuscarSobras()

        Dim da As New SQLiteDataAdapter
        Dim dt As New DataTable
        Dim sql As String
        Try

            abrir()
            sql = "Select Peneira,Tamanho,Quantidade,PorcPorPeneira,Acima8mm from Sobras where Nome = " & "'" & Me.txtNomeSobra.Text & "'"
            'sql = "Select Nome, QtdPeneiras from Forragem group by Nome"
            'SUM(Quantidade)
            da = New SQLiteDataAdapter(sql, con)
            dt = New DataTable
            da.Fill(dt)
            dtgSobras.DataSource = dt

            fechar()

        Catch ex As Exception

        End Try

        'SomarAmostrasSobras()

        'dtgClientes.DataSource = dtgFazendas.DataSource
    End Sub

    Private Sub DeletarSobras()

        Dim cmd As SQLiteCommand
        Dim sqlDelete As String = "Delete from Sobras where Nome = " & "'" & Me.txtNomeSobra.Text & "'"
        'Mensagem se realmente quer excluir
        If MsgBox("Excluir forragem?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            Try
                abrir()
                cmd = New SQLiteCommand(sqlDelete, con)
                'cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@Nome", txtNomeSobra.Text)
                cmd.ExecuteNonQuery()
                MsgBox("Amostra excluida com sucesso!")

                DeletarKPS() ' Isso precisa ser aqui.
                TabelaForragem()
                txtNomeForragem.Text = ""
                cbxQtdPeneiras.Text = "04 Peneiras"
                ConfigGrid()
            Catch ex As Exception
                MsgBox("Erro ao exluir Amostra!" + ex.Message)
                fechar()
            End Try

        Else
            MsgBox("Você precisa escolher uma Amostra na tabela!")
        End If

    End Sub

    Private Sub TabelaSobras()

        Try

            Dim dt As New DataTable()

            dt.Columns.Add("Peneiras")
            dt.Columns.Add("Tamanho (mm)")
            dt.Columns.Add("Qtd por peneira (g)")
            dt.Columns.Add("% Por peneira")
            dt.Columns.Add("Forragem acima de 8mm")

            'If cbxQtdPeneirasSobras.Text = "01 Peneira" Then
            '    dt.Rows.Add("Peneira 01", "19mm", "0", "0")
            '    'dt.Rows.Add("Fundo", "0", "0", "0")
            '    ' Label45.Location = New Point(180, 143)
            '    ' lblTAmostraSobra.Location = New Point(430, 143)
            'ElseIf cbxQtdPeneirasSobras.Text = "02 Peneiras" Then
            '    dt.Rows.Add("Peneira 01", "19mm", "0", "0")
            '    dt.Rows.Add("Peneira 02", "8mm", "0", "0")
            '    'dt.Rows.Add("Fundo", "0", "0", "0")
            '    ' Label45.Location = New Point(180, 164)
            '    ' lblTAmostraSobra.Location = New Point(430, 164)
            If cbxQtdPeneirasSobras.Text = "03 Peneiras" Then
                dt.Rows.Add("Peneira 01", "19mm", "0", "0")
                dt.Rows.Add("Peneira 02", "8mm", "0", "0")
                'dt.Rows.Add("Peneira 03", "1.8mm", "0", "0")
                dt.Rows.Add("Fundo", "0", "0", "0")
                ' Label45.Location = New Point(180, 187)
                ' lblTAmostraSobra.Location = New Point(430, 187)
            ElseIf cbxQtdPeneirasSobras.Text = "04 Peneiras" Then
                dt.Rows.Add("Peneira 01", "19mm", "0", "0")
                dt.Rows.Add("Peneira 02", "8mm", "0", "0")
                dt.Rows.Add("Peneira 03", "1.8mm", "0", "0")
                dt.Rows.Add("Fundo", "0", "0", "0")
                '    ' Label45.Location = New Point(180, 209)
                '    ' lblTAmostraSobra.Location = New Point(430, 209)
            End If

            dtgSobras.DataSource = dt
        Catch ex As Exception
            Throw ex
        End Try
        AjustarLabelAmostrasSobras()
    End Sub

    Private Sub cbxQtdPeneirasSobras_MouseClick(sender As Object, e As MouseEventArgs) Handles cbxQtdPeneirasSobras.MouseClick
        Threading.Thread.Sleep(100)
        TabelaSobras()
        AjustarLabelAmostrasSobras()

        'Label42.Visible = True
        'lblTAmostra.Visible = True
        'ConfigGrid()
        'Label42.Visible = True
        'lblTAmostra.Visible = True
    End Sub

    Private Sub cbxQtdPeneirasSobras_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxQtdPeneirasSobras.SelectedIndexChanged
        Threading.Thread.Sleep(100)
        TabelaSobras()
        AjustarLabelAmostrasSobras()

        'ConfigGrid()
    End Sub

    Private Sub AjustarLabelAmostrasSobras()
       


        ''If cbxQtdPeneirasSobras.Text = "01 Peneira" Then
        ''    Label45.Location = New Point(180, 257)
        ''    lblTAmostraSobra.Location = New Point(430, 257)
        ''ElseIf cbxQtdPeneirasSobras.Text = "02 Peneiras" Then
        ''    Label45.Location = New Point(180, 257)
        ''    lblTAmostraSobra.Location = New Point(430, 257)
        If cbxQtdPeneirasSobras.Text = "03 Peneiras" Then
            Label45.Location = New Point(180, 280)
            lblTAmostraSobra.Location = New Point(430, 280)
        ElseIf cbxQtdPeneirasSobras.Text = "04 Peneiras" Then
            Label45.Location = New Point(180, 303)
            lblTAmostraSobra.Location = New Point(430, 303)
        End If

    End Sub

    Private Sub SomarAmostrasSobras()

        Dim linha As DataGridViewRow
        Dim soma As Double
        'Dim v As Double

        For Each linha In dtgSobras.Rows
            If linha.Cells(2).Value <> "" Then
                soma = soma + linha.Cells(2).Value
            End If

        Next
        lblTAmostraSobra.Text = soma '  & " " & "g"
    End Sub

    Private Sub CalcularParticulasSobras()
        Dim pct As Double

        On Error Resume Next
        For i As Integer = 0 To dtgSobras.Rows.Count - 1

            pct = dtgSobras.Rows(i).Cells(2).Value / lblTAmostraSobra.Text * 100
            dtgSobras.Rows(i).Cells(3).Value = Format(pct, "#.#0")

        Next
        Dim ac8 As Double
        Dim ac82 As Double
        ac8 = dtgSobras.Rows(0).Cells(3).Value
        ac82 = dtgSobras.Rows(1).Cells(3).Value
        dtgSobras.Rows(0).Cells(4).Value = Format(ac8 + ac82, "#.#0")
    End Sub

    Private Sub dtgHistSobras_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dtgHistSobras.CellClick
        txtNomeSobra.Text = dtgHistSobras.CurrentRow.Cells(0).Value
        cbxQtdPeneirasSobras.Text = dtgHistSobras.CurrentRow.Cells(1).Value
        txtNomeSobra.Enabled = False
        cbxQtdPeneirasSobras.Enabled = False
        btnSalvarSobras.Enabled = False
        btnExcluirSobras.Enabled = True
        btnEditarSobras.Enabled = True
        BuscarSobras()
        'Label42.Visible = True
        'lblTAmostra.Visible = True
        SomarAmostrasSobras()
        AjustarLabelAmostrasSobras()
        ConfigGrid()
    End Sub

    Private Sub dtgSobras_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles dtgSobras.CellBeginEdit
        If txtNomeSobra.Text <> "" And cbxQtdPeneirasSobras.Text <> "" And lblTAmostraSobra.Text <> "0" And txtNomeSobra.Enabled = True Then
            btnSalvarSobras.Enabled = True
        Else
            btnSalvarSobras.Enabled = False
        End If
        SomarAmostrasSobras()
        CalcularParticulasSobras()
    End Sub


    Private Sub btnNovaSobra_Click(sender As Object, e As EventArgs) Handles btnNovaSobra.Click
        txtNomeSobra.Text = ""
        cbxQtdPeneirasSobras.Text = "04 Peneiras"
        txtNomeSobra.Enabled = True
        cbxQtdPeneirasSobras.Enabled = True

        TabelaSobras()
        ConfigGrid()
        Label45.Visible = True
        lblTAmostraSobra.Visible = True
        lblTAmostraSobra.Text = 0

        btnExcluirSobras.Enabled = False
        btnEditarSobras.Enabled = False
    End Sub

    Private Sub btnSalvarSobras_Click(sender As Object, e As EventArgs) Handles btnSalvarSobras.Click
        CadastrarSobras()
        btnSalvarSobras.Enabled = False

        BuscarSobrasAgrup()
    End Sub
    Dim gpos7 As Integer = 0
    Dim sobras1 As String = ""
    Private Sub GraficoSobras1()

        Chart7.Series.Clear()
        Chart7.Titles.Clear()
        sobras1 = ""

        On Error Resume Next
        Dim p1 As Double = dtgSobras.Rows(0).Cells(3).Value
        Dim p2 As Double = dtgSobras.Rows(1).Cells(3).Value
        Dim p3 As Double = dtgSobras.Rows(2).Cells(3).Value
        Dim p4 As Double = 0
        If dtgSobras.Rows(3).Cells(3).Value > 0 Then
            p4 = dtgSobras.Rows(3).Cells(3).Value
        Else
            p4 = 0
        End If
        Dim ac8 As Double = dtgSobras.Rows(0).Cells(4).Value

        Dim nome() As String
        nome = txtNomeSobra.Text.Split(" ")
        sobras1 = nome(0) & nome(1)

        Dim title = New Title()
        title.Font = (New Font("Arial", 8, FontStyle.Bold))
        title.ForeColor = Color.Black
        title.Text = sobras1
        Chart7.Titles.Add(title)

        Chart7.Series.Add("Sobras")

        On Error Resume Next
        With Chart7.Series("Sobras")

            'define o tipo de gráfico
            .ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column
            .BorderWidth = 2
            'define o titulo do gráfico
            ' .Titles.Add("Ali")
            .Palette = ChartColorPalette.BrightPastel
            .Points.AddXY("P1", p1)
            .Points.AddXY("P2", p2)
            If dtgForragem.Rows(2).Cells(3).Value <> 0 Then
                .Points.AddXY("P3", p3)
            End If
            .Points.AddXY("FD", p4)
            .Points.AddXY(">8mm", ac8)

            'Tamanho
            '.Size = New Size(Size.Width, 250)
            ' .Size = New Size(Size.Height, 165)

        End With
        With Chart7.ChartAreas("ChartArea1")
            'Eixo X
            Chart7.ChartAreas(0).AxisX.LabelStyle.Angle = -90
            .AxisX.Title = "Peneiras em (mm)"
            .AxisX.TitleFont = (New Font("Arial", 7, FontStyle.Bold))
            .AxisX.TitleForeColor = Color.Black
            'Eixo Y
            .AxisY.Title = "Particulas em (%)"
            .AxisY.TitleFont = (New Font("Arial", 7, FontStyle.Bold))
            .AxisY.TitleForeColor = Color.Black

        End With

        Chart7.DataBind()
        Chart7.Visible = True
    End Sub
    Dim gpos8 As Integer = 0
    Dim sobras2 As String = ""
    Private Sub GraficoSobras2()

        Chart8.Series.Clear()
        Chart8.Titles.Clear()
        sobras2 = ""

        On Error Resume Next
        Dim p1 As Double = dtgSobras.Rows(0).Cells(3).Value
        Dim p2 As Double = dtgSobras.Rows(1).Cells(3).Value
        Dim p3 As Double = dtgSobras.Rows(2).Cells(3).Value
        Dim p4 As Double = 0
        If dtgSobras.Rows(3).Cells(3).Value > 0 Then
            p4 = dtgSobras.Rows(3).Cells(3).Value
        Else
            p4 = 0
        End If
        Dim ac8 As Double = dtgSobras.Rows(0).Cells(4).Value

        Dim nome() As String
        nome = txtNomeSobra.Text.Split(" ")
        sobras2 = nome(0) & nome(1)

        Dim title = New Title()
        title.Font = (New Font("Arial", 8, FontStyle.Bold))
        title.ForeColor = Color.Black
        title.Text = sobras2
        Chart8.Titles.Add(title)

        Chart8.Series.Add("Sobras")

        On Error Resume Next
        With Chart8.Series("Sobras")

            'define o tipo de gráfico
            .ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column
            .BorderWidth = 2
            'define o titulo do gráfico
            ' .Titles.Add("Ali")
            .Palette = ChartColorPalette.BrightPastel
            .Points.AddXY("P1", p1)
            .Points.AddXY("P2", p2)
            If dtgForragem.Rows(2).Cells(3).Value <> 0 Then
                .Points.AddXY("P3", p3)
            End If
            .Points.AddXY("FD", p4)
            .Points.AddXY(">8mm", ac8)

        End With
        With Chart8.ChartAreas("ChartArea1")
            'Eixo X
            Chart8.ChartAreas(0).AxisX.LabelStyle.Angle = -90
            .AxisX.Title = "Peneiras em (mm)"
            .AxisX.TitleFont = (New Font("Arial", 7, FontStyle.Bold))
            .AxisX.TitleForeColor = Color.Black
            'Eixo Y
            .AxisY.Title = "Particulas em (%)"
            .AxisY.TitleFont = (New Font("Arial", 7, FontStyle.Bold))
            .AxisY.TitleForeColor = Color.Black

        End With

        Chart8.DataBind()
        Chart8.Visible = True
    End Sub

    'Dim sobras3 As String = ""
    'Private Sub GraficoSobras3()

    '    Chart9.Series.Clear()
    '    Chart9.Titles.Clear()
    '    sobras3 = ""

    '    On Error Resume Next
    '    Dim p1 As Double = dtgSobras.Rows(0).Cells(3).Value
    '    Dim p2 As Double = dtgSobras.Rows(1).Cells(3).Value
    '    Dim p3 As Double = dtgSobras.Rows(2).Cells(3).Value
    '    Dim p4 As Double = 0
    '    If dtgSobras.Rows(3).Cells(3).Value > 0 Then
    '        p4 = dtgSobras.Rows(3).Cells(3).Value
    '    Else
    '        p4 = 0
    '    End If
    '    Dim ac8 As Double = dtgSobras.Rows(0).Cells(4).Value
    '    sobras3 = txtNomeSobra.Text

    '    Dim title = New Title()
    '    title.Font = (New Font("Arial", 10, FontStyle.Bold))
    '    title.ForeColor = Color.Black
    '    title.Text = sobras3
    '    Chart9.Titles.Add(title)

    '    Chart9.Series.Add("Sobras")

    '    On Error Resume Next
    '    With Chart9.Series("Sobras")

    '        'define o tipo de gráfico
    '        .ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column
    '        .BorderWidth = 2
    '        'define o titulo do gráfico
    '        ' .Titles.Add("Ali")
    '        .Palette = ChartColorPalette.BrightPastel
    '        .Points.AddXY("P1", p1)
    '        .Points.AddXY("P2", p2)
    '        If dtgForragem.Rows(2).Cells(3).Value <> 0 Then
    '            .Points.AddXY("P3", p3)
    '        End If
    '        .Points.AddXY("FD", p4)
    '        .Points.AddXY(">8mm", ac8)

    '        'Tamanho
    '        '.Size = New Size(Size.Width, 250)
    '        ' .Size = New Size(Size.Height, 165)

    '    End With
    '    ' Chart1.Update()
    '    Chart9.DataBind()
    '    Chart9.Visible = True
    'End Sub

    Private Sub btnExcluirSobras_Click(sender As Object, e As EventArgs) Handles btnExcluirSobras.Click
        DeletarSobras()
    End Sub
    'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX  
    'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX        KPS        XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

    Private Sub CadastrarKPS()

        Dim sql As String
        Dim cmd As SQLiteCommand

        data = Now.ToString("dd-MM-yyyy HH:mm:ss")

        For Each row As DataGridViewRow In dtgKPS.Rows
            Try
                abrir()
                sql = "Insert into KPS (Nome,Peneira,PorcPorPeneira,IdPropriedade) values (@Nome,@Peneira,@PorcPorPeneira,@IdPropriedade)"
                cmd = New SQLiteCommand(sql, con)
                'cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@Nome", txtNomeForragem.Text & " - " & data)
                cmd.Parameters.AddWithValue("@Peneira", row.Cells("Peneiras").Value.ToString)
                cmd.Parameters.AddWithValue("@PorcPorPeneira", row.Cells("% por peneira").Value.ToString)
                cmd.Parameters.AddWithValue("@IdPropriedade", lblIdProp.Text)

                cmd.ExecuteNonQuery()
                'ListarComissoes()

            Catch ex As Exception
                'MsgBox("Erro ao salvar!" + ex.Message)
                fechar()
            End Try

        Next
        ' MsgBox("KPS cadastrado com sucesso!")
        TabelaKPS()
        'btnEditarCliente.Enabled = False

        ' dtgForragem.Rows.Clear()
        'txtNomeForragem.Text = ""
        'cbxQtdPeneiras.Text = ""


        'Else
        'MsgBox("Preencha os campos corretamente!")

        'End If
    End Sub

    Private Sub BuscarKPSAgrup()

        Dim da As New SQLiteDataAdapter
        Dim dt As New DataTable
        Dim sql As String
        Try

            abrir()
            sql = "Select Nome from KPS where IdPropriedade = " & "'" & idFaz & "'" & " group by Nome"
            'sql = "Select Nome, QtdPeneiras from Forragem group by Nome"
            'SUM(Quantidade)
            da = New SQLiteDataAdapter(sql, con)
            dt = New DataTable
            da.Fill(dt)
            dtgHistKPS.DataSource = dt

            fechar()

        Catch ex As Exception

        End Try

        'dtgClientes.DataSource = dtgFazendas.DataSource
    End Sub

    Private Sub BuscarKPS()

        Dim da As New SQLiteDataAdapter
        Dim dt As New DataTable
        Dim sql As String
        Try

            abrir()
            sql = "Select Peneira,PorcPorPeneira from KPS where Nome = " & "'" & Me.txtNomeForragem.Text & "'"
            'sql = "Select Nome, QtdPeneiras from Forragem group by Nome"
            'SUM(Quantidade)
            da = New SQLiteDataAdapter(sql, con)
            dt = New DataTable
            da.Fill(dt)
            dtgKPS.DataSource = dt

            fechar()

        Catch ex As Exception

        End Try

        'SomarAmostras()

        'dtgClientes.DataSource = dtgFazendas.DataSource
    End Sub

    Private Sub DeletarKPS()

        Dim cmd As SQLiteCommand
        Dim sqlDelete As String = "Delete from KPS where Nome = " & "'" & Me.txtNomeForragem.Text & "'"
        'Mensagem se realmente quer excluir
        'If MsgBox("Excluir KPS?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
        Try
            abrir()
            cmd = New SQLiteCommand(sqlDelete, con)
            'cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@Nome", txtNomeForragem.Text)
            cmd.ExecuteNonQuery()
            'MsgBox("KPS excluida com sucesso!")
            'TabelaKPS()
        Catch ex As Exception
            'MsgBox("Erro ao exluir KPS!" + ex.Message)
            fechar()
        End Try

        'Else
        'MsgBox("Você precisa escolher uma opção na tabela!")
        'End If

    End Sub

    'Private Sub DeletarKPSPE()

    '    Dim cmd As SQLiteCommand
    '    Dim sqlDelete As String = "Delete from KPS where Nome = " & "'" & Me.txtNomeForragem.Text & "'"

    '    abrir()
    '    cmd = New SQLiteCommand(sqlDelete, con)
    '    cmd.Parameters.AddWithValue("@Nome", txtNomeForragem.Text)
    '    cmd.ExecuteNonQuery()

    '    fechar()


    'End Sub
    'Private Sub EditarKPS()

    '    Dim sql As String
    '    Dim cmd As SQLiteCommand

    '    For Each row As DataGridViewRow In dtgKPS.Rows
    '        Try
    '            abrir()
    '            'sql = "Insert into KPS (Nome,Peneira,PorcPorPeneira,IdPropriedade) values (@Nome,@Peneira,@PorcPorPeneira,@IdPropriedade)"
    '            cmd = New SQLiteCommand(sql, con)
    '            'cmd.CommandType = CommandType.StoredProcedure
    '            cmd.Parameters.AddWithValue("@Nome", txtNomeForragem.Text)
    '            cmd.Parameters.AddWithValue("@Peneira", row.Cells("Peneiras").Value.ToString)
    '            cmd.Parameters.AddWithValue("@PorcPorPeneira", row.Cells("% por peneira").Value.ToString)
    '            cmd.Parameters.AddWithValue("@IdPropriedade", lblIdProp.Text)

    '            cmd.ExecuteNonQuery()
    '            'ListarComissoes()

    '        Catch ex As Exception
    '            'MsgBox("Erro ao salvar!" + ex.Message)
    '            fechar()
    '        End Try

    '    Next
    '    ' MsgBox("KPS cadastrado com sucesso!")
    '    'TabelaKPS()

    'End Sub

    Private Sub TabelaKPS()

        Try

            Dim dt As New DataTable()

            dt.Columns.Add("Peneiras")
            dt.Columns.Add("% por peneira")

            dt.Rows.Add("Abaixo de 4.75mm", "0")
            dt.Rows.Add("Acima de 4.75mm", "0")

            dtgKPS.DataSource = dt
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Private Sub btnSalvarKPS_Click(sender As Object, e As EventArgs)
        CadastrarKPS()
        TabelaKPS()
    End Sub

    Private Sub CoresKPS()

        Try

            If dtgKPS.Rows(0).Cells(1).Value > 70 Then
                dtgKPS.Rows(0).Cells(1).Style.ForeColor = Color.Green
                dtgKPS.Rows(0).Cells(1).Style.Font = New Font("Arial", 8, FontStyle.Bold)
            ElseIf dtgKPS.Rows(0).Cells(1).Value >= 50 And dtgKPS.Rows(0).Cells(1).Value <= 70 Then
                dtgKPS.Rows(0).Cells(1).Style.ForeColor = Color.Goldenrod
                dtgKPS.Rows(0).Cells(1).Style.Font = New Font("Arial", 8, FontStyle.Bold)
            ElseIf dtgKPS.Rows(0).Cells(1).Value < 50 Then
                dtgKPS.Rows(0).Cells(1).Style.ForeColor = Color.Red
                dtgKPS.Rows(0).Cells(1).Style.Font = New Font("Arial", 8, FontStyle.Bold)
            End If

        Catch ex As Exception
        End Try
    End Sub

    Private Sub btnNovoKPS_Click(sender As Object, e As EventArgs)

        TabelaKPS()
        txtNomeKPS.Text = ""
        txtNomeKPS.Enabled = True

    End Sub

    Dim gpos3 As Integer = 0
    Dim kps1 As String = ""
    Private Sub GraficoKPS1()

        Chart3.Series.Clear()
        Chart3.Titles.Clear()
        kps1 = ""
        Dim var1 As Double = dtgKPS.Rows(0).Cells(1).Value
        Dim var2 As Double = dtgKPS.Rows(1).Cells(1).Value

        Dim nome() As String
        nome = txtNomeForragem.Text.Split(" ")
        kps1 = nome(0) & nome(1)

        Dim title = New Title()
        title.Font = (New Font("Arial", 8, FontStyle.Bold))
        title.ForeColor = Color.Black
        title.Text = "KPS" & "-" & kps1
        Chart3.Titles.Add(title)


        Chart3.Series.Add("KPS")

        On Error Resume Next
        With Chart3.Series("KPS")
            'define o tipo de gráfico
            .ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column
            .BorderWidth = 2
            'define o titulo do gráfico
            ' .Titles.Add("Ali")
            .Palette = ChartColorPalette.BrightPastel
            .Points.AddXY("< 4.75", var1)
            .Points.AddXY("> 4.75", var2)

        End With

        With Chart3.ChartAreas("ChartArea1")
            'Eixo X
            Chart3.ChartAreas(0).AxisX.LabelStyle.Angle = 0
            .AxisX.Title = "Peneiras em (mm)"
            .AxisX.TitleFont = (New Font("Arial", 7, FontStyle.Bold))
            .AxisX.TitleForeColor = Color.Black
            'Eixo Y
            .AxisY.Title = "Particulas em (%)"
            .AxisY.TitleFont = (New Font("Arial", 7, FontStyle.Bold))
            .AxisY.TitleForeColor = Color.Black

        End With
        Chart3.DataBind()
        Chart3.Visible = True
    End Sub

    Dim gpos4 As Integer = 0
    Dim kps2 As String = ""
    Private Sub GraficoKPS2()

        Chart4.Series.Clear()
        Chart4.Titles.Clear()
        kps2 = ""
        Dim var1 As Double = dtgKPS.Rows(0).Cells(1).Value
        Dim var2 As Double = dtgKPS.Rows(1).Cells(1).Value

        Dim nome() As String
        nome = txtNomeForragem.Text.Split(" ")
        kps2 = nome(0) & nome(1)

        Dim title = New Title()
        title.Font = (New Font("Arial", 8, FontStyle.Bold))
        title.ForeColor = Color.Black
        title.Text = "KPS" & "-" & kps2
        Chart4.Titles.Add(title)

        Chart4.Series.Add("KPS")

        On Error Resume Next
        With Chart4.Series("KPS")

            'define o tipo de gráfico
            .ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column
            .BorderWidth = 2
            'define o titulo do gráfico
            ' .Titles.Add("Ali")
            .Palette = ChartColorPalette.BrightPastel
            .Points.AddXY("< 4.75", var1)
            .Points.AddXY("> 4.75", var2)

        End With
        With Chart4.ChartAreas("ChartArea1")
            'Eixo X
            Chart4.ChartAreas(0).AxisX.LabelStyle.Angle = 0
            .AxisX.Title = "Peneiras em (mm)"
            .AxisX.TitleFont = (New Font("Arial", 7, FontStyle.Bold))
            .AxisX.TitleForeColor = Color.Black
            'Eixo Y
            .AxisY.Title = "Particulas em (%)"
            .AxisY.TitleFont = (New Font("Arial", 7, FontStyle.Bold))
            .AxisY.TitleForeColor = Color.Black

        End With

        Chart4.DataBind()
        Chart4.Visible = True
    End Sub

    'Dim gpos6 As Integer = 0
    'Dim kps3 As String = ""
    'Private Sub GraficoKPS3()

    '    Chart6.Series.Clear()
    '    Chart6.Titles.Clear()
    '    kps3 = ""
    '    Dim var1 As Double = dtgKPS.Rows(0).Cells(1).Value
    '    Dim var2 As Double = dtgKPS.Rows(1).Cells(1).Value

    '    kps3 = txtNomeKPS.Text

    '    Dim title = New Title()
    '    title.Font = (New Font("Arial", 10, FontStyle.Bold))
    '    title.ForeColor = Color.Black
    '    title.Text = kps3
    '    Chart6.Titles.Add(title)


    '    Chart6.Series.Add("KPS")

    '    On Error Resume Next
    '    With Chart6.Series("KPS")

    '        'define o tipo de gráfico
    '        .ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column
    '        .BorderWidth = 2
    '        'define o titulo do gráfico
    '        ' .Titles.Add("Ali")
    '        .Palette = ChartColorPalette.BrightPastel
    '        .Points.AddXY("< 4.75", var1)
    '        .Points.AddXY("> 4.75", var2)

    '        'Tamanho
    '        '.Size = New Size(Size.Width, 250)
    '        ' .Size = New Size(Size.Height, 165)

    '    End With
    '    Chart6.DataBind()
    '    Chart6.Visible = True
    'End Sub

    Private Sub dtgKPS_CellLeave(sender As Object, e As DataGridViewCellEventArgs) Handles dtgKPS.CellLeave
        Dim x As Double
        x = dtgKPS.Rows(0).Cells(1).Value
        dtgKPS.Rows(1).Cells(1).Value = Format(100.0 - x, "##.#")
        CoresKPS()
    End Sub

    Private Sub dtgKPS_Click(sender As Object, e As EventArgs) Handles dtgKPS.Click
        Dim x As Double

        x = dtgKPS.Rows(0).Cells(1).Value
        dtgKPS.Rows(1).Cells(1).Value = Format(100.0 - x, "##.#")
        CoresKPS()
    End Sub

    'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX         GRAFICOS PARTICULAS     XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

    'Variaveis para a posição de cada gráfico de acordo com os pontos que foram ocupados
    Dim posG1 As New Point(157, 59)
    Dim posG2 As New Point(350, 59)
    Dim posG3 As New Point(543, 59)
    Dim posG4 As New Point(736, 59)
    Dim posG5 As New Point(157, 300)
    Dim posG6 As New Point(350, 300)
    Dim posG7 As New Point(543, 300)
    Dim posG8 As New Point(736, 300)
    Dim posG9 As New Point(7, 392)

    'Variaveis para a posição de cada botão Resete de acordo com os pontos que foram ocupados
    Dim posBR1 As New Point(315, 33)
    Dim posBR2 As New Point(509, 33)
    Dim posBR3 As New Point(702, 33)
    Dim posBR4 As New Point(895, 33)
    Dim posBR5 As New Point(315, 274)
    Dim posBR6 As New Point(509, 274)
    Dim posBR7 As New Point(702, 274)
    Dim posBR8 As New Point(895, 274)
    Dim posBR9 As New Point(248, 397)


    Private Sub cbxGraficos_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxGraficos.SelectedIndexChanged
        Threading.Thread.Sleep(100)
        If cbxGraficos.Text = "Forragem" Then
            dtgGraficos.DataSource = dtgHistForragem.DataSource
        ElseIf cbxGraficos.Text = "KPS" Then
            dtgGraficos.DataSource = dtgHistKPS.DataSource
        ElseIf cbxGraficos.Text = "Trato" Then
            dtgGraficos.DataSource = dtgHistTratos.DataSource
        ElseIf cbxGraficos.Text = "Sobras" Then
            dtgGraficos.DataSource = dtgHistSobras.DataSource
        End If
        On Error Resume Next
        With dtgGraficos
            .ColumnHeadersDefaultCellStyle.BackColor = Color.Gainsboro
            .ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Regular)
            .DefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Regular)
            '.Columns(0).HeaderText = "Peneira"
            .Columns(0).Width = 120
            .Columns(1).Visible = False
            .Columns(2).Visible = False
            .Columns(3).Visible = False
        End With
    End Sub

    Private Sub dtgGraficos_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dtgGraficos.CellClick

        If cbxGraficos.Text = "Forragem" Then
            txtNomeForragem.Text = dtgHistForragem.CurrentRow.Cells(0).Value
            BuscarForragem()
        ElseIf cbxGraficos.Text = "KPS" Then
            txtNomeForragem.Text = dtgGraficos.CurrentRow.Cells(0).Value
            BuscarKPS()
        ElseIf cbxGraficos.Text = "Sobras" Then
            txtNomeSobra.Text = dtgGraficos.CurrentRow.Cells(0).Value
            BuscarSobras()
        ElseIf cbxGraficos.Text = "Trato" Then
            txtNomeTrato.Text = dtgHistTratos.CurrentRow.Cells(0).Value
            BuscarTratos()
        End If

        'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXX   FORRAGEM      XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

        'VERIFICAR ERRO DE gposx e posGx pois um não está representando o outro 

        If gpos1 = 0 And cbxGraficos.Text = "Forragem" Then
            If forragem1 = "" Then
                GraficoForragem1()
                Chart1.Location = New Point(posG1) ' Posição do grafico
                btnRstGf1.Location = New Point(posBR1) ' Posição do Botão ResetGrafico sempre acopanha o numero do grafico ex: btnTstGf1 para Chart1
                btnRstGf1.Visible = True
                gpos1 = 2 'DIFERENTE DE 0 ZERO
            ElseIf forragem2 = "" Then
                GraficoForragem2()
                Chart2.Location = New Point(posG1)
                btnRstGf2.Location = New Point(posBR1)
                btnRstGf2.Visible = True
                gpos1 = 2
                'ElseIf forragem3 = "" Then
                '    GraficoForragem3()
                '    Chart3.Location = New Point(posG1)
                '    btnRstGf3.Location = New Point(posBR1)
                '    btnRstGf3.Visible = True
                '    gpos1 = 2
            Else
                MsgBox("O limite de gráficos para forragem são 2 unidades")

            End If

        ElseIf gpos2 = 0 And cbxGraficos.Text = "Forragem" Then
            If forragem1 = "" Then
                GraficoForragem1()
                Chart1.Location = New Point(posG2)
                btnRstGf1.Location = New Point(posBR2)
                btnRstGf1.Visible = True
                gpos2 = 2
            ElseIf forragem2 = "" Then
                GraficoForragem2()
                Chart2.Location = New Point(posG2)
                btnRstGf2.Location = New Point(posBR2)
                btnRstGf2.Visible = True
                gpos2 = 2
                'ElseIf forragem3 = "" Then
                '    GraficoForragem3()
                '    Chart3.Location = New Point(posG2)
                '    btnRstGf3.Location = New Point(posBR2)
                '    btnRstGf3.Visible = True
                '    gpos2 = 2
            Else
                MsgBox("O limite de gráficos para forragem são 2 unidades")

            End If

        ElseIf gpos3 = 0 And cbxGraficos.Text = "Forragem" Then
            If forragem1 = "" Then
                GraficoForragem1()
                Chart1.Location = New Point(posG3)
                btnRstGf1.Location = New Point(posBR3)
                btnRstGf1.Visible = True
                gpos3 = 2
            ElseIf forragem2 = "" Then
                GraficoForragem2()
                Chart2.Location = New Point(posG3)
                btnRstGf2.Location = New Point(posBR3)
                btnRstGf2.Visible = True
                gpos3 = 2
                'ElseIf forragem3 = "" Then
                '    GraficoForragem3()
                '    Chart3.Location = New Point(posG3)
                '    btnRstGf3.Location = New Point(posBR3)
                '    btnRstGf3.Visible = True
                '    gpos3 = 2
            Else
                MsgBox("O limite de gráficos para forragem são 2 unidades")

            End If

        ElseIf gpos4 = 0 And cbxGraficos.Text = "Forragem" Then
            If forragem1 = "" Then
                GraficoForragem1()
                Chart1.Location = New Point(posG4)
                btnRstGf1.Location = New Point(posBR4)
                btnRstGf1.Visible = True
                gpos4 = 2
            ElseIf forragem2 = "" Then
                GraficoForragem2()
                Chart2.Location = New Point(posG4)
                btnRstGf2.Location = New Point(posBR4)
                btnRstGf2.Visible = True
                gpos4 = 2
                'ElseIf forragem3 = "" Then
                '    GraficoForragem3()
                '    Chart3.Location = New Point(posG4)
                '    btnRstGf3.Location = New Point(posBR4)
                '    btnRstGf3.Visible = True
                '    gpos4 = 2
            Else
                MsgBox("O limite de gráficos para forragem são 2 unidades")

            End If
        ElseIf gpos5 = 0 And cbxGraficos.Text = "Forragem" Then
            If forragem1 = "" Then
                GraficoForragem1()
                Chart1.Location = New Point(posG5)
                btnRstGf1.Location = New Point(posBR5)
                btnRstGf1.Visible = True
                gpos5 = 2
            ElseIf forragem2 = "" Then
                GraficoForragem2()
                Chart2.Location = New Point(posG5)
                btnRstGf2.Location = New Point(posBR5)
                btnRstGf2.Visible = True
                gpos5 = 2
                'ElseIf forragem3 = "" Then
                '    GraficoForragem3()
                '    Chart3.Location = New Point(posG5)
                '    btnRstGf3.Location = New Point(posBR5)
                '    btnRstGf3.Visible = True
                '    gpos5 = 2
            Else
                MsgBox("O limite de gráficos para forragem são 2 unidades")

            End If

        ElseIf gpos6 = 0 And cbxGraficos.Text = "Forragem" Then

            If forragem1 = "" Then
                GraficoForragem1()
                Chart1.Location = New Point(posG6)
                btnRstGf1.Location = New Point(posBR6)
                btnRstGf1.Visible = True
                gpos6 = 2
            ElseIf forragem2 = "" Then
                GraficoForragem2()
                Chart2.Location = New Point(posG6)
                btnRstGf2.Location = New Point(posBR6)
                btnRstGf2.Visible = True
                gpos6 = 2
                'ElseIf forragem3 = "" Then
                '    GraficoForragem3()
                '    Chart3.Location = New Point(posG6)
                '    btnRstGf3.Location = New Point(posBR6)
                '    btnRstGf3.Visible = True
                '    gpos6 = 2
            Else
                MsgBox("O limite de gráficos para forragem são 2 unidades")

            End If

        ElseIf gpos7 = 0 And cbxGraficos.Text = "Forragem" Then

            If forragem1 = "" Then
                GraficoForragem1()
                Chart1.Location = New Point(posG7)
                btnRstGf1.Location = New Point(posBR7)
                btnRstGf1.Visible = True
                gpos7 = 2
            ElseIf forragem2 = "" Then
                GraficoForragem2()
                Chart2.Location = New Point(posG7)
                btnRstGf2.Location = New Point(posBR7)
                btnRstGf2.Visible = True
                gpos7 = 2
                'ElseIf forragem3 = "" Then
                '    GraficoForragem3()
                '    Chart3.Location = New Point(posG7)
                '    btnRstGf3.Location = New Point(posBR7)
                '    btnRstGf3.Visible = True
                '    gpos7 = 2
            Else
                MsgBox("O limite de gráficos para forragem são 2 unidades")

            End If

        ElseIf gpos8 = 0 And cbxGraficos.Text = "Forragem" Then

            If forragem1 = "" Then
                GraficoForragem1()
                Chart1.Location = New Point(posG8)
                btnRstGf1.Location = New Point(posBR8)
                btnRstGf1.Visible = True
                gpos8 = 2
            ElseIf forragem2 = "" Then
                GraficoForragem2()
                Chart2.Location = New Point(posG8)
                btnRstGf2.Location = New Point(posBR8)
                btnRstGf2.Visible = True
                gpos8 = 2
                'ElseIf forragem3 = "" Then
                '    GraficoForragem3()
                '    Chart3.Location = New Point(posG8)
                '    btnRstGf3.Location = New Point(posBR8)
                '    btnRstGf3.Visible = True
                '    gpos8 = 2
            Else
                MsgBox("O limite de gráficos para forragem são 2 unidades")

            End If

            'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX   KPS        XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

        ElseIf gpos1 = 0 And cbxGraficos.Text = "KPS" Then
            If kps1 = "" Then
                GraficoKPS1()
                Chart3.Location = New Point(posG1)
                btnRstGf3.Location = New Point(posBR1)
                btnRstGf3.Visible = True
                gpos1 = 2
            ElseIf kps2 = "" Then
                GraficoKPS2()
                Chart4.Location = New Point(posG1)
                btnRstGf4.Location = New Point(posBR1)
                btnRstGf4.Visible = True
                gpos1 = 2
                'ElseIf kps3 = "" Then
                '    GraficoKPS3()
                '    Chart6.Location = New Point(posG1)
                '    btnRstGf6.Location = New Point(posBR1)
                '    btnRstGf6.Visible = True
                '    gpos1 = 2
            Else
                MsgBox("O limite de gráficos para KPS são 2 unidades")

            End If

        ElseIf gpos2 = 0 And cbxGraficos.Text = "KPS" Then
            If kps1 = "" Then
                GraficoKPS1()
                Chart3.Location = New Point(posG2)
                btnRstGf3.Location = New Point(posBR2)
                btnRstGf3.Visible = True
                gpos2 = 2
            ElseIf kps2 = "" Then
                GraficoKPS2()
                Chart4.Location = New Point(posG2)
                btnRstGf4.Location = New Point(posBR2)
                btnRstGf4.Visible = True
                gpos2 = 2
                'ElseIf kps3 = "" Then
                '    GraficoKPS3()
                '    Chart6.Location = New Point(posG2)
                '    btnRstGf6.Location = New Point(posBR2)
                '    btnRstGf6.Visible = True
                '    gpos2 = 2
            Else
                MsgBox("O limite de gráficos para KPS são 2 unidades")

            End If

        ElseIf gpos3 = 0 And cbxGraficos.Text = "KPS" Then
            If kps1 = "" Then
                GraficoKPS1()
                Chart3.Location = New Point(posG3)
                btnRstGf3.Location = New Point(posBR3)
                btnRstGf3.Visible = True
                gpos3 = 2
            ElseIf kps2 = "" Then
                GraficoKPS2()
                Chart4.Location = New Point(posG3)
                btnRstGf4.Location = New Point(posBR3)
                btnRstGf4.Visible = True
                gpos3 = 2
                'ElseIf kps3 = "" Then
                '    GraficoKPS3()
                '    Chart6.Location = New Point(posG3)
                '    btnRstGf6.Location = New Point(posBR3)
                '    btnRstGf6.Visible = True
                '    gpos3 = 2
            Else
                MsgBox("O limite de gráficos para KPS são 2 unidades")

            End If

        ElseIf gpos4 = 0 And cbxGraficos.Text = "KPS" Then
            If kps1 = "" Then
                GraficoKPS1()
                Chart3.Location = New Point(posG4)
                btnRstGf3.Location = New Point(posBR4)
                btnRstGf3.Visible = True
                gpos4 = 2
            ElseIf kps2 = "" Then
                GraficoKPS2()
                Chart4.Location = New Point(posG4)
                btnRstGf4.Location = New Point(posBR4)
                btnRstGf4.Visible = True
                gpos4 = 2
                'ElseIf kps3 = "" Then
                '    GraficoKPS3()
                '    Chart6.Location = New Point(posG4)
                '    btnRstGf6.Location = New Point(posBR4)
                '    btnRstGf6.Visible = True
                '    gpos4 = 2
            Else
                MsgBox("O limite de gráficos para KPS são 2 unidades")

            End If

        ElseIf gpos5 = 0 And cbxGraficos.Text = "KPS" Then
            If kps1 = "" Then
                GraficoKPS1()
                Chart3.Location = New Point(posG5)
                btnRstGf3.Location = New Point(posBR5)
                btnRstGf3.Visible = True
                gpos5 = 2
            ElseIf kps2 = "" Then
                GraficoKPS2()
                Chart4.Location = New Point(posG5)
                btnRstGf4.Location = New Point(posBR5)
                btnRstGf4.Visible = True
                gpos5 = 2
                'ElseIf kps3 = "" Then
                '    GraficoKPS3()
                '    Chart6.Location = New Point(posG5)
                '    btnRstGf6.Location = New Point(posBR5)
                '    btnRstGf6.Visible = True
                '    gpos5 = 2
            Else
                MsgBox("O limite de gráficos para KPS são 2 unidades")

            End If

        ElseIf gpos6 = 0 And cbxGraficos.Text = "KPS" Then
            If kps1 = "" Then
                GraficoKPS1()
                Chart3.Location = New Point(posG6)
                btnRstGf3.Location = New Point(posBR6)
                btnRstGf3.Visible = True
                gpos6 = 2
            ElseIf kps2 = "" Then
                GraficoKPS2()
                Chart4.Location = New Point(posG6)
                btnRstGf4.Location = New Point(posBR6)
                btnRstGf4.Visible = True
                gpos6 = 2
                'ElseIf kps3 = "" Then
                '    GraficoKPS3()
                '    Chart6.Location = New Point(posG6)
                '    btnRstGf6.Location = New Point(posBR6)
                '    btnRstGf6.Visible = True
                '    gpos6 = 2
            Else
                MsgBox("O limite de gráficos para KPS são 2 unidades")

            End If

        ElseIf gpos7 = 0 And cbxGraficos.Text = "KPS" Then
            If kps1 = "" Then
                GraficoKPS1()
                Chart3.Location = New Point(posG7)
                btnRstGf3.Location = New Point(posBR7)
                btnRstGf3.Visible = True
                gpos7 = 2
            ElseIf kps2 = "" Then
                GraficoKPS2()
                Chart4.Location = New Point(posG7)
                btnRstGf4.Location = New Point(posBR7)
                btnRstGf4.Visible = True
                gpos7 = 2
                'ElseIf kps3 = "" Then
                '    GraficoKPS3()
                '    Chart6.Location = New Point(posG6)
                '    btnRstGf6.Location = New Point(posBR6)
                '    btnRstGf6.Visible = True
                '    gpos6 = 2
            Else
                MsgBox("O limite de gráficos para KPS são 2 unidades")

            End If

        ElseIf gpos8 = 0 And cbxGraficos.Text = "KPS" Then
            If kps1 = "" Then
                GraficoKPS1()
                Chart3.Location = New Point(posG8)
                btnRstGf3.Location = New Point(posBR8)
                btnRstGf3.Visible = True
                gpos8 = 2
            ElseIf kps2 = "" Then
                GraficoKPS2()
                Chart4.Location = New Point(posG8)
                btnRstGf4.Location = New Point(posBR8)
                btnRstGf4.Visible = True
                gpos8 = 2
                'ElseIf kps3 = "" Then
                '    GraficoKPS3()
                '    Chart6.Location = New Point(posG6)
                '    btnRstGf6.Location = New Point(posBR6)
                '    btnRstGf6.Visible = True
                '    gpos6 = 2
            Else
                MsgBox("O limite de gráficos para KPS são 2 unidades")

            End If

            'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX   SOBRAS      XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX


        ElseIf gpos1 = 0 And cbxGraficos.Text = "Sobras" Then
            If sobras1 = "" Then
                GraficoSobras1()
                Chart7.Location = New Point(posG1)
                btnRstGf7.Location = New Point(posBR1)
                btnRstGf7.Visible = True
                gpos1 = 2
            ElseIf sobras2 = "" Then
                GraficoSobras2()
                Chart8.Location = New Point(posG1)
                btnRstGf8.Location = New Point(posBR1)
                btnRstGf8.Visible = True
                gpos1 = 2
                'ElseIf sobras3 = "" Then
                '    GraficoSobras3()
                '    Chart9.Location = New Point(posG1)
                '    'btnrstgf9.Location = New Point(posBR1)
                '    'btnrstgf9.Visible = True
                '    gpos1 = 2
            Else
                MsgBox("O limite de gráficos para Sobras são 2 unidades")

            End If

        ElseIf gpos2 = 0 And cbxGraficos.Text = "Sobras" Then
            If sobras1 = "" Then
                GraficoSobras1()
                Chart7.Location = New Point(posG2)
                btnRstGf7.Location = New Point(posBR2)
                btnRstGf7.Visible = True
                gpos2 = 2
            ElseIf sobras2 = "" Then
                GraficoSobras2()
                Chart8.Location = New Point(posG2)
                btnRstGf8.Location = New Point(posBR2)
                btnRstGf8.Visible = True
                gpos2 = 2
                'ElseIf sobras3 = "" Then
                '    GraficoSobras3()
                '    Chart9.Location = New Point(posG2)
                '    'btnrstgf9.Location = New Point(posBR2)
                '    'btnrstgf9.Visible = True
                '    gpos2 = 2
            Else
                MsgBox("O limite de gráficos para Sobras são 2 unidades")

            End If

        ElseIf gpos3 = 0 And cbxGraficos.Text = "Sobras" Then
            If sobras1 = "" Then
                GraficoSobras1()
                Chart7.Location = New Point(posG3)
                btnRstGf7.Location = New Point(posBR3)
                btnRstGf7.Visible = True
                gpos3 = 2
            ElseIf sobras2 = "" Then
                GraficoSobras2()
                Chart8.Location = New Point(posG3)
                btnRstGf8.Location = New Point(posBR3)
                btnRstGf8.Visible = True
                gpos3 = 2
                'ElseIf sobras3 = "" Then
                '    GraficoSobras3()
                '    Chart9.Location = New Point(posG3)
                '    'btnrstgf9.Location = New Point(posBR3)
                '    'btnrstgf9.Visible = True
                '    gpos3 = 2
            Else
                MsgBox("O limite de gráficos para Sobras são 2 unidades")

            End If

        ElseIf gpos4 = 0 And cbxGraficos.Text = "Sobras" Then
            If sobras1 = "" Then
                GraficoSobras1()
                Chart7.Location = New Point(posG4)
                btnRstGf7.Location = New Point(posBR4)
                btnRstGf7.Visible = True
                gpos4 = 2
            ElseIf sobras2 = "" Then
                GraficoSobras2()
                Chart8.Location = New Point(posG4)
                btnRstGf8.Location = New Point(posBR4)
                btnRstGf8.Visible = True
                gpos4 = 2
                'ElseIf sobras3 = "" Then
                '    GraficoSobras3()
                '    Chart9.Location = New Point(posG4)
                '    'btnrstgf9.Location = New Point(posBR4)
                '    'btnrstgf9.Visible = True
                '    gpos4 = 2
            Else
                MsgBox("O limite de gráficos para Sobras são 2 unidades")

            End If
        ElseIf gpos5 = 0 And cbxGraficos.Text = "Sobras" Then
            If sobras1 = "" Then
                GraficoSobras1()
                Chart7.Location = New Point(posG5)
                btnRstGf7.Location = New Point(posBR5)
                btnRstGf7.Visible = True
                gpos5 = 2
            ElseIf sobras2 = "" Then
                GraficoSobras2()
                Chart8.Location = New Point(posG5)
                btnRstGf8.Location = New Point(posBR5)
                btnRstGf8.Visible = True
                gpos5 = 2
                'ElseIf sobras3 = "" Then
                '    GraficoSobras3()
                '    Chart9.Location = New Point(posG5)
                '    'btnrstgf9.Location = New Point(posBR5)
                '    'btnrstgf9.Visible = True
                '    gpos5 = 2
            Else
                MsgBox("O limite de gráficos para Sobras são 2 unidades")

            End If

        ElseIf gpos6 = 0 And cbxGraficos.Text = "Sobras" Then

            If sobras1 = "" Then
                GraficoSobras1()
                Chart7.Location = New Point(posG6)
                btnRstGf7.Location = New Point(posBR6)
                btnRstGf7.Visible = True
                gpos6 = 2
            ElseIf sobras2 = "" Then
                GraficoSobras2()
                Chart8.Location = New Point(posG6)
                btnRstGf8.Location = New Point(posBR6)
                btnRstGf8.Visible = True
                gpos6 = 2
                'ElseIf sobras3 = "" Then
                '    GraficoSobras3()
                '    Chart9.Location = New Point(posG6)
                '    'btnrstgf9.Location = New Point(posBR6)
                '    'btnrstgf9.Visible = True
                '    gpos6 = 2
            Else
                MsgBox("O limite de gráficos para Sobras são 2 unidades")

            End If

        ElseIf gpos7 = 0 And cbxGraficos.Text = "Sobras" Then

            If sobras1 = "" Then
                GraficoSobras1()
                Chart7.Location = New Point(posG7)
                btnRstGf7.Location = New Point(posBR7)
                btnRstGf7.Visible = True
                gpos7 = 2
            ElseIf sobras2 = "" Then
                GraficoSobras2()
                Chart8.Location = New Point(posG7)
                btnRstGf8.Location = New Point(posBR7)
                btnRstGf8.Visible = True
                gpos7 = 2
                'ElseIf sobras3 = "" Then
                '    GraficoSobras3()
                '    Chart9.Location = New Point(posG6)
                '    'btnrstgf9.Location = New Point(posBR6)
                '    'btnrstgf9.Visible = True
                '    gpos6 = 2
            Else
                MsgBox("O limite de gráficos para Sobras são 2 unidades")

            End If


        ElseIf gpos8 = 0 And cbxGraficos.Text = "Sobras" Then

            If sobras1 = "" Then
                GraficoSobras1()
                Chart7.Location = New Point(posG8)
                btnRstGf7.Location = New Point(posBR8)
                btnRstGf7.Visible = True
                gpos8 = 2
            ElseIf sobras2 = "" Then
                GraficoSobras2()
                Chart8.Location = New Point(posG8)
                btnRstGf8.Location = New Point(posBR8)
                btnRstGf8.Visible = True
                gpos8 = 2
                'ElseIf sobras3 = "" Then
                '    GraficoSobras3()
                '    Chart9.Location = New Point(posG6)
                '    'btnrstgf9.Location = New Point(posBR6)
                '    'btnrstgf9.Visible = True
                '    gpos6 = 2
            Else
                MsgBox("O limite de gráficos para Sobras são 2 unidades")

            End If

            'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX   TRATO      XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX


        ElseIf gpos1 = 0 And cbxGraficos.Text = "Trato" Then
            If trato1 = "" Then
                GraficoTrato1()
                Chart5.Location = New Point(posG1)
                btnRstGf5.Location = New Point(posBR1)
                btnRstGf5.Visible = True
                gpos1 = 2
            ElseIf trato2 = "" Then
                GraficoTrato2()
                Chart6.Location = New Point(posG1)
                btnRstGf6.Location = New Point(posBR1)
                btnRstGf6.Visible = True
                gpos1 = 2
                'ElseIf trato3 = "" Then
                '    GraficoTrato3()
                '    Chart12.Location = New Point(posG1)
                '    'btnrstgf9.Location = New Point(posBR1)
                '    'btnrstgf9.Visible = True
                '    gpos1 = 2
            Else
                MsgBox("O limite de gráficos para Trato são 2 unidades")

            End If

        ElseIf gpos2 = 0 And cbxGraficos.Text = "Trato" Then
            If trato1 = "" Then
                GraficoTrato1()
                Chart5.Location = New Point(posG2)
                btnRstGf5.Location = New Point(posBR2)
                btnRstGf5.Visible = True
                gpos2 = 2
            ElseIf trato2 = "" Then
                GraficoTrato2()
                Chart6.Location = New Point(posG2)
                btnRstGf6.Location = New Point(posBR2)
                btnRstGf6.Visible = True
                gpos2 = 2
                'ElseIf trato3 = "" Then
                '    GraficoTrato3()
                '    Chart12.Location = New Point(posG2)
                '    'btnrstgf9.Location = New Point(posBR2)
                '    'btnrstgf9.Visible = True
                '    gpos2 = 2
            Else
                MsgBox("O limite de gráficos para tratos são 2 unidades")

            End If

        ElseIf gpos3 = 0 And cbxGraficos.Text = "Trato" Then
            If trato1 = "" Then
                GraficoTrato1()
                Chart5.Location = New Point(posG3)
                btnRstGf5.Location = New Point(posBR3)
                btnRstGf5.Visible = True
                gpos3 = 2
            ElseIf trato2 = "" Then
                GraficoTrato2()
                Chart6.Location = New Point(posG3)
                btnRstGf6.Location = New Point(posBR3)
                btnRstGf6.Visible = True
                gpos3 = 2
                'ElseIf trato3 = "" Then
                '    GraficoTrato3()
                '    Chart12.Location = New Point(posG3)
                '    'btnrstgf9.Location = New Point(posBR3)
                '    'btnrstgf9.Visible = True
                '    gpos3 = 2
            Else
                MsgBox("O limite de gráficos para Tratos são 2 unidades")

            End If

        ElseIf gpos4 = 0 And cbxGraficos.Text = "Trato" Then
            If trato1 = "" Then
                GraficoTrato1()
                Chart5.Location = New Point(posG4)
                btnRstGf5.Location = New Point(posBR4)
                btnRstGf5.Visible = True
                gpos4 = 2
            ElseIf trato2 = "" Then
                GraficoTrato2()
                Chart6.Location = New Point(posG4)
                btnRstGf6.Location = New Point(posBR4)
                btnRstGf6.Visible = True
                gpos4 = 2
                'ElseIf trato3 = "" Then
                '    GraficoTrato3()
                '    Chart12.Location = New Point(posG4)
                '    'btnrstgf9.Location = New Point(posBR4)
                '    'btnrstgf9.Visible = True
                '    gpos4 = 2
            Else
                MsgBox("O limite de gráficos para Tratos são 2 unidades")

            End If
        ElseIf gpos5 = 0 And cbxGraficos.Text = "Trato" Then
            If trato1 = "" Then
                GraficoTrato1()
                Chart5.Location = New Point(posG5)
                btnRstGf5.Location = New Point(posBR5)
                btnRstGf5.Visible = True
                gpos5 = 2
            ElseIf trato2 = "" Then
                GraficoTrato2()
                Chart6.Location = New Point(posG5)
                btnRstGf6.Location = New Point(posBR5)
                btnRstGf6.Visible = True
                gpos5 = 2
                'ElseIf trato3 = "" Then
                '    GraficoTrato3()
                '    Chart12.Location = New Point(posG5)
                '    'btnrstgf9.Location = New Point(posBR5)
                '    'btnrstgf9.Visible = True
                '    gpos5 = 2
            Else
                MsgBox("O limite de gráficos para Tratos são 2 unidades")

            End If

        ElseIf gpos6 = 0 And cbxGraficos.Text = "Trato" Then

            If trato1 = "" Then
                GraficoTrato1()
                Chart5.Location = New Point(posG6)
                btnRstGf5.Location = New Point(posBR6)
                btnRstGf5.Visible = True
                gpos6 = 2
            ElseIf trato2 = "" Then
                GraficoTrato2()
                Chart6.Location = New Point(posG6)
                btnRstGf6.Location = New Point(posBR6)
                btnRstGf6.Visible = True
                gpos6 = 2
                'ElseIf trato3 = "" Then
                '    GraficoTrato3()
                '    Chart12.Location = New Point(posG6)
                '    'btnrstgf9.Location = New Point(posBR6)
                '    'btnrstgf9.Visible = True
                '    gpos6 = 2
            Else
                MsgBox("O limite de gráficos para Tratos são 2 unidades")

            End If

        ElseIf gpos7 = 0 And cbxGraficos.Text = "Trato" Then

            If trato1 = "" Then
                GraficoTrato1()
                Chart5.Location = New Point(posG7)
                btnRstGf5.Location = New Point(posBR7)
                btnRstGf5.Visible = True
                gpos7 = 2
            ElseIf trato2 = "" Then
                GraficoTrato2()
                Chart6.Location = New Point(posG7)
                btnRstGf6.Location = New Point(posBR7)
                btnRstGf6.Visible = True
                gpos7 = 2
                'ElseIf trato3 = "" Then
                '    GraficoTrato3()
                '    Chart12.Location = New Point(posG6)
                '    'btnrstgf9.Location = New Point(posBR6)
                '    'btnrstgf9.Visible = True
                '    gpos6 = 2
            Else
                MsgBox("O limite de gráficos para Tratos são 2 unidades")

            End If

        ElseIf gpos8 = 0 And cbxGraficos.Text = "Trato" Then

            If trato1 = "" Then
                GraficoTrato1()
                Chart5.Location = New Point(posG8)
                btnRstGf5.Location = New Point(posBR8)
                btnRstGf5.Visible = True
                gpos8 = 2
            ElseIf trato2 = "" Then
                GraficoTrato2()
                Chart6.Location = New Point(posG8)
                btnRstGf6.Location = New Point(posBR8)
                btnRstGf6.Visible = True
                gpos8 = 2
                'ElseIf trato3 = "" Then
                '    GraficoTrato3()
                '    Chart12.Location = New Point(posG6)
                '    'btnrstgf9.Location = New Point(posBR6)
                '    'btnrstgf9.Visible = True
                '    gpos6 = 2
            Else
                MsgBox("O limite de gráficos para Tratos são 2 unidades")

            End If

        End If

    End Sub

    Private Sub btnRstGf1_Click(sender As Object, e As EventArgs) Handles btnRstGf1.Click
        Chart1.Series.Clear()
        Chart1.Titles.Clear()

        With Me.btnRstGf1

            If .Location = posBR1 Then
                gpos1 = 0
            ElseIf .Location = posBR2 Then
                gpos2 = 0
            ElseIf .Location = posBR3 Then
                gpos3 = 0
            ElseIf .Location = posBR4 Then
                gpos4 = 0
            ElseIf .Location = posBR5 Then
                gpos5 = 0
            ElseIf .Location = posBR6 Then
                gpos6 = 0
            ElseIf .Location = posBR7 Then
                gpos7 = 0
            ElseIf .Location = posBR8 Then
                gpos8 = 0

            End If
        End With

        btnRstGf1.Visible = False
        forragem1 = ""
        Chart1.Visible = False
        Chart1.Location = New Point(posG9)
        btnRstGf1.Location = New Point(posBR9)
    End Sub

    Private Sub btnRstGf2_Click(sender As Object, e As EventArgs) Handles btnRstGf2.Click
        Chart2.Series.Clear()
        Chart2.Titles.Clear()
        With Me.btnRstGf2

            If .Location = posBR1 Then
                gpos1 = 0
            ElseIf .Location = posBR2 Then
                gpos2 = 0
            ElseIf .Location = posBR3 Then
                gpos3 = 0
            ElseIf .Location = posBR4 Then
                gpos4 = 0
            ElseIf .Location = posBR5 Then
                gpos5 = 0
            ElseIf .Location = posBR6 Then
                gpos6 = 0
            ElseIf .Location = posBR7 Then
                gpos7 = 0
            ElseIf .Location = posBR8 Then
                gpos8 = 0

            End If
        End With

        btnRstGf2.Visible = False
        forragem2 = ""
        Chart2.Visible = False
        Chart2.Location = New Point(posG9)
        btnRstGf2.Location = New Point(posBR9)
    End Sub

    Private Sub btnRstGf3_Click(sender As Object, e As EventArgs) Handles btnRstGf3.Click
        Chart3.Series.Clear()
        Chart3.Titles.Clear()
        With Me.btnRstGf3

            If .Location = posBR1 Then
                gpos1 = 0
            ElseIf .Location = posBR2 Then
                gpos2 = 0
            ElseIf .Location = posBR3 Then
                gpos3 = 0
            ElseIf .Location = posBR4 Then
                gpos4 = 0
            ElseIf .Location = posBR5 Then
                gpos5 = 0
            ElseIf .Location = posBR6 Then
                gpos6 = 0
            ElseIf .Location = posBR7 Then
                gpos7 = 0
            ElseIf .Location = posBR8 Then
                gpos8 = 0

            End If
        End With
        btnRstGf3.Visible = False
        kps1 = ""
        Chart3.Visible = False
        Chart3.Location = New Point(posG9)
        btnRstGf3.Location = New Point(posBR9)
    End Sub

    Private Sub btnRstGf4_Click(sender As Object, e As EventArgs) Handles btnRstGf4.Click
        Chart4.Series.Clear()
        Chart4.Titles.Clear()
        With Me.btnRstGf4

            If .Location = posBR1 Then
                gpos1 = 0
            ElseIf .Location = posBR2 Then
                gpos2 = 0
            ElseIf .Location = posBR3 Then
                gpos3 = 0
            ElseIf .Location = posBR4 Then
                gpos4 = 0
            ElseIf .Location = posBR5 Then
                gpos5 = 0
            ElseIf .Location = posBR6 Then
                gpos6 = 0
            ElseIf .Location = posBR7 Then
                gpos7 = 0
            ElseIf .Location = posBR8 Then
                gpos8 = 0

            End If
        End With
        btnRstGf4.Visible = False
        kps2 = ""
        Chart4.Visible = False
        Chart4.Location = New Point(posG9)
        btnRstGf4.Location = New Point(posBR9)
    End Sub

    Private Sub btnRstGf5_Click(sender As Object, e As EventArgs) Handles btnRstGf5.Click
        Chart5.Series.Clear()
        Chart5.Titles.Clear()
        With Me.btnRstGf5

            If .Location = posBR1 Then
                gpos1 = 0
            ElseIf .Location = posBR2 Then
                gpos2 = 0
            ElseIf .Location = posBR3 Then
                gpos3 = 0
            ElseIf .Location = posBR4 Then
                gpos4 = 0
            ElseIf .Location = posBR5 Then
                gpos5 = 0
            ElseIf .Location = posBR6 Then
                gpos6 = 0
            ElseIf .Location = posBR7 Then
                gpos7 = 0
            ElseIf .Location = posBR8 Then
                gpos8 = 0

            End If
        End With
        btnRstGf5.Visible = False
        trato1 = ""
        Chart5.Visible = False
        Chart5.Location = New Point(posG9)
        btnRstGf5.Location = New Point(posBR9)
    End Sub

    Private Sub btnRstGf6_Click(sender As Object, e As EventArgs) Handles btnRstGf6.Click
        Chart6.Series.Clear()
        Chart6.Titles.Clear()
        With Me.btnRstGf6

            If .Location = posBR1 Then
                gpos1 = 0
            ElseIf .Location = posBR2 Then
                gpos2 = 0
            ElseIf .Location = posBR3 Then
                gpos3 = 0
            ElseIf .Location = posBR4 Then
                gpos4 = 0
            ElseIf .Location = posBR5 Then
                gpos5 = 0
            ElseIf .Location = posBR6 Then
                gpos6 = 0
            ElseIf .Location = posBR7 Then
                gpos7 = 0
            ElseIf .Location = posBR8 Then
                gpos8 = 0

            End If
        End With
        btnRstGf6.Visible = False
        trato2 = ""
        Chart6.Visible = False
        Chart6.Location = New Point(posG9)
        btnRstGf6.Location = New Point(posBR9)
    End Sub

    Private Sub btnRstGf7_Click(sender As Object, e As EventArgs) Handles btnRstGf7.Click
        Chart7.Series.Clear()
        Chart7.Titles.Clear()
        With Me.btnRstGf7

            If .Location = posBR1 Then
                gpos1 = 0
            ElseIf .Location = posBR2 Then
                gpos2 = 0
            ElseIf .Location = posBR3 Then
                gpos3 = 0
            ElseIf .Location = posBR4 Then
                gpos4 = 0
            ElseIf .Location = posBR5 Then
                gpos5 = 0
            ElseIf .Location = posBR6 Then
                gpos6 = 0
            ElseIf .Location = posBR7 Then
                gpos7 = 0
            ElseIf .Location = posBR8 Then
                gpos8 = 0

            End If
        End With
        btnRstGf7.Visible = False
        sobras1 = ""
        Chart7.Visible = False
        Chart7.Location = New Point(posG9)
        btnRstGf7.Location = New Point(posBR9)
    End Sub

    Private Sub btnRstGf8_Click(sender As Object, e As EventArgs) Handles btnRstGf8.Click
        Chart8.Series.Clear()
        Chart8.Titles.Clear()
        With Me.btnRstGf8

            If .Location = posBR1 Then
                gpos1 = 0
            ElseIf .Location = posBR2 Then
                gpos2 = 0
            ElseIf .Location = posBR3 Then
                gpos3 = 0
            ElseIf .Location = posBR4 Then
                gpos4 = 0
            ElseIf .Location = posBR5 Then
                gpos5 = 0
            ElseIf .Location = posBR6 Then
                gpos6 = 0
            ElseIf .Location = posBR7 Then
                gpos7 = 0
            ElseIf .Location = posBR8 Then
                gpos8 = 0

            End If
        End With
        btnRstGf8.Visible = False
        sobras2 = ""
        Chart8.Visible = False
        Chart8.Location = New Point(posG9)
        btnRstGf8.Location = New Point(posBR9)
    End Sub

    'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX      PRÉ-MIX       XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX


    Private Sub ConfigLotePremix()
        On Error Resume Next
        With dtgLotesPremix
            .ColumnHeadersDefaultCellStyle.BackColor = Color.Gainsboro
            .ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Regular)
            .DefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Regular)

            .Columns(0).Visible = False
            .Columns(1).Width = 100
            .Columns(2).Visible = False
            .Columns(3).Visible = False
            .Columns(4).Visible = False
            .Columns(5).Visible = False
            .Columns(6).Visible = False
            .Columns(7).Visible = False
            .Columns(8).Visible = False
            .Columns(9).Visible = False
            .Columns(10).Visible = False
            .Columns(11).Visible = False
            .Columns(12).Visible = False
            .Columns(13).Visible = False
            .Columns(14).Visible = False
            .Columns(15).Visible = False
            .Columns(16).Visible = False
            '.Columns(17).Visible = False

        End With
    End Sub

    Private Sub BuscarDietaAgrup() ' Pré-Mix

        Dim da As New SQLiteDataAdapter
        Dim dt As New DataTable
        Dim sql As String
        Try

            abrir()
            sql = "Select Nome, IdPropriedade from Dieta group by Nome, IdPropriedade"
            'sql = "Select Nome, QtdPeneiras from Forragem group by Nome"
            'SUM(Quantidade)
            da = New SQLiteDataAdapter(sql, con)
            dt = New DataTable
            da.Fill(dt)
            dtgHistDietasPM.DataSource = dt

            fechar()

        Catch ex As Exception

        End Try
        With Me.dtgHistDietasPM

            .ColumnHeadersDefaultCellStyle.BackColor = Color.Gainsboro
            .ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Regular)
            .DefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Regular)

            .Columns(0).Width = 200
            .Columns(1).Visible = False

        End With
        'dtgClientes.DataSource = dtgFazendas.DataSource
    End Sub

    Private Sub BuscarPremix()

        Dim da As New SQLiteDataAdapter
        Dim dt As New DataTable
        Dim sql As String
        Try

            abrir()
            sql = "Select Alimento,QtdPremix from Dieta where Nome = " & "'" & Me.lblNomeDieta.Text & "'"
            'sql = "Select Nome, QtdPeneiras from Forragem group by Nome"
            'SUM(Quantidade)
            da = New SQLiteDataAdapter(sql, con)
            dt = New DataTable
            da.Fill(dt)
            dtgPremix.DataSource = dt

            fechar()

        Catch ex As Exception

        End Try

        'SomarAmostrasSobras()

        'dtgClientes.DataSource = dtgFazendas.DataSource

        With Me.dtgPremix

            .ColumnHeadersDefaultCellStyle.BackColor = Color.Gainsboro
            .ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Regular)
            .DefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Regular)

            .Columns(0).Width = 300
            .Columns(1).Width = 80
            dt.Columns.Add("Total Kg")
            dt.Columns.Add("%")
        End With

    End Sub

    Private Sub calcularPremix()
        Dim soma As Double
        'Dim somaPreMist As Double
        Dim pctProdt As Double
        '        Dim x As Integer
        '        x = dtgTemp.Rows.Count
        On Error Resume Next
        For i As Integer = 0 To dtgPremix.Rows.Count - 1
            With dtgPremix
                '.Rows(i).Cells(62).Value = .Rows(i).Cells(60).Value - .Rows(i).Cells(61).Value
                '.Rows(i).Cells(65).Value = .Rows(i).Cells(60).Value * lblNAnimais.Text
                'dtgAlimentosDieta.Rows(i).Cells(95).Value = dtgAlimentosDieta.Rows(i).Cells(94).Value / lblTotalProd.Text * 100
                ' + .Rows(i).Cells(2).Value
                .Rows(i).Cells(2).Value = .Rows(i).Cells(1).Value * txtNAnimPremix.Text
                soma += .Rows(i).Cells(2).Value
                lblTotalProd.Text = soma
            End With
        Next

        Dim linha As DataGridViewRow

        For Each linha In dtgPremix.Rows
            pctProdt = linha.Cells(2).Value / lblTotalProd.Text
            linha.Cells(3).Value = Format(pctProdt.ToString("P"))

        Next
        If lblTotalProd.Text > txtMisturador.Text Then
            txtMisturador.BackColor = Color.Red
        Else
            txtMisturador.BackColor = Color.DarkGreen
        End If

    End Sub

    Private Sub dtgHistDietasPM_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dtgHistDietasPM.CellClick
        lblNomeDieta.Text = dtgHistDietasPM.CurrentRow.Cells(0).Value
        lblNomeD.Text = dtgHistDietasPM.CurrentRow.Cells(0).Value
        BuscarPremix()
        BuscarDietaVagao()
        calcularPremix()
        OcultarLinhaPm()
        OcultarLinhaVg()
    End Sub

    Private Sub dtgPremix_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dtgPremix.CellEndEdit
        calcularPremix()
    End Sub

    Private Sub dtgLotesPremix_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dtgLotesPremix.CellContentClick
        txtNAnimPremix.Text = dtgLotesPremix.CurrentRow.Cells(3).Value
        txtNAnimais.Text = dtgLotesPremix.CurrentRow.Cells(3).Value
        lbllotePM.Text = dtgLotesPremix.CurrentRow.Cells(1).Value
        LblLoteVagao.Text = dtgLotesPremix.CurrentRow.Cells(1).Value
        calcularPremix()
        OcultarLinhaVg()
    End Sub

    Private Sub OcultarLinhaPm()

        Dim linha As DataGridViewRow

        Try
            For Each linha In dtgPremix.Rows
                If linha.Cells(2).Value > 0 Then ' if .linha(46)<> 0
                    linha.Visible = True
                Else
                    linha.Visible = False
                End If

            Next

        Catch ex As Exception

        End Try

    End Sub

    'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX         VAGÃO          XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

    Private Sub ConfigLotesVagao()
        On Error Resume Next
        With dtgHistLotes
            .ColumnHeadersDefaultCellStyle.BackColor = Color.Gainsboro
            .ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Regular)
            .DefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Regular)

            .Columns(0).Visible = False
            .Columns(1).Width = 150
            .Columns(2).Width = 70
            .Columns(3).Width = 70
            .Columns(4).Visible = False
            .Columns(5).Visible = False
            .Columns(6).Visible = False
            .Columns(7).Visible = False
            .Columns(8).Visible = False
            .Columns(9).Visible = False
            .Columns(10).Visible = False
            .Columns(11).Visible = False
            .Columns(12).Visible = False
            .Columns(13).Visible = False
            .Columns(14).Visible = False
            .Columns(15).Visible = False
            .Columns(16).Visible = False
            '.Columns(17).Visible = False

        End With

    End Sub
    Private Sub BuscarDietaVagao()

        Dim da As New SQLiteDataAdapter
        Dim dt As New DataTable
        Dim sql As String
        Try

            abrir()
            sql = "Select Alimento,Qtd,QtdPremix,QtdVagao from Dieta where Nome = " & "'" & Me.lblNomeDieta.Text & "'"
            'sql = "Select Nome, QtdPeneiras from Forragem group by Nome"
            'SUM(Quantidade)
            da = New SQLiteDataAdapter(sql, con)
            dt = New DataTable
            da.Fill(dt)
            dtgDietaVg.DataSource = dt

            fechar()

        Catch ex As Exception

        End Try

        'SomarAmostrasSobras()

        'dtgClientes.DataSource = dtgFazendas.DataSource

        With Me.dtgDietaVg

            .ColumnHeadersDefaultCellStyle.BackColor = Color.Gainsboro
            .ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Regular)
            .DefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Regular)

            .Columns(0).Width = 300
            .Columns(1).Width = 70
            .Columns(2).Visible = False
            dt.Columns.Add("Total/trato")
        End With

        'aparecer somente se o 3 for maior q 0

    End Sub

    Private Sub CalcularQtdVagao()
        Dim qtAlimento As Double
        Dim linha As DataGridViewRow

        Try
            For Each linha In dtgDietaVg.Rows
                linha.Cells(4).Value = linha.Cells(3).Value * txtNAnimais.Text / txtNTratos.Text
                qtAlimento += linha.Cells(4).Value
                lblQtAlim.Text = qtAlimento
                lblDifVagao.Text = txtPesoVagao.Text - qtAlimento
            Next
            If lblDifVagao.Text < 0 Then
                txtPesoVagao.BackColor = Color.Red
            Else
                txtPesoVagao.BackColor = Color.DarkGreen
            End If
        Catch ex As Exception

        End Try

    End Sub

    Private Sub OcultarLinhaVg()

        Dim linha As DataGridViewRow

        Try
            For Each linha In dtgDietaVg.Rows
                If linha.Cells(3).Value > 0 Then ' if .linha(46)<> 0
                    linha.Visible = True
                ElseIf linha.Cells(3).Value <= 0 Then
                    linha.Visible = False
                End If
            Next
        Catch ex As Exception
        End Try

    End Sub

    Private Sub dtgHistLotes_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dtgHistLotes.CellClick
        txtNAnimais.Text = dtgHistLotes.CurrentRow.Cells(3).Value

        ' txtNTratos.Text = dtgHistLotes.CurrentRow.Cells(13).Value
    End Sub

    Private Sub txtNAnimais_TextChanged(sender As Object, e As EventArgs) Handles txtNAnimais.TextChanged
        CalcularQtdVagao()
    End Sub

    Private Sub txtNTratos_MouseClick(sender As Object, e As MouseEventArgs) Handles txtNTratos.MouseClick
        Me.txtNTratos.SelectAll()
    End Sub

    Private Sub txtNTratos_TextChanged(sender As Object, e As EventArgs) Handles txtNTratos.TextChanged
        CalcularQtdVagao()
    End Sub

    Private Sub TabControl3_SelectedIndexChanged(sender As Object, e As EventArgs) _
         Handles TabControl3.SelectedIndexChanged

        If TabControl3.SelectedTab Is tbVagao Then
            OcultarLinhaVg()
            CalcularQtdVagao()
            ConfigLotesVagao()
        End If

    End Sub

    'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX   AMOSTRAS DE FEZES      XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

    Private Sub CadastrarFezes()

        Dim sql As String
        Dim cmd As SQLiteCommand

        data = Today.ToString("dd-MM-yyyy")
        If txtG1.Text <> "" Or txtG2.Text <> "" Or txtG3.Text <> "" Then

            Try

                abrir()

                sql = "Insert into Fezes (NomeData,Grau01,Grau02,Grau03,Grau04,Grau05,Lote,IdPropriedade) values (@NomeData,@Grau01,@Grau02,@Grau03,@Grau04,@Grau05,@Lote,@IdPropriedade)"
                cmd = New SQLiteCommand(sql, con)
                'cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@NomeData", data)
                cmd.Parameters.AddWithValue("@Grau01", txtG1.Text)
                cmd.Parameters.AddWithValue("@Grau02", txtG2.Text)
                cmd.Parameters.AddWithValue("@Grau03", txtG3.Text)
                cmd.Parameters.AddWithValue("@Grau04", txtG4.Text)
                cmd.Parameters.AddWithValue("@Grau05", txtG5.Text)
                cmd.Parameters.AddWithValue("@Lote", "Lote")
                cmd.Parameters.AddWithValue("@IdPropriedade", idFaz)

                cmd.ExecuteNonQuery()
                MsgBox("Amostras cadastradas com sucesso!")
            Catch ex As Exception
                MsgBox("Erro ao salvar!" + ex.Message)
                fechar()
            End Try

            'LimparCamposFazenda()
            'btnCadastrarFazenda.Enabled = False
            'btnSalvarFazenda.Enabled = False
            'btnEditarCliente.Enabled = False
            'btnExcluirCliente.Enabled = False
            BuscarFezesAgrup()
        Else
            MsgBox("Preencha os campos!")

        End If
    End Sub

    Private Sub BuscarFezesAgrup()

        Dim da As New SQLiteDataAdapter
        Dim dt As New DataTable
        Dim sql As String
        Try

            abrir()
            'sql = "NomeData,Grau01,Grau02,Grau03,Grau04,Grau05,Lote,IdPropriedade from Fezes where NomeData = " & "'" & Me.lblNomeFezes.Text & "'"
            sql = "Select * from Fezes"
            'SUM(Quantidade)
            da = New SQLiteDataAdapter(sql, con)
            dt = New DataTable
            da.Fill(dt)
            dtgHistFezes.DataSource = dt

            fechar()

        Catch ex As Exception

        End Try

    End Sub

    Private Sub DeletarFezes()

        Dim cmd As SQLiteCommand
        Dim sqlDelete As String = "Delete from Fezes where NomeData = " & "'" & Me.lblNomeFezes.Text & "'"
        'Mensagem se realmente quer excluir
        If MsgBox("Excluir amostras?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            Try
                abrir()
                cmd = New SQLiteCommand(sqlDelete, con)
                'cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@NomeData", lblNomeFezes.Text)
                cmd.ExecuteNonQuery()
                MsgBox("Amostras excluidas com sucesso!")
            Catch ex As Exception
                MsgBox("Erro ao exluir amostras!" + ex.Message)
                fechar()
            End Try
            'txtBuscarCliente.Text = ""
            'btnEditarCliente.Enabled = False
            'btnExcluirCliente.Enabled = False
            'BuscarFazenda()
            'lblFaz.Text = ""
            'lblCliente.Text = ""
            'lblFone.Text = ""
            'txtCidadeEstado.Text = ""
            'lnkLocal.Text = ""

        Else
            MsgBox("Você precisa escolher uma amostra na tabela!")
        End If

    End Sub

    Private Sub CadastrarFezes_Click(sender As Object, e As EventArgs) Handles btnCadastrarFezes.Click
        CadastrarFezes()

    End Sub

    Private Sub dtgHistFezes_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dtgHistFezes.CellContentClick
        lblNomeFezes.Text = dtgHistFezes.CurrentRow.Cells(0).Value
        txtG1.Text = dtgHistFezes.CurrentRow.Cells(1).Value
        txtG2.Text = dtgHistFezes.CurrentRow.Cells(2).Value
        txtG3.Text = dtgHistFezes.CurrentRow.Cells(3).Value
        txtG4.Text = dtgHistFezes.CurrentRow.Cells(4).Value
        txtG5.Text = dtgHistFezes.CurrentRow.Cells(5).Value
    End Sub

    Private Sub btnExcluirFezes_Click(sender As Object, e As EventArgs) Handles btnExcluirFezes.Click
        DeletarFezes()
    End Sub


    'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX      AMOSTRAS URINA      XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

    Private Sub ConfigAmostrasUrina()
        On Error Resume Next

        With Me.dtgUrina

            .ColumnHeadersDefaultCellStyle.BackColor = Color.Gainsboro
            .ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Regular)
            .DefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Regular)

            .Columns(0).HeaderText = "01"
            .Columns(1).HeaderText = "02"
            .Columns(2).HeaderText = "03"
            .Columns(3).HeaderText = "04"
            .Columns(4).HeaderText = "05"
            .Columns(5).HeaderText = "06"
            .Columns(6).HeaderText = "07"
            .Columns(7).HeaderText = "08"
            .Columns(8).HeaderText = "09"
            .Columns(9).HeaderText = "10"
            .Columns(10).HeaderText = "Média"
            .Columns(11).HeaderText = "CV%"


            .Columns(0).Width = 45
            .Columns(1).Width = 45
            .Columns(2).Width = 45
            .Columns(3).Width = 45
            .Columns(4).Width = 45
            .Columns(5).Width = 45
            .Columns(6).Width = 45
            .Columns(7).Width = 45
            .Columns(8).Width = 45
            .Columns(9).Width = 45
            .Columns(10).Width = 70
            .Columns(11).Width = 70


        End With

        With Me.dtgHistUrina

            .ColumnHeadersDefaultCellStyle.BackColor = Color.Gainsboro
            .ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Regular)
            .DefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Regular)

            .Columns(0).HeaderText = "Data"
            '.Columns(1).HeaderText = "02"
            '.Columns(2).HeaderText = "03"
            '.Columns(3).HeaderText = "04"
            '.Columns(4).HeaderText = "05"
            '.Columns(5).HeaderText = "06"
            '.Columns(6).HeaderText = "07"
            '.Columns(7).HeaderText = "08"
            '.Columns(8).HeaderText = "09"
            '.Columns(9).HeaderText = "10"
            '.Columns(10).HeaderText = "Média"
            '.Columns(11).HeaderText = "CV%"


            .Columns(0).Width = 140
            .Columns(1).Visible = False
            .Columns(2).Visible = False
            .Columns(3).Visible = False
            .Columns(4).Visible = False
            .Columns(5).Visible = False
            .Columns(6).Visible = False
            .Columns(7).Visible = False
            .Columns(8).Visible = False
            .Columns(9).Visible = False
            .Columns(10).Visible = False
            .Columns(11).Visible = False


        End With

    End Sub

    Private Sub TabelaAmostrasUrina()

        Dim dt As New DataTable()

        dt.Columns.Add("01")
        dt.Columns.Add("02")
        dt.Columns.Add("03")
        dt.Columns.Add("04")
        dt.Columns.Add("05")
        dt.Columns.Add("06")
        dt.Columns.Add("07")
        dt.Columns.Add("08")
        dt.Columns.Add("09")
        dt.Columns.Add("10")
        dt.Columns.Add("Média")
        dt.Columns.Add("CV%")

        dt.Rows.Add("0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "", "")


        dtgUrina.DataSource = dt
        ConfigAmostrasUrina()
    End Sub

    'Private Sub SomarAmostrasPH()


    '    '        Dim texto As String = Nothing
    '    '        txtSeq.Text = Seq
    '    'If txtSeq.Text <> [Ã”][Ã”] Then [Ã´]String.Empty Then
    '    '        '[Ã´]percorre cada linha do DataGridView
    '    '        For Each linha As DataGridViewRow In dgvAuto.Rows
    '    '            '[Ã´]percorre cada cÃ©lula da linha
    '    '            For Each celula As DataGridViewCell In dgvAuto.Rows(linha.Index).Cells
    '    '                '[Ã´]se a coluna for a coluna 1 (Nome) então verifica o criterio
    '    '                If celula.ColumnIndex = 0 Then
    '    '                    texto = celula.Value.ToString.ToLower
    '    '['Ã´]se o texto informado estiver contido na cÃ©lula então seleciona toda linha
    '    '                    If texto.Contains(txtSeq.Text.ToLower) Then
    '    '                        '[Ã´]seleciona a linha
    '    '                        Me.dgvAuto.Rows(celula.RowIndex).Selected = True
    '    '                        Exit Sub
    '    '                    End If
    '    '                End If
    '    '            Next
    '    '        Next
    '    'End If


    '    'XXXXXXXXXXXXXXXXXXXX FUNCIONANDOXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
    '    Dim valor As Double
    '    Dim media As Integer
    '    '    valor = valor + Double.Parse(col.Cells(0).Value)

    '    'Next
    '    For Each row In dtgUrina.Rows
    '        media = 0
    '        If dtgUrina.CurrentRow.Cells(0).Value > 0 Then
    '            media += 1
    '            valor += dtgUrina.CurrentRow.Cells(0).Value
    '            If dtgUrina.CurrentRow.Cells(1).Value > 0 Then
    '                media += 1
    '                valor += dtgUrina.CurrentRow.Cells(1).Value
    '                If dtgUrina.CurrentRow.Cells(2).Value > 0 Then
    '                    media += 1
    '                    valor += dtgUrina.CurrentRow.Cells(2).Value
    '                    If dtgUrina.CurrentRow.Cells(3).Value > 0 Then
    '                        media += 1
    '                        valor += dtgUrina.CurrentRow.Cells(3).Value
    '                        If dtgUrina.CurrentRow.Cells(4).Value > 0 Then
    '                            media += 1
    '                            valor += dtgUrina.CurrentRow.Cells(4).Value
    '                            If dtgUrina.CurrentRow.Cells(5).Value > 0 Then
    '                                media += 1
    '                                valor += dtgUrina.CurrentRow.Cells(5).Value
    '                                If dtgUrina.CurrentRow.Cells(6).Value > 0 Then
    '                                    media += 1
    '                                    valor += dtgUrina.CurrentRow.Cells(6).Value
    '                                    If dtgUrina.CurrentRow.Cells(7).Value > 0 Then
    '                                        media += 1
    '                                        valor += dtgUrina.CurrentRow.Cells(7).Value
    '                                        If dtgUrina.CurrentRow.Cells(8).Value > 0 Then
    '                                            media += 1
    '                                            valor += dtgUrina.CurrentRow.Cells(8).Value
    '                                            If dtgUrina.CurrentRow.Cells(9).Value > 0 Then
    '                                                media += 1
    '                                                valor += dtgUrina.CurrentRow.Cells(9).Value
    '                                            End If
    '                                        End If
    '                                    End If
    '                                End If
    '                            End If
    '                        End If
    '                    End If
    '                End If
    '            End If
    '        End If



    '    Next
    '    lbl1.Text = media
    '    dtgUrina.CurrentRow.Cells(10).Value = valor / media

    'End Sub


    Private Sub CadastrarAmostrasUrina()

        Dim sql As String
        Dim cmd As SQLiteCommand

        data = Now.ToString("dd-MM-yyyy")

        For Each row As DataGridViewRow In dtgUrina.Rows
            Try
                abrir()
                sql = "Insert into Urina (NomeData,PH01,PH02,PH03,PH04,PH05,PH06,PH07,PH08,PH09,PH10,Media,CV,Lote,IdPropriedade) values (@NomeData,@PH01,PH02,@PH03,@PH04,@PH05,@PH06,@PH07,@PH08,@PH09,@PH10,@Media,@CV,@Lote,@IdPropriedade)"
                cmd = New SQLiteCommand(sql, con)
                'cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@NomeData", data)
                cmd.Parameters.AddWithValue("@PH01", row.Cells("01").Value.ToString)
                cmd.Parameters.AddWithValue("@PH02", row.Cells("02").Value.ToString)
                cmd.Parameters.AddWithValue("@PH03", row.Cells("03").Value.ToString)
                cmd.Parameters.AddWithValue("@PH04", row.Cells("04").Value.ToString)
                cmd.Parameters.AddWithValue("@PH05", row.Cells("05").Value.ToString)
                cmd.Parameters.AddWithValue("@PH06", row.Cells("06").Value.ToString)
                cmd.Parameters.AddWithValue("@PH07", row.Cells("07").Value.ToString)
                cmd.Parameters.AddWithValue("@PH08", row.Cells("08").Value.ToString)
                cmd.Parameters.AddWithValue("@PH09", row.Cells("09").Value.ToString)
                cmd.Parameters.AddWithValue("@PH10", row.Cells("10").Value.ToString)
                cmd.Parameters.AddWithValue("@Media", row.Cells("Média").Value.ToString)
                cmd.Parameters.AddWithValue("@CV", row.Cells("CV%").Value.ToString)
                cmd.Parameters.AddWithValue("@Lote", "Lote")
                cmd.Parameters.AddWithValue("@IdPropriedade", idFaz)

                cmd.ExecuteNonQuery()
                'ListarComissoes()

            Catch ex As Exception
                'MsgBox("Erro ao salvar!" + ex.Message)
                fechar()
            End Try

        Next
        MsgBox("Amostra cadastrada com sucesso!")

        'btnEditarCliente.Enabled = False

        ' dtgForragem.Rows.Clear()
        'txtNomeSobra.Text = ""
        'cbxQtdPeneirasSobras.Text = ""


        'Else
        'MsgBox("Preencha os campos corretamente!")

        'End If
    End Sub

    Private Sub BuscarAmostrasUrinaAgrup()

        Dim da As New SQLiteDataAdapter
        Dim dt As New DataTable
        Dim sql As String
        Try

            abrir()
            sql = "Select * from Urina"
            'sql = "Select Nome, QtdPeneiras from Forragem group by Nome"
            'SUM(Quantidade)
            da = New SQLiteDataAdapter(sql, con)
            dt = New DataTable
            da.Fill(dt)
            dtgHistUrina.DataSource = dt

            fechar()

        Catch ex As Exception

        End Try

        'dtgClientes.DataSource = dtgFazendas.DataSource
        ConfigAmostrasUrina()
    End Sub

    Private Sub BuscarAmostrasUrina()

        Dim da As New SQLiteDataAdapter
        Dim dt As New DataTable
        Dim sql As String
        Try

            abrir()
            sql = "Select * from Urina"
            'sql = "Select Nome, QtdPeneiras from Forragem group by Nome"
            'SUM(Quantidade)
            da = New SQLiteDataAdapter(sql, con)
            dt = New DataTable
            da.Fill(dt)
            dtgUrina.DataSource = dt

            fechar()

        Catch ex As Exception

        End Try
        ConfigAmostrasUrina()
        'dtgClientes.DataSource = dtgFazendas.DataSource
    End Sub

    Private Sub btnSalvarUrina_Click(sender As Object, e As EventArgs) Handles btnSalvarUrina.Click
        CadastrarAmostrasUrina()
        BuscarAmostrasUrinaAgrup()
    End Sub


    Private Sub dtgHistUrina_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dtgHistUrina.CellClick
        lblNomeUrina.Text = dtgHistUrina.CurrentRow.Cells(0).Value
        BuscarAmostrasUrina()
    End Sub

    Private Sub btnDietaVoltar_Click(sender As Object, e As EventArgs) Handles btnDietaVoltar.Click
        Me.Hide()
    End Sub

    Private Sub dtgForragem_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs)
        CalcularParticulas()
    End Sub

    Private Sub dtgAmostras_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dtgAmostras.CellEndEdit
        MediaTratos()
    End Sub

    'Private Sub dtgSobras_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dtgSobras.CellClick
    '    SomarAmostrasSobras()
    '    CalcularParticulasSobras()
    'End Sub

    Private Sub dtgSobras_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dtgSobras.CellEndEdit
        SomarAmostrasSobras()
        CalcularParticulasSobras()
    End Sub

    Private Sub dtgHistSobras_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dtgHistSobras.CellContentClick

    End Sub

    Private Sub dtgGraficos_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dtgGraficos.CellContentClick

    End Sub

    Private Sub dtgHistTratos_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dtgHistTratos.CellContentClick

    End Sub

    Private Sub dtgSobras_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dtgSobras.CellContentClick

    End Sub

    Private Sub dtgPremix_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dtgPremix.CellContentClick

    End Sub


    Private Sub dtgHistLotes_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dtgHistLotes.CellContentClick

    End Sub

    Private Sub txtMisturador_LostFocus(sender As Object, e As EventArgs) Handles txtMisturador.LostFocus

        My.Settings.MistPremix = txtMisturador.Text
        My.Settings.Save()
    End Sub

    Private Sub txtMisturador_MouseClick(sender As Object, e As MouseEventArgs) Handles txtMisturador.MouseClick
        Me.txtMisturador.SelectAll()
    End Sub


    Private Sub TextBox4_TextChanged(sender As Object, e As EventArgs) Handles txtMisturador.TextChanged

    End Sub

    Private Sub txtPesoVagao_LostFocus(sender As Object, e As EventArgs) Handles txtPesoVagao.LostFocus
        My.Settings.Vagao = txtPesoVagao.Text
        My.Settings.Save()
    End Sub

    Private Sub txtPesoVagao_MouseClick(sender As Object, e As MouseEventArgs) Handles txtPesoVagao.MouseClick
        Me.txtPesoVagao.SelectAll()
    End Sub

    Private Sub txtPesoVagao_TextChanged(sender As Object, e As EventArgs) Handles txtPesoVagao.TextChanged

    End Sub


    Private Sub TabPage11_Click(sender As Object, e As EventArgs) Handles TabPage11.Click

    End Sub
    'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX      GERAR IMPRESSÃO     XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX


    'Gerar a pagina e imprimir
    Private Sub m_PrintDocument_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles m_PrintDocument.PrintPage


        Using caneta As New Pen(Color.Black, 20)

            e.Graphics.DrawRectangle(caneta, e.MarginBounds)

            caneta.DashStyle = Drawing2D.DashStyle.Dash

            caneta.Alignment = Drawing2D.PenAlignment.Outset

            e.Graphics.DrawRectangle(caneta, e.PageBounds)

        End Using


        'Indica que nao ha  mais paginas a serem impressas

        e.HasMorePages = False

    End Sub
    Private Sub GetImpressoras()

        For Each impressora As String In PrinterSettings.InstalledPrinters

            'ListBox1.Items.Add(impressora)
            If impressora = "Microsoft Print to PDF" Then
                lblPDF.Text = (impressora)
            End If

        Next
        If lblPDF.Text <> "Microsoft Print to PDF" Then
            MsgBox("impressora não encotrada")
        End If

    End Sub
    Private Function GetImpressoraPadrao() As String

        Dim impPadrao = New PrinterSettings
        Return impPadrao.PrinterName

    End Function

    Private Sub ListBox1_MouseDoubleClick(sender As Object, e As MouseEventArgs)
        'If SetDefaultPrinter(ListBox1.Text) Then
        'lblpadrao.Text = ListBox1.Text
        'End If
    End Sub

    Private Sub Button16_Click(sender As Object, e As EventArgs) Handles Button16.Click

        GetImpressoraPadrao()
        lblpadrao.Text = GetImpressoraPadrao()
        'Procurar se existe impressora PDF
        GetImpressoras()

        'ImpressoraPDF vira a padrão
        SetDefaultPrinter(lblPDF.Text)
        'Gera a impressão
        Imprimir()

        'Volta a impressora padrão
        SetDefaultPrinter(lblpadrao.Text)
    End Sub

    Private Sub button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        Imprimir()
    End Sub

    Private Sub Imprimir()

        'obtem a string de conexao
        'MyConnection = New sqliteConnection("Provider=Microsoft.Jet.sqlite.4.0;Data Source=c:\teste\Teste.mdb")

        'define o titulo do relatorio
        RelatorioTitulo = "Vagão - "

        'define os objetos printdocument e os eventos associados
        Dim pd As Printing.PrintDocument = New Printing.PrintDocument()

        'Definimos 3 eventos para tratar a impressão : PringPage, BeginPrint e EndPrint.
        AddHandler pd.PrintPage, New Printing.PrintPageEventHandler(AddressOf Me.pdRelatorios_PrintPage)

        AddHandler pd.BeginPrint, New Printing.PrintEventHandler(AddressOf Me.Begin_Print)

        AddHandler pd.EndPrint, New Printing.PrintEventHandler(AddressOf Me.End_Print)

        'define o objeto para visualizar a impressao
        Dim objPrintPreview As New PrintPreviewDialog

        Try

            'define o formulário como maximizado e com Zoom
            With objPrintPreview

                .Document = pd

                .WindowState = FormWindowState.Maximized

                .PrintPreviewControl.Zoom = 1

                .Text = "Vagão"

                .ShowDialog()

            End With

        Catch ex As Exception

            MessageBox.Show(ex.ToString())

        End Try

    End Sub
    'A conexão e o DataReader ‚ aberto aqui
    Private Sub Begin_Print(ByVal sender As Object, ByVal e As Printing.PrintEventArgs)
        Dim dieta As String
        dieta = lblNomeDieta.Text
        Dim Sql As String = "Select Alimento, QtdVagao from Dieta where Nome = " & "'" & dieta & "'"

        Dim MyComand As New SQLiteCommand(Sql, con)

        abrir()

        Leitor = MyComand.ExecuteReader()

        paginaAtual = 1

    End Sub

    'Layout da(s) pagina(s) a imprimir
    Private Sub pdRelatorios_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs)

        'Variaveis das linhas
        Dim LinhasPorPagina As Single = 0

        Dim PosicaoDaLinha As Single = 0

        Dim LinhaAtual As Integer = 0

        'Variaveis das margens
        Dim MargemEsquerda As Single = e.MarginBounds.Left

        Dim MargemSuperior As Single = e.MarginBounds.Top + 100

        Dim MargemDireita As Single = e.MarginBounds.Right

        Dim MargemInferior As Single = e.MarginBounds.Bottom

        Dim CanetaDaImpressora As Pen = New Pen(Color.Black, 1)

        Dim Nome As String

        Dim Qtd As String

        'Variaveis das fontes
        Dim FonteNegrito As Font

        Dim FonteTitulo As Font

        Dim FonteSubTitulo As Font

        Dim FonteRodape As Font

        Dim FonteNormal As Font

        'define efeitos em fontes
        FonteNegrito = New Font("Arial", 9, FontStyle.Bold)

        FonteTitulo = New Font("Arial", 15, FontStyle.Bold)

        FonteSubTitulo = New Font("Arial", 12, FontStyle.Bold)

        FonteRodape = New Font("Arial", 8)

        FonteNormal = New Font("Arial", 9)

        'define valores para linha atual e para linha da impressao
        LinhaAtual = 0

        Dim L As Integer = 0

        'Cabecalho
        e.Graphics.DrawLine(CanetaDaImpressora, MargemEsquerda, 30, MargemDireita, 30)

        e.Graphics.DrawLine(CanetaDaImpressora, MargemEsquerda, 170, MargemDireita, 170)

        'nome da empresa
        e.Graphics.DrawString(nomeFaz, FonteTitulo, Brushes.Black, MargemEsquerda + 220, 80, New StringFormat())

        'Imagem
        e.Graphics.DrawImage(Image.FromFile("G:\Nutrition\imgem\" & "stoantonio.jpg"), 550, 35)

        e.Graphics.DrawString(RelatorioTitulo & System.DateTime.Today, FonteSubTitulo, Brushes.Black, MargemEsquerda + 250, 120, New StringFormat())

        'campos a serem impressos:Lote, n animais, nome da dieta, Alimento, qtd etc
        e.Graphics.DrawString("Dieta: " & lblNomeDieta.Text, FonteNegrito, Brushes.Black, MargemEsquerda + 10, 35, New StringFormat())
        e.Graphics.DrawString("Lote: " & lbllotePM.Text, FonteNegrito, Brushes.Black, MargemEsquerda + 10, 55, New StringFormat())
        e.Graphics.DrawString("N° de Amimais: " & txtNAnimPremix.Text, FonteNegrito, Brushes.Black, MargemEsquerda + 10, 75, New StringFormat())
        e.Graphics.DrawString("Vagão : " & txtPesoVagao.Text & " Kg", FonteNegrito, Brushes.Black, MargemEsquerda + 10, 95, New StringFormat())
        e.Graphics.DrawString("N° Tratos: " & txtNTratos.Text & " /dia", FonteNegrito, Brushes.Black, MargemEsquerda + 10, 115, New StringFormat())
        e.Graphics.DrawString("Alimento", FonteNegrito, Brushes.Black, MargemEsquerda + 100, 170, New StringFormat())
        e.Graphics.DrawString("Quantidade", FonteNegrito, Brushes.Black, MargemEsquerda + 400, 170, New StringFormat())
        e.Graphics.DrawLine(CanetaDaImpressora, MargemEsquerda, 190, MargemDireita, 190)

        LinhasPorPagina = CInt(e.MarginBounds.Height / FonteNormal.GetHeight(e.Graphics) - 9)

        'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

        'Dim LinhaAtual1 As Integer = 0
        'Dim g As Graphics = e.Graphics
        'Dim PosicaoLinha As Integer = 40
        'Dim LinhasPorPagina1 As Byte = 45
        'Dim total As String = String.Empty
        'Dim grupo As String = String.Empty
        'Dim valor As String = String.Empty
        'Dim unidade As String = String.Empty
        ''Definições da pagina
        'g.PageUnit = GraphicsUnit.Millimeter
        ''Desenhar folha
        'Dim Titulo As New Font("Arial", 4, FontStyle.Regular, GraphicsUnit.Millimeter)
        'g.DrawString("*** Resumo de vendas por Grupo *** ", Titulo, Brushes.Black, 10, 20)

        'For L = 0 To LinhasPorPagina1 - 1
        '    If LinhaAtual1 = dtgDietaVg.Rows.Count Then Continue For
        '    For i As Integer = 0 To dtgDietaVg.ColumnCount - 1

        '        If Not dtgDietaVg.Item(i, LinhaAtual1).Value = Nothing Then
        '            grupo = dtgDietaVg.Item(i, LinhaAtual1).Value

        '        End If
        '    Next
        '    For i As Integer = 0 To dtgDietaVg.ColumnCount - 3

        '        If Not dtgDietaVg.Item(i, LinhaAtual1).Value = Nothing Then
        '            unidade = dtgDietaVg.Item(i, LinhaAtual1).Value

        '        End If
        '    Next
        '    For i As Integer = 0 To dtgDietaVg.ColumnCount - 4
        '        dtgDietaVg.Columns(1).DefaultCellStyle.Format = "C2"
        '        If Not dtgDietaVg.Item(i, LinhaAtual1).Value = Nothing Then
        '            valor = dtgDietaVg.Item(i, LinhaAtual1).Value
        '        End If
        '    Next
        '    'Alimento,Qtd,QtdPremix,QtdVagao Total/trato
        '    'Desenhar conteudo na página
        '    e.Graphics.DrawString("Alimento", New Font("Verdana", 6), Brushes.Black, 20, 30)
        '    e.Graphics.DrawString(grupo, New Font("Verdana", 6), Brushes.Black, 20, PosicaoLinha)
        '    e.Graphics.DrawString("Qtd", New Font("Verdana", 6), Brushes.Black, 40, 30)
        '    e.Graphics.DrawString(unidade, New Font("Verdana", 6), Brushes.Black, 40, PosicaoLinha)
        '    e.Graphics.DrawString("QtdVagao", New Font("Verdana", 6), Brushes.Black, 60, 30)
        '    e.Graphics.DrawString(valor, New Font("Verdana", 6), Brushes.Black, 60, PosicaoLinha)
        '    e.Graphics.DrawString("______________________________________________________________________________________________________________________________", New Font("Verdana", 8), Brushes.Black, 0, 34)
        '    PosicaoLinha += 5
        '    LinhaAtual += 1
        '    valor = String.Empty
        '    grupo = String.Empty
        '    unidade = String.Empty
        '    total = String.Empty
        'Next
        'e.Graphics.DrawString("Totais: ", New Font("Verdana", 6), Brushes.Black, 20, PosicaoLinha)
        'e.Graphics.DrawString(Label48.Text, New Font("Verdana", 6), Brushes.Black, 40, PosicaoLinha)
        'e.Graphics.DrawString(Label56.Text, New Font("Verdana", 6), Brushes.Black, 60, PosicaoLinha)
        ''End Sub



        'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx


        'Aqui sao lidos os dados
        While (LinhaAtual < LinhasPorPagina AndAlso Leitor.Read())

            'obtem os valores do datareader
            Nome = Leitor.GetString(0)

            Qtd = Leitor.GetString(1)

            'inicia a impressao
            PosicaoDaLinha = MargemSuperior + (LinhaAtual * FonteNormal.GetHeight(e.Graphics))

            e.Graphics.DrawString(Nome.ToString(), FonteNormal, Brushes.Black, MargemEsquerda + 100, PosicaoDaLinha, New StringFormat())

            e.Graphics.DrawString(Qtd.ToString & " Kg", FonteNormal, Brushes.Black, MargemEsquerda + 400, PosicaoDaLinha, New StringFormat())

            LinhaAtual += 1

        End While

        'Rodape
        e.Graphics.DrawLine(CanetaDaImpressora, MargemEsquerda, MargemInferior, MargemDireita, MargemInferior)

        e.Graphics.DrawString(System.DateTime.Now.ToString(), FonteRodape, Brushes.Black, MargemEsquerda, MargemInferior, New StringFormat())

        LinhaAtual += CInt(FonteNormal.GetHeight(e.Graphics))

        LinhaAtual += 1

        e.Graphics.DrawString("Pagina : " & paginaAtual, FonteRodape, Brushes.Black, MargemDireita - 50, MargemInferior, New StringFormat())

        'Incrementa o n£mero da pagina
        paginaAtual += 1

        'verifica se continua imprimindo
        If (LinhaAtual > LinhasPorPagina) Then
            e.HasMorePages = True
        Else
            e.HasMorePages = False
        End If

    End Sub

    'Encerra a conexÆo e o DataReader
    Private Sub End_Print(ByVal sender As Object, ByVal byvale As Printing.PrintEventArgs)

        Leitor.Close()

        fechar()

    End Sub

    Private Sub TabPage15_Click(sender As Object, e As EventArgs) Handles TabPage15.Click

    End Sub


    'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX      RELATÓRIOS      XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
    Private Sub btnDesfazer_Click(sender As Object, e As EventArgs) Handles btnDesfazer.Click
        RichTextBoxPrintCtrl1.Undo()
        RichTextBoxPrintCtrl1.Focus()
    End Sub

    Private Sub btnRefazer_Click(sender As Object, e As EventArgs) Handles btnRefazer.Click
        RichTextBoxPrintCtrl1.Redo()
        RichTextBoxPrintCtrl1.Focus()
    End Sub

    Private Sub btnNeglito_Click(sender As Object, e As EventArgs) Handles btnNeglito.Click
        If RichTextBoxPrintCtrl1.SelectionFont.Bold = True Then
            If RichTextBoxPrintCtrl1.SelectionFont.Italic = True Then
                RichTextBoxPrintCtrl1.SelectionFont = New Font(Me.RichTextBoxPrintCtrl1.SelectionFont, FontStyle.Regular + FontStyle.Italic)
            Else
                RichTextBoxPrintCtrl1.SelectionFont = New Font(Me.RichTextBoxPrintCtrl1.SelectionFont, FontStyle.Regular)
            End If
        ElseIf RichTextBoxPrintCtrl1.SelectionFont.Bold = False Then
            If RichTextBoxPrintCtrl1.SelectionFont.Italic = True Then
                RichTextBoxPrintCtrl1.SelectionFont = New Font(Me.RichTextBoxPrintCtrl1.SelectionFont, FontStyle.Bold + FontStyle.Italic)
            Else
                RichTextBoxPrintCtrl1.SelectionFont = New Font(Me.RichTextBoxPrintCtrl1.SelectionFont, FontStyle.Bold)
            End If
        End If
        RichTextBoxPrintCtrl1.Focus()
    End Sub

    Private Sub btnItalico_Click(sender As Object, e As EventArgs) Handles btnItalico.Click
        If RichTextBoxPrintCtrl1.SelectionFont.Italic = True Then
            If RichTextBoxPrintCtrl1.SelectionFont.Bold = True Then
                RichTextBoxPrintCtrl1.SelectionFont = New Font(Me.RichTextBoxPrintCtrl1.SelectionFont, FontStyle.Regular + FontStyle.Bold)
            Else
                RichTextBoxPrintCtrl1.SelectionFont = New Font(Me.RichTextBoxPrintCtrl1.SelectionFont, FontStyle.Regular)
            End If
        ElseIf RichTextBoxPrintCtrl1.SelectionFont.Italic = False Then
            If RichTextBoxPrintCtrl1.SelectionFont.Bold = True Then
                RichTextBoxPrintCtrl1.SelectionFont = New Font(Me.RichTextBoxPrintCtrl1.SelectionFont, FontStyle.Italic + FontStyle.Bold)
            Else
                RichTextBoxPrintCtrl1.SelectionFont = New Font(Me.RichTextBoxPrintCtrl1.SelectionFont, FontStyle.Italic)
            End If
        End If
        RichTextBoxPrintCtrl1.Focus()
    End Sub

    Private Sub btnFonte_Click(sender As Object, e As EventArgs) Handles btnFonte.Click
        Dim fontDialog As New FontDialog
        fontDialog.Font = RichTextBoxPrintCtrl1.SelectionFont
        fontDialog.ShowDialog()
        RichTextBoxPrintCtrl1.SelectionFont = fontDialog.Font
        RichTextBoxPrintCtrl1.Focus()

    End Sub

    Private Sub btnMaior_Click(sender As Object, e As EventArgs) Handles btnMaior.Click
        Try
            RichTextBoxPrintCtrl1.SelectionFont = New Font(RichTextBoxPrintCtrl1.SelectionFont.FontFamily, Int(RichTextBoxPrintCtrl1.SelectionFont.SizeInPoints + 5))
        Catch ex As Exception

        End Try
        RichTextBoxPrintCtrl1.Focus()
    End Sub

    Private Sub btnMenor_Click(sender As Object, e As EventArgs) Handles btnMenor.Click
        Try
            RichTextBoxPrintCtrl1.SelectionFont = New Font(RichTextBoxPrintCtrl1.SelectionFont.FontFamily, Int(RichTextBoxPrintCtrl1.SelectionFont.SizeInPoints - 5))

        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnFonteCor_Click(sender As Object, e As EventArgs) Handles btnFonteCor.Click
        Dim colorDialog As New ColorDialog
        colorDialog.Color = RichTextBoxPrintCtrl1.SelectionColor
        colorDialog.ShowDialog()
        RichTextBoxPrintCtrl1.SelectionColor = colorDialog.Color
        RichTextBoxPrintCtrl1.Focus()
    End Sub

    Private Sub btnMarcador_Click(sender As Object, e As EventArgs) Handles btnMarcador.Click
        Dim colorDialog As New ColorDialog
        colorDialog.Color = RichTextBoxPrintCtrl1.SelectionBackColor
        colorDialog.ShowDialog()
        RichTextBoxPrintCtrl1.SelectionBackColor = colorDialog.Color
        RichTextBoxPrintCtrl1.Focus()

    End Sub

    Private Sub btnCorFundo_Click(sender As Object, e As EventArgs) Handles btnCorFundo.Click
        Dim colorDialog As New ColorDialog
        colorDialog.Color = RichTextBoxPrintCtrl1.BackColor
        colorDialog.ShowDialog()
        RichTextBoxPrintCtrl1.BackColor = colorDialog.Color
        RichTextBoxPrintCtrl1.Focus()
    End Sub

    Private Sub btnAlinEsq_Click(sender As Object, e As EventArgs) Handles btnAlinEsq.Click
        RichTextBoxPrintCtrl1.SelectionAlignment = HorizontalAlignment.Left
    End Sub

    Private Sub btnAlinCentro_Click(sender As Object, e As EventArgs) Handles btnAlinCentro.Click
        RichTextBoxPrintCtrl1.SelectionAlignment = HorizontalAlignment.Center
    End Sub

    Private Sub btnAlinDir_Click(sender As Object, e As EventArgs) Handles btnAlinDir.Click
        RichTextBoxPrintCtrl1.SelectionAlignment = HorizontalAlignment.Right
    End Sub

    Private Sub btnCopiar_Click(sender As Object, e As EventArgs) Handles btnCopiar.Click
        My.Computer.Clipboard.Clear()
        Try
            Clipboard.SetText(RichTextBoxPrintCtrl1.SelectedText)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnRecort_Click(sender As Object, e As EventArgs) Handles btnRecort.Click
        My.Computer.Clipboard.Clear()
        Try
            Clipboard.SetText(RichTextBoxPrintCtrl1.SelectedText)
            RichTextBoxPrintCtrl1.SelectedText = ""
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnColar_Click(sender As Object, e As EventArgs) Handles btnColar.Click
        If My.Computer.Clipboard.ContainsText Then
            RichTextBoxPrintCtrl1.Paste()
        End If


    End Sub

    Private Sub btnImagem_Click(sender As Object, e As EventArgs) Handles btnImagem.Click

        'If OpenFileDialog1.ShowDialog() = DialogResult.OK Then
        '    picImagem.Load(OpenFileDialog1.FileName)
        '    Clipboard.SetImage(picImagem.Image)
        '    'picImagem.Image = Nothing
        '    Me.RichTextBoxPrintCtrl1.Paste()
        'End If
        'txtfoto.Text = OpenFileDialog1.FileName

    End Sub

    Private Sub SalvarImagem()
        '    'Abrir
        '    Dim fileReader As String
        '    fileReader = My.Computer.FileSystem.ReadAllText(Application.StartupPath & "\modelo.rtf")
        '    fileReader = fileReader.Replace("<nome_da_pessoa>", TextBox1.Text)

        '    Dim foto As String = txtfoto.Text
        '    'Aqui uma imagem
        '    Dim img As Bitmap = New Bitmap(picImagem.Image, New Size(150, 150))
        '    ' Dim img As Bitmap = New Bitmap(My.Resources.imagem, New Size(150, 150))
        '    Dim memStream As New IO.MemoryStream()
        '    img.Save(memStream, Imaging.ImageFormat.Png)
        '    Dim bytes() As Byte = memStream.ToArray()
        '    Dim sImg As String = BitConverter.ToString(bytes, 0).Replace("-", String.Empty)
        '    Dim larg As Integer = 100
        '    Dim alt As Integer = 100
        '    Dim cImg As String = "{\pict\pngblip\picw" & img.Width.ToString() & "\pich" & img.Height.ToString() & "\picwgoa" & img.Width.ToString() & "\pichgoa" & img.Height.ToString() & "\hex " & sImg & "}"
        '    fileReader = fileReader.Replace("<foto_pessoa>", cImg)

        '    'Salvar
        '    Dim result As DialogResult
        '    Dim fname As String

        '    SaveFileDialog1.Filter = "Rich Text Format|*.rtf|Text File|*.txt"

        '    SaveFileDialog1.InitialDirectory = "C:\"
        '    result = SaveFileDialog1.ShowDialog
        '    If result = Windows.Forms.DialogResult.OK Then
        '        fname = SaveFileDialog1.FileName
        '        If SaveFileDialog1.FilterIndex = 1 Then
        '            RichTextBoxPrintCtrl1.SaveFile(fname)
        '        Else
        '            RichTextBoxPrintCtrl1.SaveFile(fname, RichTextBoxStreamType.PlainText)
        '        End If
        '    End If





        '    'Dim StreamW As New IO.StreamWriter(SaveFileDialog1.FileName, True, System.Text.Encoding.Default)
        '    'StreamW.Write(fileReader)
        '    'StreamW.Close()
        '    'If SaveFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
        '    '    Dim SW As New IO.StreamWriter(SaveFileDialog1.FileName)
        '    '    SW.Write(RichTextBoxPrintCtrl1.Text)
        '    '    SW.Flush()
        '    '    SW.Close()
        '    '    FileOpen(1, SaveFileDialog1.FileName, OpenMode.Output)
        '    '    Print(1, RichTextBoxPrintCtrl1.Text)
        '    '    FileClose(1)
        '    'End If



    End Sub




    Private Sub btnSalvarRelatorio_Click(sender As Object, e As EventArgs) Handles btnSalvarRelatorio.Click
        'If PrintDialog1.ShowDialog() = DialogResult.OK Then
        '    PrintDocument1.Print()
        'End If
        ''PrintPreviewDialog1.ShowDialog()

        ''SalvarImagem()

        ''xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
        ''If SaveFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
        ''    Dim SW As New IO.StreamWriter(SaveFileDialog1.FileName)
        ''    SW.Write(RichTextBoxPrintCtrl1.Text)
        ''    SW.Flush()
        ''    SW.Close()
        ''    FileOpen(1, SaveFileDialog1.FileName, OpenMode.Output)
        ''    Print(1, RichTextBoxPrintCtrl1.Text)
        ''    FileClose(1)
        ''End If
    End Sub

    Private Sub btnAbrir_Click(sender As Object, e As EventArgs) Handles btnAbrir.Click
        'Dim result As DialogResult
        'OpenFileDialog1.Filter = "Rich Text Format|*.rtf|Text File|*.txt"

        'OpenFileDialog1.InitialDirectory = "C:\"
        'result = OpenFileDialog1.ShowDialog
        'If result = Windows.Forms.DialogResult.OK Then
        '    If OpenFileDialog1.FilterIndex = 1 Then
        '        RichTextBoxPrintCtrl1.LoadFile(OpenFileDialog1.FileName)
        '    Else
        '        RichTextBoxPrintCtrl1.LoadFile(OpenFileDialog1.FileName, RichTextBoxStreamType.PlainText)
        '    End If
        'End If


        ''xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
        ''Dim filename As String = ""

        ''Dim openfileDialog As FileDialog = New OpenFileDialog()
        ''openfileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*" 'sets the filter for txt or all files
        ''If openfileDialog.ShowDialog(Me) = DialogResult.OK Then
        ''    filename = openfileDialog.FileName
        ''End If

        ''If filename = "" Then        'you don't really need this if you check for DialogResult.Cancel, but this was a quick example so just a safeguard
        ''    filename = "dude.txt"
        ''End If
        ''File.AppendAllText(filename, "Here is a line" + Environment.NewLine) 'Opens the file (or creates one if it doesnt exist), writes the string plus a newline and closes the file

    End Sub


    Private verificaImpressao As Integer
    'Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
    '    verificaImpressao = 0
    'End Sub
    'Private Sub PrintDocument1_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
    '    ' imprime o conteudo do RichTextBox. 
    '    ' armazena o ultimo caractere impresso
    '    verificaImpressao = RichTextBoxPrintCtrl1.Print(verificaImpressao, RichTextBoxPrintCtrl1.TextLength, e)
    '    ' verifica se há mais paginas
    '    If verificaImpressao < RichTextBoxPrintCtrl1.TextLength Then
    '        e.HasMorePages = True
    '    Else
    '        e.HasMorePages = False
    '    End If
    'End Sub

    Private Sub btnVisualizarrImpr_Click(sender As Object, e As EventArgs) Handles btnVisualizarrImpr.Click
        'PrintPreviewDialog1.ShowDialog()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        picImagem.Load("G:\Nutrition\imgem\esquerda.png")
        Clipboard.SetImage(picImagem.Image)
        Me.RichTextBoxPrintCtrl1.Paste()
        picImagem.Image = Nothing

    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        picImagem.Load("G:\Nutrition\imgem\direita.png")
        Clipboard.SetImage(picImagem.Image)
        Me.RichTextBoxPrintCtrl1.Paste()
        picImagem.Image = Nothing
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        picImagem.Load("G:\Nutrition\imgem\acima.png")
        Clipboard.SetImage(picImagem.Image)
        Me.RichTextBoxPrintCtrl1.Paste()
        picImagem.Image = Nothing
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        picImagem.Load("G:\Nutrition\imgem\abaixo.png")
        Clipboard.SetImage(picImagem.Image)
        Me.RichTextBoxPrintCtrl1.Paste()
        picImagem.Image = Nothing
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        picImagem.Load("G:\Nutrition\imgem\esqacima.png")
        Clipboard.SetImage(picImagem.Image)
        Me.RichTextBoxPrintCtrl1.Paste()
        picImagem.Image = Nothing
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        picImagem.Load("G:\Nutrition\imgem\diracima.png")
        Clipboard.SetImage(picImagem.Image)
        Me.RichTextBoxPrintCtrl1.Paste()
        picImagem.Image = Nothing
    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        picImagem.Load("G:\Nutrition\imgem\dirabaixo.png")
        Clipboard.SetImage(picImagem.Image)
        Me.RichTextBoxPrintCtrl1.Paste()
        picImagem.Image = Nothing
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        picImagem.Load("G:\Nutrition\imgem\esqabaixo.png")
        Clipboard.SetImage(picImagem.Image)
        Me.RichTextBoxPrintCtrl1.Paste()
        picImagem.Image = Nothing
    End Sub

    Private Sub TabPage1_Click(sender As Object, e As EventArgs) Handles TabPage1.Click

    End Sub

    Private Sub btnCadastrarForragem_Click(sender As Object, e As EventArgs) Handles btnCadastrarForragem.Click

        CadastrarKPS()
        CadastrarForragem()

        BuscarForragemAgrup()
        BuscarKPS()
    End Sub

    Private Sub Button14_Click(sender As Object, e As EventArgs)
        MediaTratos()
    End Sub

    Private Sub TabPage10_Click(sender As Object, e As EventArgs) Handles TabPage10.Click

    End Sub

    Private Sub tabVagao_Click(sender As Object, e As EventArgs) Handles tabVagao.Click

    End Sub


    Private Sub Button28_Click(sender As Object, e As EventArgs) Handles Button28.Click
        Me.Close()
    End Sub

   
    Private Sub tabParticulas_Click(sender As Object, e As EventArgs) Handles tabParticulas.Click

    End Sub

    Private Sub btnVisLote_Click(sender As Object, e As EventArgs) Handles btnVisLote.Click

    End Sub

    Private Sub btnTelaPrincForrgem_Click(sender As Object, e As EventArgs) Handles btnTelaForrgem.Click
        pnlHistForragem.Visible = False
        pnlForragemLabel.Visible = False
        row1 = 0
        row2 = 1
    End Sub

    Private Sub CadastrarTratos()
        data = Now.ToString("dd-MM-yyyy")
        Dim sql As String
        Dim cmd As SQLiteCommand


        For Each row As DataGridViewRow In dtgAmostras.Rows
            Try
                abrir()
                sql = "Insert into Tratos (Nome,NomeLote,QtdAmostras,QtdPeneiras,Peneira,Am01,Am02,Am03,Am04,Am05,Am06,Am07,Am08,Am09,Am10,Media,CV,IdPropriedade) values (@Nome,@NomeLote,@QtdAmostras,@QtdPeneiras,@Peneira,@Am01,@Am02,@Am03,@Am04,@Am05,@Am06,@Am07,@Am08,@Am09,@Am10,@Media,@CV,@IdPropriedade)"
                cmd = New SQLiteCommand(sql, con)
                'cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@Nome", txtNomeTrato.Text & " - " & data)
                cmd.Parameters.AddWithValue("@NomeLote", cbxNomeLote.Text)
                cmd.Parameters.AddWithValue("@QtdAmostras", cbxQtdAmostras.Text)
                cmd.Parameters.AddWithValue("@QtdPeneiras", cbxQtdPeneirasTrato.Text)
                cmd.Parameters.AddWithValue("@Peneira", row.Cells("Peneiras").Value.ToString)
                cmd.Parameters.AddWithValue("@Am01", row.Cells("01").Value.ToString)
                cmd.Parameters.AddWithValue("@Am02", row.Cells("02").Value.ToString)
                cmd.Parameters.AddWithValue("@Am03", row.Cells("03").Value.ToString)
                cmd.Parameters.AddWithValue("@Am04", row.Cells("04").Value.ToString)
                cmd.Parameters.AddWithValue("@Am05", row.Cells("05").Value.ToString)
                cmd.Parameters.AddWithValue("@Am06", row.Cells("06").Value.ToString)
                cmd.Parameters.AddWithValue("@Am07", row.Cells("07").Value.ToString)
                cmd.Parameters.AddWithValue("@Am08", row.Cells("08").Value.ToString)
                cmd.Parameters.AddWithValue("@Am09", row.Cells("09").Value.ToString)
                cmd.Parameters.AddWithValue("@Am10", row.Cells("10").Value.ToString)
                cmd.Parameters.AddWithValue("@Media", row.Cells("Média").Value.ToString)
                cmd.Parameters.AddWithValue("@CV", row.Cells("CV%").Value.ToString)
                cmd.Parameters.AddWithValue("@IdPropriedade", lblIdProp.Text)

                cmd.ExecuteNonQuery()
                'ListarComissoes()

            Catch ex As Exception
                MsgBox("Erro ao salvar!" + ex.Message)
                fechar()
            End Try

        Next
        MsgBox("Trato cadastrado com sucesso!")

        'btnEditarCliente.Enabled = False

        'dtgForragem.Rows.Clear()
        txtNomeTrato.Text = ""
        cbxQtdPeneirasTrato.Text = ""


        'Else
        'MsgBox("Preencha os campos corretamente!")

        'End If
    End Sub

    Private Sub BuscarTratosAgrup()

        Dim da As New SQLiteDataAdapter
        Dim dt As New DataTable
        Dim sql As String
        Try

            abrir()
            sql = "Select Nome,NomeLote,QtdAmostras,QtdPeneiras from Tratos  where IdPropriedade = " & "'" & idFaz & "'" & "  group by Nome,NomeLote,QtdAmostras,QtdPeneiras"
            'sql = "Select Nome, QtdPeneiras from Forragem group by Nome"
            'SUM(Quantidade)
            da = New SQLiteDataAdapter(sql, con)
            dt = New DataTable
            da.Fill(dt)
            dtgHistTratos.DataSource = dt

            fechar()

        Catch ex As Exception

        End Try

        'dtgClientes.DataSource = dtgFazendas.DataSource
    End Sub

    Private Sub BuscarTratos()

        Dim da As New SQLiteDataAdapter
        Dim dt As New DataTable
        Dim sql As String
        Try

            abrir()
            sql = "Select Peneira,Am01,Am02,Am03,Am04,Am05,Am06,Am07,Am08,Am09,Am10,Media,CV from Tratos where Nome = " & "'" & IdTrato & "'"
            da = New SQLiteDataAdapter(sql, con)
            dt = New DataTable
            da.Fill(dt)
            dtgAmostras.DataSource = dt

            fechar()

        Catch ex As Exception

        End Try

        'SomarAmostras()

        'dtgClientes.DataSource = dtgFazendas.DataSource
    End Sub

    Private Sub DeletarTratos()

        Dim cmd As SQLiteCommand
        Dim sqlDelete As String = "Delete from Tratos where Nome = " & "'" & Me.txtNomeTrato.Text & "'"
        'Mensagem se realmente quer excluir
        If MsgBox("Excluir trato?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            Try
                abrir()
                cmd = New SQLiteCommand(sqlDelete, con)
                'cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@Nome", txtNomeTrato.Text)
                cmd.ExecuteNonQuery()
                MsgBox("Amostras excluidas com sucesso!")
            Catch ex As Exception
                MsgBox("Erro ao exluir amostras!" + ex.Message)
                fechar()
            End Try

        Else
            MsgBox("Você precisa escolher uma amostra na tabela!")
        End If

    End Sub

 

    Private Sub ConfigAmostras() ' Trato

        On Error Resume Next
        Dim qtdAmostras As Integer = Val(cbxQtdAmostras.Text)

        With dtgAmostras
            .DefaultCellStyle.Font = New Font("Inter", 8.5, FontStyle.Regular)

            .Columns(0).HeaderText = "Peneira"
            .Columns(1).HeaderText = "01"
            .Columns(2).HeaderText = "02"
            .Columns(3).HeaderText = "03"
            .Columns(4).HeaderText = "04"
            .Columns(5).HeaderText = "05"
            .Columns(6).HeaderText = "06"
            .Columns(7).HeaderText = "07"
            .Columns(8).HeaderText = "08"
            .Columns(9).HeaderText = "09"
            .Columns(10).HeaderText = "10"
            .Columns(11).HeaderText = "Média"
            '.Columns(12).HeaderText = "CV%"

            .Columns(0).DefaultCellStyle.BackColor = Color.White
            If cbxQtdAmostras.Text = "10" Then
                .Columns(0).Width = 119
            ElseIf cbxQtdAmostras.Text = "09" Then
                .Columns(0).Width = 133
            ElseIf cbxQtdAmostras.Text = "08" Then
                .Columns(0).Width = 152
            ElseIf cbxQtdAmostras.Text = "07" Then
                .Columns(0).Width = 171
            ElseIf cbxQtdAmostras.Text = "06" Then
                .Columns(0).Width = 175
            ElseIf cbxQtdAmostras.Text = "05" Then
                .Columns(0).Width = 191
            End If

            .Columns(0).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            ' Configura colunas de amostras
            For i As Integer = 1 To 11
                '.Columns(i).HeaderText = i.ToString("D2")
                If cbxQtdAmostras.Text = "10" Then
                    .Columns(i).Width = 84
                ElseIf cbxQtdAmostras.Text = "09" Then
                    .Columns(i).Width = 91
                ElseIf cbxQtdAmostras.Text = "08" Then
                    .Columns(i).Width = 99
                ElseIf cbxQtdAmostras.Text = "07" Then
                    .Columns(i).Width = 109
                ElseIf cbxQtdAmostras.Text = "06" Then
                    .Columns(i).Width = 124
                ElseIf cbxQtdAmostras.Text = "05" Then
                    .Columns(i).Width = 142
                End If

                .Columns(i).DefaultCellStyle.BackColor = Color.WhiteSmoke
                .Columns(i).Visible = (i <= qtdAmostras)
                .Columns(i).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            Next

            ' Colunas finais
            'If cbxQtdAmostras.Text = "10" Then
            'Columns(11).Width = 84
            'ElseIf cbxQtdAmostras.Text = "09" Then
            '    .Columns(10).Width = 150
            'ElseIf cbxQtdAmostras.Text = "08" Then
            '    .Columns(9).Width = 100
            'ElseIf cbxQtdAmostras.Text = "07" Then
            '    .Columns(8).Width = 115
            'ElseIf cbxQtdAmostras.Text = "06" Then
            '    .Columns(7).Width = 125
            'ElseIf cbxQtdAmostras.Text = "05" Then
            '    .Columns(6).Width = 135

            'End If
            .Columns(11).Visible = True
            '.Columns(11).DefaultCellStyle.BackColor = Color.WhiteSmoke
            '.Columns(11).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns(12).Visible = False


            '.Columns(12).HeaderText = "CV%"
            '.Columns(12).Width = 80

        End With

        With dtgAmostras.ColumnHeadersDefaultCellStyle
            .Font = New Font("Inter", 9, FontStyle.Bold)
            .Padding = New Padding(0, 10, 0, 10) ' espaço interno
            .BackColor = Color.White
            .Alignment = DataGridViewContentAlignment.MiddleCenter
        End With
        ' dtgAmostras.ColumnHeadersHeight = 40
    End Sub
    Private Sub btnSalvarTratos_Click(sender As Object, e As EventArgs) Handles btnSalvarTratos.Click
        CadastrarTratos()
        TabelaAmostras()
        BuscarTratosAgrup()
    End Sub

    Private Sub BarraTratos()
        If cbxQtdPeneirasTrato.Text = "03 Peneiras" Then
            pnlTratoBarra.Location = New Point(37, 404)
        ElseIf cbxQtdPeneirasTrato.Text = "04 Peneiras" Then
            pnlTratoBarra.Location = New Point(37, 429)
        End If
    End Sub

  

    Private Sub btnNovaAmostra_Click(sender As Object, e As EventArgs) Handles btnNovaAmostra.Click

        txtNomeTrato.Text = ""
        cbxNomeLote.Text = ""
        cbxQtdPeneirasTrato.Text = "04 Peneiras"
        cbxQtdAmostras.Text = "05"

        txtNomeTrato.Enabled = True
        cbxNomeLote.Enabled = True
        cbxQtdPeneirasTrato.Enabled = True
        cbxQtdAmostras.Enabled = True

        btnExcluirTrato.Enabled = False
        btnEditarTrato.Enabled = False

        lbltrat19.Text = ""
        lbltrat8.Text = ""
        lbltrat18.Text = ""
        lbltratFundo.Text = ""

    End Sub

    Private Sub dtgAmostras_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles dtgAmostras.CellBeginEdit
        If txtNomeTrato.Text <> "" And cbxNomeLote.Text <> "" And cbxQtdPeneirasTrato.Text <> "" And cbxQtdAmostras.Text <> "" And txtNomeTrato.Enabled = True Then
            btnSalvarTratos.Enabled = True
        Else
            btnSalvarTratos.Enabled = False
        End If
        'MediaTratos()
    End Sub

    Private Sub cbxQtdAmostras_MouseClick(sender As Object, e As MouseEventArgs) Handles cbxQtdAmostras.MouseClick
        'TabelaAmostras()
        Threading.Thread.Sleep(100)
        TabelaAmostras()
    End Sub

    Private Sub cbxQtdAmostras_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxQtdAmostras.SelectedIndexChanged
        dtgAmostras.DataSource = Nothing
        Threading.Thread.Sleep(100)
        TabelaAmostras()

    End Sub

    Dim IdTrato As String
    Private Sub btnTrato_Click(sender As Object, e As EventArgs) Handles btnTrato.Click
        ' Exibe a aba de tratos e a seleciona
        Me.tabTratos.Parent = Me.tcManejo
        Me.tcManejo.SelectedTab = tabTratos

        ' Carrega dados
        TabelaAmostras()
        BuscarTratosAgrup()

        ' Verifica se há dados no histórico de tratos
        If dtgHistTratos.Rows.Count > 0 Then
            ' Recupera valores da primeira linha
            Dim idValor = dtgHistTratos.Rows(0).Cells(0).Value
            Dim loteValor = dtgHistTratos.Rows(0).Cells(1).Value
            Dim qtdAmostras = dtgHistTratos.Rows(0).Cells(2).Value
            Dim qtdPeneiras = dtgHistTratos.Rows(0).Cells(3).Value

            ' Atribui valores aos controles, com verificação de nulos
            If idValor IsNot Nothing Then IdTrato = idValor
            If loteValor IsNot Nothing Then cbxNomeLote.Text = loteValor.ToString()
            If qtdPeneiras IsNot Nothing Then cbxQtdPeneirasTrato.Text = qtdPeneiras.ToString()
            If qtdAmostras IsNot Nothing Then cbxQtdAmostras.Text = qtdAmostras.ToString()

            ' Buscar dados do trato selecionado
            BuscarTratos()

            ' Desabilita edição dos campos para prevenir alterações sem intenção
            cbxNomeLote.Enabled = False
            cbxQtdPeneirasTrato.Enabled = False
            cbxQtdAmostras.Enabled = False
            btnSalvarTratos.Enabled = False

            ' Configura grid conforme quantidade de amostras
            ConfigAmostras()

            Dim p19mm As Double = dtgAmostras.Rows(0).Cells(12).Value
            Dim p8mm As Double = dtgAmostras.Rows(1).Cells(12).Value
            Dim p18mm As Double = dtgAmostras.Rows(2).Cells(12).Value
            Dim pFundo As Double = dtgAmostras.Rows(3).Cells(12).Value

            lbltrat19.Text = p19mm.ToString("F1")
            lbltrat8.Text = p8mm.ToString("F1")
            lbltrat18.Text = p18mm.ToString("F1")
            lbltratFundo.Text = pFundo.ToString("F1")

        Else
            ' Configuração padrão caso não existam tratos anteriores
            cbxQtdPeneirasTrato.Text = "04 Peneiras"
            cbxQtdAmostras.Text = "10"
        End If

        ' Atualiza barra de progresso ou status
        BarraTratos()
    End Sub

    Private Sub TabelaAmostras()
        Dim dt As New DataTable()
        Try
            ' Adiciona as colunas fixas
            dt.Columns.Add("Peneiras")
            For i As Integer = 1 To 10
                dt.Columns.Add(i.ToString("00"))
            Next
            dt.Columns.Add("Média")
            dt.Columns.Add("CV%")

            ' Define número de linhas (peneiras)
            Dim peneiras As New List(Of String)
            peneiras.Add("19mm")
            peneiras.Add("8mm")
            If cbxQtdPeneirasTrato.Text = "04 Peneiras" Then
                peneiras.Add("1.8mm")
            End If
            peneiras.Add("Fundo")

            ' Define número de amostras a preencher
            Dim qtdAmostras As Integer
            If Not Integer.TryParse(cbxQtdAmostras.Text, qtdAmostras) Then
                qtdAmostras = 10 ' padrão
            End If

            ' Cria linhas
            For Each peneira As String In peneiras
                Dim row As New List(Of String)
                row.Add(peneira)
                For i As Integer = 1 To 10
                    If i <= qtdAmostras Then
                        row.Add("0")
                    Else
                        row.Add("") ' célula vazia
                    End If
                Next
                row.Add("0") ' Média
                row.Add("0") ' CV%
                dt.Rows.Add(row.ToArray())
            Next

            dtgAmostras.DataSource = dt
        Catch ex As Exception
            MessageBox.Show("Erro ao criar tabela de amostras: " & ex.Message)
        End Try
        ConfigAmostras()
    End Sub
  

    Private Sub Button26_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub cbxQtdPeneirasTrato_SelectedIndexChanged_1(sender As Object, e As EventArgs) Handles cbxQtdPeneirasTrato.SelectedIndexChanged
        BarraTratos()
    End Sub
End Class
'Private Sub dtgHistTratos_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dtgHistTratos.CellClick
'    txtNomeTrato.Text = dtgHistTratos.CurrentRow.Cells(0).Value
'    cbxNomeLote.Text = dtgHistTratos.CurrentRow.Cells(1).Value
'    cbxQtdPeneirasTrato.Text = dtgHistTratos.CurrentRow.Cells(3).Value
'    cbxQtdAmostras.Text = dtgHistTratos.CurrentRow.Cells(2).Value
'    BuscarTratos()

'    txtNomeTrato.Enabled = False
'    cbxNomeLote.Enabled = False
'    cbxQtdPeneirasTrato.Enabled = False
'    cbxQtdAmostras.Enabled = False

'    btnSalvarTratos.Enabled = False
'    btnExcluirTrato.Enabled = True
'    btnEditarTrato.Enabled = True
'    ConfigAmostras()

'    'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx     revisar de forragem    xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

'    'txtNomeForragem.Text = dtgHistForragem.CurrentRow.Cells(0).Value
'    'cbxQtdPeneiras.Text = dtgHistForragem.CurrentRow.Cells(1).Value
'    'txtNomeForragem.Enabled = False
'    'cbxQtdPeneiras.Enabled = False
'    'btnSalvarForragem.Enabled = False
'    'BuscarForragem()

'    'Label42.Visible = True
'    'lblTAmostra.Visible = True
'    'AjustarLabelAmostras()
'    'SomarAmostras()
'    'dtgForragem.Enabled = False
'    'dtgKPS.Enabled = False
'    'BuscarKPS()
'    'BuscarKPS()
'    'CoresKPS()
'    'ConfigGrid()



'End Sub

'Private Sub TabelaAmostras() ' Tratos
'    Dim dt As New DataTable()
'    Try

'        If cbxQtdAmostras.Text = "05" Then

'            dt.Columns.Add("Peneiras")
'            dt.Columns.Add("01")
'            dt.Columns.Add("02")
'            dt.Columns.Add("03")
'            dt.Columns.Add("04")
'            dt.Columns.Add("05")
'            dt.Columns.Add("06")
'            dt.Columns.Add("07")
'            dt.Columns.Add("08")
'            dt.Columns.Add("09")
'            dt.Columns.Add("10")
'            dt.Columns.Add("Média")
'            dt.Columns.Add("CV%")

'            If cbxQtdPeneirasTrato.Text = "03 Peneiras" Then
'                dt.Rows.Add("19mm", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
'                dt.Rows.Add("8mm", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
'                dt.Rows.Add("Fundo", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")

'            ElseIf cbxQtdPeneirasTrato.Text = "04 Peneiras" Then
'                dt.Rows.Add("19mm", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
'                dt.Rows.Add("8mm", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
'                dt.Rows.Add("1.8mm", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
'                dt.Rows.Add("Fundo", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
'            End If

'        ElseIf cbxQtdAmostras.Text = "06" Then

'            dt.Columns.Add("Peneiras")
'            dt.Columns.Add("01")
'            dt.Columns.Add("02")
'            dt.Columns.Add("03")
'            dt.Columns.Add("04")
'            dt.Columns.Add("05")
'            dt.Columns.Add("06")
'            dt.Columns.Add("07")
'            dt.Columns.Add("08")
'            dt.Columns.Add("09")
'            dt.Columns.Add("10")
'            dt.Columns.Add("Média")
'            dt.Columns.Add("CV%")
'            If cbxQtdPeneirasTrato.Text = "03 Peneiras" Then
'                dt.Rows.Add("19mm", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
'                dt.Rows.Add("8mm", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
'                dt.Rows.Add("Fundo", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
'            ElseIf cbxQtdPeneirasTrato.Text = "04 Peneiras" Then
'                dt.Rows.Add("19mm", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
'                dt.Rows.Add("8mm", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
'                dt.Rows.Add("1.8mm", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
'                dt.Rows.Add("Fundo", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
'            End If

'        ElseIf cbxQtdAmostras.Text = "07" Then

'            dt.Columns.Add("Peneiras")
'            dt.Columns.Add("01")
'            dt.Columns.Add("02")
'            dt.Columns.Add("03")
'            dt.Columns.Add("04")
'            dt.Columns.Add("05")
'            dt.Columns.Add("06")
'            dt.Columns.Add("07")
'            dt.Columns.Add("08")
'            dt.Columns.Add("09")
'            dt.Columns.Add("10")
'            dt.Columns.Add("Média")
'            dt.Columns.Add("CV%")
'            If cbxQtdPeneirasTrato.Text = "03 Peneiras" Then
'                dt.Rows.Add("19mm", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
'                dt.Rows.Add("8mm", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
'                dt.Rows.Add("Fundo", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
'            ElseIf cbxQtdPeneirasTrato.Text = "04 Peneiras" Then
'                dt.Rows.Add("19mm", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
'                dt.Rows.Add("8mm", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
'                dt.Rows.Add("1.8mm", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
'                dt.Rows.Add("Fundo", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
'            End If

'        ElseIf cbxQtdAmostras.Text = "08" Then

'            dt.Columns.Add("Peneiras")
'            dt.Columns.Add("01")
'            dt.Columns.Add("02")
'            dt.Columns.Add("03")
'            dt.Columns.Add("04")
'            dt.Columns.Add("05")
'            dt.Columns.Add("06")
'            dt.Columns.Add("07")
'            dt.Columns.Add("08")
'            dt.Columns.Add("09")
'            dt.Columns.Add("10")
'            dt.Columns.Add("Média")
'            dt.Columns.Add("CV%")
'            If cbxQtdPeneirasTrato.Text = "03 Peneiras" Then
'                dt.Rows.Add("19mm", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
'                dt.Rows.Add("8mm", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
'                dt.Rows.Add("Fundo", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
'            ElseIf cbxQtdPeneirasTrato.Text = "04 Peneiras" Then
'                dt.Rows.Add("19mm", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
'                dt.Rows.Add("8mm", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
'                dt.Rows.Add("1.8mm", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
'                dt.Rows.Add("Fundo", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
'            End If


'        ElseIf cbxQtdAmostras.Text = "09" Then

'            dt.Columns.Add("Peneiras")
'            dt.Columns.Add("01")
'            dt.Columns.Add("02")
'            dt.Columns.Add("03")
'            dt.Columns.Add("04")
'            dt.Columns.Add("05")
'            dt.Columns.Add("06")
'            dt.Columns.Add("07")
'            dt.Columns.Add("08")
'            dt.Columns.Add("09")
'            dt.Columns.Add("10")
'            dt.Columns.Add("Média")
'            dt.Columns.Add("CV%")
'            If cbxQtdPeneirasTrato.Text = "03 Peneiras" Then
'                dt.Rows.Add("19mm", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
'                dt.Rows.Add("8mm", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
'                dt.Rows.Add("Fundo", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
'            ElseIf cbxQtdPeneirasTrato.Text = "04 Peneiras" Then
'                dt.Rows.Add("19mm", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
'                dt.Rows.Add("8mm", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
'                dt.Rows.Add("1.8mm", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
'                dt.Rows.Add("Fundo", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
'            End If

'        ElseIf cbxQtdAmostras.Text = "10" Then

'            dt.Columns.Add("Peneiras")
'            dt.Columns.Add("01")
'            dt.Columns.Add("02")
'            dt.Columns.Add("03")
'            dt.Columns.Add("04")
'            dt.Columns.Add("05")
'            dt.Columns.Add("06")
'            dt.Columns.Add("07")
'            dt.Columns.Add("08")
'            dt.Columns.Add("09")
'            dt.Columns.Add("10")
'            dt.Columns.Add("Média")
'            dt.Columns.Add("CV%")
'            If cbxQtdPeneirasTrato.Text = "03 Peneiras" Then
'                dt.Rows.Add("19mm", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
'                dt.Rows.Add("8mm", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
'                dt.Rows.Add("Fundo", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
'            ElseIf cbxQtdPeneirasTrato.Text = "04 Peneiras" Then
'                dt.Rows.Add("19mm", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
'                dt.Rows.Add("8mm", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
'                dt.Rows.Add("1.8mm", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
'                dt.Rows.Add("Fundo", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0")
'            End If

'        End If
'        dtgAmostras.DataSource = dt
'    Catch ex As Exception
'        Throw ex
'    End Try
'    ConfigAmostras()
'End Sub

'Public Class Form1

'    Private Sub btnConfigurar_Click(sender As Object, e As EventArgs) Handles btnConfigurar.Click
'        PageSetupDialog1.ShowDialog()
'    End Sub
'    '2- Visualizar Impressão

'    Private Sub btnVisualizar_Click(sender As Object, e As EventArgs) Handles btnVisualizar.Click
'        PrintPreviewDialog1.ShowDialog()
'    End Sub
'    '3- Imprimir

'    Private Sub btnImprimir_Click(sender As Object, e As EventArgs) Handles btnImprimir.Click
'        If PrintDialog1.ShowDialog() = DialogResult.OK Then
'            PrintDocument1.Print()
'        End If
'    End Sub
'    'No evento Click do botão Imagem vamos definir o código para incluir uma imagem no controle RichtTextBox conforme abaixo:

'    Private Sub btnImagem_Click(sender As Object, e As EventArgs) Handles btnImagem.Click
'        If OpenFileDialog1.ShowDialog() = DialogResult.OK Then
'            picImagem.Load(OpenFileDialog1.FileName)
'            Clipboard.SetImage(picImagem.Image)
'            picImagem.Image = Nothing
'            Me.RichTextBoxPrintCtrl1.Paste()
'        End If
'    End Sub
'    'Agora no evento Click do botão Iniciar Dados inclua o código abaixo para preencher o controle com alguns dados:

'    Private Sub btnIniciar_Click(sender As Object, e As EventArgs) Handles btnIniciar.Click
'        With RichTextBoxPrintCtrl1
'            .BackColor = Color.White
'            .Clear()
'            .BulletIndent = 10
'            .SelectionFont = New Font("Georgia", 18, FontStyle.Bold)
'            .SelectedText = "Macoratti .net " & vbLf
'            .SelectionFont = New Font("Verdana", 16)
'            .SelectionBullet = True
'            .SelectionColor = Color.DarkBlue
'            .SelectedText = "Quase Tudo" + vbLf
'            .SelectionFont = New Font("Verdana", 14)
'            .SelectionColor = Color.Orange
'            .SelectedText = "Para VB.NET" + vbLf
'            .SelectionFont = New Font("Verdana", 12)
'            .SelectionColor = Color.Green
'            .SelectedText = "C# , ASP .NET " + vbLf
'            .SelectionColor = Color.Red
'            .SelectedText = "e outros recursos da plataforma .NET" + vbLf
'            .SelectionBullet = False
'            .SelectionFont = New Font("Tahoma", 10)
'            .SelectionColor = Color.Black
'            .SelectedText = "http://www.macoratti.net" & vbLf
'        End With
'    End Sub
'    'Finalmente inclua o código abaixo no formulário onde

'    Private verificaImpressao As Integer
'    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
'        verificaImpressao = 0
'    End Sub
'    Private Sub PrintDocument1_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
'        ' imprime o conteudo do RichTextBox. 
'        ' armazena o ultimo caractere impresso
'        verificaImpressao = RichTextBoxPrintCtrl1.Print(verificaImpressao, RichTextBoxPrintCtrl1.TextLength, e)
'        ' verifica se há mais paginas
'        If verificaImpressao < RichTextBoxPrintCtrl1.TextLength Then
'            e.HasMorePages = True
'        Else
'            e.HasMorePages = False
'        End If
'    End Sub
'End Class



'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX      CODIGOS COMENTADOS     XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX      PRINT RICHTEXT     XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
'Public Class Form1
'    Private Sub btnConfigurar_Click(sender As Object, e As EventArgs) Handles btnConfigurar.Click
'        PageSetupDialog1.ShowDialog()
'    End Sub

'    Private Sub btnVisualizar_Click(sender As Object, e As EventArgs) Handles btnVisualizar.Click
'        PrintPreviewDialog1.ShowDialog()
'    End Sub

'    Private Sub btnImprimir_Click(sender As Object, e As EventArgs) Handles btnImprimir.Click
'        If PrintDialog1.ShowDialog() = DialogResult.OK Then
'            PrintDocument1.Print()
'        End If
'    End Sub

'    Private Sub btnIniciar_Click(sender As Object, e As EventArgs) Handles btnIniciar.Click
'        With RichTextBoxPrintCtrl1
'            .BackColor = Color.White
'            .Clear()

'            .BulletIndent = 10
'            .SelectionFont = New Font("Georgia", 18, FontStyle.Bold)
'            .SelectedText = "Macoratti .net " & vbLf
'            .SelectionFont = New Font("Verdana", 16)
'            .SelectionBullet = True
'            .SelectionColor = Color.DarkBlue
'            .SelectedText = "Quase Tudo" + vbLf
'            .SelectionFont = New Font("Verdana", 14)
'            .SelectionColor = Color.Orange
'            .SelectedText = "Para VB.NET" + vbLf
'            .SelectionFont = New Font("Verdana", 12)
'            .SelectionColor = Color.Green
'            .SelectedText = "C# , ASP .NET " + vbLf
'            .SelectionColor = Color.Red
'            .SelectedText = "e outros recursos da plataforma .NET" + vbLf
'            .SelectionBullet = False
'            .SelectionFont = New Font("Tahoma", 10)
'            .SelectionColor = Color.Black
'            .SelectedText = "http://www.macoratti.net" & vbLf
'        End With
'    End Sub

'    Private Sub btnImagem_Click(sender As Object, e As EventArgs) Handles btnImagem.Click
'        If OpenFileDialog1.ShowDialog() = DialogResult.OK Then
'            picImagem.Load(OpenFileDialog1.FileName)
'            Clipboard.SetImage(picImagem.Image)
'            picImagem.Image = Nothing
'            Me.RichTextBoxPrintCtrl1.Paste()
'        End If
'    End Sub

'    Private verificaImpressao As Integer

'    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
'        verificaImpressao = 0
'    End Sub

'    Private Sub PrintDocument1_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
'        ' imprime o conteudo do RichTextBox. 
'        ' armazena o ultimo caractere impresso
'        verificaImpressao = RichTextBoxPrintCtrl1.Print(verificaImpressao, RichTextBoxPrintCtrl1.TextLength, e)

'        ' verifica se há mais paginas
'        If verificaImpressao < RichTextBoxPrintCtrl1.TextLength Then
'            e.HasMorePages = True
'        Else
'            e.HasMorePages = False
'        End If
'    End Sub

'    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

'    End Sub
'End Class

'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXConfigurar richtest posição e tamanho

'Private Sub InitializeContextMenu()
'    Dim contextMenu1 As New ContextMenu()
'    Dim menuItem1 As New MenuItem("&Cut")
'    AddHandler menuItem1.Click, AddressOf btnRecort_Click
'    Dim menuItem2 As New MenuItem("&Copy")
'    AddHandler menuItem2.Click, AddressOf btnCopiar_Click
'    Dim menuItem3 As New MenuItem("&Paste")
'    AddHandler menuItem3.Click, AddressOf btnColar_Click
'    contextMenu1.MenuItems.Add(menuItem1)
'    contextMenu1.MenuItems.Add(menuItem2)
'    contextMenu1.MenuItems.Add(menuItem3)
'    RichTextBox1.ContextMenu = contextMenu1
'End Sub

'Private Sub frmResize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Resize
'    If Me.Size.Width > richtextboxsize + 100 Then
'        RichTextBox1.Width = richtextboxsize
'        RichTextBox1.Anchor = AnchorStyles.None
'        RichTextBox1.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom
'        RichTextBox1.Location = New Point((Me.Size.Width - richtextboxsize) / 2, RichTextBox1.Location.Y)

'    End If
'    If Me.Size.Width <= richtextboxsize + 100 Then
'        If RichTextBox1.Location.X < 10 Then
'            RichTextBox1.Location = New Point(10, RichTextBox1.Location.Y)

'        End If
'        If RichTextBox1.Width + 50 > Me.Size.Width Then
'            RichTextBox1.Size = New Point(Me.Size.Width - 20 - RichTextBox1.Location.X, RichTextBox1.Size.Height)
'        End If
'        RichTextBox1.Anchor = AnchorStyles.None
'        RichTextBox1.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
'    End If
'End Sub
'Private Sub sizechange(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.SizeChanged
'    If Me.Size.Width > richtextboxsize + 100 Then
'        RichTextBox1.Width = richtextboxsize
'        RichTextBox1.Anchor = AnchorStyles.None
'        RichTextBox1.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom
'        RichTextBox1.Location = New Point((Me.Size.Width - richtextboxsize) / 2, RichTextBox1.Location.Y)

'    End If
'    If Me.Size.Width <= richtextboxsize + 100 Then
'        If RichTextBox1.Location.X < 10 Then
'            RichTextBox1.Location = New Point(10, RichTextBox1.Location.Y)

'        End If
'        If RichTextBox1.Width + 50 > Me.Size.Width Then
'            RichTextBox1.Size = New Point(Me.Size.Width - 20 - RichTextBox1.Location.X, RichTextBox1.Size.Height)
'        End If
'        RichTextBox1.Anchor = AnchorStyles.None
'        RichTextBox1.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
'    End If
'End Sub
'XXXXXXXXXXXXXXXXXXXXXXXXXXXXX IMPRESSÃO 1 XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
'Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
'    m_PrintDocument = New PrintDocument

'    m_PrintDocument.Print()
'End Sub

'Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
'    m_PrintDocument = New PrintDocument


'    PrintPreviewDialog1.Text = "Usando - PrintPreviewDialog"

'    PrintPreviewDialog1.Document = m_PrintDocument

'    PrintPreviewDialog1.ShowDialog()
'End Sub

'Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
'    m_PrintDocument = New PrintDocument


'    PrintDialog1.Document = m_PrintDocument

'    PrintDialog1.ShowDialog()
'End Sub

'Private Sub RelatorioIS()
'    Dim printIt As New PrintDocument
'    Dim printPreview As New PrintPreviewDialog
'    printPreview.Document = printIt
'    AddHandler printIt.PrintPage, AddressOf PrintDocument1_PrintPage
'End Sub
'Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
'    'Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
'    'Variável Estática para indicar o numero pada linha actual
'    Static Linha_a_Imprimir As Integer = 0
'    ' Desenha um rectângulo à volta do documento
'    Dim rect As New Rectangle(0, 300, 900, 800)
'    e.Graphics.DrawRectangle(Pens.Gray, rect)
'    ' 300 = coordenadasX :: 250 = coordenadasY
'    e.Graphics.DrawString("Numero de Entradas", New Font("Verdana", 20, FontStyle.Bold), Brushes.Black, 300, 250)
'    ' Informações do Fornecedor:
'    With dtgDietaVg.Rows(Linha_a_Imprimir)
'        e.Graphics.DrawString("Data: ", New Font("Verdana", 16, FontStyle.Bold), Brushes.Black, 40, 350)
'        e.Graphics.DrawString(.Cells(1).Value, New Font("Verdana", 15, FontStyle.Regular), Brushes.Black, 175, 351)
'        e.Graphics.DrawString("N_Func: ", New Font("Verdana", 16, FontStyle.Bold), Brushes.Black, 40, 400)
'        e.Graphics.DrawString(.Cells(2).Value, New Font("Verdana", 15, FontStyle.Regular), Brushes.Black, 217, 402)
'        e.Graphics.DrawString("Unidade: ", New Font("Verdana", 16, FontStyle.Bold), Brushes.Black, 40, 450)
'        e.Graphics.DrawString(.Cells(3).Value, New Font("Verdana", 15, FontStyle.Regular), Brushes.Black, 162, 451)
'        'e.Graphics.DrawString("Matricula: ", New Font("Verdana", 16, FontStyle.Bold), Brushes.Black, 40, 560)
'        'e.Graphics.DrawString(.Cells(4).Value, New Font("Verdana", 15, FontStyle.Regular), Brushes.Black, 170, 561)
'        'e.Graphics.DrawString("H_Saída: ", New Font("Verdana", 16, FontStyle.Bold), Brushes.Black, 40, 610)
'        'e.Graphics.DrawString(.Cells(5).Value, New Font("Verdana", 15, FontStyle.Regular), Brushes.Black, 130, 611)
'        'e.Graphics.DrawString("Horário: ", New Font("Verdana", 16, FontStyle.Bold), Brushes.Black, 40, 720)
'        'e.Graphics.DrawString(.Cells(6).Value, New Font("Verdana", 15, FontStyle.Regular), Brushes.Black, 135, 721)
'        ' Data do documento
'        e.Graphics.DrawString(Date.Now, New Font("Verdana", 14, FontStyle.Bold), Brushes.Black, 525, 1120)
'    End With
'    Linha_a_Imprimir += 1
'    e.HasMorePages = (Linha_a_Imprimir < dtgDietaVg.Rows.Count)
'End Sub

'XXXXXXXXXXXXXXXXXXXXXXXXXXXXX IMPRESSÃO 2 XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX


'Private Sub PrintDocument1_PrintPage(sender As Object, e As Printing.PrintPageEventArgs) ' Handles PrintDocument1.PrintPage
'    Dim LinhaAtual As Integer = 0
'    Dim g As Graphics = e.Graphics
'    Dim PosicaoLinha As Integer = 40
'    Dim LinhasPorPagina As Byte = 45
'    Dim total As String = String.Empty
'    Dim grupo As String = String.Empty
'    Dim valor As String = String.Empty
'    Dim unidade As String = String.Empty
'    'Definições da pagina
'    g.PageUnit = GraphicsUnit.Millimeter
'    'Desenhar folha
'    Dim Titulo As New Font("Arial", 4, FontStyle.Regular, GraphicsUnit.Millimeter)
'    g.DrawString("*** Resumo de vendas por Grupo *** ", Titulo, Brushes.Black, 10, 20)

'    For L = 0 To LinhasPorPagina - 1
'        If LinhaAtual = dtgDietaVg.Rows.Count Then Continue For
'        For i As Integer = 0 To dtgDietaVg.ColumnCount - 5

'            If Not dtgDietaVg.Item(i, LinhaAtual).Value = Nothing Then
'                grupo = dtgDietaVg.Item(i, LinhaAtual).Value

'            End If
'        Next
'        For i As Integer = 0 To dtgDietaVg.ColumnCount - 3

'            If Not dtgDietaVg.Item(i, LinhaAtual).Value = Nothing Then
'                unidade = dtgDietaVg.Item(i, LinhaAtual).Value

'            End If
'        Next
'        For i As Integer = 0 To dtgDietaVg.ColumnCount - 4
'            dtgDietaVg.Columns(4).DefaultCellStyle.Format = "C2"
'            If Not dtgDietaVg.Item(i, LinhaAtual).Value = Nothing Then
'                valor = dtgDietaVg.Item(i, LinhaAtual).Value
'            End If
'        Next
'        'Alimento,Qtd,QtdPremix,QtdVagao Total/trato
'        'Desenhar conteudo na página
'        e.Graphics.DrawString("Alimento", New Font("Verdana", 6), Brushes.Black, 20, 30)
'        e.Graphics.DrawString(grupo, New Font("Verdana", 6), Brushes.Black, 20, PosicaoLinha)
'        e.Graphics.DrawString("Qtd", New Font("Verdana", 6), Brushes.Black, 40, 30)
'        e.Graphics.DrawString(unidade, New Font("Verdana", 6), Brushes.Black, 40, PosicaoLinha)
'        e.Graphics.DrawString("QtdVagao", New Font("Verdana", 6), Brushes.Black, 60, 30)
'        e.Graphics.DrawString(valor, New Font("Verdana", 6), Brushes.Black, 60, PosicaoLinha)
'        e.Graphics.DrawString("______________________________________________________________________________________________________________________________", New Font("Verdana", 8), Brushes.Black, 0, 34)
'        PosicaoLinha += 5
'        LinhaAtual += 1
'        valor = String.Empty
'        grupo = String.Empty
'        unidade = String.Empty
'        total = String.Empty
'    Next
'    e.Graphics.DrawString("Totais: ", New Font("Verdana", 6), Brushes.Black, 20, PosicaoLinha)
'    e.Graphics.DrawString(Label48.Text, New Font("Verdana", 6), Brushes.Black, 40, PosicaoLinha)
'    e.Graphics.DrawString(Label56.Text, New Font("Verdana", 6), Brushes.Black, 60, PosicaoLinha)
'End Sub
'Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
'    'RelatorioIS()

'End Sub

'Private Sub TabelaPH()

'    Try

'        Dim dt As New DataTable()

'        dt.Columns.Add("Vaca")
'        dt.Columns.Add("pH")

'        dtgPH.DataSource = dt
'    Catch ex As Exception
'        Throw ex
'    End Try

'End Sub


'Private Sub dtgHistKPS_CellClick(sender As Object, e As DataGridViewCellEventArgs)
'    txtNomeKPS.Text = dtgHistKPS.CurrentRow.Cells(0).Value
'    BuscarKPS()
'    CoresKPS()
'    txtNomeKPS.Enabled = False
'End Sub


'Private Sub dtgKPS_CellClick(sender As Object, e As DataGridViewCellEventArgs)


'    'If txtNomeKPS.Text <> "" Then
'    '    btnSalvarKPS.Enabled = True
'    'End If

'End Sub

'Dim titulo4 As String = ""
'Private Sub Grafico4()

'    Chart4.Series.Clear()
'    Chart4.Titles.Clear()
'    Dim var4 As Double = dtgForragem.Rows(0).Cells(3).Value
'    Dim var42 As Double = dtgForragem.Rows(1).Cells(3).Value
'    Dim var43 As Double = dtgForragem.Rows(2).Cells(3).Value
'    Dim var44 As Double = dtgForragem.Rows(3).Cells(3).Value
'    Dim var45 As Double = dtgForragem.Rows(4).Cells(3).Value
'    titulo4 = txtNomeForragem.Text

'    Dim title = New Title()
'    title.Font = (New Font("Arial", 10, FontStyle.Bold))
'    title.ForeColor = Color.Black
'    title.Text = titulo3
'    Chart4.Titles.Add(title)


'    Chart4.Series.Add("Forragem")

'    On Error Resume Next
'    With Chart4.Series("Forragem")

'        'define o tipo de gráfico
'        .ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column
'        .BorderWidth = 2
'        'define o titulo do gráfico
'        ' .Titles.Add("Ali")
'        .Palette = ChartColorPalette.BrightPastel
'        .Points.AddXY("P1", var4)
'        .Points.AddXY("P2", var42)
'        .Points.AddXY("P3", var43)
'        '.Points.AddXY("P4", var44)
'        .Points.AddXY("Fd", var45)

'        'Tamanho
'        '.Size = New Size(Size.Width, 250)
'        ' .Size = New Size(Size.Height, 165)

'    End With
'    Chart4.DataBind()

'End Sub

'Dim titulo5 As String = ""
'Private Sub Grafico5()

'    Chart5.Series.Clear()
'    Chart5.Titles.Clear()
'    Dim var5 As Double = dtgForragem.Rows(0).Cells(3).Value
'    Dim var52 As Double = dtgForragem.Rows(1).Cells(3).Value
'    Dim var53 As Double = dtgForragem.Rows(2).Cells(3).Value
'    Dim var54 As Double = dtgForragem.Rows(3).Cells(3).Value
'    Dim var55 As Double = dtgForragem.Rows(4).Cells(3).Value
'    titulo5 = txtNomeForragem.Text

'    Dim title = New Title()
'    title.Font = (New Font("Arial", 10, FontStyle.Bold))
'    title.ForeColor = Color.Black
'    title.Text = titulo3
'    Chart5.Titles.Add(title)


'    Chart5.Series.Add("Forragem")

'    On Error Resume Next
'    With Chart5.Series("Forragem")

'        'define o tipo de gráfico
'        .ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column
'        .BorderWidth = 2
'        'define o titulo do gráfico
'        ' .Titles.Add("Ali")
'        .Palette = ChartColorPalette.BrightPastel
'        .Points.AddXY("P1", var5)
'        .Points.AddXY("P2", var52)
'        .Points.AddXY("P3", var53)
'        '.Points.AddXY("P4", var54)
'        .Points.AddXY("Fd", var55)

'        'Tamanho
'        '.Size = New Size(Size.Width, 250)
'        ' .Size = New Size(Size.Height, 165)

'    End With
'    Chart5.DataBind()

'End Sub

'Dim titulo6 As String = ""
'Private Sub Grafico6()

'    Chart6.Series.Clear()
'    Chart6.Titles.Clear()
'    Dim var6 As Double = dtgForragem.Rows(0).Cells(3).Value
'    Dim var62 As Double = dtgForragem.Rows(1).Cells(3).Value
'    Dim var63 As Double = dtgForragem.Rows(2).Cells(3).Value
'    Dim var64 As Double = dtgForragem.Rows(3).Cells(3).Value
'    Dim var65 As Double = dtgForragem.Rows(4).Cells(3).Value
'    titulo6 = txtNomeForragem.Text

'    Dim title = New Title()
'    title.Font = (New Font("Arial", 10, FontStyle.Bold))
'    title.ForeColor = Color.Black
'    title.Text = titulo3
'    Chart6.Titles.Add(title)


'    Chart6.Series.Add("Forragem")

'    On Error Resume Next
'    With Chart6.Series("Forragem")

'        'define o tipo de gráfico
'        .ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column
'        .BorderWidth = 2
'        'define o titulo do gráfico
'        ' .Titles.Add("Ali")
'        .Palette = ChartColorPalette.BrightPastel
'        .Points.AddXY("P1", var6)
'        .Points.AddXY("P2", var62)
'        .Points.AddXY("P3", var63)
'        '.Points.AddXY("P4", var64)
'        .Points.AddXY("Fd", var65)

'        'Tamanho
'        '.Size = New Size(Size.Width, 250)
'        ' .Size = New Size(Size.Height, 165)

'    End With
'    Chart6.DataBind()

'End Sub







'Private Sub CadastrarrForragem()

'    'Nome           
'    'QtdPeneiras    
'    'Peneira        
'    'Tamanho        
'    'Quantidade     
'    'PorcPorPeneira 
'    'Acima8mm       
'    'IdPropriedade  

'    Dim sql As String
'    Dim cmd As sqliteCommand
'    Dim data As String

'    'Formatando a data para o padrão aa/mm/dd
'  

'    If txtNomeFazenda.Text <> "" Or txtProdutor.Text <> "" Then

'        Try

'            abrir()

'            sql = "Insert into Forragem (Fazenda,Produtor,Municipio,Estado,Localizacao,Tecnico,Fone,DataNascimento,Foto) values (@Fazenda,@Produtor,@Municipio,@Estado,@Localizacao,@Tecnico,@Fone,@DataNascimento,@Foto)"
'            cmd = New sqliteCommand(sql, con)
'            cmd.Parameters.AddWithValue("@Fazenda", txtNomeFazenda.Text)
'            cmd.Parameters.AddWithValue("@Produtor", txtProdutor.Text)
'            cmd.Parameters.AddWithValue("@Municipio", txtMunicipioFazenda.Text)
'            cmd.Parameters.AddWithValue("@Estado", txtEstadoFazenda.Text)
'            cmd.Parameters.AddWithValue("@Localizacao", txtLocalizacaoFazenda.Text)
'            cmd.Parameters.AddWithValue("@Tecnico", txtTecRespFazenda.Text)
'            cmd.Parameters.AddWithValue("@Fone", txtFone.Text)
'            cmd.Parameters.AddWithValue("@DataNascimento", txtNascimento.Text)
'            cmd.Parameters.AddWithValue("@Foto", txtFoto.Text)

'            cmd.ExecuteNonQuery()
'            MsgBox("Cliente cadastrado com sucesso!")
'        Catch ex As Exception
'            MsgBox("Erro ao salvar!" + ex.Message)
'            fechar()
'        End Try

'        'LimparCamposFazenda()
'        'btnCadastrarFazenda.Enabled = False
'        'btnSalvarFazenda.Enabled = False
'        'btnEditarCliente.Enabled = False
'        'btnExcluirCliente.Enabled = False
'        'BuscarFazenda()
'    Else
'        MsgBox("Preencha os campos!")

'    End If

'End Sub



'Private Sub ConfigAmostras() ' Trato
'    On Error Resume Next

'    With Me.dtgAmostras
'        If cbxQtdAmostras.Text = "05" Then
'            .ColumnHeadersDefaultCellStyle.BackColor = Color.Gainsboro
'            .ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Regular)
'            .DefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Regular)

'            .Columns(0).Width = 120
'            .Columns(1).Width = 70
'            .Columns(2).Width = 35
'            .Columns(3).Width = 35
'            .Columns(4).Width = 35
'            .Columns(5).Width = 35
'            .Columns(6).Visible = False
'            .Columns(7).Visible = False
'            .Columns(8).Visible = False
'            .Columns(9).Visible = False
'            .Columns(10).Visible = False
'            .Columns(11).Width = 70
'            .Columns(12).Width = 70

'        ElseIf cbxQtdAmostras.Text = "06" Then
'            .ColumnHeadersDefaultCellStyle.BackColor = Color.Gainsboro
'            .ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Regular)
'            .DefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Regular)

'            .Columns(0).HeaderText = "Peneira"
'            .Columns(1).HeaderText = "01"
'            .Columns(2).HeaderText = "02"
'            .Columns(3).HeaderText = "03"
'            .Columns(4).HeaderText = "04"
'            .Columns(5).HeaderText = "05"
'            .Columns(6).HeaderText = "06"
'            .Columns(7).HeaderText = "07"
'            .Columns(8).HeaderText = "08"
'            .Columns(9).HeaderText = "09"
'            .Columns(10).HeaderText = "10"
'            .Columns(11).HeaderText = "Média"
'            .Columns(12).HeaderText = "CV%"

'            .Columns(0).Width = 80
'            .Columns(1).Width = 35
'            .Columns(2).Width = 35
'            .Columns(3).Width = 35
'            .Columns(4).Width = 35
'            .Columns(5).Width = 35
'            .Columns(6).Width = 35
'            .Columns(7).Visible = False
'            .Columns(8).Visible = False
'            .Columns(9).Visible = False
'            .Columns(10).Visible = False
'            .Columns(11).Width = 70
'            .Columns(12).Width = 70

'        ElseIf cbxQtdAmostras.Text = "07" Then
'            .ColumnHeadersDefaultCellStyle.BackColor = Color.Gainsboro
'            .ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Regular)
'            .DefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Regular)

'            .Columns(0).HeaderText = "Peneira"
'            .Columns(1).HeaderText = "01"
'            .Columns(2).HeaderText = "02"
'            .Columns(3).HeaderText = "03"
'            .Columns(4).HeaderText = "04"
'            .Columns(5).HeaderText = "05"
'            .Columns(6).HeaderText = "06"
'            .Columns(7).HeaderText = "07"
'            .Columns(8).HeaderText = "08"
'            .Columns(9).HeaderText = "09"
'            .Columns(10).HeaderText = "10"
'            .Columns(11).HeaderText = "Média"
'            .Columns(12).HeaderText = "CV%"

'            .Columns(0).Width = 80
'            .Columns(1).Width = 35
'            .Columns(2).Width = 35
'            .Columns(3).Width = 35
'            .Columns(4).Width = 35
'            .Columns(5).Width = 35
'            .Columns(6).Width = 35
'            .Columns(7).Width = 35
'            .Columns(8).Visible = False
'            .Columns(9).Visible = False
'            .Columns(10).Visible = False
'            .Columns(11).Width = 70
'            .Columns(12).Width = 70

'        ElseIf cbxQtdAmostras.Text = "08" Then
'            .ColumnHeadersDefaultCellStyle.BackColor = Color.Gainsboro
'            .ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Regular)
'            .DefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Regular)

'            .Columns(0).HeaderText = "Peneira"
'            .Columns(1).HeaderText = "01"
'            .Columns(2).HeaderText = "02"
'            .Columns(3).HeaderText = "03"
'            .Columns(4).HeaderText = "04"
'            .Columns(5).HeaderText = "05"
'            .Columns(6).HeaderText = "06"
'            .Columns(7).HeaderText = "07"
'            .Columns(8).HeaderText = "08"
'            .Columns(9).HeaderText = "09"
'            .Columns(10).HeaderText = "10"
'            .Columns(11).HeaderText = "Média"
'            .Columns(12).HeaderText = "CV%"

'            .Columns(0).Width = 80
'            .Columns(1).Width = 35
'            .Columns(2).Width = 35
'            .Columns(3).Width = 35
'            .Columns(4).Width = 35
'            .Columns(5).Width = 35
'            .Columns(6).Width = 35
'            .Columns(7).Width = 35
'            .Columns(8).Width = 35
'            .Columns(9).Visible = False
'            .Columns(10).Visible = False
'            .Columns(11).Width = 70
'            .Columns(12).Width = 70

'        ElseIf cbxQtdAmostras.Text = "09" Then
'            .ColumnHeadersDefaultCellStyle.BackColor = Color.Gainsboro
'            .ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Regular)
'            .DefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Regular)

'            .Columns(0).HeaderText = "Peneira"
'            .Columns(1).HeaderText = "01"
'            .Columns(2).HeaderText = "02"
'            .Columns(3).HeaderText = "03"
'            .Columns(4).HeaderText = "04"
'            .Columns(5).HeaderText = "05"
'            .Columns(6).HeaderText = "06"
'            .Columns(7).HeaderText = "07"
'            .Columns(8).HeaderText = "08"
'            .Columns(9).HeaderText = "09"
'            .Columns(10).HeaderText = "10"
'            .Columns(11).HeaderText = "Média"
'            .Columns(12).HeaderText = "CV%"

'            .Columns(0).Width = 80
'            .Columns(1).Width = 35
'            .Columns(2).Width = 35
'            .Columns(3).Width = 35
'            .Columns(4).Width = 35
'            .Columns(5).Width = 35
'            .Columns(6).Width = 35
'            .Columns(7).Width = 35
'            .Columns(8).Width = 35
'            .Columns(9).Width = 35
'            .Columns(10).Visible = False
'            .Columns(11).Width = 70
'            .Columns(12).Width = 70


'        ElseIf cbxQtdAmostras.Text = "10" Then
'            .ColumnHeadersDefaultCellStyle.BackColor = Color.Gainsboro
'            .ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Regular)
'            .DefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Regular)

'            .Columns(0).HeaderText = "Peneira"
'            .Columns(1).HeaderText = "01"
'            .Columns(2).HeaderText = "02"
'            .Columns(3).HeaderText = "03"
'            .Columns(4).HeaderText = "04"
'            .Columns(5).HeaderText = "05"
'            .Columns(6).HeaderText = "06"
'            .Columns(7).HeaderText = "07"
'            .Columns(8).HeaderText = "08"
'            .Columns(9).HeaderText = "09"
'            .Columns(10).HeaderText = "10"
'            .Columns(11).HeaderText = "Média"
'            .Columns(12).HeaderText = "CV%"

'            .Columns(0).Width = 80
'            .Columns(1).Width = 35
'            .Columns(2).Width = 35
'            .Columns(3).Width = 35
'            .Columns(4).Width = 35
'            .Columns(5).Width = 35
'            .Columns(6).Width = 35
'            .Columns(7).Width = 35
'            .Columns(8).Width = 35
'            .Columns(9).Width = 35
'            .Columns(10).Width = 35
'            .Columns(11).Width = 70
'            .Columns(12).Width = 70
'            '.Columns(13).Visible = False

'        End If

'    End With

'End Sub