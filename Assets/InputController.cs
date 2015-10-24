using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour
{
	public int playerNumber;
	public RobotController robot;
	public float powerChangeSpeed;
	public bool tankDrive;
	
	public Rect guiRect;
	
	void OnGUI()
	{
    Rect r = guiRect;
    if (r.x < 0) r.x += Screen.width;
    if (r.y < 0) r.y += Screen.height;
    
		GUILayout.BeginArea(r);
		
		tankDrive = GUILayout.Toggle(tankDrive, "Arcade/Tank Drive");
		
		GUILayout.EndArea();
	}

	// Update is called once per frame
	void Update ()
	{
		float leftPower, rightPower;
		if (tankDrive)
		{
			leftPower = Input.GetAxis("P" + playerNumber + " Left");
			rightPower = Input.GetAxis("P" + playerNumber + " Right");
		}
		else
		{
			float drive = Input.GetAxis("P" + playerNumber + " Vertical");
			float steer = Input.GetAxis("P" + playerNumber + " Horizontal");
			leftPower = Mathf.Clamp(drive + steer, -1, 1);
			rightPower = Mathf.Clamp(drive - steer, -1, 1);
		}
		
		robot.SetMotors(leftPower, rightPower);
		
		float gripper = Input.GetAxis ("P" + playerNumber + " Gripper");
		if (gripper > 0)
			robot.SetGripper(true);
		else if (gripper < 0)
			robot.SetGripper(false);
		
		//TODO: robot.ShootPower = Mathf.Clamp01(robot.ShootPower + Input.GetAxis ("P" + playerNumber + " Shoot Power") * powerChangeSpeed * Time.deltaTime);
		
		if (Input.GetButton("P" + playerNumber + " Launch"))
			robot.Launch();
	}
}
