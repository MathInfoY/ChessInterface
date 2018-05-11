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