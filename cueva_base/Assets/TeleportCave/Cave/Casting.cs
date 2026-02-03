using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class Casting : MonoBehaviour {
    public float TargetDistance;
    public TextMesh texto;
    public bool activo = false;
	// Use this for initialization
	void Start () {
		
	}
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 direction = transform.TransformDirection(Vector3.up) * TargetDistance;
      if (activo)
            Gizmos.DrawRay(transform.position, direction);
      
    }
    // Update is called once per frame
    void Update () {
        RaycastHit theHit;
        if ( activo && Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out theHit))
        {
            TargetDistance = theHit.distance;
            texto.text =string.Format("{0} m",TargetDistance.ToString("F2"));
         //   Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * theHit.distance, Color.yellow);
            Debug.Log(texto.text);
            Gizmos.color = Color.red;
            Vector3 direction = transform.TransformDirection(Vector3.up) * theHit.distance;
            //Gizmos.DrawRay(transform.position, direction);
        }
        else
            texto.text = "";
	}
}
