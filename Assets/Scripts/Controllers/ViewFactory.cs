using System;
using System.ComponentModel;
using UnityEngine;

public class ViewFactory: MonoBehaviour
{

    [SerializeField] private GameObject PlayerPrefap;
    [SerializeField] private GameObject StovePrefap;
    [SerializeField] private GameObject ShelfPrefap;
    [SerializeField] private GameObject PotPrefap;
    [SerializeField] private GameObject GeneratorPrefap;
    [SerializeField] private GameObject PotatoPrefap;
    [SerializeField] private GameObject TomatoPrefap;
    [SerializeField] private GameObject CuttingBoardP;

  

   

    public ITileView CreateView(IOcuppier ocuppier, Vector3 pos)
    {
        return ocuppier switch
        {
            Stove           => Instantiate(StovePrefap, pos, new()).GetComponent<ITileView>(),
            Shelf           => Instantiate(ShelfPrefap, pos, new()).GetComponent<ITileView>(),
            Pot             => Instantiate(PotPrefap, pos, new()).GetComponent<ITileView>(),
            CharacterState  => Instantiate(PlayerPrefap, pos, new()).GetComponent<CharacterView>(),
            Generator       => Instantiate(GeneratorPrefap,pos, new()).GetComponent<ITileView>(),
            CuttingBoard    => Instantiate(CuttingBoardP, pos, new()).GetComponent<ITileView>(),
            _ => null,

        };
         
    }

    public ITileView CreateCarriable(ICarriable carriable, Transform parent)
    {
        ITileView view = carriable switch
        {
            Pot => Instantiate(PotPrefap, parent).GetComponent<ContainerView>(),
            Ingredient ingredient => ingredient.Type switch
            {
                IngredientType.POTATO => Instantiate(PotatoPrefap, parent).GetComponent<CarriabaleView>(),
                IngredientType.TOMATO => Instantiate(TomatoPrefap, parent).GetComponent<CarriabaleView>(),
                _ => null,
            },
            _ => null,
        };
        if (view == null) return view;

        view.transform.localPosition = Vector3.zero;

        return view;
    }
}