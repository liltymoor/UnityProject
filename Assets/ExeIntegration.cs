using System.Diagnostics;
using System.Threading;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class ExeIntegration : MonoBehaviour
{
    private static List<string> localResults;
    static public void ExeIntegrate(
        string                  exePath,
        string[]                args,
        Action<List<string>>    endFunction)
    {

        //Setting up process and local properties
        localResults  =      new List<string>(){};
        Process proc  =      new Process
        {
            StartInfo =
            {
                FileName = exePath,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                UseShellExecute = false
            }
        };

        //Setting up process arguments
        foreach (string arg in args)
            proc.StartInfo.Arguments += arg + " ";

        //Setting up output handle event
        using (AutoResetEvent outputWaitHandle = new AutoResetEvent(false))
        {
            proc.OutputDataReceived += (sender,e) => {
                if (e.Data == null)
                    outputWaitHandle.Set();
                else
                    localResults.Add(e.Data);
            };
        }

        //Starting the process and waiting for an answer
        proc.Start();
        SessionHandler(proc, endFunction);
    }

    private static async void SessionHandler(
        Process                 proc,
        Action<List<string>>    endFunction)
    {
        //Starting the output handler
        proc.BeginOutputReadLine();

        //Waiting till its done
        while(!proc.WaitForExit(10))
            await Task.Yield();

        //Running passed end function and after that deleting results
        endFunction(localResults);
        localResults = null;
    }


    //
    //
    //

    static public void ExeIntegrate<T>(
        string                  exePath,
        string[]                args,
        Action<T>               endFunction,
        Func<List<string>,T>    castToTemplate)
    {

        //Setting up process and local properties
        localResults  =      new List<string>(){};
        Process proc  =      new Process
        {
            StartInfo =
            {
                FileName = exePath,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                UseShellExecute = false
            }
        };

        //Setting up process arguments
        foreach (string arg in args)
            proc.StartInfo.Arguments += arg + " ";

        //Setting up output handle event
        using (AutoResetEvent outputWaitHandle = new AutoResetEvent(false))
        {
            proc.OutputDataReceived += (sender,e) => {
                if (e.Data == null)
                    outputWaitHandle.Set();
                else
                    localResults.Add(e.Data);
            };
        }

        //Starting the process and waiting for an answer
        proc.Start();
        SessionHandler<T>(proc, endFunction, castToTemplate);
    }

    private static async void SessionHandler<T>(
        Process                 proc,
        Action<T>               endFunction,
        Func<List<string>,T>    castToTemplate)
    {
        //Starting the output handler
        proc.BeginOutputReadLine();

        //Waiting till its done
        while(!proc.WaitForExit(10))
            await Task.Yield();

        //Running passed end function and after that deleting results
        endFunction(castToTemplate(localResults));
        localResults = null;
    }

}
