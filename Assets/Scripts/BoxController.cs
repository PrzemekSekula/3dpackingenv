using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BoxController : MonoBehaviour
{
    
    public BoxManager boxManager = null;

    public int boxId { get; private set; }
    private Vector3 oldPosition;
    private Vector3 currentPosition;

    public Rigidbody rb;
    public bool isTeleporting = false;

    void Awake()
    {
        boxManager = (BoxManager) GameObject.FindObjectOfType (typeof(BoxManager));
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
        boxManager.PrepareTeleport();
    
        isTeleporting = true;
        gameObject.AddComponent<Rigidbody>();
        
        oldPosition = gameObject.transform.position; 
        currentPosition = new Vector3(x, y, z);

        gameObject.transform.position = currentPosition;  
    }  

    void OnTriggerEnter(Collider col)
    {
        if (isTeleporting)
        {
            Debug.Log("I (" + gameObject.name + ") " +
            "collided with " + col.gameObject.name + ". Moving back.");
            Destroy(gameObject.GetComponent<Rigidbody>());
            isTeleporting = false;
            gameObject.transform.position = oldPosition;
        } 
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