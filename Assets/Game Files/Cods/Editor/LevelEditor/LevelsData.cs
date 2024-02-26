using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData")]
public class LevelsData : ScriptableObject
{
    public List<Scenes> _scenes = new List<Scenes>();

}
[Serializable]
public class Scenes
{
    public SceneAsset _scene;
    public GameObject _scene3;
}
