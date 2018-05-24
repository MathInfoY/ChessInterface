using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Messaging;
using Tool;

namespace InterfaceChess
{
    static public partial class Master
    {
        static public void LoopNoir()
        {
            Dictionary<string, int> items = null;
            Tool.CaseActivite[] cloneActivite = null;
            String[] txtMove = null;
            byte lastDep = 0;
            byte lastArr = 0;
            byte Dep = 0;
            byte Arr = 0;
            byte roque = 0;
            short nbMoveFind = 0;
            Boolean RegeneratedCorner = false;
          
            int counter_time = 0;

            while(true)
            {
                Thread.Sleep(100);

                counter_time++;

                items = (Dictionary<string, int>)CallContext.LogicalGetData("_items");

                if (items["END"] == 1)
                    break;

                if (items["HOLD"] == 1)
                {
                    // Mode Hold confirmer
                    items["THREAD_WAITING_TCATUS"] = 1;

//                    if (counter_time % 100 == 0)
//                        Log.LogText("Waiting...");

                    continue;
                }


                if (items["NO_COUP_B"] == items["NO_COUP_N"] + 1)
                {
                    lastDep = (byte)items["CASE_DEPART"];
                    lastArr = (byte)items["CASE_DESTINATION"];

                    // Regenere les coins (cases 8 et 57)  de l'echiquiers afin qu'ils ne soient pas
                    // selectionnes. Le 1er coup Blanc a été joué donc les cases sont revenues à la normal. 
                    if (!RegeneratedCorner)
                    {
                        setColorCornerBoard();
                        RegeneratedCorner = true;
                    }

                    // Cherche Case Depart et Case Destination
                    nbMoveFind = BusinessNoir.Process_Noir_Player(lastDep, lastArr, out roque, out cloneActivite);

                    if (nbMoveFind == 1)
                    {
                        // Récupère Départ et Destination
                        BusinessNoir.Get_Move_Dep_Player(out Dep);
                        BusinessNoir.Get_Move_Arr_Player(out Arr);

                        // Converti en text le coup
                        txtMove = ToolBoard.TranslateMoveInName(Dep, Arr, roque, cloneActivite);

                        ToolBoard.TakePictures(Dep, Arr, roque,K.Noir);

                        // Photographies le coup précédent (Les cases sont maintenant désélectionnées) 
                        if (Business.isSquareStillEmpty(lastDep))
                            ToolBoard.UpdateBitmap(lastDep);

                        ToolBoard.UpdateBitmap(lastArr);

                        // Ecrit le coup dans le fichier
                        Log.LogCoups(txtMove[0], K.Noir, (byte)items["NO_COUP_N"], K.Player);

                        items["CASE_DEPART"] = Dep;
                        items["CASE_DESTINATION"] = Arr;
                        items["NO_COUP_N"]++;

                        Log.LogText("(" + Dep + "," + Arr + ")" + "\t" + txtMove[0]);
                    }
                    else if (nbMoveFind == 0)
                    {
                        Log.LogText("...-");
                    }
                    else if (nbMoveFind == -1)
                    {
#if ON_SERVICE
                        Log.LogText(" *** ERROR (N) ***");
                        Business.StopGame(items["HUMAIN_COULEUR"] == K.Blanc ? K.Human : K.Player, K.Blanc, (byte)items["NO_COUP_N"]);
                        items["END"] = 1;
#else
                         Log.LogText("...-");
#endif
                    }
                    else if (nbMoveFind == K.isResetingGame)
                    {
                        items["HOLD"] = 1;
                        Log.LogText("End Game");
                    }
                    
                }

                // Time-out atteint. Le coup de l'adversaire n'a jamais été joué (configuré pour 3 min)
/*
                if (counter_time >= K.TimeOut)
                {
                    items["END"] = 1;
                }
*/
            }

            items["END"] = 1;
        }
        /*
         * Déselectionne les 2 cases du coin de l'échiquier 
         * */
        static private void UnSelectBoardCorner()
        {
            if (ToolBoard.getBitmap(8) == null)
            {
                ToolBoard.UpdateBitmap(8);
                ToolBoard.UpdateColorCase(8);
            }
            if (ToolBoard.getBitmap(57) == null)
            {
                ToolBoard.UpdateBitmap(57);
                ToolBoard.UpdateColorCase(57);
            }

        }
    };

}