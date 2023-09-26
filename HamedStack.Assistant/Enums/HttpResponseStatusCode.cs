// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedType.Global

namespace HamedStack.Assistant.Enums;

/// <summary>
/// Represents HTTP response status codes.
/// </summary>
public enum HttpResponseStatusCode
{
    /// <summary>
    /// The server has received the request headers and the client should proceed to send the request body.
    /// </summary>
    Continue = 100,

    /// <summary>
    /// The requester has asked the server to switch protocols and the server has agreed to do so.
    /// </summary>
    SwitchingProtocols = 101,

    /// <summary>
    /// A WebDAV request may contain many sub-requests involving file operations, requiring a long time to complete the request.
    /// </summary>
    ProcessingWebDAV = 102,

    /// <summary>
    /// This status code is primarily intended to be used with the Link header to allow the user agent to start preloading resources while the server is still preparing a response.
    /// </summary>
    EarlyHints = 103,

    /// <summary>
    /// The request has succeeded.
    /// </summary>
    Ok = 200,

    /// <summary>
    /// The request has been fulfilled and resulted in a new resource being created.
    /// </summary>
    Created = 201,

    /// <summary>
    /// The request has been accepted for processing, but the processing has not been completed.
    /// </summary>
    Accepted = 202,

    /// <summary>
    /// The returned meta-information is not authoritative.
    /// </summary>
    NonAuthoritativeInformation = 203,

    /// <summary>
    /// The server has fulfilled the request, but there is no new information to send back.
    /// </summary>
    NoContent = 204,

    /// <summary>
    /// The server has reset the content to be delivered to the client.
    /// </summary>
    ResetContent = 205,

    /// <summary>
    /// The server has fulfilled the partial GET request for the resource.
    /// </summary>
    PartialContent = 206,

    /// <summary>
    /// Indicates multiple status for a WebDAV operation.
    /// </summary>
    MultiStatusWebDAV = 207,

    /// <summary>
    /// The message body that follows is an XML message and can contain a number of separate response codes, depending on how many sub-requests were made.
    /// </summary>
    AlreadyReportedWebDAV = 208,

    /// <summary>
    /// The server has fulfilled a request for the resource, and the response is a representation of the result of one or more instance-manipulations applied to the current instance.
    /// </summary>
    IMUsed = 226,

    /// <summary>
    /// Indicates multiple options for the resource from which the client may choose.
    /// </summary>
    MultipleChoice = 300,

    /// <summary>
    /// This and all future requests should be directed to the given URI.
    /// </summary>
    MovedPermanently = 301,

    /// <summary>
    /// The resource can be found at the different URI.
    /// </summary>
    Found = 302,

    /// <summary>
    /// The response to the request can be found under another URI using the GET method.
    /// </summary>
    SeeOther = 303,

    /// <summary>
    /// Indicates that the resource has not been modified since the version specified by the request headers.
    /// </summary>
    NotModified = 304,

    /// <summary>
    /// The requested resource must be accessed through the proxy given by the Location field.
    /// </summary>
    UseProxy = 305,

    /// <summary>
    /// The requested resource resides temporarily under a different URI.
    /// </summary>
    TemporaryRedirect = 307,

    /// <summary>
    /// The target resource has been assigned a new permanent URI and any future references to this resource should be done using one of the enclosed URIs.
    /// </summary>
    PermanentRedirect = 308,

    /// <summary>
    /// The server could not understand the request due to invalid syntax.
    /// </summary>
    BadRequest = 400,

    /// <summary>
    /// The request has not been applied because it lacks valid authentication credentials.
    /// </summary>
    Unauthorized = 401,

    /// <summary>
    /// Reserved for future use.
    /// </summary>
    PaymentRequired = 402,

    /// <summary>
    /// The server understood the request but refuses to authorize it.
    /// </summary>
    Forbidden = 403,

    /// <summary>
    /// The server has not found anything matching the requested URI.
    /// </summary>
    NotFound = 404,

    /// <summary>
    /// The method specified in the request is not allowed for the resource identified by the request URI.
    /// </summary>
    MethodNotAllowed = 405,

    /// <summary>
    /// The client has indicated with Accept headers that it will not accept any media type that the server can produce.
    /// </summary>
    NotAcceptable = 406,

    /// <summary>
    /// Similar to 401 (Unauthorized), but indicates that the client needs to authenticate to the proxy before the requested resource can be returned.
    /// </summary>
    ProxyAuthenticationRequired = 407,

    /// <summary>
    /// The server timed out waiting for the request.
    /// </summary>
    RequestTimeout = 408,

    /// <summary>
    /// Indicates that the request could not be processed because of conflict in the request.
    /// </summary>
    Conflict = 409,

    /// <summary>
    /// The requested resource is no longer available at the server and no forwarding address is known.
    /// </summary>
    Gone = 410,

    /// <summary>
    /// The server refuses to accept the request without a defined Content-Length.
    /// </summary>
    LengthRequired = 411,

    /// <summary>
    /// The precondition given in one or more of the request-header fields evaluated to false when it was tested on the server.
    /// </summary>
    PreconditionFailed = 412,

    /// <summary>
    /// The server is refusing to process a request because the request payload is too large.
    /// </summary>
    PayloadTooLarge = 413,

    /// <summary>
    /// The server is refusing to service the request because the request-target is longer than the server is willing to interpret.
    /// </summary>
    URITooLong = 414,

    /// <summary>
    /// The server is refusing to service the request because the entity of the request is in a format not supported by the requested resource for the requested method.
    /// </summary>
    UnsupportedMediaType = 415,

    /// <summary>
    /// The client has asked for a portion of the file (byte serving), but the server cannot supply that portion.
    /// </summary>
    RangeNotSatisfiable = 416,

    /// <summary>
    /// The expectation given in the request's Expect header field could not be met by at least one of the inbound servers.
    /// </summary>
    ExpectationFailed = 417,

    /// <summary>
    /// This code was defined in 1998 as one of the traditional IETF April Fools' jokes, in RFC 2324.
    /// </summary>
    ImATeapot = 418,

    /// <summary>
    /// The request was directed at a server that is not able to produce a response (for example because a connection reuse).
    /// </summary>
    MisdirectedRequest = 421,

    /// <summary>
    /// The request was well-formed but was unable to be followed due to semantic errors.
    /// </summary>
    UnprocessableEntityWebDAV = 422,

    /// <summary>
    /// The resource that is being accessed is locked.
    /// </summary>
    LockedWebDAV = 423,

    /// <summary>
    /// The method could not be performed on the resource because the requested action depended on another action and that action failed.
    /// </summary>
    FailedDependencyWebDAV = 424,

    /// <summary>
    /// Indicates that the server is unwilling to risk processing a request that might be replayed.
    /// </summary>
    TooEarly = 425,

    /// <summary>
    /// The client should switch to a different protocol given in the Upgrade header field.
    /// </summary>
    UpgradeRequired = 426,

    /// <summary>
    /// The server requires the request to be conditional.
    /// </summary>
    PreconditionRequired = 428,

    /// <summary>
    /// The user has sent too many requests in a given amount of time ("rate limiting").
    /// </summary>
    TooManyRequests = 429,

    /// <summary>
    /// Indicates that the server is unwilling to process the request because its header fields are too large.
    /// </summary>
    RequestHeaderFieldsTooLarge = 431,

    /// <summary>
    /// A server operator has received a legal demand to deny access to a resource or to a set of resources that includes the requested resource.
    /// </summary>
    UnavailableForLegalReasons = 451,

    /// <summary>
    /// A generic error message, given when an unexpected condition was encountered and no more specific message is suitable.
    /// </summary>
    InternalServerError = 500,

    /// <summary>
    /// The server does not support, or refuses to support, the major version of HTTP that was used in the request.
    /// </summary>
    NotImplemented = 501,

    /// <summary>
    /// The server, while acting as a gateway or proxy, received an invalid response from the upstream server it accessed in attempting to fulfill the request.
    /// </summary>
    BadGateway = 502,

    /// <summary>
    /// The server is currently unable to handle the request due to temporary overloading or maintenance of the server.
    /// </summary>
    ServiceUnavailable = 503,

    /// <summary>
    /// The server, while acting as a gateway or proxy, did not receive a timely response from the upstream server or some other auxiliary server it needed to access in order to complete the request.
    /// </summary>
    GatewayTimeout = 504,

    /// <summary>
    /// The server does not support the HTTP protocol version used in the request.
    /// </summary>
    HTTPVersionNotSupported = 505,

    /// <summary>
    /// Transparent content negotiation for the request results in a circular reference.
    /// </summary>
    VariantAlsoNegotiates = 506,

    /// <summary>
    /// The server is unable to store the representation needed to complete the request.
    /// </summary>
    InsufficientStorageWebDAV = 507,

    /// <summary>
    /// The server has detected an infinite loop while processing a request with "Depth: infinity".
    /// </summary>
    LoopDetectedWebDAV = 508,

    /// <summary>
    /// Further extensions to the request are required for the server to fulfill it.
    /// </summary>
    NotExtended = 510,

    /// <summary>
    /// The client needs to authenticate to gain network access.
    /// </summary>
    NetworkAuthenticationRequired = 511
}
