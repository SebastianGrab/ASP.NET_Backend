using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

public class TemplateValidationService
{
    public static bool IsValidTamplate(string TemplateContent)
    {
        if (string.IsNullOrEmpty(TemplateContent))
        {
            return false;
        }

        try
        {
            JObject json = JObject.Parse(TemplateContent);

            // Check for "Name"
            if (json["Name"] == null)
            {
                return false;
            }

            // Check for "Schema"
            JArray schema = (JArray)json["Schema"];
            if (schema == null)
            {
                return false;
            }

            // Check each entry in the "Schema" array
            foreach (JObject item in schema)
            {
                if (item["Kategorie"] == null)
                {
                    return false;
                }
                if (item["ID"] == null)
                {
                    return false;
                }
                if (item["Inputs"] == null)
                {
                    return false;
                }
            }
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }
}