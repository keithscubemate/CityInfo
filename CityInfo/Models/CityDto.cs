namespace CityInfo.Models;

public class CityDto
{
  public int Id { get; init; }
  public string Name { get; set; } = string.Empty;
  public string? Description { get; set; }

  public ICollection<PointOfInterestDto> PointsOfInterest { get; set; }
    = new List<PointOfInterestDto>();

  public int NumberOfPointsOfInterest => this.PointsOfInterest.Count;
}
