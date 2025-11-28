using UnityEngine;


[RequireComponent (typeof(Camera))]
public class CameraFollowing : MonoBehaviour
{
	public Transform target;
	public bool smoothnesse = false;

	public float smoothSpeed = 0.125f;
	public Vector3 offset; // Offset from the target

	void LateUpdate()
	{
		if (target != null && smoothnesse)
		{
			Vector3 desiredPosition = target.position + offset;
			Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
			transform.position = smoothedPosition;
			//transform.LookAt(target);
		}
	}
}
