﻿using Microsoft.AspNetCore.Identity;

namespace PathOfServices.Business.Database
{
    public class UserEntity : IdentityUser
    {
        public string Name { get; set; }
        public string UUID { get; set; }
        public string Realm { get; set; }
    }
}
