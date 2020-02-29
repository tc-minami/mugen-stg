using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : PoolableObject
{
    public BulletInfo bulletInfo = new BulletInfo();

    public void UpdateBulletInfo(BulletType _bulletType)
    {
        bulletInfo.UpdateBulletInfo(_bulletType);
        SpriteRenderer bulletRenderer = this.gameObject.AddComponent<SpriteRenderer>();
        bulletRenderer.sprite = Resources.Load<Sprite>(bulletInfo.spriteFilePath);
    }

    public void SetBulletVelocity(Vector2 _startPos, Vector2 _destPos)
    {
        bulletInfo.SetBulletVelocity(_startPos, _destPos);
    }

    /// <summary>
    /// Called when bullet hits some object.
    /// </summary>
    public void OnHit()
    {
        Return2Pool();
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
