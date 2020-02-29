using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableObject : MonoBehaviour
{
    public GenericPool Pool { private get; set; }
    [SerializeField]
    private bool isDestroyOnReturnPool = false;

    public static PoolableObject CreateNewPoolableObject(Transform _parent, string _name)
    {
        return GameObjectUtil.CreateInstance<PoolableObject>(_parent, _name);
    }

    public static PoolableObject CreateNewPoolableObject<T>(Transform _parent, string _name) where T : MonoBehaviour
    {
        PoolableObject poolableObj = CreateNewPoolableObject(_parent, _name);
        poolableObj.gameObject.AddComponent<T>();
        return poolableObj;
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

    //protected new void Destroy(Object _obj)
    //{
    //    Return2Pool();
    //}
}
