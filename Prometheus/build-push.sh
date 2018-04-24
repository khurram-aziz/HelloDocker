docker build -t 192.168.10.14:5000/netcoreconsole NetCoreConsole/
docker build -t 192.168.10.14:5000/bauth ../BAuth/
docker push 192.168.10.14:5000/netcoreconsole
docker push 192.168.10.14:5000/bauth