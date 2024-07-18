using UnityEngine;

[System.Serializable]
public class AnimationFrame
{
    public float[] curve;
    public AnimationTransform transform;
    public float duration;
}

[System.Serializable]
public class AnimationTransform
{
    public float x;
    public float y;
    public float skX;
    public float skY;
    public float scX;
    public float scY;
}

[System.Serializable]
public class Animation
{
    public AnimationFrame[] frame;
    public string name;
}

[System.Serializable]
public class AnimationData
{
    public Animation[] animation;
}

public class MimicMonster : MonoBehaviour
{
    public TextAsset jsonFile; // DragonBone���� ������ JSON ����
    private AnimationData animationData;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (jsonFile != null)
        {
            LoadAnimationData(jsonFile.text);
        }
        else
        {
            Debug.LogError("JSON file is not assigned.");
        }

        PlayDefaultAction("Attack A");
    }

    private void LoadAnimationData(string jsonText)
    {
        animationData = JsonUtility.FromJson<AnimationData>(jsonText);
    }

    private void PlayDefaultAction(string animationName)
    {
        Animation animation = FindAnimation(animationName);
        if (animation != null)
        {
            PlayAnimation(animation);
        }
        else
        {
            Debug.LogError("Animation " + animationName + " not found.");
        }
    }

    public Animation FindAnimation(string animationName)
    {
        foreach (Animation animation in animationData.animation)
        {
            if (animation.name == animationName)
            {
                return animation;
            }
        }
        return null; // animationName�� �ش��ϴ� �ִϸ��̼��� ���� ���
    }

    private void PlayAnimation(Animation animation)
    {
        foreach (AnimationFrame frame in animation.frame)
        {
            ApplyTransform(frame.transform, frame.duration);
        }
    }

    private void ApplyTransform(AnimationTransform transform, float duration)
    {
        // �� ���� Ʈ�������� �����ϴ� ������ ���⿡ �����ؾ� �մϴ�.
        // ���÷δ� Debug.Log�� ����Ͽ� Ʈ�������� ����մϴ�.
        Debug.Log("Applying transform: " + transform.x + ", " + transform.y +
                  ", skX: " + transform.skX + ", skY: " + transform.skY +
                  ", scX: " + transform.scX + ", scY: " + transform.scY +
                  ", duration: " + duration);
    }
}
