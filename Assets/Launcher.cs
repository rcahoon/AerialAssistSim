using UnityEngine;
using System.Collections;

public class Launcher : MonoBehaviour
{
	public float ShootPower;
	
	public Gripper gripper;
	
	public float maxForce;
	
	public void Launch()
	{
		if (gripper.holding)
		{
			gripper.holding.AddForce(this.transform.forward * Mathf.Clamp01(ShootPower) * maxForce, ForceMode.Impulse);
			
			gripper.Drop();
		}
	}
}
