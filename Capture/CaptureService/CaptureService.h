#pragma once

#include "ICaptureHelper.h"
#include <map>
using std::map;

class CaptureService
{
public:
	CaptureService() = default;
	static CaptureService& GetInstance();

	enum CaptureType
	{
		//使用CreateDIBSection抓图，速度快，但是无法抓取D3D等渲染的窗口
		CreateDibSection = 0,

		//使用PrintWindow抓图，速度慢(16ms左右)，但是可以抓取D3D等渲染的窗口
		PrintWindow
	};

	bool RegisterCapture(string name, string windowName, CaptureType type = CreateDibSection); //注册抓图服务
	bool RegisterCapture(string name, HWND hwnd, CaptureType type = CreateDibSection); //注册抓图服务
	void UnRegisterCapture(string name); //注销抓图服务
	bool IsRegister(string name); //获取是否已注册抓图服务

	bool RefreshWindow(string name); //刷新窗口
	bool ChangeWindowHandle(string name, string windowName); //修改窗口句柄
	bool ChangeWindowHandle(string name, HWND hwnd); //修改窗口句柄
	bool Capture(string name); //抓图

	bool GetWindowRect(string name, RECT& winRect); //获取窗口尺寸
	bool GetClientRect(string name, RECT& clientRect); //获取窗口客户区尺寸
	bool GetBitmapDataSize(string name, int& bmpDataSize); //获取抓图数据大小
	bool GetBitmap(string name, HBITMAP& bitmap); //获取窗口位图
	bool GetBitmapAddress(string name, void** bitsPtr); //获取窗口位图地址

	void Cleanup(); //清理所有抓图服务

private:
	~CaptureService();

private:
	map<string, ICaptureHelper*> captureHelpers_;
};