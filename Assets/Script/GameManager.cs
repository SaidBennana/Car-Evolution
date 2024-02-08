using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Configer")]
    public bool Game_Start = false;
    public bool _Can_Shoot = true;
    [Space(20)][Tooltip("Cameras")]
    public Transform CameraGame, CameraStart;
    [Header("----CarControlle----")]
    public CarControlle carControlle;





    private void Awake()
    {
        Instance = this;
    }

}
