namespace RestApi.ViewModel;
public interface ILogService
{
    void SaveLogFile(string message);
}

public class LogService : ILogService
{
    private readonly string _logDirectoryPath;

    public LogService()
    {
        // Desktop Pfad für den Benutzer holen
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        // Logdateien Ordner erstellen (falls nicht vorhanden)
        _logDirectoryPath = Path.Combine(desktopPath, "Logdatei");
        Directory.CreateDirectory(_logDirectoryPath);
    }

    public void SaveLogFile(string message)
    {
        try
        {
            var timestamp = DateTime.Now;
            var logFilePath = Path.Combine(_logDirectoryPath, $"{timestamp:yyyy-MM-dd}.log");

            if (!File.Exists(logFilePath))
                File.Create(logFilePath).Dispose();  // Datei erstellen und sofort freigeben

            var logMessage = $"{timestamp:yyyy-MM-dd HH:mm:ss.fff} [INF] {message}";
            File.AppendAllText(logFilePath, logMessage + Environment.NewLine);
        }
        catch (Exception ex)
        {
            SaveLogFile($"Fehler beim Schreiben in die Logdatei: {ex.Message}");
        }
    }
}
