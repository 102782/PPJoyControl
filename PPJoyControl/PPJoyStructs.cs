using System;
using System.Runtime.InteropServices;

namespace PPJoyControler
{

    [StructLayout(LayoutKind.Explicit)]
    internal struct JOYSTICK_SET_STATE
    {
        [FieldOffset(0)]
        public uint Version;
        [FieldOffset(4), MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] Data;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct JOYSTICK_STATE
    {
        public uint Signature;
        public byte NumAnalog;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public int[] Analog;
        public byte NumDigital;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] Digital;
    }

    [StructLayout(LayoutKind.Explicit)]
    internal struct SECURITY_ATTRIBUTES
    {
        [FieldOffset(0)]
        public uint nLength;
        [FieldOffset(4)]
        public IntPtr lpSecurityDescriptor;
        [FieldOffset(8)]
        public bool bInheritHandle;
    }

    [StructLayout(LayoutKind.Explicit)]
    internal struct OVERLAPPED
    {
        [FieldOffset(0)]
        public uint Internal;
        [FieldOffset(4)]
        public uint InternalHigh;
        [FieldOffset(8)]
        public uint Offset;
        [FieldOffset(12)]
        public uint OffsetHigh;
        [FieldOffset(16)]
        public IntPtr hEvent;
    }

}