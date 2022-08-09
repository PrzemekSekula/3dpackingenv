using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BoxController : MonoBehaviour
{
    
    //public BoxManager boxManager = null;

    public int boxId { get; private set; }
    private Vector3 oldPosition;
    private Vector3 currentPosition;

    void Awake()
    {
        //boxManager = (BoxManager) GameObject.FindObjectOfType (typeof(BoxManager));
        currentPosition = gameObject.transform.position; 
    }

    // Start is called before the first frame update
    void Start()
    {

    }   

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GetStateString() 
    {
        MyState myState = new MyState();

        myState.x = gameObject.transform.position.x;
        myState.y = gameObject.transform.position.y;
        myState.z = gameObject.transform.position.z;
        myState.rotation = 0;

        string json = JsonUtility.ToJson(myState);
        return json;
    }

    public void Teleport(float x, float y, float z)
    {
        Debug.Log("Teleporting " + gameObject.name);
        oldPosition = gameObject.transform.position; 
        currentPosition = new Vector3(x, y, z);

        gameObject.transform.position = currentPosition;  
    }  

    public void StayAtCurrentPosition()
    {
        gameObject.transform.position = currentPosition;  
    }

    void OnTriggerEnter(Collider col)
    {
        /*
        Debug.Log("On Trigger Enter " + col.gameObject.name + " Old position: " + oldPosition);
        if (boxManager.currentBox.gameObject.name == gameObject.name)
        {
            gameObject.transform.position = oldPosition; 
            //BoxController newBox = (BoxController) col.g;
            col.gameObject.StayAtCurrentPosition();
            
            //Debug.Log("CurrentObject" + col.gameObject.name + " Old position: " + oldPosition);
        }
        gameObject.transform.position = oldPosition; 
        //Rigidbody rb = GetComponent<Rigidbody>();
        //rb.velocity = Vector3.zero;
        //rb.angularVelocity = Vector3.zero;
        */
    } 
}


[System.Serializable]
public class MyState
{
    public float x;
    public float y;
    public float z;

    public int rotation;

}