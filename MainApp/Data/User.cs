using System;

namespace ProjectManager.Data
{
    public class User
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Bio { get; set; }

        public string Email { get; set; }

        public Array ProjectIDs { get; set; }
    }
}