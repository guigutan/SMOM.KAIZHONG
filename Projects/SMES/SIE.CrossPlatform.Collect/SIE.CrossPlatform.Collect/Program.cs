using Avalonia;
using Avalonia.Media;
using System;

namespace SIE.CrossPlatform.Collect
{
    internal class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args) => BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
        {
            FontManagerOptions options = new();
            if (OperatingSystem.IsLinux())
            {
                options.DefaultFamilyName = "国标宋体";
                //options.FontFallbacks =
                //[
                //    new FontFallback { FontFamily = "方正仿宋_GBK" }
                //];
            }
            else if (OperatingSystem.IsWindows())
            {
                options.DefaultFamilyName = "Microsoft YaHei";
            }
            return AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace()
                .With(options);

        }
    }
}
