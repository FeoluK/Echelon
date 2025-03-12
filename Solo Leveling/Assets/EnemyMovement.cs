using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private Transform player;                 // Reference to the player's Transform
    private NavMeshAgent navMeshAgent;        // Reference to the NavMeshAgent component

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        // Dynamically find and assign the player transform
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;  // Assign the transform of the found player object
        }
        else
        {
            Debug.LogError("Player not found! Make sure the player has the 'Player' tag.");
        }
    }

    void Update()
    {
        if (player != null)
        {
            navMeshAgent.SetDestination(player.position);
        }
    }
}
