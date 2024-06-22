using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using csharp.Interfaces;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Repository;

public class ProtocolValidationService
{
    public static bool IsValidProtocol(List<string> templateVersions, string ProtocolContent)
    {
        var protocolIds = ExtractIds(ProtocolContent);

        if (protocolIds == null)
        {
            return false;
        }

        foreach (var templateVersion in templateVersions)
        {
            var templateIds = ExtractIds(templateVersion);

            if (protocolIds.SetEquals(templateIds))
            {
                return true;
            }
        }
        return false;
    }

    private static HashSet<string> ExtractIds(string json)
    {
        var jObject = JObject.Parse(json);
        try
        {
            var ids = jObject["Schema"]
                    .Select(token => (string)token["ID"])
                    .ToHashSet();
            return ids;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}