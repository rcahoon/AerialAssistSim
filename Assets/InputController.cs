using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour
{
	public string playerPrefix;
	public RobotController robot;
	public float powerChangeSpeed;
	public bool tankDrive;
	
	public Rect guiRect;
	
	void OnGUI()
	{
		GUILayout.BeginArea(guiRect);
		
		tankDrive = GUILayout.Toggle(tankDrive, "Arcade/Tank Drive");
		
		GUILayout.EndArea();
	}

	// Update is called once per frame
	void Update ()
	{
		float leftPower, rightPower;
		if (tankDrive)
		{
			leftPower = Input.GetAxis(playerPrefix + " Left");
			rightPower = Input.GetAxis(playerPrefix + " Right");
		}
		else
		{
			float drive = Input.GetAxis(playerPrefix + " Vertical");
			float steer = Input.GetAxis(playerPrefix + " Horizontal");
			leftPower = Mathf.Clamp(drive + steer, -1, 1);
			rightPower = Mathf.Clamp(drive - steer, -1, 1);
		}
		
		robot.SetMotors(leftPower, rightPower);
		
		float gripper = Input.GetAxis (playerPrefix + " Gripper");
		if (gripper > 0)
			robot.SetGripper(true);
		else if (gripper < 0)
			robot.SetGripper(false);
		
		//TODO: robot.ShootPower = Mathf.Clamp01(robot.ShootPower + Input.GetAxis (playerPrefix + " Shoot Power") * powerChangeSpeed * Time.deltaTime);
		
		if (Input.GetButton(playerPrefix + " Launch"))
			robot.Launch();
	}
}
