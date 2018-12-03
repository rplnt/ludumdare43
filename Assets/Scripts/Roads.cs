using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roads : MonoBehaviour {

    public int maxRoadLength;
    public List<Node> nodes;
    public GameObject roads;
    public GameObject waypoints;
    const float errDst = 0.1f;

    private void Awake() {
        nodes = new List<Node>();
    }

    void Start () {
        /* copy over all points */
        var toConnect = new List<Node>();
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
                    toConnect.Add(n);
                }
            }
            roadId++;
        }

        for(int i=0; i < waypoints.transform.childCount; i++) {
            Node n = new Node(10000 + i, waypoints.transform.GetChild(i).transform.position);
            nodes.Add(n);
            toConnect.Add(n);
        }

        /* find connections */
        foreach (Node node in toConnect) {
            foreach(Node n in nodes) {
                if (node.road == n.road) continue;
                if (n.linked.Contains(node)) continue;
                float cutOff = node.road > 10000 ? 1.5f : 0.5f;
                if (Vector2.Distance(node.position, n.position) < cutOff) {
                    node.DoubleLink(n);
                }
            }
        }
        

        Debug.Log("Loaded " + nodes.Count + " path nodes");
	}
    
    private void Update() {
        foreach (Node n in nodes) {
            foreach (Node ln in n.linked) {
                Debug.DrawLine(n.position, ln.position, Color.yellow);
            }
        }

        //if (Input.GetKeyDown(KeyCode.Space)) {
        //    var path = Pathfinding.FindPath(nodes[Random.Range(0, nodes.Count - 1)], nodes[Random.Range(0, nodes.Count - 1)]);
        //    if (path == null) {
        //        Debug.Log("Couldn't find path");
        //        return;
        //    }

        //    Debug.Log("Found path:");
        //    for(int i=0; i < path.Count; i++) {
        //        if (i > 0) {
        //            Debug.DrawLine(path[i - 1].position, path[i].position, Color.red, 1f);
        //        }
        //    }
        //}
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
