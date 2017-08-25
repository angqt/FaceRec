using System;
using System.Runtime.InteropServices;
using System.Windows.Input;

namespace WpfApplicationHotKey.WinApi
{
    /// <summary>
    /// author: https://www.outcoldman.com/en/archive/2010/09/11/register-hotkey-in-system-for-wpf-application/
    /// </summary>
	internal class HotKeyWinApi
	{
		public const int WmHotKey = 0x0312;

		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool RegisterHotKey(IntPtr hWnd, int id, ModifierKeys fsModifiers, Keys vk);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool UnregisterHotKey(IntPtr hWnd, int id);
	}
}
