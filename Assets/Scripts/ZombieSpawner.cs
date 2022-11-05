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
    public int zombieLimit = 0;
    public int dayLimit = 5;
    public int nightLimit = 15;
    private bool spawn = false;
    private float SpawnerX;
    private float SpawnerZ;
    private float SpawnerXScale;
    private float SpawnerZScale;
    public GameObject GameManager;
    public int NightDelay = 10;
    public int DayDelay = 30;
    
    void Start()
    {
        SpawnerX = transform.position.x;
        SpawnerZ = transform.position.z;
        SpawnerXScale = transform.localScale.x / 2;
        SpawnerZScale = transform.localScale.z / 2;
    }

    // Update is called once per frame
    void Update()
    {
        if(zombieLimit > 0)
        {
            zombieLimit -= 1;
            SpawnZombie();
        } else {
            zombieLimit = -1;
        }
        //Respawn zombie
        if(spawn == false)
        {
            StartCoroutine(Spawn());
        }
    }

    void SpawnZombie()
    {
        SpawnXPos = Random.Range(SpawnerX - SpawnerXScale, SpawnerX + SpawnerXScale);
        SpawnZPos = Random.Range(SpawnerZ - SpawnerZScale, SpawnerZ + SpawnerZScale);
        Instantiate(zombie, new Vector3( SpawnXPos, 2.96f, SpawnZPos), Quaternion.identity);
    }

    IEnumerator Spawn()
    {
        if(GameManager.GetComponent<DayNightTime>().night == false) {
            Debug.Log("Day Spawn");
            zombieLimit = dayLimit;
            spawnDelay = Random.Range(DayDelay,DayDelay+5);
            spawn = true;
            SpawnZombie();
            yield return new WaitForSeconds(spawnDelay);
            spawn = false;
        } else {
            Debug.Log("Night Spawn");
            zombieLimit = nightLimit;
            spawnDelay = Random.Range(NightDelay,NightDelay+5);
            spawn = true;
            SpawnZombie();
            yield return new WaitForSeconds(spawnDelay);
            spawn = false;
        }
    }
}
