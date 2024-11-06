using UnityEngine;

public class WallCollider: MonoBehaviour {

    public bool isColliding;

    void FixedUpdate(){
        isColliding = false;
    }

    void OnCollisionEnter(Collision collision){
        isColliding = true;
    }
}