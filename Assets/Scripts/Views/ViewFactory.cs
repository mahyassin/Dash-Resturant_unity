using System;
using UnityEngine;

public class ViewFactory: MonoBehaviour
{

    [SerializeField] private GameObject PlayerPrefap;
    [SerializeField] private GameObject CarriablePrefap;
    [SerializeField] private IconsLibrary icons;
   
    public CharacterView CreateCharacter(Vector3 pos)
    {
        return Instantiate(PlayerPrefap, pos, new()).GetComponent<CharacterView>();
    }

    public CarriabaleView CreateCarraible(Vector3 pos, Icon icon, Transform ancor)
    {
        var carriableObject =  Instantiate(CarriablePrefap, ancor);
        carriableObject.transform.localPosition = Vector3.zero;

        var view = carriableObject.GetComponent<CarriabaleView>();
        view.RenderIcon(icon, icons);
        return view;
    }



}