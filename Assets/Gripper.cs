using UnityEngine;
using System.Collections;

public class Gripper : MonoBehaviour
{
	public bool state;
	public Vector3 rotationOut, rotationIn;
	
	public Rigidbody holding;
	public HoldObject payload;
	
	Quaternion holdRotation = Quaternion.identity;
	
	public void Drop()
	{
		payload.Drop(holding);
		
		holding = null;
	}
	
	public void MoveIn()
	{
		if (state)
		{
			holding = payload.Get();
			
			if (holding)
				holdRotation = Quaternion.Inverse(payload.transform.rotation) * holding.transform.rotation;
		}
		
		state = false;
	}
	
	public void MoveOut()
	{
		state = true;
		
		this.transform.localEulerAngles = rotationOut;
		
		if (holding)
			holding.transform.position = payload.transform.position;
		
		holding = null; 
	}
	
	void Update()
	{
		if (state)
		{
			this.transform.localEulerAngles = rotationOut;
		}
		else
		{
			this.transform.localEulerAngles = rotationIn;
			
			if (holding)
			{
				holding.transform.position = payload.transform.position;
				holding.transform.rotation = payload.transform.rotation * holdRotation;
			}
		}
	}
}
