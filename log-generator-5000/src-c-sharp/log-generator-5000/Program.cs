using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Templates;
using Serilog.Templates.Themes;

namespace log_generator_5000
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .WriteTo.File(new ExpressionTemplate(
                     "{ {timestamp: UtcDateTime(@t), level: @l, message: @m, exception: @x, ..@p} }\n"),
                     "logs/log-generator-5000.log",
                     rollOnFileSizeLimit: true,
                     fileSizeLimitBytes: (50 * 1024 * 1024),
                     retainedFileCountLimit: 5)
                .WriteTo.Console(new ExpressionTemplate(
                     "[{UtcDateTime(@t)} {@l:u3}] {@m}\n{@x}", theme: TemplateTheme.Code))
                .CreateLogger();

            logger.Information("Log Generator 5000 is ready to generate logs");

            var customer = new ObjectIWantLogged();
            try
            {
                throw new Exception("Well, that didn't work");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception thrown in Main() | customer name: {customerName} | customer favorite food: {favoriteFood} | customer: {@customer} | raiseTheAlarm: {raiseTheAlarm}",
                    customer.CustomerName,
                    customer.FavoriteFood,
                    customer,
                    true);
            }
        }
    }
}