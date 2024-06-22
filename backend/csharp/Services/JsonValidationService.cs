using System;
using System.Text.Json;

public class JsonValidationService
{
    public static bool IsValidJson(string strInput)
    {
        if (string.IsNullOrWhiteSpace(strInput)) return false;
        
        strInput = strInput.Trim();
        
        try
        {
            JsonDocument.Parse(strInput);
            return true;
        }
        catch (JsonException)
        {
            // Invalid JSON format
            return false;
        }
    }
}