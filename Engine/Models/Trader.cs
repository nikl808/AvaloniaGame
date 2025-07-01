using System.Collections.ObjectModel;

namespace Engine.Models;

public class Trader
{
    public string Name { get; set; }
    public ObservableCollection<GameItem> Inventory { get; set; } = [];
    public Trader(string name)
    {
        Name = name;
    }

    public void AddItemToInventory(GameItem? item)
    {
        if(item is null) return;
        Inventory.Add(item);
    }

    public void RemoveItemFromInventory(GameItem? item)
    {
        if(item is null) return;
        Inventory.Remove(item);
    }
}
