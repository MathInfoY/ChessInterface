using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Data;

namespace InterfaceChess
{
    public partial class Business
    {

        static public bool CompareBitmaps(Image left, Image right)
        {
            if (right == null)
                return (true);

            if (object.Equals(left, right))
                return true;
            if (left == null || right == null)
                return false;
            if (!left.Size.Equals(right.Size) || !left.PixelFormat.Equals(right.PixelFormat))
                return false;

            Bitmap leftBitmap = left as Bitmap;
            Bitmap rightBitmap = right as Bitmap;
            if (leftBitmap == null || rightBitmap == null)
                return true;

            #region Optimized code for performance

            int bytes = left.Width * left.Height * (Image.GetPixelFormatSize(left.PixelFormat) / 8);

            bool result = true;
            byte[] b1bytes = new byte[bytes];
            byte[] b2bytes = new byte[bytes];

            BitmapData bmd1 = leftBitmap.LockBits(new Rectangle(0, 0, leftBitmap.Width - 1, leftBitmap.Height - 1), ImageLockMode.ReadOnly, leftBitmap.PixelFormat);
            BitmapData bmd2 = rightBitmap.LockBits(new Rectangle(0, 0, rightBitmap.Width - 1, rightBitmap.Height - 1), ImageLockMode.ReadOnly, rightBitmap.PixelFormat);

            Marshal.Copy(bmd1.Scan0, b1bytes, 0, bytes);
            Marshal.Copy(bmd2.Scan0, b2bytes, 0, bytes);

            for (int n = 0; n <= bytes - 1; n++)
            {
                if (b1bytes[n] != b2bytes[n])
                {
                    result = false;
                    break;
                }
            }

            leftBitmap.UnlockBits(bmd1);
            rightBitmap.UnlockBits(bmd2);

            #endregion

            return result;
        }
        static public Boolean ValidateMove(byte dep, byte color)
        {
            Boolean isValid = true;

            CaseActivite[] CasesActivite = Board.getCasesAvailable();

            if (CasesActivite[dep].getActivite() != CaseActivite.Actif.CanPlay)
                isValid = false;
            else if (CasesActivite[dep].getColor() == "B" && color == K.Noir)
                isValid = false;
            else if (CasesActivite[dep].getColor() == "N" && color == K.Blanc)
                isValid = false;
            else if (CasesActivite[dep].getColor() == "-")
                isValid = false;
            else if (dep > 64)
                isValid = false;

            return (isValid);
        }

        static public byte ValidateRoque(byte firstCaseDepart,byte nextCaseDepart)
        {
            CaseActivite[] CasesActivite = Board.getCasesAvailable();

            byte valueRoque = 0;

            if (firstCaseDepart == 5 && nextCaseDepart == 1)
            {
                if (CasesActivite[1].getPiece() == "T" && CasesActivite[2].getColor() == "-" && CasesActivite[3].getColor() == "-" && CasesActivite[4].getColor() == "-" && CasesActivite[5].getPiece() == "T")
                    valueRoque = K.GRoque;
            }

            else if (firstCaseDepart == 5 && nextCaseDepart == 8)
            {
                if (CasesActivite[5].getPiece() == "R" && CasesActivite[6].getColor() == "-" && CasesActivite[7].getColor() == "-" && CasesActivite[8].getPiece() == "T")
                    valueRoque = K.PRoque;
            }

            else if (firstCaseDepart == 61 && nextCaseDepart == 64)
            {
                if (CasesActivite[61].getPiece() == "R" && CasesActivite[62].getColor() == "-" && CasesActivite[63].getColor() == "-" && CasesActivite[64].getPiece() == "T")
                    valueRoque = K.PRoque;
            }

            else if (firstCaseDepart == 61 && nextCaseDepart == 57)
            {
                if (CasesActivite[57].getPiece() == "T" && CasesActivite[58].getColor() == "-" && CasesActivite[59].getColor() == "-" && CasesActivite[60].getColor() == "-" && CasesActivite[61].getPiece() == "R")
                    valueRoque = K.GRoque;
            }
           
            return (valueRoque);
        }

        static private Boolean LegalMove(CaseActivite[] CasesActivite, byte source, byte destination, byte colorMove)
        {
            int noFunction = 0;
            Func<CaseActivite[], bool> FunctionPTR = null;
            Boolean isWhiteMove = colorMove == K.Blanc ? true : false;

            if (destination == source)
                return (false);

            if (CasesActivite[source].getColor() != "B" && isWhiteMove)
                return (false);

            if (CasesActivite[source].getColor() != "N" && !isWhiteMove)
                return (false);

            if (CasesActivite[source].getPiece() == "T")
            {
                if (ScanBoard.T[source, destination] == 0)
                    return (false);
            }
            else if (CasesActivite[source].getPiece() == "C")
            {
                if (ScanBoard.C[source, destination] == 0)
                    return (false);
            }

            else if (CasesActivite[source].getPiece() == "F")
            {
                if (ScanBoard.F[source, destination] == 0)
                    return (false);
            }

            else if (CasesActivite[source].getPiece() == "D")
            {
                if (ScanBoard.T[source, destination] == 0 && ScanBoard.F[source, destination] == 0)
                    return (false);
            }

            else if (CasesActivite[source].getPiece() == "R")
            {
                if (ScanBoard.R[source, destination] == 0)
                    return (false);
            }

            else if (CasesActivite[source].getPiece() == "P")
            {
                if (CasesActivite[source].getColor() == "B")
                {
                    if (ScanBoard.PB[source, destination] == 0 && ScanBoard.PBX[source, destination] == 0 && ScanBoard.PBPEP[source, destination] == 0)
                        return (false);

                    // Pion avance
                    if (ScanBoard.PB[source, destination] == 1 && CasesActivite[destination].isPiece())
                        return (false);

                    // Pion attaque une piece
                    if (ScanBoard.PBX[source, destination] == 1 && !CasesActivite[destination].isPiece())
                        return (false);

                    // Prise en Passant
                    if (ScanBoard.PBPEP[source, destination] == 1 && CasesActivite[destination].getPiece() != "P")
                        return (false);

                }

                else if (CasesActivite[source].getColor() == "N")
                {
                    if (ScanBoard.PN[source, destination] == 0 && ScanBoard.PNX[source, destination] == 0 && ScanBoard.PNPEP[source, destination] == 0)
                        return (false);

                    // Pion avance
                    if (ScanBoard.PN[source, destination] == 1 && CasesActivite[destination].isPiece())
                        return (false);

                    // Pion attaque une piece
                    if (ScanBoard.PNX[source, destination] == 1 && !CasesActivite[destination].isPiece())
                        return (false);

                    // Prise en Passant
                    if (ScanBoard.PNPEP[source, destination] == 1 && CasesActivite[destination].getPiece() != "P")
                        return (false);

                }
            }

            if (CasesActivite[destination].getColor() == CasesActivite[source].getColor())
                return (false);

            // Verifie l'interception entre le coup A et le coup B

            noFunction = source < destination ? ScanBoard.NoFunction[source, destination] : ScanBoard.NoFunction[destination, source];

            if (noFunction > 0)
            {
                FunctionPTR = Functions.FunctionLST[noFunction];

                if (!FunctionPTR(CasesActivite))
                {
                    return (false);
                }
            }


            return (true);
        }

        static private byte PossibleMovesSelectedCases(byte CaseDepart, byte LastMoveSrc, byte LastMoveDest, byte color)
        {
            byte caseArrivee = 0;
            CaseActivite[] CasesActivite = Board.getCasesAvailable();

            if (LegalMove(CasesActivite, CaseDepart, LastMoveSrc, color))
            {
                if (LegalMove(CasesActivite, CaseDepart, LastMoveDest,color))
                {
                    // Les 2 cases sont des coups possibles
                    caseArrivee = K.CSelected;
                }
                else
                    caseArrivee = LastMoveSrc;
            }
            else if (LegalMove(CasesActivite, CaseDepart, LastMoveDest,color))
                caseArrivee = LastMoveDest;

            return (caseArrivee);
        }

        /*
 * La couleur du Pion (assume) de la piece Jouée est ColorPiecePlayed (CaseDep = d5 (B) 36)
 * La couleur du Pion mangée corresponds à PieceArr et ColorPieceArr  (CaseArr = e5 (N) 37)    
 */
        static public bool PriseEnPassant(byte CaseDep, byte CaseArr)
        {
            CaseActivite[] BoardLogic = Board.getCasesAvailable();

            if (ScanBoard.PBPEP[CaseDep, CaseArr] == 1)
            {
                if (BoardLogic[CaseDep].getPiece() == "P")
                {
                    if (BoardLogic[CaseArr].getPiece() == "P" && BoardLogic[CaseArr].getColor() != BoardLogic[CaseDep].getColor())
                        return (true);
                }
            }

            return (false);
        }
    }
}