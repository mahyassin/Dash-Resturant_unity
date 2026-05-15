using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public class Dish : ICarriable, IContainer, IIdentifialbe
{
    private List<Ingredient> _dish = new();
    private Dish _stakedDish;
    private bool _isClean = true;
    private int _maxCapacity = 4;

    public IEnumerable<ICarriable> Content => _dish;
    public bool IsClean => _isClean;
    public List<Ingredient> DishContent => _dish;
    public Dish StackedDish => _stakedDish;

    public int Id {get;}

    public Dish(int id)
    {
        _isClean = true;
        Id = id;
    }

    public Dish(bool isclean, int id)
    {
        _isClean = isclean;
        Id = id;
    }
    public void AddToContainer(ICarriable carriable)
    {
        if (_dish.Count >= _maxCapacity) return;

        if(carriable is not Ingredient ingredient) return;
        if (!_isClean) return;
        
        _dish.Add(ingredient);
    }

    public void EmptyTheContainer()
    {
        _dish = new();

        _isClean = false;
    }

    public void CleanDish()
    {
        _isClean = true;
    }

    public void AddToContainer(List<ICarriable> carriable)
    {
        _dish.AddRange(carriable.Select(it => it as Ingredient));
    }

    public void Stack(Dish dish)
    {
        _stakedDish = dish;
    }
}


public struct Recipes
{
   // c ingriedint

   // 00, 10, 20, 30 cutting grade
   // 01, 02, 03, 04 cooking grade 

    public static (string, Icon) TomatoSuop      = ("t20t20t20", Icon.TOMATO_SOUP);
    public static (string, Icon) TomatoWithOnion = ("t03t03o03", Icon.TOMATO_ONION);
    public static (string, Icon) PotatoSuop      = ("p20t20o20",Icon.PTATO_SOUP);

    public static string RecepieIncoder(List<Ingredient> ingredients)
    {
        var output = "";

        foreach(var ingriedint in ingredients)
        {
            output += ingriedint.Type switch
            {
                IngredientType.ONION  => "o",
                IngredientType.POTATO => "p",
                IngredientType.TOMATO => "t",
                _                     => "?",
            }
            +
            ingriedint.cookingGrade switch
            {
                CookingGrade.RAW         => "0 ",
                CookingGrade.MEDUIM_RARE => "1 ",
                CookingGrade.COOKED      => "2 ",
                _                        => "3 ",
            }
            +  
            ingriedint.cuttingGrade switch
            {
                CuttingGrade.WHOLE   => "0",
                CuttingGrade.BIG     => "1",
                CuttingGrade.MEDUIM  => "2",
                _                    => "3",
            };
        }

        return output;
    }

}

public struct Menu
{
    private List<(string, Icon)> _currentList;
    
    public List<(string, Icon)> CurrentList => _currentList;
    
    public Menu(List<(string, Icon)> dishs)
    {
        _currentList = dishs;
    }

    public void AddToMenu((string, Icon) dish)
    {
        _currentList.Add(dish);
    }
}

public class Order
{
    public string Code {get;}
    public Icon Icon {get;}
    public int MaxPatiance {get;}
    private int _patianceMeter;
    private int _id;
    public int Id => _id;
    
    private State state = State.Pending;
    public enum State
    {
        Pending,
        Success,
        Fial,
    }
    public int OrderMakerId;

    public Order(string code, Icon icon, int orderMakerId, int id, int paitance)
    {
        Code = code;
        Icon = icon;
        OrderMakerId = orderMakerId;
        _patianceMeter = paitance;
        MaxPatiance = paitance;
        _id = id;

    }

    public void LosePatiance()
    {
        if(_patianceMeter < 0) state = State.Fial;

        _patianceMeter--;
    }
    public State GetState => state;
    public int Pataince => _patianceMeter;

}

