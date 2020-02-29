using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableObject : MonoBehaviour
{
    public GenericPool Pool { private get; set; }
    [SerializeField]
    private bool isDestroyOnReturnPool = false;

    public static T CreateNewPoolableObject<T>(Transform _parent, string _name) where T : PoolableObject
    {
        return GameObjectUtil.CreateInstance<T>(_parent, _name);
    }

    public void ReserveDestroyOnReturnPool(bool _doDestroy = true)
    {
        isDestroyOnReturnPool = _doDestroy;
    }

    protected void Return2Pool()
    {
        if (isDestroyOnReturnPool)
        {
            Destroy(gameObject);
        }
        else
        {
            Pool.ReturnPooledObject(this);
        }
    }
}
