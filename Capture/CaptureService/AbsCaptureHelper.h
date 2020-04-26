#pragma once

#include "ICaptureHelper.h"

class AbsCaptureHelper : public ICaptureHelper
{
public:
	AbsCaptureHelper();
	virtual ~AbsCaptureHelper();

	bool Init(const string& windowName) override;
	bool Init(HWND hwnd) override;
	void Cleanup() override;
	bool RefreshWindow() override;
	bool ChangeWindowHandle(const string& windowName) override;
	bool ChangeWindowHandle(HWND hwnd) override;
	bool Capture() override;

	const RECT& GetWindowRect() const override { return windowRect_; }
	const RECT& GetClientRect() const override { return clientRect_; }
	int GetBitmapDataSize() const override { return bmpDataSize_; }
	HBITMAP GetBitmap() const override { return bitmap_; }
	void* GetBitmapAddress() const override { return bitsPtr_; }

protected:
	virtual bool InitDC(const BITMAPINFO& bitmapInfo) = 0;
	virtual bool DoCapture() = 0;

protected:
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