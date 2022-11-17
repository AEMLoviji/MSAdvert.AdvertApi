namespace AdvertApi.Models.Messages;

public sealed class AdvertConfirmedMessage
{
    public required string Id { get; set; }

    public required string Title { get; set; }
}
