using Microsoft.Office.Interop.PowerPoint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Core;
using Application = Microsoft.Office.Interop.PowerPoint.Application;

namespace PptSample
{
    public partial class ControlForm : Form
    {
        private Application _app; //PPT进程
        private readonly List<SlideShowWindow> _slideShowWindows = new List<SlideShowWindow>();

        public ControlForm()
        {
            InitializeComponent();
        }

        private void buttonDetect_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            _slideShowWindows.Clear();
            try
            {
                _app = Marshal.GetActiveObject("PowerPoint.Application") as Application;
            }
            catch (Exception exception)
            {
                textBox1.Text = "没有打开PPT";
                return;
            }

            if (_app == null)
            {
                textBox1.Text = "没有打开PPT";
                return;
            }

            var sb = new StringBuilder();
            sb.AppendLine($"共有{_app.Presentations.Count}个打开的PPT：");
            foreach (Presentation presentation in _app.Presentations)
            {
                sb.AppendLine($"{presentation.Path}\\{presentation.Name}");
                sb.AppendLine($"{presentation.Slides.Count}页");
                AddSlideShowWindow(presentation);
            }

            textBox1.Text = sb.ToString();
        }

        private void AddSlideShowWindow(Presentation presentation)
        {
            var slideShowSettings = presentation?.SlideShowSettings;
            if (slideShowSettings == null)
            {
                return;
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

            var window = slideShowSettings.Run();
            _slideShowWindows.Add(window);
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            var presentations = _app?.Presentations;
            if (presentations == null)
            {
                return;
            }

            try
            {
                foreach (var window in _slideShowWindows)
                {
                    window.View.Next();
                }

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void buttonPrev_Click(object sender, EventArgs e)
        {
            var presentations = _app?.Presentations;
            if (presentations == null)
            {
                return;
            }

            try
            {
                foreach (var window in _slideShowWindows)
                {
                    window.View.Previous();
                }

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }
}
