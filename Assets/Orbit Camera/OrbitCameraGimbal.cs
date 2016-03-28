using UnityEngine;
using System.Collections;
using System.Text;

public class OrbitCameraGimbal : MonoBehaviour
{
	public float YawSpeed = 7.0f;
	public float PitchSpeed = 7.0f;

	public bool DebugLoggingEnabled = false;

	public void Start()
	{
	}

	public void Update()
	{
		if (Input.GetMouseButton(0))
		{
			Vector3 eulerAngles = transform.localRotation.eulerAngles;

			if (DebugLoggingEnabled)
			{
				Debug.LogFormat("Old: Yaw {0:F2}, Pitch {1:F2}", eulerAngles.y, eulerAngles.x);
			}

			// Yaw
			eulerAngles.y += (Input.GetAxis("Mouse X") * YawSpeed);

			// Pitch
			{
				eulerAngles.x -= (Input.GetAxis("Mouse Y") * PitchSpeed);

				if (eulerAngles.x < 0)
				{
					eulerAngles.x += 360;
				}

				if (eulerAngles.x < 180)
				{
					eulerAngles.x = Mathf.Min(eulerAngles.x, 89.9f);
				}
				else
				{
					eulerAngles.x = Mathf.Max(eulerAngles.x, 270.1f);
				}
			}
			
			if (DebugLoggingEnabled)
			{
				Debug.LogFormat("New: Yaw {0:F2}, Pitch {1:F2}", eulerAngles.y, eulerAngles.x);
			}

			transform.localRotation = Quaternion.Euler(eulerAngles);
		}
	}
}

