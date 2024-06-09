﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.models.ViewModels
{
    public class UserInfo
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string StoreName { get; set; }
        public string RoleName { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }
    }
}
