# Telegraf, InfluxDB, Chronograf, Kapacitor

## docker-compose.yml
Use docker-compose up for InfluxDB and Chronograf

- UDP at port 8888 is enabled for InfluxDB; allow this UDP port in your docker host firewall
- For Arduino sketch; see nodemcu-influxdb folder