using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//점프대 오브젝트 스크립트
public class JumpCube : MonoBehaviour
{
    [SerializeField]
    private float force;

    private void OnCollisionEnter(Collision collision)
    {
        collision.rigidbody.AddForce(Vector3.up * force, ForceMode.Impulse);
    }

}
