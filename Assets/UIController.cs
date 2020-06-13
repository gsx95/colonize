using System;
using System.Collections;
using UnityEngine;

public class UIController : MonoBehaviour
{

    public GameObject arrowHome;
    public GameObject arrowWork;
    public GameObject arrowCitizen;

    private static UIController Instance;

    private Func<Vector3> homeFunc = null;
    private Func<Vector3> workFunc = null;
    private Func<Vector3> citizenFunc = null;

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        arrowHome.SetActive(false);
        arrowWork.SetActive(false);
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

        if (citizenFunc != null)
        {
            var citizenPos = citizenFunc();
            arrowCitizen.transform.position = Camera.main.WorldToScreenPoint(citizenPos);
            arrowCitizen.SetActive(true);
        }
    }

    public static void ShowCitizenArrows(Func<Vector3> citizenPosFunc, Func<Vector3> homePosFunc, Func<Vector3> workPosFunc)
    {
        Instance.ShowArrowsShortly(citizenPosFunc, homePosFunc, workPosFunc);
    }

    private void ShowArrowsShortly(Func<Vector3> citizenPosFunc, Func<Vector3> homePosFunc, Func<Vector3> workPosFunc)
    {
        StartCoroutine(ShowArrows(citizenPosFunc, homePosFunc, workPosFunc));
    }

    private IEnumerator ShowArrows(Func<Vector3> citizenPosFunc, Func<Vector3> homePosFunc, Func<Vector3> workPosFunc)
    {
        this.homeFunc = homePosFunc;
        this.workFunc = workPosFunc;
        this.citizenFunc = citizenPosFunc;
        yield return new WaitForSeconds(1f);
        this.homeFunc = null;
        this.workFunc = null;
        this.citizenFunc = null;
        arrowHome.SetActive(false);
        arrowWork.SetActive(false);
        arrowCitizen.SetActive(false);
    }
}