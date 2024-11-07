using System;
using System.Diagnostics;
using System.Windows.Forms;
using XmlToDB.io.form;

static class Program
{
    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new MainForm());
    }
}