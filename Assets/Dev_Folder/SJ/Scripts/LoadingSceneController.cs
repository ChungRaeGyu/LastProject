using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneController : MonoBehaviour
{
    public Slider loadingSlider; // ���� ��
    public Text loadingText; // ���� �ؽ�Ʈ

    void Start()
    {
        // ����: ���� ���� �ε� ó��
        StartCoroutine(LoadGameScene());
    }

    IEnumerator LoadGameScene()
    {
        yield return null;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("GameScene"); // �ε��� Scene �̸� ����

        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f); // ����� ���
            loadingSlider.value = progress; // ���� �� ������Ʈ
            loadingText.text = $"�ε� ��... {progress * 100f}%"; // ���� �ؽ�Ʈ ������Ʈ
            yield return null;
        }

        // �� �ε� �Ϸ� �� ���� ���� ����
        OnLoadComplete();
    }

    void OnLoadComplete()
    {
        // ���� ������ �̵��ϰų� ���� ���� ����
        SceneManager.LoadScene("GameScene"); // ���÷� ���� ������ �̵��ϴ� �ڵ�
    }
}
