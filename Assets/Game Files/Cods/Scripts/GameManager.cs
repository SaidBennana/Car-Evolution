using System.Collections;
using System.Collections.Generic;
using As_Star;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Configer")]
    public bool Game_Start = false;
    [HideInInspector] public bool FinshLine = false;
    [HideInInspector] public bool _Can_Shoot = true;
    [Space(20)]
    [Tooltip("Cameras")]
    public Transform CameraGame, CameraStart;
    [Header("----CarControlle----")]
    public CarControlle carControlle;


    private void Awake()
    {
        Instance = this;
        SoundManager.instance.InitializeSound();

    }

    /// Checks if a key exists in the save file and returns the value if it does, otherwise returns the default value.
    public T LoadData<T>(string key, T def)
    {
        if (ES3.FileExists("SaveFile.es3"))
        {
            if (ES3.KeyExists(key, "SaveFile.es3"))
            {
                return ES3.Load<T>(key);

            }

        }
        return def;

    }

}
