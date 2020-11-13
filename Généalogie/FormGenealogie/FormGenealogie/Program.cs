using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace FormGenealogie
{
    static class Program
    {
        // utilisée pour avoir un numéro d'index ds la lb permettant de faire un requête
        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int[] lParam);

        private static Requetes rq = new Requetes();
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (rq.OuvreLaBase())
            {
                Application.Run(new FRM_Genealogie(rq));
            }
        }
    }
}
