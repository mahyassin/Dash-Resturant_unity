using System;
using System.Collections.Generic;
using UnityEngine;

public class Stove: IInteractable, IOcuppier, ICarrier
{
    public ICarriable OnCarrier => _onStove;
    
    private Pot _onStove;
     private bool _isOn = false;
    public bool IsOn => _isOn;
     private Vector2Int _pos;
    public Vector2Int Pos => _pos;
    public void ChangePos(Vector2Int v)
    {
        throw new NotImplementedException();
    }
      public void Interact()
    {
        _isOn = !_isOn;
    }

    public void Carry(ICarriable carriable)
    {
        if (carriable is not Pot) return;

        _onStove = carriable as Pot;
    }
}

public class CuttingBoard: IInteractable, IOcuppier, ICarrier
{
    public ICarriable OnCarrier => _onboard;
    
    private Ingredient _onboard;

    private Vector2Int _pos;
    public Vector2Int Pos => _pos;

    public void Carry(ICarriable carriable)
    {
        if (carriable is Ingredient ingredient)
        {
            _onboard = ingredient;
        }
    }

    public void ChangePos(Vector2Int v)
    {
        throw new NotImplementedException();
    }

    public void Interact()
    {
        _onboard.CuttingProgress++;
    }
}

public class Generator: IOcuppier, ICarrier
{
    public int InStok;
    public ICarriable OnCarrier => _carriable;

    private ICarriable _carriable;

    public Generator(int instok, ICarriable carriable)
    {
        InStok = instok;
        _carriable = carriable;
    }

    private Vector2Int _pos;
    public Vector2Int Pos => _pos;
    public void ChangePos(Vector2Int v)
    {
        throw new NotImplementedException();
    }

    public void Carry(ICarriable carriable)
    {
        _carriable = carriable;
    }
}

public class Pot: ICarriable
{
    public List<Ingredient> ingredients = new();
}