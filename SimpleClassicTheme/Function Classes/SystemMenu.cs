using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	[Flags]
	public enum SystemMenuItemFlags : uint
	{
		Default = 0x0000,
		GrayedAndDisabled = 0x0001,
		Disabled = 0x0002,
		Bitmap = 0x0004,
		Checked = 0x0008,
		Popup = 0x0010,
		OwnerDraw = 0x0100,
		IsSeperator = 0x0800
	}

	[Flags]
	public enum SystemMenuTrackPopupMenuFlags : uint
	{
		Default = 0x0000,

		LeftAlign = 0x0000,
		HorizontalCenterAlign = 0x0004,
		RightAlign = 0x0008,

		TopAlign = 0x0000,
		VerticalCenterAlign = 0x0010,
		BottomAlign = 0x0020,

		TrackLeftButton = 0x0000,
		TrackRightButton = 0x0002,

		LeftToRightAnimation = 0x0400,
		RightToLeftAnimation = 0x0800,
		TopToBottomAnimation = 0x1000,
		BottomToTopAnimation = 0x2000,
		NoAnimation = 0x4000,

		ReturnResult = 0x0100
	}

	public class SystemMenuItem
	{
		public SystemMenuItemFlags MenuItemFlags;
		public Bitmap Image;
		public string Text;
		public Action OnClick;
		public int ID;
		public SystemMenu SubMenu;

		public SystemMenuItem(string text, Action onClick, SystemMenuItemFlags flags)
		{
			Text = text;
			OnClick = onClick;
			MenuItemFlags = flags;
		}

		public SystemMenuItem(string text, Action onClick, SystemMenuItemFlags flags, Image image)
		{
			if (!flags.HasFlag(SystemMenuItemFlags.Bitmap))
				flags = flags | SystemMenuItemFlags.Bitmap;

			Text = text;
			OnClick = onClick;
			MenuItemFlags = flags;
			Image = new Bitmap(image);
		}
	}

	public class SystemMenu
	{
		[DllImport("user32.dll")]
		static extern IntPtr CreateMenu();
		[DllImport("user32.dll")]
		static extern IntPtr CreatePopupMenu();
		[DllImport("user32.dll")]
		static extern bool AppendMenu(IntPtr hMenu, SystemMenuItemFlags uFlags, int uIDNewItem, string lpNewItem);
		[DllImport("user32.dll")]
		static extern bool AppendMenu(IntPtr hMenu, SystemMenuItemFlags uFlags, int uIDNewItem, IntPtr lpNewItem);
		[DllImport("user32.dll")]
		static extern bool DeleteMenu(IntPtr hMenu, int uPosition, uint uFlags);
		[DllImport("user32.dll")]
		static extern int GetSystemMetrics(int nIndex);
		[DllImport("user32.dll")]
		static extern int TrackPopupMenuEx(IntPtr hmenu, SystemMenuTrackPopupMenuFlags fuFlags, int x, int y, IntPtr hwnd, IntPtr lptpm);

		public IntPtr MenuHandle;
		public List<SystemMenuItem> Items = new List<SystemMenuItem>();
		public int Height => GetSystemMetrics(15);

		public SystemMenu(bool popup = false)
		{
			MenuHandle = popup ? CreatePopupMenu() : CreateMenu();
		}

		public void AddItem(SystemMenuItem item)
		{
			Items.Add(item);

			if (item.MenuItemFlags.HasFlag(SystemMenuItemFlags.Bitmap))
				AppendMenu(MenuHandle, item.MenuItemFlags, item.ID, item.Image.GetHbitmap());
			else
				AppendMenu(MenuHandle, item.MenuItemFlags, item.ID, item.Text);
		}

		public void RemoveItem(SystemMenuItem item)
		{
			if (Items.Contains(item))
			{
				Items.Remove(item);
				DeleteMenu(MenuHandle, item.ID, 0x0000);
			}
		}

		public void ShowContextMenu(IntPtr windowHandle, int x, int y)
		{
			int id = TrackPopupMenuEx(MenuHandle, SystemMenuTrackPopupMenuFlags.ReturnResult, x, y, windowHandle, IntPtr.Zero);
			PerformAction(id);
		}

		public void PerformAction(int itemId)
		{
			IEnumerable<SystemMenu> subMenus = Items.Where(a => a.MenuItemFlags.HasFlag(SystemMenuItemFlags.Popup)).Select(a => a.SubMenu);
			foreach (SystemMenu subMenu in subMenus)
				subMenu.PerformAction(itemId);

			IEnumerable<SystemMenuItem> items = Items.Where(a => a.ID == itemId);
			if (items.Count() > 0)
			{
				SystemMenuItem item = items.First();
				item.OnClick();
			}
		}

		public static int CopyToolStripToMenu(ToolStrip menuStrip, SystemMenu systemMenu, int currentId = 0)
		{
			foreach (ToolStripItem item in menuStrip.Items)
			{
				if (item is ToolStripMenuItem menuItem)
				{
					SystemMenuItemFlags flags = SystemMenuItemFlags.Default;
					if (menuItem.Checked)
						flags |= SystemMenuItemFlags.Checked;
					if (!menuItem.Enabled)
						flags |= SystemMenuItemFlags.Disabled;
					if (menuItem.HasDropDownItems)
						flags |= SystemMenuItemFlags.Popup;

					SystemMenuItem systemMenuItem;
					if (menuItem.Image != null)
					{
						flags |= SystemMenuItemFlags.Bitmap;
						systemMenuItem = new SystemMenuItem(menuItem.Text, menuItem.PerformClick, flags, menuItem.Image);
					}
					else
						systemMenuItem = new SystemMenuItem(menuItem.Text, menuItem.PerformClick, flags);
					systemMenuItem.ID = currentId;

					if (menuItem.HasDropDownItems)
                    {
						SystemMenu subMenu = new SystemMenu(true);
						currentId = CopyToolStripToMenu(menuItem.DropDown, subMenu, currentId);
						systemMenuItem.SubMenu = subMenu;
						systemMenuItem.ID = subMenu.MenuHandle.ToInt32();
                    }
					else currentId++;

					systemMenu.AddItem(systemMenuItem);
				}
				else if (item is ToolStripSeparator seperator)
					systemMenu.AddItem(new SystemMenuItem("", new Action(() => { }), SystemMenuItemFlags.IsSeperator));
			}
			return currentId;
		}
	}
}
