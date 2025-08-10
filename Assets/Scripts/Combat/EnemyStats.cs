using Cards.ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(fileName = "Stats_Enemy_", menuName = "ScriptableObjects/Combat/EnemyStats", order = 1)]
public class EnemyStats : CombatParticipantStats
{
    public CardDeck defeatRewards;

}
