using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public class CarControlle : MonoBehaviour
{
    [SerializeField] float Speed = 30;
    [SerializeField] Transform[] Wheels;
    [SerializeField] SwipManager swipManager;
    private void Start()
    {
        foreach (Transform item in Wheels)
        {
            item.DORotate(new Vector3(360, 0, 0), 0.3f, RotateMode.FastBeyond360).SetLoops(-1);

        }
    }


    private void Update()
    {
        Move();
    }

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



}
