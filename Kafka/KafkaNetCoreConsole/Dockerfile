FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

FROM microsoft/dotnet:2.1-runtime 
LABEL maintainer="khuziz@hotmail.com"
WORKDIR /app
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "KafkaNetCoreConsole.dll"]