
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;

public class CarStoreEditorEindow : EditorWindow
{
    private DataCarsShop _DataCarsShop;
    [MenuItem("MyTools/CarsData")]
    private static void ShowWindow()
    {
        var window = GetWindow<CarStoreEditorEindow>();
        window.titleContent = new GUIContent("CarStoreEindow");
        window.maxSize = new Vector2(500, 700);
        window.minSize = window.maxSize;
        window.Show();
    }
    private void OnEnable()
    {
        GameObject CarObject = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Game Files/Prefabs/Car/Car.prefab");
        _DataCarsShop = CarObject.GetComponent<DataCarsShop>();

    }

    public void CreateGUI()
    {
        rootVisualElement.Add(new InspectorElement(_DataCarsShop));

    }

}
