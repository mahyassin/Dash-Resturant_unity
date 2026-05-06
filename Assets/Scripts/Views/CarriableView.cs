using System;
using UnityEditor;
using UnityEngine;

public class CarriabaleView: MonoBehaviour, ITileView
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    [SerializeField] private Type type;


    public Transform Anchor {get;}

    public Type Type => type;

    public void ViewGrade(string grade) => animator.SetTrigger(grade);

    public void ViewWhole() => animator.SetTrigger("whole");
    public void ViewBig() => animator.SetTrigger("big");
    public void ViewMeduim() => animator.SetTrigger("meduim");
    public void ViewSmall() => animator.SetTrigger("small");
   
}

