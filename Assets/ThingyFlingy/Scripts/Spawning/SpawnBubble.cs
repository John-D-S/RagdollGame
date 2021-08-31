using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBubble : MonoBehaviour
{
    [Header("-- Bubble Settings --")]
    [SerializeField, Tooltip("How high the bubble rises after spawning.")] private float riseHeight;
    [SerializeField, Tooltip("How large the bubble will be after lifetime.")] private float endScale;
    [SerializeField, Tooltip("The time it takes to rise.")] private float lerpSpeed;
    [SerializeField, Tooltip("How long after the bubble spawns to pop the bubble.")] private float lifeTime;
    [SerializeField, Tooltip("The list of things to spawn when the bubble pops.")] private List<GameObject> spawnedGameObjects;

    private Vector3 endPosition;
    private void Start()
    {
        //set the end position
        endPosition = transform.position + Vector3.up * riseHeight;
        //start the count down for the bubble to die
        StartCoroutine(StartLifetime());
    }

    /// <summary>
    /// Waits for the lifeTime of the bubble, then spawns all the spawnedGameObjects, then destroys the bubble
    /// </summary>
    private IEnumerator StartLifetime()
    {
        yield return new WaitForSeconds(lifeTime);
        foreach(GameObject gOToGame in spawnedGameObjects)
            Instantiate(gOToGame, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    
    private void FixedUpdate()
    {
        // lerp the position and scale with time.
        transform.position = Vector3.Lerp(transform.position, endPosition, lerpSpeed);
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * endScale, lerpSpeed); 
    }
}
