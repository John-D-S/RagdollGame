using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBubble : MonoBehaviour
{
    [SerializeField] private float riseHeight;
    [SerializeField] private float endScale;
    [SerializeField, Tooltip("The time it takes to rise")] private float lerpSpeed;
    [SerializeField] private float lifeTime;
    [SerializeField] private List<GameObject> spawnedGameObjects;

    private Vector3 endPosition;
    private void Start()
    {
        endPosition = transform.position + Vector3.up * riseHeight;
        StartCoroutine(StartLifetime());
    }

    private IEnumerator StartLifetime()
    {
        yield return new WaitForSeconds(lifeTime);
        foreach(GameObject gOToGame in spawnedGameObjects)
            Instantiate(gOToGame, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    
    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, endPosition, lerpSpeed);
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * endScale, lerpSpeed); 
    }
}
