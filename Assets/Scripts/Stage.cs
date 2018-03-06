using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Stage : MonoBehaviour {

    public PrefabList List;

    public GameObject GetRandomPrefab()
    {
        if(List == null) { return null; }

        var target = List.List.Where(x => x != null).ToList();
        if(target.Count == 0) { return null; }

        var index = Random.Range(0, target.Count);
        return target[index];
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
