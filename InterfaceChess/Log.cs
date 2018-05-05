using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;


namespace InterfaceChess
{
    static public class Log
    {
        private static int m_countFile = 0;
        private static uint m_waiting = 0;
        private static string m_pathFileBoard = string.Empty;
        private static string m_pathGameFile = string.Empty;
        private static string m_pathPiecesFile = string.Empty;
        private static string m_indexFile = string.Empty;
        private static string m_Board = string.Empty;
        private static string m_interfaceLog = string.Empty;
 
        static public void InitializeLog()
        {
            m_pathFileBoard = ConfigurationManager.AppSettings["PathFileBoard"].ToString().Trim();
            m_pathGameFile = ConfigurationManager.AppSettings["PathGamesFile"].ToString().Trim();
            m_pathPiecesFile = ConfigurationManager.AppSettings["PathPiecesFile"].ToString().Trim();
            m_Board = ConfigurationManager.AppSettings["Board"].ToString().Trim();
            m_indexFile = ConfigurationManager.AppSettings["IndexFile"].ToString().Trim();
            m_interfaceLog = ConfigurationManager.AppSettings["Interface"].ToString().Trim();
        }

        static public void Init()
        {
            try
            {
                File.Delete(@m_interfaceLog);
            }
            catch(Exception)
            {

            }
        }

        static public int GetIndexGame()
        {
            return (Convert.ToInt32(File.ReadAllText(@m_indexFile, Encoding.ASCII)));
        }

        static public void WriteIndexGame(int counter)
        {
            File.WriteAllText(@m_indexFile, (counter).ToString());
            m_countFile = counter;
        }

        static public bool GetCoordBoard(out int Board_xLeft, out int Board_yTop, out int Board_width, out int Board_Height)
        {
            Boolean isData = false;
            string[] words = null;
            String str = string.Empty;

            Board_xLeft = Board_yTop = Board_width = Board_Height = 0;

            str = File.ReadAllText(@m_pathFileBoard, Encoding.ASCII);

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

        static public void WriteCoordBoard(int Board_xLeft, int Board_yTop, int Board_width, int Board_Height)
        {
            File.WriteAllText(@m_pathFileBoard, Board_xLeft.ToString() + " " + Board_yTop.ToString() + " " + Board_width.ToString() + " " + Board_Height.ToString());
        }

        static public void LogPieces(List<String[]> pieces, List<String> color, List<String> activity)
        {

            int noGame = GetIndexGame();

            using (StreamWriter writer = new StreamWriter(m_pathPiecesFile  + noGame + "_" + noGame + ".txt"))
            {
                for (byte i = 1; i <= 64; i++)
                {
                    writer.WriteLine(i + "\t" + pieces[i] + "\t" + color[i] + "\t" + activity[i]);
                }
            }
        }

        static public void LogCoups(String move, int color, byte noMove, byte WhoPlayWhite)
        {
            using (StreamWriter writer = new StreamWriter(m_pathGameFile + m_countFile  + ".txt", true))
            {
                if (color == K.Blanc)
                    writer.Write(noMove.ToString() + "\t" + move);
                else
                    writer.Write("," + move + Environment.NewLine);
            }
        }

        static public void LogText(string txt)
        {
            if (txt.Equals("...-"))
            {
                if (m_waiting <= 50)
                {
                    File.AppendAllText(@m_interfaceLog, txt);
                    m_waiting++;
                }
                else
                {
                    File.AppendAllText(@m_interfaceLog, txt + Environment.NewLine);
                    m_waiting = 0;
                }               
            }
            else 
                File.AppendAllText(@m_interfaceLog, txt + Environment.NewLine);
        }

        static public  int GetNoFile()
        {
            return (m_countFile);
        }

        static public void PhotoBoard()
        {
            TakeScreenShotBoard();
/*
            Bitmap screenBmpCaseTest = null;
            String filename = string.Empty;

            for (byte i = 1; i <= 64; i++)
            {
                filename = "C:\\Board\\Case_" + i + ".bmp";
                screenBmpCaseTest = Board.TakePictureCase(i, 1);
                screenBmpCaseTest.Save(@filename, System.Drawing.Imaging.ImageFormat.MemoryBmp);
            }
 */ 
        }

        static private void TakeScreenShotBoard()
        {
            if (Board.isDefined())
            {
                int x, y, w, h;

                x = Board.getXLeftTop();
                y = Board.getYLeftTop();
                w = Board.getWidth();
                h = Board.getHeight();

                Bitmap screenBmp = new Bitmap(Board.getWidth(), Board.getHeight());
                Graphics g = Graphics.FromImage(screenBmp);

                IntPtr dc1 = API.GetDC(API.GetDesktopWindow());
                IntPtr dc2 = g.GetHdc();

                //Main drawing, copies the screen to the bitmap
                //last number is the copy constant
                API.BitBlt(dc2, 0, 0, Board.getWidth(), Board.getHeight(), dc1, Board.getXLeftTop(), Board.getYLeftTop(), 13369376);

                //Clean up
                API.ReleaseDC(API.GetDesktopWindow(), dc1);
                g.ReleaseHdc(dc2);
                g.Dispose();

                try
                {
                    screenBmp.Save(@m_Board, System.Drawing.Imaging.ImageFormat.MemoryBmp);
                }
                catch
                {

                }
            }
        }

        static public void UpdateNoGame()
        {
            int countFile = Log.GetIndexGame();
            Log.WriteIndexGame(countFile + 1);
        }  
    }

}