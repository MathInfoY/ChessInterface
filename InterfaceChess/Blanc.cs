using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Messaging;
using System.Threading;

namespace InterfaceChess
{
    static public partial class Master
    {

        static public void LoopBlanc()
        {
            SquareTimeProcessingService.SquareTimeProcessingServiceClient client = new SquareTimeProcessingService.SquareTimeProcessingServiceClient("BasicHttpBinding_ISquareTimeProcessingService");
            Dictionary<string, int> items = (Dictionary<string, int>)CallContext.LogicalGetData("_items");
            CaseActivite[] cloneActivite = null;
            String[] txtMove = null;
            int counter_time = 0;
            byte Dep, Arr;
            byte lastDep = 0;
            byte roque = 0;
            byte lastArr = 0;

            short nbMoveFind = 0;

            for (int counter = 0; counter < 2000; counter++)
            {
                Thread.Sleep(100);

                counter_time++;

                if (items["END"] == 1)
                {
                    break;
                }
                
                if (items["CONFIG_BOARD"] == 0)
                    continue;

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
                        txtMove = Business.TranslateMoveInName(Dep, Arr, roque, cloneActivite);

                        // Photographies les cases Depart et d'Arrivée
                        Board.TakePictures(Dep, Arr, roque, K.Blanc);

                        // Photographies le coup précédent (Les cases sont maintenant désélectionnées) 
                        Board.UpdateBitmap(lastDep);
                        Board.UpdateBitmap(lastArr);

                        // Ecrit le coup dans le fichier
                        Log.LogCoups(txtMove.ToString(), K.Blanc, (byte)items["NO_COUP_B"], K.Player);

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
                        Business.StopGame(items["HUMAIN_COULEUR"] == K.Blanc ? K.Human : K.Player, K.Blanc, (byte)items["NO_COUP_B"]);
                        items["END"] = 1;
                    }
                }

                // Time-out atteint. Le coup de l'adversaire n'a jamais été joué (configuré pour 3 min)
                if (counter_time >= K.TimeOut)
                {
                    items["END"] = 1;
                }
            }

            GlobalSMEXE.SendMsg_Call_SquareTime_EXE(GlobalParameters_SquareTime.ST_STOPRUNNING, 0);
            client.Suspend(true);

            items["END"] = 1;
        }





    };

}