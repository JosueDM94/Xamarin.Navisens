using System;
using Foundation;
using MotionDnaApi;

namespace MotionDnaSample.iOS
{
    public class MotionDnaManager : MotionDnaSDK
    {
        public ViewController receiver;

        public override void ReceiveMotionDna(MotionDna motionDna)
        {
            receiver.Receive(motionDna);
        }

        public override void ReportError(ErrorCode error, string message)
        {
            switch(error)
            {
                case ErrorCode.SensorTiming:
                    Console.WriteLine("Error: Sensor Timing " + message);
                    break;
                case ErrorCode.AuthenticationFailed:
                    Console.WriteLine("Error: Authentication Failed " + message);
                    break;
                case ErrorCode.SensorMissing:
                    Console.WriteLine("Error: Sensor Missing " + message);
                    break;
                case ErrorCode.SdkExpired:
                    Console.WriteLine("Error: SDK Expired " + message);
                    break;
                case ErrorCode.WrongFloorInput:
                    Console.WriteLine("Error: Wrong Floor Input " + message);
                    break;
                default:
                    Console.WriteLine("Error: Unknown Cause");
                    break;
            }
        }

        public override void ReceiveNetworkData(MotionDna motionDna)
        {
            receiver?.ReceiveNetworkData(motionDna);
        }

        public override void ReceiveNetworkData(NetworkCode opcode, NSDictionary payload)
        {
            receiver?.ReceiveNetworkData(opcode, payload);
        }
    }
}