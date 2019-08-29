# SimpleClassicTheme<div><font size="5">
</font></div>Basically, I saw <a href="http://winclassic.boards.net/thread/413/reversibly-enable-disable-classic-powershell">this thread</a> which is an amazing solution for Classic Theme and decided I wanted to create a GUI for it. It's a simple .NET app which does the following things:<ul><li>Installs and configures Open-Shell and StartIsBack to emulate a Classic taskbar/start menu experience</li>    <li>Enables/Disables Classic Theme with a press of a button (or command)</li>    <li>Automatically enables Classic Theme on boot-up after explorer.exe has started (This makes sure all of the explorer bugs don't occur and that explorer behaves kinda like an immersive app)</li>    <li>Configure Classic Theme (requires disabling Defender, but thats implemented too)</li></ul>
<div align="center"><b><font size="3">How It works</font></b></div><div align="center">Winlogon uses a shared memory section for handling themes. By removing access to this memory section the Winlogon process can no longer manage themes so the default theme is used, the Classic Theme. It's better then suspending the Desktop Window Manager because 
</div>
<div align="center"><b>Screenshot of GUI running on Windows Insider Preview (post-1903):</b></div><div align="center"><b></b>
</div><img alt="sc" src="https://i.imgur.com/PDkTtsB.png" style="max-width:100%;">

<div align="center"><b><font size="3">Command Line Usage</font></b>
</div>
<code>SimpleClassicTheme.exe <OPERATION> {ARGS..}</code><br>
<b>Operations:</b>
    <ul><li>/enable This enables the classic theme</li>	<li>/disable This disables the classic theme</li>	<li>/configure This opens the classic theme control panel</li>
</ul><b>Arguments:</b>
<ul><li>-t, --enable-taskbar Enables/Disables classic taskbar as well (Depending on operation)</li></ul><b>
</b><div align="center"><b><font size="3">TODO</font></b>
    </div><ul><li><div align="left">Actually automatically install OpenShell
</div></li><li><div align="left">Make File Explorer classic as well
</div></li></ul><div align="center"><font size="3"><b>Download</b></font></div><div align="center"><font size="3">
</font></div><div align="center"><font size="3"><font size="2">The application can be downloaded through my Github <a href="https://github.com/AEAEAEAE4343/SimpleClassicTheme/releases/download/b6/SimpleClassicTheme.exe">here</a> (b6)</font>
</font></div>
