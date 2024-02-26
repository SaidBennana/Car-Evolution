using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelEditor : EditorWindow
{
    public VisualTreeAsset visualTreeAsset;
    public LevelsData levelsData;
    VisualElement root;

    #region  varibals
    /// List to store ObjectField elements.
    List<ObjectField> All_SceneFold = new List<ObjectField>();
    SceneLevel SelectedScene;


    #endregion


    #region Elements
    PropertyField SenceProperty;
    Button Add_New_LevelBtn;
    Button Remove_LevelSelect;
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

        // inilize UI Elements
        Add_New_LevelBtn = root.Q<VisualElement>("ScenePerant1").Q<Button>("Add_level");
        Remove_LevelSelect = root.Q<VisualElement>("ScenePerant1").Q<Button>("RemoveSelect");

    }


    public void CreateGUI()
    {
        Add_New_LevelBtn.clicked -= Add_New_Level();
        /// Sets the currently selected scene to null.
        SelectedScene = null;

        // Add Level button
        Add_New_LevelBtn.clicked += Add_New_Level();




        // set level in ui
        ScenesConfig();
    }
    private void Update()
    {
        if (SelectedScene != null)
            Remove_LevelSelect.RemoveFromClassList("hide");
        else
            Remove_LevelSelect.AddToClassList("hide");





    }

    /// Adds a new level to the scene list.
    Action Add_New_Level()
    {
        return () =>
        {
            levelsData._scenes.Add(new SceneLevel());
            CreateGUI();
        };
    }
    Action Remove_Level(string ElementName = "level")
    {
        return () =>
        {
            if (EditorUtility.DisplayDialog("Remove Level", $"are you sure you wont to remove {ElementName}", "OK", "NO"))
            {
                levelsData._scenes.Remove(SelectedScene);
                SelectedScene = null;
                Remove_LevelSelect.clicked -= Remove_Level();
                CreateGUI();
            }
        };

    }


    /// Configures the UI to display the list of scenes from the levelsData._scenes list.
    void ScenesConfig()
    {
        All_SceneFold.Clear();
        root.Q<VisualElement>("ScenePerant2").Clear();


        int i = 0;

        foreach (SceneLevel item in levelsData._scenes)
        {


            ObjectField objectField = new ObjectField();
            objectField.objectType = typeof(SceneAsset);
            objectField.value = item._scene;
            // objectField.label = $"LEVEL {i}";

            objectField.RegisterValueChangedCallback((value) =>
            {
                item._scene = (SceneAsset)value.newValue;

                EditorUtility.SetDirty(levelsData);
                AssetDatabase.SaveAssets();
            });

            objectField.RegisterCallback<ClickEvent>((e) =>
            {
                Action dd = new Action(() =>
                {
                    Remove_LevelSelect.clicked -= Remove_Level();
                    ElemetSelect(item, objectField);
                });
                dd.Invoke();

            });



            /// Adds the given Foldout UI element to the list of all scene foldouts.
            /// This allows the foldout to be rendered and updated.
            All_SceneFold.Add(objectField);

            root.Q<VisualElement>("ScenePerant2").Add(objectField);

            i++;
        }


    }



    /// <summary>
    /// Selects the given scene element and foldout in the UI.
    /// </summary>
    /// <param name="Scenes_item">The sceneLevel element to select.</param>
    /// <param name="flodoutd">The foldout UI element for the sceneLevel.</param>
    private void ElemetSelect(SceneLevel Scenes_item, ObjectField flodoutd)
    {
        foreach (var item in All_SceneFold)
        {
            item.RemoveFromClassList("SelectElement");
        }
        SelectedScene = Scenes_item;

        flodoutd.AddToClassList("SelectElement");

        string ElementName = SelectedScene._scene == null ? "Null" : SelectedScene._scene.name;
        Remove_LevelSelect.clicked += Remove_Level();
    }








}
