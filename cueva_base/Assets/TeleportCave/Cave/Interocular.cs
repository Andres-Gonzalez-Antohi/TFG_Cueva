using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interocular : MonoBehaviour {

    public float distance = 0.044f;
    public float factor = 1.0f;

    public Transform left, right;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.I))
        {
            factor *= -1.0f;
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            factor *= 1.1f;
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            factor *= 0.9f;
        }

        left.localPosition = new Vector3(-distance*factor / 2.0f, 0, 0);
        right.localPosition = new Vector3(distance*factor / 2.0f, 0, 0);
    }
}
