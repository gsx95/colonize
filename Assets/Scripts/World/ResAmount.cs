using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ResAmount
{
    [SerializeField]
    private ResourceHolder.ResType resType;
    [SerializeField]
    private float amount;

    public ResAmount(ResourceHolder.ResType resType, float amount)
    {
        this.resType = resType;
        this.amount = amount;
    }

    public ResourceHolder.ResType GetResType()
    {
        return resType;
    }

    public float GetAmount()
    {
        return amount;
    }

}
