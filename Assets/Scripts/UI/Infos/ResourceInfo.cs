using UnityEngine;
using UnityEngine.UI;

public class ResourceInfo : MonoBehaviour {

    public ResourceHolder.ResType resType;
    public string resName;

    private Text text;

    void Start() {
        text = gameObject.GetComponent<Text>();
    }

    void Update() {
        text.text = resName + ": " + ResourceHolder.GetRes(resType);
    }
}
