using UnityEngine;
using System.Collections;
using System.Text;

public class PlaneSweeper : MonoBehaviour
{
	public Vector3 PositionalSpeed = Vector3.back;
	public Vector3 EulerRotationalSpeed = Vector3.zero;

	public void Start()
	{
	}

	public void Update()
	{
		transform.localRotation = 
			Quaternion.LerpUnclamped(
				transform.localRotation,
				(Quaternion.Euler(EulerRotationalSpeed) * transform.localRotation),
				Time.deltaTime);

		transform.localPosition += (
			transform.localRotation *
			(PositionalSpeed * Time.deltaTime));
	}
}

