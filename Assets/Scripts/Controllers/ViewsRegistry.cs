using System.Collections.Generic;
using System.Diagnostics;

public class ViewsRigistry
{
    private Dictionary<int , ITileView> _views = new();
    private Dictionary<Type, Queue<ITileView>> _pools = new();


    public void AddView(int id, ITileView view)
    {
        if (view == null) return;
        _views[id] = view;
    }


    public ITileView GetOnTile(int id)
    {
        if (id == -1) return null;
        return _views[id];
    }

    public StationView GetStation(int id)
    {
        if (id == -1) return null;
        var view = _views[id];

        if(view is not StationView station) return null;

        return station;
    }

    public CharacterView GetCharacter(int id)
    {
        if (id == -1) return null;
        var view = _views[id];

        if(view is not CharacterView character) return null;

        return character;
    }

    public CarriabaleView GetCarriable(int id)
    {
        if (id == -1) return null;
        var view = _views[id];

        if(view is not CarriabaleView carriable) return null;

        return carriable;
    }

    public ContainerView GetContainer(int id)
    {
        if (id == -1) return null;
        var view = _views[id];

        if(view is not ContainerView container) return null;

        return container;
    }

    public void AddToPool(Type poolName, ITileView view)
    {
        if (!_pools.ContainsKey(poolName))
        {
            _pools[poolName] = new();
        }
        _pools[poolName].Enqueue(view);
    }

    public ITileView GetFromPool(IIdentifialbe state)
    {
        var poolType = state switch
        {
            Ingredient i => i.Type switch
            {
                IngredientType.TOMATO => Type.Tomato,
                IngredientType.POTATO => Type.Potato,
                IngredientType.ONION  => Type.Onion,
                _ => Type.Fail,
            },

            IContainer => Type.Container,
            _ => Type.Fail
        };

        if(poolType == Type.Fail) return null;
        if(!_pools.ContainsKey(poolType)) return null;
        if(_pools[poolType].Count <= 0) return null;

        var view = _pools[poolType].Dequeue();
        _views[state.Id] = view;
        return view;
    }
}