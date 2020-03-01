using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletType
{
    None, // Just for default value.
    Normal,
}

public enum BulletHitType
{
    Default,
    HitExplode,
    Trumple,
}

public enum BulletTriggerType
{
    WhileTap,
    OnClick,
    Auto,
}

public enum BulletAimType
{
    Target,
    FixDir,
}

public static class BulletSpriteFilePath
{
    public const string Normal = "Sprites/bullet";
}

public class BulletInfo
{
    public float sizeScale = 1.0f;
    public float damage = 1.0f;
    public float shootPeriod = 1.0f;
    public float baseVelocity = 1.0f; // Basic speed of bullet
    public Vector2 velocityPerSec = new Vector2(0, 0); // Actual move speed of bullet per sec.

    public BulletHitType hitType = BulletHitType.Default;
    public string spriteFilePath = BulletSpriteFilePath.Normal;

    public void UpdateBulletInfo(BulletType _bulletType)
    {
        switch(_bulletType)
        {
            case BulletType.Normal:
                sizeScale = 0.3f;
                damage = 1.0f;
                shootPeriod = 1.0f;
                baseVelocity = 10.0f;
                hitType = BulletHitType.Default;
                spriteFilePath = BulletSpriteFilePath.Normal;
                break;
        }
    }

    public void SetBulletVelocity(Vector2 _startPos, Vector2 _destPos)
    {
        Vector2 dirVel = _destPos - _startPos;
        velocityPerSec = dirVel.normalized * baseVelocity;
    }
}
