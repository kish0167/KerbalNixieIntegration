using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KerbalNixieIntegration;

public class In12BService
{
    public static In12BService Instance => _instance;

    private static In12BService _instance;
    private readonly BteSerialClient _bte = new();
    private readonly IdleAnimationProvider _animationProvider = new();
    private IFormattedStringProvider _currentProvider = null;
    
    public static void Instantiate()
    {
        _instance = new In12BService();
    }
    
    public async Task Run(CancellationToken ct)
    {
        await _bte.Connect();
        _currentProvider = _animationProvider;
        _ = Task.Run(CurrentProviderCycle, ct);
        
        while (!ct.IsCancellationRequested)
        {
            await Task.Delay(12000);
        }
    }

    public void SetProvider(IFormattedStringProvider provider)
    {
        _currentProvider = provider;
    }
    
    public void ResetProvider()
    {
        _currentProvider = _animationProvider;
    }
    
    private async Task CurrentProviderCycle()
    {
        while (true)
        {
            if (_currentProvider == null)
            {
                ResetProvider();
                await Task.Delay(30);
                continue;
            }
            
            _bte.SendString(_currentProvider.GetValueString() ?? "0100011100end.\n");
            await Task.Delay(30);
        }
    }
}