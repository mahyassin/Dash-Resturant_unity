using System;
using System.Collections.Generic;
using UnityEngine;


public class StationView: MonoBehaviour,ITileView
{
    [SerializeField] Animator animator;
    [SerializeField] Transform anchor;
    [SerializeField] private Type type;
    public Transform Anchor => anchor;

    public Type Type => type;

    public void Interact(Context ctx)
    {
        if(ctx is StoveContext stove)
        {
            animator.SetBool("isOn", stove.IsOn);

        } 

            animator.SetTrigger("Interact");
    }
}
public interface Context{}

public struct StoveContext: Context
{
    public bool IsOn;

    public StoveContext(bool isOn)
    {
        IsOn = isOn;
    }
}
