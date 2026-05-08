
public class Ingredient: ICarriable, IIdentifialbe
{
    public int Id {get;}
    private int _cutting;
    private int _cooking;
    private IngredientType _type;

    private const int _rawPoint = 5;
    private const int _meduimPoint = 10;
    private const int _cookedPoint = 20;
    private const int _overcookedMark = 25;

    public int RawPoint => _rawPoint;
    public int MeduimPoint => _meduimPoint;
    public int CookedMark => _cookedPoint;
    public int OverCookedMark => _overcookedMark;



    
    
    public Ingredient(IngredientType type, int id )
    {
        _type = type;
        Id = id;
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
        < _rawPoint                        => CookingGrade.RAW,
        >= _rawPoint and < _meduimPoint    => CookingGrade.MEDUIM_RARE,
        >= _meduimPoint and < _cookedPoint => CookingGrade.COOKED,
        > _cookedPoint                     => CookingGrade.OVERCOOKED,
        _              => CookingGrade.OVERCOOKED,
    };

    public CuttingGrade cuttingGrade => _cutting switch
    {


        < 3            => CuttingGrade.WHOLE,
        >= 3 and < 5   => CuttingGrade.BIG,
        >= 5 and < 7   => CuttingGrade.MEDUIM,
        >= 7 and < 10  => CuttingGrade.SMALL,
        _              => CuttingGrade.SMALL,
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