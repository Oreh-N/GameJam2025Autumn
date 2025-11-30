using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Wandering : MonoBehaviour
{
	public List<Transform> points = new List<Transform>();
	public bool isMoving;
	public int pointIndex;
	public float speed = 4;


	private void Start()
	{
		StartMoving();
	}

	private void StartMoving()
	{
		pointIndex = 0;
		isMoving = true;
	}

	private void Update()
	{
		if (!isMoving) return;
		transform.position = Vector3.MoveTowards(transform.position, points[pointIndex].position, Time.deltaTime*speed);
		var dist = Vector3.Distance(transform.position, points[pointIndex].position);
		if (dist < 0.05f) 
		{ 
			pointIndex = (pointIndex+1) % points.Count;
		}
	}

}
