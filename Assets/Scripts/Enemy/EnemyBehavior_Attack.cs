using UnityEngine;

[CreateAssetMenu(fileName = "EnemyBehavior_Attack_", menuName = "ScriptableObjects/EnemyBehavior/Attack", order = 1)]
public class EnemyBehavior_Attack : EnemyBehavior
{
    public int attackDamage = 10;

    public override void TakeAction()
    {
        Debug.Log($"Enemy attacks for {attackDamage} damage!");
    }
}
