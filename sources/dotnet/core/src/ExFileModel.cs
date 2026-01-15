using HtmlAgilityPack;

namespace EXHL;

public class ExFileModel(ExFileType type, string path, HtmlDocument document)
{
    public readonly ExFileType Type = type;
    public readonly string Path = path;
    public readonly HtmlDocument Document = document;
}