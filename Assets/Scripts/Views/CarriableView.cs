using UnityEditor;
using UnityEngine;

public class CarriabaleView: MonoBehaviour
{
    [SerializeField] private Icon icon;
    [SerializeField] private SpriteRenderer spriteRenderer;

    public void RenderIcon(Icon iconToRender, IconsLibrary icons)
    {
        spriteRenderer.sprite = iconToRender switch
        {
            Icon.Dish   => icons.Dish,
            Icon.ONION  => icons.Onion,
            Icon.Pot    => icons.Pot,
            Icon.POTATO => icons.Potato,
            Icon.TOMATO => icons.Tomato,
            _           => icons.Error,
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
}