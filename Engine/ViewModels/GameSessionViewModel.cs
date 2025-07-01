using Engine.EventArgs;
using Engine.Factories;
using Engine.Models;
using ReactiveUI;
using System.Reactive;

namespace Engine.ViewModels;

public class GameSessionViewModel : ViewModelBase
{
    private Location? _currentLocation;
    private Monster? _currentMonster;
    private Trader? _currentTrader;

    public event EventHandler<GameMessageEventArgs>? OnMessageRaised;
    public Player CurrentPlayer { get; set; }
    public Weapon? CurrentWeapon { get; set; }
    public Monster? CurrentMonster
    {
        get => _currentMonster;
        set
        {
            this.RaiseAndSetIfChanged(ref _currentMonster, value);
            this.RaisePropertyChanged(nameof(HasMonster));
            if (CurrentMonster is not null)
            {
                RaiseMessage("");
                RaiseMessage($"You see a {CurrentMonster.Name} here.");
            }
                
        }
    }
    
    public Location? CurrentLocation 
    { 
        get => _currentLocation; 
        set {
            this.RaiseAndSetIfChanged(ref _currentLocation, value);
            GivePlayerQuestsAtLocation();
            GetMonsterAtLocation();
            CurrentTrader = _currentLocation?.TraderHere;
        }
    }

    public Trader? CurrentTrader
    {
        get => _currentTrader; 
        set
        {
            this.RaiseAndSetIfChanged(ref _currentTrader, value);
            this.RaisePropertyChanged(nameof(HasTrader));
        }
    }

    public World CurrentWorld { get; set; }

    public ReactiveCommand<Unit, Unit> MoveNorthCommand {get; }
    public ReactiveCommand<Unit, Unit> MoveWestCommand { get; }
    public ReactiveCommand<Unit, Unit> MoveEastCommand { get; }
    public ReactiveCommand<Unit, Unit> MoveSouthCommand { get; }
    public ReactiveCommand<Unit, Unit> AttackMonsterCommand { get; }
    public ReactiveCommand<Unit, Unit> TradeCommand { get; }

    public bool HasMonster => CurrentMonster != null;
    public bool HasTrader => CurrentLocation?.TraderHere != null;

    public GameSessionViewModel()
    {
        CurrentPlayer = new()
        {
            Name = "Scott",
            CharacterClass = "Fighter",
            HitPoints = 10,
            Gold = 100000,
            ExpPoints = 0,
            Level = 1
        };
        if (!CurrentPlayer.Weapons.Any())
        {
            CurrentPlayer.AddItemToInventory(ItemFactory.CreateGameItem(1001));
        }

        CurrentWorld = WorldFactory.CreateWorld();
        CurrentLocation = CurrentWorld.LocationAt(0, 0);
       
        var moveToNorth = this.WhenAnyValue(loc => loc.CurrentLocation, 
            loc => MoveToLocation(loc?.XCoordinate, loc?.YCoordinate + 1) != null);
        var moveToSouth = this.WhenAnyValue(loc => loc.CurrentLocation,
            loc => MoveToLocation(loc?.XCoordinate, loc?.YCoordinate - 1) != null);
        var moveToWest = this.WhenAnyValue(loc => loc.CurrentLocation,
            loc => MoveToLocation(loc?.XCoordinate - 1, loc?.YCoordinate) != null);
        var moveToEast = this.WhenAnyValue(loc => loc.CurrentLocation,
            loc => MoveToLocation(loc?.XCoordinate + 1, loc?.YCoordinate) != null);
        var hasTrader = this.WhenAnyValue(loc => loc.HasTrader);

        MoveNorthCommand = ReactiveCommand.Create(MoveNorth, moveToNorth);
        MoveWestCommand = ReactiveCommand.Create(MoveWest, moveToWest);
        MoveEastCommand = ReactiveCommand.Create(MoveEast, moveToEast);
        MoveSouthCommand = ReactiveCommand.Create(MoveSouth, moveToSouth);
        AttackMonsterCommand = ReactiveCommand.Create(AttackMonster);
        TradeCommand = ReactiveCommand.Create(() => { }, hasTrader);
    }

   

    private void MoveNorth() =>
        CurrentLocation = MoveToLocation(CurrentLocation?.XCoordinate, CurrentLocation?.YCoordinate + 1);

    private void MoveWest() =>
        CurrentLocation = MoveToLocation(CurrentLocation?.XCoordinate - 1, CurrentLocation?.YCoordinate );
    
    private void MoveEast() =>
        CurrentLocation = MoveToLocation(CurrentLocation?.XCoordinate + 1 , CurrentLocation?.YCoordinate );
    
    private void MoveSouth() =>
        CurrentLocation = MoveToLocation(CurrentLocation?.XCoordinate, CurrentLocation?.YCoordinate-1);
    
    private Location? MoveToLocation(int? xCoordinate, int? yCoordinate) => CurrentWorld.LocationAt(xCoordinate, yCoordinate);

    private void AttackMonster()
    {
        if (CurrentMonster is not null)
        {
            if (CurrentWeapon is null)
            {
                RaiseMessage("You must select a wepon, to attack.");
                return;
            }

            var damageToMonster = RandomNumberGenerator.NumberBetween(CurrentWeapon.MinimumDamage, CurrentWeapon.MaximumDamage);
            if (damageToMonster == 0)
            {
                RaiseMessage($"You missed the {CurrentMonster?.Name}");
            }
            else
            {
                CurrentMonster.HitPoints -= damageToMonster;
                RaiseMessage($"You hit the {CurrentMonster?.Name} for {damageToMonster} points.");
            }

            if (CurrentMonster?.HitPoints <= 0)
            {
                RaiseMessage($"");
                RaiseMessage($"You defeated the {CurrentMonster.Name}!");
                CurrentPlayer.ExpPoints += CurrentMonster.RewardExperiencePoints;
                RaiseMessage($"You receive {CurrentMonster.RewardExperiencePoints} experience points.");
                CurrentPlayer.Gold += CurrentMonster.RewardGold;
                RaiseMessage($"You receive {CurrentMonster.RewardGold} gold.");
                foreach (var itemQuantity in CurrentMonster.Inventory)
                {
                    var item = ItemFactory.CreateGameItem(itemQuantity.ItemId);
                    CurrentPlayer.AddItemToInventory(item);
                    RaiseMessage($"You receive {itemQuantity.Quantity} {item?.Name}.");
                }
                GetMonsterAtLocation();
            }
            var damageToPlayer = RandomNumberGenerator.NumberBetween(CurrentMonster.MinimumDamage, CurrentMonster.MaximumDamage);

            if (damageToPlayer == 0)
            {
                RaiseMessage("The monster attacks, but misses you.");
            }
            else
            {
                CurrentPlayer.HitPoints -= damageToPlayer;
                RaiseMessage($"The {CurrentMonster.Name} hit you for {damageToPlayer} points.");
            }

            if(CurrentPlayer?.HitPoints <= 0)
            {
                RaiseMessage("");
                RaiseMessage($"The {CurrentMonster.Name} killed you.");
                CurrentLocation = CurrentWorld.LocationAt(0, -1);
                CurrentPlayer.HitPoints = CurrentPlayer.Level * 10;
            }
        }
    }

    public void CompleteQuestsAtLocation()
    {
        if (CurrentLocation is not null)
        {
            foreach (var quest in CurrentLocation.QuestsAvailableHere)
            {
                if (quest is not null && CurrentPlayer.HasAllTheseItems(quest.ItemsToComplete))
                {
                    var questStatus = CurrentPlayer.Quests.FirstOrDefault(q => q.PlayerQuest.Id == quest.Id && !q.IsCompleted);
                    if (questStatus != null)
                    {
                        // Remove the quest completion items from the player's inventory
                        foreach (ItemQuantity itemQuantity in quest.ItemsToComplete)
                        {
                            for (int i = 0; i < itemQuantity.Quantity; i++)
                            {
                                CurrentPlayer.RemoveItemFromInventory(CurrentPlayer.Inventory.First(item => item.ItemTypeId == itemQuantity.ItemId));
                            }
                        }
                        RaiseMessage("");
                        RaiseMessage($"You completed the '{quest.Name}' quest");
                        // Give the player the quest rewards
                        CurrentPlayer.ExpPoints += quest.RewardExperiencePoints;
                        RaiseMessage($"You receive {quest.RewardExperiencePoints} experience points");
                        CurrentPlayer.Gold += quest.RewardGold;
                        RaiseMessage($"You receive {quest.RewardGold} gold");
                        foreach (ItemQuantity itemQuantity in quest.RewardItems)
                        {
                            var rewardItem = ItemFactory.CreateGameItem(itemQuantity.ItemId);
                            CurrentPlayer.AddItemToInventory(rewardItem);
                            RaiseMessage($"You receive a {rewardItem?.Name}");
                        }
                        // Mark the Quest as completed
                        questStatus.IsCompleted = true;
                    }
                }
            }
        }
    }

    private void GivePlayerQuestsAtLocation()
    {
        if (CurrentLocation is not null)
        {
            foreach (var quest in CurrentLocation.QuestsAvailableHere)
            {
                if (quest is not null)
                {
                    CurrentPlayer.Quests.Add(new QuestStatus(quest));
                    RaiseMessage("");
                    RaiseMessage($"You receive the '{quest.Name}' quest");
                    RaiseMessage(quest.Description);
                    RaiseMessage("Return with:");
                    foreach (ItemQuantity itemQuantity in quest.ItemsToComplete)
                    {
                        RaiseMessage($"{itemQuantity.Quantity} {ItemFactory.CreateGameItem(itemQuantity.ItemId)?.Name}");
                    }
                    RaiseMessage("And you will receive:");
                    RaiseMessage($"   {quest.RewardExperiencePoints} experience points");
                    RaiseMessage($"   {quest.RewardGold} gold");
                    foreach (ItemQuantity itemQuantity in quest.RewardItems)
                    {
                        RaiseMessage($"{itemQuantity.Quantity} {ItemFactory.CreateGameItem(itemQuantity.ItemId)?.Name}");
                    }
                }
            }
        }
    }
    private void GetMonsterAtLocation() => CurrentMonster = CurrentLocation?.GetMonster() ?? new Monster("No Monster", string.Empty, 0, 0, 0, 0,0,0);

    private void RaiseMessage(string message)
    {
        OnMessageRaised?.Invoke(this, new GameMessageEventArgs(message));
    }
}
