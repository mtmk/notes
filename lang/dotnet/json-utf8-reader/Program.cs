using System.Text;
using json_utf8_reader.JsonParser;

var json = """
           [{
               "name": "a",
               "id": 1
           }]
           """;

var options = new JsonReaderOptions
{
    AllowTrailingCommas = true,
    CommentHandling = JsonCommentHandling.Skip
};
var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(json), options);

while (reader.Read())
{
    Console.Write(reader.TokenType);

    switch (reader.TokenType)
    {
        case JsonTokenType.PropertyName:
        case JsonTokenType.String:
        {
            if (reader.ValueTextEquals(Encoding.UTF8.GetBytes("name")))
            {
                Console.WriteLine(">> found name");
            }
            string? text = reader.GetString();
            Console.Write(" ");
            Console.Write(text);
            break;
        }

        case JsonTokenType.Number:
        {
            int intValue = reader.GetInt32();
            Console.Write(" ");
            Console.Write(intValue);
            break;
        }

        // Other token types elided for brevity
    }
    Console.WriteLine();
}
