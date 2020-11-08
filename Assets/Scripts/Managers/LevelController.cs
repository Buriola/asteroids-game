using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Asteroids.Controller;

public class LevelController : MonoBehaviour
{
    #region Variables
    public static LevelController instance;

    [Header("Level Settings")]
    public Transform spawnPointsParent;
    public StateManager player;
    public List<Wave> waves = new List<Wave>();
    [HideInInspector] public int playerScore;
    [HideInInspector] public int highestScore;
    [HideInInspector] public int enemiesKilled;

    [Header("Audio Settings")]
    public AudioClip levelMusic;

    private int waveIndex; //Keep track of which index of the list I'm in
    private bool waveComplete;
    private int lastWaveIndex; //The last index
    private bool isLastWave;

    #endregion

    #region Unity Functions
    private void Awake()
    {
        instance = this;
    }

    private void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<StateManager>();

        waveIndex = 0;
        waveComplete = false;

        LoadHighScore();

        if (levelMusic != null)
            AudioHandler.PlayMusic(levelMusic, true);

        //Initialize game if there are waves 
        if (waves.Count > 0)
        {
            StartCoroutine(StartWave());
            lastWaveIndex = waves.Count - 1;
        }
        else
            Debug.Log("No waves set in the inspector, please verify.");
	}
	
	private void Update ()
    {
        //If the player dies, we should stop the coroutine from spawning more enemies
        if(player.isDead)
        {
            StopAllCoroutines(); //Stop everything

            //Save new high score
            if (playerScore > highestScore)
                SaveHighScore(playerScore);

            HUD.instance.retryMenu.gameObject.SetActive(true); //Call the retry menu
        }

        //If a wave finish we should make some checks
        if (waveComplete)
        {
            if (waveIndex < lastWaveIndex)
            {
                waveIndex++;
                waveComplete = false;
                StartCoroutine(StartWave());
                enemiesKilled = 0;
                return;
            }
            else if (waveIndex == lastWaveIndex && !isLastWave) //This will ensure that last wave to run, wont skip it
            {
                waveComplete = false;
                isLastWave = true;
                StartCoroutine(StartWave());
                enemiesKilled = 0;
                return;
            }
            else if (isLastWave) //resets everything to the first wave
            {
                isLastWave = false;
                waveIndex = 0;
                waveComplete = false;
                StartCoroutine(StartWave());
                enemiesKilled = 0;

                if (playerScore > highestScore)
                    SaveHighScore(playerScore);
                return;
            }
        }
        else return;
	}

    #endregion

    #region My Functions

    /// <summary>
    /// This method is responsible for the wave spawner. It will spawn waves based on the current index of the list
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartWave()
    {
        
        yield return new WaitForSeconds(waves[waveIndex].startTime); // Wait to start

        while(enemiesKilled < waves[waveIndex].numberOfEnemies) //While not every enemy of that wave is killed
        {
            yield return new WaitForSeconds(waves[waveIndex].spawnRate); // We'll keep spawning during this rate
            string tag = TakeOneEnemyFromWave();
            int r = RandomSpawnPoint();

            GameObject enemy = ObjectPooler.Instance.GetEnemyObject(tag); //Getting from the pool of enemies
            //Positioning and enabling.
            enemy.transform.position = spawnPointsParent.GetChild(r).position;
            enemy.transform.rotation = spawnPointsParent.GetChild(r).rotation;
            enemy.SetActive(true);
        }

        //After the wave finishes, we set this to close the loop
        waveComplete = true;
    }

    /// <summary>
    /// This will select an enemy tag from the list of enemies within the wave based on a rate/weight that was set 
    /// for each enemy
    /// </summary>
    /// <returns>The enemy tag</returns>
    private string TakeOneEnemyFromWave()
    {
        string retVal = "";
        float rateSum = 0;

        for (int i = 0; i < waves[waveIndex].waveEnemies.Count; i++)
        {
            float rate = waves[waveIndex].waveEnemies[i].rate;
            float r = Random.Range(0, rateSum + rate);
            if (r >= rateSum)
                retVal = waves[waveIndex].waveEnemies[i].enemyTag;

            rateSum += waves[waveIndex].waveEnemies[i].rate;
        }
        return retVal;
    }

    /// <summary>
    /// Choose a random spawnPoint from the child transforms of the SpawnPointsParent
    /// </summary>
    /// <returns>The spawn point index</returns>
    private int RandomSpawnPoint()
    {
        int retVal = 0;

        int count = spawnPointsParent.childCount;
        retVal = Random.Range(0, count - 1);

        return retVal;
    }

    /// <summary>
    /// Simple Player Prefs save to keep track of the highest score of each level
    /// </summary>
    /// <param name="score"></param>
    private void SaveHighScore(int score)
    {
        highestScore = score;
        //Adding the buildIndex as a suffix will ensure a unique key for every level
        PlayerPrefs.SetInt("HighScore" + SceneManager.GetActiveScene().buildIndex.ToString(), highestScore);
    }

    /// <summary>
    /// Loads from the Player Prefs the highest score of that level
    /// </summary>
    private void LoadHighScore()
    {
        highestScore = PlayerPrefs.GetInt("HighScore" + SceneManager.GetActiveScene().buildIndex);
    }

    #endregion
}
