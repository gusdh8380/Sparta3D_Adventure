using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//ç���� ���� ���� ��Ʈ
//������Ʈ ������ ���� ������� �ڵ�
public class SceneLoder : MonoBehaviour
{
    public Text statusText;

   
    private void Start()
    {
        //StartCoroutine(LoadSceneCoroutine("Game"));
        LoadSceneByAsync("SceneName");
    }

    //�ڷ�ƾ ���----------------------------------------------------------------------------
    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        yield return null;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        while(!asyncOperation.isDone)
        {
            //�� �ε� ���� ��
            statusText.text = $"����� : {asyncOperation.progress}";

            yield return null;
        }
        //�� �ε尡 �Ϸ�
        yield return new WaitForSeconds(3);
        Debug.Log("�� �ε� �Ϸ�");
    }

    //Asyne/Await ���----------------------------------------------------------
    private async void LoadSceneByAsync(string sceneName)
    {
        statusText.text = "�ε� ����";


        await LoadSceneAsync(sceneName);

        //statusText.text = "�ε� ��";


    }

    private async Task LoadSceneAsync(string sceneName)
    {
        AsyncOperation a = SceneManager.LoadSceneAsync(sceneName);
        while (!a.isDone) 
        {
            statusText.text = "�ε� ���� ��";
            
            await Task.Yield();
           
        }
    }

    //�ݹ� ���
    public void LoadSceneWithCallback(string name, Action callback)
    {
        StartCoroutine(LoadSceneCoroutinec(name, callback));
    }
    private IEnumerator LoadSceneCoroutinec(string sceneName, Action a)
    {
        yield return null;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncOperation.isDone)
        {
            //�� �ε� ���� ��
            statusText.text = $"����� : {asyncOperation.progress}";

            yield return null;
        }
        a?.Invoke();
    }
}

public class LoadScene : MonoBehaviour
{
    public SceneLoder SceneLoder;
    private void Start()
    {

        SceneLoder.LoadSceneWithCallback("name", () => 
        { //�� �Ѿ �� ó��? ���� �غ� ����
            GameSceneLogging();
                                                        
        });

    }
    void GameSceneLogging()
    {
        //
    }
}


