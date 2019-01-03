using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wall : MonoBehaviour {

    private int hp = 10;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    public void bullet_hit(int attack) {
        hp = hp - attack;
        if (hp == 0)
        {
            var r = Random.Range(0f, 1f);
            var g = Random.Range(0f, 1f);
            var b = Random.Range(0f, 1f);
            GetComponent<Renderer>().material.color = new Color(r, g, b);

            hp = 10;
        }


    }

}
