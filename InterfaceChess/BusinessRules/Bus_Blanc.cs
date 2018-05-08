using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Messaging;
using System.Threading;


namespace InterfaceChess
{

     static public class BusinessBlanc
     {
        private static byte m_Depart_Hum = 0;
        private static byte m_Dest_Hum = 0;

        private static byte m_Depart_Adv = 0;
        private static byte m_Dest_Adv = 0;

        private static bool m_PRoque = false;
        private static bool m_GRoque = false;
         
        static public bool Process_Blanc_Human(out byte roque)
        {
            int x, y;

            roque = 0;

            String msg = QueueMsg.ReadQueue();

            if (!msg.Equals(K.Msg))
            {
                if (QueueMsg.isClickDown(msg))
                {
                    QueueMsg.GetCoordonneeBoard(msg, out x, out y);

                    m_Depart_Hum = Board.getCase(x, y);
                    m_Dest_Hum = 0;
                }
                else if (m_Depart_Hum > 0 && QueueMsg.isClickUp(msg))
                {
                    QueueMsg.GetCoordonneeBoard(msg, out x, out y);

                    m_Dest_Hum = Board.getCase(x, y);

                    if (m_Dest_Hum == m_Depart_Hum || m_Dest_Hum == 0)
                        m_Dest_Hum = m_Depart_Hum = 0;
                }

                roque = Business.ValidateRoque(m_Depart_Hum, m_Dest_Hum);
            }
            else
                return (false);

            return (m_Dest_Hum > 0);
        }
         /*
          * Retourne  0 si aucun coup
          * Retourne  1 si coup trouvé
          * Retourne -1 si erreur coup depart
          * Retourne -2 si erreur coup d'arrivee
          */
        static public short Process_Blanc_Player(byte lastDep, byte lastDest, out byte roque, out CaseActivite[] cloneActivite)
        {
            short FindMoveArr = 0; // 0 => aucun coup, 1 => un coup, -1 => 2 coups trouvés et +, 99 = entrain de redessiner l'echiquier (nouvelle partie)
            short FindMoveDep = 0; // 0 => aucun coup, 1 => un coup, -1 => 2 coups trouvés et +, 99 = entrain de redessiner l'echiquier (nouvelle partie)
            byte caseDepart_WS = 0;
            byte caseDepart = 0;
            byte caseDest = 0;

            byte caseDest_WS = 0;
            bool PriseEnPassant = false;
            List<byte> Departs = new List<byte>();
            List<byte> Destination = new List<byte>();
#if USE_WS
            WhitePiecesService.WhitePiecesClient client_WPiece = new WhitePiecesService.WhitePiecesClient("BasicHttpBinding_IWhitePieces");
#endif
            cloneActivite = null;
            roque = 0;

            // Trouve Coup Depart
            FindMoveDep = Business.GetDepartMovePlayer(lastDep, lastDest, K.Blanc, out roque, Departs);

            // Erreur trouve au moins 2 coups de départ ont été trouvé on re-essaie
            if (FindMoveDep == -1)
            {
                if(Departs.Count > 5)
                {
                    Departs.Clear();
                    return (K.isResetingGame); // Nouvelle partie , beaucoup de changements 
                }

                Departs.Clear();
                FindMoveDep = Business.TestDepartAgain(lastDep, lastDest, K.Blanc, out roque, Departs);

            // Plusieurs coups de départ
                if (FindMoveDep == -1)
                {
#if SERVICE
                    FindMoveDep = Business.findSmallerTimeWS(Departs, out caseDepart_WS);

                    if (FindMoveDep == 1)
                        caseDepart = caseDepart_WS;
                    else if (FindMoveDep == -1)
                        return (-1);
#else
                    return(-1);
#endif
                }

                else if (FindMoveDep == 1)
                    caseDepart = Departs[0];
            }
            else if (FindMoveDep == 1)
                caseDepart = Departs[0];

            if (FindMoveDep == 0)
                return (0);

            else if (FindMoveDep == K.isResetingGame)
                return (K.isResetingGame);

            // Case Arrivee
            else
            {
                // Copie l'échiquier logique avant le déplacement des pièces
                cloneActivite = Board.getCloneCaseActivite();

                // Tous les coups sauf le roque
                if (roque == K.AucunRoque)
                {
                    // Set_Activities_Move est appelé dans cette fonction
                    FindMoveArr = Business.GetDestMovePlayer(lastDep, lastDest, caseDepart, K.Blanc, Destination, out PriseEnPassant);
                    if (FindMoveArr == K.isResetingGame)
                        return (K.isResetingGame);

                    // Aucun coup trouve
                    if (FindMoveArr == 0)
                    {
                        Log.LogText("Check Again ...");
                        Thread.Sleep(500);
                        FindMoveArr = Business.GetDestMovePlayer(lastDep, lastDest, caseDepart, K.Blanc, Destination, out PriseEnPassant);
                        if (FindMoveArr == K.isResetingGame)
                            return (K.isResetingGame);
                    }
                    // Plusieurs coups trouvé : Identifier le bon.
                    if (FindMoveArr == -1)
                    {
                        Thread.Sleep(500);
                        Destination.Clear();
                        FindMoveArr = Business.TestDestAgain(lastDep, lastDest, caseDepart, K.Blanc, Destination, out PriseEnPassant);
                        if (FindMoveArr == K.isResetingGame)
                            return (K.isResetingGame);
                    }

                    // Aucun coup trouvé : Vérifie si le coup arrive sur la case de départ ou d'arrivée du coup précédent 
                    if (FindMoveArr == 0)
                    {
                        FindMoveArr = Business.WhichCaseSelected(lastDep, lastDest, caseDepart, K.Blanc, out caseDest, out PriseEnPassant);
                    }
                    // Appel le Web Service des pieces
#if SERVICE
                    if (FindMoveArr == 0)
                        FindMoveArr = Business.GetDestSquareWebService(lastDep, lastDest, caseDepart, K.Blanc, Destination, out PriseEnPassant);
#endif
                    // Si un coup : Success ! 
                    else if (FindMoveArr == 1)
                    {
                        caseDest = Destination[0];
                        EndProcessMove(caseDepart, caseDest, K.Blanc, PriseEnPassant);
                    }
                    // Plusieurs coups trouvé ou plus ont ete trouvé on appel le Web Service pour trouver le premier coup joué

                    else if (FindMoveArr == -1)
                    {
#if SERVICE
                        FindMoveArr = Business.findSmallerTimeWS(Destination, out caseDest_WS);

                        if (FindMoveArr == 1)
                        {
                            caseDest = caseDest_WS;

                            if (Business.PriseEnPassant(caseDepart, caseDest_WS))
                                PriseEnPassant = true;
                            else
                                PriseEnPassant = false;

                            EndProcessMove(caseDepart, caseDest_WS, K.Blanc, PriseEnPassant);
                        }

                        else if (FindMoveArr == -1)
                            return (-1);
#else
                        return (-1);
#endif
                    }

                    else
                        return (0);
                }
                // Roque
                else
                {
                    EndProcessMove(roque, 0, K.Blanc, false);
                    FindMoveArr = 1;
                }



                // Remets a zero (timestamp) la case modifiee.

#if SERVICE
                if (FindMoveArr == 1)
                {
                    bool success;

                    success = Business.UpdateSquareWebService(caseDepart, caseDest, roque, PriseEnPassant, K.Blanc);

                    // *** ERREUR LE WS n'a pas detecte le coup ***
                    if (!success)
                        FindMoveArr = -1;
                }
#endif
            }
            return (FindMoveArr);
        }

        static public void EndProcessMove(byte caseDepart, byte caseDest, byte color, bool PriseEnPassant)
        {
            Business.Set_Activities_Move(caseDepart, caseDest, color, PriseEnPassant);

            if (caseDepart == K.PRoque)
            {
                m_Depart_Adv = 5;
                m_Dest_Adv = 7;
                m_PRoque = true;
            }
            else if (caseDepart == K.GRoque)
            {
                m_Depart_Adv = 5;
                m_Dest_Adv = 3;
                m_GRoque = true;
            }
            else
            {
                m_Depart_Adv = caseDepart;
                m_Dest_Adv = caseDest;
            }
        }

        static public bool isPRoque()
        {
            return (m_PRoque);
        }

        static public bool isGRoque()
        {
            return (m_GRoque);
        }

         static public void Get_Move_Dep_Player(out byte Dep)
         {
             Dep = m_Depart_Adv;
         }

         public static void Get_Move_Arr_Player(out byte Arr)
         {
             Arr = m_Dest_Adv;
         }
        
     }
}