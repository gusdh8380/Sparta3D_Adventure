using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingPlatform : MonoBehaviour, IInteractable
{
    public string GetInteractPrompt()
    {
        string str = $"º®Å¸±â : F";
        return str;
    }

    public void OnInteract()
    {
        GameObject.FindWithTag("Player").GetComponent<PlayerController>().EnterClimbMode();
    }
}
