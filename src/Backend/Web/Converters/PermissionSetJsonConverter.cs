using System.Text.Json;
using System.Text.Json.Serialization;
using Meetline.Modules.SharedKernel.Domain.Wrappers;

namespace Web.Converters;

public class PermissionSetJsonConverter : JsonConverter<PermissionSet>
{
    public override PermissionSet Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String) throw new JsonException("Expected string for PermissionSet");

        var base64String = reader.GetString();
        if (string.IsNullOrEmpty(base64String)) return PermissionSet.None;

        var bytes = Convert.FromBase64String(base64String);
        return PermissionSet.FromBytes(bytes);
    }

    public override void Write(Utf8JsonWriter writer, PermissionSet value, JsonSerializerOptions options)
    {
        var bytes = value.ToByteArray();
        writer.WriteStringValue(Convert.ToBase64String(bytes));
    }
}