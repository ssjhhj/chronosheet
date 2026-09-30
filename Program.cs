using System;
using System.Windows.Forms;

namespace Chronosheet
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            DbHelper.EnsureDatabase();
            Application.Run(new MainForm());
        }
    }
}
