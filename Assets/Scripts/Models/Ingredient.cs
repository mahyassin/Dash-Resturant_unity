public class Ingredient: ICarriable
{
    public IngredientType Type {get;}
    public CookingGrade cookingGrade = CookingGrade.RAW;
    public CuttingGrade cuttingGrade = CuttingGrade.WHOLE;

    public int CuttingProgress = 0;
    public int CookingProgress = 0;
    
    public Ingredient(IngredientType type)
    {
        Type = type;
    }
}

public enum IngredientType
{
    NONE,
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