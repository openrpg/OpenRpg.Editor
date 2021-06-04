using System;
using Photino.Blazor;

namespace OpenRpg.Editor.Desktop
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            ComponentsDesktop.Run<Startup>("OpenRpg - Data Editor", "./wwwroot/index.html");
        }
    }
}