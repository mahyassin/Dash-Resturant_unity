using System;
using System.Collections.Generic;
using UnityEngine;


public class StationView: MonoBehaviour,ITileView
{
    [SerializeField] Animator animator;
    [SerializeField] Transform anchor;
    [SerializeField] private Type type;
    [SerializeField] private Transform FillPos;
    public Transform Anchor => anchor;

    public Transform Transform => transform;

    public Type Type => type;

    public void Interact(Context ctx)
    {
        if(ctx is StoveContext stove)
        {
            animator.SetBool("isOn", stove.IsOn);

        } 

            animator.SetTrigger("Interact");
    }

    public void StartFilling(float amount)
    {
        // _fillRoutine = StartCoroutine(FillBar(amount));
        var clamped = amount < 0? 0: amount > 100? 100: amount;
        float movmentMount = (clamped * .00615f) - 0.615f;

        var targetPOs = Vector3.zero;
        targetPOs.x = targetPOs.x + movmentMount;


        FillPos.transform.localPosition = targetPOs;

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

