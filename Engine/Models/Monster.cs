using ReactiveUI;
using System.Collections.ObjectModel;

namespace Engine.Models;

public class Monster : ReactiveObject
{
    private int _hitPoints;

    public string Name { get; private set; }
    public string Image { get; set; }
    public int MaximumHitPoints { get; private set; }
    public int RewardExperiencePoints { get; private set; }
    public int RewardGold { get; private set; }
    public int MinimumDamage { get; set; }
    public int MaximumDamage { get; set; }
    public ObservableCollection<ItemQuantity> Inventory { get; set; }

    public int HitPoints 
    { 
        get => _hitPoints; 
        set => this.RaiseAndSetIfChanged(ref _hitPoints, value);
    }

    public Monster(string name, string image, int maximumHitPoints, int hitPoints, 
        int minimumDamage, int maximumDamage, int rewardExperiencePoints, int rewardGold)
    {
        Name = name;
        Image = string.IsNullOrEmpty(image) ? string.Empty : $"Engine/Images/Locations/{image}";
        MaximumHitPoints = maximumHitPoints;
        HitPoints = hitPoints;
        MinimumDamage = minimumDamage;
        MaximumDamage = maximumDamage;
        RewardExperiencePoints = rewardExperiencePoints;
        RewardGold = rewardGold;
        Inventory = [];
    }
}
