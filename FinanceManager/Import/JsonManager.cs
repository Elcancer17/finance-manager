using System.Diagnostics;
using System.IO;
using System.Text.Json.Serialization;
using System.Text.Json;
using FinanceManager.Logging;
using FinanceManager.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization.Metadata;
using System;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace FinanceManager.Import
{
    public static class JsonManager
    {
        public static TObject LoadJson<TObject>(string filename) where TObject : class, new()
        {
            if (File.Exists(filename))
            {
                Trace.WriteLine(string.Format("Loading {0}", filename), LogLevel.Information.ToString());
                using (StreamReader r = new StreamReader(filename))
                {
                    string json = r.ReadToEnd();
                    return JsonSerializer.Deserialize<TObject>(json);
                }
            }
            return new TObject();
        }

        public static void SaveToJson(string filename, List<FinancialTransaction> obj)
        {
            if (string.IsNullOrEmpty(filename) || obj == null) { return; };

            obj = obj.OrderBy(x => x.FinancialInstitution)
                     .ThenBy(x => x.AccountNumber)
                     .ThenBy(x => x.TimeStamp)
                     .ToList();
            /*
            TextEncoderSettings encoderSettings = new TextEncoderSettings();
            encoderSettings.AllowCharacters('\u0027');
            encoderSettings.AllowRange(UnicodeRanges.BasicLatin);
            JavaScriptEncoder jse1 = JavaScriptEncoder.Create(encoderSettings);

            JavaScriptEncoder jse2 = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic);
            */

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                WriteIndented = true,
                TypeInfoResolver = new OrderedPropertiesJsonTypeInfoResolver(),
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            string jsonString = JsonSerializer.Serialize(obj, options);
            File.WriteAllText(filename, jsonString);
            Trace.WriteLine(string.Format("{0} was save.", filename), LogLevel.Information.ToString());
        }

        public class OrderedPropertiesJsonTypeInfoResolver : DefaultJsonTypeInfoResolver
        {
            public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
            {
                var order = 0;
                JsonTypeInfo typeInfo = base.GetTypeInfo(type, options);
                if (typeInfo.Kind == JsonTypeInfoKind.Object)
                {
                    foreach (JsonPropertyInfo property in typeInfo.Properties.OrderBy(a => a.Name))
                    {
                        property.Order = order++;
                    }
                }
                return typeInfo;
            }
        }
    }
}
