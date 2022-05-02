using System;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using static SimpleClassicTheme.Theming.NativeTheming;

namespace SimpleClassicTheme.Theming
{
    internal static class NativeTheming
    {
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateFontIndirectW(ref LogicalFont lfFont);
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateRectRgnIndirect(ref RECT rect);
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateSolidBrush(ColorReference color);
        [DllImport("gdi32.dll")]
        public static extern int DeleteObject(IntPtr color);
        [DllImport("gdi32.dll")]
        public static extern int GetDeviceCaps(IntPtr hDc, int index);
        [DllImport("gdi32.dll")]
        public static extern int GetTextExtentPoint32(IntPtr hDc, string lpString, int c, ref Size pSize);
        [DllImport("gdi32.dll")]
        public static extern int SelectClipRgn(IntPtr hDc, IntPtr hRgn);
        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hDc, IntPtr obj);
        [DllImport("gdi32.dll")]
        public static extern ColorReference SetTextColor(IntPtr hDc, ColorReference color);

        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern ColorReference GetSysColor(int nIndex);
        [DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDc);
        [DllImport("user32.dll")]
        public static extern int SetSysColors(int cElements, int[] lpaElements, ColorReference[] lpaRgbValues);
        [DllImport("user32.dll")]
        public static extern bool SystemParametersInfoW(uint uiAction, uint uiParam, IntPtr pvParam, uint fWinIni);

        // TODO: Abstract Win32 calls
        // DWORD WINAPI DrawCaptionTemp(HWND hwnd, HDC hDC, LPRECT lprect, HFONT hFont, HICON hIcon, LPTSTR caption, DWORD flags)
        [DllImport("user32.dll")]
        public static extern bool DrawCaptionTemp(IntPtr hWnd, IntPtr hDc, ref RECT lpRect, IntPtr hFont, IntPtr hIcon, string caption, uint flags);
        // DWORD_PTR WINAPI SetSysColorsTemp(const ColorReference *, const HBRUSH *, DWORD_PTR);
        [DllImport("user32.dll")]
        public static extern int SetSysColorsTemp(ColorReference[] lpaElements, IntPtr[] lpaRgbValues, int cElements);
        // DWORD WINAPI DrawMenuBarTemp(HWND hwnd, HDC hDC, LPRECT lprect, HMENU hMenu, HFONT hFont);
        //[DllImport("user32.dll")]
        //static extern uint DrawMenuBarTemp(IntPtr hWnd, IntPtr hDc, IntPtr lpRect, IntPtr hMenu, IntPtr hFont);

        [DllImport("user32.dll")]
        public static extern bool DrawEdge(IntPtr hDc, ref RECT lpRect, int edge, int flags);
        [DllImport("user32.dll")]
        public static extern bool DrawFrameControl(IntPtr hDc, ref RECT lpRect, int cFrameType, int cFrameState);
        [DllImport("user32.dll")]
        public static extern bool DrawText(IntPtr hDc, string text, int c, ref RECT lpRect, uint format);
        [DllImport("user32.dll")]
        public static extern bool FillRect(IntPtr hDc, ref RECT lpRect, IntPtr brush);
        [DllImport("user32.dll")]
        public static extern bool FrameRect(IntPtr hDc, ref RECT lpRect, IntPtr brush);
        [DllImport("user32.dll")]
        public static extern int RedrawWindow(IntPtr hWnd, IntPtr lprcUpdate, IntPtr hrgnUpdate, uint flags);

        public static uint RDW_INVALIDATE = 0x0001;
        public static uint RDW_ALLCHILDREN = 0x0080;
        public static uint RDW_UPDATENOW = 0x0100;
        public static uint RDW_FRAME = 0x0400;

        public static int BDR_RAISEDOUTER = 0x0001;
        public static int BDR_SUNKENOUTER = 0x0002;
        public static int BDR_RAISEDINNER = 0x0004;
        public static int BDR_SUNKENINNER = 0x0008;

        public static int BF_MIDDLE = 0x0800;
        public static int BF_ADJUST = 0x2000;
        public static int BF_RECT = 0x0001 | 0x0002 | 0x0004 | 0x0008;

        public static int DFC_CAPTION = 1;
        public static int DFCS_CAPTIONCLOSE = 0;
        public static int DFCS_CAPTIONMIN = 1;
        public static int DFCS_CAPTIONMAX = 2;
        public static int DFC_SCROLL = 3;
        public static int DFCS_SCROLLUP = 0;
        public static int DFCS_SCROLLDOWN = 1;
        public static int DFCS_SCROLLLEFT = 2;
        public static int DFCS_SCROLLRIGHT = 3;
        public static int DFCS_SCROLLSIZEGRIP = 8;
        public static int DFCS_INACTIVE = 0x0100;

        public static uint DC_SMALLCAP = 0x0002;
        public static uint DC_BUTTONS = 0x1000;
        public static uint DC_ACTIVE = 0x0001;
        public static uint DC_ICON = 0x0004;
        public static uint DC_TEXT = 0x0008;
        public static uint DC_GRADIENT = 0x0020;

        public static uint DT_TOP = 0x0000;
        public static uint DT_LEFT = 0x0000;
        public static uint DT_CENTER = 0x0001;
        public static uint DT_VCENTER = 0x0004;
        public static uint DT_SINGLELINE = 0x0020;
        public static uint DT_NOCLIP = 0x0100;

        public const int LOGPIXELSY = 90;

        public const uint SPI_GETNONCLIENTMETRICS = 0x0029;
        public const uint SPI_SETNONCLIENTMETRICS = 0x002A;
        public const uint SPI_GETICONMETRICS = 0x002D;
        public const uint SPI_SETICONMETRICS = 0x002E;
        public const uint SPIF_UPDATEINIFILE = 1;
        public const uint SPIF_SENDWININICHANGE = 2;
    }

    [StructLayout(LayoutKind.Sequential, Size = 16)]
    struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;

        public int Width
        {
            get => Right - Left;
            set => Right = Left + value;
        }
        public int Height
        {
            get => Bottom - Top;
            set => Bottom = Top + value;
        }

        public Size Size 
        {
            get => new Size(Width, Height);
            set
            {
                Width = value.Width;
                Height = value.Height;
            }
        }

        public Point Location 
        {
            get => new Point(Left, Top);
            set
            {
                Size temp = Size;
                Left = value.X; Top = value.Y;
                Width = temp.Width; Height = temp.Height;
            }
        }

        public void Inflate(int by)
        {
            Left -= by; Right += by;
            Top -= by; Bottom += by;
        }

        public void Add(RECT rct)
        {
            Left += rct.Left;
            Right += rct.Right;
            Top += rct.Top;
            Bottom += rct.Bottom;
        }

        public RECT(int l, int t, int w, int h)
        {
            Left = l; Top = t; Right = l + w; Bottom = t + h;
        }
        public Rectangle ToRectangle() => new Rectangle(Left, Top, Width, Height);
        public static RECT FromRectangle(Rectangle r) => new RECT(r.Left, r.Top, r.Right, r.Bottom);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LogicalFont
    {
        public int lfHeight;
        public int lfWidth;
        public int lfEscapement;
        public int lfOrientation;
        public int lfWeight;
        public byte lfItalic;
        public byte lfUnderline;
        public byte lfStrikeOut;
        public byte lfCharSet;
        public byte lfOutPrecision;
        public byte lfClipPrecision;
        public byte lfQuality;
        public byte lfPitchAndFamily;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public byte[] lfFaceName;

        public string GetName()
        {
            string temp = Encoding.Unicode.GetString(lfFaceName);
            return temp.Remove(temp.IndexOf('\0'));
        }

        public void SetName(string name)
        {
            lfFaceName = Encoding.Unicode.GetBytes(name.Take(63).Append('\x00').ToArray());
        }

        public int GetSize()
        {
            IntPtr hDc = GetDC(IntPtr.Zero);
            int size = (int)Math.Abs(Math.Round(lfHeight * 72 / (double)GetDeviceCaps(hDc, LOGPIXELSY)));
            ReleaseDC(IntPtr.Zero, hDc);
            return size;
        }

        public void SetSize(int size)
        {
            IntPtr hDc = GetDC(IntPtr.Zero);
            lfHeight = -(int)Math.Abs(Math.Round(size * GetDeviceCaps(hDc, LOGPIXELSY) / 72.0));
            ReleaseDC(IntPtr.Zero, hDc);
        }

        public IntPtr GetHandle()
        {
            return CreateFontIndirectW(ref this);
        }

        public static void FreeHandle(IntPtr font)
        {
            DeleteObject(font);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SystemMetricsStruct
    {
        public uint cbSize;
        public int iBorderWidth;
        public int iScrollWidth;
        public int iScrollHeight;
        public int iCaptionWidth;
        public int iCaptionHeight;
        public LogicalFont lfCaptionFont;
        public int iSmCaptionWidth;
        public int iSmCaptionHeight;
        public LogicalFont lfSmCaptionFont;
        public int iMenuWidth;
        public int iMenuHeight;
        public LogicalFont lfMenuFont;
        public LogicalFont lfStatusFont;
        public LogicalFont lfMessageFont;
        public int iPaddedBorderWidth;
    }

    public struct SystemIconMetricsStruct
    {
        public uint cbSize;
        public int iHorzSpacing;
        public int iVertSpacing;
        public int iTitleWrap;
        public LogicalFont lfFont;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ColorReference
    {
        public byte r;
        public byte g;
        public byte b;
        public byte reserved;

        public Color ToColor() => Color.FromArgb(r, g, b);
        public static ColorReference FromColor(Color c) => new ColorReference() { r = c.R, g = c.G, b = c.B, reserved = 0 };
        public int Integer => r + (g << 8) + (b << 16);
    }
}