using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : PoolableObject
{
    public BulletInfo bulletInfo = new BulletInfo();

    public void UpdateBulletInfo(BulletType _bulletType)
    {
        bulletInfo.UpdateBulletInfo(_bulletType);
    }

    public void ApplyBulletInfo()
    {
        transform.localScale = new Vector2(
                bulletInfo.sizeScale,
                bulletInfo.sizeScale);
    }

    public void InitBullet(BulletType _bulletType, bool _isAlly)
    {
        UpdateBulletInfo(_bulletType);

        SpriteRenderer bulletRenderer = gameObject.AddComponent<SpriteRenderer>();
        bulletRenderer.sprite = Resources.Load<Sprite>(bulletInfo.spriteFilePath);
        CircleCollider2D col = gameObject.AddComponent<CircleCollider2D>();
        col.isTrigger = true;
        col.tag = _isAlly ? Const.Tag.AllyBullet : Const.Tag.EnemyBullet;
    }

    /// <summary>
    /// Called when bullet hits any object.
    /// </summary>
    /// <param name="_col">Col.</param>
    void OnTriggerEnter2D(Collider2D _col)
    {
        if(Const.Tag.IsUnit(_col.tag)
            && Const.Tag.IsBulletHitValid(_col.tag, this.tag))
        {
            Return2Pool();
        }
    }

    /// <summary>
    /// Called when bullet hits some object.
    /// </summary>
    public void OnHit()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = this.transform.position;
        Vector2 vel = bulletInfo.velocityPerSec * Time.deltaTime;
        this.transform.position = new Vector3(pos.x + vel.x, pos.y + vel.y, pos.z);
    }
}
