# DnsClientCOM
COM object for performing DNS queries / lookups. Utilizes the following code:
https://github.com/MichaCo/DnsClient.NET
&nbsp;

Example registration:
C:\WINDOWS\system32>%SystemRoot%\Microsoft.NET\Framework64\v4.0.30319\RegAsm.exe "c:\DNS\DnsClientCOM.dll" /tlb: "c:\DNS\DnsClientCOM.tlb" /codebase

Note: Be sure to register \DnsClientCOM.dll and not \DnsClient.dll as it will not work!
&nbsp;

VBScript code example:
```vbscript
set objTest = wscript.createobject("DnsClientCOM.DnsQuery+comReverseLookup")
msgbox objTest.dnsNameQuery("rhythmengineering.com", "", "")
````
&nbsp;

Need to set the assembly type to 64-bit to work with 64-bit wscript/cscript. 32-bit needs to be run from syswow64:
https://www.c-sharpcorner.com/UploadFile/nipuntomar/describing-com-component-object-model/
