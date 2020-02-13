using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public int enemyCount;
        public float TimeBetweenWaves;
    } 
    
    public Wave[] Waves;
    public Enemy enemy;

    private LivingEntity playerEntity;
    private Transform playerT;
    
    private Wave currentWave;
    private int currentWaveNumber;

    private int enemiesRemainingToSpawn;
    private int enemiesRemainingAlive;
    private float nextSpawnTime;

    private MapGenerator Map;

    private float timeBetweenCampingChecks = 2f;
    private float campThresholdDistance = 1.5f;
    private float nextCampCheck;
    private Vector3 campPositionHold;
    private bool isCamping;
    private bool isDisabled;

    public event Action<int> OnNewWave;
    
    private void Start()
    {
        playerEntity = FindObjectOfType<Player>();
        playerT = playerEntity.transform;
        nextCampCheck = timeBetweenCampingChecks + Time.time;
        campPositionHold = playerT.position;
        playerEntity.OnDeath += onPlayerDeath;
        
        Map = FindObjectOfType<MapGenerator>(); 
        NextWave();
    }

    private void Update()
    {
        if (!isDisabled)
        {
            if (Time.time > nextCampCheck)
            {
                nextCampCheck = Time.time + timeBetweenCampingChecks;
                isCamping = (Vector3.Distance(playerT.position, campPositionHold) < campThresholdDistance);
                campPositionHold = playerT.position;
            }
        
            if (enemiesRemainingToSpawn > 0 && Time.time > nextSpawnTime)
            {
                enemiesRemainingToSpawn--;
                nextSpawnTime = Time.time + currentWave.TimeBetweenWaves;

                StartCoroutine(SpawnEnemy());
            }
        }
    }

    IEnumerator SpawnEnemy()
    {
        float spawnDelay = 1;
        float tileFlashSpeed = 4;
        
        Transform spawnTile = Map.GetRandomOpenTile();
        if (isCamping)
        {
            spawnTile = Map.GetTileFromPosition(playerT.position);
        }
        Material tileMat = spawnTile.GetComponent<Renderer>().material;
        Color initialColor = tileMat.color;
        Color flashColor = Color.red;
        float spawnTimer = 0;

        while (spawnTimer < spawnDelay)
        {
            tileMat.color = Color.Lerp(initialColor, flashColor, Mathf.PingPong(spawnTimer * tileFlashSpeed, 1));
            
            spawnTimer += Time.deltaTime;
            yield return null;
        }
        Enemy spawnedEnemy = Instantiate(enemy, spawnTile.position + Vector3.up, Quaternion.identity);
        spawnedEnemy.OnDeath += OnEnemyDeath;
    }

    void onPlayerDeath()
    {
        isDisabled = true;
    }
    
    void OnEnemyDeath()
    {
        print("enemy died");
        enemiesRemainingAlive--;
        if (enemiesRemainingAlive == 0)
        {
            NextWave();
        }
    }

    void NextWave()
    {
        currentWaveNumber++;
        //print("wave " + currentWaveNumber);
        if (currentWaveNumber - 1 < Waves.Length)
        {
            currentWave = Waves[currentWaveNumber - 1];

            enemiesRemainingToSpawn = currentWave.enemyCount;
            enemiesRemainingAlive = enemiesRemainingToSpawn;

            if (OnNewWave != null)
            {
                OnNewWave(currentWaveNumber);
            }
            ResetPlayerPosition();
        }
    }

    void ResetPlayerPosition()
    {
        playerT.position = Map.GetTileFromPosition(Vector3.zero).position + Vector3.up * 3;
    }
}
