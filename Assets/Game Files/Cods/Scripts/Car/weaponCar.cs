using As_Star;
using GDTools.ObjectPooling;
using UnityEngine;

public class weaponCar : MonoBehaviour
{
    [Header("Settings")]
    /// Distance in units that the weapon has range before despawning.
    /// Fire rate in seconds per shot for the weapon.
    /// Speed of each shot in units per second.
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
        /// Checks if shooting is allowed before shooting a raycast forward from the pivot point shoot transform.
        if (!GameManager.Instance._Can_Shoot && !GameManager.Instance.Game_Start) return;
        ray.direction = Vector3.forward;
        ray.origin = PivotPointShoot.position;

        /// Checks if raycast hits ItemShoots layer within distance. 
        /// If hit, checks if enough time passed since last shot.
        /// If so, instantiates bullet from pool, sets velocity and destroy timer.
        /// Updates last shot timer.
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
            /// <summary>
            /// Checks if colliding with PPlatform and updates fire rate if so.
            /// Only allows updating fire rate once until next collision. 
            /// Clamps fire rate between min and max values.
            /// </summary>
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
            /// Checks if colliding with PPlatform and updates can_PluseRire flag
            /// Resets can_PluseRire to false when colliding with PPlatform
            /// This prevents updating fire rate more than once per PPlatform collision
            case "PPlatform":
                {
                    can_PluseRire = false;
                    break;

                }

        }
    }

}
