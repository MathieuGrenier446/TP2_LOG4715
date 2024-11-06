using UnityEngine;

public class WallCollider: MonoBehaviour {

    public bool isColliding;

    void OnTriggerEnter(Collider collider){
        if(collider.tag!="Player") isColliding = true;
    }
}