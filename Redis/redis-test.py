import redis

r = redis.StrictRedis(host="localhost", port=6379, password="foobaar")
r.incr("hitcounter")
r.lpush("mainpageitems", "product:456")
r.lpush("mainpageitems", "offer:234")

print "Counter: ", r.get("hitcounter")
