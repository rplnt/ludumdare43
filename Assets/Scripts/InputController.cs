using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InputController : MonoBehaviour {
    public System.Action<Vector2> NewTarget;
    public Grid world;
    public Tilemap obstacles;


    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector3Int cellPos = world.WorldToCell(pos);
            TileBase tile = obstacles.GetTile(cellPos);
            if (tile !=null && tile.name == "forrest") return;

            if (NewTarget != null) {
                NewTarget(pos);
            }
        }
    }
}
