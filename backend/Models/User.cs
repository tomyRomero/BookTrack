using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; } 

        [Required]
        [StringLength(50)]
        public required string Username { get; set; }

        [Required]
        [StringLength(255)]
        public required string PasswordHash { get; set; }

        [Required]
        public required string Salt { get; set; }

        // Navigation properties  (optional)
        public ICollection<Review>? Reviews { get; set; } // One-to-many
        public ICollection<Rating>? Ratings { get; set; } // One-to-many
        public ICollection<Book>? Books { get; set; } // One-to-many
    }
}