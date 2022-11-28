using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance = null;
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != null)
            Destroy(gameObject);
        //Destroy(Instance);  // 같을듯

        DontDestroyOnLoad(gameObject);
    }

    public T Load <T> (string path) where T : Object
    {
        return Resources.Load<T>(path);
    }
    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject prefab = this.Load<GameObject>($"Prefabs/{path}");
        if (prefab == null)
        {
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }
        return Object.Instantiate(prefab, parent);  // Object는 재귀방지
    }
    public void Destroy(GameObject go, float time = 0f)
    {
        if (go == null)
            return;

        Object.Destroy(go, time);  // Object는 재귀방지
    }
}
