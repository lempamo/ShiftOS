﻿Imports System.IO


Public Class Main_Launcher

    Public FirstBoot As Boolean = False
    Public Stable As Boolean = True
    Dim loaddata(100) As String
    Dim lateststable As String
    Dim latestunstable As String
    Dim doneversionsetup As Boolean = False
    Dim currentlydownloading As Boolean = False
    Dim currentversion As String = "0.0.8 RC2"
    Dim exepath As String = "C:\ShiftOS\SoftwareData\Launcher\ShiftOSVersion\ShiftOS 0.0.8 RC2"

    Private Sub Main_Launcher_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Directory.Exists("C:\ShiftOS\SoftwareData\Launcher") Then  Else Directory.CreateDirectory("C:\ShiftOS\SoftwareData\Launcher")

        If File.Exists("C:\ShiftOS\SoftwareData\Launcher\Log.txt") Then File.Delete("C:\ShiftOS\SoftwareData\Launcher\Log.txt")
        File.Create("C:\ShiftOS\SoftwareData\Launcher\Log.txt")

        If Directory.Exists("C:\ShiftOS\SoftwareData\Launcher\UserPreferences") Then  Else FirstBoot = True
        If FirstBoot = True Then Directory.CreateDirectory("C:\ShiftOS\SoftwareData\Launcher\UserPreferences")

        If FirstBoot = True Then
            FirstBootSetup.Show()
            Me.Text = "ShiftOS Launcher [Setting Up]"
        Else
            UpdateOptions()
        End If
    End Sub

    Public Sub UpdateOptions()
        If Me.Text = "ShiftOS Launcher [Setting Up]" Then Me.Text = "ShiftOS Launcher [Downloading]"

        Dim fileContents As String
        fileContents = My.Computer.FileSystem.ReadAllText("C:\ShiftOS\SoftwareData\Launcher\UserPreferences\Nickname.dat")

        Label1.Text = ("Greetings, " + fileContents)

        Dim fileContents1 As String
        fileContents1 = My.Computer.FileSystem.ReadAllText("C:\ShiftOS\SoftwareData\Launcher\UserPreferences\BuildOption.dat")

        If currentlydownloading = True Then
            If fileContents1 = "Stable" Then
                Me.Text = "ShiftOS Launcher [Downloading: Stable]"
                Label2.Text = "Status: Downloading [Stable]"
                Stable = True
            End If
            If fileContents1 = "Unstable" Then
                Me.Text = "ShiftOS Launcher [Downloading: Unstable]"
                Label2.Text = "Status: Downloading [Unstable]"
                Stable = False
            End If
        Else
            If fileContents1 = "Stable" Then
                Me.Text = "ShiftOS Launcher"
                Label2.Text = "Status: Ready to play ShiftOS " & currentversion
                Stable = True
            End If
            If fileContents1 = "Unstable" Then
                Me.Text = "ShiftOS Launcher"
                Label2.Text = "Status: Ready to play ShiftOS " & currentversion
                Stable = False
            End If
        End If
    End Sub

    Private Sub readversiondata()
        Dim sr As New StreamReader("C:\ShiftOS\SoftwareData\Launcher\ShiftOSVersion\versiondata.dat", True)
        If File.Exists("C:\ShiftOS\SoftwareData\Launcher\ShiftOSVersion\versiondata.dat") Then
            For i As Integer = 0 To 4 Step 1
                loaddata(i) = sr.ReadLine
                If i = 4 Then
                    sr.Close()
                    Exit For
                End If
            Next
            lateststable = loaddata(3)
            latestunstable = loaddata(4)
            doneversionsetup = True
            DownloadLatestVersion(Stable)
        Else : MessageBox.Show("Unable to read version data. This error has occured because you are either not connected to a network with internet access or our servers are down. We apologize for any inconvenience.", "Can't read version information")
        End If
    End Sub

    Private Sub DownloadLatestVersion(ByVal BuildOption As Boolean)
        If My.Computer.Network.IsAvailable Then
            If File.Exists("C:\ShiftOS\SoftwareData\Launcher\ShiftOSVersion\versiondata.dat") Then File.Delete("C:\ShiftOS\SoftwareData\Launcher\ShiftOSVersion\versiondata.dat")
            My.Computer.Network.DownloadFile("http://shiftos.bitbucket.org/downloads/versiondata.dat", "C:\ShiftOS\SoftwareData\Launcher\ShiftOSVersion\versiondata.dat")
            readversiondata()
            If doneversionsetup = True Then
                If BuildOption = True Then
                    My.Computer.Network.DownloadFile("http://shiftos.bitbucket.org/downloads/ShiftOS" & lateststable & ".exe", "C:\ShiftOS\SoftwareData\Launcher\ShiftOSVersion\ShiftOS" & lateststable & ".exe")
                    exepath = "C:\ShiftOS\SoftwareData\Launcher\ShiftOSVersion\ShiftOS" & lateststable & ".exe"
                    launchshiftos()
                    ' Detect when finished, do fancy progress bar stuff and run
                Else
                    My.Computer.Network.DownloadFile("http://shiftos.bitbucket.org/downloads/ShiftOS" & latestunstable & ".exe", "C:\ShiftOS\SoftwareData\Launcher\ShiftOSVersion\ShiftOS" & latestunstable & ".exe")
                End If
            End If
        Else 'Offlinemode()
        End If        ' No idea how to do this, we need a dedicated server
        ' This also needs to check if they have the latest unstable / stable
        ' But it cant do that here, it has to do it at the end of updateoptions
        ' Where the little green text is on updateoptions is where it needs
        ' to check, if you read the code - it should be obvious what does
        ' what, once this is done, i will add backups to this and an
        ' offline mode option
    End Sub

    Private Sub launchshiftos()
        Process.Start("F:\william\Documents\Visual Studio\ShiftOS 0.0.8 RCs\ShiftOS 0.0.8 RC2.exe") ' For some reason ShiftOS dosen't auto-extract it's dll when started like this, therefore producing an error.
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If Button1.Text = "Play" Then
            launchshiftos()
        ElseIf Button1.Text = "Download" Then
            DownloadLatestVersion(Stable)
        End If
    End Sub
End Class
