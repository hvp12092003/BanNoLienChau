using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Forms;
using Mb;
using UnityEngine;

namespace AppModbus
{
    [System.Serializable]
    public class AppModbus
    {
        [SerializeField] private string portName = "";
        private int baudrate = 9600;
        private ModbusClient client = new ModbusClient();
        private byte slaveAddr = 1;
        public static readonly int MODBUS_LOADCELL_ADDR = 0;
        public static readonly int MODBUS_LOADCELL_ZERO_REGISTER = 2;
        private int zeroOffset = 0;
        private int point1Offset = 0;
        private int point1WeightInG = 0;
        // kg = a*x + b
        [StructLayout(LayoutKind.Explicit)]
        public struct UInt32ToFloat
        {
            [FieldOffset(0)]
            public uint UInt32;

            [FieldOffset(0)]
            public float Single;
        }

        public AppModbus()
        {

        }

        private bool TryConnect(string portName)
        {
            Debug.Log(portName);
            bool retval = false;
            if (!String.IsNullOrEmpty(portName))
            {
                var tmpClient = new ModbusClient(portName);
                try
                {
                    //SerialPort serialPort = new SerialPort(portName, baudrate);
                    //serialPort.Open();
                    //serialPort.Close();
                    //FixDebug.Instance.Log($"Checking port {portName}");
                    tmpClient.UnitIdentifier = 1;
                    tmpClient.Baudrate = 9600;
                    tmpClient.Parity = Parity.None;
                    tmpClient.ConnectionTimeout = 1000;
                    tmpClient.Connect();
                    tmpClient.ReadHoldingRegisters(MODBUS_LOADCELL_ADDR, 2);
                    tmpClient.Disconnect();
                    retval = true;
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.Message);
                    try
                    {
                        if (tmpClient.Connected)
                        {
                            tmpClient.Disconnect();
                        }
                    }
                    catch { }
                }
            }

            return retval;
        }
        public bool DoAutoConnect(string portName, byte addr = 1)
        {
            bool ret = false;
            try
            {
                var availablePorts = SerialPort.GetPortNames();
                if (availablePorts.Length == 0)
                {
                    //FixDebug.Instance.Log("Không tìm thấy cổng COM nào kết nối vào máy tính, vui lòng kiểm tra lại");
                    return false;
                }

                if (!String.IsNullOrEmpty(portName))
                {
                    Debug.Log("Trying to connect");
                    if (availablePorts.Contains(portName))
                    {
                        ret = TryConnect(portName);
                        if (ret)
                        {
                            //FixDebug.Instance.Log($"Connected port: {portName}");
                            this.slaveAddr = addr;
                            this.portName = portName;
                        }
                    }
                }
                else
                {
                    foreach (var tmpPort in availablePorts)
                    {
                        ret = TryConnect(tmpPort);
                        if (ret)
                        {
                            //FixDebug.Instance.Log($"Connected port: {tmpPort}");
                            this.slaveAddr = addr;
                            this.portName = tmpPort;
                            break;
                        }
                    }
                }
            }//
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }
            return ret;
        }

        public bool OpenModbus()
        {
            bool retval = false;
            try
            {
                this.client = new ModbusClient(this.portName);
                client.UnitIdentifier = this.slaveAddr;
                client.Baudrate = this.baudrate;
                client.Parity = Parity.None;
                client.ConnectionTimeout = 1000;
                client.Connect();
                retval = true;
            }
            catch (Exception ex)
            {
                Debug.Log($"Open modbus failed {ex.Message}");
            }
            return retval;
        }

        public void CloseModbus()
        {
            try
            {
                client.Disconnect();
            }
            catch (Exception ex)
            {
                Debug.Log($"CloseModbus failed {ex.Message}");
            }
        }


        public bool ReadInputRegister(int addr, int registerCount, ref int[] registerValue)
        {
            bool retval = false;

            try
            {
                registerValue = this.client.ReadInputRegisters(addr, registerCount);
                retval = true;
                //string val = "";
                //string hexa = "";
                //for (int i = 0; i < registerCount; i++)
                //{
                //    val += string.Format("{0} ", registerValue[i], registerValue[i].ToString("X"));
                //    hexa += string.Format("{0} ", registerValue[i].ToString("X"));
                //}
                //Debug.Log(String.Format("Value of {0} registers from offset {1}: \nInterger : {2}\nHexa : {3}",
                //                registerCount,
                //                addr, val,
                //                hexa));    //Reads Discrete Input #1
            }
            catch (Exception ex)
            {
                retval = false;
                //Debug.Log(string.Format("Modbus read input register {0}, err {1} ", addr, ex.Message));
                //if (ex.Message.Contains("serial port not opened"))
                //{
                //    //ClosePort();
                //}
            }

            return retval;
        }


        public bool ReadHoldingRegister(int addr, int registerCount, ref int[] registerValue)
        {
            bool retval = false;

            try
            {
                registerValue = this.client.ReadHoldingRegisters(addr, registerCount);
                retval = true;
                //string val = "";
                //string hexa = "";
                //for (int i = 0; i < registerCount; i++)
                //{
                //    val += string.Format("{0} ", registerValue[i], registerValue[i].ToString("X"));
                //    hexa += string.Format("{0} ", registerValue[i].ToString("X"));
                //}
                //Debug.Log(String.Format("Value of {0} holding registers from offset {1}: \nInterger : {2}\nHexa : {3}",
                //                registerCount,
                //                addr, val,
                //                hexa));    //Reads Discrete Input #1
            }
            catch (Exception ex)
            {
                retval = false;
                //Debug.Log(string.Format("Modbus read holding register {0}, err {1} ", addr, ex.Message));
                //if (ex.Message.Contains("serial port not opened"))
                //{
                //    //ClosePort();
                //}
            }

            return retval;
        }

        private bool WriteHoldingRegister(int addr, int[] registerValue)
        {
            bool retval = false;
            try
            {
                this.client.WriteMultipleRegisters(addr, registerValue);
                retval = true;
            }
            catch
            {

            }
            return retval;
        }

        public static float IntArrayRegister2Float(int[] interger)
        {
            int[] tmp = new int[2];
            tmp[0] = interger[1];
            tmp[1] = interger[0];

            return ModbusClient.ConvertRegistersToFloat(tmp);
        }

        public static UInt32 IntArrayRegister2Uint32(int[] interger)
        {
            return (UInt32)ModbusClient.ConvertRegistersToInt(interger);
        }

        public static Int32 IntArrayRegister2Int32(int[] interger)
        {
            return ModbusClient.ConvertRegistersToInt(interger);
        }

        public static Int32 IntArrayRegister2Int32Reverse(int[] interger)
        {
            int[] rsvInt = new int[2];
            rsvInt[0] = interger[1];
            rsvInt[1] = interger[0];
            return ModbusClient.ConvertRegistersToInt(rsvInt);
        }

        public void SoftwareSetZeroOffset(int offset)
        {
            zeroOffset = offset;
        }

        public bool ReadLoadcellValue(ref Int32 loadCellValue)
        {
            bool ret = false;

            try
            {
                //OpenModbus();
                int raw = 0;
                int[] registerValue = new int[2];
                ret = ReadHoldingRegister(MODBUS_LOADCELL_ADDR, 2, ref registerValue);
                if (ret)
                {
                    raw = IntArrayRegister2Int32Reverse(registerValue);
                    //FixDebug.Instance.Log($"Raw value {raw}");
                    int diff = raw - zeroOffset;
                    if (diff <= 0)
                    {
                        loadCellValue = 0;
                    }
                    else
                    {

                        // 162 = offset at 6.15kg
                        // 27 = offset at 0Kg, no load
                        // y = ax+b
                        // y0 = b
                        // y_calib = a*x_calib + y0 -> a = ((y_calib)-y0) / x_calib
                        loadCellValue = (int)((double)(raw - zeroOffset) / (this.point1Offset - this.zeroOffset) * this.point1WeightInG);       // magin number, giai thich sau
                        //FixDebug.Instance.Log("Value: " + loadCellValue);
                    }
                }
                //CloseModbus();
            }
            catch // (Exception ex)
            {

            }

            return ret;
        }

        public bool ReadLoadcellRawValue(ref Int32 rawValue)
        {
            bool ret = false;

            try
            {
                int[] registerValue = new int[2];
                ret = ReadHoldingRegister(MODBUS_LOADCELL_ADDR, 2, ref registerValue);
                if (ret)
                {
                    rawValue = IntArrayRegister2Int32Reverse(registerValue);
                }
            }
            catch // (Exception ex)
            {

            }

            return ret;
        }

        public bool SetLoadcellZero()
        {
            bool ret = false;
            //ret = OpenModbus();
            //if (ret)
            {
                int[] registerValue = new int[2];
                ret = ReadHoldingRegister(MODBUS_LOADCELL_ADDR, 2, ref registerValue);
                if (ret)
                {
                    //ret = WriteHoldingRegister(MODBUS_LOADCELL_ZERO_REGISTER, registerValue);
                }
                //CloseModbus();
            }

            return ret;
        }

        public bool ReadLoadcellZero(ref int zeroValue)
        {
            bool ret = false;
            //ret = OpenModbus();
            //if (ret)
            {
                int[] registerValue = new int[2];
                ret = ReadHoldingRegister(MODBUS_LOADCELL_ZERO_REGISTER, 2, ref registerValue);
                if (ret)
                {
                    zeroValue = IntArrayRegister2Int32(registerValue);
                }
                else
                {
                    Debug.Log("Read loadcell failed");
                }
                //CloseModbus();
            }

            return ret;
        }

        public void SetCalibValue(int offsetAtZero, int offsetAtMax, int weightAtMaxInGUnix)
        {
            zeroOffset = offsetAtZero;
            point1Offset = offsetAtMax;
            point1WeightInG = weightAtMaxInGUnix;
        }
    }
}