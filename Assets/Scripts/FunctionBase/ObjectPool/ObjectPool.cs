using System.Collections.Generic;
using UnityEngine;

namespace FunctionBase.Utilities.ObjectPool 
{


    public class ObjectPool : MonoBehaviour {
        [SerializeField]
        private GameObject _prefab;

        private List<GameObject> _pooledObjects = new();
        private List<GameObject> _spawnedObjects = new();

        private bool isDestroying;

        #region Properties

        public GameObject Prefab { get => _prefab; set => _prefab = value; }
        public List<GameObject> PooledObjects { get => _pooledObjects; set => _pooledObjects = value; }
        public List<GameObject> SpawnedObjects { get => _spawnedObjects; set => _spawnedObjects = value; }
        public bool IsDestroying { get => isDestroying; set => isDestroying = value; }

        #endregion

        public GameObject Spawn(Transform parent, Vector3 position, Quaternion rotation) {
            GameObject pooledObject = null;
            var pooledObjectsCount = _pooledObjects.Count;

            while (pooledObject == null && pooledObjectsCount > 0)
            {
                pooledObject = _pooledObjects[pooledObjectsCount - 1];
                _pooledObjects.RemoveAt(pooledObjectsCount - 1);
            }
            if (!pooledObject)
            {
                pooledObject = Instantiate(_prefab);
            }
            pooledObject.transform.SetLocalPositionAndRotation(position, rotation);
            pooledObject.transform.parent = parent ? parent : this.transform;
            pooledObject.SetActive(true);
            _spawnedObjects.Add(pooledObject);
            return pooledObject;
        }

        public void Recycle(GameObject spawnObject) {
            if (!spawnObject) return;
            _pooledObjects.Add(spawnObject);
            _spawnedObjects.Remove(spawnObject);
            spawnObject.SetActive(false);
            if (!IsDestroying)
            {
                spawnObject.transform.SetParent(this.transform);
            }
        }

        public void CleanUpPool() {
            foreach (GameObject obj in _pooledObjects)
            {
                Destroy(obj);
            }
            _pooledObjects.Clear();
        }

        private void OnDestroy() {
            IsDestroying = true;
            _prefab.CleanUpAll();
        }
    }
}
