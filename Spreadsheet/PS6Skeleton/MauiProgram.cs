﻿using Microsoft.Maui.LifecycleEvents;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Maui;
using Microsoft.Maui.Controls;


namespace SpreadsheetGUI;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()

            .ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
        });

        

        builder.Services.AddTransient<MainPage>();
        return builder.Build();
    }
}