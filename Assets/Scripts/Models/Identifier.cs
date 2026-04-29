using System;
using System.Collections.Generic;
using UnityEngine;

public class Identfier
{
    private int _nextId;
    private Dictionary<int ,IIdentifialbe> _entities = new();

    public void RegisterEntity(IIdentifialbe entity) => _entities.Add(entity.Id, entity);
    public int GetNextId() => _nextId++;
    public IIdentifialbe GetEntity(int id) => _entities[id];

}

public class EntitiesFactory
{
    private Identfier _identifier;

    public EntitiesFactory(Identfier identfier)
    {
        _identifier = identfier;
    }

    public T Create<T>(Func<int, T> constructor) where T: IIdentifialbe
    {
        int id = _identifier.GetNextId();
        T instance = constructor(id);

        _identifier.RegisterEntity(instance);

        return instance;
    }

    
    public Ingredient CreateIngredient(IngredientType type) => Create(it => new Ingredient(type, it));
    public Pot CreatePot()   => Create<Pot>(id => new(id));
    public Dish CreateDish() => Create<Dish>(id => new());
    public CharacterState CreateCharachter(Vector2Int pos) => Create<CharacterState>(id => new(pos, id));
    public Generator CreateGenerator(int instock, Ingredient ingredient) => Create<Generator>(id => new(instock, ingredient,id));
    public Stove CreateAStove(Pot pot) => Create(id => new Stove(pot, id));
    public CuttingBoard CreatecuttingBoard() => Create<CuttingBoard>(id => new(id));
    public Shelf CreateShelf(ICarriable onShelf) => Create<Shelf>(id => new(onShelf, id));
    public OrderTable CreateOrderTable() => Create<OrderTable>(id => new(id));
    public TrashCan CreateTrashCan() => Create<TrashCan>(id => new(id));
}



