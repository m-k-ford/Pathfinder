using System;
using System.Runtime.InteropServices;

namespace Launcher
{
    public sealed class Settings
    {
        private const int HIDE = 0;
        private const int MAXIMIZE = 3;
        private const int MINIMIZE = 6;
        private const int RESTORE = 9;

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();
        private static IntPtr ThisConsole = GetConsoleWindow();
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public Action SetCurrent = delegate { };

        public void SetFullscreen()
        {
            SetCurrent = SetFullscreen;
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            ShowWindow(ThisConsole, MAXIMIZE);
            Console.CursorVisible = false;
            Console.Clear();
        }

        public void SetDefault()
        {
            SetCurrent = SetDefault;
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetWindowSize(150, 45);
            Console.CursorVisible = false;
            Console.Clear();
        }
    }
}
