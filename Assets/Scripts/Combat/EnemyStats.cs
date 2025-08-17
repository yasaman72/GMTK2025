using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stats_Enemy_", menuName = "ScriptableObjects/Combat/EnemyStats", order = 1)]
public class EnemyStats : CombatCharacterStats
{
    public LootSet defeatRewards;
    public List<EnemyAction> EnemyActions;
}
