// Copyright (c) Pixeval.Extensions.Downloaders.Aria2.
// Licensed under the GPL v3 License.

using Pixeval.Extensions.Common;
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

    public async Task DownloadAsync(
        IProgressNotifier notifier,
        string uri,
        string directory,
        string output)
    {
        try
        {
            var dictionary = new Dictionary<string, string>
            {
                ["dir"] = directory,
                ["out"] = output
            };
            if (!string.IsNullOrWhiteSpace(UserAgent))
                dictionary["user-agent"] = UserAgent;
            if (!string.IsNullOrWhiteSpace(Referrer))
                dictionary["referer"] = Referrer;
            var result = await AddUriAsync([uri], dictionary, null, Secret);
            var gid = result.Result;
            while (true)
            {
                await Task.Delay(500);
                var tell = (await GetTellStatusAsync(gid, Secret)).Result;
                switch (tell.Status)
                {
                    case TellState.Active:
                        if (tell.TotalLength is 0)
                            continue;
                        notifier.ProgressChanged(tell.CompletedLength / (double)tell.TotalLength);
                        break;
                    case TellState.Complete:
                        notifier.Completed();
                        break;
                    case TellState.Error or TellState.Removed or TellState.Stopped:
                        notifier.Aborted(new Exception($"Task {tell.Status}"));
                        break;
                    default:
                        break;
                }
            }
        }
        catch (Exception e)
        {
            notifier.Aborted(e);
        }
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _httpClient.Dispose();
    }

    private async Task<ResultCode<T>> RequestAsync<T>(
        string method,
        string? secret,
        CancellationToken token,
        params IEnumerable<object?> parameters)
    {
        if (secret is not null)
        {
            secret = "token:" + secret;
            parameters = parameters.Prepend(secret);
        }

        var request = new Aria2Request(
            "pixevalExtensionAria2",
            "2.0",
            method,
            parameters.Where(t => t is not null).ToList()!
        );

        var jsonRequest = JsonSerializer.Serialize(request, typeof(Aria2Request), Aria2RequestContext.Default);
        var pos = await _httpClient.PostAsync(
            EndPoint + "/jsonrpc",
            new StringContent(jsonRequest), token);
        if (!pos.IsSuccessStatusCode)
            throw new HttpRequestException(
                $"{pos.StatusCode} {pos.ReasonPhrase} {await pos.Content.ReadAsStringAsync(token)}");
        var result = await pos.Content.ReadAsStringAsync(token);
        return (ResultCode<T>)JsonSerializer.Deserialize(result, typeof(ResultCode<T>), Aria2RequestContext.Default)!;
    }

    /// <summary>
    /// 添加下载链接
    /// </summary>
    /// <param name="uriList">下载列表</param>
    /// <param name="options">配置</param>
    /// <param name="secret">密钥</param>
    /// <param name="location">位置</param>
    /// <param name="token">令牌</param>
    /// <returns></returns>
    public Task<ResultCode<string>> AddUriAsync(
        IEnumerable<object?>? uriList,
        IDictionary<string, string> options,
        int? location = null,
        string? secret = null,
        CancellationToken token = default) =>
        RequestAsync<string>(
            "aria2.addUri",
            secret,
            token,
            uriList,
            options, location);

    /// <summary>
    /// 根据gid查找任务
    /// </summary>
    /// <param name="gid"></param>
    /// <param name="secret"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task<ResultCode<FileDownloadTell>> GetTellStatusAsync(
        string gid,
        string? secret = null,
        CancellationToken token = default) =>
        RequestAsync<FileDownloadTell>(
            "aria2.tellStatus",
            secret,
            token, gid);

    /// <summary>
    /// 设置aria2全局设置
    /// </summary>
    /// <param name="queries"></param>
    /// <param name="secret"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task<ResultCode<string>> ChangeGlobalOptionAsync(IDictionary<string, string> queries,
        string? secret = null,
        CancellationToken token = default) =>
        RequestAsync<string>(
            "aria2.changeGlobalOption",
            secret,
            token, queries);

    /// <summary>
    /// 获得aria2全局设置
    /// </summary>
    /// <param name="secret"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task<ResultCode<object>> GetGlobalOptionAsync(
        string? secret = null,
        CancellationToken token = default) =>
        RequestAsync<object>("aria2.getGlobalOption", secret, token);

    /// <summary>
    /// 获得正在活动的任务列表
    /// </summary>
    /// <param name="secret"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task<ResultCode<List<FileDownloadTell>>> GetAllTellActiveAsync(
        string? secret = null,
        CancellationToken token = default) =>
        RequestAsync<List<FileDownloadTell>>(
            "aria2.tellActive",
            secret, token);

    /// <summary>
    /// 暂停所有任务
    /// </summary>
    /// <param name="secret"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task<ResultCode<string>> PauseAllTaskAsync(
        string? secret = null,
        CancellationToken token = default) =>
        RequestAsync<string>("aria2.pauseAll", secret, token);

    /// <summary>
    /// 暂停一个任务
    /// </summary>
    /// <param name="gid">任务gid</param>
    /// <param name="secret"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task<ResultCode<string>> PauseTaskAsync(
        string gid,
        string? secret = null,
        CancellationToken token = default) =>
        RequestAsync<string>("aria2.pause", gid, token, secret);

    /// <summary>
    /// 立即暂停一个下载
    /// </summary>
    /// <param name="gid">gid</param>
    /// <param name="secret"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task<ResultCode<string>> ForcePauseAsync(
        string gid,
        string? secret = null,
        CancellationToken token = default) =>
        RequestAsync<string>("aria2.forcePause", gid, token, secret);

    /// <summary>
    /// 立即暂停所有下载
    /// </summary>
    /// <param name="secret"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task<ResultCode<List<string>>> ForcePauseAllAsync(
        string? secret = null,
        CancellationToken token = default) =>
        RequestAsync<List<string>>("aria2.forcePauseAll", secret, token);

    /// <summary>
    /// 立即移除一个任务
    /// </summary>
    /// <param name="gid"></param>
    /// <param name="secret"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task<ResultCode<string>> ForceRemoveAsync(
        string gid,
        string? secret = null,
        CancellationToken token = default) =>
        RequestAsync<string>("aria2.forceRemove", gid, token, secret);

    /// <summary>
    /// 设置aria2全局设置
    /// </summary>
    /// <param name="type">设置枚举</param>
    /// <param name="value">值</param>
    /// <param name="secret"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task<ResultCode<string>> ChangeGlobalOptionAsync(
        string type,
        string value,
        string? secret = null,
        CancellationToken token = default) =>
        ChangeGlobalOptionAsync(
            new Dictionary<string, string>
            {
                [type] = value,
            },
            secret,
            token);

    /// <summary>
    /// 设置的aria2全局设置
    /// </summary>
    /// <param name="keyValues">多个设置项目</param>
    /// <param name="secret"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task<bool> ChangeGlobalOptionsAsync(
        Dictionary<string, string> keyValues,
        string? secret = null,
        CancellationToken token = default)
    {
        List<bool> flags = [];
        foreach (var value in keyValues)
            if (await ChangeGlobalOptionAsync(value.Key, value.Value, secret, token) is not { Result: "OK" })
                flags.Add(true);
        return !flags.Contains(true);
    }

    /// <summary>
    /// 添加BT文件下载
    /// </summary>
    /// <param name="torrentPath">BT文件位置</param>
    /// <param name="options">配置</param>
    /// <param name="location">位置</param>
    /// <param name="secret"></param>
    /// <param name="token">令牌</param>
    /// <returns></returns>
    public async Task<ResultCode<string>> AddTorrentAsync(
        string torrentPath,
        IDictionary<string, object> options,
        int? location = null,
        string? secret = null,
        CancellationToken token = default) =>
        await RequestAsync<string>(
            "aria2.addTorrent",
            secret,
            token,
            Convert.ToBase64String(await File.ReadAllBytesAsync(torrentPath, token)),
            new List<string>(),
            options, location);

    /// <summary>
    /// 恢复全部下载到活动任务
    /// </summary>
    /// <param name="secret"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task<ResultCode<string>> UnpauseAllAsync(
        string? secret = null,
        CancellationToken token = default) =>
        RequestAsync<string>("aria2.unpauseAll", secret, token);

    /// <summary>
    /// 恢复一个下载
    /// </summary>
    /// <param name="gid"></param>
    /// <param name="secret"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task<ResultCode<string>> UnpauseAsync(
        string gid,
        string? secret = null,
        CancellationToken token = default) =>
        RequestAsync<string>("aria2.unpause", gid, token, secret);

    /// <summary>
    /// 获取下载的选项
    /// </summary>
    /// <param name="gid"></param>
    /// <param name="secret"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task<ResultCode<object>> GetTellOptionAsync(
        string gid,
        string? secret = null,
        CancellationToken token = default) =>
        RequestAsync<object>("aria2.getOption", gid, token, secret);

    /// <summary>
    /// 获得整体下载速度
    /// </summary>
    /// <param name="secret"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task<ResultCode<object>> GetAllTellStatusAsync(
        string? secret = null, CancellationToken token = default) =>
        RequestAsync<object>("aria2.getGlobalStat", secret, token);

    public async Task<ResultCode<T>> MultiCallRequestAsync<T>(
        string? secret = null,
        CancellationToken token = default,
        params IEnumerable<IReadOnlyList<object>> args
    )
    {
        var request = args.Select(item => new MultiCallRequest((string)item[0], item.Skip(1)));

        var result = await RequestAsync<T>("system.multicall", secret, token, request);
        return result;
    }

    /// <summary>
    /// 获得正在等待的任务
    /// </summary>
    /// <param name="offset"></param>
    /// <param name="pageSize"></param>
    /// <param name="secret"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task<ResultCode<List<FileDownloadTell>>> GetWaitingTaskAsync(
        int offset = 0,
        int pageSize = 1000,
        string? secret = null,
        CancellationToken token = default) =>
        RequestAsync<List<FileDownloadTell>>(
            "aria2.tellWaiting",
            secret,
            token,
            offset, pageSize);

    /// <summary>
    /// 获得一个已经终止的任务
    /// </summary>
    /// <param name="offset"></param>
    /// <param name="pageSize"></param>
    /// <param name="secret"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task<ResultCode<List<FileDownloadTell>>> GetStoppedTaskAsync(
        int offset = 0,
        int pageSize = 1000,
        string? secret = null,
        CancellationToken token = default) =>
        RequestAsync<List<FileDownloadTell>>(
            "aria2.tellStopped",
            secret,
            token,
            offset, pageSize);

    public async Task<ResultCode<List<FileDownloadTell>>?> GetAllTaskAsync(
        string? secret = null,
        CancellationToken token = default) =>
        await MultiCallRequestAsync<List<FileDownloadTell>>(
            secret,
            token,
            ["aria2.tellStopped", 0, 1000],
            ["aria2.tellWaiting", 0, 1000],
            ["aria2.tellActive"]);

    /// <summary>
    /// 移除一个任务
    /// </summary>
    /// <param name="gid">gid</param>
    /// <param name="secret"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task<ResultCode<string>> RemoveTaskAsync(
        string gid,
        string? secret = null,
        CancellationToken token = default) =>
        RequestAsync<string>("aria2.remove", secret, token, gid);

    /// <summary>
    /// 移除指定任务
    /// </summary>
    /// <param name="gid"></param>
    /// <param name="secret"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task<ResultCode<string>> RemoveDownloadResultAsync(
        string gid,
        string? secret = null,
        CancellationToken token = default) =>
        RequestAsync<string>("aria2.removeDownloadResult", secret, token, gid);

    /// <summary>
    /// 移除全部的下载结果
    /// </summary>
    /// <param name="secret"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task<ResultCode<string>> RemoveAllDownloadResultAsync(
        string? secret = null,
        CancellationToken token = default) =>
        RequestAsync<string>("aria2.purgeDownloadResult", secret, token);

    /// <summary>
    /// 获得一个任务对应的全部文件
    /// </summary>
    /// <param name="gid">任务ID</param>
    /// <param name="secret"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task<ResultCode<List<object>>> GetFilesAsync(
        string gid,
        string? secret = null,
        CancellationToken token = default) =>
        RequestAsync<List<object>>("aria2.getFiles", secret, token, gid);

    /// <summary>
    /// 改变一个任务的配置设置
    /// </summary>
    /// <param name="gid"></param>
    /// <param name="values"></param>
    /// <param name="secret"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task<ResultCode<string>> ChangeTellOptionAsync(
        string gid,
        Dictionary<string, object> values,
        string? secret = null,
        CancellationToken token = default) =>
        RequestAsync<string>("aria2.changeOption", secret, token, gid, values);

    /// <summary>
    /// 获得一个Bt任务的所有Ip连接
    /// </summary>
    /// <param name="gid"></param>
    /// <param name="secret"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task<ResultCode<List<object>>> GetBittorrentPeersAsync(
        string gid,
        string? secret = null,
        CancellationToken token = default) =>
        RequestAsync<List<object>>("aria2.getPeers", secret, token, gid);
}
