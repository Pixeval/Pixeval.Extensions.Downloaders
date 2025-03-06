// Copyright (c) Pixeval.Extensions.Downloaders.Aria2.
// Licensed under the GPL v3 License.

using Pixeval.Extensions.Downloaders.Aria2.Downloaders.Network;
using System.Text.Json;

namespace Pixeval.Extensions.Downloaders.Aria2;

public class Aria2Client : IDisposable
{
    private readonly HttpClient _httpClient = new();

    public string EndPoint
    {
        get;
        set => field = new(!EndPoint.Contains("://")
            ? "http://" + value
            : value);
    } = "http://localhost:6800";

    public string Secret { get; set; } = "";

    public string UserAgent { get; set; } = "PixivIOSApp/5.8.7 Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/133.0.0.0 Safari/537.36 Edg/133.0.0.0";

    public string Referrer { get; set; } = "https://www.pixiv.net";

    public async Task<string?> DownloadAsync(string uri, string directory, string output)
    {
        try
        {
            var parameters = new List<object>();
            if (!string.IsNullOrWhiteSpace(Secret))
                parameters.Add("token:" + Secret);
            parameters.Add(new[] { uri });
            var dictionary = new Dictionary<string, string>
            {
                ["dir"] = directory,
                ["out"] = output
            };
            if (!string.IsNullOrWhiteSpace(UserAgent))
                dictionary["user-agent"] = UserAgent;
            if (!string.IsNullOrWhiteSpace(Referrer))
                dictionary["referer"] = Referrer;
            parameters.Add(dictionary);
            var request = new Aria2Request(
                "pixevalExtensionAria2",
                "2.0",
                "aria2.addUri",
                parameters
            );

            var jsonRequest = JsonSerializer.Serialize(request, typeof(Aria2Request), Aria2RequestContext.Default);
            var pos = await _httpClient.PostAsync(
                EndPoint + "/jsonrpc",
                new StringContent(jsonRequest));
            return pos.IsSuccessStatusCode 
                ? null
                : $"{pos.StatusCode} {pos.ReasonPhrase} {await pos.Content.ReadAsStringAsync()}";
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _httpClient.Dispose();
    }
}
