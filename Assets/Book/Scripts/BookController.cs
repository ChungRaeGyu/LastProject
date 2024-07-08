using UnityEngine;
using UnityEngine.UI;

public class BookController : MonoBehaviour
{
    public Camera bookCamera;
    public Material leftPage;
    public Material rightPage;
    public RenderTexture currentPageTexture;
    public RenderTexture nextPageTexture;
    public PageTurnAnimation nextPageAnimation;
    public PageTurnAnimation previousPageAnimation;
    public GraphicRaycaster[] graphicRaycasters;

    public void NextPage()
    {
        EnableRaycasters(false);

        bookCamera.targetTexture = nextPageTexture;
        rightPage.mainTexture = nextPageTexture;

        nextPageAnimation.leftSideMaterial.mainTexture = currentPageTexture;
        nextPageAnimation.rightSideMaterial.mainTexture = nextPageTexture;
        nextPageAnimation.Play(OnTurnPageComplete);
    }

    public void PreviousPage()
    {
        EnableRaycasters(false);

        bookCamera.targetTexture = nextPageTexture;
        leftPage.mainTexture = nextPageTexture;

        previousPageAnimation.leftSideMaterial.mainTexture = nextPageTexture;
        previousPageAnimation.rightSideMaterial.mainTexture = currentPageTexture;
        previousPageAnimation.Play(OnTurnPageComplete);
    }

    void OnTurnPageComplete()
    {
        EnableRaycasters(true);

        bookCamera.targetTexture = currentPageTexture;
        leftPage.mainTexture = currentPageTexture;
        rightPage.mainTexture = currentPageTexture;
    }

    void EnableRaycasters(bool value)
    {
        foreach (GraphicRaycaster graphicRaycaster in graphicRaycasters)
            graphicRaycaster.enabled = value;
    }

    void Start()
    {
        OnTurnPageComplete();
    }
}