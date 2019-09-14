using System;
using System.Runtime.InteropServices;

namespace PositioningLib
{
    //a programhoz szükséges Windows dllek importálása
    public class NativeMethods
    {
        //a console handle pointer megszerzése
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetConsoleWindow();

        //a console eltüntetésének beállítását teszi lehetővé
        [DllImport("user32.dll", EntryPoint = "ShowWindow", SetLastError = true)]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        //az ablak előtérbe helyezése
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        //az előtérben levő alak pointerének megadása
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetForegroundWindow();

        //az ablakok mozgatása a kijelzőn
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);
    }
}
