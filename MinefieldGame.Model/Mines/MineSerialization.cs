using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace MinefieldGame.Model.Mines
{
    public class MineConverter : JsonConverter<Mine>
    {
        public override Mine? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
            {
                JsonElement root = doc.RootElement;

                if (root.TryGetProperty("MineType", out JsonElement typeElement))
                {
                    int type = typeElement.GetInt32();
                    MineType mine = (MineType)type;

                    switch (mine)
                    {
                        case MineType.Easy:
                            return JsonSerializer.Deserialize<EasyMine>(root.GetRawText(), options);
                        case MineType.Medium:
                            return JsonSerializer.Deserialize<MediumMine>(root.GetRawText(), options);
                        case MineType.Hard:
                            return JsonSerializer.Deserialize<HardMine>(root.GetRawText(), options);
                        default:
                            throw new NotSupportedException($"Type '{type}' is not supported.");
                    }
                }

                throw new JsonException("Missing 'MineType' property");
            }
        }

        public override void Write(Utf8JsonWriter writer, Mine value, JsonSerializerOptions options)
        {
            var type = value.GetType().Name;

            switch (value)
            {
                case EasyMine easy:
                    JsonSerializer.Serialize(writer, easy, options);
                    break;

                case MediumMine medium:
                    JsonSerializer.Serialize(writer, medium, options);
                    break;

                case HardMine hard:
                    JsonSerializer.Serialize(writer, hard, options);
                    break;

                default:
                    throw new NotSupportedException($"Type '{type}' is not supported.");
            }
        }
    }
}
