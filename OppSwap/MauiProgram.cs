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

            builder.Services.AddTransient<JoinPage>();
            builder.Services.AddTransient<JoinPageViewModel>();

            builder.Services.AddTransient<CreatePage>();
            builder.Services.AddTransient<CreatePageViewModel>();

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<MainPageViewModel>();

            builder.Services.AddTransient<CreateGamePage>();
            builder.Services.AddTransient<CreateGameViewModel>();

            builder.Services.AddTransient<RoomDetailPage>();
            builder.Services.AddTransient<RoomDetailPageViewModel>();

            builder.Services.AddTransient<FetchedGamesPage>();
            builder.Services.AddTransient<FetchedGamesPageViewModel>();

            builder.Services.AddTransient<StartGamePage>();
            builder.Services.AddTransient<StartGamePageViewModel>();

            builder.Services.AddTransient<WonGamePage>();
            builder.Services.AddTransient<WonGamePageViewModel>();

            builder.Services.AddTransient<DeadPage>();
            builder.Services.AddTransient<DeadPageViewModel>();
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}