using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float moveDistance;
    [SerializeField]
    private float waitTime;

    private bool isMovingRight;
    private Rigidbody rb;
    private Vector3 startPos;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPos = transform.position;
        isMovingRight = true;
        StartCoroutine(MovingPlatform());
    }

    private IEnumerator MovingPlatform()
    {
        while(true)
        {
            //2�� ���
            yield return new WaitForSeconds(waitTime);

            //������ ���
            Vector3 targetPos = CalculateDistance(startPos, moveDistance);

            //�̵�
            while (Vector3.Distance(transform.position, targetPos) > 0.01f)
            {
                Vector3 newPos = Vector3.MoveTowards(transform.position, targetPos, moveSpeed *Time.fixedDeltaTime);
                rb.MovePosition(newPos);
                yield return new WaitForFixedUpdate();
            }

            //������ ��ġ ����
            rb.MovePosition(targetPos);
            //������ȯ
            isMovingRight = !isMovingRight;
            startPos = targetPos;

        }
    }
    private Vector3 CalculateDistance(Vector3 startPos, float moveDistance)
    {
        return startPos + (isMovingRight ? Vector3.right : Vector3.left) * moveDistance;
    }
}
