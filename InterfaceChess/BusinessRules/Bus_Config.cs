using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Windows.Forms;
using Data;

namespace InterfaceChess

{
    public partial class Business
    {
        static private Bitmap m_colorCasePairEmpty = null;
        static private Bitmap m_colorCaseImPairEmpty = null;

        public Business()
        {

        }
        /*
            Blanc Humain     = Les Blancs n'ont pas encore joués     => status = 0
            Blanc Adversaire = Les Blancs ont joués le premier coup  => status = 1
            Blanc Adversaire = Les Blancs n'ont pas encore joués le premier coup  => status = -1
        */

        public static Boolean FirstOpponentWhiteMove(out byte CaseDepart, out byte CaseArrivee)
        {
            CaseActivite[] BoardLogic = Board.getCasesAvailable();
            byte noCaseDepart = 0;
            byte noCaseArrivee = 0;
            byte compteurDiff = 0;

            // Get template middle White color case #1 
            Bitmap screenBmpCase_9 = Board.TakePictureCaseColor(9);   // case 9 Pion Blanc 
            Bitmap screenBmpCase_17 = Board.TakePictureCaseColor(17); // case 17 blanche
            Bitmap screenBmpCase_18 = Board.TakePictureCaseColor(18); // case 18 noire

            CaseDepart = 0;
            CaseArrivee = 0;

            // La case d'arrivee est p-e sur la 4ieme rangée ?
            for (byte i = 25; i <= 32; i++)
            {
                if (!CompareBitmaps((Image)screenBmpCase_17, (Image)Board.TakePictureCaseColor(i)))
                {
                    if (!CompareBitmaps((Image)screenBmpCase_18, (Image)Board.TakePictureCaseColor(i)))
                    {
                        if (noCaseArrivee > 0)
                            noCaseArrivee = i;

                        noCaseArrivee = i;
                        noCaseDepart = (byte)(i - 16);
                    }
                }
            }

            // Sinon Le premier coup est p-e un pion (verifie 2ieme rangee) ?
            if (noCaseArrivee == 0)
            {
                // Verifie la 2ieme rangée de pions Blanc
                for (byte i = 9; i <= 16; i++)
                {
                    if (!CompareBitmaps((Image)screenBmpCase_9, (Image)Board.TakePictureCaseColor(i)))
                    {
                        if (noCaseDepart > 0)
                            compteurDiff++;

                        noCaseDepart = i;

                    }
                }

                if (compteurDiff > 5) noCaseDepart = 9;

                // La case d'arrivée se trouve donc sur la seconde rangée : Verification supplementaire
                // On vérifie la case d'arrivée elle ne doit pas être vide
                if (noCaseDepart > 0)
                {
                    if (!CompareBitmaps((Image)screenBmpCase_17, (Image)Board.TakePictureCaseColor((byte)(noCaseDepart+8))))
                    {
                        if (!CompareBitmaps((Image)screenBmpCase_18, (Image)Board.TakePictureCaseColor((byte)(noCaseDepart+8))))
                        {
                            noCaseArrivee = (byte)(noCaseDepart + 8);
                        }
                    }
                    
                }

            }

            // Alors c'est un coup de Cavalier...essaie de trouver la case d'arrivée. verifie la 3ieme rangee (4 cases à vérifier)
            if (noCaseArrivee == 0)
            {
                for (byte i = 17; i <= 24; i++)
                {
                    if (!CompareBitmaps((Image)screenBmpCase_17, (Image)Board.TakePictureCaseColor(i)))
                    {
                        if (!CompareBitmaps((Image)screenBmpCase_18, (Image)Board.TakePictureCaseColor(i)))
                        {
                            if (noCaseArrivee > 0)
                                noCaseArrivee = i;

                            noCaseArrivee = i;
                            noCaseDepart = (i <= (byte)19 ? (byte)2 : (byte)7);
                        }
                    }
                }
            }
            // Validation du coup
            if (LegalMove(BoardLogic, noCaseDepart, noCaseArrivee, K.Blanc))
            {
                CaseDepart = noCaseDepart;
                CaseArrivee = noCaseArrivee;
            }

            return (noCaseArrivee > 0);
        }

        static public String[] TranslateMoveInName(CaseActivite[] cloneActiviteCase, byte noCaseDepart, byte noCaseFin, byte colorMove)
        {
            String[] name = new String[1];

            name[0] = "";
            int l = name[0].Length;

            if (noCaseFin == K.PRoque)
            {
                name = new String[5];

                name = new[] { "0-0" };
            }
            else if (noCaseFin == K.GRoque)
            {
                name = new String[6];

                name = new[] { "0-0-0" };

            }

            else if (cloneActiviteCase[noCaseDepart].getPiece() == "T" || cloneActiviteCase[noCaseDepart].getPiece() == "C")
            {
                String[] nameTest = new String[5]; // 5 differents facons d'écrire le coup

                nameTest[0] = cloneActiviteCase[noCaseDepart].getPiece(); // T
                nameTest[1] = cloneActiviteCase[noCaseDepart].getPiece() + Board.getNameCase(noCaseDepart).Substring(0, 1); // Ta
                nameTest[2] = cloneActiviteCase[noCaseDepart].getPiece() + Board.getNameCase(noCaseDepart).Substring(1, 1); // T1
                nameTest[3] = cloneActiviteCase[noCaseDepart].getPiece(); // Ta
                nameTest[4] = nameTest[1];

                if (cloneActiviteCase[noCaseFin].getPiece() != "-")
                {
                    nameTest[1] += "x"; // Tax
                    nameTest[2] += "x"; // T1x
                    nameTest[3] += "x"; // Tx
                }

                nameTest[0] += Board.getNameCase(noCaseFin); // Ta2
                nameTest[1] += Board.getNameCase(noCaseFin); // Taxa2 Taa2
                nameTest[2] += Board.getNameCase(noCaseFin); // T1xa2 T1a2
                nameTest[3] += Board.getNameCase(noCaseFin); // Txa2
                nameTest[4] += Board.getNameCase(noCaseFin); // Taa2

                for (byte i = 1; i <= 64; i++)
                {
                    if (cloneActiviteCase[i].getPiece() == cloneActiviteCase[noCaseDepart].getPiece() &&
                        cloneActiviteCase[i].getColor() == cloneActiviteCase[noCaseDepart].getColor() &&
                        i != noCaseDepart)
                    {
                        if (LegalMove(cloneActiviteCase, i, noCaseFin,colorMove))
                        {
                            if (cloneActiviteCase[noCaseFin].getPiece() != "-")
                                name[0] = nameTest[1];
                            else
                                name[0] = nameTest[4];
                        }
                        else
                        {
                            if (cloneActiviteCase[noCaseFin].getPiece() != "-")
                                name[0] = nameTest[3];
                            else
                                name[0] = nameTest[0];
                        }

                    }
                    else if (name[0].Length == 0)
                    {
                        if (cloneActiviteCase[noCaseFin].getPiece() != "-")
                            name[0] = nameTest[3];
                        else
                            name[0] = nameTest[0];
                    }
                }
            }

           // Fxd4,Df3, etc...
            else if (cloneActiviteCase[noCaseDepart].getPiece() == "F" || cloneActiviteCase[noCaseDepart].getPiece() == "D")
            {
                name[0] = cloneActiviteCase[noCaseDepart].getPiece();

                if (cloneActiviteCase[noCaseFin].getPiece() != "-")
                    name[0] += "x";

                name[0] += Board.getNameCase(noCaseFin);
            }

            else if (cloneActiviteCase[noCaseDepart].getPiece() == "R")
            {
                name[0] = cloneActiviteCase[noCaseDepart].getPiece();

                if (cloneActiviteCase[noCaseFin].getPiece() != "-")
                    name[0] += "x";

                name[0] += Board.getNameCase(noCaseFin);
            }

            else if (cloneActiviteCase[noCaseDepart].getPiece() == "P")
            {
                if (cloneActiviteCase[noCaseFin].getPiece() != "-")
                {
                    name[0] = Board.getNameCase(noCaseDepart).Substring(0, 1);
                    name[0] += "x";
                    name[0] += Board.getNameCase(noCaseFin);
                }
                else
                {
                    name[0] = Board.getNameCase(noCaseFin);
                }

            }

            if (name == null)
            {
            }

            return (name);
        }

 

        private static Boolean SmallCastle(CaseActivite[] CasesActivite, byte source)
        {
            Boolean Roque = true; // 0-0

            if (source == 5)
            {
                Bitmap screenBmp = Board.TakePictureCase(8, Convert.ToByte(ConfigurationManager.AppSettings["ZoomOutCase"].ToString().Trim()));

                if (CompareBitmaps((Image)screenBmp, (Image)Board.getBitmap(8)))
                    Roque = false;
                else
                    Roque_Activity(K.PRoque, K.Blanc);
            }
            else if (source == 61)
            {
                Bitmap screenBmp = Board.TakePictureCase(64, Convert.ToByte(ConfigurationManager.AppSettings["ZoomOutCase"].ToString().Trim()));

                if (CompareBitmaps((Image)screenBmp, (Image)Board.getBitmap(64)))
                    Roque = false;
                else
                    Roque_Activity(K.PRoque, K.Noir);
            }

            else
                Roque = false;

            return (Roque);

        }

        private static Boolean BigCastle(CaseActivite[] CasesActivite, byte source)
        {
            Boolean Roque = true; // 0-0-0

            if (source == 5)
            {
                Bitmap screenBmp = Board.TakePictureCase(4, Convert.ToByte(ConfigurationManager.AppSettings["ZoomOutCase"].ToString().Trim()));

                if (CompareBitmaps((Image)screenBmp, (Image)Board.getBitmap(4)))
                    Roque = false;
                else
                    Roque_Activity(K.GRoque,K.Blanc);
            }
            else if (source == 61)
            {
                Bitmap screenBmp = Board.TakePictureCase(60, Convert.ToByte(ConfigurationManager.AppSettings["ZoomOutCase"].ToString().Trim()));

                if (CompareBitmaps((Image)screenBmp, (Image)Board.getBitmap(60)))
                    Roque = false;
                else
                    Roque_Activity(K.GRoque, K.Noir);
            }

            else
                Roque = false;

            return (Roque);

        }

    

        static public void SetColorCaseEmpty()
        {
            m_colorCaseImPairEmpty = Board.TakePictureCaseColor(39);
            m_colorCasePairEmpty   = Board.TakePictureCaseColor(40);           
        }
    }

}