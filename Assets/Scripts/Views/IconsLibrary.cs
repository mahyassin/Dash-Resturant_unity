using UnityEngine;

public class IconsLibrary: MonoBehaviour
{
    [SerializeField]private Sprite Pot;
    [SerializeField]private Sprite Tomato;
    [SerializeField]private Sprite Potato;
    [SerializeField]private Sprite Onion;
    [SerializeField]private Sprite Dish;
    [SerializeField]private Sprite Error;
    [SerializeField]private Sprite Stove;
    [SerializeField]private Sprite Empty;
    [SerializeField]private Transform StoveAnchor;
    [SerializeField]private Sprite Shelf;
    [SerializeField]private Sprite CutIcon;
    [SerializeField]private Sprite CookIcon;
   

    public Sprite GetSprite(Icon icon)
    {
        return icon switch
        {
            Icon.Dish   => Dish,
            Icon.ONION  => Onion,
            Icon.Pot    => Pot,
            Icon.POTATO => Potato,
            Icon.TOMATO => Tomato,
            Icon.Stove  => Stove,
            Icon.Shelf  => Shelf,
            Icon.Empty  => Empty,
            Icon.CookIcon => CookIcon,
            Icon.CutIcon  => CutIcon,
            _           => Error,
        };
    }


}
