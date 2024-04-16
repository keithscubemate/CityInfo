using CityInfo.Models;
using CityInfo.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.JSInterop.Implementation;

namespace CityInfo.Controllers;

[Route("api/cities/{cityId:int}/pointsofinterest")]
[ApiController]
public class PointsOfInterestController(
    ILogger<PointsOfInterestController> logger,
    IMailService mailService,
    CitiesDataStore citiesDataStore)
    : ControllerBase
{

    private readonly ILogger<PointsOfInterestController> _logger =
        logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly IMailService _mailService =
        mailService ?? throw new ArgumentNullException(nameof(mailService));
    private readonly CitiesDataStore _citiesDataStore = 
        citiesDataStore ?? throw new ArgumentNullException(nameof(citiesDataStore));

    [HttpGet]
    public ActionResult<IEnumerable<PointOfInterestDto>> GetPointsOfInterest(int cityId)
    {
        switch (this._citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId))
        {
            case null:
                this._logger.LogInformation($"City of id {cityId} not found");
                return NotFound();
            case var c:
                return Ok(c.PointsOfInterest);
        }
    }

    [HttpGet("{pointOfInterestId:int}", Name = "GetPointOfInterest")]
    public ActionResult<PointOfInterestDto> GetPointOfInterest(int cityId, int pointOfInterestId) =>
        this._citiesDataStore
            .Cities
            .FirstOrDefault(c => c.Id == cityId)?
            .PointsOfInterest
            .FirstOrDefault(poi => poi.Id == pointOfInterestId) switch
        {
            null => NotFound(),
            var p => Ok(p),
        };

    [HttpPost]
    public ActionResult<PointOfInterestDto> CreatePointOfInterest(
        int cityId,
        PointOfInterestForCreationDto pointOfInterest)
    {
        var city = this._citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
        if (city == null)
        {
            return NotFound();
        }

        var maxPointOfInterestId = this._citiesDataStore.Cities.SelectMany(c => c.PointsOfInterest).Max(p => p.Id);
        var newPointOfInterestId = maxPointOfInterestId + 1;

        var poi = new PointOfInterestDto()
        {
            Id = newPointOfInterestId,
            Name = pointOfInterest.Name,
            Description = pointOfInterest.Description
        };

        city.PointsOfInterest.Add(poi);

        return CreatedAtRoute("GetPointOfInterest",
            new
            {
                cityId,
                pointOfInterestId = poi.Id,

            }, poi);
    }

    [HttpPut("{pointOfInterestId}")]
    public ActionResult UpdatePointOfInterest(
        int cityId,
        int pointOfInterestId,
        PointOfInterestForCreationDto pointOfInterest)
    {
        var poi =
            this._citiesDataStore
            .Cities
            .FirstOrDefault(c => c.Id == cityId)?
            .PointsOfInterest
            .FirstOrDefault(p => p.Id == pointOfInterestId);
        if (poi == null)
        {
            return NotFound();
        }

        poi.Name = pointOfInterest.Name;
        poi.Description = pointOfInterest.Description;

        return NoContent();
    }

    [HttpPatch("{pointOfInterestId}")]
    public ActionResult PartiallyUpdatePointOfInterest(
        int cityId,
        int pointOfInterestId,
        JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
    {
        var poi =
            this._citiesDataStore
            .Cities
            .FirstOrDefault(c => c.Id == cityId)?
            .PointsOfInterest
            .FirstOrDefault(p => p.Id == pointOfInterestId);
        if (poi == null)
        {
            return NotFound();
        }

        var pointOfInterestForUpdate = new PointOfInterestForUpdateDto()
        {
            Name = poi.Name,
            Description = poi.Description,
        };

        patchDocument.ApplyTo(pointOfInterestForUpdate, this.ModelState);

        if (!this.ModelState.IsValid || !TryValidateModel(this.ModelState))
        {
            return BadRequest(this.ModelState);
        }

        poi.Name = pointOfInterestForUpdate.Name;
        poi.Description = pointOfInterestForUpdate.Name;

        return NoContent();
    }

    [HttpDelete("{pointOfInterestId}")]
    public ActionResult DeletePointOfInterest(
        int cityId,
        int pointOfInterestId)
    {
        var city = this._citiesDataStore
            .Cities
            .FirstOrDefault(c => c.Id == cityId);
        if (city == null)
        {
            return NotFound();
        }

        var poi = city
            .PointsOfInterest
            .FirstOrDefault(p => p.Id == pointOfInterestId);
        if (poi == null)
        {
            return NotFound();
        }

        city.PointsOfInterest.Remove(poi);

        this._mailService.Send($"POI {poi.Id} deleted", $"Point of interest {poi.Name} deleted from {city.Name}");

        return NoContent();
    }
}
