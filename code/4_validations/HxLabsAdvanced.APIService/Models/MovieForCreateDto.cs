using System;

namespace HxLabsAdvanced.APIService.Models
{
    //REF 4 – Modelos basicos
    public class MovieForCreateDto
    {
        public string Title { get; set; }

        public string DirectorName { get; set; }

        public string DirectorLastName { get; set; }

        public DateTimeOffset Publish { get; set; }

        public string Summary { get; set; }

        public string Genre { get; set; }
    }
}
