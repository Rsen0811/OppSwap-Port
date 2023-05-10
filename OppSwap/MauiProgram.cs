using Microsoft.Extensions.Logging;
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

            builder.Services.AddSingleton<CreatePage>();
            builder.Services.AddSingleton<CreatePageViewModel>();

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<MainPageViewModel>();

            builder.Services.AddSingleton<CreateGamePage>();
            builder.Services.AddSingleton<CreateGameViewModel>();

            builder.Services.AddTransient<RoomDetailPage>();
            builder.Services.AddTransient<RoomDetailPageViewModel>();


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}