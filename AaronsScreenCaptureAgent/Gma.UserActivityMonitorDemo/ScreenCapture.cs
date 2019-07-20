using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Text;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace Gma.UserActivityMonitorDemo
{
    public class ScreenCapture
    {
        #region Imports
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateDC(string strDriver, string strDevice,
            string strOutput, IntPtr pData);
        [DllImport("gdi32.dll")]
        public static extern bool DeleteDC(IntPtr hdc);
        [DllImport("gdi32.dll")]
        public static extern int GetPixel(IntPtr hdc, int x, int y);
        #endregion

        #region Private member variables
        private static Bitmap mBitmap;
        private string mFilename;
        private static IntPtr m_HBitmap;
        private string prefix = "";
        #endregion

        #region Public constructors
        public ScreenCapture()
        {
            new ScreenCapture("");
        }
        public ScreenCapture(string saveFileName)
        {
        }
        public ScreenCapture(string saveFileName, string saveFileName2)
        {
            prefix = saveFileName;
            if (prefix.Length > 0) prefix = prefix + " ";
            Bitmap bitmap = GetDesktopImage();
            SaveJpg(bitmap, saveFileName2);


            bool autoRepeat = false;
            if (autoRepeat)
            {
                while (1 == 1)
                {
                    System.Threading.Thread.Sleep(5000);
                    Bitmap bitmap2 = GetDesktopImage();
                    SaveJpg(bitmap2, saveFileName2);
                }
            }
        }
        #endregion

        #region Public properties

        /// <summary>
        /// Filename of the file to save
        /// </summary>
        public string Filename
        {
            get
            {
                return this.mFilename;
            }
            set
            {
                this.mFilename = value;
            }
        }

        /// <summary>
        /// Bitmap of the screen shot
        /// </summary>
        public Bitmap Bitmap
        {
            get
            {
                return mBitmap;
            }
            set
            {
                mBitmap = value;
            }
        }

        #endregion

        #region Public methods
        public string GetSafeFilename(string filename)
        {

            return string.Join("_", filename.Split(Path.GetInvalidFileNameChars()));

        }
        public void SaveJpg(Bitmap bitmap, string fileName)
        {
            string prefixText = prefix;
            if (System.Windows.Forms.Clipboard.ContainsText(System.Windows.Forms.TextDataFormat.Text))
            {
                string text = System.Windows.Forms.Clipboard.GetText(System.Windows.Forms.TextDataFormat.Text);
                if (text != prefixText)
                {
                    prefixText = text;
                }
            }
            prefixText = GetSafeFilename(prefixText);
            
            string currentDatetime = DateTime.Now.ToString("yyyy MMM d ddd hhmmss");
            string desktopFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string extension = ".jpg";
            if (prefixText.Length > 20) currentDatetime = "";
            string outputFile = desktopFolder + "\\" + prefixText + " " + currentDatetime + extension;
            try
            {
                FileStream fileStream = File.Create(outputFile);
                bitmap.Save(fileStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                fileStream.Close();

                if (TestFormStatic.ShellChecked)
                {
                    string text = TestFormStatic.ShellTextValue;
                    if (String.IsNullOrEmpty(text) == false)
                    {
                        //string commandLine = text + " " +  outputFile;
                        System.Diagnostics.Process.Start(text, "\"" + outputFile + "\"");
                    }
                }
            }
            catch { }
        }
        public struct SIZE
        {
            public int cx;
            public int cy;
        }
        public static Bitmap GetDesktopImage()
        {
            SIZE size;
            IntPtr hDC = PlatformInvokeUSER32.GetDC(PlatformInvokeUSER32.GetDesktopWindow());

            IntPtr hMemDC = PlatformInvokeGDI32.CreateCompatibleDC(hDC);
            size.cx = PlatformInvokeUSER32.GetSystemMetrics(PlatformInvokeUSER32.SM_CXSCREEN);
            size.cy = PlatformInvokeUSER32.GetSystemMetrics(PlatformInvokeUSER32.SM_CYSCREEN);


            m_HBitmap = PlatformInvokeGDI32.CreateCompatibleBitmap(hDC, size.cx, size.cy);
            if (m_HBitmap != IntPtr.Zero)
            {
                IntPtr hOld = (IntPtr)PlatformInvokeGDI32.SelectObject(hMemDC, m_HBitmap);
                PlatformInvokeGDI32.BitBlt(hMemDC, 0, 0, size.cx, size.cy, hDC, 0, 0, PlatformInvokeGDI32.SRCCOPY);
                PlatformInvokeGDI32.SelectObject(hMemDC, hOld);
                PlatformInvokeGDI32.DeleteDC(hMemDC);
                PlatformInvokeUSER32.ReleaseDC(PlatformInvokeUSER32.GetDesktopWindow(), hDC);
                mBitmap = System.Drawing.Image.FromHbitmap(m_HBitmap);
                return mBitmap;
            }
            return null;
        }
        #endregion

    }

    #region Auxillary win32 classes
    public class ConvMatrix
    {
        public int TopLeft = 0, TopMid = 0, TopRight = 0;
        public int MidLeft = 0, Pixel = 1, MidRight = 0;
        public int BottomLeft = 0, BottomMid = 0, BottomRight = 0;
        public int Factor = 1;
        public int Offset = 0;
        public void SetAll(int nVal)
        {
            TopLeft = TopMid = TopRight = MidLeft = Pixel = MidRight = BottomLeft = BottomMid = BottomRight = nVal;
        }
    }






    public class PlatformInvokeUSER32
    {
        public const int SM_CXSCREEN = 0;
        public const int SM_CYSCREEN = 1;

        [DllImport("user32.dll", EntryPoint = "GetDesktopWindow")]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll", EntryPoint = "GetDC")]
        public static extern IntPtr GetDC(IntPtr ptr);

        [DllImport("user32.dll", EntryPoint = "GetSystemMetrics")]
        public static extern int GetSystemMetrics(int abc);

        [DllImport("user32.dll", EntryPoint = "GetWindowDC")]
        public static extern IntPtr GetWindowDC(Int32 ptr);

        [DllImport("user32.dll", EntryPoint = "ReleaseDC")]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDc);

        public PlatformInvokeUSER32()
        {
        }
    }

    public struct SIZE
    {
        public int cx;
        public int cy;
    }

    public class PlatformInvokeGDI32
    {
        public const int SRCCOPY = 13369376;
        [DllImport("gdi32.dll", EntryPoint = "DeleteDC")]
        public static extern IntPtr DeleteDC(IntPtr hDc);

        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        public static extern IntPtr DeleteObject(IntPtr hDc);

        [DllImport("gdi32.dll", EntryPoint = "BitBlt")]
        public static extern bool BitBlt(IntPtr hdcDest, int xDest, int yDest, int wDest, int hDest, IntPtr hdcSource, int xSrc, int ySrc, int RasterOp);

        [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleBitmap")]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);

        [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleDC")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("gdi32.dll", EntryPoint = "SelectObject")]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr bmp);
    }

    #endregion

}
