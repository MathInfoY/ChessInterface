using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.IO;
using Tool;

namespace InterfaceChess
{
   
    static public class Board
    {
//        static private Dictionary<byte, Bitmap> m_dictGrapheCases = new Dictionary<byte, Bitmap>();
//        static private Dictionary<byte, Bitmap> m_dictColorCases = new Dictionary<byte, Bitmap>();
//        static private CaseActivite[] m_CasesActivite;
        static private String[] m_noNameCase = new string[65];
        static private byte m_ColorBoardDown = 0;

        static public void Reset_Echiquier()
        {
            byte i = 0;

            for (i = 1; i <= 8; i++)
            {
                if (m_CasesActivite[i] == null) m_CasesActivite[i] = new CaseActivite();
                m_CasesActivite[i].setActivite(CaseActivite.Actif.CannotPlay);
                m_CasesActivite[i].setColor("B");
                m_CasesActivite[i].setCase(i);
            }
            m_CasesActivite[1].setPiece("T");
            m_CasesActivite[8].setPiece("T");
            m_CasesActivite[2].setPiece("C");
            m_CasesActivite[7].setPiece("C");
            m_CasesActivite[3].setPiece("F");
            m_CasesActivite[6].setPiece("F");
            m_CasesActivite[4].setPiece("D");
            m_CasesActivite[5].setPiece("R");


            m_CasesActivite[2].setActivite(CaseActivite.Actif.CanPlay);
            m_CasesActivite[7].setActivite(CaseActivite.Actif.CanPlay);

            for (i = 9; i <= 16; i++)
            {
                if (m_CasesActivite[i] == null) m_CasesActivite[i] = new CaseActivite();
                m_CasesActivite[i].setActivite(CaseActivite.Actif.CanPlay);
                m_CasesActivite[i].setPiece("P");
                m_CasesActivite[i].setColor("B");
                m_CasesActivite[i].setCase(i);
            }
            for (i = 17; i <= 48; i++)
            {
                if (m_CasesActivite[i] == null) m_CasesActivite[i] = new CaseActivite();
                m_CasesActivite[i].setActivite(CaseActivite.Actif.None);
                m_CasesActivite[i].setPiece("-");
                m_CasesActivite[i].setColor("-");
                m_CasesActivite[i].setCase(i);
            }

            for (i = 49; i <= 56; i++)
            {
                if (m_CasesActivite[i] == null) m_CasesActivite[i] = new CaseActivite();
                m_CasesActivite[i].setActivite(CaseActivite.Actif.CanPlay);
                m_CasesActivite[i].setPiece("P");
                m_CasesActivite[i].setColor("N");
                m_CasesActivite[i].setCase(i);
            }

            for (i = 57; i <= 64; i++)
            {
                if (m_CasesActivite[i] == null) m_CasesActivite[i] = new CaseActivite();
                m_CasesActivite[i].setActivite(CaseActivite.Actif.CannotPlay);
                m_CasesActivite[i].setColor("N");
                m_CasesActivite[i].setCase(i);
            }

            m_CasesActivite[57].setPiece("T");
            m_CasesActivite[64].setPiece("T");
            m_CasesActivite[58].setPiece("C");
            m_CasesActivite[63].setPiece("C");
            m_CasesActivite[59].setPiece("F");
            m_CasesActivite[62].setPiece("F");
            m_CasesActivite[60].setPiece("D");
            m_CasesActivite[61].setPiece("R");

            m_CasesActivite[58].setActivite(CaseActivite.Actif.CanPlay);
            m_CasesActivite[63].setActivite(CaseActivite.Actif.CanPlay);

        }


        static public CaseActivite[] getCasesAvailable()
        {
            return (m_CasesActivite);
        }

        static public CaseActivite[] getCloneCaseActivite()
        {
            CaseActivite[] copy = new CaseActivite[65];
            byte i = 0;

            foreach (CaseActivite value in m_CasesActivite)
            {
                if (value == null)
                {
                    i++;
                    continue;
                }
                copy[i] = new CaseActivite();
                copy[i].setCase(value.getNoCase());
                copy[i].setColor(value.getColor());
                copy[i].setPiece(value.getPiece());
                copy[i++].setActivite(value.getActivite());
            }

            return (copy);
        }


    

   

 

   
    }




    public class CaseActivite
    {
        public enum Actif
        {
            CanPlay,
            CannotPlay,
            None
        };

        public byte m_noCase;
        public string m_piece;
        public string m_color;
        public Actif m_activite;

        public CaseActivite()
        {
            m_noCase = 0;
            m_activite = Actif.None;
        }

        public byte getNoCase()
        {
            return (m_noCase);
        }

        public void setCase(byte noCase)
        {
            m_noCase = noCase;
        }

        public string getPiece()
        {
            return (m_piece);
        }

        public void setPiece(String piece)
        {
            m_piece = piece;
        }

        public string getColor()
        {
            return (m_color);
        }

        public void setColor(String color)
        {
            m_color = color;
        }

        public bool isPiece()
        {
            return (m_color != "-");
        }

        public Actif getActivite()
        {
            return (m_activite);
        }

        public void setActivite(Actif activite)
        {
            m_activite = activite;
        }
    }
}
