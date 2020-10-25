using System;
using System.Diagnostics;
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

        private void Updater_Load(object sender, EventArgs e)
        {
            //Get latest release info
            string f;
            using (WebClient c = new WebClient())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
                c.Headers.Set(HttpRequestHeader.UserAgent, "SimpleClasicTheme");
                f = c.DownloadString("https://api.github.com/repos/AEAEAEAE4343/SimpleClassicTheme/releases/latest");
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
                    if ((string)Configuration.GetItem("UpdateMode", "Automatic") == "Ask on startup" && MessageBox.Show($"SCT version {newestVersion} is available.\nWould you like to update now?", "Update available") != DialogResult.Yes)
                        Close();
                    else
                    {
                        label1.Text = "Downloading update " + newestVersion.ToString(3) + "...";
                        ver = newestVersion;
                        DownloadNewestVersion();
                    }
                }
            }

            if (File.Exists("C:\\SCT\\Taskbar\\SCT_Taskbar.exe") && (string)Configuration.GetItem("TaskbarType", "SiB+OS") == "SCTT")
			{
                //Get latest release info
                f = "";
                using (WebClient c = new WebClient())
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
                    c.Headers.Set(HttpRequestHeader.UserAgent, "SimpleClasicTheme");
                    f = c.DownloadString("https://api.github.com/repos/AEAEAEAE4343/SimpleClassicThemeTaskbar/releases/latest");
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
                    Version currentVersion = Assembly.LoadFrom("C:\\SCT\\Taskbar\\SCT_Taskbar.exe").GetName().Version;

                    //Check if newestVersion is bigger then currentVersion
                    if (currentVersion.CompareTo(newestVersion) < 0)
                    {
                        if ((string)Configuration.GetItem("UpdateMode", "Automatic") == "Ask on startup" && MessageBox.Show($"SCT Taskbar version {newestVersion} is available.\nWould you like to update now?", "Update available") != DialogResult.Yes)
                            Close();
                        else
                        {
                            label1.Text = "Downloading update " + newestVersion.ToString(3) + "...";
                            ver = newestVersion;
                            DownloadNewestVersion();
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
				Environment.Exit(0);
			};

			c.Headers.Set(HttpRequestHeader.UserAgent, "SimpleClasicTheme");
			c.DownloadFileAsync(new Uri("https://github.com/AEAEAEAE4343/SimpleClassicTheme/releases/latest/download/SimpleClassicTheme.exe"), "___SCT.exe");
		}

        private void DownloadNewestTaskbarVersion()
        {
            ClassicTaskbar.InstallSCTT(this, false);
        }
    }
}
