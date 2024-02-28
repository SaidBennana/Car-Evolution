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






        Set_Style_Image_Objects();





        flod.Add(price);
        flod.Add(CarObject);
        flod.Add(imagesPerant);

        if (property.FindPropertyRelative("CarObject").objectReferenceValue)
            flod.text = property.FindPropertyRelative("CarObject").objectReferenceValue.name;
        else
        {
            flod.text = "Car Null Object";

        }





        root.Add(flod);

        return root;
    }
    void Set_Style_Image_Objects()
    {
        lockImage.AddToClassList("ImageFeild");
        CarImage.AddToClassList("ImageFeild");


        VisualElement ImageObject = new VisualElement();
        ImageObject.AddToClassList("ImageObject");
        CarImage.Add(ImageObject);


        ImageObject = new VisualElement();
        ImageObject.AddToClassList("ImageObject");
        lockImage.Add(ImageObject);


        imagesPerant.Add(lockImage);
        imagesPerant.Add(CarImage);

    }


}


