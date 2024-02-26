using UnityEditor;
using UnityEditor.Rendering;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(DataCarsShop))]
public class CarStore : Editor
{
    public VisualTreeAsset U_UXML;


    public override VisualElement CreateInspectorGUI()
    {
        VisualElement root = new VisualElement();
        U_UXML.CloneTree(root);
        return root;
    }




}
[CustomPropertyDrawer(typeof(Car))]
public class CatEditor : PropertyDrawer
{
    PropertyField lockImage;
    PropertyField CarImage;
    VisualElement imagesPerant;
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        var root = new VisualElement();

        var flod = new Foldout();
        flod.value = false;
        flod.AddToClassList("FoldOut");

        imagesPerant = new VisualElement();
        imagesPerant.AddToClassList("ImagePerants");



        // property.FindPropertyRelative("lockImage").objectReferenceValu
        PropertyField price = new PropertyField(property.FindPropertyRelative("price"), "price");
        PropertyField CarObject = new PropertyField(property.FindPropertyRelative("CarObject"), "CarObject");

        // lockImage = new ObjectField("lockImage");
        // CarImage = new ObjectField("CarImage");
        lockImage = new PropertyField(property.FindPropertyRelative("lockImage"));
        CarImage = new PropertyField(property.FindPropertyRelative("CarImage"));






        Set_Style_Image_Objects(property);





        flod.Add(price);
        flod.Add(CarObject);
        flod.Add(imagesPerant);

        flod.text = property.FindPropertyRelative("CarObject").objectReferenceValue.name;





        root.Add(flod);

        return root;
    }
    void Set_Style_Image_Objects(SerializedProperty property)
    {
        lockImage.AddToClassList("ImageFeild");
        CarImage.AddToClassList("ImageFeild");

        /// CarImage initialize 
        // CarImage.objectType = typeof(Texture2D);
        // CarImage.value = property.FindPropertyRelative("CarImage").objectReferenceValue;

        VisualElement ImageObject = new VisualElement();
        ImageObject.AddToClassList("ImageObject");
        CarImage.Add(ImageObject);
        /// CarImage initialize end


        /// lockImage initialize
        // lockImage.objectType = typeof(Texture2D);
        // lockImage.value = property.FindPropertyRelative("lockImage").objectReferenceValue;

        ImageObject = new VisualElement();
        ImageObject.AddToClassList("ImageObject");
        lockImage.Add(ImageObject);

        /// lockImage initialize end


        // lockImage.RegisterValueChangedCallback((vv) =>
        // {
        //     Debug.Log("Image Changet");
        //     property.FindPropertyRelative("lockImage").objectReferenceValue = vv.newValue;

        // });
        // CarImage.RegisterValueChangedCallback((vv) =>
        // {
        //     property.FindPropertyRelative("CarImage").objectReferenceValue = vv.newValue;

        // });

        imagesPerant.Add(lockImage);
        imagesPerant.Add(CarImage);

    }


}


/*
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
*/

