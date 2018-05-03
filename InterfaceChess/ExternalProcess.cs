using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;

namespace InterfaceChess
{

    class ExternalProcess
    {
        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll", ExactSpelling = true)]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll", ExactSpelling = true)]
        public static extern IntPtr BitBlt(IntPtr hDestDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

        [DllImport("user32.dll", EntryPoint = "GetDesktopWindow")]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("User32")]
        public static extern int MessageBox(int Hwnd, string text, string caption, int type);

        // Thread to call GMA Application
        private Thread m_Thread;
        MyThreadClass myThreadClassObject;

        public ExternalProcess()
        {
            m_Thread = new Thread(new ThreadStart(ThreadFunction));

            m_Thread.Start();
        }

        private void ThreadFunction()
        {
            myThreadClassObject = new MyThreadClass();
        }

        public void Start()
        {

        }

        ~ExternalProcess()
        {
            try
            {
                myThreadClassObject.m_process.Kill();
                myThreadClassObject.m_process.Close();
            }
            catch(Exception)
            {

            }
        }
    }

    public class MyThreadClass
    {
        public Process m_process;
        private Process[] nameProcess;

        public MyThreadClass()
        {
            string Visible = ConfigurationManager.AppSettings["GMA_Monitor_Visible"].ToString().Trim();

            nameProcess = Process.GetProcessesByName("Gma.UserActivityMonitorDemo");

            if (nameProcess.GetLength(0) == 0)
            {
                if (Visible.Length > 0)
                    m_process = Process.Start("Gma.UserActivityMonitorDemo.exe", Visible);
                else
                    m_process = Process.Start("Gma.UserActivityMonitorDemo.exe");
            }
        }

    }
}
