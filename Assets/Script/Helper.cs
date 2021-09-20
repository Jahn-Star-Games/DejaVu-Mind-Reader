
//Developed by Halil Emre Yildiz - @Jahn_Star
using UnityEngine;
using UnityEngine.UI;

public class Helper : MonoBehaviour
{
    [Header("Calculator")]
    public Text equationTextSource;
    public Text equalsTextSource;
    [Header("Animation")]
    public Animator animator;
    private static string split = ":";
    private bool expand = false;
    private string output, _firstNumber, _operator, _secondNumber;
    private double _equals;
    private void OnEnable()
    {
        output = _firstNumber = _operator = _secondNumber = "";
        _equals = 0;
    }
    public void ExpandCalculator(int value = -1)
    {
        expand = value == -1 ? !expand : value <= 0 ? false : true;
        animator.SetBool("expand", expand);
    }
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