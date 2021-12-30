
using System;
using System.Collections.Generic;
// using API.Extensions; // Commented after inheriting from IdentityUser class
using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    public class AppUser : IdentityUser<int>
    {
        //// Below four properties no needed, after inheriting from IdentityUser class
        // public int  Id { get; set; }
        // public string UserName { get; set; }
        // public byte[] PasswordHash { get; set; }
        // public byte[] PasswordSalt {get; set;}

        public DateTime DateOfBirth { get; set; }
        public string KnownAs { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; } = DateTime.Now;
        public string Gender { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public ICollection<Photo> Photos { get; set; }
       
       //// We have commented below method, because causing mapping problem using AutoMapper ProjectTo method
        // public int GetAge()
        // {
        //   return DateOfBirth.CalculateAge();
        // }

        public ICollection<UserLike> LikedByUsers { get; set; }
        public ICollection<UserLike> LikedUsers { get; set; }
        public ICollection<Message> MessagesSent { get; set; }
        public ICollection<Message> MessagesReceived { get; set; }
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}