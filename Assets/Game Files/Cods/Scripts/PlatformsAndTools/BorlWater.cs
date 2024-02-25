using As_Star;
using DG.Tweening;
using UnityEngine;

public class BorlWater : MonoBehaviour
{
    [SerializeField] int health = 4;

    [SerializeField] Transform GFX;
    [SerializeField] Transform Effetc;
    [SerializeField] Transform MonyObject;
    private Transform Cars;

    private void Start()
    {
        Cars = FindObjectOfType<CarControlle>().transform;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullit"))
        {
            SoundManager.instance.PlayeWithIndex(7);
            health--;
            other.gameObject.SetActive(false);
            if (health < 0)
            {
                SoundManager.instance.PlayeWithIndex(6);

                for (int i = 0; i < 5; i++)
                {
                    Vector3 pos = new Vector3(transform.position.x + Random.Range(-2, 2), transform.position.y, transform.position.z + Random.Range(-2, 2));
                    Transform obj = Instantiate(MonyObject, transform.position, Quaternion.identity);
                    obj.DOJump(pos, 1, 2, 1).OnComplete(() =>
                    {
                        obj.DOMove(Cars.position, 1).OnComplete(() =>
                        {
                            SoundManager.instance.PlayeWithIndex(2);
                            Destroy(obj.gameObject);
                        });
                    });

                }
                distroyThis();
            }
        }
    }

    public void distroyThis()
    {
        GFX.gameObject.SetActive(false);
        Effetc.gameObject.SetActive(true);
        GetComponent<BoxCollider>().enabled = false;
    }

}
