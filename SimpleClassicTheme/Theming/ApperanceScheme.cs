using System;
using System.Runtime.InteropServices;
using System.Threading;

using static SimpleClassicTheme.Theming.NativeTheming;

namespace SimpleClassicTheme.Theming
{
    [StructLayout(LayoutKind.Sequential, Size = 712, Pack = 1)]
    public struct ClassicAppearanceScheme
    {
        public int iVersion;
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
        public LogicalFont IconFont;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 29)]
        public ColorReference[] SchemeColors;

        public static ClassicAppearanceScheme FromBytes(byte[] bytes)
        {
            int size = Marshal.SizeOf(typeof(ClassicAppearanceScheme));
            IntPtr lpscheme = Marshal.AllocHGlobal(size);
            Marshal.Copy(bytes, 0, lpscheme, size);
            ClassicAppearanceScheme scheme = Marshal.PtrToStructure<ClassicAppearanceScheme>(lpscheme);
            Marshal.FreeHGlobal(lpscheme);
            return scheme;
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

    public enum SchemeMetric
    {
        WindowBorderWidth,
        PaddedWindowBorderWidth,
        ScrollBarWidth,
        ScrollBarHeight,
        CaptionWidth,
        CaptionHeight,
        SmallCaptionWidth,
        SmallCaptionHeight,
        MenuWidth,
        MenuHeight,
        HorizontalIconSpacing,
        VerticalIconSpacing,
        WrapIcons,
        IconSize,
        Unused,
    }

    public enum SchemeFont
    {
        CaptionFont,
        SmallCaptionFont,
        MenuFont,
        ToolTipFont,
        DialogFont,
        IconFont,
        Unused,
    }

    public class AppearanceScheme : IDisposable
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct BinaryData
        {
            public int version;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 14)]
            public int[] metrics;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public LogicalFont[] fonts;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 29)]
            public ColorReference[] colors;
        }

        BinaryData data = new BinaryData() { version = 3 };

        int[] metrics 
        { 
            get => data.metrics; 
            set 
            { 
                if (value.Length == 14) data.metrics = value; 
                else throw new ArgumentException("The array must be of length 14."); 
            } 
        }

        LogicalFont[] fonts
        {
            get => data.fonts;
            set
            {
                if (value.Length == 6) data.fonts = value;
                else throw new ArgumentException("The array must be of length 6.");
            }
        }

        ColorReference[] colors
        {
            get => data.colors;
            set
            {
                if (value.Length == 29) data.colors = value;
                else throw new ArgumentException("The array must be of length 29.");
            }
        }

        IntPtr[] _brushes;
        IntPtr[] brushes 
        { 
            get 
            {
                if (_brushes is null)
                {
                    _brushes = new IntPtr[29];
                    for (int i = 0; i < _brushes.Length; i++)
                        _brushes[i] = IntPtr.Zero;
                }
                return _brushes;
            } 
        }

        /// <summary>
        /// Retrieves the specified metric in the scheme.
        /// </summary>
        /// <param name="metric">A SchemeMetric specifying what metric to retrieve.</param>
        /// <returns>A 32-bit integer containing the requested metric.</returns>
        public int GetMetric(SchemeMetric metric) => metrics[(int)metric];

        /// <summary>
        /// Sets the specified metric in the scheme.
        /// </summary>
        /// <param name="metric">A SchemeMetric specifying what metric to set.</param>
        /// <param name="value">A 32-bit integer specifying the new value of the metric.</param>
        public void SetMetric(SchemeMetric metric, int value)
        {
            metrics[(int)metric] = value;
        }

        public int[] GetAllMetrics() => metrics;

        /// <summary>
        /// Retrieves the specified font in the scheme.
        /// </summary>
        /// <param name="metric">A SchemeFont specifying what font to retrieve.</param>
        /// <returns>A LogicalFont containing the requested font.</returns>
        public LogicalFont GetFont(SchemeFont font) => fonts[(int)font];

        /// <summary>
        /// Sets the specified font in the scheme.
        /// </summary>
        /// <param name="metric">A SchemeFont specifying what font to set.</param>
        /// <param name="value">A LogicalFont specifying the new value of the font.</param>
        public void SetFont(SchemeFont font, LogicalFont value)
        {
            fonts[(int)font] = value;
        }

        public LogicalFont[] GetAllFonts() => fonts;

        /// <summary>
        /// Retrieves the specified color in the scheme.
        /// </summary>
        /// <param name="color">A SchemeColor specifying what color to retrieve.</param>
        /// <returns>A ColorReference containing the requested RGB colors.</returns>
        public ColorReference GetColor(SchemeColor color) => colors[(int)color];

        /// <summary>
        /// Sets the specified color in the scheme.
        /// NOTE: If a brush handle for the specified color was retrieved, it will be deleted.
        /// </summary>
        /// <param name="color">A SchemeColor specifying what color to set.</param>
        /// <param name="value">A ColorReference specifying the new value of the color.</param>
        public void SetColor(SchemeColor color, ColorReference value)
        {
            if (brushes[(int)color] != IntPtr.Zero)
            {
                DeleteObject(brushes[(int)color]);
                brushes[(int)color] = IntPtr.Zero;
            }
            colors[(int)color] = value;
        }

        /// <summary>
        /// Retrieves all colors in the scheme.
        /// </summary>
        /// <returns>An array of type ColorReference with all colors in the scheme.</returns>
        public ColorReference[] GetAllColors() => colors;

        /// <summary>
        /// Creates a brush handle for the specified scheme color.
        /// NOTE: Disposing this AppearanceScheme deletes any handles retrieved.
        /// </summary>
        /// <param name="color">A SchemeColor specifying what color to get a brush handle for.</param>
        /// <returns>A brush handle for the specified SchemeColor.</returns>
        public IntPtr GetBrush(SchemeColor color)
        {
            if (brushes[(int)color] == IntPtr.Zero)
                brushes[(int)color] = CreateSolidBrush(colors[(int)color]);
            return brushes[(int)color];
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
        /// Applies the settings inside of this ApperanceScheme to the system.
        /// </summary>
        public void ApplyToSystem()
        {
            Thread t = new Thread(() => 
            {
                string[] regNames = { "Scrollbar", "Background", "ActiveTitle", "InactiveTitle", "Menu", "Window",
                                        "WindowFrame", "MenuText", "WindowText", "TitleText", "ActiveBorder", "Inactive Border",
                                        "AppWorkspace", "Hilight", "HilightText", "ButtonFace", "ButtonShadow", "GrayText", "ButtonText",
                                        "InactiveTitleText", "ButtonHilight", "ButtonDkShadow", "ButtonLight", "InfoText", "InfoWindow",
                                        "_", "HotTracking", "GradientActiveTitle", "GradientInactiveTitle" };
                int[] indeces = new int[29];
                for (int i = 0; i < 29; i++)
                {
                    indeces[i] = i;
                    if (regNames[i] != "_")
                        Microsoft.Win32.Registry.CurrentUser.SetValue($"Control Panel\\Colors\\{regNames[i]}", $"{colors[i].r} {colors[i].g} {colors[i].b}");
                }
                SetSysColors(29, indeces, colors);

                int simSize = Marshal.SizeOf(typeof(SystemIconMetricsStruct));
                SystemIconMetricsStruct sim = new SystemIconMetricsStruct();
                sim.cbSize = (uint)simSize;
                sim.iHorzSpacing = GetMetric(SchemeMetric.HorizontalIconSpacing);
                sim.iVertSpacing = GetMetric(SchemeMetric.VerticalIconSpacing);
                sim.iTitleWrap = GetMetric(SchemeMetric.WrapIcons);
                sim.lfFont = GetFont(SchemeFont.IconFont);

                IntPtr lpSim = Marshal.AllocHGlobal(simSize);
                Marshal.StructureToPtr(sim, lpSim, true);
                SystemParametersInfoW(SPI_SETICONMETRICS, (uint)simSize, lpSim, SPIF_UPDATEINIFILE);
                Marshal.FreeHGlobal(lpSim);

                int smSize = Marshal.SizeOf(typeof(SystemMetricsStruct));
                SystemMetricsStruct sm = new SystemMetricsStruct();
                sm.cbSize = (uint)smSize;
                sm.iBorderWidth = GetMetric(SchemeMetric.WindowBorderWidth);
                sm.iScrollWidth = GetMetric(SchemeMetric.ScrollBarWidth);
                sm.iScrollHeight = GetMetric(SchemeMetric.ScrollBarHeight);
                sm.iCaptionWidth = GetMetric(SchemeMetric.CaptionWidth);
                sm.iCaptionHeight = GetMetric(SchemeMetric.CaptionHeight);
                sm.iSmCaptionWidth = GetMetric(SchemeMetric.SmallCaptionWidth);
                sm.iSmCaptionHeight = GetMetric(SchemeMetric.CaptionHeight);
                sm.iMenuWidth = GetMetric(SchemeMetric.MenuWidth);
                sm.iMenuHeight = GetMetric(SchemeMetric.MenuHeight);
                sm.iPaddedBorderWidth = GetMetric(SchemeMetric.PaddedWindowBorderWidth);
                sm.lfCaptionFont = GetFont(SchemeFont.CaptionFont);
                sm.lfSmCaptionFont = GetFont(SchemeFont.SmallCaptionFont);
                sm.lfMenuFont = GetFont(SchemeFont.MenuFont);
                sm.lfStatusFont = GetFont(SchemeFont.ToolTipFont);
                sm.lfMessageFont = GetFont(SchemeFont.DialogFont);

                IntPtr lpSm = Marshal.AllocHGlobal(smSize);
                Marshal.StructureToPtr(sm, lpSm, true);
                SystemParametersInfoW(SPI_SETNONCLIENTMETRICS, (uint)smSize, lpSm, SPIF_UPDATEINIFILE); 
                Marshal.FreeHGlobal(lpSm);

                // Due to Windows being bugged, the code might never return, because it's waiting
                // on a result from a window message that is never handled. This shouldn't happen
                // if SPIF_SENDCHANGE isn't set, but for SPI_SETNONCLIENTMETRICS specifically, it
                // broadcasts the message anyway.
                // This isn't that big of a deal because the function will succeed in its purpose
                // but it does mean that this bit of native memory will stay allocated throughout
                // the applications lifespan. The application will not block because this code is
                // running on a separate thread which is abandoned after a few seconds.
                return;
            });
            t.Start();
            t.Join(5000);
            t.Interrupt();
            t.Abort();
        }

        public void SaveToRegistry(string schemeName)
        {
            int dataSize = Marshal.SizeOf(typeof(BinaryData));
            byte[] bytes = new byte[dataSize];
            IntPtr lpData = Marshal.AllocHGlobal(dataSize);
            Marshal.StructureToPtr(data, lpData, true);
            Marshal.Copy(lpData, bytes, 0, dataSize);
            Marshal.FreeHGlobal(lpData);
            Microsoft.Win32.Registry.SetValue("HKEY_CURRENT_USER\\Control Panel\\Appearance\\Schemes", schemeName, bytes);
        }

        /// <summary>
        /// Creates an AppearanceScheme from the current theme settings.
        /// </summary>
        /// <returns>An assembled AppearanceScheme object containing the current theme settings.</returns>
        public static AppearanceScheme FromSystem()
        {
            int smSize = Marshal.SizeOf(typeof(SystemMetricsStruct));
            IntPtr lpSm = Marshal.AllocHGlobal(smSize);
            Marshal.StructureToPtr(new SystemMetricsStruct { cbSize = (uint)smSize }, lpSm, true);
            SystemParametersInfoW(SPI_GETNONCLIENTMETRICS, (uint)smSize, lpSm, 0);
            SystemMetricsStruct metrics = Marshal.PtrToStructure<SystemMetricsStruct>(lpSm);
            Marshal.FreeHGlobal(lpSm);

            int simSize = Marshal.SizeOf(typeof(SystemIconMetricsStruct));
            IntPtr lpSim = Marshal.AllocHGlobal(simSize);
            Marshal.StructureToPtr(new SystemIconMetricsStruct { cbSize = (uint)simSize }, lpSim, true);
            SystemParametersInfoW(SPI_GETICONMETRICS, (uint)simSize, lpSim, 0);
            SystemIconMetricsStruct iconMetrics = Marshal.PtrToStructure<SystemIconMetricsStruct>(lpSim);
            Marshal.FreeHGlobal(lpSim);

            ColorReference[] colors = new ColorReference[29];
            for (int i = 0; i < 29; i++)
                colors[i] = GetSysColor(i);

            return new AppearanceScheme()
            {
                metrics = new[]
                {
                    metrics.iBorderWidth, metrics.iPaddedBorderWidth,
                    metrics.iScrollWidth, metrics.iScrollHeight,
                    metrics.iCaptionWidth, metrics.iCaptionHeight,
                    metrics.iSmCaptionWidth, metrics.iSmCaptionHeight,
                    metrics.iMenuWidth, metrics.iMenuHeight,
                    iconMetrics.iHorzSpacing, iconMetrics.iVertSpacing,
                    iconMetrics.iTitleWrap, 32,
                },
                fonts = new[]
                {
                    metrics.lfCaptionFont, metrics.lfSmCaptionFont,
                    metrics.lfMenuFont, metrics.lfStatusFont,
                    metrics.lfMessageFont, iconMetrics.lfFont,
                },
                colors = colors,
            };
        }

        /// <summary>
        /// Creates an AppearanceScheme from the specified scheme stored in registry.
        /// </summary>
        /// <param name="schemeName">The name of the scheme to retrieve from registry.</param>
        /// <returns>An AppearanceScheme containing the theme settings retrieved from the specified scheme from registry.</returns>
        public static AppearanceScheme FromRegistry(string schemeName)
        {
            if (schemeName is null)
                throw new ArgumentNullException("The scheme name supplied is null");
            byte[] bytes = (byte[])Microsoft.Win32.Registry.GetValue("HKEY_CURRENT_USER\\Control Panel\\Appearance\\Schemes", schemeName, new byte[] { 0 });
            if (bytes.Length == 1)
                throw new ArgumentException("The scheme name supplied is invalid");

            int version = BitConverter.ToInt32(bytes, 0);
            if (version == 2)
                return FromClassicAppearanceScheme(ClassicAppearanceScheme.FromBytes(bytes));

            int dataSize = Marshal.SizeOf(typeof(BinaryData));
            if (version != 3 || bytes.Length != dataSize)
                throw new NotSupportedException("The scheme specified is not supported.");

            IntPtr lpData = Marshal.AllocHGlobal(dataSize);
            Marshal.Copy(bytes, 0, lpData, dataSize);
            BinaryData data = Marshal.PtrToStructure<BinaryData>(lpData);
            Marshal.FreeHGlobal(lpData);

            return new AppearanceScheme() { data = data };
        }

        private static AppearanceScheme FromClassicAppearanceScheme(ClassicAppearanceScheme structure)
        {
            int simSize = Marshal.SizeOf(typeof(SystemIconMetricsStruct));
            IntPtr lpSim = Marshal.AllocHGlobal(simSize);
            Marshal.StructureToPtr(new SystemIconMetricsStruct { cbSize = (uint)simSize }, lpSim, true);
            SystemParametersInfoW(SPI_GETICONMETRICS, (uint)simSize, lpSim, 0);
            SystemIconMetricsStruct iconMetrics = Marshal.PtrToStructure<SystemIconMetricsStruct>(lpSim);
            Marshal.FreeHGlobal(lpSim);

            return new AppearanceScheme()
            {
                metrics = new[]
                {
                    structure.iBorderWidth, 0,
                    structure.iScrollWidth, structure.iScrollHeight,
                    structure.iCaptionWidth, structure.iCaptionHeight,
                    structure.iSmCaptionWidth, structure.iSmCaptionHeight,
                    structure.iMenuWidth, structure.iMenuHeight,
                    iconMetrics.iHorzSpacing, iconMetrics.iVertSpacing,
                    iconMetrics.iTitleWrap, 32,
                },
                fonts = new[]
                {
                    structure.lfCaptionFont, structure.lfSmCaptionFont,
                    structure.lfMenuFont, structure.lfStatusFont,
                    structure.lfMessageFont, structure.IconFont,
                },
                colors = structure.SchemeColors,
            };
        }

        ~AppearanceScheme() => Dispose();

        /// <summary>
        /// Cleans up the scheme and deletes all handles to created objects
        /// </summary>
        public void Dispose()
        {
            for (int i = 0; i < brushes.Length; i++)
                if (_brushes[i] != IntPtr.Zero)
                    DeleteObject(_brushes[i]);
        }
    }
}
