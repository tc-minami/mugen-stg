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

    private BulletManager bulletManager;

    void Start()
    {
        bulletManager = GameObjectUtil.CreateInstance<BulletManager>(this.transform, "BulletManager");
        bulletManager.UpdateBulletType(BulletType.Normal);
    }

    // Update is called once per frame
    void Update()
    {
        //bulletPool.GetOrCreate();
    }
}
