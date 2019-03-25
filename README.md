# OmadaVoucherProxy
Generates Voucher Codes for the TP-Link Omada Captive Portal to use with TP-Links EAP Access Point Series.

## Usage
 1. If your Omada Controller uses a self signed certificate, add it to your computers trusted hosts.
 2. Set host, user and password in appsettings.json
 3. Deploy and Run
 4. POST to URL/api/voucher with a json Body like this
 
```json
{
  "CodeLength":6,
  "Amount":1,
  "DurationMinutes":1440
}
```

* CodeLength: amount of digits the vouchers will have
* Amount: amount of vouchers to be generated
* DurationMinutes: amount of minutes the voucher will be valid