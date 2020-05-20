using System.Collections.Generic;
using UnityEngine;

public class BuildingPlaceHolder : MonoBehaviour
{
    public State state = State.NONE;
    public GameObject building;
    public List<ResAmount> costs = new List<ResAmount>();
    private int triesToCollide = 3;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(state == State.NONE)
        {
            triesToCollide--;
            if(triesToCollide == 0)
            {
                state = State.PLACING;
            }
        }
    }

    public void Place()
    {
        foreach(ResAmount cost in costs)
        {
            ResourceHolder.Consume(cost.GetResType(), cost.GetAmount());
        }
        Instantiate(building, gameObject.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }


    public void SetGreen()
    {
        var matGreen = Resources.Load("Material/Placable", typeof(Material)) as Material;
        gameObject.GetComponent<MeshRenderer>().material = matGreen;
    }
    public void SetRed()
    {
        var matRed = Resources.Load("Material/NotPlacable", typeof(Material)) as Material;
        gameObject.GetComponent<MeshRenderer>().material = matRed;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Building")
        {
            state = State.PLACING_NOT_POSSIBLE;
        }
    }

    public enum State
    {
        PLACING,
        PLACING_NOT_POSSIBLE,
        NONE,
    }

}
