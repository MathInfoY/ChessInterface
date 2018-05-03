using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.Remoting.Messaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace InterfaceChess
{
/*
    static class GlobalParameters_SquareTime
    {
        public const uint ST_STARTRUNNING = 1;
        public const uint ST_GETFIRSTHIT = 2;
        public const uint ST_STOPRUNNING = 3;
        public const uint ST_SETEMPTYCASE = 4;
        public const uint ST_NEWGAME = 5;

        public static int ST_noCase; // Si 0 initialisation seulement , si -1 alors il y a une piece sur la case. 
        public static string ST_time;
    }
*/
    static class GlobalParameters_GMA
    {
        public static string GMA_color;
    }
/*
    static class GlobalFile
    {
        static public string Log = "C:\\Chess\\Interface.Log";
    }
*/

/*
    // Send Messages to SquareTime application
    public static class GlobalSMEXE
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int SendMessage(IntPtr hwnd, [MarshalAs(UnmanagedType.U4)] int Msg, IntPtr wParam, IntPtr lParam);

        private const int WM_COPYDATA = 0x004A;


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
                            copyData.cbData = buffer.Length + 1;;
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
    }
*/
/*
    static public partial class Master
    {
         static public void ResponseFromSquareTimeApp(int noCase,string answer)
        {
            GlobalParameters_SquareTime.ST_noCase = noCase;
            GlobalParameters_SquareTime.ST_time = answer;
        }


    }
 */ 
}
