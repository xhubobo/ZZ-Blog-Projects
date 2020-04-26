#include "stdafx.h"
#include "DibCaptureHelper.h"
#include <sstream>

static int BmpCount = 0;
static int BmpMaxCount = 50;

DibCaptureHelper::DibCaptureHelper()
	: saveBitmap_(false)
	, mockPageNumber(++BmpCount)
	, bmpCount_(0)
{
}

DibCaptureHelper::~DibCaptureHelper()
{
}

bool DibCaptureHelper::InitDC(const BITMAPINFO& bitmapInfo)
{
	scrDc_ = ::GetWindowDC(hwnd_);
	memDc_ = ::CreateCompatibleDC(scrDc_);

	bitmap_ = ::CreateDIBSection(memDc_, &bitmapInfo, DIB_RGB_COLORS, &bitsPtr_, nullptr, 0);
	if (bitmap_ == nullptr)
	{
		::DeleteDC(memDc_);
		::ReleaseDC(hwnd_, scrDc_);
		return false;
	}

	oldBitmap_ = static_cast<HBITMAP>(::SelectObject(memDc_, bitmap_));
	return true;
}

bool DibCaptureHelper::DoCapture()
{
	const auto clientRectWidth = clientRect_.right - clientRect_.left;
	const auto clientRectHeight = clientRect_.bottom - clientRect_.top;

	const auto ret = ::BitBlt(
		memDc_, 0, 0, clientRectWidth, clientRectHeight,
		scrDc_, 0, 0, SRCCOPY);

	return ret != 0;
}