using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.JSInterop;
using Nudge.Client.Models;

namespace Nudge.Client.Services;

public class ApiService
{
    private HttpClient _http;
    private IJSRuntime _js;

    public ApiService
    (
        HttpClient http,
        IJSRuntime js
    )
    {
        _http = http;
        _js = js;
    }

    public static async Task CreateRequest(Request req)
    {
        using var httpReq = new HttpRequestMessage(req.Method, req.Url);
        if (req.Body is not null)
        {
            httpReq.Content = JsonContent.Create(req.Body);
            httpReq.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        }

        using var response = await _http.SendAsync(httpReq);

    }
}