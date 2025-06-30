namespace Engine.Models;

public class Weapon : GameItem
{
    public int MinimumDamage { get; set; }
    public int MaximumDamage { get; set; }

    public Weapon(int itemTypeId, string name, int price, int minDamage, int maxDamage) 
        : base(itemTypeId, name, price)
    {
        MinimumDamage = minDamage;
        MaximumDamage = maxDamage;
    }

    public override Weapon Clone()
    {
        return new Weapon(ItemTypeId, Name, Price, MinimumDamage, MaximumDamage);
    }
}
