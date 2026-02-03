using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class Frustum : MonoBehaviour {
	
	public FrustumScreen screen;

	public void Start()
	{
		UnityEngine.XR.XRDevice.DisableAutoXRCameraTracking (GetComponent<Camera> (), true);
	}

    public void LateUpdate()
    {
        computeFrustum();
    }

    public void computeFrustum()
	{
		if (screen == null)
			return;
		
		Quaternion rot = screen.transform.rotation;
		transform.rotation = rot;
		
		Vector3 pos = screen.transform.InverseTransformPoint(transform.position);
		
		Vector2 screenSize = screen.screenSize;
		
		float left = (-screenSize.x / 2.0f) - pos.x;
		float right = (screenSize.x / 2.0f) - pos.x;
		
		float up = (screenSize.y / 2.0f) - pos.y;
		float down = (-screenSize.y / 2.0f) - pos.y;
		
		float dist = -pos.z;
		
		Camera c = GetComponent<Camera>();
		float ratio = dist / c.nearClipPlane;
		
		c.aspect = Mathf.Abs(right - left) / Mathf.Abs(up - down);
		c.fieldOfView = 2 * 2.0f * Mathf.Rad2Deg * Mathf.Atan2(Mathf.Max(Mathf.Abs(up), Mathf.Abs(down)), dist);
		

        Matrix4x4 projM = PerspectiveOffCenter(
            left / ratio,
            right / ratio,
            down / ratio,
            up / ratio,
            c.nearClipPlane,
            c.farClipPlane);

        c.projectionMatrix = projM;
        Camera.StereoscopicEye eye = Camera.StereoscopicEye.Left;
        if (c.stereoTargetEye == StereoTargetEyeMask.Right)
            eye = Camera.StereoscopicEye.Right;
        c.SetStereoProjectionMatrix(eye, projM);    
	}
	
	Matrix4x4 PerspectiveOffCenter(float left, float right, float bottom, float top, float near, float far)
	{
		float x = (2.0f * near) / (right - left);
		float y = (2.0f * near) / (top - bottom);
		float a = (right + left) / (right - left);
		float b = (top + bottom) / (top - bottom);
		float c = -(far + near) / (far - near);
		float d = -(2.0f * far * near) / (far - near);
		float e = -1.0f;

		Matrix4x4 m = new Matrix4x4();
		m[0, 0] = x; m[0, 1] = 0; m[0, 2] = a; m[0, 3] = 0;
		m[1, 0] = 0; m[1, 1] = y; m[1, 2] = b; m[1, 3] = 0;
		m[2, 0] = 0; m[2, 1] = 0; m[2, 2] = c; m[2, 3] = d;
		m[3, 0] = 0; m[3, 1] = 0; m[3, 2] = e; m[3, 3] = 0;
		return m;
	}
	
}
