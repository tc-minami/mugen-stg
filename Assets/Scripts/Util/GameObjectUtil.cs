using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Game object util.
/// </summary>
public static class GameObjectUtil
{

    /// <summary>
    /// Creates the GameObject instance.
    /// </summary>
    /// <returns>The instance.</returns>
    /// <param name="_parent">Parent.</param>
    /// <param name="_name">Name.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    public static T CreateInstance<T>(Transform _parent, string _name) where T : MonoBehaviour
    {
        GameObject instance = new GameObject(_name);
        if (_parent != null) instance.transform.SetParent(_parent);
        instance.AddComponent<T>();
        return instance.GetComponent<T>();
    }

    /// <summary>
    /// Creates the instance.
    /// </summary>
    /// <returns>The instance.</returns>
    /// <param name="_parent">Parent.</param>
    /// <param name="_name">Name.</param>
    public static GameObject CreateInstance(Transform _parent, string _name)
    {
        GameObject instance = new GameObject(_name);
        if (_parent != null) instance.transform.SetParent(_parent);
        return instance;
    }
}
