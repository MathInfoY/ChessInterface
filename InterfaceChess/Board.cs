using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.IO;


namespace InterfaceChess
{
    static public class API
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

    static public class Board
    {
        static private int[] m_noCase_x; // Centre de la case
        static private int[] m_noCase_y; // Centre de la case
        static private int m_xLeftTop;   // Coin gauche de l'echiquier
        static private int m_yLeftTop;   // Coin gauche de l'echiquier
        static private int m_offset_x = 0; // Demi Largeur de la case
        static private int m_offset_y = 0; // Demi Hauteur de la case
        static private int m_width = 0;
        static private int m_height = 0;
        static private byte m_factorZoomCase = 0;
        static private Dictionary<byte, Bitmap> m_dictGrapheCases = new Dictionary<byte, Bitmap>();
        static private Dictionary<byte, Bitmap> m_dictColorCases = new Dictionary<byte, Bitmap>();
        static private CaseActivite[] m_CasesActivite;
        static private String[] m_noNameCase = new string[65];
        static private byte m_ColorBoardDown = 0;
//        static private Boolean m_BoardConfigured = false;


        static public void InitBoard(Boolean WhiteDownBoard = true)
        {            
            byte i = 0;


            if (m_ColorBoardDown == 0)
                m_ColorBoardDown = K.Blanc;

            try
            {
                m_noCase_x = new int[65];
                m_noCase_y = new int[65];
                m_CasesActivite = new CaseActivite[65];

                m_xLeftTop = -1;
                m_yLeftTop = -1;

                byte column = 1;

                for (i = 1; i <= 57; i = (byte)(i + 8), column++)
                {
                    m_noNameCase[i] = "a" + column;
                    m_noNameCase[i + 1] = "b" + column;
                    m_noNameCase[i + 2] = "c" + column;
                    m_noNameCase[i + 3] = "d" + column;
                    m_noNameCase[i + 4] = "e" + column;
                    m_noNameCase[i + 5] = "f" + column;
                    m_noNameCase[i + 6] = "g" + column;
                    m_noNameCase[i + 7] = "h" + column;
                }
            }
            catch (Exception)
            {

            }
        }

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

        static public void setColorBoard(byte color)
        {
            m_ColorBoardDown = color;
        }

        static public byte getColorBoard()
        {
            return (m_ColorBoardDown);
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

        static public void setCaseAvailable(byte noCase, CaseActivite.Actif value)
        {
            m_CasesActivite[noCase].setActivite(value);
        }

        static public Boolean isDefined()
        {
            return (m_xLeftTop != -1 && m_yLeftTop != -1);
        }

        static public void config_PositionBoard(int xLeftTop, int yLeftTop, int width, int height, byte factorZoomCase)
        {
            m_xLeftTop = xLeftTop;            // Coin Bas Gauche Echiquier
            m_yLeftTop = yLeftTop;
            m_width = width;
            m_height = height;
            m_offset_x = width / 16;
            m_offset_y = height / 16;
            m_factorZoomCase = factorZoomCase;

            //          File.AppendAllText(@"Board_Conf.txt", "/*****************************************/" + Environment.NewLine);

            for (byte i = 1; i <= 64; i++)
                config_PositionCase(i);

        }

        static public void Reset_GrapheCases()
        {
            Bitmap screenBmp = null;
            Bitmap screenColorBmp = null;

            m_dictColorCases.Clear();
            m_dictGrapheCases.Clear();

            for (byte i = 1; i <= 64; i++)
            {
                // On ne prends pas de screenshot de chaque coin de l'équichier, il y a un risque que la case soit
                // sélectionnée lors de la configuration de l'échiquier ce qu'on ne veut pas. Si c'est le cas alors 
                // le risque est tres élevé que l'application crache. Le screenshot de chaque case sera pris au 1er coup des noirs

                if ((i == 8 || i == 57))
                    screenBmp = null;
                else
                    screenBmp = TakePictureCase(i, m_factorZoomCase); // Facteur reduction
                m_dictGrapheCases.Add(i, screenBmp);

                // couleur de la case
                screenColorBmp = TakePictureCaseColor(i);
                m_dictColorCases.Add(i, screenColorBmp);
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

        static public void TakePictures(byte CaseDepart, byte CaseDestination, byte roque, byte colorMove)
        {
            Bitmap screenBmp = null;
            Bitmap screenColorCaseBmp = null;


            if (roque == K.PRoque)
            {
                screenBmp = TakePictureCase((byte)(colorMove == K.Blanc ? 5 : 61));
                m_dictGrapheCases[(byte)(colorMove == K.Blanc ? 5 : 61)] = screenBmp;

                screenBmp = TakePictureCase((byte)(colorMove == K.Blanc ? 6 : 62));
                m_dictGrapheCases[(byte)(colorMove == K.Blanc ? 6 : 62)] = screenBmp;

                screenBmp = TakePictureCase((byte)(colorMove == K.Blanc ? 7 : 63));
                m_dictGrapheCases[(byte)(colorMove == K.Blanc ? 7 : 63)] = screenBmp;

                screenBmp = TakePictureCase((byte)(colorMove == K.Blanc ? 8 : 64));
                m_dictGrapheCases[(byte)(colorMove == K.Blanc ? 8 : 64)] = screenBmp;

                // Color
                screenColorCaseBmp = TakePictureCaseColor((byte)(colorMove == K.Blanc ? 5 : 61));
                m_dictColorCases[(byte)(colorMove == K.Blanc ? 5 : 61)] = screenColorCaseBmp;

                screenColorCaseBmp = TakePictureCaseColor((byte)(colorMove == K.Blanc ? 6 : 62));
                m_dictColorCases[(byte)(colorMove == K.Blanc ? 6 : 62)] = screenColorCaseBmp;

                screenColorCaseBmp = TakePictureCaseColor((byte)(colorMove == K.Blanc ? 7 : 63));
                m_dictColorCases[(byte)(colorMove == K.Blanc ? 7 : 63)] = screenColorCaseBmp;

                screenColorCaseBmp = TakePictureCaseColor((byte)(colorMove == K.Blanc ? 8 : 64));
                m_dictColorCases[(byte)(colorMove == K.Blanc ? 8 : 64)] = screenColorCaseBmp;

            }
            else if (roque == K.GRoque)
            {
                screenBmp = TakePictureCase((byte)(colorMove == K.Blanc ? 1 : 57));
                m_dictGrapheCases[(byte)(colorMove == K.Blanc ? 1 : 57)] = screenBmp;

                screenBmp = TakePictureCase((byte)(colorMove == K.Blanc ? 2 : 58));
                m_dictGrapheCases[(byte)(colorMove == K.Blanc ? 2 : 58)] = screenBmp;

                screenBmp = TakePictureCase((byte)(colorMove == K.Blanc ? 3 : 59));
                m_dictGrapheCases[(byte)(colorMove == K.Blanc ? 3 : 59)] = screenBmp;

                screenBmp = TakePictureCase((byte)(colorMove == K.Blanc ? 4 : 60));
                m_dictGrapheCases[(byte)(colorMove == K.Blanc ? 4 : 60)] = screenBmp;

                screenBmp = TakePictureCase((byte)(colorMove == K.Blanc ? 5 : 61));
                m_dictGrapheCases[(byte)(colorMove == K.Blanc ? 5 : 61)] = screenBmp;

                // Color
                screenColorCaseBmp = TakePictureCaseColor((byte)(colorMove == K.Blanc ? 1 : 57));
                m_dictColorCases[(byte)(colorMove == K.Blanc ? 1 : 57)] = screenColorCaseBmp;

                screenColorCaseBmp = TakePictureCaseColor((byte)(colorMove == K.Blanc ? 2 : 58));
                m_dictColorCases[(byte)(colorMove == K.Blanc ? 2 : 58)] = screenColorCaseBmp;

                screenColorCaseBmp = TakePictureCaseColor((byte)(colorMove == K.Blanc ? 3 : 59));
                m_dictColorCases[(byte)(colorMove == K.Blanc ? 3 : 59)] = screenColorCaseBmp;

                screenColorCaseBmp = TakePictureCaseColor((byte)(colorMove == K.Blanc ? 4 : 60));
                m_dictColorCases[(byte)(colorMove == K.Blanc ? 4 : 60)] = screenColorCaseBmp;

                screenColorCaseBmp = TakePictureCaseColor((byte)(colorMove == K.Blanc ? 5 : 61));
                m_dictColorCases[(byte)(colorMove == K.Blanc ? 5 : 61)] = screenColorCaseBmp;
            }

            else
            {
                screenBmp = TakePictureCase(CaseDepart);
                m_dictGrapheCases[CaseDepart] = screenBmp;

                screenColorCaseBmp = TakePictureCaseColor(CaseDepart);
                m_dictColorCases[CaseDepart] = screenColorCaseBmp;

                screenBmp = TakePictureCase(CaseDestination);
                m_dictGrapheCases[CaseDestination] = screenBmp;

                screenColorCaseBmp = TakePictureCaseColor(CaseDestination);
                m_dictColorCases[CaseDestination] = screenColorCaseBmp;
            }

        }

        static public void UpdateBitmap(byte nocase)
        {
            Bitmap screenBmp = null;

            if (nocase > 0)
            {
                screenBmp = TakePictureCase(nocase);
                m_dictGrapheCases[nocase] = screenBmp;
            }
        }

        static public void UpdateColorCase(byte nocase)
        {
            if (nocase > 0)
            {
                m_dictColorCases[nocase] = TakePictureCaseColor(nocase);
            }
        }

        static public Bitmap getBitmap(byte noCase)
        {

            // Une exception peut se produire si on demarre subitement une nouvelle partie
            // La liste est effacee lors du demarrage et n'est plus valide ici.
            try
            {
                return (m_dictGrapheCases[noCase]);
            }
            catch(Exception)
            {
                return(null);
            }
            
        }

        static public void getPositionCase(byte noCase, out int x, out int y)
        {
            x = m_noCase_x[noCase];
            y = m_noCase_y[noCase];
        }

        static public byte getCase(int x, int y)
        {
            byte noCase = 0;

            for (byte i = 1; i <= 64 && noCase == 0; i++)
            {
                if (x >= m_noCase_x[i] - m_offset_x && x <= m_noCase_x[i] + m_offset_x)
                    if (y >= m_noCase_y[i] - m_offset_y && y <= m_noCase_y[i] + m_offset_y)
                        noCase = i;
            }

            if (m_ColorBoardDown == K.Noir)
            {
                if (noCase > 0)
                    return ((byte)(65 - noCase));
                else
                    return (noCase);
            }
            else
                return (noCase);
        }

        /*
         *  La position x et y est centre de la case
         *  
         *  Si FacteurZoomCase = 1 
         *  => Case Normale 
         *  
         *  Si FacteurZoomCase = 2
         *  => Case Dimension = 1/4 de la Case normale.  Coordonnée coin gauche 
         */
        static public Bitmap TakePictureCase(byte nocaseReceived, byte FactorZoomCase = 0)
        {
            byte nocase = nocaseReceived;

            if (m_ColorBoardDown == K.Noir)
                nocase = (byte)(65 - nocaseReceived);

            if (FactorZoomCase == 0) FactorZoomCase = m_factorZoomCase;

            int xPosScreen = m_noCase_x[nocase] - m_offset_x / FactorZoomCase;
            int yPosScreen = m_noCase_y[nocase] - m_offset_y / FactorZoomCase;
            int caseWidth = m_offset_x * 2 / FactorZoomCase;
            int caseHeight = m_offset_y * 2 / FactorZoomCase;

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

            // screenBmp.Save("C:\\Board\\Temp\\Coups\\Test_Picture.bmp", System.Drawing.Imaging.ImageFormat.MemoryBmp);

            return (screenBmp);

        }

        /*
         *  
         * */

        static public Bitmap TakePictureCaseColor(byte nocaseReceived)
        {
            byte nocase = nocaseReceived;

            if (m_ColorBoardDown == K.Noir)
                nocase = (byte)(65 - nocaseReceived);

            int xPosScreen = m_noCase_x[nocase] - 3;
            int yPosScreen = m_noCase_y[nocase] - 3;
            int caseWidth = 6;
            int caseHeight = 6;


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

            return (screenBmp);
        }


        static public Bitmap getColorCase(byte nocase)
        {
            return (m_dictColorCases[nocase]);
        }


        /*
         * 
         * Rotation de l'Echiquier : Blanc en haut Noir en Bas
         * 
         * Equation = y = -x + 65 ou x est la case
         *            et y est la nouvelle case en mirroir. 
         */

        static public byte getFlipBoard(byte x)
        {
            return ((byte)(65 - x));
        }

        static public int getXLeftTop()
        {
            return (m_xLeftTop);
        }

        static public int getYLeftTop()
        {
            return (m_yLeftTop);
        }

        static public int getWidth()
        {
            return (m_width);
        }

        static public int getHeight()
        {
            return (m_height);
        }

        static public String getNameCase(byte no)
        {
            return (m_noNameCase[no]);
        }
/*
        static public Boolean isBoardConfigured()
        {
            return (m_BoardConfigured);
        }

        static public void setBoardConfigured(Boolean isConfigured = true)
        {
            m_BoardConfigured = isConfigured;
        }
        */
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
