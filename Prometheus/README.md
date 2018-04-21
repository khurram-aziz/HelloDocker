# Prometheus
http://weblogs.com.pk/khurram/archive/2018/04/16/prometheus.aspx

## docker-compose.yml
Use docker-compose up --build for single host deployment

- Edit prometheus.yml according to your Docker Host / VM
- Edit alertmanager-config.yml according to your Slack settings
- NetCoreConsole is demo .NET Core app exposing metrices; it will be built using Docker Multi Stage builds; you dont need to install any build tools
- Grafana will also be setup @ http://localhost:90; admin/foobar will be administrative credentials that you can change in docker-compose.yml
- Nginx will be setup @ http://localhost reverse proxying Grafana with Basic Authentication; magic master password is khurram for any login

## docker-compose.no-dotnet.yml
Use this with docker-compose (-f docker-compose.no-dotnet.yml) if you dont want to download .NET SDK/Runtime Docker images

- Edit prometheus.yml according to your Docker Host / VM; also remove netcoreconsole monitoring
- There will be no Nginx/Basic Authentication; Grafana with admin/foobar administrative credentials will be @ http://localhost; you can change in the YML

## docker-compose.v3.yml
Use with docker swarm (docker stack deploy)

- Change netcoreconsole and bauth image names according to your registry
- Change extra hosts settings according to your swarm
- Will deploy node-exporter and netcoreconsole globally
- Will deploy one instance of alertmanager and bauth each
- Will deploy prometheus, grafana and nginx on swarm manager
- You will be able to access grafana @ http://swarm-manager behind nginx with basic auth; raw grafana @ http://swarm-manager:90 and prometheus @ http://swarm-manager:91
- Look for "machine" metric; it will have name label giving you all the container host names of netcoreconsole