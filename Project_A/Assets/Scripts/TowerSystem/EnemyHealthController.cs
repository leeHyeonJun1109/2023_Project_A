using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthController : MonoBehaviour
{
    public int totalHealth;
    // Start is called before the first frame update
    public void TakeDamage(int damageAmount)
    {
        totalHealth -= damageAmount;

        if(totalHealth <= 0)
        {
            totalHealth = 0;
            Destroy(gameObject);
        }
    }
   
}
