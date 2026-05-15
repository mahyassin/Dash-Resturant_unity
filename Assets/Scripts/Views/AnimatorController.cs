using System;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    [SerializeField] Animator movmentAnimator;
    [SerializeField] Animator overlayAnimator;
    public event Action<string> OnAnimationFinished;

    public void AnimationFinished(string animationName)
    {
        OnAnimationFinished?.Invoke(animationName);
    }

    public void PlayMovment(string trigger)
    {
        movmentAnimator.SetTrigger(trigger);
    }

    public void PlayeOverlay(string trigger)
    {
        overlayAnimator.SetTrigger(trigger);
    }

    public void GoIdle()
    {
        overlayAnimator.Play("idle");
        movmentAnimator.Play("idle");
    }

}
