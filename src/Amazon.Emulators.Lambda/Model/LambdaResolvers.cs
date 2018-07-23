using System.Threading;
using System.Threading.Tasks;
using Amazon.Lambda.Core;

namespace Amazon.Lambda.Model
{
  public delegate Task<object>  LambdaHandler(object  input, ILambdaContext context, CancellationToken cancellationToken = default);
  public delegate LambdaHandler LambdaResolver(object input, ILambdaContext context);

  public static class LambdaResolvers
  {
    // TODO: add some defaults here
  }
}