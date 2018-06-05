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
using Tool;

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
            Boolean isResetingGame = false;
            Bitmap screenBmp = null;
            Bitmap screenBmpFromList = null;
            byte caseRoi = 0;
            byte caseTour = 0;
            short findMove = 0;
           
            roque = 0;

            CaseActivite[] BoardLogic = ToolBoard.getCasesAvailable();

            // Coup Depart
            for (byte i = 1; i <= 64 && !isResetingGame; i++)
            {
                if (!ToolBoard.ValidateMove(i, color))
                    continue;

                if (i == lastDep || i == lastDest)
                    continue;

                screenBmp = ToolBoard.TakePictureCase(i);
                screenBmpFromList = ToolBoard.getBitmap(i);

                if (screenBmpFromList == null)
                {
                    isResetingGame = true; // Est en train de redessiner l'echiquier (nouvelle partie)
                    Log.LogText("ResetGame" + i);
                }
                else if (!ToolBoard.CompareBitmaps((Image)screenBmp, (Image)screenBmpFromList))
                {
                    if (i == 1 || i == 8 || i == 57 || i == 64) caseTour = i;
                    else if (i == 5 || i == 61) caseRoi = i;
                    Departs.Add(i);
                    Log.LogText("Depart ---> " + i);
                }

            }

            if (isResetingGame)
                findMove = K.isResetingGame;

            else if (caseRoi > 0 && caseTour > 0)
            {
                roque = ToolBoard.ValidateRoque(caseRoi, caseTour);
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
            bool isResetingGame;

            PriseEnPassant = false;

            findDestMoves = FindMovesChanged(LastDep, LastDest, CaseDepart, color, out PriseEnPassant, out isResetingGame);

            if (isResetingGame)
                return (K.isResetingGame);

            if (findDestMoves.Count > 1)
                moveFoundDest = -1;
            else if (findDestMoves.Count == 1)
                moveFoundDest = 1;

            for (byte i = 0; i < findDestMoves.Count(); i ++)
                DestMoves.Add(findDestMoves[i]);

            return (moveFoundDest);
        }

        static public short GetDest_TC_WS(byte LastDep, byte LastDest, byte CaseDepart, byte color, List<byte> DestMoves, out bool PriseEnPassant)
        {
            List<byte> findDestMoves = null;
            short moveFoundDest = 0; // aucun coup trouve

            PriseEnPassant = false;

            findDestMoves = FindMoves_TC_WS(LastDep, LastDest, CaseDepart, color, out PriseEnPassant);

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
            bool isResetingGame;

            PriseEnPassant = false;

            Log.LogText("Test Again ...");
            Thread.Sleep(timer);

            findDestMoves = FindMovesChanged(LastDep, LastArr, CaseDepart, color, out PriseEnPassant, out isResetingGame);

            if (isResetingGame)
                return (K.isResetingGame);

            if (findDestMoves.Count == 1)
                FindMoveDest = 1;
            else if (findDestMoves.Count == 2)
            {                
                CaseArrivee = ToolBoard.FindPawnCaseDestination(CaseDepart, findDestMoves);
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

        static private List<byte> FindMovesChanged(byte LastDep, byte LastArr, byte CaseDepart, byte color, out bool isPriseEnPassant, out bool isResetingGame)
        {
            List<byte> findNewMoves = new List<byte>();
            Bitmap colorCaseArr = null;
            Bitmap screenBmp = null;
            Bitmap screenBmpFromList = null;
            Boolean isFind = false;
            Boolean test = false;
            
            isResetingGame = false;

            CaseActivite[] CasesActivite = ToolBoard.getCasesAvailable();
            isPriseEnPassant = false;

            for (byte i = 1; i <= 64 && !isResetingGame; i++)
            {
                if (i == LastDep || i == LastArr)
                    continue;

                if (!ToolBoard.LegalMove(CasesActivite, CaseDepart, i, color))
                    continue;

                if (test)
                    Log.LogText("Legal Depart = " + CaseDepart + "\t Arrivee = " + i);

//                if (CaseDepart > 45)
//                    ToolBoard.PhotoBoard(i);

                screenBmp = ToolBoard.TakePictureCase(i);
                screenBmpFromList = ToolBoard.getBitmap(i);

                if (screenBmpFromList == null)
                    isResetingGame = true;

                else if (!ToolBoard.CompareBitmaps(screenBmp, screenBmpFromList))
                {
                    colorCaseArr = ToolBoard.TakePictureCaseColor(i);

                    if (i == 21)
                    {
                        colorCaseArr.Save(@"C:\Chess\Case_21.bmp", System.Drawing.Imaging.ImageFormat.MemoryBmp);
                    }
                    if (i == 29)
                    {
                        colorCaseArr.Save(@"C:\Chess\Case_29.bmp", System.Drawing.Imaging.ImageFormat.MemoryBmp);
                    }

                    Log.LogText("Arrivee ---> " + i);
                    isFind = true;

                    if (!ToolBoard.CompareBitmaps((Image)colorCaseArr, (Image)ToolBoard.getPairCaseEmpty()) && !ToolBoard.CompareBitmaps((Image)colorCaseArr, (Image)ToolBoard.getImPairCaseEmpty()))
                        findNewMoves.Add(i);
                    else if (ToolBoard.PriseEnPassant(CaseDepart, i))
                    {
                        findNewMoves.Add(i);
                        isPriseEnPassant = true;
                    }
                }
            }

            if (findNewMoves.Count == 0 && isFind == true)
            {
                Log.LogText("Arrivee Case Vide (Err...)");
            }
            return (findNewMoves);
        }

        static private List<byte> FindMoves_TC_WS(byte LastDep, byte LastArr, byte CaseDepart, byte color, out bool isPriseEnPassant)
        {
            List<byte> findNewMoves = new List<byte>();

            SquareTimeProcessingService.SquareTimeProcessingServiceClient Client_TC = new SquareTimeProcessingService.SquareTimeProcessingServiceClient("BasicHttpBinding_ISquareTimeProcessingService");

            CaseActivite[] CasesActivite = ToolBoard.getCasesAvailable();
            isPriseEnPassant = false;

            for (byte i = 1; i <= 64; i++)
            {
                if (i == LastDep || i == LastArr)
                    continue;

                if (!ToolBoard.LegalMove(CasesActivite, CaseDepart, i, color))
                    continue;

                if (Client_TC.GetFirstHit(i) != default(DateTime))
                {
                    Log.LogText("Square Time Service Arrivee ---> " + i);

                    if (ToolBoard.PriseEnPassant(CaseDepart, i))
                        isPriseEnPassant = true;

                    findNewMoves.Add(i);
                }
            }

            return (findNewMoves);
        }

        static public void Set_Activities_Move(byte CaseDepart, byte CaseDest,byte color,bool PriseEnPassant)
        {
            // Roque
            if (!ToolBoard.Roque_Activity(CaseDepart, color))
            {
                // Pion
                if (!ToolBoard.Pawn_Activity(CaseDepart, CaseDest, color, PriseEnPassant))
                {
                    // Toutes les pieces sauf le Pion et le roque
                    ToolBoard.Pieces_Activity(CaseDepart, CaseDest);
                }
            }

            // Active les cases du voisinage
            ToolBoard.EnablePieces_Neighbour_Activity(CaseDepart);                      
        }

        static public short WhichCaseSelected(byte LastDep, byte LastDest, byte CaseDepart, byte color, out byte CaseDest, out bool PriseEnPassant)
        {
            short findMoveDest = 0;            
            Bitmap colorDepart = null;
            CaseActivite[] BoardLogic = ToolBoard.getCasesAvailable();
            PriseEnPassant = false;

            CaseDest = 0;

            if (LastDep == 0)
                return (0);

            CaseDest = ToolBoard.PossibleMovesSelectedCases(CaseDepart, LastDep, LastDest, color);
            
            Log.LogText("Adversaire... CaseSelectionnee CaseDest :" + CaseDest);

            // Deux cases possibles (coup precedent m_LastMoveSrc et m_LastMoveDest)
            if (CaseDest == K.CSelected)
            {
                colorDepart = ToolBoard.TakePictureCaseColor(LastDep);

                // Si la case de depart est vide alors le seul coup possible est la case d'arrivee  
                if (ToolBoard.CompareBitmaps((Image)colorDepart, ToolBoard.getPairCaseEmpty()) ||
                    ToolBoard.CompareBitmaps((Image)colorDepart, ToolBoard.getImPairCaseEmpty()))
                {
                    if (ToolBoard.PriseEnPassant(CaseDepart, LastDest))
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

 
        static public void StopGame(byte WhoPlayWhite,byte color_move, byte noMove)
        {
            Log.LogCoups("** ERR ***", color_move, noMove, WhoPlayWhite);
        }

        static public byte FindOlderMove_TC_WS(List<byte> Cases, out DateTime SmallerTime_TC)
        {
            SquareTimeProcessingService.SquareTimeProcessingServiceClient Client_TC = new SquareTimeProcessingService.SquareTimeProcessingServiceClient("BasicHttpBinding_ISquareTimeProcessingService");
            DateTime dt;
            byte Case_TC = 0;

            SmallerTime_TC = DateTime.Today.AddYears(1);

            byte ColorBelow  = ToolBoard.getColorBoard();

            try
            {
                for (byte i = 0; i < Cases.Count; i++)
                {
                    if (ColorBelow == K.Blanc)
                        dt = Client_TC.GetFirstHit(Cases[i]);
                    else
                        dt = Client_TC.GetFirstHit((byte)(65 - Cases[i]));

                    if (DateTime.Compare(dt, SmallerTime_TC) < 0)
                    {
                        Case_TC = Cases[i];
                        SmallerTime_TC = dt;
                    }

                    Log.LogText("TC Web Service " + "\t (" + Cases[i] + ")" + "\t\t" + dt.ToString("yyyy/MM/dd hh:mm:ss.fff tt"));
                }

            }
            catch (Exception)
            {
                Log.LogText("Le Web Service n'est pas démarré !! ");
            }


            return (Case_TC);
        }

        static public byte FindOlderMove_PB_WS(List<byte> Cases, out DateTime SmallerTime_PB)
        {
            PBlanches.WhitePiecesClient Client_PB = new PBlanches.WhitePiecesClient("BasicHttpBinding_IWhitePieces");           
            DateTime dt;
            byte Case_PB = 0;

            byte ColorBelow = ToolBoard.getColorBoard();

            SmallerTime_PB = DateTime.Today.AddYears(1); 
            
            for (byte i = 0; i < Cases.Count; i++)
            {
                if (ColorBelow == K.Blanc)
                    dt = Client_PB.GetFirstHit(Cases[i]);
                else
                    dt = Client_PB.GetFirstHit((byte)(65 - Cases[i]));

                if (DateTime.Compare(dt, SmallerTime_PB) < 0)
                {
                    Case_PB = Cases[i];
                    SmallerTime_PB = dt;
                }

                Log.LogText("PB Web Service " + " (" + Cases[i] + ")\t =" + "\t\t\t" + dt.ToString("yyyy/MM/dd hh:mm:ss.fff tt"));

            }

            if (Case_PB == 0) 
            {
                SmallerTime_PB = DateTime.Today.AddYears(1);
            }
            return (Case_PB);
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
            int ResultCmp = 0;

            response = caseA;

            PBlanches.WhitePiecesClient Client_PB = new PBlanches.WhitePiecesClient("BasicHttpBinding_IWhitePieces"); 

            try
            {
                FirstDTHitCaseA = Client_PB.GetFirstHit(ChessBoardColorDown == K.Blanc ? caseA : (byte)(65 - caseA));
                FirstDTHitCaseB = Client_PB.GetFirstHit(ChessBoardColorDown == K.Blanc ? caseB : (byte)(65 - caseB));

                Log.LogText("Info WP Service Value " + " (" + caseA + ")\t = \t" + ValueCaseA + "\t\t" + FirstDTHitCaseA.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));
                Log.LogText("Info WP Service Value " + " (" + caseB + ")\t = \t" + ValueCaseB + "\t\t" + FirstDTHitCaseB.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));


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
  
            }
            catch (Exception)
            {
                Log.LogText("Le Web Service WP n'est pas démarré !! ");
            }

            return (FindMove);
        }

        static public bool MovePiece_TC_WS(byte Dep, byte Dest, byte roque, bool PriseEnPassant, byte color)
        {
            bool success = true;
            byte ChessBoardColorDown = ToolBoard.getColorBoard();

            SquareTimeProcessingService.SquareTimeProcessingServiceClient Client_TC = new SquareTimeProcessingService.SquareTimeProcessingServiceClient("BasicHttpBinding_ISquareTimeProcessingService");

            // Case Depart
            if (roque == K.PRoque)
            {
                if (ChessBoardColorDown == K.Blanc)
                {
                    if (color == K.Blanc)
                    {
                        Client_TC.SetFirstHit(ChessBoardColorDown == K.Blanc ? (byte) 5 : (byte) 60);
                        Client_TC.SetFirstHit(ChessBoardColorDown == K.Blanc ? (byte) 6 : (byte) 59);
                        Client_TC.SetFirstHit(ChessBoardColorDown == K.Blanc ? (byte) 7 : (byte) 58);
                        Client_TC.SetFirstHit(ChessBoardColorDown == K.Blanc ? (byte) 8 : (byte) 57);
                    }
                    else
                    {
                        Client_TC.SetFirstHit(ChessBoardColorDown == K.Blanc ? (byte) 61 : (byte) 4);
                        Client_TC.SetFirstHit(ChessBoardColorDown == K.Blanc ? (byte) 62 : (byte) 3);
                        Client_TC.SetFirstHit(ChessBoardColorDown == K.Blanc ? (byte) 63 : (byte) 2);
                        Client_TC.SetFirstHit(ChessBoardColorDown == K.Blanc ? (byte) 64 : (byte) 1);
                    }
                }
            }
            else if (roque == K.GRoque)
            {
                if (color == K.Blanc)
                {
                    Client_TC.SetFirstHit(ChessBoardColorDown == K.Blanc ? (byte) 1 : (byte) 64);
                    Client_TC.SetFirstHit(ChessBoardColorDown == K.Blanc ? (byte) 2 : (byte) 63);
                    Client_TC.SetFirstHit(ChessBoardColorDown == K.Blanc ? (byte) 3 : (byte) 62);
                    Client_TC.SetFirstHit(ChessBoardColorDown == K.Blanc ? (byte) 4 : (byte) 61);
                    Client_TC.SetFirstHit(ChessBoardColorDown == K.Blanc ? (byte) 5 : (byte) 60);
                }
                else
                {
                    Client_TC.SetFirstHit(ChessBoardColorDown == K.Blanc ? (byte) 57 : (byte) 8);
                    Client_TC.SetFirstHit(ChessBoardColorDown == K.Blanc ? (byte) 58 : (byte) 7);
                    Client_TC.SetFirstHit(ChessBoardColorDown == K.Blanc ? (byte) 59 : (byte) 6);
                    Client_TC.SetFirstHit(ChessBoardColorDown == K.Blanc ? (byte) 60 : (byte) 5);
                    Client_TC.SetFirstHit(ChessBoardColorDown == K.Blanc ? (byte) 61 : (byte) 4);
                }
            }
            else
                Client_TC.SetFirstHit(ChessBoardColorDown == K.Blanc ? Dep : ((byte) (65 - Dep)));

            // Case Arrivee 
            if (roque == K.AucunRoque)
            {
                if (PriseEnPassant)
                {
                    if (color == K.Blanc)
                        Client_TC.SetFirstHit(ChessBoardColorDown == K.Blanc ? (byte)(Dest + 8) : (byte)(65 - (Dest + 8)));
                    else
                        Client_TC.SetFirstHit(ChessBoardColorDown == K.Blanc ? (byte)(Dest - 8) : (byte)(65 - (Dest - 8)));
                }

 //               success = Client_TC.SetFirstHit(Dest);
                Client_TC.SetFirstHit(Dest);
            }

            return (success);
        }

        static public bool MovePiece_PB_WS(byte Dep, byte Dest, byte roque, bool PriseEnPassant, byte color)
        {
            bool success = true;
            byte ChessBoardColorDown = ToolBoard.getColorBoard();

            PBlanches.WhitePiecesClient Client_PB = new PBlanches.WhitePiecesClient("BasicHttpBinding_IWhitePieces");

            // Case Depart
            if (roque == K.PRoque)
            {
                if (ChessBoardColorDown == K.Blanc)
                {
                    if (color == K.Blanc)
                    {
                        Client_PB.SetFirstHit(ChessBoardColorDown == K.Blanc ? (byte)5 : (byte)60);
                        Client_PB.SetFirstHit(ChessBoardColorDown == K.Blanc ? (byte)6 : (byte)59);
                        Client_PB.SetFirstHit(ChessBoardColorDown == K.Blanc ? (byte)7 : (byte)58);
                        Client_PB.SetFirstHit(ChessBoardColorDown == K.Blanc ? (byte)8 : (byte)57);
                    }
                    else
                    {
                        Client_PB.SetFirstHit(ChessBoardColorDown == K.Blanc ? (byte)61 : (byte)4);
                        Client_PB.SetFirstHit(ChessBoardColorDown == K.Blanc ? (byte)62 : (byte)3);
                        Client_PB.SetFirstHit(ChessBoardColorDown == K.Blanc ? (byte)63 : (byte)2);
                        Client_PB.SetFirstHit(ChessBoardColorDown == K.Blanc ? (byte)64 : (byte)1);
                    }
                }
            }
            else if (roque == K.GRoque)
            {
                if (color == K.Blanc)
                {
                    Client_PB.SetFirstHit(ChessBoardColorDown == K.Blanc ? (byte)1 : (byte)64);
                    Client_PB.SetFirstHit(ChessBoardColorDown == K.Blanc ? (byte)2 : (byte)63);
                    Client_PB.SetFirstHit(ChessBoardColorDown == K.Blanc ? (byte)3 : (byte)62);
                    Client_PB.SetFirstHit(ChessBoardColorDown == K.Blanc ? (byte)4 : (byte)61);
                    Client_PB.SetFirstHit(ChessBoardColorDown == K.Blanc ? (byte)5 : (byte)60);
                }
                else
                {
                    Client_PB.SetFirstHit(ChessBoardColorDown == K.Blanc ? (byte)57 : (byte)8);
                    Client_PB.SetFirstHit(ChessBoardColorDown == K.Blanc ? (byte)58 : (byte)7);
                    Client_PB.SetFirstHit(ChessBoardColorDown == K.Blanc ? (byte)59 : (byte)6);
                    Client_PB.SetFirstHit(ChessBoardColorDown == K.Blanc ? (byte)60 : (byte)5);
                    Client_PB.SetFirstHit(ChessBoardColorDown == K.Blanc ? (byte)61 : (byte)4);
                }
            }
 //           else
 //               Client_PB.SetFirstHit(ChessBoardColorDown == K.Blanc ? Dep : ((byte)(65 - Dep)));

            else if (roque == K.AucunRoque)
            {
                if (PriseEnPassant)
                {
                    // Case depart
                    Client_PB.SetFirstHit(ChessBoardColorDown == K.Blanc ? Dep : ((byte)(65 - Dep)));

                    // Case Arrivee
                    if (color == K.Blanc)
                    {
                        Client_PB.SetFirstHit(ChessBoardColorDown == K.Blanc ? (byte)(Dest + 8) : (byte)(65 - (Dest + 8)));
                    }
                    else
                    {
                        Client_PB.SetFirstHit(ChessBoardColorDown == K.Blanc ? (byte)(Dest - 8) : (byte)(65 - (Dest - 8)));
                    }

                    success = Client_PB.SetFirstHit(Dest);

                }
                else
                {
                    success = Client_PB.MovePiece(Dep, Dest);
                }


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

        static public byte Compare(byte noCaseTC, byte noCasePX, DateTime tc, DateTime px)
        {
            if (DateTime.Compare(tc, px) < 0)
                return (noCaseTC);
            else
                return (noCasePX);
        }

        static public bool PriseEnPassant(byte caseDepart, byte caseDest)
        {
            if (ToolBoard.PriseEnPassant(caseDepart, caseDest))
                return (true);

            return (false);
        }

        static public bool isSquareStillEmpty(byte caseX)
        {
            Bitmap colorCase = ToolBoard.TakePictureCaseColor(caseX);
            Bitmap colorCaseWhiteVide = ToolBoard.getImPairCaseEmpty();
            Bitmap colorCaseBlackVide = ToolBoard.getPairCaseEmpty();

            if (ToolBoard.CompareBitmaps((Image)colorCase, colorCaseWhiteVide) ||
                ToolBoard.CompareBitmaps((Image)colorCase, colorCaseBlackVide))
                return (true);

            return (false);
        }

    }

   

}
