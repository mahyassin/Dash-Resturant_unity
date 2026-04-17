using System;
using System.Collections.Generic;
using UnityEngine;

public class Stove: IInteractable, IOcuppier
{
    public Pot OnStove;
    public bool isOn = false;

     private Vector2Int _pos;
    public Vector2Int Pos => _pos;
    public void ChangePos(Vector2Int v)
    {
        throw new NotImplementedException();
    }
}

public class CuttingBoard: IInteractable, IOcuppier
{
    public Ingredient OnBoard;

     private Vector2Int _pos;
    public Vector2Int Pos => _pos;
    public void ChangePos(Vector2Int v)
    {
        throw new NotImplementedException();
    }
}

public class Generator: IOcuppier
{
    public int InStok;
    public IngredientType IngredientType;

    public Generator(int instok, IngredientType type)
    {
        InStok = instok;
        IngredientType = type;
    }

     private Vector2Int _pos;
    public Vector2Int Pos => _pos;
    public void ChangePos(Vector2Int v)
    {
        throw new NotImplementedException();
    }
}

public class Pot: ICarriable
{
    public List<Ingredient> ingredients = new();
}