PUT /_template/my_index_settings
{
  "index_patterns": [ "syslog-*", "logstash-*", "orders" ],
  "settings": {
    "number_of_shards": 1,
    "number_of_replicas": 0
  }
}

GET /_cat/templates

GET /_template/logstash

GET /_cat/health?v

GET /_cat/nodes?v

GET /_cat/indices?v

POST /orders/_doc/4
{
  "orderID": "4", "orderAmount": "99"
}

GET /orders/_search
{
  "query" : {
    "range": { "orderAmount": { "gte": "500" } }
  }
}

POST /orders/_doc/4
{
  "orderID": "4", "orderAmount": "99"
}

GET /orders/_search
{
  "query" : {
    "range": { "orderAmount": { "gte": "500" } }
  }
}

PUT /_template/orders
{
  "index_patterns": [ "orders-*" ],
  "mappings" : {
    "properties" : {
      "orderID" : { "type" : "integer" },
      "orderAmount" : { "type" : "integer" }
    }
  }

}

PUT /orders-2019.06.01
{
  "mappings" : {
    "properties" : {
      "orderID" : { "type" : "integer" },
      "orderAmount" : { "type" : "integer" }
    }
  }
}

POST /orders-2019.06.01/_doc/1
{
  "orderID": "101", "orderAmount": "500"
}

POST /orders-2019.06.01/_doc/2
{
  "orderID": "102", "orderAmount": "600"
}

POST /orders-2019.06.01/_doc/3
{
  "orderID": "103", "orderAmount": "700"
}

POST /orders-2019.06.01/_doc/4
{
  "orderID": "104", "orderAmount": "99"
}

GET /orders-*/_search
{
  "query" : {
    "range": { "orderAmount": { "gte": "600" } }
  }
}