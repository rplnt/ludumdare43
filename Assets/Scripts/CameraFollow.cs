using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public Transform player;
    public Transform limitSW;
    public Transform limitNE;

    float vMin, vMax, hMin, hMax;

    private void Start() {
        CalculateBounds();
    }

    void CalculateBounds() {
        Camera camera = GetComponent<Camera>();
        float vert = camera.orthographicSize;
        float horiz = vert * ((float)Screen.width / (float)Screen.height);

        hMin = limitSW.position.x + horiz;
        hMax = limitNE.position.x - horiz;
        vMin = limitSW.position.y + vert;
        vMax = limitNE.position.y - vert;
    }

    //private void OnDrawGizmos() {
    //    Gizmos.DrawSphere(new Vector3(hMin, vMin), 1.0f);
    //    Gizmos.DrawSphere(new Vector3(hMin, vMax), 1.0f);
    //    Gizmos.DrawSphere(new Vector3(hMax, vMin), 1.0f);
    //    Gizmos.DrawSphere(new Vector3(hMax, vMax), 1.0f);
    //}

    private void Update() {
        float x = Mathf.Clamp(player.position.x, hMin, hMax);
        float y = Mathf.Clamp(player.position.y, vMin, vMax);
        transform.position = new Vector3(x, y, transform.position.z);
    }
}
