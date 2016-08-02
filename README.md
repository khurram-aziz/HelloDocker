# HelloDocker
Docker playground

## Dockerfile.gluster*: GlusterFS Volume as Samba Share
Mount the GlusterFS Volume and expose it as Samba Share

### Usage
docker run --name glustersamba --cap-add SYS_ADMIN --device /dev/fuse --rm -e glusterip=Gluster-Server-IP -e glusterhost=Gluster-Server-FriendlyName -e glustervolume=Gluster-Volume-Name -p 138:138/udp -p 139:139 -p 445:445 -p 445:445/udp -it khurramaziz/gluster
- where Gluster-Server-IP, Gluster-Server-FriendlyName and Gluster-Volume-Name are as per your Gluster environment

### Details
- http://weblogs.com.pk/khurram/archive/2016/07/14/glusterfs.aspx
- http://weblogs.com.pk/khurram/archive/2016/07/19/glusterfs-volume-as-samba-share.aspx

### Notes
Its built with intention for Raspberry PI based Gluster environment; at the time of build, out of the box
Gluster 3.5.2 is available for PI using its package repository; however 3.5.7 is also available at 
https://download.gluster.org/pub/gluster/glusterfs/3.7/3.7.3/Raspbian/jessie/


## Dockerfile.linuxtools
http://weblogs.com.pk/khurram/archive/2016/07/20/visual-c-for-linux-development.aspx

## Dockerfile.mongodb, Dockerfile.node
http://weblogs.com.pk/khurram/archive/2016/07/04/dockerizing-mongo-and-express.aspx<br>
http://weblogs.com.pk/khurram/archive/2016/07/11/docker-compose.aspx

## Dockerfile.ucarp: Virtual / Floating IP with UCARP for High Availability
Assign Virtual / Floating IP with UCARP to your Docker Host for High Availability.

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
