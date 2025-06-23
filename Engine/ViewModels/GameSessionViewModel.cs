using Engine.Models;
using ReactiveUI;
using System.Reactive;

namespace Engine.ViewModels;

public class GameSessionViewModel : ViewModelBase
{
    public Player CurrentPlayer { get; set; }

    public ReactiveCommand<Unit, Unit> AddXPCommand { get; }

    public GameSessionViewModel()
    {
        CurrentPlayer = new Player();
        CurrentPlayer.Name = "Scott";
        CurrentPlayer.CharacterClass = "Fighter";
        CurrentPlayer.HitPoints = 10;
        CurrentPlayer.Gold = 100000;
        CurrentPlayer.ExpPoints = 0;
        CurrentPlayer.Level = 1;

        AddXPCommand = ReactiveCommand.Create(AddXP);
    }

    private void AddXP()
    {
        CurrentPlayer.ExpPoints = CurrentPlayer.ExpPoints + 10;
    }
}
