using NUnit.Framework.Internal;
using UnityEngine;


public class ProgressBarView : MonoBehaviour
{
    [SerializeField] private Transform FillPos;
    [SerializeField] private float fill;

    public void StartFilling(float amount)
    {
        // _fillRoutine = StartCoroutine(FillBar(amount));
        var clamped = amount < 0? 0: amount > 100? 100: amount;
        float movmentMount = (clamped * .00615f) - 0.615f;

        var targetPOs = Vector3.zero;
        targetPOs.x = targetPOs.x + movmentMount;

        FillPos.transform.localPosition = targetPOs;

    }


    [ContextMenu("test")]
    public void Test()
    {
        StartFilling(fill);
    }

}
