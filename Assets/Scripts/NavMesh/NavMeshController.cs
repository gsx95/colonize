using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshController : MonoBehaviour
{

    public NavMeshSurface surface;
    public static NavMeshController Instance;

    private void Awake()
    {
        Instance = this;
    }

    public static void Bake()
    {
        Instance.surface.BuildNavMesh();
    }
}
