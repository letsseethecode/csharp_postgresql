FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build

ARG TARGETARCH=
ARG TARGETFRAMEWORK=net9.0
ARG VERSION=1.0.0

WORKDIR /app
COPY *.props ./
COPY *.ruleset .l/
COPY src/ ./src/
RUN dotnet publish \
    ./src/LSTC.CheeseShop.Api/LSTC.CheeseShop.Api.csproj \
    -c release \
    --self-contained \
    -p:SelfContained=true \
    /p:Version="$VERSION" \
    -o /publish

FROM alpine:3 AS installer
RUN apk add icu-libs

FROM installer AS runtime
WORKDIR /app

COPY --chmod=0755 --from=build /publish .
RUN mkdir /output
# Add globalization support to the OS so .Net can use cultures
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

CMD [ \
    "./LSTC.CheeseShop.Api" \
    ]