using System.ComponentModel.DataAnnotations;

namespace Fcg.Users.Api.Contracts.Users;

public sealed class RegisterUserRequest
{
    [Required(AllowEmptyStrings = false)]
    public string Nome { get; init; } = string.Empty;

    [Required(AllowEmptyStrings = false)]
    public string Email { get; init; } = string.Empty;

    [Required(AllowEmptyStrings = false)]
    public string Senha { get; init; } = string.Empty;
}
