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

using Craftplacer.ClassicSuite.Wizards.Forms;
using Craftplacer.ClassicSuite.Wizards.Pages;
using Craftplacer.ClassicSuite.Wizards.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using Microsoft.Win32;

namespace SimpleClassicTheme.SetupWizard
{
    class SetupHandlerObject
	{
        //Properties
        public TaskbarType SelectedTaskbarType { get => SetupHandler.SelectedTaskbarType; }
        public bool EnableOnBoot { get => SetupHandler.EnableOnBoot; }
        public List<InstallableUtility> UtilitiesToBeInstalled { get => SetupHandler.UtilitiesToBeInstalled; }
        public bool ConfigureOSSM { get => SetupHandler.ConfigureOSSM; }
        public bool ConfigureOSTB { get => SetupHandler.ConfigureOSTB; }
        public bool ConfigureSiB { get => SetupHandler.ConfigureSiB; }
    }

    static class SetupHandler
    {
        // Wizard stuff
        public static WizardForm Installer;

        // Install options page results
        public static TaskbarType SelectedTaskbarType = TaskbarType.SimpleClassicThemeTaskbar;
        public static bool EnableOnBoot = true;

        // Utilities page results
        public static List<InstallableUtility> UtilitiesToBeInstalled = new List<InstallableUtility>();
        public static bool ConfigureOSSM = false;
        public static bool ConfigureOSTB = false;
        public static bool ConfigureSiB = false;

        public static string InstallPath = Configuration.InstallPath;

        /// <summary>
        /// Constructs a Craftplacer.ClassicSuite.Wizards.Forms.WizardForm and places the required pages for an SCT installation in it.
        /// </summary>
        /// <returns></returns>
        public static WizardForm CreateWizard()
        {
            List<WizardPage> pages = new List<WizardPage>()
            {
                new WelcomePage(),
                new LicensePage(),
                new InstallOptionsPage(),
                new InstallPathPage(),
                new InstallationPage(),
                new FinishedPage()
            };

            WizardForm form = WizardForm.FromList(pages);

            form.Text = "Simple Classic Theme Setup Wizard";
            form.Icon = Properties.Resources.sct_logo;
            form.Shown += delegate { UxTheme.SetWindowTheme(form.Handle, " ", " "); };

            return form;
        }

        /// <summary>
        /// Shows an error page on the specified wizard.
        /// </summary>
        /// <param name="wizard"></param>
        public static void ErrorWizard(WizardPage wizard)
		{
            wizard.NextPage = new FinishedPage(true);
            //wizard.
		}

        /// <summary>
        /// Begins running a standard application message loop on the current thread, and the specified wizard visible.
        /// </summary>
        /// <param name="wizard"></param>
        public static void ShowWizard(WizardForm wizard)
        {
            Installer = wizard;
            Application.Run(wizard);
        }

        /// <summary>
        /// Shows the wizard as a modal dialog box with the specified owner.
        /// </summary>
        /// <param name="wizard"></param>
        /// <param name="parent"></param>
        public static void ShowWizardPopup(WizardForm wizard, Form parent)
        {
            Installer = wizard;
            wizard.ShowDialog(parent);
        }

        /// <summary>
        /// Performs the complete installation of SCT based on the user's options and reports back progress when needed.
        /// </summary>
        /// <param name="progressDisplay"></param>
        public static void InstallSCT(InstallationPage progressDisplay)
        {
            if (SelectedTaskbarType == TaskbarType.SimpleClassicThemeTaskbar && !ExtraFunctions.IsDotNetRuntimeInstalled())
            {
                progressDisplay.progressWorker.ReportProgress(5);
                progressDisplay.Invoke(new Action(() =>
                {
                    progressDisplay.SetProgressBarColor(2);
                    MessageBox.Show(progressDisplay.ParentForm, "Simple Classic Theme Taskbar requires the .NET Desktop Runtime. Install the .NET Desktop Runtime and run this wizard again.", "Cannot install SCT Taskbar");
                    Process.Start("https://dotnet.microsoft.com/download/dotnet/5.0/runtime");
                    ErrorWizard(progressDisplay);
                }));
                return;
			}

            // Move any existing SCT stuff to Configuration.InstallPath
            if (Configuration.InstallPath != InstallPath)
            {
                progressDisplay.progressText = "Moving SCT files to installation directory...";
                progressDisplay.progressWorker.ReportProgress(0);
                ExtraFunctions.DirectoryCopy(Configuration.InstallPath, InstallPath, true);
                Directory.Delete(Configuration.InstallPath, true);
                Configuration.InstallPath = InstallPath;
            }

            // Install SCT to Configuration.InstallPath
            // Create a task for SCT
            progressDisplay.progressText = "Installing Simple Classic Theme...";
            progressDisplay.progressWorker.ReportProgress(0);
            ExtraFunctions.UpdateStartupExecutable(true);

            // Go through all extra utilities and install them
            float temp = 10;
            foreach (InstallableUtility utility in UtilitiesToBeInstalled)
            {
                if (utility.Name != "Open-Shell" && utility.Name != "StartIsBack++")
                {
                    progressDisplay.progressText = "Installing " + utility.Name + "...";
                    progressDisplay.progressWorker.ReportProgress((int)temp);
                    temp += 10F / UtilitiesToBeInstalled.Count;
                    int returnCode = utility.Install();
                    if (returnCode != 0)
                    {
                        progressDisplay.Invoke(new Action(() =>
                        {
                            progressDisplay.SetProgressBarColor(2);
                            MessageBox.Show(progressDisplay.ParentForm, $"Could not install {utility.Name}. Installer returned code {returnCode}.", "Installation failed");
                            ErrorWizard(progressDisplay);
                        }));
                        return;
                    }
                }
            }

            // Configure StartIsBack++ if OS+SiB
            if (ConfigureSiB)
            {
                progressDisplay.progressText = "Configuring StartIsBack++ before installing...";
                progressDisplay.progressWorker.ReportProgress(20);

                ExtraFunctions.ReConfigureOS(false, false, true);
            }

            // Configure Open-Shell Start Menu if OS+SiB or for manual selection
            if (ConfigureOSSM)
            {
                progressDisplay.progressText = "Configuring Open-Shell Menu's Start Menu before installing...";
                progressDisplay.progressWorker.ReportProgress(30);

                ExtraFunctions.ReConfigureOS(true, false, false);
            }

            // Configure Open-Shell Taskbar if OS+SiB or for manual selection
            if (ConfigureOSTB)
            {
                progressDisplay.progressText = "Configuring Open-Shell Menu's Taskbar before installing...";
                progressDisplay.progressWorker.ReportProgress(40);

                ExtraFunctions.ReConfigureOS(false, true, false);
            }

            // Install StartIsBack++ if OS+SiB or for manual selection
            if (UtilitiesToBeInstalled.Where((a) => a.Name == "StartIsBack++").Count() > 0)
            {
                progressDisplay.progressText = "Installing StartIsBack++...";
                progressDisplay.progressWorker.ReportProgress(50);
                int returnCode = InstallableUtility.StartIsBackPlusPlus.Install();
                if (returnCode != 0)
                {
                    progressDisplay.Invoke(new Action(() =>
                    {
                        progressDisplay.SetProgressBarColor(2);
                        MessageBox.Show(progressDisplay.ParentForm, $"Could not install StartIsBack++. Installer returned code {returnCode}.", "Installation failed");
                        ErrorWizard(progressDisplay);
                    }));
                    return;
                }
            }

            // Install Open-Shell if OS+SiB or for manual selection
            if (UtilitiesToBeInstalled.Where((a) => a.Name == "Open-Shell").Count() > 0)
            {
                progressDisplay.progressText = "Installing Open-Shell...";
                progressDisplay.progressWorker.ReportProgress(60);
                int returnCode = InstallableUtility.OpenShell.Install();
                if (returnCode != 0)
                {
                    progressDisplay.Invoke(new Action(() =>
                    {
                        progressDisplay.SetProgressBarColor(2);
                        MessageBox.Show(progressDisplay.ParentForm, $"Could not install Open-Shell. Installer returned code {returnCode}.", "Installation failed");
                        ErrorWizard(progressDisplay);
                    }));
                    return;
                }
            }

            // Install SCT Taskbar
            if (SelectedTaskbarType == TaskbarType.SimpleClassicThemeTaskbar)
            {
                progressDisplay.progressText = "Installing Simple Classic Theme Taskbar...";
                progressDisplay.progressWorker.ReportProgress(70);
                progressDisplay.Invoke(new Action(() => new GithubDownloader(GithubDownloader.DownloadableGithubProject.SimpleClassicThemeTaskbar).ShowDialog()));
            }

            // Install RetroBar
            else if (SelectedTaskbarType == TaskbarType.RetroBar)
            {
                progressDisplay.progressText = "Installing RetroBar...";
                progressDisplay.progressWorker.ReportProgress(70);
                progressDisplay.Invoke(new Action(() => new GithubDownloader(GithubDownloader.DownloadableGithubProject.RetroBar).ShowDialog()));
            }

            // Configure SCT
            progressDisplay.progressText = "Configuring Simple Classic Theme...";
            progressDisplay.progressWorker.ReportProgress(80);
            Configuration.EnableTaskbar = SelectedTaskbarType != TaskbarType.None;
            Configuration.TaskbarType = SelectedTaskbarType != TaskbarType.None ? SelectedTaskbarType : TaskbarType.SimpleClassicThemeTaskbar;
            Configuration.UpdateMode = "Automatic";

            // Finalize installation and record data for uninstall
            progressDisplay.progressText = "Finalizing installation and enabling Classic Theme...";
            progressDisplay.progressWorker.ReportProgress(90);

            // Sleep for dramatic installation effect
            ClassicTheme.MasterEnable(SelectedTaskbarType != TaskbarType.None);
            System.Threading.Thread.Sleep(1200);
            progressDisplay.progressWorker.ReportProgress(100);
        }
    }
}
