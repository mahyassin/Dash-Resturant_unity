using UnityEditor;
using UnityEngine;

public class CarriabaleView: MonoBehaviour, ITileView
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    public Transform Anchor {get;}

    public void RenderIcon(Icon iconToRender, IconsLibrary icons)
    {
        spriteRenderer.sprite = icons.GetSprite(iconToRender);
    }
}

