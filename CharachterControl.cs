﻿using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bot
{
    class CharachterControl
    {
        public static void TryToAttackMob()
        {
            Click();
            RandomDelaySleep(100); // todo: change to RandomDelay
            PreventFromRunningFarAway();
        }

        static void Click()
        {
            Input.mouse_event(Input.MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            Input.mouse_event(Input.MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }

        static void PreventFromRunningFarAway() // By pressing "S" button, char go back
        {
            for (int i = 0; i<3; i++) { 
                Input.keybd_event(Convert.ToByte(Keys.S), 0, Input.KEYBOARDEVENTF_KEYDOWN, 0);
                Input.keybd_event(Convert.ToByte(Keys.S), 0, Input.KEYBOARDEVENTF_KEYUP, 0);
                SendKeys.Send("s");
            }
        }

        public static void AttackMobAndWait(int delay)
        {
            SendKeys.Send("1"); // Press "1" - "1" mean attack mob
            RandomDelaySleep(delay);
        }

        public static void GetLoot()
        {
            for (int j = 0; j < 50; j++)
            {
                Input.keybd_event(Convert.ToByte(Keys.X), 0, Input.KEYBOARDEVENTF_KEYDOWN, 0);
                Input.keybd_event(Convert.ToByte(Keys.X), 0, Input.KEYBOARDEVENTF_KEYUP, 0);
                RandomDelaySleep(100);
            }
        }

        public static void PressKeyBoardButton(byte key)
        {
            Input.keybd_event(key, 0, Input.KEYBOARDEVENTF_KEYDOWN, 0);
            Input.keybd_event(key, 0, Input.KEYBOARDEVENTF_KEYUP, 0);
        }
        
        public static void PressKeyBoardButton(Keys key)
        {
            Input.keybd_event(Convert.ToByte(key), 0, Input.KEYBOARDEVENTF_KEYDOWN, 0);
            Input.keybd_event(Convert.ToByte(key), 0, Input.KEYBOARDEVENTF_KEYUP, 0);
        }

        static void RandomDelaySleep(int delayInMilliseconds) // min delay = 5 mSec
        {
            if (delayInMilliseconds < 5)
            {
                delayInMilliseconds = 5;
            }
            
            int dispersion = 20; // +-20%
            int percentsFromDelay = delayInMilliseconds / 100 * dispersion;

            var rand = new Random();
            int randomDelay = rand.Next(-delayInMilliseconds / percentsFromDelay, delayInMilliseconds / percentsFromDelay);

            Thread.Sleep(delayInMilliseconds + randomDelay);
        }
    }
}
