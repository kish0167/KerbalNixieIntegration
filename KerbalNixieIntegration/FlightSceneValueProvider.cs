using System;
using System.Globalization;
using UnityEngine;
using KSP.UI.Screens.Flight;

namespace KerbalNixieIntegration
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class FlightSceneValueProvider : MonoBehaviour , IFormattedStringProvider
    {
        private void Start()
        {
            In12BService.Instance.SetProvider(this);
        }

        private void OnDestroy()
        {
            In12BService.Instance.ResetProvider();
        }

        public string GetValueString()
        {
            double number = GetCurrentVesselSpeedInCurrentMode();
            
            if (number < 0)
            {
                return "2222222200end.\n";
            }
            
            string str = "end.\n";
            string numberString = number.ToString(CultureInfo.InvariantCulture);
            string[] numberSplit = numberString.Split('.');
            string integer = FormatIntegerString(numberSplit[0]);
            string fraction = FormatFractionString(numberSplit.Length > 1 ? numberSplit[1] : "0");
            
            
            if (number > 1d)
            {
                str = integer + "-" + fraction.Substring(0,2) + "04" + str;
            }
            else
            {
                str ="-" + fraction.Substring(0,7) + "80" + str;
            }
            
            return str;
        }

        private string FormatIntegerString(string integer)
        {
            while (integer.Length < 5)
            {
                integer = "-" + integer;
            }

            if (integer.Length > 5)
            {
                integer = integer.Substring(integer.Length - 5);
            }
            
            return integer;
        }

        private static string FormatFractionString(string fraction)
        {
            if (fraction.Length < 7)
            {
                fraction += "000000";
            }
            
            return fraction;
        }
        
        private double GetCurrentVesselSpeedInCurrentMode()
        {
            Vessel activeVessel = FlightGlobals.ActiveVessel;
            
            if (activeVessel == null)
            {
                return -1f;
            }

            double value = -1f;

            switch (FlightGlobals.speedDisplayMode)
            {
                case FlightGlobals.SpeedDisplayModes.Orbit:
                {
                    value = activeVessel.obt_speed;
                    break;
                }
                case FlightGlobals.SpeedDisplayModes.Surface:
                {
                    value = activeVessel.srfSpeed;
                    break;
                }
                case FlightGlobals.SpeedDisplayModes.Target:
                {
                    ITargetable target = FlightGlobals.fetch.VesselTarget;
                    
                    if (target != null)
                    {
                        value = (activeVessel.obt_velocity - target.GetObtVelocity()).magnitude;
                    }
                    
                    break;
                }
            }

            return value;
        }

        public void Init()
        {
            
        }
        
    }
}