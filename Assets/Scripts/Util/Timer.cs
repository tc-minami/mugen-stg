using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple timer class for count down.
/// Has feature of on finish callback, and loop timer for continuous events.
/// </summary>
public class Timer : MonoBehaviour
{
    public delegate void OnTimerComplete();

    [SerializeField]
    private float maxTimerSec;
    [SerializeField]
    private bool isTimerLoop;

    private float timeElapsed;
    private bool isTimerActive = false;
    private OnTimerComplete onTimerComplete = null;

    /// <summary>
    /// Instantiates the timer with gameObject.
    /// </summary>
    /// <returns>The timer.</returns>
    /// <param name="_parent">Parent.</param>
    /// <param name="_name">Name.</param>
    public static Timer InstantiateTimer(Transform _parent, string _name)
    {
        return GameObjectUtil.CreateInstance<Timer>(_parent, _name);
    }

    /// <summary>
    /// Destroies the timer object.
    /// </summary>
    public void DestroyTimerObject()
    {
        StopTimer();
        Destroy(this.gameObject);
    }

    /// <summary>
    /// Runs the timer.
    /// </summary>
    /// <param name="_callback">Callback.</param>
    /// <param name="_sec">Sec.</param>
    /// <param name="_isLoop">If set to <c>true</c> is loop.</param>
    public void RunTimer(OnTimerComplete _callback, float _sec, bool _isLoop = false)
    {
        SetOnCompleteCallback(_callback);
        RunTimer(_sec, _isLoop);
    }

    /// <summary>
    /// Runs the timer.
    /// </summary>
    /// <param name="_sec">Sec.</param>
    /// <param name="_isLoop">If set to <c>true</c> is loop.</param>
    public void RunTimer(float _sec, bool _isLoop = false)
    {
        SetMaxTimerSec(_sec);
        timeElapsed = 0.0f;
        isTimerLoop = _isLoop;

        isTimerActive = true;
    }

    /// <summary>
    /// Sets the max timer sec.
    /// </summary>
    /// <param name="_sec">Sec.</param>
    public void SetMaxTimerSec(float _sec)
    {
        maxTimerSec = _sec;
    }

    /// <summary>
    /// Sets the on complete callback.
    /// </summary>
    /// <param name="callback">Callback.</param>
    public void SetOnCompleteCallback(OnTimerComplete callback)
    {
        onTimerComplete = callback;
    }

    /// <summary>
    /// Resumes the timer.
    /// </summary>
    public void ResumeTimer()
    {
        isTimerActive = true;
    }

    /// <summary>
    /// Pauses the timer.
    /// </summary>
    public void PauseTimer()
    {
        isTimerActive = false;
    }

    /// <summary>
    /// Is the timer active.
    /// </summary>
    /// <returns><c>true</c>, if timer active was ised, <c>false</c> otherwise.</returns>
    public bool IsTimerActive()
    {
        return isTimerActive;
    }

    /// <summary>
    /// Gets the elapsed time.
    /// </summary>
    /// <returns>The elapsed time.</returns>
    public float GetElapsedTime()
    {
        return timeElapsed;
    }

    /// <summary>
    /// Gets the remain time.
    /// </summary>
    /// <returns>The remain time.</returns>
    public float GetRemainTime()
    {
        return maxTimerSec - timeElapsed;
    }

    /// <summary>
    /// Stops the timer.
    /// </summary>
    void StopTimer()
    {
        PauseTimer();
        ResetElapsedTime();
    }

    /// <summary>
    /// Resets the elapsed time.
    /// </summary>
    private void ResetElapsedTime()
    {
        timeElapsed = 0.0f;
    }

    /// <summary>
    /// Update this instance.
    /// </summary>
    protected void Update()
    {
        if (!isTimerActive) return;

        timeElapsed += Time.deltaTime;
        if (timeElapsed >= maxTimerSec)
        {
            timeElapsed = maxTimerSec;

            onTimerComplete?.Invoke();

            if (isTimerLoop)
            {
                // Restart timer if timer is set to loop.
                RunTimer(maxTimerSec, isTimerLoop);
            }
        }
    }
}
