using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //Variables
    public float health;
    private GameObject player;
    public float pointsToGive = 5f;

    //Methods
    public void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    public void Update()
    {
        //Player is dead
        if(health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Enemy dead");
        player.GetComponent<Player>().points += pointsToGive;
        Destroy(this.gameObject);
    }
}
