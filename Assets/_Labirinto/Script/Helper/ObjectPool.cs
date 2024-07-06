using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPool : BasicSingleton<ObjectPool>
{
    public List<GameObject> PrefabsForPool;
    private List<GameObject> _pooledObjects = new List<GameObject>();

    public GameObject GetObjectFromPool(string objectName)
    {
        // try to get the instance of pooled
        var instance = _pooledObjects.FirstOrDefault(obj => obj.name == objectName);
        
        // if there is pooled instance
        if (instance != null)
        {
            _pooledObjects.Remove(instance);
            instance.SetActive(true);
            return instance;
        }

        // if dont have a pool
        var prefab = PrefabsForPool.FirstOrDefault(obj => obj.name == objectName);
        if (prefab != null)
        {
            // create new instance
            var newInstance = Instantiate(prefab, Vector3.zero, Quaternion.identity, transform);
            newInstance.transform.localPosition = Vector3.zero;
            newInstance.name = objectName;
            return newInstance;
        }
        Debug.LogWarning("Object pool dont have a prefab for the object");
        return null;
    }

    public void PoolObject(GameObject obj)
    {
        obj.SetActive(false);
        _pooledObjects.Add(obj);
    }
}
