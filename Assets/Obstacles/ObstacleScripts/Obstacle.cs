using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private float travelDistance = 5f;
    private Vector3 startPosition;
    [SerializeField] private bool moveRight = true;
    void Start()
    {
        startPosition = transform.position;
        StartCoroutine(MoveBackAndForth());

    }

    private IEnumerator MoveBackAndForth()
    {
        while (true)
        {
            Vector3 targetPosition;

            if (moveRight)
            {
                targetPosition = startPosition + Vector3.forward * travelDistance;
                transform.rotation = Quaternion.Euler(0, 90, 0);
            }
            else
            {
                targetPosition = startPosition - Vector3.forward * travelDistance;
                transform.rotation = Quaternion.Euler(0, -90, 0);
            }

            while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                yield return null;
            }

            moveRight = !moveRight;
        }
    }
}
