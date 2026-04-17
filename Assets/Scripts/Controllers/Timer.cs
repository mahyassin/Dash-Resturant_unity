using System;
using System.Collections;
using UnityEngine;
using UnityEngine.U2D;

public class Timer: MonoBehaviour
{
    public event Action<int> OnTimerTick;
    private int clock = 0;


    void Start()
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

            clock = clock >= 5? 0: clock + 1;

            OnTimerTick?.Invoke(clock);
        }
    }
}