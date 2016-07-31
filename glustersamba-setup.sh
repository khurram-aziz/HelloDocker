#!/bin/sh
smbpath="/etc/samba/smb.conf"
echo $glusterip $glusterhost >> /etc/hosts
mkdir /data
mount -t glusterfs $glusterhost:$glustervolume /data
smbpasswd -a root
echo [data] >> $smbpath
echo path = /data >> $smbpath
echo read only = no >> $smbpath
service smbd restart
