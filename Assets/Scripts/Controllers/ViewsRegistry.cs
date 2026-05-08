using System.Collections.Generic;

public class ViewsRigistry
{
    private Dictionary<int , ITileView> _views = new();
    private Dictionary<Type, Queue<ITileView>> _pools = new();


    public void AddView(int id, ITileView view)
    {
        if (view == null) return;
        _views.Add(id, view);
    }


    public ITileView GetOnTile(int id)
    {
        if (id == -1) return null;
        return _views[id];
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
        var poolName = state switch
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

        if(poolName == Type.Fail) return null;
        if(!_pools.ContainsKey(poolName)) return null;
        if(_pools[poolName].Count <= 0) return null;

        var view = _pools[poolName].Dequeue();
        _views[state.Id] = view;

        return view;
    }
}