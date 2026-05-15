using System.Collections.Generic;
using UnityEngine;

public class OrderView : MonoBehaviour
{
    [SerializeField] private List<SpriteRenderer> Slots;
    [SerializeField] private AnimatorController animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private ProgressBarView PatainceBar;

    private int _orderId = -1;
    public int OrderId => _orderId;
    private int slotIndex = 0;
    private int cookslot => slotIndex + 1;
    private int cutslot => slotIndex + 2;


    void Awake()
    {
        animator.OnAnimationFinished += OnAnimationFinished;
    }

    private void OnAnimationFinished(string name)
    {
        switch (name)
        {
            case "orderCompleteAnimation":

                PlayExit();

            break;

            case "orderFailed":

                PlayExit();

            break;

            case "orderExit":

                gameObject.SetActive(false);
                transform.localPosition = Vector3.zero;

            break;

        }
    }

    private void NextSlot()
    {
        slotIndex = cutslot + 1;
    }
    
    public void PlayExit()
    {
        animator.PlayMovment("exit");
    }
    public void PlayEnter()
    {
        gameObject.SetActive(true);
        animator.GoIdle();
        animator.PlayMovment("enter");

    }

    public void ViewPatianceBar(float amount)
    {
        PatainceBar.StartFilling(amount);
    }

    public void SetOrder(string ordercode, IconsLibrary icons, int id)
    {
        _orderId = id;

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
        PlayEnter();
    }

    public void SetOrderSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }

    private void ResetView(IconsLibrary icons)
    {
        foreach(var sprite in Slots)
        {
            sprite.sprite = icons.GetSprite(Icon.Empty);
        }
    }

    public void CompleteOrder()
    {
        animator.PlayeOverlay("complete");
        _orderId = -1;
    }

    public void FailOrder()
    {
        animator.PlayeOverlay("fail");
        _orderId = -1;
    }

    public void SetPatainceMeter(int current, int max)
    {
        float precentge = (float) current / (float) max * 100;
        PatainceBar.StartFilling(precentge);
    }

}
