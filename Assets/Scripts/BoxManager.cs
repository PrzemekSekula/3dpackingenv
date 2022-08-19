using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BoxManager : MonoBehaviour
{
    public BoxController[] boxList;

    void Start()
    {
        UpdateBoxList(); 
        Debug.Log(GetBoxStates());
    }

    void Update()
    {

    }

    public void PrepareTeleport()
    {
        for (int i = 0; i < boxList.Length; i++) 
        {
            boxList[i].isTeleporting = false;
            Destroy(boxList[i].gameObject.GetComponent<Rigidbody>());
        }

    }


    public void UpdateBoxList()
    {
        boxList = (BoxController[]) GameObject.FindObjectsOfType (typeof(BoxController));
        Debug.Log("New len of boxList: " + boxList.Length);
    }

    public string GetBoxStates() {

        List<string> boxStates = new List<string>();

        for (int i = 0; i < boxList.Length; i++) 
        {
            BoxState boxState = new BoxState();
            boxState.boxId = i;
            boxState.state = boxList[i].GetStateString();
            string json = JsonUtility.ToJson(boxState);
            boxStates.Add(json);
        }
        return string.Join(",", boxStates);
    }

    public string GetState()
    {
        return "{\"boxes\":"+GetBoxStates();
    }



    public void TakeAction (string content) 
    {
        MoveBoxAction action = JsonUtility.FromJson<MoveBoxAction>(content);

        //Debug.Log($"Box id: {action.boxId}, x: {action.x}, y: {action.y}, z: {action.z}");          
        if (action.boxId < boxList.Length)
        {
            boxList[action.boxId].Teleport(action.x, action.y, action.z);
        }
        else
        {
            Debug.Log($"We have only {boxList.Length} strings registered.");
        } 
    }



}  


[System.Serializable]
public class MoveBoxAction
{
    public int boxId;
    public float x;
    public float y;
    public float z;
    public int rotation;

}

[System.Serializable]
public class BoxState
{
    public string objType = "box";
    public int boxId;   
    public string state;

}