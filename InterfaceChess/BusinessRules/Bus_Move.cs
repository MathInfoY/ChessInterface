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

                screenBmp = ToolBoard.TakePictureCase(i, Convert.ToByte(ConfigurationManager.AppSettings["ZoomOutCase"].ToString().Trim()));
                screenBmpFromList = ToolBoard.getBitmap(i);

                if (screenBmpFromList == null)
                    isResetingGame = true; // Est en train de redessiner l'echiquier (nouvelle partie)

                else if (!ToolBoard.CompareBitmaps(screenBmp, screenBmpFromList))
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

        static public short GetDest_ST_WS(byte LastDep, byte LastDest, byte CaseDepart, byte color, List<byte> DestMoves, out bool PriseEnPassant)
        {
            List<byte> findDestMoves = null;
            short moveFoundDest = 0; // aucun coup trouve

            PriseEnPassant = false;

            findDestMoves = FindMoves_ST_WS(LastDep, LastDest, CaseDepart, color, out PriseEnPassant);

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

                screenBmp = ToolBoard.TakePictureCase(i, Convert.ToByte(ConfigurationManager.AppSettings["ZoomOutCase"].ToString().Trim()));
                screenBmpFromList = ToolBoard.getBitmap(i);

                if (screenBmpFromList == null)
                    isResetingGame = true;

                else if (!ToolBoard.CompareBitmaps(screenBmp, screenBmpFromList))
                {
                    colorCaseArr = ToolBoard.TakePictureCaseColor(i);

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

        static private List<byte> FindMoves_ST_WS(byte LastDep, byte LastArr, byte CaseDepart, byte color, out bool isPriseEnPassant)
        {
            List<byte> findNewMoves = new List<byte>();

            SquareTimeProcessingService.SquareTimeProcessingServiceClient client = new SquareTimeProcessingService.SquareTimeProcessingServiceClient("BasicHttpBinding_ISquareTimeProcessingService");

            CaseActivite[] CasesActivite = ToolBoard.getCasesAvailable();
            isPriseEnPassant = false;

            for (byte i = 1; i <= 64; i++)
            {
                if (i == LastDep || i == LastArr)
                    continue;

                if (!ToolBoard.LegalMove(CasesActivite, CaseDepart, i, color))
                    continue;

                if (client.GetFirstHit(i) != default(DateTime))
                {
                    Log.LogText("WPService Arrivee ---> " + i);

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
            ChessBoardColorDown = ToolBoard.getColorBoard();

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
            bool isSmallerFound = false;
            PBlanches.WhitePiecesClient Client_PB = new PBlanches.WhitePiecesClient("BasicHttpBinding_IWhitePieces"); 

            DateTime dt;

            response = 0;

            for (byte i = 0; i < Cases.Count; i++)
            {
                dt = Client_PB.GetFirstHit(Cases[i]);

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

            SquareTimeProcessingService.SquareTimeProcessingServiceClient Client_SquareTime_WS = new SquareTimeProcessingService.SquareTimeProcessingServiceClient("BasicHttpBinding_ISquareTimeProcessingService");

            try
            {
                // Si le numero de caseA = 200 et caseB = 100 alors retourne True donc c'est la case B qui fut jouée la première
                WS_ValueCaseA = Client_SquareTime_WS.GetValue(ChessBoardColorDown == K.Blanc ? caseA : (byte)(65 - caseA));
                WS_ValueCaseB = Client_SquareTime_WS.GetValue(ChessBoardColorDown == K.Blanc ? caseB : (byte)(65 - caseB));

                WS_FirstDTHitCaseA = Client_SquareTime_WS.GetFirstHit(ChessBoardColorDown == K.Blanc ? caseA : (byte)(65 - caseA));
                WS_FirstDTHitCaseB = Client_SquareTime_WS.GetFirstHit(ChessBoardColorDown == K.Blanc ? caseB : (byte)(65 - caseB));

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

            GlobalSMEXE.SendMsg_Call_SquareTime_EXE(GlobalParameters_SquareTime.ST_GETFIRSTHIT, ChessBoardColorDown == K.Blanc ? caseA : (byte)(65 - caseA),1);

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

        static short DoTestSquare(byte noCaseA, byte noCaseB, out int ValA, out int ValB)
        {
            short err = -1;
            SquareTimeProcessingService.SquareTimeProcessingServiceClient Client_SquareTime_WS = new SquareTimeProcessingService.SquareTimeProcessingServiceClient("BasicHttpBinding_ISquareTimeProcessingService");

            ValA = 0;
            ValB = 0;

            for (byte nbTry = 1; nbTry <= 5 && err == -1; nbTry++)
            {
                Thread.Sleep(500);
                ValA = Client_SquareTime_WS.GetValue(noCaseA);
                ValB = Client_SquareTime_WS.GetValue(noCaseB);

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
            CaseActivite[] BoardLogic = ToolBoard.getCasesAvailable();
            SquareTimeProcessingService.SquareTimeProcessingServiceClient Client_SquareTime_WS = new SquareTimeProcessingService.SquareTimeProcessingServiceClient("BasicHttpBinding_ISquareTimeProcessingService");

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
                                    valRook = Client_SquareTime_WS.GetValue(8);

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
                                        valRook = Client_SquareTime_WS.GetValue(1);

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
                                    valRook = Client_SquareTime_WS.GetValue(64);

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
                                        valRook = Client_SquareTime_WS.GetValue(57);

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
            byte ChessBoardColorDown = ToolBoard.getColorBoard();

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
