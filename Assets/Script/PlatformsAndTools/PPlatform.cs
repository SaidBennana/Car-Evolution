using TMPro;
using UnityEngine;

public class PPlatform : MonoBehaviour
{
    public int value;
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] TextMeshPro textMesh;
    [SerializeField] Material blue, Red;

    private void Start()
    {
        if (value < 0)
        {
            meshRenderer.material = Red;
        }
        else
        {
            meshRenderer.material = blue;
        }

        textMesh.text = value.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullit"))
        {
            if (value < 0)
            {
                value -= 1;
            }
            else
            {
                value += 1;
            }
            textMesh.text = value.ToString();
            other.gameObject.SetActive(false);
        }
    }

}
