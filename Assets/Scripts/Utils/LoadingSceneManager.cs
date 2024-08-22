using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using System.Collections.Generic;

public class LoadingSceneManager : MonoBehaviour
{
    public static int nextSceneIndex;
    [SerializeField] private Image progressBar;
    public TMP_Text randomTipText;

    // �����Ϳ��� �ؽ�Ʈ�� �Է��� �� �ִ� ����Ʈ
    [SerializeField] private List<string> tipTexts = new List<string>();

    private void Start()
    {
        // ����Ʈ���� �������� �ϳ��� �ؽ�Ʈ�� �����Ͽ� ����
        if (tipTexts.Count > 0)
        {
            int randomIndex = Random.Range(0, tipTexts.Count);
            randomTipText.text = $"- <color=#FFFF00>Tip</color> {tipTexts[randomIndex]} -";
        }

        StartCoroutine(LoadScene());
    }

    public static void LoadScene(int sceneIndex)
    {
        nextSceneIndex = sceneIndex;
        SceneManager.LoadScene(5); // �ε� ������ ��ȯ
    }

    private IEnumerator LoadScene()
    {
        // ���̵� ��
        if (SceneFader.instance.fadeImage != null)
        {
            yield return StartCoroutine(SceneFader.instance.FadeIn());
        }

        yield return null; // ������ ���

        AsyncOperation op = SceneManager.LoadSceneAsync(nextSceneIndex);
        op.allowSceneActivation = false; // ���� �ε�� �Ŀ� �ڵ����� Ȱ��ȭ���� �ʵ��� ����

        while (!op.isDone)
        {
            // ���� ��Ȳ ������ ������Ʈ
            float progress = Mathf.Clamp01(op.progress / 0.9f); // op.progress�� 0���� 0.9 ������ ���� ����
            progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, progress, Time.deltaTime * 2); // Lerp�� �ð� �ӵ� ����

            // �� �ε尡 �Ϸ�ǰ� �� Ȱ��ȭ ������ �����Ǹ� ���� Ȱ��ȭ
            if (op.progress >= 0.9f && progressBar.fillAmount >= 0.99f)
            {
                op.allowSceneActivation = true;
            }

            yield return null; // ���� ������ ���
        }
    }
}
