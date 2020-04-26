#pragma once

#include "AbsCaptureHelper.h"

class PrintCaptureHelper : public AbsCaptureHelper
{
public:
	PrintCaptureHelper();
	virtual ~PrintCaptureHelper();

protected:
	bool InitDC(const BITMAPINFO& bitmapInfo) override;
	bool DoCapture() override;
};