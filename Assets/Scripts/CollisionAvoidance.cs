using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAvoidance : MonoBehaviour
{
    public Enemy enemy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        enemy.StopMovement();
        enemy.ChangeState(EnemyStates.IsMoving);
    }
}
