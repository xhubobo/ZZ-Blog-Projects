using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PptSample
{
    public partial class MainForm : Form
    {
        private const string OfficeFilter =
            "All files (*.*)|*.*|" +
            "PPT files (*.ppt;*.pptx)|*.ppt;*.pptx";

        private readonly PptHelper _pptHelper;

        #region Win32
        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern long SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter,
            int x, int y, int width, int height, uint flags);

        public const int GWL_STYLE = -16;

        public const long WS_CAPTION = 0x00C00000L;     /* WS_BORDER | WS_DLGFRAME  */
        public const long WS_BORDER = 0x00800000L;
        public const long WS_DLGFRAME = 0x00400000L;
        public const long WS_SYSMENU = 0x00080000L;
        public const long WS_THICKFRAME = 0x00040000L;

        public const int HWND_TOP = 0;

        public const uint SWP_NOACTIVATE = 0x0010;
        #endregion

        public MainForm()
        {
            InitializeComponent();

            _pptHelper = new PptHelper();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            MinimumSize = new Size(800, 480);
            _pptHelper.Init();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _pptHelper.UnInit();
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            //设置位置、大小
            if (_pptHelper.OfficeHandle > 0)
            {
                SetWindowPos(new IntPtr(_pptHelper.OfficeHandle), (IntPtr)HWND_TOP,
                    0, 0, pictureBox.Width, pictureBox.Height, SWP_NOACTIVATE);
            }
        }

        private void buttonBrowser_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                Filter = OfficeFilter
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                textBoxFilePath.Text = ofd.FileName;
                buttonOpen_Click(this, null);
            }
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            if (!_pptHelper.Open(textBoxFilePath.Text))
            {
                MessageBox.Show("Open PPT failed.");
                return;
            }

            //内嵌进Winform
            var hwnd = new IntPtr(_pptHelper.OfficeHandle);
            SetParent(hwnd, pictureBox.Handle);

            //设置无边框
            var wndStyle = GetWindowLong(hwnd, GWL_STYLE);
            wndStyle &= ~(int)WS_THICKFRAME;
            wndStyle &= ~(int)WS_BORDER;
            wndStyle &= ~(int)WS_DLGFRAME;
            wndStyle &= ~(int)WS_CAPTION;
            wndStyle &= ~(int)WS_SYSMENU;
            SetWindowLong(hwnd, GWL_STYLE, wndStyle);

            //设置位置、大小
            SetWindowPos(hwnd, (IntPtr)HWND_TOP,
                0, 0, pictureBox.Width, pictureBox.Height, SWP_NOACTIVATE);
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            _pptHelper.Close();
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            _pptHelper.Next();
        }

        private void buttonPrev_Click(object sender, EventArgs e)
        {
            _pptHelper.Previous();
        }

        private void buttonFirst_Click(object sender, EventArgs e)
        {
            _pptHelper.GotoSlide(1);
        }

        private void buttonLast_Click(object sender, EventArgs e)
        {
            _pptHelper.GotoSlide(_pptHelper.PageNumber);
        }
    }
}
