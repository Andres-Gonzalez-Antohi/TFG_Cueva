using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using System.Reflection;

public class RemoteController : MonoBehaviour {

    UdpClient udp = null;
    public int localPortNumber = 6000;
    public Transform TargetTransform;
    public Transform PointerTransform;
    public Casting Activado;
    //public CutController cutController;

    // Use this for initialization
    void Start () {
        initSocket();
    }

    void OnDestroy() {
        udp.Close();
    }

    // Update is called once per frame
    void Update () {
        receive();
	}

    void initSocket()
    {
        //System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
        //customCulture.NumberFormat.NumberDecimalSeparator = ".";

        //System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
        udp = new UdpClient(localPortNumber);
        udp.Client.ReceiveTimeout = 3;
        //udp.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

        Debug.Log("Initialized socket on port " + localPortNumber);
    }

    void receive()
    {
        try
        {
            IPEndPoint rep = new IPEndPoint(IPAddress.Any, 0);
            byte[] bytes = udp.Receive(ref rep);

            string msg = System.Text.Encoding.UTF8.GetString(bytes);
            Debug.Log(msg);

            Quaternion q = TargetTransform.localRotation;

            string[] data = msg.Split(' ');
            if (data[0] == "Move")
            {
                Vector3 off = new Vector3(float.Parse(data[2]), float.Parse(data[3]), float.Parse(data[1]));
                TargetTransform.localPosition += q * (off * Time.deltaTime * 0.00001f);
            }
            else if (data[0] == "Turn")
            {
                float v = Time.deltaTime * 0.0001f;
                Quaternion rot = Quaternion.Euler(v * float.Parse(data[2]), v * float.Parse(data[1]), 0);
                TargetTransform.localRotation = q * rot;
                //TargetTransform.Rotate(new Vector3(0, float.Parse(data[1]), 0) * Time.deltaTime);
            }
            else if (data[0] == "MovePointer")
            {
                float v = Time.deltaTime * 0.0000001f;
                TargetTransform.position += PointerTransform.up * v * float.Parse(data[1]);
            }
            else if (data[0] == "LookAxis")
            {
                Vector3 axis = new Vector3(float.Parse(data[1]), float.Parse(data[3]), float.Parse(data[2]));
                Quaternion rot = Quaternion.Euler(axis);
                TargetTransform.rotation = rot;
            }
            else if (data[0] == "PointerIntersection")
            {
                Debug.Log("detectada pulsacion");
                Activado.activo = bool.Parse(data[1]);
            }
            else if (data[0] == "ShowClipPlane")
            {
                //cutController.enableCutPlane = (data[1] == "true");
            }
            else if (data[0] == "MoveClipPlaneWithPointer") 
            {
                //cutController.enableMovement = (data[1] == "true");
            }

        }
        catch (System.Exception e)
        {
            //Debug.Log(e.Message);
        }
    }

}
