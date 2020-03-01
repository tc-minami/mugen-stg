using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Unit : MonoBehaviour
{
    private BulletInfo bulletInfo;
    [SerializeField]
    private UnitType unitType;
    private UnitInfo unitInfo;

    private BulletManager bulletManager;

    public static Unit CreateNewUnit(Transform _parent, UnitType _unitType, string _name = "")
    {
        return GameObjectUtil.CreateInstance<Unit>(
            _parent,
            _name.Length <= 0 ? _unitType.ToString() : _name);
    }

    /// <summary>
    /// Called when bullet hits any object.
    /// </summary>
    /// <param name="_col">Col.</param>
    private void OnTriggerEnter2D(Collider2D _col)
    {
        if (Const.Tag.IsBullet(_col.tag)
                    && Const.Tag.IsBulletHitValid(this.tag, _col.tag))
        {
            // Get Damage
        }
    }

    void Start()
    {
        SpriteRenderer spriteRenderer = this.gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = GetUnitSprite();

        bulletManager = GameObjectUtil.CreateInstance<BulletManager>(this.transform, "BulletManager");
        bulletManager.IsAllyBullet = true;
        bulletManager.UpdateBulletType(BulletType.Normal);
    }

    // Update is called once per frame
    void Update()
    {
        //bulletPool.GetOrCreate();
    }

    private Sprite GetUnitSprite()
    {
        switch(unitType)
        {
            case UnitType.Player:
            default:
                return Resources.Load<Sprite>("Sprites/unit");
        }
    }
}
