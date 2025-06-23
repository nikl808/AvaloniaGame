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
            this.RaisePropertyChanged("HasLocationToNorth");
            this.RaisePropertyChanged("HasLocationToEast");
            this.RaisePropertyChanged("HasLocationToSouth");
            this.RaisePropertyChanged("HasLocationToWest");
        }
    }
    public World CurrentWorld { get; set; }

    public bool HasLocationToNorth
    {
        get => MoveToLocation(CurrentLocation?.XCoordinate, CurrentLocation?.YCoordinate + 1) != null;
    }
    public bool HasLocationToEast
    {
        get => MoveToLocation(CurrentLocation?.XCoordinate + 1, CurrentLocation?.YCoordinate) != null;
    }
    public bool HasLocationToSouth
    {
        get => MoveToLocation(CurrentLocation?.XCoordinate, CurrentLocation?.YCoordinate - 1) != null;
    }
    public bool HasLocationToWest
    {
        get => MoveToLocation(CurrentLocation?.XCoordinate - 1, CurrentLocation?.YCoordinate) != null;
    }

    public ReactiveCommand<Unit, Unit> MoveNorthCommand {get; }
    public ReactiveCommand<Unit, Unit> MoveWestCommand { get; }
    public ReactiveCommand<Unit, Unit> MoveEastCommand { get; }
    public ReactiveCommand<Unit, Unit> MoveSouthCommand { get; }

    public GameSessionViewModel()
    {
        CurrentPlayer = new();
        CurrentPlayer.Name = "Scott";
        CurrentPlayer.CharacterClass = "Fighter";
        CurrentPlayer.HitPoints = 10;
        CurrentPlayer.Gold = 100000;
        CurrentPlayer.ExpPoints = 0;
        CurrentPlayer.Level = 1;

        WorldFactory factory = new();
        CurrentWorld = factory.CreateWorld();
        CurrentLocation = CurrentWorld.LocationAt(0, 0);

        MoveNorthCommand = ReactiveCommand.Create(MoveNorth);
        MoveWestCommand = ReactiveCommand.Create(MoveWest);
        MoveEastCommand = ReactiveCommand.Create(MoveEast);
        MoveSouthCommand = ReactiveCommand.Create(MoveSouth);
    }

    private void MoveNorth() =>
        CurrentLocation = MoveToLocation(CurrentLocation?.XCoordinate, CurrentLocation?.YCoordinate + 1);

    private void MoveWest() =>
        CurrentLocation = MoveToLocation(CurrentLocation?.XCoordinate + 1, CurrentLocation?.YCoordinate );

    private void MoveEast() =>
        CurrentLocation = MoveToLocation(CurrentLocation?.XCoordinate , CurrentLocation?.YCoordinate - 1);
    private void MoveSouth() =>
        CurrentLocation = MoveToLocation(CurrentLocation?.XCoordinate - 1, CurrentLocation?.YCoordinate);
    
    private Location? MoveToLocation(int? xCoordinate, int? yCoordinate) => CurrentWorld.LocationAt(xCoordinate, yCoordinate);
}
