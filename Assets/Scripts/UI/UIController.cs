using System;
using System.Collections;
using UnityEngine;

public class UIController : MonoBehaviour
{

    public GameObject arrowHome;
    public GameObject arrowWork;
    public GameObject arrowWork2;
    public GameObject arrowCitizen;

    private static UIController Instance;

    private Func<Vector3> homeFunc = null;
    private Func<Vector3> workFunc = null;
    private Func<Vector3> secondWorkPosFunc = null;
    private Func<Vector3> citizenFunc = null;
    
    

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        arrowHome.SetActive(false);
        arrowWork.SetActive(false);
        arrowWork2.SetActive(false);
        arrowCitizen.SetActive(false);
    }

    void Update()
    {
        if (homeFunc != null)
        {
            var homePos = homeFunc();
            arrowHome.transform.position = Camera.main.WorldToScreenPoint(homePos);
            arrowHome.SetActive(true);
        }

        if (workFunc != null)
        {
            var workPos = workFunc();
            arrowWork.transform.position = Camera.main.WorldToScreenPoint(workPos);
            arrowWork.SetActive(true);
        }

        if (secondWorkPosFunc != null)
        {
            var workPos = secondWorkPosFunc();
            arrowWork2.transform.position = Camera.main.WorldToScreenPoint(workPos);
            arrowWork2.SetActive(true);
        }

        if (citizenFunc != null)
        {
            var citizenPos = citizenFunc();
            arrowCitizen.transform.position = Camera.main.WorldToScreenPoint(citizenPos);
            arrowCitizen.SetActive(true);
        }
    }

    public static void HideArrows()
    {
        Instance.HideAllArrows();
    }

    private void HideAllArrows()
    {
        this.homeFunc = null;
        this.workFunc = null;
        this.secondWorkPosFunc = null;
        this.citizenFunc = null;
        arrowHome.SetActive(false);
        arrowWork.SetActive(false);
        arrowWork2.SetActive(false);
        arrowCitizen.SetActive(false);
    }

    public static void ShowCitizenArrows(Func<Vector3> citizenPosFunc, Func<Vector3> homePosFunc, Func<Vector3> workPosFunc, Func<Vector3> secondWorkPosFunc)
    {
        Instance.ShowArrows(citizenPosFunc, homePosFunc, workPosFunc, secondWorkPosFunc);
    }

    private void ShowArrows(Func<Vector3> citizenPosFunc, Func<Vector3> homePosFunc, Func<Vector3> workPosFunc, Func<Vector3> secondWorkPosFunc)
    {
        this.homeFunc = homePosFunc;
        this.workFunc = workPosFunc;
        this.citizenFunc = citizenPosFunc;
        this.secondWorkPosFunc = secondWorkPosFunc;

    }
}