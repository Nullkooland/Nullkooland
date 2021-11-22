using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using MudBlazor.Utilities;

namespace Nullkooland.Client.Models.Theme
{
    public class MudColorJsonConverter : JsonConverter<MudColor>
    {
        public override MudColor? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return new MudColor(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, MudColor value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
