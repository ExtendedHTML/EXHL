using HtmlAgilityPack;

namespace EXHL;

public class ExFileClassifier(string file)
{
    public ExFileModel GetModel()
    {
        var document = new HtmlDocument();
        document.Load(file);

        var root = document.DocumentNode;
        var pageNode = root.ChildNodes.FirstOrDefault(x => x.Name == "page");
        var layoutNode = root.ChildNodes.FirstOrDefault(x => x.Name == "layout");
        var componentNode = root.ChildNodes.FirstOrDefault(x => x.Name == "component");

        var type = ExFileType.Unknown;

        if (pageNode != null && layoutNode == null && componentNode == null)
            type = ExFileType.Page;

        if (pageNode == null && layoutNode != null && componentNode == null)
            type = ExFileType.Layout;

        if (pageNode == null && layoutNode == null && componentNode != null)
            type = ExFileType.Component;

        return new ExFileModel(type, file, document);
    }

    public static IEnumerable<ExFileModel> GetModels(IEnumerable<string> files)
    {
        return files
            .Select(file => new ExFileClassifier(file))
            .Select(classifier => classifier.GetModel())
            .Where(model => model.Type != ExFileType.Unknown)
            .ToList();
    }
}