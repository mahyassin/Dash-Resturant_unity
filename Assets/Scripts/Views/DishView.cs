using UnityEngine;

public class DishView: MonoBehaviour, ITileView, IcontainerView
{
    public Transform _onDish;
    public ContentPubleView contentPuble;


    public Transform Anchor => _onDish;
    public Type type;

    public Type Type => type;

    public ContentPubleView PubleView => contentPuble;

    public SpriteRenderer DishSprite;

    public void SetOnDish(ITileView view)
    {
        view.transform.SetParent(_onDish);
        view.transform.localPosition = Vector3.zero;
    }

    public void SetDish(Sprite sprite)
    {
        DishSprite.sprite = sprite;
    }
    void Awake()
    {
        // contentPuble.gameObject.SetActive(false);   
    }
}