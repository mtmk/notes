using System.Diagnostics;
using System.Text;

namespace OtelDemoCommon;

public record MessageData
{
    public string Text { get; init; }
    public List<KeyValuePair<string, string>> Headers { get; init; } = new();

    public override string ToString()
    {
        var builder = new StringBuilder();
        foreach (var keyValuePair in Headers)
        {
            builder.AppendLine($"{keyValuePair.Key}: {keyValuePair.Value}");
        }

        if (Headers.Count > 0)
            builder.AppendLine();
        
        builder.AppendLine(Text);
        
        return builder.ToString();
    }
    
    public bool TryParseTraceContext(out ActivityContext context)
    {
        DistributedContextPropagator.Current.ExtractTraceIdAndState(
            carrier: this,
            getter: static (object? carrier, string fieldName, out string? fieldValue, out IEnumerable<string>? fieldValues) =>
            {
                if (carrier is not MessageData messageData)
                {
                    Debug.Assert(false, "This should never be hit.");
                    fieldValue = null;
                    fieldValues = null;
                    return;
                }

                var values = messageData
                    .Headers
                    .Where(h => h.Key == fieldName)
                    .Select(kv => kv.Value)
                    .ToArray();
                
                if (values.Length == 0)
                {
                    fieldValue = null;
                    fieldValues = null;
                }
                else if (values.Length == 1)
                {
                    fieldValue = values[0];
                    fieldValues = null;
                }
                else
                {
                    fieldValue = null;
                    fieldValues = values;
                }
            },
            out var traceParent,
            out var traceState);

        return ActivityContext.TryParse(traceParent, traceState, out context);
    }
}
