using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yamap
{
    public class Weapon_TearGasGrenade : BulletDetailBase
    {
        public override void AddTriggerBullet(EnemyController enemyController)
        {
            enemyController.PrepareTearGasGrenade();
        }
    }
}