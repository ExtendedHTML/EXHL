using HtmlAgilityPack;

namespace EXHL;

public class ExLayoutModel
{
    public readonly List<ExInjectModel> Injects;
    public readonly HtmlNode Node;
    public readonly string Name;

    public ExLayoutModel(ExFileModel file)
    {
        var html = file.Document.DocumentNode;

        var layout = html.ChildNodes.FindFirst("layout");

        if (layout == null)
            throw new Exception($"<layout> definition not found: {file.Path}");

        var name = layout.GetAttributeValue("name", string.Empty).Trim();

        if (string.IsNullOrWhiteSpace(name))
            throw new Exception($"<layout> find null component name: [file: {file.Path}]");

        Name = name;

        Node = layout;

        Injects = ExInjectModel.GetInjectsFromFile(file);
    }
}