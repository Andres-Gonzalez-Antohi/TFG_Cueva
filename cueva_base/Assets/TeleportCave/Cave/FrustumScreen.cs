using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrustumScreen : MonoBehaviour {
	
	public Vector2 screenSize = new Vector2(2.5f, 2.5f);

	// Use this for initialization
	void OnDrawGizmos () 
	{
        Gizmos.color = Color.red;
        
		//Gizmos.DrawWireCube(transform.position, new Vector3(screenSize.x, screenSize.y, 0.01f));
		Gizmos.DrawLine(transform.TransformPoint(new Vector3(-screenSize.x / 2, -screenSize.y / 2, 0)),
						transform.TransformPoint(new Vector3(screenSize.x / 2, -screenSize.y / 2, 0))
		);
		
		Gizmos.DrawLine(transform.TransformPoint(new Vector3(screenSize.x / 2, -screenSize.y / 2, 0)),
			transform.TransformPoint(new Vector3(screenSize.x / 2, screenSize.y / 2, 0))
		);
		
		Gizmos.DrawLine(transform.TransformPoint(new Vector3(screenSize.x / 2, screenSize.y / 2, 0)),
			transform.TransformPoint(new Vector3(-screenSize.x / 2, screenSize.y / 2, 0))
		);
		
		Gizmos.DrawLine(transform.TransformPoint(new Vector3(-screenSize.x / 2, screenSize.y / 2, 0)),
			transform.TransformPoint(new Vector3(-screenSize.x / 2, -screenSize.y / 2, 0))
		);
			
	}


}
