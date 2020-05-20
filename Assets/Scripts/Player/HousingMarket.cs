using System.Collections.Generic;
using UnityEngine;

public class HousingMarket : MonoBehaviour
{

    private static List<Housing> houses = new List<Housing>();
 
    public static void AddHouse(Housing housing)
    {
        houses.Add(housing);
    }

    public static void RemoveHouse(Housing housing)
    {
        houses.Remove(housing);
    }

    public static Housing GetVacantHouse()
    {
        foreach(Housing housing in houses)
        {
            if(housing.GetVacantRooms() > 0)
            {
                return housing;
            }
        }
        return null;
    }

    public static int GetVacantPlacesNum()
    {
        var num = 0;
        foreach(Housing housing in houses)
        {
            num += housing.GetVacantRooms();
        }
        return num;
    }

    void Start()
    {
        DebugPanel.AddDebug(() => { return GetVacantPlacesNum().ToString(); }, "Vacant rooms");
    }
}
