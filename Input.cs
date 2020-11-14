using System.Runtime.InteropServices;
//to do: сделать из этого нормальные класс с конструктором.
namespace bot
{
    class Input
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void Mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);

        #region Mouse Fields 
        public const int MOUSEEVENTF_MOVE = 0x0001; /* mouse move */
        public const int MOUSEEVENTF_LEFTDOWN = 0x0002; /* left button down */
        public const int MOUSEEVENTF_LEFTUP = 0x0004; /* left button up */
        public const int MOUSEEVENTF_RIGHTDOWN = 0x0008; /* right button down */
        public const int MOUSEEVENTF_RIGHTUP = 0x0010; /* right button up */
        public const int MOUSEEVENTF_MIDDLEDOWN = 0x0020; /* middle button down */
        public const int MOUSEEVENTF_MIDDLEUP = 0x0040; /* middle button up */
        public const int MOUSEEVENTF_XDOWN = 0x0080; /* x button down */
        public const int MOUSEEVENTF_XUP = 0x0100; /* x button down */
        public const int MOUSEEVENTF_WHEEL = 0x0800; /* wheel button rolled */
        public const int MOUSEEVENTF_VIRTUALDESK = 0x4000; /* map to entire virtual desktop */
        public const int MOUSEEVENTF_ABSOLUTE = 0x8000; /* absolute move */
        #endregion

        [DllImport("user32.dll")]
        public static extern void Keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        #region Keyboard Fields
        public const int KEYBOARDEVENTF_KEYDOWN = 0x0000; //KeyDown
        public const int KEYBOARDEVENTF_KEYUP = 0x0002; //KeyUp
        #endregion
    }
}
