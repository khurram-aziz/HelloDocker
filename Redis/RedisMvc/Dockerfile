FROM microsoft/dotnet:1.1-runtime

WORKDIR /app 
COPY output .

ENV ASPNETCORE_URLS http://+:80
EXPOSE 80

ENTRYPOINT ["dotnet", "RedisMvc.dll"] 