using Engine.Models;

namespace Engine.Factories;

internal static class QuestFactory
{
    private static readonly List<Quest> _quests;
    static QuestFactory()
    {
        List<ItemQuantity> itemsToComplete = [new ItemQuantity(9001,5)];
        List<ItemQuantity> rewardItems = [new ItemQuantity(1002, 1)];
        _quests = [
            new Quest(1, "Clear the herb garden", "Defeat the snake in Herbalist's garden",
            itemsToComplete, 25,10,rewardItems)
            ];
    }
    internal static Quest? GetQuestById(int id) => _quests.FirstOrDefault(quest => quest.Id == id);
}
