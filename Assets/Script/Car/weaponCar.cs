using GDTools.ObjectPooling;
using UnityEngine;

public class weaponCar : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float distance = 3;
    [SerializeField] float FireRate = 1f;
    [SerializeField] float BullitSpeed = 100;

    [Header("Configer")]
    [SerializeField] LayerMask ItemShoots;
    [SerializeField] Transform PivotPointShoot;
    [SerializeField] Pool Bullit;


    private float FireRateTime;

    bool can_PluseRire = false;



    Ray ray;
    RaycastHit Hit;

    private void FixedUpdate()
    {
        ray.direction = Vector3.forward;
        ray.origin = PivotPointShoot.position;

        if (Physics.Raycast(ray, distance, ItemShoots))
        {
            if (Time.time > FireRateTime)
            {
                // Shoot
                PoolObject bullitIns = Bullit.InstantiateObject(PivotPointShoot.position);
                if (bullitIns.gameObject.TryGetComponent(out Rigidbody bb))
                {
                    bb.AddForce(transform.forward * BullitSpeed, ForceMode.Impulse);
                }
                Bullit.DestroyObject(bullitIns, 20);
                FireRateTime = Time.time + FireRate * Time.deltaTime;
            }
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(ray.origin, ray.direction * distance);
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "PPlatform":
                {
                    if (!can_PluseRire)
                    {
                        can_PluseRire = true;

                        if (other.gameObject.TryGetComponent(out PPlatform pPlatform))
                        {
                            print("fdsfdsf");
                            //FireRate -= pPlatform.value;
                            FireRate -= pPlatform.value;
                            FireRate = Mathf.Clamp(FireRate, 10, 100);

                        }
                    }

                    break;
                }
            case "BoolWater":
                {

                    break;
                }

        }
    }
    private void OnTriggerExit(Collider other)
    {
        switch (other.tag)
        {
            case "PPlatform":
                {
                    can_PluseRire = false;
                    break;

                }

        }
    }

}
