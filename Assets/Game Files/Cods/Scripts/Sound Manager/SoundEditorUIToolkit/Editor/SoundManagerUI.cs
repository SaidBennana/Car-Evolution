using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;
using System;
using static UnityEngine.UI.InputField;
using System.Linq;
using System.IO;

public class SoundManagerUI : EditorWindow
{

    #region varibel UI
    ScrollView _ScrollView;
    VisualElement CardSound;
    Foldout All_Sounds;
    Foldout foldSound;
    static VisualTreeAsset _CardSoundTree;


    #endregion

    #region Varibals
    static SoundObject _SoundObject;

    static List<Sound> ListSounds = new List<Sound>();
    #endregion

    [MenuItem("MyTools/SoundManager")]
    private static void ShowWindow()
    {

        SoundManagerUI window = GetWindow<SoundManagerUI>();
        window.titleContent = new GUIContent("SoundManagerUI");
        window.maxSize = new Vector2(500, 600);
        window.minSize = window.maxSize;
        window.Show();

    }

    private void CreateGUI()
    {
        /// Loads the VisualTreeAsset for the sound card UI component.
        /// This allows us to instantiate the reusable sound card UI prefab.
        _CardSoundTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("CardSound_AsSatr_8988475435& t:VisualTreeAsset")[0]));


        /// Loads the SoundObject asset which contains the list of Sounds.
        /// Gets the ListSounds from the SoundObject to populate the UI.
        if (!Directory.Exists("Assets/Resources")) Directory.CreateDirectory("Assets/Resources");
        if (!File.Exists("Assets/Resources/SoundData.asset"))
        {
            SoundObject SoundObjectCreate = new SoundObject();
            AssetDatabase.CreateAsset(SoundObjectCreate, "Assets/Resources/SoundData.asset");
        }
        _SoundObject = (SoundObject)AssetDatabase.LoadAssetAtPath("Assets/Resources/SoundData.asset", typeof(SoundObject));

        ListSounds = _SoundObject.Sounds;


        VisualElement root = rootVisualElement;
        string[] dd = AssetDatabase.FindAssets("UI_Sound_AsSatr_8988475435& t:VisualTreeAsset");
        Debug.Log(dd.Length);
        VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(AssetDatabase.GUIDToAssetPath(dd[0]));
        root.Add(visualTree.Instantiate());

        // Add Button
        root.Q<Button>("AddButton").clicked += () =>
        {
            ListSounds.Add(new Sound());
            _SoundObject.Sounds = ListSounds;
            All_Sounds.Clear();
            generateCard();
        };
        root.Q<Button>("Save").clicked += () =>
        {
            Save();
        };






        /// Retrieves the ScrollView visual element and adds a Foldout element 
        /// for displaying all sounds.
        _ScrollView = root.Q<ScrollView>("_ScrollView");
        All_Sounds = new Foldout();
        All_Sounds.name = "All_Sounds";
        All_Sounds.text = "All Sounds";
        All_Sounds.AddToClassList("All_Sounds");
        _ScrollView.Add(All_Sounds);



        Debug.Log("ss2");
        generateCard();



    }
    private void OnDestroy()
    {
        Save();
    }



    public void generateCard()
    {
        int i = 0;
        foreach (Sound item in ListSounds.ToList())
        {
            foldSound = new Foldout();

            foldSound.AddToClassList("SoundCards");



            VisualElement Card = _CardSoundTree.Instantiate().Q<VisualElement>("CardSound");
            Card.Q<Button>("Remove").clicked += () =>
            {
                if (EditorUtility.DisplayDialog("Remove Clip", $"Are you sure you want to remove \"{item.nameClip}\"", "OK", "NO"))
                {
                    ListSounds.Remove(item);
                    All_Sounds.Clear();
                    generateCard();
                }
            };

            if (string.IsNullOrEmpty(item.nameClip))
            {
                foldSound.text = $"Element (Index {i}) ";

                Card.Q<Label>("NameClip").text = $"Element (Index {i}) ";
            }
            else
            {
                foldSound.text = $"{item.nameClip} (Index {i}) ";

                Card.Q<Label>("NameClip").text = $"{item.nameClip} (Index {i}) ";
            }


            Card.Q<TextField>("ClipnameFeild").value = item.nameClip;
            Card.Q<ObjectField>("ClipObject").value = item.clip;
            Card.Q<Slider>("_Volame").value = item.volume;

            // // Events

            Card.Q<TextField>("ClipnameFeild").RegisterValueChangedCallback(x =>
            {
                item.nameClip = x.newValue;
            });
            Card.Q<ObjectField>("ClipObject").RegisterValueChangedCallback(x =>
            {
                item.clip = (AudioClip)x.newValue;
            });

            Card.Q<Slider>("_Volame").RegisterValueChangedCallback(x =>
            {
                item.volume = x.newValue;
            });




            foldSound.Add(Card);
            foldSound.value = false;

            All_Sounds.Add(foldSound);
            i++;
        }

    }

    void Save()
    {
        _SoundObject.Sounds = ListSounds;
        EditorUtility.SetDirty(_SoundObject);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

    }



}


