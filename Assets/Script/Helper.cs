using System;
using UnityEngine;
using UnityEngine.UI;

public class Helper : MonoBehaviour
{
    [Header("Helper Mode")]
    public Text equationTextSource;
    public Text equalsTextSource;
    [Header("Panel Animation")]
    public Vector2 panelMinMax_Pos; // -183.6, 582
    public float animationDuration = 5;
    private float animTime, time_normalized;
    private RectTransform rectTransform;
    internal bool calculator = false, changeMode;
    private string output;
    private int process;
    private float lastResult;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
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
    }

    internal void PressKey(string key)
    {
        float number;
        if (float.TryParse(key, out number))
        {
            if (process == 1) process = 2;
            output += number + "";
        }
        else if (key != ".")
        {
            string lastChar = "";
            try { lastChar = output.Length > 0 ? output[output.Length - 1] + "" : ""; }
            catch { }
            if (key == "C")
            {
                output = "";
                process = 0;
                equalsTextSource.text = "";
            }
            else if (lastChar == "+" || lastChar == "-" || lastChar == "*" || lastChar == "/")
            {
                output = output.Remove(output.Length - 1);
                process = 0;
            }
            if (output.Length > 0 && lastChar != key && process != 2)
            {
                output += key;
                process = 1;
            }
            else if (process == 2)
            {
                output = lastResult + key;
                process = 1;
            }
        }
        else if (output.Length > 0 && output[output.Length - 1] != '.') output += ".";
        if (process == 2)
        {
            try
            {
                if (output.Contains("+")) lastResult = float.Parse(output.Split('+')[0]) + float.Parse(output.Split('+')[1]);
                else if (output.Contains("-")) lastResult = float.Parse(output.Split('-')[0]) - float.Parse(output.Split('-')[1]);
                else if (output.Contains("*")) lastResult = float.Parse(output.Split('*')[0]) * float.Parse(output.Split('*')[1]);
                else if (output.Contains("/")) lastResult = float.Parse(output.Split('/')[0]) / float.Parse(output.Split('/')[1]);
            }
            catch (FormatException) { output = output.Remove(output.Length - 1); }
            catch { }
            equalsTextSource.text = "= " + (int)lastResult;
        }
        else if (process == 0 && output.Split('.').Length > 2) output = output.Remove(output.Length - 1);
        equationTextSource.text = output;
    }

    public void HelperDisplay()
    {
        calculator = !calculator;
        changeMode = true;
        animTime = 0;
    }
}