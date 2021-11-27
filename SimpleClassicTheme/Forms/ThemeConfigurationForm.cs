using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleClassicTheme.Forms
{
	public partial class ThemeConfigurationForm : Form
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

			public RECT(int l, int t, int r, int b)
			{
				Left = l; Top = t; Right = r; Bottom = b;
			}
			public Rectangle ToRectangle() => new Rectangle(Left, Top, Width, Height);
			public static RECT FromRectangle(Rectangle r) => new RECT(r.Left, r.Top, r.Right, r.Bottom);
		}

		[DllImport("user32.dll")]
		static extern IntPtr SetSysColorsTemp(int cElements, int[] lpaElements, int[] lpaRgbValues);
		[DllImport("user32.dll")]
		static extern IntPtr SetSysColorsTemp(int cElements, int[] lpaElements, IntPtr lpaRgbValues);
		[DllImport("user32.dll")]
		static extern bool DrawFrameControl(IntPtr hDc, IntPtr lpRect, int cFrameType, int cFrameState);
		static int COLOR_BTNFACE = 15;
		static int DFC_CAPTION = 1;

		public ThemeConfigurationForm()
		{
			InitializeComponent();
		}

		private void panel1_Paint(object sender, PaintEventArgs e)
		{
			IntPtr restoreHandle = SetSysColorsTemp(1, new int[] { COLOR_BTNFACE }, new int[] { ColorTranslator.ToWin32(Color.Lime) });

			IntPtr hDc = e.Graphics.GetHdc();

			RECT rect = RECT.FromRectangle(e.ClipRectangle);
			IntPtr lpRect = Marshal.AllocHGlobal(Marshal.SizeOf(rect));
			Marshal.StructureToPtr(rect, lpRect, true);

			DrawFrameControl(hDc, lpRect, DFC_CAPTION, 1);

			SetSysColorsTemp(0, null, restoreHandle);
		}
	}
}
