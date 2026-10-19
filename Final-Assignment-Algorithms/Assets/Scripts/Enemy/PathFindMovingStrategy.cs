using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PathFindMovingStrategy : AbstractEnemyMovingStrategy
{
    private TileGraphGenerator graphGenerator;
    private AbstractPathFinderAlgorithm algorithm;

    private Vector3 startNode;
    private Vector3 endNode;

    public List<Vector3> path = new List<Vector3>();
    private Graph<Vector3> graph;

    private bool isMoving = false;
    private bool playerFound = false;

    private Vector2 moveVector = Vector2.zero;

    [SerializeField]
    private float posUpdateFrequency = 0.5f;

    Coroutine lastDestination = null;

    void Start()
    {
        graphGenerator = GameObject.FindGameObjectWithTag("DungeonGenerator").GetComponent<TileGraphGenerator>();
        graph = graphGenerator.GetGraph();
        algorithm = gameObject.GetComponent<AbstractPathFinderAlgorithm>();
        if(algorithm == null)
        {
            throw new System.Exception("Enemy does not have a pathfinding algorithm");
        }
    }


    public override void PlayerFound(Transform pPlayer)
    {
        playerFound = true;
        //GoToDestination(playerPos.position);
       StartCoroutine(UpdatePosition(pPlayer));
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveVector * enemy.Speed;
    }

    private IEnumerator UpdatePosition(Transform player)
    {
        while (playerFound)
        {
            GoToDestination(player.position);
            yield return new WaitForSeconds(posUpdateFrequency);
        }
    }
    public override void PlayerLost()
    {
        StopAllCoroutines();
        moveVector = Vector3.zero;
        onMove?.Invoke(moveVector);
        playerFound= false;
        isMoving = false;
    }
    private void GoToDestination(Vector3 destination)
    {
        if (isMoving && lastDestination != null)
        {
            //StopAllCoroutines();
            StopCoroutine(lastDestination);
            isMoving = false;
        }
            
       lastDestination = StartCoroutine(FollowPathCoroutine(CalculatePath(transform.position, destination)));
    }
    private Vector3 GetClosestNodeToPosition(Vector3 position)
    {
        graph = graphGenerator.GetGraph();
        Vector3 closestNode = Vector3.zero;
        float closestDistance = Mathf.Infinity;

        foreach (var node in graph.ReturnNodesList())
        {
            if (Vector3.Distance(node, position) < closestDistance)
            {
                closestDistance = Vector3.Distance(node, position);
                closestNode = node;
            }
        }

        return closestNode;
    }

    public List<Vector3> CalculatePath(Vector3 from, Vector3 to)
    {
        Vector3 playerPosition = from;

        startNode = GetClosestNodeToPosition(playerPosition);
        endNode = GetClosestNodeToPosition(to);

        List<Vector3> shortestPath = new List<Vector3>();

        shortestPath = algorithm.FindPath(startNode, endNode, graph);

        path = shortestPath; //Used for drawing the path

        return shortestPath;
    }

    IEnumerator FollowPathCoroutine(List<Vector3> path)
    {
        if (path == null || path.Count == 0)
        {
            Debug.Log("No path found");
            yield break;
        }
        isMoving = true;
        for (int i = 0; i < path.Count; i++)
        {
            Vector3 target = path[i];
            // Move towards the target position
            while (Vector3.Distance(transform.position, target) > 0.1f)
            {
                moveVector = (target - transform.position).normalized;
            onMove?.Invoke(moveVector.normalized);
                yield return null;
            }
            //Debug.Log($"Reached target: {target}");
        }
        isMoving = false;
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(startNode, .3f);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(endNode, .3f);

        //if (discovered != null)
        //{
        //    foreach (var node in discovered)
        //    {
        //        Gizmos.color = Color.red;
        //        Gizmos.DrawSphere(node, .3f);
        //    }
        //}

        if (path != null)
        {
            foreach (var node in path)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(node, .3f);
            }
        }


    }
}
