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

    private void Start() {
        PlayerGamer player = FindObjectOfType<PlayerGamer>();
        player.StatsUpdated += UpdateStats;

        Calendar calendar = FindObjectOfType<Calendar>();
        calendar.NewDay += UpdateCalendar;
    }

    public void UpdateStats(int health, int def, int sheeps) {
        hp.text = health.ToString();
        shield.text = def.ToString();
        sheep.text = sheeps.ToString();
    }

    public void UpdateCalendar(int weekNumber, string day) {
        week.text = (weekNumber + 1).ToString();
        weekday.text = day;
    }
}
