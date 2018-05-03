using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Runtime.InteropServices;

namespace InterfaceChess
{
    static class GlobalFile
    {
        static public string Log = "C:\\Chess\\Interface.Log";
    }

    static class GlobalTask
    {
        public static Task<bool> g_Task; 
    }

    static class GlobalParameters_SquareTime
    {
        public const uint ST_INITSTART = 0;
        public const uint ST_STARTRUNNING = 1;
        public const uint ST_GETFIRSTHIT = 2;
        public const uint ST_STOPRUNNING = 3;
        public const uint ST_SETEMPTYCASE = 4;
        public const uint ST_NEWGAME = 5;

        public static int ST_noCase; // Si 0 initialisation seulement , si -1 alors il y a une piece sur la case. 
        public static string ST_time;
    }


    public struct COPYDATASTRUCT
    {
        /// <summary>
        /// User defined data to be passed to the receiving application.
        /// </summary>
        public IntPtr dwData;

        /// <summary>
        /// The size, in bytes, of the data pointed to by the lpData member.
        /// </summary>
        public int cbData;

        /// <summary>
        /// The data to be passed to the receiving application. This member can be IntPtr.Zero.
        /// </summary>
        public IntPtr lpData;
    }

    public partial class StartUp : System.Windows.Forms.Form
    {
        Thread Thread_Blanc = null;
        Thread Thread_Noir = null;

        private const int RF_TESTMESSAGE = 0xA123;
        private const int WM_COPYDATA = 0x004A;
        private const int WM_ENTERIDLE = 0x0121;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int SendMessage(IntPtr hwnd, [MarshalAs(UnmanagedType.U4)] int Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        public static extern bool Beep(int freq, int duration);

        public StartUp()
        {
            // Start les Web Services aynchrone
            GlobalTask.g_Task = Start_SquareTime_WS();

            InitializeComponent();

            this.KeyPreview = true;

            this.KeyPress += new KeyPressEventHandler(StartUp_KeyPress);

//          ExternalProcess GMA_Monitor_App = new ExternalProcess();
//          GMA_Monitor_App.Start();

//          CallSquareTime_EXE();

        }

        /*****************************************************************************
         * 
         *                  Demarre le Web Service SquareTime.exe
         *          
         ******************************************************************************/ 
        static public async Task<bool> Start_SquareTime_WS()
        {
            bool isOk = true;

            isOk = await Task.Run(() => Start_ST_WS());

            // Lance l'application pour scanner toutes les cases appel le Web Service lors de la discrimination deux cases possibles
            if (!isOk)
            {
                MessageBox.Show("Le Web Service des Cases n'est pas demarre");
            }
#if USE_WS
            // Lance l'application pour scanner toutes les pieces Blanches 
            if (!Business.Start_WebService_WP())
            {
                MessageBox.Show("Le Web Service des Pièces Blanches n'est pas demarre");
                isOk = false;
            }
#endif
            return (isOk);
        }

        static public bool Start_ST_WS()
        {
            Boolean isStarted = true;
            SquareTimeProcessingService.SquareTimeProcessingServiceClient Client_SquareTime_WS = new SquareTimeProcessingService.SquareTimeProcessingServiceClient("BasicHttpBinding_ISquareTimeProcessingService");
            String pathBoardFile = ConfigurationManager.AppSettings["PathFileBoard"].ToString().Trim();

            try
            {
                // Le WS demarre par defaut en mode suspend
                if (Client_SquareTime_WS.Start(@pathBoardFile))
                {
                    Log.LogText("Web Service Started !!" + Environment.NewLine);
                }
                else
                    Log.LogText("*** Web Service Error *** " + Environment.NewLine);
            }
            catch
            {
                Log.LogText("*** Web Service Error *** " + Environment.NewLine);
                isStarted = false;
            }

            return (isStarted);
        }

        /*********************************************************************
         * 
         *                          Function Test
         *      
         *********************************************************************/
        private void CallSquareTime_EXE()
        {
            string test = "C:\\Chess\\CoordonneesBoard.txt";

            Process proc = Process.GetCurrentProcess();
            //get all other (possible) running instances
            Process[] processes = Process.GetProcessesByName(proc.ProcessName);

            // Process[] all = Process.GetProcesses();
            Process[] SquareTimeProcess = Process.GetProcessesByName("SquareTime", "CANL-5Z48K72");

            if (SquareTimeProcess.Length == 1)
            {
                IntPtr ptrCopyData = IntPtr.Zero;

                try
                {
                    COPYDATASTRUCT copyData = new COPYDATASTRUCT();
                    copyData.dwData = new IntPtr(1); // Test
                    copyData.cbData = test.Length + 1;
                    copyData.lpData = Marshal.StringToHGlobalAnsi(test);

                    // Allocate memory for the data and copy
                    ptrCopyData = Marshal.AllocCoTaskMem(Marshal.SizeOf(copyData));
                    Marshal.StructureToPtr(copyData, ptrCopyData, false);

                    foreach (Process p in SquareTimeProcess)
                    {
                        SendMessage(p.MainWindowHandle, WM_COPYDATA, IntPtr.Zero, ptrCopyData);
                    }
                }
                catch(Exception)
                {

                }
            }
        }

        /*********************************************************************************************************
         * 
         *      Recois les Messages de SquareTime.exe
         *      
         *      Messages    WM_CREATE   --->    (Interne)           Créer échiquier
         *                                                          Demarre les Threads Blancs et Noir
         *                  WM_COPYDATA --->    (SquareTime.exe)    Message de SquareTime.exe ST_GETFIRSTHIT No de Case et TimeStamp
         *                  WM_DESTROY  --->    (Interne)           Stop les Threads
         *                  
         *********************************************************************************************************/
        protected override void WndProc(ref Message message)
        {
            switch(message.Msg)
            {
                case 1: // WM_CREATE
                    {
                        Dictionary<string, int> items = new Dictionary<string, int>();

                        Master.CreateBoard();

                        Log.Init();

                        items.Add("NO_COUP_B", 0);
                        items.Add("NO_COUP_N", 0);
                        items.Add("HUMAIN_COULEUR", K.Blanc);
                        items.Add("CONFIG_BOARD", 0);
                        items.Add("END", 0);
                        items.Add("CASE_DEPART", 0);
                        items.Add("CASE_DESTINATION", 0);

                        CallContext.LogicalSetData("_items", items);

                        Thread_Blanc = new Thread(new ThreadStart(Master.LoopBlanc));
                        Thread_Noir = new Thread(new ThreadStart(Master.LoopNoir));
                        Thread_Blanc.Start();
                        Thread_Noir.Start();

                        GlobalSMEXE.SendMsg_Call_SquareTime_EXE(GlobalParameters_SquareTime.ST_INITSTART, 0);

                        break;
                    }
                case 2: // WM_DESTROY
                    {
                        Dictionary<string, int> items = (Dictionary<string, int>)CallContext.LogicalGetData("_items");
                        items["END"] = 1;
                        Thread_Blanc.Join();
                        Thread_Noir.Join();
                        base.WndProc(ref message);
                        break;
                    }

                // Messages received from Square Time Application
                case WM_COPYDATA:
                    // Get the COPYDATASTRUCT struct from lParam.
                    {
                        COPYDATASTRUCT cds = (COPYDATASTRUCT)message.GetLParam(typeof(COPYDATASTRUCT));

                        // Marshal the data from the unmanaged memory block to a 
                        COPYDATASTRUCT copyData =
                        (COPYDATASTRUCT)Marshal.PtrToStructure
                        (message.LParam, typeof(COPYDATASTRUCT));

                        if ((int)copyData.dwData == GlobalParameters_SquareTime.ST_GETFIRSTHIT) // Message from  SquareTime App : case & time
                        {
                            GlobalParameters_SquareTime.ST_time = Marshal.PtrToStringUni(copyData.lpData);
                            GlobalParameters_SquareTime.ST_noCase = (int)copyData.dwData;
                        }
                        break;
                    }
 
                case WM_ENTERIDLE :
                    {
                        base.WndProc(ref message);
                        break;
                    }
                   
                 default:
                    {
                        base.WndProc(ref message);
                        break;
                    }                    
            }

        }

        /*************************************************************************
         * 
         *      Recois les messages du clavier
         *      
         *      Messages    Touche 1 ---> COULEUR BLANC (49)
         *                  Touche 2 ---> COULEUR NOIR  (50)
         *                  Touche 9 ---> FIN           (57)
         *                  
         *     Demarre le looping des cases de l'application SquareTime.exe 
         *     Demarre le looping des cases du Web Service
         *     ReInitialise l'echiquier   
         *                  
         *************************************************************************/
        void StartUp_KeyPress(object sender, KeyPressEventArgs e)
        {
            SquareTimeProcessingService.SquareTimeProcessingServiceClient Client_SquareTime_WS = new SquareTimeProcessingService.SquareTimeProcessingServiceClient("BasicHttpBinding_ISquareTimeProcessingService");
            Dictionary<string, int> items = (Dictionary<string, int>)CallContext.LogicalGetData("_items");
            bool boardConfigured =  items["CONFIG_BOARD"] == 1 ? true:false;

            if (e.KeyChar >= 48 && e.KeyChar <= 57)
            {
//                MessageBox.Show("Form.KeyPress: '" +
//                    e.KeyChar.ToString() + "' pressed.");

                switch (e.KeyChar)
                {
                    case (char)48:
                        break;

                    case (char)49:
                        // 1) Envoi message vers l'application EXE : Demarre des cases vides
                        if (!boardConfigured)
                        {
                            // On demarre la partie avec les Blancs (Les applications roulent deja avec les Blancs on active le looping)
                            GlobalSMEXE.SendMsg_Call_SquareTime_EXE(GlobalParameters_SquareTime.ST_STARTRUNNING, 0);
                            Client_SquareTime_WS.Suspend(false);
                        }
                        else
                        {
                            // On reset la partie avec les Blancs pour les 2 applications (redessine les 64 cases)
                            GlobalSMEXE.SendMsg_Call_SquareTime_EXE(GlobalParameters_SquareTime.ST_NEWGAME, 0);
                            Client_SquareTime_WS.NewGame(); 
                        }

                        // 3) Demarrage des thread Blanc et Noir
                        items["HUMAIN_COULEUR"] = K.Blanc;
                        Master.NewGame("Blanc");
                        items["CONFIG_BOARD"] = 1;
                        Console.Beep();
                        break;

                    case (char)50:
                        // 1) Envoi message vers l'application EXE
 
                        if (!boardConfigured)
                            GlobalSMEXE.SendMsg_Call_SquareTime_EXE(GlobalParameters_SquareTime.ST_STARTRUNNING, 0);
                        else
                            GlobalSMEXE.SendMsg_Call_SquareTime_EXE(GlobalParameters_SquareTime.ST_NEWGAME, 0);

                        // 2) Envoi message vers le Web Service
                        Client_SquareTime_WS.Suspend(false);

                        items["HUMAIN_COULEUR"] = K.Noir;
                        Master.NewGame("Noir");
                        items["CONFIG_BOARD"] = 1;

                        break;
                    case (char)52:
                    case (char)55:
                        break;

                    case (char)57: // Partie Terminée click sur la touche 9 du Pad. Arrete le WS et le EXE
                        items["END"] = 1;
                        GlobalSMEXE.SendMsg_Call_SquareTime_EXE(GlobalParameters_SquareTime.ST_STOPRUNNING, 0);
                        Client_SquareTime_WS.Suspend(true);
                        break;
                }
            }
        }

        private bool Test_WebServices(out byte WhichServiceFail)
        {
            bool isSuccess = false;
            
            WhichServiceFail = 0;

            SquareTimeProcessingService.SquareTimeProcessingServiceClient client_Square = new SquareTimeProcessingService.SquareTimeProcessingServiceClient("BasicHttpBinding_ISquareTimeProcessingService");
            WhitePiecesService.WhitePiecesClient client_WPiece = new WhitePiecesService.WhitePiecesClient("BasicHttpBinding_IWhitePieces");

            try
            {
                isSuccess = client_Square.Ping();
            }
            catch (Exception)
            {
                WhichServiceFail = 1;
                return (false);
            }

            try
            {
                isSuccess = client_WPiece.Ping();
            }
            catch (Exception)
            {
                WhichServiceFail = 2;
                return (false);
            }


            return (true);
        }
    }

    //////////////////////////////////////////////////////
    //          Envoi des messages vers SquareTime.exe
    //////////////////////////////////////////////////////
    public static class GlobalSMEXE
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int SendMessage(IntPtr hwnd, [MarshalAs(UnmanagedType.U4)] int Msg, IntPtr wParam, IntPtr lParam);

        private const int WM_COPYDATA = 0x004A;


        /*******************************************************************
         * 
         *      Envoi des messages vers le programme SquareTime.exe   
         * 
         *******************************************************************/
        public static void SendMsg_Call_SquareTime_EXE(uint msg, int noCase)
        {
            string buffer = noCase.ToString();

            Process proc = Process.GetCurrentProcess();
            //get all other (possible) running instances
            Process[] processes = Process.GetProcessesByName(proc.ProcessName);

            // Process[] all = Process.GetProcesses();
            Process[] SquareTimeProcess = Process.GetProcessesByName("SquareTime", "CANL-5Z48K72");

            if (SquareTimeProcess.Length == 1)
            {
                IntPtr ptrCopyData = IntPtr.Zero;
                COPYDATASTRUCT copyData = new COPYDATASTRUCT();
                copyData.dwData = new IntPtr(msg);

                try
                {
                    switch (msg)
                    {
                        case GlobalParameters_SquareTime.ST_STARTRUNNING:
                            string File = "C:\\Chess\\CoordonneesBoard.txt";
                            copyData.cbData = File.Length + 1;
                            copyData.lpData = Marshal.StringToHGlobalAnsi(File);

                            // Allocate memory for the data and copy
                            ptrCopyData = Marshal.AllocCoTaskMem(Marshal.SizeOf(copyData));
                            Marshal.StructureToPtr(copyData, ptrCopyData, false);
                            break;

                        case GlobalParameters_SquareTime.ST_GETFIRSTHIT:
                            copyData.cbData = buffer.Length + 1;
                            copyData.lpData = Marshal.StringToHGlobalAnsi(buffer);

                            // Allocate memory for the data and copy
                            ptrCopyData = Marshal.AllocCoTaskMem(Marshal.SizeOf(copyData));
                            Marshal.StructureToPtr(copyData, ptrCopyData, false);
                            break;

                        case GlobalParameters_SquareTime.ST_SETEMPTYCASE:
                            copyData.cbData = buffer.Length + 1; ;
                            copyData.lpData = Marshal.StringToHGlobalAnsi(buffer);

                            // Allocate memory for the data and copy
                            ptrCopyData = Marshal.AllocCoTaskMem(Marshal.SizeOf(copyData));
                            Marshal.StructureToPtr(copyData, ptrCopyData, false);
                            break;

                        case GlobalParameters_SquareTime.ST_STOPRUNNING:
                            copyData.cbData = 0;
                            copyData.lpData = ptrCopyData;

                            // Allocate memory for the data and copy
                            ptrCopyData = Marshal.AllocCoTaskMem(Marshal.SizeOf(copyData));
                            Marshal.StructureToPtr(copyData, ptrCopyData, false);
                            break;

                        case GlobalParameters_SquareTime.ST_INITSTART:
                        case GlobalParameters_SquareTime.ST_NEWGAME:
                            copyData.cbData = 0;
                            copyData.lpData = ptrCopyData;

                            // Allocate memory for the data and copy
                            ptrCopyData = Marshal.AllocCoTaskMem(Marshal.SizeOf(copyData));
                            Marshal.StructureToPtr(copyData, ptrCopyData, false);
                            break;
                    }

                    foreach (Process p in SquareTimeProcess)
                    {
                        // Remets a zero
                        GlobalParameters_SquareTime.ST_noCase = 0;
                        GlobalParameters_SquareTime.ST_time = string.Empty;

                        SendMessage(p.MainWindowHandle, WM_COPYDATA, IntPtr.Zero, ptrCopyData);
                    }

                }

                catch (Exception)
                {

                }
            }
        }

        static public void ResponseFromSquareTimeApp(int noCase, string answer)
        {
            GlobalParameters_SquareTime.ST_noCase = noCase;
            GlobalParameters_SquareTime.ST_time = answer;
        }
    }
}
