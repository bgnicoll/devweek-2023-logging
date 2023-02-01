FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /App

COPY ./src/es-alerting ./
RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/runtime:6.0

ADD es-alerts-cron /etc/cron.d/es-alerts-cron
RUN chmod 0644 /etc/cron.d/es-alerts-cron
RUN touch /var/log/cron.log
RUN apt-get update && apt-get -y install cron

WORKDIR /App
COPY --from=build-env /App/out .

CMD cron && tail -f /var/log/cron.log