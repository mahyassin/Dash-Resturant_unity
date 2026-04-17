using System;
using System.Collections;
using UnityEngine;

class Timer: MonoBehaviour
{
    public event Action OnTimerTick;

    public Timer()
    {
        StartTimer();
    }

    public void StartTimer()
    {
        StartCoroutine(ClockRutine());
    }

    public void StopTimer()
    {
        StopCoroutine(ClockRutine());
    }

    private IEnumerator ClockRutine()
    {
        var wait = new WaitForSeconds(0.5f);

        while (true)
        {
            
        
            yield return wait;

            OnTimerTick?.Invoke();
        }
    }
}