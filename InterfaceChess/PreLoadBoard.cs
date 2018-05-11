using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Messaging;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tool;

namespace InterfaceChess
{
    static public partial class Master
    {
  

        // Nouvelle partie lorsq'on click sur Blanc ou Noir
        static public int NewGame(byte color)
        {
            bool findMove = false;

            ToolBoard.setColorBoard(color);
            ToolBoard.Reset_Echiquier();
            ToolBoard.Reset_GrapheCases();

            findMove = PlayFirstWhiteMove(color);

            Log.UpdateNoGame();

            return (findMove == true ? 1 : 0);
        }

        // Mise a jour du premier coup dans le cas ou Je joue avec les Noirs
        static private bool PlayFirstWhiteMove(byte color)
        {
            bool findMove = true;
            byte Dep = 0;
            byte Arr = 0;
            String[] txtMove = null;
            Dictionary<string, int> items = null;

            if (color == K.Noir)
            {
                items = (Dictionary<string, int>)CallContext.LogicalGetData("_items");

                // L'adversaire a joué son premier coup.
                findMove = ToolBoard.FirstOpponentWhiteMove(out Dep, out Arr);

                if (!findMove)
                {
                    for (byte tryAgain = 0; !findMove && tryAgain < 20; tryAgain++)
                    {
                        Thread.Sleep(500);
                        findMove = ToolBoard.FirstOpponentWhiteMove(out Dep, out Arr);
                    }
                }

                if (findMove)
                {
                    // Traduit le coup Blanc en texte, TransalateMove, Activite,TakePicture,
                    txtMove = ConfigMove(Dep, Arr);

                    // Ecrit le premier coup dans le fichier
                    Log.LogText(" ");
                    Log.LogText("*** Nouvelle Partie ***");
                    Log.LogText(" ");

                    Log.LogCoups(txtMove.ToString(), K.Blanc, 1, K.Player);

                    items["CASE_DEPART"] = Dep;
                    items["CASE_DESTINATION"] = Arr;
                    items["NO_COUP_B"] = 1;

//                  QueueMsg.SendMessage_To_OpenWindow(string.Format("{0};{1}", Dep, Arr)); // envoie le coup Blanc à l'interface 

                    Log.LogText("Adversaire : CasesDepart   : " + Dep);
                    Log.LogText("Adversaire : CasesArrivee  : " + Arr);
                }
            }
            else
            {
                Log.LogText(" ");
                Log.LogText("*** Nouvelle Partie ***");
                Log.LogText(" ");
            }

            return (findMove);
        }

        static private String[] ConfigMove(byte Dep, byte Arr)
        {
            CaseActivite[] cloneActivite = null;

            String[] txtMove = null;

            // Sauve les activites avant le coup joué i.e avant d'appeler EndProcessMove sinon on perd l'information
            cloneActivite = ToolBoard.getCloneCaseActivite();

            // Mise a jour des pieces pour chacune des cases
            BusinessBlanc.EndProcessMove(Dep, Arr, K.Blanc, false);

            // Traduit le coup en Texte
            txtMove = ToolBoard.TranslateMoveInName(Dep, Arr, 0, cloneActivite);

            // Photographies les cases Depart et d'Arrivée
            ToolBoard.TakePictures(Dep, Arr, 0, K.Blanc);

            return (txtMove);

        }


        static public void setColorCornerBoard()
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

    }
}
