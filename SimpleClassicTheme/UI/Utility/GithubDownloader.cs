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
using System.Threading;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Diagnostics;

namespace SimpleClassicTheme
{
	public partial class GithubDownloader : Form
	{
		public class DownloadableGithubProject
		{
			public static DownloadableGithubProject SimpleClassicThemeTaskbar = new DownloadableGithubProject() 
			{ 
				Name = "WinClassic/SimpleClassicTheme.Taskbar", 
				Filename = (IntPtr.Size == 8) ? "SimpleClassicThemeTaskbar_x64.zip" : "SimpleClassicThemeTaskbar_x86.zip",
				ProcessName = "SimpleClassicThemeTaskbar",
				TargetDirectory = $"{SCT.Configuration.InstallPath}Taskbar\\",
				NeedsExtraction = true
			};

			public static DownloadableGithubProject RetroBar = new DownloadableGithubProject()
			{
				Name = "dremin/RetroBar",
				Filename = (IntPtr.Size == 8) ? "RetroBar.64-bit.zip" : "RetroBar.32-bit.zip",
				ProcessName = "RetroBar",
				TargetDirectory = $"{SCT.Configuration.InstallPath}RetroBar\\",
				NeedsExtraction = true
			};

			public static DownloadableGithubProject ExplorerPatcher = new DownloadableGithubProject()
			{
				Name = "valinet/ExplorerPatcher",
				Filename = "dxgi.dll",
				ProcessName = "",
				TargetDirectory = $"{SCT.Configuration.InstallPath}ExplorerPatcher\\",
				NeedsExtraction = false
			};

			public static DownloadableGithubProject SimpleClassicThemeFEH = new DownloadableGithubProject()
			{
				Name = "valinet/ExplorerPatcher",
				Filename = (IntPtr.Size == 8) ? "SCT.FEH.x64.zip" : "SCT.FEH.x64.zip",
				ProcessName = "",
				TargetDirectory = $"{SCT.Configuration.InstallPath}AHK\\",
				NeedsExtraction = true
			};

			public string Name;
			public string Filename;
			public string ProcessName;
			public string TargetDirectory;
			public bool NeedsExtraction;
		}

		public DownloadableGithubProject project;
		public int progressDownload = 0;
		public int progressExtract = 0;

		public GithubDownloader()
		{
			InitializeComponent();
		}

		public GithubDownloader(DownloadableGithubProject project)
		{
			this.project = project;
			InitializeComponent();
		}

		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams param = base.CreateParams;
				param.ClassStyle ^= 0x200;
				return param;
			}
		}

		private void GithubDownloader_Load(object sender, EventArgs e)
		{
			if (project != null)
			{
				label1.Text = "Downloading...";
				label2.Text = $"Retrieving latest release for '{project.Name}'";

				new Thread(ThreadFunction).Start();
				timer1.Enabled = true;
			}
			else
			{
				label1.Text = "Unhandled exception: No GitHub project specified";
			}
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			progressBar1.Value = progressDownload == 100 ? progressExtract : progressDownload;
			if (progressDownload == 100)
				label1.Text = "Extracting...";
			if (progressExtract == 100 || (progressDownload == 100 && !project.NeedsExtraction))
			{
				timer1.Enabled = false;
				Close();
			}
			progressBar2.Value = project.NeedsExtraction ? (progressDownload / 2) + (progressExtract / 2) : progressDownload; 
		}

		private void ThreadFunction()
		{
			if (project.ProcessName != "")
				while (Process.GetProcessesByName(project.ProcessName).Length > 0)
					foreach (Process p in Process.GetProcessesByName(project.ProcessName))
						if (!p.HasExited)
							p.Kill();

			string dlUrl = $"https://github.com/{project.Name}/releases/latest/download/{project.Filename}";
			string dlDest = $"{SCT.Configuration.InstallPath}ghtemp.tmp";
			string dlDestExtract = project.TargetDirectory;

			if (Directory.Exists(dlDestExtract))
				Directory.Delete(dlDestExtract, true);
			if (File.Exists(dlDest))
				File.Delete(dlDest);

			//Download
			WebRequest request = WebRequest.Create(dlUrl);
			request.Proxy = null;
			WebResponse response = request.GetResponse();
			Stream ws = response.GetResponseStream();
			Directory.CreateDirectory(dlDestExtract);
			FileStream fs = File.Create(dlDest);
			byte[] buffer = new byte[1024];
			while (true)
			{
				int bytesRead = ws.Read(buffer, 0, 1024);
				fs.Write(buffer, 0, bytesRead);
				progressDownload = (int)((float)bytesRead / buffer.Length * 100);
				if (bytesRead == 0)
					break;
			}
			fs.Close(); ws.Close(); response.Close();

			// Extraction / Installation
			if (project.NeedsExtraction)
			{
				ZipArchive archive = ZipFile.Open(dlDest, ZipArchiveMode.Read);
				for (int i = 0; i < archive.Entries.Count; i++)
				{
					ZipArchiveEntry entry = archive.Entries[i];
					if (entry.Length == 0)
						continue;
					string destPath = Path.GetFullPath(Path.Combine(dlDestExtract, entry.FullName));
					Directory.CreateDirectory(Path.GetDirectoryName(destPath));
					entry.ExtractToFile(destPath);
					progressExtract = (int)((float)i / archive.Entries.Count * 100);
					if (i == archive.Entries.Count - 1)
						progressExtract = 100;
				}
				archive.Dispose();
			}
			else
			{
				File.Copy(dlDest, Path.GetFullPath(Path.Combine(dlDestExtract, project.Filename)));
			}
		}
	}
}
