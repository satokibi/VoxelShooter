using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public GameObject Player;
	//[HideInInspector]
	public bool Pause = false;

	private CharacterMotor motor = null;
	private Animator animator = null;
	private float Speed = 0;
	private Vector3 direction;
	private Vector3 CameraForward;
	private bool AttackCooldown=false;
	private bool ComboCheck = false;
	private int ComboState = 0; 

	// Use this for initialization
	void Start () {
		animator = Player.GetComponent<Animator> ();
		Speed = 0f;
		motor = GetComponent<CharacterMotor>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Pause) {
			if(motor.grounded) motor.inputMoveDirection = Vector3.zero;
			animator.SetFloat ("Speed", 0);
			return;
		}
		//カメラの向きを基準に前後左右を決める
		CameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
		direction = CameraForward * Input.GetAxisRaw("Vertical") +
			Camera.main.transform.right * Input.GetAxisRaw("Horizontal");

		if (AttackCooldown) {
			if (Input.GetButtonDown ("Fire1") && !ComboCheck) {
				ComboState++;
				if (ComboState < 3)
					ComboCheck = true;
			} else {
				motor.inputMoveDirection = Vector3.zero;
				animator.SetFloat ("Speed", 0);
			}
		}
		else{
			//移動
			if (direction != Vector3.zero) {
				//キャラの向きを決定
				transform.rotation = Quaternion.LookRotation (transform.position +
					(Camera.main.transform.right * Input.GetAxisRaw ("Horizontal")) +
					(CameraForward * Input.GetAxisRaw ("Vertical"))
					- transform.position);
			}
			motor.inputMoveDirection = direction;

			//ジャンプ
			motor.inputJump = Input.GetButton ("Jump");

			//歩行アニメーション
			if (motor.grounded) {
				Speed = direction.magnitude;
			}
			else
				Speed = 0;

			animator.SetFloat ("Speed", Speed);


			if (Input.GetButtonDown ("Fire1")) {
				StartCoroutine ("WeponAttack",ComboState);
			}
			if (Input.GetButtonDown ("Fire2")) {
				StartCoroutine ("SkillAttack",0);
			}
		}
	}
	//attack 
	private IEnumerator WeponAttack(int AttackNo)
	{
		AttackCooldown = true;
		switch(AttackNo)
		{
			case 0 : animator.Play ("Attack1", 0, 0);
			break;
			case 1 : animator.Play ("Attack2", 0, 0);
			break;
			case 2 : animator.Play ("Attack3", 0, 0);
			break;
		default :
			ComboCheck = false;
			break;
		} 
		yield return null;
		yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
		if (!ComboCheck) {
			ComboState = 0;
			AttackCooldown = false;
		}
		else {
			ComboCheck = false;
			StartCoroutine ("WeponAttack",ComboState);
			AttackCooldown = true;
		}
	}
	//skill
	private IEnumerator SkillAttack(int AttackNo)
	{
		AttackCooldown = true;
		ComboState = 0;
		ComboCheck = false;
		switch(AttackNo)
		{
		case 0 : animator.Play ("Skill1", 0, 0);
			break;
		} 
		yield return null;
		yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
		AttackCooldown = false;
	}
	//着地処理
	private IEnumerator OnLandWait()
	{
		yield return null;
		AttackCooldown = true;
		yield return new WaitForSeconds (animator.GetCurrentAnimatorStateInfo(0).length);
		AttackCooldown = false;
	}
	void OnLand(){
		animator.Play ("Landing", 0, 0);
		StartCoroutine ("OnLandWait");
	}
}