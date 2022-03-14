using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject zombie;
    private float SpawnXPos;
    private float SpawnZPos;
    private int spawnDelay;
    public int zombieLimit = 20;
    private bool spawn = false;

    void Start()
    {
        //SpawnZombie();
    }

    // Update is called once per frame
    void Update()
    {
        //StartCoroutine(Spawn());
        if(zombieLimit > 0)
        {
            zombieLimit -= 1;
            SpawnZombie();
        } else {
            zombieLimit = -1;
        }
    }

    void SpawnZombie()
    {
        SpawnXPos = Random.Range(-40f, 40f);
        SpawnZPos = Random.Range(20f, 80f);
        Instantiate(zombie, new Vector3( SpawnXPos, 2.96f, SpawnZPos), Quaternion.identity);
    }

    /*IEnumerator Spawn()
    {
        spawnDelay = Random.Range(1,3);
        spawn = true;
        yield return new WaitForSeconds(spawnDelay);
        spawn = false;
    }*/
}
