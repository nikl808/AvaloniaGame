namespace Engine.Models;

public class World
{
    private List<Location> _locations = [];
    internal void AddLocation(int xCoordinate, int yCoordinate, string name, string description, string image)
    {
        _locations.Add(new Location(){
                XCoordinate = xCoordinate,
                YCoordinate = yCoordinate,
                Name = name,
                Description = description,
                Image = string.IsNullOrEmpty(image)? string.Empty : $"Engine/Images/Locations/{image}"});
    }

    public Location? LocationAt(int? xCoordinate, int? yCoordinate)
    { 
        if(xCoordinate == null || yCoordinate == null) return null;

        foreach (var location in _locations)
        {
            if(location.XCoordinate == xCoordinate && location.YCoordinate == yCoordinate) return location;
        }
        return null;
    }
}
