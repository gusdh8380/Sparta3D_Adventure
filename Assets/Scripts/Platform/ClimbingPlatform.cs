using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Ż���ִ� ��
public class ClimbingPlatform : MonoBehaviour, IInteractable
{
    public string GetInteractPrompt()
    {
        string str = $"��Ÿ�� : F";
        return str;
    }

    public void OnInteract()
    {
        GameObject.FindWithTag("Player").GetComponent<PlayerController>().EnterClimbMode();
    }
}
