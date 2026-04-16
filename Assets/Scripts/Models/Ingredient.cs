public class Ingredient: ICarriable
{
    public IngredientType Type {get;}
    public CookingGrade cookingGrade;
    public CuttingGrade cuttingGrade;
    
    public Ingredient(IngredientType type)
    {
        Type = type;
    }
    
    
}

public enum IngredientType
{
    TOMATO,
    ONION,
    POTATO,
}

public enum CookingGrade
{
    RAW,
    MEDUIM_RARE,
    COOKED,
    OVERCOOKED,
}

public enum CuttingGrade
{
    WHOLE,
    BIG,
    MEDUIM,
    SMALL,
}