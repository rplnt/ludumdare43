using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : MonoBehaviour {
    Vector2 born;
    Vector2 target;
    float spread = 0.2f;

    private void Start() {
        born = transform.position;
        target = born;
    }

    private void Update() {
        if (GameManager.TimeStopped) return;
        if (Vector2.Distance(transform.position, target) < 0.1) {
            PickNewTarget();
        }

        Move();

    }

    void PickNewTarget() {
        target = new Vector2(born.x + Random.Range(-spread, spread), born.y + Random.Range(-spread, spread));
    }

    void Move() {
        /* rotate */
        Vector2 vector = target - (Vector2)transform.position;
        float angle = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg - 90.0f;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), 300.0f * Time.deltaTime);

        /* go */
        transform.Translate(Vector2.up * 0.15f * Time.deltaTime);
    }
}
