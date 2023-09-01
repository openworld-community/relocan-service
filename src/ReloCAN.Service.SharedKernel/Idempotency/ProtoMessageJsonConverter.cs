using Google.Protobuf;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ReloCAN.Service.SharedKernel.Idempotency;

internal class ProtoMessageJsonConverter : JsonConverter
{
  public override bool CanConvert(Type objectType)
  {
    return typeof(IMessage).IsAssignableFrom(objectType);
  }

  public override object ReadJson(JsonReader reader,
    Type objectType, object? existingValue,
    JsonSerializer serializer)
  {
    // The only way to find where this json object begins and ends is by
    // reading it in as a generic ExpandoObject.
    // Read an entire object from the reader.
    var converter = new ExpandoObjectConverter();
    var o = converter.ReadJson(reader, objectType, existingValue, serializer);
    // Convert it back to json text.
    var text = JsonConvert.SerializeObject(o);
    // And let protobuf's parser parse the text.
    var message = (IMessage?)Activator.CreateInstance(objectType);
    return JsonParser.Default.Parse(text, message?.Descriptor);
  }

  /// <summary>
  /// Writes the json representation of a Protocol Message.
  /// </summary>
  public override void WriteJson(JsonWriter writer, object? value,
    JsonSerializer serializer)
  {
    // Let Protobuf's JsonFormatter do all the work.
    writer.WriteRawValue(JsonFormatter.Default.Format((IMessage?)value));
  }
}
