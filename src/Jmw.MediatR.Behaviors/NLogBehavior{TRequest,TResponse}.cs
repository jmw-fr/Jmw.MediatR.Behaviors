// <copyright file="NLogBehavior{TRequest,TResponse}.cs" company="Weeger Jean-Marc">
// Copyright Weeger Jean-Marc under MIT Licence. See https://opensource.org/licenses/mit-license.php.
// </copyright>

namespace Jmw.MediatR.Behaviors
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using global::MediatR;
    using NLog;

    /// <summary>
    /// Behavior MediatR en charge de l'enregistrement des logs.
    /// </summary>
    /// <typeparam name="TRequest">Type de données de la requête.</typeparam>
    /// <typeparam name="TResponse">Type de données de la réponse.</typeparam>
    public class NLogBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        /// <inheritdoc/>
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            DateTime dtStart = DateTime.Now;
            string requestType = request.GetType().ToString();
            string requestBody = string.Empty;
            string requestAnswer = string.Empty;

            try
            {
                requestBody = Newtonsoft.Json.JsonConvert.SerializeObject(request);

                var response = await next();

                requestAnswer = Newtonsoft.Json.JsonConvert.SerializeObject(response);

                return response;
            }
            catch (Exception ex)
            {
                requestAnswer = ex.ToString();
                throw;
            }
            finally
            {
                int processedIn = (DateTime.Now - dtStart).Milliseconds;

                Logger.Info($"{requestType} in {processedIn} ms: \r\n{requestBody}\r\n{requestAnswer}.");
            }
        }
    }
}
