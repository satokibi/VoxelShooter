using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}


    void OnCollisionEnter(Collision collision)
    {
        var r = Random.Range(0f, 1f);
        var g = Random.Range(0f, 1f);
        var b = Random.Range(0f, 1f);
        GetComponent<Renderer>().material.color = new Color(r, g, b);
        Debug.Log("hit");

        wall w = collision.gameObject.GetComponent(typeof(wall)) as wall;
        if( w != null ) {
            w.bullet_hit(1);
        }
        // Destroy(this.gameObject);
    }
}
