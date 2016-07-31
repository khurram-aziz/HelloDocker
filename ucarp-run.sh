#!/bin/bash

PIDFILE=/tmp/ucarp.pid
trap "shutdown" SIGINT SIGTERM

shutdown() {
  RET=$1
  echo "Shutting down"
  PID=`cat $PIDFILE`
  if [ -z "$PID" ]
  then
    echo "ucarp pid not found"
    exit 99
  fi

  echo "Demoting master"
  kill -USR2 $PID
  sleep 1
  echo "Sending TERM to ucarp"
  kill $PID
  sleep 1
  echo "Sending KILL to ucarp" # we need to make sure everything is dead before
  kill -9 $PID                 # ucarp might promote itself to master afer 3s again
  exit $RET
}

export VIP=$1
export PASS=$2
shift; shift
export IPS=$@
export DEV=${DEV-eth1}

if [ -z "$PASS" ]
then
  echo "$0 virtual-ip password"
  exit 1
fi

/ucarp/ucarp.sh &
echo $! > $PIDFILE
wait $!
