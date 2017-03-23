FROM jenkins
MAINTAINER Khurram <khuziz@hotmail.com>

#jenkins image has USER jenkins; to before installing
#lets get back to root
USER root

RUN apt-get update \
    && apt-get install -qy curl libunwind8 gettext

#ADD dotnet.tar.gz /opt/dotnet
#RUN ln -s /opt/dotnet/dotnet /usr/local/bin
RUN curl -sSL -o dotnet.tar.gz https://go.microsoft.com/fwlink/?linkid=843453
RUN mkdir -p /opt/dotnet \
    && tar zxf dotnet.tar.gz -C /opt/dotnet \
    && ln -s /opt/dotnet/dotnet /usr/local/bin \
    && rm dotnet.tar.gz

ENV DOTNET_CLI_TELEMETRY_OPTOUT 1
ENV NUGET_XMLDOC_MODE skip
#ENV DOTNET_SKIP_FIRST_TIME_EXPERIENCE true
RUN mkdir warmup && cd warmup \
    && dotnet new console && dotnet restore && dotnet build \
    && cd .. && rm -rf warmup
RUN mkdir warmup && cd warmup \
    && dotnet new web && dotnet restore && dotnet build \
    && cd .. && rm -rf warmup
RUN mkdir warmup && cd warmup \
    && dotnet new mvc && dotnet restore && dotnet build \
    && cd .. && rm -rf warmup
RUN rm -rf /tmp/NuGetScratch

#installing few jenkins plugins
RUN install-plugins.sh token-macro git docker-plugin

#docker-cli
RUN apt-get install -qy docker.io sudo
#so sudo docker works; jenkins will not have /var/run/docker.sock access
RUN echo "jenkins ALL=NOPASSWD: ALL" >> /etc/sudoers
#RUN gpasswd -a jenkins root

VOLUME /var/run/docker.sock

USER jenkins
#docker run --name jenk --rm -p 8080:8080 -v /var/run/docker.sock:/var/run/docker.sock jen
