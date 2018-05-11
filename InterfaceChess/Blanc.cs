using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using Tool;

namespace InterfaceChess
{
    static public partial class Master
    {

        static public void LoopBlanc()
        {
            Dictionary<string, int> items = null;
            Tool.CaseActivite[] cloneActivite = null;
            String[] txtMove = null;
            byte Dep, Arr;
            byte lastDep = 0;
            byte roque = 0;
            byte lastArr = 0;
            int counter_time = 0;

            short nbMoveFind = 0;

            while (true)
            {
                Thread.Sleep(100);

                counter_time++;

                items = (Dictionary<string, int>)CallContext.LogicalGetData("_items");

                if (items["END"] == 1)
                    break;

                if (items["HOLD"] == 1)
                {
                    // Mode Hold confirmer
                    items["THREAD_WAITING_STATUS"] = 1;

                    if (counter_time%100 == 0)
                        Log.LogText("Waiting...");

                    continue;
                }

                if (items["NO_COUP_B"] == items["NO_COUP_N"])
                {
                    lastDep = (byte)items["CASE_DEPART"];
                    lastArr = (byte)items["CASE_DESTINATION"];

                    // Cherche Case Depart et Case Destination
                    nbMoveFind = BusinessBlanc.Process_Blanc_Player(lastDep, lastArr, out roque, out cloneActivite);

                    if (nbMoveFind == 1)
                    {
                        // Récupère Départ et Destination
                        BusinessBlanc.Get_Move_Dep_Player(out Dep);
                        BusinessBlanc.Get_Move_Arr_Player(out Arr);

                        // Converti en text le coup
                        txtMove = ToolBoard.TranslateMoveInName(Dep, Arr, roque, cloneActivite);

                        // Photographies les cases Depart et d'Arrivée
                        ToolBoard.TakePictures(Dep, Arr, roque, K.Blanc);

                        // Photographies le coup précédent (Les cases sont maintenant désélectionnées) 
                        ToolBoard.UpdateBitmap(lastDep);
                        ToolBoard.UpdateBitmap(lastArr);

                        // Ecrit le coup dans le fichier
                        Log.LogCoups(txtMove[0], K.Blanc, (byte)items["NO_COUP_B"], K.Player);

                        items["CASE_DEPART"] = Dep;
                        items["CASE_DESTINATION"] = Arr;
                        items["NO_COUP_B"]++;

 //                     QueueMsg.SendMessage_To_OpenWindow(string.Format("{0};{1}", Dep, Arr)); // envoie le coup Blanc à l'interface 

                        Log.LogText("(" + Dep + "," + Arr + ")" + "\t" + txtMove[0]);
                    }
                    else if (nbMoveFind == 0)
                    {
                        Log.LogText("...-");
                    }
                    else if (nbMoveFind == -1)
                    {
#if ON_SERVICE
                        Log.LogText(" *** ERROR (B) ***");
                        Business.StopGame(items["HUMAIN_COULEUR"] == K.Blanc ? K.Human : K.Player, K.Blanc, (byte)items["NO_COUP_B"]);
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

            }

            items["END"] = 1;
        }





    };

}