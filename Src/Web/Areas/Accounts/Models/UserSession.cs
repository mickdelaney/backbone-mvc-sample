using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MvcAndBackbone.Areas.Accounts.Models
{
    public class UserSession
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }
        public List<string> Tags { get; set; }

        public UserSession()
        {
            Tags = new List<string>();
        }
    }
}