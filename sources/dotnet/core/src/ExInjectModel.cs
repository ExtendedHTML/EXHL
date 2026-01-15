using HtmlAgilityPack;

namespace EXHL;

public class ExInjectModel(string name, string value, bool optional)
{
    public readonly string Name = name;
    public readonly string Value = value;
    public readonly bool Optional = optional;

    public static List<ExInjectModel> GetInjectsFromFile(ExFileModel file)
    {
        var injects = new List<ExInjectModel>();
        var elements = file.Document.DocumentNode.ChildNodes.Where(x => x.Name == "inject").ToList();
        var owner = file.Type.ToString().ToLower();

        foreach (var element in elements)
        {
            var name = element.GetAttributeValue("name", string.Empty).Trim();
            var @as = element.GetAttributeValue("as", string.Empty).Trim();
            var optional = false;

            var optionalAttribute = element.Attributes.FirstOrDefault(x => x.Name.Equals("optional"));

            if (optionalAttribute != null)
            {
                var value = optionalAttribute.Value.Trim().ToLower();

                if (value != string.Empty && value != "true" && value != "false" && value != "0" && value != "1")
                    throw new Exception(
                        $"<{owner}> bad <inject> value, attribute 'optional' must be 'empty value', '1', '0', 'true' or 'false'. [file: {file.Path}]");

                optional = string.IsNullOrWhiteSpace(value) || value == "true" || value == "1";
            }

            if (string.IsNullOrWhiteSpace(name))
                throw new Exception(
                    $"<{owner}> bad <inject> definition. attribute 'name' is required. [file: {file.Path}]");
            if (string.IsNullOrWhiteSpace(@as))
                throw new Exception(
                    $"<{owner}> bad <inject> definition. attribute 'as' is required. [name={name}], [file: {file.Path}]");

            if (injects.Any(x => x.Name.Equals(name, StringComparison.Ordinal)))
                throw new Exception(
                    $"<{owner}> inject already exists with same attribute 'name'. [name={name}], [file: {file.Path}]");

            if (injects.Any(x => x.Value.Equals(@as, StringComparison.Ordinal)))
                throw new Exception(
                    $"<{owner}> inject already exists with same attribute 'as'. [as={@as}], [file: {file.Path}]");

            injects.Add(new ExInjectModel(name, @as, optional));
        }

        return injects;
    }
}