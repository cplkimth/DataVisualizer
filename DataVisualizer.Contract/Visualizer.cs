#region
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.Json;
#endregion

namespace DataVisualizer.Contract;

public class Visualizer
{
    public const string Separator = "\n#Visualizer#\n";

    public static void Serialzie<T>(IEnumerable<T> list, string rootPath)
    {
        var options = new JsonSerializerOptions {WriteIndented = true, IgnoreReadOnlyProperties = true};

        List<VisualColumn> visualColumns = ExtractVisualColumns(typeof(T));
        string metaJson = JsonSerializer.Serialize(visualColumns, options);

        string dataJson = JsonSerializer.Serialize(list, options);

        string json = metaJson + Separator + dataJson;

        var directory = Path.Combine(rootPath, typeof(T).FullName!);
        if (Directory.Exists(directory) == false)
            Directory.CreateDirectory(directory);

        // var path = Path.Combine(directory, $"{DateTime.Now:yyyyMMdd_HHmmss}.json");
        var path = Path.Combine(directory, "Price.json");
        File.WriteAllText(path, json);
    }

    private static T? GetAttributeValue<T>(Type type, string memberName) where T : Attribute
    {
        var member = type.GetMember(memberName).FirstOrDefault();
        if (member == null)
            return default;

        var attribute = member.GetCustomAttribute<T>(true);
        if (attribute != null)
            return attribute;

        var metadataAttribute = type.GetCustomAttribute<MetadataTypeAttribute>(inherit: true);

        if (metadataAttribute == null)
            return default;

        type = metadataAttribute.MetadataClassType;
        member = type.GetMember(memberName).FirstOrDefault();

        if (member == null)
            return default;

        attribute = member.GetCustomAttribute<T>(inherit: true);
        return attribute;
    }

    private static List<VisualColumn> ExtractVisualColumns(Type type)
    {
        List<VisualColumn> visualColumns = new List<VisualColumn>();

        var properties = type.GetProperties();
        foreach (var property in properties)
        {
            VisualColumnAttribute? attribute = GetAttributeValue<VisualColumnAttribute>(type, property.Name);
            if (attribute == default)
                continue;

            ColumnType? columnType = GetColumnType(property.PropertyType);
            if (columnType == null)
                continue;

            VisualColumn visualColumn = new VisualColumn(property.Name, columnType.Value, attribute.Index, attribute.Format, attribute.Width);
            visualColumns.Add(visualColumn);
        }

        return visualColumns;
    }

    private static ColumnType? GetColumnType(Type propertyType)
    {
        var parsed = Enum.TryParse(typeof(ColumnType), propertyType.Name, out object? columnType);

        if (parsed)
            return (ColumnType) columnType!;
        else
            return null;
    }
}