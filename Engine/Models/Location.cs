using Engine.Factories;

namespace Engine.Models;

public class Location
{
    public int XCoordinate { get; set; }
    public int YCoordinate { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }
    public List<Quest?> QuestsAvailableHere { get; set; } = [];
    public List<MonsterEncounter> MonstersHere { get; set; } = [];
    public Trader? TraderHere { get; set; }
    public void AddMonster(int monsterId, int chanceOfEncountering)
    {
        var existingMonster = MonstersHere.FirstOrDefault(me => me.MonsterId == monsterId);
        if (existingMonster != null)
        {
            existingMonster.ChanceOfEncountering = chanceOfEncountering;
        }
        else
        {
            MonstersHere.Add(new MonsterEncounter(monsterId, chanceOfEncountering));
        }
    }

    public Monster? GetMonster()
    {
        if(!MonstersHere.Any()) return null;
        var totalChance = MonstersHere.Sum(me => me.ChanceOfEncountering);
        var randomNumber = RandomNumberGenerator.NumberBetween(1, totalChance);
        var runningTotal = 0;
        foreach(var monsterEncounter in MonstersHere)
        {
            runningTotal += monsterEncounter.ChanceOfEncountering;
            if (runningTotal >= randomNumber)
            {
                return MonsterFactory.GetMonster(monsterEncounter.MonsterId);
            }
        }
        return MonsterFactory.GetMonster(MonstersHere.Last().MonsterId);
    }
}
