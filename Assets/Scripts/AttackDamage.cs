using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDamage : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D other)
    {
        PlayerMovement player;
        EnemyController enemy;
        if (player = other.GetComponent<PlayerMovement>())
        {
            player.DoDmg();
        }
        else if(enemy = other.GetComponent<EnemyController>())
        {
            enemy.DoDmg();
        }
    }
}
