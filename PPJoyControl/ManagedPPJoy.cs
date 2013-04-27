using System;
using System.Runtime.InteropServices;

namespace PPJoyControler
{
    public enum VIRTUAL_JOYSTICK_DEVICE
    {
        PPJoyIOCTL1,
        PPJoyIOCTL2
    }

    internal class ManagedPPJoy
    {
        private IntPtr handle;
        private JOYSTICK_STATE state;

        public static readonly byte NUM_ANALOG = 8;
        public static readonly byte NUM_DIGITAL = 16;

        public static readonly int PPJOY_AXIS_MIN = 1;
        public static readonly int PPJOY_AXIS_MAX = 32767;

        private static readonly uint FILE_DEVICE_UNKNOWN = 0x00000022;
        private static readonly uint FILE_ANY_ACCESS = 0;
        private static readonly uint METHOD_BUFFERED = 0;

        private static readonly uint FILE_DEVICE_PPORTJOY = ManagedPPJoy.FILE_DEVICE_UNKNOWN;

        private static readonly uint JOYSTICK_STATE_V1 = 0x53544143;

        private static IntPtr INVALID_HANDLE_VALUE = ((IntPtr)(int)-1);

        private static readonly uint GENERIC_WRITE = 0x40000000;
        private static readonly uint FILE_SHARE_WRITE = 0x00000002;
        private static readonly uint OPEN_EXISTING = 3;

        [DllImport("Kernel32.dll")]
        internal extern static IntPtr CreateFile(
              string lpFileName,                        // ファイル名
              uint dwDesiredAccess,                     // アクセスモード
              uint dwShareMode,                         // 共有モード
              IntPtr lpSecurityAttributes,              // セキュリティ記述子
              uint dwCreationDisposition,               // 作成方法
              uint dwFlagsAndAttributes,                // ファイル属性
              IntPtr hTemplateFile                      // テンプレートファイルのハンドル
        );

        [DllImport("Kernel32.dll")]
        internal extern static bool DeviceIoControl(
            IntPtr hDevice,                         // デバイス、ファイル、ディレクトリいずれかのハンドル
            uint dwIoControlCode,                   // 実行する動作の制御コード
            ref JOYSTICK_STATE lpInBuffer,          // 入力データを供給するバッファへのポインタ
            uint nInBufferSize,                     // 入力バッファのバイト単位のサイズ
            IntPtr lpOutBuffer,                     // 出力データを受け取るバッファへのポインタ
            uint nOutBufferSize,                    // 出力バッファのバイト単位のサイズ
            ref uint lpBytesReturned,               // バイト数を受け取る変数へのポインタ
            IntPtr lpOverlapped                     // 非同期動作を表す構造体へのポインタ
        );

        [DllImport("Kernel32.dll")]
        internal extern static bool CloseHandle(
            IntPtr hObject   // オブジェクトのハンドル
        );

        private int GetLastError()
        {
            return Marshal.GetLastWin32Error();
        }

        private static uint CTL_CODE(uint DeviceType, uint Function, uint Method, uint Access)
        {
            return (((DeviceType) << 16) | ((Access) << 14) | ((Function) << 2) | (Method));
        }

        private static uint PPORTJOY_IOCTL(uint _index_)
        {
            return (CTL_CODE(FILE_DEVICE_PPORTJOY, _index_, METHOD_BUFFERED, FILE_ANY_ACCESS));
        }

        private static uint IOCTL_PPORTJOY_SET_STATE()
        {
            return (PPORTJOY_IOCTL(0x00));
        }

        public ManagedPPJoy(VIRTUAL_JOYSTICK_DEVICE device)
        {
            this.state.Signature = 0;
            this.state.NumAnalog = 0;
            this.state.NumDigital = 0;
        }

        private bool isHandleOpend
        {
            get { return this.handle == INVALID_HANDLE_VALUE; }
        }

        public void Open(VIRTUAL_JOYSTICK_DEVICE device)
        {
            var virtualJoystickDevice = device == VIRTUAL_JOYSTICK_DEVICE.PPJoyIOCTL2 ? "\\\\.\\PPJoyIOCTL2" : "\\\\.\\PPJoyIOCTL1";

            this.handle = CreateFile(virtualJoystickDevice,
                            GENERIC_WRITE,
                            FILE_SHARE_WRITE,
                            IntPtr.Zero,
                            OPEN_EXISTING,
                            0,
                            IntPtr.Zero
            );
            this.state.Signature = JOYSTICK_STATE_V1;
            this.state.NumAnalog = NUM_ANALOG;
            this.state.NumDigital = NUM_DIGITAL;

            if (this.isHandleOpend)
            {
                Console.WriteLine("Opened");
                SetAnalogDefault();
                SetDigitalDefault();
            }
        }

        public void Send()
        {
            if (!this.isHandleOpend) return;
            uint RetSize = 0;
            if (!DeviceIoControl(this.handle, IOCTL_PPORTJOY_SET_STATE(), 
                ref this.state, (uint)Marshal.SizeOf(this.state), IntPtr.Zero, 0, ref RetSize, IntPtr.Zero))
            {
                int errorCode = GetLastError();
                if (errorCode == 2)
                {
                    Console.WriteLine("device deleted ");
                }
                else
                {
                    Console.WriteLine("error {0}", errorCode);
                }
            }
            else
            {
                Console.WriteLine("send {0}", RetSize);
            }
        }

        public void Close()
        {
            if (!CloseHandle(this.handle))
            {
                int errorCode = GetLastError();
                Console.WriteLine("CloseHandle error {0}", errorCode);
            }
        }

        public void SetAnalog(uint index, double value)
        {
            if (!this.isHandleOpend) return;
            if (index >= NUM_ANALOG) return;
            unsafe
            {
                fixed (int* p = this.state.Analog)
                {
                    p[index] = (int)((PPJOY_AXIS_MAX - PPJOY_AXIS_MIN) * value);
                }
            }
        }

        public void SetDigital(uint index, byte value)
        {
            if (!this.isHandleOpend) return;
            if (index >= NUM_DIGITAL) return;
            unsafe
            {
                fixed (byte* p = this.state.Digital)
                {
                    p[index] = value;
                }
            }
        }

        public void SetAnalogDefault()
        {
            if (!this.isHandleOpend) return;
            for (uint i = 0; i < (uint)NUM_ANALOG; i++)
            {
                SetAnalog(i, 0.5);
            }
        }

        public void SetDigitalDefault()
        {
            if (!this.isHandleOpend) return;
            for (uint i = 0; i < (uint)NUM_DIGITAL; i++)
            {
                SetDigital(i, 0);
            }
        }
    }
}
