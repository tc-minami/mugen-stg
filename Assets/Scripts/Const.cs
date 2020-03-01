using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Const
{
    public static class Tag
    {
        public static string AllyBullet = "AllyBullet";
        public static string EnemyBullet = "EnemyBullet";
        public static string PlayerUnit = "PlayerUnit";
        public static string AllyUnit = "AllyUnit";
        public static string EnemyUnit = "EnemyUnit";

        public static bool IsUnit(string _tag)
        {
            return _tag == PlayerUnit
                || _tag == AllyUnit
                || _tag == EnemyUnit;
        }

        public static bool IsBullet(string _tag)
        {
            return _tag == AllyBullet
                || _tag == EnemyBullet;
        }

        public static bool IsAllyUnit(string _tag)
        {
            return _tag == PlayerUnit
                || _tag == AllyUnit;
        }

        public static bool IsEnemyUnit(string _tag)
        {
            return _tag == EnemyUnit;
        }

        public static bool IsAllyBullet(string _tag)
        {
            return _tag == AllyBullet;
        }

        public static bool IsEnemyBullet(string _tag)
        {
            return _tag == EnemyBullet;
        }

        public static bool IsBulletHitValid(string _unitTag, string _bulletTag)
        {
            return (IsAllyUnit(_unitTag) && IsEnemyBullet(_bulletTag))
                && (IsEnemyUnit(_unitTag) && IsAllyBullet(_bulletTag));
        }
    }
}
