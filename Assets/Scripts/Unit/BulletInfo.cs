using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletHitType
{
    Default,
    HitExplode,
    Trumple,
}

public class BulletInfo
{
    private float sizePercentage = 100.0f;
    private float damage = 1.0f;

    private BulletHitType hitType = BulletHitType.Default;

    private Sprite image;

}
