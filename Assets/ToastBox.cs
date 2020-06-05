using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToastBox : MonoBehaviour
{
    public GameObject prefab;

    private static ToastBox Instance;

    void Awake() {
        Instance = this;
    }
    public static void ShowMsg(string msg) {
        Instance.ShowShortMsg(msg);
    }

    private void ShowShortMsg(string msg) {
        StartCoroutine(ShowMessage(msg));
    }
    private IEnumerator ShowMessage(string msg) {
        var entry = Instantiate(prefab);
        entry.transform.SetParent(transform, false);
        entry.GetComponent<Toast>().setText(msg);
        yield return new WaitForSeconds(3);
        Destroy(entry);
    }
}
