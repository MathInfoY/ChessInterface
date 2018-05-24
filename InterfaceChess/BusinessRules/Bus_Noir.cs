using System;
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
    static public class BusinessNoir
    {
        private static byte m_Depart_Adv = 0;
        private static byte m_Dest_Adv = 0;

        private static bool m_PRoque = false;
        private static bool m_GRoque = false;

        static public short Process_Noir_Player(byte lastDep, byte lastDest, out byte roque,out CaseActivite[] cloneActivite)
        {
            
            short FindMoveArr = 0;
            short FindMoveDep = 0;
            byte caseDepart = 0;
            byte caseDest = 0;
            byte caseDepart_TC = 0;
            byte caseDepart_PN = 0;
            byte caseDest_TC = 0;
            byte caseDest_PB = 0;

            bool PriseEnPassant = false;
            List<byte> Departs = new List<byte>();
            List<byte> Destination = new List<byte>();

            DateTime LowestTime_TC = DateTime.Today.AddYears(1); // Timestamp pour une case du Web Service qui traite toute les cases 
            DateTime LowestTime_PB = DateTime.Today.AddYears(1); // Timestamp pour une case du Web Service qui traite seulement les Pieces Blanches
            DateTime LowestTime_PN = DateTime.Today.AddYears(1); // Timestamp pour une case du Web Service qui traite seulement les Pieces Blanches

            cloneActivite = null;
            roque = 0;

            // Trouve Coup Depart
            FindMoveDep = Business.GetDepartMovePlayer(lastDep, lastDest, K.Noir, out roque, Departs);

            // Erreur trouve au moins 2 coups de départ ont été trouvé on re-essaie
            if (FindMoveDep == -1)
            {
                if (Departs.Count > 5)
                {
                    Departs.Clear();
                    return (K.isResetingGame); // Nouvelle partie , beaucoup de changements 
                }

                Departs.Clear();
                FindMoveDep = Business.TestDepartAgain(lastDep, lastDest, K.Noir, out roque, Departs);
 
                if (FindMoveDep == -1) // Toujours la meme erreur on appel le Web Service pour trouver le coup
                {
#if SERVICE_TC
                    caseDepart_TC = Business.FindOlderMove_TC_WS(Departs, out LowestTime_TC);
#endif

#if SERVICE_PN
                    caseDepart_PN = Business.FindOlderMove_PB_WS(Departs, out LowestTime_PN);
#endif
                    if (caseDepart_TC > 0 || caseDepart_PN > 0)
                        caseDepart = Business.Compare(caseDepart_TC, caseDepart_PN, LowestTime_TC, LowestTime_PN);
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
                cloneActivite = ToolBoard.getCloneCaseActivite();

                // Tous les coups sauf le roque
                if (roque == K.AucunRoque)
                {
                    // Set_Activities_Move est appelé dans cette fonction
                    FindMoveArr = Business.GetDestMovePlayer(lastDep, lastDest, caseDepart, K.Noir, Destination, out PriseEnPassant);
                    if (FindMoveArr == K.isResetingGame)
                        return (K.isResetingGame);

                    // Aucun coup trouve
                    if (FindMoveArr == 0)
                    {
                        Log.LogText("Check Again ...");
                        Thread.Sleep(500);
                        FindMoveArr = Business.GetDestMovePlayer(lastDep, lastDest, caseDepart, K.Noir, Destination, out PriseEnPassant);
                        if (FindMoveArr == K.isResetingGame)
                            return (K.isResetingGame);
                    }

                    // Plusieurs coups trouvé : Identifier le bon.
                    if (FindMoveArr == -1)
                    {
                        Destination.Clear();
                        FindMoveArr = Business.TestDestAgain(lastDep, lastDest, caseDepart, K.Noir, Destination, out PriseEnPassant);
                        if (FindMoveArr == K.isResetingGame)
                            return (K.isResetingGame);
                    }

                    // Si Aucun coup : Verifie la case sélectionnée du coup précédent 
                    if (FindMoveArr == 0)
                    {
                        FindMoveArr = Business.WhichCaseSelected(lastDep, lastDest, caseDepart, K.Noir, out caseDest, out PriseEnPassant);
                        if (FindMoveArr == 1)
                            Destination.Add(caseDest);
                    }
#if SERVICE_TC
                    // Appel le Web Service des cases
                    if (FindMoveArr == 0)
                        FindMoveArr = Business.GetDest_TC_WS(lastDep, lastDest, caseDepart, K.Noir, Destination, out PriseEnPassant);
#endif
                    // Aucun coup trouvé : Vérifie si le coup arrive sur la case de départ ou d'arrivée du coup précédent 
                    if (FindMoveArr == 0)
                    {
                        FindMoveArr = Business.WhichCaseSelected(lastDep, lastDest, caseDepart, K.Noir, out caseDest, out PriseEnPassant);
                        if (FindMoveArr == 1)
                            EndProcessMove(caseDepart, caseDest, K.Noir, PriseEnPassant);
                    }

                    // Si un coup : Success ! 
                    else if (FindMoveArr == 1)
                    {
                        caseDest = Destination[0];
                        EndProcessMove(caseDepart, caseDest, K.Noir, PriseEnPassant);
                    }

                    // Si 2 coups ou plus ont ete trouves on appel le Web Service pour trouver le coup

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

#if SERVICE_PB
                        caseDest_PB = Business.FindOlderMove_PB_WS(Destination, out LowestTime_PB);
                        caseDest = caseDest_PB;
#endif

                        if (caseDest_TC > 0 || caseDest_PB > 0)
                        {
                            caseDest = Business.Compare(caseDest_TC, caseDest_PB, LowestTime_TC, LowestTime_PB);
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
                    EndProcessMove(roque, 0, K.Noir, false);
                    FindMoveArr = 1;
                }

                if (FindMoveArr == 1)
                {
#if SERVICE_TC
                    bool success;

                    success = Business.MovePiece_TC_WS(caseDepart, caseDest, roque, PriseEnPassant, K.Noir);

                    // *** ERREUR LE WS n'a pas detecte le coup ***
                    if (!success)
                        FindMoveArr = -1;
#endif
                }
            }

            return (FindMoveArr);
             
             



        }

        static private void EndProcessMove(byte caseDepart, byte caseDest, byte color, bool PriseEnPassant)
        {
            Business.Set_Activities_Move(caseDepart, caseDest, color, PriseEnPassant);

            if (caseDepart == K.PRoque)
            {
                m_Depart_Adv = 61;
                m_Dest_Adv = 63;
                m_PRoque = true;
            }
            else if (caseDepart == K.GRoque)
            {
                m_Depart_Adv = 61;
                m_Dest_Adv = 59;
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