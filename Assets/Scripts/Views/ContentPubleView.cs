using UnityEngine;
using System.Collections.Generic;

public class ContentPubleView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer Slot1;
    [SerializeField] private SpriteRenderer Slot2;
    [SerializeField] private SpriteRenderer Slot3;
    [SerializeField] private SpriteRenderer Slot4;


    

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
}
