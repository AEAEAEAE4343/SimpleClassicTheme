<div align="center"><font size="5"># Simple Classic Theme</font></div><div><font size="5">
</font></div>Basically, I saw <a href="http://winclassic.boards.net/thread/413/reversibly-enable-disable-classic-powershell">this thread</a> which is an amazing solution for Classic Theme and decided I wanted to create a GUI for it. It's a simple .NET app which does the following things:
    <ul><li>Installs PowerShell 6 and the module required</li>    <li>Installs and configures Open-Shell and StartIsBack to emulate a Classic taskbar/start menu experience</li>    <li>Enables/Disables Classic Theme with a press of a button (or command)</li>    <li>Automatically enables Classic Theme on boot-up after explorer.exe has started (This makes sure all of the explorer bugs don't occur and that explorer behaves kinda like an immersive app)</li>    <li>Configure Classic Theme (requires disabling Defender, but thats implemented too)</li></ul>
<div>Please give me suggestions on how to improve this app (eg. registry tweaks for buggy things)</div>
<div align="center"><b>Screenshot of GUI running on Windows Insider Preview (post-1903):</b></div><div align="center"><b></b>
</div><img src="https://i.imgur.com/PDkTtsB.png" alt="sc" style="max-width:100%;">

<div align="center"><b><font size="3">Command Line Usage</font></b>
</div>
<code>SimpleClassicTheme.exe [OPERATION] {ARGS..}</code>
<b>Operations:</b>
    <ul><li>/enable This enables the classic theme</li>	<li>/disable This disables the classic theme</li>	<li>/configure This opens the classic theme control panel</li>
</ul><b>Arguments:</b>
<ul><li>-t, --enable-taskbar Enables/Disables classic taskbar as well (Depending on operation)</li></ul><b>
</b><div align="center"><b><font size="3">TODO</font></b>
    </div><ul><li><div align="left">Actually automatically install PowerShell and OpenShell</div></li>	<li><div align="left">Make it not use PowerShell and call API's directly</div></li>
</ul>