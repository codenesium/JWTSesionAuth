using System;

namespace Codenesium.JWTSessionAuth
{
    public class Session
    {
        public string Email { get; private set; }
        public int UserId { get; private set; }
        public DateTime Date { get; private set; }
        public string RequestToken { get; private set; } //sent with the request. We authenticate on this.
        public string ResponseToken { get; private set; } //sent with the response. This is updated each request with a new date.

        public Session(DateTime date, string email, int userId, string requestToken, string responseToken)
        {
            this.Email = email;
            this.Date = date;
            this.UserId = userId;
            this.RequestToken = requestToken;
            this.ResponseToken = responseToken;
        }
    }
}