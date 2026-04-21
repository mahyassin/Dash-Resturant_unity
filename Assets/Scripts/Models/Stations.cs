using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Stove: IInteractable, IOcuppier, ICarrier, ICooker
{
    public ICarriable OnCarrier => _onStove;
    
    private Pot _onStove;

    private bool _isOn = false;
    public bool IsOn => _isOn;

    private Vector2Int _pos;
    public Vector2Int Pos => _pos;

    public Stove(Pot pot)
    {
        _onStove = pot;
    }


    public void ChangePos(Vector2Int v)
    {
        
    }

      public void Interact()
    {
        _isOn = !_isOn;
    }

    public void Carry(ICarriable carriable)
    {
        if (carriable == null) _onStove = null;
        if (carriable is not Pot) return;

        _onStove = carriable as Pot;
    }

    public void Cook()
    {
        if(!_isOn) return;
        _onStove.Cook();
    }

    public int GetCookingProgress()
    {
        return _onStove.Carriables.Sum(it => (it as Ingredient).CookingProgress);
    }

    public CookingGrade GetCookingGrade()
    {
        
        if (_onStove.Carriables.DefaultIfEmpty()?.Last() is not Ingredient ingredient) return CookingGrade.RAW;
        return ingredient.cookingGrade;
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
        if (carriable is Ingredient ingredient )
        {
            _onboard = ingredient;
        }
        if (carriable == null)
        {
            _onboard = null;
        }
    }

    public void ChangePos(Vector2Int v)
    {
        throw new NotImplementedException();
    }

    public void Interact()
    {
        if (_onboard == null) return;
        
        _onboard.Chop();
    }

}

public class Generator: IOcuppier, ICarrier
{
    private IngredientType _ingredientType;
    public int InStok;
    public ICarriable OnCarrier => _carriable;

    private ICarriable _carriable;

    public Generator(int instok, ICarriable carriable)
    {
        InStok = instok;
        _carriable = carriable;
        _ingredientType = (carriable as Ingredient).Type;
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
        if(InStok > 0 && _carriable == null)
        {
            _carriable = new Ingredient(_ingredientType);
            InStok--;
        }
    }
}

public class Pot: ICarriable, IContainer
{
    private List<Ingredient> _ingredients = new();

    public IEnumerable<ICarriable> Carriables => _ingredients;

    public void AddToContainer(ICarriable carriable)
    {
        if (carriable is not Ingredient ingredient) return;
        _ingredients.Add(ingredient);
    }

    public void EmptyTheContainer()
    {
        _ingredients = new();
    }

    public void Cook()
    {
        foreach(var ingredient in _ingredients)
        {
            if(ingredient.cookingGrade == CookingGrade.OVERCOOKED) continue;
            ingredient.Cook(); return;
        }
    }

    public void AddToContainer(List<ICarriable> carriable)
    {
        _ingredients.AddRange(carriable.Select(it => it as Ingredient));
    }
}

public class Shelf: ICarrier, IOcuppier
{
    private ICarriable _onShelf;
    private Vector2Int _pos;
   

    public Shelf(ICarriable carriable)
    {
        _onShelf = carriable;
    }


    public ICarriable OnCarrier => _onShelf;

    public Vector2Int Pos => _pos;

    public void Carry(ICarriable carriable)
    {
        _onShelf = carriable;
    }

    public void ChangePos(Vector2Int v)
    {
        _pos = v;
    }
}