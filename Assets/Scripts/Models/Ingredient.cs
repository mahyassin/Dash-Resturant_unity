public class Ingredient: ICarriable
{
   
    private int _cutting;
    private int _cooking;
    private IngredientType _type;

    
    
    public Ingredient(IngredientType type)
    {
        _type = type;
    }

    public void Cook()
    {
        _cooking++;
    }

    public void Chop()
    {
        _cutting++;
    }

    public IngredientType Type => _type;
    public CookingGrade cookingGrade => _cooking switch
    {
        < 5            => CookingGrade.RAW,
        >= 5 and < 10  => CookingGrade.MEDUIM_RARE,
        >= 10 and < 15 => CookingGrade.COOKED,
        > 15           => CookingGrade.OVERCOOKED,
        _              => CookingGrade.RAW,
    };

    public CuttingGrade cuttingGrade => _cutting switch
    {
        >= 3 and < 5   => CuttingGrade.BIG,
        >= 5 and < 7   => CuttingGrade.MEDUIM,
        >= 7 and < 10  => CuttingGrade.SMALL,
        _              => CuttingGrade.WHOLE,
    };

    public int CuttingProgress => _cutting;
    public int CookingProgress => _cooking;
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