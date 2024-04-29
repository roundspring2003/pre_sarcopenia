using System;
using CoolSoCoreLibrary;
using System.Threading;
using pre_sarcopenia.sln;
using System.Collections.ObjectModel;
namespace pre_sarcopenia.sln
{
    public class Coolso
    {
        private CoolSoDevice[] devices_Array;
        private CoolSoConnection coolSoConnectionDevices;
        
        public Coolso()
        {
            while (true)
            {
                CoolSoApi.RefreshDeviceTableBlocking();
                Thread.Sleep(2000);
                devices_Array = CoolSoApi.DeviceTable;
                if (devices_Array.Length != 0)
                    break;
            }
            coolSoConnectionDevices = CoolSoApi.Connect(devices_Array[0]);
        }
        public void activate_coolso()
        {
            coolSoConnectionDevices.Activate(SubjectHand.Right, SampleParameter.KijinMode);
        }
        public void exit_coolso()
        {
            coolSoConnectionDevices.Deactivate();
        }
        public double get_VerticalRate_data()
        {
            double VerticalRate = coolSoConnectionDevices.Gesture.VerticalRate;
            return VerticalRate;
        }
        public double get_HorizontalRate_data()
        {
            double HorizontalRate = coolSoConnectionDevices.Gesture.HorizontalRate;
            return HorizontalRate;
        }
        public double get_RotationRate_data()
        {
            double RotationRate = coolSoConnectionDevices.Gesture.RotationRate;
            return RotationRate;
        }
        public double get_DuctionRate_data()
        {
            double DuctionRate = coolSoConnectionDevices.Gesture.DuctionRate;
            return DuctionRate;
        }
        public double get_FlexionRate_data()
        {
            double FlexionRate = coolSoConnectionDevices.Gesture.FlexionRate;
            return FlexionRate;
        }
        public double get_StrengthAmplitude_data()
        {
            double StrengthAmplitude = coolSoConnectionDevices.Gesture.StrengthAmplitude;
            return StrengthAmplitude;
        }
    }
   
}

