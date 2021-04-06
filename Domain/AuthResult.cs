using System.Collections.Generic;

namespace TodoApp.Domain
{
    public class AuthResult
    {
        //Return token
        public string Token { get; set; }
        public bool Result { get; set; }
        public List<string> Errors { get; set; }
    }
}