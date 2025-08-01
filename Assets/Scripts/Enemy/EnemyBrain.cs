using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    [SerializeField] private EnemyBehavior[] enemyBehaviors;

    public void TakeAction()
    {
        int behaviorIndex = Random.Range(0, enemyBehaviors.Length);
        EnemyBehavior selectedBehavior = enemyBehaviors[behaviorIndex];
        selectedBehavior.TakeAction();
    }
}
