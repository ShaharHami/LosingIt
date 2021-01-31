using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObstacleManager : MonoBehaviour
{
    public float checkInterval;
    public float obstacleProbability;
    public int maxObstacles;
    public List<Obstacle> obstacles;
    public float minDistance;
    public Player player;
    [HideInInspector] public bool canInteract = true;
    [HideInInspector] public bool gameOver;

    private Obstacle currentCoolDown;

    private void Start()
    {
        StartCoroutine(DelayedStart());
        canInteract = true;
    }

    private IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(15);
        StartCoroutine(ObstacleLoop());
    }

    private IEnumerator ObstacleLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(checkInterval);

            foreach (var obstacle in GetObstaclesByState(Obstacle.State.Idle))
            {
                if (GetObstaclesByNotState(Obstacle.State.Idle).Count < maxObstacles && Random.Range(0f, 1f) < obstacleProbability)
                {
                    obstacle.Annoyed();
                    break;
                }
            }
        }
    }

    private List<Obstacle> GetObstaclesByState(Obstacle.State state)
    {
        return obstacles.Where(obstacle => obstacle.state == state).ToList();
    }
    
    private List<Obstacle> GetObstaclesByNotState(Obstacle.State state)
    {
        return obstacles.Where(obstacle => obstacle.state != state).ToList();
    }

    public List<Obstacle> GetActiveObstacles()
    {
        return obstacles.Where(obstacle => obstacle.state == Obstacle.State.Annoying || obstacle.state == Obstacle.State.Calming).ToList();
    }

    private void Update()
    {
        if (gameOver) return;
        if (canInteract && Input.GetKey(KeyCode.E))
        {
            if (currentCoolDown != null && currentCoolDown.calmDownMeter > 0)
            {
                currentCoolDown.ShowPrompt();
                currentCoolDown.CalmDown();
            }
            else if (GetActiveObstacles().Count > 0)
            {
                currentCoolDown = GetClosestObstacle(GetActiveObstacles());
            }
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            currentCoolDown = null;
            canInteract = true;
        }
        else
        {
            var closestObstacle = GetClosestObstacle(GetActiveObstacles());
            if (closestObstacle != null)
            {
                closestObstacle.ShowPrompt();
            }
        }
        player.ErodeSanity(GetActiveObstacles());
    }

    private Obstacle GetClosestObstacle(List<Obstacle> filteredObstacles)
    {
        Obstacle closest = null;
        foreach (var obstacle in filteredObstacles)
        {
            var distance = Vector2.Distance(obstacle.transform.position, player.transform.position);
            if (distance < minDistance)
            {
                if (closest == null || distance < Vector2.Distance(closest.transform.position, player.transform.position))
                {
                    closest = obstacle;
                }
            }
            obstacle.HidePrompt();
        }
        
        return closest;
    }
}