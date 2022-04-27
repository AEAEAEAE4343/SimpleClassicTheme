using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SimpleClassicTheme.Theming
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct COLORREF
    {
        public byte r;
        public byte g;
        public byte b;
        public byte reserved;

        public static COLORREF FromColor(Color c)
        {
            return new COLORREF() { r = c.R, g = c.G, b = c.B, reserved = 0 };
        }
    }

    public enum SchemeColor
    {
        ScrollBarColor,
        DesktopColor,
        ActiveCaptionColor,
        InactiveCaptionColor,
        MenuColor,
        WindowColor,
        WindowFrameColor,
        MenuFontColor,
        WindowFontColor,
        ActiveCaptionFontColor,
        ActiveWindowBorderColor,
        InactiveWindowBorderColor,
        ApplicationBackgroundColor,
        SelectedItemsColor,
        SelectedItemsFontColor,
        ThreeDimensionalObjectColor,
        ThreeDimensionalObjectShadowColor,
        DisabledFontColor,
        ThreeDimensionalObjectFontColor,
        InactiveCaptionFontColor,
        ThreeDimensionalObjectHighlightColor,
        ThreeDimensionalObjectDarkShadowColor,
        ThreeDimensionalObjectLightColor,
        ToolTipFontColor,
        ToolTipColor,
        Unused,
        HyperlinkColor,
        ActiveCaptionGradientColor,
        InactiveCaptionGradientColor,
    }

    public class AppearanceScheme : IDisposable
    {
        [DllImport("gdi32.dll")]
        static extern IntPtr CreateFontIndirectW(ref LOGFONTW lfFont);
        [DllImport("gdi32.dll")]
        static extern IntPtr CreateSolidBrush(COLORREF color);
        [DllImport("gdi32.dll")]
        static extern int DeleteObject(IntPtr color);

        [DllImport("user32.dll")]
        static extern COLORREF GetSysColor(int nIndex);
        [DllImport("user32.dll")]
        static extern bool SystemParametersInfoW(uint uiAction, uint uiParam, IntPtr pvParam, uint fWinIni);


        const uint SPI_GETNONCLIENTMETRICS = 0x0029;
        const uint SPI_GETICONMETRICS = 0x002D;

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct LOGFONTW
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

            public IntPtr GetFont()
            {
                return CreateFontIndirectW(ref this);
            }

            public static void FreeFont(IntPtr font)
            {
                DeleteObject(font);
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct WindowMetricData
        {
            public uint cbSize;
            public int iBorderWidth;
            public int iScrollWidth;
            public int iScrollHeight;
            public int iCaptionWidth;
            public int iCaptionHeight;
            public LOGFONTW lfCaptionFont;
            public int iSmCaptionWidth;
            public int iSmCaptionHeight;
            public LOGFONTW lfSmCaptionFont;
            public int iMenuWidth;
            public int iMenuHeight;
            public LOGFONTW lfMenuFont;
            public LOGFONTW lfStatusFont;
            public LOGFONTW lfMessageFont;

            public static WindowMetricData FromSystem()
            {
                int size = Marshal.SizeOf(typeof(WindowMetricData));
                WindowMetricData ncmetrics = new WindowMetricData();
                ncmetrics.cbSize = (uint)size;
                IntPtr lpncmetrics = Marshal.AllocHGlobal(size);
                Marshal.StructureToPtr(ncmetrics, lpncmetrics, true);
                SystemParametersInfoW(SPI_GETNONCLIENTMETRICS, (uint)size, lpncmetrics, 0);
                ncmetrics = Marshal.PtrToStructure<WindowMetricData>(lpncmetrics);
                Marshal.FreeHGlobal(lpncmetrics);
                return ncmetrics;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct ICONMETRICSW
        {
            public uint cbSize;
            public int iHorzSpacing;
            public int iVertSpacing;
            public int iTitleWrap;
            public LOGFONTW lfFont;

            public static ICONMETRICSW FromSystem()
            {
                int size = Marshal.SizeOf(typeof(ICONMETRICSW));
                ICONMETRICSW iconmetrics = new ICONMETRICSW();
                iconmetrics.cbSize = (uint)size;
                IntPtr lpiconmetrics = Marshal.AllocHGlobal(size);
                Marshal.StructureToPtr(iconmetrics, lpiconmetrics, true);
                SystemParametersInfoW(SPI_GETICONMETRICS, (uint)size, lpiconmetrics, 0);
                iconmetrics = Marshal.PtrToStructure<ICONMETRICSW>(lpiconmetrics);
                Marshal.FreeHGlobal(lpiconmetrics);
                return iconmetrics;
            }
        }

        [StructLayout(LayoutKind.Sequential, Size = 712, Pack = 1)]
        private struct ColorSchemeStruct
        {
            public int padding;
            public WindowMetricData NonClientMetrics;
            public LOGFONTW IconFont;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 29)]
            public COLORREF[] SchemeColors;

            public static ColorSchemeStruct FromSystem()
            {
                ColorSchemeStruct str = new ColorSchemeStruct();
                str.padding = 2;
                str.NonClientMetrics = WindowMetricData.FromSystem();
                str.IconFont = ICONMETRICSW.FromSystem().lfFont;
                str.SchemeColors = new COLORREF[29];
                for (int i = 0; i < str.SchemeColors.Length; i++)
                    str.SchemeColors[i] = GetSysColor(i);

                return str;
            }

            public static ColorSchemeStruct FromRegistry(string schemeName)
            {
                if (schemeName is null)
                    throw new ArgumentNullException("The scheme name supplied is null");
                byte[] bytes = (byte[])Microsoft.Win32.Registry.GetValue("HKEY_CURRENT_USER\\Control Panel\\Appearance\\Schemes", schemeName, new byte[]{ 0 });
                if (bytes.Length == 1)
                    throw new ArgumentException("The scheme name supplied is invalid");

                int size = Marshal.SizeOf(typeof(ColorSchemeStruct));
                IntPtr lpscheme = Marshal.AllocHGlobal(size);
                Marshal.Copy(bytes, 0, lpscheme, size);
                ColorSchemeStruct scheme = Marshal.PtrToStructure<ColorSchemeStruct>(lpscheme);
                Marshal.FreeHGlobal(lpscheme);
                return scheme;
            }
        }

        public WindowMetricData WindowMetrics;
        public LOGFONTW IconFont;

        COLORREF[] Colors = new COLORREF[29];
        IntPtr[] brushes;
        IntPtr[] Brushes 
        { 
            get 
            {
                if (brushes is null)
                {
                    brushes = new IntPtr[29];
                    for (int i = 0; i < brushes.Length; i++)
                        brushes[i] = IntPtr.Zero;
                }
                return brushes;
            } 
        }

        /// <summary>
        /// Retrieves the specified color in the scheme.
        /// </summary>
        /// <param name="color">A SchemeColor specifying what color to retrieve.</param>
        /// <returns>A COLORREF containing the requested RGB colors.</returns>
        public COLORREF GetColor(SchemeColor color) => Colors[(int)color];

        /// <summary>
        /// Sets the specified color in the scheme.
        /// NOTE: If a brush handle for the specified color was retrieved, it will be deleted.
        /// </summary>
        /// <param name="color">A SchemeColor specifying what color to set.</param>
        /// <param name="value">A COLORREF specifying the new value of the color.</param>
        public void SetColor(SchemeColor color, COLORREF value)
        {
            if (Brushes[(int)color] != IntPtr.Zero)
                DeleteObject(Brushes[(int)color]);
            Colors[(int)color] = value;
        }

        /// <summary>
        /// Retrieves all colors in the scheme.
        /// </summary>
        /// <returns>An array of type COLORREF with all colors in the scheme.</returns>
        public COLORREF[] GetAllColors() => Colors;

        /// <summary>
        /// Creates a brush handle for the specified scheme color.
        /// NOTE: Disposing this AppearanceScheme deletes any handles retrieved.
        /// </summary>
        /// <param name="color">A SchemeColor specifying what color to get a brush handle for.</param>
        /// <returns>A brush handle for the specified SchemeColor.</returns>
        public IntPtr GetBrush(SchemeColor color)
        {
            if (Brushes[(int)color] == IntPtr.Zero)
                Brushes[(int)color] = CreateSolidBrush(Colors[(int)color]);
            return Brushes[(int)color];
        }

        /// <summary>
        /// Creates brush handles for all colors in the scheme.
        /// NOTE: Disposing this AppearanceScheme deletes any handles retrieved.
        /// </summary>
        /// <returns>An array with brush handles for all colors in the scheme.</returns>
        public IntPtr[] GetAllBrushes()
        {
            for (int i = 0; i < 29; i++)
                GetBrush((SchemeColor)i);
            return brushes;
        }

        /// <summary>
        /// Creates an AppearanceScheme from the current theme settings.
        /// </summary>
        /// <returns>An assembled AppearanceScheme object containing the current theme settings.</returns>
        public static AppearanceScheme FromSystem()
        {
            return FromStruct(ColorSchemeStruct.FromSystem());
        }

        /// <summary>
        /// Creates an AppearanceScheme from the specified scheme stored in registry.
        /// </summary>
        /// <param name="schemeName">The name of the scheme to retrieve from registry.</param>
        /// <returns>An AppearanceScheme containing the theme settings retrieved from the specified scheme from registry.</returns>
        public static AppearanceScheme FromRegistry(string schemeName)
        {
            return FromStruct(ColorSchemeStruct.FromRegistry(schemeName));
        }

        private static AppearanceScheme FromStruct(ColorSchemeStruct structure)
        {
            AppearanceScheme scheme = new AppearanceScheme();
            scheme.Colors = structure.SchemeColors;
            scheme.WindowMetrics = structure.NonClientMetrics;
            scheme.IconFont = structure.IconFont;
            return scheme;
        }

        /// <summary>
        /// Cleans up the scheme and deletes all handles to created objects
        /// </summary>
        public void Dispose()
        {
            for (int i = 0; i < Brushes.Length; i++)
                if (Brushes[i] != IntPtr.Zero)
                    DeleteObject(Brushes[i]);
        }
    }
}
