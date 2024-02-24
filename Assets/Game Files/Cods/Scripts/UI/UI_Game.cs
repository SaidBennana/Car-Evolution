using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UI_Game : MonoBehaviour
{
    public static UI_Game intance;
    [SerializeField] UIDocument _UIDocument;
    VisualElement root;

    #region Varbals UI
    Button StartGameButton;
    VisualElement WinUI, loseUI, StorePerant;
    ScrollView ScrollStore;

    #endregion

    #region  Varbals
    List<Car> _CarsData;

    #endregion

    private void Awake()
    {
        intance = this;
    }
    void Start()
    {
        root = _UIDocument.rootVisualElement;

        // initialize varbals UI
        WinUI = root.Q<VisualElement>("WinUI");
        loseUI = root.Q<VisualElement>("LoseUI");
        StorePerant = root.Q<VisualElement>("StorePerant");
        ScrollStore = StorePerant.Q<ScrollView>("ScrollView");


        /// Initializes the start game and other buttons 
        initializeButtons();

        DataCarsShop.ins.CarsLIstShop = GameManager.Instance.LoadData<List<Car>>(SaveKeys.DataCardKey, DataCarsShop.ins.CarsLIstShop);
        _CarsData = DataCarsShop.ins.CarsLIstShop;

        InitilaizeStore();

    }

    void initializeButtons()
    {
        StartGameButton = root.Q<Button>("StartGameButton");
        StartGameButton.clicked += () =>
        {
            GameManager.Instance.CameraStart.gameObject.SetActive(false);
            GameManager.Instance.CameraGame.gameObject.SetActive(true);
            StartCoroutine(GameManager.Instance.carControlle.StartGame(1));
            StartGameButton.parent.style.display = DisplayStyle.None;

        };

        WinUI.Q<Button>("NextGameButton").clicked += () =>
        {
            print("Next Level");

        };
        loseUI.Q<Button>("RelodeGameButton").clicked += () =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        };

        // Store Button open and close
        root.Q<Button>("OpenStoreButton").clicked += () =>
        {
            StorePerant.ToggleInClassList("Animation");
        };
        StorePerant.Q<Button>("CloseButton").clicked += () =>
        {
            StorePerant.ToggleInClassList("Animation");
        };


    }


    void InitilaizeStore()
    {
        ScrollStore.Clear();
        GroupBox groupBox = new GroupBox();
        groupBox.AddToClassList("CardPerents");
        int i = 0;
        foreach (var item in _CarsData)
        {

            Button Cars = new Button();

            if (item.is_Select)
                Cars.AddToClassList("CardStoreSelected");
            else
                Cars.AddToClassList("CardStore");

            if (item.is_get_it)
                Cars.style.backgroundImage = new StyleBackground(item.CarImage);
            else
            {

                Cars.style.backgroundImage = new StyleBackground(item.CarImage);

                IMGUIContainer iMGUIContainer = new IMGUIContainer();
                iMGUIContainer.name = "iMGUIContainer";

                Label PriceText = new Label();
                PriceText.text = $"{item.price}M";
                iMGUIContainer.Add(PriceText);
                Cars.Add(iMGUIContainer);
            }
            Cars.clicked += SelectItem(i);





            groupBox.Add(Cars);
            i++;

        }
        ScrollStore.Add(groupBox);

    }

    Action SelectItem(int index)
    {

        return () =>
        {
            Debug.Log(index);
            DataCarsShop.ins.UNSelectAllCar(index);
            InitilaizeStore();

        };

    }
    public IEnumerator WinFun()
    {
        yield return new WaitForSeconds(1.6f);
        WinUI.style.display = DisplayStyle.Flex;
    }

    public IEnumerator loseFun()
    {
        yield return new WaitForSeconds(1.6f);
        loseUI.style.display = DisplayStyle.Flex;
    }

}
