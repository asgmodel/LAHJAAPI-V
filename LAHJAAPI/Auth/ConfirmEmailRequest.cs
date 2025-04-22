using System.ComponentModel;

namespace AutoBuilderApiCore.Auth;

public class ConfirmEmailRequest
{
    public required string Email { get; set; }

    [DefaultValue("")]
    public string? ChangedEmail { get; set; } = string.Empty;

    [DefaultValue(false)]
    public bool IsChangedEmail { get; set; } = false;
    public required string Code { get; set; }
}
