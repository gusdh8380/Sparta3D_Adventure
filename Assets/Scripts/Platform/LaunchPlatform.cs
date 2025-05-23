using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LaunchPlatform : MonoBehaviour
{
    [SerializeField]
    private float force;
    [SerializeField]
    private int WaitTime;
    [SerializeField]
    private TextMeshPro time;
    [SerializeField] 
    private float launchAngle = 60f;

    private int wtime;

    private Coroutine launchCoroutine;
    private Rigidbody targetBody;


    private void Start()
    {
        wtime = WaitTime;
    }
    public void OnCollisionEnter(Collision collision)
    {
        // �÷��̾ (�±� �Ǵ� ������Ʈ üũ)
        if (!collision.gameObject.CompareTag("Player")) return;

        // �̹� �ڷ�ƾ�� ���� �ִٸ� ���߰� ���� ����
        if (launchCoroutine != null)
            StopCoroutine(launchCoroutine);

        targetBody = collision.rigidbody;
        time.gameObject.SetActive(true);
        launchCoroutine = StartCoroutine(Launching());
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.rigidbody == targetBody)
        {
            // �÷������� ����� Ÿ�̸ӿ� �ڷ�ƾ ���
            if (launchCoroutine != null)
                StopCoroutine(launchCoroutine);
            launchCoroutine = null;
            time.gameObject.SetActive(false);
        }
    }

    private IEnumerator Launching()
    {
        
        for (int i = WaitTime; i >=0; i--)
        {
            time.text = $"{i}";
            Debug.Log($"�߻� {i}�� ��");
            yield return new WaitForSeconds(1);
        }
        time.text = "�߻�!";
        var pc = targetBody.GetComponent<PlayerController>();
        pc?.BeginBallistic();
        float rad = launchAngle * Mathf.Deg2Rad;
        Vector3 dir = new Vector3(-Mathf.Cos(rad), Mathf.Sin(rad), 0f);
        
        targetBody.AddForce(dir * force , ForceMode.VelocityChange);

        yield return new WaitForSeconds(1f);
        time.gameObject.SetActive(false);
        launchCoroutine = null; ;


    }
}
