using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;


namespace Bakery.NPC
{
    [InitializeOnLoad]
    static class MyStartupCode
    {
        static AddRequest _request;
        static readonly Queue<string> _packageToInstall = new();
        static readonly string[] dependencies = new string[]
        {
            "https://github.com/KyleBanks/scene-ref-attribute.git",
            "https://github.com/starikcetin/Eflatun.SceneReference.git#upm"
        };

        static MyStartupCode()
        {
            if (SessionState.GetBool("FirstInitDone", true)) return;


            foreach (string packageName in dependencies)
            {
                _packageToInstall.Enqueue(packageName);
            }

            if (_packageToInstall.Count == 0) return;
            _request = Client.Add(_packageToInstall.Dequeue());
            EditorApplication.update += ProgressRequest;
        }

        private static void ProgressRequest()
        {
            if (_request.IsCompleted)
            {
                if (_request.Status == StatusCode.Success)
                {
                    Debug.Log("Installed " + _request.Result.packageId);
                }
                else if (_request.Status >= StatusCode.Failure)
                {
                    Debug.Log("Failed to install " + _request.Result.packageId);
                }

                if (_packageToInstall.Count > 0)
                {
                    _request = Client.Add(_packageToInstall.Dequeue());
                }
                else
                {
                    EditorApplication.update -= ProgressRequest;
                    SessionState.SetBool("FirstInitDone", true);
                }
            }
        }
    }

}