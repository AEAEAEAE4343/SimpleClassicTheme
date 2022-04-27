using Microsoft.Win32;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using SimpleClassicTheme.Theming;

namespace SimpleClassicTheme.Forms
{
	public partial class ThemeConfigurationForm : SystemMenuForm
	{
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

			public RECT(int l, int t, int w, int h)
			{
				Left = l; Top = t; Right = l + w; Bottom = t + h;
			}
			public Rectangle ToRectangle() => new Rectangle(Left, Top, Width, Height);
			public static RECT FromRectangle(Rectangle r) => new RECT(r.Left, r.Top, r.Right, r.Bottom);
		}

		// DWORD WINAPI DrawCaptionTemp(HWND hwnd, HDC hDC, LPRECT lprect, HFONT hFont, HICON hIcon, LPTSTR caption, DWORD flags)
		[DllImport("user32.dll")]
		static extern bool DrawCaptionTemp(IntPtr hWnd, IntPtr hDc, IntPtr lpRect, IntPtr hFont, IntPtr hIcon, string caption, uint flags);
		// DWORD_PTR WINAPI SetSysColorsTemp(const COLORREF *, const HBRUSH *, DWORD_PTR);
		[DllImport("user32.dll")]
		static extern int SetSysColorsTemp(COLORREF[] lpaElements, IntPtr[] lpaRgbValues, int cElements);
		// DWORD WINAPI DrawMenuBarTemp(HWND hwnd, HDC hDC, LPRECT lprect, HMENU hMenu, HFONT hFont);
		//[DllImport("user32.dll")]
		//static extern uint DrawMenuBarTemp(IntPtr hWnd, IntPtr hDc, IntPtr lpRect, IntPtr hMenu, IntPtr hFont);

		[DllImport("gdi32.dll")]
		static extern int GetTextExtentPoint32(IntPtr hDc, string lpString, int c, ref Size pSize);

		[DllImport("user32.dll")]
		static extern bool DrawEdge(IntPtr hDc, IntPtr lpRect, int edge, int flags);
		[DllImport("user32.dll")]
		static extern bool DrawFrameControl(IntPtr hDc, IntPtr lpRect, int cFrameType, int cFrameState);
		[DllImport("user32.dll")]
		static extern bool DrawText(IntPtr hDc, string text, int c, IntPtr lpRect, uint format);

		[DllImport("gdi32.dll")]
		static extern COLORREF SetTextColor(IntPtr hDc, COLORREF color);
		[DllImport("gdi32.dll")]
		static extern IntPtr SelectObject(IntPtr hDc, IntPtr obj);

		[DllImport("user32.dll")]
		static extern bool FillRect(IntPtr hDc, IntPtr lpRect, IntPtr brush);
		[DllImport("user32.dll")]
		static extern bool FrameRect(IntPtr hDc, IntPtr lpRect, IntPtr brush);

		static int BDR_RAISEDOUTER = 0x0001;
		static int BDR_SUNKENOUTER = 0x0002;
		static int BDR_RAISEDINNER = 0x0004;
		static int BDR_SUNKENINNER = 0x0008;

		static int BF_MIDDLE = 0x0800;
		static int BF_ADJUST = 0x2000;
		static int BF_RECT = 0x0001 | 0x0002 | 0x0004 | 0x0008;

		static int DFC_CAPTION = 1;
		static int DFCS_CAPTIONCLOSE = 0;
		static int DFCS_CAPTIONMIN = 1;
		static int DFCS_CAPTIONMAX = 2;

		static uint DC_SMALLCAP = 0x0002;
		static uint DC_BUTTONS = 0x1000;
		static uint DC_ACTIVE = 0x0001;
		static uint DC_ICON = 0x0004;
		static uint DC_TEXT = 0x0008;
		static uint DC_GRADIENT = 0x0020;

		static uint DT_TOP = 0x0000;
		static uint DT_LEFT = 0x0000;
		static uint DT_CENTER = 0x0001;
		static uint DT_VCENTER = 0x0004;
		static uint DT_SINGLELINE = 0x0020;
		static uint DT_NOCLIP = 0x0100;

		public ThemeConfigurationForm()
		{
			InitializeComponent();

			string[] names = Registry.CurrentUser.OpenSubKey("Control Panel", false)?.OpenSubKey("Appearance", false)?.OpenSubKey("Schemes", false)?.GetValueNames();
			if (names is null)
			{
				label1.Text = "The preview failed to load: Could not get scheme names from registry.";
				panel1.Dispose();
			}

			comboBox1.Items.AddRange(names);
		}

		private void DrawWindow(IntPtr hDc, AppearanceScheme appearanceScheme, Rectangle windowRectangle, IntPtr hWnd, 
								IntPtr hIcon, string caption, bool isActive = true, bool isDialog = false)
        {
			IntPtr lpRect = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(RECT)));
			RECT windowRect = RECT.FromRectangle(windowRectangle);

			// Prepare fonts
			IntPtr captionFont = appearanceScheme.WindowMetrics.lfCaptionFont.GetFont();
			IntPtr menuFont = appearanceScheme.WindowMetrics.lfMenuFont.GetFont();

			// Draw window edge
			Marshal.StructureToPtr(windowRect, lpRect, true);
			DrawEdge(hDc, lpRect, BDR_RAISEDOUTER | BDR_RAISEDINNER, BF_RECT | BF_MIDDLE | BF_ADJUST);
			windowRect = Marshal.PtrToStructure<RECT>(lpRect);

			// Draw window border
			if (!isDialog)
			{
				FillRect(hDc, lpRect, appearanceScheme.GetBrush(isActive ? SchemeColor.ActiveWindowBorderColor : SchemeColor.InactiveWindowBorderColor));
				windowRect.Left += appearanceScheme.WindowMetrics.iBorderWidth; windowRect.Width -= appearanceScheme.WindowMetrics.iBorderWidth;
				windowRect.Top += appearanceScheme.WindowMetrics.iBorderWidth; windowRect.Height -= appearanceScheme.WindowMetrics.iBorderWidth;
				Marshal.StructureToPtr(windowRect, lpRect, true);
				FillRect(hDc, lpRect, appearanceScheme.GetBrush(SchemeColor.ThreeDimensionalObjectColor));
			}
			windowRect.Left++; windowRect.Width--;
			windowRect.Top++; windowRect.Height--;

			// Draw window caption
			int captionWidth = appearanceScheme.WindowMetrics.iCaptionHeight - 2;
			RECT captionRect = windowRect;
			captionRect.Height = appearanceScheme.WindowMetrics.iCaptionHeight;
			windowRect.Top += captionRect.Height + 1;
			Marshal.StructureToPtr(captionRect, lpRect, true);
			FillRect(hDc, lpRect, appearanceScheme.GetBrush(isActive ? SchemeColor.ActiveCaptionGradientColor : SchemeColor.InactiveCaptionGradientColor));

			// Draw window caption buttons
			RECT captionButtonRect = captionRect;
			captionButtonRect.Left = captionButtonRect.Right - 2 - captionWidth;
			captionButtonRect.Top += 2;
			captionButtonRect.Bottom -= 2;
			captionButtonRect.Width = captionWidth;
			Marshal.StructureToPtr(captionButtonRect, lpRect, true);
			DrawFrameControl(hDc, lpRect, DFC_CAPTION, DFCS_CAPTIONCLOSE);

			captionButtonRect.Left -= 2 + captionWidth;
			captionButtonRect.Width = captionWidth;
			Marshal.StructureToPtr(captionButtonRect, lpRect, true);
			DrawFrameControl(hDc, lpRect, DFC_CAPTION, DFCS_CAPTIONMAX);

			captionButtonRect.Left -= captionWidth;
			captionButtonRect.Width = captionWidth;
			Marshal.StructureToPtr(captionButtonRect, lpRect, true);
			DrawFrameControl(hDc, lpRect, DFC_CAPTION, DFCS_CAPTIONMIN);

			// Draw window caption gradient, text and icon
			captionRect.Right = captionButtonRect.Left - 1;
			Marshal.StructureToPtr(captionRect, lpRect, true);
			if (hIcon != IntPtr.Zero || isDialog)
				DrawCaptionTemp(hWnd, hDc, lpRect, captionFont, hIcon, caption, DC_BUTTONS | DC_GRADIENT | DC_ICON | DC_TEXT | (isActive ? DC_ACTIVE : 0));
			else
				DrawCaptionTemp(hWnd, hDc, lpRect, captionFont, IntPtr.Zero, caption, DC_BUTTONS | DC_GRADIENT | DC_SMALLCAP | DC_TEXT | (isActive ? DC_ACTIVE : 0));

			// Draw menu bar
			if (isActive && !isDialog)
			{
				RECT menuRect = windowRect;
				menuRect.Height = appearanceScheme.WindowMetrics.iMenuHeight;
				windowRect.Top = menuRect.Bottom + 1;
				Marshal.StructureToPtr(menuRect, lpRect, true);
				FillRect(hDc, lpRect, appearanceScheme.GetBrush(SchemeColor.MenuColor));
				IntPtr restoreFont = SelectObject(hDc, menuFont);
				RECT menuItemRect = menuRect;
				Size size = new Size();
				
				// Normal menu item
				GetTextExtentPoint32(hDc, "Normal", 6, ref size);
				menuItemRect.Width = size.Width + 16;
				Marshal.StructureToPtr(menuItemRect, lpRect, true);

				COLORREF restoreColor = SetTextColor(hDc, appearanceScheme.GetColor(SchemeColor.MenuFontColor));
				DrawText(hDc, "Normal", -1, lpRect, DT_CENTER | DT_NOCLIP | DT_VCENTER | DT_SINGLELINE);

				// Disabled menu item
				GetTextExtentPoint32(hDc, "Disabled", 8, ref size);
				menuItemRect.Left = menuItemRect.Right;
				menuItemRect.Width = size.Width + 16;
				Marshal.StructureToPtr(menuItemRect, lpRect, true);

				SetTextColor(hDc, appearanceScheme.GetColor(SchemeColor.DisabledFontColor));
				DrawText(hDc, "Disabled", -1, lpRect, DT_CENTER | DT_NOCLIP | DT_VCENTER | DT_SINGLELINE);

				// Selected menu item
				GetTextExtentPoint32(hDc, "Selected", 8, ref size);
				menuItemRect.Left = menuItemRect.Right;
				menuItemRect.Width = size.Width + 16;
				Marshal.StructureToPtr(menuItemRect, lpRect, true);

				SetTextColor(hDc, appearanceScheme.GetColor(SchemeColor.SelectedItemsFontColor));
				FillRect(hDc, lpRect, appearanceScheme.GetBrush(SchemeColor.SelectedItemsColor));
				DrawText(hDc, "Selected", -1, lpRect, DT_CENTER | DT_NOCLIP | DT_VCENTER | DT_SINGLELINE);

				SelectObject(hDc, restoreFont);
				SetTextColor(hDc, restoreColor);
			}

			if (isActive && !isDialog)
			{
				Marshal.StructureToPtr(windowRect, lpRect, true);
				DrawEdge(hDc, lpRect, BDR_SUNKENOUTER | BDR_SUNKENINNER, BF_RECT | BF_MIDDLE | BF_ADJUST);
				FillRect(hDc, lpRect, appearanceScheme.GetBrush(SchemeColor.WindowColor));
				windowRect = Marshal.PtrToStructure<RECT>(lpRect);

				windowRect.Left += 2; windowRect.Width -= 2;
				windowRect.Top += 2; windowRect.Height -= 2;

				Marshal.StructureToPtr(windowRect, lpRect, true);
				DrawText(hDc, "Window Text", -1, lpRect, DT_LEFT | DT_NOCLIP | DT_TOP | DT_SINGLELINE);
			}

			AppearanceScheme.LOGFONTW.FreeFont(captionFont);
			AppearanceScheme.LOGFONTW.FreeFont(menuFont);

			Marshal.FreeHGlobal(lpRect);
		}

		private void PreviewPanelPaint(object sender, PaintEventArgs e)
		{
			// Custom colors
			AppearanceScheme appearanceScheme;
			try 
			{
				appearanceScheme = AppearanceScheme.FromRegistry(comboBox1.Items[comboBox1.SelectedIndex].ToString()); 
			}
			catch (ArgumentException)
			{
				appearanceScheme = AppearanceScheme.FromSystem(); 
			}

			// Setting custom colors
			int restoreHandle = SetSysColorsTemp(appearanceScheme.GetAllColors(), appearanceScheme.GetAllBrushes(), 29);
			IntPtr hDc = e.Graphics.GetHdc();

			// Allocate unmanaged space for a RECT
			IntPtr lpRect = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(RECT)));

			// Set the background color
			Marshal.StructureToPtr(RECT.FromRectangle(panel1.ClientRectangle), lpRect, true);
			FillRect(hDc, lpRect, appearanceScheme.GetBrush(SchemeColor.DesktopColor));

			DrawWindow(hDc, appearanceScheme, new Rectangle(8, 8, 317, 151), Handle, IntPtr.Zero, "Inactive Window", false);
			DrawWindow(hDc, appearanceScheme, new Rectangle(12, 31, 335, 133), Handle, Icon.Handle, "Active Window");

			// Restore custom colors
			Marshal.FreeHGlobal(lpRect);
			SetSysColorsTemp(null, null, restoreHandle);
			appearanceScheme.Dispose();
		}

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
			panel1.Invalidate();
			panel1.Refresh();
        }
    }
}
