using UnityEngine;


[RequireComponent (typeof(Camera))]
public class CameraFollowing : MonoBehaviour
{
	public Transform target;
	public bool smoothnesse = false;

	public float smoothSpeed = 0.2f;
	public Vector3 offset; // Offset from the target

	private void Update()
	{
		if (target != null && !smoothnesse)
		{
			transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
		}
	}

	void LateUpdate()
	{
		if (target != null && smoothnesse)
		{
			Vector3 desiredPosition = target.position + offset;
			desiredPosition.z = transform.position.z;
			Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
			transform.position = smoothedPosition;
			//transform.LookAt(target);
		}
	}
}
