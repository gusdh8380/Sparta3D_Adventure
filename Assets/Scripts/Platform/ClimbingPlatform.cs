using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//탈수있는 벽
public class ClimbingPlatform : MonoBehaviour, IInteractable
{
    public string GetInteractPrompt()
    {
        string str = $"벽타기 : F";
        return str;
    }

    public void OnInteract()
    {
        GameObject.FindWithTag("Player").GetComponent<PlayerController>().EnterClimbMode();
    }
}
