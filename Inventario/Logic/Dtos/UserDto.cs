﻿using Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Dtos
{
    public class UserDto : Entity
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Dni { get; set; }
        public string Phone { get; set; }
        public string Mail { get; set; }
        public bool IsAdmin { get; set; }
        public bool Active { get; set; }
    }
}
