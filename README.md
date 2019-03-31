# OmadaVoucherProxy
Generates voucher codes for the TP-Link Omada captive portal to use with TP-Link's EAP access point series.
Includes a .NET Standard library to use in a standalone application.
I reverse engineered the private Omada API in order to create a voucher code printer. (Coming Soon)
Currently only supports cloud-connected Omada clients. I host the service on another network than the Omada controller.


## Deployment
### Docker (using compose)
```
omada-proxy:
  container_name: omada-proxy
  image: canonip/omadavoucherproxy
  expose: # use ports if not using a proxy like jwilder/nginx-proxy
	- 17875
  environment:
	- CLOUD_USER=myemail@whatever.com
	- CLOUD_PASSWORD=supersecretpassword
	- CLOUD_CLIENT_ID=myid #OPTIONAL, see in FAQ
```
### Manually
 1. Set CLOUD_USER, and CLOUD_PASSWORD (and eventually CLOUD_CLIENT_ID) in appsettings.json
 2. Deploy and Run

## Usage
POST to URL/api/cloud/createVouchers with a json body like this
 
```json
{
  "CodeLength":6,
  "Amount":1,
  "DurationMinutes":1440,
  "CloudClientId":"optional"
}
```

* CodeLength: amount of digits the vouchers will have (min 6, max 10)
* Amount: amount of vouchers to be generated
* DurationMinutes: amount of minutes the voucher will be valid


### FAQ
 * What is CloudClientId/CLOUD_CLIENT_ID and (why) do I need it  
 You can find your ID in the URL when accessing your Omada Controller in a web browser. It's the value after `deviceId`  
 The proxy checks the CloudClientId property in the HTTP Request, then the CLOUD_CLIENT_ID environment variable and then whether your account is connected to exactly one Omada Controller.  
 Only if none of these 3 conditions apply, a 400 BadRequest will be sent.  
 So if exactly one Omada Controller is connected to your account, you may ignore the CloudClientId  
