namespace Identity.API.Models.Dto
{
    public class UserPermissionDto
    {
        public string AppName { get; set; }
        public List<string> PermissioKeys { get; set; } = new List<string>();
    }
}
