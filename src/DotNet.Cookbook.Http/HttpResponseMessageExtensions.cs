using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace DotNet.Cookbook.Http
{
    public static class HttpResponseMessageExtensions
    {
        /// <summary>
        /// Adds response's content to the message of <see cref="HttpRequestException"/> for InternalServerError http status code if any.
        /// </summary>
        /// <exception cref="HttpRequestException">When <see cref="HttpResponseMessage.StatusCode"/> is not successful.</exception>
        public static async Task<HttpResponseMessage> EnsureSuccessStatusCodeAsync(this HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.InternalServerError &&
                response.Content.Headers.ContentLength > 0)
            {
                var serviceMessage = await response.Content
                    .ReadAsStringAsync()
                    .ConfigureAwait(false);

                if (!string.IsNullOrEmpty(serviceMessage))
                {
                    throw new HttpRequestException(string.Format(
                        System.Globalization.CultureInfo.InvariantCulture,
                        "Response status code does not indicate success: {0} ({1}).{2}",
                        (int)response.StatusCode,
                        response.ReasonPhrase,
                        serviceMessage));
                }
            }

            return response
                .EnsureSuccessStatusCode();
        }
    }
}
