using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerGamer : MonoBehaviour {
    Calendar calendar;

    public int Health;
    public int Sheep { get; protected set; }
    public int Shield { get; protected set; }

    public System.Action<int, int, int> StatsUpdated;
    public System.Action<string> GameEvent;

    public System.Action<string, int> GameOver;
    public System.Action<string, int> Win;

    int lastSacrificeWeek = -1;

    private void Start() {
        calendar = FindObjectOfType<Calendar>();
        Sheep = 0;
        Shield = 0;

        if (StatsUpdated != null) {
            StatsUpdated(Health, Shield, Sheep);
        }

        GameEvent += EventLogger;
        calendar.NewWeek += ReallyCollectSheep;
    }


    public void EventLogger(string message) {
        Debug.Log("Player: " + message);
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
        } else if (collision.gameObject.CompareTag("Castle")) {
            GameManager.over = true;
            Win("YOU'VE WON", calendar.Day);
        }

        if (StatsUpdated != null) {
            StatsUpdated(Health, Shield, Sheep);
        }
    }

    private void Altar() {
        if (lastSacrificeWeek == calendar.Week) {
            GameEvent("You already made sacrifice this week...");
            return;
        }
        
        if (Sheep < calendar.Week) {
            GameEvent("Not enough sheep to make a sacrifice!");
            return;
        }

        Sheep -= calendar.Week;
        lastSacrificeWeek = calendar.Week;
        Shield++;
        GameEvent("Next week I'll need " + (calendar.Week + 1) + " sheep!");
    }

    private void CollectSheep(GameObject sheep) {
        Destroy(sheep);
        Sheep++;
    }
    

    private void ReallyCollectSheep(int week) {
        if (lastSacrificeWeek != week - 1) {
            GameManager.over = true;
            GameOver("WEEK'S OVER", calendar.Day);
        }
    }

    private void Fight(GameObject enemy) {
        Destroy(enemy);
        if (Shield > 0 && Random.Range(0.0f, 1.2f) > 1.0f/Shield) {
            // Block
            return;
        }

        // Hit
        Health--;
        if (Health <= 0) {
            GameManager.over = true;
            GameOver("YOU'VE DIED", calendar.Day);
        }
    }

    public void RestartLevel() {
        SceneManager.LoadScene(0);
    }
}
