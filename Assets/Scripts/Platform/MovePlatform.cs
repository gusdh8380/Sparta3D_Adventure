using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float moveDistance;
    [SerializeField]
    private float waitTime;
    [SerializeField] 
    private LayerMask playerLayer;

    private bool isMovingRight;
    private Rigidbody rb;
    private Vector3 startPos;
    private Vector3 endPos;

    private HashSet<Transform> playerOnPlatform = new HashSet<Transform>();

    [SerializeField]
    private TextMeshPro Right;
    [SerializeField]
    private TextMeshPro Left;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPos = transform.position;
        endPos = transform.position;
        isMovingRight = true;
        Right.enabled = true;
        Left.enabled = false;
        StartCoroutine(MovingPlatform());
    }
    void FixedUpdate()
    {
        Vector3 delta = transform.position - endPos;
        endPos = transform.position;   

        foreach(Transform t in playerOnPlatform)
        {
            t.position += delta;
        }
    }

    private IEnumerator MovingPlatform()
    {
        while(true)
        {
            //2초 대기
            yield return new WaitForSeconds(waitTime);

            //도착지 계산
            Vector3 targetPos = CalculateDistance(startPos, moveDistance);

            //이동
            while (Vector3.Distance(transform.position, targetPos) > 0.01f)
            {
                Vector3 newPos = Vector3.MoveTowards(transform.position, targetPos, moveSpeed *Time.fixedDeltaTime);
                rb.MovePosition(newPos);
                yield return new WaitForFixedUpdate();
            }

            //도착지 위치 고정
            rb.MovePosition(targetPos);
            //방향전환
            isMovingRight = !isMovingRight;
            DirectionTextConversion();
            startPos = targetPos;

        }
    }
    private Vector3 CalculateDistance(Vector3 startPos, float moveDistance)
    {
        return startPos + (isMovingRight ? Vector3.right : Vector3.left) * moveDistance;
    }
    private void DirectionTextConversion()
    {
        Right.enabled = !Right.enabled;
        Left.enabled = !Left.enabled;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(((1<<other.gameObject.layer) & playerLayer) != 0)
        {
            playerOnPlatform.Add(other.transform);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            playerOnPlatform.Remove(other.transform);
        }
    }
}
