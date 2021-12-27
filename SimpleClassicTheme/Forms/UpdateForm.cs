/*
 *  SimpleClassicTheme, a basic utility to bring back classic theme to newer versions of the Windows operating system.
 *  Copyright (C) 2021 Anis Errais
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <https://www.gnu.org/licenses/>.
 *
 */

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reflection;
using System.Windows.Forms;

namespace SimpleClassicTheme
{
    public partial class Updater : Form
    {
        public Updater()
        {
            InitializeComponent();
            Text += " v" + Assembly.GetExecutingAssembly().GetName().Version;
        }

        Version ver;
        public bool HasUpdated = false;
        private void Updater_Load(object sender, EventArgs e)
        {
            //Get latest release info
            string f;
            using (WebClient c = new WebClient())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
                c.Headers.Set(HttpRequestHeader.UserAgent, "SimpleClasicTheme");
                f = c.DownloadString("https://api.github.com/repos/WinClassic/SimpleClassicTheme/releases/latest");
            }

            //Resond to messages
            Application.DoEvents(); Application.DoEvents(); Application.DoEvents();

            //Get version string
            string s = f.Substring(f.IndexOf("\"tag_name\""));
            string tagName = s.Remove(s.IndexOf("\","));
            tagName = tagName.Substring(tagName.LastIndexOf('"') + 1);

            //Resond to messages
            Application.DoEvents(); Application.DoEvents(); Application.DoEvents();

            //Make sure we got version string
            if (tagName != "")
            {
                Version newestVersion = Version.Parse(tagName);
                Version currentVersion = Assembly.GetExecutingAssembly().GetName().Version;

                //Check if newestVersion is bigger then currentVersion
                if (currentVersion.CompareTo(newestVersion) < 0)
                {
                    if (Configuration.UpdateMode == "Ask on startup" && MessageBox.Show($"SCT version {newestVersion} is available.\nWould you like to update now?", "Update available") != DialogResult.Yes)
                        Close();
                    else
                    {
                        label1.Text = "Downloading update " + newestVersion.ToString(3) + "...";
                        ver = newestVersion;
                        HasUpdated = true;
                        DownloadNewestVersion();
                    }
                }
            }

            if (File.Exists("C:\\SCT\\Taskbar\\SimpleClassicThemeTaskbar.exe") && Configuration.TaskbarType == TaskbarType.SimpleClassicThemeTaskbar)
			{
                //Get latest release info
                f = "";
                using (WebClient c = new WebClient())
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
                    c.Headers.Set(HttpRequestHeader.UserAgent, "SimpleClasicTheme");
                    f = c.DownloadString("https://api.github.com/repos/WinClassic/SimpleClassicTheme.Taskbar/releases/latest");
                }

                //Resond to messages
                Application.DoEvents(); Application.DoEvents(); Application.DoEvents();

                //Get version string
                s = f.Substring(f.IndexOf("\"tag_name\""));
                tagName = s.Remove(s.IndexOf("\","));
                tagName = tagName.Substring(tagName.LastIndexOf('"') + 1);

                //Resond to messages
                Application.DoEvents(); Application.DoEvents(); Application.DoEvents();

                //Make sure we got version string
                if (tagName != "")
                {
                    Version newestVersion = Version.Parse(tagName);
                    FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo("C:\\SCT\\Taskbar\\SimpleClassicThemeTaskbar.exe");
                    Version currentVersion;
                    Version.TryParse(versionInfo.ProductVersion, out currentVersion);

                    //Check if newestVersion is bigger then currentVersion
                    if (currentVersion != null && currentVersion.CompareTo(newestVersion) < 0)
                    {
                        if (Configuration.UpdateMode == "Ask on startup" && MessageBox.Show($"SCT Taskbar version {newestVersion} is available.\nWould you like to update now?", "Update available") != DialogResult.Yes)
                            Close();
                        else
                        {
                            label1.Text = "Downloading update " + newestVersion.ToString(3) + "...";
                            ver = newestVersion;
                            DownloadNewestTaskbarVersion();
                        }
                    }
                }
            }

            if (File.Exists("C:\\SCT\\RetroBar\\RetroBar.exe") && Configuration.TaskbarType == TaskbarType.RetroBar)
			{
                //Get latest release info
                f = "";
                using (WebClient c = new WebClient())
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
                    c.Headers.Set(HttpRequestHeader.UserAgent, "SimpleClasicTheme");
                    f = c.DownloadString("https://api.github.com/repos/dremin/RetroBar/releases/latest");
                }

                //Resond to messages
                Application.DoEvents(); Application.DoEvents(); Application.DoEvents();

                //Get version string
                s = f.Substring(f.IndexOf("\"tag_name\""));
                tagName = s.Remove(s.IndexOf("\","));
                tagName = tagName.Substring(tagName.LastIndexOf('"') + 1);
                tagName = tagName.Replace("v", "");

                //Resond to messages
                Application.DoEvents(); Application.DoEvents(); Application.DoEvents();

                //Make sure we got version string
                if (tagName != "")
                {
                    Version newestVersion = Version.Parse(tagName);
                    FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo("C:\\SCT\\RetroBar\\RetroBar.exe");
                    Version currentVersion;
                    Version.TryParse(versionInfo.ProductVersion, out currentVersion);

                    //Check if newestVersion is bigger then currentVersion
                    if (currentVersion != null && currentVersion.CompareString(tagName.Remove(tagName.LastIndexOf('.'))) < 0)
                    {
                        if (Configuration.UpdateMode == "Ask on startup" && MessageBox.Show($"RetroBar version {newestVersion} is available.\nWould you like to update now?", "Update available") != DialogResult.Yes)
                            Close();
                        else
                        {
                            label1.Text = "Downloading update " + newestVersion.ToString(3) + "...";
                            ver = newestVersion;
                            DownloadNewestRetroBarVersion();
                        }
                    }
                }
            }
            Close();
        }

		private void DownloadNewestVersion()
		{
			//Respond to messages
			Application.DoEvents(); Application.DoEvents(); Application.DoEvents();

			WebClient c = new WebClient();

			c.DownloadProgressChanged += delegate (object sender, DownloadProgressChangedEventArgs e)
			{
				progressBar1.Maximum = (int)(e.TotalBytesToReceive / 1000);
				progressBar1.Value = (int)(e.BytesReceived / 1000);
			};
			c.DownloadFileCompleted += delegate
			{
				File.WriteAllText("___UPDATESCT.bat", Properties.Resources.updateString);
				Process.Start("___UPDATESCT.bat", $"{ver.ToString(3)} {Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName)} ___SCT.exe");
			};

			c.Headers.Set(HttpRequestHeader.UserAgent, "SimpleClasicTheme");
			c.DownloadFileAsync(new Uri("https://github.com/WinClassic/SimpleClassicTheme/releases/latest/download/SimpleClassicTheme.exe"), "___SCT.exe");
		}

        private void DownloadNewestTaskbarVersion()
        {
            ClassicTaskbar.InstallSCTT(this, false);
        }

        private void DownloadNewestRetroBarVersion()
        {
            GithubDownloader download = new GithubDownloader(GithubDownloader.DownloadableGithubProject.RetroBar);
            download.ShowDialog();
        }
    }
}
