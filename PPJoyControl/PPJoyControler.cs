using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PPJoyControler
{
    [Flags]
    public enum PAD_BUTTON_CODE : int
    {
        UP = 0x00000001,
        DOWN = 0x00000002,
        LEFT = 0x00000004,
        RIGHT = 0x00000008,
        BUTTON1 = 0x00000010,
        BUTTON2 = 0x00000020,
        BUTTON3 = 0x00000040,
        BUTTON4 = 0x00000080,
        BUTTON5 = 0x00000100,
        BUTTON6 = 0x00000200,
        BUTTON7 = 0x00000400,
        BUTTON8 = 0x00000800,
        BUTTON9 = 0x00001000,
        BUTTON10 = 0x00002000,
        BUTTON11 = 0x00004000,
        BUTTON12 = 0x00008000,
        BUTTON13 = 0x00010000,
        BUTTON14 = 0x00020000,
        BUTTON15 = 0x00040000,
        BUTTON16 = 0x00080000,
    }

    public enum BUTTON_PUSH_STATE
    {
        UP,
        DOWN
    }

    internal class PADSTATE
    {
        public int value { get; set; }

        public PADSTATE()
        {
            this.value = 0;
        }

        public void Add(int key)
        {
            this.value |= key;
        }

        public void Sub(int key)
        {
            this.value &= ~key;
        }

        public bool IsState(int key)
        {
            return ((this.value & key) != 0);
        }
    }

    public class PPJoyControler
    {
        private PADSTATE state;
        private ManagedPPJoy ppj;

        public PPJoyControler()
        {
            this.state = new PADSTATE();
        }

        public void Open(VIRTUAL_JOYSTICK_DEVICE device)
        {
            this.ppj = new ManagedPPJoy(device);
        }

        public void Close()
        {
            this.state = null;
            if (this.ppj != null)
            {
                this.ppj.Close();
                this.ppj = null;
            }
        }

        private int GetState()
        {
            return (this.state.value);
        }

        private void SetState(int key)
        {
            this.state.value = key;
        }

        private void AddState(int key)
        {
            this.state.Add(key);
        }

        private void SubstractState(int key)
        {
            this.state.Sub(key);
        }

        private void SetPPJDefaultAnalog()
        {
            this.state.Sub(0x0000000f);
        }

        private void SetPPJDefaultDigital()
        {
            this.state.Sub(0x000ffff0);
        }

        private void SetPPJDefault()
        {
            this.state.value = 0;
        }

        public void SetButtonState(PAD_BUTTON_CODE code, BUTTON_PUSH_STATE state)
        {
            switch (state)
            {
                case BUTTON_PUSH_STATE.DOWN: AddState((int)code); break;
                case BUTTON_PUSH_STATE.UP: SubstractState((int)code); break;
            }
        }

        public void ResetButtonState()
        {
            SetPPJDefault();
        }

        private void SetStateToPPJ()
        {
            if (this.state.IsState((int)PAD_BUTTON_CODE.UP)) { this.ppj.SetAnalog(0, 1); }
            else if (this.state.IsState((int)PAD_BUTTON_CODE.DOWN)) { this.ppj.SetAnalog(0, 0); }
            else { this.ppj.SetAnalog(0, 0.5); }
            if (this.state.IsState((int)PAD_BUTTON_CODE.LEFT)) { this.ppj.SetAnalog(1, 1); }
            else if (this.state.IsState((int)PAD_BUTTON_CODE.RIGHT)) { this.ppj.SetAnalog(1, 0); }
            else { this.ppj.SetAnalog(1, 0.5); }

            if (this.state.IsState((int)PAD_BUTTON_CODE.BUTTON1)) { this.ppj.SetDigital(0, 1); } else { this.ppj.SetDigital(0, 0); }
            if (this.state.IsState((int)PAD_BUTTON_CODE.BUTTON2)) { this.ppj.SetDigital(1, 1); } else { this.ppj.SetDigital(1, 0); }
            if (this.state.IsState((int)PAD_BUTTON_CODE.BUTTON3)) { this.ppj.SetDigital(2, 1); } else { this.ppj.SetDigital(2, 0); }
            if (this.state.IsState((int)PAD_BUTTON_CODE.BUTTON4)) { this.ppj.SetDigital(3, 1); } else { this.ppj.SetDigital(3, 0); }
            if (this.state.IsState((int)PAD_BUTTON_CODE.BUTTON5)) { this.ppj.SetDigital(4, 1); } else { this.ppj.SetDigital(4, 0); }
            if (this.state.IsState((int)PAD_BUTTON_CODE.BUTTON6)) { this.ppj.SetDigital(5, 1); } else { this.ppj.SetDigital(5, 0); }
            if (this.state.IsState((int)PAD_BUTTON_CODE.BUTTON7)) { this.ppj.SetDigital(6, 1); } else { this.ppj.SetDigital(6, 0); }
            if (this.state.IsState((int)PAD_BUTTON_CODE.BUTTON8)) { this.ppj.SetDigital(7, 1); } else { this.ppj.SetDigital(7, 0); }
            if (this.state.IsState((int)PAD_BUTTON_CODE.BUTTON9)) { this.ppj.SetDigital(8, 1); } else { this.ppj.SetDigital(8, 0); }
            if (this.state.IsState((int)PAD_BUTTON_CODE.BUTTON10)) { this.ppj.SetDigital(9, 1); } else { this.ppj.SetDigital(9, 0); }
            if (this.state.IsState((int)PAD_BUTTON_CODE.BUTTON11)) { this.ppj.SetDigital(10, 1); } else { this.ppj.SetDigital(10, 0); }
            if (this.state.IsState((int)PAD_BUTTON_CODE.BUTTON12)) { this.ppj.SetDigital(11, 1); } else { this.ppj.SetDigital(11, 0); }
            if (this.state.IsState((int)PAD_BUTTON_CODE.BUTTON13)) { this.ppj.SetDigital(12, 1); } else { this.ppj.SetDigital(12, 0); }
            if (this.state.IsState((int)PAD_BUTTON_CODE.BUTTON14)) { this.ppj.SetDigital(13, 1); } else { this.ppj.SetDigital(13, 0); }
            if (this.state.IsState((int)PAD_BUTTON_CODE.BUTTON15)) { this.ppj.SetDigital(14, 1); } else { this.ppj.SetDigital(14, 0); }
            if (this.state.IsState((int)PAD_BUTTON_CODE.BUTTON16)) { this.ppj.SetDigital(15, 1); } else { this.ppj.SetDigital(15, 0); }
        }

        public void Send()
        {
            SetStateToPPJ();
            this.ppj.Send();
        }
    }
}
