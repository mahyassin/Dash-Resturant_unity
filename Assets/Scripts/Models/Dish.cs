using System.Collections.Generic;
using System.Linq;

public class Dish : ICarriable, IContainer
{
    private List<Ingredient> _dish = new();
    private Dish _stakedDish;
    private bool _isClean = true;
    private int _maxCapacity = 4;

    public IEnumerable<ICarriable> Carriables => _dish;
    public bool IsClean => _isClean;
    public Dish StackedDish => _stakedDish;


    public void AddToContainer(ICarriable carriable)
    {
        if (_dish.Count >= _maxCapacity) return;

        if(carriable is not Ingredient ingredient) return;

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

    public static string TomatoSuop      = "t33 t33 t33";
    public static string TomatoWithOnion = "t33 t33 o33";
    public static string PotatoSuop      = "p33 t33 o33";

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

