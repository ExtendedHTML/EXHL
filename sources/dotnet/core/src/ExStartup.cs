namespace EXHL;

public class ExStartup
{
    public readonly List<string> Files;
    public readonly List<ExFileModel> Models;
    public readonly List<ExPageModel> Pages;
    public readonly List<ExComponentModel> Components;
    public readonly List<ExLayoutModel> Layouts;

    public ExStartup(string basePath)
    {
        Files = new ExFileLocator(basePath).FindFiles().ToList();
        Models = ExFileClassifier.GetModels(Files).ToList();
        
        Components = Models
            .Where(file => file.Type == ExFileType.Component)
            .Select(file => new ExComponentModel(file))
            .ToList();
        
        Layouts = Models
            .Where(file => file.Type == ExFileType.Layout)
            .Select(file => new ExLayoutModel(file))
            .ToList();
        
        Pages = Models
            .Where(file => file.Type == ExFileType.Page)
            .Select(file => new ExPageModel(file))
            .ToList();
        
        Pages.ForEach(page =>
        {
            var pages = Pages
                .Where(model => page.Path.Equals(model.Path, StringComparison.OrdinalIgnoreCase))
                .Select(model => $"'{model.File.Path}'")
                .ToList();
            
            if (pages.Count > 1)
                throw new Exception(
                    $"<page> conflicts, already exist page with [path='{page.Path}'] at [files={string.Join(',', pages)}]");
        });

        Debug();
    }

    public void Debug()
    {
        Console.WriteLine($"\nFound {Models.Count} files");
        foreach (var model in Models)
        {
            Console.WriteLine($"  > @{model.Type} {model.Path}");
        }

        Console.WriteLine($"\nFound {Pages.Count}  pages");
        foreach (var page in Pages)
        {
            var render = new TxRender(this);
            var injects = string.Join(',', page.Injects.Select(x => $"('{x.Name}':'{x.Value}:{x.Optional}')"));
            Console.WriteLine($" > [path={page.Path}], [injects:{injects}], [file: {page.File.Path}]");
            Console.WriteLine($"\t {render.DrawHtmlString(page.Body)}");
        }
    }
}