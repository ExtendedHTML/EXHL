using System.Collections.Immutable;
using HtmlAgilityPack;

namespace EXHL;

public class TxRender(ExStartup startup)
{
    public string DrawHtmlString(HtmlNodeCollection nodes)
    {
        var repass = false;

        var document = new HtmlDocument();
        document.LoadHtml(string.Empty);

        do
        {
            HtmlNodeCollection elements;

            if (repass)
            {
                elements = document.DocumentNode.ChildNodes;

                document = new HtmlDocument();
                document.LoadHtml(string.Empty);
            }
            else
            {
                elements = nodes;
            }

            repass = false;

            foreach (var element in elements)
            {
                Solve(document.DocumentNode, element, ref repass);
            }

            if (!repass)
            {
                return document.DocumentNode.OuterHtml;
            }
            Console.WriteLine($"+++PREPASS:\n{document.DocumentNode.OuterHtml}\n---END PREPASS");
        } while (repass);

        throw new Exception();
    }

    private void Solve(HtmlNode parent, HtmlNode node, ref bool repass)
    {
        if (node.Name is "page" or "inject")
            return;
        
        if (node.Name == "component")
        {
            var name = node.GetAttributeValue("name", string.Empty);

            if (string.IsNullOrWhiteSpace(name))
                throw new Exception("<component> missing name");

            var component = startup.Components.FirstOrDefault(x => x.Name == name);

            if (component == null)
                throw new Exception($"Component not found: {name}");

            foreach (var bodyNode in component.Body)
            {
                var clone = bodyNode.CloneNode(true);
                Solve(parent, clone, ref repass);
            }

            return;
        }
        
        
        if (node.Name == "layout")
        {
            var layoutName = node.GetAttributeValue("name", string.Empty);

            if (string.IsNullOrWhiteSpace(layoutName))
                throw new Exception("<layout> missing name");

            var baseLayout = startup.Layouts.FirstOrDefault(x => x.Name == layoutName);

            if (baseLayout == null)
                throw new Exception($"Layout not found: {layoutName}");

            var layout = baseLayout.Node.CloneNode(true);

            var renders = layout.SelectNodes(".//render");

            if (renders == null || renders.Count == 0)
                throw new Exception(
                    $"<layout> definition error, <render /> tag is required exactly once: [name={baseLayout.Name}]");

            if (renders.Count > 1)
                throw new Exception(
                    $"<layout> definition error. Only one <render /> tag is allowed per layout: [found:{renders.Count}] [name={baseLayout.Name}]");

            var render = renders[0];
            var renderParent = render.ParentNode;
            var parentSnapshot = node.ChildNodes.Select(x => x.CloneNode(true)).ToList();
            
            foreach (var child in parentSnapshot.ToImmutableList())
            {
                renderParent.InsertBefore(child, render);
            }
            
            renderParent.RemoveChild(render);

            parent.AppendChildren(layout.ChildNodes);
            
            repass = true;

            return;
        }

        var current = node.CloneNode(false);
        parent.AppendChild(current);

        foreach (var child in node.ChildNodes)
        {
            Solve(current, child, ref repass);
        }
    }
}