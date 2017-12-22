FROM microsoft/aspnetcore-build:1.1 AS builder
WORKDIR /app
COPY *.csproj ./
RUN dotnet restore
COPY . ./
RUN dotnet publish -c Release -o out

FROM microsoft/aspnetcore:1.1
WORKDIR /app 
COPY --from=builder /app/out .

ENV ASPNETCORE_URLS http://+:80
EXPOSE 80

ENTRYPOINT ["dotnet", "RedisMvc.dll"] 
