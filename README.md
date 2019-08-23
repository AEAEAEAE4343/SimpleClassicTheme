# Simple Classic Theme
Basically, I saw <a href="http://winclassic.boards.net/thread/413/reversibly-enable-disable-classic-powershell">this thread</a> which is an amazing solution for Classic Theme and decided I wanted to create a GUI for it. It's a simple .NET app which does the following things:
<ul>
    <li>Installs PowerShell 6 and the module required</li>
    <li>Installs and configures Open-Shell and StartIsBack to emulate a Classic taskbar/start menu experience</li>
    <li>Enables/Disables Classic Theme with a press of a button</li>
    <li>Automatically enables Classic Theme on boot-up after explorer.exe has started (This makes sure all of the explorer bugs don't occur and that explorer behaves kinda like an immersive app)</li>
    <li>Configure Classic Theme (requires disabling Defender, but thats implemented too)</li>
</ul>
Please give me suggestions on how to improve this app (eg. registry tweaks for buggy things)
I'm thinking about how to implement taskbar but we'll have to see. (suggestions on that are welcome too)

<b>UPDATE:</b> Now also changes you taskbar into this:
<img src="https://i.imgur.com/ocRzYt4.png" style="max-width:100%;" alt="image of taskbar">

Screenshot of GUI running on Windows Insider Preview (post-1903):
<img style="max-width:100%;" alt="sc" src="https://i.imgur.com/mfy1h02.png">
TODO:
<ul>
    <li>Actually automatically install PowerShell and OpenShell</li>
    <li>Fix weird coloring of taskbar (see bottom right of picture)</li>
    <li><strike>Classic Taskbar</strike></li>
    <li>Force classic theme on buttons too</li>
</ul>
