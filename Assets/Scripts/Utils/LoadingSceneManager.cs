using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadingSceneManager : MonoBehaviour
{
    public static int nextSceneIndex;
    [SerializeField] private Image progressBar;

    private void Start()
    {
        StartCoroutine(LoadScene());
    }

    public static void LoadScene(int sceneIndex)
    {
        nextSceneIndex = sceneIndex;
        SceneManager.LoadScene(5); // 로딩 씬으로 전환
    }

    private IEnumerator LoadScene()
    {
        // 페이드 인
        if (SceneFader.instance.fadeImage != null)
        {
            yield return StartCoroutine(SceneFader.instance.FadeIn());
        }

        yield return null; // 프레임 대기

        AsyncOperation op = SceneManager.LoadSceneAsync(nextSceneIndex);
        op.allowSceneActivation = false; // 씬이 로드된 후에 자동으로 활성화되지 않도록 설정

        while (!op.isDone)
        {
            // 진행 상황 비율을 업데이트
            float progress = Mathf.Clamp01(op.progress / 0.9f); // op.progress는 0에서 0.9 사이의 값을 가짐
            progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, progress, Time.deltaTime * 2); // Lerp의 시간 속도 조정

            // 씬 로드가 완료되고 씬 활성화 조건이 충족되면 씬을 활성화
            if (op.progress >= 0.9f && progressBar.fillAmount >= 0.99f)
            {
                op.allowSceneActivation = true;
            }

            yield return null; // 다음 프레임 대기
        }
    }
}
