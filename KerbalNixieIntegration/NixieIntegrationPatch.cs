using System;
using System.Threading;
using UnityEngine;

namespace KerbalNixieIntegration
{
    [KSPAddon(KSPAddon.Startup.MainMenu, false)]
    public class NixieIntegrationPatch : MonoBehaviour
    {
        private void Start()
        {
            In12BService.Instantiate();
            _ = In12BService.Instance.Run(new CancellationToken());
        }
    }
}