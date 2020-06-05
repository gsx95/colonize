using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericClose : MonoBehaviour
{
    public GameObject toHide;

    public void Click() {
        toHide.SetActive(false);
    }
}
