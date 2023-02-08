using UnityEngine;
using System.Collections.Generic;



public class CalcResultObj
{
    string val;
    public CalcResultObj(string value = "")
    {
        val = value;
    }

    public string GetValue()
    {
        return val;
    }
    public static CalcResultObj ParseConsoleOutput(List<string> output)
    {
        return new CalcResultObj(output[0]);
    }
}

public class CalculatorExecutor : MonoBehaviour
{
    public TMPro.TextMeshProUGUI returnObject;

    public TMPro.TextMeshProUGUI value1;
    public TMPro.TextMeshProUGUI value2;
    public TMPro.TMP_Dropdown operation;


    public void ExeCalculate()
    {
        ExeIntegration.ExeIntegrate(
            Application.dataPath + "/Resources/C++/calculator.exe",
            new string[]
            {
                "!" + operation.options[operation.value].text,                      // operation
                value1.GetParsedText().Remove(value1.GetParsedText().Length - 1),   // left num
                value2.GetParsedText().Remove(value2.GetParsedText().Length - 1)    // right num
            },
            UpdateUI,
            CalcResultObj.ParseConsoleOutput);
    }

    private void UpdateUI(CalcResultObj obj)
    {
        returnObject.text = obj.GetValue();
    }
}
