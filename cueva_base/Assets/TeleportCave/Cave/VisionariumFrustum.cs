using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionariumFrustum : MonoBehaviour
{
    public FrustumScreen left;
    public FrustumScreen right;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("VisionariumFrustum"))
        {
            Vector3 pos = Vector3.right * PlayerPrefs.GetFloat("VisionariumFrustum",0);
            left.transform.localPosition = pos;
            right.transform.localPosition = -pos;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveFrustum(0.001f);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            moveFrustum(-0.001f);
        }

        
    }

    void moveFrustum(float val)
    {
        Vector3 pos = left.transform.localPosition;
        pos += val * Vector3.right;
        left.transform.localPosition = pos;
        right.transform.localPosition = -pos;

        PlayerPrefs.SetFloat("VisionariumFrustum", pos.x);
    }
}
