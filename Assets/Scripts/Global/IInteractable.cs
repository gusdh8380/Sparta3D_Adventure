using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//��ȣ�ۿ� �������̽�
public interface IInteractable 
{
    public string GetInteractPrompt();
    public void OnInteract();

}
