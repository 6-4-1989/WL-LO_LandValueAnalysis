using Esri.ArcGISRuntime;
using Esri.ArcGISRuntime.Http;
using Esri.ArcGISRuntime.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;

namespace LandValueAnalysis.Views
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IConfiguration? Configuration { get; private set; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                //configure appsettings.json to hide api key
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                Configuration = builder.Build();

                ArcGISRuntimeEnvironment.Initialize(c => c
                    .UseApiKey(Configuration.GetConnectionString("ApiKey")
                    ?? throw new InvalidOperationException("Couldn't find connection string"))
                    .ConfigureAuthentication(x => x
                    .UseDefaultChallengeHandler())
                    .ConfigureHttp(y => y
                    .UseDefaultReferer(new Uri("https://urbanprose1212")))
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "ArcGIS Maps SDK runtime initialization failed.");
                this.Shutdown();
            }
        }
    }
}
