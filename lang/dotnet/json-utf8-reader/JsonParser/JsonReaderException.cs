using System.Runtime.Serialization;

namespace json_utf8_reader.JsonParser;

// This class exists because the serializer needs to catch reader-originated exceptions in order to throw JsonException which has Path information.
[Serializable]
internal sealed class JsonReaderException : JsonException
{
    public JsonReaderException(string message, long lineNumber, long bytePositionInLine) : base(message, path: null,
        lineNumber, bytePositionInLine)
    {
    }

#if NET8_0_OR_GREATER
    [Obsolete(Obsoletions.LegacyFormatterImplMessage, DiagnosticId = Obsoletions.LegacyFormatterImplDiagId,
        UrlFormat = Obsoletions.SharedUrlFormat)]
#endif
    private JsonReaderException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}