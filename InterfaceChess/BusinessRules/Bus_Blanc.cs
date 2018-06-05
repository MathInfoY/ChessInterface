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

        private static byte m_Count_MultipleMovesDest = 0; 
         
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
         * 
         * Regle Coups
         * 
         * Depart
         *         1) Si on trouve une seule case (Succès) 
         *         2) Si on ne trouve pas de cases on recommence (Echec)
         *         3) Si on trouve plusieurs coups de départ au dela de 5 cases alors on recommence à zéro c'est une nouvelle partie (Echec)
         *         4) Si on trouve plusieurs coups de départ alors on appel le Web Service TC pour trouver le plus vieux (Succès).
         *         
         * Arrivée
         *         1) Si on trouve une seule case (Succès)
         *         2) Si on ne trouve pas de cases  (Echec)
         *            a. On attends un peu et on refait un Test.
         *            b. Si on ne trouve pas de cases, alors on appel le Web Service TC pour trouver le plus vieux coup. 
         *            c. Si on ne trouve toujours pas de cases alors on vérifie si le coup Blanc pourrait être l'une des deux cases du coup Noir précédent 
         *         3) Si on trouve plusieurs coups (Echec)
         *            a. On refait au maximun 3 tentatives pour trouver le coup d'Arrivé (Trouve le coup de Depart etc ...)
         *            b. Au dela de 3 tentatives, on appel le Web Service TC pour trouver le plus vieux coup (Success). 
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
//          byte caseDest_PN = 0;    // Case Arrivee    : Du Web Service qui traite toute les cases 

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
                        Log.LogText("Arr Not Found Check Again ...");
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
                    if (FindMoveArr == 1)
                    {
                        caseDest = Destination[0];
                        EndProcessMove(caseDepart, caseDest, K.Blanc, PriseEnPassant);
                    }

                    // Plusieurs coups ont ete trouvés il est inutile d'appeler les Web Service pour ces raisons :
                    // 1) Premier cas. La business rule du plus vieux coup ne tient plus pour les coups d'arrivees.
                    //    Il est possible que le joueur n'aie pas encore fini de deplacer la piece sur la case d'arrivee,
                    //    En consequence plusieurs cases sont rafraichies.
                    // 2) Second cas. Cas ou le Cb1 joue en c3 et que la case d2 est libre. Le Fou vient de jouer en d2 mais
                    //    le coup du cavalier n'a pas encore ete enregistré par l'application. Si La case d2 a ete redessinnee
                    //    avant la case c3 alors il y a un erreur. On doit appeler le Web Service.

                    //    On test 3 fois (pour le même coup de départ). Si apres 3 fois il y a toujours plusieurs coups
                    //    alors on appel le Web Service

                    else if (FindMoveArr == -1)
                    {
                        if (m_Count_MultipleMovesDest == 2)
                        {

                            // Call Web Service
                            caseDest_TC = Business.FindOlderMove_TC_WS(Destination, out LowestTime_TC);
                            if (caseDest_TC == 0)
                            {
                                Log.LogText(" *** ABNORMAL ERROR WEB SERVICE TC NO FOUND CASE ARR *** ");
                                return (-1);
                            }
                            caseDest = caseDest_TC;
                            m_Count_MultipleMovesDest = 0;
                        }
                        else
                        {
#if SERVICE_TC
                            m_Count_MultipleMovesDest++;
#endif
                            return (-1);
                        }
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

         public static void ResetMultipleMovesDest()
         {
             m_Count_MultipleMovesDest = 0;
         }
        
     }
}