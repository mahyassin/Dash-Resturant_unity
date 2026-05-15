using System;
using GluonGui;
using UnityEditor;
using UnityEngine;

public class CarriabaleView: MonoBehaviour, ITileView
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite whole;
    [SerializeField] private Sprite big;
    [SerializeField] private Sprite meduim;
    [SerializeField] private Sprite small;
    [SerializeField] private Type type;


    public Transform Anchor {get;}

    public Type Type => type;

    public void ViewGrade(string grade)
    {

        switch (grade)
        {
            case "whole": 
                spriteRenderer.sprite = whole;
            break;

             case "big": 
                spriteRenderer.sprite = big;

            break;

             case "meduim": 
                spriteRenderer.sprite = meduim;

            break;

             case "small": 
                spriteRenderer.sprite = small;

            break;

        }

    }


    public void ViewWhole() => spriteRenderer.sprite = whole;
    public void ViewBig() => spriteRenderer.sprite = big;
    public void ViewMeduim() => spriteRenderer.sprite = meduim;
    public void ViewSmall() => spriteRenderer.sprite = small;
   
}

