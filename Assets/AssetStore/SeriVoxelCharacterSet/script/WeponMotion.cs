using UnityEngine;
//using UnityEditor;
using System.Collections;

//
// 装備武器によって、AnimatorControllerが替わる
// 
public class WeponMotion : MonoBehaviour {

	public GameObject Wepon;
	public Vector3 WeponScale = new Vector3(1f,1f,1f);
	public GameObject Shield;
	public GameObject RightHandPosition;
	public GameObject LeftHandPosition;

	[HideInInspector]
	public WeponStatus ws; // WeponStatus 
	/*							L WeponType (int)
	 * 								0:none 
	 * 								1:one hand wepon 
	 * 								2:two hand wepon 
	 * 								3:dual wepon 
	 * 								4:pole wepon
	 * 								5:bow
	 *                              6:magic wepon
	*/
	private Animator animator;
	private Animator DefaultAnimator; //初期設定時のモーション
	private GameObject[] EquipWepon= new GameObject[2]; //装備中の武器
	private GameObject EquipShield = null; //装備中の盾



	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		DefaultAnimator = animator;
		EquipWepon [0] = null;
		EquipWepon [1] = null;

		this.SetWepon (Wepon);
		this.SetShield (Shield);
	}

	//指定した武器に持ち替える。
	public void SetWepon(GameObject inWepon)
	{
		//武器を持っていない場合は設定されているアニメーション
		if (inWepon != null) {
			animator.runtimeAnimatorController = inWepon.GetComponent<Animator> ().runtimeAnimatorController;
			ws = inWepon.GetComponent<WeponStatus> ();
			Wepon = inWepon;
		}else{
			animator.runtimeAnimatorController = DefaultAnimator.runtimeAnimatorController;
			ws = null;
			Wepon = null;
		}
		//デフォルトの姿勢（T字姿勢）にしないとうまく装備できないので・・・・。
		animator.Play ("Default",0,0f);
		StartCoroutine("SetWeponProc");
	}
	//指定した盾に持ち替える。
	public void SetShield(GameObject inShield)
	{
		//武器を持っていない場合は設定されているアニメーション
		if (inShield != null) {
			Shield = inShield;
		}else{
			Shield = null;
		}
		//デフォルトの姿勢（T字姿勢）にしないとうまく装備できないので・・・・。
		animator.Play ("Default",0,0f);
		StartCoroutine("SetShieldProc");
	}
	//武器持ち替え　コルーチン
	private  IEnumerator SetWeponProc(){

		if (!animator.GetCurrentAnimatorStateInfo (0).IsName ("Default"))
			yield return null;

		if (Wepon != null) {
			if(ws != null) {
				if (EquipWepon[0] != null) {
					Destroy (EquipWepon[0]);
					Destroy (EquipWepon[1]);
				}
				switch (ws.WeponType) {
				case 0 : // none
					break;
				case 1: // one hand wepon
				case 6: // magic wepon
					EquipWepon [0] = (GameObject)Instantiate (Wepon, RightHandPosition.transform.position, Quaternion.identity);
					EquipWepon [0].transform.parent = RightHandPosition.transform;
					EquipWepon [0].transform.rotation = gameObject.transform.rotation;
					if (Shield != null) { //盾を持っている場合、装着する
						if(EquipShield != null){ Destroy(EquipShield);}
						EquipShield = (GameObject)Instantiate (Shield,LeftHandPosition.transform.position, LeftHandPosition.transform.rotation);
						EquipShield.transform.parent = LeftHandPosition.transform;
						EquipShield.transform.localScale = WeponScale;
						EquipShield.transform.localPosition = new Vector3 (0.4f, -0.5f, 0.0f);
					}
					break;
				case 2: // two hand wepon
					if(EquipShield != null){ Destroy(EquipShield);}

					EquipWepon[0] = (GameObject)Instantiate (Wepon, RightHandPosition.transform.position, Quaternion.identity);
					EquipWepon[0].transform.parent = RightHandPosition.transform;
					EquipWepon[0].transform.rotation = gameObject.transform.rotation;
					break;
				case 4: // pole wepon
					if(EquipShield != null){ Destroy(EquipShield);}

					EquipWepon[0] = (GameObject)Instantiate (Wepon, RightHandPosition.transform.position, Quaternion.identity);
					EquipWepon[0].transform.parent = RightHandPosition.transform;
					EquipWepon[0].transform.rotation = gameObject.transform.rotation;
					break;

				case 3: // dual wepon
					if (EquipShield != null) {
						Destroy (EquipShield);
					}

					//右手
					EquipWepon [0] = (GameObject)Instantiate (Wepon, RightHandPosition.transform.position, Quaternion.identity);
					EquipWepon [0].transform.parent = RightHandPosition.transform;
					EquipWepon [0].transform.rotation = gameObject.transform.rotation;
					//左手
					EquipWepon [1] = (GameObject)Instantiate (Shield, LeftHandPosition.transform.position, Quaternion.identity);
					EquipWepon [1].transform.parent = LeftHandPosition.transform;
					EquipWepon [1].transform.rotation = gameObject.transform.rotation;
					EquipWepon [1].transform.localScale = WeponScale;
					break;

				case 5 : // bow
					if(EquipShield != null) Destroy(EquipShield);

					EquipWepon[0] = (GameObject)Instantiate (Wepon, LeftHandPosition.transform.position, Quaternion.identity);
					EquipWepon[0].transform.parent = LeftHandPosition.transform;
					EquipWepon[0].transform.rotation = gameObject.transform.rotation;
					break;	
				}
				EquipWepon[0].transform.localScale = WeponScale;
			}
		}
		animator.Play ("Idle",0,0f);
	}
	//盾持ち替え　コルーチン
	private  IEnumerator SetShieldProc(){

		if (!animator.GetCurrentAnimatorStateInfo (0).IsName ("Default"))
			yield return null;

		if (Shield != null) {
			if (EquipShield != null)
				Destroy (EquipShield);
			switch (ws.WeponType) {
			case 0: // none
				break;
			case 1: // one hand wepon
			case 6: // magic wepon 
				EquipShield = (GameObject)Instantiate (Shield, LeftHandPosition.transform.position, LeftHandPosition.transform.rotation);
				EquipShield.transform.parent = LeftHandPosition.transform;
				EquipShield.transform.localScale = WeponScale;
				EquipShield.transform.localPosition = new Vector3 (0.4f, -0.5f, 0.0f);
					
				break;
			case 2: // two hand wepon
				break;
			case 4: // pole wepon
				break;

			case 3: // dual wepon
				break;

			case 5: // bow
				break;	
			}
			EquipWepon [0].transform.localScale = WeponScale;
		}
		animator.Play ("Idle",0,0f);
	}
}
