using System.Diagnostics;
using System.Threading;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class ExeIntegration : MonoBehaviour
{
    private static List<string> localResults;
    static public void ExeIntegrate(string exePath, string[] args, Action<List<string>> endFunction)
    {
        Process proc = new Process();
        localResults = new List<string>(){};

        //Setting up .exe path and passing arguments to it
        proc.StartInfo.FileName = exePath;
        foreach (string arg in args)
            proc.StartInfo.Arguments += arg + " ";

        UnityEngine.Debug.Log("[ Exe file path: "+ proc.StartInfo.FileName +" ] | [ " + "Passed args: " + proc.StartInfo.Arguments + " ]");

                //Setting up process properties
        proc.StartInfo.CreateNoWindow = true;
        proc.StartInfo.RedirectStandardOutput = true;
        proc.StartInfo.UseShellExecute = false;

        using (AutoResetEvent outputWaitHandle = new AutoResetEvent(false))
        {
            proc.OutputDataReceived += (sender,e) => {
                if (e.Data == null)
                    outputWaitHandle.Set();
                else
                {
                    localResults.Add(e.Data);
                }
            };
        }

        proc.Start();
        SessionHandler(proc, endFunction);
    }

    private static async void SessionHandler(Process proc, Action<List<string>> endFunction)
    {
        proc.BeginOutputReadLine();
        while(!proc.WaitForExit(10))
            await Task.Yield();

        endFunction(localResults);
        localResults = null;
    }

}
