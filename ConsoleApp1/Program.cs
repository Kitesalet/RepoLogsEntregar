using Microsoft.Extensions.Configuration;
using NLog;
using Serilog;

class Program
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    static void Main()
    {

        //Serilog

        //Serilog();

        //NLog

        NLog();

        //log4net

        //Log4net();

    }

    public static void Log4net()
    {

        // Diferentes log levels
        Logger.Debug("Mensaje debug, útil para logs de info sobre debug");
        Logger.Info("Mensaje informacional en log4net");
        Logger.Warn("Mensaje de warning / alerta, posible problema");
        Logger.Error("Mensaje de error");
        Logger.Fatal("Error fatal");

        // Logeo estructurado
        Logger.Info("Logeo estructurado: {Username} logged in.", "ImanolEchazarreta");

        // Log con propiedades customizadas
        var customProperties = new { Environment = "Production", Version = "1.0" };
        Logger.Info("Custom Props: {@CustomProperties}", customProperties);

        // Logeo exc
        try
        {
            ThrowException();
        }
        catch (Exception ex)
        {
            // Log con mas detalles en base al error
            Logger.Error("Ha ocurrido un error: {@ExceptionDetails}", ex, new { CustomDetail = "AdditionalDetail" });
        }

        Logger.Info("Application shutdown.");

        LogManager.Flush();

    }

    public static void NLog()
    {
        try
        {
            // Log de muchos mensajes con diferentes niveles
            Logger.Trace("Para información muy detallada.");
            Logger.Debug("Mensajes con información de debugeo.");
            Logger.Info("Mensaje informacional");
            Logger.Warn("Mensaje de alerta, posible problema");
            Logger.Error("Mensaje de error");
            Logger.Fatal("Mensaje crítico");

            // Logeo estructurado
            Logger.Info("Structured log: {Username} se logueo.", "ImanolEchazarreta");

            // Log con propiedades custom / Tipo Anonimo
            var customProperties = new { Environment = "Production", Version = "1.0" };
            Logger.Info("Propiedades custom: {@CustomProperties}", customProperties);

            // Loggeo excepciones
            try
            {
                ThrowException();
            }
            catch (Exception ex)
            {
                // Log excepción con detalles
                Logger.Error(ex, "Ocurrio un error. Detalles: {@ExceptionDetails}", new { CustomDetail = "AdditionalDetail" });
            }

            Logger.Fatal("Shutdown.");
        }
        finally
        {
            // Shutdown NLog.
            LogManager.Shutdown();
        }
    }
    public static void Serilog()
    {

        var configuration = new ConfigurationBuilder()
                            .Build();


        //Se crea el logger
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .Enrich.FromLogContext()
            .WriteTo.Console() // Log a consola
            .WriteTo.File("logs.txt", rollingInterval: RollingInterval.Day) //Log a un archivo
            .CreateLogger();

        try
        {
            Log.Information("Estoy usando Serilog!");
            Log.Warning("Mensaje de Warning");

            // Logging estructurado
            Log.Information("Structured log: {Username} se ha logueado.", "ImanolEchazarreta");

            // Exception logging
            try
            {
                ThrowException();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Ocurrio un error");
            }

            Log.Fatal("La aplicación dejó de responder.");
        }
        finally
        {
            Log.CloseAndFlush(); // Close and flush the log
        }
    }

    static void ThrowException()
    {
        throw new InvalidOperationException("Error simulado");
    }
}