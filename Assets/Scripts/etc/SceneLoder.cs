using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//챌린지 세션 강의 노트
//프로젝트 구현과 전혀 상관없는 코드
public class SceneLoder : MonoBehaviour
{
    public Text statusText;

   
    private void Start()
    {
        //StartCoroutine(LoadSceneCoroutine("Game"));
        LoadSceneByAsync("SceneName");
    }

    //코루틴 방식----------------------------------------------------------------------------
    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        yield return null;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        while(!asyncOperation.isDone)
        {
            //씬 로드 진행 중
            statusText.text = $"진행룰 : {asyncOperation.progress}";

            yield return null;
        }
        //씬 로드가 완료
        yield return new WaitForSeconds(3);
        Debug.Log("씬 로드 완료");
    }

    //Asyne/Await 방식----------------------------------------------------------
    private async void LoadSceneByAsync(string sceneName)
    {
        statusText.text = "로딩 시작";


        await LoadSceneAsync(sceneName);

        //statusText.text = "로딩 끝";


    }

    private async Task LoadSceneAsync(string sceneName)
    {
        AsyncOperation a = SceneManager.LoadSceneAsync(sceneName);
        while (!a.isDone) 
        {
            statusText.text = "로딩 진행 중";
            
            await Task.Yield();
           
        }
    }

    //콜백 방식
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
            //씬 로드 진행 중
            statusText.text = $"진행룰 : {asyncOperation.progress}";

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
        { //씬 넘어갈 때 처리? 전투 준비 같은
            GameSceneLogging();
                                                        
        });

    }
    void GameSceneLogging()
    {
        //
    }
}


