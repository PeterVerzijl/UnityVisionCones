using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Component that forever sets a random target for a NavMeshAgent then waits until the destination has been reached
/// and then continues to find a new random point. 
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class SetRandomTarget : MonoBehaviour
{
    // Start is called before the first frame update
    IEnumerator Start() {
        NavMeshAgent navMeshAgent = GetComponent<NavMeshAgent>();
        
        // Loop forever
        while (true) {
            // Find random reachable point inside radius
            Vector2 randomPointInCircle = Random.insideUnitCircle * 10; 
            Vector3 randomPoint = new Vector3(randomPointInCircle.x, 0, randomPointInCircle.y);
            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 1.0f, NavMesh.AllAreas)) {
                navMeshAgent.SetDestination(hit.position);
                
                // Wait until agent has reached destination
                while (!HasReachedDestination(navMeshAgent)) {
                    yield return null;
                }
            }
        }
    }

    /// <summary>
    /// Returns true when the destination has been reached.
    /// Returns false if the destination is unreachable or the path is invalid or the agent is still in the process
    /// of trying to reach the destination.
    /// </summary>
    /// <param name="navMeshAgent"></param>
    /// <returns></returns>
    private bool HasReachedDestination(NavMeshAgent navMeshAgent) {
        return !navMeshAgent.pathPending &&
               navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance &&
               (!navMeshAgent.hasPath || Mathf.Approximately(navMeshAgent.velocity.sqrMagnitude, 0));
    }
}
