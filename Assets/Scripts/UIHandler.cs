using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour {
    public Text hp;
    public Text shield;
    public Text sheep;

    public Text week;
    public Text weekday;

    public GameObject gameOver;
    public Text gameOverReason;
    public Text gameOverStats;

    public Text messageBox;

    private void Awake() {
        PlayerGamer player = FindObjectOfType<PlayerGamer>();
        player.StatsUpdated += UpdateStats;
        player.GameOver = GameOver;
        player.Win = Win;
        player.GameEvent = ShowMessage;

        Calendar calendar = FindObjectOfType<Calendar>();
        calendar.NewDay += UpdateCalendar;

        gameOver.SetActive(false);
        messageBox.enabled = false;
    }

    public void UpdateStats(int health, int def, int sheeps) {
        hp.text = (Mathf.Max(health, 0)).ToString();
        shield.text = def.ToString();
        sheep.text = sheeps.ToString();
    }

    public void UpdateCalendar(int weekNumber, string day) {
        week.text = weekNumber.ToString();
        weekday.text = day;
    }

    public void GameOver(string reason, int days) {
        gameOverReason.text = reason;
        gameOverStats.text = "You've survived " + days + " days, but you couldn't save the Princess.";
        gameOver.SetActive(true);
    }

    public void Win(string reason, int days) {
        gameOverReason.text = reason;
        gameOverStats.text = "It took you " + days + " days, but you've saved the Princess!";
        gameOver.SetActive(true);
    }

    public void ShowMessage(string message) {
        messageBox.enabled = true;
        messageBox.text = message;
        messageBox.color = Color.white;
        StartCoroutine(fadeOutText(messageBox, 1.0f, 0.8f));
    }

    IEnumerator fadeOutText(Text s, float duration, float fadeDuration) {
        yield return new WaitForSeconds(duration);
        float elapsed = 0.0f;

        while (elapsed < fadeDuration) {
            elapsed += Time.deltaTime;
            s.color = new Color(s.color.r, s.color.g, s.color.b, Mathf.Lerp(1.0f, 0.0f, elapsed / fadeDuration));
            yield return null;
        }

        s.enabled = false;
    }
}
