namespace EXHL;

public class ExFileLocator(string basePath)
{
    public IEnumerable<string> FindFiles()
    {
        return Directory.Exists(basePath)
            ? Directory.EnumerateFiles(basePath, "*.html", SearchOption.AllDirectories)
            : [];
    }
}