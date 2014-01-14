using UnityEngine;
using System.Collections;

public class RobotController : MonoBehaviour {
	public Wheel[] leftWheels;
	public Wheel[] rightWheels;
	public float motorScaler;
	
	public Gripper gripper;
	public Launcher launcher;
	
	public void SetMotors(float left, float right)
	{
		foreach(var h in leftWheels)
		{
			h.RunJoint(motorScaler * left);
		}
		foreach(var h in rightWheels)
		{
			h.RunJoint(motorScaler * right);
		}
	}
	
	public void SetGripper(bool state)
	{
		if (state)
			gripper.MoveOut();
		else
			gripper.MoveIn();
	}
	
	public float ShootPower
	{
		get
		{
			return launcher.ShootPower;
		}
		set
		{
			launcher.ShootPower = value;
		}
	}
	
	public void Launch()
	{
		launcher.Launch();
	}
}
