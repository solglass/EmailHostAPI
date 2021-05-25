INSTALLATION
------------

Please make sure the release file is unpacked under a accessible
directory. Setup values for these _environment variables_ from __launchSettings.json__ file:

        "EMAILSERVICE_ADMIN_LOGIN"
        "EMAILSERVICE_SMTPSERVER_ADDRESS"
        "EMAILSERVICE_ADMIN_EMAIL_ADDRESS"
        "EMAILSERVICE_SMTPPORT"
        "EMAILSERVICE_ADMIN_PASSWORD"



QUICK START
-----------

Use Windows PowerShell, type in the following commands:

    sc.exe create EmailHost binpath= C:\services\EmailHostService\EmailHost.exe start= auto                
then go to your Task manager, services and run EmailHost.

For update or delete service use: 

    sc.exe delete EmailHost

but don't forget to stop service in Task Manager before doing any operations.

LOGGING
-----------

You can find all information about the current state of the service in __logFile.txt__ placed under the folder _C:\services\EmailHostService\Logs_ which updates daily.
