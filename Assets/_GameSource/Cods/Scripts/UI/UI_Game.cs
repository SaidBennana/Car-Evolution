using System;
using System.Collections;
using System.Collections.Generic;
using As_Star;
using DG.Tweening;
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
    VisualElement NavBarStartGame;
    VisualElement Setting;
    ScrollView ScrollStore;
    Label StartUIMont;

    #endregion

    #region  Varbals
    private int CountShowAds = 3;
    List<Car> _CarsData;

    #endregion

    private void Awake()
    {
        intance = this;

        root = _UIDocument.rootVisualElement;

        // initialize varbals UI
        WinUI = root.Q<VisualElement>("WinUI");
        loseUI = root.Q<VisualElement>("LoseUI");
        StorePerant = root.Q<VisualElement>("StorePerant");
        ScrollStore = StorePerant.Q<ScrollView>("ScrollView");
        StartUIMont = root.Q<Label>("StartUIMont");
        NavBarStartGame = root.Q<VisualElement>("NavBarStartGame");
        Setting = NavBarStartGame.Q<VisualElement>("Setting");

    }
    void Start()
    {
        SetTextMony(GameManager.Instance.Mony);


        /// Initializes the start game and other buttons 
        initializeButtons();

        DataCarsShop.ins.CarsLIstShop = GameManager.Instance.LoadData<List<Car>>(SaveKeys.DataCardKey, DataCarsShop.ins.CarsLIstShop);
        _CarsData = DataCarsShop.ins.CarsLIstShop;

        InitilaizeStore();

    }

    /// Initializes the start game button and other buttons in the UI.
    void initializeButtons()
    {
        StartGameButton = root.Q<Button>("StartGameButton");
        StartGameButton.clicked += () =>
        {
            GameManager.Instance.CameraStart.gameObject.SetActive(false);
            GameManager.Instance.CameraGame.gameObject.SetActive(true);
            StartCoroutine(GameManager.Instance.carControlle.StartGame(1));
            StartGameButton.parent.style.display = DisplayStyle.None;
            SoundManager.instance.PlayeWithIndex(0);

        };

        WinUI.Q<Button>("NextGameButton").clicked += () =>
        {
            if ((SceneManager.GetActiveScene().buildIndex + 1) == SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(1);
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            ES3.Save(SaveKeys.levelindexKey, SceneManager.GetActiveScene().buildIndex);

            SoundManager.instance.PlayeWithIndex(0);
            ShowAds();


        };
        loseUI.Q<Button>("RelodeGameButton").clicked += () =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            SoundManager.instance.PlayeWithIndex(0);
            ShowAds();

        };

        // Store Button open and close
        root.Q<Button>("OpenStoreButton").clicked += () =>
        {
            SoundManager.instance.PlayeWithIndex(0);
            StorePerant.ToggleInClassList("Animation");
        };
        StorePerant.Q<Button>("CloseButton").clicked += () =>
        {
            SoundManager.instance.PlayeWithIndex(0);
            StorePerant.ToggleInClassList("Animation");
        };
        NavBarStartGame.Q<Button>("SettingButton").clicked += () =>
        {
            Setting.ToggleInClassList("CloseSetting");
        };

        // Sound Manager

        Setting.Q<Button>("Sound").clicked += () =>
        {
            Setting.Q<Button>("Sound").ToggleInClassList("SoundClose");

            SoundManager.instance.CanPlaySound = !SoundManager.instance.CanPlaySound;

            ES3.Save(SaveKeys.SoundKey, SoundManager.instance.CanPlaySound);


            SoundManager.instance.PlayeWithIndex(0);
        };

        Setting.Q<Button>("Music").clicked += () =>
        {
            Setting.Q<Button>("Music").ToggleInClassList("MusicClose");

            SoundManager.instance.CanPlayMusic = !SoundManager.instance.CanPlayMusic;
            SoundManager.instance.MusicControlle();

            ES3.Save(SaveKeys.MusicKey, SoundManager.instance.CanPlayMusic);

            SoundManager.instance.PlayeWithIndex(0);

        };

        /// Handles click event for the "GetReword" button on the lose UI.
        /// Shows a rewarded video ad and grants reward if completed.
        loseUI.Q<Button>("GetReword").clicked += () =>
        {
            /// Shows a rewarded video ad using the Gley Mobile Ads API.
            /// The complate callback indicates if the user completed watching the ad.
            /// If complate is true, the reward can be granted.
            Gley.MobileAds.API.ShowRewardedVideo((complate) =>
            {
                if (complate)
                {
                    /// Increases the player's money by 500 when a rewarded video ad is completed.
                    /// This rewards the player for watching the full video advertisement.
                    GameManager.Instance.Mony += 500;

                    /// Saves the player's current money to persistent storage using ES3.
                    /// The money amount is saved with the key 'MonyKey'.
                    /// This allows the player's money to persist across game sessions.
                    ES3.Save(SaveKeys.MonyKey, GameManager.Instance.Mony);

                    SoundManager.instance.PlayeWithIndex(8);

                }

            });
            Debug.Log("Get Reword lose");

        };

        // Rewords Button win lose 
        WinUI.Q<Button>("GetReword").clicked += () =>
        {
            Gley.MobileAds.API.ShowRewardedVideo((complate) =>
            {
                if (complate)
                {
                    GameManager.Instance.Mony += 500;
                    ES3.Save(SaveKeys.MonyKey, GameManager.Instance.Mony);
                    SoundManager.instance.PlayeWithIndex(8);
                }

            });
            Debug.Log("Get Reword Win");

        };




    }
    // Button Sound And Music StartGame load data
    public void InitButtons(bool Sound, bool music)
    {
        if (!Sound)
            Setting.Q<Button>("Sound").ToggleInClassList("SoundClose");
        if (!music)
            Setting.Q<Button>("Music").ToggleInClassList("MusicClose");




    }


    /// <summary>
    /// Initializes the in-game store UI and hooks up event handlers.
    /// </summary>
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

    /// <summary>
    /// Selects the car item at the given index when clicked.
    /// </summary>
    Action SelectItem(int index)
    {
        /// Selects the car item at the given index when clicked.
        /// Checks if the car is already owned, plays a sound, and updates the UI.
        /// If not owned, checks if there is enough money to purchase, 
        /// completes the purchase, plays a purchase sound, updates money text,
        /// deselects other cars, and re-initializes the store UI.
        /// If not enough money, plays an error sound.

        return () =>
        {
            if (_CarsData[index].is_get_it)
            {

                DataCarsShop.ins.UNSelectAllCar(index);
                SoundManager.instance.PlayeWithIndex(0);

            }
            else if (GameManager.Instance.Mony >= _CarsData[index].price)
            {
                SoundManager.instance.PlayeWithIndex(8);
                GameManager.Instance.Mony -= _CarsData[index].price;
                DataCarsShop.ins.UNSelectAllCar(index);
                SetTextMony(GameManager.Instance.Mony);
            }
            else if (GameManager.Instance.Mony <= _CarsData[index].price)
            {
                SoundManager.instance.PlayeWithIndex(9);

            }

            InitilaizeStore();
        };

    }

    /// <summary>
    /// Coroutine that handles win game logic and UI.
    /// </summary>
    public IEnumerator WinFun()
    {
        GameManager.Instance.Game_Start = false;


        yield return new WaitForSeconds(1.6f);
        WinUI.style.display = DisplayStyle.Flex;
        int MonyAnimtion = 0;

        Label WinScroreText;
        WinScroreText = WinUI.Q<Label>("WinScroreText");

        int value = -170;
        DOTween.To(() => value, x => value = x, 0, 1).SetEase(Ease.InBack).OnUpdate(() =>
        {
            WinUI.style.translate = new StyleTranslate(new Translate(new Length(value, LengthUnit.Percent), 0));
        }).OnComplete(() =>
        {
            DOTween.To(() => MonyAnimtion, x => MonyAnimtion = x, GameManager.Instance.Mony, 1).OnUpdate(() =>
            {
                WinScroreText.text = MonyAnimtion.ToString();
            });

        });
    }

    /// Coroutine that handles the lose game flow by displaying the lose UI.
    public IEnumerator loseFun()
    {
        GameManager.Instance.Game_Start = false;

        yield return new WaitForSeconds(1.6f);
        loseUI.style.display = DisplayStyle.Flex;

        int MonyAnimtion = 0;

        Label LoseScroreText;
        LoseScroreText = loseUI.Q<Label>("LoseScroreText");
        int value = 170;
        DOTween.To(() => value, x => value = x, 0, 1).SetEase(Ease.InBack).OnUpdate(() =>
        {
            loseUI.style.translate = new StyleTranslate(new Translate(new Length(value, LengthUnit.Percent), 0));

        }).OnComplete(() =>
        {
            DOTween.To(() => MonyAnimtion, x => MonyAnimtion = x, GameManager.Instance.Mony, 1).OnUpdate(() =>
            {
                LoseScroreText.text = MonyAnimtion.ToString();
            });

        });
    }

    public void SetTextMony(int mony)
    {
        ES3.Save(SaveKeys.MonyKey, GameManager.Instance.Mony);
        StartUIMont.text = $"{mony}M";
    }

    /// <summary>
    /// Shows an interstitial ad if the count of ads shown is less than the max count. 
    /// Updates the count of ads shown after showing an ad.
    /// </summary>
    void ShowAds()
    {
        if (CountShowAds <= 0)
        {
            Gley.MobileAds.API.ShowInterstitial();
            CountShowAds = GameManager.Instance.MaxCountShowAds;
        }
        else
        {
            CountShowAds--;
        }


    }
}

