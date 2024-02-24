using UnityEngine;

public class BorlWater : MonoBehaviour
{
    [SerializeField] int health = 4;

    [SerializeField] Transform GFX;
    [SerializeField] Transform Effetc;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullit"))
        {
            health--;
            other.gameObject.SetActive(false);
            if (health < 0)
            {
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
