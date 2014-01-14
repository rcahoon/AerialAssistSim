using UnityEngine;
using System.Collections;

[RequireComponent(typeof(HingeJoint))]
public class Wheel : MonoBehaviour
{
	private Vector3 neutralPosition = Vector3.zero;
	
	void Awake ()
	{
		neutralPosition = transform.localPosition;
	}
	
	public void RunJoint (float speed)
	{
		float targetVel = speed;
		
		// set joint motor parameters
		JointMotor myMotor = hingeJoint.motor;
		myMotor.targetVelocity = targetVel;
		hingeJoint.motor = myMotor;
		
		transform.localPosition = neutralPosition;
	}
}
