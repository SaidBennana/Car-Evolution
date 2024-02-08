using UnityEngine;

public class UI_Manager : MonoBehaviour
{


    public void StartGame()
    {
        GameManager.Instance.CameraStart.gameObject.SetActive(false);
        GameManager.Instance.CameraGame.gameObject.SetActive(true);
        StartCoroutine(GameManager.Instance.carControlle.StartGame(1));

    }
}
