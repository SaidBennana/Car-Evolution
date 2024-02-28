using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System;
using Random = UnityEngine.Random;
using UnityEngine.Events;
using NUnit.Framework;

public class LevelEditor : EditorWindow
{
    public VisualTreeAsset visualTreeAsset;
    public LevelsData levelsData;
    public SceneAsset SceneCreate;
    VisualElement root;

    // Edit Platforms
    #region  varibals
    /// List to store ObjectField elements.
    List<Label> All_SceneObjectsFeild = new List<Label>();
    SceneLevel SelectedScene;

    /// List of Platform game objects in the scene
    PPlatform[] _pPlatforms;

    // Generate Levev Platforms
    List<Transform> PointsSpown = new List<Transform>();// postiin to intanctet the PPlatform and poralWater
    public Transform BPlatfroObject, PorrlWaterObject;

    #endregion


    #region Elements
    PropertyField SenceProperty;
    Button _Add_New_LevelBtn;
    Button _Remove_LevelSelect;
    Button _RandomValuesBtn, _generatePropsBtn, _OpenLevelBtn;
    ScrollView PPLatformPerant;
    #endregion

    [MenuItem("MyTools/Level Editor")]
    public static void openWindow()
    {
        LevelEditor win = GetWindow<LevelEditor>("Level Editor");
        win.maxSize = new Vector2(700, 400);
        win.minSize = win.maxSize;
        win.maximized = true;
        win.Show();

    }
    private void OnEnable()
    {
        rootVisualElement.Add(visualTreeAsset.Instantiate());
        root = rootVisualElement;


        // inilize UI Elements
        PPLatformPerant = root.Q<ScrollView>("PPlatfromPerantScroll");



        // buttons
        _Add_New_LevelBtn = root.Q<VisualElement>("ScenePerant1").Q<Button>("Add_level");
        _Remove_LevelSelect = root.Q<VisualElement>("ScenePerant1").Q<Button>("RemoveSelect");
        _RandomValuesBtn = root.Q<Button>("RandomValues");
        _generatePropsBtn = root.Q<Button>("GenetarProps");
        _OpenLevelBtn = root.Q<Button>("OpenSelectScene");




        // Add Level button
        _Add_New_LevelBtn.clicked += () =>
        {
            Add_New_Level();

            RemoveAllElementsInScene();
            Generate_LevelPeops();
            GetPointsSpwon();
            RandomValueProp();
        };
        _Remove_LevelSelect.clicked += () =>
        {
            Remove_Level();
        };
        _OpenLevelBtn.clicked += () =>
        {
            if (SelectedScene != null)
            {
                EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(SelectedScene._scene));
                GetPointsSpwon();

            }

        };


        // randome value prop game button
        _RandomValuesBtn.clicked += () =>
        {

            RandomValueProp();
        };

        // generate PPlatformd and BorrleWater
        _generatePropsBtn.clicked += () =>
        {
            RemoveAllElementsInScene();
            Generate_LevelPeops();
            GetPointsSpwon();
            RandomValueProp();

            CreateGUI();
        };
        GetPointsSpwon();
    }



    public void CreateGUI()
    {
        /// Sets the currently selected scene to null.
        SelectedScene = null;

        // set level in ui
        ScenesConfig();
        if (levelsData._scenes.Count > 0)
            // inililaize UI
            inilizePropsScroll();

        Repaint();

    }
    void GetPointsSpwon()
    {
        _pPlatforms = null;
        // Get All PPlatform from her
        _pPlatforms = FindObjectsOfType<PPlatform>();



        PointsSpown.Clear();
        // get points from his
        Transform PerantPoints = GameObject.Find("------PointSpwin-------").transform;

        Debug.Log(PerantPoints.childCount);
        for (int i = 0; i < PerantPoints.childCount; i++)
        {
            PointsSpown.Add(PerantPoints.GetChild(i));
        }
        CreateGUI();

    }
    private void Update()
    {
        if (_Remove_LevelSelect != null)
        {

            if (SelectedScene != null)
                _Remove_LevelSelect.RemoveFromClassList("hide");
            else
                _Remove_LevelSelect.AddToClassList("hide");
        }
    }

    /// Adds a new level to the scene list.
    void Add_New_Level()
    {
        SceneLevel sceneLevel = new SceneLevel();


        string newScenePath = AssetDatabase.GenerateUniqueAssetPath("Assets/Scenes/Level.unity");

        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), newScenePath);

        sceneLevel._scene = AssetDatabase.LoadAssetAtPath<SceneAsset>(newScenePath);
        AssetDatabase.Refresh();

        // Add scene from build setting
        EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
        ArrayUtility.Add(ref scenes, new EditorBuildSettingsScene(newScenePath, true));
        EditorBuildSettings.scenes = scenes;


        levelsData._scenes.Add(sceneLevel);

        EditorUtility.SetDirty(levelsData);
        AssetDatabase.SaveAssets();

        CreateGUI();
    }
    void Remove_Level(string ElementName = "level")
    {
        if (EditorUtility.DisplayDialog("Remove Level", $"are you sure you wont to remove {ElementName}", "OK", "NO"))
        {
            // EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
            // ArrayUtility.Remove(ref scenes, new EditorBuildSettingsScene(AssetDatabase.GetAssetPath(SelectedScene._scene), false));
            // EditorBuildSettings.scenes = scenes;


            // remove scene from build setting
            EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
            for (int i = 0; i < scenes.Length; i++)
            {
                if (scenes[i].path == AssetDatabase.GetAssetPath(SelectedScene._scene))
                {
                    ArrayUtility.RemoveAt(ref scenes, i);
                    EditorBuildSettings.scenes = scenes;
                    break;
                }
            }



            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(SelectedScene._scene));
            AssetDatabase.Refresh();

            levelsData._scenes.Remove(SelectedScene);

            SelectedScene = null;

            foreach (SceneLevel item in levelsData._scenes)
            {
                if (item._scene == null)
                {
                    levelsData._scenes.Remove(item);
                }

            }
            EditorUtility.SetDirty(levelsData);
            CreateGUI();
        }

    }



    /// Configures the UI to display the list of scenes from the levelsData._scenes list.
    void ScenesConfig()
    {
        All_SceneObjectsFeild.Clear();
        root.Q<VisualElement>("ScenePerant2").Clear();


        int i = 0;

        foreach (SceneLevel item in levelsData._scenes)
        {


            Label objectField = new Label();
            objectField.AddToClassList("SceneElement");

            //objectField.objectType = typeof(SceneAsset);
            objectField.text = item._scene.name;
            // objectField.label = $"LEVEL {i}";

            // objectField.RegisterValueChangedCallback((value) =>
            // {
            //     item._scene = (SceneAsset)value.newValue;

            //     EditorUtility.SetDirty(levelsData);
            //     AssetDatabase.SaveAssets();
            // });

            objectField.RegisterCallback<ClickEvent>((e) =>
            {
                Action dd = new Action(() =>
                {
                    ElemetSelect(item, objectField);
                });
                dd.Invoke();

            });



            /// Adds the given Foldout UI element to the list of all scene foldouts.
            /// This allows the foldout to be rendered and updated.
            All_SceneObjectsFeild.Add(objectField);

            root.Q<VisualElement>("ScenePerant2").Add(objectField);

            i++;
        }


    }



    /// <summary>
    /// Selects the given scene element and foldout in the UI.
    /// </summary>
    /// <param name="Scenes_item">The sceneLevel element to select.</param>
    /// <param name="flodoutd">The foldout UI element for the sceneLevel.</param>
    private void ElemetSelect(SceneLevel Scenes_item, Label flodoutd)
    {
        foreach (var item in All_SceneObjectsFeild)
        {
            item.RemoveFromClassList("SelectElement");
        }
        SelectedScene = Scenes_item;

        flodoutd.AddToClassList("SelectElement");

        string ElementName = SelectedScene._scene == null ? "Null" : SelectedScene._scene.name;
    }


    void inilizePropsScroll()
    {
        if (EditorSceneManager.GetActiveScene().name == "Level") return;

        PPLatformPerant.Clear();
        int i = 0;
        foreach (var item in _pPlatforms)
        {
            VisualElement perantCon = new VisualElement();

            ObjectField objectPP = new ObjectField("Object");
            perantCon.AddToClassList(item.value < 0 ? "PerantFieldsPropRed" : "PerantFieldsPropBlue");

            if (item)
                objectPP.value = item.gameObject;

            IntegerField ValuePP = new IntegerField("Value");
            ValuePP.value = item.value;

            ValuePP.RegisterValueChangedCallback((e) =>
            {
                item.value = e.newValue;
                EditorUtility.SetDirty(item);
                perantCon.AddToClassList(item.value < 0 ? "PerantFieldsPropRed" : "PerantFieldsPropBlue");
                item.SetColor();

            });


            perantCon.Add(objectPP);
            perantCon.Add(ValuePP);

            PPLatformPerant.Add(perantCon);
            i++;



        }

    }

    void RandomValueProp()
    {
        if (EditorSceneManager.GetActiveScene().name == "Level") return;


        bool ValueUp = true;
        int to = 1;

        int i = 0;

        foreach (var item in _pPlatforms)
        {
            if (i == to)
            {
                to += i;

                ValueUp = true;
            }
            else
            {
                ValueUp = false;
            }

            if (ValueUp)
            {
                item.value = Random.Range(10, 40);
            }
            else
            {
                item.value = Random.Range(-10, -40);
            }



            item.SetColor();

            EditorUtility.SetDirty(item);
            i++;
        }
        inilizePropsScroll();

    }


    void Generate_LevelPeops()
    {
        if (EditorSceneManager.GetActiveScene().name == "Level") return;

        foreach (var item in PointsSpown)
        {
            if (item.childCount <= 0)
            {
                if (Random.Range(0, 3) < 1)
                {
                    Instantiate(BPlatfroObject, item.position, Quaternion.identity, item);
                }
                else
                {
                    Transform dd = Instantiate(PorrlWaterObject);
                    dd.SetParent(item.transform.parent);

                    if (Random.Range(0, 3) > 1)
                    {
                        dd.position = new Vector3(-0.80f, item.position.y, item.position.z);
                    }
                    else
                    {
                        dd.position = item.position;
                    }
                    dd.SetParent(item);
                }
            }
        }

    }
    public void RemoveAllElementsInScene()
    {
        if (EditorSceneManager.GetActiveScene().name == "Level") return;

        foreach (Transform item in PointsSpown)
        {
            for (int i = 0; i < item.childCount; i++)
            {
                if (!EditorApplication.isPlaying)
                    DestroyImmediate(item.GetChild(i).gameObject);
                else
                    Destroy(item.GetChild(i).gameObject);
            }
        }
    }
}
