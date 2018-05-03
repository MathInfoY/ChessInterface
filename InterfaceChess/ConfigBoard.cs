using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Messaging;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Diagnostics;
using System.Threading;
using Board_v2;

/*
 * Configuration de l'echiquer lorsqu'onj clique sur le bouton Config
 * 
 * => Envoi un message est envoyé de l'application GMA vers l'Interface que LoopSetupBoard() intercepte.
 * => Puis un autre message est envoyé contenant la position du coin haut gauche de l'echiquier
 * => Puis un dernier message est envoyé contenant la position du coin bas droite de l'echiquier
 * 
 * */

namespace InterfaceChess
{
    public partial class Master
    {
        static private int m_Board_xLeft = 0;
        static private int m_Board_width = 0;
        static private int m_Board_yTop = 0;
        static private int m_Board_Height = 0;


        /*
         * Configuration de l'échiquier : Messages
         * 
         * 1) Message ConfigBoard   => Se Prépare à la réception des messages du coin gauche haut et coin droit bas
         * 2) Message X             => Message du Coin Gauche Haut
         * 3) Message Y             => Message du Coin Droit Bas : 
         *                             Si Humain Blanc      =>   Echiquier Configuré
         *                             Si Adversaire Blanc  =>   Inscrit Premier coup joue    
         *                                                  =>   Echiquier Configuré
         *                             Sinon Echiquier est Configuré 
         * 4) Aucun Message         => Si Echiquier Configuré et l'adversaire Blanc n'a pas encore joué son premier coup
         *                                                  =>   Inscrit Premier coup joue    
         *                                                  =>   Echiquier Configuré
         */

        private void LoopSetupBoard()
        {
            byte Dep = 0;
            byte Arr = 0;
            String msg = null;
            String[] txtMove = null;
            string pathBoardFile = string.Empty;
            Dictionary<string, int> items = null;
            Business Business_Rules = null;
            short nbMoveFind = 0;

          QueueMsg.Init_ConfigBoard();

          for (int counter = 0; counter < 1000; counter++)
            {

                Thread.Sleep(100);
                items = (Dictionary<string, int>)CallContext.LogicalGetData("_items");

                if (items["END"] == 1)
                {
                    while (QueueMsg.ReadMessage_ConfigBoard() != K.Msg) ;
                    break;
                }

                msg = QueueMsg.ReadMessage_ConfigBoard();

                if (msg.Equals("PreLoad"))
                {
                    Arr = 0;
                }
  
                // #1 Click sur Configuration 
                if (msg.Equals("ConfigBoard"))
                {
                    ResetBoardCoord();
                    items["CONFIG_BOARD"] = 0;
                }

                // #2 Lecture d'un coin suite au Click sur Configuration : 2 coins à lire.
               else if (ReadCoordonneesBoard(msg))
                {
                    if (Business_Rules != null)
                        Business_Rules = null;

                    BuildBoard();
                    Log.PhotoBoard();

                    // L'adversaire a les Blancs
                    if (items["HUMAIN_COULEUR"] == K.Noir)
                    {
                        // L'adversaire Joue son premier coup... attendre jusqu'a 10 secondess 
                        nbMoveFind = Business.ProcessFirstMove(out Dep, out Arr);

                        if (nbMoveFind == 0)
                        {
                            for (byte tryAgain = 0; nbMoveFind == 0 && tryAgain < 20; tryAgain++)
                            {
                                Thread.Sleep(500);
                                nbMoveFind = Business.ProcessFirstMove(out Dep, out Arr);
                            }
                        }

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

                            items["CONFIG_BOARD"] = 1;
                        }
                        else
                        {
                            items["END"] = 0;
                            Log.LogText("Blancs n'ont pas joués ");
                        }

                    }
                    else
                    {
                        Log.LogText(" ");
                        Log.LogText("*** Nouvelle Partie ***");
                        Log.LogText(" ");

                        items["CONFIG_BOARD"] = 1;


                    }
// Lance l'application pour scanner toutes les cases

                    if (!Business.Start_WebService_Cases())
                        items["END"] = 1; // On arrete tout

                }
            }
        }


        static public void SetupBoard()
        {
            GenerateAutomaticallyBoardFromFile();

            Console.Beep();
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
            txtMove = Business.TranslateMoveInName(Dep, Arr,0, cloneActivite);

            // Photographies les cases Depart et d'Arrivée
            Board.TakePictures(Dep, Arr, 0, K.Blanc);

            return(txtMove);

        }

        private static void setColorCornerBoard()
        {
            if (Board.getBitmap(8) == null)
            {
                Board.UpdateBitmap(8);
                Board.UpdateColorCase(8);
            }
            if (Board.getBitmap(57) == null)
            {
                Board.UpdateBitmap(57);
                Board.UpdateColorCase(57);
            }
        }

        static private void ResetBoardCoord()
        {
            m_Board_xLeft = 0;
            m_Board_yTop = 0;
            m_Board_width = 0;
            m_Board_Height = 0;
        }
  
        public Boolean ReadCoordonneesBoard(String message)
        {
            byte i = 1;
            string data = string.Empty;

            int x = -1, y = -1;

            if (message == "No Message")
                return (false);

            if (message.Contains("="))
            {
                message = message.Substring(message.IndexOf("=") + 1);

                string[] words = message.Split(';');

                foreach (string word in words)
                {
                    data = word.Substring(word.IndexOf('=') + 1);

                    // Coordonnees lues sont dans le coin bas gauche et le coin superieur droit
                    try
                    {
                        if (i == 1) x = Convert.ToInt32(data);
                        else if (i == 2) y = Convert.ToInt32(data);
                    }
                    catch
                    {
                    }

                    i++;
                }

                if (m_Board_xLeft == 0) m_Board_xLeft = x;
                else m_Board_width = x - m_Board_xLeft;

                if (m_Board_yTop == 0) m_Board_yTop = y;
                else m_Board_Height = y - m_Board_yTop;
            }

            return (m_Board_width > 0 && m_Board_Height > 0);

        }

        private bool isBoardConfigured()
        {
            return (m_Board_width > 0 && m_Board_Height > 0);
        }

        private void BuildBoard()
        {
            Board.InitBoard();
            Board.config_PositionBoard(m_Board_xLeft, m_Board_yTop, m_Board_width, m_Board_Height, 2);
            Board.Reset_Echiquier();
            Board.Reset_GrapheCases(true);
            Board.setBoardConfigured();

            // Prendre la couleur d'une case vide paire et impaire dans le but de les utiliser pour comparaison ulterieure
            Business.SetColorCaseEmpty();

            Log.WriteCoordBoard(m_Board_xLeft, m_Board_yTop, m_Board_width, m_Board_Height);

            UpdateNoGame();

        }

        static private void UpdateNoGame()
        {
            int countFile = Log.GetIndexGame();
            Log.WriteIndexGame(countFile + 1);          
        }       
    }


}