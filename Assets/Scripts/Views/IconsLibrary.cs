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
    [SerializeField]private Transform StoveAnchor;
    [SerializeField]private Sprite Shelf;
    [SerializeField]private Transform ShelfAnchor;
    [SerializeField]private Transform DefualtAnchor;
    [SerializeField]private Animator StoveAnimation;

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
            _           => Error,
        };
    }

    public Transform GetAnchor(Icon icon)
    {
        return icon switch
        {
            Icon.Stove => StoveAnchor,
            Icon.Shelf => ShelfAnchor,

            _          => DefualtAnchor,
        };
    }

}

public enum Icon
{
    Pot,
    Dish,
    POTATO,
    TOMATO,
    ONION,
    Error,
    Stove,
    Shelf,
}