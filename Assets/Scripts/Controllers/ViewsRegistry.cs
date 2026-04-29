using System.Collections.Generic;

public class ViewsRigistry
{
    private Dictionary<int , CharacterView> _charachters = new();
    private Dictionary<int , CarriabaleView> _carriables = new();


    public void AddCharacter(int id, CharacterView view)
    {
        _charachters.Add(id, view);
    }

    public void AddCarriable(int id, CarriabaleView view)
    {
        _carriables.Add(id, view);
    }

    public CharacterView GetCharacter(int id)  => _charachters[id];
    public CarriabaleView GetCarraible(int id) => _carriables[id];
}