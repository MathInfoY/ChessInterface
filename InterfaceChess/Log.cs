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
        private static uint m_waiting_new_game = 0; 
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
                    writer.Write(noMove.ToString() + "." + move);
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
            else if (txt.Equals("Waiting..."))
            {
                if (m_waiting_new_game == 0)
                    File.AppendAllText(@m_interfaceLog, txt);
                else
                {
                    if (m_waiting_new_game == 50)
                        File.AppendAllText(@m_interfaceLog, txt + Environment.NewLine);
                    else
                        File.AppendAllText(@m_interfaceLog, ".");
                }
                m_waiting_new_game++;
            }
            else
            {
                File.AppendAllText(@m_interfaceLog, txt + Environment.NewLine);
                m_waiting_new_game = 0;
            }
        }

        static public  int GetNoFile()
        {
            return (m_countFile);
        }

        static public void UpdateNoGame()
        {
            int countFile = Log.GetIndexGame();
            Log.WriteIndexGame(countFile + 1);
        }  
    }

}