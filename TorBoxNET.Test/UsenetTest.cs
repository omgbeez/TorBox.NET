using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TorBoxNET.Test;

public class UsenetTest
{
    private readonly TorBoxNetClient _client;

    public UsenetTest()
    {
        _client = new TorBoxNetClient();
        _client.UseApiAuthentication(Setup.API_KEY);
    }

    [Fact]
    public async Task CurrentUsenetDownloads()
    {
        var result = await _client.Usenet.GetCurrentAsync(true);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task HashInfo()
    {
        var result = await _client.Usenet.GetHashInfoAsync("54248863835323fdd692da1e2adfb151");

        Assert.NotNull(result);
    }

    [Fact]
    public async Task AddFile()
    {
        const String filePath = @"big-buck-bunny.nzb";

        var fileBytes = await File.ReadAllBytesAsync(filePath);

        var result = await _client.Usenet.AddFileAsync(fileBytes, 3, "Big Buck Bunny");

        Assert.True(result.Success);
    }

    [Fact]
    public async Task AddLink()
    {
        var link = "https://gist.github.com/sanderjo/aa1f9d4720696cc11640/raw/dd9a3e14df80353f2ad187cc5cd5f5dafe9f2d24/Big.Buck.Bunny%2520---%2520missing%2520segemnts.nzb";
        var result = await _client.Usenet.AddLinkAsync(link);

        Assert.True(result.Success);
    }

    [Fact]
    public async Task ControlUsenet()
    {
        var hash = "54248863835323fdd692da1e2adfb151";
        var action = "delete";

        var result = await _client.Usenet.ControlAsync(hash, action);

        var link = "https://gist.github.com/sanderjo/aa1f9d4720696cc11640/raw/dd9a3e14df80353f2ad187cc5cd5f5dafe9f2d24/Big.Buck.Bunny%2520---%2520missing%2520segemnts.nzb";
        await _client.Usenet.AddLinkAsync(link);

        Assert.True(result.Success);
    }

    [Fact]
    public async Task CheckAvailability()
    {
        var hash = "54248863835323fdd692da1e2adfb151";
        var result = await _client.Usenet.GetAvailabilityAsync(hash, false);

        Assert.True(result.Success);
    }

    [Fact]
    public async Task RequestDownload()
    {
        var usenet_item = await _client.Usenet.GetHashInfoAsync("54248863835323fdd692da1e2adfb151");

        var result = await _client.Usenet.RequestDownloadAsync(Convert.ToInt32(usenet_item.Id), 0, false);

        Assert.True(result.Success);
    }
}