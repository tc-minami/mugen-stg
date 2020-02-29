using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitType
{
    Player,
    Enemy,
    SupportItem
}

public class Unit : MonoBehaviour
{
    private BulletInfo bulletInfo;
    [SerializeField]
    private UnitType unitType;

    // Bullet
    private GenericPool bulletPool = null;
    private bool autoFire = true;

    void Start()
    {
        PoolableObject bullet = PoolableObject.CreateNewPoolableObject<Bullet>(this.transform, "Bullet");
        bulletPool = GenericPool.CreateNewPoolWithPrefab(bullet, this.transform);
        bulletPool.SetMaxPoolSize(5);
        Object.Destroy(bullet);
    }

    // Update is called once per frame
    void Update()
    {
        bulletPool.GetOrCreate();
    }
}
