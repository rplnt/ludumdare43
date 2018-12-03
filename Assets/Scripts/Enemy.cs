using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public GameObject minion;
    public int count;
    public float spread;

    private void Start() {
        for (int i = 0; i < count; i++) {
            Vector2 pos = Random.insideUnitCircle * spread;
            GameObject go = Instantiate(minion, pos, Quaternion.identity);
            go.transform.SetParent(transform, false);
        }
    }
}
