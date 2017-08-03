using System;
using System.Collections.Generic;
using HxLabsAdvanced.APIService.Data;
using HxLabsAdvanced.APIService.Entities;

namespace HxLabsAdvanced.APIService.Helpers
{
    public class CinemaData
    {
        private readonly CinemaContext cinemaContext;

        public CinemaData(CinemaContext cinemaContext)
        {
            this.cinemaContext = cinemaContext;
        }

        public void Seed()
        {
            this.cinemaContext.Movies.RemoveRange(this.cinemaContext.Movies);

            this.cinemaContext.SaveChanges();

            var data = this.GetMoviesData();

            this.cinemaContext.Movies.AddRange(data);

            this.cinemaContext.SaveChanges();
        }

        private IList<Movie> GetMoviesData()
        {
            var data = new List<Movie>();

            data.Add(new Movie
            {
                Id = new Guid("cde27ce7-d2c5-460c-8a25-10e31b988c4d"),
                DirectorName = "Ridley",
                DirectorLastName = "Scott",
                Title = "Alien",
                Genre = "Horror",
                Publish = new DateTime(1979, 5, 25),
                Summary = "After a space merchant vessel perceives an unknown transmission as a distress call, its landing on the source moon finds one of the crew attacked by a mysterious life-form, and they soon realize that its life cycle has merely begun.",
                Actors = new List<Actor>
                {
                    new Actor
                    {
                        Id = new Guid("c0c4bba9-feae-4b2f-8956-01c48c13a3fd"),
                        Name = " Sigourney",
                        LastName = "Weaver",
                    },
                    new Actor
                    {
                        Id = new Guid("3f46ddd6-3f9c-444f-ba75-0dbeaca2dbc6"),
                        Name = "Tom",
                        LastName = "Skerritt",
                    },
                    new Actor
                    {
                        Id = new Guid("c3994c81-2aac-4a3b-af8b-f81acaaa0b8a"),
                        Name = "John",
                        LastName = "Hurt",
                    }
                }
            });

            data.Add(new Movie
            {
                Id = new Guid("79a26b88-046a-4b80-a94b-38cb5330a280"),
                DirectorName = "John",
                DirectorLastName = "Badham",
                Title = "WarGames",
                Genre = "Thriller",
                Publish = new DateTime(1983, 5, 19),
                Summary = "A young man finds a back door into a military central computer in which reality is confused with game-playing, possibly starting World War III.",
                Actors = new List<Actor>
                {
                    new Actor
                    {
                        Id = new Guid("124a6d94-5006-4e09-8582-0e07cdc5deaf"),
                        Name = "Matthew",
                        LastName = "Broderick",
                    },
                    new Actor
                    {
                        Id = new Guid("d856d255-6627-41f3-89b8-053f294949a8"),
                        Name = "Ally",
                        LastName = "Sheedy",
                    },
                    new Actor
                    {
                        Id = new Guid("8fc2a940-b010-4ffa-a1ae-33c1aa14feab"),
                        Name = "John",
                        LastName = "Wood",
                    }
                }
            });

            data.Add(new Movie
            {
                Id = new Guid("f30ffa24-22c9-4e90-b695-fa8479d3b17d"),
                DirectorName = "Ridley",
                DirectorLastName = "Scott",
                Title = "Prometheus",
                Genre = "Adventure, Mystery",
                Publish = new DateTime(2012, 4, 11),
                Summary = "Following clues to the origin of mankind, a team finds a structure on a distant moon, but they soon realize they are not alone.",
                Actors = new List<Actor>
                {
                    new Actor
                    {
                        Id = new Guid("145084ad-bf0a-4a74-9a71-963a4c2949c1"),
                        Name = "Noomi",
                        LastName = "Rapace",
                    },
                    new Actor
                    {
                        Id = new Guid("ac754ac4-06e3-46b1-bfd6-dc4f94574e05"),
                        Name = "Logan",
                        LastName = "Marshall-Green",
                    },
                    new Actor
                    {
                        Id = new Guid("c60c9adc-5f04-4814-bc37-85c30b898b5b"),
                        Name = "Michael",
                        LastName = "Fassbender",
                    }
                }
            });

            data.Add(new Movie
            {
                Id = new Guid("35d75137-9f0d-4b99-9643-c44cb7f88281"),
                DirectorName = "John",
                DirectorLastName = "McTiernan",
                Title = "Depredador",
                Genre = " Action, Thriller",
                Publish = new DateTime(1987, 6, 12),
                Summary = "A team of commandos on a mission in a Central American jungle find themselves hunted by an extraterrestrial warrior",
                Actors = new List<Actor>
                {
                    new Actor
                    {
                        Id = new Guid("7ecad1bf-a8a9-42ae-bc55-be41520b253b"),
                        Name = " Arnold",
                        LastName = "Schwarzenegger",
                    },
                    new Actor
                    {
                        Id = new Guid("76d183d0-2299-4870-abc9-d60c6c0823c3"),
                        Name = "Kevin Peter",
                        LastName = "Hall",
                    },
                    new Actor
                    {
                        Id = new Guid("95099d8a-ad18-468d-a5bc-cee61dd549e5"),
                        Name = "Carl",
                        LastName = "Weathers",
                    }
                }
            });

            data.Add(new Movie
            {
                Id = new Guid("46e2b252-3e10-4cca-be70-069afc9fdccf"),
                DirectorName = "Ridley",
                DirectorLastName = "Scott",
                Title = "Blade Runner",
                Genre = "Thriller",
                Publish = new DateTime(1982, 6, 25),
                Summary = "A blade runner must pursue and try to terminate four replicants who stole a ship in space and have returned to Earth to find their creator.",
                Actors = new List<Actor>
                {
                    new Actor
                    {
                        Id = new Guid("8044f801-1076-4b95-a2c1-010960afcd25"),
                        Name = "Harrison",
                        LastName = "Ford",
                    },
                    new Actor
                    {
                        Id = new Guid("0553df75-22ef-4468-bafe-9bc98367bf00"),
                        Name = "Sean",
                        LastName = "Young",
                    },
                    new Actor
                    {
                        Id = new Guid("15c06856-f02b-4faa-9937-333a2ebdb964"),
                        Name = "Rutger",
                        LastName = "Hauer",
                    }
                }
            });

            data.Add(new Movie
            {
                Id = new Guid("5f690521-a6d9-4f80-90b8-8f8df326bc51"),
                DirectorName = "Irvin",
                DirectorLastName = "Kershner",
                Title = "Star Wars: Episode V - The Empire Strikes Back",
                Genre = "Action, Adventure",
                Publish = new DateTime(1980, 5, 17),
                Summary = "After the rebels have been brutally overpowered by the Empire on their newly established base, Luke Skywalker takes advanced Jedi training with Master Yoda, while his friends are pursued by Darth Vader as part of his plan to capture Luke.",
                Actors = new List<Actor>
                {
                    new Actor
                    {
                        Id = new Guid("84c6ce0b-8ecb-447c-b9a6-c45b85245c9b"),
                        Name = "",
                        LastName = "Mark Hamill",
                    },
                    new Actor
                    {
                        Id = new Guid("02d1959c-2422-4e1c-a2ad-42d017843b12"),
                        Name = "Carrie",
                        LastName = "Fisher",
                    },
                    new Actor
                    {
                        Id = new Guid("b3152f7e-abab-4a21-8337-eea1ff85895c"),
                        Name = "Harrison",
                        LastName = "Ford",
                    }
                }
            });

            return data;
        }
    }
}
