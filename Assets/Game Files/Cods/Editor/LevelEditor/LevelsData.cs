using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData")]
public class LevelsData : ScriptableObject
{
    public List<SceneLevel> _scenes = new List<SceneLevel>();

}
[Serializable]
public class SceneLevel
{
    public SceneAsset _scene;
}


