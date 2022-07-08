using System.Collections;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;

public class PathfinderMaster : MonoBehaviour
{
    #region Variables
    public static PathfinderMaster singleton;

    List<Pathfinder> currentJobs = new List<Pathfinder>();
    List<Pathfinder> toDoJobs = new List<Pathfinder>();
    public int MaxJobs = 3;
    public float timerThreshold = 5;
    #endregion

    #region Unity Methods
    //initialises this script as a singleton, ensuring no duplicates of this script
    private void Awake()
    {
        singleton = this;
    }

    private void Update()
    {
        int i = 0;
        float delta = Time.deltaTime;

        //loops through all the jobs and times them. Removes them from the list if they reach the threshold time.
        while (i < currentJobs.Count)
        {
            if (currentJobs[i].jobDone)
            {
                currentJobs[i].NotifyComplete();
                currentJobs.RemoveAt(i);
            }
            else
            {
                currentJobs[i].timer += delta;

                if (currentJobs[i].timer > timerThreshold)
                {
                    currentJobs[i].jobDone = true;
                    Debug.Log("WARNING: Job timer reached threshold.");
                }

                i++;
            }
        }

        //ensures that the amount of jobs cannot surpass the max limit
        if (toDoJobs.Count > 0 && currentJobs.Count < MaxJobs)
        {
            Pathfinder job = toDoJobs[0];
            toDoJobs.RemoveAt(0);
            currentJobs.Add(job);

            Thread jobThread = new Thread(job.FindPath);
            jobThread.Start();
        }
    }
    #endregion

    //sends the pathfinder a request to find a path to the target node
    public void RequestPathFind(GridCharacter character, Node start, Node target, Pathfinder.PathfindingComplete callback, GridManager gridManager)
    {
        Pathfinder newJob = new Pathfinder(character, start, target, callback, gridManager);
        toDoJobs.Add(newJob);
    }
}
