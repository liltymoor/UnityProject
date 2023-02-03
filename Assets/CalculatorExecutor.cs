using UnityEngine;
using System.Diagnostics;
using System.Threading;
using System;
using System.Threading.Tasks;

public class CalculatorExecutor : MonoBehaviour
{
    public TMPro.TextMeshProUGUI returnObject;

    public TMPro.TextMeshProUGUI value1;
    public TMPro.TextMeshProUGUI value2;
    public TMPro.TMP_Dropdown operation;


    private string returned = String.Empty;

    private Process proc;
    public void ExeCalculate()
    {
        if (value1.text == String.Empty || value2.text == String.Empty)
            return;

        proc = new Process();

        //Setting up .exe path and passing arguments to it
        //proc.StartInfo.FileName = Application.dataPath + "/Resources/C++/calculator.exe";
        proc.StartInfo.FileName = Application.dataPath + "/Resources/C++/5sec.exe";
        proc.StartInfo.Arguments +=
            "!" + operation.options[operation.value].text + " "                      // operation
            + value1.GetParsedText().Remove(value1.GetParsedText().Length - 1) + " " // value #1 (Removing null-terminator and passing original value)
            + value2.GetParsedText().Remove(value2.GetParsedText().Length - 1) ;     // value #2 (Removing null-terminator and passing original value)

        UnityEngine.Debug.Log("Passed arguments: [ " + proc.StartInfo.Arguments + " ] ");

        //Setting up process properties
        proc.StartInfo.CreateNoWindow = true;
        proc.StartInfo.RedirectStandardOutput = true;
        proc.StartInfo.UseShellExecute = false;

        //std::cout Reciever | handles console output
        using (AutoResetEvent outputWaitHandle = new AutoResetEvent(false))
        {
            proc.OutputDataReceived += (sender,e) => {
                if (e.Data == null)
                    outputWaitHandle.Set();
                else
                {
                    UnityEngine.Debug.Log("Returned output: [ " + e.Data.ToString() + " ]");
                    returned = e.Data.ToString();
                }
            };
        }

        proc.Start();
        CalculationHandler();
    }

    private async void CalculationHandler()
    {
        proc.BeginOutputReadLine();
        while(returned == String.Empty)
            await Task.Yield();

        returnObject.text = returned;
    }
}
