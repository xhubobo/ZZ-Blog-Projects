﻿using System;

namespace CaptureSharp
{
    internal class DibCaptureHelper
    {
        public IntPtr BitmapPtr => _hBitmap;
        public Win32Types.BitmapInfo BitmapInfo => _bitmapInfo;
        public Win32Types.Rect WindowRect => _windowRect;
        public Win32Types.Rect ClientRect => _clientRect;
        public int BitmapDataSize => _bmpDataSize;

        private IntPtr _hWnd = IntPtr.Zero;
        private IntPtr _hScrDc = IntPtr.Zero;
        private IntPtr _hMemDc = IntPtr.Zero;
        private IntPtr _hBitmap = IntPtr.Zero;
        private IntPtr _hOldBitmap = IntPtr.Zero;
        private IntPtr _bitsPtr = IntPtr.Zero;

        private Win32Types.BitmapInfo _bitmapInfo;
        private Win32Types.Rect _windowRect;
        private Win32Types.Rect _clientRect;
        private int _bmpDataSize;

        public bool Init(string windowName)
        {
            var handle = Win32Funcs.FindWindow(null, windowName);
            if (handle.Equals(IntPtr.Zero))
            {
                return false;
            }

            return Init(handle);
        }

        public bool Init(IntPtr handle)
        {
            _hWnd = handle;

            //获取窗口大小
            if (!Win32Funcs.GetWindowRect(_hWnd, out _windowRect)
                || !Win32Funcs.GetClientRect(_hWnd, out _clientRect))
            {
                return false;
            }

            _bmpDataSize = _clientRect.Width * _clientRect.Height * 3;

            //位图信息
            _bitmapInfo = new Win32Types.BitmapInfo {bmiHeader = new Win32Types.BitmapInfoHeader()};
            _bitmapInfo.bmiHeader.Init();
            _bitmapInfo.bmiHeader.biWidth = _clientRect.Width;
            _bitmapInfo.bmiHeader.biHeight = _clientRect.Height;
            _bitmapInfo.bmiHeader.biPlanes = 1;
            _bitmapInfo.bmiHeader.biBitCount = 24;
            _bitmapInfo.bmiHeader.biSizeImage = (uint) (_clientRect.Width * _clientRect.Height);
            _bitmapInfo.bmiHeader.biCompression = (uint) Win32Consts.BitmapCompressionMode.BI_RGB;

            _hScrDc = Win32Funcs.GetWindowDC(_hWnd);
            _hMemDc = Win32Funcs.CreateCompatibleDC(_hScrDc);
            _hBitmap = Win32Funcs.CreateDIBSection(_hMemDc, ref _bitmapInfo,
                (uint) Win32Consts.DibColorMode.DIB_RGB_COLORS,
                out _bitsPtr, IntPtr.Zero, 0);
            _hOldBitmap = Win32Funcs.SelectObject(_hMemDc, _hBitmap);

            return true;
        }

        public void Cleanup()
        {
            if (_hBitmap.Equals(IntPtr.Zero))
            {
                return;
            }

            //删除用过的对象
            Win32Funcs.SelectObject(_hMemDc, _hOldBitmap);
            Win32Funcs.DeleteObject(_hBitmap);
            Win32Funcs.DeleteDC(_hMemDc);
            Win32Funcs.ReleaseDC(_hWnd, _hScrDc);

            _hWnd = IntPtr.Zero;
            _hScrDc = IntPtr.Zero;
            _hMemDc = IntPtr.Zero;
            _hBitmap = IntPtr.Zero;
            _hOldBitmap = IntPtr.Zero;
            _bitsPtr = IntPtr.Zero;
        }

        public bool RefreshWindow()
        {
            var hWnd = _hWnd;
            Cleanup();
            return Init(hWnd);
        }

        public bool ChangeWindowHandle(string windowName)
        {
            Cleanup();
            return Init(windowName);
        }

        public bool ChangeWindowHandle(IntPtr handle)
        {
            Cleanup();
            return Init(handle);
        }

        public IntPtr Capture()
        {
            if (_hBitmap.Equals(IntPtr.Zero) || _hMemDc.Equals(IntPtr.Zero) || _hScrDc.Equals(IntPtr.Zero))
            {
                return IntPtr.Zero;
            }

            var ret = Win32Funcs.BitBlt(
                _hMemDc, 0, 0, _clientRect.Width, _clientRect.Height,
                _hScrDc, 0, 0,
                (uint) Win32Consts.RasterOperationMode.SRCCOPY);

            return ret ? _bitsPtr : IntPtr.Zero;
        }

        public bool Capture(out IntPtr bitsPtr, out int bufferSize, out Win32Types.Rect rect)
        {
            bitsPtr = _bitsPtr;
            bufferSize = _bmpDataSize;
            rect = _clientRect;

            if (_hBitmap.Equals(IntPtr.Zero) || _hMemDc.Equals(IntPtr.Zero) || _hScrDc.Equals(IntPtr.Zero))
            {
                return false;
            }

            var ret = Win32Funcs.BitBlt(
                _hMemDc, 0, 0, _clientRect.Width, _clientRect.Height,
                _hScrDc, 0, 0,
                (uint) Win32Consts.RasterOperationMode.SRCCOPY);

            return ret;
        }
    }
}