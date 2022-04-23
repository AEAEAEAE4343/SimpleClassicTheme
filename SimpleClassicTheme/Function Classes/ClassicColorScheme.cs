using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SimpleClassicTheme.Function_Classes
{
	public class ColorScheme
	{
		[StructLayout(LayoutKind.Explicit, Size = 92, Pack = 1)]
		private struct LOGFONTW
		{
			[FieldOffset(0)]
			public int lfHeight;
			[FieldOffset(4)]
			public int lfWidth;
			[FieldOffset(8)]
			public int lfEscapement;
			[FieldOffset(12)]
			public int lfOrientation;
			[FieldOffset(16)]
			public int lfWeight;
			[FieldOffset(20)]
			public byte lfItalic;
			[FieldOffset(21)]
			public byte lfUnderline;
			[FieldOffset(22)]
			public byte lfStrikeOut;
			[FieldOffset(23)]
			public byte lfCharSet;
			[FieldOffset(24)]
			public byte lfOutPrecision;
			[FieldOffset(25)]
			public byte lfClipPrecision;
			[FieldOffset(26)]
			public byte lfQuality;
			[FieldOffset(27)]
			public byte lfPitchAndFamily;
			[FieldOffset(28), MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
			public byte[] lfFaceName;
		}

		[StructLayout(LayoutKind.Explicit, Size = 712, Pack = 1)]
		private struct ColorSchemeStruct
		{
			[FieldOffset(8)]
			public byte WindowBorderSize;
			[FieldOffset(12)]
			public byte ScrollBarSize1;
			[FieldOffset(16)]
			public byte ScrollBarSize2;
			
			[FieldOffset(20)]
			public byte CaptionSize1;
			[FieldOffset(24)]
			public byte CaptionSize2;
			[FieldOffset(0x1C), MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x5C)]
			public LOGFONTW CaptionFont;

			[FieldOffset(120)]
			public byte PaletteTitleSize;
			[FieldOffset(0x80), MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x5C)]
			public LOGFONTW PaletteTitleFont;

			[FieldOffset(220)]
			public byte MenuSize;
			[FieldOffset(0xE4), MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x5C)]
			public LOGFONTW MenuFont;
			
			[FieldOffset(0x140), MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x5C)]
			public LOGFONTW ToolTipFont;
			[FieldOffset(0x19C), MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x5C)]
			public LOGFONTW MessageBoxFont;
			[FieldOffset(0x1F8), MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x5C)]
			public LOGFONTW IconFont;

			[FieldOffset(600), MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public byte[] DesktopColor;
			[FieldOffset(604), MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public byte[] ActiveTitleBarColor;
			[FieldOffset(608), MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public byte[] InactiveTitleBarColor;
			[FieldOffset(612), MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public byte[] MenuColor;
			[FieldOffset(616), MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public byte[] WindowColor;
			[FieldOffset(624), MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public byte[] MenuFontColor;
			[FieldOffset(628), MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public byte[] MessageBoxFontColor;
			[FieldOffset(632), MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public byte[] ActiveTitleBarFontColor;
			[FieldOffset(636), MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public byte[] ActiveWindowBorderColor;
			[FieldOffset(640), MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public byte[] InactiveWindowBorderColor;
			[FieldOffset(644), MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public byte[] ApplicationBackgroundColor;
			[FieldOffset(648), MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public byte[] SelectedItemsColor;
			[FieldOffset(672), MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public byte[] InactiveTitleBarFontColor;
			[FieldOffset(688), MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public byte[] ToolTipFontColor;
			[FieldOffset(692), MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public byte[] ToolTipColor;
			[FieldOffset(704), MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public byte[] ActiveTitleBarGradientColor;
			[FieldOffset(708), MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public byte[] InactiveTitleBarGradientColor;
		}

		public byte WindowBorderSize { get => str.WindowBorderSize; set => str.WindowBorderSize = value; }
		public byte ScrollBarSize { get => str.ScrollBarSize1; set { str.ScrollBarSize1 = value; str.ScrollBarSize2 = value; } }
		public byte CaptionSize { get => str.CaptionSize1; set { str.CaptionSize1 = value; str.CaptionSize2 = value; } }
		//public string CaptionFont { get => GetStringFromBytes(str.CaptionFont); set => str.CaptionFont = GetBytesFromString(value); }
		public byte PaletteTitleSize { get => str.PaletteTitleSize; set => str.PaletteTitleSize = value; }
		//public string PaletteTitleFont { get => GetStringFromBytes(str.PaletteTitleFont); set => str.PaletteTitleFont = GetBytesFromString(value); }
		public byte MenuSize { get => str.MenuSize; set => str.MenuSize = value; }
		//public string MenuFont { get => GetStringFromBytes(str.MenuFont); set => str.MenuFont = GetBytesFromString(value); }
		//public string ToolTipFont { get => GetStringFromBytes(str.ToolTipFont); set => str.ToolTipFont = GetBytesFromString(value); }
		//public string MessageBoxFont { get => GetStringFromBytes(str.MessageBoxFont); set => str.MessageBoxFont = GetBytesFromString(value); }
		//public string IconFont { get => GetStringFromBytes(str.IconFont); set => str.IconFont = GetBytesFromString(value); }
		public Color DesktopColor { get => GetColorFromBytes(str.DesktopColor); set => str.DesktopColor = GetBytesFromColor(value); }
		public Color ActiveTitleBarColor { get => GetColorFromBytes(str.ActiveTitleBarColor); set => str.ActiveTitleBarColor = GetBytesFromColor(value); }
		public Color InactiveTitleBarColor { get => GetColorFromBytes(str.InactiveTitleBarColor); set => str.InactiveTitleBarColor = GetBytesFromColor(value); }
		public Color MenuColor { get => GetColorFromBytes(str.MenuColor); set => str.MenuColor = GetBytesFromColor(value); }
		public Color WindowColor { get => GetColorFromBytes(str.WindowColor); set => str.WindowColor = GetBytesFromColor(value); }
		public Color MenuFontColor { get => GetColorFromBytes(str.MenuFontColor); set => str.MenuFontColor = GetBytesFromColor(value); }
		public Color MessageBoxFontColor { get => GetColorFromBytes(str.MessageBoxFontColor); set => str.MessageBoxFontColor = GetBytesFromColor(value); }
		public Color ActiveTitleBarFontColor { get => GetColorFromBytes(str.ActiveTitleBarFontColor); set => str.ActiveTitleBarFontColor = GetBytesFromColor(value); }
		public Color ActiveWindowBorderColor { get => GetColorFromBytes(str.ActiveWindowBorderColor); set => str.ActiveWindowBorderColor = GetBytesFromColor(value); }
		public Color InactiveWindowBorderColor { get => GetColorFromBytes(str.InactiveWindowBorderColor); set => str.InactiveWindowBorderColor = GetBytesFromColor(value); }
		public Color ApplicationBackgroundColor { get => GetColorFromBytes(str.ApplicationBackgroundColor); set => str.ApplicationBackgroundColor = GetBytesFromColor(value); }
		public Color SelectedItemsColor { get => GetColorFromBytes(str.SelectedItemsColor); set => str.SelectedItemsColor = GetBytesFromColor(value); }
		public Color InactiveTitleBarFontColor { get => GetColorFromBytes(str.InactiveTitleBarFontColor); set => str.InactiveTitleBarFontColor = GetBytesFromColor(value); }
		public Color ToolTipFontColor { get => GetColorFromBytes(str.ToolTipFontColor); set => str.ToolTipFontColor = GetBytesFromColor(value); }
		public Color ToolTipColor { get => GetColorFromBytes(str.ToolTipColor); set => str.ToolTipColor = GetBytesFromColor(value); }
		public Color ActiveTitleBarGradientColor { get => GetColorFromBytes(str.ActiveTitleBarGradientColor); set => str.ActiveTitleBarGradientColor = GetBytesFromColor(value); }
		public Color InactiveTitleBarGradientColor { get => GetColorFromBytes(str.InactiveTitleBarGradientColor); set => str.InactiveTitleBarGradientColor = GetBytesFromColor(value); }

		ColorSchemeStruct str;

		public ColorScheme()
		{
			str = new ColorSchemeStruct();
		}

		private ColorScheme(ColorSchemeStruct structure)
		{
			str = structure;
		}

		public static ColorScheme FromColorScheme(byte[] colorScheme)
		{
			if (colorScheme.Length != 712)
				throw new ArgumentException("Not enough data to fill struct.");
			IntPtr buffer = Marshal.AllocHGlobal(712);
			Marshal.Copy(colorScheme, 0, buffer, 712);
			ColorSchemeStruct retobj = (ColorSchemeStruct)Marshal.PtrToStructure(buffer, typeof(ColorSchemeStruct));
			Marshal.FreeHGlobal(buffer);
			return new ColorScheme(retobj);
		}

		public byte[] ToColorScheme()
		{
			IntPtr buffer = Marshal.AllocHGlobal(712);
			Marshal.StructureToPtr(str, buffer, false);
			byte[] colorScheme = new byte[712];
			Marshal.Copy(buffer, colorScheme, 0, 712);
			Marshal.FreeHGlobal(buffer);
			return colorScheme;
		}

		private Color GetColorFromBytes(byte[] bytes) => Color.FromArgb(bytes[0], bytes[1], bytes[2]);
		private byte[] GetBytesFromColor(Color color) => new byte[] { color.R, color.G, color.B };

		private string GetStringFromBytes(byte[] bytes)
		{
			if (bytes.Length != 64)
				throw new ArgumentException("The array was not 64 bytes long");

			byte[] newBytes = new byte[32];
			for (int i = 0; i < 32; i++)
				newBytes[i] = bytes[i * 2];

			return Encoding.ASCII.GetString(newBytes).Trim('\0');
		}

		private byte[] GetBytesFromString(string text)
		{
			if (text.Length > 32)
				throw new ArgumentException("The string was longer than 32 characters");

			byte[] bytes = Encoding.ASCII.GetBytes(text);
			byte[] newBytes = new byte[64];

			for (int i = 0; i < 64; i++)
				newBytes[i] = 0;
			for (int i = 0; i < bytes.Length; i++)
				newBytes[i * 2] = bytes[i];

			return newBytes;
		}
	}
}
