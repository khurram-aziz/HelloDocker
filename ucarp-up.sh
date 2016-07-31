#!/bin/sh
DEV=$1
VIP=$2
shift

if [ -z "$VIP" ]
then
  echo "$0 interface vip [additional ips...]"
fi

for i in $VIP $IPS
do
  ip addr add $i dev $DEV
done
