﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SimpleClassicTheme.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class MainResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal MainResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("SimpleClassicTheme.Properties.MainResources", typeof(MainResources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;UTF-16&quot;?&gt;
        ///&lt;Task version=&quot;1.2&quot; xmlns=&quot;http://schemas.microsoft.com/windows/2004/02/mit/task&quot;&gt;
        ///  &lt;RegistrationInfo&gt;
        ///    &lt;Date&gt;2020-08-03T20:43:25&lt;/Date&gt;
        ///    &lt;Author&gt;Anis&lt;/Author&gt;
        ///  &lt;/RegistrationInfo&gt;
        ///  &lt;Triggers&gt;
        ///    &lt;LogonTrigger&gt;
        ///      &lt;StartBoundary&gt;2020-08-03T20:43:00&lt;/StartBoundary&gt;
        ///      &lt;Enabled&gt;true&lt;/Enabled&gt;
        ///    &lt;/LogonTrigger&gt;
        ///  &lt;/Triggers&gt;
        ///  &lt;Principals&gt;
        ///    &lt;Principal id=&quot;Author&quot;&gt;
        ///      &lt;LogonType&gt;InteractiveToken&lt;/LogonType&gt;
        ///      &lt;RunLevel&gt;HighestAva [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string cmd_create_task {
            get {
                return ResourceManager.GetString("cmd_create_task", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Byte[].
        /// </summary>
        internal static byte[] desktopControlPanelCPL {
            get {
                object obj = ResourceManager.GetObject("desktopControlPanelCPL", resourceCulture);
                return ((byte[])(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to @echo off
        ///REM ===================================
        ///REM   Generated by SimpleClassicTheme
        ///REM   Version {ver}
        ///REM 
        ///REM   Copyright © 2022 Leet
        ///REM ===================================
        ///goto %1
        ///
        ///
        ///:pre
        ///REM ===================================
        ///REM       Programs to run before
        ///REM       disabling Classic Theme
        ///REM ===================================
        ///
        ///
        ///goto both
        ///:post
        ///REM ===================================
        ///REM        Programs to run after
        ///REM       disabling Classic Theme
        ///REM ===================================
        ///
        ///
        ///go [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string DisableThemeScript {
            get {
                return ResourceManager.GetString("DisableThemeScript", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to @echo off
        ///REM ===================================
        ///REM   Generated by SimpleClassicTheme
        ///REM   Version {ver}
        ///REM 
        ///REM   Copyright © 2022 Leet
        ///REM ===================================
        ///goto %1
        ///
        ///
        ///:pre
        ///REM ===================================
        ///REM       Programs to run before
        ///REM       enabling Classic Theme
        ///REM ===================================
        ///
        ///
        ///goto both
        ///:post
        ///REM ===================================
        ///REM        Programs to run after
        ///REM       enabling Classic Theme
        ///REM ===================================
        ///
        ///
        ///goto [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string EnableThemeScript {
            get {
                return ResourceManager.GetString("EnableThemeScript", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ========================================================================================================================
        ///Usage: SimpleClassicTheme.exe (arguments...) 
        ///You can add as many from the arguments listed down here as you want
        ///
        ///--boot                                                      Running this command will make SCT set Classic Theme up 
        ///                                                            the way it was before logging out. This is used for 
        ///                                               [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string helpMessage {
            get {
                return ResourceManager.GetString("helpMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap msiexec {
            get {
                object obj = ResourceManager.GetObject("msiexec", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to reg ADD &quot;HKCU\Control Panel\Appearance\Schemes&quot; /v &quot;Windows Modern&quot; /t REG_BINARY /d &quot;0200000034e439000100000010000000100000001200000012000000f5ffffff000000000000000000000000bc02000000000001000005005300650067006f0065002000550049000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000f0000000f000000f5ffffff000000000000000000000000bc02000000000001000005005300650067006f0065002000550049000000000000000000000000000000000000000000000000000000000000000000000000000000000000 [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string reg_classicschemes_add {
            get {
                return ResourceManager.GetString("reg_classicschemes_add", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to @echo off
        ///title SCT Uninstallation
        ///echo Removing final SCT files
        ///timeout 5 &gt; nul
        ///rmdir /S /Q {InstallPath}
        ///shutdown.exe -r -t 2
        ///(goto) 2&gt;nul &amp; del &quot;%~f0&quot;.
        /// </summary>
        internal static string removalString {
            get {
                return ResourceManager.GetString("removalString", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap sct_banner_dark_400x73 {
            get {
                object obj = ResourceManager.GetObject("sct_banner_dark_400x73", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap sct_banner_light_400x73 {
            get {
                object obj = ResourceManager.GetObject("sct_banner_light_400x73", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap sct_dark_164 {
            get {
                object obj = ResourceManager.GetObject("sct_dark_164", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap sct_dark_275 {
            get {
                object obj = ResourceManager.GetObject("sct_dark_275", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap sct_light_164 {
            get {
                object obj = ResourceManager.GetObject("sct_light_164", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap sct_light_275 {
            get {
                object obj = ResourceManager.GetObject("sct_light_275", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Icon similar to (Icon).
        /// </summary>
        internal static System.Drawing.Icon sct_logo {
            get {
                object obj = ResourceManager.GetObject("sct_logo", resourceCulture);
                return ((System.Drawing.Icon)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to schtasks /Delete /TN &quot;Simple Classic Theme&quot; /F
        ///schtasks /Create /TN &quot;Simple Classic Theme&quot; /XML &quot;C:\SCT\SCTTask.xml&quot;.
        /// </summary>
        internal static string taskScheduleCommands {
            get {
                return ResourceManager.GetString("taskScheduleCommands", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap ThemePreviewToolWindowIcons {
            get {
                object obj = ResourceManager.GetObject("ThemePreviewToolWindowIcons", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to @echo off
        ///title SCT Update
        ///echo Starting installation...
        ///timeout 2 &gt; nul
        ///echo Killing SCT...
        ///taskkill /im %2 /f
        ///echo Removing old files...
        ///del %2
        ///echo Installing SCT v%1...
        ///copy %3 %2
        ///echo Starting SCT...
        ///%2
        ///echo Removing temporary files...
        ///del %3
        ///(goto) 2&gt;nul &amp; del &quot;%~f0&quot;.
        /// </summary>
        internal static string updateString {
            get {
                return ResourceManager.GetString("updateString", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap winxp_wizard {
            get {
                object obj = ResourceManager.GetObject("winxp_wizard", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
    }
}
