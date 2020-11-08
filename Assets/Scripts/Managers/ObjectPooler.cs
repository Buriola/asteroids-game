using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents the Pooling System
/// </summary>
public class ObjectPooler : MonoBehaviour
{
    #region Singleton
    private static ObjectPooler instance;
    public static ObjectPooler Instance
    {
        get { return instance; }
    }
    #endregion

    #region Variables
    [Header("Player Pool")]
    public Transform playerProjectilesParent;
    List<GameObject> playerProjectilesPool;
    public GameObject projectile;

    [Header("Enemy Pool")]
    public Transform enemyParent;
    List<GameObject> enemyPool;
    public GameObject[] enemiesToPool;

    [Space]

    public Transform enemyProjectileParent;
    List<GameObject> enemyProjectilesPool;
    public GameObject enemyProjectile;

    [Space]

    [Range(1, 20)]
    public int amountToPool;
    #endregion

    #region Unity Functions
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null && instance != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        Init();
    }
    #endregion

    /// <summary>
    /// Initialization function to spawn all the objects on the first frame
    /// </summary>
    private void Init()
    {
        playerProjectilesPool = new List<GameObject>();
        enemyPool = new List<GameObject>();
        enemyProjectilesPool = new List<GameObject>();

        for (int i = 0; i < amountToPool; i++)
        {
            //Spawns the player projectiles and add them to the corresponding list
            if (projectile != null)
            {
                GameObject obj = (GameObject)Instantiate(projectile);
                obj.SetActive(false);
                if (playerProjectilesParent != null)
                    obj.transform.parent = playerProjectilesParent;

                playerProjectilesPool.Add(obj);
            }
           
            //-------------------------------------------//
            //Spawns the enemy projectiles and add them to the corresponding list
            if (enemyProjectile != null)
            {
                GameObject obj2 = (GameObject)Instantiate(enemyProjectile);
                obj2.SetActive(false);
                if (enemyProjectileParent != null)
                    obj2.transform.parent = enemyProjectileParent;

                enemyProjectilesPool.Add(obj2);
            }
            
            //------------------------------------------//
            //Spawns the enemies and add them to the corresponding list
            if (enemiesToPool.Length > 0)
            {
                for (int j = 0; j < enemiesToPool.Length; j++)
                {
                    GameObject obj3 = (GameObject)Instantiate(enemiesToPool[j]);
                    obj3.SetActive(false);
                    if (enemyParent != null)
                        obj3.transform.parent = enemyParent;

                    enemyPool.Add(obj3);
                }
            }
        }
    }

    /// <summary>
    /// Helper function to get the players projectiles
    /// </summary>
    /// <returns></returns>
    public GameObject GetPlayerProjectileObject()
    {
        GameObject retVal = null;

        for (int i = 0; i < playerProjectilesPool.Count; i++)
        {
            if(!playerProjectilesPool[i].activeInHierarchy)
            {
                retVal = playerProjectilesPool[i];
            }
        }

        return retVal;
    }

    /// <summary>
    /// Helper function to get the enemy's projectiles
    /// </summary>
    /// <returns></returns>
    public GameObject GetEnemyProjectileObject()
    {
        GameObject retVal = null;

        for (int i = 0; i < enemyProjectilesPool.Count; i++)
        {
            if (!enemyProjectilesPool[i].activeInHierarchy)
            {
                retVal = enemyProjectilesPool[i];
            }
        }

        return retVal;
    }

    /// <summary>
    /// Helper function to get an enemy from the pool
    /// </summary>
    /// <returns></returns>
    public GameObject GetEnemyObject()
    {
        GameObject retVal = null;

        for (int i = 0; i < enemyPool.Count; i++)
        {
            if (!enemyPool[i].activeInHierarchy)
            {
                retVal = enemyPool[i];
            }
        }
        return retVal;
    }

    /// <summary>
    /// Helper function to get a tagged enemy from the pool
    /// </summary>
    /// <param name="tag"></param>
    /// <returns></returns>
    public GameObject GetEnemyObject(string tag)
    {
        GameObject retVal = null;

        for (int i = 0; i < enemyPool.Count; i++)
        {
            if (!enemyPool[i].activeInHierarchy && enemyPool[i].tag == tag)
            {
                retVal = enemyPool[i];
            }
        }

        return retVal;
    }
}
