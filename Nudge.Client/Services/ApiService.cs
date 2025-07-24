using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.JSInterop;
using Nudge.Client.Dtos;
using Nudge.Client.Models;

namespace Nudge.Client.Services;

public class ApiService
{
    private HttpClient _http;
    private IJSRuntime _js;

    public ApiService(
        HttpClient http,
        IJSRuntime js
    )
    {
        _http = http;
        _js = js;
    }

    public async Task CreateRequestAsync(CreateRequestDto createRequestDto)
    {
        using var response = await _http.PostAsJsonAsync("api/request", createRequestDto);

        if (!response.IsSuccessStatusCode)
        {
            await _js.InvokeVoidAsync("alert", "Couldn't create new request");
            return;
        }
        await _js.InvokeVoidAsync("alert", "Request created successfully");
    }

    public async Task<List<Request>?> GetAllRequestsAsync()
    {
        using var response = await _http.GetAsync("api/request");

        if (!response.IsSuccessStatusCode)
        {
            await _js.InvokeVoidAsync("alert", "No requests to be displayed");
            return null;
        }
        var requestList = await response.Content.ReadFromJsonAsync<List<Request>>();
        return requestList;
    }

    public async Task<int?> DeleteRequestAsync(int id)
    {
        using var response = await _http.DeleteAsync($"api/request/{id}");

        if (!response.IsSuccessStatusCode)
        {
            await _js.InvokeVoidAsync("alert", "No requests to be displayed");
            return null;
        }
        await _js.InvokeVoidAsync("alert", "Request Deleted Successfully");
        return id;
    }
}