using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class FollowCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 positionOffset, lookOffset;
    public float distance, distanceMin, distanceMax;
    
    float currentDistance;

    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;

    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    public float x = 0.0f;
    public float y = 0.0f;
    
    public float margin = 0.2f;
    
    bool hasMouse = false;

    // Use this for initialization
    void Start()
    {
    	currentDistance = distance;
    }
    
    void LateUpdate()
    {
		if (!Input.GetMouseButton(0))
			hasMouse = false;
		if (GetComponent<Camera>().pixelRect.Contains(Input.mousePosition))
		{
			if (Input.GetMouseButtonDown(0))
				hasMouse = true;
			
			distance = Mathf.Clamp(distance * Mathf.Pow (0.8f, Input.GetAxis("Mouse ScrollWheel")), distanceMin, distanceMax);
		}
		
		if (hasMouse)
		{
			x += Input.GetAxis("Mouse X") * xSpeed * currentDistance * 0.02f;
			y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
		}
		
		y = ClampAngle(y, yMinLimit, yMaxLimit);
		
		Quaternion rotation = Quaternion.Euler(y, x, 0);
		
		
		currentDistance = distance;
		
		
		Vector3 targetPos = target.TransformPoint(lookOffset);
		
		Vector3 negDistance = positionOffset * currentDistance;
		Vector3 position = target.TransformPoint(rotation * negDistance);
		
//		float tDist = distance;
//		
//		RaycastHit hit;
//		//if (Physics.SphereCast(targetPos, margin, position - targetPos, out hit, (position - targetPos).magnitude - margin))
//		if (Physics.Linecast(targetPos, position, out hit, ~(1 << 9)))
//		{
//			if (hit.transform.tag != "Ball" && !hit.transform.IsChildOf(target))
//			{
//				float hitDist = Vector3.Project(hit.point - targetPos, position - targetPos).magnitude;
//				
//				tDist = hitDist + margin - 0.01f;
//			}
//		}
//		if (tDist > distance)
//			tDist = distance;
//		
//		currentDistance = Mathf.Lerp(currentDistance, tDist, 10 * Time.deltaTime);
//		
//		negDistance = positionOffset * currentDistance;
//		position = target.TransformPoint(rotation * negDistance);
		
		transform.position = position;
		transform.LookAt(targetPos);
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