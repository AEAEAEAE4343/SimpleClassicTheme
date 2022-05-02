using System.Drawing;
using System.Resources;
using System.Windows;

namespace SimpleClassicTheme
{
    internal class ResourceFetcher
    {
        ResourceManager mainResourceManager;
        public ResourceManager MainResourceManager
        {
            get
            {
                if (mainResourceManager is null)
                    mainResourceManager = new ResourceManager("SimpleClassicTheme.Properties.MainResources", typeof(Properties.MainResources).Assembly);
                return mainResourceManager;
            }
        }

#if SCTLITE
#else
        ResourceManager optionalResourceManager;
#endif
        public ResourceManager OptionalResourceManager
        {
            get
            {
#if SCTLITE
                return null;
#else
                if (optionalResourceManager is null)
                    optionalResourceManager = new ResourceManager("SimpleClassicTheme.Properties.OptionalResources", typeof(Properties.OptionalResources).Assembly);
                return optionalResourceManager;
#endif
            }
        }

        public byte[] GetBytesFromOptionalResourceManager(string name)
        {
            if (OptionalResourceManager is null)
            {
                MessageBox.Show($"Couldn't fetch resource '{name}'", "Simple Classic Theme");
                return null;
            }
            else
                return (byte[])OptionalResourceManager.GetObject(name);
        }

        public string TaskFile => MainResourceManager.GetString("cmd_create_task");
        public string CreateTaskScript => MainResourceManager.GetString("taskScheduleCommands");
        public string EnableThemeScript => MainResourceManager.GetString("EnableThemeScript");
        public string DisableThemeScript => MainResourceManager.GetString("DisableThemeScript");
        public string UpdateString => MainResourceManager.GetString("updateString");
        public string HelpString => MainResourceManager.GetString("HelpMessage");
        public string ColorSchemesReg => MainResourceManager.GetString("reg_classicschemes_add");

        public byte[] AppearanceCPL => (byte[])MainResourceManager.GetObject("desktopControlPanelCPL");

        public Bitmap MsiExecLogo => (Bitmap)MainResourceManager.GetObject("msiexec");
        public Bitmap XpWizardSidebar => (Bitmap)MainResourceManager.GetObject("winxp_wizard");
        public Bitmap SCTBannerLight400x73 => (Bitmap)MainResourceManager.GetObject("sct_banner_light_400x73");
        public Bitmap SCTBannerDark400x73 => (Bitmap)MainResourceManager.GetObject("sct_banner_dark_400x73");
        public Bitmap SCTLogoLight164 => (Bitmap)MainResourceManager.GetObject("sct_light_164");
        public Bitmap SCTLogoDark164 => (Bitmap)MainResourceManager.GetObject("sct_dark_164");
        public Bitmap SCTLogoLight275 => (Bitmap)MainResourceManager.GetObject("sct_light_275");
        public Bitmap SCTLogoDark275 => (Bitmap)MainResourceManager.GetObject("sct_dark_275");
        public Bitmap ThemePreviewToolWindowIcons => (Bitmap)MainResourceManager.GetObject("ThemePreviewToolWindowIcons");
        public Icon SCTLogoIcon => (Icon)MainResourceManager.GetObject("sct_logo");

        public byte[] ExplorerContextMenuTweaker => GetBytesFromOptionalResourceManager("ExplorerContextMenuTweaker");
        public byte[] ShellPayload => GetBytesFromOptionalResourceManager("ShellPayload");
        public byte[] FixStrips => GetBytesFromOptionalResourceManager("fixstrips");
        public byte[] RibbonDisabler => GetBytesFromOptionalResourceManager("ribbonDisabler");
        public byte[] ClassicTaskManager => GetBytesFromOptionalResourceManager("setup_classic-taskmgr");
        public byte[] FolderOptionsX => GetBytesFromOptionalResourceManager("setup_folder-options-x");
    }
}
