# Prometheus
http://weblogs.com.pk/khurram/archive/2018/04/16/prometheus.aspx

## docker-compose.yml
Use docker-compose up --build for single host deployment

- Edit prometheus.yml according to your Docker Host / VM
- Edit alertmanager-config.yml according to your Slack settings
- NetCoreConsole is demo .NET Core app exposing metrices; it will be built using Docker Multi Stage builds; you dont need to install any build tools
- Prometheus will be @ http://localhost:90, Pushgateway @ http://localhost:91 and Alertmanager @ http://localhost:93
- Grafana will be @ http://localhost:81; admin/foobar will be administrative credentials that you can change in docker-compose.yml
- Nginx will be setup @ http://localhost reverse proxying Grafana with Basic Authentication; magic master password is khurram for any login

## docker-compose.no-dotnet.yml
Use this with docker-compose (-f docker-compose.no-dotnet.yml) if you dont want to download .NET SDK/Runtime Docker images

- Edit prometheus.yml according to your Docker Host / VM; also remove netcoreconsole monitoring
- Prometheus will be @ http://localhost:90, Pushgateway @ http://localhost:91 and Alertmanager @ http://localhost:93
- There will be no Nginx/Basic Authentication; Grafana with admin/foobar administrative credentials will be @ http://localhost; you can change in the YML

## docker-stack.yml
Use with docker swarm (docker stack deploy)

- Change netcoreconsole and bauth image names according to your registry
- Change extra hosts settings under prometheus service according to your swarm nodes
- Will deploy node-exporter and netcoreconsole globally
- Will deploy one instance of alertmanager, prometheus, grafana and bauth each
- Will deploy nginx on swarm manager
- You will be able to access grafana @ http://swarm-manager behind nginx with basic auth; raw grafana @ http://swarm-manager:81, prometheus @ http://swarm-manager:90, pushgateway @ http://swarm-manager:91 and alertmanager @ http://swarm-manager:93