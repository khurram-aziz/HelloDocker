docker build -t khurramaziz/gluster:3.5.2 -f Dockerfile.gluster-3.5 .
docker build -t khurramaziz/gluster:3.5.2-samba -f Dockerfile.glustersamba-3.5 .
docker build -t khurramaziz/gluster:3.7.3 -f Dockerfile.gluster-3.7 .
docker build -t khurramaziz/gluster:3.7.3-samba -f Dockerfile.glustersamba-3.7 .
docker tag khurramaziz/gluster:3.7.3-samba khurramaziz/gluster:latest

docker build -t khurramaziz/linuxtools -f Dockerfile.linuxtools .
docker build -t khurramaziz/mongo -f Dockerfile.mongo .
docker build -t khurramaziz/node -f Dockerfile.node .
docker build -t khurramaziz/ucarp -f Dockerfile.ucarp .
docker build -t khurramaziz/powershell -f Dockerfile.powershel .