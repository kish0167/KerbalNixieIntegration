using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace KerbalNixieIntegration;


[KSPAddon(KSPAddon.Startup.MainMenu, false)]
public class In12BService : MonoBehaviour
{
    public static In12BService Instance => _instance;

    private static In12BService _instance;
    private readonly BteSerialClient _bte = new();
    private readonly IdleAnimationProvider _animationProvider = new();
    private IFormattedStringProvider _currentProvider = null;

    public void SetProvider(IFormattedStringProvider provider)
    {
        _currentProvider = provider;
    }
    
    public void ResetProvider()
    {
        _currentProvider = _animationProvider;
    }

    private void Start()
    {
        DontDestroyOnLoad(this);
        
        if (_instance != null)
        {
            Destroy(_instance);
        }
        
        _instance = this;
        _currentProvider = _animationProvider;
        Task.Run(_bte.Connect);

        StartCoroutine(MainCycle());
    }

    private IEnumerator MainCycle()
    {
        while (true)
        {
            if (_currentProvider == null)
            {
                ResetProvider();
                yield return new WaitForSeconds(1.00f);
            }

            Task.Run(() => _bte.SendStringAsync(_currentProvider?.GetValueString()));
            //_bte.SendString(_currentProvider?.GetValueString());
            yield return new WaitForSeconds(1.00f);
        }
    }
    
    private void UpdateT()
    {  
        if (_currentProvider == null)
        {
            ResetProvider();
            return;
        }
            
        _bte.SendString(_currentProvider.GetValueString() ?? "0100011100end.\n");
    }
}