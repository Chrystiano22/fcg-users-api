using System.ComponentModel.DataAnnotations;

namespace Fcg.Users.Api.Contracts.Users;

public sealed class ChangeUserRoleRequest
{
    [Required(AllowEmptyStrings = false)]
    public string Perfil { get; init; } = string.Empty;
}
