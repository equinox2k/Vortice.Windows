﻿// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using Vortice;
using Vortice.Win32;
using static Vortice.Win32.User32;

namespace HelloDirect3D11
{
    public sealed class Window
    {
        private const int CW_USEDEFAULT = unchecked((int)0x80000000);

        public int Width { get; private set; }
        public int Height { get; private set; }
        public IntPtr Handle { get; private set; }


        public Window(string title, int width, int height)
        {
            var x = 0;
            var y = 0;
            WindowStyles style = 0;
            WindowExStyles styleEx = 0;
            const bool resizable = true;

            // Setup the screen settings depending on whether it is running in full screen or in windowed mode.
            //if (fullscreen)
            //{
            //style = User32.WindowStyles.WS_POPUP | User32.WindowStyles.WS_VISIBLE;
            //styleEx = User32.WindowStyles.WS_EX_APPWINDOW;

            //width = screenWidth;
            //height = screenHeight;
            //}
            //else
            {
                if (width > 0 && height > 0)
                {
                    var screenWidth = GetSystemMetrics(SystemMetrics.SM_CXSCREEN);
                    var screenHeight = GetSystemMetrics(SystemMetrics.SM_CYSCREEN);

                    // Place the window in the middle of the screen.WS_EX_APPWINDOW
                    x = (screenWidth - width) / 2;
                    y = (screenHeight - height) / 2;
                }

                if (resizable)
                {
                    style = WindowStyles.WS_OVERLAPPEDWINDOW;
                }
                else
                {
                    style = WindowStyles.WS_POPUP | WindowStyles.WS_BORDER | WindowStyles.WS_CAPTION | WindowStyles.WS_SYSMENU;
                }

                styleEx = WindowExStyles.WS_EX_APPWINDOW | WindowExStyles.WS_EX_WINDOWEDGE;
            }
            style |= WindowStyles.WS_CLIPCHILDREN | WindowStyles.WS_CLIPSIBLINGS;

            int windowWidth;
            int windowHeight;

            if (width > 0 && height > 0)
            {
                var rect = new RawRectangle(0, 0, width, height);

                // Adjust according to window styles
                AdjustWindowRectEx(
                    ref rect,
                    style,
                    false,
                    styleEx);

                windowWidth = rect.Right - rect.Left;
                windowHeight = rect.Bottom - rect.Top;
            }
            else
            {
                x = y = windowWidth = windowHeight = CW_USEDEFAULT;
            }

            Handle = CreateWindowEx(
                (int)styleEx,
                Application.WndClassName,
                title,
                (int)style,
                x,
                y,
                windowWidth,
                windowHeight,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero);

            if (Handle == IntPtr.Zero)
            {
                return;
            }

            ShowWindow(Handle, ShowWindowCommand.Normal);
            Width = windowWidth;
            Height = windowHeight;
        }

        public void Destroy()
        {
            if (Handle != IntPtr.Zero)
            {
                var destroyHandle = Handle;
                Handle = IntPtr.Zero;

                Debug.WriteLine($"[WIN32] - Destroying window: {destroyHandle}");
                DestroyWindow(destroyHandle);
            }
        }
    }
}
