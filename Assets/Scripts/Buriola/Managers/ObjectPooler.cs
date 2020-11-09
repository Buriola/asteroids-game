using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Buriola.Managers
{
    public class ObjectPooler : MonoBehaviour
    {
        #region Singleton
        private static ObjectPooler instance;
        public static ObjectPooler Instance => instance;
        #endregion

        #region Variables

        [FormerlySerializedAs("playerProjectilesParent")] 
        [Header("Player Pool")]
        [SerializeField]
        private Transform _playerProjectilesParent = null;
        
        private List<GameObject> _playerProjectilesPool;
        
        [FormerlySerializedAs("projectile")] 
        [SerializeField]
        private GameObject _projectile = null;

        [FormerlySerializedAs("enemyParent")] 
        [Header("Enemy Pool")] 
        [SerializeField]
        private Transform _enemyParent = null;
        
        private List<GameObject> _enemyPool;

        [FormerlySerializedAs("enemiesToPool")] 
        [SerializeField]
        private GameObject[] _enemiesToPool = null;

        [Space] 
        [FormerlySerializedAs("enemyProjectileParent")] 
        [SerializeField]
        private Transform _enemyProjectileParent = null;
        
        private List<GameObject> enemyProjectilesPool;
        
        [FormerlySerializedAs("enemyProjectile")]
        [SerializeField]
        private GameObject _enemyProjectile = null;

        [Space] 
        [Range(1, 20), SerializeField]
        private int amountToPool = 10;

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
        
        private void Init()
        {
            _playerProjectilesPool = new List<GameObject>();
            _enemyPool = new List<GameObject>();
            enemyProjectilesPool = new List<GameObject>();

            for (int i = 0; i < amountToPool; i++)
            {
                if (_projectile != null)
                {
                    GameObject obj = (GameObject) Instantiate(_projectile);
                    obj.SetActive(false);
                    if (_playerProjectilesParent != null)
                        obj.transform.parent = _playerProjectilesParent;

                    _playerProjectilesPool.Add(obj);
                }
                
                if (_enemyProjectile != null)
                {
                    GameObject obj2 = (GameObject) Instantiate(_enemyProjectile);
                    obj2.SetActive(false);
                    if (_enemyProjectileParent != null)
                        obj2.transform.parent = _enemyProjectileParent;

                    enemyProjectilesPool.Add(obj2);
                }
                
                if (_enemiesToPool.Length > 0)
                {
                    for (int j = 0; j < _enemiesToPool.Length; j++)
                    {
                        GameObject obj3 = (GameObject) Instantiate(_enemiesToPool[j]);
                        obj3.SetActive(false);
                        if (_enemyParent != null)
                            obj3.transform.parent = _enemyParent;

                        _enemyPool.Add(obj3);
                    }
                }
            }
        }
        
        public GameObject GetPlayerProjectileObject()
        {
            GameObject retVal = null;

            for (int i = 0; i < _playerProjectilesPool.Count; i++)
            {
                if (!_playerProjectilesPool[i].activeInHierarchy)
                {
                    retVal = _playerProjectilesPool[i];
                }
            }

            return retVal;
        }
        
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
        
        public GameObject GetEnemyObject()
        {
            GameObject retVal = null;

            for (int i = 0; i < _enemyPool.Count; i++)
            {
                if (!_enemyPool[i].activeInHierarchy)
                {
                    retVal = _enemyPool[i];
                }
            }

            return retVal;
        }
        
        public GameObject GetEnemyObject(string tag)
        {
            GameObject retVal = null;

            for (int i = 0; i < _enemyPool.Count; i++)
            {
                if (!_enemyPool[i].activeInHierarchy && _enemyPool[i].CompareTag(tag))
                {
                    retVal = _enemyPool[i];
                }
            }

            return retVal;
        }
    }
}
