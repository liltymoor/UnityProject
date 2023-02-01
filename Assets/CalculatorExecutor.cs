using UnityEngine;
using System.Diagnostics;
using System.Threading;
using System;

public class CalculatorExecutor : MonoBehaviour
{
    public TMPro.TextMeshProUGUI returnObject;

    public TMPro.TextMeshProUGUI value1;
    public TMPro.TextMeshProUGUI value2;
    public TMPro.TMP_Dropdown operation;

    public void ExeCalculate()
    {
        if (value1.text == String.Empty || value2.text == String.Empty)
            return;

        Process proc = new Process();
        proc.StartInfo.FileName = Application.dataPath + "/Resources/C++/outDebug.exe";
        proc.StartInfo.Arguments +=
            "!" + operation.options[operation.value].text + " "                      // operation
            + value1.GetParsedText().Remove(value1.GetParsedText().Length - 1) + " " // value #1 (Removing null-terminator and passing original value)
            + value2.GetParsedText().Remove(value2.GetParsedText().Length - 1) ;     // value #2 (Removing null-terminator and passing original value)

        UnityEngine.Debug.Log("Passed arguments: [ " + proc.StartInfo.Arguments + " ] ");
        proc.StartInfo.CreateNoWindow = true;
        proc.StartInfo.RedirectStandardOutput = true;
        proc.StartInfo.UseShellExecute = false;

        string returned = String.Empty;
        using (AutoResetEvent outputWaitHandle = new AutoResetEvent(false))
        {
            proc.OutputDataReceived += (sender,e) => {
                if (e.Data == null)
                    outputWaitHandle.Set();
                else
                {
                    UnityEngine.Debug.Log("Returned output: [ " + e.Data.ToString() + " ]");;
                    returned = e.Data.ToString();
                }
            };
        }

        proc.Start();
        proc.WaitForExit();


        proc.BeginOutputReadLine();
        Thread.Sleep(1);

        returnObject.text = returned;
    }
}
