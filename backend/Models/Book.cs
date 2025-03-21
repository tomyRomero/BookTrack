using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class Book
    {
        [Key]
        public int BookId { get; set; } 

        [Required]
        public required string Title { get; set; }

        [Required]
        public required string Author { get; set; }

        [Required]
        public required int GenreId { get; set; } 

        [ForeignKey(nameof(GenreId))]
        public required Genre Genre { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))] 
        public required User User { get; set; }

        // Navigation properties (optional)
        public ICollection<Review>? Reviews { get; set; }
        public ICollection<Rating>? Ratings { get; set; }
    }
}