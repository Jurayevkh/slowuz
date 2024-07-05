using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace SpeedController;

[Route("api/[controller]/[action]")]
[ApiController]
public class SpeedController: ControllerBase
{
    private static readonly HttpClient httpClient = new HttpClient();

    [HttpGet("download")]
    public async Task<IActionResult> TestDownloadSpeed()
    {
        string fileUrl = "http://dl.google.com/googletalk/googletalk-setup.exe"; // Replace with a valid file URL
        Stopwatch stopwatch = new Stopwatch();
        
        try
        {
            stopwatch.Start();
            var response = await httpClient.GetAsync(fileUrl);
            stopwatch.Stop();
            
            response.EnsureSuccessStatusCode();

            var fileSize = response.Content.Headers.ContentLength ?? 0;
            var timeTaken = stopwatch.Elapsed.TotalSeconds;

            var speedMbps = (fileSize * 8) / (timeTaken * 1024 * 1024); // Convert to Megabits per second
            var speedMbpsInt = (int)Math.Round(speedMbps); // Convert to integer

            return Ok(new { speedMbps = speedMbpsInt });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

}
