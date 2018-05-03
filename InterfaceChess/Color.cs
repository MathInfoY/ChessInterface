using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Messaging;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using Board_v2;

namespace InterfaceChess
{
    public partial class Master
    {
        private void LoopColor()
        {
            byte color = 0;
            bool isColorCompleted= false;
            String msg = null;
            Dictionary<string, int> items = null;

            QueueMsg.Init_Color();

            for (int counter = 0; counter < 500 && !isColorCompleted; counter++)
            {
                Thread.Sleep(100);
                items = (Dictionary<string, int>)CallContext.LogicalGetData("_items");

                if (items["END"] == 1)
                    break;

                msg = QueueMsg.ReadMessage_Color();

                if (msg.Equals("Humain_Blanc"))
                {
                    items["HUMAIN_COULEUR"] = K.Blanc;
                    color = K.Blanc;
                    Board.setColorBoard(K.Blanc);
                    Log.LogText(" ");
                    Log.LogText("*** Nouvelle Partie ***");
                    Log.LogText(" ");
                    isColorCompleted = true;
                }
                else if (msg.Equals("Adversaire_Blanc"))
                {
                    items["HUMAIN_COULEUR"] = K.Noir;
                    Board.setColorBoard(K.Noir);
                    color = K.Noir;
                    isColorCompleted = FirstMove(items);
                }
                else if (color == K.Noir)
                {
                    isColorCompleted = FirstMove(items);
                }

            }
        }

        private bool FirstMove(Dictionary<string, int> items)
        {
            bool isFirstMovePlayed = false;
            byte Dep = 0;
            byte Arr = 0;
            String[] txtMove = null;
            short nbMoveFind = 0;

            // L'adversaire Joue son premier coup... attendre jusqu'a 10 secondess 
            nbMoveFind = Business.ProcessFirstMove(out Dep, out Arr);

            if (nbMoveFind == 1)
            {
                Log.LogText(" ");
                Log.LogText("*** Nouvelle Partie ***");
                Log.LogText(" ");

                // Traduit le coup Blanc en texte
                txtMove = ConfigMove(Dep, Arr);

                // Ecrit le premier coup dans le fichier
                Log.LogCoups(txtMove.ToString(), K.Blanc, 1, K.Player);

                items["CASE_DEPART"] = Dep;
                items["CASE_DESTINATION"] = Arr;
                items["NO_COUP_B"] = 1;

                QueueMsg.SendMessage_To_OpenWindow(string.Format("{0};{1}", Dep, Arr)); // envoie le coup Blanc à l'interface 

                Log.LogText("Mon Coup : CasesDepart   : " + Dep);
                Log.LogText("Mon Coup : CasesArrivee  : " + Arr);
                Log.LogText("Mon Coup : Move          : " + txtMove);

                isFirstMovePlayed = true;

            }
            else
            {
                Log.LogText("Blancs n'ont pas joués ");
            }

           
            return (isFirstMovePlayed);
        }

        static public String[] ConfigMove(byte Dep, byte Arr)
        {
            CaseActivite[] cloneActivite = null;

            String[] txtMove = null;

            // Sauve les activites avant le coup joué i.e avant d'appeler EndProcessMove sinon on perd l'information
            cloneActivite = Board.getCloneCaseActivite();

            // Mise a jour des pieces pour chacune des cases
            BusinessBlanc.EndProcessMove(Dep, Arr, K.Blanc, false);

            // Traduit le coup en Texte
            txtMove = Business.TranslateMoveInName(Dep, Arr, 0, cloneActivite);

            // Photographies les cases Depart et d'Arrivée
            Board.TakePictures(Dep, Arr, 0, K.Blanc);

            return (txtMove);

        }
    }


}