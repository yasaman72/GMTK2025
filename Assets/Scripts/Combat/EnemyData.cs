using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stats_Enemy_", menuName = "Scriptable Objects/Combat/EnemyData", order = 1)]
public class EnemyData : CombatCharacterStats
{
    public GameObject prefab;
    public LootSet defeatRewards;
    public List<EnemyAction> EnemyActions;
}
