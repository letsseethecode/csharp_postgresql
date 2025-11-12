FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

RUN mkdir /db && mkdir /output

COPY --chmod=0755 /bin/grate .
COPY --chmod=0755 ./entrypoint.sh .
COPY ./grate-sql /db

CMD [ "/bin/bash", "./entrypoint.sh" ]
