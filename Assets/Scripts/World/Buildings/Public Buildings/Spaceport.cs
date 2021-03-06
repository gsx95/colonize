﻿using System;
using System.Linq;
using UnityEngine;

public class Spaceport : Building {
    public GameObject citizenPrefab;

    public SpaceportInfo spaceportInfo;
    
    private static float newCitizenTimerIngameHours = 72;
    private int newCitizensNum = 1;

    private int spawnedCitizen = 0;

    private string newCitizensTimerId;

    public GameObject spawnPosObj;
    void Start() {
        var rnd = new System.Random(DateTime.Now.Millisecond);
        names = names.OrderBy(x => rnd.Next()).ToArray();

        newCitizensTimerId = Clock.AddTimer(TimerEnded, newCitizenTimerIngameHours);

        DebugPanel.AddDebug(() => { return newCitizensNum.ToString(); }, "Max Arrivals");
        DebugPanel.AddDebug(() => { return ActualArrivals().ToString(); }, "Actual Arrivals");

        SpawnNew();
        //Clock.AddOneTimeTimer((s) => { SpawnNew(8); }, 4);
        //Clock.AddOneTimeTimer((s) => { SpawnNew(8); }, 8);
    }

    // Update is called once per frame
    void Update() {

    }

    public void ShowInfo()
    {
        spaceportInfo.Show(newCitizensTimerId, () => { return ActualArrivals();  });
    }

    private void TimerEnded(string id) {
        int actualArrivals = ActualArrivals();
        for (int i = 0; i < actualArrivals; i++) {
           // SpawnNew();
        }
    }

    private void SpawnNew() {

        var posY = citizenPrefab.transform.position.y;
        var newC = Instantiate(citizenPrefab, exit.transform.position, Quaternion.identity);
        newC.transform.position = new Vector3(newC.transform.position.x, posY, newC.transform.position.z);
        var name = names[spawnedCitizen];
        newC.GetComponent<Citizen>().SetName(name);
        spawnedCitizen++;
        Census.AddCitizen(newC.GetComponent<Citizen>());
        ToastBox.ShowMsg(newC.GetComponent<Citizen>().Name() + " arrived.");
    }

    private int ActualArrivals() {
        int max = newCitizensNum;
        int freeRooms = HousingMarket.GetVacantPlacesNum();
        int alarmLimit = max;
        if (Census.GetCitizensNum() > 9 && (ResourceHolder.FoodAlarmLast3Days() || ResourceHolder.WaterAlarmLast3Days())) {
            alarmLimit = Mathf.FloorToInt(max * 0.25f);
        }
        return Min(freeRooms, alarmLimit, max);
    }

    private static int Min(params int[] values) {
        return Enumerable.Min(values);
    }

    private static string[] names = new string[] {
        "Kajol Benson",
        "Amrit Shea",
        "Caelan Esquivel",
        "Sally Velazquez",
        "Tasha Mckee",
        "Kaycee Gilmour",
        "Eadie Callahan",
        "Rebecca Bate",
        "Maureen Lyons",
        "Paisley Haigh",
        "Helen Eastwood",
        "Eliot Cartwright",
        "Alaw House",
        "Lilly Hubbard",
        "Hafsa Mann",
        "Drew Bean",
        "Haya Hulme",
        "Bertha Peel",
        "Josie Hutchings",
        "Tania Browne",
        "Jodie Flower",
        "Menna Drummond",
        "Lula Dickerson",
        "Kelsie Bishop",
        "Isla-Mae Duran",
        "Marina O'Moore",
        "Beulah Russo",
        "Angelica Bauer",
        "Stevie Kim",
        "Glen Wilkins",
        "Laaibah Middleton",
        "Eleasha Melia",
        "Vicki Frederick",
        "Bethanie Hawes",
        "Iga Donovan",
        "Louisa Benton",
        "Yvette Austin",
        "Kim Mitchell",
        "Atlanta Burch",
        "Sarah-Jayne Lam",
        "Viktoria Bentley",
        "Tyra Watkins",
        "Jaskaran Mccann",
        "Kiya Nava",
        "Lorena Chang",
        "Sukhmani Sandoval",
        "Rui Lambert",
        "Ellesse Eaton",
        "Safa Dickinson",
        "Talia Churchill",
        "Sulaiman Ewing",
        "Eduard Fulton",
        "Brendon Franklin",
        "Stella Hull",
        "Diego Sullivan",
        "Peyton Robins",
        "Richard Barrow",
        "Carter Pope",
        "Nasir Pennington",
        "Karson Neal",
        "Jaylan Wright",
        "Abdurrahman Strickland",
        "Landon House",
        "Tayla Mcgregor",
        "Kairon Blevins",
        "Tommy Bass",
        "Evangeline Oakley",
        "Olly Markham",
        "Kallum Alcock",
        "Charli Bailey",
        "Lukas Houston",
        "Noah Cooper",
        "Aled Draper",
        "Connah Devine",
        "Charles Everett",
        "Maxim Vincent",
        "Ariya Nava",
        "Zachery Osborn",
        "Denzel Conner",
        "Cassius Alvarez",
        "Kia Ashley",
        "Zidane Burke",
        "Bailey Hebert",
        "Roma Savage",
        "Lloyd Summers",
        "Zander Charles",
        "Kajetan Short",
        "Ayush Leech",
        "Jillian Rigby",
        "Mitchell Rees",
        "Andrea Rosales",
        "Elena Busby",
        "Macy Sanchez",
        "Kenzo Hines",
        "Jimi Ireland",
        "Nabil Poole",
        "Onur Holding",
        "Bayley Richmond",
        "Karam Buchanan",
        "Cillian Figueroa"
    };
}
