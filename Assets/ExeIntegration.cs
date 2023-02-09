using System.Diagnostics;
using System.Threading;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;


public interface IExeIntegrationOutputCaster<T>
{
    public abstract T OutputCast (List<string> args);
}


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

    static public void ExeIntegrate<TFinalType, TParser>(
        string                  exePath,
        string[]                args,
        Action<TFinalType>      endFunction,
        TParser                 castObject) where TParser : IExeIntegrationOutputCaster<TFinalType>
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
        SessionHandler<TFinalType,TParser>(proc, endFunction, castObject);
    }

    private static async void SessionHandler<TFinalType, TParser>(
        Process                 proc,
        Action<TFinalType>      endFunction,
        TParser                 castObject) where TParser : IExeIntegrationOutputCaster<TFinalType>
    {
        //Starting the output handler
        proc.BeginOutputReadLine();

        //Waiting till its done
        while(!proc.WaitForExit(10))
            await Task.Yield();

        //Running passed end function and after that deleting results
        endFunction(castObject.OutputCast(localResults));
        localResults = null;
    }

}
