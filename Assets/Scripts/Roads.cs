using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roads : MonoBehaviour {

    public int maxRoadLength;
    public List<Node> nodes;
    public GameObject roads;
    const float errDst = 0.1f;

    private void Awake() {
        nodes = new List<Node>();
    }

    void Start () {
        /* copy over all points */
        var ends = new List<Node>();
        int roadId = 0;
		foreach(LineRenderer road in roads.GetComponentsInChildren<LineRenderer>()) {
            Vector3[] positions = new Vector3[road.positionCount];
            road.GetPositions(positions);
            for(int i=0; i < positions.Length; i++) {
                Node n = new Node(roadId, positions[i]);
                if (i > 0) {
                    n.DoubleLink(nodes[nodes.Count-1]);
                }

                nodes.Add(n);
                if (i == 0 || i == (positions.Length - 1)) {
                    ends.Add(n);
                }
            }
            roadId++;
        }

        /* find connections */
        foreach (Node end in ends) {
            foreach(Node n in nodes) {
                if (end.road == n.road) continue;
                if (Vector2.Distance(end.position, n.position) < 0.5f) {
                    end.DoubleLink(n);
                }
            }
        }
        

        Debug.Log("Loaded " + nodes.Count + " path nodes");
	}
    
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            var path = Pathfinding.FindPath(nodes[Random.Range(0, nodes.Count - 1)], nodes[Random.Range(0, nodes.Count - 1)]);
            if (path == null) {
                Debug.Log("Couldn't find path");
                return;
            }

            Debug.Log("Found path:");
            for(int i=0; i < path.Count; i++) {
                Debug.Log(path[i].position);
                if (i > 0) {
                    Debug.DrawLine(path[i - 1].position, path[i].position, Color.yellow, 1.2f);
                }
            }
        }
    }


    private void OnDrawGizmos() {
        if (nodes == null) return;
        Gizmos.color = Color.red;
        foreach(Node n in nodes) {
            Gizmos.DrawSphere(n.position, 0.1f);
        }
    }


    public List<Vector2> GetPath(Vector2 start, Vector2 target) {
        List<Vector2> path = new List<Vector2>();
        if (Vector2.Distance(start, target) > 1.0f) {
            var nodePath = Pathfinding.FindPath(FindRoadNode(start), FindRoadNode(target));
            if (nodePath == null) {
                return null;
            }

            foreach (Node n in nodePath) {
                path.Add(n.position);
            }
        }

        path.Add(target);

        return path;
    }


    public Node FindRoadNode(Vector2 position) {
        Node closest = null;
        float minDistance = Mathf.Infinity;

        for(int i = 0; i < nodes.Count; i++) {
            float distance = Vector3.Distance(position, nodes[i].position);
            if (distance < minDistance) {
                closest = nodes[i];
                minDistance = distance;
            }
        }

        return closest;
    }
}
