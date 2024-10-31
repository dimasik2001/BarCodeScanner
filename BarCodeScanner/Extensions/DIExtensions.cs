using BarCodeScanner.Helpers;
using BarCodeScanner.Services;
using BarCodeScanner.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ZXing.Net.Maui.Controls;
using ZXing.Net.Maui.Readers;
using ZXing.Net.Maui;

namespace BarCodeScanner.Extensions
{
    public static class DIExtensions
    {

        public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder mauiAppBuilder)
        {
            var baseViewModelType = typeof(ViewModels.Base.BaseViewModel);
            var assembly = baseViewModelType.Assembly;
            var viewModelsTypes = assembly.GetExportedTypes().Where(t => t.BaseType == baseViewModelType);
            if (viewModelsTypes.Any())
            {
                foreach (var type in viewModelsTypes)
                {
                    mauiAppBuilder.Services.AddSingleton(type);
                }
            }


            return mauiAppBuilder;
        }

        public static MauiAppBuilder RegisterPages(this MauiAppBuilder mauiAppBuilder)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            var baseContentPageType = typeof(ContentPage);
            var pageTypes = assembly.GetExportedTypes().Where(t => t.BaseType == baseContentPageType);
            if (pageTypes.Any())
            {
                var routeRegistrator = new RouteRegistrator();
                routeRegistrator.RegisterPagesRoutes(pageTypes);
                foreach (var type in pageTypes)
                {
                    mauiAppBuilder.Services.AddSingleton(type);
                }
            }
            return mauiAppBuilder;
        }
        public static MauiAppBuilder RegisterServices(this MauiAppBuilder mauiAppBuilder)
        {

            mauiAppBuilder.Services
                .AddSingleton<IGoogleAuthService, GoogleAuthService>()
                .AddKeyedSingleton<IStorageService, SecureStorageService>("secure")
                .AddSingleton<IStorageService, FileStorageService>()
                .AddSingleton<IBarcodeScannerService, BarcodeScannerService>()
                .AddSingleton<IMicrosoftAuthService, MicrosoftAuthService>();
            return mauiAppBuilder;
        }

        public static MauiAppBuilder UseBarcodeReader1(this MauiAppBuilder builder)
        {
            builder.ConfigureMauiHandlers(delegate (IMauiHandlersCollection handlers)
            {
                handlers.AddHandler(typeof(CameraView), typeof(CameraViewHandler));
                handlers.AddHandler(typeof(CameraBarcodeReaderView), typeof(CameraBarcodeReaderViewHandler));
                handlers.AddHandler(typeof(BarcodeGeneratorView), typeof(BarcodeGeneratorViewHandler));
            });
            builder.Services.AddTransient<IBarcodeReader, ZXingBarcodeReader>();
            return builder;
        }
    }
}
