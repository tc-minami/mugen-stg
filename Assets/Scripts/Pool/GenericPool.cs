using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generic pool.
/// </summary>
public class GenericPool : MonoBehaviour
{
    [SerializeField]
    private PoolableObject pooledObject;

    private const int NoSizeLimit = -1;
    private int maxPoolSize = NoSizeLimit;
    private Queue<PoolableObject> queue;

    /// <summary>
    /// Creates the new pool with prefab.
    /// </summary>
    /// <returns>The new pool with prefab.</returns>
    /// <param name="_poolableObjectPrefab">Poolable object prefab.</param>
    /// <param name="_parent">Parent.</param>
    /// <param name="_poolName">Pool name.</param>
    /// <param name="_poolObjName">Pool object name.</param>
    /// <param name="_maxPoolSize">Max pool size.</param>
    public static GenericPool CreateNewPoolWithPrefab(
        PoolableObject _poolableObjectPrefab,
        Transform _parent,
        string _poolName = "Pool",
        string _poolObjName = "PooledObj",
        int _maxPoolSize = NoSizeLimit)
    {
        GenericPool pool = CreateNewPool(_parent, _poolName, _maxPoolSize);
        pool.SetPoolableObject(_poolableObjectPrefab, _poolObjName);
        return pool;
    }

    /// <summary>
    /// Creates the new pool.
    /// </summary>
    /// <returns>The new pool.</returns>
    /// <param name="_parent">Parent.</param>
    /// <param name="_poolName">Pool name.</param>
    /// <param name="_poolObjName">Pool object name.</param>
    /// <param name="_maxPoolSize">Max pool size.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    public static GenericPool CreateNewPool<T>(
        Transform _parent,
        string _poolName = "Pool",
        string _poolObjName = "PooledObj",
        int _maxPoolSize = NoSizeLimit)
        where T : PoolableObject
    {
        GenericPool pool = CreateNewPool(_parent, _poolName, _maxPoolSize);
        T poolObj = GameObjectUtil.CreateInstance<T>(pool.transform, _poolObjName);
        pool.SetPoolableObject(poolObj, _poolObjName);
        return pool;
    }

    /// <summary>
    /// Creates the new pool.
    /// </summary>
    /// <returns>The new pool.</returns>
    /// <param name="_parent">Parent.</param>
    /// <param name="_poolName">Pool name.</param>
    /// <param name="_maxPoolSize">Max pool size.</param>
    public static GenericPool CreateNewPool(
        Transform _parent,
        string _poolName = "Pool",
        int _maxPoolSize = NoSizeLimit)
    {
        GenericPool pool = GameObjectUtil.CreateInstance<GenericPool>(_parent, _poolName);
        pool.SetMaxPoolSize(_maxPoolSize);
        pool.InitializePool();
        return pool;
    }

    /// <summary>
    /// Sets the poolable object.
    /// </summary>
    /// <param name="_poolableObject">Poolable object.</param>
    /// <param name="_poolObjName">Pool object name.</param>
    /// <param name="_removeCurrentPooledObj">If set to <c>true</c> remove current pooled object.</param>
    public void SetPoolableObject(
        PoolableObject _poolableObject,
        string _poolObjName,
        bool _removeCurrentPooledObj = true)
    {
        GameObject poolPrefabManager = GameObject.Find("PoolPrefabManager");
        if(poolPrefabManager == null)
        {
            poolPrefabManager = GameObjectUtil.CreateInstance(null, "PoolPrefabManager");
        }

        // Destroy current pool object prefab if any.
        DestoryPoolObjectPrefab();

        pooledObject = Object.Instantiate(_poolableObject);
        pooledObject.name = _poolObjName;
        pooledObject.gameObject.SetActive(false);
        pooledObject.transform.SetParent(poolPrefabManager.transform);

        if (_removeCurrentPooledObj)
        {
            ClearPooledObjects(false);
        }
    }

    /// <summary>
    /// Sets the size of the max pool.
    /// </summary>
    /// <param name="_max">Max.</param>
    public void SetMaxPoolSize(int _max)
    {
        maxPoolSize = _max;
    }

    /// <summary>
    /// Get or create new PoolableObject from Pool.
    /// </summary>
    /// <returns>The or create.</returns>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    public T GetOrCreate<T>() where T : PoolableObject
    {
        return GetOrCreate() as T;
    }

    /// <summary>
    /// Get or create new PoolableObject from Pool.
    /// </summary>
    /// <returns>The or create.</returns>
    public PoolableObject GetOrCreate()
    {
        // Just to make sure pool is initialized.
        InitializePool();

        PoolableObject obj = GetPooledObject();
        if (obj == null) obj = CreatePooledObject();
        return obj;
    }

    /// <summary>
    /// Gets the pooled object.
    /// </summary>
    /// <returns>The pooled object.</returns>
    private PoolableObject GetPooledObject()
    {
        PoolableObject poolableObject = null;
        while (queue.Count > 0 && poolableObject == null)
        {
            poolableObject = queue.Dequeue();
        }
        if (poolableObject != null) poolableObject.gameObject.SetActive(true);
        return poolableObject;
    }

    /// <summary>
    /// Creates the pooled object.
    /// </summary>
    /// <returns>The pooled object.</returns>
    private PoolableObject CreatePooledObject()
    {
        if (pooledObject == null)
        {
            Debug.LogError("PoolableObject prefab is not set.");
            return null;
        }
        if(this.transform.childCount >= maxPoolSize)
        {
            Debug.LogWarning("Will not create PoolableObject due to exceed maxPoolSize(=" + maxPoolSize + ")");
            return null;
        }

        PoolableObject obj = Instantiate(pooledObject);
        obj.Pool = this;
        obj.transform.SetParent(this.transform);
        return obj;
    }

    /// <summary>
    /// Returns the pooled object.
    /// </summary>
    /// <param name="_poolableObject">Poolable object.</param>
    public void ReturnPooledObject(PoolableObject _poolableObject)
    {
        if (_poolableObject == null)
        {
            return;
        }
        _poolableObject.gameObject.SetActive(false);
        _poolableObject.transform.SetParent(this.transform);
        queue.Enqueue(_poolableObject);
    }

    /// <summary>
    /// Initializes the pool.
    /// </summary>
    private void InitializePool()
    {
        if (queue != null) return;
        queue = new Queue<PoolableObject>();
    }

    /// <summary>
    /// Clears the pooled objects.
    /// </summary>
    /// <param name="_clearImmediately">If set to <c>true</c> clear right now. Else will destroy when returned to pool.</param>
    public void ClearPooledObjects(bool _clearImmediately = false)
    {
        foreach (Transform t in this.transform)
        {
            // Destroy all no matter if PoolObject is active or not.
            if (_clearImmediately)
            {
                Destroy(t.gameObject);
            }
            else
            {
                PoolableObject poolObj = t.GetComponent<PoolableObject>();
                // If PoolObject is currently used, reserve for destory.
                if (poolObj != null && !queue.Contains(poolObj))
                {
                    poolObj.ReserveDestroyOnReturnPool(true);
                }
                // If PoolObject is not currently used, destroy.
                else
                {
                    Destroy(t.gameObject);
                }

            }
        }

        PoolableObject obj = GetPooledObject();
        while (obj != null)
        {
            Destroy(obj);
            obj = GetPooledObject();
        }
    }

    /// <summary>
    /// Destories the pool object prefab.
    /// </summary>
    public void DestoryPoolObjectPrefab()
    {
        if(pooledObject != null)
        {
            Destroy(pooledObject.gameObject);
        }
    }

}
