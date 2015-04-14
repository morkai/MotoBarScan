// Copyright (c) 2015, Łukasz Walukiewicz <lukasz@walukiewicz.eu>
// Licensed under the MIT License <http://lukasz.walukiewicz.eu/p/MIT>
// Part of the MotoBarScan project <http://lukasz.walukiewicz.eu/p/MotoBarScan>

using CoreScanner;
using System;
using System.Collections.Concurrent;
using System.Text;
using System.Threading;
using System.Xml;

namespace MotoBarScan
{
    class Program
    {
        public static int ERR_GENERAL = 0x01;
        public static int ERR_SCANNER_OPEN = 0x02;
        public static int ERR_BARCODE_EVENT = 0x03;

        public static int OP_REGISTER_FOR_EVENTS = 1001;
        public static int OP_DEVICE_LED_OFF = 2009;
        public static int OP_DEVICE_LED_ON = 2010;
        public static int OP_SET_ACTION = 6000;

        static CCoreScanner coreScanner;

        static void Main(string[] args)
        {
            OpenBarcodeScanner();

            var tokenSource = new CancellationTokenSource();
            var actionQueue = new BlockingCollection<IAction>();
            var stdinWorker = new StdinWorker(tokenSource, actionQueue);
            var stdinThread = new Thread(stdinWorker.Run);
            var actionWorker = new ActionWorker(tokenSource, actionQueue);
            var actionThread = new Thread(actionWorker.Run);

            stdinWorker.RegisterAction("LED", new LedAction(coreScanner));
            stdinWorker.RegisterAction("BEEP", new BeepAction(coreScanner));

            Console.CancelKeyPress += (object sender, ConsoleCancelEventArgs e) => { tokenSource.Cancel(); };

            try
            {
                stdinThread.Start();
                actionThread.Start();
                stdinThread.Join();
                actionThread.Join();
            }
            catch (Exception x)
            {
                Exit(x, ERR_GENERAL);
            }
        }

        public static void Exit(Exception x, int exitCode)
        {
            Console.Error.WriteLine("ERROR: " + x.ToString());
            Environment.Exit(exitCode);
        }

        public static void Exit(int exitCode)
        {
            Environment.Exit(exitCode);
        }

        private static void OpenBarcodeScanner()
        {
            try
            {
                coreScanner = new CCoreScannerClass();

                short[] scannerTypes = new short[] { 1 };
                int status;

                coreScanner.Open(0, scannerTypes, (short)scannerTypes.Length, out status);

                if (status != 0)
                {
                    Exit(ERR_SCANNER_OPEN);
                }

                coreScanner.BarcodeEvent += OnBarcodeEvent;

                string outXML;
                string inXML = "<inArgs>"
                    + "<cmdArgs>"
                    + "<arg-int>1</arg-int>"
                    + "<arg-int>1</arg-int>"
                    + "</cmdArgs>"
                    + "</inArgs>";

                coreScanner.ExecCommand(OP_REGISTER_FOR_EVENTS, ref inXML, out outXML, out status);

                if (status != 0)
                {
                    Exit(ERR_BARCODE_EVENT);
                }
            }
            catch (Exception x)
            {
                Exit(x, ERR_GENERAL);
            }
        }

        private static void OnBarcodeEvent(short eventType, ref string pscanData)
        {
            try
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(pscanData);

                var scannerIdNode = xmlDoc.DocumentElement.SelectSingleNode("/outArgs/scannerID");

                if (scannerIdNode == null)
                {
                    return;
                }

                var dataLabelNode = xmlDoc.DocumentElement.SelectSingleNode("/outArgs/arg-xml/scandata/datalabel");

                if (dataLabelNode == null)
                {
                    return;
                }

                var hexStrings = dataLabelNode.InnerText.Split(' ');
                var hexBytes = new byte[hexStrings.Length];

                for (var i = 0; i < hexStrings.Length; ++i)
                {
                    hexBytes[i] = Convert.ToByte(hexStrings[i], 16);
                }

                Console.WriteLine("BARCODE {0} {1}", scannerIdNode.InnerText, Encoding.ASCII.GetString(hexBytes));
            }
            catch (Exception x)
            {
                Console.Error.WriteLine(x.Message);
            }
        }
    }
}
