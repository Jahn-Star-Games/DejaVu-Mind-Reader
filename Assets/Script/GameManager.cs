using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

[RequireComponent(typeof(VideoPlayer)), RequireComponent(typeof(AudioSource)), RequireComponent(typeof(Image))]
public class GameManager : MonoBehaviour
{
    [Header("Resources")]
    public VideoClip backgroundVideoClip;
    [Space]
    public VideoClip resultVideoClip;
    public Text resultTextSource;
    [Space]
    public AudioClip audioClip;
    public float audioClipEndOffset = 0.25f;
    private VideoPlayer videoPlayer;
    private AudioSource audioSource;
    [Header("GUI")]
    public GameObject[] settingButtons;
    [Header("Game 1")]
    public GameObject gameUI;
    public GameObject playButton;
    private bool showResult;
    private void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        //
        videoPlayer = gameObject.GetComponent<VideoPlayer>() ? gameObject.GetComponent<VideoPlayer>() : gameObject.AddComponent<VideoPlayer>();
        videoPlayer.renderMode = VideoRenderMode.CameraFarPlane;
        videoPlayer.targetCamera = Camera.main;
        videoPlayer.aspectRatio = VideoAspectRatio.FitOutside;
        videoPlayer.playOnAwake = videoPlayer.isLooping = true;
        videoPlayer.waitForFirstFrame = videoPlayer.skipOnDrop = false;
        videoPlayer.audioOutputMode = VideoAudioOutputMode.None;
        videoPlayer.clip = backgroundVideoClip ? backgroundVideoClip : null;
        videoPlayer.Play();
        //
        audioSource = gameObject.GetComponent<AudioSource>() ? gameObject.GetComponent<AudioSource>() : gameObject.AddComponent<AudioSource>();
        audioSource.clip = audioClip ? audioClip : null;
        audioSource.playOnAwake = audioSource.loop = true;
        audioSource.Play();
        //
        gameUI.SetActive(false);
        resultTextSource.gameObject.SetActive(false);
        //
        SettingsDisplay("false");
    }
    public void Exit()
    {
        playButton.SetActive(true);
        GetComponent<Image>().enabled = false;
        gameUI.SetActive(false);
    }
    public void Play()
    {
        GetComponent<Image>().enabled = true;
        playButton.SetActive(false);
        gameUI.SetActive(true);
    }
    public void MuteMode()
    {
        audioSource.mute = !audioSource.mute;
    }
    public void SettingsDisplay(string show_or_hide = "value")
    {
        if (show_or_hide == "true") foreach (GameObject button in settingButtons) button.SetActive(true);
        else if (show_or_hide == "false") foreach (GameObject button in settingButtons) button.SetActive(false);
        else foreach (GameObject button in settingButtons) button.SetActive(!button.activeInHierarchy);
    }

    void Update()
    {
        if (showResult)
        {
            if (videoPlayer.time >= resultVideoClip.length) 
            {
                showResult = false;
                resultTextSource.gameObject.SetActive(false);
                gameUI.SetActive(true);
                videoPlayer.clip = backgroundVideoClip;
                videoPlayer.Stop();
                videoPlayer.Play();
                resultTextSource.text = "";
            }
            else if (videoPlayer.time >= 2.25f) resultTextSource.gameObject.SetActive(true);
        }
        else if (videoPlayer.time >= backgroundVideoClip.length - audioClipEndOffset)
        {
            videoPlayer.Stop();
            videoPlayer.Play();
        }
    }
    public void ShowResults(string yourNumber)
    {
        showResult = true;
        gameUI.SetActive(false);
        videoPlayer.clip = resultVideoClip;
        resultTextSource.text = yourNumber;
    }
}