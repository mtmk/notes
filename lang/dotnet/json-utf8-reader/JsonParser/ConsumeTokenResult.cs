namespace json_utf8_reader.JsonParser;

/// <summary>
/// This enum captures the tri-state return value when trying to read the
/// next JSON token.
/// </summary>
internal enum ConsumeTokenResult : byte
{
    /// <summary>
    /// Reached a valid end of token and hence no action is required.
    /// </summary>
    Success,

    /// <summary>
    /// Observed incomplete data but progressed state partially in looking ahead.
    /// Return false and roll-back to a previously saved state.
    /// </summary>
    NotEnoughDataRollBackState,

    /// <summary>
    /// Observed incomplete data but no change was made to the state.
    /// Return false, but do not roll-back anything since nothing changed.
    /// </summary>
    IncompleteNoRollBackNecessary,
}