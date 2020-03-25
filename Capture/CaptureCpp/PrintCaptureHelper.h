#pragma once

#include <windows.h>
#include <string>
using std::string;

class PrintCaptureHelper
{
public:
	PrintCaptureHelper();
	virtual ~PrintCaptureHelper();

	bool Init(const string& windowName);
	bool Init(HWND hwnd);
	void Cleanup();
	bool RefreshWindow();
	bool ChangeWindowHandle(const string& windowName);
	bool ChangeWindowHandle(HWND hwnd);
	bool Capture() const;

	const RECT& GetWindowRect() const { return windowRect_; }
	const RECT& GetClientRect() const { return clientRect_; }
	int GetBitmapDataSize() const { return bmpDataSize_; }
	HBITMAP GetBitmap() const { return bitmap_; }
	void* GetBitmapAddress() const { return bitsPtr_; }

private:
	HWND hwnd_;
	HDC scrDc_;
	HDC memDc_;
	HBITMAP bitmap_;
	HBITMAP oldBitmap_;
	void* bitsPtr_;

	RECT windowRect_;
	RECT clientRect_;
	int bmpDataSize_;
};