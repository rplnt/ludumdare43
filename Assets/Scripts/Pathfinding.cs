using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {
    public int gCost;
    public int hCost;
    public int FCost { get { return gCost + hCost; } }

    public Vector2 position;
    public Node parent;

    public List<Node> linked;

    public int road;

    public Node(int roadId, Vector2 pos) {
        road = roadId;
        position = pos;
        linked = new List<Node>();
    }

    public void DoubleLink(Node adjacent) {
        linked.Add(adjacent);
        adjacent.Link(this);
    }

    public void Link(Node adjacent) {
        linked.Add(adjacent);
    }

    public List<Node> GetAdjacent() {
        return linked;
    }

    public override string ToString() {
        var main = "Node( " + road + ") @ " + position + " [";
        foreach(Node n in linked) {
            main += (n.position + " ");
        }
        return main + "]";
    }
}


public static class Pathfinding {
    public static List<Node> Path;

    public static List<Node> FindPath(Node start, Node target) {
        //Debug.Log("FindPath");
        if (start == null || target == null) return null;

        Path = null;
        List<Node> open = new List<Node>();
        HashSet<Node> closed = new HashSet<Node>();

        open.Add(start);
        while (open.Count > 0) {
            Node current = open[0];
            //Debug.Log("Searching from: " + current);
            int currentIndex = 0;
            for (int i = 1; i < open.Count; i++) {
                if (open[i].FCost < current.FCost || open[i].FCost == current.FCost && open[i].hCost < current.hCost) {
                    current = open[i];
                    currentIndex = i;
                }
            }
            open.RemoveAt(currentIndex);
            closed.Add(current);

            if (current == target) {
                //Debug.Log("Reached goal: " + target);
                GeneratePath(start, target);
            }

            foreach(Node next in current.GetAdjacent()) {
                //Debug.Log("Next: " + next);
                if (closed.Contains(next)) continue;
                float cost = current.gCost + Vector2.Distance(current.position, next.position);
                if (cost < next.gCost || !open.Contains(next)) {
                    next.gCost = (int)cost;
                    next.hCost = (int)Vector2.Distance(next.position, target.position);
                    next.parent = current;

                    if (!open.Contains(next)) {
                        open.Add(next);
                    }
                }
            }
        }

        return Path;
    }
    
    public static void GeneratePath(Node start, Node target) {
        List<Node> path = new List<Node>();
        Node current = target;

        while (current != start) {
            path.Add(current);
            current = current.parent;
        }

        path.Reverse();
        Path = path;
    }
}
