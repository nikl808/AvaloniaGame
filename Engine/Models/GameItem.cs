namespace Engine.Models;

public class GameItem : ICloneable
{
    public int ItemTypeId { get; set; }
    public int Price { get; set; }
    public string Name { get; set; }

    public GameItem(int itemTypeId, string name, int price)
    {
        ItemTypeId = itemTypeId;
        Price = price;
        Name = name;
    }

    public virtual object Clone()
    {
        return new GameItem(ItemTypeId, Name, Price);
    }
}
