using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calendar : MonoBehaviour {
    public GameObject sun;
    public float dayLength;
    public static string[] weekdays = {"Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

    float clock;
    int currentDay = 0;
    public int Weekday { get { return Day % 7; } }
    public int Week { get { return 1 + Day / 7; } }
    public int Day { get { return (int)clock / 24; }}
    public float Hours { get { return clock % 24; } }
    public int Hour { get { return (int)Hours; } }
    public int Minute { get { return (int)(60.0f * (Hours - Hour)); } }

    float SunRotation { get { return 180.0f + (Hours/24) * 360.0f; } }

    public float SunRotationSpeed { get { return 360.0f / dayLength; } }

    public System.Action<int, string> NewDay;
    public System.Action<int> NewWeek;

    private void Start() {
        clock = 12.0f;
    }


    private void Update() {
        if (GameManager.TimeStopped) return;
        clock += Time.deltaTime;

        // new day
        if (currentDay != Weekday) {
            if (Weekday < currentDay) {
                if (NewWeek != null) {
                    NewWeek(Week);
                }
            }

            if (NewDay != null) {
                NewDay(Week, weekdays[Weekday]);
            }
            currentDay = Weekday;
        }

        //Debug.Log("Day " + Day + " (" + weekdays[Weekday] + ") - " + Hour + ":" + Minute);

        sun.transform.rotation = Quaternion.Euler(0.0f, SunRotation, 0.0f);
    }
}
