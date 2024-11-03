using UnityEngine;

public class Gizmo : MonoBehaviour
{
    private void OnDrawGizmosSelected(){
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 0.10f);
    }
}
