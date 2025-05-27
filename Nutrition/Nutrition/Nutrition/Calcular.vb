Module Calcular

    Public v_id As Integer
    Public v_Tipo As String
    Public almntoFamilia As String
    Public almnto As String
    Public v_MS As Decimal
    Public v_PB As Decimal
    Public v_PDR As Decimal
    Public v_PND As Decimal
    Public v_FDN As Decimal
    Public v_eFDN As Decimal
    Public v_MOR As Decimal
    Public v_MNmaior8 As Decimal
    Public v_MNmaior19 As Decimal
    Public v_FDNF As Decimal
    Public v_FDA As Decimal
    Public v_Nel As Decimal
    Public v_NDT As Decimal
    Public v_EE As Decimal
    Public v_EE_Insat As Decimal
    Public v_Cinzas As Decimal
    Public v_CNF As Decimal
    Public v_Amido As Decimal
    Public v_kd_Amid As Decimal
    Public v_Ca As Decimal
    Public v_P As Decimal
    Public v_Mg As Decimal
    Public v_K As Decimal
    Public v_S As Decimal
    Public v_Na As Decimal
    Public v_Cl As Decimal
    Public v_Co As Decimal
    Public v_Cu As Decimal
    Public v_Mn As Decimal
    Public v_Zn As Decimal
    Public v_Se As Decimal
    Public v_I As Decimal
    Public v_A As Decimal
    Public v_D As Decimal
    Public v_E As Decimal
    Public v_Cromo As Decimal
    Public v_DCAD As Decimal
    Public v_Biotina As Decimal
    Public v_Virginiamicina As Decimal
    Public v_Monensina As Decimal
    Public v_Levedura As Decimal
    Public v_Arginina As Decimal
    Public v_Histidina As Decimal
    Public v_Isoleucina As Decimal
    Public v_Leucina As Decimal
    Public v_Lisina As Decimal
    Public v_Metionina As Decimal
    Public v_Fenilalanina As Decimal
    Public v_Treonina As Decimal
    Public v_Triptofano As Decimal
    Public v_Valina As Decimal
    Public v_dFDNp48h As Decimal
    Public v_dAmido_7h As Decimal
    Public v_TTNDFD As Decimal

    Public v_Pers1 As Decimal
    Public v_Pers2 As Decimal
    Public v_Pers3 As Decimal
    Public v_Pers4 As Decimal
    Public v_Pers5 As Decimal
    Public v_Pers6 As Decimal
    Public v_Pers7 As Decimal
    Public v_Pers8 As Decimal
    Public v_Pers9 As Decimal
    Public v_Pers10 As Decimal
    Public v_Pers11 As Decimal
    Public v_Pers12 As Decimal
    Public v_Pers13 As Decimal

    Public v_EFDN2 As Decimal
    'xxxxxxxxxxxxxx

    Public totalMS As Decimal
    Public totalPB As Decimal
    Public totalPDR As Decimal
    Public totalPND As Decimal
    Public totalFDN As Decimal
    Public totaleFDN As Decimal
    Public totaleFDN2 As Decimal
    Public totalFDNF As Decimal
    Public totalFDA As Decimal
    Public totalNEl As Decimal
    Public totalNDT As Decimal
    Public totalEE As Decimal
    Public totalCinzas As Decimal
    Public totalCNF As Decimal
    Public totalAmido As Decimal
    Public totalMor As Decimal
    Public totalCa As Decimal
    Public totalP As Decimal
    Public totalMg As Decimal
    Public totalK As Decimal
    Public totalS As Decimal
    Public totalNa As Decimal
    Public totalCl As Decimal
    Public totalCo As Decimal
    Public totalCu As Decimal
    Public totalMn As Decimal
    Public totalZn As Decimal
    Public totalSe As Decimal
    Public totalI As Decimal
    Public totalA As Decimal
    Public totalD As Decimal
    Public totalE As Decimal
    Public totalCromo As Decimal
    Public totalDCAD As Decimal
    Public totalBiotina As Decimal
    Public totalVirginiamicina As Decimal
    Public totalMonensina As Decimal
    Public totalLevedura As Decimal
    Public totalArginina As Decimal
    Public totalHistidina As Decimal
    Public totalIsoleucina As Decimal
    Public totalLeucina As Decimal
    Public totalLisina As Decimal
    Public totalMetionina As Decimal
    Public totalFenilalanina As Decimal
    Public totalTreonina As Decimal
    Public totalTriptofano As Decimal
    Public totalValina As Decimal
    Public totalTTNDFD As Decimal
    Public totalEstimatina_prd_leite_EL As Decimal
    Public totalEstimatina_prd_leite_EL_Lactose As Decimal
    Public totalFator_Correcao_FL As Decimal
    Public totalMNat As Decimal
    Public totalMNmaior8 As Decimal
    Public totalMNmaior19 As Decimal
    Public totalEE_Insat As Decimal
    Public totalkd_Amid As Decimal
    Public totaldFDNp48h As Decimal
    Public totaldAmido7h As Decimal

    Public totalPers1 As Decimal
    Public totalPers2 As Decimal
    Public totalPers3 As Decimal
    Public totalPers4 As Decimal
    Public totalPers5 As Decimal
    Public totalPers6 As Decimal
    Public totalPers7 As Decimal
    Public totalPers8 As Decimal
    Public totalPers9 As Decimal
    Public totalPers10 As Decimal
    Public totalPers11 As Decimal
    Public totalPers12 As Decimal
    Public totalPers13 As Decimal
    'xxxxxxxxxxxxxxxxxx


    Public somaProduto As Double = 0
    Public mn8AmiDR As Double = 0
    Public mn8PV As Double = 0
    Public dfnfPV As Double = 0
    Public forragem As Double = 0
    Public concentrado As Double = 0
    Public dcad As Double = 0
    Public consumo As Double = 0
    Public caP As Double = 0
    Public lysMet As Double = 0
    Public enerProdLeite As Double = 0
    Public ProtPrudLeite As Double = 0
    Public amiDR As Double = 0
    Public kc As Double = 0
    Public msConc As Double = 0
    Public msVol As Double = 0
    Public kcVol As Double = 0
    Public kcConc As Double = 0
    Public qtdAmid As Double = 0
    Public mnMaiorq8Dieta As Double = 0
    Public msConc1 As Double = 0
    Public qamidr As Double = 0
    Public qtdNel As Double = 0
    Public estimatLeiteLact As Double
    Public estimatLeite As Double
    Public kgTotalMS As Double = 0


    Public pb_ms As Decimal
    Public pnd_ms As Decimal
    Public pcms_pnd As Decimal
    Public msLactacao As Double
    Public msBezerra As Double
    Public msNovilha As Double
    Public msVacaSeca As Double
    Public msPreParto As Double

    'Variáveis para cadastro de alimentos personalizáveis
    Public Pers1 As String
    Public Pers2 As String
    Public Pers3 As String
    Public Pers4 As String
    Public Pers5 As String
    Public Pers6 As String
    Public Pers7 As String
    Public Pers8 As String
    Public Pers9 As String
    Public Pers10 As String
    Public Pers11 As String
    Public Pers12 As String
    Public Pers13 As String

    Public nomeFaz As String
    Public idFaz As String
    Public idDieta As String

    Public MSa() As Decimal = {0} ' 20}
    Public PBa() As Decimal = {0} ' 20}
    Public PDRa() As Decimal = {0} ' 20}
    Public PNDa() As Decimal = {0} ' 20}
    Public FDNa() As Decimal = {0} ' 20}
    Public eFDNa() As Decimal = {0} ' 20}
    Public eFDN2a() As Decimal = {0} ' 20}
    Public MN8a() As Decimal = {0} ' 20}
    Public MN19a() As Decimal = {0} ' 20}
    Public FDNFa() As Decimal = {0} ' 20}
    Public FDAa() As Decimal = {0} ' 20}
    Public NEla() As Decimal = {0} ' 20}
    Public NDTa() As Decimal = {0} ' 20}
    Public EEa() As Decimal = {0} ' 20}
    Public EEInsata() As Decimal = {0} ' 20}
    Public Cinzasa() As Decimal = {0} ' 20}
    Public CNFa() As Decimal = {0} ' 20}
    Public Amidoa() As Decimal = {0} ' 20}
    Public kdAmida() As Decimal = {0} ' 20}
    Public Caa() As Decimal = {0} ' 20}
    Public Pa() As Decimal = {0} ' 20}
    Public Mga() As Decimal = {0} ' 20}
    Public Ka() As Decimal = {0} ' 20}
    Public Sa() As Decimal = {0} ' 20}
    Public Naa() As Decimal = {0} ' 20}
    Public Cla() As Decimal = {0} ' 20}
    Public Coa() As Decimal = {0} ' 20}
    Public Cua() As Decimal = {0} ' 20}
    Public Mna() As Decimal = {0} ' 20}
    Public Zna() As Decimal = {0} ' 20}
    Public Sea() As Decimal = {0} ' 20}
    Public Ia() As Decimal = {0} ' 20}
    Public Aa() As Decimal = {0} ' 20}
    Public Da() As Decimal = {0} ' 20}
    Public Ea() As Decimal = {0} ' 20}
    Public Cromoa() As Decimal = {0} ' 20}
    Public Biotinaa() As Decimal = {0} ' 20}
    Public Virginiamicinaa() As Decimal = {0} ' 20}
    Public Monensinaa() As Decimal = {0} ' 20}
    Public Leveduraa() As Decimal = {0} ' 20}
    Public Lisinaa() As Decimal = {0} ' 20}
    Public Metioninaa() As Decimal = {0} ' 20}
    Public dFDNp48ha() As Decimal = {0} ' 20}
    Public dAmido7ha() As Decimal = {0} ' 20}
    Public Pers1a() As Decimal = {0} ' 20}
    Public Pers2a() As Decimal = {0} ' 20}
    Public Pers3a() As Decimal = {0} ' 20}
    Public Pers4a() As Decimal = {0} ' 20}
    Public Pers5a() As Decimal = {0} ' 20}
    Public Pers6a() As Decimal = {0} ' 20}
    Public Pers7a() As Decimal = {0} ' 20}
    Public Pers8a() As Decimal = {0} ' 20}
    Public Pers9a() As Decimal = {0} ' 20}
    Public Pers10a() As Decimal = {0} ' 20}
    Public FDNAmiDRa() As Decimal = {0} ' 20}
    Public FDNPVa() As Decimal = {0} ' 20}
    Public FDNFPVa() As Decimal = {0} ' 20}
    Public Forragema() As Decimal = {0} ' 20}
    Public Concentradoa() As Decimal = {0} ' 20}
    Public Dcada() As Decimal = {0} ' 20}
    Public Consumoa() As Decimal = {0} ' 20}
    Public RelLeiteConcentradoa() As Decimal = {0} ' 20}
    Public RelLeiteConsumoa() As Decimal = {0} ' 20}
    Public LysMeta() As Decimal = {0} ' 20}
    Public Energiaprodleitea() As Decimal = {0} ' 20}
    Public Protprodleitea() As Decimal = {0} ' 20}
    'Variáveis para os avaliadores

    Public pctMS As Double
    Public pctPB As Double
    Public pctPDR As Double
    Public pctPND As Double
    Public pctFDN As Double
    Public pcteFDN As Double
    Public pcteFDN2 As Double
    Public pctMNmaior8 As Double
    Public pctMNmaior19 As Double
    Public pctFDNF As Double
    Public pctFDA As Double
    Public pctNel As Double
    Public pctNDT As Double
    Public pctEE As Double
    Public pctEE_Insat As Double
    Public pctCinzas As Double
    Public pctCNF As Double
    Public pctAmido As Double
    Public pctkd_Amid As Double
    Public pctMor As Double
    Public pctCa As Double
    Public pctP As Double
    Public pctMg As Double
    Public pctK As Double
    Public pctS As Double
    Public pctNa As Double
    Public pctCl As Double
    Public pctCo As Double
    Public pctCu As Double
    Public pctMn As Double
    Public pctZn As Double
    Public pctSe As Double
    Public pctI As Double
    Public pctA As Double
    Public pctD As Double
    Public pctE As Double
    Public pctCromo As Double

    Public pctDCAD As Double

    Public pctBiotina As Double
    Public pctVirginiamicina As Double
    Public pctMonensina As Double
    Public pctLevedura As Double

    Public pctArginina As Decimal
    Public pctHistidina As Decimal
    Public pctIsoleucina As Decimal
    Public pctLeucina As Decimal

    Public pctLisina As Double
    Public pctMetionina As Double

    Public pctFenilalanina As Decimal
    Public pctTreonina As Decimal
    Public pctTriptofano As Decimal
    Public pctValina As Decimal
    Public pctTTNDFD As Decimal

    Public pctdFDNp48h As Double
    Public pctdAmido7h As Double

    Public pctPers1 As Double = 0
    Public pctPers2 As Double = 0
    Public pctPers3 As Double = 0
    Public pctPers4 As Double = 0
    Public pctPers5 As Double = 0
    Public pctPers6 As Double = 0
    Public pctPers7 As Double = 0
    Public pctPers8 As Double = 0
    Public pctPers9 As Double = 0
    Public pctPers10 As Double = 0
    Public pctPers11 As Double = 0
    Public pctPers12 As Double = 0
    Public pctPers13 As Double = 0
    Public qtdProdutoPremix As Double = 0
    Public valorPremix As Double = 0

    Public bezr As String
    Public nov As String
    Public sec As String
    Public prep As String
    Public lac As String
    Public nvo As String


    Public qtdKgMs As Double

    Public edtAlim As Boolean


End Module
