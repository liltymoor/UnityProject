using UnityEngine;
using System.Collections.Generic;



public class ParseToInteger : IExeIntegrationOutputCaster<int>
{
    public ParseToInteger() { }
    public int OutputCast(List<string> args)
    {
        return System.Int32.Parse(args[0]);
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

        ExeIntegration.ExeIntegrate<int,ParseToInteger>(
            Application.dataPath + "/Resources/C++/calculator.exe",
            new string[]
            {
                "!" + operation.options[operation.value].text,                      // operation
                value1.GetParsedText().Remove(value1.GetParsedText().Length - 1),   // left num
                value2.GetParsedText().Remove(value2.GetParsedText().Length - 1)    // right num
            },
            UpdateUI, new ParseToInteger());
    }

    private void UpdateUI(int obj)
    {
        returnObject.text = obj.ToString();
    }
}
