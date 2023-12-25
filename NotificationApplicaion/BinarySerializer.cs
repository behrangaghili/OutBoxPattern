using System.Text;
using System.Text.Json;

namespace NotificationMicroservice
{
    public class BinarySerializer
    {
        public static byte[] Serilize(object value)
        {
            var content = JsonSerializer.Serialize(value);
            return Encoding.UTF8.GetBytes(content);
        }

        public static T Deserilize<T>(byte[] value)
        {
            var content = Encoding.UTF8.GetString(value);
            return JsonSerializer.Deserialize<T>(content);
        }
    }
}
