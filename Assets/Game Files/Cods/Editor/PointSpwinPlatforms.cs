using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointSpwinPlatforms : MonoBehaviour
{
    public List<Transform> PointSpwon = new List<Transform>();

    [SerializeField] Transform BB, PorrlWater;


    public void SpwinBB()
    {

        foreach (var item in PointSpwon)
        {
            if (item.childCount <= 0)
            {
                if (Random.Range(0, 3) < 1)
                {
                    Instantiate(BB, item.position, Quaternion.identity, item);
                }
                else
                {
                    SpwinPorrlWater(item);
                }
            }
        }
    }
    public void SpwinPorrlWater(Transform perant)
    {
        Transform dd = Instantiate(PorrlWater);
        dd.SetParent(perant.transform.parent);

        if (Random.Range(0, 3) > 1)
        {
            dd.position = new Vector3(-0.80f, perant.position.y, perant.position.z);
        }
        else
        {
            dd.position = perant.position;
        }
        dd.SetParent(perant);
    }

    public void RemoveAllElementsInScene()
    {
        foreach (Transform item in PointSpwon)
        {
            for (int i = 0; i < item.childCount; i++)
            {
                Destroy(item.GetChild(i).gameObject);
            }
        }
        SpwinBB();
    }



}
