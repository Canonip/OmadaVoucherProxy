# OmadaVoucherProxy
Generates Voucher Codes for the TP-Link Omada Captive Portal to use with TP-Links EAP Access Point Series.
Includes a .NET Standard Library to use in a standalone application.
I reverse engineered the private Omada API in order to create a voucher code printer. (Coming Soon)
Currently only supports Cloud connected Omada Clients. I hosted the service on another network than the Omada Controller.


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
 1. Set CLOUD_USER, and CLOUD_PASSWORD (and maybe CLOUD_CLIENT_ID) in appsettings.json
 2. Deploy and Run
## Usage
POST to URL/api/cloud/createVouchers with a json Body like this
 
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
 The proxy checks the cloudclientid property, then the CLOUD_HOST environment variable and then if your account is connected to only one Omada Controller.  
 Only if none of these 3 conditions apply, a 400 BadRequest will be sent.  
 So if only one Omada Controller is connected to your account, you can ignore the CloudClientId  