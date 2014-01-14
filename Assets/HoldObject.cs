using UnityEngine;
using System.Collections.Generic;

public class HoldObject : MonoBehaviour {

	HashSet<Rigidbody> contained = new HashSet<Rigidbody>();
	
	public Rigidbody Get()
	{
		Rigidbody holding = null;
		float bestDist = float.MaxValue;
		foreach (var c in contained)
		{
			float dist = Vector3.Distance(c.position, this.transform.position);
			if (dist < bestDist)
			{
				holding = c;
				bestDist = dist;
			}
		}
		return holding;
	}
	
	public void Drop(Rigidbody obj)
	{
		contained.Remove (obj);
	}
	
	void FixedUpdate()
	{
		foreach(var c in contained)
		{
			c.velocity = Vector3.zero;
			c.angularVelocity = Vector3.zero;
		}
	}

	void OnTriggerEnter(Collider c)
	{
		if (c.tag == "Ball")
		{
			contained.Add(c.attachedRigidbody);
		}
	}
	void OnTriggerExit(Collider c)
	{
		contained.Remove(c.attachedRigidbody);
	}
}
