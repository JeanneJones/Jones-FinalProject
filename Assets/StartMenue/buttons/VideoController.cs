using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public Button nextSceneButton;
    public Button skipButton; // Add a new button for skipping

    private CanvasGroup buttonCanvasGroup;

    void Start()
    {
        // Ensure the VideoPlayer and Buttons are properly assigned in the Unity Editor
        if (videoPlayer == null || nextSceneButton == null || skipButton == null)
        {
            Debug.LogError("VideoPlayer, Next Scene Button, or Skip Button is not assigned.");
            return;
        }

        // Get or add the CanvasGroup component to the buttons
        buttonCanvasGroup = nextSceneButton.GetComponent<CanvasGroup>();
        if (buttonCanvasGroup == null)
        {
            buttonCanvasGroup = nextSceneButton.gameObject.AddComponent<CanvasGroup>();
        }

        // Set the alpha to 0 initially to make the buttons invisible
        buttonCanvasGroup.alpha = 0;

        // Subscribe to the videoPlayer's loopPointReached event
        videoPlayer.loopPointReached += OnVideoEnd;

        // Add a listener to the skipButton to call SkipToEnd method when clicked
        skipButton.onClick.AddListener(SkipToEnd);
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        // Video playback has reached the end
        // Make the buttons visible when the video finishes
        StartCoroutine(FadeInButtons());
    }

    IEnumerator FadeInButtons()
    {
        while (buttonCanvasGroup.alpha < 1)
        {
            // Gradually increase the alpha value over time
            buttonCanvasGroup.alpha += Time.deltaTime / 2f;
            yield return null;
        }

        // Ensure the alpha is exactly 1
        buttonCanvasGroup.alpha = 1;

        // Make the buttons interactable if needed
        nextSceneButton.interactable = true;
        skipButton.interactable = true;
    }

    public void LoadNextScene()
    {
        // Add your code here to load the next scene
        SceneManager.LoadScene("YourNextSceneName");
    }

    public void SkipToEnd()
    {
        // Skip to the end of the video
        videoPlayer.frame = (long)videoPlayer.frameCount;
    }
}
