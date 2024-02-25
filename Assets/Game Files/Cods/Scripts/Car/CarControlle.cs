using System.Collections;
using As_Star;
using DG.Tweening;
using UnityEngine;

public class CarControlle : MonoBehaviour
{
    [SerializeField] float Speed = 30;
    [SerializeField] Transform[] Wheels;
    [SerializeField] SwipManager swipManager;


    [Header("Exploion Effetc")]
    [SerializeField] Transform EffetcExplosion;

    /// Updates the car's movement each frame during gameplay.
    /// Only moves the car if the game has started.
    private void Update()
    {
        if (GameManager.Instance.Game_Start)
            Move();
    }
    /// Waits for the specified time before starting the game. 
    /// Sets the car speed to 2, rotates the wheels continuously, 
    /// and sets the game start flag to true.

    public IEnumerator StartGame(float time)
    {
        yield return new WaitForSeconds(time);
        Speed = 2;
        foreach (Transform item in Wheels)
        {
            item.DORotate(new Vector3(360, 0, 0), 0.3f, RotateMode.FastBeyond360).SetLoops(-1);
        }
        GameManager.Instance.Game_Start = true;
    }

    /// Moves the car horizontally based on swipe input. 
    /// Checks swipeLeft and swipeRight bools to determine which direction to move.
    /// Moves the car until it reaches the specified x bounds.
    /// Calls limitPos() afterwards to constrain position.
    void Move()
    {
        if (swipManager.swipeRight && transform.position.x < 0.701f)
        {
            print("right");
            transform.DOMoveX(0.701f, 0.5f);
        }
        if (swipManager.swipeLeft && transform.position.x > -0.701f)
        {
            print("Lift");
            transform.DOMoveX(-0.701f, 0.5f);
        }

        transform.Translate(0, 0, Speed * Time.deltaTime);
        limitPos();

    }

    void limitPos()
    {
        if (transform.position.x < -0.701f)
        {
            transform.position = new Vector3(-0.701f, transform.position.y, transform.position.z);
        }
        else if (transform.position.x > 0.701f)
        {
            transform.position = new Vector3(0.701f, transform.position.y, transform.position.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        /// Stops the car on collision with water, plays explosion effect, 
        /// stops wheel rotation, destroys the water object, and disables shooting.
        /// Handles the collision logic when the car hits water.
        switch (other.tag)
        {
            case "finashLine":
                {
                    GameManager.Instance.FinshLine = true;
                    Debug.Log("finashLine");
                    break;
                }
            case "BoolWater":
                {
                    Speed = 0;
                    EffetcExplosion.gameObject.SetActive(true);
                    foreach (Transform item in Wheels)
                    {
                        DOTween.Kill(item);
                    }
                    if (other.gameObject.TryGetComponent(out BorlWater borl))
                    {
                        borl.distroyThis();
                    }
                    GameManager.Instance._Can_Shoot = false;
                    if (GameManager.Instance.FinshLine)
                    {
                        StartCoroutine(UI_Game.intance.WinFun()); ;
                        SoundManager.instance.PlayeWithIndex(3);


                    }
                    else
                    {
                        StartCoroutine(UI_Game.intance.loseFun());
                        SoundManager.instance.PlayeWithIndex(4);

                    }
                    SoundManager.instance.PlayeWithIndex(5);

                    break;
                }


        }
    }



}
