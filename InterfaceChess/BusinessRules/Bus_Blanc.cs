using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using Tool;

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

                    m_Depart_Hum = ToolBoard.getCase(x, y);
                    m_Dest_Hum = 0;
                }
                else if (m_Depart_Hum > 0 && QueueMsg.isClickUp(msg))
                {
                    QueueMsg.GetCoordonneeBoard(msg, out x, out y);

                    m_Dest_Hum = ToolBoard.getCase(x, y);

                    if (m_Dest_Hum == m_Depart_Hum || m_Dest_Hum == 0)
                        m_Dest_Hum = m_Depart_Hum = 0;
                }

                roque = ToolBoard.ValidateRoque(m_Depart_Hum, m_Dest_Hum);
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
            // Success/Echec : 

            // 0 => aucun coup, 
            // 1 => un coup, 
            //-1 => plusieurs coups, 
            // 99 => est en train de rejouer une partie

            short FindMoveDep = 0;       // Case Finale Depart
            short FindMoveArr = 0;       // Case Finale Arrivee

            // Cases Depart + Arrivee
            byte caseDepart = 0;
            byte caseDest = 0;
            byte caseDepart_TC = 0;  // Case Depart     : Du Web Service qui traite toute les cases 
            byte caseDepart_PB = 0;  // Case Depart     : Du Web Service qui traite seulement les Pieces Blanches
            byte caseDest_TC = 0;    // Case Arrivee    : Du Web Service qui traite toute les cases 
            byte caseDest_PN = 0;    // Case Arrivee    : Du Web Service qui traite toute les cases 

            DateTime LowestTime_TC = DateTime.Today.AddYears(1); // Timestamp pour une case du Web Service qui traite toute les cases 
            DateTime LowestTime_PB = DateTime.Today.AddYears(1); // Timestamp pour une case du Web Service qui traite seulement les Pieces Blanches
            DateTime LowestTime_PN = DateTime.Today.AddYears(1); // Timestamp pour une case du Web Service qui traite seulement les Pieces Blanches

            bool PriseEnPassant = false;
            List<byte> Departs = new List<byte>();
            List<byte> Destination = new List<byte>();

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
#if SERVICE_TC
                    caseDepart_TC = Business.FindOlderMove_TC_WS(Departs, out LowestTime_TC);
#endif

#if SERVICE_PB
                    caseDepart_PB = Business.FindOlderMove_PB_WS(Departs, out LowestTime_PB);
#endif
                    if (caseDepart_TC > 0 || caseDepart_PB > 0)
                        caseDepart = Business.Compare(caseDepart_TC, caseDepart_PB, LowestTime_TC, LowestTime_PB);
                    else
                        return (-1);
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
                cloneActivite = ToolBoard.getCloneCaseActivite();

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

                    // Appel le Web Service qui traite Toute les Cases
#if SERVICE_TC
                    if (FindMoveArr == 0)
                        FindMoveArr = Business.GetDest_TC_WS(lastDep, lastDest, caseDepart, K.Blanc, Destination, out PriseEnPassant);
#endif
                    // Aucun coup trouvé : Vérifie si le coup arrive sur la case de départ ou d'arrivée du coup précédent 
                    if (FindMoveArr == 0)
                        FindMoveArr = Business.WhichCaseSelected(lastDep, lastDest, caseDepart, K.Blanc, out caseDest, out PriseEnPassant);
                    
                    // Si un coup : Success ! 
                    else if (FindMoveArr == 1)
                    {
                        caseDest = Destination[0];
                        EndProcessMove(caseDepart, caseDest, K.Blanc, PriseEnPassant);
                    }

                    // Plusieurs coups ont ete trouvé on appel le Web Service pour trouver le premier coup joué                    
                    else if (FindMoveArr == -1)
                    {
#if SERVICE_TC
                        caseDest_TC = Business.FindOlderMove_TC_WS(Destination, out LowestTime_TC);
                        if (caseDest_TC == 0)
                        {
                            Log.LogText(" *** ABNORMAL ERROR ARRIVEE *** ");
                            return (-1);
                        }
                        caseDest = caseDest_TC;
#endif

#if SERVICE_PN
                        caseDest_PN = Business.FindOlderMove_PN_WS(Destination, out LowestTime_PN);
                        caseDest = caseDest_PN;
#endif
                        if (caseDest_TC > 0 || caseDest_PN > 0)
                        {
                            caseDest = Business.Compare(caseDest_TC, caseDest_PN, LowestTime_TC, LowestTime_PN);
                            PriseEnPassant = Business.PriseEnPassant(caseDepart, caseDest);
                            EndProcessMove(caseDepart, caseDest, K.Blanc, PriseEnPassant);
                         }
                         else
                           return (-1);

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


                if (FindMoveArr == 1)
                {
                    bool success = true;

#if SERVICE_TC
                    success = Business.MovePiece_TC_WS(caseDepart, caseDest, roque, PriseEnPassant, K.Blanc);
#endif

#if SERVICE_PB
                    success = Business.MovePiece_PB_WS(caseDepart, caseDest, roque, PriseEnPassant, K.Blanc);
#endif
                    // *** ERREUR LE WS n'a pas detecte le coup ***
                    if (!success)
                        FindMoveArr = -1;
                }
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