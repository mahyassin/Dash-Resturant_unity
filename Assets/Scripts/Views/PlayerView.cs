using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class CharacterView: MonoBehaviour, IViewable
{

    [SerializeField] private int speed;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer Renderer;
    public Transform HandAnchor;

    private Coroutine _moverutine;
    

    private Animations animations;

    public Vector3 Pos => transform.position;

    private IEnumerator Move(Vector3 from, Vector3 to)
    {
        var dir = to.x - from.x;
        Renderer.flipX = dir == 0? Renderer.flipX: dir < 0;

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
        
    }


    public void MoveCharacter(Vector3 from, Vector3 to)
    {
        if(_moverutine != null) StopCoroutine(_moverutine);

        _moverutine = StartCoroutine(Move(from, to)); 
    }   

    public void PlayHorizontalDashAnimation() => TriggerAnimation(animations.DashTrigger, "isH");
    public void PlayUpDashAnimation() => TriggerAnimation(animations.DashTrigger, "isUp");

}

public struct Animations
{
    public string DashTrigger => "Dash";
}