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
    internal class AHKScripts {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal AHKScripts() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("SimpleClassicTheme.Properties.AHKScripts", typeof(AHKScripts).Assembly);
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
        ///   Looks up a localized string similar to #NoTrayIcon
        ///#NoEnv
        ///
        ///Gui +LastFound
        ///hWnd := WinExist()
        ///SetControlDelay, -1
        ///
        ///DllCall( &quot;RegisterShellHookWindow&quot;, UInt,hWnd )
        ///MsgNum := DllCall( &quot;RegisterWindowMessage&quot;, Str,&quot;SHELLHOOK&quot; )
        ///OnMessage( MsgNum, &quot;ShellMessage&quot; )
        ///Return
        ///
        ///ShellMessage(wParam,lParam) 
        ///{
        ///	If (wParam = 1 or wParam = 6) ; HSHELL_WINDOWCREATED := 1
        ///	{
        ///		WinGetClass, WinClass, ahk_id %lParam%
        ///		if (WinClass = &quot;CabinetWClass&quot;) 
        ///		{
        ///			Control, ExStyle, +0x200, SysTreeView321, ahk_id %lParam%
        ///			Control, ExStyle, +0x200, FolderView, ahk_id % [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string clientedge {
            get {
                return ResourceManager.GetString("clientedge", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #NoTrayIcon
        ///#NoEnv
        ///
        ///Gui +LastFound
        ///hWnd := WinExist()
        ///SetControlDelay, -1
        ///SetBatchLines -1
        ///
        ///DllCall( &quot;RegisterShellHookWindow&quot;, UInt,hWnd )
        ///MsgNum := DllCall( &quot;RegisterWindowMessage&quot;, Str,&quot;SHELLHOOK&quot; )
        ///OnMessage( MsgNum, &quot;ShellMessage&quot; )
        ///Return
        ///
        ///ShellMessage(wParam,lParam) 
        ///{
        ///	If (wParam = 1 ) ; HSHELL_WINDOWCREATED := 1
        ///	{
        ///		WinGetClass, WinClass, ahk_id %lParam%
        ///		if (WinClass = &quot;CabinetWClass&quot;) 
        ///		{
        ///			ControlGetPos, ,y1,,ha,ReBarWindow321, ahk_id %lParam%
        ///
        ///			SendMessage, 0x0082,,,ReBarWindow321, ahk_id [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string noaddressbar {
            get {
                return ResourceManager.GetString("noaddressbar", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #NoTrayIcon
        ///#NoEnv
        ///
        ///#If WinActive(&quot;ahk_class CabinetWClass&quot;)
        ///^c::Send {F10}{e}{c}
        ///
        ///#If WinActive(&quot;ahk_class CabinetWClass&quot;)
        ///^x::Send {F10}{e}{t}
        ///
        ///#If WinActive(&quot;ahk_class CabinetWClass&quot;)
        ///^v::Send {F10}{e}{p}
        ///
        ///#If WinActive(&quot;ahk_class CabinetWClass&quot;)
        ///F2::Send {Shift down}{F10 down}{F10 up}{Shift up}{m}
        ///
        ///#If WinActive(&quot;ahk_class CabinetWClass&quot;)
        ///!F4::Send {F10}{f}{c}
        ///
        ///#If WinActive(&quot;ahk_class CabinetWClass&quot;)
        ///!Left::Send {XButton1}
        ///
        ///#If WinActive(&quot;ahk_class CabinetWClass&quot;)
        ///!Right::Send {XButton2}
        ///
        ///#If WinActive [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string querohotkeys {
            get {
                return ResourceManager.GetString("querohotkeys", resourceCulture);
            }
        }
    }
}
