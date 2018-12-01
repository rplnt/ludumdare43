using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour {

    public Vector2 target;
    public Vector2 currentTarget;
    List<Vector2> path;
    int pathIndex = 0;


    public float rotateSpeed;
    public float walkSpeed;

    public InputController input;
    public Roads roads;

    void Start () {
        input.NewTarget += SetTarget;
	}
	
    void Update () {
        if (GameManager.paused) return;
        Move();
    }

    void SetTarget(Vector2 targetPosition) {
        Debug.Log("Player received new target " + targetPosition);
        path = roads.GetPath(transform.position, targetPosition);
        if (path == null) {
            return;
        }

        target = targetPosition;
        currentTarget = path[0];
        pathIndex = 0;
        GameManager.moving = true;
    }


    void Move() {
        if (GameManager.moving) {
            if (Vector2.Distance(transform.position, currentTarget) < 0.1) {
                Debug.Log("next");

                if (Vector2.Distance(currentTarget, target) < 0.1) {
                    Debug.Log("stop");
                    GameManager.moving = false;
                } else {
                    pathIndex++;
                    if (pathIndex >= path.Count) {
                        Debug.LogError("Walking too far (index too large)");
                        return;
                    }
                    currentTarget = path[pathIndex];
                }
            }

            Walk();
        }
    }

    private void OnDrawGizmos() {
        if (GameManager.moving) Gizmos.DrawSphere(target, 0.2f);
    }

    void Walk() {
        /* rotate */
        Vector2 vector = currentTarget - (Vector2)transform.position;
        float angle = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg - 90.0f;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), rotateSpeed * Time.deltaTime);

        /* go */
        float roadBonus = (pathIndex > 0) ? 1.2f : 1.0f;
        transform.Translate(Vector2.up * walkSpeed * roadBonus * Time.deltaTime);

    }
}
