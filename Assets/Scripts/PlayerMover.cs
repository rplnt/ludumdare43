using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour {

    public Vector2 target;
    public Vector2 currentTarget;
    List<Vector2> path;
    int pathIndex = 0;

    public SpriteRenderer targeter;


    public float rotateSpeed;
    public float walkSpeed;
    public float Speed { get { return walkSpeed * ((pathIndex > 0) ? 1.2f : 1.0f); } }

    public InputController input;
    public Roads roads;

    public List<Minion> minions;


    void Start () {
        targeter.enabled = false;
        input.NewTarget += SetTarget;
        minions = new List<Minion>();
        minions.AddRange(FindObjectsOfType<Minion>());
	}


    void SetTarget(Vector2 targetPosition) {
        Debug.Log("Player received new target " + targetPosition);
        path = roads.GetPath(transform.position, targetPosition);
        if (path == null) {
            return;
        }

        targeter.transform.position = targetPosition;
        targeter.enabled = true;

        target = targetPosition;
        currentTarget = path[0];
        pathIndex = 0;
        GameManager.moving = true;
    }


    void Update () {
        if (GameManager.TimeStopped) return;
        Move();
    }
    

    void Move() {
        if (GameManager.moving && !GameManager.over) {
            if (Vector2.Distance(transform.position, target) < 0.5f) {
                currentTarget = target;
            }
            if (Vector2.Distance(transform.position, currentTarget) < 0.1f) {
                if (Vector2.Distance(currentTarget, target) < 0.1f) {
                    GameManager.moving = false;
                    targeter.enabled = false;
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
        transform.Translate(Vector2.up * Speed * Time.deltaTime);

    }
}
