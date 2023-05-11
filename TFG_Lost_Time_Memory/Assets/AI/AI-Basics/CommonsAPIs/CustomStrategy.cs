using System.Text.RegularExpressions;
using Newtonsoft.Json.Serialization;

public class CustomStrategy : NamingStrategy
{
    protected override string ResolvePropertyName(string name)
    {
        var result = Regex.Replace(name, "([A-Z])",
            m => (m.Index > 0 ? "_" : "") + m.Value[0].ToString().ToLowerInvariant());
        return result;
    }
}
