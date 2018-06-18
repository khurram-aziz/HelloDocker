# openjdk:10-jre and 11-jre giving error https://github.com/wurstmeister/kafka-docker/issues/151
FROM openjdk:9-jre

# ADD kafka_2.11-1.1.0.tgz /
ADD http://www-us.apache.org/dist/kafka/1.1.0/kafka_2.11-1.1.0.tgz /
RUN tar xf kafka_2.11-1.1.0.tgz
RUN rm kafka_2.11-1.1.0.tgz
RUN mv /kafka_2.11-1.1.0 /kafka

# we will use entrypoint in docker-compose.yml
ENTRYPOINT ["/bin/bash"]