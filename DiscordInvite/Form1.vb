Imports System.Net
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Net.WebClient
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports System.Web.Script.Serialization
Imports System.Text
Imports System.Web
Imports System.Threading
Public Class Form1
    Dim TxtId As Integer = 0
    Dim StartId As Integer = 0
    Dim UsrAgId As Integer = 0
    Dim Element As New ListViewItem
    Dim IdRegex As New Regex("https://discordapp.com/invite/.......")
    Dim ProxyIp, ProxyPort, JsonTxt As String
  
    Public DisCodeList As New List(Of String)
    Public UsrAgList As New List(Of String)
    Dim Good, Bad, Restant, Loaded, TestedP As Integer


    Private Downloader As New WebClient
    Dim WebDl As HttpWebRequest
    'Dim request As HttpWebRequest


    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        UserAgents()
        Control.CheckForIllegalCrossThreadCalls = False
    End Sub
    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles DiscordButton1.Click
        BackgroundWorker2.RunWorkerAsync()
        DiscordButton1.Enabled = False
        DiscordTextBox1.Enabled = False
        DiscordButton13.Enabled = False
        NumericUpDown1.Enabled = False
        DiscordCheckBox1.Enabled = False

        ListView1.Items.Clear()
        DisCodeList.Clear()
        JsonTxt = Nothing
        TxtId = 0
        Dim StartId As Integer = 0
        Search()

    End Sub



    Private Sub Search()




    End Sub

    Private Sub ListView1_MouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ListView1.MouseClick
        Dim lvi As ListViewHitTestInfo = ListView1.HitTest(e.X, e.Y)

        My.Computer.Clipboard.SetText(lvi.SubItem.Text)
        MsgBox("The selected element vas copied to clipboard." + vbNewLine + ">    " + My.Computer.Clipboard.GetText)
    End Sub



    Private Sub ParseJson()

        Dim DSerializer As New JavaScriptSerializer
        Dim ResultDisctionnary As Dictionary(Of String, Object) = DSerializer.DeserializeObject(JsonTxt)



        Dim InviterDictionnary As Dictionary(Of String, Object) = ResultDisctionnary.Item("inviter")

        MsgBox(InviterDictionnary.Item("username"))
    End Sub


    Private Sub UserAgents()
        UsrAgList.Clear()

        Dim Usrfile As System.IO.StreamWriter
        Usrfile = My.Computer.FileSystem.OpenTextFileWriter(Application.StartupPath & "\UsrAgentList.txt", True)
        Usrfile.WriteLine(Downloader.DownloadString("https://pastebin.com/raw/5hTAHVay").ToString)
        Usrfile.Close()


        Dim reader() As String = IO.File.ReadAllLines(Application.StartupPath & "\UsrAgentList.txt")

        For x As Integer = 0 To reader.GetUpperBound(0)

            UsrAgList.Add(reader(x))
        Next

    End Sub

    Private Sub OpenUsrAgents()
        Dim OFD As New OpenFileDialog
        OFD.Title = "Please select an user agent list"

        OFD.Filter = "Text file|*.Txt"


        If OFD.ShowDialog() = DialogResult.OK Then

            Dim Usrfile As System.IO.StreamWriter
            Usrfile = My.Computer.FileSystem.OpenTextFileWriter(Application.StartupPath & "\UsrAgentList.txt", True)
            Usrfile.WriteLine(Downloader.DownloadString("https://pastebin.com/raw/5hTAHVay"))
            Usrfile.Close()


            Dim lines() As String = IO.File.ReadAllLines(OFD.FileName)

            For x As Integer = 0 To lines.GetUpperBound(0)
                UsrAgList.Add(lines(x))
            Next

        End If



    End Sub






    Private Sub DblString()



        Dim i As Integer = 0
        While (i < ListView1.Items.Count)
            RemoveListViewLine(i, ListView1.Items(i).SubItems(1).Text, ListView1.Items(i).SubItems(4).Text)
            i = i + 1
        End While


        For Each item As ListViewItem In ListView1.Items
            If item.SubItems(4) Is Nothing Or item.SubItems(4).Text = "" Then

                ListView1.Items.Remove(item)
                ToolStripStatusLabel2.Text = "Invalid item removed !"

            End If

        Next

    End Sub
    Private Sub RemoveListViewLine(ByVal n As Integer, ByVal TextCrit As String, ByVal SubCrit As String)
        Dim li As ListViewItem
        n = n + 1
        While (n < ListView1.Items.Count)
            li = ListView1.Items(n)
            If li.SubItems(1).Text = TextCrit And li.SubItems(4).Text = SubCrit Then
                ListView1.Items.Remove(li)
            Else
                n = n + 1
            End If
        End While
    End Sub






    Private Sub PictureBox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click
        Dim MsgBoxChoice As DialogResult
        MsgBoxChoice = MsgBox("Are you sure you want to leave ? ", vbYesNo, "Exit application...")
        If MsgBoxChoice = vbYes Then
            End
        Else

        End If
    End Sub



    Private Sub CurrentIp()
        Try
            Dim CurrIp As String = Downloader.DownloadString(New Uri("http://checkip.dyndns.org/"))
            CurrIp = CurrIp.Split(":").Last
            CurrIp = CurrIp.Split("</b").First
            'DiscordLabel1.Text = "Current IP Address: " + CurrIp
        Catch ex As Exception
        End Try
    End Sub



    Private Sub DiscordButton21_Click(sender As Object, e As EventArgs) Handles DiscordButton21.Click
        Dim OFD As New OpenFileDialog
        OFD.Title = "Please select an proxy list"

        OFD.Filter = "Text file|*.Txt"


        If OFD.ShowDialog() = DialogResult.OK Then

            Dim lines() As String = IO.File.ReadAllLines(OFD.FileName)

            For x As Integer = 0 To lines.GetUpperBound(0)
                ListBox1.Items.Add(lines(x))
            Next

        End If
    End Sub

    Private Sub BackgroundWorker2_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker2.DoWork



        DiscordButton13.Text = "Export all Results"

            StartId = 0
            ToolStripStatusLabel2.Text = "Connection test..."

            'Credentials
            'WebDl.Proxy.Credentials = New NetworkCredential("username", "password", "domain")


            If DiscordCheckBox1.Checked = True Then


            If ListBox3.Items.Count = 0 Then
                MsgBox("You don't have any working proxy to use.", vbInformation)
            Else

                Dim proxyline As String = ListBox3.Items.Item(UsrAgId)

                Dim AProxyName As String = proxyline.Split(":").First
                    Dim AProxyPort As Integer = proxyline.Split(":").Last

                    Downloader.Proxy = New WebProxy(AProxyName, AProxyPort)
                End If

            End If
            'Downloader.Headers("User-Agent") = TextBox1.Text
            Dim Rndm As New Random



            StartId = NumericUpDown1.Value
            For x As Integer = 0 To StartId / 10
                Downloader.Headers("User-Agent") = UsrAgList.Item(Rndm.Next(UsrAgList.Count))
                'WebDl.UserAgent = UsrAgList.Item(StartId / 10)

                ToolStripStatusLabel1.Text = "Using:  " + Downloader.Headers("User-Agent").ToString



                Dim GContent As String






            On Error Resume Next

            If DiscordTextBox1.Text = Nothing Or DiscordTextBox1.Text = "" Then
                On Error Resume Next
                'Too many requests 429
                GContent = Downloader.DownloadString("https://www.google.com/search?q=site:discordapp.com/invite&start=" & StartId.ToString)
                'GContent = Downloader.DownloadString("https://www.google.com/search?q=site:discordapp.com/invite")
                ' request = HttpWebRequest.Create("https://www.google.com/search?q=site:discordapp.com/invite")
                ' request.Proxy = myproxy
                ' request.Timeout = 8000


            Else

                Dim CustomDurl As String = "https://www.google.com/search?q=site:discordapp.com/invite+intitle:" + DiscordTextBox1.Text + "&start=" & StartId.ToString

                    'Too many requests 429
                    GContent = Downloader.DownloadString(CustomDurl)

            End If




            ' Dim response As HttpWebResponse = request.GetResponse

            ' Dim reader As StreamReader
            ' reader = New StreamReader(response.GetResponseStream())

            ' Dim rawresp As String
            ' rawresp = reader.ReadToEnd()

            ToolStripStatusLabel2.Text = "Parsing invite codes..."

            Dim ServerId As String
                For Each m As Match In IdRegex.Matches(GContent.ToString)
                    Threading.Thread.Sleep(10)
                    ServerId = m.Value.Split("/").Last
                    DisCodeList.Add(ServerId)

            Next

            StartId = StartId + 10
            Next
        BackgroundWorker3.RunWorkerAsync()
        BackgroundWorker2.WorkerSupportsCancellation = True
        BackgroundWorker2.CancelAsync()

    End Sub

    Private Sub BackgroundWorker3_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker3.DoWork



        Threading.Thread.Sleep(10)
            Dim DSerializer As New JavaScriptSerializer


            For m As Integer = 0 To DisCodeList.Count
                ToolStripStatusLabel2.Text = "Adding collected items..."

            Dim LContent As String
                LContent = Nothing


            On Error Resume Next
            LContent = Downloader.DownloadString("https://discordapp.com/api/v6/invites/" & DisCodeList.Item(TxtId))

                TxtId += 1


                JsonTxt = Nothing

                JsonTxt = LContent.ToString


                Dim ResultDisctionnary As Dictionary(Of String, Object) = DSerializer.DeserializeObject(JsonTxt)



                Dim GuildDictionnary As Dictionary(Of String, Object) = ResultDisctionnary.Item("guild")
                Dim InviterDictionnary As Dictionary(Of String, Object) = ResultDisctionnary.Item("inviter")


                Dim InviterName As String = InviterDictionnary.Item("username")
                Dim InviterHTag As String = InviterDictionnary.Item("discriminator")
                Dim ServerName As String = GuildDictionnary.Item("name")




                ListView1.Items.Add(New ListViewItem({"https://discordapp.com/invite/" + DisCodeList.Item(m).ToString, DisCodeList.Item(m).ToString, ServerName, InviterName + "#" + InviterHTag, LContent}))
                DblString()


            Next




            ToolStripStatusLabel1.Text = "Items: " & ListView1.Items.Count
            DiscordButton13.Text = "Export all " & ListView1.Items.Count & " Results"
            ToolStripStatusLabel2.Text = "Operation Fisnished."
            DiscordButton1.Enabled = True
            DiscordTextBox1.Enabled = True
            DiscordButton13.Enabled = True
            NumericUpDown1.Enabled = True
            DiscordCheckBox1.Enabled = True
            MsgBox("Operation finished, " & ListView1.Items.Count & " items found!")






    End Sub

    Private Sub DiscordButton27_Click(sender As Object, e As EventArgs) Handles DiscordButton27.Click
        My.Computer.Clipboard.SetText(RichTextBox1.Text)
        MsgBox("Logs were added to your clipboard successfully.")
    End Sub

    Private Sub DiscordButton28_Click(sender As Object, e As EventArgs) Handles DiscordButton28.Click
        Dim OFD As New OpenFileDialog
        OFD.Title = "Please select an proxy list"
        OFD.Filter = "Text file|*.Txt"


        If OFD.ShowDialog() = DialogResult.OK Then

            Dim lines() As String = IO.File.ReadAllLines(OFD.FileName)

            For x As Integer = 0 To lines.GetUpperBound(0)
                ListBox3.Items.Add(lines(x))
            Next

        End If
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        MsgBox("There can be less results as expected because not working links are removed.", MsgBoxStyle.Information)
    End Sub

    Private Sub DiscordButton12_Click(sender As Object, e As EventArgs)
        UserAgents()
    End Sub

    Private Sub DiscordButton13_Click(sender As Object, e As EventArgs) Handles DiscordButton13.Click
        If ListView1.Items.Count <> 0 Then


            Dim sfile As New SaveFileDialog
            With sfile
                .Title = "Choose your path to save list"

                .Filter = ("Text File (*.txt) | *.txt")
            End With

            If sfile.ShowDialog() = Windows.Forms.DialogResult.OK Then

                Dim Write As New IO.StreamWriter(sfile.FileName)
                Dim k As ListView.ColumnHeaderCollection = ListView1.Columns
                For Each x As ListViewItem In ListView1.Items
                    Dim StrLn As String = ""
                    For i = 0 To x.SubItems.Count - 2
                        StrLn += k(i).Text + ": " + x.SubItems(i).Text + Space(3)
                    Next
                    Write.WriteLine(StrLn)
                Next
                Write.Close()

            End If
        Else
            MsgBox("You don't have items to export.", MsgBoxStyle.Information)
        End If

    End Sub





    Private Sub DiscordButton14_Click(sender As Object, e As EventArgs) Handles DiscordButton14.Click
        If ListBox1.Items.Count = 0 Then
            MsgBox("You don't have any proxy to check.", vbInformation)
        Else
            If DiscordButton14.Text = "Check" Then
                BackgroundWorker1.RunWorkerAsync()
                DiscordButton14.Text = "Abort"
            ElseIf DiscordButton14.Text = "Abort" Then
                MsgBox("Ah difficult")
            End If
        End If




    End Sub



    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork


        For Each proxyline As String In ListBox1.Items
            DiscordLabel5.Text = "Total: " & ListBox1.Items.Count
            DiscordLabel7.Text = "Working: " & ListBox3.Items.Count

            DiscordLabel8.Text = "Not Working: " & ListBox4.Items.Count
            Restant = ListBox1.Items.Count - (Good + Bad)
            TestedP = ListBox3.Items.Count + ListBox4.Items.Count
            DiscordLabel6.Text = "Tested: " & TestedP.ToString
            DiscordLabel11.Text = "Left: " & Restant
            Try
                Dim myproxy As WebProxy
                myproxy = New WebProxy(proxyline)
                Dim request As HttpWebRequest = HttpWebRequest.Create(DiscordTextBox2.Text)
                request.Proxy = myproxy
                request.Timeout = 8000
                Dim response As HttpWebResponse = request.GetResponse





                Good += 1
                ListBox3.Items.Add(proxyline)



            Catch ex As Exception
                Bad += 1
                ListBox4.Items.Add(proxyline)
                DiscordLabel8.Text = "Not Working: " & ListBox4.Items.Count

            End Try

            Restant = ListBox1.Items.Count - (Good + Bad)
            DiscordLabel11.Text = "Left: " & Restant
        Next

        MessageBox.Show("Finished!")

    End Sub
End Class





