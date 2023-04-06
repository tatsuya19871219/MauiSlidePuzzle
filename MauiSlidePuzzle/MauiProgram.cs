using Microsoft.Extensions.Logging;
//using Microsoft.Maui;
//using Microsoft.Maui.Hosting;
//using Microsoft.Maui.Controls.Hosting;
//using SkiaSharp.Views.Maui.Controls.Hosting;
//using Microsoft.Maui.Controls.Compatibility.Hosting;

namespace MauiSlidePuzzle;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			//.UseSkiaSharp(true)
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
