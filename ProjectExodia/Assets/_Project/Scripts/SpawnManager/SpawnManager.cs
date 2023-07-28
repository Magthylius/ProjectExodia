using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ProjectExodia
{
    public class SpawnManager : ManagerBase
    {
        [SerializeField]private GameObject[] enemyPrefabs;
        [SerializeField]private GameObject tileManagerObj;
        [SerializeField] private float SpawnDistance;
        [SerializeField] private float SpawnGap;
        [SerializeField]private float spawnTime;
        [SerializeField]private float yPositionOffset;

        private List<GameObject> activeEnemies;
        private float timerElapsed;
       
        private int lastPrefabIndex = 0;

        private TileManager tileManager;
        public event Action<GameObject> OnDeleteEnemy;
        
        private void Start()
        {
            activeEnemies = new List<GameObject>();
            if (tileManagerObj.TryGetComponent<TileManager>(out TileManager pTileManager))
            {
                tileManager = pTileManager;
            }
            else
            {
                Debug.LogError("Missing TileManager, disabling Enemy Manager");
                gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            timerElapsed += Time.deltaTime;

            if (timerElapsed >= spawnTime)
            {
                SpawnEnemy();
                timerElapsed = 0;
            }
        }

        void SpawnEnemy()
        {
            GameObject goEnemy = Instantiate(enemyPrefabs[RandomPrefabIndex()], transform, true);

            float randomGap = Random.Range(-SpawnGap, SpawnGap);

            goEnemy.transform.position = new Vector3(randomGap, (tileManagerObj.transform.position.y + yPositionOffset),
                tileManager.GetLastSpawnLocation().z);
            activeEnemies.Add(goEnemy);
        }

        void DeleteEnemy(GameObject pEnemyToRemove)
        {
            GameObject enemyToRemove = activeEnemies.Find(obj => obj == pEnemyToRemove);
            if (enemyToRemove != null)
            {
                Destroy(enemyToRemove, 5.0f);
                activeEnemies.Remove(pEnemyToRemove);
            }
        }
        
        private int RandomPrefabIndex()
        {
            if (enemyPrefabs.Length <= 1) return 0;

            int randomIndex = lastPrefabIndex;
            
            while (randomIndex == lastPrefabIndex)
                randomIndex = Random.Range(0, enemyPrefabs.Length);

            lastPrefabIndex = randomIndex;
            return randomIndex;
        }

        private void OnDrawGizmosSelected()
        {

            Transform tileManagerTransform = tileManagerObj.transform;
            Vector3 MinBoundaryLocation = new Vector3((tileManagerTransform.position.x + SpawnGap),
                tileManagerTransform.position.y, tileManagerTransform.position.z);
            
            Vector3 MaxBoundaryLocation = new Vector3((tileManagerTransform.position.x + -SpawnGap),
                tileManagerTransform.position.y, tileManagerTransform.position.z);
            
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(MinBoundaryLocation, new Vector3( 1, 1, 1));
            Gizmos.DrawWireCube(MaxBoundaryLocation, new Vector3( 1, 1, 1));
        }
    }
}
