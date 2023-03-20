﻿using Microsoft.Extensions.Logging;
using OppSwap.ViewModels;
namespace OppSwap
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<MainPageViewModel>();

            builder.Services.AddSingleton<JoinPage>();
            builder.Services.AddSingleton<JoinPageViewModel>();

            builder.Services.AddSingleton<CreatePage>();
            builder.Services.AddSingleton<CreatePageViewModel>();

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<MainPageViewModel>();

            builder.Services.AddTransient<RoomDetailPage>();
            builder.Services.AddTransient<RoomDetailPageViewModel>();


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}