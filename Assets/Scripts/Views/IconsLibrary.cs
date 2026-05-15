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
    [SerializeField]private Sprite TomatoCut;
    [SerializeField]private Sprite PotatoCut;
    [SerializeField]private Sprite OnionCut;
    [SerializeField]private Sprite TomatoSoup;
    [SerializeField]private Sprite TomatoOnion;
    [SerializeField]private Sprite PotatoSoup;


   

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
            Icon.TOMATO_CUT => TomatoCut,
            Icon.POTATP_CUT => PotatoCut,
            Icon.ONION_CUT  => OnionCut,
            Icon.TOMATO_SOUP => TomatoSoup,
            Icon.TOMATO_ONION => TomatoOnion,
            Icon.PTATO_SOUP => PotatoSoup,
            _           => Error,
        };
    }


}
