using System.Collections;
using UnityEngine;

public class ContainerView : MonoBehaviour, ITileView
{
    [SerializeField] private Transform anchor;
    [SerializeField] private SpriteRenderer Slot1;
    [SerializeField] private SpriteRenderer Slot2;
    [SerializeField] private SpriteRenderer Slot3;
    [SerializeField] private SpriteRenderer Slot4;
    [SerializeField] private GameObject FillPos;
    [SerializeField] private float FillSpeed, FillAmont;

    // private int _fill = 0;
    public Transform Anchor => anchor;

    public Type Type => Type.Container;


    public void ChangeSlot(int slotNum, Sprite sprite)
    {
        if(slotNum > 4 || slotNum < 1)
        {
            return;
        }

        SpriteRenderer temp = slotNum switch
        {
            1 => Slot1,
            2 => Slot2,
            3 => Slot3,
            4 => Slot4,
            _ => Slot1,
        };

        temp.sprite = sprite;   
    }

    [ContextMenu("testFill")]
    public void Test()  
    {
        StartFilling(FillAmont);
    }
    public void StartFilling(float amount)
    {
        StartCoroutine(FillBar(amount));
    }

    private IEnumerator FillBar(float amount)
    {
        var clamped = amount < 0? 0: amount > 100? 100: amount;
        float movmentMount = (clamped * .006f) - 0.6f;

        var targetPOs = Vector3.zero;
        targetPOs.x = targetPOs.x + movmentMount;
        

        while(FillPos.transform.localPosition != targetPOs)
        {
            FillPos.transform.localPosition = Vector3.MoveTowards(FillPos.transform.localPosition, targetPOs, FillSpeed * Time.deltaTime);
            yield return null;

        }

    }
}
