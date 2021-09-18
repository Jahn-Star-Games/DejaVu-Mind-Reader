
//Developed by Halil Emre Yildiz - @Jahn_Star
using UnityEngine;
using UnityEngine.UI;

public class Helper : MonoBehaviour
{
    [Header("GUI")]
    public Text equationTextSource;
    public Text equalsTextSource;
    public ScrollRect doorsScrollRect;
    [Header("Panel Animation")]
    public Vector2 panelMinMax_Pos; // -183.6, 582
    public float animationDuration = 5;
    private float animTime, time_normalized;
    private RectTransform rectTransform;
    internal bool calculator = false, changeMode;
    private string output, _firstNumber, _operator, _secondNumber;
    private double _equals;
    private static string split = ":";
    internal float scroll, scrollTarget;
    private void OnEnable()
    {
        rectTransform = GetComponent<RectTransform>();
        //
        output = _firstNumber = _operator = _secondNumber = "";
        _equals = 0;
    }
    void Update()
    {
        if (changeMode && calculator)
        {
            animTime += Time.deltaTime;
            time_normalized = animTime / animationDuration;
            rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, new Vector2(0, panelMinMax_Pos.x), time_normalized);
            if (rectTransform.anchoredPosition.y - panelMinMax_Pos.x < 0.25f) changeMode = false;
        }
        else if (changeMode)
        {
            animTime += Time.deltaTime;
            time_normalized = animTime / animationDuration;
            rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, new Vector2(0, panelMinMax_Pos.y), time_normalized);
            if (panelMinMax_Pos.y - rectTransform.anchoredPosition.y < 0.25f) changeMode = false;
        }
        //
        if (scroll != scrollTarget)
        {
            if (scroll > scrollTarget)
            {
                scroll -= Time.deltaTime * 0.8f;
                if (scroll < scrollTarget) scroll = scrollTarget;
            }
            else
            {
                scroll += Time.deltaTime * 0.4f;
                if (scroll > scrollTarget) scroll = scrollTarget;
            }
            doorsScrollRect.normalizedPosition = new Vector2(0, scroll);
        }
    }
    public void HelperDisplay()
    {
        calculator = !calculator;
        changeMode = true;
        animTime = 0;
    }
    public void Scroll(RectTransform scrollButton)
    {
        if (scrollButton) scrollButton.eulerAngles = new Vector3(scrollButton.eulerAngles.x, scrollButton.eulerAngles.y, scrollButton.eulerAngles.z + 180);
        scroll = doorsScrollRect.normalizedPosition.y;
        scrollTarget = scrollTarget == 0 ? 1 : 0;
    }
    // Calculator
    public void PressKey(string character)
    {
        if (character == "C") output = ""; // clean
        else if (char.IsDigit(character[0]) || character == ",") // is a digit
        {
            string first = "", second = "";
            //
            if (!output.Contains(split)) first = output;
            else second = output.Split(split[0])[1];
            //
            string part = first + second;
            if (character != "," || (!part.Contains(",") && part.Length > 0)) output += character;
        }
        else if (character == "<") output = output.Length > 0 ? output.Remove(output.Length - 1) : ""; // backspace
        else if (!output.Contains(split))  // is a operator
        {
            _operator = character;
            output += split;
        }
        else // new process
        {
            if (_secondNumber != "") _firstNumber = _equals.ToString();
            _operator = character;
            output = _firstNumber + split;
        }
        //
        if (!output.Contains(split)) _firstNumber = output;
        else _secondNumber = output.Split(split[0])[1];
        _equals = Calculate(_firstNumber, _secondNumber, _operator);
        //
        equationTextSource.text = output.Replace(split, _operator);
        equalsTextSource.text = _equals + "" == double.NaN + "" ? "" : "= " + _equals.ToString("0.###");
    }
    public static double Calculate(string firstNumber, string secondNumber, string op)
    {
        double n1, n2;
        if (!double.TryParse(firstNumber, out n1) || !double.TryParse(secondNumber, out n2)) return double.NaN;
        switch (op)
        {
            case "+": return n1 + n2;
            case "-": return n1 - n2;
            case "*": return n1 * n2;
            case "/": return n2 != 0 ? n1 / n2 : double.NaN;
            default: return double.NaN;
        }
    }
}