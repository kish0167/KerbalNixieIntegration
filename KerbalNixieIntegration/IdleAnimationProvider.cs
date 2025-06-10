using System;

namespace KerbalNixieIntegration;

public class IdleAnimationProvider : IFormattedStringProvider
{
    private readonly string[] _anim =
    [
        "1-------",
        "-1------",
        "--1-----",
        "---1----",
        "----1---",
        "-----1--",
        "------1-",
        "-------1",
        "------1-",
        "-----1--",
        "----1---",
        "---1----",
        "--1-----",
        "-1------"
    ];

    private int _counter = 0;
    
    public void Init()
    {
        
    }

    public string GetValueString()
    {
        _counter++;
        
        if (_counter > 13)
        {
            _counter = 0;
        }
        
        return _anim[_counter] + "00end.\n";
    }
}