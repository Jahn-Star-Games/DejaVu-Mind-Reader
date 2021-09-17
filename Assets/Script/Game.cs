using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Game : MonoBehaviour
{
    [Header("Resources")]
    public GameObject doorPrefab;
    public Transform doorsGrid;
    [HideInInspector]
    public string result;
    [Header("Input")]
    public int doorsCount = 99;
    [Header("Output"), Tooltip("Pick a number")]
    public List<float> values;
    public List<string> steps;
    [Header("GUI")]
    public Text stepsTextSource;
    public GameManager background;
    public Helper gameHelper;
    public GameObject gameResults, prevStepButon, nextStepButton;
    private int currentStep;
    private bool createDoors;
    private int multiply, add, divide, subtract, _redivide;
    void Start()
    {
        createDoors = true;
        gameResults.SetActive(false);
        prevStepButon.SetActive(false);
        nextStepButton.SetActive(true);
    }
    private void OnEnable()
    {
        createDoors = true;
        gameResults.SetActive(false);
        prevStepButon.SetActive(false);
        nextStepButton.SetActive(true);
        currentStep = 0;
        gameHelper.PressKey("C");
    }
    void Update()
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

                    _redivide = Random.Range(1, 15);
                    _redivide += _redivide % 2 != 0 ? 1 : 0;

                    firstValue = (int)Reverse(0);
                    lastValue = (int)Reverse(doorsCount);
                    test = (int)Reverse(((((lastValue * 2 * _redivide) / ((_redivide * 2) / (multiply * 2) > 1 ? (_redivide * 2) / (multiply * 2) : 1)) + add) / divide) - subtract);
                }
                while ((_redivide * 2) / (multiply * 2) == 0 || firstValue < 0 || lastValue > doorsCount || divide < 1 || test != lastValue);

                result = "";
                values.Clear();

                int prevValue = 0;
                bool notConsecutive = false;
                for (int i = 0; i <= doorsCount; i++)
                {
                    if (prevValue != (int)Reverse(i) && prevValue + 1 != (int)Reverse(i)) notConsecutive = true;
                    prevValue = (int)Reverse(i);
                    values.Add((int)Reverse(i));
                    result += i.ToString("00") + ": <color=red>" + values[i] + "</color> | ";
                }

                createDoors = notConsecutive;
                if (!notConsecutive) // add steps
                {
                    steps.Clear();
                    steps.Add("Merhaba, seninle bir oyun oynayacağız.");
                    steps.Add("Sana sihirli işlemler yaptırarak aklındaki sayıyı bulacağım.");
                    //steps.Add("İşlemleri aklından yada yukardaki mavi çubuğa tıklayarak hesap makinesinden yapabilirsin.");
                    steps.Add("Aklından " + (firstValue <= 1 ? 1 : firstValue) + " ile " + lastValue + " arasında bir sayı tut...");
                    steps.Add("Kensi ile topla...");
                    steps.Add(_redivide + " ile çarp...");
                    if ((_redivide * 2) / (multiply * 2) > 1) steps.Add((_redivide * 2) / (multiply * 2) + " ile böl..."); // (_redivide * 2) ile bol ve (multiply * 2) ile carp demek
                    steps.Add(add + " ile topla...");
                    steps.Add(divide + " ile böl...");
                    steps.Add(subtract + " ile çıkar...");
                    steps.Add("Sonuc tam sayı değilse tam sayıya yuvarla ve aynı numaralı kapıya tıkla.");

                    stepsTextSource.text = steps[0];
                    CreateDoor();
                }
            }
        }
        catch { Application.Quit(); }
    }
    public void NextStep(bool next)
    {
        if (next)
        {
            if (currentStep + 1 < steps.Count)
            {
                currentStep++;
                stepsTextSource.text = steps[currentStep];
                nextStepButton.SetActive(true);
                prevStepButon.SetActive(true);
                if (currentStep + 1 >= steps.Count) 
                {
                    nextStepButton.SetActive(false);
                    gameHelper.changeMode = gameHelper.calculator = true;
                    gameResults.SetActive(true);
                    //
                    gameHelper.doorsScrollRect.normalizedPosition = new Vector2(0, 0);
                    gameHelper.Scroll(null);
                }
            }
        }
        else if (currentStep - 1 >= 0)
        {
            currentStep--;
            stepsTextSource.text = steps[currentStep];
            prevStepButon.SetActive(true);
            nextStepButton.SetActive(true);

            if (currentStep - 1 < 0) prevStepButon.SetActive(false);
        }
    }
    public void CreateDoor()
    {
        foreach (Transform door in doorsGrid) Destroy(door.gameObject);
        if (values.Count > 0)
            for (int i = 0; i < values.Count; i++)
                Instantiate(doorPrefab, doorsGrid.transform).transform.GetChild(0).GetComponent<Text>().text = i.ToString("00");
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
}