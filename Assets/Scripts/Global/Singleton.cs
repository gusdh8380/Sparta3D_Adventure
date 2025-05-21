using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    private static bool applicationIsQuitting = false;


    public static T Instance
    {
        get
        {
            if (applicationIsQuitting)
            {
                Debug.LogWarning($"[{typeof(T)}] Instance �̹� �ı��Ǿ��ų� ���� ���Դϴ�.");
                return null;
            }

            if (instance == null)
            {
                // �� �� ���� ��ü Ž��
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    // ������ ���� ����
                    var go = new GameObject(typeof(T).Name);
                    instance = go.AddComponent<T>();
                    DontDestroyOnLoad(go);
                }
            }
            return instance;
        }
    }

    public static T Current => instance;
    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    protected virtual void OnApplicationQuit()
    {
        applicationIsQuitting = true;
    }
}
