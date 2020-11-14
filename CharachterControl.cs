using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bot
{
    class CharachterControl
    {
        public static void TryToAttackMob()
        {
            AttackMob();
            Thread.Sleep(100); // todo: change to RandomDelay
            PreventFromRunningFarAway();
        }

        static void AttackMob()
        {
            Input.Mouse_event(Input.MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            Input.Mouse_event(Input.MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }


        static void PreventFromRunningFarAway() // By pressing "S" button char go back
        {
            for (int i = 0; i<3; i++) { 
                Input.Keybd_event(Convert.ToByte(Keys.S), 0, Input.KEYBOARDEVENTF_KEYDOWN, 0);
                Input.Keybd_event(Convert.ToByte(Keys.S), 0, Input.KEYBOARDEVENTF_KEYUP, 0);
                SendKeys.Send("s");
            }
        }


        public static void AttackMob(int delay)
        {
            SendKeys.Send("1");
            Thread.Sleep(delay);
        }

        public static async void AttackMobAsync(int delay)
        {
            await Task.Run(() => AttackMob(delay));
        }

        public static void GetLoot()
        {
            for (int j = 0; j < 50; j++)
            {
                Input.Keybd_event(Convert.ToByte(Keys.X), 0, Input.KEYBOARDEVENTF_KEYDOWN, 0);
                Input.Keybd_event(Convert.ToByte(Keys.X), 0, Input.KEYBOARDEVENTF_KEYUP, 0);
                Thread.Sleep(100);
            }
        }

        public static async void GetLootAsync()
        {
            await Task.Run(() => GetLoot());
        }

        public static void PressKeyBoardButton(byte key)
        {
            Input.Keybd_event(key, 0, Input.KEYBOARDEVENTF_KEYDOWN, 0);
            Input.Keybd_event(key, 0, Input.KEYBOARDEVENTF_KEYUP, 0);
        }
        
        public static void PressKeyBoardButton(Keys key)
        {
            Input.Keybd_event(Convert.ToByte(key), 0, Input.KEYBOARDEVENTF_KEYDOWN, 0);
            Input.Keybd_event(Convert.ToByte(key), 0, Input.KEYBOARDEVENTF_KEYUP, 0);
        }

    }
}
