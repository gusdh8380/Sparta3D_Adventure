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
        // 플레이어만 (태그 또는 컴포넌트 체크)
        if (!collision.gameObject.CompareTag("Player")) return;

        // 이미 코루틴이 돌고 있다면 멈추고 새로 시작
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
            // 플랫폼에서 벗어나면 타이머와 코루틴 취소
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
            Debug.Log($"발사 {i}초 전");
            yield return new WaitForSeconds(1);
        }
        time.text = "발사!";
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
