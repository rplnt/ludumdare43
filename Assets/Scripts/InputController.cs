using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {
    public System.Action<Vector2> NewTarget;

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (NewTarget != null) {
                NewTarget(pos);
            }
        }
    }
}
