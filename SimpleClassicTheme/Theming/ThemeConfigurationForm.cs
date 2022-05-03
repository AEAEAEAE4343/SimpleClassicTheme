using Microsoft.VisualBasic;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using static SimpleClassicTheme.Theming.NativeTheming;

namespace SimpleClassicTheme.Theming
{
    public partial class ThemeConfigurationForm : SystemMenuForm
    {
        private class UIAppearanceBinding
        {
            public string ItemName = "";
            public SchemeColor PrimaryColor = SchemeColor.Unused;
            public SchemeColor SecondaryColor = SchemeColor.Unused;
            public SchemeColor FontColor = SchemeColor.Unused;
            public SchemeMetric[] SizeMetrics = new SchemeMetric[0];
            public SchemeFont FontMetric = SchemeFont.Unused;

            public ColorReference GetPrimaryColor(AppearanceScheme scheme) => scheme.GetColor(PrimaryColor);
            public void SetPrimaryColor(AppearanceScheme scheme, ColorReference value) => scheme.SetColor(PrimaryColor, value);
            public ColorReference GetSecondaryColor(AppearanceScheme scheme) => scheme.GetColor(SecondaryColor);
            public void SetSecondaryColor(AppearanceScheme scheme, ColorReference value) => scheme.SetColor(SecondaryColor, value);
            public ColorReference GetFontColor(AppearanceScheme scheme) => scheme.GetColor(FontColor);
            public void SetFontColor(AppearanceScheme scheme, ColorReference value) => scheme.SetColor(FontColor, value);

            public LogicalFont GetFont(AppearanceScheme scheme) => scheme.GetFont(FontMetric);
            public void SetFont(AppearanceScheme scheme, LogicalFont font) => scheme.SetFont(FontMetric, font);

            public int GetSize(AppearanceScheme scheme) => scheme.GetMetric(SizeMetrics[0]);
            public void SetSize(AppearanceScheme scheme, int value)
            {
                foreach (SchemeMetric metric in SizeMetrics)
                    scheme.SetMetric(metric, value);
            }

            public override string ToString() => ItemName;
        }

        private List<UIAppearanceBinding> Bindings = new List<UIAppearanceBinding>()
        {
            new UIAppearanceBinding() { ItemName = "3D Objects", PrimaryColor = SchemeColor.ThreeDimensionalObjectColor, FontColor = SchemeColor.ThreeDimensionalObjectFontColor },
            new UIAppearanceBinding() { ItemName = "Active Title Bar", PrimaryColor = SchemeColor.ActiveCaptionColor, SecondaryColor = SchemeColor.ActiveCaptionGradientColor, FontColor = SchemeColor.ActiveCaptionFontColor, SizeMetrics = new[] { SchemeMetric.CaptionHeight, SchemeMetric.CaptionWidth }, FontMetric = SchemeFont.CaptionFont },
            new UIAppearanceBinding() { ItemName = "Active Window Border", PrimaryColor = SchemeColor.ActiveWindowBorderColor, SizeMetrics = new []{ SchemeMetric.WindowBorderWidth } },
            new UIAppearanceBinding() { ItemName = "Caption Buttons", SizeMetrics = new[] { SchemeMetric.CaptionWidth, SchemeMetric.CaptionHeight } },
            new UIAppearanceBinding() { ItemName = "Desktop", PrimaryColor = SchemeColor.DesktopColor },
            new UIAppearanceBinding() { ItemName = "Icon", SizeMetrics = new [] { SchemeMetric.IconSize }, FontMetric = SchemeFont.IconFont },
            new UIAppearanceBinding() { ItemName = "Icon Spacing (Horizontal)", SizeMetrics = new [] { SchemeMetric.HorizontalIconSpacing } },
            new UIAppearanceBinding() { ItemName = "Icon Spacing (Vertical)", SizeMetrics = new [] { SchemeMetric.VerticalIconSpacing } },
            new UIAppearanceBinding() { ItemName = "Inactive Title Bar", PrimaryColor = SchemeColor.InactiveCaptionColor, SecondaryColor = SchemeColor.InactiveCaptionGradientColor, FontColor = SchemeColor.InactiveCaptionFontColor, SizeMetrics = new[] { SchemeMetric.CaptionHeight, SchemeMetric.CaptionWidth }, FontMetric = SchemeFont.CaptionFont },
            new UIAppearanceBinding() { ItemName = "Inactive Window Border", PrimaryColor = SchemeColor.InactiveWindowBorderColor, SizeMetrics = new []{ SchemeMetric.WindowBorderWidth } },
            new UIAppearanceBinding() { ItemName = "MDI Application Background", PrimaryColor = SchemeColor.ApplicationBackgroundColor },
            new UIAppearanceBinding() { ItemName = "Menu", PrimaryColor = SchemeColor.MenuColor, FontColor = SchemeColor.MenuFontColor, SizeMetrics = new [] { SchemeMetric.MenuHeight, SchemeMetric.MenuWidth }, FontMetric = SchemeFont.MenuFont },
            new UIAppearanceBinding() { ItemName = "Message Box / Dialog Window", FontColor = SchemeColor.WindowFontColor, FontMetric = SchemeFont.DialogFont },
            new UIAppearanceBinding() { ItemName = "Scrollbar", PrimaryColor = SchemeColor.ScrollBarColor, SizeMetrics = new [] { SchemeMetric.ScrollBarWidth, SchemeMetric.ScrollBarHeight } },
            new UIAppearanceBinding() { ItemName = "Selected Items", PrimaryColor = SchemeColor.SelectedItemsColor, FontColor = SchemeColor.SelectedItemsFontColor, SizeMetrics = new [] { SchemeMetric.MenuHeight, SchemeMetric.MenuWidth }, FontMetric = SchemeFont.MenuFont },
            new UIAppearanceBinding() { ItemName = "ToolTip", PrimaryColor = SchemeColor.ToolTipColor, FontColor = SchemeColor.ToolTipFontColor, FontMetric = SchemeFont.ToolTipFont },
            new UIAppearanceBinding() { ItemName = "Window", PrimaryColor = SchemeColor.WindowColor, FontColor = SchemeColor.WindowFontColor },
            
        };

        AppearanceScheme appearanceScheme;
        bool loadingBindings = false;
        Bitmap toolWindowButtons = SCT.ResourceFetcher.ThemePreviewToolWindowIcons;

        public ThemeConfigurationForm()
        {
            InitializeComponent();

            string[] names = Registry.CurrentUser.CreateSubKey("Control Panel").CreateSubKey("Appearance").CreateSubKey("Schemes").GetValueNames();
            comboBoxScheme.Items.AddRange(names);

            appearanceScheme = AppearanceScheme.FromSystem();

            panelWindowPreview.Invalidate();
            panelWindowPreview.Refresh();

            label13.Text = SCT.VersionString;

            GenerateLists();
        }

        private void GenerateLists()
        {
            comboBoxItem.Items.Clear();
            comboBoxFont.Items.Clear();

            foreach (UIAppearanceBinding binding in Bindings)
                comboBoxItem.Items.Add(binding);

            using (InstalledFontCollection col = new InstalledFontCollection())
                foreach (FontFamily fa in col.Families)
                    comboBoxFont.Items.Add(fa.Name);

            comboBoxItem.SelectedIndex = 0;
            comboBoxFont.SelectedIndex = -1;
        }

        private void comboBoxScheme_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                appearanceScheme = AppearanceScheme.FromRegistry(comboBoxScheme.Items[comboBoxScheme.SelectedIndex].ToString());
            }
            catch (ArgumentException)
            {
                appearanceScheme = AppearanceScheme.FromSystem();
            }

            panelWindowPreview.Invalidate();
            panelWindowPreview.Refresh();

            comboBoxItem_SelectedIndexChanged(sender, e);
        }

        private void comboBoxItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadingBindings = true;
            UIAppearanceBinding binding = (UIAppearanceBinding)comboBoxItem.Items[comboBoxItem.SelectedIndex];
            
            bool primaryColor = binding.PrimaryColor != SchemeColor.Unused;
            buttonColorPrimary.Enabled = primaryColor;
            buttonColorPrimary.Color = primaryColor ? appearanceScheme.GetColor(binding.PrimaryColor).ToColor() : SystemColors.Control;

            bool secondaryColor = binding.SecondaryColor != SchemeColor.Unused;
            buttonColorSecondary.Enabled = secondaryColor;
            buttonColorSecondary.Color = secondaryColor ? appearanceScheme.GetColor(binding.SecondaryColor).ToColor() : SystemColors.Control;

            bool fontColor = binding.FontColor != SchemeColor.Unused;
            buttonColorFont.Enabled = fontColor;
            buttonColorFont.Color = fontColor ? appearanceScheme.GetColor(binding.FontColor).ToColor() : SystemColors.Control;

            if (binding.FontMetric != SchemeFont.Unused)
            {
                LogicalFont font = binding.GetFont(appearanceScheme);
                comboBoxFont.SelectedItem = font.GetName();
                upDownFontSize.Value = font.GetSize();
                checkBoxBold.Checked = font.lfWeight == 700;
                checkBoxItalic.Checked = font.lfItalic == 1;
                comboBoxFont.Enabled = upDownFontSize.Enabled = checkBoxBold.Enabled = checkBoxItalic.Enabled = true;
            }
            else
            {
                comboBoxFont.SelectedIndex = -1;
                upDownFontSize.Value = 0;
                checkBoxBold.Checked = false;
                checkBoxItalic.Checked = false;
                comboBoxFont.Enabled = upDownFontSize.Enabled = checkBoxBold.Enabled = checkBoxItalic.Enabled = false;
            }

            bool size = binding.SizeMetrics.Length != 0;
            upDownItemSize.Enabled = size;
            upDownItemSize.Value = size ? binding.GetSize(appearanceScheme) : 0;
            loadingBindings = false;
        }

        private void buttonColorPrimary_ColorChanged(object sender, EventArgs e)
        {
            if (!loadingBindings)
            {
                UIAppearanceBinding binding = (UIAppearanceBinding)comboBoxItem.Items[comboBoxItem.SelectedIndex];
                binding.SetPrimaryColor(appearanceScheme, ColorReference.FromColor(buttonColorPrimary.Color));
                RedrawImmediately();
            }
        }

        private void buttonColorSecondary_ColorChanged(object sender, EventArgs e)
        {
            if (!loadingBindings)
            {
                UIAppearanceBinding binding = (UIAppearanceBinding)comboBoxItem.Items[comboBoxItem.SelectedIndex];
                binding.SetSecondaryColor(appearanceScheme, ColorReference.FromColor(buttonColorSecondary.Color));
                RedrawImmediately();
            }
        }

        private void buttonColorFont_ColorChanged(object sender, EventArgs e)
        {
            if (!loadingBindings)
            {
                UIAppearanceBinding binding = (UIAppearanceBinding)comboBoxItem.Items[comboBoxItem.SelectedIndex];
                binding.SetFontColor(appearanceScheme, ColorReference.FromColor(buttonColorFont.Color));
                RedrawImmediately();
            }
        }

        private void upDownItemSize_ValueChanged(object sender, EventArgs e)
        {
            if (!loadingBindings)
            {
                UIAppearanceBinding binding = (UIAppearanceBinding)comboBoxItem.Items[comboBoxItem.SelectedIndex];
                binding.SetSize(appearanceScheme, (int)upDownItemSize.Value);
                RedrawImmediately();
            }
        }

        private void upDownFontSize_ValueChanged(object sender, EventArgs e)
        {
            if (!loadingBindings)
            {
                UIAppearanceBinding binding = (UIAppearanceBinding)comboBoxItem.Items[comboBoxItem.SelectedIndex];
                LogicalFont font = binding.GetFont(appearanceScheme);
                font.SetSize((int)upDownFontSize.Value);
                binding.SetFont(appearanceScheme, font);
            }
        }

        private void checkBoxBold_CheckedChanged(object sender, EventArgs e)
        {
            if (!loadingBindings)
            {
                UIAppearanceBinding binding = (UIAppearanceBinding)comboBoxItem.Items[comboBoxItem.SelectedIndex];
                LogicalFont font = binding.GetFont(appearanceScheme);
                font.lfWeight = checkBoxBold.Checked ? 700 : 400;
                binding.SetFont(appearanceScheme, font);
            }
        }

        private void checkBoxItalic_CheckedChanged(object sender, EventArgs e)
        {
            if (!loadingBindings)
            {
                UIAppearanceBinding binding = (UIAppearanceBinding)comboBoxItem.Items[comboBoxItem.SelectedIndex];
                LogicalFont font = binding.GetFont(appearanceScheme);
                font.lfItalic = (byte)(checkBoxItalic.Checked ? 1 : 0);
                binding.SetFont(appearanceScheme, font);
            }
        }

        private void comboBoxFont_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loadingBindings)
            {
                UIAppearanceBinding binding = (UIAppearanceBinding)comboBoxItem.Items[comboBoxItem.SelectedIndex];
                LogicalFont font = binding.GetFont(appearanceScheme);
                font.SetName(comboBoxFont.SelectedItem.ToString());
                binding.SetFont(appearanceScheme, font);
            }
        }

        [Flags]
        private enum WindowDrawingFlags
        {
            Default = 0,
            Active = 1,
            Dialog = 2,
            Icon = 4,
            Menu = 8,
            ClientEdge = 16,
            CloseOnly = 32,
            TextContent = 64,
            Scrollbars = 128,
            ToolWindow = Dialog | CloseOnly | 256,
            MessageBox = Dialog | 512,
        }

        private struct WindowDrawingResult
        {
            public RECT WindowRect;
            public RECT CaptionRect;
            public RECT MenuRect;
            public RECT ClientRect;
        }

        /// <summary>
        /// Returns the dimensions of a window with a client area of size (0x0) at location (0;0) with a window frame around it.
        /// </summary>
        /// <param name="appearanceScheme">An AppearanceScheme to use for calculating the window rectangle.</param>
        /// <param name="flags">Flags specifying what parts of the window frame should be accounted for.</param>
        private static RECT GetWindowFrameSize(AppearanceScheme appearanceScheme, WindowDrawingFlags flags)
        {
            RECT windowRectangle = new RECT(0, 0, 0, 0);

            // Window edge (2px + 1px padding)
            windowRectangle.Inflate(3);

            // Window border
            if (!flags.HasFlag(WindowDrawingFlags.Dialog))
                windowRectangle.Inflate(appearanceScheme.GetMetric(SchemeMetric.WindowBorderWidth) + appearanceScheme.GetMetric(SchemeMetric.PaddedWindowBorderWidth));

            // Window caption
            windowRectangle.Top -= appearanceScheme.GetMetric(flags.HasFlag(WindowDrawingFlags.ToolWindow) ? SchemeMetric.SmallCaptionHeight : SchemeMetric.CaptionHeight) + 1;

            // Menu bar
            if (flags.HasFlag(WindowDrawingFlags.Menu))
                windowRectangle.Top -= appearanceScheme.GetMetric(SchemeMetric.MenuHeight) + 1;

            // Client edge
            if (flags.HasFlag(WindowDrawingFlags.ClientEdge))
                windowRectangle.Inflate(2);

            // Scrollbars
            if (flags.HasFlag(WindowDrawingFlags.Scrollbars))
            {
                windowRectangle.Right += appearanceScheme.GetMetric(SchemeMetric.ScrollBarWidth);
                windowRectangle.Bottom += appearanceScheme.GetMetric(SchemeMetric.ScrollBarHeight);
            }

            return windowRectangle;
        }

        /// <summary>
        /// Adjusts the given client rectangle to accomodate for the window frame.
        /// </summary>
        /// <param name="appearanceScheme">An AppearanceScheme to use for calculating the new window rectangle.</param>
        /// <param name="flags">Flags specifying what parts of the window frame should be accounted for.</param>
        /// <param name="windowRectangle">A RECT specifying the desired client area.</param>
        /// <returns>A RECT specifying the window area with all border elements drawn around the specified client area.</returns>
        private static RECT AdjustWindowRect(AppearanceScheme appearanceScheme, WindowDrawingFlags flags, RECT windowRectangle)
        {
            windowRectangle.Add(GetWindowFrameSize(appearanceScheme, flags));
            return windowRectangle;
        }

        /// <summary>
        /// Adjusts the given client rectangle to accomodate for the window frame, keeping the origin of the window fixed.
        /// </summary>
        /// <param name="appearanceScheme">An AppearanceScheme to use for calculating the new window rectangle.</param>
        /// <param name="flags">Flags specifying what parts of the window frame should be accounted for.</param>
        /// <param name="windowRectangle">A RECT specifying the desired client area. The X and Y coordinates represent the upper left corner of the window frame, while the Width and Height of the rectangle specify the desired client area.</param>
        /// <returns>A RECT specifying the window size with all border elements drawn around the specified client area, positioned at the location originally specified.</returns>
        private static RECT AdjustWindowRectFixed(AppearanceScheme appearanceScheme, WindowDrawingFlags flags, RECT windowRectangle)
        {
            RECT temp = GetWindowFrameSize(appearanceScheme, flags);
            windowRectangle.Width += temp.Width;
            windowRectangle.Height += temp.Height;
            return windowRectangle;
        }

        /// <summary>
        /// Draws a window at the specified location in to the specified device context using the supplied parameters.
        /// </summary>
        /// <param name="hDc">A handle to the device context.</param>
        /// <param name="appearanceScheme">An AppearanceScheme to use for drawing the window.</param>
        /// <param name="windowRect">A RECT specifying the location and size of the window.</param>
        /// <param name="hWnd">A handle to a window. The specific window doesn't matter, it's just that some Win32 functions need a valid handle to a window to succeed.</param>
        /// <param name="flags">Flags specifying how the window should be drawn.</param>
        /// <param name="hIcon">A handle to an icon. Set this to IntPtr.Zero when not using WindowDrawingFlags.Icon.</param>
        /// <param name="caption">The window title to use in the caption.</param>
        /// <param name="caption">The text content to be drawn in the window. This parameter is ignored if WindowDrawingFlags.TextContent is not specified.</param>
        private static WindowDrawingResult DrawWindow(IntPtr hDc, AppearanceScheme appearanceScheme, RECT windowRect, IntPtr hWnd, 
                                               WindowDrawingFlags flags, string caption, IntPtr hIcon, string content = "")
        {
            WindowDrawingResult result = new WindowDrawingResult();
            result.WindowRect = windowRect;

            // Prepare fonts
            IntPtr captionFont = appearanceScheme.GetFont(SchemeFont.CaptionFont).GetHandle();
            IntPtr dialogFont = appearanceScheme.GetFont(SchemeFont.DialogFont).GetHandle();
            IntPtr menuFont = appearanceScheme.GetFont(SchemeFont.MenuFont).GetHandle();

            // Draw window edge
            DrawEdge(hDc, ref windowRect, BDR_RAISEDOUTER | BDR_RAISEDINNER, BF_RECT | BF_MIDDLE | BF_ADJUST);

            // Draw window border
            if (!flags.HasFlag(WindowDrawingFlags.Dialog))
            {
                FillRect(hDc, ref windowRect, appearanceScheme.GetBrush(flags.HasFlag(WindowDrawingFlags.Active) ? SchemeColor.ActiveWindowBorderColor : SchemeColor.InactiveWindowBorderColor));
                windowRect.Inflate(-(appearanceScheme.GetMetric(SchemeMetric.WindowBorderWidth) + appearanceScheme.GetMetric(SchemeMetric.PaddedWindowBorderWidth)));
                FillRect(hDc, ref windowRect, appearanceScheme.GetBrush(SchemeColor.ThreeDimensionalObjectColor));
            }
            windowRect.Inflate(-1);

            // Draw window caption
            int captionWidth = appearanceScheme.GetMetric(flags.HasFlag(WindowDrawingFlags.ToolWindow) ? SchemeMetric.SmallCaptionHeight : SchemeMetric.CaptionHeight) - 2;
            RECT captionRect = windowRect;
            captionRect.Height = appearanceScheme.GetMetric(flags.HasFlag(WindowDrawingFlags.ToolWindow) ? SchemeMetric.SmallCaptionHeight : SchemeMetric.CaptionHeight);
            windowRect.Top += captionRect.Height + 1;
            FillRect(hDc, ref captionRect, appearanceScheme.GetBrush(flags.HasFlag(WindowDrawingFlags.Active) ? SchemeColor.ActiveCaptionGradientColor : SchemeColor.InactiveCaptionGradientColor));
            result.CaptionRect = captionRect;

            // Draw window caption buttons
            RECT captionButtonRect = captionRect;
            captionButtonRect.Left = captionButtonRect.Right - 2 - captionWidth;
            captionButtonRect.Top += 2;
            captionButtonRect.Bottom -= 2;
            captionButtonRect.Width = captionWidth;
            DrawFrameControl(hDc, ref captionButtonRect, DFC_CAPTION, DFCS_CAPTIONCLOSE);

            if (!flags.HasFlag(WindowDrawingFlags.CloseOnly))
            {
                captionButtonRect.Left -= 2 + captionWidth;
                captionButtonRect.Width = captionWidth;
                DrawFrameControl(hDc, ref captionButtonRect, DFC_CAPTION, DFCS_CAPTIONMAX);

                captionButtonRect.Left -= captionWidth;
                captionButtonRect.Width = captionWidth;
                DrawFrameControl(hDc, ref captionButtonRect, DFC_CAPTION, DFCS_CAPTIONMIN);
            }

            // Draw window caption gradient, text and icon
            captionRect.Right = captionButtonRect.Left - 1;
            if (flags.HasFlag(WindowDrawingFlags.Icon) && !flags.HasFlag(WindowDrawingFlags.ToolWindow))
            {
                if (hIcon == IntPtr.Zero)
                    throw new ArgumentNullException();
                DrawCaptionTemp(hWnd, hDc, ref captionRect, captionFont, hIcon, caption, DC_BUTTONS | DC_GRADIENT | DC_ICON | DC_TEXT | (flags.HasFlag(WindowDrawingFlags.Active) ? DC_ACTIVE : 0));
            }
            else
                DrawCaptionTemp(hWnd, hDc, ref captionRect, captionFont, IntPtr.Zero, caption, DC_BUTTONS | DC_GRADIENT | DC_SMALLCAP | DC_TEXT | (flags.HasFlag(WindowDrawingFlags.Active) ? DC_ACTIVE : 0));

            // Draw menu bar
            if (flags.HasFlag(WindowDrawingFlags.Menu))
            {
                RECT menuRect = windowRect;
                menuRect.Height = appearanceScheme.GetMetric(SchemeMetric.MenuHeight);
                windowRect.Top = menuRect.Bottom + 1;
                FillRect(hDc, ref menuRect, appearanceScheme.GetBrush(SchemeColor.MenuColor));
                result.MenuRect = menuRect;

                IntPtr restoreFont = SelectObject(hDc, menuFont);
                RECT menuItemRect = menuRect;
                Size size = new Size();
                
                // Normal menu item
                GetTextExtentPoint32(hDc, "Normal", 6, ref size);
                menuItemRect.Width = size.Width + 16;

                ColorReference restoreColor = SetTextColor(hDc, appearanceScheme.GetColor(SchemeColor.MenuFontColor));
                if (menuRect.ToRectangle().Contains(menuItemRect.ToRectangle()))
                {
                    DrawText(hDc, "Normal", -1, ref menuItemRect, DT_CENTER | DT_NOCLIP | DT_VCENTER | DT_SINGLELINE);
                }

                // Disabled menu item
                GetTextExtentPoint32(hDc, "Disabled", 8, ref size);
                menuItemRect.Left = menuItemRect.Right;
                menuItemRect.Width = size.Width + 16;

                if (menuRect.ToRectangle().Contains(menuItemRect.ToRectangle()))
                {
                    SetTextColor(hDc, appearanceScheme.GetColor(SchemeColor.DisabledFontColor));
                    DrawText(hDc, "Disabled", -1, ref menuItemRect, DT_CENTER | DT_NOCLIP | DT_VCENTER | DT_SINGLELINE);
                }

                // Selected menu item
                GetTextExtentPoint32(hDc, "Selected", 8, ref size);
                menuItemRect.Left = menuItemRect.Right;
                menuItemRect.Width = size.Width + 16;

                if (menuRect.ToRectangle().Contains(menuItemRect.ToRectangle()))
                {
                    SetTextColor(hDc, appearanceScheme.GetColor(SchemeColor.SelectedItemsFontColor));
                    FillRect(hDc, ref menuItemRect, appearanceScheme.GetBrush(SchemeColor.SelectedItemsColor));
                    DrawText(hDc, "Selected", -1, ref menuItemRect, DT_CENTER | DT_NOCLIP | DT_VCENTER | DT_SINGLELINE);
                }

                SelectObject(hDc, restoreFont);
                SetTextColor(hDc, restoreColor);
            }

            // Draw client edge
            if (flags.HasFlag(WindowDrawingFlags.ClientEdge))
            {
                DrawEdge(hDc, ref windowRect, BDR_SUNKENOUTER | BDR_SUNKENINNER, BF_RECT | BF_MIDDLE | BF_ADJUST);
                FillRect(hDc, ref windowRect, appearanceScheme.GetBrush(SchemeColor.WindowColor));
            }

            // Draw scrollbars
            if (flags.HasFlag(WindowDrawingFlags.Scrollbars))
            {
                RECT scrollbarRect = windowRect;
                scrollbarRect.Left = scrollbarRect.Right - appearanceScheme.GetMetric(SchemeMetric.ScrollBarWidth);
                scrollbarRect.Bottom -= appearanceScheme.GetMetric(SchemeMetric.ScrollBarHeight);
                FillRect(hDc, ref scrollbarRect, appearanceScheme.GetBrush(SchemeColor.ScrollBarColor));
                
                RECT scrollbarButtonRect = scrollbarRect;
                scrollbarButtonRect.Bottom = scrollbarButtonRect.Top + appearanceScheme.GetMetric(SchemeMetric.ScrollBarWidth);
                DrawFrameControl(hDc, ref scrollbarButtonRect, DFC_SCROLL, DFCS_SCROLLUP | DFCS_INACTIVE);
                scrollbarButtonRect = scrollbarRect;
                scrollbarButtonRect.Top = scrollbarButtonRect.Bottom - appearanceScheme.GetMetric(SchemeMetric.ScrollBarWidth);
                DrawFrameControl(hDc, ref scrollbarButtonRect, DFC_SCROLL, DFCS_SCROLLDOWN | DFCS_INACTIVE);
                windowRect.Width -= appearanceScheme.GetMetric(SchemeMetric.ScrollBarWidth);

                scrollbarRect = windowRect;
                scrollbarRect.Top = scrollbarRect.Bottom - appearanceScheme.GetMetric(SchemeMetric.ScrollBarHeight);
                FillRect(hDc, ref scrollbarRect, appearanceScheme.GetBrush(SchemeColor.ScrollBarColor));

                scrollbarButtonRect = scrollbarRect;
                scrollbarButtonRect.Right = scrollbarButtonRect.Left + appearanceScheme.GetMetric(SchemeMetric.ScrollBarHeight);
                DrawFrameControl(hDc, ref scrollbarButtonRect, DFC_SCROLL, DFCS_SCROLLLEFT | DFCS_INACTIVE);
                scrollbarButtonRect = scrollbarRect;
                scrollbarButtonRect.Left = scrollbarButtonRect.Right - appearanceScheme.GetMetric(SchemeMetric.ScrollBarHeight);
                DrawFrameControl(hDc, ref scrollbarButtonRect, DFC_SCROLL, DFCS_SCROLLRIGHT | DFCS_INACTIVE);
                windowRect.Height -= appearanceScheme.GetMetric(SchemeMetric.ScrollBarHeight);

                scrollbarButtonRect.Left = scrollbarButtonRect.Right;
                scrollbarButtonRect.Width = appearanceScheme.GetMetric(SchemeMetric.ScrollBarWidth);
                DrawFrameControl(hDc, ref scrollbarButtonRect, DFC_SCROLL, DFCS_SCROLLSIZEGRIP | DFCS_INACTIVE);
            }

            result.ClientRect = windowRect;

            // Draw window text
            if (flags.HasFlag(WindowDrawingFlags.TextContent))
            {
                ColorReference oldColor = SetTextColor(hDc, appearanceScheme.GetColor(SchemeColor.WindowFontColor));
                IntPtr temp = IntPtr.Zero;
                if (flags.HasFlag(WindowDrawingFlags.Dialog))
                    temp = SelectObject(hDc, dialogFont);

                windowRect.Inflate(-2);
                if (!flags.HasFlag(WindowDrawingFlags.ClientEdge))
                    windowRect.Left += 1;
                DrawText(hDc, content, -1, ref windowRect, DT_LEFT | DT_NOCLIP | DT_TOP | DT_SINGLELINE);

                SetTextColor(hDc, oldColor);
                if (flags.HasFlag(WindowDrawingFlags.Dialog))
                    SelectObject(hDc, temp);
            }

            LogicalFont.FreeHandle(captionFont);
            LogicalFont.FreeHandle(dialogFont);
            LogicalFont.FreeHandle(menuFont);

            return result;
        }

        private void panelWindowPreviewPaint(object sender, PaintEventArgs e)
        {
            // Setting custom colors
            int restoreHandle = SetSysColorsTemp(appearanceScheme.GetAllColors(), appearanceScheme.GetAllBrushes(), 29);
            IntPtr hDc = e.Graphics.GetHdc();

            // Draw the desktop
            RECT clientRect = RECT.FromRectangle(panelWindowPreview.ClientRectangle);
            FillRect(hDc, ref clientRect, appearanceScheme.GetBrush(SchemeColor.DesktopColor));

            // Draw inactive window
            WindowDrawingFlags flags = WindowDrawingFlags.Default;
            WindowDrawingResult result = DrawWindow(hDc, appearanceScheme, AdjustWindowRectFixed(appearanceScheme, flags, new RECT(8, 8, 300, 75)), Handle, flags, "Inactive Window", IntPtr.Zero);
            
            // Draw active window based on inactive window's caption
            flags = WindowDrawingFlags.Active | WindowDrawingFlags.ClientEdge | WindowDrawingFlags.Menu | WindowDrawingFlags.Scrollbars | WindowDrawingFlags.Icon | WindowDrawingFlags.TextContent;
            result = DrawWindow(hDc, appearanceScheme, AdjustWindowRectFixed(appearanceScheme, flags, new RECT(12, result.CaptionRect.Bottom, 300, 75)), Handle, flags, "Active Window", Icon.Handle, "Window Text");
            
            // Restore custom colors
            e.Graphics.ReleaseHdc(hDc);
            SetSysColorsTemp(null, null, restoreHandle);
        }

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDc);

        [DllImport("Msimg32.dll")]
        public static extern int TransparentBlt(IntPtr hdcDest, int xoriginDest, int yoriginDest, int wDest, int hDest, IntPtr hDcSrc, int xoriginSrc, int yoriginSrc, int wSrc, int hSrc, ColorReference crTransparent);

        private void panelDialogPreview_Paint(object sender, PaintEventArgs e)
        {
            // Setting custom colors
            int restoreHandle = SetSysColorsTemp(appearanceScheme.GetAllColors(), appearanceScheme.GetAllBrushes(), 29);
            IntPtr hDc = e.Graphics.GetHdc();

            // Load bitmap
            IntPtr hdcBitmap = CreateCompatibleDC(hDc);
            IntPtr hBitmap = toolWindowButtons.GetHbitmap();
            IntPtr oldBitmap = SelectObject(hdcBitmap, hBitmap);

            // Draw the desktop
            RECT clientRect = RECT.FromRectangle(panelWindowPreview.ClientRectangle);
            FillRect(hDc, ref clientRect, appearanceScheme.GetBrush(SchemeColor.DesktopColor));

            // Draw dialog window

            // Draw message box
            WindowDrawingFlags flags = WindowDrawingFlags.Active | WindowDrawingFlags.MessageBox | WindowDrawingFlags.TextContent;
            WindowDrawingResult result = DrawWindow(hDc, appearanceScheme, AdjustWindowRectFixed(appearanceScheme, flags, new RECT(16, 16, 200, 50)), Handle, flags, "Message Box", Icon.Handle, "Message text");

            // Draw tool window
            flags = WindowDrawingFlags.Active | WindowDrawingFlags.ToolWindow | WindowDrawingFlags.ClientEdge;
            RECT toolWindowRect = AdjustWindowRect(appearanceScheme, flags, new RECT(0, 0, 50, 150));
            toolWindowRect.Location = new Point(panelDialogPreview.ClientRectangle.Right - toolWindowRect.Width - 16, 16);
            result = DrawWindow(hDc, appearanceScheme, toolWindowRect, Handle, flags, "Tool Window", IntPtr.Zero, "");

            // Draw the tool window buttons
            for (int x = 0; x < 2; x++)
                for (int y = 0; y < 6; y++)
                {
                    RECT buttonDestRect = result.ClientRect;
                    buttonDestRect.Size = new Size(0, 0);
                    buttonDestRect.Add(new RECT(x * 25, y * 25, 25, 25));
                    DrawEdge(hDc, ref buttonDestRect, BDR_RAISEDOUTER | BDR_RAISEDINNER, BF_RECT | BF_MIDDLE | BF_ADJUST);
                    TransparentBlt(hDc, buttonDestRect.Left, buttonDestRect.Top, 21, 21, hdcBitmap, x * 21, y * 21, 21, 21, new ColorReference { r = 128, g = 128, b = 128 });
                }

            label12.ForeColor = SystemColors.HighlightText;

            // Restore bitmap
            SelectObject(hdcBitmap, oldBitmap);
            DeleteObject(hBitmap);
            DeleteObject(hdcBitmap);

            // Restore custom colors
            e.Graphics.ReleaseHdc(hDc);
            SetSysColorsTemp(null, null, restoreHandle);
        }

        private void RedrawImmediately()
        {
            RedrawWindow(Handle, IntPtr.Zero, IntPtr.Zero, RDW_FRAME | RDW_INVALIDATE | RDW_UPDATENOW | RDW_ALLCHILDREN);
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            string name = Interaction.InputBox("Save this color scheme as:", "Save Scheme");
            foreach (char c in Path.GetInvalidFileNameChars())
                name = name.Replace(c, '_');

            appearanceScheme.SaveToRegistry(name);
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            buttonApply_Click(sender, e);
            buttonCancel_Click(sender, e);

        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            appearanceScheme.ApplyToSystem();
            RedrawImmediately();
        }
    }
}
