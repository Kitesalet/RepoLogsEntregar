using log4net;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using NLog;
using Serilog;
using System.Security.Principal;
using LogManager = NLog.LogManager;

class Program
{

    private static readonly ILog Log4NetLogger = log4net.LogManager.GetLogger(typeof(Program));
    private static readonly Logger NLogLogger = LogManager.GetCurrentClassLogger();

    static void Main()
    {

        //Serilog

        //Serilog();

        //Serilog AAI

        //SerilogAAI();

        //SerilogExceptions();

        //NLog

        //NLog();


        //log4net

        //Log4net();

        //LoggerManual

        ManualLogger();


    }

    public static void Log4net()
    {

        // Diferentes log levels
        Log4NetLogger.Debug("Mensaje debug, útil para logs de info sobre debug");
        Log4NetLogger.Info("Mensaje informacional en log4net");
        Log4NetLogger.Warn("Mensaje de warning / alerta, posible problema");
        Log4NetLogger.Error("Mensaje de error");
        Log4NetLogger.Fatal("Error fatal");

        // Log con propiedades customizadas
        var customProperties = new { Environment = "Production", Version = "1.0" };

        // Logeo exc
        try
        {
            ThrowException();
        }
        catch (Exception ex)
        {
            // Log con mas detalles en base al error
            Log4NetLogger.Error("Ha ocurrido un error: {@ExceptionDetails}", ex);
        }

        Log4NetLogger.Info("Application shutdown.");


    }



    public static void NLog()
    {
        try
        {
            // Log de muchos mensajes con diferentes niveles
            NLogLogger.Trace("Para información muy detallada.");
            NLogLogger.Debug("Mensajes con información de debugeo.");
            NLogLogger.Info("Mensaje informacional");
            NLogLogger.Warn("Mensaje de alerta, posible problema");
            NLogLogger.Error("Mensaje de error");
            NLogLogger.Fatal("Mensaje crítico");

            // Logeo estructurado
            NLogLogger.Info("Structured log: {Username} se logueo.", "ImanolEchazarreta");

            // Log con propiedades custom / Tipo Anonimo
            var customProperties = new { Environment = "Production", Version = "1.0" };
            NLogLogger.Info("Propiedades custom: {@CustomProperties}", customProperties);

            // Loggeo excepciones
            try
            {
                ThrowException();
            }
            catch (Exception ex)
            {
                // Log excepción con detalles
                NLogLogger.Error(ex, "Ocurrio un error. Detalles: {@ExceptionDetails}", new { CustomDetail = "AdditionalDetail" });
            }

            NLogLogger.Fatal("Shutdown.");
        }
        finally
        {
            // Shutdown NLog.
            LogManager.Shutdown();
        }
    }


    public static void SerilogAAI()
    {

        //Se crea el logger
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .Enrich.FromLogContext()
            .WriteTo.Console() // Log a consola
            .WriteTo.File("logs.txt") //Log a un archivo
                .WriteTo.ApplicationInsights(new TelemetryConfiguration { InstrumentationKey = "x"}, TelemetryConverter.Traces) //Loggea en mi ApplicationInsights, si la InstrumentationKey es correcta
            .CreateLogger();

        Random rng = new Random();
        int i = 11;
        while(i != 10)
        {
            Log.Information($"El nuevo numero loggeado es: {i}");

            i = rng.Next(1, 20);

            Log.Debug("El numero no ha sido 10...");

            Thread.Sleep(1000); //Descanso 1 min entre logs
            
        }

    }

    public static void ManualLogger()
    {

        //Se crea un archivo para loggear mensajes

        StreamWriter file = File.AppendText("consoleLogger.txt");

        ManLog("Inicio del loggeo", file);

        try
        {

            ManLog("Se intenta tirar exception", file);

            ThrowException();

        }
        catch(Exception ex)
        {

            ManLog($"{ex.Message}", file);
        }

        ManLog("Se finaliza la aplicacion", file);

    }

    public static void ManLog(string mensaje, StreamWriter file)
    {
        //Timestamp del log
        string tiempo = DateTime.Now.ToString();

        Console.WriteLine($"{mensaje} || {tiempo}");

        file.WriteLine($"{mensaje} || {tiempo}");

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