using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class CharacterView: MonoBehaviour, ITileView, IViewable
{

    [SerializeField] private int speed;
    [SerializeField] private Animator animator;
    [SerializeField] private Type type;
    public Transform Anchor => HandAnchor;

    public Transform HandAnchor;

    private Coroutine _moverutine;
    

    private Animations animations;

    public Vector3 Pos => transform.position;

    public Type Type => type;

    private IEnumerator Move(Vector3 from, Vector3 to)
    {
        var dir = to.x - from.x;
        var scale = transform.localScale;
        
        scale.x = dir == 0? scale.x: dir < 0? -1: 1;

        transform.localScale = scale;

        while (transform.position != to)
        {
            transform.position = Vector3.MoveTowards(transform.position, to, speed * Time.deltaTime);
            yield return null;
        }
    }
    

    private void TriggerAnimation(string triggerName, string direction)
    {
        ClearDirections();
        
        animator.SetBool(direction, true);
        animator.SetTrigger(triggerName);
        // animator.SetBool(direction, false);
    }

    private void ClearDirections()
    {
        animator.SetBool("isUp", false);
        animator.SetBool("isH", false);
        animator.SetBool("isD", false);
        
    }


    public void MoveCharacter(Vector3 from, Vector3 to)
    {
        if(_moverutine != null) StopCoroutine(_moverutine);

        _moverutine = StartCoroutine(Move(from, to)); 
    }   

    public void PlayHorizontalDashAnimation() => TriggerAnimation(animations.DashTrigger, "isH");
    public void PlayUpDashAnimation()   => TriggerAnimation(animations.DashTrigger, "isUp");
    public void PlayDownDashAnimation() => TriggerAnimation(animations.DashTrigger, "isD");
    public void PlayCuttingAnimation()  => animator.SetTrigger("Cutting");

}

public struct Animations
{
    public string DashTrigger => "Dash";
}