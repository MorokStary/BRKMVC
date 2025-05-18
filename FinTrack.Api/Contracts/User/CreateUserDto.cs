namespace FinTrack.Api.Contracts.User
{
    public class CreateUserDto
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }

    public class ReadUserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
