using System;
using System.Runtime.InteropServices;

namespace PPJoyControler
{

    [StructLayout(LayoutKind.Explicit)]
    internal unsafe struct JOYSTICK_SET_STATE
    {
        [FieldOffset(0)]
        uint Version;
        [FieldOffset(4)]
        fixed byte Data[1];
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct JOYSTICK_STATE
    {
        public uint Signature;
        public byte NumAnalog;
        public fixed int Analog[8];
        public byte NumDigital;
        public fixed byte Digital[16];
    }

    [StructLayout(LayoutKind.Explicit)]
    internal struct SECURITY_ATTRIBUTES
    {
        [FieldOffset(0)]
        uint nLength;
        [FieldOffset(4)]
        IntPtr lpSecurityDescriptor;
        [FieldOffset(8)]
        bool bInheritHandle;
    }

    [StructLayout(LayoutKind.Explicit)]
    internal struct OVERLAPPED
    {
        [FieldOffset(0)]
        uint Internal;
        [FieldOffset(4)]
        uint InternalHigh;
        [FieldOffset(8)]
        uint Offset;
        [FieldOffset(12)]
        uint OffsetHigh;
        [FieldOffset(16)]
        IntPtr hEvent;
    }

}