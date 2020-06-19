using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class HousingInfo : MonoBehaviour
{
    public Text current;
    public Text max;

    private static HousingInfo Instance;

    private Housing housing;

    private void Awake()
    {
        Instance = this;
        this.gameObject.SetActive(false);
    }
    public static void Show(Housing housing)
    {
        Instance.housing = housing;
        Instance.gameObject.SetActive(true);
    }

    void Update()
    {
        current.text = housing.GetResidents().ToString();
        max.text = housing.GetMaxRooms().ToString();
    }
}
