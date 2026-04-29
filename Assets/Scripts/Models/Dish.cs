using System.Collections.Generic;
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

    public Dish()
    {
        _isClean = true;
    }

    public Dish(bool isclean)
    {
        _isClean = isclean;
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

    public static string TomatoSuop      = "t32 t32 t32";
    public static string TomatoWithOnion = "t32 t32 o32";
    public static string PotatoSuop      = "p32 t32 o32";

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
            ingriedint.cuttingGrade switch
            {
                CuttingGrade.WHOLE  => "0",
                CuttingGrade.BIG    => "1",
                CuttingGrade.MEDUIM => "2",
                _                   => "3",
            }
            +  
            ingriedint.cookingGrade switch
            {
                CookingGrade.RAW         => "0 ",
                CookingGrade.MEDUIM_RARE => "1 ",
                CookingGrade.COOKED      => "2 ",
                _                        => "3 ",
            };
        }

        return output;
    }

}

public struct Menu
{
    private List<string> _currentList;
    
    public List<string> CurrentList => _currentList;
    
    public Menu(List<string> dishs)
    {
        _currentList = dishs;
    }

    public void AddToMenu(string dish)
    {
        _currentList.Add(dish);
    }
}

public class Order
{
    public string code {get;}
    private int _patianceMeter;
    
    private State state = State.Pending;
    public enum State
    {
        Pending,
        Success,
        Fial,
    }
    public int OrderMakerId;

    public Order(string dish, int orderMakerId)
    {
        code = dish;
        OrderMakerId = orderMakerId;
    }

    public void LosePatiance()
    {
        if(_patianceMeter > 0) state = State.Fial;

        _patianceMeter--;
    }

    public void ReciveOrder(string recived)
    {
        if(code != recived) {state = State.Fial; return;}
        state = State.Success;
    }

    public State GetState => state;

}

