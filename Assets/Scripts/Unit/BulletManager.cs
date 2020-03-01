using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages bullet using GenericPool.
/// </summary>
public class BulletManager : MonoBehaviour
{
    public bool IsAllyBullet { get; set; }

    private GenericPool bulletPool = null;
    private BulletType bulletType = BulletType.None;
    private BulletTriggerType bulletTriggerType = BulletTriggerType.Auto;
    private Timer bulletShootTimer = null;

    private BulletAimType aimType = BulletAimType.FixDir;
    // Bullet count per shot
    private int unitBulletShootCount = 1;
    // Bullet shoot period
    private float bulletShootPeriod = 1.0f;
    // Bullet start, end pos;
    private Vector2[] startPos = new Vector2[] { new Vector2(0.0f, 0.0f) };
    private Vector2[] destPos = new Vector2[] { new Vector2(0.0f, 1.0f) };
    private Transform[] targets = new Transform[] { null };


    public void UpdateBulletType(BulletType _bulletType)
    {
        // If bullet type is same, do nothing.
        if (_bulletType == this.bulletType) return;

        this.bulletType = _bulletType;

        // For no bullet, just clear pool.
        if (this.bulletType == BulletType.None)
        {
            bulletPool?.ClearPooledObjects();
            return;
        }

        // Once bullet type is changed, need to re-init pool as well.
        InitBulletPool();

        // Run timer so that first spawn time comes immediately.
        RunBulletShootTimer(0.0f);
    }

    /// <summary>
    /// Shoots the bullet.
    /// Will return whether bullet is successfully shot.
    /// </summary>
    /// <returns><c>true</c>, if bullet was shot, <c>false</c> otherwise.</returns>
    public bool ShootBullet()
    {
        RunBulletShootTimer(bulletShootPeriod);

        bool isBulletShot = false;
        if (this.bulletTriggerType != BulletTriggerType.Auto)
        {
            // If timer is still active (still counting down), cannot shoot yet.
            if (bulletShootTimer.IsTimerActive())
            {
                return isBulletShot;
            }
        }

        for(int i = 0; i < unitBulletShootCount; i++)
        {
            // In case pool count exceeds max, it may return null.
            Bullet bullet = bulletPool.GetOrCreate<Bullet>();
            if (bullet == null)
            {
                continue;
            }

            // Update bullet status.
            bullet.UpdateBulletInfo(this.bulletType);
            bullet.ApplyBulletInfo();

            bullet.gameObject.SetActive(true);
            if(this.aimType == BulletAimType.FixDir)
            {
                bullet.bulletInfo.SetBulletVelocity(startPos[i], destPos[i]);
            }
            else
            {
                bullet.bulletInfo.SetBulletVelocity(startPos[i], targets[i].transform.position);
            }
            isBulletShot = true;
        }
        return isBulletShot;
    }


    private void InitBulletPool()
    {
        if (bulletPool == null)
        {
            bulletPool = GenericPool.CreateNewPool(this.transform, "BulletPool", 100);
        }

        Bullet bullet = CreateBulletPrefab(this.bulletType);
        bulletPool.SetPoolableObject(bullet, "Bullet", true);
        Destroy(bullet.gameObject);
    }

    private Bullet CreateBulletPrefab(BulletType _bulletType)
    {
        Bullet bulletPoolObj = PoolableObject.CreateNewPoolableObject<Bullet>(this.transform, "Bullet");
        bulletPoolObj.InitBullet(_bulletType, IsAllyBullet);

        bulletShootPeriod = bulletPoolObj.bulletInfo.shootPeriod;
        return bulletPoolObj;
    }

    /// <summary>
    /// Runs the bullet shoot timer.
    /// </summary>
    /// <param name="_period">Period.</param>
    private void RunBulletShootTimer(float _period)
    {
        if (bulletShootTimer == null)
        {
            bulletShootTimer = this.gameObject.AddComponent<Timer>();
            bulletShootTimer.SetOnCompleteCallback(OnCompleteBulletSpawnTimer);
        }

        bulletShootTimer.RunTimer(_period, false);
    }

    /// <summary>
    /// On the complete bullet spawn timer.
    /// If bullet trigger is auto, shoot bullet every time timer is complete.
    /// </summary>
    private void OnCompleteBulletSpawnTimer()
    {
        if (this.bulletTriggerType == BulletTriggerType.Auto)
        {
            ShootBullet();
        }
    }
}
