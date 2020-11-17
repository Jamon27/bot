﻿using System;
using System.Diagnostics;
using System.Threading;

namespace bot
{
    class WorkWithProcess
    {
        public static void BringProcessWindowToFront(Process proc)
        {
            if (proc == null)
                return;
            IntPtr handle = proc.MainWindowHandle;
            int i = 0;

            while (!NativeMethodsForDirect3D.IsWindowInForeground(handle))
            {
                if (i == 0)
                {
                    // Initial sleep if target window is not in foreground - just to let things settle
                    Thread.Sleep(1);
                }

                if (NativeMethodsForDirect3D.IsIconic(handle))
                {
                    // Minimized so send restore
                    NativeMethodsForDirect3D.ShowWindow(handle, NativeMethodsForDirect3D.WindowShowStyle.Restore);
                }
                else
                {
                    // Already Maximized or Restored so just bring to front
                    NativeMethodsForDirect3D.SetForegroundWindow(handle);
                }
                Thread.Sleep(1);

                // Check if the target process main window is now in the foreground
                if (NativeMethodsForDirect3D.IsWindowInForeground(handle))
                {
                    // Leave enough time for screen to redraw
                    Thread.Sleep(5);
                    return;
                }

                // Prevent an infinite loop
                if (i > 620)
                {
                    throw new Exception("Could not set process window to the foreground");
                }
                i++;
            }
        }
    }
}
