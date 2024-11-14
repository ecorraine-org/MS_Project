using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIKnight_Shield : EnemyAction
{
    [SerializeField, Header("防御率"), Range(0f, 100f)]
    int guardChance = 50;

    // 確率で防御
    public void Guard()
    {
        int random = Random.Range(1, 10);
        if (random >= guardChance / 10)
        {
            enemy.Anim.Play("Guard", 0, 0f);
        }
    }

    #region オノマトペ情報
    private void KnightWalkData()
    {
        GenerateWalkOnomatopoeia();
    }

    private void KnightAttackData()
    {
        GenerateAttackOnomatopoeia();
    }
    #endregion
}
