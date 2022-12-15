using Microsoft.AspNetCore;

namespace SeaweedChat
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) => WebHost
            .CreateDefaultBuilder(args)
            .UseContentRoot(System.IO.Directory.GetCurrentDirectory())
            .UseKestrel()
            .UseStartup<Startup>();
    }
}
