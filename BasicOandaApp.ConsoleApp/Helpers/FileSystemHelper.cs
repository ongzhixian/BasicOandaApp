namespace BasicOandaApp.ConsoleApp.Helpers;

internal static class FileSystemHelper
{
    /// <summary>
    /// Get full path; creates directory if it does not exists
    /// </summary>
    /// <param name="outputDirectoryPath"></param>
    /// <returns></returns>
    internal static string GetEnsuredFullPath(string outputDirectoryPath)
    {
        var outputDirectoryFullPath = Path.GetFullPath(outputDirectoryPath);

        if (!Directory.Exists(outputDirectoryFullPath))
        {
            Directory.CreateDirectory(outputDirectoryFullPath);
        }

        return outputDirectoryFullPath;
    }
}
