using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Stove: IInteractable, IOcuppier, ICarrier, ICooker, IIdentifialbe
{
    public ICarriable OnCarrier => _onStove;
    
    private Pot _onStove;
    private int _timeInterval = 1;

    private State _state = State.Off;

    public enum State
    {
        On,
        Off,
    }
    private bool _onConfirm = false;

    private Vector2Int _pos;
    public Vector2Int Pos => _pos;

    public int Id {get;}

    public Stove(Pot pot, int id)
    {
        _onStove = pot;
        Id = id;
    }


    public void ChangePos(Vector2Int v)
    {
        
    }


    public void Interact()
    {
        if(!_onConfirm) {_onConfirm = true; return;}
        _state = _state switch
        {
            State.Off       => State.On,
            State.On        => State.Off,
            _               => State.Off,
        };
        _onConfirm = false;
    }

    public void Carry(ICarriable carriable)
    {
        if (carriable == null) _onStove = null;
        if (carriable is not Pot) return;

        _onStove = carriable as Pot;
    }

    public void Cook()
    {
        if(_state != State.On) return;
        if(_onStove is not Pot pot) return;
        pot.Cook();
    }

    public void UnConfirm()
    {
        if(!_onConfirm) return;
        if(_timeInterval > 0) {_timeInterval--; return;}

        _onConfirm = false;
        _timeInterval = 1;
        
    }

    public int GetCookingProgress()
    {
        return _onStove.Content.Sum(it => (it as Ingredient).CookingProgress);
    }

    public CookingGrade GetCookingGrade()
    {
        
        if (_onStove.Content.DefaultIfEmpty()?.Last() is not Ingredient ingredient) return CookingGrade.RAW;
        return ingredient.cookingGrade;
    }

    public bool IsOn()
    {
        return _state == State.On;
    }

}

public class CuttingBoard: IInteractable, IOcuppier, ICarrier, IIdentifialbe
{
    public ICarriable OnCarrier => _onboard;
    
    private Ingredient _onboard;

    private Vector2Int _pos;
    public Vector2Int Pos => _pos;

    public int Id {get;}

    public CuttingBoard(int id)
    {
        Id = id;
    }
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

public class Generator: IOcuppier, ICarrier, IIdentifialbe
{
    private IngredientType _ingredientType;
    public int InStok;
    public ICarriable OnCarrier => _carriable;

    private ICarriable _carriable;

    public Generator(int instok, ICarriable carriable, int id)
    {
        InStok = instok;
        _carriable = carriable;
        _ingredientType = (carriable as Ingredient).Type;
        Id = id;
    }

    private Vector2Int _pos;
    public Vector2Int Pos => _pos;

    public int Id {get;}

    public void ChangePos(Vector2Int v)
    {
        throw new NotImplementedException();
    }

    public void Carry(ICarriable carriable)
    {
        
        _carriable = carriable;
        if(InStok > 0 && _carriable == null)
        {
            InStok--;
        }
    }
}

public class Pot: ICarriable, IContainer, IIdentifialbe
{
    public int Id {get;}
    private List<Ingredient> _ingredients = new();

    public Pot(int id)
    {
        Id = id;
    }
    public IEnumerable<ICarriable> Content => _ingredients;

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
            if(ingredient.cookingGrade == CookingGrade.COOKED) continue;
            ingredient.Cook(); return;
        }

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

public class Shelf: ICarrier, IOcuppier, IIdentifialbe
{
    private ICarriable _onShelf;
    private Vector2Int _pos;
   

    public Shelf(ICarriable carriable, int id)
    {
        _onShelf = carriable;
        Id = id;
    }


    public ICarriable OnCarrier => _onShelf;

    public Vector2Int Pos => _pos;

    public int Id {get;}

    public void Carry(ICarriable carriable)
    {
        _onShelf = carriable;
    }

    public void ChangePos(Vector2Int v)
    {
        _pos = v;
    }

}
public class OrderTable : IOrderMaker, IOcuppier, ICarrier, IIdentifialbe
{
    public int Id {get;}

    private int _coolDown;
    private Dish _finishedDish;


    public bool MakeOrderReady()
    {
        if(_coolDown <= 0) 
        {
            _coolDown = UnityEngine.Random.Range(5, 60);
            return true;
        };

        _coolDown--;
        return false;
    }

    public void ReciveOrder(List<Ingredient> order, OrdersState state)
    {
        string code = Recipes.RecepieIncoder(order).Replace(" ","");
        Debug.Log(code);

        var pendingOrders = new List<Order>(state.PendingOrders);

        foreach(var pending in pendingOrders)
        {
            string pendingCode = pending.code.Replace(" ","");

            if( code != pendingCode) continue;
            Debug.Log("success receipe");
            if(pending.OrderMakerId != Id) continue;
            Debug.Log("success costumer");
            state.CompleteOrder(pending);
            return;
        }
    }



    public void ChangePos(Vector2Int v)
    {
        throw new NotImplementedException();
    }

    public void RetrunUncleanDish()
    {
        var tempDish = _finishedDish;
        if( tempDish == null)
        {
            _finishedDish = new(false);
            Debug.Log("plateCreated");
            return;
        }
        while(tempDish.StackedDish != null)
        {
            tempDish = tempDish.StackedDish;
        }

        tempDish.Stack(new(false));
        
    }

    public void Carry(ICarriable carriable)
    {
        if (carriable == null)
        _finishedDish = null;

        if (carriable is Dish dish )
        _finishedDish = dish;
    }

    public OrderTable(int id)
    {
        Id = id;
    }
    public Vector2Int Pos => throw new NotImplementedException();

    public ICarriable OnCarrier => _finishedDish;
}

public class TrashCan : IOcuppier, ICarrier,IIdentifialbe
{
    public Vector2Int Pos => throw new NotImplementedException();

    public ICarriable OnCarrier => null;

    public TrashCan(int id)
    {
        Id = id;
    }

    public int Id {get;}

    public void Carry(ICarriable carriable)
    {
        return;
    }

    public void ChangePos(Vector2Int v)
    {
        throw new NotImplementedException();
    }
}