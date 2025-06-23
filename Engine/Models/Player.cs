using ReactiveUI;

namespace Engine.Models;

public class Player : ReactiveObject
{
    private string? _name;
    private string? _characterClass;
    private int _hitPoints;
    private int _expPoints;
    private int _level;
    private int _gold;

    public string? Name 
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }

    public string? CharacterClass 
    { 
        get => _characterClass; 
        set => this.RaiseAndSetIfChanged(ref _characterClass, value); 
    }

    public int HitPoints 
    { 
        get => _hitPoints;
        set => this.RaiseAndSetIfChanged(ref _hitPoints, value); 
    }

    public int ExpPoints 
    { 
        get => _expPoints;
        set => this.RaiseAndSetIfChanged(ref _expPoints, value); 
    }

    public int Level 
    { 
        get => _level;
        set => this.RaiseAndSetIfChanged(ref _level, value); 
    }

    public int Gold 
    {  
        get => _gold;
        set => this.RaiseAndSetIfChanged(ref _gold, value); 
    }
}
