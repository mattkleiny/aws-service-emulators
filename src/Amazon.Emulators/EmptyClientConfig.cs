using System;
using System.Net;
using Amazon.Runtime;

namespace Amazon.Emulators
{
  public sealed class EmptyClientConfig : IClientConfig
  {
    public static readonly EmptyClientConfig Instance = new();

    public RegionEndpoint   RegionEndpoint            => RegionEndpoint.APSoutheast2;
    public string           RegionEndpointServiceName => string.Empty;
    public string           ServiceURL                => string.Empty;
    public bool             UseHttp                   => true;
    public string           ServiceVersion            => string.Empty;
    public SigningAlgorithm SignatureMethod           => SigningAlgorithm.HmacSHA256;
    public string           SignatureVersion          => string.Empty;
    public string           AuthenticationRegion      => string.Empty;
    public string           AuthenticationServiceName => string.Empty;
    public string           UserAgent                 => string.Empty;
    public bool             DisableLogging            => false;
    public bool             LogMetrics                => false;
    public bool             LogResponse               => false;
    public bool             ReadEntireResponse        => false;
    public bool             AllowAutoRedirect         => false;
    public int              BufferSize                => 0;
    public int              MaxErrorRetry             => 0;
    public long             ProgressUpdateInterval    => 0;
    public bool             ResignRetries             => false;
    public ICredentials     ProxyCredentials          => CredentialCache.DefaultCredentials;
    public TimeSpan?        Timeout                   => null;
    public bool             UseDualstackEndpoint      => false;
    public bool             ThrottleRetries           => false;
    public DateTime         CorrectedUtcNow           => DateTime.UtcNow;
    public TimeSpan         ClockOffset               => TimeSpan.Zero;
    public int?             MaxConnectionsPerServer   => null;
    public bool             CacheHttpClient           => true;
    public int              HttpClientCacheSize       => 4096;
    public string           ProxyHost                 => string.Empty;
    public int              ProxyPort                 => 0;

    public string DetermineServiceURL() => ServiceURL;

    public void Validate()
    {
      // no-op
    }

    public IWebProxy GetWebProxy() => throw new NotSupportedException();
  }
}