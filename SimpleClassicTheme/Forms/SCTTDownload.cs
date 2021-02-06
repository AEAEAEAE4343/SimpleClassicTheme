/*
 *  SimpleClassicTheme, a basic utility to bring back classic theme to newer version of the Windows operating system.
 *  Copyright (C) 2020 Anis Errais
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
	public partial class SCTTDownload : Form
	{
		public int progressDownload = 0;
		public int progressExtract = 0;

		public SCTTDownload()
		{
			InitializeComponent();
		}

		private void SCTTDownload_Load(object sender, EventArgs e)
		{
			label1.Text = "Downloading...";

			new Thread(ThreadFunction).Start();
			timer1.Enabled = true;
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			progressBar1.Value = progressDownload == 100 ? progressExtract : progressDownload;
			if (progressDownload == 100)
				label1.Text = "Extracting...";
			if (progressExtract == 100)
			{
				timer1.Enabled = false;
				Close();
			}
			progressBar2.Value = (progressDownload / 2) + (progressExtract / 2);
		}

		private void ThreadFunction()
		{
			foreach (Process p in Process.GetProcessesByName("SCT_Taskbar"))
				p.Kill();

			string dlUrl = "https://github.com/AEAEAEAE4343/SimpleClassicThemeTaskbar/releases/latest/download/SimpleClassicThemeTaskbar_x64.zip";
			string destDl = "C:\\SCT\\sctt.zip";
			string destExtract = "C:\\SCT\\Taskbar\\";

			if (Directory.Exists(destExtract))
				Directory.Delete(destExtract, true);
			if (File.Exists(destDl))
				File.Delete(destDl);

			//Download
			WebRequest request = WebRequest.Create(dlUrl);
			request.Proxy = null;
			WebResponse response = request.GetResponse();
			Stream ws = response.GetResponseStream();

			Directory.CreateDirectory(destExtract);
			FileStream fs = File.Create(destDl);
			
			byte[] buffer = new byte[1024];
			while (true)
			{
				int bytesRead = ws.Read(buffer, 0, 1024);
				fs.Write(buffer, 0, bytesRead);
				progressDownload = (int)((float)bytesRead / buffer.Length * 100);
				if (bytesRead == 0)
					break;
			}
			fs.Close();

			//Extraction
			ZipArchive archive = ZipFile.Open(destDl, ZipArchiveMode.Read);
			for (int i = 0; i < archive.Entries.Count; i++)
			{
				ZipArchiveEntry entry = archive.Entries[i];
				string destPath = Path.GetFullPath(Path.Combine(destExtract, entry.FullName));
				Directory.CreateDirectory(destPath);
				entry.ExtractToFile(destPath);
				progressExtract = (int)((float)i / archive.Entries.Count * 100);
				if (i == archive.Entries.Count - 1)
					progressExtract = 100;
			}
			archive.Dispose();
			if (File.Exists("C:\\SCT\\Taskbar\\SimpleClassicThemeTaskbar.exe"))
				File.Move("C:\\SCT\\Taskbar\\SimpleClassicThemeTaskbar.exe", "C:\\SCT\\Taskbar\\SCT_Taskbar.exe");
		}
	}
}
