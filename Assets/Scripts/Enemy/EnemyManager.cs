using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public float xSpawnOffset = 10f;
    public List<EnemyAI> availableGroundEnemies;
    public List<EnemyAI> availableAirEnemies;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    //Gets a list of all towers and picks a random one to assign as the current segment to focus
    public GameObject FocusSegment()
    {
        GameObject[] towerSegments = GameObject.FindGameObjectsWithTag("Tower");

        return towerSegments[Random.Range(0, towerSegments.Length)];
    }
    //Targets a specific index in the list of tower segments, used for ground troops only right now
    public GameObject FocusSegment(int index)
    {
        GameObject[] towerSegments = GameObject.FindGameObjectsWithTag("Tower");
        if(towerSegments.Length > 0)
            return towerSegments[index];
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            SpawnGroundEnemy();
        else if (Input.GetKeyDown(KeyCode.R))
            SpawnAirEnemy();
    }

    void SpawnAirEnemy()
    {
        GameObject focus = FocusSegment();
        Vector3 spawnPosition = focus.transform.position + new Vector3(xSpawnOffset, 0 , 0);
        EnemyAI newEnemy = Instantiate(availableAirEnemies[0], spawnPosition, Quaternion.identity);
        newEnemy.target = focus;
        
    }

    void SpawnGroundEnemy()
    {
        GameObject focus = FocusSegment(0);
        Vector3 spawnPosition = focus.transform.position + new Vector3(xSpawnOffset, -focus.transform.position.y, 0);
        EnemyAI newEnemy = Instantiate(availableGroundEnemies[0], spawnPosition, Quaternion.identity);
        newEnemy.target = focus;
    }
}
