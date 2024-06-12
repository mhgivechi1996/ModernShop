namespace Identity.API.Entities
{
    public class UserRefreshToken
    {
        public long Id { get; set; }
        public string RefreshToken { get; set; }
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public int RefreshTokenTime { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
