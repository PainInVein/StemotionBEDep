using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.DTO.RequestDTOs
{
    public class CreateUserRequestDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public String? RoleName { get; set; } 
        public int? GradeLevel { get; set; }
        public string? AvatarUrl { get; set; }
        public string Status { get; set; } = "Active";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
    
    public class UpdateUserRequestDTO
    {
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        //public Guid? RoleId { get; set; }
        public int? GradeLevel { get; set; }
        public string? AvatarUrl { get; set; }
        public string? Status { get; set; }
    }
    public class ChangePasswordRequestDTO
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }

    public class LoginRequestDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
