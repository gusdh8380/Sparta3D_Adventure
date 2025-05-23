using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//상호작용 인터페이스
public interface IInteractable 
{
    public string GetInteractPrompt();
    public void OnInteract();

}
