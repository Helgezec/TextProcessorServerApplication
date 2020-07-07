# TextProcessorServerApplication
Server version of https://github.com/Helgezec/TextProcessorApplication

Server is a TCP-listener that should be configured with connection string and port on start. It can also create-update-delete dictionary if you write corresponding command in console (after application starts).

Client should be configured with two command prompt arguments: IP and port. Then it can get autocomplete from server by entering commands like "get prefix".
