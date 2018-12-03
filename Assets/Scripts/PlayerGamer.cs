using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGamer : MonoBehaviour {
    Calendar calendar;

    public int Health;
    public int Sheep { get; protected set; }
    public int Shield { get; protected set; }

    public System.Action<int, int, int> StatsUpdated;

    private void Start() {
        calendar = FindObjectOfType<Calendar>();
        Sheep = 0;
        Shield = 0;

        if (StatsUpdated != null) {
            StatsUpdated(Health, Shield, Sheep);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Altar")) {
            Altar();
        } else if (collision.gameObject.CompareTag("Sheep")) {
            CollectSheep(collision.gameObject);
        } else if (collision.gameObject.CompareTag("Enemy")) {
            Fight(collision.gameObject);
        } else if (collision.gameObject.CompareTag("Shield")) {
            Destroy(collision.gameObject);
            Shield++;
        } else if (collision.gameObject.CompareTag("Heart")) {
            Destroy(collision.gameObject);
            Health++;
        }

        if (StatsUpdated != null) {
            StatsUpdated(Health, Shield, Sheep);
        }
    }

    private void Altar() {
        if (calendar.Weekday != 7) {
            Debug.Log("Not sunday");
            return;
        }
    }

    private void CollectSheep(GameObject sheep) {
        Destroy(sheep);
        Sheep++;
    }


    private void Fight(GameObject enemy) {
        Destroy(enemy);
        if (Shield > 0 && Random.Range(0.0f, 1.0f) > 1.0f/Shield) {
            // Block
            return;
        }

        // Hit
        Health--;
        if (Health <= 0) {
            //Dead
        }
    }
}
