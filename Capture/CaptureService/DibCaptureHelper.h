#pragma once

#include "AbsCaptureHelper.h"

class DibCaptureHelper : public AbsCaptureHelper
{
public:
	DibCaptureHelper();
	virtual ~DibCaptureHelper();

protected:
	bool InitDC(const BITMAPINFO& bitmapInfo) override;
	bool DoCapture() override;

private:
	bool saveBitmap_;
	int mockPageNumber;
	int bmpCount_;
};

