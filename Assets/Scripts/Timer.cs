using UnityEngine;

public class Timer : MonoBehaviour
{
    float timer;

    bool started, paused;


    void Awake()
    {
        started = false;
        paused = true;

        timer = 0.0f;
    }

    public void Start()
    {
        if (!started)
        {
            started = true;
            paused = false;
            timer = 0.0f;
        }
    }

    void Update()
    {
        if (!paused && started)
        {
            timer += Time.deltaTime;
        }
    }

    public float GetTime()
    {
        return timer;
    }

    public void Pause()
    {
        paused = true;
    }

    public void UnPause()
    {
        paused = false;
    }

    public void Stop()
    {
        started = false;
    }

    public void Reset()
    {
        Stop();
        Start();
    }

    public bool GetStarted()
    {
        return started;
    }

    public bool GetPaused()
    {
        return paused;
    }
}