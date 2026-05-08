using System.Collections.Generic;
using UnityEngine;

public class OrderView : MonoBehaviour
{
    [SerializeField] private List<SpriteRenderer> Slots;
    [SerializeField] private Animator animator;
   

    private int slotIndex = 0;
    private int cookslot => slotIndex + 1;
    private int cutslot => slotIndex + 2;

    private void NextSlot()
    {
        slotIndex = cutslot + 1;
    }
    
    public void PlayExit()
    {
        animator.SetTrigger("exit");
    }

    public void PlayIdel()
    {
        animator.SetTrigger("idle");
    }

    public void SetOrder(string ordercode, IconsLibrary icons)
    {

        ResetView(icons);

        char ingredient = '?';
        char cookg = '?';
        char cutg = '?';

        slotIndex = 0;

        foreach(char s in ordercode)
        {
            if(ingredient == '?')
            {
                ingredient = s;
                continue;
            }

            if(cookg == '?')
            {
                cookg = s;
                continue;
            }

            if(cutg == '?')
            {
                cutg = s;
            }

            Slots[slotIndex].sprite = ingredient switch
            {
                't' => icons.GetSprite(Icon.TOMATO),
                'p' => icons.GetSprite(Icon.POTATO),
                'o' => icons.GetSprite(Icon.ONION),
                _   => icons.GetSprite(Icon.Empty),
            };

            Slots[cookslot].sprite = cookg switch
            {
                '2' => icons.GetSprite(Icon.CookIcon),
                _   => icons.GetSprite(Icon.Empty),
            };

            Slots[cutslot].sprite = cutg switch
            {
                '3' => icons.GetSprite(Icon.CutIcon),
                _ => icons.GetSprite(Icon.Empty),
            };

            ingredient = '?';
            cutg = '?';
            cookg = '?';

            NextSlot();

        }
        gameObject.SetActive(true);
    }

    private void ResetView(IconsLibrary icons)
    {
        foreach(var sprite in Slots)
        {
            sprite.sprite = icons.GetSprite(Icon.Empty);
        }
    }

}
