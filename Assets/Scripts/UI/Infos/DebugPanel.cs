using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class DebugPanel : MonoBehaviour
{

    public List<Text> texts = new List<Text>();

    private Dictionary<DebugValue, Text> debugValues = new Dictionary<DebugValue, Text>();

    private static DebugPanel Instance;

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        foreach(Text text in texts)
        {
            text.text = "";
        }
    }

    void Update()
    {
        foreach(DebugValue debugValue in debugValues.Keys)
        {
            string valName = debugValue.valueName;
            string val = debugValue.GetValue().ToString();
            debugValues[debugValue].text = valName + ": " + val;
        }
    }

    public static void AddDebug(Func<object> getValue, string valueName)
    {
        var text = Instance.texts[0];
        Instance.texts.Remove(text);
        Instance.debugValues.Add(new DebugValue(getValue, valueName), text);
    }

    private class DebugValue
    {
        public Func<object> GetValue;
        public string valueName;

        public DebugValue(Func<object> getValue, string valueName)
        {
            GetValue = getValue;
            this.valueName = valueName;
        }
    }
    
}
