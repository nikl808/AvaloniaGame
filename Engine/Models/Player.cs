using ReactiveUI;
using System.Collections.ObjectModel;

namespace Engine.Models;

public class Player : ReactiveObject
{
    private string? _name;
    private string? _characterClass;
    private int _hitPoints;
    private int _expPoints;
    private int _level;
    private int _gold;

    public ObservableCollection<GameItem> Inventory { get; set; } = [];
    public ObservableCollection<QuestStatus> Quests { get; set; } = [];

    public List<GameItem> Weapons => Inventory
        .Where(item => item is Weapon)
        .ToList();

    public string? Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }

    public string? CharacterClass
    {
        get => _characterClass;
        set => this.RaiseAndSetIfChanged(ref _characterClass, value);
    }

    public int HitPoints
    {
        get => _hitPoints;
        set => this.RaiseAndSetIfChanged(ref _hitPoints, value);
    }

    public int ExpPoints
    {
        get => _expPoints;
        set => this.RaiseAndSetIfChanged(ref _expPoints, value);
    }

    public int Level
    {
        get => _level;
        set => this.RaiseAndSetIfChanged(ref _level, value);
    }

    public int Gold
    {
        get => _gold;
        set => this.RaiseAndSetIfChanged(ref _gold, value);
    }

    public void AddItemToInventory(GameItem? item)
    {
        if(item is null) return;
        Inventory.Add(item);
        this.RaisePropertyChanged(nameof(Weapons));
    }

    public void RemoveItemFromInventory(GameItem? item)
    {
        if(item is null) return;
        Inventory.Remove(item);
        this.RaisePropertyChanged(nameof(Weapons));
    }

    public bool HasAllTheseItems(List<ItemQuantity> items)
    {
        foreach (var item in items) 
        {
            if(Inventory.Count(i => i.ItemTypeId == item.ItemId) < item.Quantity)
            {
                return false;
            }
        }
        return true;
    }
}