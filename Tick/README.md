# Telegraf, InfluxDB, Chronograf, Kapacitor

## docker-compose.yml
Use docker-compose up for InfluxDB, Chronograf and Telegraf

- UDP at port 8888 is enabled for InfluxDB; allow this UDP port in your docker host firewall
- For Arduino sketch; see nodemcu-influxdb folder
- Telegraf polls SNMP values and sends to InfluxDB as well Prometheus