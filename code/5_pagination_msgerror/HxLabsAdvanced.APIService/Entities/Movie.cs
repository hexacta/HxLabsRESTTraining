using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HxLabsAdvanced.APIService.Entities
{
    public class Movie
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required]
        [MaxLength(50)]
        public string DirectorName { get; set; }

        [Required]
        [MaxLength(50)]
        public string DirectorLastName { get; set; }

        [Required]
        public DateTimeOffset Publish { get; set; }

        public string Summary { get; set; }

        [Required]
        [MaxLength(50)]
        public string Genre { get; set; }

        public ICollection<Actor> Actors { get; set; } = new List<Actor>();
    }
}
