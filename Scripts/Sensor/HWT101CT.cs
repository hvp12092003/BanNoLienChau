using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
namespace IMU
{
    [System.Serializable]
    public class HWT101CT
    {
        private SerialPort serialPort;
        [SerializeField] private string portName = "";
        private bool doClose = false;
        public delegate void HWT101CTDataCallback(object sender, float yawL);
        public const byte START_BYTE = 0x55;
        public const byte DATE_LEN_BYTE = 11;
       // public const int BAUD_RATE = 115200;
        public const int BAUD_RATE = 9600;
        private int witDataCnt = 0;
        private byte[] witDataBuff = new byte[256];
        private Int16[] regBuffer = new Int16[0x90];
        public const byte WIT_TIME = 0x50;
        public const byte WIT_ACC = 0x51;
        public const byte WIT_GYRO = 0x52;
        public const byte WIT_ANGLE = 0x53;
        public const byte WIT_MAGNETIC = 0x54;
        public const byte WIT_DPORT = 0x55;
        public const byte WIT_PRESS = 0x56;
        public const byte WIT_GPS = 0x57;
        public const byte WIT_VELOCITY = 0x58;
        public const byte WIT_QUATER = 0x59;
        public const byte WIT_GSA = 0x5A;
        public const byte WIT_REGVALUE = 0x5F;
        public const byte AX = 0x34;
        public const byte AY = 0x35;
        public const byte AZ = 0x36;
        public const byte GX = 0x37;
        public const byte GY = 0x38;
        public const byte GZ = 0x39;
        public const byte HX = 0x3a;
        public const byte HY = 0x3b;
        public const byte HZ = 0x3c;
        public const byte Roll = 0x3d;
        public const byte Pitch = 0x3e;
        public const byte Yaw = 0x3f;
        public const byte TEMP = 0x40;
        public const byte CCBW = 0x2f;
        public const byte YYMM = 0x30;
        public const byte DDHH = 0x31;
        public const byte MMSS = 0x32;
        public const byte VERSION = 0x2e;
        public const byte D0Status = 0x41;
        public const byte D1Status = 0x42;
        public const byte D2Status = 0x43;
        public const byte D3Status = 0x44;
        public const byte PressureL = 0x45;
        public const byte PressureH = 0x46;
        public const byte HeightL = 0x47;
        public const byte HeightH = 0x48;
        public const byte LonL = 0x49;
        public const byte LonH = 0x4a;
        public const byte LatL = 0x4b;
        public const byte LatH = 0x4c;
        public const byte GPSHeight = 0x4d;
        public const byte GPSYAW = 0x4e;
        public const byte GPSVL = 0x4f;
        public const byte GPSVH = 0x50;
        public const byte q0 = 0x51;
        public const byte q1 = 0x52;
        public const byte q2 = 0x53;
        public const byte q3 = 0x54;
        public const byte SVNUM = 0x55;
        public bool Is360DegreeMode = false;
        private HWT101CTDataCallback callback;
        private float yawL;
        private float VelocityZ;
        private float VelocityY;
        public HWT101CT()
        {
            serialPort = new SerialPort();
        }
        private async Task<bool> DoAutoConnect(string portName)
        {
            try
            {
                serialPort = new SerialPort(portName, BAUD_RATE);
                serialPort.Open();

                await Task.Delay(500);

                var rxSize = serialPort.BytesToRead;
                if (rxSize > 0)
                {
                    Console.WriteLine(string.Format("Connected to port {0}", portName));
                    serialPort.DiscardInBuffer();
                    serialPort.Close();
                    return true;
                }
                serialPort.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        public async Task<bool> ConnectComport(string portName = "")
        {
                       bool connected = false;
            doClose = false;
            try
            {
                var availablePorts = SerialPort.GetPortNames();
                if (availablePorts.Length == 0)
                {
                    Debug.Log("Không tìm thấy cổng COM nào kết nối vào máy tính, vui lòng kiểm tra lại");
                    return false;
                }


                if (serialPort.IsOpen)
                {
                    serialPort.Close();
                }


                if (String.IsNullOrEmpty(portName))
                {
                    foreach (var tmpPort in availablePorts)
                    {
                        Debug.Log(tmpPort);
                        connected = await DoAutoConnect(tmpPort);
                        if (connected)
                        {
                            this.portName = tmpPort;
                            break;
                        }
                    }
                }
                else
                {
                    this.portName = portName;
                    connected = await DoAutoConnect(portName);
                }

                if (connected == false)
                    Debug.Log("Không kết nối được tới thiết bị, vui lòng kiểm tra lại");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {

            }
            return connected;
        }
        static byte CalculateChecksum(byte[] data, int len)
        {
            UInt32 i;
            byte check = 0;
            for (i = 0; i < len; i++) check += data[i];
            return check;
        }

        private void CopeWitData(byte index, UInt16[] data, int len)
        {

            int reg1 = 0, reg2 = 0;
            int reg1Len = 0, reg2Len = 0;
            reg1Len = 4;
            //Console.WriteLine("Index {0}", index);
            switch (index)
            {
                case WIT_ACC: reg1 = AX; reg1Len = 3; reg2 = TEMP; reg2Len = 1; break;
                case WIT_ANGLE:
                    {
                        reg1 = Roll;
                        reg1Len = 3;
                        reg2 = VERSION;
                        reg2Len = 1;
                        float yawL = ((float)data[2]) / 32760.0f * 180.0f;
                        if (Is360DegreeMode && yawL > 180.0f)
                        {
                            yawL -= 360.0f;
                        }
                        this.yawL = yawL;
                        this.callback(this, this.yawL);
                        return;

                    }
                //break;

                //case WIT_TIME: reg1 = YYMM; break;
                //case WIT_GYRO:
                //    {
                //        reg1 = GX; len = 3;
                //        float vZ = ((float)data[0]) / 32760.0f * 2000.0f;
                //        float vY = ((float)data[1]) / 32760.0f * 2000.0f;
                //        this.VelocityZ = vZ;
                //        this.VelocityY = vY;
                //        //Console.WriteLine($"VZ = {this.VelocityZ}, VY {this.VelocityY}");
                //    }
                //    break;
                //case WIT_MAGNETIC: reg1 = HX; len = 3; break;
                //case WIT_DPORT: reg1 = D0Status; break;
                //case WIT_PRESS: reg1 = PressureL; break;
                //case WIT_GPS: reg1 = LonL; break;
                //case WIT_VELOCITY: reg1 = GPSHeight; break;
                //case WIT_QUATER: reg1 = q0; break;
                //case WIT_GSA: reg1 = SVNUM; break;
                //case WIT_REGVALUE: reg1 = s_uiReadRegIndex; break;
                default:
                    return;
            }
        }
        /*private void CopeWitData(byte index, UInt16[] data, int len)
        {
            switch (index)
            {
                case WIT_ANGLE:
                    float yawL = ((float)data[2]) / 32760.0f * 180.0f;
                    if (Is360DegreeMode && yawL > 180.0f)
                    {
                        yawL -= 360.0f;
                    }
                    this.yawL = yawL;
                    this.callback?.Invoke(this, this.yawL);
                    break;
                // Các trường hợp xử lý dữ liệu khác có thể được thêm vào ở đây
                default:
                    break;
            }
        }*/

        void WitSerialDataIn(byte raw)
        {
            UInt16 crc16, temp, i;
            UInt16[] data = new UInt16[4];
            byte ucSum;

            witDataBuff[witDataCnt++] = raw;
            if (witDataBuff[0] != 0x55)
            {
                witDataCnt--;
                Buffer.BlockCopy(witDataBuff, 1, witDataBuff, 0, witDataCnt);
                return;
            }
            if (witDataCnt >= 11)
            {
                ucSum = CalculateChecksum(witDataBuff, 10);
                if (ucSum != witDataBuff[10])
                {
                    witDataCnt--;
                    Buffer.BlockCopy(witDataBuff, 1, witDataBuff, 0, witDataCnt);
                    return;
                }
                data[0] = (UInt16)(((UInt16)witDataBuff[3] << 8) | (UInt16)witDataBuff[2]);
                data[1] = (UInt16)(((UInt16)witDataBuff[5] << 8) | (UInt16)witDataBuff[4]);
                data[2] = (UInt16)(((UInt16)witDataBuff[7] << 8) | (UInt16)witDataBuff[6]);
                data[3] = (UInt16)(((UInt16)witDataBuff[9] << 8) | (UInt16)witDataBuff[8]);
                CopeWitData(witDataBuff[1], data, 4);
                witDataCnt = 0;
            }
            if (witDataCnt == witDataBuff.Length) witDataCnt = 0;
        }
        /* void WitSerialDataIn(byte raw)
         {
             witDataBuff[witDataCnt++] = raw;
             if (witDataBuff[0] != START_BYTE)
             {
                 witDataCnt = 0;
                 return;
             }
             if (witDataCnt >= DATE_LEN_BYTE)
             {
                 byte checksum = CalculateChecksum(witDataBuff, DATE_LEN_BYTE - 1);
                 if (checksum == witDataBuff[DATE_LEN_BYTE - 1])
                 {
                     UInt16[] data = new UInt16[4];
                     for (int i = 0; i < 4; i++)
                     {
                         data[i] = (UInt16)(((UInt16)witDataBuff[i * 2 + 3] << 8) | (UInt16)witDataBuff[i * 2 + 2]);
                     }
                     CopeWitData(witDataBuff[1], data, 4);
                 }
                 witDataCnt = 0;
             }
         }*/

        public async Task<bool> PollData(HWT101CTDataCallback callback = null)
        {
            long timeout = 2;
            var receivedData = new List<byte>();
            int size;
            this.callback = callback;
            if (callback == null)
            {
                return false;
            }

            try
            {
                serialPort.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            try
            {
                while (timeout > 0 && doClose == false)
                {
                    //timeout--;
                    await Task.Delay(1);
                    size = serialPort.BytesToRead;
                    if (size > 0)
                    {
                        timeout = 5;
                        byte[] tmp = new byte[size];
                        serialPort.Read(tmp, 0, size);
                        for (int i = 0; i < size; i++)
                        {
                            WitSerialDataIn(tmp[i]);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                //Log.Information(logBuffer.ToString());
                serialPort.Close();
                doClose = false;
            }

            return false;
        }

        public void SendSerial(ref byte[] data, int size)
        {
            try
            {
                serialPort.Write(data, 0, size);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void ClosePort()
        {
            if (serialPort == null || serialPort.IsOpen == false) return;

            doClose = true;
            try
            {
                serialPort.Close();
                doClose = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public bool VerifyChecksum(byte[] raw, int len)
        {
            if (raw[1] != 0x0D)
            {
                return false;
            }
            //int intSum = 0;
            //short shortSum = 0;
            //byte byteSum = 0;
            //for (int i = 1; i < len-2; i++)
            //{
            //    intSum += raw[i];
            //    shortSum += raw[i];
            //    byteSum += raw[i];
            //}
            //if (raw[len-1] == (intSum & 0x0FF))
            //{
            //    return true;
            //}
            //if (raw[len-1] == (shortSum & 0x0FF))
            //{
            //    return true;
            //}
            //if (raw[len-1] == byteSum)
            //{
            //    return true;
            //}
            //return false;
            return true;
        }

        private int BcdToInt(byte data)
        {
            int high = data >> 4;
            int low = data & 0xF;
            return 10 * high + low;
        }

        public bool IsSerialPortConnected()
        {
            return serialPort.IsOpen;
        }

        public string GetPortName()
        {
            if (serialPort.IsOpen) return this.portName;
            return "";
        }
    }
}
