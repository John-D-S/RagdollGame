using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public abstract class MeshSpawner : MonoBehaviour
{
	[SerializeField] private MeshFilter spawningMeshFilter;
	[SerializeField] private Vector3 spawningOffset;
	private Mesh Mesh => spawningMeshFilter.mesh;

	/// <summary>
	/// Returns a random position in 3d space in the triangle defined by the three given points
	/// </summary>
	/// <param name="_points">The bounds of the triangle. Only the first 3 points will be used.</param>
	private Vector3 GetRandomPositionWithinTriangle(List<Vector3> _points)
	{
		float randomPointA = Random.Range(0f, 1f);
		float randomPointB = Random.Range(0f, 1f);
		//the below algorithm is sourced from https://math.stackexchange.com/questions/538458/how-to-sample-points-on-a-triangle-surface-in-3d
		//Vector3 returnValue = _point1 + randomPointA * (_point2 - _point1) + randomPointB * (_point3 - _point1);
		Vector3 returnValue = _points[0] + randomPointA * (_points[1] - _points[0]) + randomPointB * (_points[2] - _points[0]);
		return returnValue;
	}
	
	/// <summary>
	/// will return a random position on the surface of the given mesh.
	/// This works better when the triangles on the mesh are regularly sized since all triangles are weighted evenly.
	/// </summary>
	protected Vector3 GetRandomPosOnMesh(Mesh _mesh)
	{
		List<Vector3> TrianglePoints(int triangleIndex)
		{
			int firstVertexIndex = triangleIndex * 3;
			List<int> triVertexIndices = new List<int>();
			for(int i = 0; i < 3; i++)
			{
				triVertexIndices.Add(_mesh.triangles[firstVertexIndex + i]);
			}
			List<Vector3> returnValue = new List<Vector3>();
			foreach(int vertexIndex in triVertexIndices)
			{
				returnValue.Add(Mesh.vertices[vertexIndex]);
			}
			return returnValue;
		}
		
		int numberOfTriangles = Mathf.RoundToInt(Mesh.triangles.Length + 1) / 3;
		int randomTriangle = Random.Range(0, numberOfTriangles);
		List<Vector3> randomTriangleVertices = TrianglePoints(randomTriangle);
		return GetRandomPositionWithinTriangle(randomTriangleVertices);
	}

	/// <summary>
	/// Instantiates the given object on a random postition on the mesh.
	/// </summary>
	protected void InstantiateObjectAtRandomPointOnMesh(GameObject _gameObject)
	{
		Vector3 positionToSpawnObject = GetRandomPosOnMesh(Mesh) + transform.position + spawningOffset;
		Instantiate(_gameObject, positionToSpawnObject, Quaternion.identity);
	}

	/// <summary>
	/// Starts periodically spawning _gameObject at random positions on the mesh.
	/// </summary>
	/// <param name="_gameObject">The GameObject to spawn</param>
	/// <param name="_timeBetweenSpawns">The period of time between spawns</param>
	protected IEnumerator StartSpawning(GameObject _gameObject, float _timeBetweenSpawns)
	{
		while(true)
		{
			InstantiateObjectAtRandomPointOnMesh(_gameObject);
			yield return new WaitForSeconds(_timeBetweenSpawns);
		}
	}
}
