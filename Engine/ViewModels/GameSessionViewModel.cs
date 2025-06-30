using Engine.Factories;
using Engine.Models;
using ReactiveUI;
using System.Reactive;

namespace Engine.ViewModels;

public class GameSessionViewModel : ViewModelBase
{
    private Location? _currentLocation;
   
    public Player CurrentPlayer { get; set; }
    public Location? CurrentLocation 
    { 
        get => _currentLocation; 
        set {
            this.RaiseAndSetIfChanged(ref _currentLocation, value);
            GivePlayerQuestsAtLocation();
        }
    }
    public World CurrentWorld { get; set; }

    public ReactiveCommand<Unit, Unit> MoveNorthCommand {get; }
    public ReactiveCommand<Unit, Unit> MoveWestCommand { get; }
    public ReactiveCommand<Unit, Unit> MoveEastCommand { get; }
    public ReactiveCommand<Unit, Unit> MoveSouthCommand { get; }

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

        MoveNorthCommand = ReactiveCommand.Create(MoveNorth, moveToNorth);
        MoveWestCommand = ReactiveCommand.Create(MoveWest, moveToWest);
        MoveEastCommand = ReactiveCommand.Create(MoveEast, moveToEast);
        MoveSouthCommand = ReactiveCommand.Create(MoveSouth, moveToSouth);
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

    private void GivePlayerQuestsAtLocation()
    {
        if (CurrentLocation is not null)
        {
            foreach (var quest in CurrentLocation.QuestsAvailableHere)
            {
                if (!CurrentPlayer.Quests.Any(q => q.PlayerQuest.Id == quest?.Id)) 
                {
                    if(quest is not null) CurrentPlayer.Quests.Add(new QuestStatus(quest));
                }   
            }
        }
    }
}
