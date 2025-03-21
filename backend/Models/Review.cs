using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }

        [Required]
        public required int UserId { get; set; } 

        [Required]
        public required int BookId { get; set; } 

        [Required]
        public required string Content { get; set; }

        // Navigation properties
        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }

        [ForeignKey(nameof(BookId))]
        public Book? Book { get; set; }
    }
}