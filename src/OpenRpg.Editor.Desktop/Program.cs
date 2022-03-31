﻿using System;
using Microsoft.Extensions.DependencyInjection;
using OpenRpg.Editor.Desktop.Modules;
using OpenRpg.Editor.Web;
using Photino.Blazor;

namespace OpenRpg.Editor.Desktop;

class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        var appBuilder = PhotinoBlazorAppBuilder.CreateDefault(args);

        appBuilder.Services.AddLogging();
        DataServiceModule.Setup(appBuilder.Services);
        appBuilder.RootComponents.Add<App>("app");

        var app = appBuilder.Build();
        AppDomain.CurrentDomain.UnhandledException += (sender, error) =>
            { app.MainWindow.OpenAlertWindow("Fatal exception", error.ExceptionObject.ToString()); };

        app.MainWindow
            .SetTitle("OpenRpg - Data Editor")
            .SetSize(1920, 1080)
            .SetUseOsDefaultSize(false)
            .Load("./wwwroot/index.html");

        app.Run();
    }
}