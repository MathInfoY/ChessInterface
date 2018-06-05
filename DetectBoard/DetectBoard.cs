using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.Remoting.Messaging;
using System.Messaging;


namespace DetectBoard
{
    public class API
    {
        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll", ExactSpelling = true)]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll", ExactSpelling = true)]
        public static extern IntPtr BitBlt(IntPtr hDestDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

        [DllImport("user32.dll", EntryPoint = "GetDesktopWindow")]
        public static extern IntPtr GetDesktopWindow();
    }


    public partial class CheckBoard : Form
    {
        static private int[] m_noCase_x; // Centre de la case
        static private int[] m_noCase_y; // Centre de la case
        static private int m_xLeftTop;   // Coin gauche de l'echiquier
        static private int m_yLeftTop;   // Coin gauche de l'echiquier
        static private int m_offset_x = 0; // Demi Largeur de la case
        static private int m_offset_y = 0; // Demi Hauteur de la case
        static private int m_width = 0;
        static private int m_height = 0;

        static private MessageQueue m_qSetupBoard = null;

        public CheckBoard()
        {
            bool isGoodBoard = false;

            InitializeComponent();

            m_noCase_x = new int[65];
            m_noCase_y = new int[65];

//          this.KeyPreview = true;

            this.KeyPress += new KeyPressEventHandler(StartUp_KeyPress);

            StartGMA();

            isGoodBoard = DB_BuildBoardFromFile(); // Display Board

            if (!isGoodBoard)
            {
                isGoodBoard = GC_GenerateCoordonnes();
            }
        }

        // Detect all numeric characters at the form level and consume 1, 
        // 4, and 7. Note that Form.KeyPreview must be set to true for this
        // event handler to be called.
        void StartUp_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= 48 && e.KeyChar <= 57)
            {
                MessageBox.Show("Form.KeyPress: '" +
                    e.KeyChar.ToString() + "' pressed.");

                switch (e.KeyChar)
                {
                    case (char)49:
                    case (char)52:
                    case (char)55:
                        MessageBox.Show("Form.KeyPress: '" +
                            e.KeyChar.ToString() + "' consumed.");
                        e.Handled = true;
                        break;
                }
            }
        }

        static private bool DB_BuildBoardFromFile()
        {
            int leftCorner;
            int rightCorner;
            int width;
            int height;

            // Lecture de la derniere position
            GetCoordBoard(out leftCorner, out rightCorner, out width, out height);

            config_PositionBoard(leftCorner, rightCorner, width,  height);

            TakeScreenShotBoard();

            if (DisplayBoard() == DialogResult.No)
                return (false);

            return (true);
        }
       
        static public bool GetCoordBoard(out int Board_xLeft, out int Board_yTop, out int Board_width, out int Board_Height)
        {
            Boolean isData = false;
            string[] words = null;
            String str = string.Empty;

            Board_xLeft = Board_yTop = Board_width = Board_Height = 0;

            str = File.ReadAllText(@"C:\\Chess\\CoordonneesBoard.txt", Encoding.ASCII);

            if (str != null && str.Length > 0)
            {
                words = str.Split(' ');

                Board_xLeft = Convert.ToInt32(words[0]);
                Board_yTop = Convert.ToInt32(words[1]);
                Board_width = Convert.ToInt32(words[2]);
                Board_Height = Convert.ToInt32(words[3]);

                isData = true;
            }

            return (isData);
        }

        static public void config_PositionBoard(int xLeftTop, int yLeftTop, int width, int height)
        {
            m_xLeftTop = xLeftTop;            // Coin Bas Gauche Echiquier
            m_yLeftTop = yLeftTop;
            m_width = width;
            m_height = height;
            m_offset_x = width / 16;
            m_offset_y = height / 16;

            for (byte i = 1; i <= 64; i++)
            {
                config_PositionCase(i);
                TakePictureCase(i);
            }
        }

        static public void config_PositionCase(byte noCase)
        {
            int xCase_A1 = m_xLeftTop + m_offset_x;
            int lenXCase = m_offset_x * 2;

            int yCase_A1 = m_yLeftTop + m_height - m_offset_y;
            int lenYCase = m_offset_y * 2;

            byte noRow = 0;

            if (noCase <= 8) noRow = 1;
            else if (noCase <= 16) noRow = 2;
            else if (noCase <= 24) noRow = 3;
            else if (noCase <= 32) noRow = 4;
            else if (noCase <= 40) noRow = 5;
            else if (noCase <= 48) noRow = 6;
            else if (noCase <= 56) noRow = 7;
            else if (noCase <= 64) noRow = 8;

            m_noCase_x[noCase] = xCase_A1 + (noCase - 1) % 8 * lenXCase;
            m_noCase_y[noCase] = yCase_A1 - (noRow - 1) * lenYCase;
        }

        static private void TakeScreenShotBoard()
        {
            String filename = "C:\\Board\\Board.bmp";

            Bitmap screenBmp = new Bitmap(m_width, m_height);

            Graphics g = Graphics.FromImage(screenBmp);

            IntPtr dc1 = API.GetDC(API.GetDesktopWindow());
            IntPtr dc2 = g.GetHdc();

            //Main drawing, copies the screen to the bitmap
            API.BitBlt(dc2, 0, 0, m_width, m_height, dc1, m_xLeftTop, m_yLeftTop, 13369376);

            //Clean up
            API.ReleaseDC(API.GetDesktopWindow(), dc1);
            g.ReleaseHdc(dc2);
            g.Dispose();

            try
            {
                screenBmp.Save(@filename, System.Drawing.Imaging.ImageFormat.MemoryBmp);
            }
            catch
            {

            }
            
        }

        static private Bitmap TakePictureCase(byte noCase)
        {
            string filename = string.Empty;
            int xPosScreen = m_noCase_x[noCase] - m_offset_x;
            int yPosScreen = m_noCase_y[noCase] - m_offset_y;
            int caseWidth = m_offset_x * 2;
            int caseHeight = m_offset_y * 2;

            Bitmap screenBmp = new Bitmap(caseWidth, caseHeight);
            Graphics g = Graphics.FromImage(screenBmp);

            IntPtr dc1 = API.GetDC(API.GetDesktopWindow());
            IntPtr dc2 = g.GetHdc();

            //Main drawing, copies the screen to the bitmap
            //last number is the copy constant
            API.BitBlt(dc2, 0, 0, caseWidth, caseHeight, dc1, xPosScreen, yPosScreen, 13369376);

            //Clean up
            API.ReleaseDC(API.GetDesktopWindow(), dc1);
            g.ReleaseHdc(dc2);
            g.Dispose();

            filename = "C:\\Board\\Case_" + noCase + ".bmp";
            screenBmp.Save(@filename, System.Drawing.Imaging.ImageFormat.MemoryBmp);

            return (screenBmp);

        }
 

        static private DialogResult DisplayBoard()
        { 
            Process.Start("C:\\Board\\Board.bmp");

            DialogResult Result = MessageBox.Show("L'échiquier est correct ?", "Note", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            return (Result);

        }

        static private bool GC_GenerateCoordonnes()
        {
            bool BoardIsInitialized = false;
            String msg = null;
            string pathBoardFile = string.Empty;

            GC_Init_ConfigBoard();

//          while (GC_ReadMessage_ConfigBoard() != "No Message");

            for (int counter = 0; counter < 1000; counter++)
            {
                Thread.Sleep(100);

                msg = GC_ReadMessage_ConfigBoard();


                // #1 Click sur Configuration 
                if (msg.Equals("ConfigBoard"))
                {
                    m_xLeftTop = 0;   // Coin gauche de l'echiquier
                    m_yLeftTop = 0;   // Coin gauche de l'echiquier
                    m_width = 0;
                    m_height = 0;
                }

                // #2 Lecture d'un coin suite au Click sur Configuration : 2 coins à lire.

                else if (msg != "No Message")
                {
                    if (ReadCoordonneesBoard(msg))
                    {
                        File.WriteAllText(@"C:\\Chess\\CoordonneesBoard.txt", m_xLeftTop.ToString() + " " + m_yLeftTop.ToString() + " " + m_width.ToString() + " " + m_height.ToString());
                        BoardIsInitialized = DB_BuildBoardFromFile();
                        if (BoardIsInitialized)
                            break;
                    }
                }

            }

            return (BoardIsInitialized);
        }

        static private void GC_Init_ConfigBoard()
        {
            if (MessageQueue.Exists(@".\Private$\MyQueueConfig"))
                m_qSetupBoard = new System.Messaging.MessageQueue(@".\Private$\MyQueueConfig");
            else
                m_qSetupBoard = MessageQueue.Create(@".\Private$\MyQueueConfig");
        }

        static private String GC_ReadMessage_ConfigBoard()
        {
            String message = String.Empty;

            System.Messaging.Message mes;

            try
            {
                mes = m_qSetupBoard.Receive(new TimeSpan(0, 0, 3));
                mes.Formatter = new XmlMessageFormatter(new String[] { "System.String,mscorlib" });
                message = mes.Body.ToString();
            }
            catch
            {
                message = "No Message";
            }

            return (message);
        }

        static private Boolean ReadCoordonneesBoard(String message)
        {
            byte i = 1;
            string data = string.Empty;

            int x = -1, y = -1;

            if (message.Contains("="))
            {
                message = message.Substring(message.IndexOf("=") + 1);

                string[] words = message.Split(';');

                foreach (string word in words)
                {
                    data = word.Substring(word.IndexOf('=') + 1);

                    // Coordonnees lues sont dans le coin bas gauche et le coin superieur droit
                    try
                    {
                        if (i == 1) x = Convert.ToInt32(data);
                        else if (i == 2) y = Convert.ToInt32(data);
                    }
                    catch
                    {
                    }

                    i++;
                }

                if (m_xLeftTop == 0) m_xLeftTop = x;
                else m_width = x - m_xLeftTop;

                if (m_yLeftTop == 0) m_yLeftTop = y;
                else m_height = y - m_yLeftTop;
            }

            return (m_width > 0 && m_height > 0);

        }

        static private void StartGMA()
        {

          Process m_process;
          Process[] nameProcess;

          string Visible = ConfigurationManager.AppSettings["GMA_Monitor_Visible"].ToString().Trim();

           nameProcess = Process.GetProcessesByName("Gma.UserActivityMonitorDemo");

           if (nameProcess.GetLength(0) == 0)
            {
                if (Visible.Length > 0)
                    m_process = Process.Start(@"C:\Users\yv.tremblay\Documents\Visual Studio 2013\Projects\Mouse\GlobalHook\Gma.UserActivityMonitorDemo\bin\Release\Gma.UserActivityMonitorDemo.exe", Visible);
                else
                    m_process = Process.Start(@"C:\Users\yv.tremblay\Documents\Visual Studio 2013\Projects\Mouse\GlobalHook\Gma.UserActivityMonitorDemo\bin\Release\Gma.UserActivityMonitorDemo.exe");
            }
        }
    }
}
