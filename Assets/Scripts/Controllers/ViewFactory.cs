using System;
using System.ComponentModel;
using UnityEngine;

public class ViewFactory: MonoBehaviour
{
    [SerializeField] private Transform Root;
    [SerializeField] private GameObject PlayerPrefap;
    [SerializeField] private GameObject StovePrefap;
    [SerializeField] private GameObject ShelfPrefap;
    [SerializeField] private GameObject PotPrefap;
    [SerializeField] private GameObject GeneratorPrefap;
    [SerializeField] private GameObject PotatoPrefap;
    [SerializeField] private GameObject TomatoPrefap;
    [SerializeField] private GameObject OnionPrefap;
    [SerializeField] private GameObject CuttingBoardP;
    [SerializeField] private GameObject OrderTablePrefap;
    [SerializeField] private GameObject DishPrefap;




  

   

    public ITileView CreateView(IOcuppier ocuppier, Vector3 pos)
    {
        var view = ocuppier switch
        {
            Stove           => GetView(StovePrefap, pos),
            Shelf           => GetView(ShelfPrefap, pos),
            Pot             => GetView(PotPrefap, pos),
            CharacterState  => GetView(PlayerPrefap, pos),
            Generator       => GetView(GeneratorPrefap,pos),
            CuttingBoard    => GetView(CuttingBoardP, pos),
            OrderTable      => GetView(OrderTablePrefap, pos),
            _ => null,

        };
        if(view != null)
        {
            view.transform.SetParent(Root);
        }

        return view;
    }

    public ITileView CreateCarriable(ICarriable carriable, Transform parent)
    {
        ITileView view = carriable switch
        {
            Pot  => Instantiate(PotPrefap, parent).GetComponent<ContainerView>(),
            Dish => Instantiate(DishPrefap, parent).GetComponent<DishView>(),
            Ingredient ingredient => ingredient.Type switch
            {
                IngredientType.POTATO => Instantiate(PotatoPrefap, parent).GetComponent<CarriabaleView>(),
                IngredientType.TOMATO => Instantiate(TomatoPrefap, parent).GetComponent<CarriabaleView>(),
                IngredientType.ONION => Instantiate(OnionPrefap,parent).GetComponent<CarriabaleView>(),
                _ => null,
            },
            _ => null,
        };
        if (view == null) return view;

        view.transform.localPosition = Vector3.zero;

        return view;
    }

    public ITileView GetView(GameObject prefap, Vector3 pos) => Instantiate(prefap, pos, new()).GetComponent<ITileView>();
}