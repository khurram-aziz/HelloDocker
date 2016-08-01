# HelloDocker
Docker playground

## Dockerfile.gluster
http://weblogs.com.pk/khurram/archive/2016/07/14/glusterfs.aspx

## Dockerfile.glustersamba
http://weblogs.com.pk/khurram/archive/2016/07/19/glusterfs-volume-as-samba-share.aspx

## Dockerfile.linuxtools
http://weblogs.com.pk/khurram/archive/2016/07/20/visual-c-for-linux-development.aspx

## Dockerfile.mongodb, Dockerfile.node
http://weblogs.com.pk/khurram/archive/2016/07/04/dockerizing-mongo-and-express.aspx<br>
http://weblogs.com.pk/khurram/archive/2016/07/11/docker-compose.aspx

## Dockerfile.ucarp
### Usage
docker run --rm --cap-add=NET_ADMIN --net=host -it khurramaziz/ucarp 10.0.75.20 foobar
- where 10.0.75.20 is the floating / virtual ip and foobar is the password

### Details
https://debian-administration.org/article/678/Virtual_IP_addresses_with_ucarp_for_high-availability

### Credits
Scripts are taken from
- https://libraries.io/github/docker-infra/ucarp-docker
- https://hub.docker.com/r/swcc/haproxy-docker/
- https://5pi.de/2014/11/10/running-a-highly-available-load-balancer-on-docker/
