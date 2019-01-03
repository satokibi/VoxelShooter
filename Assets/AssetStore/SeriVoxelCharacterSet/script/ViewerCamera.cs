using UnityEngine;
using System.Collections;

public class ViewerCamera : MonoBehaviour
{

    public GameObject viewObject = null;

    public float rotationSensitivity = 0.01f;
    public float distanceSensitivity = 5.0f;
    public float followObjectSmooth = 3f;
    public float maxRotationY = 0.45f;
    public float minRotationY = -0.45f;
    public float minDistance = 0.5f;
    public float maxDistance = 5f;

    public float defaultDistance = 4f;
    public float defaultAngularPositionX = 0f;
    public float defaultAngularPositionY = 0f;

    protected float distance = 0f;
    protected Vector2 cameraPosParam = Vector2.zero;

    private Vector3 clickedPos = Vector3.zero;
    private int clickedFlag = 0; //0:none 1:left 2:right
    private Vector3 pivotTemp = Vector3.zero;
    private float distanceTemp = 0f;
    private Vector2 cameraPosParamTemp = Vector2.zero;
    private float ZoomPower = 0.0f;

    // Use this for initialization
    void Start()
    {
        this.distance = this.defaultDistance;
        this.distanceTemp = this.defaultDistance;
        this.cameraPosParam = new Vector2(this.defaultAngularPositionX / 180f * Mathf.PI, this.defaultAngularPositionY / 180f * Mathf.PI);
        this.pivotTemp = this.transform.position;
	}

    // Update is called once per frame
    void Update()
    {
		if (this.clickedFlag == 0)
        {
            if (Input.GetMouseButtonDown(1))
            {
                this.clickedPos = Input.mousePosition;
                this.cameraPosParamTemp = this.cameraPosParam;
                this.clickedFlag = 1;
            }
        }
        if (this.clickedFlag == 1 && Input.GetMouseButtonUp(1)){ this.clickedFlag = 0; }
        Vector3 mousePosDistance = Input.mousePosition - this.clickedPos;
       
        if(this.clickedFlag==1)
        {
            var diff = new Vector2(mousePosDistance.x, -mousePosDistance.y) * rotationSensitivity;
            this.cameraPosParam.x = this.cameraPosParamTemp.x + diff.x;
            this.cameraPosParam.y = Mathf.Clamp(this.cameraPosParamTemp.y + diff.y, this.minRotationY * Mathf.PI, this.maxRotationY * Mathf.PI);
        }

        float MouseWheel = Input.GetAxis("Mouse ScrollWheel");
		if (MouseWheel != 0f) {
			ZoomPower = this.distanceTemp + MouseWheel * 60.0f;
			MouseWheel = 0f;
		} else
			ZoomPower = this.distanceTemp;
        this.distance = Mathf.Clamp(Mathf.Lerp(this.distanceTemp, this.ZoomPower, Time.deltaTime * this.distanceSensitivity),
                                    this.minDistance,
                                    this.maxDistance);
        this.distanceTemp = this.distance;

        Vector3 orbitPos = GetOrbitPosition(this.cameraPosParam, this.distance);
        Vector3 pivot = Vector3.Lerp(this.pivotTemp, this.viewObject.transform.position, Time.deltaTime * this.followObjectSmooth);
        this.transform.position = pivot + orbitPos;
        this.transform.LookAt(this.viewObject.transform);
        
        //カメラの後方にレイを飛ばし近くのオブジェクトに当たったら
        //当たった位置にカメラを移動する
        RaycastHit hit;
        if (Physics.Raycast(this.viewObject.transform.position, -this.transform.forward, out hit, this.distance))
        {
			this.transform.position = hit.point;// + this.transform.forward;
        }

        this.pivotTemp = pivot;
    }

    private Vector3 GetOrbitPosition(Vector2 anglarParam, float distance)
    {
        float x = Mathf.Sin(anglarParam.x) * Mathf.Cos(anglarParam.y);
        float z = Mathf.Cos(anglarParam.x) * Mathf.Cos(anglarParam.y);
        float y = Mathf.Sin(anglarParam.y);

        return new Vector3(x, y, z) * distance;
    }
}