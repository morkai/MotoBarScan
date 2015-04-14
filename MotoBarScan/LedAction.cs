using CoreScanner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MotoBarScan
{
    public class LedAction : IAction
    {
        const short LED_GREEN_OFF = 42;
        const short LED_GREEN_ON = 43;
        const short LED_YELLOW_ON = 45;
        const short LED_YELLOW_OFF = 46;
        const short LED_RED_ON = 47;
        const short LED_RED_OFF = 48;

        private CCoreScanner coreScanner;

        private short scannerId = 1;

        private short led = LED_RED_ON;

        public LedAction(CCoreScanner coreScanner)
        {
            this.coreScanner = coreScanner;
        }

        public void ReadArgs(string[] args)
        {
            scannerId = Convert.ToInt16(args[1]);

            var color = args[2].ToUpper();
            var state = Convert.ToInt16(args[3]) == 1;

            switch (color)
            {
                case "GREEN":
                    led = state ? LED_GREEN_ON : LED_GREEN_OFF;
                    break;

                case "YELLOW":
                    led = state ? LED_YELLOW_ON : LED_YELLOW_OFF;
                    break;

                default:
                    led = state ? LED_RED_ON : LED_RED_OFF;
                    break;
            }
        }

        public void Execute()
        {
            int status;
            string outXML;
            string inXML = "<inArgs>"
                + "<scannerID>" + scannerId + "</scannerID>"
                + "<cmdArgs>"
                + "<arg-int>" + led + "</arg-int>"
                + "</cmdArgs>"
                + "</inArgs>";

            coreScanner.ExecCommand(Program.OP_SET_ACTION, ref inXML, out outXML, out status);
        }
    }
}
