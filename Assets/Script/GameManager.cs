
//Developed by Halil Emre Yildiz - @Jahn_Star
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

[RequireComponent(typeof(VideoPlayer)), RequireComponent(typeof(AudioSource)), RequireComponent(typeof(Image))]
public class GameManager : MonoBehaviour
{
    [Header("Background Video")]
    public VideoClip videoClip;
    public float playbackSpeed = 1f, endOffset = 1.25f;
    [Header("Result Video")]
    public VideoClip _videoClip;
    public float _playbackSpeed = 1f, _endOffset = 1.25f;
    [Space]
    public AudioClip[] audioClips;
    private VideoPlayer videoPlayer;
    private AudioSource audioSource;
    [Header("GUI")]
    public Text audioTextSource;
    public Text resultTextSource;
    public GameObject[] settingPanel;
    [Header("Game 1")]
    public GameObject gameUI;
    public GameObject playButton;
    private bool showResult;
    private static Text _textAnim;
    private static float _textAnim_time, _textAnim_speed;
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
        videoPlayer.clip = videoClip ? videoClip : null;
        videoPlayer.Play();
        //
        audioSource = gameObject.GetComponent<AudioSource>() ? gameObject.GetComponent<AudioSource>() : gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = audioSource.loop = true;
        ChangeAudio();
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
    public void MuteAudio()
    {
        audioSource.mute = !audioSource.mute;
    }
    public void ChangeAudio()
    {
        audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
        audioTextSource.text = audioSource.clip.name;
        audioSource.Play();
    }
    public void SettingsDisplay(string show_or_hide = "value")
    {
        if (show_or_hide == "true") foreach (GameObject button in settingPanel) button.SetActive(true);
        else if (show_or_hide == "false") foreach (GameObject button in settingPanel) button.SetActive(false);
        else foreach (GameObject button in settingPanel) button.SetActive(!button.activeInHierarchy);
    }
    public void ShowResults(string yourNumber)
    {
        showResult = true;
        gameUI.SetActive(false);
        videoPlayer.clip = _videoClip;
        videoPlayer.playbackSpeed = _playbackSpeed;
        resultTextSource.text = yourNumber;
    }
    public static void TextAnimation(Text textSource, float speed)
    {
        if (_textAnim != textSource)
        {
            _textAnim = textSource;
            _textAnim_speed = speed;
            _textAnim_time = 0;
        }
    }
    void Update()
    {
        if (showResult)
        {
            if (videoPlayer.time >= videoPlayer.clip.length - _endOffset)
            {
                showResult = false;
                resultTextSource.gameObject.SetActive(false);
                gameUI.SetActive(true);
                videoPlayer.clip = videoClip;
                videoPlayer.playbackSpeed = playbackSpeed;
                videoPlayer.Stop();
                videoPlayer.Play();
                resultTextSource.text = "";
                Exit();
            }
            else if (videoPlayer.time >= 2.6f && !resultTextSource.gameObject.activeInHierarchy)
            {
                resultTextSource.gameObject.SetActive(true);
                TextAnimation(resultTextSource, 0.4f);
            }
        }
        else if (videoPlayer.time >= videoClip.length - endOffset)
        {
            videoPlayer.Stop();
            videoPlayer.Play();
        }
        if (audioSource.time > audioSource.clip.length - 0.1f) ChangeAudio();
        if (_textAnim)
        {
            Color temp_color = _textAnim.color;
            temp_color.a = Mathf.Clamp(Mathf.Sin(_textAnim_time += Time.deltaTime * 1.5f), 0, 1);
            _textAnim.color = temp_color;
            if (temp_color.a > 0.9f) _textAnim = null;
        }
    }
}