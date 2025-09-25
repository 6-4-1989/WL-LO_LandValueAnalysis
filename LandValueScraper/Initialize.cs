using LandValueScraper.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandValueScraper
{
    public class Initialize
    {
        public static IConfiguration Configuration { get; private set; }
        public static IHttpClientFactory HttpClientFactory { get; private set; }

        public static void Init()
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            ServiceProvider serviceProvider = new ServiceCollection().AddHttpClient().BuildServiceProvider();
            HttpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
            //inject httpclient instances
            ScrapeLandValues scrapeLandValues = new ScrapeLandValues(HttpClientFactory.CreateClient(), Configuration);
        }
    }
}
