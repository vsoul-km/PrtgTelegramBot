#To install service copy binaries to C:\Program Files\PrtgTelegramBot

#Delete Service
sc stop "PrtgTelegramBot"
sc.exe delete "PrtgTelegramBot"

#create Service
sc.exe create "PrtgTelegramBot" binpath= "C:\Program Files\PrtgTelegramBot\PrtgTelegramBot.exe" start= auto
sc description "PrtgTelegramBot" "Telegram Bot for PRTG Server"
sc failure "PrtgTelegramBot" reset= 86400 actions= restart/60000/restart/60000/restart/60000/