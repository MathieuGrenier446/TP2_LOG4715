using UnityEngine;

public class Obstacle : MonoBehaviour
{
    GameObject BaseObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // create object
        BaseObject = new GameObject("Obstacle");

        // add components
        BaseObject.AddComponent<CapsuleCollider>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
