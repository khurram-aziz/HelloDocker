FROM microsoft/dotnet:2.0-sdk AS build-env
WORKDIR /app
COPY *.csproj ./
RUN dotnet restore
COPY . ./
RUN dotnet publish -c Release -o out 

FROM microsoft/aspnetcore:2.0
WORKDIR /app 
COPY --from=build-env /app/out .

#ENV ASPNETCORE_URLS http://+:80
#EXPOSE 80

ENTRYPOINT ["dotnet", "RedisMvc2.dll"] 
