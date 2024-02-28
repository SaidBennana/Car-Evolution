using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    [SerializeField] RectTransform Icon;
    [SerializeField] int levelIndex = 1;
    void Start()
    {
        Gley.MobileAds.API.Initialize();
        Gley.MobileAds.API.ShowAppOpen();
        if (Icon)
        {
            Icon.DOScale(new Vector3(7.73f, 7.73f, 7.73f), 1).SetDelay(1).SetEase(Ease.OutBack).OnComplete(() =>
            {
                LoadScene();
            });
        }
        else
        {
            LoadScene();
        }

    }

    void LoadScene()
    {
        levelIndex = LoadData<int>(SaveKeys.levelindexKey, 1);
        SceneManager.LoadScene(levelIndex);

        SceneManager.sceneLoaded +=LoadSceneEvenet;
    }

    private void LoadSceneEvenet(Scene arg0, LoadSceneMode arg1)
    {
        Gley.MobileAds.API.ShowBanner(Gley.MobileAds.BannerPosition.Bottom,Gley.MobileAds.BannerType.Banner);
    }

    /// Checks if a key exists in the save file and returns the value if it does, otherwise returns the default value.
    public T LoadData<T>(string key, T def)
    {
        if (ES3.FileExists("SaveFile.es3"))
        {
            if (ES3.KeyExists(key, "SaveFile.es3"))
            {
                return ES3.Load<T>(key);

            }

        }
        return def;

    }
}
