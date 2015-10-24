using UnityEngine;
using System.Collections;

public class DriverCamera : MonoBehaviour
{
	public float xSpeed = 120.0f;
	public float ySpeed = 120.0f;
	
	public float yMinLimit = -20f;
	public float yMaxLimit = 80f;
	
	public float x = 0.0f;
	public float y = 0.0f;
	
	bool hasMouse = false;
	
	void Start()
	{
		Vector3 angles = transform.eulerAngles;
		x = angles.y;
		y = angles.x;
	}
	
	void LateUpdate()
	{
		if (Input.GetMouseButtonDown(0) && GetComponent<Camera>().pixelRect.Contains(Input.mousePosition))
			hasMouse = true;
		if (!Input.GetMouseButton(0))
			hasMouse = false;
		
		if (hasMouse)
		{
			x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
			y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
		}
		
		y = ClampAngle(y, yMinLimit, yMaxLimit);
		
		transform.rotation = Quaternion.Euler(y, x, 0);
	}
	
	static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360F)
			angle += 360F;
		if (angle > 360F)
			angle -= 360F;
		return Mathf.Clamp(angle, min, max);
	}
}