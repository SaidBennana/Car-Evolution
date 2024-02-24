using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Rendering;

[CustomEditor(typeof(DataCarsShop))]
public class CarStore : Editor
{
    SerializedProperty ThisList;
    SerializedObject GetTarget;
    Vector2 scrollPos;

    List<bool> flodOut = new List<bool>();

    Transform PerantOmjects;



    private void OnEnable()
    {
        GetTarget = new SerializedObject(target);
        ThisList = GetTarget.FindProperty("CarsLIstShop");
        flodOut.Clear();
        for (int i = 0; i < ThisList.arraySize; i++)
        {
            flodOut.Add(new bool());

        }
    }


    public override void OnInspectorGUI()
    {
        GetTarget.Update();
        // PerantOmjects = (Transform)EditorGUILayout.ObjectField("perantObject", PerantOmjects, typeof(Transform));

        // if (GUILayout.Button("Set All Object"))
        // {
        //     DataCarsShop dataCarsShop = (DataCarsShop)target;
        //     dataCarsShop.CarsLIstShop.Clear();

        //     for (int i = 0; i < PerantOmjects.childCount; i++)
        //     {

        //         Car cd = new Car();
        //         cd.CarObject = PerantOmjects.GetChild(i).gameObject;
        //         dataCarsShop.CarsLIstShop.Add(cd);
        //     }



        // }


        using (var ss = new EditorGUILayout.ScrollViewScope(scrollPos, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
        {
            scrollPos = ss.scrollPosition;

            for (int i = 0; i < ThisList.arraySize; i++)
            {

                SerializedProperty MyListRef = ThisList.GetArrayElementAtIndex(i);
                SerializedProperty price = MyListRef.FindPropertyRelative("price");
                SerializedProperty CarObject = MyListRef.FindPropertyRelative("CarObject");
                SerializedProperty lockImage = MyListRef.FindPropertyRelative("lockImage");
                SerializedProperty CarImage = MyListRef.FindPropertyRelative("CarImage");
                SerializedProperty is_get_it = MyListRef.FindPropertyRelative("is_get_it");
                SerializedProperty is_Select = MyListRef.FindPropertyRelative("is_Select");


                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                flodOut[i] = EditorGUILayout.Foldout(flodOut[i], $"Element {i}", true, EditorStyles.foldout);
                if (flodOut[i])
                {

                    price.intValue = EditorGUILayout.IntField("price", price.intValue);

                    CarObject.objectReferenceValue = (GameObject)EditorGUILayout.ObjectField("Car Object", CarObject.objectReferenceValue, typeof(GameObject));

                    CarImage.objectReferenceValue = (Texture2D)EditorGUILayout.ObjectField("CarImage", CarImage.objectReferenceValue, typeof(Texture2D));
                    lockImage.objectReferenceValue = (Texture2D)EditorGUILayout.ObjectField("lockImage", lockImage.objectReferenceValue, typeof(Texture2D));


                    GUILayout.Space(10);
                    EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
                    is_get_it.boolValue = EditorGUILayout.Toggle("is_get_it", is_get_it.boolValue, GUILayout.ExpandWidth(true));
                    is_Select.boolValue = EditorGUILayout.Toggle("is_Select", is_Select.boolValue, GUILayout.ExpandWidth(true));
                    EditorGUILayout.EndHorizontal();
                    GUILayout.Space(10);
                    if (GUILayout.Button("Remove"))
                    {
                        if (EditorUtility.DisplayDialog("Remove", "Are you sure you wont to remove this object", "ok", "no"))
                        {
                            DataCarsShop dataCarsShop = (DataCarsShop)target;
                            dataCarsShop.CarsLIstShop.RemoveAt(i);
                            flodOut.RemoveAt(i);
                        }
                    }

                    GUILayout.Space(10);
                }



                EditorGUILayout.EndVertical();


            }

        }
        if (GUILayout.Button("Add"))
        {
            DataCarsShop dataCarsShop = (DataCarsShop)target;
            dataCarsShop.CarsLIstShop.Add(new Car());
            flodOut.Add(new bool());
        }




        GetTarget.ApplyModifiedProperties();
    }



}
