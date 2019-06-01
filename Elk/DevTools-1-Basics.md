GET _search
{
  "query": {
    "match_all": {}
  }
}

PUT /orders

POST /orders/_doc/1
{
  "orderID": "1", "orderAmount": "500"
}

POST /orders/_doc/2
{
  "orderID": "2", "orderAmount": "600"
}

POST /orders/_doc/3
{
  "orderID": "3", "orderAmount": "700"
}

GET /orders/_doc/1

GET /orders/_search

GET /orders/_search
{
  "query": {
    "match": { "orderAmount": "500" }
  }
}

GET /orders/_search
{
  "query" : {
    "range": { "orderAmount": { "gte": "600" } }
  }
}