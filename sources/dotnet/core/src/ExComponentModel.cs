using HtmlAgilityPack;

namespace EXHL;

public class ExComponentModel
{
    public readonly List<ExInjectModel> Injects;
    public readonly HtmlNodeCollection Body;
    public readonly string Name;

    public ExComponentModel(ExFileModel file)
    {
        var html = file.Document.DocumentNode;

        var component = html.ChildNodes.FindFirst("component");

        var name = component.GetAttributeValue("name", string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(name))
            throw new Exception($"<component> find null component name: [file: {file.Path}]");

        if (component == null)
            throw new Exception($"<component> definition not found: {file.Path}");

        Name = name;

        Body = component.ChildNodes;

        Injects = ExInjectModel.GetInjectsFromFile(file);
    }
}