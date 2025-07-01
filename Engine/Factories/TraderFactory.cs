using Engine.Models;

namespace Engine.Factories;

internal static class TraderFactory
{
    private static readonly List<Trader> _traders = [];

    static TraderFactory()
    {
        var susan = new Trader("Susan");
        susan.AddItemToInventory(ItemFactory.CreateGameItem(1001));
        var farmerTed = new Trader("Farmer Ted");
        farmerTed.AddItemToInventory(ItemFactory.CreateGameItem(1001));
        var peteHerbalist = new Trader("Pete the Herbalist");
        peteHerbalist.AddItemToInventory(ItemFactory.CreateGameItem(1001));
        AddTraderToList(susan);
        AddTraderToList(farmerTed);
        AddTraderToList(peteHerbalist);
    }

    public static Trader? GetTraderByName(string name)
    {
        return _traders.FirstOrDefault(t => t.Name == name);
    }

    private static void AddTraderToList(Trader trader)
    {
        if (_traders.Any(t => t.Name == trader.Name))
        {
            throw new ArgumentException($"There is already a trader named '{trader.Name}'");
        }
        _traders.Add(trader);
    }
}
