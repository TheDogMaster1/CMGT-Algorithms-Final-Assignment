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
    private bool inKnockBack = false;

    private Vector2 moveVector = Vector2.zero;

    [SerializeField]
    private float posUpdateFrequency = 0.5f;
    [SerializeField]
    private float kbForce = 20f;

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


    protected override void PlayerFound(Transform pPlayer)
    {
        playerFound = true;
        //GoToDestination(playerPos.position);
       StartCoroutine(UpdatePosition(pPlayer));
    }
    protected override void PlayerLost()
    {
        StopAllCoroutines();
        moveVector = Vector3.zero;
        onMove?.Invoke(moveVector);
        playerFound= false;
        isMoving = false;
    }

    protected override void OnEnemyHit(Enemy enemy, Transform playerTransform)
    {
        inKnockBack = true;
        rb.linearVelocity = Vector2.zero;
        Vector2 kbVector = (transform.position - playerTransform.position).normalized;
        kbVector.x = Mathf.Round(kbVector.x);
        kbVector.y = Mathf.Round(kbVector.y);
        rb.AddForce(kbVector.normalized * kbForce, ForceMode2D.Impulse);
        StartCoroutine(BeStunned());
    }

    private IEnumerator BeStunned()
    {
        yield return new WaitForSeconds(0.07f);
        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(.5f);
        inKnockBack= false;
    }

    private void FixedUpdate()
    {
        if(!inKnockBack) rb.linearVelocity = moveVector * enemy.Speed;
        //Debug.Log(enemy);
    }

    private IEnumerator UpdatePosition(Transform player)
    {
        while (playerFound)
        {
            GoToDestination(player.position);
            yield return new WaitForSeconds(posUpdateFrequency);
        }
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
        for (int i = 1; i < path.Count; i++)
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
