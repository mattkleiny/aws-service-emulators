using System;
using System.Net;
using Amazon.Runtime;

namespace Amazon.Emulators
{
  public sealed class EmptyClientConfig : IClientConfig
  {
    public static readonly EmptyClientConfig Instance = new EmptyClientConfig();

    public RegionEndpoint   RegionEndpoint            { get; } = RegionEndpoint.APSoutheast2;
    public string           RegionEndpointServiceName { get; } = string.Empty;
    public string           ServiceURL                { get; } = string.Empty;
    public bool             UseHttp                   { get; } = true;
    public string           ServiceVersion            { get; } = string.Empty;
    public SigningAlgorithm SignatureMethod           { get; } = SigningAlgorithm.HmacSHA256;
    public string           SignatureVersion          { get; } = string.Empty;
    public string           AuthenticationRegion      { get; } = string.Empty;
    public string           AuthenticationServiceName { get; } = string.Empty;
    public string           UserAgent                 { get; } = string.Empty;
    public bool             DisableLogging            { get; } = false;
    public bool             LogMetrics                { get; } = false;
    public bool             LogResponse               { get; } = false;
    public bool             ReadEntireResponse        { get; } = false;
    public bool             AllowAutoRedirect         { get; } = false;
    public int              BufferSize                { get; } = 0;
    public int              MaxErrorRetry             { get; } = 0;
    public long             ProgressUpdateInterval    { get; } = 0;
    public bool             ResignRetries             { get; } = false;
    public ICredentials     ProxyCredentials          { get; } = CredentialCache.DefaultCredentials;
    public TimeSpan?        Timeout                   { get; } = null;
    public bool             UseDualstackEndpoint      { get; } = false;
    public bool             ThrottleRetries           { get; } = false;
    public DateTime         CorrectedUtcNow           { get; } = DateTime.UtcNow;
    public TimeSpan         ClockOffset               { get; } = TimeSpan.Zero;
    public int?             MaxConnectionsPerServer   { get; } = null;
    public bool             CacheHttpClient           { get; } = true;
    public int              HttpClientCacheSize       { get; } = 4096;
    public string           ProxyHost                 { get; } = string.Empty;
    public int              ProxyPort                 { get; } = 0;

    public string DetermineServiceURL() => ServiceURL;

    public void Validate()
    {
      // no-op
    }

    public IWebProxy GetWebProxy() => throw new NotSupportedException();
  }
}