using System;

namespace HxLabsAdvanced.APIService.Models
{
    //REF 4 – Modelos basicos
    public class MovieDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Director { get; set; }

        public int PublishYear { get; set; }

        public string Summary { get; set; }

        public string Genre { get; set; }
    }
}
