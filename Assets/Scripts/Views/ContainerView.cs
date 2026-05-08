using System.Collections;
using System.Collections.Generic;
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

    private Coroutine _fillRoutine;

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

    public void ShowSprites(List<Icon> icons, IconsLibrary library)
    {

        var list = new List<Icon>()
        {
            Icon.Empty,
            Icon.Empty,
            Icon.Empty,
            Icon.Empty,
        };

        for(var i = 0; i <= 4 && i < icons.Count; i++)
        {
            list[i] = icons[i];
        }

        for(var i = 0; i < 4 || i < icons.Count; i++)
        {
            ChangeSlot(i + 1, library.GetSprite(list[i]));
        }
    }

    [ContextMenu("testFill")]
    public void Test()  
    {
        StartFilling(FillAmont);
    }
    public void StartFilling(float amount)
    {
        // _fillRoutine = StartCoroutine(FillBar(amount));
        var clamped = amount < 0? 0: amount > 100? 100: amount;
        float movmentMount = (clamped * .006f) - 0.6f;

        var targetPOs = Vector3.zero;
        targetPOs.x = targetPOs.x + movmentMount;


        FillPos.transform.localPosition = targetPOs;

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
