using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Messaging;
using System.Threading;

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
            byte caseDepart_WS = 0;
            byte caseDest_WS = 0;
            bool PriseEnPassant = false;
            List<byte> Departs = new List<byte>();
            List<byte> Destination = new List<byte>();
//          WhitePiecesService.WhitePiecesClient client = new WhitePiecesService.WhitePiecesClient("BasicHttpBinding_IWhitePieces");

            cloneActivite = null;
            roque = 0;

            // Trouve Coup Depart
            FindMoveDep = Business.GetDepartMovePlayer(lastDep, lastDest, K.Noir, out roque, Departs);

            // Erreur trouve au moins 2 coups de départ ont été trouvé on re-essaie
            if (FindMoveDep == -1)
            {
                Departs.Clear();
                FindMoveDep = Business.TestDepartAgain(lastDep, lastDest, K.Noir, out roque, Departs);
 
                if (FindMoveDep == -1) // Toujours la meme erreur on appel le Web Service pour trouver le coup
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

            // Case Arrivee
            else
            {
                cloneActivite = Board.getCloneCaseActivite();

                // Tous les coups sauf le roque
                if (roque == K.AucunRoque)
                {
                    // Set_Activities_Move est appelé dans cette fonction
                    FindMoveArr = Business.GetDestMovePlayer(lastDep, lastDest, caseDepart, K.Noir, Destination, out PriseEnPassant);

                    // Aucun coup trouve
                    if (FindMoveArr == 0)
                    {
                        Log.LogText("Check Again ...");
                        Thread.Sleep(500);
                        FindMoveArr = Business.GetDestMovePlayer(lastDep, lastDest, caseDepart, K.Noir, Destination, out PriseEnPassant);
                    }

                    // Plusieurs coups trouvé : Identifier le bon.
                    if (FindMoveArr == -1)
                    {
                        Destination.Clear();
                        FindMoveArr = Business.TestDestAgain(lastDep, lastDest, caseDepart, K.Noir, Destination, out PriseEnPassant);
                    }

                    // Si Aucun coup : Verifie la case sélectionnée du coup précédent 
                    if (FindMoveArr == 0)
                    {
                        FindMoveArr = Business.WhichCaseSelected(lastDep, lastDest, caseDepart, K.Noir, out caseDest, out PriseEnPassant);
                        if (FindMoveArr == 1)
                            Destination.Add(caseDest);
                    }
#if SERVICE
                    // Appel le Web Service des cases
                    if (FindMoveArr == 0)
                    {
                        FindMoveArr = Business.GetDestSquareWebService(lastDep, lastDest, caseDepart, K.Noir, Destination, out PriseEnPassant);
                    }
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
#if SERVICE
                        FindMoveArr = Business.findSmallerTimeWS(Destination, out caseDest_WS);

                        if (FindMoveArr == 1)
                        {
                            caseDest = caseDest_WS;

                            if (Business.PriseEnPassant(caseDepart, caseDest_WS))
                                PriseEnPassant = true;
                            else
                                PriseEnPassant = false;

                            EndProcessMove(caseDepart, caseDest_WS, K.Noir, PriseEnPassant);
                        }
                        else if (FindMoveArr == -1)
                            return (-1);
#else
                        return(-1);
#endif
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

#if SERVICE
                if (FindMoveArr == 1)
                { 
                    bool success;

                    success = Business.UpdateSquareWebService(caseDepart, caseDest, roque, PriseEnPassant, K.Noir);

                    // *** ERREUR LE WS n'a pas detecte le coup ***
                    if (!success)
                        FindMoveArr = -1;

                }
#endif
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