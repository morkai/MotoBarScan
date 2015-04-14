# MotoBarScan

A simple Windows Console application that prints scanned barcode values to standard
output and sends commands from standard input to the Motorola Barcode Scanners.

## Requirements

### .NET Framework

  * __Version__: 4.x
  * __Website__: http://www.microsoft.com/net
  * __Download__: http://www.microsoft.com/net/downloads

### Motorola CoreScanner SDK

SDK is required for development. On the client's machine, the CoreScanner Driver must be installed.

  * __Version__: 2.x
  * __Website__: [Scanner SDK for Windows](http://www.motorolasolutions.com/US-EN/Business+Product+and+Services/Software+and+Applications/Mobility+Software/Scanner+Drivers+and+Utilities/Scanner+SDK+for+Windows_US-EN)
  * __Download__: [Software Development Kit (SDK) Download](https://portal.motorolasolutions.com/Support/US-EN/Resolution?solutionId=87666)

## Usage

Run the executable `MotoBarScan.exe` and try scanning a barcode or send any of the supported
commands to stdin.

Scanned barcodes are written to stdout in the following format:
```
BARCODE <scanner ID> <scanned value>
```

### Supported commands

#### LED

`LED <scanner ID> <LED color> <LED state>`

  * LED color - `GREEN`, `YELLOW` or `RED`
  * LED state - `1` to turn on, `0` to turn off

#### BEEP

`BEEP <scanner ID> <beep index>`

Available beeps:

<ol start=0>
  <li>ONE SHORT HIGH
  <li>TWO SHORT HIGH
  <li>THREE SHORT HIGH
  <li>FOUR SHORT HIGH
  <li>FIVE SHORT HIGH
  <li>ONE SHORT LOW
  <li>TWO SHORT LOW
  <li>THREE SHORT LOW
  <li>FOUR SHORT LOW
  <li>FIVE SHORT LOW
  <li>ONE LONG HIGH
  <li>TWO LONG HIGH
  <li>THREE LONG HIGH
  <li>FOUR LONG HIGH
  <li>FIVE LONG HIGH
  <li>ONE LONG LOW
  <li>TWO LONG LOW
  <li>THREE LONG LOW
  <li>FOUR LONG LOW
  <li>FIVE LONG LOW
  <li>FAST HIGH LOW HIGH LOW
  <li>SLOW HIGH LOW HIGH LOW
  <li>HIGH LOW
  <li>LOW HIGH
  <li>HIGH LOW HIGH
  <li>LOW HIGH LOW
  <li>HIGH HIGH LOW LOW
</ol>

## License

This project is released under the [MIT License](https://raw.github.com/morkai/MotoBarScan/master/license.md).
