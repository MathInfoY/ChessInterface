using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Data;

namespace InterfaceChess
{
    public partial class Business
    {
/*
         Si findMove = -1   => trouve 2 coups qui ont joués (joué rapidement)   
         Si findMove =  1   => retourne le coup Depart ou le roque
         Si findMove =  0   => aucun coup trouve
*/

//  m_DateFirstHitNoCase[i] = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt");


        static public short GetDepartMovePlayer(byte lastDep, byte lastDest, byte color, out byte roque, List<byte> Departs)
        {
            Bitmap screenBmp = null;
            byte caseRoi = 0;
            byte caseTour = 0;
            short findMove = 0;
           
            roque = 0;

            CaseActivite[] BoardLogic = Board.getCasesAvailable();

            // Coup Depart
            for (byte i = 1; i <= 64; i++)
            {
                if (!Business.ValidateMove(i, color))
                    continue;

                if (i == lastDep || i == lastDest)
                    continue;

                screenBmp = Board.TakePictureCase(i, Convert.ToByte(ConfigurationManager.AppSettings["ZoomOutCase"].ToString().Trim()));

                if (!Business.CompareBitmaps((Image)screenBmp, (Image)Board.getBitmap(i)))
                {
                    if (i == 1 || i == 8 || i == 57 || i == 64) caseTour = i;
                    else if (i == 5 || i == 61) caseRoi = i;
                    Departs.Add(i);
                    Log.LogText("Depart ---> " + i);
                }

            }

            if (caseRoi > 0 && caseTour > 0)
            {
                roque = Business.ValidateRoque(caseRoi, caseTour);
                if (roque > 0)
                    findMove = 1;
                else
                    findMove = -1;
            }
            else
            {
                if (Departs.Count == 1)
                    findMove = 1;
                else if (Departs.Count > 1)
                    findMove = -1;
            }

            return (findMove);
        }

        static public short GetDestMovePlayer(byte LastDep, byte LastDest, byte CaseDepart, byte color, List<byte> DestMoves, out bool PriseEnPassant)
        {
            List<byte> findDestMoves = null;
            short moveFoundDest = 0; // aucun coup trouve

            PriseEnPassant = false;

            findDestMoves = FindMovesChanged(LastDep, LastDest, CaseDepart, color, out PriseEnPassant);

            if (findDestMoves.Count > 1)
                moveFoundDest = -1;
            else if (findDestMoves.Count == 1)
                moveFoundDest = 1;

            for (byte i = 0; i < findDestMoves.Count(); i ++)
                DestMoves.Add(findDestMoves[i]);

            return (moveFoundDest);
        }

        static public short GetDestSquareWebService(byte LastDep, byte LastDest, byte CaseDepart, byte color, List<byte> DestMoves, out bool PriseEnPassant)
        {
            List<byte> findDestMoves = null;
            short moveFoundDest = 0; // aucun coup trouve

            PriseEnPassant = false;

            findDestMoves = FindMovesSquareWebService(LastDep, LastDest, CaseDepart, color, out PriseEnPassant);

            if (findDestMoves.Count > 1)
                moveFoundDest = -1;
            else if (findDestMoves.Count == 1)
                moveFoundDest = 1;

            for (byte i = 0; i < findDestMoves.Count(); i++)
                DestMoves.Add(findDestMoves[i]);

            return (moveFoundDest);
        }


        static public short TestDepartAgain(byte LastDep, byte LastArr, byte color, out byte roque,List<byte> Departs, int timer = 500)
        {
            short FindMoveDep = 0;
             
            roque = 0;
           
            Log.LogText("Test Depart Again ...");
            Thread.Sleep(timer);
            FindMoveDep = Business.GetDepartMovePlayer(LastDep, LastArr, color, out roque, Departs);
            
            return (FindMoveDep);
        }

        static public short TestDestAgain(byte LastDep, byte LastArr, byte CaseDepart, byte color, List<byte> LstMoves, out bool PriseEnPassant, int timer = 500)
        {
            List<byte> findDestMoves = null;
            short FindMoveDest = 0;
            byte CaseArrivee = 0;

            PriseEnPassant = false;

            Log.LogText("Test Again ...");
            Thread.Sleep(timer);

            findDestMoves = FindMovesChanged(LastDep, LastArr, CaseDepart, color, out PriseEnPassant);

            if (findDestMoves.Count == 1)
                FindMoveDest = 1;
            else if (findDestMoves.Count == 2)
            {                
                CaseArrivee = FindPawnCaseDestination(CaseDepart, findDestMoves);
                if (CaseArrivee > 0)
                {
                    findDestMoves.Clear();
                    findDestMoves.Add(CaseArrivee);
                    FindMoveDest = 1;
                }
                else
                    FindMoveDest = -1;
            }
            else if (findDestMoves.Count > 2)
                FindMoveDest = -1;

// FindPawnCaseDestination(byte CaseDepart,List<byte> CasesDest, byte color, out byte response)
            if (findDestMoves != null)
            {
                for (byte i = 0; i < findDestMoves.Count(); i++)
                    LstMoves.Add(findDestMoves[i]);
            }
            return (FindMoveDest);
        }

        static private List<byte> FindMovesChanged(byte LastDep, byte LastArr, byte CaseDepart, byte color, out bool isPriseEnPassant)
        {
            List<byte> findNewMoves = new List<byte>();
            Bitmap colorCaseArr = null;
            Bitmap screenBmp = null;
            Boolean isFind = false;
            Boolean test = false;

            CaseActivite[] CasesActivite = Board.getCasesAvailable();
            isPriseEnPassant = false;

/*
            List<byte> activeCases = new List<byte>();

            for (byte k = 1; k <= 64; k++ )
            {
                if (ScanBoard.linkedCases[CaseDepart, k] == 1)
                {
                    activeCases.Add(k);
                }
            }
  
            foreach (byte val in activesCases)
            {
 
            }
*/

            for (byte i = 1; i <= 64; i++)
            {
                if (i == LastDep || i == LastArr)
                    continue;

                if (!LegalMove(CasesActivite, CaseDepart, i, color))
                    continue;

                if (test)
                    Log.LogText("Legal Depart = " + CaseDepart + "\t Arrivee = " + i);

                screenBmp = Board.TakePictureCase(i, Convert.ToByte(ConfigurationManager.AppSettings["ZoomOutCase"].ToString().Trim()));

                if (!CompareBitmaps((Image)screenBmp, (Image)Board.getBitmap(i)))
                {
                    colorCaseArr = Board.TakePictureCaseColor(i);

                    Log.LogText("Arrivee ---> " + i);
                    isFind = true;

                    if (!CompareBitmaps((Image)colorCaseArr, (Image)m_colorCasePairEmpty) && !CompareBitmaps((Image)colorCaseArr, (Image)m_colorCaseImPairEmpty))
                        findNewMoves.Add(i);
                    else if (PriseEnPassant(CaseDepart, i))
                    {
                        findNewMoves.Add(i);
                        isPriseEnPassant = true;
                    }
                }
            }

            if (findNewMoves.Count == 0 && isFind == true)
            {
                Log.LogText("Arrivee Case Vide (Err)");
            }
            return (findNewMoves);
        }

        static private List<byte> FindMovesSquareWebService(byte LastDep, byte LastArr, byte CaseDepart, byte color, out bool isPriseEnPassant)
        {
            List<byte> findNewMoves = new List<byte>();

            SquareTimeProcessingService.SquareTimeProcessingServiceClient client = new SquareTimeProcessingService.SquareTimeProcessingServiceClient("BasicHttpBinding_ISquareTimeProcessingService");

            CaseActivite[] CasesActivite = Board.getCasesAvailable();
            isPriseEnPassant = false;

            for (byte i = 1; i <= 64; i++)
            {
                if (i == LastDep || i == LastArr)
                    continue;

                if (!LegalMove(CasesActivite, CaseDepart, i, color))
                    continue;

                if (client.GetFirstHit(i) != default(DateTime))
                {
                    Log.LogText("WPService Arrivee ---> " + i);

                    if (PriseEnPassant(CaseDepart, i))
                        isPriseEnPassant = true;

                    findNewMoves.Add(i);
                }
            }

            return (findNewMoves);
        }

        static public void Set_Activities_Move(byte CaseDepart, byte CaseDest,byte color,bool PriseEnPassant)
        {
            // Roque
            if (!Roque_Activity(CaseDepart, color))
            {
                // Pion
                if(!Pawn_Activity(CaseDepart, CaseDest, color, PriseEnPassant))
                {
                    // Toutes les pieces sauf le Pion et le roque
                    Pieces_Activity(CaseDepart, CaseDest);
                }
            }

            // Active les cases du voisinage
            EnablePieces_Neighbour_Activity(CaseDepart);                      
        }

        static public short WhichCaseSelected(byte LastDep, byte LastDest, byte CaseDepart, byte color, out byte CaseDest, out bool PriseEnPassant)
        {
            short findMoveDest = 0;            
            Bitmap colorDepart = null;
            CaseActivite[] BoardLogic = Board.getCasesAvailable();
            PriseEnPassant = false;

            CaseDest = 0;

            if (LastDep == 0)
                return (0);

            CaseDest = PossibleMovesSelectedCases(CaseDepart, LastDep, LastDest, color);
            
            Log.LogText("Adversaire... CaseSelectionnee CaseDest :" + CaseDest);

            // Deux cases possibles (coup precedent m_LastMoveSrc et m_LastMoveDest)
            if (CaseDest == K.CSelected)
            {
                colorDepart = Board.TakePictureCaseColor(LastDep);

                // Si la case de depart est vide alors le seul coup possible est la case d'arrivee  
                if (CompareBitmaps((Image)colorDepart, (Image)m_colorCasePairEmpty) ||
                    CompareBitmaps((Image)colorDepart, (Image)m_colorCaseImPairEmpty))
                {
                    if (Business.PriseEnPassant(CaseDepart,LastDest))
                        PriseEnPassant = true;

                    CaseDest = LastDest;
                }
                else
                    CaseDest = LastDep;

                findMoveDest = 1;
            }
            
            // Un coup trouve
            else if (CaseDest > 0)
                findMoveDest = 1;

            return (findMoveDest); 
        }

        static private bool Roque_Activity(byte roque,byte color)
        {
            bool isRoque = true;
            CaseActivite[] BoardLogic = Board.getCasesAvailable();

            if (roque == K.PRoque && color == K.Blanc)
            {
                BoardLogic[5].setColor("-");
                BoardLogic[5].setPiece("-");
                BoardLogic[5].setActivite(CaseActivite.Actif.None);

                BoardLogic[6].setColor("B");
                BoardLogic[6].setPiece("T");
                BoardLogic[6].setActivite(CaseActivite.Actif.CanPlay);

                BoardLogic[7].setColor("B");
                BoardLogic[7].setPiece("R");
                BoardLogic[7].setActivite(CaseActivite.Actif.CanPlay);

                BoardLogic[8].setColor("-");
                BoardLogic[8].setPiece("-");
                BoardLogic[8].setActivite(CaseActivite.Actif.None);
            }
            else if (roque == K.GRoque && color == K.Blanc)
            {
                BoardLogic[1].setColor("-");
                BoardLogic[1].setPiece("-");
                BoardLogic[1].setActivite(CaseActivite.Actif.None);

                BoardLogic[3].setColor("B");
                BoardLogic[3].setPiece("R");
                BoardLogic[3].setActivite(CaseActivite.Actif.CanPlay);

                BoardLogic[4].setColor("B");
                BoardLogic[4].setPiece("T");
                BoardLogic[4].setActivite(CaseActivite.Actif.CanPlay);

                BoardLogic[5].setColor("-");
                BoardLogic[5].setPiece("-");
                BoardLogic[5].setActivite(CaseActivite.Actif.None);
            }
            else if (roque == K.PRoque && color == K.Noir)
            {
                BoardLogic[61].setColor("-");
                BoardLogic[61].setPiece("-");
                BoardLogic[61].setActivite(CaseActivite.Actif.None);

                BoardLogic[62].setColor("N");
                BoardLogic[62].setPiece("T");
                BoardLogic[62].setActivite(CaseActivite.Actif.CanPlay);

                BoardLogic[63].setColor("N");
                BoardLogic[63].setPiece("R");
                BoardLogic[63].setActivite(CaseActivite.Actif.CanPlay);

                BoardLogic[64].setColor("-");
                BoardLogic[64].setPiece("-");
                BoardLogic[64].setActivite(CaseActivite.Actif.None);
            }
            else if (roque == K.GRoque && color == K.Noir)
            {
                BoardLogic[57].setColor("-");
                BoardLogic[57].setPiece("-");
                BoardLogic[57].setActivite(CaseActivite.Actif.None);

                BoardLogic[59].setColor("N");
                BoardLogic[59].setPiece("R");
                BoardLogic[59].setActivite(CaseActivite.Actif.CanPlay);

                BoardLogic[60].setColor("N");
                BoardLogic[60].setPiece("T");
                BoardLogic[60].setActivite(CaseActivite.Actif.CanPlay);

                BoardLogic[61].setColor("-");
                BoardLogic[61].setPiece("-");
                BoardLogic[61].setActivite(CaseActivite.Actif.None);

            }
            else
                isRoque = false;

            return (isRoque);
        }
        static private void Pieces_Activity(byte CaseDep, byte CaseArr, bool isPriseEnPassant = false)
        {
            CaseActivite[] BoardLogic = Board.getCasesAvailable();

            BoardLogic[CaseArr].setColor(BoardLogic[CaseDep].getColor());
            BoardLogic[CaseArr].setPiece(BoardLogic[CaseDep].getPiece());
            BoardLogic[CaseArr].setActivite(CaseActivite.Actif.CanPlay);

            BoardLogic[CaseDep].setColor("-");
            BoardLogic[CaseDep].setPiece("-");
            BoardLogic[CaseDep].setActivite(CaseActivite.Actif.None);   
        }

        static private void EnablePieces_Neighbour_Activity(byte CaseDep)
        {
            CaseActivite[] BoardLogic = Board.getCasesAvailable();

            if (CaseDep == 2 && BoardLogic[1].getPiece() == "T") BoardLogic[1].setActivite(CaseActivite.Actif.CanPlay);
            else if (CaseDep == 3 && BoardLogic[4].getPiece() == "D") BoardLogic[4].setActivite(CaseActivite.Actif.CanPlay);
            else if (CaseDep == 4 && BoardLogic[5].getPiece() == "R") BoardLogic[5].setActivite(CaseActivite.Actif.CanPlay);
            else if (CaseDep == 5 && BoardLogic[4].getPiece() == "D") BoardLogic[4].setActivite(CaseActivite.Actif.CanPlay);
            else if (CaseDep == 6 && BoardLogic[5].getPiece() == "R") BoardLogic[5].setActivite(CaseActivite.Actif.CanPlay);
            else if (CaseDep == 7 && BoardLogic[8].getPiece() == "T") BoardLogic[8].setActivite(CaseActivite.Actif.CanPlay);
            else if (CaseDep == 58 && BoardLogic[57].getPiece() == "T") BoardLogic[57].setActivite(CaseActivite.Actif.CanPlay);
            else if (CaseDep == 59 && BoardLogic[60].getPiece() == "D") BoardLogic[60].setActivite(CaseActivite.Actif.CanPlay);
            else if (CaseDep == 60 && BoardLogic[61].getPiece() == "R") BoardLogic[61].setActivite(CaseActivite.Actif.CanPlay);
            else if (CaseDep == 61 && BoardLogic[60].getPiece() == "D") BoardLogic[60].setActivite(CaseActivite.Actif.CanPlay);
            else if (CaseDep == 62 && BoardLogic[61].getPiece() == "R") BoardLogic[61].setActivite(CaseActivite.Actif.CanPlay);
            else if (CaseDep == 63 && BoardLogic[64].getPiece() == "T") BoardLogic[64].setActivite(CaseActivite.Actif.CanPlay);
        }

        static public bool Pawn_Activity(byte source, byte destination, byte color, bool isPriseEnPassant)
        {
            bool isPawn = true;
            CaseActivite[] BoardLogic = Board.getCasesAvailable();

            byte positionDestPawn = destination;

            if (BoardLogic[source].getPiece() == "P")
            {
                if (isPriseEnPassant)
                {
                    if (color == K.Blanc)
                    {
                        // c6 = pion b
                        BoardLogic[destination + 8].setColor(BoardLogic[source].getColor());
                        BoardLogic[destination + 8].setPiece(BoardLogic[source].getPiece());
                        BoardLogic[destination + 8].setActivite(CaseActivite.Actif.CanPlay);

                        positionDestPawn = (byte)(destination + 8);
                    }
                    else
                    {
                        BoardLogic[destination - 8].setColor(BoardLogic[source].getColor());
                        BoardLogic[destination - 8].setPiece(BoardLogic[source].getPiece());
                        BoardLogic[destination - 8].setActivite(CaseActivite.Actif.CanPlay);

                        positionDestPawn = (byte)(destination - 8);
                    }

                    BoardLogic[destination].setColor("-");
                    BoardLogic[destination].setPiece("-");
                    BoardLogic[destination].setActivite(CaseActivite.Actif.None);
                }
                else
                {
                    // derniere rangee

                    // Promotion Pion B
                    if (color == K.Blanc && destination > 56)
                    {
                        BoardLogic[destination].setColor("B");
                        BoardLogic[destination].setPiece("D");
                        BoardLogic[destination].setActivite(CaseActivite.Actif.CanPlay);
                    }
                    // Promotion Pion N
                    else if (color == K.Noir && destination < 9)
                    {
                        BoardLogic[destination].setColor("N");
                        BoardLogic[destination].setPiece("D");
                        BoardLogic[destination].setActivite(CaseActivite.Actif.CanPlay);
                    }
                    // autre rangee
                    else
                    {
                        BoardLogic[destination].setColor(BoardLogic[source].getColor());
                        BoardLogic[destination].setPiece(BoardLogic[source].getPiece());
                        BoardLogic[destination].setActivite(CaseActivite.Actif.CanPlay);
                    }
                }

                BoardLogic[source].setColor("-");
                BoardLogic[source].setPiece("-");
                BoardLogic[source].setActivite(CaseActivite.Actif.None);

                EnablePiecesActivesPawn(BoardLogic, source);
                DisablePawnPlayed(BoardLogic, positionDestPawn);
            }
            else
                isPawn = false;

            return (isPawn);
        }

        static private void EnablePiecesActivesPawn(CaseActivite[] CasesActivite, byte nocasePlayed)
        {

            // Enable pieces derriere le pion
            // Blancs

            // Pion a
            // Active la Tour
            if (nocasePlayed == 9)
                CasesActivite[1].setActivite(CaseActivite.Actif.CanPlay);
            // Pion b
            // Active le Fou
            else if (nocasePlayed == 10)
                CasesActivite[3].setActivite(CaseActivite.Actif.CanPlay);
            // Pion c
            // Active la Dame
            else if (nocasePlayed == 11)
                CasesActivite[4].setActivite(CaseActivite.Actif.CanPlay);
            // Pion d
            // Active le Fou,la Dame,le Roi
            else if (nocasePlayed == 12)
            {
                CasesActivite[3].setActivite(CaseActivite.Actif.CanPlay);
                CasesActivite[4].setActivite(CaseActivite.Actif.CanPlay);
                CasesActivite[5].setActivite(CaseActivite.Actif.CanPlay);
            }
            // Pion e
            // Active le Fou,la Dame,le Roi (le Cav est deja actif)
            else if (nocasePlayed == 13)
            {
                CasesActivite[4].setActivite(CaseActivite.Actif.CanPlay);
                CasesActivite[5].setActivite(CaseActivite.Actif.CanPlay);
                CasesActivite[6].setActivite(CaseActivite.Actif.CanPlay);
            }
            // Pion f 
            // Active le Roi
            else if (nocasePlayed == 14)
                CasesActivite[5].setActivite(CaseActivite.Actif.CanPlay);
            // Pion g
            // Active le Fou
            else if (nocasePlayed == 15)
                CasesActivite[6].setActivite(CaseActivite.Actif.CanPlay);
            // Pion h
            // Active la Tour
            else if (nocasePlayed == 16)
                CasesActivite[8].setActivite(CaseActivite.Actif.CanPlay);

            // Noir
            // Active la Tour
            if (nocasePlayed == 49)
                CasesActivite[57].setActivite(CaseActivite.Actif.CanPlay);

            // Actice le Fou
            else if (nocasePlayed == 50)
                CasesActivite[59].setActivite(CaseActivite.Actif.CanPlay);

            // Active la Dame
            else if (nocasePlayed == 51)
                CasesActivite[60].setActivite(CaseActivite.Actif.CanPlay);

            // Active le Fou, la Dame, le Roi
            else if (nocasePlayed == 52)
            {
                CasesActivite[59].setActivite(CaseActivite.Actif.CanPlay);
                CasesActivite[60].setActivite(CaseActivite.Actif.CanPlay);
                CasesActivite[61].setActivite(CaseActivite.Actif.CanPlay);
            }
            // Pion e
            // Active le Fou, la Dame, le Roi
            else if (nocasePlayed == 53)
            {
                CasesActivite[60].setActivite(CaseActivite.Actif.CanPlay);
                CasesActivite[61].setActivite(CaseActivite.Actif.CanPlay);
                CasesActivite[62].setActivite(CaseActivite.Actif.CanPlay);
            }
            // Pion f
            // Active le Roi
            else if (nocasePlayed == 54)
                CasesActivite[61].setActivite(CaseActivite.Actif.CanPlay);

            // Pion g
            // Active le Fou
            else if (nocasePlayed == 55)
                CasesActivite[62].setActivite(CaseActivite.Actif.CanPlay);

            // Pion h
            // Active la Tour
            else if (nocasePlayed == 56)
                CasesActivite[64].setActivite(CaseActivite.Actif.CanPlay);

        }

        static private void DisablePawnPlayed(CaseActivite[] CasesActivite, byte nocasePlayed)
        {
            Boolean isDisable = false;

            // Disable le pion qui vient d'etre joué si bloqué

            if (CasesActivite[nocasePlayed].getColor() == "B")
            {
                if (nocasePlayed < 57)
                {
                    // Devant le pion
                    if (ScanBoard.PB[nocasePlayed, nocasePlayed + 8] == 1)
                        isDisable = CasesActivite[nocasePlayed + 8].isPiece();

                    // gauche
                    if (ScanBoard.PB[nocasePlayed, nocasePlayed + 7] == 1)
                        isDisable = isDisable && (CasesActivite[nocasePlayed + 7].getColor() != "N");

                    // droite
                    if (ScanBoard.PB[nocasePlayed, nocasePlayed + 9] == 1)
                        isDisable = isDisable && (CasesActivite[nocasePlayed + 9].getColor() != "N");

                    if (isDisable)
                        CasesActivite[nocasePlayed].setActivite(CaseActivite.Actif.CannotPlay);
                }

            }

            if (CasesActivite[nocasePlayed].getColor() == "N")
            {
                if (nocasePlayed > 8)
                {
                    // Devant le pion 
                    if (ScanBoard.PB[nocasePlayed, nocasePlayed - 8] == 1)
                        isDisable = CasesActivite[nocasePlayed - 8].isPiece();

                    // gauche
                    if (ScanBoard.PB[nocasePlayed, nocasePlayed - 7] == 1)
                        isDisable = isDisable && (CasesActivite[nocasePlayed - 7].getColor() != "B");

                    // droite
                    if (ScanBoard.PB[nocasePlayed, nocasePlayed - 9] == 1)
                        isDisable = isDisable && (CasesActivite[nocasePlayed - 9].getColor() != "B");

                    if (isDisable)
                        CasesActivite[nocasePlayed].setActivite(CaseActivite.Actif.CannotPlay);
                }

            }
        }

        static private byte Optimize_Move_Human(byte noCaseDepart, byte noCaseArrivee, byte colorMove)
        {
            Boolean isPriseEnPassant = false;
            Boolean Roque = false;
            byte newCaseArrivee = noCaseArrivee;
            CaseActivite[] CasesActivite = Board.getCasesAvailable();

            if (!LegalMove(CasesActivite, noCaseDepart, noCaseArrivee, colorMove))
            {
                Log.LogText("*** ERR *** OptimizeMyMove ... noCaseDepart = " + noCaseDepart + " noCaseArrivee= " + noCaseArrivee);
                return (0);
            }

            // Regles spéciales pour le pion et le Roi

            if (CasesActivite[noCaseDepart].getPiece() == "P")
                Pawn_Activity(noCaseDepart, noCaseArrivee, colorMove,isPriseEnPassant);

            // 0-0-0 Blanc
            else if (CasesActivite[noCaseDepart].getPiece() == "R" && noCaseDepart == 5 && noCaseArrivee == 3)
            {
                Roque = BigCastle(CasesActivite, noCaseDepart);
                if (Roque)
                {
                    newCaseArrivee = K.GRoque;
                }
            }

            // 0-0-0 Noir
            else if (CasesActivite[noCaseDepart].getPiece() == "R" && noCaseDepart == 61 && noCaseArrivee == 59)
            {
                Roque = BigCastle(CasesActivite, noCaseDepart);
                if (Roque)
                {
                    newCaseArrivee = K.GRoque;
                }
            }

            // 0-0 Blanc
            else if (CasesActivite[noCaseDepart].getPiece() == "R" && noCaseDepart == 5 && noCaseArrivee == 7)
            {
                Roque = SmallCastle(CasesActivite, noCaseDepart);
                if (Roque)
                {
                    newCaseArrivee = K.PRoque;
                }
            }
            // 0-0  Noir
            else if (CasesActivite[noCaseDepart].getPiece() == "R" && noCaseDepart == 61 && noCaseArrivee == 63)
            {
                Roque = SmallCastle(CasesActivite, noCaseDepart);
                if (Roque)
                {
                    newCaseArrivee = K.PRoque;
                }
            }

            else EnablePieces_Neighbour_Activity(noCaseDepart);

            // Prise en passant exclut
            if (!isPriseEnPassant && !Roque)
            {
                // Promotion Pion B
                if (noCaseArrivee > 56 && noCaseDepart > 48 && CasesActivite[noCaseDepart].getPiece() == "P" && CasesActivite[noCaseDepart].getColor() == "B")
                {
                    CasesActivite[noCaseArrivee].setColor("B");
                    CasesActivite[noCaseArrivee].setPiece("D");
                    CasesActivite[noCaseArrivee].setActivite(CaseActivite.Actif.CanPlay);
                }
                // Promotion Pion N
                else if (noCaseArrivee < 9 && noCaseDepart < 17 && CasesActivite[noCaseDepart].getPiece() == "P" && CasesActivite[noCaseDepart].getColor() == "N")
                {
                    CasesActivite[noCaseArrivee].setColor("N");
                    CasesActivite[noCaseArrivee].setPiece("D");
                    CasesActivite[noCaseArrivee].setActivite(CaseActivite.Actif.CanPlay);
                }

                // Coup normal pour toutes les pieces
                else
                {
                    CasesActivite[noCaseArrivee].setColor(CasesActivite[noCaseDepart].getColor());
                    CasesActivite[noCaseArrivee].setPiece(CasesActivite[noCaseDepart].getPiece());
                    CasesActivite[noCaseArrivee].setActivite(CaseActivite.Actif.CanPlay);
                }
            }

            // Supprime la piece de la case depart
            CasesActivite[noCaseDepart].setColor("-");
            CasesActivite[noCaseDepart].setPiece("-");
            CasesActivite[noCaseDepart].setActivite(CaseActivite.Actif.None);


            return (newCaseArrivee);
        }

        static public String[] TranslateMoveInName(byte noCaseDepart, byte noCaseFin, byte roque, CaseActivite[] cloneActivite)
        {
            byte color = cloneActivite[noCaseDepart].getColor() == "B" ? K.Blanc : K.Noir; 

            String[] name = new String[1];

            name[0] = "";
            int l = name[0].Length;

            if (roque == K.PRoque)
            {
                name = new String[5];

                name = new[] { "0-0" };
            }
            else if (roque == K.GRoque)
            {
                name = new String[6];

                name = new[] { "0-0-0" };

            }

            else if (cloneActivite[noCaseDepart].getPiece() == "T" || cloneActivite[noCaseDepart].getPiece() == "C")
            {
                String[] nameTest = new String[5]; // 5 differents facons d'écrire le coup

                nameTest[0] = cloneActivite[noCaseDepart].getPiece(); // T
                nameTest[1] = cloneActivite[noCaseDepart].getPiece() + Board.getNameCase(noCaseDepart).Substring(0, 1); // Ta
                nameTest[2] = cloneActivite[noCaseDepart].getPiece() + Board.getNameCase(noCaseDepart).Substring(1, 1); // T1
                nameTest[3] = cloneActivite[noCaseDepart].getPiece(); // Ta
                nameTest[4] = nameTest[1];

                if (cloneActivite[noCaseFin].getPiece() != "-")
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
                    if (cloneActivite[i].getPiece() == cloneActivite[noCaseFin].getPiece() &&
                        cloneActivite[i].getColor() == cloneActivite[noCaseFin].getColor() &&
                        i != noCaseDepart)
                    {
                        if (LegalMove(cloneActivite, i, noCaseFin, color))
                        {
                            if (cloneActivite[noCaseFin].getPiece() != "-")
                                name[0] = nameTest[1];
                            else
                                name[0] = nameTest[4];
                        }
                        else
                        {
                            if (cloneActivite[noCaseFin].getPiece() != "-")
                                name[0] = nameTest[3];
                            else
                                name[0] = nameTest[0];
                        }

                    }
                    else if (name[0].Length == 0)
                    {
                        if (cloneActivite[noCaseFin].getPiece() != "-")
                            name[0] = nameTest[3];
                        else
                            name[0] = nameTest[0];
                    }
                }
            }

           // Fxd4,Df3, etc...
            else if (cloneActivite[noCaseDepart].getPiece() == "F" || cloneActivite[noCaseDepart].getPiece() == "D")
            {
                name[0] = cloneActivite[noCaseDepart].getPiece();

                if (cloneActivite[noCaseFin].getPiece() != "-")
                    name[0] += "x";

                name[0] += Board.getNameCase(noCaseFin);
            }

            else if (cloneActivite[noCaseDepart].getPiece() == "R")
            {
                name[0] = cloneActivite[noCaseDepart].getPiece();

                if (cloneActivite[noCaseFin].getPiece() != "-")
                    name[0] += "x";

                name[0] += Board.getNameCase(noCaseFin);
            }

            else if (cloneActivite[noCaseDepart].getPiece() == "P")
            {
                if (cloneActivite[noCaseFin].getPiece() != "-")
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

        static public void StopGame(byte WhoPlayWhite,byte color_move, byte noMove)
        {
            Log.LogCoups("** ERR ***", color_move, noMove, WhoPlayWhite);
/*
            Console.Beep();
            Console.Beep();
            Console.Beep();
*/        }
/*
        static public bool Start_WebService_Cases()
        {
            Boolean isStarted = true;
            SquareTimeProcessingService.SquareTimeProcessingServiceClient client = new SquareTimeProcessingService.SquareTimeProcessingServiceClient("BasicHttpBinding_ISquareTimeProcessingService");
            String pathBoardFile = ConfigurationManager.AppSettings["PathFileBoard"].ToString().Trim();

            try
            {
                if (client.Start(@pathBoardFile))
                {
                    Log.LogText(" " + Environment.NewLine + "Web Service Started !!");
                }
                else
                    Log.LogText(" " + Environment.NewLine + "*** Web Service Errosr *** ");
            }
            catch
            {
                Log.LogText(" " + Environment.NewLine + "*** Web Service Error *** ");
                isStarted = false;
            }

            return (isStarted);
        }
*/
        static public bool Start_WebService_WP()
        {
            Boolean isStarted = true;

            WhitePiecesService.WhitePiecesClient client = new WhitePiecesService.WhitePiecesClient("BasicHttpBinding_IWhitePieces");

            String pathBoardFile = ConfigurationManager.AppSettings["PathFileBoard"].ToString().Trim();

            try
            {
                if (client.Running(@pathBoardFile))
                    Log.LogText(" " + Environment.NewLine + "Web Service Started !!");
                else
                    Log.LogText(" " + Environment.NewLine + "*** Web Service Error *** ");
            }
            catch
            {
                Log.LogText(" " + Environment.NewLine + "*** Web Service Error *** ");
                isStarted = false;
            }

            return (isStarted);
        }


        static public byte FindPawnCaseDestination(byte CaseDepart, List<byte> CasesDest)
        {
            byte CaseArrivee = 0;

            CaseActivite[] BoardLogic = Board.getCasesAvailable();
         
            if (CasesDest.Count == 2)
            {
                if (BoardLogic[CaseDepart].getPiece() == "P")
                {
                    if (CaseDepart + 8 == CasesDest[0] && CaseDepart + 16 == CasesDest[1])
                    {
                        CaseArrivee = CasesDest[1];
                    }
                    else if (CaseDepart + 8 == CasesDest[1] && CaseDepart + 16 == CasesDest[0])
                    {
                        CaseArrivee = CasesDest[0];
                    }
                    else if (CaseDepart - 8 == CasesDest[0] && CaseDepart - 16 == CasesDest[1])
                    {
                        CaseArrivee = CasesDest[1];
                    }
                    else if (CaseDepart - 8 == CasesDest[1] && CaseDepart - 16 == CasesDest[0])
                    {
                        CaseArrivee = CasesDest[0];
                    }                   
                }
            }

            return (CaseArrivee);
        }

        /*
         * Retourne la case qui fut refraichit la première fois entre caseA et la caseB.
         * Peut avoir jusqu'a 3 coups en traitement
         * */
        static public short findSmallerTimeWS(List<byte> Cases, out byte response)
        {
            short FindMove = 1;
            byte ChessBoardColorDown = 0;
            byte caseX = 0;
            byte caseOther = 0;
//            byte 6rehj2responseWhitePiece = 0;
            DateTime SmallerTime = default(DateTime);

            response = 0;

            // Position des couleurs des pieces sur Echiquier : Les Blancs sont en bas ou en haut
            ChessBoardColorDown = Board.getColorBoard();

            try
            {
                for (byte i = 0; i < Cases.Count; i++)
                {
                    if (i == 0)
                        FindMove = findSmallerSquaresTime(ChessBoardColorDown, Cases[0], Cases[1], out caseX, out SmallerTime);
                    else if (i == 1)
                        continue;
                    else
                        FindMove = findSmallerSquaresTime(ChessBoardColorDown, caseOther, Cases[i], out caseX, out SmallerTime);

                    caseOther = caseX;                    
                }

                response = caseX;
#if USE_WS
                if (findSmallerWPiecesTime(Cases,SmallerTime, out responseWhitePiece))
                    response = responseWhitePiece;
#endif
            }
            catch (Exception)
            {
                Log.LogText("Le Web Service n'est pas démarré !! ");
            }


            return (FindMove);
        }

        static private bool findSmallerWPiecesTime(List<byte> Cases,DateTime SmallerTime, out byte response)
        {
            WhitePiecesService.WhitePiecesClient client_WPiece = new WhitePiecesService.WhitePiecesClient("BasicHttpBinding_IWhitePieces");
            DateTime dt;
            bool isSmallerFound = false;

            response = 0;

            for (byte i = 0; i < Cases.Count; i++)
            {
                dt = client_WPiece.GetFirstHit(Cases[i]);

                if (dt != default(DateTime) && DateTime.Compare(dt, SmallerTime) < 0)
                {
                    response = Cases[i];
                    isSmallerFound = true;
                }

                Log.LogText("WP Web Service Value " + " (" + Cases[i] + ")\t =" + "\t\t\t" + dt.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));

            }

            return (isSmallerFound);
        }

        static short findSmallerSquaresTime(byte ChessBoardColorDown, byte caseA, byte caseB, out byte response, out DateTime dtResponse)
        {
            short FindMove = 1;

            DateTime WS_FirstDTHitCaseA;
            DateTime WS_FirstDTHitCaseB;

            int WS_ValueCaseA = 0;
            int WS_ValueCaseB = 0;
            
            int ResultCmp = 0;

            response = caseA;
            dtResponse = default(DateTime);

            SquareTimeProcessingService.SquareTimeProcessingServiceClient client = new SquareTimeProcessingService.SquareTimeProcessingServiceClient("BasicHttpBinding_ISquareTimeProcessingService");

            try
            {
                // Si le numero de caseA = 200 et caseB = 100 alors retourne True donc c'est la case B qui fut jouée la première
                WS_ValueCaseA = client.GetValue(ChessBoardColorDown == K.Blanc ? caseA : (byte)(65 - caseA));
                WS_ValueCaseB = client.GetValue(ChessBoardColorDown == K.Blanc ? caseB : (byte)(65 - caseB));

                WS_FirstDTHitCaseA = client.GetFirstHit(ChessBoardColorDown == K.Blanc ? caseA : (byte)(65 - caseA));
                WS_FirstDTHitCaseB = client.GetFirstHit(ChessBoardColorDown == K.Blanc ? caseB : (byte)(65 - caseB));

                Log.LogText("Web Service Value " + " (" + caseA + ")\t = \t" + WS_ValueCaseA + "\t\t" + WS_FirstDTHitCaseA.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));
                Log.LogText("Web Service Value " + " (" + caseB + ")\t = \t" + WS_ValueCaseB + "\t\t" + WS_FirstDTHitCaseB.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));

                if (WS_FirstDTHitCaseA != default(DateTime) && WS_FirstDTHitCaseB != default(DateTime))
                {
                    ResultCmp = DateTime.Compare(WS_FirstDTHitCaseA, WS_FirstDTHitCaseB);

                    // A < B
                    if (ResultCmp < 0)
                    {
                        response = caseA;
                        dtResponse = WS_FirstDTHitCaseA;
                    }
                    else
                    {
                        response = caseB;
                        dtResponse = WS_FirstDTHitCaseB;
                    }

// On valide avec une autre application pour s'assurer que le temps de reponse est vraiment le bon
                    response = STA_GetLowerTime(ChessBoardColorDown, caseA, caseB, out dtResponse);

                    if (response == 0)
                        response = caseA;
                }

                else if (WS_FirstDTHitCaseA == default(DateTime) && WS_FirstDTHitCaseB == default(DateTime))
                    FindMove = -1; // Aucun Hit
                else if (WS_FirstDTHitCaseA != default(DateTime))
                {
                    response = caseA;
                    dtResponse = WS_FirstDTHitCaseA;
                }
                else
                {
                    response = caseB;
                    dtResponse = WS_FirstDTHitCaseB;
                }
            }
            catch (Exception)
            {
                Log.LogText("Le Web Service n'est pas démarré !! ");
            }

            return (FindMove);
        }

        static byte STA_GetLowerTime(byte ChessBoardColorDown, byte caseA, byte caseB,out DateTime dtResponse)
        {
            Boolean bTimeOutA = false;
            Boolean bTimeOutB = false;
            byte response = 0;
            dtResponse = default(DateTime);

            DateTime SQA_FirstHitCaseA = default(DateTime);
            DateTime SQA_FirstHitCaseB = default(DateTime);

            int ResultCmp = 0;

            GlobalSMEXE.SendMsg_Call_SquareTime_EXE(GlobalParameters_SquareTime.ST_GETFIRSTHIT, ChessBoardColorDown == K.Blanc ? caseA : (byte)(65 - caseA));

            if (!STA_WaitingAnswer())
            {
                // No Answer received.
                bTimeOutA = true;
            }
            else
            {
                // Answer received

                // Case vide
                if (GlobalParameters_SquareTime.ST_noCase == caseA)
                {
                    SQA_FirstHitCaseA = DateTime.Parse(GlobalParameters_SquareTime.ST_time);
                    GlobalSMEXE.SendMsg_Call_SquareTime_EXE(GlobalParameters_SquareTime.ST_GETFIRSTHIT, ChessBoardColorDown == K.Blanc ? caseB : (byte)(65 - caseB));
                    if (!STA_WaitingAnswer())
                    {
                        if (GlobalParameters_SquareTime.ST_noCase == caseB)
                        {
                            SQA_FirstHitCaseB = DateTime.Parse(GlobalParameters_SquareTime.ST_time);

                            ResultCmp = DateTime.Compare(SQA_FirstHitCaseA, SQA_FirstHitCaseB);

                            // A < B
                            if (ResultCmp < 0)
                            {
                                response = caseA;
                                dtResponse = SQA_FirstHitCaseA;

                                // Remets a zero le timestamp de la case A de l'application SquareTime
                                GlobalSMEXE.SendMsg_Call_SquareTime_EXE(GlobalParameters_SquareTime.ST_SETEMPTYCASE, ChessBoardColorDown == K.Blanc ? caseA : (byte)(65 - caseA));
                            }
                            else
                            {
                                response = caseB;
                                dtResponse = SQA_FirstHitCaseB;

                                // Remets a zero le timestamp de la case B de l'application SquareTime
                                GlobalSMEXE.SendMsg_Call_SquareTime_EXE(GlobalParameters_SquareTime.ST_SETEMPTYCASE, ChessBoardColorDown == K.Blanc ? caseB : (byte)(65 - caseB));
                            }
                        }
                    }
                    else 
                        bTimeOutB = true;
                }
            }

            if (!bTimeOutA)
            {
                Log.LogText("SquareTime Application " + " (" + caseA + ")\t = \t" + "\t\t" + SQA_FirstHitCaseA.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));
                if (!bTimeOutB)
                    Log.LogText("SquareTime Application " + " (" + caseB + ")\t = \t" + "\t\t" + SQA_FirstHitCaseB.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));
                else
                    Log.LogText("SquareTime Application Time Out ! " + " (" + caseB + ")\t = \t" + "\t\t");
            }
            else
                Log.LogText("SquareTime Application Time Out ! " + " (" + caseA + ")\t = \t" + "\t\t");
            
            return (response);
        }

        static short DisplayLog_WP_Pieces(byte ChessBoardColorDown, byte caseA, byte caseB, out byte response)
        {
            short FindMove = 1;

            string FirstHitCaseA = string.Empty;
            string FirstHitCaseB = string.Empty;
            DateTime FirstDTHitCaseA;
            DateTime FirstDTHitCaseB;
            int ValueCaseA = 0;
            int ValueCaseB = 0;
//          int ResultCmp = 0;

            response = caseA;

            WhitePiecesService.WhitePiecesClient client = new WhitePiecesService.WhitePiecesClient("BasicHttpBinding_IWhitePieces");

            try
            {
                FirstDTHitCaseA = client.GetFirstHit(ChessBoardColorDown == K.Blanc ? caseA : (byte)(65 - caseA));
                FirstDTHitCaseB = client.GetFirstHit(ChessBoardColorDown == K.Blanc ? caseB : (byte)(65 - caseB));

                Log.LogText("Info WP Service Value " + " (" + caseA + ")\t = \t" + ValueCaseA + "\t\t" + FirstDTHitCaseA.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));
                Log.LogText("Info WP Service Value " + " (" + caseB + ")\t = \t" + ValueCaseB + "\t\t" + FirstDTHitCaseB.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));

/*
                if (FirstDTHitCaseA != default(DateTime) && FirstDTHitCaseB != default(DateTime))
                {
                    ResultCmp = DateTime.Compare(FirstDTHitCaseA, FirstDTHitCaseB);

                    // A < B
                    if (ResultCmp < 0)
                        response = caseA;
                    else
                        response = caseB;
                }

                else if (FirstDTHitCaseA == default(DateTime) && FirstDTHitCaseB == default(DateTime))
                    FindMove = -1; // Aucun Hit
                else if (FirstDTHitCaseA != default(DateTime))
                    response = caseA;
                else
                    response = caseB;
 */ 
            }
            catch (Exception)
            {
                Log.LogText("Le Web Service WP n'est pas démarré !! ");
            }

            return (FindMove);
        }

        static short DoTestSquare(byte noCaseA, byte noCaseB, out int ValA, out int ValB)
        {
            short err = -1;
            SquareTimeProcessingService.SquareTimeProcessingServiceClient client = new SquareTimeProcessingService.SquareTimeProcessingServiceClient("BasicHttpBinding_ISquareTimeProcessingService");

            ValA = 0;
            ValB = 0;

            for (byte nbTry = 1; nbTry <= 5 && err == -1; nbTry++)
            {
                Thread.Sleep(500);
                ValA = client.GetValue(noCaseA);
                ValB = client.GetValue(noCaseB);

                Log.LogText("Try Again...\t(" + ValA + "," + ValB + ")");

                if (ValA > 0 && ValB > 0)
                    err=0;
            }

            return (err);
        }

        /*
         * Cas typique le Fou en f8 vient d'etre joue en b4 un coup plus tard
         * fut le 0-0. La sequence typique est A(61) = 22  B(62) = 27 pour le Roi en e8 (61) 
         * et le Fou en f8 (62). Le coup du Fou semble plus recent que celui du Roi (roque)
         * L'explication vient du fait que le roque genere un refresh de la case 61,62,63 et 64.
         * On doit tenir compte de ce fait. Si le Fou ou le Cavalier a ete joue et que le roque
         * n'a pas encore ete fait alors il y a un risque que no de sequence soit inverse.
         * Si la case A est le Roi et la case B est le Fou ou le Roi alors on inverse le resultat
         * entre le Roi et la piece B
         **/

        static private short isSquareAlreadyCalled(byte noCaseA, byte noCaseB, int ValA, int ValB, out byte OldestCall)
        {
            short FindNoCase = -1;
            int valRook = 0;
            CaseActivite[] BoardLogic = Board.getCasesAvailable();
            SquareTimeProcessingService.SquareTimeProcessingServiceClient client = new SquareTimeProcessingService.SquareTimeProcessingServiceClient("BasicHttpBinding_ISquareTimeProcessingService");

            OldestCall = 0;

            // 0-0 Blanc
            if (noCaseA == 5 && (noCaseB == 6 || noCaseB == 7))
            {
                if (BoardLogic[noCaseA].getPiece() == "R" && BoardLogic[noCaseA].getColor() == "B")
                {
                    if (BoardLogic[6].getPiece() == "-")
                    {
                        if (BoardLogic[7].getPiece() == "-")
                        {
                            if (BoardLogic[8].getPiece() == "T" && BoardLogic[8].getColor() == "B")
                            {
                                if (ValA < ValB)
                                {
                                    valRook = client.GetValue(8);

                                    // Refresh de la case Tour s'est fait apres le Roi
                                    if (valRook > ValA)
                                        FindNoCase = 1;
                                }
                            }
                        }
                    }
                }
            }

            // 0-0-0 Blanc
            else if (noCaseA == 5 && (noCaseB == 4 || noCaseB == 3 || noCaseB == 2))
            {
                if (BoardLogic[noCaseA].getPiece() == "R" && BoardLogic[noCaseA].getColor() == "B")
                {
                    if (BoardLogic[4].getPiece() == "-")
                    {
                        if (BoardLogic[3].getPiece() == "-")
                        {
                            if (BoardLogic[2].getPiece() == "-")
                            {
                                if (BoardLogic[1].getPiece() == "T" && BoardLogic[1].getColor() == "B")
                                {
                                    if (ValA < ValB)
                                    {
                                        valRook = client.GetValue(1);

                                        if (valRook > ValA)
                                            FindNoCase = 1;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // 0-0 Noir
            if (noCaseA == 61 && (noCaseB == 62 || noCaseB == 63))
            {
                if (BoardLogic[noCaseA].getPiece() == "R" && BoardLogic[noCaseA].getColor() == "N")
                {
                    if (BoardLogic[62].getPiece() == "-")
                    {
                        if (BoardLogic[63].getPiece() == "-")
                        {
                            if (BoardLogic[64].getPiece() == "T" && BoardLogic[64].getColor() == "N")
                            {
                                if (ValA < ValB)
                                {
                                    valRook = client.GetValue(64);

                                    if (valRook > ValA)
                                        FindNoCase = 1;
                                }
                            }
                        }
                    }
                }
            }

            // 0-0-0 Noir
            else if (noCaseA == 61 && (noCaseB == 60 || noCaseB == 59 || noCaseB == 58))
            {
                if (BoardLogic[noCaseA].getPiece() == "R" && BoardLogic[noCaseA].getColor() == "N")
                {
                    if (BoardLogic[60].getPiece() == "-")
                    {
                        if (BoardLogic[59].getPiece() == "-")
                        {
                            if (BoardLogic[58].getPiece() == "-")
                            {
                                if (BoardLogic[57].getPiece() == "T" && BoardLogic[57].getColor() == "N")
                                {
                                    if (ValA < ValB)
                                    {
                                        valRook = client.GetValue(57);

                                        if (valRook > ValA)
                                            FindNoCase = 1;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (FindNoCase == 1)
                OldestCall = noCaseB;

            return (FindNoCase);
        }

        static public bool UpdateSquareWebService(byte Dep, byte Dest, byte roque, bool PriseEnPassant, byte color)
        {
            bool success = true;
            byte ChessBoardColorDown = Board.getColorBoard();

            SquareTimeProcessingService.SquareTimeProcessingServiceClient client = new SquareTimeProcessingService.SquareTimeProcessingServiceClient("BasicHttpBinding_ISquareTimeProcessingService");

            // Case Depart
            if (roque == K.PRoque)
            {
                if (ChessBoardColorDown == K.Blanc)
                {
                    if (color == K.Blanc)
                    {
                        client.SetFirstHit(ChessBoardColorDown == K.Blanc ? (byte) 5 : (byte) 60);
                        client.SetFirstHit(ChessBoardColorDown == K.Blanc ? (byte) 6 : (byte) 59);
                        client.SetFirstHit(ChessBoardColorDown == K.Blanc ? (byte) 7 : (byte) 58);
                        client.SetFirstHit(ChessBoardColorDown == K.Blanc ? (byte) 8 : (byte) 57);
                    }
                    else
                    {
                        client.SetFirstHit(ChessBoardColorDown == K.Blanc ? (byte) 61 : (byte) 4);
                        client.SetFirstHit(ChessBoardColorDown == K.Blanc ? (byte) 62 : (byte) 3);
                        client.SetFirstHit(ChessBoardColorDown == K.Blanc ? (byte) 63 : (byte) 2);
                        client.SetFirstHit(ChessBoardColorDown == K.Blanc ? (byte) 64 : (byte) 1);
                    }
                }
            }
            else if (roque == K.GRoque)
            {
                if (color == K.Blanc)
                {
                    client.SetFirstHit(ChessBoardColorDown == K.Blanc ? (byte) 1 : (byte) 64);
                    client.SetFirstHit(ChessBoardColorDown == K.Blanc ? (byte) 2 : (byte) 63);
                    client.SetFirstHit(ChessBoardColorDown == K.Blanc ? (byte) 3 : (byte) 62);
                    client.SetFirstHit(ChessBoardColorDown == K.Blanc ? (byte) 4 : (byte) 61);
                    client.SetFirstHit(ChessBoardColorDown == K.Blanc ? (byte) 5 : (byte) 60);
                }
                else
                {
                    client.SetFirstHit(ChessBoardColorDown == K.Blanc ? (byte) 57 : (byte) 8);
                    client.SetFirstHit(ChessBoardColorDown == K.Blanc ? (byte) 58 : (byte) 7);
                    client.SetFirstHit(ChessBoardColorDown == K.Blanc ? (byte) 59 : (byte) 6);
                    client.SetFirstHit(ChessBoardColorDown == K.Blanc ? (byte) 60 : (byte) 5);
                    client.SetFirstHit(ChessBoardColorDown == K.Blanc ? (byte) 61 : (byte) 4);
                }
            }
            else
                client.SetFirstHit(ChessBoardColorDown == K.Blanc ? Dep : ((byte) (65 - Dep)));

            // Case Arrivee 
            if (roque == K.AucunRoque)
            {
                if (PriseEnPassant)
                {
                    if (color == K.Blanc)
                        client.SetFirstHit(ChessBoardColorDown == K.Blanc ? (byte)(Dest + 8) : (byte)(65 - (Dest + 8)));
                    else
                        client.SetFirstHit(ChessBoardColorDown == K.Blanc ? (byte)(Dest - 8) : (byte)(65 - (Dest - 8)));
                }

                success = client.SetFirstHit(Dest);
            }

            return (success);
        }

        static bool STA_WaitingAnswer()
        {
            bool SquareTimeAppReceived = false;
            uint maxTime = 10000; // 10 secondes
            uint totTime = 0;

            while (!SquareTimeAppReceived && totTime < maxTime)
            {
                if (GlobalParameters_SquareTime.ST_noCase > 0)
                    SquareTimeAppReceived = true;
                else
                {
                    Thread.Sleep(250);
                    totTime += 250;
                }
            }

            return (SquareTimeAppReceived);
        }

    }

   

}
