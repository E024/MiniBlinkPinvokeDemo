using MiniBlinkPinvoke;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MiniBlinkPinvokeDemo
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            BlinkBrowserPInvoke.ResourceAssemblys.Add("MiniBlinkPinvokeDemo", System.Reflection.Assembly.GetExecutingAssembly());
            Application.Run(new Form1());
            ////Application.Run(new TabForm());
        }
    }
}
