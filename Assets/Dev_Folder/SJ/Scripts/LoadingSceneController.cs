using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneController : MonoBehaviour
{
    public Slider loadingSlider; // 진행 바
    public Text loadingText; // 진행 텍스트

    void Start()
    {
        // 예시: 게임 시작 로딩 처리
        StartCoroutine(LoadGameScene());
    }

    IEnumerator LoadGameScene()
    {
        yield return null;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("GameScene"); // 로드할 Scene 이름 지정

        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f); // 진행률 계산
            loadingSlider.value = progress; // 진행 바 업데이트
            loadingText.text = $"로딩 중... {progress * 100f}%"; // 진행 텍스트 업데이트
            yield return null;
        }

        // 씬 로드 완료 후 다음 동작 수행
        OnLoadComplete();
    }

    void OnLoadComplete()
    {
        // 다음 씬으로 이동하거나 다음 동작 수행
        SceneManager.LoadScene("GameScene"); // 예시로 다음 씬으로 이동하는 코드
    }
}
