using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameManager {
    public static bool fighting = false;
    public static bool moving = false;

    public static bool TimeStopped { get { return !(fighting || moving); } }
}
