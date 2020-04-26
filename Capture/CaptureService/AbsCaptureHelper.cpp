#include "stdafx.h"
#include "AbsCaptureHelper.h"


AbsCaptureHelper::AbsCaptureHelper()
	: hwnd_(nullptr)
	, scrDc_(nullptr)
	, memDc_(nullptr)
	, bitmap_(nullptr)
	, oldBitmap_(nullptr)
	, bitsPtr_(nullptr)
	, windowRect_{ 0, 0, 0, 0 }
	, clientRect_{ 0, 0, 0, 0 }
	, bmpDataSize_(0)
{
}

AbsCaptureHelper::~AbsCaptureHelper()
{
	AbsCaptureHelper::Cleanup();
}

bool AbsCaptureHelper::Init(const string& windowName)
{
	const auto handle = ::FindWindowA(nullptr, windowName.c_str());
	if (handle == nullptr)
	{
		return false;
	}

	return Init(handle);
}

bool AbsCaptureHelper::Init(HWND hwnd)
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

	//位图信息
	BITMAPINFO bitmapInfo;
	bitmapInfo.bmiHeader.biSize = sizeof(bitmapInfo);
	bitmapInfo.bmiHeader.biWidth = clientRectWidth;
	bitmapInfo.bmiHeader.biHeight = clientRectHeight;
	bitmapInfo.bmiHeader.biPlanes = 1;
	bitmapInfo.bmiHeader.biBitCount = 32;
	bitmapInfo.bmiHeader.biSizeImage = clientRectWidth * clientRectHeight;
	bitmapInfo.bmiHeader.biCompression = BI_RGB;

	return InitDC(bitmapInfo);
}

void AbsCaptureHelper::Cleanup()
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

bool AbsCaptureHelper::RefreshWindow()
{
	const auto hwnd = hwnd_;
	Cleanup();
	return Init(hwnd);
}

bool AbsCaptureHelper::ChangeWindowHandle(const string& windowName)
{
	Cleanup();
	return Init(windowName);
}

bool AbsCaptureHelper::ChangeWindowHandle(HWND hwnd)
{
	Cleanup();
	return Init(hwnd);
}

bool AbsCaptureHelper::Capture()
{
	if (bitmap_ == nullptr || memDc_ == nullptr || scrDc_ == nullptr)
	{
		return false;
	}

	return DoCapture();
}