using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using Cinemachine.Editor;

public class LevelEditor : EditorWindow
{
    public VisualTreeAsset visualTreeAsset;
    public LevelsData levelsData;
    VisualElement root;


    #region Elements
    PropertyField SenceProperty;
    #endregion

    [MenuItem("MyTools/Level Editor")]
    public static void openWindow()
    {
        LevelEditor win = GetWindow<LevelEditor>("Level Editor");
        win.Show();

    }
    private void OnEnable()
    {
        rootVisualElement.Add(visualTreeAsset.Instantiate());
        root = rootVisualElement;

    }

    public void CreateGUI()
    {
        ScenesConfig();

    }


    void ScenesConfig()
    {
        root.Q<VisualElement>("ScenePerant2").Add(new InspectorElement(levelsData));


    }





}
