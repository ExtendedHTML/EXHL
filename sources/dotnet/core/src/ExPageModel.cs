using HtmlAgilityPack;

namespace EXHL;

public class ExPageModel
{
    public readonly string Path;
    public readonly ExFileModel File;
    public readonly List<ExInjectModel> Injects;
    public readonly HtmlNodeCollection Body;

    public ExPageModel(ExFileModel file)
    {
        File = file;
        Injects = [];

        var html = file.Document.DocumentNode;

        var page = html.ChildNodes.FindFirst("page");

        if (page == null)
            throw new Exception($"<page> definition not found: {file.Path}");

        Path = page.GetAttributeValue("path", string.Empty).Trim();

        if (string.IsNullOrWhiteSpace(Path))
            throw new Exception(
                $"<page> bad definition. attribute 'path' is required, and must not be empty. [file: {file.Path}]");

        Path = Path[0] == '/' ? Path : "/" + Path;
        Path = Path[^1] == '/' ? Path : Path + "/";

        if (Path.Contains("//"))
            throw new Exception($"<page> bad path definition, must not contain '//...'. [file={file.Path}]");

        Body = page.ChildNodes;
        
        Injects = ExInjectModel.GetInjectsFromFile(file);
    }
}