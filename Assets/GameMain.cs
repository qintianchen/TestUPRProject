using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameMain : MonoBehaviour
{
    public Button btnAllocate;
    public Button btnRelease;
    public GameObject prefab;

    private bool isAllocating;

    private List<int[]> hashList;
    
    void Start()
    {
        hashList = new();
        
        btnAllocate.onClick.RemoveAllListeners();
        btnRelease.onClick.RemoveAllListeners();
        
        btnAllocate.onClick.AddListener(OnClickAllocate);    
        btnRelease.onClick.AddListener(OnClickRelease);    
    }

    private void OnClickAllocate()
    {
        StopAllCoroutines();
        StartCoroutine(CoAllocate());
    }

    private IEnumerator CoAllocate()
    {
        isAllocating = true;
        
        var rootGo = new GameObject("Root");
        rootGo.transform.position = Vector3.zero;
        
        for (int i = 0; i < 100; i++)
        {
            if (!isAllocating)
            {
                yield break;
            }
            
            for (int j = 0; j < 30; j++)
            {
                var go = Instantiate(prefab, rootGo.transform, true);

                var x = Random.Range(-100, 100);
                var y = Random.Range(-100, 100);
                var z = Random.Range(-100, 100);
                go.transform.localPosition = new Vector3(x, y, z);
                
                hashList.Add(new int[Random.Range(10000, 20000)]);
            }

            yield return new WaitForSeconds(0.1f);
        }

        isAllocating = false;
    }

    private void OnClickRelease()
    {
        isAllocating = false;

        var rootGo = GameObject.Find("Root");
        if (rootGo != null)
        {
            rootGo.name = "RootToDestory";
            Destroy(rootGo);
        }
        
        hashList.Clear();
        GC.Collect();
    }
}
