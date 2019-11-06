using System;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;
using Application = Microsoft.Office.Interop.PowerPoint.Application;

namespace PptSample
{
    internal class PptHelper
    {
        public int OfficeHandle => _window?.HWND ?? 0;
        public int PageNumber => _presentation?.Slides.Count ?? 0;
        public int CurPageIndex => _window?.View.CurrentShowPosition ?? 0;

        private Application _app; //PPT进程
        private Presentation _presentation; //演示文稿对象
        private SlideShowWindow _window; //幻灯片放映的窗口
        private string _filePath;

        public void Init()
        {
            if (_app != null)
            {
                return;
            }

            _app = new Application();
            _app.SlideShowBegin += OnSlideShowBegin;
            _app.SlideShowEnd += OnSlideShowEnd;
            _app.SlideShowNextSlide += OnSlideShowNextSlide;
            _app.SlideShowOnNext += OnSlideShowOnNext;
            _app.SlideShowOnPrevious += OnSlideShowOnPrevious;
        }

        public void UnInit()
        {
            if (_app == null)
            {
                return;
            }

            _app.SlideShowBegin -= OnSlideShowBegin;
            _app.SlideShowEnd -= OnSlideShowEnd;
            _app.SlideShowNextSlide -= OnSlideShowNextSlide;
            _app.SlideShowOnNext -= OnSlideShowOnNext;
            _app.SlideShowOnPrevious -= OnSlideShowOnPrevious;

            try
            {
                _app.Quit();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                _app = null;
            }

            GC.Collect();
        }

        #region PPT操作

        public bool Open(string filePath)
        {
            if (_app == null)
            {
                return false;
            }

            //正斜杠替换为反斜杠
            _filePath = filePath.Replace('/', '\\');

            try
            {
                //以非只读方式打开,方便操作结束后保存
                _presentation = _app.Presentations.Open(
                    _filePath,
                    MsoTriState.msoTrue, //ReadOnly: true
                    MsoTriState.msoTrue, //Untitled: true
                    MsoTriState.msoFalse); //WithWindow: false

                //获取真实分辨率及其比率
                //SlideWidth：幻灯片的宽度（以磅为单位）
                //SlideHeight：幻灯片的高度（以磅为单位）
                var width = _presentation.PageSetup.SlideWidth;
                var height = _presentation.PageSetup.SlideHeight;
                Console.WriteLine($"OfficeWidth: {width}, OfficeHeight: {height}");

                var slideShowSettings = _presentation?.SlideShowSettings;
                if (slideShowSettings == null)
                {
                    return false;
                }

                //窗口设置
                //ppShowTypeSpeaker 1   演讲者
                //ppShowTypeWindow  2   窗口
                //ppShowTypeKiosk   3   展台
                slideShowSettings.ShowType = PpSlideShowType.ppShowTypeWindow;
                //ppSlideShowManualAdvance      手动换页
                //ppSlideShowUseSlideTimings    为每张幻灯片指定的计时
                //ppSlideShowRehearseNewTimings 排练计时
                //slideShowSettings.AdvanceMode = PpSlideShowAdvanceMode.ppSlideShowManualAdvance;
                slideShowSettings.ShowScrollbar = MsoTriState.msoFalse;
                slideShowSettings.ShowPresenterView = MsoTriState.msoFalse; //返回SlideShowSettings对象的演示者视图
                slideShowSettings.ShowMediaControls = MsoTriState.msoFalse; //允许访问SlideShowSettings对象中的媒体控件
                slideShowSettings.ShowWithNarration = MsoTriState.msoFalse; //确定显示指定幻灯片放映时是否有旁白
                //是否显示页内动画
                slideShowSettings.ShowWithAnimation = MsoTriState.msoTrue;

                _window = slideShowSettings.Run();

                try
                {
                    //设置位置
                    _window.Left = -10000;
                    _window.Top = 0;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"设置PPT位置失败: {e.Message}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"打开PPT失败: FilePath: {_filePath}, ErrMsg: {e.Message}");
                return false;
            }

            return true;
        }

        public void Close()
        {
            try
            {
                _window?.View.Exit();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Close {_filePath} exception: {e.Message}");
            }

            //关闭窗口
            _window = null;
            _presentation = null;
        }

        public void GotoSlide(int index)
        {
            try
            {
                _window?.View.GotoSlide(index);
            }
            catch (Exception e)
            {
                Console.WriteLine($"GotoSlide {_filePath} exception: {e.Message}");
            }
        }

        public void Previous()
        {
            try
            {
                _window?.View.Previous();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Previous {_filePath} exception: {e.Message}");
            }
        }

        public void Next()
        {
            try
            {
                _window?.View.Next();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Next {_filePath} exception: {e.Message}");
            }
        }

        #endregion

        #region PPT事件

        private void OnSlideShowBegin(SlideShowWindow window)
        {
            var pos = window.View.CurrentShowPosition;
            var time = DateTime.Now.ToString("HH:mm:ss fff");
            Console.WriteLine($"OnSlideShowBegin: {pos}, {time}");
        }

        private void OnSlideShowEnd(Presentation presentation)
        {
            Console.WriteLine("OnSlideShowEnd");
        }

        private void OnSlideShowNextSlide(SlideShowWindow window)
        {
            var pos = window.View.CurrentShowPosition;
            Console.WriteLine($"OnSlideShowNextSlide: {pos}");
        }

        private void OnSlideShowOnNext(SlideShowWindow window)
        {
            var pos = window.View.CurrentShowPosition;
            Console.WriteLine($"OnSlideShowOnNext: {pos}");
        }

        private void OnSlideShowOnPrevious(SlideShowWindow window)
        {
            var pos = window.View.CurrentShowPosition;
            Console.WriteLine($"OnSlideShowOnPrevious: {pos}");
        }

        #endregion
    }
}
