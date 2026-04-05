using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public bool isLocal = true;
    public float rspeed = 5; // degrees
    public float mSpeed = 0.1f; //meters
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, rspeed * Time.deltaTime, 0);
        if (isLocal)
            transform.Translate(0, 0, mSpeed);
        else
            transform.Translate(0, 0, mSpeed * Time.deltaTime, Space.World);
    }
        
}
