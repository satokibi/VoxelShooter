using UnityEngine;
using System.Collections;

public class TopViewCamera : MonoBehaviour {

	public GameObject FollowObject = null; //カメラが追従するオブジェクト
	public float FollowSmooth = 3.5f;
	public float defaultDistance = 80f;
	public float defaultAngularPosition = 35f;
	public float MaxAngularPosition = 89.9f;

	protected Vector2 defaultCameraPos;
	protected Vector3 orbitPos;
	protected RaycastHit hit;
	protected float distance = 0f;
	protected float LookOverPosition;
	protected int layermask;

	// Use this for initialization
	void Start()
	{
		distance = defaultDistance;
		defaultCameraPos = new Vector2(0f, defaultAngularPosition / 180f * Mathf.PI);
		layermask = ~(1 << LayerMask.NameToLayer ("Player")); 
	}
	// Update is called once per frame
	void Update()
	{
		
		//カメラをプレイヤーに追従させる
		orbitPos = FollowObject.transform.position + GetOrbitPosition (defaultCameraPos, distance); 
		transform.position = orbitPos;

		//遮蔽物に隠れる場合、キャラが見えるようにカメラを動かす
		if (Physics.Raycast (FollowObject.transform.position, transform.position - FollowObject.transform.position, 
			    out hit, distance, layermask)) {
			orbitPos = GetOrbitPosition (defaultCameraPos, distance) + FollowObject.transform.position + Vector3.back;
			for (LookOverPosition = defaultAngularPosition; LookOverPosition <= MaxAngularPosition; LookOverPosition += 1.0f) {
				if (Physics.Raycast (FollowObject.transform.position, orbitPos - FollowObject.transform.position, 
					    out hit, distance, layermask)) {
					orbitPos = GetOrbitPosition (new Vector2 (0f, LookOverPosition / 180 * Mathf.PI), distance) + FollowObject.transform.position;
				} else
					break;
			}
			if (LookOverPosition > MaxAngularPosition) {
				orbitPos = GetOrbitPosition (new Vector2 (0f, defaultAngularPosition / 180 * Mathf.PI), distance) + FollowObject.transform.position;
			}
		} 
		Camera.main.transform.position = Vector3.Lerp (Camera.main.transform.position, orbitPos, Time.deltaTime * FollowSmooth);
		Camera.main.transform.LookAt (FollowObject.transform);
	}

	private Vector3 GetOrbitPosition(Vector2 anglarParam, float distance)
	{
		float x = Mathf.Sin(anglarParam.x) * Mathf.Cos(anglarParam.y);
		float z = Mathf.Cos(anglarParam.x) * Mathf.Cos(anglarParam.y);
		float y = Mathf.Sin(anglarParam.y);

		return new Vector3 (x, y, z) * distance;
	}
}
