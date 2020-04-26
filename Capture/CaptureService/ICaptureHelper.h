#pragma once

#include <windows.h>
#include <string>
using std::string;

class ICaptureHelper
{
public:
	virtual ~ICaptureHelper() {}
	virtual bool Init(const string& windowName) = 0;
	virtual bool Init(HWND hwnd) = 0;
	virtual void Cleanup() = 0;
	virtual bool RefreshWindow() = 0;
	virtual bool ChangeWindowHandle(const string& windowName) = 0;
	virtual bool ChangeWindowHandle(HWND hwnd) = 0;
	virtual bool Capture() = 0;

	virtual const RECT& GetWindowRect() const = 0;
	virtual const RECT& GetClientRect() const = 0;
	virtual int GetBitmapDataSize() const = 0;
	virtual HBITMAP GetBitmap() const = 0;
	virtual void* GetBitmapAddress() const = 0;
};