using UnityEngine;

public class StationView: MonoBehaviour, ITileView
{
    public Transform Anchor => StationAnchor;
    public Transform StationAnchor;
    public Animator animator;
    private bool _isON;

    [SerializeField]private SpriteRenderer spriteRenderer;


    public void SetSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }

    public void SetAnchor(Transform anchor)
    {
        StationAnchor.localPosition = anchor.localPosition;
    }

    private void TriggerAnimation()
    {
        animator.SetBool("isOn", _isON);
        animator.SetTrigger("Interact");
    }

    public void Interact(bool isOn)
    {
        _isON = isOn;
        TriggerAnimation();

    }


}