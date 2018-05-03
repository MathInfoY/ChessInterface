using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InterfaceChess
{
    public class Program : System.Windows.Forms.Form
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            StartUp objectStartUp = new StartUp();

            try 
            {
                Application.Run(objectStartUp);            
            }
            catch(Exception)
            { }
            
        }

 
    }
}
