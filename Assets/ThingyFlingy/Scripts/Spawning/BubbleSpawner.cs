using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSpawner : MeshSpawner
{
	[Header("-- Bubble Spawning GameObjects and Rates --")]
    [SerializeField, Tooltip("The empty bubble to spawn on the mesh.")] private GameObject emptyBubble;
    [SerializeField, Tooltip("The time between the spawning of the Empty Bubble GameObject")] private float timeBetweenEmptyBubbles;
    [SerializeField, Tooltip("The Spawning bubble to spawn on the mesh.")] private GameObject spawningBubble;
    [SerializeField, Tooltip("The time between the spawning of the Spawning Bubble GameObject")] private float timeBetweenSpawningBubbles;
    
    [Header("-- Bubble Spawning Restrictions --")]
    [SerializeField, Tooltip("The tag to check for before spawning the spawning bubble.")] private string objectTagToCheck;
    [SerializeField, Tooltip("The radius around this GameObject to check for objects with the tag.")] private float distanceTorCheckForNearbyObjects;
    [SerializeField, Tooltip("If there are more gameobjects within the distance to check for nearby objects with the object tag to check than max nearby objects with tag, spawning bubbles will not spawn")] private int maxNearbyObjectsWithTag = 1200;
    
    /// <summary>
    /// returns whether the spawning bubbles can be spawned
    /// </summary>
    private bool SpawnCondition
    {
	    get
	    {
		    //get all colliders within distanceTorCheckForNearbyObjects
		    Collider[] hitColliders = Physics.OverlapSphere(transform.position, distanceTorCheckForNearbyObjects);
		    int numberOfObjectsWithTagToCheck = 0;
			foreach (var hitCollider in hitColliders)
		    {
			    // if the collider's tag matches objectTagToCheck, add one to the number of nearby objects with tag.
			    if(hitCollider.CompareTag(objectTagToCheck))
			    {
				    numberOfObjectsWithTagToCheck++;
				    //if the number of objects withTagTocheck is greater than the maxnearby objects with tag, return false.
				    if(numberOfObjectsWithTagToCheck > maxNearbyObjectsWithTag)
				    {
					    return false;
				    }
			    }
			}
			return true;
	    }
    }
    
    /// <summary>
    /// Start spawning bubbles but only if SpawnCondition is true
    /// </summary>
	private IEnumerator StartConditionalSpawning(GameObject _gameObject, float _timeBetweenSpawns) 
	{
		while(true) 
		{
			if(SpawnCondition)
			{
				InstantiateObjectAtRandomPointOnMesh(_gameObject);
			}
			yield return new WaitForSeconds(_timeBetweenSpawns);
		}
    }

	private void Start()
	{
		StartCoroutine(StartConditionalSpawning(spawningBubble, timeBetweenSpawningBubbles));
		StartCoroutine(StartSpawning(emptyBubble, timeBetweenEmptyBubbles));
	}
}
