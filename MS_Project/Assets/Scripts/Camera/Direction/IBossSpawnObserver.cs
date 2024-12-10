using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBossSpawnObserver
{
    void OnBossSpawned(GameObject boss);
    void OnBossDead();
}
