using CityInfo.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.Controllers;

[ApiController]
[Route("/api/cities")]
public class CitiesController(CitiesDataStore citiesDataStore) : ControllerBase
{
    private readonly CitiesDataStore _citiesDataStore = citiesDataStore ?? throw new ArgumentNullException(nameof(citiesDataStore));

    [HttpGet]
    public ActionResult<IEnumerable<CityDto>> GetCities() => Ok(this._citiesDataStore.Cities);

    [HttpGet("{id:int}")]
    public ActionResult<CityDto> GetCity(int id) =>
      this._citiesDataStore.Cities.FirstOrDefault(c => c.Id == id) switch
      {
          null => NotFound(),
          var c => Ok(c),
      };
}
