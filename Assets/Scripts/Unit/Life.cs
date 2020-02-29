using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life
{
    private float maxLife;
    private float currentLife;

    /// <summary>
    /// Initializes life info.
    /// </summary>
    /// <param name="_maxLife">Max life.</param>
    public void Init(float _maxLife)
    {
        maxLife = _maxLife;
        currentLife = _maxLife;
    }

    /// <summary>
    /// Gets the life info in "current / max" format.
    /// </summary>
    /// <returns>The life info in "current / max" format.</returns>
    public string getLifeInfo()
    {
        return getCurrentLife() + " / " + getMaxLife();
    }

    /// <summary>
    /// Gets the current life.
    /// </summary>
    /// <returns>The current life.</returns>
    public float getCurrentLife()
    {
        return currentLife;
    }

    /// <summary>
    /// Gets the max life.
    /// </summary>
    /// <returns>The max life.</returns>
    public float getMaxLife()
    {
        return maxLife;
    }
}
