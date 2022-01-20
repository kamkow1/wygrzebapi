namespace wygrzebapi.Email
{
    public interface IEmailService
    {
        public void Send(string to, string subject, string email, string password, string body);
    }
}
