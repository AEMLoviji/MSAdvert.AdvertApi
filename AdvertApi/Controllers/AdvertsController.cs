using AdvertApi.Models;
using AdvertApi.Models.Messages;
using AdvertApi.Services;
using Amazon.SimpleNotificationService;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AdvertApi.Controllers;

[ApiController]
[Route("api/v1/adverts")]
public class AdvertsController : ControllerBase
{
    private readonly IAdvertStorageService _advertStorageService;
    private readonly IConfiguration _configuration;

    public AdvertsController(IAdvertStorageService advertStorageService, IConfiguration configuration)
    {
        _advertStorageService = advertStorageService;
        _configuration = configuration;
    }

    [HttpPost]
    [Route("Create")]
    [ProducesResponseType(404)]
    [ProducesResponseType(201, Type = typeof(CreateAdvertResponse))]
    public async Task<IActionResult> Create(CreateAdvertRequest input)
    {
        string recordId;
        try
        {
            recordId = await _advertStorageService.AddAsync(input);
        }
        // TODO: I know exceptions should be handled in a centralized way using middleware for example
        catch (KeyNotFoundException)
        {
            return NotFound();
        }

        return Accepted(new CreateAdvertResponse() { Id = recordId });
    }

    [HttpPut]
    [Route("Confirm")]
    [ProducesResponseType(404)]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Confirm(ConfirmAdvertRequest input)
    {
        try
        {
            await _advertStorageService.ConfirmAsync(input);
            await RaiseAdvertConfirmedMessage(input);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }

        return Ok();
    }

    private async Task RaiseAdvertConfirmedMessage(ConfirmAdvertRequest input)
    {
        static string PrepareAdvertConfirmedMessage(string id, string title)
        {
            return JsonSerializer.Serialize(new AdvertConfirmedMessage()
            {
                Id = id,
                Title = title
            });
        }

        var topicArn = _configuration.GetValue<string>("TopicArn");

        var dbAdvert = await _advertStorageService.GetByIdAsync(input.Id);

        string serializedMsg = PrepareAdvertConfirmedMessage(input.Id, dbAdvert.Title);

        using var client = new AmazonSimpleNotificationServiceClient();
        await client.PublishAsync(topicArn, serializedMsg);
    }
}
