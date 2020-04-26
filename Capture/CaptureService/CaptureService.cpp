#include "stdafx.h"
#include "CaptureService.h"
#include "DibCaptureHelper.h"
#include "PrintCaptureHelper.h"


CaptureService::~CaptureService()
{
	Cleanup();
}

CaptureService& CaptureService::GetInstance()
{
	static CaptureService instance;
	return instance;
}

bool CaptureService::RegisterCapture(string name, string windowName, CaptureType type /* = CreateDibSection */)
{
	const auto hwnd = ::FindWindowA(nullptr, windowName.c_str());
	return RegisterCapture(name, hwnd, type);
}

bool CaptureService::RegisterCapture(string name, HWND hwnd, CaptureType type /* = CreateDibSection */)
{
	if (name.empty() || captureHelpers_.find(name) != captureHelpers_.end())
	{
		return false;
	}

	ICaptureHelper* helper;
	switch (type)
	{
	case CreateDibSection:
		helper = new DibCaptureHelper();
		break;
	case PrintWindow:
		helper = new PrintCaptureHelper();
		break;
	default:
		return false;
	}

	if (helper == nullptr)
	{
		return false;
	}

	if (!helper->Init(hwnd))
	{
		delete helper;
		return false;
	}

	captureHelpers_[name] = helper;
	return true;
}

void CaptureService::UnRegisterCapture(string name)
{
	if (name.empty() || captureHelpers_.find(name) == captureHelpers_.end())
	{
		return;
	}

	auto* captureHelper = captureHelpers_[name];
	if (captureHelper != nullptr)
	{
		captureHelper->Cleanup();
		delete captureHelper;
	}

	captureHelpers_.erase(name);
}

bool CaptureService::IsRegister(string name)
{
	return !name.empty() && captureHelpers_.find(name) != captureHelpers_.end();
}

bool CaptureService::RefreshWindow(string name)
{
	if (!IsRegister(name))
	{
		return false;
	}
	return captureHelpers_[name]->RefreshWindow();
}

bool CaptureService::ChangeWindowHandle(string name, string windowName)
{
	if (!IsRegister(name))
	{
		return false;
	}
	return captureHelpers_[name]->ChangeWindowHandle(windowName);
}

bool CaptureService::ChangeWindowHandle(string name, HWND hwnd)
{
	if (!IsRegister(name))
	{
		return false;
	}
	return captureHelpers_[name]->ChangeWindowHandle(hwnd);
}

bool CaptureService::Capture(string name)
{
	if (!IsRegister(name))
	{
		return false;
	}
	return captureHelpers_[name]->Capture();
}

bool CaptureService::GetWindowRect(string name, RECT& winRect)
{
	if (!IsRegister(name))
	{
		return false;
	}
	winRect = captureHelpers_[name]->GetWindowRect();
	return true;
}

bool CaptureService::GetClientRect(string name, RECT& clientRect)
{
	if (!IsRegister(name))
	{
		return false;
	}
	clientRect = captureHelpers_[name]->GetClientRect();
	return true;
}

bool CaptureService::GetBitmapDataSize(string name, int& bmpDataSize)
{
	if (!IsRegister(name))
	{
		return false;
	}
	bmpDataSize = captureHelpers_[name]->GetBitmapDataSize();
	return true;
}

bool CaptureService::GetBitmap(string name, HBITMAP& bitmap)
{
	if (!IsRegister(name))
	{
		return false;
	}
	bitmap = captureHelpers_[name]->GetBitmap();
	return true;
}

bool CaptureService::GetBitmapAddress(string name, void** bitsPtr)
{
	if (!IsRegister(name))
	{
		return false;
	}
	*bitsPtr = captureHelpers_[name]->GetBitmapAddress();
	return true;
}

void CaptureService::Cleanup()
{
	for (auto iter = captureHelpers_.cbegin(); iter != captureHelpers_.cend(); ++iter)
	{
		auto* captureHelper = iter->second;
		if (captureHelper != nullptr)
		{
			captureHelper->Cleanup();
			delete captureHelper;
		}
	}
	captureHelpers_.clear();
}