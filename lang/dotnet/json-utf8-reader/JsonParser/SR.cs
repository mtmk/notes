namespace json_utf8_reader.JsonParser;

public class SR
{
    public static string MaxDepthMustBePositive = "MaxDepthMustBePositive";
    public static string CommentHandlingMustBeValid = "CommentHandlingMustBeValid";
    public static string ArrayIndexNegative = "ArrayIndexNegative";
    public static string SerializerConverterFactoryInvalidArgument = "SerializerConverterFactoryInvalidArgument: {0}";
    public static string ArrayTooSmall = "ArrayTooSmall";
    public static string CallFlushToAvoidDataLoss = "CallFlushToAvoidDataLoss: {0}";
    public static string DestinationTooShort = "DestinationTooShort";
    public static string PropertyNameTooLarge = "PropertyNameTooLarge: {0}";
    public static string ValueTooLarge = "ValueTooLarge: {0}";
    public static string SpecialNumberValuesNotSupported = "SpecialNumberValuesNotSupported";
    public static string FailedToGetLargerSpan = "FailedToGetLargerSpan";
    public static string DepthTooLarge = "DepthTooLarge: {0} {1}";
    public static string ZeroDepthAtEnd = "ZeroDepthAtEnd: {0}";
    public static string EmptyJsonIsInvalid = "EmptyJsonIsInvalid";
    public static string CannotSkip = "CannotSkip";
    public static string InvalidCast = "InvalidCast: {0} {1}";
    public static string InvalidComparison = "InvalidComparison: {0}";
    public static string JsonElementHasWrongType = "JsonElementHasWrongType: {0} {1}";
    public static string ArrayDepthTooLarge = "ArrayDepthTooLarge: {0}";
    public static string MismatchedObjectArray = "MismatchedObjectArray: {0}";
    public static string TrailingCommaNotAllowedBeforeArrayEnd = "TrailingCommaNotAllowedBeforeArrayEnd";
    public static string TrailingCommaNotAllowedBeforeObjectEnd = "TrailingCommaNotAllowedBeforeObjectEnd";
    public static string EndOfStringNotFound = "EndOfStringNotFound";
    public static string RequiredDigitNotFoundAfterSign = "RequiredDigitNotFoundAfterSign: {0}";
    public static string RequiredDigitNotFoundAfterDecimal = "RequiredDigitNotFoundAfterDecimal: {0}";
    public static string RequiredDigitNotFoundEndOfData = "RequiredDigitNotFoundEndOfData";
    public static string ExpectedEndAfterSingleJson = "ExpectedEndAfterSingleJson: {0}";
    public static string ExpectedEndOfDigitNotFound = "ExpectedEndOfDigitNotFound: {0}";
    public static string ExpectedNextDigitEValueNotFound = "ExpectedNextDigitEValueNotFound: {0}";
    public static string ExpectedSeparatorAfterPropertyNameNotFound = "ExpectedSeparatorAfterPropertyNameNotFound: {0}";
    public static string ExpectedStartOfPropertyNotFound = "ExpectedStartOfPropertyNotFound: {0}";
    public static string ExpectedStartOfPropertyOrValueNotFound = "ExpectedStartOfPropertyOrValueNotFound";
    public static string ExpectedStartOfPropertyOrValueAfterComment = "ExpectedStartOfPropertyOrValueAfterComment: {0}";
    public static string ExpectedStartOfValueNotFound = "ExpectedStartOfValueNotFound: {0}";
    public static string ExpectedValueAfterPropertyNameNotFound = "ExpectedValueAfterPropertyNameNotFound";
    public static string FoundInvalidCharacter = "FoundInvalidCharacter, {0}";
    public static string InvalidEndOfJsonNonPrimitive = "InvalidEndOfJsonNonPrimitive: {0}";
    public static string ObjectDepthTooLarge = "ObjectDepthTooLarge: {0}";
    public static string ExpectedFalse = "ExpectedFalse: {0}";
    public static string ExpectedNull = "ExpectedNull: {0}";
    public static string ExpectedTrue = "ExpectedTrue: {0}";
    public static string InvalidCharacterWithinString = "InvalidCharacterWithinString: {0}";
    public static string InvalidCharacterAfterEscapeWithinString = "InvalidCharacterAfterEscapeWithinString: {0}";
    public static string InvalidHexCharacterWithinString = "InvalidHexCharacterWithinString: {0}";
    public static string EndOfCommentNotFound = "EndOfCommentNotFound";
    public static string ExpectedJsonTokens = "ExpectedJsonTokens";
    public static string NotEnoughData = "NotEnoughData";
    public static string ExpectedOneCompleteToken = "ExpectedOneCompleteToken";
    public static string InvalidCharacterAtStartOfComment = "InvalidCharacterAtStartOfComment: {0}";
    public static string UnexpectedEndOfDataWhileReadingComment = "UnexpectedEndOfDataWhileReadingComment";
    public static string UnexpectedEndOfLineSeparator = "UnexpectedEndOfLineSeparator";
    public static string InvalidLeadingZeroInNumber = "InvalidLeadingZeroInNumber: {0}";
    public static string CannotWriteCommentWithEmbeddedDelimiter = "CannotWriteCommentWithEmbeddedDelimiter";
    public static string CannotEncodeInvalidUTF8 = "CannotEncodeInvalidUTF8: {0}";
    public static string CannotEncodeInvalidUTF16 = "CannotEncodeInvalidUTF16: {0}";
    public static string CannotReadInvalidUTF16 = "CannotReadInvalidUTF16: {0}";
    public static string CannotReadIncompleteUTF16 = "CannotReadIncompleteUTF16";
    public static string CannotTranscodeInvalidUtf8 = "CannotTranscodeInvalidUtf8";
    public static string CannotTranscodeInvalidUtf16 = "CannotTranscodeInvalidUtf16: {0}";
    public static string BufferMaximumSizeExceeded = "BufferMaximumSizeExceeded: {0}";
    public static string CannotWriteEndAfterProperty = "CannotWriteEndAfterProperty: {0}";
    public static string CannotStartObjectArrayWithoutProperty = "CannotStartObjectArrayWithoutProperty: {0}";
    public static string CannotStartObjectArrayAfterPrimitiveOrClose = "CannotStartObjectArrayAfterPrimitiveOrClose: {0}";
    public static string CannotWriteValueWithinObject = "CannotWriteValueWithinObject: {0}";
    public static string CannotWritePropertyAfterProperty = "CannotWritePropertyAfterProperty";
    public static string CannotWritePropertyWithinArray = "CannotWritePropertyWithinArray: {0}";
    public static string CannotWriteValueAfterPrimitiveOrClose = "CannotWriteValueAfterPrimitiveOrClose: {0}";
    public static string FormatByte = "FormatByte";
    public static string FormatSByte = "FormatSByte";
    public static string FormatInt16 = "FormatInt16";
    public static string FormatInt32 = "FormatInt32";
    public static string FormatInt64 = "FormatInt64";
    public static string FormatInt128 = "FormatInt128";
    public static string FormatUInt16 = "FormatUInt16";
    public static string FormatUInt32 = "FormatUInt32";
    public static string FormatUInt64 = "FormatUInt64";
    public static string FormatUInt128 = "FormatUInt128";
    public static string FormatHalf = "FormatHalf";
    public static string FormatSingle = "FormatSingle";
    public static string FormatDouble = "FormatDouble";
    public static string FormatDecimal = "FormatDecimal";
    public static string UnsupportedFormat = "UnsupportedFormat: {0}";
    public static string CannotDecodeInvalidBase64 = "CannotDecodeInvalidBase64";


    internal static string Format(string resourceFormat, params object?[] p)
    {
        return string.Format(resourceFormat, p);
    }
}