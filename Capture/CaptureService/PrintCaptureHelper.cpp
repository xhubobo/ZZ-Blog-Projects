#include "stdafx.h"
#include "PrintCaptureHelper.h"


PrintCaptureHelper::PrintCaptureHelper()
{
}

PrintCaptureHelper::~PrintCaptureHelper()
{
}

bool PrintCaptureHelper::InitDC(const BITMAPINFO& bitmapInfo)
{
	scrDc_ = ::GetWindowDC(hwnd_);
	memDc_ = ::CreateCompatibleDC(scrDc_);

	bitmap_ = ::CreateDIBSection(scrDc_, &bitmapInfo, DIB_RGB_COLORS, &bitsPtr_, nullptr, 0);
	if (bitmap_ == nullptr)
	{
		::DeleteDC(memDc_);
		::ReleaseDC(hwnd_, scrDc_);
		return false;
	}
	
	oldBitmap_ = static_cast<HBITMAP>(::SelectObject(memDc_, bitmap_));
	return true;
}

bool PrintCaptureHelper::DoCapture()
{
	const auto ret = ::PrintWindow(hwnd_, memDc_, PW_CLIENTONLY | PW_RENDERFULLCONTENT);
	return ret != 0;
}