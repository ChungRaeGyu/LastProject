using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneController : MonoBehaviour
{
    void Update()
    {
#if UNITY_EDITOR // ����Ƽ ������
        if (Input.GetMouseButtonDown(0))
        {
            LoadLobbyScene();
        }
#elif UNITY_ANDROID || UNITY_IOS // �ȵ���̵� �Ǵ� iOS
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            LoadLobbyScene();
        }
#endif
    }

    void LoadLobbyScene()
    {
        SceneManager.LoadScene("LobbyScene");
    }
}
