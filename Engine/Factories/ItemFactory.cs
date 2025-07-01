using Engine.Models;

namespace Engine.Factories;

internal static class ItemFactory
{
    private static List<GameItem> _standardGameItems;
    static ItemFactory()
    {
        _standardGameItems = [ 
            new Weapon(1001, "Pointy Stick", 1,1,2),
            new Weapon(1002, "Rusty Sword", 5,1,3),
            new GameItem(9001, "Snake fang", 1),
            new GameItem(9002, "Snakeskin", 2),
            new GameItem(9003, "Rat tail", 1),
            new GameItem(9004, "Rat fur", 2),
            new GameItem(9005, "Spider fang", 1),
            new GameItem(9006, "Spider silk", 2)
        ];
    }

    public static GameItem? CreateGameItem(int itemTypeId)
    {
        var standardItem = _standardGameItems.FirstOrDefault(item => item.ItemTypeId == itemTypeId);
        if (standardItem != null) 
        {
            if(standardItem is Weapon weapon)
                return (standardItem as Weapon)?.Clone();
            
            return (GameItem)standardItem.Clone();
        }
        return null;
    }
}
