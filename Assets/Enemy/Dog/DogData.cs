using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogData : MonsterData
{
    public static float walkSpeed = 3f;
    public static float chaseSpeed = 6.5f;

    public static float innerRadius = 5f;
    public static float outerRadius = 15f;

    public static float lambda = 2f;

    public static float nextWaypointDistance = 0.8f;

    public static float maxDetectionDistance = 10f;
    public static float FOV = 40f;
    public static float maxDiscrepency = 1f;

    public static string idleAnimation = "Idle";
    public static string trotAnimation = "Trot";
    public static string chaseAnimation = "Zoom";

}
