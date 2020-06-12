using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{

    public GameObject arrowHome;
    public GameObject arrowWork;

    private static UIController Instance;

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        arrowHome.SetActive(false);
        arrowWork.SetActive(false);
    }

    void Update()
    {
        
    }

    public static void ShowCitizenArrows(Vector3 homePos, Vector3 workPos)
    {
        Instance.ShowArrowsShortly(homePos, workPos);
    }

    private void ShowArrowsShortly(Vector3 homePos, Vector3 workPos)
    {
        StartCoroutine(ShowArrows(homePos, workPos));
    }

    private IEnumerator ShowArrows(Vector3 homePos, Vector3 workPos)
    {
        if (homePos.x != 0 || homePos.y != 0 || homePos.z != 0)
        {
            arrowHome.transform.position = homePos;
            arrowHome.SetActive(true);
        }

        if (workPos.x != 0 || workPos.y != 0 || workPos.z != 0)
        {
            arrowWork.transform.position = workPos;
            arrowWork.SetActive(true);
        }
        yield return new WaitForSeconds(0.5f);
        arrowHome.SetActive(false);
        arrowWork.SetActive(false);
    }
}