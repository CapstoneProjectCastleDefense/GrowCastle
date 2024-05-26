namespace FunctionBase.ObjectPool 
{
    using System.Collections.Generic;
    using UnityEngine;


    public class ObjectPool : MonoBehaviour {
        [SerializeField]
        private GameObject _prefab;

        private List<GameObject> _pooledObjects = new();
        private List<GameObject> _spawnedObjects = new();

        private bool isDestroying;

        #region Properties

        public GameObject       Prefab         { get => this._prefab;         set => this._prefab = value; }
        public List<GameObject> PooledObjects  { get => this._pooledObjects;  set => this._pooledObjects = value; }
        public List<GameObject> SpawnedObjects { get => this._spawnedObjects; set => this._spawnedObjects = value; }
        public bool             IsDestroying   { get => this.isDestroying;    set => this.isDestroying = value; }

        #endregion

        public GameObject Spawn(Transform parent, Vector3 position, Quaternion rotation) {
            GameObject pooledObject = null;
            var pooledObjectsCount = this._pooledObjects.Count;

            while (pooledObject == null && pooledObjectsCount > 0)
            {
                pooledObject = this._pooledObjects[pooledObjectsCount - 1];
                this._pooledObjects.RemoveAt(pooledObjectsCount - 1);
            }
            if (!pooledObject)
            {
                pooledObject = Instantiate(this._prefab);
            }
            pooledObject.transform.SetLocalPositionAndRotation(position, rotation);
            pooledObject.transform.parent = parent ? parent : this.transform;
            pooledObject.SetActive(true);
            this._spawnedObjects.Add(pooledObject);
            return pooledObject;
        }

        public void Recycle(GameObject spawnObject) {
            if (!spawnObject) return;
            this._pooledObjects.Add(spawnObject);
            this._spawnedObjects.Remove(spawnObject);
            spawnObject.SetActive(false);
            if (!this.IsDestroying)
            {
                spawnObject.transform.SetParent(this.transform);
            }
        }

        public void CleanUpPool() {
            foreach (GameObject obj in this._pooledObjects)
            {
                Destroy(obj);
            }
            this._pooledObjects.Clear();
        }

        private void OnDestroy() {
            this.IsDestroying = true;
            this._prefab.CleanUpAll();
        }
    }
}
