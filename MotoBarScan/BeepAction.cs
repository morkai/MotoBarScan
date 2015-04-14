using CoreScanner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MotoBarScan
{
    public class BeepAction : IAction
    {
        private CCoreScanner coreScanner;

        private short scannerId = 1;

        private short beepIndex = 0;

        public BeepAction(CCoreScanner coreScanner)
        {
            this.coreScanner = coreScanner;
        }

        public void ReadArgs(string[] args)
        {
            scannerId = Convert.ToInt16(args[1]);
            beepIndex = Convert.ToInt16(args[2]);
        }

        public void Execute()
        {
            int status;
            string outXML;
            string inXML = "<inArgs>"
                + "<scannerID>" + scannerId + "</scannerID>"
                + "<cmdArgs>"
                + "<arg-int>" + beepIndex + "</arg-int>"
                + "</cmdArgs>"
                + "</inArgs>";

            coreScanner.ExecCommand(Program.OP_SET_ACTION, ref inXML, out outXML, out status);
        }
    }
}
