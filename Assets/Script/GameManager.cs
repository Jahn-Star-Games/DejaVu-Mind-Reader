//Developed by Halil Emre Yildiz - @Jahn_Star
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
[RequireComponent(typeof(Image))]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public VideoPlayer videoPlayer;
    public AudioSource audioSource;
    [Header("Resources")]
    public VideoClip backgroundVideoClip;
    public VideoClip resultVideoClip;
    public AudioClip[] audioClips;
    private int prevAudio = -1;
    [Header("Game")]
    public GameObject gameUI;
    public GameObject playButton;
    private bool showResult;
    private static Text _textAnim;
    private static float _textAnim_time, _textAnim_speed;
    [Header("GUI")]
    public Helper gameHelper;
    public ScrollRect doorsScrollRect;
    internal float scroll = 1, scrollTarget = 1;
    public Text resultTextSource;
    public Text stepsTextSource, audioTextSource;
    public GameObject[] settingPanel;
    [Space]
    public Text _hint;
    public Text _playButton, _doorsTitle;
    public Button trButton;
    public Button enButton;
    internal string lang;
    public void Scroll(RectTransform scrollButton)
    {
        if (scrollButton) scrollButton.eulerAngles = new Vector3(scrollButton.eulerAngles.x, scrollButton.eulerAngles.y, scrollButton.eulerAngles.z + 180);
        scroll = doorsScrollRect.normalizedPosition.y;
        scrollTarget = scrollTarget == 0 ? 1 : 0;
    }
    private void LateUpdate()
    {
        if (scroll != scrollTarget)
        {
            if (scroll > scrollTarget)
            {
                scroll -= Time.deltaTime * 1f;
                if (scroll < scrollTarget) scroll = scrollTarget;
            }
            else
            {
                scroll += Time.deltaTime * 0.75f;
                if (scroll > scrollTarget) scroll = scrollTarget;
            }
            doorsScrollRect.normalizedPosition = new Vector2(0, scroll);
        }
    }
    // Game
    private void Awake()
    {
        if (!Instance) Instance = this;
        videoPlayer.timeReference = VideoTimeReference.InternalTime;
        videoPlayer.Play();
        Screen.orientation = ScreenOrientation.Portrait;
        Screen.SetResolution(Screen.width, Screen.height, true, 60);
        ChangeLanguage(Application.systemLanguage == SystemLanguage.Turkish ? "tr" : "en");
        //
        ChangeAudio();
        gameUI.SetActive(false);
        resultTextSource.gameObject.SetActive(false);
        //
        SettingsDisplay("false");
    }
    public void OpenURL(string url)
    {
        Application.OpenURL(url);
    }
    public void ChangeAudio()
    {
        int currentAudio;
        do { currentAudio = Random.Range(0, audioClips.Length); }
        while (prevAudio == currentAudio);
        prevAudio = currentAudio;
        audioSource.clip = audioClips[currentAudio];
        audioTextSource.text = audioSource.clip.name;
        audioSource.Play();
    }
    public void MuteAudio()
    {
        audioSource.mute = !audioSource.mute;
    }
    public void ChangeLanguage(string language)
    {
        this.lang = language;
        trButton.enabled = language == "tr" ? true : false;
        trButton.image.enabled = trButton.enabled;
        enButton.enabled = language == "tr" ? false : true;
        enButton.image.enabled = enButton.enabled;
        _hint.text = language == "tr" ? "Not: Hesap makinesini kullanmak zorunda değilsin. Sonraki adımı görmek için '>'a tıkla" : "Note: You can use the calculator if you want. Click '>' to see next step.";
        _playButton.text = language == "tr" ? "OYNA" : "PLAY";
        _doorsTitle.text = language == "tr" ? "Sihirli Kapılar" : "Magic Doors";
        Restart();
    }
    public void SettingsDisplay(string show_or_hide)
    {
        if (show_or_hide == "true") foreach (GameObject button in settingPanel) button.SetActive(true);
        else if (show_or_hide == "false") foreach (GameObject button in settingPanel) button.SetActive(false);
        else foreach (GameObject button in settingPanel) button.SetActive(!button.activeInHierarchy);
    }
    public static void TextAnimation(Text textSource, float speed = 1f)
    {
        if (_textAnim != textSource)
        {
            _textAnim = textSource;
            _textAnim_speed = speed;
            _textAnim_time = 0;
        }
    }
    public void Restart()
    {
        Color _color = stepsTextSource.color;
        _color.a = 0;
        stepsTextSource.color = _color;
        resultTextSource.text = stepsTextSource.text = "";
        //
        GetComponent<Image>().enabled = false;
        playButton.SetActive(true);
        gameUI.SetActive(false);
        gameHelper.ExpandCalculator(0);
    }
    public void Play()
    {
        GameManager.TextAnimation(stepsTextSource, 1f);
        GetComponent<Image>().enabled = true;
        playButton.SetActive(false);
        gameUI.SetActive(true);
        gameHelper.ExpandCalculator(1);
        SettingsDisplay("false");
    }
    public void ShowResults(string yourNumber)
    {
        GetComponent<Image>().enabled = false;
        showResult = true;
        gameUI.SetActive(false);
        videoPlayer.clip = resultVideoClip;
        resultTextSource.text = yourNumber;
    }
    private bool firstGame;
    private void Update()
    {
        if (showResult)
        {
            if (videoPlayer.time >= videoPlayer.clip.length - 2f)
            {
                showResult = false;
                resultTextSource.gameObject.SetActive(false);
                //
                videoPlayer.clip = backgroundVideoClip;
                videoPlayer.Play();
                if (!firstGame)
                {
                    SettingsDisplay("true");
                    firstGame = true;
                }
                Restart();
            }
            else if (videoPlayer.time >= 2.6f && !resultTextSource.gameObject.activeInHierarchy)
            {
                resultTextSource.gameObject.SetActive(true);
                TextAnimation(resultTextSource, 0.5f);
            }
        }
        if (audioSource.time > audioSource.clip.length - 0.1f) ChangeAudio();
        if (_textAnim)
        {
            Color temp_color = _textAnim.color;
            temp_color.a = Mathf.Clamp(Mathf.Sin(_textAnim_time += Time.deltaTime * _textAnim_speed), 0, 1);
            _textAnim.color = temp_color;
            if (temp_color.a > 0.9f) _textAnim = null;
        }
    }
}