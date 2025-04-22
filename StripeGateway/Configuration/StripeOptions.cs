namespace StripeGateway
{
    public record StripeOptions
    {
        public required string SecretKey { get; set; }
        public string PublishableKey { get; set; }
        public string WebhookSecret { get; set; }
    }


}
