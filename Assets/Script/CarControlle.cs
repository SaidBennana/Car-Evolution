using DG.Tweening;
using UnityEngine;

public class CarControlle : MonoBehaviour
{
    [SerializeField] float Speed = 30;
    [SerializeField] Transform[] Wheels;
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
        transform.Translate(0, 0, Speed * Time.deltaTime);

    }



}
