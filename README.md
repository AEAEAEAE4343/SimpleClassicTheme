# Simple Classic Theme
Basically, I saw <a href="http://winclassic.boards.net/thread/413/reversibly-enable-disable-classic-powershell">this thread</a> which is an amazing solution for Classic Theme and decided I wanted to create a GUI for it. It's a simple .NET app which does the following things:

    Installs PowerShell 6 and the module required
    Enables/Disables Classic Theme with a press of a button
    Automatically enables Classic Theme on boot-up after explorer.exe has started (Windows Explorer is buggy with Classic Theme imo)
    Configure Classic Theme (requires disabling Defender, but thats implemented too)

Please give me suggestions on how to improve this app (eg. registry tweaks for buggy things)
I'm thinking about how to implement taskbar but we'll have to see. (suggestions on that are welcome too)

Screenshot of GUI running on Windows Insider Preview (post-1903):
<img src="https://i.imgur.com/mfy1h02.png" style="max-width:100%;" alt="sc">
