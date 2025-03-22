using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.Audio;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    AudioResource waveStart;
    [SerializeField]
    AudioResource waveOver;
    public float xSpawnOffset = 10f;
    public float xSpawnVariation = 0.25f;
    public float ySpawnVariation = 2f;
    public List<EnemyAI> availableEnemies;
    public Dictionary<EnemyAI, int> enemyCost = new Dictionary<EnemyAI, int>();
    public float waveDuration;
    public float numSpawnTimes = 5;
    float spawnIntervals;
    public float waveStartDelay;
    public int[] levelPoints;
    int currentLevel = 0;
    int currentSpawnTime;
    public MiniWave miniWave;
    GameObject currentFocus;
    int focusedIndex;
    GameObject[] towerSegments;
    int cheapestEnemyCost = int.MaxValue;
    int highestEnemyCost = int.MinValue;
    public bool activeWave = false;
    
    public static EnemyManager instance;

    
    private IEnumerator wave;
    
    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public bool GetActiveWave(){
        return activeWave;
    }

    //Grabs all the enemies, spawns them in so we can access their cost
    //then obliterates them after they've served their purpose
    void Start()
    {
        foreach(EnemyAI enemyAI in availableEnemies){
            Debug.Log("checking" + enemyAI.name);
            EnemyAI tempAI = Instantiate(enemyAI, new Vector3(10000, 10000, 0), Quaternion.identity);
            enemyCost.Add(enemyAI, tempAI.GetSpawnPoints());
            if(tempAI.GetSpawnPoints() < cheapestEnemyCost){
                cheapestEnemyCost = tempAI.GetSpawnPoints();
            } else if (tempAI.GetSpawnPoints() > highestEnemyCost){
                highestEnemyCost = tempAI.GetSpawnPoints();
            }
            Destroy(tempAI.gameObject);
        }
        Debug.Log("Cheapest enemy is " + cheapestEnemyCost);
        Debug.Log("Expensive enemy is " + highestEnemyCost);
        Debug.Log(enemyCost.Keys);
    }

    public void StartWave(){
        if(activeWave){return;}
        activeWave = true;
        SoundFXManager.instance.PlaySoundFXClip(waveStart, transform, 1f);
        spawnIntervals =  waveDuration / numSpawnTimes;
        currentSpawnTime = 1;
        currentFocus = FocusSegment();
        Debug.Log("Starting wave " + currentLevel + 
                "\nTargeting Tower at " + currentFocus.transform.position + 
                "\nAvailable points to spend: " + GetLevelPoints(currentLevel) + 
                "\nWave Duration: " + waveDuration +
                 "\nSpawning " + numSpawnTimes + " times, once every " + spawnIntervals + " seconds");
        wave = SpawnCoroutine(spawnIntervals, GetLevelPoints(currentLevel));
        StartCoroutine(wave);
    }

    IEnumerator SpawnCoroutine(float waitTime, int points){
        int remainingPoints = points; 
        Debug.Log("in spawn routine");
        do{
            yield return new WaitForSeconds(waveStartDelay);
            //Makes it to where the last wave has a lot more points to work with
            int miniWavePoints = (int) (remainingPoints / Math.Floor(numSpawnTimes * 1.5));
            
            //Skip miniwaves until we have enough points to spawn something
            while(miniWavePoints < cheapestEnemyCost || currentSpawnTime == numSpawnTimes){
                miniWavePoints += (int) (remainingPoints / Math.Floor(numSpawnTimes * 1.5));
                currentSpawnTime++;
            }
            //if its the last wave we use the rest of the points
            if(currentSpawnTime >= numSpawnTimes){

                miniWavePoints = remainingPoints;  
                Debug.Log("Creating miniwave " + currentSpawnTime + " with points: " + miniWavePoints);
                MiniWave finalMiniWave = StartMiniWave(miniWavePoints);
                miniWave.miniWaveOver = waveOver;

                while(finalMiniWave != null){//waiting until the miniwave is over
                    yield return new WaitForFixedUpdate();
                }
                break;
            }
            
            Debug.Log("Creating miniwave " + currentSpawnTime + " with points: " + miniWavePoints);
            MiniWave newMiniWave = StartMiniWave(miniWavePoints);
            // while(newMiniWave != null){//waiting until the miniwave is over
            //     yield return new WaitForFixedUpdate();
            // }

            currentSpawnTime++;
            yield return new WaitForSeconds(waitTime);
        } while (remainingPoints >= cheapestEnemyCost && currentSpawnTime <= numSpawnTimes);
        Debug.Log("end spawn routine");
        FinishWave();
    }

    //Tells the selection manager to display the win reward
    //Tells the tower manager to full heal all the segments
    private void FinishWave()
    {
        activeWave = false;
        FindAnyObjectByType<SelectionManager>().NewSelections();
        FindAnyObjectByType<TowerManager>().HealAllSegments();
        FindAnyObjectByType<ShopManager>().InitializeShop();
        currentLevel++;
    }

    //Recursively randomly picks from the list of available enemies until a spawnable one is found.
    //This assumes that remainingPoints > cheapestEnemyCost, which should be true.
    private EnemyAI PickEnemy(int remainingPoints)
    {
        EnemyAI choice = availableEnemies[UnityEngine.Random.Range(0, availableEnemies.Count)];
        if(enemyCost[choice] <= remainingPoints){
            return choice;
        } else {
            return PickEnemy(remainingPoints);
        }
    }
    //Creates a miniWave Object, the instantiates the miniwave enemies as its children until we run out of points
    private MiniWave StartMiniWave(int points)
    {
        MiniWave newMiniWave = Instantiate(miniWave, transform.position, Quaternion.identity, transform);
        while (points >= cheapestEnemyCost){
            EnemyAI choice = PickEnemy(points);
            points -= enemyCost[choice];
            SpawnEnemy(choice, newMiniWave.transform);
        }
        return newMiniWave;
    }

    //While we have hardcoded levels pull from the matrix, otherwise just calculate it
    int GetLevelPoints(int level){
        if(level < levelPoints.Count())
            return levelPoints[level];
        return (int) (10 * MathF.Pow(level, 1.2f));//PLACEHOLDER FUNCTION, probably want to think of a better one
    }

    //Gets a list of all towers and picks a random one to assign as the current segment to focus
    public GameObject FocusSegment()
    {
        towerSegments = GameObject.FindGameObjectsWithTag("Tower");
        focusedIndex = UnityEngine.Random.Range(0, towerSegments.Length);
        return towerSegments[focusedIndex];
    }
    //Targets a specific index in the list of tower segments, used for ground troops only right now
    public GameObject FocusSegment(int index)
    {
        towerSegments = GameObject.FindGameObjectsWithTag("Tower");
        if(towerSegments.Length > 0)
            return towerSegments[index];
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.E))
        //     SpawnGroundEnemy();
        // else if (Input.GetKeyDown(KeyCode.R))
        //     SpawnAirEnemy();
    }

    void SpawnAirEnemy(EnemyAI toSpawn, Transform parent)
    {
        int spawnIndex = Math.Clamp(focusedIndex + UnityEngine.Random.Range(-1, 2), 0, towerSegments.Length - 1);
        Vector3 spawnPosition = towerSegments[spawnIndex].transform.position + GetRandomSpawnOffset();
        int left = UnityEngine.Random.Range(0,2); 
        if(left == 0){
            spawnPosition += new Vector3(xSpawnOffset, 0 , 0);
        }else
        {
            spawnPosition += new Vector3(-xSpawnOffset, 0 , 0);
        }
        EnemyAI newEnemy = Instantiate(toSpawn, spawnPosition, Quaternion.identity, parent);
        newEnemy.target = currentFocus;
        
    }

    void SpawnGroundEnemy(EnemyAI toSpawn, Transform parent)
    {
        GameObject focus = FocusSegment(0);
        Vector3 spawnPosition = focus.transform.position + new Vector3(0, -focus.transform.position.y, 0);
        spawnPosition.x += UnityEngine.Random.Range(-xSpawnVariation,xSpawnVariation);
        int left = UnityEngine.Random.Range(0,2); 
        if(left == 0){
            spawnPosition += new Vector3(xSpawnOffset, 0 , 0);
        }else
        {
            spawnPosition += new Vector3(-xSpawnOffset, 0 , 0);
        }
        EnemyAI newEnemy = Instantiate(toSpawn, spawnPosition, Quaternion.identity, parent);
        newEnemy.target = focus;
    }

    Vector3 GetRandomSpawnOffset(){
        return new Vector3(UnityEngine.Random.Range(-xSpawnVariation,xSpawnVariation), UnityEngine.Random.Range(-ySpawnVariation,ySpawnVariation), 0);
    }

    void SpawnEnemy(EnemyAI toSpawn){
        SpawnEnemy(toSpawn, transform);
    }
    void SpawnEnemy(EnemyAI toSpawn, Transform parent){
        // i fucking hate doing this but I dont want to wastee time refactoring code rn
        // :P
        EnemyAI tempAI = Instantiate(toSpawn, new Vector3(10000, 10000, 0), Quaternion.identity);
        if(tempAI.IsGroundEnemy())
        {
            SpawnGroundEnemy(toSpawn, parent);
        } else 
        {
             SpawnAirEnemy(toSpawn, parent);
        }
        Destroy(tempAI.gameObject);//They've served their purpose, kill!!!
    }
}
