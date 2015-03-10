// Copyright (c) 2015, Łukasz Walukiewicz <lukasz@walukiewicz.eu>
// Licensed under the MIT License <http://lukasz.walukiewicz.eu/p/MIT>
// Part of the MotoBarScan project <http://lukasz.walukiewicz.eu/p/MotoBarScan>

using System;
using System.Text;
using System.Threading;
using System.Xml;
using CoreScanner;

namespace MotoBarScan
{
    class Program
    {
        static CCoreScanner cCoreScannerClass;

        static void Main(string[] args)
        {
            try
            {
                cCoreScannerClass = new CCoreScannerClass();

                short[] scannerTypes = new short[] { 1 };
                int status;

                cCoreScannerClass.Open(0, scannerTypes, (short)scannerTypes.Length, out status);

                if (status != 0)
                {
                    Environment.Exit(1);
                }

                cCoreScannerClass.BarcodeEvent += cCoreScannerClass_BarcodeEvent;

                string outXML;
                string inXML = "<inArgs>"
                    + "<cmdArgs>"
                    + "<arg-int>1</arg-int>"
                    + "<arg-int>1</arg-int>"
                    + "</cmdArgs>"
                    + "</inArgs>";

                cCoreScannerClass.ExecCommand(1001, ref inXML, out outXML, out status);

                if (status != 0)
                {
                    Environment.Exit(2);
                }

                Thread.Sleep(-1);
            }
            catch (Exception x)
            {
                Console.Error.WriteLine(x.Message);
            }
        }

        static void cCoreScannerClass_BarcodeEvent(short eventType, ref string pscanData)
        {
            try
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(pscanData);

                var xmlNode = xmlDoc.DocumentElement.SelectSingleNode("/outArgs/arg-xml/scandata/datalabel");

                if (xmlNode == null)
                {
                    return;
                }

                var hexStrings = xmlNode.InnerText.Split(' ');
                var hexBytes = new byte[hexStrings.Length];

                for (var i = 0; i < hexStrings.Length; ++i)
                {
                    hexBytes[i] = Convert.ToByte(hexStrings[i], 16);
                }

                Console.WriteLine(Encoding.ASCII.GetString(hexBytes));
            }
            catch (Exception x)
            {
                Console.Error.WriteLine(x.Message);
            }
        }
    }
}
