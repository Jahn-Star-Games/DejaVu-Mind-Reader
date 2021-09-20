
//Developed by Halil Emre Yildiz - @Jahn_Star
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Game : MonoBehaviour
{
    [Header("Resources")]
    public GameObject doorPrefab;
    public Transform doorsGrid;
    public int doorsCount = 100;
    [HideInInspector]
    public string result;

    [Header("GUI")]
    public GameObject gameResults;
    public GameObject prevStepButon, nextStepButton;
    public Text stepsTextSource;
    private GameManager background;
    [Header("Output"), Tooltip("Pick a number")]
    public List<float> values;
    public List<string> steps;
    private int currentStep;
    private bool createDoors;
    private int multiply, add, divide, subtract, _divideAndMultiply;
    private void Awake()
    {
        CreateDoor(doorsCount);
        background = GameManager.Instance;
    }
    private void OnEnable()
    {
        gameResults.SetActive(false);
        prevStepButon.SetActive(false);
        nextStepButton.SetActive(true);
        //
        createDoors = true;
        currentStep = 0;
        background.gameHelper.PressKey("C");
    }
    public void CreateDoor(int value)
    {
        foreach (Transform door in doorsGrid) Destroy(door.gameObject);
        if (value > 0)
            for (int i = 0; i < value; i++)
                Instantiate(doorPrefab, doorsGrid.transform).transform.GetChild(0).GetComponent<Text>().text = i.ToString("00");
    }
    public void NextStep(bool next)
    {
        if (next)
        {
            if (currentStep + 1 < steps.Count)
            {
                currentStep++;
                stepsTextSource.text = steps[currentStep];
                GameManager.TextAnimation(stepsTextSource, 1.5f);
                nextStepButton.SetActive(true);
                prevStepButon.SetActive(true);
                if (currentStep + 1 >= steps.Count) 
                {
                    nextStepButton.SetActive(false);
                    gameResults.SetActive(true);
                    background.gameHelper.ExpandCalculator(0);
                    // Scroll to Bottom doorsScrollRect
                    background.scroll = background.scrollTarget = 0;
                    background.doorsScrollRect.normalizedPosition = new Vector2(0, 0);
                    background.Scroll(null);
                }
            }
        }
        else if (currentStep - 1 >= 0)
        {
            currentStep--;
            stepsTextSource.text = steps[currentStep];
            GameManager.TextAnimation(stepsTextSource, 1.5f);
            prevStepButon.SetActive(true);
            nextStepButton.SetActive(true);

            if (currentStep - 1 < 0) prevStepButon.SetActive(false);
        }
    }
    public void AklimdakiniBul(Text door)
    {
        int doorNumber = int.Parse(door.text);
        background.ShowResults((int)Reverse((float)doorNumber) + "");
    }
    private float Reverse(float value) 
    {
        // than => ((((value * 2) * multiply) + add) / divide) - subtract;
        return ((((value + subtract) * divide) - add) / multiply) / 2;
    }
    private void Update()
    {
        try
        {
            if (createDoors)
            {
                int firstValue, lastValue, test;
                do
                {
                    multiply = Random.Range(1, 15);
                    multiply += multiply % 2 != 0 ? 1 : 0;
                    add = Random.Range(1, 100);
                    add += add % 2 != 0 ? 1 : 0;
                    divide = Random.Range(1, 10);
                    divide += divide % 2 != 0 ? 1 : 0;
                    subtract = Random.Range(1, 75);
                    subtract += subtract % 2 != 0 ? 1 : 0;

                    _divideAndMultiply = Random.Range(1, 15);
                    _divideAndMultiply += _divideAndMultiply % 2 != 0 ? 1 : 0;

                    firstValue = (int)Reverse(0);
                    lastValue = (int)Reverse(doorsCount-1);
                    test = (int)Reverse(((((lastValue * 2 * _divideAndMultiply) / ((_divideAndMultiply * 2) / (multiply * 2) > 1 ? (_divideAndMultiply * 2) / (multiply * 2) : 1)) + add) / divide) - subtract);
                }
                while ((_divideAndMultiply * 2) / (multiply * 2) == 0 || firstValue < 0 || lastValue > (doorsCount - 1) || divide < 1 || test != lastValue);

                result = "";
                values.Clear();

                int prevValue = 0;
                bool notConsecutive = false;
                for (int i = 0; i <= (doorsCount-1); i++)
                {
                    if (prevValue != (int)Reverse(i) && prevValue + 1 != (int)Reverse(i)) notConsecutive = true;
                    prevValue = (int)Reverse(i);
                    values.Add((int)Reverse(i));
                    result += i.ToString("00") + ": <color=red>" + values[i] + "</color> | ";
                }

                createDoors = notConsecutive;
                if (!notConsecutive) // add steps
                {
                    string lang = background.lang;
                    steps.Clear();
                    steps.Add(lang == "tr" ? "Hoş geldin,\nseninle bir oyun oynayacağız..." : "Welcome,\nlet's play a game...");
                    steps.Add(lang == "tr" ? "Sana sihirli işlemler yaptırarak aklındaki sayıyı bulacağım." : "I will guess the number your thinking.");
                    steps.Add(lang == "tr" ? "Aklından " + (firstValue <= 1 ? 1 : firstValue) + " ile " + lastValue + " arasında bir sayı tut..." : "Think of a number between " + (firstValue <= 1 ? 1 : firstValue) + " and " + lastValue);
                    steps.Add(lang == "tr" ? "Kendisi ile topla..." : "Double it"); // Multiply by 2 - Add your number again
                    steps.Add(lang == "tr" ? _divideAndMultiply + " ile çarp..." : "Multiply this number by " + _divideAndMultiply);
                    if ((_divideAndMultiply * 2) / (multiply * 2) > 1) steps.Add(lang == "tr" ? (_divideAndMultiply * 2) / (multiply * 2) + " ile böl..." : "Divide by " + (_divideAndMultiply * 2) / (multiply * 2)); // (_redivide * 2) ile bol ve (multiply * 2) ile carp demek
                    steps.Add(lang == "tr" ? add + " ekle..." : "Add " + add);
                    steps.Add(lang == "tr" ? divide + " ile böl..." : "Divide it by " + divide);
                    steps.Add(lang == "tr" ? subtract + " çıkar..." : "Subtract " + subtract + " from total");
                    steps.Add(lang == "tr" ? "İlk tuttuğun sayıyı tahmin etmem için, çıkan sonuç ile aynı numaralı kapıyı aç..." : "Let me guess... Tap the door with the same number as this number...");

                    stepsTextSource.text = steps[0];
                }
            }
        }
        catch { Application.Quit(); }
    }
}