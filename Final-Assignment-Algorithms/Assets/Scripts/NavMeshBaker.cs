using NaughtyAttributes;
using Unity.AI.Navigation;
using UnityEngine;

public class NavMeshBaker : MonoBehaviour
{
    public NavMeshSurface navMeshSurface;

    [Button(enabledMode: EButtonEnableMode.Playmode)]
    private void BakeNavMesh()
    {
        navMeshSurface.BuildNavMesh();
    }
}
