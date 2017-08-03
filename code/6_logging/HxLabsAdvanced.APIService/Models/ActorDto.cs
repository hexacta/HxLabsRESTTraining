using System;

namespace HxLabsAdvanced.APIService.Models
{
    //REF 4 – Modelos basicos
    public class ActorDto
    {
        public Guid Id { get; set; }

        public string CompleteName { get; set; }

        public Guid MovieId { get; set; }
    }
}
