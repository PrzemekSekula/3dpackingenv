using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using UnityEngine;

public class TCP_client : MonoBehaviour
{

    public String Host = "127.0.0.1";
    public Int32 Port = 65432;

    TcpClient mySocket = null;
    NetworkStream theStream = null;
    StreamWriter theWriter = null;

    int waitBeforeResponse = 0;

    // Start is called before the first frame update
    void Start()
    {
        mySocket = new TcpClient();
        waitBeforeResponse = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (waitBeforeResponse > 0)
        {
            waitBeforeResponse -= 1;
            if (waitBeforeResponse <= 0)
            {
                SendResponse();
            }            
        }
        else
        {
            GetAction();
        }
    }

    public bool SendResponse()
    {
        try
        {
            if (!mySocket.Connected)
            {
                mySocket.Connect(Host, Port);
            }

            theStream = mySocket.GetStream();
            theWriter = new StreamWriter(theStream);

            SetResponse sr = new SetResponse();
            sr.response = gameObject.GetComponent<BoxManager>().GetState();
            
            string response = JsonUtility.ToJson(sr);
            Debug.Log("Response ready: " + response);
            Byte[] sendBytes = System.Text.Encoding.UTF8.GetBytes(response);
            mySocket.GetStream().Write(sendBytes, 0, sendBytes.Length); 
            return true;
        }
        catch (Exception e)
        {
            Debug.Log("Socket error: " + e);
            return false;
        }
       
    }

    public bool GetAction()
    {
        try
        {
            if (!mySocket.Connected)
            {
                mySocket.Connect(Host, Port);
            }

            theStream = mySocket.GetStream();
            theWriter = new StreamWriter(theStream);
            Byte[] sendBytes = System.Text.Encoding.UTF8.GetBytes("{\"mode\": \"getAction\"}");
            mySocket.GetStream().Write(sendBytes, 0, sendBytes.Length);
            //Debug.Log("socket is sent");

            Byte[] readBytes = new Byte[1024]; 
            int length = mySocket.GetStream().Read(readBytes, 0, readBytes.Length);
            var incommingData = new byte[length]; 							
            Array.Copy(readBytes, 0, incommingData, 0, length);  							
            string response = System.Text.Encoding.ASCII.GetString(incommingData); 							
            // Debug.Log("client message received as: " + response); 	
            if (response != "None")
            {
                waitBeforeResponse = 5;
                gameObject.GetComponent<BoxManager>().TakeAction(response);
            }
  
            return true;
        }
        catch (Exception e)
        {
            Debug.Log("Socket error: " + e);
            return false;
        }
    }

    private void OnApplicationQuit()
    {
        if (mySocket != null && mySocket.Connected)
            mySocket.Close();
    }
}

[System.Serializable]
public class SetResponse
{
    public string mode = "setResponse";
    public string response;

}
