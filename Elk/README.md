# Elasticsearch, Logstash, Kibana

## docker-compose.yml
Use docker-compose up for Elasticsearch, Logstash and Kibana

- Elasticsearch will start in single-node mode
    - CORS enabled; you can expose and access it from Javascript app etc
- Kibana will be accessible at http://localhost
- Logstash will start pinging Hello messages into Elasticsearch
