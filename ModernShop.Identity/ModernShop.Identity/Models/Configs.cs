namespace Identity.API.Models
{
    public class Configs
    {
        public string TokenKey { get; set; }
        public int TokenTimeOut { get; set; }
        public int RefreshTokenTimeOut { get; set; }
    }
}
