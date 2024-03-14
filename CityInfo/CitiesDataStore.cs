using CityInfo.Models;

namespace CityInfo;

public class CitiesDataStore
{
  // public static CitiesDataStore Current { get; } = new CitiesDataStore();
  public List<CityDto> Cities { get; set; } =
  [
      new CityDto()
      {
          Id = 1,
          Name = "New York City",
          Description = "The one with the park",
          PointsOfInterest = new List<PointOfInterestDto>()
          {
              new PointOfInterestDto()
              {
                  Id = 0,
                  Name = "Empire State Building",
                  Description = "Really tall",
              },
              new PointOfInterestDto()
              {
                  Id = 1,
                  Name = "Central Park",
              },
          },
      },

      new CityDto()
      {
          Id = 2,
          Name = "Antwerp",
          Description = "the one with the cathedral",
          PointsOfInterest = new List<PointOfInterestDto>()
          {
              new PointOfInterestDto()
              {
                  Id = 2,
                  Name = "Who",
              },
              new PointOfInterestDto()
              {
                  Id = 3,
                  Name = "Cares",
              },
          },
      },

      new CityDto()
      {
          Id = 3,
          Name = "Paris",
          PointsOfInterest = new List<PointOfInterestDto>()
          {
              new PointOfInterestDto()
              {
                  Id = 4,
                  Name = "Eiffel Tower",
              },
              new PointOfInterestDto()
              {
                  Id = 5,
                  Name = "Le Louvre",
              },
          },
      },

      new CityDto()
      {
          Id = 4,
          Name = "Nashville",
          Description = "The one with the country music",
          PointsOfInterest = new List<PointOfInterestDto>()
          {
              new PointOfInterestDto()
              {
                  Id = 6,
                  Name = "TPAC",
              },
              new PointOfInterestDto()
              {
                  Id = 7,
                  Name = "Country Music Hall of fame",
              },
          },
      }

  ];
}
