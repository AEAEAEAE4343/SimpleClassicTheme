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

using Microsoft.Win32;
using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace SimpleClassicTheme
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
            ClientSize = new Size(400, 440);

            if (ExtraFunctions.ShouldDrawLight(SystemColors.Window))
                pictureBox1.Image = Properties.Resources.sct_banner_light_400x73;
            else
                pictureBox1.Image = Properties.Resources.sct_banner_dark_400x73;
        }

        private void About_Load(object sender, EventArgs e)
        {
            string ver = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ReleaseId", Environment.OSVersion.Version.Build.ToString()).ToString();
            label2.Text = label2.Text.Replace("%build%", ver);

            string name = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ProductName", "Windows " + Environment.OSVersion.Version.Major.ToString()).ToString();
            label2.Text = label2.Text.Replace("%osver%", name);

            string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            label2.Text = label2.Text.Replace("%username%", userName);

            string sctVer = Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
            label2.Text = label2.Text.Replace("%ver%", sctVer);

            Color A = SystemColors.ActiveCaption;
            Color B = SystemColors.GradientActiveCaption;
            Bitmap bitmap = new Bitmap(400, 5);
            for (int i = 0; i < 200; i++)
            {
                int r = A.R + ((B.R - A.R) * i / 200);
                int g = A.G + ((B.G - A.G) * i / 200);
                int b = A.B + ((B.B - A.B) * i / 200);

                for (int y = 0; y < 5; y++)
                    bitmap.SetPixel(i, y, Color.FromArgb(r, g, b));

                for (int y = 0; y < 5; y++)
                    bitmap.SetPixel(399 - i, y, Color.FromArgb(r, g, b));
            }
            pictureBox2.Image = bitmap;
        }

        private void About_Shown(object sender, EventArgs e)
        {
            UxTheme.SetWindowTheme(Handle, " ", " ");
        }
    }
}
