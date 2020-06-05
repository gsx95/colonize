using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class FactoryInfo : MonoBehaviour
{
    public Text current;
    public Text employees;
    public Text max;
    public Slider slider;

    private static FactoryInfo Instance;

    private Factory factory = null;
    void Awake() {
        Instance = this;
    }
    void Start() {
        gameObject.SetActive(false);
    }

    public static void display(Factory factory) {
        Instance.factory = factory;
        Instance.gameObject.SetActive(true);
    }

    void Update() {
        Instance.current.text = factory.getCurrent().ToString();
        Instance.employees.text = factory.getEmployees().ToString();
        Instance.max.text = factory.getMax().ToString();
        Instance.slider.value = factory.getValue();
    }

    public static void hide() {
        Instance.gameObject.SetActive(false);
    }
}
