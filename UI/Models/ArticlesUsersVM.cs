using ENTITIES.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UI.Models
{
    public class ArticlesUsersVM
    {
        public List<Article> Articles { get; set; }
        public List<AppUser> AppUsers { get; set; }
        public List<AppRole> Roles { get; set; }
    }
}
