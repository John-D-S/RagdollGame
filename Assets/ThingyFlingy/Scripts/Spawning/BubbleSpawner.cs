using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSpawner : MeshSpawner
{
    [SerializeField] private GameObject emptyBubble;
    [SerializeField] private float timeBetweenEmptyBubbles;
    [SerializeField] private GameObject spawningBubble;
    [SerializeField] private float timeBetweenSpawningBubbles;
    
    [SerializeField] private string objectTagToCheck;
    [SerializeField] private float distanceTorCheckForNearbyObjects;
    [SerializeField] private float maxNearbyObjectsWithTag;
    
    private bool SpawnCondition
    {
	    get
	    {
		    Collider[] hitColliders = Physics.OverlapSphere(transform.position, distanceTorCheckForNearbyObjects);
		    int numberOfObjectsWithTagToCheck = 0;
			foreach (var hitCollider in hitColliders)
		    {
			    if(hitCollider.CompareTag(objectTagToCheck))
			    {
				    numberOfObjectsWithTagToCheck++;
				    if(numberOfObjectsWithTagToCheck > maxNearbyObjectsWithTag)
				    {
					    return false;
				    }
			    }
			}
			return true;
	    }
    }
    
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
