#include "stdafx.h"
#include "DibCaptureHelper.h"


DibCaptureHelper::DibCaptureHelper()
	: hwnd_(nullptr)
	, scrDc_(nullptr)
	, memDc_(nullptr)
	, bitmap_(nullptr)
	, oldBitmap_(nullptr)
	, bitsPtr_(nullptr)
	, windowRect_{ 0, 0, 0, 0 }
	, clientRect_{ 0, 0, 0, 0 }
	, bitbltStartPoint_{ 0,0 }
	, bmpDataSize_(0)
{
}


DibCaptureHelper::~DibCaptureHelper()
{
	Cleanup();
}

bool DibCaptureHelper::Init(const string& windowName)
{
	const auto handle = ::FindWindowA(nullptr, windowName.c_str());
	if (handle == nullptr)
	{
		return false;
	}

	return Init(handle);
}

bool DibCaptureHelper::Init(HWND hwnd)
{
	hwnd_ = hwnd;

	//获取窗口大小
	if (!::GetWindowRect(hwnd_, &windowRect_) || !::GetClientRect(hwnd_, &clientRect_))
	{
		return false;
	}

	const auto clientRectWidth = clientRect_.right - clientRect_.left;
	const auto clientRectHeight = clientRect_.bottom - clientRect_.top;
	bmpDataSize_ = clientRectWidth * clientRectHeight * 4;

	bitbltStartPoint_.x = 0;
	bitbltStartPoint_.y = 0;

	//位图信息
	BITMAPINFO bitmapInfo;
	bitmapInfo.bmiHeader.biSize = sizeof(bitmapInfo);
	bitmapInfo.bmiHeader.biWidth = clientRectWidth;
	bitmapInfo.bmiHeader.biHeight = clientRectHeight;
	bitmapInfo.bmiHeader.biPlanes = 1;
	bitmapInfo.bmiHeader.biBitCount = 32;
	bitmapInfo.bmiHeader.biSizeImage = clientRectWidth * clientRectHeight;
	bitmapInfo.bmiHeader.biCompression = BI_RGB;

	scrDc_ = ::GetWindowDC(hwnd_); //获取窗口DC
	memDc_ = ::CreateCompatibleDC(scrDc_); //缓冲内存DC
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

void DibCaptureHelper::Cleanup()
{
	if (bitmap_ == nullptr)
	{
		return;
	}

	//删除用过的对象
	::SelectObject(memDc_, oldBitmap_);
	::DeleteObject(bitmap_);
	::DeleteDC(memDc_);
	::ReleaseDC(hwnd_, scrDc_);

	hwnd_ = nullptr;
	scrDc_ = nullptr;
	memDc_ = nullptr;
	bitmap_ = nullptr;
	oldBitmap_ = nullptr;
	bitsPtr_ = nullptr;
}

bool DibCaptureHelper::RefreshWindow()
{
	const auto hwnd = hwnd_;
	Cleanup();
	return Init(hwnd);
}

bool DibCaptureHelper::ChangeWindowHandle(const string& windowName)
{
	Cleanup();
	return Init(windowName);
}

bool DibCaptureHelper::ChangeWindowHandle(HWND hwnd)
{
	Cleanup();
	return Init(hwnd);
}

bool DibCaptureHelper::Capture() const
{
	if (bitmap_ == nullptr || memDc_ == nullptr || scrDc_ == nullptr)
	{
		return false;
	}

	const auto clientRectWidth = clientRect_.right - clientRect_.left;
	const auto clientRectHeight = clientRect_.bottom - clientRect_.top;

	const auto ret = ::BitBlt(
		memDc_, 0, 0, clientRectWidth, clientRectHeight,
		scrDc_, bitbltStartPoint_.x, bitbltStartPoint_.y,
		SRCCOPY);
	return ret != 0;
}