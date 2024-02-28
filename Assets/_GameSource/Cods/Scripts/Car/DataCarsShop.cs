using System;
using System.Collections.Generic;
using UnityEngine;

public class DataCarsShop : MonoBehaviour
{
    public static DataCarsShop ins;

    public List<Car> CarsLIstShop = new List<Car>();

    private void Awake()
    {
        ins = this;
    }
    private void Start()
    {
        CarsLIstShop = GameManager.Instance.LoadData<List<Car>>(SaveKeys.DataCardKey, CarsLIstShop);
        foreach (var item in CarsLIstShop)
        {
            item.CarObject.SetActive(false);
            if (item.is_Select)
            {
                item.CarObject.SetActive(true);

            }
        }
    }

    public void UNSelectAllCar(int indexSelect)
    {
        foreach (var item in CarsLIstShop)
        {
            item.is_Select = false;
            item.CarObject.SetActive(false);

        }
        CarsLIstShop[indexSelect].is_Select = true;
        CarsLIstShop[indexSelect].CarObject.SetActive(true);
        CarsLIstShop[indexSelect].is_get_it = true;
        ES3.Save(SaveKeys.DataCardKey, CarsLIstShop);
    }
}
[Serializable]
public class Car
{
    public int price;
    public GameObject CarObject;
    public Texture2D lockImage;
    public Texture2D CarImage;
    [Space(10)]
    public bool is_get_it = false;
    public bool is_Select = false;
}
