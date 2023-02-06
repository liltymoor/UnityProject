using UnityEngine;
using System.Collections.Generic;


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
                "!" + operation.options[operation.value].text,
                value1.GetParsedText().Remove(value1.GetParsedText().Length - 1),
                value2.GetParsedText().Remove(value2.GetParsedText().Length - 1)
            },
            UpdateUI);
    }

    private void UpdateUI(List<string> result)
    {
        returnObject.text = result[0];
    }
}
